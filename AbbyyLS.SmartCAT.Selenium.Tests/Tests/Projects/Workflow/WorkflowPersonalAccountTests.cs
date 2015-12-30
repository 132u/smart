using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class WorkflowPersonalAccountTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public WorkflowPersonalAccountTests()
		{
			StartPage = StartPage.PersonalAccount;
		}

		[SetUp]
		public void WorkflowPersonalAccountTestsSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_settingsDialog = new SettingsDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
		}

		[Test]
		public void WorkflowStepNotExistInDialogProjectCreationTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				createNewTm: true,
				glossaryName:_glossaryUniqueName,
				personalAccount: true);
		}

		[Test]
		public void WorkflowNotExistInProjectSettingsTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage.ClickSettingsButton();

			Assert.IsFalse(_settingsDialog.IsWorkflowSetupExistInSettings(),
				"Произошла ошибка:\n 'Workflow Setup' присутствует в настройках проекта");
		}

		[Test]
		public void ChooseTaskDialogNotDisplayTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper.UploadDocument(new []{PathProvider.DocumentFile});

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(PathProvider.DocumentFile);
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsHelper _projectSettingsHelper;
		private ProjectSettingsPage _projectSettingsPage;
		private SettingsDialog _settingsDialog;
		private ProjectsPage _projectsPage;
		private string _projectUniqueName;
		private string _glossaryUniqueName;
	}
}
