using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSetUpPretranslationDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSetUpPretranslationDialog>
	{
		public NewProjectSetUpPretranslationDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectSetUpPretranslationDialog GetPage()
		{
			var newProjectSetUpPretranslationDialog = new NewProjectSetUpPretranslationDialog(Driver);
			InitPage(newProjectSetUpPretranslationDialog, Driver);

			return newProjectSetUpPretranslationDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(PRETRANSLATION_HEADER)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти кэтапу Pretranslation.");
			}
		}

		protected const string PRETRANSLATION_HEADER = "//span[contains(text(), 'Pretranslation')]";
	}
}
