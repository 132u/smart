using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	[Parallelizable(ParallelScope.Fixtures)]
	[TranslationMemories]
	class FilterTmByLanguageTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
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
					targetLanguage: Language.German)
				.CreateTranslationMemory(
					translationMemoryName: _tmForFilteringName_2,
					sourceLanguage: Language.German,
					targetLanguage: Language.English);
		}

		[SetUp]
		public void Setup()
		{
			TranslationMemoriesPage.ClearFiltersPanelIfExist();
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
				.CreateNewTMFilter(setSourceLanguageFilter: Language.French)
				.CreateNewTMFilter(setSourceLanguageFilter: Language.German, clearFilters: false);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationOneTargetLanguage()
		{
			TranslationMemoriesHelper.CreateNewTMFilter(setTargetLanguageFilter: Language.German);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_1),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _tmForFilteringName_1);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(_tmForFilteringName_2),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", _tmForFilteringName_2);
		}

		[Test]
		public void TmFiltrationTwoTargetLanguage()
		{
			TranslationMemoriesHelper
				.CreateNewTMFilter(setTargetLanguageFilter: Language.German)
				.CreateNewTMFilter(setTargetLanguageFilter: Language.English, clearFilters: false);

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
	}
}
