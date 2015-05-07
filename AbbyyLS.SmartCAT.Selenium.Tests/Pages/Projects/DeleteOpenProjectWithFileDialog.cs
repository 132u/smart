using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class DeleteOpenProjectWithFileDialog : ProjectsPage, IAbstractPage<DeleteOpenProjectWithFileDialog>
	{
		public new DeleteOpenProjectWithFileDialog GetPage()
		{
			var newDeleteOpenProjectWithFileDialog = new DeleteOpenProjectWithFileDialog();
			InitPage(newDeleteOpenProjectWithFileDialog);

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
			Logger.Debug("Нажать кнопку 'Удалить проект'.");
			DeleteProjectButton.Click();

			return new ProjectsPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = DELETE_PROJECT_BTN)]
		protected IWebElement DeleteProjectButton { get; set; }

		protected const string DELETE_PROJECT_BTN = "//div[contains(@class,'js-popup-delete-mode')]//input[contains(@class,'js-delete-project-btn')]";
	}
}