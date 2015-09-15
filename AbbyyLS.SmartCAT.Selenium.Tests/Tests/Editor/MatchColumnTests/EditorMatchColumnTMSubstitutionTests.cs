using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Standalone]
	public class EditorMatchColumnTMSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(
					projectName: projectUniqueName,
					filePath: PathProvider.TxtFileForMatchTest,
					createNewTm: true,
					tmxFilePath: PathProvider.TmxFileForMatchTest)
				.AssertIsProjectLoadedSuccessfully(projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest), ConfigurationManager.NickName)
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

			var catRowNumber = _editorHelper.CATRowNumber(CatType.TM);

			_editorHelper
				.AssertCATPercentMatchTargetPercent(catRowNumber: catRowNumber)
				.AssertTargetMatchPercenrCollorCorrect();
		}

		[Test]
		public void CheckMatchAfterEditCell()
		{
			_editorHelper
				.PasteTranslationFromCAT(catType: CatType.TM)
				.AssertMatchColumnCatTypeMatch(catType: CatType.TM)
				.AddTextWithoutClearing()
				.AssertMatchColumnCatTypeMatch(catType: CatType.TM);

			var catRowNumber = _editorHelper.CATRowNumber(CatType.TM);

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

			var catRowNumber = _editorHelper.CATRowNumber(CatType.TM);

			_editorHelper.AssertCATPercentMatchTargetPercent(1, catRowNumber);
		}

		[Test]
		public void CheckTmMatchAfterAdd()
		{
			_editorHelper
				.AddTextToSegment()
				.PasteTranslationFromCAT(CatType.TM)
				.AssertMatchColumnCatTypeMatch(CatType.TM);

			var catRowNumber = _editorHelper.CATRowNumber(CatType.TM);

			_editorHelper.AssertCATPercentMatchTargetPercent(1, catRowNumber);
		}

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		private readonly ProjectsHelper _projectsHelper = new ProjectsHelper();
		private readonly EditorHelper _editorHelper = new EditorHelper();
	}
}
