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
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickClientsButton();

			return new ClientsHelper();
		}

		public ProjectGroupsHelper GoToProjectGroupsPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickProjectGroupsButton();

			return new ProjectGroupsHelper();
		}

		public GlossariesHelper GoToGlossariesPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickGlossariesButton();

			return new GlossariesHelper();
		}

		public WorkspaceHelper AssertLingvoDictionariesDisplayed()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.AssertLingvoDictionariesDisplayed();

			return this;
		}

		public WorkspaceHelper AssertLingvoDictionariesIsNotDisplayed()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.AssertLingvoDictionariesMenuIsNotDisplayed();

			return this;
		}

		public LingvoDictionariesHelper GoToLingvoDictionariesPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickLingvoDictionariesButton();

			return new LingvoDictionariesHelper();
		}

		public TranslationMemoriesHelper GoToTranslationMemoriesPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickTranslationMemoriesButton();

			return new TranslationMemoriesHelper();
		}

		public ProjectSettingsHelper GoToProjectSettingsPage(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.AssertProjectAppearInList(projectName)
				.AssertIsProjectLoaded(projectName)
				.ClickProject(projectName);

			return new ProjectSettingsHelper();
		}

		public ProjectsHelper GoToProjectsPage() 
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickProjectsButton();

			return new ProjectsHelper();
		}

		public UsersRightsHelper GoToUsersRightsPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickUsersRightsButton();

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
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickSearchButton();

			return new SearchHelper();
		}

		private readonly ProjectsPage _projectsPage = new ProjectsPage();
		private readonly WorkspacePage _workspacePage = new WorkspacePage();


	}
}
