using System.IO;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;


namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectSettingsHelper : WorkspaceHelper
	{
		public ProjectSettingsHelper ClickDocumentProgress(string filePath)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickDocumentProgress(Path.GetFileNameWithoutExtension(filePath));

			return this;
		}

		public ProjectSettingsHelper AssignTasksOnDocument(string documentName, string nickName)
		{
			this
				.ClickDocumentProgress(documentName)
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(nickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton();

			return this;
		}

		public ProjectSettingsHelper CreateRevision(string documentName)
		{
			this
				.OpenDocument<SelectTaskDialog>(documentName)
				.SelectTask()
				.CloseTutorialIfExist()
				.AssertTargetDisplayed()
				.FillTarget()
				.ClickConfirmButton()
				.ClickHomeButton();

			return this;
		}

		public TaskAssignmentPageHelper ClickAssignButtonInDocumentInfo()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickAssignButtonInDocumentInfo();

			return new TaskAssignmentPageHelper();
		}

		public TaskAssignmentPageHelper ClickAssignButtonOnPanel()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickAssignButtonOnPanel();

			return new TaskAssignmentPageHelper();
		}

		/// <summary>
		/// Открыть документ в редакторе
		/// </summary>
		/// <param name="documentName">название документа</param>
		public EditorHelper OpenDocument<T>(string documentName) where T : class , IAbstractPage<T>, new()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage
				.AssertIsDocumentProcessed()
				.ClickDocument<T>(documentName);

			return new EditorHelper();
		}

		/// <summary>
		/// Загрузить файл в проект
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public ProjectSettingsHelper UploadDocument(string filePath)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage
				.ClickDocumentUploadButton()
				.UploadDocument(filePath)
				.AssertFileUploaded(Path.GetFileName(filePath))
				.ClickFinish<ProjectSettingsPage>()
				.WaitUntilUploadDocumentDialogDissapeared();

			return this;
		}

		public UploadDocumentHelper ClickDocumentUploadButton()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickDocumentUploadButton();

			return new UploadDocumentHelper();
		}

		/// <summary>
		/// Удалить документ из проекта
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsHelper DeleteDocument(string documentName)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage
				.ClickProjectsTableCheckbox(documentName)
				.ClickDeleteButton()
				.ConfirmDelete()
				.WaitDeleteDocumentDialogDissappeared()
				.AssertDocumentNotExist(documentName);

			return this;
		}

		public ExportFileHelper ClickDownloadInMainMenuButton()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickDownloadInMainMenuButton();

			return new ExportFileHelper();
		}

		public ProjectSettingsHelper ClickDocumentCheckbox(string filePath)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickDocumentCheckbox(Path.GetFileNameWithoutExtension(filePath));

			return this;
		}

		public ProjectSettingsHelper OpenWorkflowSettings()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage
				.ClickSettingsButton()
				.ClickWorkflowTab();

			return this;
		}

		public ProjectSettingsHelper ClickDeleteTaskButton(int taskNumber = 1)
		{
			BaseObject.InitPage(_settingsDialog);
			_settingsDialog.ClickDeleteTaskButton(taskNumber);

			return this;
		}

		public ProjectSettingsHelper AssertConfirmDeleteDialogDisplay()
		{
			BaseObject.InitPage(_settingsDialog);
			_settingsDialog.AssertConfirmDeleteDialogDislpay();

			return this;
		}

		public ProjectSettingsHelper AcceptAllTasks(string filePath)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.AcceptAllTasksForDocument(Path.GetFileNameWithoutExtension(filePath));

			return this;
		}

		public ProjectSettingsHelper AssertDocumentExist(string fileName)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.AssertDocumentExist(fileName);

			return this;
		}

		private readonly ProjectSettingsPage _projectPage = new ProjectSettingsPage();
		private readonly SettingsDialog _settingsDialog = new SettingsDialog();
	}
}
