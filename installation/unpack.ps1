function Expand-ZIPFile($file, $destination)
{
    $shell = new-object -com shell.application
    $zip = $shell.NameSpace($file)
    $dest = $shell.NameSpace($destination)
    $folderItems = $zip.items()
    foreach($item in $folderItems)
    {
        $dest.copyhere($item)
    }
}

function AbsPath($folder)
{
    [System.IO.Directory]::SetCurrentDirectory(((Get-Location -PSProvider FileSystem).ProviderPath))
    $path = [IO.Path]::GetFullPath($folder)
    
    return $path
}

$Registry_Key ="HKLM:\SOFTWARE\Wow6432Node\Mercury Interactive\QuickTest Professional\CurrentVersion"
$result = Test-Path $Registry_Key

if($result)
{
    $value = "QuickTest Professional"
    $uftPath = Get-ItemProperty -Path $Registry_Key | Select-Object -ExpandProperty $value

    $currdir = AbsPath -folder .\
    $zipFile = Join-Path -Path $currdir -ChildPath "UFT.zip"
    Expand-ZIPFile $zipFile $currdir
	
    $uft_dir = Join-Path $currdir -ChildPath "UFT"
	
	$launcherFolder = Get-ChildItem -Path $uft_dir -recurse -Directory| Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match "UFTWorking"}
	$launcherPath = $launcherFolder.fullName
	[Environment]::SetEnvironmentVariable("UFT_LAUNCHER", $launcherPath, "Machine")

}else
{
     Write-Error "UFT One is not installed"
}
