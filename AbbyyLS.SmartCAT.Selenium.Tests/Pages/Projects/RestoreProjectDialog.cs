using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class RestoreProjectDialog : CancelledProjectsPage, IAbstractPage<RestoreProjectDialog>
	{
		public RestoreProjectDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new RestoreProjectDialog LoadPage()
		{
			if (!IsRestoreProjectDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть диалог восстановления проекта.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть кнопку Restore
		/// </summary>
		public CancelledProjectsPage ClickRestoreButton()
		{
			CustomTestContext.WriteLine("Кликнуть кнопку Restore");
			RestoreButton.JavaScriptClick();

			return new CancelledProjectsPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог восстановления проекта
		/// </summary>
		public bool IsRestoreProjectDialogOpened()
		{
			return Driver.WaitUntilElementIsAppear(By.XPath(RESTORE_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = RESTORE_BUTTON)]
		protected IWebElement RestoreButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string RESTORE_BUTTON = "//div[contains(@class,'js-popup-confirm')]//input[@value='Restore']";

		#endregion
	}
}