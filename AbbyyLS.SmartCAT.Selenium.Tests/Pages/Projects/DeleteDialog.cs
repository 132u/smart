using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class DeleteDialog : ProjectsPage, IAbstractPage<DeleteDialog>
	{
		public DeleteDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new DeleteDialog LoadPage()
		{
			if (!IsDeleteDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог удаления проекта/документа.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку "Удалить"
		/// </summary>
		public ProjectsPage ClickConfirmDeleteButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить'.");
			ConfirmDeleteButton.Click();

			if (!IsDeleteDialogDissapeared())
			{
				throw new InvalidElementStateException("Произошла ошибка:\n диалог удаления не закрылся");
			}

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Получить текст из диалога удаления проекта/документа
		/// </summary>
		public string GetTextFromDeleteDialog()
		{
			CustomTestContext.WriteLine("Получить текст из диалога удаления проекта/документа.");

			return DeleteDialogText.Text;
		}

		/// <summary>
		/// Нажать на кнопку удаления документов.
		/// </summary>
		public ProjectsPage ClickDeleteDocumentsButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку удаления документов.");
			DeleteDocumentsButton.Click();

			return new ProjectsPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, ожидающие определенного состояния страницы

		/// <summary>
		/// Проверить, открыт ли диалог удаления проекта
		/// </summary>
		public bool IsDeleteDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_DIALOG));
		}

		/// <summary>
		/// Проверить, закрыт ли диалога удаления проекта
		/// </summary>
		public bool IsDeleteDialogDissapeared()
		{
			CustomTestContext.WriteLine("Проверить, закрыт ли диалога удаления проекта");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CONFIRM_DELETE_BUTTON)]
		protected IWebElement ConfirmDeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_DIALOG)]
		protected IWebElement DeleteDialogText { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_DOCUMENTS_BUTTON)]
		protected IWebElement DeleteDocumentsButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DELETE_DIALOG = "//div[contains(@class, 'popupbox l-confirm')]//div[contains(text(), 'Delete')]";
		protected const string CONFIRM_DELETE_BUTTON = "//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]";
		protected const string DELETE_DIALOG_TEXT = "//div[contains(@class,'js-confirm-text')]";
		protected const string DELETE_DOCUMENTS_BUTTON = "//input[contains(@class,'js-delete-document-btn')]";

		#endregion
	}
}