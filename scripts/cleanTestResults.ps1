$checkoutDir=$args[0]

if(Test-Path "$checkoutDir\TestResults")
{
	Remove-item -Path "$checkoutDir\TestResults" -Recurse
}

New-Item -ItemType directory -Path "$checkoutDir\TestResults"