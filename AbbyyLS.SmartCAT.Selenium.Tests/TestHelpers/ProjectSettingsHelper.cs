using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectSettingsHelper : WorkspaceHelper
	{
		public ProjectSettingsHelper(WebDriver driver) : base(driver)
		{
			_documentSettings = new DocumentSettings(Driver);
			_projectPage = new ProjectSettingsPage(Driver);
			_settingsDialog = new SettingsDialog(Driver);
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
		}

		public ProjectSettingsHelper ClickDocumentProgress(string filePath)
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickDocumentProgress(Path.GetFileNameWithoutExtension(filePath));

			return this;
		}

		public ProjectSettingsHelper AssignTasksOnDocument(string documentName, string nickName, int taskNumber = 1)
		{
			this
				.ClickDocumentProgress(documentName)
				.ClickAssignButtonInDocumentInfo()
				.SelectAssigneesForEntireDocument(taskNumber)
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
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickAssignButtonInDocumentInfo();

			return new TaskAssignmentPageHelper(Driver);
		}

		public TaskAssignmentPageHelper ClickAssignButtonOnPanel()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickAssignButtonOnPanel();

			return new TaskAssignmentPageHelper(Driver);
		}

		/// <summary>
		/// Открыть документ в редакторе
		/// </summary>
		/// <param name="documentName">название документа</param>
		public EditorHelper OpenDocument<T>(string documentName) where T : class , IAbstractPage<T>
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage
				.AssertIsDocumentProcessed()
				.ClickDocument<T>(documentName, Driver);

			return new EditorHelper(Driver);
		}

		/// <summary>
		/// Загрузить файл в проект
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public ProjectSettingsHelper UploadDocument(string filePath)
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage
				.ClickDocumentUploadButton()
				.UploadDocument(filePath);

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsFileUploaded(filePath));

			_documentUploadGeneralInformationDialog
				.ClickFinish<ProjectSettingsPage>()
				.WaitUntilUploadDocumentDialogDissapeared()
				.AssertIsDocumentProcessed();

			return this;
		}

		public DocumentUploadGeneralInformationDialog ClickDocumentUploadButton()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickDocumentUploadButton();

			return new DocumentUploadGeneralInformationDialog(Driver).GetPage();
		}

		/// <summary>
		/// Удалить документ из проекта
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsHelper DeleteDocument(string documentName)
		{
			BaseObject.InitPage(_projectPage, Driver);
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
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickDownloadInMainMenuButton();

			return new ExportFileHelper(Driver);
		}

		public ProjectSettingsHelper ClickDocumentCheckbox(string filePath)
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickDocumentCheckbox(Path.GetFileNameWithoutExtension(filePath));

			return this;
		}

		public ProjectSettingsHelper OpenWorkflowSettings()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage
				.ClickSettingsButton()
				.ClickWorkflowTab();

			return this;
		}

		public ProjectSettingsHelper OpenProjectSettings()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickSettingsButton();

			return this;
		}

		public ProjectSettingsHelper AssertWorkflowSettingsNotExist()
		{
			BaseObject.InitPage(_settingsDialog, Driver);
			_settingsDialog.AssertWorkflowSettingsNotExist();

			return this;
		}

		public ProjectSettingsHelper AssertWorkflowTaskCountMatch(int taskCount = 1)
		{
			BaseObject.InitPage(_settingsDialog, Driver);
			CustomTestContext.WriteLine("Проверить, что количество задач в настройках Workflow = {0}.", taskCount);

			Assert.AreEqual(taskCount, _settingsDialog.WorkflowTaskList().Count,
				"Произошла ошибка:\n неверное количество задач в настройках Workflow.");

			return this;
		}

		public ProjectSettingsHelper AssertWorkflowTaskMatch(WorkflowTask workflowTask, int taskNumber = 1)
		{
			BaseObject.InitPage(_settingsDialog, Driver);
			CustomTestContext.WriteLine("Проверить, что задача №{0} в настройках Workflow - это {1}", taskNumber, workflowTask);

			Assert.AreEqual(workflowTask.ToString(), _settingsDialog.WorkflowTaskList()[taskNumber - 1],
				"Произошла ошибка:\n задача не соответствует.");

			return this;
		}

		public ProjectSettingsHelper ClickDeleteTaskButton(int taskNumber = 1)
		{
			BaseObject.InitPage(_settingsDialog, Driver);
			_settingsDialog.ClickDeleteTaskButton(taskNumber);

			return this;
		}

		public ProjectSettingsHelper AssertConfirmDeleteDialogDisplay()
		{
			BaseObject.InitPage(_settingsDialog, Driver);
			_settingsDialog.AssertConfirmDeleteDialogDislpay();

			return this;
		}

		public ProjectSettingsHelper ClickSaveButton()
		{
			BaseObject.InitPage(_settingsDialog, Driver);
			_settingsDialog
				.ClickSaveButton()
				.AssertSettingsDialogDissappear();

			return this;
		}

		public ProjectSettingsHelper AddTask(WorkflowTask task, int taskNumber = 2)
		{
			BaseObject.InitPage(_settingsDialog, Driver);
			_settingsDialog
				.ClickNewTaskButton()
				.ExpandTask(taskNumber)
				.ClickTaskInDropdown(task);

			return this;
		}

		public ProjectSettingsHelper EditTask(WorkflowTask task, int taskNumber = 2)
		{
			BaseObject.InitPage(_settingsDialog, Driver);
			_settingsDialog
				.ExpandTask(taskNumber)
				.ClickTaskInDropdown(task);

			return this;
		}

		public ProjectSettingsHelper ClickCancelButton()
		{
			BaseObject.InitPage(_settingsDialog, Driver);
			_settingsDialog
				.ClickCancelButton()
				.AssertSettingsDialogDissappear();

			return this;
		}

		public ProjectSettingsHelper AssertDocumentExist(string fileName)
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.AssertDocumentExist(fileName);

			return this;
		}

		public ProjectSettingsHelper AssertAssignButtonNotDisplayed()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.AssertAssignButtonNotExist();

			return this;
		}

		public ProjectSettingsHelper ClickSortByTranslationDocument()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickSortByTranslationDocument();

			return this;
		}

		public ProjectSettingsHelper ClickSortByType()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickSortByType();

			return this;
		}

		public ProjectSettingsHelper ClickSortByStatus()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickSortByStatus();

			return this;
		}

		public ProjectSettingsHelper ClickSortByTarget()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickSortByTarget();

			return this;
		}

		public ProjectSettingsHelper ClickSortByAuthor()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickSortByAuthor();

			return this;
		}

		public ProjectSettingsHelper ClickSortByCreated()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickSortByCreated();

			return this;
		}

		public ProjectSettingsHelper ClickSortByQA()
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage.ClickSortByQA();

			return this;
		}

		public ProjectSettingsHelper AddGlossaryToDocument(string documentName, string glossaryName)
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage
				.ClickDocumentProgress(documentName)
				.ClickDocumentSettings();

			BaseObject.InitPage(_documentSettings, Driver);
			_documentSettings
				.HoverGlossaryTableDocumentSettingsDialog()
				.ClickGlossaryByName(glossaryName)
				.ClickSaveButton<ProjectSettingsPage>(Driver)
				.AssertDialogBackgroundDisappeared<ProjectSettingsPage>(Driver)
				.ClickDocumentProgress(documentName);

			return this;
		}

		private readonly ProjectSettingsPage _projectPage;
		private readonly DocumentSettings _documentSettings;
		private readonly SettingsDialog _settingsDialog;
		private readonly DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
	}
}
