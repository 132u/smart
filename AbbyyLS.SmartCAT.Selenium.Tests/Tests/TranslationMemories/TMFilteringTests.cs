using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	[Parallelizable(ParallelScope.Fixtures)]
	class TMFilteringTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			_loginHelper = new LoginHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);

			_signInPage = new SignInPage(Driver);
			_clientsPage = new ClientsPage(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			
			_projectGroupName_1 = _projectGroupsPage.GetProjectGroupUniqueName();
			_projectGroupName_2 = _projectGroupsPage.GetProjectGroupUniqueName();
			_clientName_1 = _clientsPage.GetClientUniqueName();
			_clientName_2 = _clientsPage.GetClientUniqueName();
			_tmForFilteringName_1 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_tmForFilteringName_2 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			_workspaceHelper.GoToProjectGroupsPage();

			_projectGroupsPage
				.CreateProjectGroup(_projectGroupName_1)
				.CreateProjectGroup(_projectGroupName_2);

			_workspaceHelper.GoToClientsPage();

			_clientsPage
				.CreateNewClient(_clientName_1)
				.CreateNewClient(_clientName_2);

			_workspaceHelper.GoToTranslationMemoriesPage();

			_translationMemoriesHelper
				.CreateTranslationMemory(_tmForFilteringName_1, Language.French, targetLanguage: Language.German)
				.CreateTranslationMemory(_tmForFilteringName_2, Language.German, targetLanguage: Language.English);

			_translationMemoriesPage
				.EditTranslationMemory(_tmForFilteringName_1,
					addTopic: _topicName_1, addProjectGroup: _projectGroupName_1, addClient: _clientName_1)
				.EditTranslationMemory(_tmForFilteringName_2,
					addTopic: _topicName_2, addProjectGroup: _projectGroupName_2, addClient: _clientName_2);
		}

		[SetUp]
		public void Setup()
		{
			_secondUser = null;

			TranslationMemoriesHelper.GoToTranslationMemoriesPage();

			TranslationMemoriesPage.ClearFiltersPanelIfExist();
		}

		[TearDown]
		public void TearDown()
		{
			if (_secondUser != null)
			{
				ReturnUser(ConfigurationManager.Users, _secondUser);
			}
		}

		[Test]
		public void TmFiltrationOneSourceLanguage()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setSourceLanguageFilter: Language.French);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationTwoSourceLanguage()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(setSourceLanguageFilter:Language.French)
				.CreateNewTMFilter(setSourceLanguageFilter:Language.German, clearFilters: false);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationOneTargetLanguage()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setTargetLanguageFilter:Language.German);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationTwoTargetLanguage()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(setTargetLanguageFilter:Language.German)
				.CreateNewTMFilter(setTargetLanguageFilter:Language.English, clearFilters: false);
			
			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationCreationDateNotExist()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setCreationDateTMFilterFrom:DateTime.Now.AddDays(1));

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationSpecificTopic()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setTopicFilter:_topicName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationCommonTopic()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setTopicFilter: _commonTopicName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationOneProjectGroup()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setProjectGroupFilter: _projectGroupName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationTwoProjectGroup()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(setProjectGroupFilter: _projectGroupName_1)
				.CreateNewTMFilter(setProjectGroupFilter: _projectGroupName_2, clearFilters: false);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationOneClient()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setClientFilter: _clientName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationTwoClients()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(setClientFilter: _clientName_1)
				.CreateNewTMFilter(setClientFilter: _clientName_2, clearFilters: false);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Category("PRX_12015")]
		[Test, Explicit("Тест исключен в связи с багой PRX_12015")]
		public void TmFiltrationAuthor()
		{
			_tmForFilteringName_3 = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
			_secondUser = TakeUser(ConfigurationManager.Users);

			_workspaceHelper.SignOut();
			_signInPage
				.SubmitForm(_secondUser.Login, _secondUser.Password)
				.SelectAccount();
			_workspaceHelper
				.CloseTour()
				.GoToTranslationMemoriesPage()
				.CreateTranslationMemory(_tmForFilteringName_3)
				.RefreshPage();
			
			_translationMemoriesHelper.CreateNewTMFilter(setAuthorFilter: _secondUser.NickName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_3),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_3);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void CancelTmFiltration()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setSourceLanguageFilter: Language.French);

			TranslationMemoriesPage
				.ClearFiltersPanelIfExist()
				.SearchForTranslationMemory(_tmForFilteringName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			TranslationMemoriesPage.SearchForTranslationMemory(_tmForFilteringName_2);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void CheckCancelTmFiltrationButton()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setSourceLanguageFilter:Language.French,
				cancelFilterCreation: true);

			TranslationMemoriesPage.SearchForTranslationMemory(_tmForFilteringName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			TranslationMemoriesPage.SearchForTranslationMemory(_tmForFilteringName_2);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_2);
		}
		
		[Test]
		public void CheckDifferentTmFiltersApplying()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setTopicFilter:_commonTopicName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_2);

			TranslationMemoriesHelper.CreateNewTMFilter(setSourceLanguageFilter: Language.French, clearFilters: false);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);

			TranslationMemoriesHelper.CreateNewTMFilter(setClientFilter: _clientName_2, clearFilters: false);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltersCheckOneOfManyFilterRemoving()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(setSourceLanguageFilter: Language.French)
				.CreateNewTMFilter(setClientFilter: _clientName_2, clearFilters: false);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);

			TranslationMemoriesPage.ClickRemoveFilterButton("Client");

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		private WorkspaceHelper _workspaceHelper;
		private LoginHelper _loginHelper;
		private TranslationMemoriesHelper _translationMemoriesHelper;
		private ProjectGroupsPage _projectGroupsPage;
		private ClientsPage _clientsPage;
		private TranslationMemoriesPage _translationMemoriesPage;

		private string _tmForFilteringName_1 = "TmForFiltering_First";
		private string _tmForFilteringName_2 = "TmForFiltering_Second";
		private string _tmForFilteringName_3 = "TmForFiltering_Third";
		private string _projectGroupName_1;
		private string _projectGroupName_2;
		private string _clientName_1;
		private string _clientName_2;
		private const string _topicName_1 = "Life";
		private const string _topicName_2 = "Science and technology";
		private const string _commonTopicName = "All topics";
		private TestUser _secondUser;
		private SignInPage _signInPage;
	}
}
