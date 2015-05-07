using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class WorkspaceHelper
	{
		public ClientsHelper GoToClientsPage()
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage.ClickClientsButton();

			return new ClientsHelper();
		}

		public ProjectGroupsHelper GoToProjectGroupsPage()
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage.ClickProjectGroupsButton();

			return new ProjectGroupsHelper();
		}

		public GlossariesHelper GoToGlossariesPage()
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ExpandResourcesIfNotExpanded()
				.ClickGlossariesButton();

			return new GlossariesHelper();
		}

		public TranslationMemoriesHelper GoToTranslationMemoriesPage()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
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
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickProjectsButton();

			return new ProjectsHelper();
		}

		private readonly ClientsPage _clientsPage = new ClientsPage();
		private readonly ProjectGroupsPage _projectGroupsPage = new ProjectGroupsPage();
		private readonly ProjectsPage _projectsPage = new ProjectsPage();
		private readonly GlossariesPage _glossariesPage = new GlossariesPage();
		private readonly NewProjectSetUpTMDialog _newProjectSetUpTMDialog = new NewProjectSetUpTMDialog();
		private readonly TranslationMemoriesPage _translationMemoriesPage = new TranslationMemoriesPage();
	}
}
