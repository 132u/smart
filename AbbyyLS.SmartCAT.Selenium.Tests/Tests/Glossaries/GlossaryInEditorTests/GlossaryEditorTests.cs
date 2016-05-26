using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Glossaries]
	class GlossaryEditorTests<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_addTermDialog = new AddTermDialog(Driver);
			_usersTab = new UsersTab(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_confirmTermWithoutTranskationDialog = new ConfirmTermWithoutTranskationDialog(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectName = _createProjectHelper.GetProjectUniqueName();
			_glossaryName = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(_projectName, glossaryName: _glossaryName);

			_projectsPage.OpenProjectSettingsPage(_projectName);

			_projectSettingsHelper
				.UploadDocument(new[] { PathProvider.EditorTxtFile })
				.AssignTasksOnDocument(PathProvider.EditorTxtFile, ThreadUser.NickName, _projectName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();
		}

		[Test(Description = "Проверят открытие формы добавления теримна с помощью кнопки")]
		public void OpenAddTermFormByButton()
		{
			_editorPage.ClickAddTermButton();

			Assert.IsTrue(_addTermDialog.IsAddTermDialogOpened(),
				"Произошла ошибка: \nне открылся диалог добавления термина");
		}

		[Test(Description = "Проверят открытие формы добавления теримна нажатием Ctrl+E")]
		public void OpenAddTermFormByHotKey()
		{
			_editorPage.OpenAddTermDialogByHotKey();

			Assert.IsTrue(_addTermDialog.IsAddTermDialogOpened(),
				"Произошла ошибка: \nне открылся диалог добаления нового термина");
		}

		[Test(Description = "Проверяет автозаполнения формы при выделенном слове в сорсе")]
		public void AutofillAddTermFormWithSelectedSourceWord()
		{
			var word = _editorPage
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.GetSelectedWordInSegment();

			_editorPage
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.ClickAddTermButton();

			Assert.IsTrue(_addTermDialog.IsTextExistInSourceTerm(word),
				"Произошла ошибка:\n нет автозаполнения сорса");
		}

		[Test(Description = "Проверяет автозаполнения формы при выделенном слове в таргете")]
		public void AutofillAddTermFormWithSelectedTargetWord()
		{
			var word = _editorPage
				.FillSegmentTargetField("Town")
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.GetSelectedWordInSegment();

			_editorPage
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.ClickAddTermButton();

			Assert.IsTrue(_addTermDialog.IsTextExistInTargetTerm(word),
				"Произошла ошибка:\n нет автозаполнения таргета");
		}

		[Test(Description = "Проверяет появление сообщения об успешном сохранении")]
		public void AppearenceOfTermSavedMessageTest()
		{
			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm("Word", "Слово", "Комментарий");

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			Assert.IsTrue(_editorPage.IsTermSavedMessageDisappeared(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не исчезло");
		}

		[Test(Description = "Проверяет добавление одиночного термина из сорса в глоссарий")]
		public void AddSingleTermFromSourceToGlossary()
		{
			var word = _editorPage
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.GetSelectedWordInSegment();

			_editorPage
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.ClickAddTermButton();

			_addTermDialog.ClickAddButtonExpectingConfirmation();

			_confirmTermWithoutTranskationDialog.ConfirmSaving();

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsSingleTermExists(word),
				"Произошла ошибка:\n термин не обнаружен.");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test(Description = "Проверяет добавление одиночного термина из таргета в глоссарий")]
		public void AddSingleTermFromTargetToGlossary()
		{
			var word = _editorPage
				.FillSegmentTargetField("Town")
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.GetSelectedWordInSegment();

			_editorPage
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.ClickAddTermButton();

			_addTermDialog.ClickAddButtonExpectingConfirmation();

			_confirmTermWithoutTranskationDialog.ConfirmSaving();

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(string.Empty, word),
				"Произошла ошибка:\n термин не обнаружен");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test(Description = "S-7218"), ShortCheckList]
		public void AddSourceTargetTermToGlossary()
		{
			var highlightedSegmentTermsExpected = new List<string> { "sentence" };
			var target = "изречение";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(highlightedSegmentTermsExpected[0], target);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(highlightedSegmentTermsExpected[0], target),
				"Произошла ошибка:\n термин не обнаружен");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectName, PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			var highlightedSegmentTerms = _editorPage.GetHighlightedWords(segmentNumber: 1);

			Assert.AreEqual(highlightedSegmentTerms, highlightedSegmentTermsExpected,
				"Произошла ошибка:\n Подсвечен не тот термин.");

			Assert.IsTrue(_editorPage.IsWordsMatchCatWords(highlightedSegmentTerms),
				"Произошла ошибка:\nПодсвеченные слова в сегменте не соответствуют терминам САТ.");

			highlightedSegmentTerms = _editorPage.GetHighlightedWords(segmentNumber: 3);

			Assert.AreEqual(highlightedSegmentTerms, highlightedSegmentTermsExpected,
				"Произошла ошибка:\n Подсвечен не тот термин.");

			Assert.IsTrue(_editorPage.IsWordsMatchCatWords(highlightedSegmentTerms),
				"Произошла ошибка:\nПодсвеченные слова в сегменте не соответствуют терминам САТ.");
		}

		[Test(Description = "Проверяет добавление термина из сорса с таргетом в глоссарий игнорируя автоподстановку")]
		public void AddModifiedSourceTargetTermToGlossary()
		{
			var source = "Comet";
			var target = "Комета";

			_editorPage
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(source, target),
				"Произошла ошибка:\n термин не обнаружен");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}


		[Test(Description = "Проверяет добавление измененного термина из таргета в глоссарий")]
		public void AddSourceModifiedTargetTermToGlossary()
		{
			var source = "Comet";
			var oldTarget = "Town";
			var target = "Комета";

			_editorPage
				.FillSegmentTargetField(oldTarget)
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(source, target),
				"Произошла ошибка:\n термин не обнаружен");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test(Description = "Проверяет добавление в глоссарий двух терминов с одинаковым сорсом")]
		public void AddExistedSourceTermToGlossary()
		{
			var source = "planet";
			var target = "планета";
			var modifiedTarget = "планетка";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, modifiedTarget);

			Assert.IsTrue(_editorPage.IsConfirmExistedTermMessageDisplayed(),
				"Произошла ошибка:\n не появился диалог подтверждения сохранения уже существующего термина");

			_editorPage.Confirm();

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(source, modifiedTarget),
				"Произошла ошибка:\n термин не обнаружен");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 2),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test(Description = "Проверить добавление в глоссарий двух терминов с одинаковым таргетом")]
		public void AddExistedTargetTermToGlossary()
		{
			var source = "asteroid";
			var modifiedSource = "the asteroid";
			var target = "астероид";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(modifiedSource, target);

			Assert.IsTrue(_editorPage.IsConfirmExistedTermMessageDisplayed(),
				"Произошла ошибка:\n не появился диалог подтверждения сохранения уже существующего термина");

			_editorPage.Confirm();

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(modifiedSource, target),
				"Произошла ошибка:\n термин не обнаружен");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 2),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test(Description = "Проверяет добавление в глоссарий двух абсолютно идентичных терминов")]
		public void AddExistedTermToGlossary()
		{
			var source = "sun";
			var target = "солнце";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			Assert.IsTrue(_editorPage.IsConfirmExistedTermMessageDisplayed(),
				"Произошла ошибка:\n не появился диалог подтверждения сохранения уже существующего термина");

			_editorPage.Confirm();

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(source, target),
				"Произошла ошибка:\n термин не обнаружен");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 2),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test(Description = "Проверяет добавление в глоссарий термина с комментарием")]
		public void AddTermWithCommentToGlossary()
		{
			var source = "Neptun";
			var target = "Нептун";
			var comment = "Generated By Selenium";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target, comment);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsTermWithTranslationAndCommentExists(source, target, comment),
				"Произошла ошибка:\n термин не обнаружен");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test(Description = "Проверяет добавление в глоссарий ранее удаленного термина")]
		public void DeleteAddTermToGlossary()
		{
			var source = "Galaxy";
			var target = "Галактика";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			_glossaryPage.DeleteTerm(source);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(source, target),
				"Произошла ошибка:\n термин не обнаружен");

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private WorkspacePage _workspacePage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
		private AddTermDialog _addTermDialog;
		private ConfirmTermWithoutTranskationDialog _confirmTermWithoutTranskationDialog;

		private UsersTab _usersTab;
		private ProjectsPage _projectsPage;
		private ProjectSettingsPage _projectSettingsPage;
		private GlossaryPage _glossaryPage;
		private GlossariesPage _glossariesPage;

		private string _projectName;
		private string _glossaryName;
	}
}
