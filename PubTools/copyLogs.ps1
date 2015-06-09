$srcFolder = "%system.DeployServerName%\ABBYYLS\CAT\log"
$destFolder = "%teamcity.build.checkoutDir%\TestResults"

Copy-Item $srcFolder/Web $destFolder -Recurse
Copy-Item $srcFolder/ModuleHosting $destFolder -Recurse