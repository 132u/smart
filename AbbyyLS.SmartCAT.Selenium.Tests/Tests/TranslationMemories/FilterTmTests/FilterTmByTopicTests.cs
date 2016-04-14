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
	class FilterTmByTopicTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			_loginHelper = new LoginHelper(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);

			_tmForFilteringName_1 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_tmForFilteringName_2 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesHelper
				.CreateTranslationMemory(
					translationMemoryName: _tmForFilteringName_1,
					sourceLanguage: Language.French,
					targetLanguage: Language.German,
					topic: _topicName_1)
				.CreateTranslationMemory(
					translationMemoryName: _tmForFilteringName_2,
					sourceLanguage: Language.German,
					targetLanguage: Language.English,
					topic: _topicName_2);
		}

		[SetUp]
		public void Setup()
		{
			TranslationMemoriesPage.ClearFiltersPanelIfExist();
		}

		[Test]
		public void TmFiltrationSpecificTopic()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setTopicFilter: _topicName_1);

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

		private LoginHelper _loginHelper;
		private TranslationMemoriesHelper _translationMemoriesHelper;
		private WorkspacePage _workspacePage;

		private string _tmForFilteringName_1 = "TmForFiltering_First";
		private string _tmForFilteringName_2 = "TmForFiltering_Second";
		private const string _topicName_1 = "Life";
		private const string _topicName_2 = "Science and technology";
		private const string _commonTopicName = "All topics";
	}
}
