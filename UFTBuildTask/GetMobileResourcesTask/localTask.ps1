# 
# MIT License https://github.com/MicroFocus/ADM-TFS-Extension/blob/master/LICENSE
# 
# Copyright 2016-2024 Open Text
# 
# The only warranties for products and services of Open Text and its affiliates and licensors ("Open Text") are as may be set forth in the express warranty statements accompanying such products and services.
# Nothing herein should be construed as constituting an additional warranty.
# Open Text shall not be liable for technical or editorial errors or omissions contained herein. 
# The information contained herein is subject to change without notice.
# 

using namespace PSModule.UftMobile.SDK.UI

$uftworkdir = $env:UFT_LAUNCHER
Import-Module "$uftworkdir\bin\PSModule.dll" -ErrorAction Stop

$mcServerUrl = (Get-VstsInput -Name 'mcServerUrl' -Require).Trim()
$mcAuthType = Get-VstsInput -Name 'mcAuthType' -Require
$mcUsername = (Get-VstsInput -Name 'mcUsername').Trim()
$mcPassword = Get-VstsInput -Name 'mcPassword'
$mcAccessKey = (Get-VstsInput -Name 'mcAccessKey').Trim(' "')
$mcResources = Get-VstsInput -Name 'mcResources' -Require
[bool]$includeOfflineDevices = Get-VstsInput -Name 'includeOfflineDevices' -AsBool

# $env:SYSTEM can be used also to determine the pipeline type "build" or "release"
if ($env:SYSTEM_HOSTTYPE -eq "build") {
	$buildNumber = $env:BUILD_BUILDNUMBER
	[int]$rerunIdx = [convert]::ToInt32($env:SYSTEM_STAGEATTEMPT, 10) - 1
	$rerunType = "rerun"
} else {
	$buildNumber = $env:RELEASE_RELEASEID
	[int]$rerunIdx = $env:RELEASE_ATTEMPTNUMBER
	$rerunType = "attempt"
}

$resDir = Join-Path $uftworkdir -ChildPath "res\Report_$buildNumber"
$runStatusCodeFile = "$resDir\RunStatusCode.txt"

[bool]$useMcProxy = Get-VstsInput -Name 'useMcProxy' -AsBool
[ProxyConfig]$proxyConfig = $null

if ($useMcProxy) {
	$mcProxyUrl = (Get-VstsInput -Name 'mcProxyUrl').Trim()
	[bool]$useMcProxyCredentials = Get-VstsInput -Name 'useMcProxyCredentials' -AsBool
	$mcProxyUsername = (Get-VstsInput -Name 'mcProxyUsername').Trim()
	$mcProxyPassword = Get-VstsInput -Name 'mcProxyPassword'

	if ($mcProxyUrl -eq "") {
		throw "Proxy Server is empty."
	} elseif ($useMcProxyCredentials -and ($mcProxyUsername -eq "")) {
		throw "Proxy Username is empty."
	} elseif ($useMcProxyCredentials -and ($mcProxyPassword.Trim() -eq "")) {
		throw "Proxy Password is empty."
	}
	$proxySrvConfig = [ServerConfig]::new($mcProxyUrl, $mcProxyUsername, $mcProxyPassword)
	$proxyConfig = [ProxyConfig]::new($proxySrvConfig, $useMcProxyCredentials)
}

if ($mcAuthType -eq "basic") {
	$srvConfig = [ServerConfig]::new($mcServerUrl, $mcUsername, $mcPassword)
} else {
	$mcClientId = $mcSecret = $mcTenantId = $null
	$err = [ServerConfig]::ParseAccessKey($mcAccessKey, [ref]$mcClientId, [ref]$mcSecret, [ref]$mcTenantId)
	if ($err) {
		throw $err
	}
	$srvConfig = [ServerConfig]::new($mcServerUrl, $mcClientId, $mcSecret, $mcTenantId, $false)
}
$dlServerConfig = [ServerConfigEx]::new($srvConfig, $useMcProxy, $proxyConfig)
$config = [LabResxConfig]::new($dlServerConfig, $mcResources, $includeOfflineDevices)

#---------------------------------------------------------------------------------------------------

$report = "$resDir\UFTM Report"

if ($rerunIdx) {
	Write-Host "$((Get-Culture).TextInfo.ToTitleCase($rerunType)) = $rerunIdx"
	if (Test-Path $report) {
		try {
			Remove-Item $report -ErrorAction Stop
		} catch {
			Write-Error "Cannot rerun because the file '$report' is currently in use."
		}
	}
}
#---------------------------------------------------------------------------------------------------
#Run the tests
Invoke-GMRTask $config $buildNumber -Verbose 

# read return code
if (Test-Path $runStatusCodeFile) {
	$content = Get-Content $runStatusCodeFile
	if ($content) {
		$sep = [Environment]::NewLine
		$option = [System.StringSplitOptions]::RemoveEmptyEntries
		$arr = $content.Split($sep, $option)
		[int]$retcode = [convert]::ToInt32($arr[-1], 10)
		if ($retcode -lt 0) {
			Write-Host "##vso[task.complete result=Failed;]"
		}
	} else {
		Write-Error "The file [$runStatusCodeFile] is empty!"
	}
} else {
	Write-Error "The file [$runStatusCodeFile] is missing!"
}
