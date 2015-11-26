using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class DeleteOpenProjectWithFileDialog : ProjectsPage, IAbstractPage<DeleteOpenProjectWithFileDialog>
	{
		public DeleteOpenProjectWithFileDialog(WebDriver driver) : base(driver)
		{
		}

		public new DeleteOpenProjectWithFileDialog GetPage()
		{
			var newDeleteOpenProjectWithFileDialog = new DeleteOpenProjectWithFileDialog(Driver);
			InitPage(newDeleteOpenProjectWithFileDialog, Driver);

			return newDeleteOpenProjectWithFileDialog;
		}

		public new void LoadPage()
		{
			if (!IsDeleteOpenProjectWithFileDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог удаления проекта с файлом");
			}
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

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, открыт ли диалог удаления проекта с файлом
		/// </summary>
		public bool IsDeleteOpenProjectWithFileDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_DIALOG_WITH_FILE));
		}

		/// <summary>
		/// Проверить, что диалог удаления проекта с файлом закрылся
		/// </summary>
		public bool IsDeleteProjectWithFileDialogDissapeared()
		{
			CustomTestContext.WriteLine("Проверить, что диалог удаления проекта с файлом закрылся");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_DIALOG_WITH_FILE));
		}

		[FindsBy(How = How.XPath, Using = DELETE_PROJECT_BTN)]
		protected IWebElement DeleteProjectButton { get; set; }

		protected const string DELETE_DIALOG_WITH_FILE = "//div[contains(@class,'js-popup-confirm')]";
		protected const string DELETE_PROJECT_BTN = "//div[contains(@class, 'js-popup-confirm')]//form[@class='js-ajax-form-submit']//input[@data-close-text='Close']";
	}
}