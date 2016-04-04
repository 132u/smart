using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class ConfirmTermWithoutTranskationDialog : AddTermDialog, IAbstractPage<ConfirmTermWithoutTranskationDialog>
	{
		public ConfirmTermWithoutTranskationDialog(WebDriver driver) :base(driver)
		{
		}

		public new ConfirmTermWithoutTranskationDialog LoadPage()
		{
			if (!IsConfirmTermWithoutTranslationDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог подтверждения сохранения термина без перевода");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать Yes в окне подтверждения добавления термина
		/// </summary>
		public EditorPage ConfirmSaving()
		{
			CustomTestContext.WriteLine("Нажать Yes в окне подтверждения");
			ConfirmYesBtn.Click();

			return new EditorPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что появился диалог подтверждения сохранения термина без перевода
		/// </summary>
		public bool IsConfirmTermWithoutTranslationDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_TERM_WITHOUT_TRANSLATION_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = СONFIRM_YES_BTN)]
		protected IWebElement ConfirmYesBtn { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CONFIRM_TERM_WITHOUT_TRANSLATION_DIALOG = "//div[contains(@id, 'messagebox') and contains(string(), 'Do you want to add a term without translation?')]";
		protected const string СONFIRM_YES_BTN = "//div[contains(@id, 'messagebox')]//span[contains(string(), 'Yes')]";

		#endregion
	}
}
