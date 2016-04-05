using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.LoadTests
{
	[LoadTests]
	[Parallelizable(ParallelScope.Fixtures)]
	class OpenDocumentAndConfirmSegment<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_editorPage = new EditorPage(Driver);
		}

		[Test]
		public void OpenDocumentAndConfirmSegmentTest()
		{
			var projectName = _createProjectHelper.GetProjectUniqueName();
			var document = PathProvider.EditorTagsRepetitionsNumbersFile;

			_createProjectHelper.CreateNewProject(
				projectName: projectName,
				filesPaths: new[] {document},
				createNewTm: true);

			_projectsPage
				.OpenProjectInfo(projectName)
				.ClickDocumentRefExpectingEditorPage(document);

			for (int segmentNumber = 1; segmentNumber < 241; segmentNumber++)
			{
				_editorPage
				.ClickOnTargetCellInSegment(segmentNumber)
				.CopySourceToTargetHotkey()
				.ConfirmSegmentTranslation();

				Assert.IsTrue(_editorPage.IsSegmentConfirmed(segmentNumber),
					"Произошла ошибка: сегмент {0} не подтвердился.", segmentNumber);
			}
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private EditorPage _editorPage;
	}
}
