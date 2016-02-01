﻿using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class StatisticsPage : StatisticsBasePage, IAbstractPage<StatisticsPage>
	{
		public StatisticsPage(WebDriver driver) : base(driver)
		{
		}

		public new StatisticsPage GetPage()
		{
			var statisticsPage = new StatisticsPage(Driver);
			InitPage(statisticsPage, Driver);

			return statisticsPage;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();

			if (!IsStatisticsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Страница статистики проекта не открылась.");
			}
		}

		/// <summary>
		/// Проверить, что открылась страница статистики проекта
		/// </summary>
		public bool IsStatisticsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(COLLAPSE_ALL_BUTTON));
		}

		protected const string COLLAPSE_ALL_BUTTON= "//button[contains(@data-bind,'expandCollapse(false)')]";
	}
}
