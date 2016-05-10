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



		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница с отменёнными проектами.
		/// </summary>
		public bool IsCancelledProjectsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CANCELLED_PROJECTS_TAB));
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

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CANCELLED_PROJECTS_TAB)]
		protected IWebElement CancelledProjectsTab { get; set; }

		protected IWebElement ProjectNameInList { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CANCELLED_PROJECTS_TAB = "//div[contains(@class, 'g-nav-tabs')]//a[contains(@class, 'active') and contains(text(), 'Cancelled Projects') ]";
		protected const string PROJECT_NAME_IN_LIST = "//table[contains(@class, 'l-corpr__tbl js-tasks-table js-tour-projects JColResizer')]//tbody//td[2]//div//a[contains(text(), '*#*')]";

		#endregion
	}
}