using OpenQA.Selenium;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Driver
{
	public interface IWebDriverSettings
	{
		IWebDriver Driver { get; set; }

		string[] ProcessNames { get; set; }
	}
}