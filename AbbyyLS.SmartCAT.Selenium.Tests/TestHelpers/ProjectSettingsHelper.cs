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

		public ProjectSettingsHelper AssignTasksOnDocument(string documentName, string nickName, int taskNumber = 1)
		{
			this
				.ClickDocumentProgress(documentName)
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType(taskNumber)
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

		public ProjectSettingsHelper AddTask(TaskMode task)
		{
			BaseObject.InitPage(_settingsDialog);
			_settingsDialog
				.ClickNewTaskButton()
				.ExpandTask(2)
				.ClickTaskInDropdown(task)
				.ClickSaveButton()
				.AssertSettingsDialogDissappear();

			return this;
		}

		public ProjectSettingsHelper AssertDocumentExist(string fileName)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.AssertDocumentExist(fileName);

			return this;
		}

		public ProjectSettingsHelper ClickSortByTranslationDocument()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickSortByTranslationDocument();

			return this;
		}

		public ProjectSettingsHelper ClickSortByType()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickSortByType();

			return this;
		}

		public ProjectSettingsHelper ClickSortByStatus()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickSortByStatus();

			return this;
		}

		public ProjectSettingsHelper ClickSortByTarget()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickSortByTarget();

			return this;
		}

		public ProjectSettingsHelper ClickSortByAuthor()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickSortByAuthor();

			return this;
		}

		public ProjectSettingsHelper ClickSortByCreated()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickSortByCreated();

			return this;
		}

		public ProjectSettingsHelper ClickSortByQA()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickSortByQA();

			return this;
		}

		private readonly ProjectSettingsPage _projectPage = new ProjectSettingsPage();
		private readonly SettingsDialog _settingsDialog = new SettingsDialog();
	}
}
