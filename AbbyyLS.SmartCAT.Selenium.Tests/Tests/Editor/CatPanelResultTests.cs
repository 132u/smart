using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class CatPanelResultsTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupCatPanelResultsTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_editorHelper = new EditorHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_projectsPage = new ProjectsPage(Driver);
			_documentSettings = new DocumentSettings(Driver);
		}

		[Test]
		[Standalone]
		public void TMTest()
		{
			_createProjectHelper
				.CreateNewProject(
					_projectUniqueName,
					filePath: PathProvider.EditorTxtFile,
					createNewTm: true,
					tmxFilePath: PathProvider.EditorTmxFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile), ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_editorHelper
				.SelectTask()
				.CloseTutorialIfExist()
				.AssertCatPanelExist()
				.AssertCatPanelContainsCatType(CatType.TM)
				.AssertCatPercentMatch(catRowNumber: 1, percent: 94);
		}

		[TestCase(MachineTranslationType.DefaultMT)]
		[TestCase(MachineTranslationType.Google)]
		[TestCase(MachineTranslationType.MicrosoftBing)]
		public void MTTest(MachineTranslationType machineTranslation)
		{
			_createProjectHelper
				.CreateNewProject(
					projectName: _projectUniqueName,
					filePath: PathProvider.DocumentFile,
					createNewTm: true,
					useMachineTranslation: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDocumentSettings(_projectUniqueName, documentNumber: 1)
				.UnselectMachineTranslation()
				.SelectMachineTranslation(machineTranslation);

			_documentSettings.CloseDocumentSettings(_projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(_projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));

			_editorHelper
				.SelectTask()
				.CloseTutorialIfExist()
				.AssertCatPanelExist()
				.AssertCatPanelContainsCatType(CatType.MT)
				.AssertCatTermsMatchSourceTerms(segmentNumber: 1);
		}

		private string _projectUniqueName;
		private EditorHelper _editorHelper;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private DocumentSettings _documentSettings;
		private ProjectSettingsPage _projectSettingsPage;
	}
}
