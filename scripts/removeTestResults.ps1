$checkoutDir=$args[0]

if(Test-Path "$checkoutDir\TestResults")
{
	Remove-item -Path "$checkoutDir\TestResults" -Recurse
}