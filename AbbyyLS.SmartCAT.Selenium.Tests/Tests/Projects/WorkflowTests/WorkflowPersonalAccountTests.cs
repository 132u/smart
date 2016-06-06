using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	class WorkflowPersonalAccountTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public WorkflowPersonalAccountTests()
		{
			StartPage = StartPage.PersonalAccount;
		}

		[SetUp]
		public void WorkflowPersonalAccountTestsSetUp()
		{
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_editorPage = new EditorPage(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test, Description("S-7174"), ShortCheckList]
		public void WorkflowStepNotExistInDialogProjectCreationTest()
		{
			var document = PathProvider.OneLineTxtFile;

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { document });

			_newProjectSettingsPage
				.ClickNextButtonExpectingError()
				.FillGeneralProjectInformation(_projectUniqueName);

			Assert.IsFalse(_newProjectSettingsPage.IsWorkFlowLinkDisplayed(),
				"Проищошла ошибка:\n Отобразилась ссылка для создания этапа WF в персональном аккаунте.");

			Assert.IsTrue(_newProjectSettingsPage.IsFinishButtonDisplayed(),
				"Проищошла ошибка:\n Не отобразилась кнопка Finish при создании проекта в персональном аккаунте.");
		}

		[Test]
		public void WorkflowNotExistInProjectSettingsTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickSettingsButton();

			Assert.IsFalse(_settingsDialog.IsWorkflowSetupExistInSettings(),
				"Произошла ошибка:\n 'Workflow Setup' присутствует в настройках проекта");
		}

		[Test]
		public void ChooseTaskDialogNotDisplayTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.UploadDocument(new []{PathProvider.DocumentFile});

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(PathProvider.DocumentFile);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка:\n Не открылась страница редактора.");
		}

		private NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private NewProjectSettingsPage _newProjectSettingsPage;
		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectSettingsPage _projectSettingsPage;
		private ProjectSettingsDialog _settingsDialog;
		private ProjectsPage _projectsPage;
		private EditorPage _editorPage;
		private string _projectUniqueName;
	}
}
