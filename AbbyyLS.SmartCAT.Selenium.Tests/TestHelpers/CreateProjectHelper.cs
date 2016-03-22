using System;
using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class CreateProjectHelper
	{
		public WebDriver Driver { get; private set; }

		public CreateProjectHelper(WebDriver driver)
		{
			Driver = driver;

			_projectsPage = new ProjectsPage(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_newProjectEditGlossaryDialog = new NewProjectEditGlossaryDialog(Driver);
		}

		public CreateProjectHelper CreateNewProject(
			string projectName,
			IList<string> filesPaths = null,
			string glossaryName = null,
			bool createNewTm = false,
			IList<string> tmxFilesPaths = null,
			bool useMachineTranslation = false,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			IEnumerable<WorkflowTask> tasks = null,
			bool personalAccount = false,
			Deadline deadline = Deadline.CurrentDate,
			bool expectingError = false,
			string deadlineDate = null)
		{
			_projectsPage.ClickCreateProjectButton();

			if (filesPaths == null && tmxFilesPaths == null)
			{
				_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();
			}
			else
			{
				if (filesPaths != null)
				{
					_newProjectDocumentUploadPage.UploadDocumentFiles(filesPaths);
				}

				if (tmxFilesPaths != null)
				{
					_newProjectDocumentUploadPage.UploadTmxFiles(tmxFilesPaths);
				}

				_newProjectDocumentUploadPage.ClickSettingsButton();
			}

			_newProjectSettingsPage.FillGeneralProjectInformation(projectName, sourceLanguage, targetLanguage, useMachineTranslation: useMachineTranslation, deadline: deadline, date: deadlineDate);

			if (glossaryName != null)
			{
				_newProjectSettingsPage
					.ExpandAdvancedSettings()
					.ClickGlossariesTab()
					.ClickCreateGlossaryButton()
					.OpenEditGlossaryDialog();

				_newProjectEditGlossaryDialog
					.FillGlossaryName(glossaryName)
					.ClickSaveButton();
			}

			if (createNewTm)
			{
				_newProjectSettingsPage.ExpandAdvancedSettings();
			}

			if (!personalAccount)
			{
				_newProjectSettingsPage.ClickNextButton();

				if (tasks != null)
				{
					_newProjectWorkflowPage.ClickClearButton();

					foreach (var task in tasks)
					{
						_newProjectWorkflowPage.ClickNewTaskButton(task);
					}
				}
				
				_newProjectWorkflowPage.ClickCreateProjectButton();
			}
			else
			{
				_newProjectSettingsPage.ClickCreateProjectButton();
			}

			if (!expectingError)
			{
				_projectsPage.WaitUntilProjectLoadSuccessfully(projectName);
			}

			return this;
		}

		public string GetProjectUniqueName()
		{
			return "Test Project" + "-" + Guid.NewGuid();
		}

		public const string CourseraProjectName = "Communication Science";

		private readonly ProjectsPage _projectsPage;

		private readonly NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private readonly NewProjectSettingsPage _newProjectSettingsPage;
		private readonly NewProjectEditGlossaryDialog _newProjectEditGlossaryDialog;
		private readonly NewProjectWorkflowPage _newProjectWorkflowPage;
	}
}
