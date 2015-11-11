using System;

using OpenQA.Selenium;

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
			_newProjectCreateBaseDialog = new NewProjectCreateBaseDialog(Driver);
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
			_newProjectSelectGlossariesDialog = new NewProjectSelectGlossariesDialog(Driver);
			_newProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);
			_newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog(Driver);
			_newProjectCreateTMDialog = new NewProjectCreateTMDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);
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
			bool personalAccount = false)
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectGeneralInformationDialog
				.FillGeneralProjectInformation(projectName, filePath, sourceLanguage, targetLanguage, useMT: useMachineTranslation)
				.ClickNextButton<NewProjectSetUpTMDialog>();

			if (!personalAccount)
			{
				_newProjectSetUpWorkflowDialog.ClickNextButton();
			}

			if (createNewTm)
			{
				var translationMemoryName = "TM_" + Guid.NewGuid();

				if (!string.IsNullOrEmpty(tmxFilePath))
				{
					_newProjectSetUpTMDialog
						.ClickUploadTMButton()
						.UploadTmxFile(tmxFilePath);
				}
				else
				{
					_newProjectSetUpTMDialog.ClickCreateTMButton();
				}

				_newProjectCreateTMDialog
					.SetNewTMName(translationMemoryName)
					.ClickSaveButton();

				if (!_newProjectSetUpTMDialog.IsNewProjectCreateTMDialogDisappeared())
				{
					throw new InvalidElementStateException("Произошла ошибка:\n диалог создания TM не закрылся");
				}

				if (!_newProjectSetUpTMDialog.IsTranslationMemoryExist(translationMemoryName))
				{
					throw new XPathLookupException("Произошла ошибка: \nTM не существует");
				}
			}

			if (createGlossary)
			{
				_newProjectSetUpTMDialog.ClickNextButton<NewProjectSelectGlossariesDialog>();

				_newProjectSelectGlossariesDialog
					.ClickCreateGlossary()
					.SetGlossaryName(glossaryName)
					.ClickSaveButton();
			}

			if (!_newProjectCreateBaseDialog.IsFinishButtonEnabled())
			{
				throw new InvalidElementStateException("Ошибка: \n кнопка 'Готово' недоступна");
			}

			_newProjectCreateBaseDialog
				.ClickFinishButton()
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			_projectsPage.WaitUntilProjectLoadSuccessfully(projectName);

			return this;
		}

		public string GetProjectUniqueName()
		{
			return "Test Project" + "-" + Guid.NewGuid();
		}

		private readonly NewProjectCreateBaseDialog _newProjectCreateBaseDialog;
		private readonly NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
		private readonly NewProjectSetUpWorkflowDialog _newProjectSetUpWorkflowDialog;
		private readonly NewProjectSetUpTMDialog _newProjectSetUpTMDialog;
		private readonly NewProjectSelectGlossariesDialog _newProjectSelectGlossariesDialog;
		private readonly NewProjectCreateTMDialog _newProjectCreateTMDialog;
		private readonly ProjectsPage _projectsPage;
	}
}
