$deployServerName=$args[0]
$configFileName=$args[1]
$stageConfigFileName=$args[2]

Remove-Item $configFileName
(Get-Content $stageConfigFileName).Replace('$(servername)', $deployServerName) | Set-Content $configFileName