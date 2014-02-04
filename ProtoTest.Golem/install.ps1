param($installPath, $toolsPath, $package, $project)
Write-Host "InstallPath: $installPath"
Write-Host "ToolsPath: $toolsPath"
Write-Host "Package: $package"
Write-Host "Project: $project"

$CopyChromeDriver = "`nxcopy /y `"`$(ProjectDir)chromedriver.exe`" `"`$(TargetDir)`""
$CopyIEDriver = "`nxcopy /y `"`$(ProjectDir)IEDriverServer.exe`" `"`$(TargetDir)`""


# Get the current Post Build Event cmd
$currentPostBuildCmd = $project.Properties.Item("PostBuildEvent").Value

# Append our post build command if it's not already there
if (!$currentPostBuildCmd.Contains($CopyChromeDriver)) {
    $project.Properties.Item("PostBuildEvent").Value += $CopyChromeDriver
}
if (!$currentPostBuildCmd.Contains($CopyIEDriver)) {
    $project.Properties.Item("PostBuildEvent").Value += $CopyIEDriver
}

$mobFile = $project.ProjectItems.Item("Proxy").ProjectItems.Item("browsermob-proxy-2.0-beta-8-bin.zip")
$mobFile.Properties.Item("CopyToOutputDirectory").Value = 2