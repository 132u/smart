﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class BuildStatisticsPage : StatisticsPage, IAbstractPage<BuildStatisticsPage>
	{
		public BuildStatisticsPage(WebDriver driver)
			: base(driver)
		{
		}

		public new BuildStatisticsPage GetPage()
		{
			var buildStatisticsPage = new BuildStatisticsPage(Driver);
			InitPage(buildStatisticsPage, Driver);

			return buildStatisticsPage;
		}

		public new void LoadPage()
		{
			if (!IsBuildStatisticsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылась страница построения статистики.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку построения статистики.
		/// </summary>
		public StatisticsPage ClickBuildStatisticsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку построения статистики.");
			BuildStatisticsButton.Click();

			return new StatisticsPage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница создания статистики
		/// </summary>
		private bool IsBuildStatisticsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(BUILD_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = BUILD_BUTTON)]
		protected IWebElement BuildStatisticsButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string BUILD_BUTTON = "//button[@data-bind='click: build']";

		#endregion


	}
}