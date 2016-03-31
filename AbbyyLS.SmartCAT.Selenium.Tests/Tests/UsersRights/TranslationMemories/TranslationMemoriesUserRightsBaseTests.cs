using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TranslationMemoriesUserRightsBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_loginHelper.Authorize(StartPage, AdditionalUser);
				_workspacePage.GoToTranslationMemoriesPage();

				_translationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_deleteTmDialog = new DeleteTmDialog(Driver);
			_importTmxDialog = new ImportTmxDialog(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);
			_clientsPage = new ClientsPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_confirmReplacementDialog = new ConfirmReplacementDialog(Driver);
			_usersTab = new UsersTab(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		protected string _clientName2;
		protected string _commonClientName;
		protected string _commonClientName2;

		protected string _translationMemoryWithoutClient;
		protected string _translationMemoryWithClient;
		protected string _translationMemory;
		protected string _translationMemoryWithSecondClient;

		protected DeleteTmDialog _deleteTmDialog;
		protected ConfirmReplacementDialog _confirmReplacementDialog;
		protected ImportTmxDialog _importTmxDialog;
		protected ProjectGroupsPage _projectGroupsPage;
		protected TranslationMemoriesHelper _translationMemoriesHelper;
		protected TranslationMemoriesPage _translationMemoriesPage;
		protected ClientsPage _clientsPage;
		protected UserRightsHelper _userRightsHelper;
		protected WorkspacePage _workspacePage;
		protected LoginHelper _loginHelper;
		protected ExportNotification _exportNotification;
		protected UsersTab _usersTab;
		protected NewGroupDialog _newGroupDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected AddAccessRightDialog _addAccessRightDialog;
	}
}
