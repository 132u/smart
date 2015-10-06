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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_DIALOG_WITH_FILE)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог удаления проекта.");
			}
		}

		/// <summary>
		/// Нажать кнопку 'Удалить проект'
		/// </summary>
		public ProjectsPage ClickDeleteProjectButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить проект'.");
			DeleteProjectButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		[FindsBy(How = How.XPath, Using = DELETE_PROJECT_BTN)]
		protected IWebElement DeleteProjectButton { get; set; }

		protected const string DELETE_PROJECT_BTN = "//div[contains(@class, 'js-popup-confirm')]//form[@class='js-ajax-form-submit']//input[@data-close-text='Close']";
	}
}