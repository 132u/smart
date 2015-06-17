using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
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

		public WorkspaceHelper AssertLingvoDictionariesDisplayed()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.ExpandResourcesIfNotExpanded()
				.AssertLingvoDictionariesDisplayed();

			return this;
		}

		public WorkspaceHelper AssertLingvoDictionariesIsNotDisplayed()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.ExpandResourcesIfNotExpanded()
				.AssertLingvoDictionariesMenuIsNotDisplayed();

			return this;
		}

		public LingvoDictionariesHelper GoToLingvoDictionariesPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.ExpandResourcesIfNotExpanded()
				.ClickLingvoDictionariesButton();

			return new LingvoDictionariesHelper();
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

		public UsersRightsHelper GoToUsersRightsPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage.ClickUsersRightsButton();

			return new UsersRightsHelper();
		}

		public BillingHelper GoToBillingPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.ClickAccount()
				.AssertAccountProfileDisplayed()
				.ClickLicenseAndServices()
				.SwitchToLicenseAndServicesWindow();

			return new BillingHelper();
		}

		public WorkspaceHelper SelectLocale(Language language)
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage.SelectLocale(language);

			return this;
		}

		public SearchHelper GotToSearchPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.ExpandResourcesIfNotExpanded()
				.ClickSearchButton();

			return new SearchHelper();
		}

		private readonly ProjectsPage _projectsPage = new ProjectsPage();
		private readonly WorkspacePage _workspacePage = new WorkspacePage();


	}
}
