using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	class DeleteTmDialog : TranslationMemoriesPage, IAbstractPage<DeleteTmDialog>
	{
		public DeleteTmDialog(WebDriver driver) : base(driver)
		{
		}

		public new DeleteTmDialog GetPage()
		{
			var deleteTmDialog = new DeleteTmDialog(Driver);
			InitPage(deleteTmDialog, Driver);

			return deleteTmDialog;
		}

		public new void LoadPage()
		{
			if (IsDeleteConfirmatonDialogPresent())
			{
				throw new XPathLookupException("Произошла ошибка: \nне открылся диалог подтверждения удаления ТМ");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Delete в диалоге подтверждения удаления ТМ
		/// </summary>
		public TranslationMemoriesPage ConfirmReplacement()
		{
			CustomTestContext.WriteLine("Нажать кнопку Delete в диалоге подтверждения удаления ТМ.");
			//Перепробовал все что можно. Проблема в особенностях UI.
			Thread.Sleep(1000);
			DeleteButtonInConfirmationDialog.Click();
			Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_CONFIRMATION_DIALOG));

			return new TranslationMemoriesPage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что диалог подтверждения удаления ТМ появился
		/// </summary>
		public bool IsDeleteConfirmatonDialogPresent()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_CONFIRMATION_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = DELETE_CONFIRMATION_DIALOG)]
		protected IWebElement DeleteConfirmationDialog { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_BUTTON)]
		protected IWebElement DeleteButtonInConfirmationDialog { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DELETE_CONFIRMATION_DIALOG = "//form[contains(@action,'Delete')]";
		protected const string DELETE_BUTTON = "//form[contains(@action,'Delete')]//input[@value='Delete']";

		#endregion
	}
}
