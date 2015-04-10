using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.CreateProjectDialog
{
	public class CreateTMDialog : NewProjectSetUpTMDialog, IAbstractPage<CreateTMDialog>
	{
		public new CreateTMDialog GetPage()
		{
			var createTMDialog = new CreateTMDialog();
			InitPage(createTMDialog);
			LoadPage();

			return createTMDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.ElementIsPresent(By.XPath(NEW_TM_NAME_INPUT_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог создания ТМ.");
			}
		}

		/// <summary>
		/// Ввести имя новой ТМ
		/// </summary>
		/// <param name="newTMName">имя ТМ</param>
		public CreateTMDialog SetNewTMName(string newTMName)
		{
			Logger.Debug("Вводим имя новой ТМ: {0}.", newTMName);
			NewTMNameInput.SetText(newTMName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Сохранить"
		/// </summary>
		public NewProjectSetUpTMDialog ClickSaveBtn()
		{
			Logger.Debug("Нажимаем кнопку 'Сохранить'.");
			SaveTMBtn.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = NEW_TM_NAME_INPUT_XPATH)]
		protected IWebElement NewTMNameInput { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_TM_BTN_XPATH)]
		protected IWebElement SaveTMBtn { get; set; }

		protected const string CREATE_TM_DIALOG_XPATH = "//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string NEW_TM_NAME_INPUT_XPATH = CREATE_TM_DIALOG_XPATH + "//input[contains(@class,'js-tm-name')]";
		protected const string SAVE_TM_BTN_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@class,'js-save')]";
	}
}
