using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class SelectTaskDialog : BaseObject, IAbstractPage<SelectTaskDialog>
	{
		public SelectTaskDialog GetPage()
		{
			var selectTaskDialog = new SelectTaskDialog();
			InitPage(selectTaskDialog);

			return selectTaskDialog;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(TRANSLATE_BTN_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог с выбором задания в редакторе.");
			}
		}

		/// <summary>
		/// Нажать на кнопку "Перевод"
		/// </summary>
		public SelectTaskDialog ClickTranslateButton()
		{
			Logger.Debug("Нажать кнопку 'Перевод'.");
			//TODO: вставить нажатие кнопки 'перевод'

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Менеджер"
		/// </summary>
		public SelectTaskDialog ClickManagerButton()
		{
			Logger.Debug("Нажать кнопку 'Менеджер'.");
			ManagerButton.Click();
			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Продолжить"
		/// </summary>
		public EditorPage ClickContinueButton()
		{
			Logger.Debug("Нажать кнопку 'Продолжить'.");
			ContinueButton.Click();
			var editorPage = new EditorPage();

			return editorPage.GetPage();
		}

		[FindsBy(How = How.XPath, Using = TRANSLATE_BTN_XPATH)]
		protected IWebElement TranslateButton { get; set; }

		[FindsBy(How = How.XPath, Using = MANAGER_BTN_XPATH)]
		protected IWebElement ManagerButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONTINUE_BTN_XPATH)]
		protected IWebElement ContinueButton { get; set; }

		protected const string TRANSLATE_BTN_XPATH = "//span[contains(@id, 'stagenumber-1')]";
		protected const string MANAGER_BTN_XPATH = "//span[contains(@id, 'manager')]";
		protected const string CONTINUE_BTN_XPATH = "//span[contains(@id, 'continue-btn')]";
	}
}
