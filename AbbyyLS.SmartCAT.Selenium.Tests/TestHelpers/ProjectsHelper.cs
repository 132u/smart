using System.IO;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectsHelper : WorkspaceHelper
	{

		/// <summary>
		/// Кликнуть чекбокс проекта. Если чекбокс уже отмечен, отметка снимается
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsHelper SelectProjectInList(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.SelectProjectInList(projectName);

			return this;
		}

		public ProjectsHelper DeleteProjectFromList()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.ClickDeleteProject()
				.ClickDeleteButton()
				.WaitDeleteProjectDialogDissapeared();

			return this;
		}

		public ProjectsHelper DeleteProjectWithFileFromList()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.ClickDeleteOpenProjectWithFile()
				.ClickDeleteProjectButton()
				.WaitDeleteProjectWithFileDialogDissapeared();

			return this;
		}

		public ProjectsHelper AssertProjectSuccessfullyDeleted(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.AssertProjectNotExist(projectName);

			return this;
		}

		public ProjectsHelper OpenProjectInfo(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.OpenProjectInfo(projectName);

			return this;
		}

		public ProjectsHelper CheckProjectAppearInList(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.AssertProjectAppearInList(projectName);

			return this;
		}

		public CreateProjectHelper ClickCreateProjectButton()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickCreateProjectButton();

			return new CreateProjectHelper();
		}

		public EditorHelper OpenDocument(string projectName, string filePath)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.OpenProjectInfo(projectName)
				.ClickDocumentRef(Path.GetFileNameWithoutExtension(filePath));

			return new EditorHelper();
		}

		public ProjectsHelper AssertIsProjectLoaded(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.AssertIsProjectLoaded(projectName);

			return this;
		}

		public ExportFileHelper ClickDownloadInMainMenuButton()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickDownloadInMainMenuButton();

			return new ExportFileHelper();
		}

		public ExportFileHelper ClickDownloadInProjectButton(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickDownloadInProjectButton(projectName);

			return new ExportFileHelper();
		}

		public ExportFileHelper ClickDownloadInDocumentButton(string projectName, int documentNumber = 1)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickDownloadInDocumentButton(projectName, documentNumber);

			return new ExportFileHelper();
		}

		public ProjectsHelper OpenDocumentInfoForProject(string projectName, int documentNumber = 1)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.OpenDocumentInfoForProject(projectName, documentNumber);

			return this;
		}

		public ProjectsHelper ClickDocumentSettings(string projectName, int documentNumber = 1)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickDocumentSettings(projectName, documentNumber);

			return this;
		}

		public ProjectsHelper RenameDocument(string projectName, string newName, int documentNumber = 1)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.ClickDocumentSettings(projectName, documentNumber)
				.SetDocumentName(newName)
				.ClickSaveButton()
				.AssertDialogBackgroundDisappeared<ProjectsPage>();

			return this;
		}

		public ProjectsHelper AddMachineTranslationToDocument(string projectName, int documentNumber = 1, MachineTranslationType machineTranslation = MachineTranslationType.DefaultMT)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.ClickDocumentSettings(projectName, documentNumber)
				.SelectMachineTranslation(machineTranslation)
				.ClickSaveButton()
				.AssertDialogBackgroundDisappeared<ProjectsPage>()
				.AssertIsProjectLoaded(projectName);

			return this;
		}

		public TaskAssignmentDialogHelper OpenAssignDialog(string projectName)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage
				.OpenProjectInfo(projectName)
				.OpenDocumentInfoForProject(projectName)
				.ClickDocumentAssignButton(projectName);

			return new TaskAssignmentDialogHelper();
		}

		public UploadDocumentHelper ClickDocumentUploadButton()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickDocumentUploadButton();

			return new UploadDocumentHelper();
		}

		public ProjectsHelper WaitCreateProjectDialogDisappear()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.WaitCreateProjectDialogDissapear();

			return new ProjectsHelper();
		}

		private readonly ProjectsPage _projectsPage = new ProjectsPage();
		private readonly DocumentSettings _documentSettings = new DocumentSettings();
	}
}