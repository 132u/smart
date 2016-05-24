using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class StatisticsPage : StatisticsBasePage, IAbstractPage<StatisticsPage>
	{
		public StatisticsPage(WebDriver driver) : base(driver)
		{
		}

		public new StatisticsPage LoadPage()
		{
			if (!IsStatisticsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Страница статистики проекта не открылась.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку скачивания статистики.
		/// </summary>
		public StatisticsPage ClickExportStatisticButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку скачивания статистики.");
			ExportStatisticButton.Click();
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(EXPORT_MENU)))
			{
				throw new Exception("Произошла ошибка: не открылось контекстное меню выбора формата скачивания статистики.");
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Collapse All'
		/// </summary>
		public StatisticsPage ClickCollapseAllButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Collapse All'.");
			CollapseAllButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Expand All'
		/// </summary>
		public StatisticsPage ClickExpandAllButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Expand All'.");
			ExpandAllButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Trados Xml'
		/// </summary>
		public StatisticsPage ClickTradosXmlButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Trados Xml'.");
			TradosXml.Click();
			
			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Trados Xlsx'
		/// </summary>
		public StatisticsPage ClickTradosXlsxButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Trados Xlsx'.");
			TradosXlsx.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'By Assignee'
		/// </summary>
		public StatisticsPage ClickByAssigneesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'By Assignee'.");
			ByAssigneesButton.Click();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница статистики проекта
		/// </summary>
		public bool IsStatisticsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(COLLAPSE_ALL_BUTTON));
		}

		/// <summary>
		/// Проверить, что все панели языков свернуты
		/// </summary>
		public bool IsAllLanguagesPanelsCollapsed()
		{
			CustomTestContext.WriteLine("Проверить, что все панели языков свернуты.");
			var languagePanels = Driver.GetElementList(By.XPath(LANGUAGE_PANELS));

			for (int i = 0; i < languagePanels.Count; i++)
			{
				if (languagePanels[i].GetAttribute("class").Contains("expanded"))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Проверить, что все панели 'Total Statistics' свернуты
		/// </summary>
		public bool IsAllTotalStatistickPanelsCollapsed()
		{
			CustomTestContext.WriteLine("Проверить, что все панели 'Total Statistics' свернуты.");
			var totalStatisticsPanel = Driver.GetElementList(By.XPath(TOTAL_STATISTICS_PANELS));

			for (int i = 0; i < totalStatisticsPanel.Count; i++)
			{
				if (totalStatisticsPanel[i].GetAttribute("class").Contains("expanded"))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Проверить, что все панели документов свернуты
		/// </summary>
		public bool IsAllFilePanelsCollapsed()
		{
			CustomTestContext.WriteLine("Проверить, что все панели документов свернуты.");
			var filePanel = Driver.GetElementList(By.XPath(FILE_PANELS));

			for (int i = 0; i < filePanel.Count; i++)
			{
				if (filePanel[i].GetAttribute("class").Contains("expanded"))
				{
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = COLLAPSE_ALL_BUTTON)]
		protected IWebElement CollapseAllButton { get; set; }

		[FindsBy(How = How.XPath, Using = EXPANDE_ALL_BUTTON)]
		protected IWebElement ExpandAllButton { get; set; }

		[FindsBy(How = How.XPath, Using = EXPORT_STATISTIC_BUTTON)]
		protected IWebElement ExportStatisticButton { get; set; }

		[FindsBy(How = How.XPath, Using = TRADOS_XML)]
		protected IWebElement TradosXml { get; set; }

		[FindsBy(How = How.XPath, Using = TRADOS_XLSX)]
		protected IWebElement TradosXlsx { get; set; }

		[FindsBy(How = How.XPath, Using = BY_ASSIGNEES_BUTTON)]
		protected IWebElement ByAssigneesButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string COLLAPSE_ALL_BUTTON= "//button[contains(@data-bind,'expandCollapse(false)')]";
		protected const string EXPANDE_ALL_BUTTON = "//button[contains(@data-bind,'expandCollapse(true)')]";
		protected const string LANGUAGE_PANELS = "//li[contains(@class, 'stats__lang-pair')]";
		protected const string TOTAL_STATISTICS_PANELS = "//li[contains(@class, 'stats__total')]";
		protected const string FILE_PANELS = "//li[contains(@class, 'stats__file-item')]";
		protected const string EXPORT_STATISTIC_BUTTON = "//button[contains(@data-bind, 'openProjectExportMenu')]";
		protected const string EXPORT_MENU = "//div[contains(@data-bind, 'projectExportMenuOpened')]";
		protected const string TRADOS_XML = "//li[contains(@data-bind, 'downloadXml')]";
		protected const string TRADOS_XLSX = "//li[contains(@data-bind, 'downloadXlsx')]";
		protected const string BY_ASSIGNEES_BUTTON = "//button[contains(@data-bind, 'downloadAssignmentXlsx')]";
		#endregion
	}
}
