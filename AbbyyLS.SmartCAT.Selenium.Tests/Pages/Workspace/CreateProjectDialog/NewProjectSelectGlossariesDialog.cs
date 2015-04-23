using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.CreateProjectDialog
{
	public class NewProjectSelectGlossariesDialog : ProjectsPage, IAbstractPage<NewProjectSelectGlossariesDialog>
	{
		public new NewProjectSelectGlossariesDialog GetPage()
		{
			var newProjectSelectGlossariesDialog = new NewProjectSelectGlossariesDialog();
			InitPage(newProjectSelectGlossariesDialog);
			LoadPage();

			return newProjectSelectGlossariesDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_GLOSSARY_BTN_XPATH), 7))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти к третьему шагу создания проекта (выбор глоссария).");
			}
		}

		/// <summary>
		/// Нажать кнопку "Далее"
		/// </summary>
		public NewProjectSetUpMTDialog ClickNextBtn()
		{
			Logger.Debug("Нажать кнопку 'Далее'.");
			NextBtn.Click();
			var newProjectSetUpMTDialog = new NewProjectSetUpMTDialog();

			return newProjectSetUpMTDialog.GetPage();
		}

		[FindsBy(How = How.XPath, Using = NEXT_BTN_XPATH)]
		protected IWebElement NextBtn { get; set; }

		protected const string CREATE_GLOSSARY_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-glossary-create')]";
		protected const string NEXT_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
	}
}
