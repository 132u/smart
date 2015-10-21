using System.IO;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectsHelper : WorkspaceHelper
	{
		public ProjectsHelper(WebDriver driver) : base(driver)
		{
			_projectsPage = new ProjectsPage(Driver);
			_documentSettings = new DocumentSettings(Driver);
		}

		/// <summary>
		/// Кликнуть чекбокс проекта. Если чекбокс уже отмечен, отметка снимается
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsHelper SelectProjectInList(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.SelectProjectInList(projectName);

			return this;
		}

		public ProjectsHelper DeleteFromList()
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage
				.ClickDeleteButton()
				.ClickConfirmDeleteButton()
				.WaitDeleteDialogDissapeared();

			return this;
		}

		public ProjectsHelper DeleteProjectWithFileFromList()
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage
				.ClickDeleteOpenProjectWithFile()
				.ClickDeleteProjectButton()
				.WaitDeleteProjectWithFileDialogDissapeared();

			return this;
		}

		public ProjectsHelper DeleteInProjectMenu(string projectName)
		{
			_projectsPage
				.ClickDeleteInProjectButton(projectName)
				.ClickConfirmDeleteButton()
				.WaitDeleteDialogDissapeared();

			return this;
		}

		public ProjectsHelper AssertProjectSuccessfullyDeleted(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.AssertProjectNotExist(projectName);

			return this;
		}

		public ProjectsHelper OpenProjectInfo(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.OpenProjectInfo(projectName);

			return this;
		}

		public ProjectsHelper CheckProjectAppearInList(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.AssertProjectAppearInList(projectName);

			return this;
		}

		public CreateProjectHelper ClickCreateProjectButton()
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.ClickCreateProjectButton();

			return new CreateProjectHelper(Driver);
		}

		public EditorHelper OpenDocument(string projectName, string filePath)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage
				.OpenProjectInfo(projectName)
				.ClickDocumentRef(Path.GetFileNameWithoutExtension(filePath));

			return new EditorHelper(Driver);
		}

		public ProjectsHelper AssertIsProjectLoadedSuccessfully(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage
				.AssertIsProjectLoaded(projectName)
				.AssertProjectLoadFatalErrorNotDisplayed(projectName)
				.AssertProjectLoadWarningErrorNotDisplayed(projectName);

			return this;
		}

		public ExportFileHelper ClickDownloadInMainMenuButton()
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.ClickDownloadInMainMenuButton();

			return new ExportFileHelper(Driver);
		}

		public ExportFileHelper ClickDownloadInProjectButton(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.ClickDownloadInProjectButton(projectName);

			return new ExportFileHelper(Driver);
		}

		public ExportFileHelper ClickDownloadInDocumentButton(string projectName, int documentNumber = 1)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.ClickDownloadInDocumentButton(projectName, documentNumber);

			return new ExportFileHelper(Driver);
		}

		public ProjectsHelper OpenDocumentInfoForProject(string projectName, int documentNumber = 1)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.OpenDocumentInfoForProject(projectName, documentNumber);

			return this;
		}

		public ProjectsHelper ClickDocumentSettings(string projectName, int documentNumber = 1)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.ClickDocumentSettings(projectName, documentNumber);

			return this;
		}

		public ProjectsHelper RenameDocument(string projectName, string newName, int documentNumber = 1)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage
				.ClickDocumentSettings(projectName, documentNumber)
				.SetDocumentName(newName)
				.ClickSaveButton<ProjectsPage>(Driver)
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			return this;
		}

		public ProjectsHelper AddMachineTranslationToDocument(string projectName, int documentNumber = 1, MachineTranslationType machineTranslation = MachineTranslationType.DefaultMT)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage
				.ClickDocumentSettings(projectName, documentNumber)
				.SelectMachineTranslation(machineTranslation)
				.ClickSaveButton<ProjectsPage>(Driver)
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver)
				.AssertIsProjectLoaded(projectName);

			return this;
		}

		public TaskAssignmentDialogHelper OpenAssignDialog(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage
				.OpenProjectInfo(projectName)
				.OpenDocumentInfoForProject(projectName)
				.ClickDocumentAssignButton(projectName);

			return new TaskAssignmentDialogHelper(Driver);
		}

		public DocumentUploadGeneralInformationDialog ClickDocumentUploadButton()
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.ClickDocumentUploadButton();

			return new DocumentUploadGeneralInformationDialog(Driver).GetPage();
		}

		public ProjectsHelper AssertLinkProjectNotExist(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.AssertLinkProjectNotExist(projectName);

			return this;
		}

		public ProjectsHelper SelectDocument(string projectName, string documentName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.SelectDocument(projectName, documentName);

			return this;
		}

		public ProjectsHelper AssertSignInToConnectorButtonNotExist()
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.AssertSignInToConnectorButtonNotExist();

			return this;
		}

		public ProjectsHelper AssertQACheckButtonExist(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.AssertQACheckButtonExist(projectName);

			return this;
		}

		public ProjectsHelper ClickProjectSettingsButton(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.ClickProjectSettingsButton(projectName);

			return this;
		}

		public ProjectsHelper ClickProjectAnalysisButton(string projectName)
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.ClickProjectAnalysisButton(projectName);

			return this;
		}

		private readonly ProjectsPage _projectsPage;
		private readonly DocumentSettings _documentSettings;
	}
}