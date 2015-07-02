using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	class ExportProjectBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpProjectExportTests()
		{
			WorkspaceHelper.GoToProjectsPage();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.DocumentFileToConfirm1)
				.AssertIsProjectLoaded(_projectUniqueName)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.DocumentFileToConfirm1))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenDocument<SelectTaskDialog>(PathProvider.DocumentFileToConfirm1)
				.SelectTask()
				.CloseTutorialIfExist()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton();
		}

		[TearDown]
		public void Teardown()
		{
			_projectsHelper.GoToProjectsPage();
			_exportFileHelper.CancelAllNotifiers<ProjectsPage>();
		}

		protected string _projectUniqueName;
		protected readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		protected readonly ExportFileHelper _exportFileHelper = new ExportFileHelper();
		protected readonly ProjectSettingsHelper _projectSettingsHelper = new ProjectSettingsHelper();
		protected readonly ProjectsHelper _projectsHelper = new ProjectsHelper();
	}
}
