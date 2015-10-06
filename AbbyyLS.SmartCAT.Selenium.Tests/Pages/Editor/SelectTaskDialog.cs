using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class SelectTaskDialog : EditorPage, IAbstractPage<SelectTaskDialog>
	{
		public SelectTaskDialog(WebDriver driver) : base(driver)
		{
		}

		public new SelectTaskDialog GetPage()
		{
			var selectTaskDialog = new SelectTaskDialog(Driver);
			InitPage(selectTaskDialog, Driver);

			return selectTaskDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(TRANSLATE_BTN_XPATH), 30))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог с выбором задания в редакторе.");
			}
		}

		/// <summary>
		/// Нажать на кнопку "Перевод"
		/// </summary>
		public SelectTaskDialog ClickTranslateButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Перевод'.");
			TranslateButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Менеджер"
		/// </summary>
		public SelectTaskDialog ClickManagerButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Менеджер'.");
			ManagerButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Editing"
		/// </summary>
		public SelectTaskDialog ClickEditingButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Editing'.");
			EditingButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Продолжить"
		/// </summary>
		public EditorPage ClickContinueButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Продолжить'.");
			ContinueButton.Click();

			return new EditorPage(Driver).GetPage();
		}

		[FindsBy(How = How.XPath, Using = TRANSLATE_BTN_XPATH)]
		protected IWebElement TranslateButton { get; set; }

		[FindsBy(How = How.XPath, Using = MANAGER_BTN_XPATH)]
		protected IWebElement ManagerButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONTINUE_BTN_XPATH)]
		protected IWebElement ContinueButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDITING_BUTTON)]
		protected IWebElement EditingButton { get; set; }

		protected const string TRANSLATE_BTN_XPATH = "//span[contains(string(), 'Translation')]";
		protected const string MANAGER_BTN_XPATH = "//span[contains(@id, 'manager')]";
		protected const string CONTINUE_BTN_XPATH = "//span[contains(string(), 'Continue')]";
		protected const string EDITING_BUTTON = "//span[contains(string(), 'Editing')]";
	}
}
