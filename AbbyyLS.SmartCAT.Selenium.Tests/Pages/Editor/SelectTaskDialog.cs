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
			LoadPage();
			return selectTaskDialog;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(TRANSLATE_BTN_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог с выбором задания в редакторе.");
			}
		}

		/// <summary>
		/// Нажать на кнопку "Перевод"
		/// </summary>
		public SelectTaskDialog ClickTranslateBtn()
		{
			Logger.Trace("Нажимаем кнопку 'Перевод'.");
			//TODO: вставить нажатие кнопки 'перевод'
			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Менеджер"
		/// </summary>
		public SelectTaskDialog ClickManagerBtn()
		{
			Logger.Trace("Нажимаем кнопку 'Менеджер'.");
			ManagerBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Продолжить"
		/// </summary>
		public EditorPage ClickContinueBtn()
		{
			Logger.Trace("Нажимаем кнопку 'Продолжить'.");
			ContinueBtn.Click();
			var editorPage = new EditorPage();
			return editorPage.GetPage();
		}

		[FindsBy(How = How.XPath, Using = TRANSLATE_BTN_XPATH)]
		protected IWebElement TranslateBtn { get; set; }

		[FindsBy(How = How.XPath, Using = MANAGER_BTN_XPATH)]
		protected IWebElement ManagerBtn { get; set; }

		[FindsBy(How = How.XPath, Using = CONTINUE_BTN_XPATH)]
		protected IWebElement ContinueBtn { get; set; }

		protected const string TRANSLATE_BTN_XPATH = "//span[contains(@id, 'stagenumber-1')]";
		protected const string MANAGER_BTN_XPATH = "//span[contains(@id, 'manager')]";
		protected const string CONTINUE_BTN_XPATH = "//span[contains(@id, 'continue-btn')]";
	}
}
