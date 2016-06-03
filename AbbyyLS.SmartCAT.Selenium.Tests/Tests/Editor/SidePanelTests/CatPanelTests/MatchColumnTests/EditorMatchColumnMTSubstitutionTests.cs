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
	[Editor]
	class EditorMatchColumnMTSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_editorPage = new EditorPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var txtFileForMatchTest = PathProvider.TxtFileForMatchTest;
			var tmxFileForMatchTest = PathProvider.TmxFileForMatchTest;

			_createProjectHelper.CreateNewProject(
				projectName: projectUniqueName,
				filesPaths: new[] { txtFileForMatchTest },
				createNewTm: true,
				tmxFilesPaths: new[] { tmxFileForMatchTest },
				useMachineTranslation: true);

			_projectsPage.OpenProjectSettingsPage(projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(txtFileForMatchTest, ThreadUser.NickName, projectUniqueName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(txtFileForMatchTest);

			_selectTaskDialog.SelectTask();
		}

		[Test]
		public void CheckMatchAfterMtSubstitution()
		{
			_editorPage.PasteTranslationFromCAT(catType: CatType.MT);

			Assert.IsTrue(_editorPage.IsMatchColumnCatTypeMatch(catType: CatType.MT),
				"Произошла ошибка:\n тип подстановки в колонке Match Column не совпал с типом перевода {0}.", CatType.MT);
		}

		[Test]
		public void CheckMatchAfterBothSubstitutions()
		{
			_editorPage
				.PasteTranslationFromCAT(catType: CatType.MT)
				.FillSegmentTargetField(string.Empty)
				.PasteTranslationFromCAT(catType: CatType.TM);

			Assert.IsTrue(_editorPage.IsMatchColumnCatTypeMatch(catType: CatType.TM),
				"Произошла ошибка:\n тип подстановки в колонке Match Column не совпал с типом перевода {0}.", CatType.TM);

			var catRowNumber = _editorPage.CatTypeRowNumber(CatType.TM);

            Assert.IsTrue(_editorPage.IsCATPercentMatchTargetPercent(1, catRowNumber),
				"Произошла ошибка:\n Процент совпадения в CAT-панели и в таргете не совпадает.");
		}

		[Test]
		public void CheckMtMatchAfterAdd()
		{
			_editorPage
				.FillSegmentTargetField()
				.PasteTranslationFromCAT(CatType.MT);

			Assert.IsTrue(_editorPage.IsMatchColumnCatTypeMatch(catType: CatType.MT),
				"Произошла ошибка:\n тип подстановки в колонке Match Column не совпал с типом перевода {0}.", CatType.MT);
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectsPage _projectsPage;
		private ProjectSettingsPage _projectSettingsPage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
	}
}
