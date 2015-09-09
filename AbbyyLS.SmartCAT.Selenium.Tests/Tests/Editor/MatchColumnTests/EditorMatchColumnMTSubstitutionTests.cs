using System.IO;
using NUnit.Framework;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	class EditorMatchColumnMTSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(
				projectName: projectUniqueName,
				filePath: PathProvider.TxtFileForMatchTest,
				createNewTm: true,
				tmxFilePath: PathProvider.TmxFileForMatchTest,
				useMachineTranslation: true,
				createGlossary: true)
				.CheckProjectAppearInList(projectUniqueName)
				.AssertIsProjectLoaded(projectUniqueName);

			_projectsHelper
				.OpenProjectInfo(projectUniqueName)
				.OpenDocumentInfoForProject(projectUniqueName)
				.AddMachineTranslationToDocument(projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest), NickName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest))
				.SelectTask()
				.CloseTutorialIfExist();
		}

		[Test]
		public void CheckMatchAfterMtSubstitution()
		{
			_editorHelper
				.PasteTranslationFromCAT(catType: CatType.MT)
				.AssertMatchColumnCatTypeMatch(catType: CatType.MT);
		}

		[Test]
		public void CheckMatchAfterBothSubstitutions()
		{
			_editorHelper
				.PasteTranslationFromCAT(catType: CatType.MT)
				.AssertMatchColumnCatTypeMatch(catType: CatType.MT)
				.AddTextToSegment(string.Empty)
				.PasteTranslationFromCAT(catType: CatType.TM)
				.AssertMatchColumnCatTypeMatch(catType: CatType.TM);

			var catRowNumber = _editorHelper.CATRowNumber(CatType.TM);

			_editorHelper.AssertCATPercentMatchTargetPercent(1, catRowNumber);
		}

		[Test]
		public void CheckMtMatchAfterAdd()
		{
			_editorHelper
				.AddTextToSegment()
				.PasteTranslationFromCAT(CatType.MT)
				.AssertMatchColumnCatTypeMatch(CatType.MT);
		}

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		private readonly ProjectsHelper _projectsHelper = new ProjectsHelper();
		private readonly EditorHelper _editorHelper = new EditorHelper();
	}
}
