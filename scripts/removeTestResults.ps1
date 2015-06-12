$checkoutDir=$args[0]

if(Test-Path "$checkoutDir\TestResults")
{
	Get-ChildItem -Path "$checkoutDir\TestResults" -Include *.png -recurse | foreach ($_) {Remove-Item $_.fullname}
}