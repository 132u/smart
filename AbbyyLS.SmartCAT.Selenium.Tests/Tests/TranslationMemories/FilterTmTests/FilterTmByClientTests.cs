using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	[Parallelizable(ParallelScope.Fixtures)]
	[TranslationMemories]
	class FilterTmByClientTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			_loginHelper = new LoginHelper(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);
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
					client: _clientName_1)
				.CreateTranslationMemory(
					translationMemoryName: _tmForFilteringName_2,
					sourceLanguage: Language.German,
					targetLanguage: Language.English,
					client: _clientName_2);
		}

		[SetUp]
		public void Setup()
		{
			TranslationMemoriesPage.ClearFiltersPanelIfExist();
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

		private LoginHelper _loginHelper;
		private TranslationMemoriesHelper _translationMemoriesHelper;
		private ClientsPage _clientsPage;
		private WorkspacePage _workspacePage;

		private string _tmForFilteringName_1 = "TmForFiltering_First";
		private string _tmForFilteringName_2 = "TmForFiltering_Second";
		private string _clientName_1;
		private string _clientName_2;
	}
}
