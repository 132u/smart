using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Editor]
	public class EditorMatchColumnTBSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_addTermDialog = new AddTermDialog(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var _glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			var txtFileForMatchTest = PathProvider.TxtFileForMatchTest;

			_createProjectHelper.CreateNewProject(
				projectName: projectUniqueName,
				filesPaths: new[] { txtFileForMatchTest },
				glossaryName: _glossaryUniqueName);

			_projectsPage.OpenProjectSettingsPage(projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(txtFileForMatchTest, ThreadUser.NickName, projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(txtFileForMatchTest);

			_selectTaskDialog.SelectTask();

			var sourceTerm = _editorPage
					.ClickOnTargetCellInSegment(1)
					.GetSourceText(1);

			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(sourceTerm, "термин глоссария");
		}

		[Ignore("PRX-17035")]
		[Test, Description("S-7220"), ShortCheckList]
		public void CheckMatchAfterGlossarySubstitutionByDoubleClick()
		{
			_editorPage.PasteTranslationFromCAT(catType: CatType.TB);

			Assert.IsTrue(_editorPage.IsMatchColumnCatTypeMatch(catType: CatType.TB),
				"Произошла ошибка:\n тип подстановки в колонке Match Column не совпал с типом перевода {0}.", CatType.TB);
		}

		[Ignore("PRX-17035")]
		[Test, Description("S-29713"), ShortCheckList]
		public void CheckMatchAfterGlossarySubstitutionByHotkey()
		{
			_editorPage.PasteTranslationFromCATByHotkey(catType: CatType.TB);

			Assert.IsTrue(_editorPage.IsMatchColumnCatTypeMatch(catType: CatType.TB),
				"Произошла ошибка:\n тип подстановки в колонке Match Column не совпал с типом перевода {0}.", CatType.TB);
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectsPage _projectsPage;
		private ProjectSettingsPage _projectSettingsPage;
		private AddTermDialog _addTermDialog;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
	}
}
