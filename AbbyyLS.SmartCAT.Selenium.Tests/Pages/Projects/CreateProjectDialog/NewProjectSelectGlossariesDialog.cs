using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSelectGlossariesDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSelectGlossariesDialog>
	{
		public new NewProjectSelectGlossariesDialog GetPage()
		{
			var newProjectSelectGlossariesDialog = new NewProjectSelectGlossariesDialog();
			InitPage(newProjectSelectGlossariesDialog);

			return newProjectSelectGlossariesDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_GLOSSARY_BTN), 7))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти к третьему шагу создания проекта (выбор глоссария).");
			}
		}

		protected const string CREATE_GLOSSARY_BTN = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-glossary-create')]";
	}
}
