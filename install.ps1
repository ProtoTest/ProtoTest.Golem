param($installPath, $toolsPath, $package, $project)
Write-Host "InstallPath: $installPath"
Write-Host "ToolsPath: $toolsPath"
Write-Host "Package: $package"
Write-Host "Project: $project"

$CopyChromeDriver = "`nxcopy /y `"`$(ProjectDir)chromedriver.exe`" `"`$(TargetDir)`""
$CopyIEDriver = "`nxcopy /y `"`$(ProjectDir)IEDriverServer.exe`" `"`$(TargetDir)`""
$CopyPhantomJSDriver = "`nxcopy /y `"`$(ProjectDir)phantomjs.exe`" `"`$(TargetDir)`""
$CopyCSS = "`nxcopy /y `"`$(ProjectDir)dashboard.css`" `"`$(TargetDir)`""
$CopyServer = "`nxcopy /y `"`$(ProjectDir)selenium-server-standalone.jar`" `"`$(TargetDir)`""

# Get the current Post Build Event cmd
$currentPostBuildCmd = $project.Properties.Item("PostBuildEvent").Value

# Append our post build command if it's not already there
if (!$currentPostBuildCmd.Contains($CopyChromeDriver)) {
    $project.Properties.Item("PostBuildEvent").Value += $CopyChromeDriver
}
if (!$currentPostBuildCmd.Contains($CopyPhantomJSDriver)) {
    $project.Properties.Item("PostBuildEvent").Value += $CopyPhantomJSDriver
}
if (!$currentPostBuildCmd.Contains($CopyIEDriver)) {
    $project.Properties.Item("PostBuildEvent").Value += $CopyIEDriver
}
if (!$currentPostBuildCmd.Contains($CopyCSS)) {
    $project.Properties.Item("PostBuildEvent").Value += $CopyCSS
}
if (!$currentPostBuildCmd.Contains($CopyServer)) {
    $project.Properties.Item("PostBuildEvent").Value += $CopyServer
}
$mobFile = $project.ProjectItems.Item("Proxy").ProjectItems.Item("browsermob-proxy-2.1.0-beta-6-bin.zip")
$mobFile.Properties.Item("CopyToOutputDirectory").Value = 2