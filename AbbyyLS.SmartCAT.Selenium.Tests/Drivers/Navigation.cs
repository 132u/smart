using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Drivers
{
	public class Navigation : INavigation
	{
		private readonly RemoteWebDriver _driver;

		public Navigation(RemoteWebDriver driver)
		{
			_driver = driver;
		}

		public void Back()
		{
			_driver.Navigate().Back();
		}

		public void GoToUrl(string url)
		{
			_driver.Manage().Cookies.DeleteAllCookies();
			_driver.Navigate().GoToUrl(url);
		}

		public void GoToUrl(Uri url)
		{
			_driver.Manage().Cookies.DeleteAllCookies();
			_driver.Navigate().GoToUrl(url);
		}

		public void Forward()
		{
			_driver.Navigate().Forward();
		}

		public void Refresh()
		{
			_driver.Navigate().Refresh();
		}
	}
}
