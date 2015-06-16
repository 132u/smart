using System;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	public class BaseObject
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();
		public static WebDriver Driver;

		public static void InitPage<T>(T pageClass) where T : BaseObject, IAbstractPage<T>
		{
			pageClass.LoadPage();
			PageFactory.InitElements(Driver, pageClass);
		}
	}
}

