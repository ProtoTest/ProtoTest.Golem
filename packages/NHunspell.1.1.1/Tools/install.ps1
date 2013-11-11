param($installPath, $toolsPath, $package, $project)
Write-Host "InstallPath: $installPath"
Write-Host "ToolsPath: $toolsPath"
Write-Host "Package: $package"
Write-Host "Project: $project"

. (Join-Path $toolsPath "NHunspellCopyNativeDllsCmd.ps1")

Write-Host $NHunspellCopyNativeDllsCmd

# Get the current Post Build Event cmd
$currentPostBuildCmd = $project.Properties.Item("PostBuildEvent").Value

# Append our post build command if it's not already there
if (!$currentPostBuildCmd.Contains($NHunspellCopyNativeDllsCmd)) {
    $project.Properties.Item("PostBuildEvent").Value += $NHunspellCopyNativeDllsCmd
}