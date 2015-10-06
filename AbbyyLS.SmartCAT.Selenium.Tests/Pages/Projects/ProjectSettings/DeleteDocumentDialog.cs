using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class DeleteDocumentDialog : ProjectSettingsPage, IAbstractPage<DeleteDocumentDialog>
	{
		public DeleteDocumentDialog(WebDriver driver) : base(driver)
		{
		}

		public new DeleteDocumentDialog GetPage()
		{
			var newDeleteDocumentDialog = new DeleteDocumentDialog(Driver);
			InitPage(newDeleteDocumentDialog, Driver);
			newDeleteDocumentDialog.DeleteDocumentButton = Driver.FindElement(By.XPath(DELETE_DOCUMENT_BTN));

			return newDeleteDocumentDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_DOCUMENT_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог удаления документа.");
			}
		}

		/// <summary>
		/// Подтвердить удаление
		/// </summary>
		public ProjectSettingsPage ConfirmDelete()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить'.");
			DeleteDocumentButton.Click();

			return new ProjectSettingsPage(Driver).GetPage();
		}

		protected IWebElement DeleteDocumentButton { get; set; }

		protected const string DELETE_DOCUMENT_BTN = "//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]";

	}
}