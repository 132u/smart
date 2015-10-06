using System.IO;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class UploadDocumentHelper : ProjectsHelper 
	{
		public UploadDocumentHelper(WebDriver driver) : base(driver)
		{
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_documentUploadSetUpTMDialog = new DocumentUploadSetUpTMDialog(Driver);
		}

		public UploadDocumentHelper UploadDocument(string filePath)
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog, Driver);
			_documentUploadGeneralInformationDialog.UploadDocument(filePath);

			return this;
		}

		public UploadDocumentHelper AssertFileUploaded(string filePath)
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog, Driver);
			_documentUploadGeneralInformationDialog.AssertFileUploaded(Path.GetFileName(filePath));

			return this;
		}

		public UploadDocumentHelper ClickNextOnGeneralInformationPage()
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog, Driver);
			_documentUploadGeneralInformationDialog.ClickNext<DocumentUploadSetUpTMDialog>(Driver);

			return this;
		}

		public UploadDocumentHelper ClickNextOnSetUpTMPage()
		{
			BaseObject.InitPage(_documentUploadSetUpTMDialog, Driver);
			_documentUploadSetUpTMDialog.ClickNext<DocumentUploadTaskAssignmentDialog>(Driver);

			return this;
		}

		public ProjectSettingsHelper ClickFinishUploadOnProjectSettingsPage()
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog, Driver);
			_documentUploadGeneralInformationDialog.ClickFinish<ProjectSettingsPage>(Driver);

			return new ProjectSettingsHelper(Driver);
		}

		public ProjectsHelper ClickFihishUploadOnProjectsPage()
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog, Driver);
			_documentUploadGeneralInformationDialog
				.ClickFinish<ProjectsPage>(Driver)
				.WaitUploadDocumentDialogDisappear()
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			return new ProjectsHelper(Driver);
		}

		public UploadDocumentHelper AssertErrorDuplicateDocumentNameExist()
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog, Driver);
			_documentUploadGeneralInformationDialog.AssertErrorDuplicateDocumentNameExist();

			return this;
		}

		private readonly DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private readonly DocumentUploadSetUpTMDialog _documentUploadSetUpTMDialog;
	}
}
