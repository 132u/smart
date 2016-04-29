using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.QualityAssurance
{
	public class QualityAssuranceBaseTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_projectSettingsDialog = new ProjectSettingsDialog(Driver);
			_advancedSettingsSection = new AdvancedSettingsSection(Driver);
			_qualityAssuranceAdvancedSettingsSection = new QualityAssuranceAdvancedSettingsSection(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_qualityAssuranceSettings = new QualityAssuranceSettings(Driver);
			_qualityAssuranceDialog = new QualityAssuranceDialog(Driver);
			_errorsDialog = new ErrorsDialog(Driver);
		}

		protected ErrorsDialog _errorsDialog;
		protected QualityAssuranceDialog _qualityAssuranceDialog;
		protected EditorPage _editorPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected ProjectSettingsDialog _projectSettingsDialog;
		protected QualityAssuranceAdvancedSettingsSection _qualityAssuranceAdvancedSettingsSection;
		protected AdvancedSettingsSection _advancedSettingsSection;
		protected QualityAssuranceSettings _qualityAssuranceSettings;

		protected const string _error1 = "Unexpected whitespace at the end of the segment";
		protected const string _error2 = "Repeated word";
		protected const string _error3 = "Source and target are identical";
	}
}
