using System.Collections.Generic;

using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class DeleteDocumentDialog : ProjectSettingsPage, IAbstractPage<DeleteDocumentDialog>
	{
		public new DeleteDocumentDialog GetPage()
		{
			var newDeleteDocumentDialog = new DeleteDocumentDialog();
			InitPage(newDeleteDocumentDialog);
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
			Logger.Debug("Нажать кнопку 'Удалить'.");
			DeleteDocumentButton.Click();

			return new ProjectSettingsPage().GetPage();
		}

		protected IWebElement DeleteDocumentButton { get; set; }

		protected const string DELETE_DOCUMENT_BTN = "//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]";

	}
}