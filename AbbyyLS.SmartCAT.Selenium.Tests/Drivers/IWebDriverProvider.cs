using OpenQA.Selenium;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Drivers
{
	public interface IWebDriverProvider
	{
		IWebDriver GetWebDriver(string tempFolder);
	}
}
