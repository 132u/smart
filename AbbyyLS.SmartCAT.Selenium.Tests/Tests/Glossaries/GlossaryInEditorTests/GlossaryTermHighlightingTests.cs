using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Glossaries]
	class GlossaryTermHighlightingTests<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossaryTermHighlightingTestsSetUp()
		{
			_glossaryHelper = new GlossariesHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);

			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_document = PathProvider.EditorTxtFile;

			_createProjectHelper.CreateNewProject(
					_projectUniqueName,
					filesPaths: new[] { _document },
					createNewTm: true);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(_document, ThreadUser.NickName, _projectUniqueName);

			_workspacePage.GoToGlossariesPage();

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);
		}

		[Test, Description("S-7217"), ShortCheckList]
		[TestCase(1)]
		[TestCase(3)]
		public void HighlightedWordsFromGlossarySegmentTest(int segmentNumber)
		{
			var dictionary = new Dictionary<string, string>
			{ 
				{"more", "еще"},
				{"first", "первый"},
				{"test", "тест"},
			};

			_glossaryPage.CreateTerms(dictionary);
			
			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.SelectGlossaryByName(_glossaryUniqueName)
				.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();

			var highlightedSegmentTerms = _editorPage.GetHighlightedWords(segmentNumber: segmentNumber);

			Assert.IsTrue(_editorPage.IsCatTableExist(), "Произошла ошибка:\nCAT-панель пустая.");

			Assert.IsTrue(_editorPage.IsCatTypeExist(CatType.TB),
				"Произошла ошибка:\nВ CAT-панели отсутствует подстановка {0}.", CatType.TB);

			Assert.IsTrue(_editorPage.IsWordsMatchCatWords(highlightedSegmentTerms),
				"Произошла ошибка:\nПодсвеченные слова в сегменте не соответствуют терминам САТ.");
		}

		[Test, Description("S-7219"), ShortCheckList]
		public void HighlightedWordsFromGlossaryInFewSegmentTest()
		{
			var dictionary = new Dictionary<string, string>
			{ 
				{"second", "второй"},
				{"one", "один"},
				{"more", "еще"},
				{"test", "тест"}
			};

			_glossaryPage.CreateTerms(dictionary);

			_workspacePage.GoToProjectsPage();
			
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.SelectGlossaryByName(_glossaryUniqueName)
				.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();

			var highlightedTermsSecondSegments = _editorPage.GetHighlightedWords(segmentNumber: 2);
			var highlightedTermsThirdSegments = _editorPage.GetHighlightedWords(segmentNumber: 3);
			var highlightedTermsFourthSegments = _editorPage.GetHighlightedWords(segmentNumber: 4);

			Assert.AreEqual(1, highlightedTermsSecondSegments.Count,
				"Произошла ошибка:\nколичество совпадений второго сегмента не корректно.");
			
			Assert.AreEqual(3, highlightedTermsThirdSegments.Count,
				"Произошла ошибка:\nколичество совпадений третьего сегмента не корректно.");

			Assert.AreEqual(0, highlightedTermsFourthSegments.Count,
				"Произошла ошибка:\nколичество совпадений четвертого сегмента не корректно.");
		}

		private GlossaryPage _glossaryPage;
		protected string _projectUniqueName;
		protected ProjectsPage _projectsPage;
		private GlossariesHelper _glossaryHelper;
		private WorkspacePage _workspacePage;
		private string _glossaryUniqueName;
		private TestUser _secondUser;
		private ProjectSettingsPage _projectSettingsPage;
		private CreateProjectHelper _createProjectHelper;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
		private ProjectSettingsHelper _projectSettingsHelper;
		private string _document;
	}
}
