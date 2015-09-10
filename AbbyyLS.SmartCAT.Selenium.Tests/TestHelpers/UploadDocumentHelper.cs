using System.IO;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class UploadDocumentHelper : ProjectsHelper 
	{
		public UploadDocumentHelper UploadDocument(string filePath)
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog);
			_documentUploadGeneralInformationDialog.UploadDocument(filePath);

			return this;
		}

		public UploadDocumentHelper AssertFileUploaded(string filePath)
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog);
			_documentUploadGeneralInformationDialog.AssertFileUploaded(Path.GetFileName(filePath));

			return this;
		}

		public UploadDocumentHelper ClickNextOnGeneralInformationPage()
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog);
			_documentUploadGeneralInformationDialog.ClickNext<DocumentUploadSetUpTMDialog>();

			return this;
		}

		public UploadDocumentHelper ClickNextOnSetUpTMPage()
		{
			BaseObject.InitPage(_documentUploadSetUpTMDialog);
			_documentUploadSetUpTMDialog.ClickNext<DocumentUploadTaskAssignmentDialog>();

			return this;
		}

		public ProjectSettingsHelper ClickFinishUploadOnProjectSettingsPage()
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog);
			_documentUploadGeneralInformationDialog.ClickFinish<ProjectSettingsPage>();

			return new ProjectSettingsHelper();
		}

		public ProjectsHelper ClickFihishUploadOnProjectsPage()
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog);
			_documentUploadGeneralInformationDialog
				.ClickFinish<ProjectsPage>()
				.WaitUploadDocumentDialogDisappear()
				.AssertDialogBackgroundDisappeared<ProjectsPage>();

			return new ProjectsHelper();
		}

		public UploadDocumentHelper AssertErrorDuplicateDocumentNameExist()
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog);
			_documentUploadGeneralInformationDialog.AssertErrorDuplicateDocumentNameExist();

			return this;
		}

		private readonly DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog();
		private readonly DocumentUploadSetUpTMDialog _documentUploadSetUpTMDialog = new DocumentUploadSetUpTMDialog();
	}
}
