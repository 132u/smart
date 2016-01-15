using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class UserRightsWorkflowPersonalAccountTests<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		public UserRightsWorkflowPersonalAccountTests()
		{
			StartPage = StartPage.PersonalAccount;
		}

		[SetUp]
		public void UserRightsWorkflowPersonalAccountTestsSetUp()
		{
			document1 = PathProvider.DocumentFile;
			
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);

			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_editorPage = new EditorPage(Driver);
			
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}
		
		[Test, Description("ТС-76")]
		public void PersonalAccountWorkflowTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: document1, personalAccount: true);
			
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(document1);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка:\nРедактор не открылся.");

			Assert.IsFalse(_editorPage.IsStageNameIsEmpty(),
				"Произошла ошибка:\nВ редакторе отображается название этапа.");

			Assert.IsFalse(_editorPage.IsWorkflowColumnDisplayed(),
				"Произошла ошибка:\nВ редакторе присутствует колонка этапа.");
		}

		protected string _projectUniqueName;

		protected CreateProjectHelper _createProjectHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;

		protected TaskAssignmentPage _taskAssignmentPage;
		protected ProjectsPage _projectsPage;
		protected EditorPage _editorPage;
		private string document1;
	}
}
