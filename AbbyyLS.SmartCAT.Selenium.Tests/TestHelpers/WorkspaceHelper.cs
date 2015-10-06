﻿using System;
using System.Threading;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class WorkspaceHelper
	{
		public WebDriver Driver { get; private set; }

		public WorkspaceHelper(WebDriver driver)
		{
			Driver = driver;
			_projectsPage = new ProjectsPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
		}

		public ClientsHelper GoToClientsPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickClientsButton();

			return new ClientsHelper(Driver);
		}

		public ProjectGroupsHelper GoToProjectGroupsPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickProjectGroupsButton();

			return new ProjectGroupsHelper(Driver);
		}

		public GlossariesHelper GoToGlossariesPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickGlossariesButton();

			return new GlossariesHelper(Driver);
		}

		public WorkspaceHelper AssertLingvoDictionariesDisplayed()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.AssertLingvoDictionariesDisplayed();

			return this;
		}

		public WorkspaceHelper AssertLingvoDictionariesIsNotDisplayed()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.AssertLingvoDictionariesMenuIsNotDisplayed();

			return this;
		}

		public LingvoDictionariesHelper GoToLingvoDictionariesPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickLingvoDictionariesButton();

			return new LingvoDictionariesHelper(Driver);
		}

		public TranslationMemoriesHelper GoToTranslationMemoriesPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickTranslationMemoriesButton();

			return new TranslationMemoriesHelper(Driver);
		}

		public ProjectSettingsHelper GoToProjectSettingsPage(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage
				.AssertProjectAppearInList(projectName)
				.AssertIsProjectLoaded(projectName)
				.ClickProject(projectName);

			return new ProjectSettingsHelper(Driver);
		}

		public ProjectsHelper GoToProjectsPage() 
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickProjectsButton();

			return new ProjectsHelper(Driver);
		}

		public WorkspaceHelper RefreshPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage.RefreshPage<WorkspacePage>(Driver);

			return this;
		}

		public THelper RefreshPage<TAbstractPage, THelper>() where TAbstractPage : class, IAbstractPage<TAbstractPage>
															 where THelper : class
		{
			BaseObject.InitPage(_workspacePage, Driver);
			// Sleep нужен для предотвращения появления unexpected alert
			Thread.Sleep(1000);
			_workspacePage.RefreshPage<TAbstractPage>(Driver);

			var instance = Activator.CreateInstance(typeof(THelper), new object[] { Driver }) as THelper;
			return instance;
		}


		public UsersRightsHelper GoToUsersRightsPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickUsersRightsButton();

			return new UsersRightsHelper(Driver);
		}

		public BillingHelper GoToBillingPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			BaseObject.InitPage(_projectsPage, Driver);

			_workspacePage
				.ClickAccount()
				.AssertAccountProfileDisplayed()
				.ClickLicenseAndServices()
				.SwitchToLicenseAndServicesWindow();

			return new BillingHelper(Driver);
		}

		public WorkspaceHelper SelectLocale(Language language)
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.ClickCloseHelp()
				.SelectLocale(language);

			if (language == Language.Russian)
			{
				_workspacePage.RefreshPage<WorkspacePage>(Driver);
			}

			return this;
		}

		public SearchHelper GotToSearchPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickSearchButton();

			return new SearchHelper(Driver);
		}
		
		public WorkspaceHelper AssertUserNameAndAccountNameCorrect(string userName, string accountName)
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.AssertUserNameMatch(userName)
				.AssertAccountNameMatch(accountName);

			return this;
		}
		
		public WorkspaceHelper CloseTour()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage.ClickCloseHelp();

			return this;
		}

		public LoginHelper SignOut()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.ClickAccount()
				.ClickSignOut();

			return new LoginHelper(Driver);
		}

		public WorkspaceHelper AssertAccountExistInList(string account)
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.ClickAccount()
				.AssertAccountListContainsAccountName(account);

			return this;
		}

		public WorkspaceHelper AssertAlertNoExist()
		{
			BaseObject.InitPage(_workspacePage, Driver);
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

		private readonly ProjectsPage _projectsPage;
		private readonly WorkspacePage _workspacePage;
	}
}
