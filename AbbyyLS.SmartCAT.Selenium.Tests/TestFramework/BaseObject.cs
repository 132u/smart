using AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	public class BaseObject
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();
		public static IWebDriver Driver;

		public static void InitPage<T>(T pageClass) where T : BaseObject, IAbstractPage<T>
		{
			pageClass.LoadPage();
			PageFactory.InitElements(Driver, pageClass);
		}
	}
}

