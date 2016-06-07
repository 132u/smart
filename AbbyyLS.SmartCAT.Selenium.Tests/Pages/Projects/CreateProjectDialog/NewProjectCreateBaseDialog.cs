using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectCreateBaseDialog : ProjectsPage, IAbstractPage<NewProjectCreateBaseDialog>
	{
		public NewProjectCreateBaseDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectCreateBaseDialog LoadPage()
		{
			if (!IsProjectCreateDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось загрузить диалог создания проекта");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на ссылку Settings в навигационном меню
		/// </summary>
		public NewProjectSettingsPage ClickSettingsLink()
		{
			CustomTestContext.WriteLine("Нажать на ссылку Settings в навигационном меню");
			SettingsLink.Click();

			return new NewProjectSettingsPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыт ли диалог создания нового проекта
		/// </summary>
		public bool IsProjectCreateDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SETTINGS_LINK)]
		protected IWebElement SettingsLink { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CREATE_PROJECT_DIALOG = "//div[contains(@class,'js-popup-create-project')][2]";
		protected const string SETTINGS_LINK = "//ul[contains(@data-bind, 'steps')]//li[3]//span";

		#endregion
	}
}
