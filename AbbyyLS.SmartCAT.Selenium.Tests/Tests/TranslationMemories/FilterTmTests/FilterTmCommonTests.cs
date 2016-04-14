using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	[Parallelizable(ParallelScope.Fixtures)]
	class FilterTmCommonTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			_loginHelper = new LoginHelper(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);

			_signInPage = new SignInPage(Driver);
			_clientsPage = new ClientsPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			
			_clientName_1 = _clientsPage.GetClientUniqueName();
			_clientName_2 = _clientsPage.GetClientUniqueName();
			_tmForFilteringName_1 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_tmForFilteringName_2 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			
			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();

			_clientsPage
				.CreateNewClient(_clientName_1)
				.CreateNewClient(_clientName_2);

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesHelper
				.CreateTranslationMemory(
					translationMemoryName: _tmForFilteringName_1,
					sourceLanguage: Language.French,
					targetLanguage: Language.German,
					client: _clientName_1,
					topic: _topicName_1)
				.CreateTranslationMemory(
					translationMemoryName: _tmForFilteringName_2,
					sourceLanguage: Language.German,
					targetLanguage: Language.English,
					client: _clientName_2,
					topic: _topicName_2);
		}

		[SetUp]
		public void Setup()
		{
			_secondUser = null;
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
		public void TmFiltrationCreationDateNotExist()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setCreationDateTMFilterFrom:DateTime.Now.AddDays(1));

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationAuthor()
		{
			_tmForFilteringName_3 = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
			_secondUser = TakeUser(ConfigurationManager.Users);

			WorkspacePage.SignOut();

			_signInPage
				.SubmitForm(_secondUser.Login, _secondUser.Password)
				.SelectAccount();

			WorkspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesHelper.CreateTranslationMemory(_tmForFilteringName_3);

			WorkspacePage.RefreshPage<WorkspacePage>();
			
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

		private LoginHelper _loginHelper;
		private TranslationMemoriesHelper _translationMemoriesHelper;
		private ProjectGroupsPage _projectGroupsPage;
		private ClientsPage _clientsPage;
		private SignInPage _signInPage;
		private WorkspacePage _workspacePage;

		private string _tmForFilteringName_1 = "TmForFiltering_First";
		private string _tmForFilteringName_2 = "TmForFiltering_Second";
		private string _tmForFilteringName_3 = "TmForFiltering_Third";
		private string _clientName_1;
		private string _clientName_2;
		private const string _topicName_1 = "Life";
		private const string _topicName_2 = "Science and technology";
		private const string _commonTopicName = "All topics";
		private TestUser _secondUser;
	}
}
