using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Glossaries]
	class GlossariesEditorTests<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_addTermDialog = new AddTermDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);

			_projectName = _createProjectHelper.GetProjectUniqueName();
			_glossary1Name = GlossariesHelper.UniqueGlossaryName();
			_glossary2Name = GlossariesHelper.UniqueGlossaryName();
			_glossary3Name = GlossariesHelper.UniqueGlossaryName();
			var document = PathProvider.DocumentFile;

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossary1Name);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossary2Name);

			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(_projectName, glossaryName: _glossary3Name);

			_projectsPage.OpenProjectSettingsPage(_projectName);

			_projectSettingsHelper
				.UploadDocument(new[] {document})
				.AssignTasksOnDocument(document, ThreadUser.NickName, _projectName);

			_projectSettingsPage
				.SelectGlossaryByName(_glossary1Name)
				.SelectGlossaryByName(_glossary2Name);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(document);

			_selectTaskDialog.SelectTask();
		}

		[Test(Description = "Проверяет выпадающий список с глоссариями, при создании проекта подключалось 2 глоссария")]
		public void CheckGlossaryListInProjectWithTwoGlossaries()
		{
			_editorPage.ClickAddTermButton();

			_addTermDialog.OpenGlossarySelect();

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary1Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary2Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");
		}

		[Test(Description = "Проверяет добавление одинаковых терминов в разные глоссарии")]
		public void AddEqualTermsInTwoGlossaries()
		{
			var source = "Space";
			var target = "Космос";

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target, glossaryName: _glossary1Name);

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			Assert.IsTrue(_editorPage.IsTermSavedMessageDisappeared(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не исчезло");

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(source, target, glossaryName: _glossary2Name);

			Assert.IsTrue(_editorPage.IsTermSaved(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			Assert.IsTrue(_editorPage.IsTermSavedMessageDisappeared(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не исчезло");
		}

		[Test(Description = "Проверяет выпадающий список с глоссариями (проект с 2 глоссариями, второй глоссарий подключается в настройках проекта")]
		public void CheckGlossaryListInProjectCreatedWithOneGlossary()
		{
			_editorPage.ClickAddTermButton();

			_addTermDialog.OpenGlossarySelect();

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary1Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary2Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");

			Assert.IsTrue(_addTermDialog.IsGlossaryExistInDropdown(_glossary3Name),
				"Произошла ошибка:\n глоссарий не найден в выпадающем списке");
		}

		private string _projectName;
		private string _glossary1Name;
		private string _glossary2Name;
		private string _glossary3Name;

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private GlossariesHelper _glossariesHelper;
		private ProjectsPage _projectsPage;
		private ProjectSettingsPage _projectSettingsPage;
		private WorkspacePage _workspacePage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
		private AddTermDialog _addTermDialog;
	}
}
