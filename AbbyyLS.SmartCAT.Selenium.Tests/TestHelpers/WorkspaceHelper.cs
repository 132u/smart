using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class WorkspaceHelper
	{
		public ClientsHelper GoToClientsPage()
		{
			BaseObject.InitPage(_clientsPage);
			_clientsPage.ClickClientsBtn();

			return new ClientsHelper();
		}

		public GlossariesHelper GoToGlossariesPage()
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ExpandResourcesIfNotExpanded()
				.ClickGlossariesBtn();

			return new GlossariesHelper();
		}

		public TranslationMemoriesHelper GoToTranslationMemoriesPage()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ExpandResourcesIfNotExpanded()
				.ClickTranslationMemoriesBtn();

			return new TranslationMemoriesHelper();
		}

		public ProjectSettingsHelper GoToProjectSettingsPage(string projectName)
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog);
			_projectsPage
				.AssertIsProjectLoaded(projectName)
				.ClickProjectRef(projectName);

			return new ProjectSettingsHelper();
		}

		private readonly ClientsPage _clientsPage = new ClientsPage();
		private readonly ProjectsPage _projectsPage = new ProjectsPage();
		private readonly GlossariesPage _glossariesPage = new GlossariesPage();
		private readonly NewProjectSetUpTMDialog _newProjectSetUpTMDialog = new NewProjectSetUpTMDialog();
		private readonly TranslationMemoriesPage _translationMemoriesPage = new TranslationMemoriesPage();
	}
}
