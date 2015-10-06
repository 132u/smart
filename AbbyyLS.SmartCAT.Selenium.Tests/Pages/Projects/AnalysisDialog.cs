using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class AnalysisDialog: ProjectsPage, IAbstractPage<AnalysisDialog>
	{
		public AnalysisDialog(WebDriver driver) : base(driver)
		{
		}

		public new AnalysisDialog GetPage()
		{
			var settingsDialog = new AnalysisDialog(Driver);
			InitPage(settingsDialog, Driver);

			return settingsDialog;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ANALYSIS_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог настроек проекта.");
			}
		}

		protected const string ANALYSIS_DIALOG = "//div[contains(@class,'js-popup-analyse')][2]";
	}
}
