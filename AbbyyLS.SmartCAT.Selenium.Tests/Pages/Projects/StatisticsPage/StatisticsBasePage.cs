using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class StatisticsBasePage : ProjectsPage, IAbstractPage<StatisticsBasePage>
	{
		public StatisticsBasePage(WebDriver driver) : base(driver)
		{
		}

		public new StatisticsBasePage LoadPage()
		{
			if (!IsStatisticsBasePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился заголовок 'Statistics'.");
			}

			return this;
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
