using NUnit.Framework;
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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог удаления проекта.");
			}
		}

		/// <summary>
		/// Нажать кнопку "Удалить"
		/// </summary>
		public ProjectsPage ClickConfirmDeleteButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Удалить'.");
			ConfirmDeleteButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		[FindsBy(How = How.XPath, Using = CONFIRM_DELETE_BUTTON)]
		protected IWebElement ConfirmDeleteButton { get; set; }

		protected const string CONFIRM_DELETE_BUTTON = "//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]";
	}
}