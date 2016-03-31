using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	class BaseTmTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpTmTests()
		{
			_translationMemoryAdvancedSettingsSection = new TranslationMemoryAdvancedSettingsSection(Driver);
			TranslationMemoriesHelper = new TranslationMemoriesHelper(Driver);
			CreateProjectHelper = new CreateProjectHelper(Driver);
			TranslationMemoriesHelper = new TranslationMemoriesHelper(Driver);
			WorkspacePage = new WorkspacePage(Driver);
			TranslationMemoriesPage = new TranslationMemoriesPage(Driver);
			TranslationMemoriesFilterDialog = new TranslationMemoriesFilterDialog(Driver);
			NewTranslationMemoryDialog = new NewTranslationMemoryDialog(Driver);
			ImportTmxDialog = new ImportTmxDialog(Driver);
			ProjectsPage = new ProjectsPage(Driver);
			NewProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			NewProjectSettingsPage = new NewProjectSettingsPage(Driver);
			NewProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			NewProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);
			ConfirmReplacementDialog = new ConfirmReplacementDialog(Driver);
			DeleteTmDialog = new DeleteTmDialog(Driver);
			UniqueTMName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			WorkspacePage.GoToTranslationMemoriesPage();
			TranslationMemoriesPage.CloseAllNotifications<TranslationMemoriesPage>();
		}

		public string UniqueTMName { get; set; }

		public TranslationMemoriesHelper TranslationMemoriesHelper { get; private set; }
		public CreateProjectHelper CreateProjectHelper;
		public TranslationMemoriesPage TranslationMemoriesPage;
		public TranslationMemoriesFilterDialog TranslationMemoriesFilterDialog;
		public NewTranslationMemoryDialog NewTranslationMemoryDialog;
		public ImportTmxDialog ImportTmxDialog;
		public WorkspacePage WorkspacePage;
		public ProjectsPage ProjectsPage;
		public NewProjectDocumentUploadPage NewProjectDocumentUploadPage;
		public NewProjectSettingsPage NewProjectSettingsPage;
		public NewProjectWorkflowPage NewProjectWorkflowPage;
		public NewProjectSetUpTMDialog NewProjectSetUpTMDialog;
		public ConfirmReplacementDialog ConfirmReplacementDialog;
		public DeleteTmDialog DeleteTmDialog;
		protected TranslationMemoryAdvancedSettingsSection _translationMemoryAdvancedSettingsSection;
	}
}
