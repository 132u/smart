using System.IO;

using NLog;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectSettingsHelper : WorkspaceHelper
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

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

		public ProjectSettingsHelper CreateRevision(string documentName, string text = "Translation")
		{
			this
				.OpenDocument<SelectTaskDialog>(documentName)
				.SelectTask()
				.CloseTutorialIfExist()
				.AssertTargetDisplayed()
				.FillTarget(text)
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

		public ProjectSettingsHelper AssertWorkflowTaskCountMatch(int taskCount = 1)
		{
			BaseObject.InitPage(_settingsDialog);
			Logger.Trace("Проверить, что количество задач в настройках Workflow = {0}.", taskCount);

			Assert.AreEqual(taskCount, _settingsDialog.WorkflowTaskList().Count,
				"Произошла ошибка:\n неверное количество задач в настройках Workflow.");

			return this;
		}

		public ProjectSettingsHelper AssertWorkflowTaskMatch(WorkflowTask workflowTask, int taskNumber = 1)
		{
			BaseObject.InitPage(_settingsDialog);
			Logger.Trace("Проверить, что задача №{0} в настройках Workflow - это {1}", taskNumber, workflowTask);

			Assert.AreEqual(workflowTask.ToString(), _settingsDialog.WorkflowTaskList()[taskNumber - 1],
				"Произошла ошибка:\n задача не соответствует.");

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

		public ProjectSettingsHelper ClickSaveButton()
		{
			BaseObject.InitPage(_settingsDialog);
			_settingsDialog
				.ClickSaveButton()
				.AssertSettingsDialogDissappear();

			return this;
		}

		public ProjectSettingsHelper AddTask(WorkflowTask task, int taskNumber = 2)
		{
			BaseObject.InitPage(_settingsDialog);
			_settingsDialog
				.ClickNewTaskButton()
				.ExpandTask(taskNumber)
				.ClickTaskInDropdown(task);

			return this;
		}

		public ProjectSettingsHelper EditTask(WorkflowTask task, int taskNumber = 2)
		{
			BaseObject.InitPage(_settingsDialog);
			_settingsDialog
				.ExpandTask(taskNumber)
				.ClickTaskInDropdown(task);

			return this;
		}

		public ProjectSettingsHelper ClickCancelButton()
		{
			BaseObject.InitPage(_settingsDialog);
			_settingsDialog
				.ClickCancelButton()
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
