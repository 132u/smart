using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.TopPanelTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Editor]

	class EditorResourceConcordanceSearchTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_signInPage = new SignInPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_workflowSetUptab = new WorkflowSetUpTab(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(
					projectName: _projectUniqueName,
					filesPaths: new[] { PathProvider.TxtFileForMatchTest },
					createNewTm: true,
					tmxFilesPaths: new[] { PathProvider.TmxFileForMatchTest });

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(PathProvider.TxtFileForMatchTest, ThreadUser.NickName, _projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest));

			_selectTaskDialog.SelectTask();
		}

		[Test, Description("S-7239")]
		public void SourceConcordanceSearchTest()
		{
			var sourceText = _editorPage.GetSourceText(rowNumber: 1);
			var searchText = "searchText";
			var catSourceText = _editorPage.GetSourceCatTranslationText(rowNumber: 1).Replace(".", "");
			var catTargetText = _editorPage.GetTargetCatTranslationText(rowNumber: 1);

			_editorPage
				.SelectAllTextByHotkeyInSource(segmentNumber: 1)
				.ClickConcordanceSearchButton();

			Assert.IsTrue(_editorPage.IsSourceRadioButtonChecked(),
				"Произошла ошибка: радиокнопка Source не отменчена в конкордансном поиске.");

			Assert.AreEqual(sourceText, _editorPage.GetTextFromConcordanceSearchField(),
				"Произошла ошибка: неверный текст  в конкордансном поиске.");

			_editorPage
				.SetConcordanceSearch(searchText)
				.ClickSearchButtonInConcordanceTable();

			Assert.IsFalse(_editorPage.IsConcordanceSearchResultTableDisplayed(),
				"Произошла ошибка: отображается таблица с результатами конкордансного поиска");
			
			_editorPage
				.SetConcordanceSearch(catSourceText)
				.ClickSearchButtonInConcordanceTable();

			Assert.IsTrue(_editorPage.IsConcordanceSearchResultTableDisplayed(),
				"Произошла ошибка: не отображается таблица с результатами конкордансного поиска");

			Assert.AreEqual(catSourceText, _editorPage.GetSourceConcordanceResultBySourceSearch(),
				"Произошла ошибка: неверный сорс в результирующей таблице конкордансного поиска.");

			Assert.AreEqual(catTargetText, _editorPage.GetTargetConcordanceResultBySourceSearch(),
				"Произошла ошибка: неверный таргет в результирующей таблице конкордансного поиска.");
		}

		[Test, Description("S-7240")]
		public void TargetConcordanceSearchTest()
		{
			var targetText = _editorPage.GetTargetText(rowNumber: 2);
			var searchText = "searchText";
			var catSourceText = _editorPage.GetSourceCatTranslationText(rowNumber: 2).Replace(".", "");
			var catTargetText = _editorPage.GetTargetCatTranslationText(rowNumber: 2);

			_editorPage
				.SelectAllTextByHotkey(segmentNumber: 2)
				.ClickConcordanceSearchButton();

			Assert.IsTrue(_editorPage.IsTargetRadioButtonChecked(),
				"Произошла ошибка: радиокнопка Source не отменчена в конкордансном поиске.");

			Assert.AreEqual(targetText, _editorPage.GetTextFromConcordanceSearchField(),
				"Произошла ошибка: неверный текст  в конкордансном поиске.");

			_editorPage
				.SetConcordanceSearch(searchText)
				.ClickSearchButtonInConcordanceTable();

			Assert.IsFalse(_editorPage.IsConcordanceSearchResultTableDisplayed(),
				"Произошла ошибка: отображается таблица с результатами конкордансного поиска");

			_editorPage
				.SetConcordanceSearch(catTargetText)
				.ClickSearchButtonInConcordanceTable();

			Assert.IsTrue(_editorPage.IsConcordanceSearchResultTableDisplayed(),
				"Произошла ошибка: не отображается таблица с результатами конкордансного поиска");

			Assert.AreEqual(catSourceText, _editorPage.GetSourceConcordanceResultByTargetSearch(),
				"Произошла ошибка: неверный сорс в результирующей таблице конкордансного поиска.");

			Assert.AreEqual(catTargetText, _editorPage.GetTargetConcordanceResultByTargetSearch(),
				"Произошла ошибка: неверный таргет в результирующей таблице конкордансного поиска.");
		}

		[Test, Description("S-7241")]
		public void TargetConcordanceSearchSubstitutionTest()
		{
			_editorPage
				.SelectAllTextByHotkeyInSource(segmentNumber: 1)
				.ClickConcordanceSearchButton()
				.FillTarget("")
				.ClickOnTargetCellInSegment()
				.DoubleClickConcordanceSearchResult();

			Assert.AreEqual(_editorPage.GetTargetText(rowNumber: 1),
				_editorPage.GetTargetConcordanceResultBySourceSearch(),
				"Произошла ошибка: неверный текст в таргете.");
		}

		protected CreateProjectHelper _createProjectHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected ProjectSettingsPage _projectSettingsPage;
		protected ProjectSettingsDialog _settingsDialog;
		protected EditorPage _editorPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected SignInPage _signInPage;
		protected ProjectsPage _projectsPage;
		protected string _projectUniqueName;
		protected WorkspacePage _workspacePage;
		protected WorkflowSetUpTab _workflowSetUptab;
	}
}
