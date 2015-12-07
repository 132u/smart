﻿using System;
using System.Collections.Generic;

using OpenQA.Selenium;

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
			_documentSettingsDialog = new DocumentSettingsDialog(Driver);
			_projectPage = new ProjectSettingsPage(Driver);
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_selectAssigneePage = new SelectAssigneePage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_editorPage = new EditorPage(Driver);
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

			_taskAssignmentPage.ClickSaveButtonProjectSettingsPage();

			return this;
		}

		public ProjectSettingsHelper CreateRevision(string documentName, string text = "Translation")
		{
			_projectPage.OpenDocumentInEditorWithTaskSelect(documentName);

			_selectTaskDialog.SelectTask();

			_editorPage.CloseTutorialIfExist();

			if (!_editorPage.IsTargetDisplayed())
			{
				throw new XPathLookupException("Произошла ошибка:\n сегмент с не появился.");
			}

			_editorPage
				.FillTarget(text)
				.ConfirmSegmentTranslation()
				.ClickHomeButton();

			return this;
		}

		/// <summary>
		/// Загрузить файл в проект
		/// </summary>
		/// <param name="filePaths">путь к файлу</param>
		public ProjectSettingsHelper UploadDocument(IList<string> filePaths)
		{
			BaseObject.InitPage(_projectPage, Driver);
			_projectPage
				.ClickDocumentUploadButton()
				.UploadDocument(filePaths);

			foreach (var filePath in filePaths)
			{
				if (!_documentUploadGeneralInformationDialog.IsFileUploaded(filePath))
				{
					throw new Exception("Произошла ошибка: '\nдокумент не загружен");
				}
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

			BaseObject.InitPage(_documentSettingsDialog, Driver);
			_documentSettingsDialog
				.HoverGlossaryTableDocumentSettingsDialog()
				.ClickGlossaryByName(glossaryName)
				.ClickSaveButton<ProjectSettingsPage>(Driver)
				.AssertDialogBackgroundDisappeared<ProjectSettingsPage>(Driver)
				.ClickDocumentProgress(documentName);

			return this;
		}

		private readonly ProjectSettingsPage _projectPage;
		private readonly DocumentSettingsDialog _documentSettingsDialog;
		private readonly DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		private readonly TaskAssignmentPage _taskAssignmentPage;
		private readonly SelectAssigneePage _selectAssigneePage;
		private readonly EditorPage _editorPage;
		private readonly SelectTaskDialog _selectTaskDialog;
	}
}
