using OpenQA.Selenium;

using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class DeleteDocumentDialog : ProjectSettingsPage, IAbstractPage<DeleteDocumentDialog>
	{
		public DeleteDocumentDialog(WebDriver driver) : base(driver)
		{
		}

		public new DeleteDocumentDialog LoadPage()
		{
			if (!IsDeleteDocumentDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог удаления документа");
			}

			return this;
		}

		/// <summary>
		/// Подтвердить удаление
		/// </summary>
		public ProjectSettingsPage ConfirmDelete()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить'.");
			DeleteDocumentButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Проверить, открылся ли диалог удаления документа
		/// </summary>
		public bool IsDeleteDocumentDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_DOCUMENT_DIALOG));
		}

		[FindsBy(How = How.XPath, Using = DELETE_DOCUMENT_BUTTON)]
		protected IWebElement DeleteDocumentButton { get; set; }

		protected const string DELETE_DOCUMENT_BUTTON = "//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]";

	}
}