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
			_workspaceHelper = new WorkspaceHelper(Driver);
			UniqueTranslationMemoryName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
			TranslationMemoriesHelper = _workspaceHelper.GoToTranslationMemoriesPage();
			CreateProjectHelper = new CreateProjectHelper(Driver);
			NewProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog(Driver);
			NewProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);
			NewProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
		}

		public string UniqueTranslationMemoryName { get; set; }

		public TranslationMemoriesHelper TranslationMemoriesHelper { get; private set; }
		public CreateProjectHelper CreateProjectHelper;
		public ProjectsPage ProjectsPage;
		public NewProjectSetUpWorkflowDialog NewProjectSetUpWorkflowDialog;
		public NewProjectSetUpTMDialog NewProjectSetUpTMDialog;
		public NewProjectGeneralInformationDialog NewProjectGeneralInformationDialog;
		private WorkspaceHelper _workspaceHelper;
	}
}
