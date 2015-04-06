﻿using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	public class BaseObject
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();
		public static IWebDriver Driver;

		public static void InitPage<T>(T pageClass) where T : BaseObject
		{
			PageFactory.InitElements(Driver, pageClass);
		}
	}
}
