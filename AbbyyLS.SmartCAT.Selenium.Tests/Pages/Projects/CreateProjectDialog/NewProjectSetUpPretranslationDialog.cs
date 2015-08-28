using NUnit.Framework;

using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSetUpPretranslationDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSetUpPretranslationDialog>
	{
		public new NewProjectSetUpPretranslationDialog GetPage()
		{
			var newProjectSetUpPretranslationDialog = new NewProjectSetUpPretranslationDialog();
			InitPage(newProjectSetUpPretranslationDialog);

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
