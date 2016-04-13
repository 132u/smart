$checkoutDir=$args[0]

if(Test-Path "$checkoutDir\TestResult.xml")
{
	Remove-item -Path "$checkoutDir\TestResult.xml"
}