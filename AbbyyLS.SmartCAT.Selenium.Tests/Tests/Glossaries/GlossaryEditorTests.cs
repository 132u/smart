﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class GlossaryEditorTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_addTermDialog = new AddTermDialog(Driver);
			_usersRightsPage = new UsersRightsPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_confirmTermWithoutTranskationDialog = new ConfirmTermWithoutTranskationDialog(Driver);

			_projectName = _createProjectHelper.GetProjectUniqueName();
			_glossaryName = GlossariesHelper.UniqueGlossaryName();

			_workspaceHelper.GoToUsersRightsPage();

			_usersRightsPage
				.ClickGroupsButton()
				.AddUserToGroupIfNotAlredyAdded("Administrators", ThreadUser.NickName);

			_workspaceHelper.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectName, glossaryName: _glossaryName)
				.GoToProjectSettingsPage(_projectName)
				.UploadDocument(new []{PathProvider.DocumentFile})
				.RefreshPage<ProjectSettingsPage, ProjectSettingsHelper>()
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();
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

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, word);
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

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, "", word);
		}

		[Test(Description = "Проверяет добавление термина с сорсом и таргетом в глоссарий")]
		public void AddSourceTargetTermToGlossary()
		{
			var source = "Comet";
			var target = "Комета";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target);
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

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target);
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

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target);
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

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, modifiedTarget, termsCount: 2);
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

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, modifiedSource, target, termsCount: 2);
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

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target, termsCount: 2);
		}

		[Test(Description = "Проверяет добавление в глоссарий термина с комментарием")]
		public void AddTermWithCommentToGlossary()
		{
			var source = "Neptun";
			var target = "Нептун";
			var comment = "Generated By Selenium";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target, comment);

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target, comment);
		}

		[Test(Description = "Проверяет добавление в глоссарий ранее удаленного термина")]
		public void DeleteAddTermToGlossary()
		{
			var source = "Galaxy";
			var target = "Галактика";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target)
				.DeleteTerm(source)
				.GoToProjectsPage();
			_workspaceHelper
				.GoToProjectSettingsPage(_projectName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));

			_selectTaskDialog.SelectTask();

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target);

			_editorPage.ClickHomeButton();

			_workspaceHelper
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target);
		}

		private CreateProjectHelper _createProjectHelper;
		private WorkspaceHelper _workspaceHelper;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
		private AddTermDialog _addTermDialog;
		private ConfirmTermWithoutTranskationDialog _confirmTermWithoutTranskationDialog;

		private UsersRightsPage _usersRightsPage;
		private ProjectSettingsPage _projectSettingsPage;

		private string _projectName;
		private string _glossaryName;
	}
}
