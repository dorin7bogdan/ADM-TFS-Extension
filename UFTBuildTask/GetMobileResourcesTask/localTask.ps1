# 
# Certain versions of software accessible here may contain branding from Hewlett-Packard Company (now HP Inc.) and Hewlett Packard Enterprise Company.
# This software was acquired by Micro Focus on September 1, 2017, and is now offered by OpenText.
# Any reference to the HP and Hewlett Packard Enterprise/HPE marks is historical in nature, and the HP and Hewlett Packard Enterprise/HPE marks are the property of their respective owners.
# __________________________________________________________________
# MIT License
# 
# Copyright 2012-2023 Open Text
# 
# The only warranties for products and services of Open Text and
# its affiliates and licensors ("Open Text") are as may be set forth
# in the express warranty statements accompanying such products and services.
# Nothing herein should be construed as constituting an additional warranty.
# Open Text shall not be liable for technical or editorial errors or
# omissions contained herein. The information contained herein is subject
# to change without notice.
# 
# Except as specifically indicated otherwise, this document contains
# confidential information and a valid license is required for possession,
# use or copying. If this work is provided to the U.S. Government,
# consistent with FAR 12.211 and 12.212, Commercial Computer Software,
# Computer Software Documentation, and Technical Data for Commercial Items are
# licensed to the U.S. Government under vendor's standard commercial license.
# 
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# ___________________________________________________________________
#

using namespace PSModule.UftMobile.SDK.UI

param()
$mcServerUrl = Get-VstsInput -Name 'mcServerUrl' -Require
$mcAuthType = Get-VstsInput -Name 'mcAuthType' -Require
$mcUsername = Get-VstsInput -Name 'mcUsername'
$mcPassword = Get-VstsInput -Name 'mcPassword'
[int]$mcTenantId = Get-VstsInput -Name 'mcTenantId' -AsInt
$mcAccessKey = Get-VstsInput -Name 'mcAccessKey'
$mcResources = Get-VstsInput -Name 'mcResources' -Require
[bool]$includeOfflineDevices = Get-VstsInput -Name 'includeOfflineDevices' -AsBool

$uftworkdir = $env:UFT_LAUNCHER
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

Import-Module $uftworkdir\bin\PSModule.dll

if ($mcAuthType -eq "basic") {
	$srvConfig = [ServerConfig]::new($mcServerUrl, $mcUsername, $mcPassword, $mcTenantId)
} else {
	$mcClientId = $mcSecret = $null
	$err = [ServerConfig]::ParseAccessKey($mcAccessKey, [ref]$mcClientId, [ref]$mcSecret, [ref]$mcTenantId)
	if ($err) {
		throw $err
	}
	$srvConfig = [ServerConfig]::new($mcServerUrl, $mcClientId, $mcSecret, $mcTenantId, $false)
}
$config = [MobileResxConfig]::new($srvConfig, $mcResources, $includeOfflineDevices, $false)

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
