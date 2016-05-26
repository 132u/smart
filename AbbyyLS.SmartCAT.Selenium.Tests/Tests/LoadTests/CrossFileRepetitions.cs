using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.LoadTests
{
	[LoadTests]
	class CrossFileRepetitions<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_editorPage = new EditorPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
		}

		[Test]
		public void OpenDocumentAndConfirmSegmentTest()
		{
			var projectName = _createProjectHelper.GetProjectUniqueName();
			var document1 = PathProvider.EditorCrossFileRepetitionsFirstFile;
			var document2 = PathProvider.EditorCrossFileRepetitionsSecondFile;

			_createProjectHelper.CreateNewProject(
				projectName: projectName,
				filesPaths: new[] { document1, document2 },
				createNewTm: true);

			_projectsPage
				.OpenProjectInfo(projectName)
				.ClickDocumentRefExpectingEditorPage(projectName, document1);

			for (int segmentNumber = 1; segmentNumber < 241; segmentNumber++)
			{
				_editorPage
					.ClickOnTargetCellInSegment(segmentNumber)
					.CopySourceToTargetHotkey()
					.ConfirmSegmentTranslation();

				Assert.IsTrue(_editorPage.IsSegmentConfirmed(segmentNumber),
					"Произошла ошибка: сегмент {0} не подтвердился.", segmentNumber);
			}

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(document2);

			for (int segmentNumber = 1; segmentNumber < 241; segmentNumber++)
			{
				_editorPage.ClickOnTargetCellInSegment(segmentNumber);

				Assert.IsTrue(_editorPage.IsSegmentConfirmed(segmentNumber),
					"Произошла ошибка: сегмент {0} не подтвердился.", segmentNumber);
			}
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private EditorPage _editorPage;
		private ProjectSettingsPage _projectSettingsPage;
	}
}
