using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class DeleteProjectOrFileDialog : ProjectsPage, IAbstractPage<DeleteProjectOrFileDialog>
	{
		public DeleteProjectOrFileDialog(WebDriver driver) : base(driver)
		{
		}

		public new DeleteProjectOrFileDialog LoadPage()
		{
			if (!IsDeleteProjectOrFileDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог удаления проекта с файлом");
			}

			return this;
		}

		/// <summary>
		/// Нажать кнопку 'Удалить проект'
		/// </summary>
		public ProjectsPage ClickDeleteProjectButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить проект'.");
			DeleteProjectButton.Click();

			if (!IsDeleteProjectWithFileDialogDissapeared())
			{
				throw new InvalidElementStateException("Произошла ошибка: \nне закрылся диалог удаления проекта с файлом");
			}

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Удалить файл'
		/// </summary>
		public ProjectsPage ClickDeleteFileButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить файл'.");
			DeleteFileButton.Click();

			if (!IsDeleteProjectWithFileDialogDissapeared())
			{
				throw new InvalidElementStateException("Произошла ошибка: \nне закрылся диалог удаления проекта с файлом");
			}

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Проверить, открыт ли диалог удаления проекта с файлом
		/// </summary>
		public bool IsDeleteProjectOrFileDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_DIALOG_OR_FILE));
		}

		/// <summary>
		/// Проверить, что диалог удаления проекта с файлом закрылся
		/// </summary>
		public bool IsDeleteProjectWithFileDialogDissapeared()
		{
			CustomTestContext.WriteLine("Проверить, что диалог удаления проекта с файлом закрылся");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_DIALOG_OR_FILE));
		}

		[FindsBy(How = How.XPath, Using = DELETE_PROJECT_BTN)]
		protected IWebElement DeleteProjectButton { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_FILE_BTN)]
		protected IWebElement DeleteFileButton { get; set; }

		protected const string DELETE_DIALOG_OR_FILE = "//div[contains(@class,'js-popup-delete-mode')]";
		protected const string DELETE_PROJECT_BTN = "//div[contains(@class, 'js-popup-delete-mode')]//form[@class='js-ajax-form-submit']//input[@value='Delete project(s)']";
		protected const string DELETE_FILE_BTN = "//div[contains(@class, 'js-popup-delete-mode')]//form[@class='js-ajax-form-submit']//input[@value='Delete document(s)']";
	}
}