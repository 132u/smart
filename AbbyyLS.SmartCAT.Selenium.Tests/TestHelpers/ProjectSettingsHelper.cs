using System;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
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
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_selectAssigneePage = new SelectAssigneePage(Driver);
			_editorHelper = new EditorHelper(Driver);
		}

		public ProjectSettingsHelper AssignTasksOnDocument(string documentName, string nickName, int taskNumber = 1)
		{
			_projectPage
				.ClickDocumentProgress(documentName)
				.ClickAssignButtonInDocumentInfo();

			_taskAssignmentPage.SelectAssigneesForEntireDocument(taskNumber);

			_selectAssigneePage
				.SelectAssignee(nickName)
				.ClickClose();

			_taskAssignmentPage.ClickSaveAssignButton();

			return this;
		}

		public ProjectSettingsHelper CreateRevision(string documentName, string text = "Translation")
		{
			_projectPage.OpenDocumentInEditorWithTaskSelect(documentName);

			_editorHelper
				.SelectTask()
				.CloseTutorialIfExist()
				.AssertTargetDisplayed()
				.FillTarget(text)
				.ClickConfirmButton()
				.ClickHomeButton();

			return this;
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

			if (!_documentUploadGeneralInformationDialog.IsFileUploaded(filePath))
			{
				throw new Exception("Произошла ошибка: '\nдокумент не загружен");
			}

			_documentUploadGeneralInformationDialog
				.ClickFinish<ProjectSettingsPage>()
				.WaitUntilUploadDocumentDialogDissapeared()
				.WaitUntilDocumentProcessed();

			return this;
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
				.WaitDeleteDocumentDialogDissappeared();

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
		private readonly DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private readonly TaskAssignmentPage _taskAssignmentPage;
		private readonly SelectAssigneePage _selectAssigneePage;
		private readonly EditorHelper _editorHelper;
	}
}
