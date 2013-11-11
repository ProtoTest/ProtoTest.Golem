$solutionDir = [System.IO.Path]::GetDirectoryName($dte.Solution.FullName) + "\"
$path = $installPath.Replace($solutionDir, "`$(SolutionDir)")
Write-Host $path

$NHunspellCopyNativeDllsCmd = "xcopy /s /y `"$path\native\*.*`" `"`$(TargetDir)`""