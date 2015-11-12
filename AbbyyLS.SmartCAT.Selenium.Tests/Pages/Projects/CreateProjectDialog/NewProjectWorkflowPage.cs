using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectWorkflowPage : WorkspacePage, IAbstractPage<NewProjectWorkflowPage>
	{
		public NewProjectWorkflowPage(WebDriver driver)
			: base(driver)
		{
		}

		public new NewProjectWorkflowPage GetPage()
		{
			var newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			InitPage(newProjectWorkflowPage, Driver);

			return newProjectWorkflowPage;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!IsNewProjectWorkflowPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылась страница Workflow для создаваемого проекта");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку 'Create Project'
		/// </summary>
		public ProjectsPage ClickCreateProjectButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Create Project'");
			CreateProjectButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница Workflow для создаваемого проекта
		/// </summary>
		public bool IsNewProjectWorkflowPageOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылась ли страница Workflow для создаваемого проекта");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_BUTTON)); 
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_BUTTON)]
		protected IWebElement CreateProjectButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CREATE_PROJECT_BUTTON = "//div[contains(@data-bind,'WorkflowStep')]";

		#endregion
	}
}
