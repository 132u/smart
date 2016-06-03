using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class CancelledProjectsPage : WorkspacePage, IAbstractPage<CancelledProjectsPage>
	{
		public CancelledProjectsPage(WebDriver driver)
			: base(driver)
		{
		}

		public new CancelledProjectsPage LoadPage()
		{
			if (!IsCancelledProjectsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось перейти на вкладку \"Отменённые проекты\".");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Навести курсор на строку таблицы с проектом
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public CancelledProjectsPage HoverProjectRow(string projectName)
		{
			CustomTestContext.WriteLine("Навести курсор на строку таблицы с проектом {0}", projectName);
			ProjectRow = Driver.SetDynamicValue(How.XPath, PROJECT_ROW, projectName);
			ProjectRow.HoverElement();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку восстановления проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public RestoreProjectDialog ClickRestoreProjectButton(string projectName)
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку восстановления проекта {0}", projectName);
			RestoreProjectButton = Driver.SetDynamicValue(How.XPath, RESTORE_PROJECT_BUTTON, projectName);
			RestoreProjectButton.Click();

			return new RestoreProjectDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть на таб перехода ко всем проектам
		/// </summary>
		public ProjectsPage ClickProjectsTab()
		{
			CustomTestContext.WriteLine("Кликнуть на таб перехода ко всем проектам");
			ProjectsTab.Click();

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Открыть свёртку проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public CancelledProjectsPage OpenProjectInfo(string projectName)
		{
			HoverProjectRow(projectName);
			CustomTestContext.WriteLine(string.Format("Открыть свертку проекта {0}", projectName));

			if (!getClassAttributeProjectInfo(projectName).Contains("opened"))
			{
				ProjectRow = Driver.SetDynamicValue(How.XPath, PROJECT_ROW, projectName);
				ProjectRow.Click();
			}

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Открыть диалог восстановления проекта.
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public RestoreProjectDialog OpenRestoreProjectDialog(string projectName)
		{
			OpenProjectInfo(projectName);
			ClickRestoreProjectButton(projectName);

			return new RestoreProjectDialog(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница с отменёнными проектами.
		/// </summary>
		public bool IsCancelledProjectsPageOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(CANCELLED_PROJECTS_TAB));
		}

		/// <summary>
		/// Проверить, имеется ли в списке отменённых проектов конкретный проект.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool IsProjectNameContainsInCancelledProjects(string projectName)
		{
			CustomTestContext.WriteLine(
				"Проверить, имеется ли в списке отменённых проектов конкретный проект - {0}.", projectName);
			ProjectNameInList = Driver.SetDynamicValue(How.XPath, PROJECT_NAME_IN_LIST, projectName);

			return ProjectNameInList.Displayed;
		}

		#endregion

		#region Вспомогательные методы страницы
		
		/// <summary>
		/// Получить класс элемента, где отображается имя проекта.
		/// Используется, чтобы понять, открыта ли свёртка проекта.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		private string getClassAttributeProjectInfo(string projectName)
		{
			return Driver
				.FindElement(By.XPath(PROJECT_OPEN.Replace("*#*", projectName)))
				.GetElementAttribute("class");
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CANCELLED_PROJECTS_TAB)]
		protected IWebElement CancelledProjectsTab { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECTS_TAB)]
		protected IWebElement ProjectsTab { get; set; }

		protected IWebElement ProjectNameInList { get; set; }

		protected IWebElement ProjectRow { get; set; }

		protected IWebElement RestoreProjectButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CANCELLED_PROJECTS_TAB = "//div[contains(@class, 'g-nav-tabs')]//a[contains(@class, 'active') and contains(text(), 'Cancelled Projects') ]";
		protected const string PROJECT_NAME_IN_LIST = "//table[contains(@class, 'l-corpr__tbl js-tasks-table js-tour-projects JColResizer')]//tbody//td[2]//div//span[contains(text(), '*#*')]";
		protected const string PROJECT_ROW = "//span[text()='*#*']//ancestor::tr//td[@class='l-corpr__td l-project-list__projname-td']";
		protected const string PROJECT_OPEN = "//*[text()='*#*']/ancestor::tr";
		protected const string RESTORE_PROJECT_BUTTON = "(//span[text()='*#*']//ancestor::tr/following-sibling::tr)[1]//div[contains(@data-bind,'restoreProject')]";
		protected const string PROJECTS_TAB = "//a[@href='/Workspace?tab=Current']";
		
		#endregion
	}
}