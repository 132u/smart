using System;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;


namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	class ExportMenu : BaseObject, IAbstractPage<ExportMenu>
	{
		public WebDriver Driver { get; private set; }

		public ExportMenu(WebDriver driver)
		{
			Driver = driver;
		}

		public ExportMenu GetPage()
		{
			var exportMenu = new ExportMenu(Driver);
			InitPage(exportMenu, Driver);

			return exportMenu;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(EXPORT_TYPE_TMX)))
			{
				Assert.Fail("Произошла ошибка:\n меню экспорта не загрузилось.");
			}
		}

		/// <summary>
		/// Выбрать тип экспорта
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		public T ClickExportType<T>(ExportType exportType, WebDriver driver) where T: class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Выбрать тип экспорта");
			ExportType = Driver.SetDynamicValue(How.XPath, EXPORT_TYPE, exportType.ToString());
			ExportType.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}

		[FindsBy(How = How.XPath, Using = EXPORT_TYPE)]
		protected IWebElement ExportType { get; set; }

		protected const string EXPORT_TYPE = "//div[not(contains(@class,'g-hidden'))]/div[contains(@data-bind,'*#*')]";
		protected const string EXPORT_TYPE_TMX = "//div[not(contains(@class,'g-hidden'))]/div[contains(@data-bind,'Tmx')]";
	}
}
