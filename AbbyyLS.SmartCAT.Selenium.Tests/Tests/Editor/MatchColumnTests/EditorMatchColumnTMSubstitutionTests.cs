using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorMatchColumnTMSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_editorHelper = new EditorHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(
					projectName: projectUniqueName,
					filePath: PathProvider.TxtFileForMatchTest,
					createNewTm: true,
					tmxFilePath: PathProvider.TmxFileForMatchTest)
				.GoToTranslationMemoriesPage()
				.CloseAllNotifications()
				.GoToProjectsPage();
			_workspaceHelper
				.GoToProjectSettingsPage(projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest), ThreadUser.NickName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest))
				.SelectTask()
				.CloseTutorialIfExist();
		}

		[Test]
		public void CheckMatchAfterTmSubstitutionSegmentNumber()
		{
			_editorHelper
				.PasteTranslationFromCAT(catType: CatType.TM)
				.AssertMatchColumnCatTypeMatch(catType: CatType.TM);

			var catRowNumber = _editorHelper.CatRowNumber(CatType.TM);

			_editorHelper
				.AssertCATPercentMatchTargetPercent(catRowNumber: catRowNumber)
				.AssertTargetMatchPercentCollorCorrect();
		}

		[Test]
		public void CheckMatchAfterEditCell()
		{
			_editorHelper
				.PasteTranslationFromCAT(catType: CatType.TM)
				.AssertMatchColumnCatTypeMatch(catType: CatType.TM)
				.AddTextWithoutClearing()
				.AssertMatchColumnCatTypeMatch(catType: CatType.TM);

			var catRowNumber = _editorHelper.CatRowNumber(CatType.TM);

			_editorHelper.AssertCATPercentMatchTargetPercent(1, catRowNumber);
		}

		[Test]
		public void CheckMatchAfterDelete()
		{
			_editorHelper
				.PasteTranslationFromCAT(catType: CatType.TM)
				.AssertMatchColumnCatTypeMatch(catType: CatType.TM)
				.AddTextToSegment()
				.AssertMatchColumnCatTypeMatch(catType: CatType.TM);

			var catRowNumber = _editorHelper.CatRowNumber(CatType.TM);

			_editorHelper.AssertCATPercentMatchTargetPercent(1, catRowNumber);
		}

		[Test]
		public void CheckTmMatchAfterAdd()
		{
			_editorHelper
				.AddTextToSegment()
				.PasteTranslationFromCAT(CatType.TM)
				.AssertMatchColumnCatTypeMatch(CatType.TM);

			var catRowNumber = _editorHelper.CatRowNumber(CatType.TM);

			_editorHelper.AssertCATPercentMatchTargetPercent(1, catRowNumber);
		}

		private CreateProjectHelper _createProjectHelper;
		private EditorHelper _editorHelper;
		private WorkspaceHelper _workspaceHelper;
	}
}
