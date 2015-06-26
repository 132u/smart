using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectCreateTMDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectCreateTMDialog>
	{
		public new NewProjectCreateTMDialog GetPage()
		{
			var newProjectCreateTMDialog = new NewProjectCreateTMDialog();
			InitPage(newProjectCreateTMDialog);

			return newProjectCreateTMDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(NEW_TM_NAME_INPUT)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог создания ТМ.");
			}
		}

		/// <summary>
		/// Ввести имя новой ТМ
		/// </summary>
		/// <param name="newTMName">имя ТМ</param>
		public NewProjectCreateTMDialog SetNewTMName(string newTMName)
		{
			Logger.Debug("Ввести имя новой ТМ: {0}.", newTMName);
			NewTMNameInput.SetText(newTMName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Сохранить"
		/// </summary>
		public NewProjectSetUpTMDialog ClickSaveButton()
		{
			Logger.Debug("Нажать кнопку 'Сохранить'.");
			SaveTMButton.Click();

			return new NewProjectSetUpTMDialog().GetPage();
		}

		[FindsBy(How = How.XPath, Using = NEW_TM_NAME_INPUT)]
		protected IWebElement NewTMNameInput { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_TM_BTN)]
		protected IWebElement SaveTMButton { get; set; }

		protected const string NEW_TM_NAME_INPUT = "//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@data-bind,'value: name')]";
		protected const string SAVE_TM_BTN = "//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'js-tour-tm-save')]";
	}
}
