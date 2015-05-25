using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectsHelper : WorkspaceHelper
	{

		public ProjectsHelper SelectProjectInList(string projectNameToDelete)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.SelectProjectInList(projectNameToDelete);

			return this;
		}

		public ProjectsHelper DeleteProjectFromList()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.ClickDeleteProject()
				.ClickDeleteButton()
				.WaitDeleteProjectDialogDissapeared();

			return this;
		}

		public ProjectsHelper DeleteProjectWithFileFromList()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.ClickDeleteOpenProjectWithFile()
				.ClickDeleteProjectButton()
				.WaitDeleteProjectWithFileDialogDissapeared();

			return this;
		}

		public ProjectsHelper AssertProjectSuccessfullyDeleted(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.AssertProjectNotExist(projectName);

			return this;
		}

		public ProjectsHelper OpenProjectInfo(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.OpenProjectInfo(projectName);

			return this;
		}

		public ProjectsHelper CheckProjectAppearInList(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.CheckProjectAppearInList(projectName);

			return this;
		}

		/// <summary>
		/// Открыть диалог создания проекта
		/// </summary>
		public CreateProjectHelper ClickCreateProjectButton()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickCreateProjectButton();

			return new CreateProjectHelper();
		}

		private readonly ProjectsPage _projectsPage = new ProjectsPage();
	}
}