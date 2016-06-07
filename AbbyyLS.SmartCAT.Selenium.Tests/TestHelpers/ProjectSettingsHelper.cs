using System;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectSettingsHelper
	{
		public WebDriver Driver { get; private set; }

		public ProjectSettingsHelper(WebDriver driver)
		{
			Driver = driver;

			_documentSettingsDialog = new DocumentSettingsDialog(Driver);
			_projectPage = new ProjectSettingsPage(Driver);
			_addFilesStep = new AddFilesStep(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_editorPage = new EditorPage(Driver);
			_deleteDocumentDialog = new DeleteDocumentDialog(Driver);
			_settingResourceStep = new SettingsResourcesStep(Driver);
			_workspacePage = new WorkspacePage(Driver);
		}

		public ProjectSettingsHelper AssignTasksOnDocument(string filePath, string nickName, string projectName, int taskNumber = 1)
		{
			_projectPage.ClickAssignButtonInDocumentInfo(filePath);

			_taskAssignmentPage
				.SetResponsible(nickName, false, taskNumber: taskNumber)
				.ClickSaveButton();

			_workspacePage.ClickProjectLink(projectName);

			return this;
		}

		public ProjectSettingsHelper CreateRevision(string documentName, string text = "Translation")
		{
			_projectPage.OpenDocumentInEditorWithTaskSelect(documentName);

			_selectTaskDialog.SelectTask();

			if (!_editorPage.IsTargetDisplayed())
			{
				throw new XPathLookupException("Произошла ошибка:\n сегмент с не появился.");
			}

			_editorPage
				.FillTarget(text)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			return this;
		}

		/// <summary>
		/// Загрузить файл в проект
		/// </summary>
		/// <param name="filePaths">путь к файлу</param>
		public ProjectSettingsHelper UploadDocument(IList<string> filePaths)
		{
			_projectPage
				.ClickDocumentUploadButton()
				.UploadDocument(filePaths);

			_addFilesStep.ClickNextButton();

			_settingResourceStep
				.ClickFinish<ProjectSettingsPage>()
				.WaitUntilDocumentProcessed();

			return this;
		}

		/// <summary>
		/// Удалить документ из проекта
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsHelper DeleteDocument(string documentName)
		{
			_projectPage
				.ClickDocumentCheckbox(documentName)
				.ClickDeleteButton();

			_deleteDocumentDialog.ConfirmDelete();

			return this;
		}

		public ProjectSettingsHelper OpenWorkflowSettings()
		{
			_projectPage
				.ClickSettingsButton()
				.ClickWorkflowTab();

			return this;
		}

		public ProjectSettingsHelper AddGlossaryToDocument(string documentPath, string glossaryName)
		{
			var documentName = Path.GetFileNameWithoutExtension(documentPath);

			_projectPage
				.HoverDocumentRow(documentName)
				.ClickDocumentSettings(documentName);

			_documentSettingsDialog
				.HoverGlossaryTableDocumentSettingsDialog()
				.ClickGlossaryByName(glossaryName)
				.ClickSaveButtonExpectingProjectSettingsPage();

			return this;
		}

		private readonly SettingsResourcesStep _settingResourceStep;
		private readonly DeleteDocumentDialog _deleteDocumentDialog;
		private readonly ProjectSettingsPage _projectPage;
		private readonly DocumentSettingsDialog _documentSettingsDialog;
		private readonly AddFilesStep _addFilesStep;
		private readonly TaskAssignmentPage _taskAssignmentPage;
		private readonly EditorPage _editorPage;
		private readonly SelectTaskDialog _selectTaskDialog;
		private readonly WorkspacePage _workspacePage;
	}
}
