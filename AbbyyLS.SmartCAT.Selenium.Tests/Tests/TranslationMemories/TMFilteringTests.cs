using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	class TMFilteringTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			_projectGroupName_1 = Guid.NewGuid().ToString();
			_projectGroupName_2 = Guid.NewGuid().ToString();
			_clientName_1 = Guid.NewGuid().ToString();
			_clientName_2 = Guid.NewGuid().ToString();
			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			_workspaceHelper
				.GoToProjectGroupsPage()
				.CreateProjectGroup(_projectGroupName_1)
				.CreateProjectGroup(_projectGroupName_2)
				.GoToClientsPage()
				.CreateNewClient(_clientName_1)
				.CreateNewClient(_clientName_2)
				.GoToTranslationMemoriesPage()
				.GetTranslationMemoryUniqueName(ref _tmForFilteringName_1)
				.GetTranslationMemoryUniqueName(ref _tmForFilteringName_2)
				.CreateTranslationMemory(_tmForFilteringName_1, Language.French, Language.German)
				.CreateTranslationMemory(_tmForFilteringName_2, Language.German, Language.English)
				.OpenTranslationMemoryInformation(_tmForFilteringName_1)
				.EditTranslationMemory(
					() => _translationMemoriesHelper.AddTopicToTranslationMemory(_tmForFilteringName_1, _topicName_1))
				.EditTranslationMemory(
					() => _translationMemoriesHelper.AddProjectGroupToTranslationMemory(_tmForFilteringName_1, _projectGroupName_1))
				.EditTranslationMemory(
					() => _translationMemoriesHelper.AddClientToTranslationMemory(_tmForFilteringName_1, _clientName_1))
				.CloseTranslationMemoryInformation(_tmForFilteringName_1)
				.OpenTranslationMemoryInformation(_tmForFilteringName_2)
				.EditTranslationMemory(
					() => _translationMemoriesHelper.AddTopicToTranslationMemory(_tmForFilteringName_2, _topicName_2))
				.EditTranslationMemory(
					() => _translationMemoriesHelper.AddProjectGroupToTranslationMemory(_tmForFilteringName_2, _projectGroupName_2))
				.EditTranslationMemory(
					() => _translationMemoriesHelper.AddClientToTranslationMemory(_tmForFilteringName_2, _clientName_2))
				.CloseTranslationMemoryInformation(_tmForFilteringName_2);
		}

		[SetUp]
		public void Setup()
		{
			TranslationMemoriesHelper
				.GoToTranslationMemoriesPage()
				.ClearFiltersPanelIfExist();
		}

		[TearDown]
		public void TearDown()
		{
			if (_secondUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, _secondUser);
			}
		}

		[Test]
		public void TmFiltrationOneSourceLanguage()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetSourceLanguageFilter(Language.French))
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationTwoSourceLanguage()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetSourceLanguageFilter(Language.French))
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetSourceLanguageFilter(Language.German), clearFilters: false)
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationOneTargetLanguage()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetTargetLanguageFilter(Language.German))
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationTwoTargetLanguage()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetTargetLanguageFilter(Language.German))
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetTargetLanguageFilter(Language.English), clearFilters: false)
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationCreationDateNotExist()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetCreationDateTMFilterFrom(DateTime.Now.AddDays(1)))
				.AssertTranslationMemoryNotExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationSpecificTopic()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetTopicFilter(_topicName_1))
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationCommonTopic()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetTopicFilter(_commonTopicName))
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationOneProjectGroup()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetProjectGroupFilter(_projectGroupName_1))
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationTwoProjectGroup()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetProjectGroupFilter(_projectGroupName_1))
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetProjectGroupFilter(_projectGroupName_2), clearFilters: false)
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationOneClient()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetClientFilter(_clientName_1))
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2);
		}

		public void TmFiltrationTwoClients()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetClientFilter(_clientName_1))
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetClientFilter(_clientName_2), clearFilters: false)
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryExists(_tmForFilteringName_2);
		}

		[Category("PRX_12015")]
		[Test, Explicit("Тест исключен в связи с багой PRX_12015")]
		public void TmFiltrationAuthor()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);
			_workspaceHelper
				.SignOut()
				.SignIn(_secondUser.Login, _secondUser.Password)
				.SelectAccount()
				.CloseTour()
				.GoToTranslationMemoriesPage()
				.GetTranslationMemoryUniqueName(ref _tmForFilteringName_3)
				.CreateTranslationMemory(_tmForFilteringName_3)
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetAutorFilter(_secondUser.NickName))
				.AssertTranslationMemoryExists(_tmForFilteringName_3)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2);
		}

		[Test]
		public void CancelTmFiltration()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetSourceLanguageFilter(Language.French))
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2)
				.ClearFiltersPanelIfExist()
				.FindTranslationMemory(_tmForFilteringName_1)
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.FindTranslationMemory(_tmForFilteringName_2)
				.AssertTranslationMemoryExists(_tmForFilteringName_2);
		}

		[Test]
		public void CheckCancelTmFiltrationButton()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetSourceLanguageFilter(Language.French),
					cancelFilterCreation: true)
				.FindTranslationMemory(_tmForFilteringName_1)
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.FindTranslationMemory(_tmForFilteringName_2)
				.AssertTranslationMemoryExists(_tmForFilteringName_2);
		}
		
		[Test]
		public void CheckDifferentTmFiltersApplying()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetTopicFilter(_commonTopicName))
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryExists(_tmForFilteringName_2)
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetSourceLanguageFilter(Language.French), clearFilters: false)
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2)
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetClientFilter(_clientName_2), clearFilters: false)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2);
		}

		[Test]
		public void TmFiltersCheckOneOfManyFilterRemoving()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetSourceLanguageFilter(Language.French))
				.CreateNewTMFilter(() => TranslationMemoriesHelper.SetClientFilter(_clientName_2), clearFilters: false)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2)
				.ClickRemoveFilterButton("Client")
				.AssertTranslationMemoryExists(_tmForFilteringName_1)
				.AssertTranslationMemoryNotExists(_tmForFilteringName_2);
		}

		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
		private readonly LoginHelper _loginHelper = new LoginHelper();
		private readonly TranslationMemoriesHelper _translationMemoriesHelper = new TranslationMemoriesHelper();

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
	}
}
