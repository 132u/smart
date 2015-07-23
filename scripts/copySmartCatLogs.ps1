$deployServerName=$args[0]
$destFolder=$args[1]

if (!(Test-Path -path $destFolder\TestResults\log\Web))
 {
	New-Item $destFolder\TestResults\log\Web -Type Directory
 }

if (!(Test-Path -path $destFolder\TestResults\log\ModuleHosting))
 {
	New-Item $destFolder\TestResults\log\ModuleHosting -Type Directory
 }

Get-ChildItem \\$deployServerName.als.local\ABBYYLS\CAT\log\Web -Exclude debug.log, info.log |Copy-Item -Destination $destFolder\TestResults\log\Web

Get-ChildItem \\$deployServerName.als.local\ABBYYLS\CAT\log\ModuleHosting -Exclude debug.log, info.log |Copy-Item -Destination $destFolder\TestResults\log\ModuleHosting

Get-ChildItem \\$deployServerName.als.local\ABBYYLS\CAT\log\Web.Admin -Exclude debug.log, info.log |Copy-Item -Destination $destFolder\TestResults\log\Web.Admin