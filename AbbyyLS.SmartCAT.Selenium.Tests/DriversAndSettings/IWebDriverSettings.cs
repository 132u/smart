using OpenQA.Selenium;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings
{
	public interface IWebDriverSettings
	{
		IWebDriver Driver { get; set; }

		string[] ProcessNames { get; set; }
	}
}