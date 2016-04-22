$checkoutDir=$args[0]
$IncludeFeatures=$args[1]

chcp 1251

if($IncludeFeatures)
{
	& "C:\Program Files (x86)\NUnit.org\nunit-console\nunit3-console.exe" $checkoutDir\bin\AbbyyLS.SmartCAT.Selenium.Tests.dll --framework=net-4.0 --process=Multiple --work=$checkoutDir --teamcity --workers=5 --where:cat=$IncludeFeatures --dispose-runners --result=$checkoutDir\TestResults\TestResult.xml
}
else
{
	& "C:\Program Files (x86)\NUnit.org\nunit-console\nunit3-console.exe" $checkoutDir\bin\AbbyyLS.SmartCAT.Selenium.Tests.dll --framework=net-4.0 --process=Multiple --work=$checkoutDir --teamcity --workers=5 --where:cat!=LoadTests --dispose-runners --result=$checkoutDir\TestResults\TestResult.xml
}