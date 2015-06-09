if(Test-Path "%teamcity.build.checkoutDir%\TestResults")
{
	Remove-Item %teamcity.build.checkoutDir%\TestResults -Recurse
}