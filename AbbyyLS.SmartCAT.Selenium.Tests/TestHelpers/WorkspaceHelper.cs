using System;
using System.Threading;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.LingvoDictionaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using OpenQA.Selenium;

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

		public ClientsPage GoToClientsPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickClientsButton();

			return new ClientsPage(Driver).GetPage();
		}

		public ProjectGroupsPage GoToProjectGroupsPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickProjectGroupsButton();

			return new ProjectGroupsPage(Driver);
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

		public LingvoDictionariesPage GoToLingvoDictionariesPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickLingvoDictionariesButton();

			return new LingvoDictionariesPage(Driver);
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

			if (!_projectsPage.IsProjectAppearInList(projectName))
			{
				throw new XPathLookupException("Произошла ошибка: \nпроект не появился в списке");
			}

			if (!_projectsPage.IsProjectLoaded(projectName))
			{
				throw new InvalidElementStateException("Произошла ошибка: \nне изчезла пиктограмма загрузки проекта");
			}

			_projectsPage.ClickProject(projectName);

			return new ProjectSettingsHelper(Driver);
		}

		public ProjectsPage GoToProjectsPage() 
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickProjectsSubmenu();

			return new ProjectsPage(Driver).GetPage();
		}

		public WorkspaceHelper RefreshPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage.RefreshPage<WorkspacePage>();

			return this;
		}

		public THelper RefreshPage<TAbstractPage, THelper>() where TAbstractPage : class, IAbstractPage<TAbstractPage>
															 where THelper : class
		{
			BaseObject.InitPage(_workspacePage, Driver);
			// Sleep нужен для предотвращения появления unexpected alert
			Thread.Sleep(1000);
			_workspacePage.RefreshPage<TAbstractPage>();

			var instance = Activator.CreateInstance(typeof(THelper), new object[] { Driver }) as THelper;
			return instance;
		}


		public UsersTab GoToUsersPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ClickUsersRightsButton();

			return new UsersTab(Driver).GetPage();
		}

		public BillingPage GoToBillingPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			BaseObject.InitPage(_projectsPage, Driver);

			_workspacePage
				.ClickAccount()
				.AssertAccountProfileDisplayed()
				.ClickLicenseAndServices()
				.SwitchToLicenseAndServicesWindow();

			return new BillingPage(Driver).GetPage();
		}

		public WorkspaceHelper SelectLocale(Language language)
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.CloseHelpIfOpened()
				.SelectLocale(language);

			if (language == Language.Russian)
			{
				_workspacePage.RefreshPage<WorkspacePage>();
			}

			return this;
		}

		public SearchPage GotToSearchPage()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded()
				.ClickSearchButton();

			return new SearchPage(Driver).GetPage();
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
			_workspacePage.CloseHelpIfOpened();

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

		public LoginHelper SignOutAssumingAlert()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			_workspacePage
				.ClickAccount()
				.ClickSignOutAssumingAlert();

			_projectsPage.AcceptAlertSwitchToSignInPage();

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

		public WorkspaceHelper SetUp(
			string nickName,
			string accountName = LoginHelper.TestAccountName,
			Language language = Language.English)
		{
			_workspacePage
				.CloseHelpIfOpened()
				.AssertUserNameMatch(nickName)
				.AssertAccountNameMatch(accountName);

			SelectLocale(language);

			return this;
		}

		private readonly ProjectsPage _projectsPage;
		private readonly WorkspacePage _workspacePage;
	}
}
