using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class StatisticsBasePage : ProjectsPage, IAbstractPage<StatisticsBasePage>
	{
		public StatisticsBasePage(WebDriver driver)
			: base(driver)
		{
		}

		public new StatisticsBasePage GetPage()
		{
			var StatisticsBasePage = new StatisticsBasePage(Driver);
			InitPage(StatisticsBasePage, Driver);

			return StatisticsBasePage;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();

			if (!IsStatisticsBasePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился заголовок 'Statistics'.");
			}
		}

		/// <summary>
		/// Проверить, что появился заголовок 'Statistics'.
		/// </summary>
		public bool IsStatisticsBasePageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(STATISTICS_TITLE));
		}

		protected const string STATISTICS_TITLE = "//a[@class='current-doc' and text()='Statistics']";
	}
}
