using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class UploadDocumentHelper : ProjectsHelper 
	{
		public UploadDocumentHelper UploadDocument(string fileName)
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog);
			_documentUploadGeneralInformationDialog.UploadDocument(fileName);

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
			_documentUploadSetUpTMDialog.ClickNext<DocumentUploadAssignResponsiblesDialog>();

			return this;
		}

		public ProjectSettingsHelper ClickFihishUploadOnProjectSettingsPage()
		{
			BaseObject.InitPage(_documentUploadGeneralInformationDialog);
			_documentUploadGeneralInformationDialog.ClickFinish<ProjectSettingsPage>();

			return new ProjectSettingsHelper();
		}


		private readonly DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog();
		private readonly DocumentUploadSetUpTMDialog _documentUploadSetUpTMDialog = new DocumentUploadSetUpTMDialog();
	}
}
