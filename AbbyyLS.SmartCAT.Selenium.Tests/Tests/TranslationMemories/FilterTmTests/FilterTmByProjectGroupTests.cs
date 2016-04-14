using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	[Parallelizable(ParallelScope.Fixtures)]
	class FilterTmByProjectGroupTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			_loginHelper = new LoginHelper(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_workspacePage = new WorkspacePage(Driver);

			_projectGroupName_1 = _projectGroupsPage.GetProjectGroupUniqueName();
			_projectGroupName_2 = _projectGroupsPage.GetProjectGroupUniqueName();
			_tmForFilteringName_1 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_tmForFilteringName_2 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToProjectGroupsPage();

			_projectGroupsPage
				.CreateProjectGroup(_projectGroupName_1)
				.CreateProjectGroup(_projectGroupName_2);

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesHelper
				.CreateTranslationMemory(
					translationMemoryName: _tmForFilteringName_1,
					sourceLanguage: Language.French,
					targetLanguage: Language.German,
					projectGroup: _projectGroupName_1)
				.CreateTranslationMemory(
					translationMemoryName: _tmForFilteringName_2,
					sourceLanguage: Language.German,
					targetLanguage: Language.English,
					projectGroup: _projectGroupName_2);
		}

		[SetUp]
		public void Setup()
		{
			TranslationMemoriesPage.ClearFiltersPanelIfExist();
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

		private LoginHelper _loginHelper;
		private TranslationMemoriesHelper _translationMemoriesHelper;
		private ProjectGroupsPage _projectGroupsPage;
		private WorkspacePage _workspacePage;

		private string _tmForFilteringName_1 = "TmForFiltering_First";
		private string _tmForFilteringName_2 = "TmForFiltering_Second";
		private string _projectGroupName_1;
		private string _projectGroupName_2;
	}
}
