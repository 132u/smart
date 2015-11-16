using System;
using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class CreateProjectHelper : WorkspaceHelper
	{
		public CreateProjectHelper(WebDriver driver) : base(driver)
		{
			_projectsPage = new ProjectsPage(Driver);

			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
		}

		public CreateProjectHelper CreateNewProject(
			string projectName,
			string glossaryName = null,
			string filePath = null,
			bool createNewTm = false,
			string tmxFilePath = "",
			bool useMachineTranslation = false,
			bool createGlossary = false,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			IEnumerable<WorkflowTask> tasks = null,
			bool personalAccount = false)
		{
			_projectsPage.ClickCreateProjectButton();

			if (filePath == null)
			{
				_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();
			}
			else
			{
				_newProjectDocumentUploadPage.UploadDocument(filePath);

				if (!_newProjectDocumentUploadPage.IsFileUploaded(filePath))
				{
					throw new Exception(string.Format("Произошла ошибка: \n файл c именем {0} не загрузился.", filePath));
				}

				_newProjectDocumentUploadPage.ClickSettingsButton();
			}

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName, sourceLanguage, targetLanguage, useMT: useMachineTranslation)
				.ClickWorkflowButton();

			if (tasks != null)
			{
				_newProjectWorkflowPage.ClickClearButton();

				foreach (var task in tasks)
				{
					_newProjectWorkflowPage.ClickNewTaskButton(task);
				}
			}

			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(projectName);

			return this;
		}

		public string GetProjectUniqueName()
		{
			return "Test Project" + "-" + Guid.NewGuid();
		}

		private readonly ProjectsPage _projectsPage;

		private readonly NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private readonly NewProjectSettingsPage _newProjectSettingsPage;
		private readonly NewProjectWorkflowPage _newProjectWorkflowPage;
	}
}
