using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class DeleteProjectDialog : ProjectsPage, IAbstractPage<DeleteProjectDialog>
	{
		public new DeleteProjectDialog GetPage()
		{
			var newDeleteProjectDialog = new DeleteProjectDialog();
			InitPage(newDeleteProjectDialog);
			newDeleteProjectDialog.DeleteProjectButton = Driver.FindElement(By.XPath(DELETE_BTN_XPATH));

			return newDeleteProjectDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог удаления проекта.");
			}
		}

		/// <summary>
		/// Нажать кнопку "Удалить"
		/// </summary>
		public ProjectsPage ClickDeleteButton()
		{
			Logger.Debug("Нажать кнопку 'Удалить'.");
			DeleteProjectButton.Click();

			return new ProjectsPage().GetPage();
		}

		protected IWebElement DeleteProjectButton { get; set; }

		protected const string DELETE_BTN_XPATH = "//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]";

	}
}