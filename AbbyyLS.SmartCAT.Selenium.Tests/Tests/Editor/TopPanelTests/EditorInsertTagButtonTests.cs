using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Editor]
	class EditorInsertTagButtonTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(
				projectUniqueName, filesPaths: new[] { PathProvider.DocumentFile });

			_projectsPage.OpenProjectSettingsPage(projectUniqueName);

			_projectSettingsHelper
				.AssignTasksOnDocument(PathProvider.DocumentFile, ThreadUser.NickName, projectUniqueName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(PathProvider.DocumentFile);

			_selectTaskDialog.SelectTask();
		}

		[Test(Description = "Проверяет вставку тега с помощью кнопки")]
		public void TagButtonTest()
		{
			_editorPage.ClickInsertTag();

			Assert.IsTrue(_editorPage.IsTagDisplayed(),
				"Произошла ошибка:\n тег не появился в таргете");
		}

		[Test(Description = "Проверяет вставку тега нажатием F8")]
		public void TagHotkeyTest()
		{
			_editorPage
				.FillSegmentTargetField()
				.InsertTagByHotKey();

			Assert.IsTrue(_editorPage.IsTagDisplayed(),
				"Произошла ошибка:\n тег не появился в таргете");
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectsPage _projectsPage;
		private ProjectSettingsPage _projectSettingsPage;
		private EditorPage _editorPage;
		private SelectTaskDialog _selectTaskDialog;
	}
}
