using OpenQA.Selenium.Remote;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Drivers
{
	public interface IWebDriverProvider
	{
		RemoteWebDriver GetWebDriver(string tempFolder, string downloadDirectory);
	}
}
