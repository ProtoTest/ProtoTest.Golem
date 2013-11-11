param($installPath, $toolsPath, $package, $project)
Write-Host "InstallPath: $installPath"
Write-Host "ToolsPath: $toolsPath"
Write-Host "Package: $package"
Write-Host "Project: $project"


. (Join-Path $toolsPath "NHunspellCopyNativeDllsCmd.ps1")

Write-Host $NHunspellCopyNativeDllsCmd

# Get the current Post Build Event cmd
$currentPostBuildCmd = $project.Properties.Item("PostBuildEvent").Value

# Remove our post build command from it (if it's there)
$project.Properties.Item("PostBuildEvent").Value = $currentPostBuildCmd.Replace($NHunspellCopyNativeDllsCmd, "")
