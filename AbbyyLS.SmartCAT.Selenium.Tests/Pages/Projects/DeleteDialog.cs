using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class DeleteDialog : ProjectsPage, IAbstractPage<DeleteDialog>
	{
		public DeleteDialog(WebDriver driver) : base(driver)
		{
		}

		public new DeleteDialog GetPage()
		{
			var newDeleteProjectDialog = new DeleteDialog(Driver);
			InitPage(newDeleteProjectDialog, Driver);
			newDeleteProjectDialog.DeleteButton = Driver.FindElement(By.XPath(CONFIRM_DELETE_BUTTON));

			return newDeleteProjectDialog;
		}

		public new void LoadPage()
		{
			if (!IsDeleteDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог удаления проекта");
			}
		}

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

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, открыт ли диалог удаления проекта
		/// </summary>
		public bool IsDeleteDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, открыт ли диалог удаления проекта");

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

		[FindsBy(How = How.XPath, Using = CONFIRM_DELETE_BUTTON)]
		protected IWebElement ConfirmDeleteButton { get; set; }

		protected const string DELETE_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string CONFIRM_DELETE_BUTTON = "//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]";
	}
}