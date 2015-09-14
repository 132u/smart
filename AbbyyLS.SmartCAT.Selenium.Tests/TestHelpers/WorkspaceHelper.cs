﻿using System.Threading;

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

		public WorkspaceHelper RefreshPage()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage.RefreshPage<WorkspacePage>();

			return this;
		}
		
		public THelper RefreshPage<TAbstractPage, THelper>() where TAbstractPage : class, IAbstractPage<TAbstractPage>, new()
															 where THelper : class, new()
		{
			BaseObject.InitPage(_workspacePage);
			// Sleep нужен для предотвращения появления unexpected alert
			Thread.Sleep(1000);
			_workspacePage.RefreshPage<TAbstractPage>();
			
			return new THelper();
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
			BaseObject.InitPage(_projectsPage);

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
			_workspacePage
				.ClickCloseHelp()
				.SelectLocale(language);

			if (language == Language.Russian)
			{
				_workspacePage.RefreshPage<WorkspacePage>();
			}

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
		
		public WorkspaceHelper AssertUserNameAndAccountNameCorrect(string userName, string accountName)
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.AssertUserNameMatch(userName)
				.AssertAccountNameMatch(accountName);

			return this;
		}
		
		public WorkspaceHelper CloseTour()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage.ClickCloseHelp();

			return this;
		}

		public LoginHelper SignOut()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.ClickAccount()
				.ClickSignOut();

			return new LoginHelper();
		}

		public WorkspaceHelper AssertAccountExistInList(string account)
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage
				.ClickAccount()
				.AssertAccountListContainsAccountName(account);

			return this;
		}

		public WorkspaceHelper AssertAlertNoExist()
		{
			BaseObject.InitPage(_workspacePage);
			_workspacePage.AssertAlertNoExist();

			return this;
		}

		public WorkspaceHelper SetUp(
			string nickName,
			string accountName = LoginHelper.TestAccountName,
			Language language = Language.English)
		{
			SelectLocale(language);
			_workspacePage
				.AssertUserNameMatch(nickName)
				.AssertAccountNameMatch(accountName);
				
			return this;
		}

		private readonly ProjectsPage _projectsPage = new ProjectsPage();
		private readonly WorkspacePage _workspacePage = new WorkspacePage();
	}
}
