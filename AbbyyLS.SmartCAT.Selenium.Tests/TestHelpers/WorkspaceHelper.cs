using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class WorkspaceHelper
	{
		public ClientsHelper GoToClientsPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage.ClickClientsButton();

			return new ClientsHelper();
		}

		public ProjectGroupsHelper GoToProjectGroupsPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage.ClickProjectGroupsButton();

			return new ProjectGroupsHelper();
		}

		public GlossariesHelper GoToGlossariesPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.ExpandResourcesIfNotExpanded()
				.ClickGlossariesButton();

			return new GlossariesHelper();
		}

		public TranslationMemoriesHelper GoToTranslationMemoriesPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.ExpandResourcesIfNotExpanded()
				.ClickTranslationMemoriesButton();

			return new TranslationMemoriesHelper();
		}

		public ProjectSettingsHelper GoToProjectSettingsPage(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.AssertIsProjectLoaded(projectName)
				.ClickProject(projectName);

			return new ProjectSettingsHelper();
		}

		public ProjectsHelper GoToProjectsPage() 
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage.ClickProjectsButton();

			return new ProjectsHelper();
		}

		private readonly ProjectsPage _projectsPage = new ProjectsPage();
		private readonly WorkspacePage _workspacePage = new WorkspacePage();
	}
}
