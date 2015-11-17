using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	class BaseTmTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpTmTests()
		{
			UniqueTranslationMemoryName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			_workspaceHelper = new WorkspaceHelper(Driver);
			CreateProjectHelper = new CreateProjectHelper(Driver);

			ProjectsPage = new ProjectsPage(Driver);
			NewProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			NewProjectSettingsPage = new NewProjectSettingsPage(Driver);
			NewProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			NewProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);

			TranslationMemoriesHelper = _workspaceHelper.GoToTranslationMemoriesPage();
		}

		public string UniqueTranslationMemoryName { get; set; }

		public TranslationMemoriesHelper TranslationMemoriesHelper { get; private set; }
		public CreateProjectHelper CreateProjectHelper;
		private WorkspaceHelper _workspaceHelper;

		public ProjectsPage ProjectsPage;
		public NewProjectDocumentUploadPage NewProjectDocumentUploadPage;
		public NewProjectSettingsPage NewProjectSettingsPage;
		public NewProjectWorkflowPage NewProjectWorkflowPage;
		public NewProjectSetUpTMDialog NewProjectSetUpTMDialog;
	}
}
