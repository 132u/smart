using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSetUpMTDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSetUpMTDialog>
	{
		public new NewProjectSetUpMTDialog GetPage()
		{
			var newProjectSetUpMTDialog = new NewProjectSetUpMTDialog();
			InitPage(newProjectSetUpMTDialog);

			return newProjectSetUpMTDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(MT_TABLE), 8))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти к четвертому шагу создания проекта (выбор МТ).");
			}
		}

		[FindsBy(How = How.XPath, Using = FINISH_BTN)]
		protected IWebElement FinishButton { get; set; }

		protected const string MT_TABLE = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-mts-body')]//tbody";
		protected const string FINISH_BTN = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-finish js-upload-btn')]";
	}
}
