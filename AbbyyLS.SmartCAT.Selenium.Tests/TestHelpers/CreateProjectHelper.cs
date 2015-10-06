using System;
using System.IO;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class CreateProjectHelper : ProjectsHelper
	{
		public CreateProjectHelper(WebDriver driver) : base(driver)
		{
			_newProjectCreateBaseDialog = new NewProjectCreateBaseDialog(Driver);
			_newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
			_newProjectSelectGlossariesDialog = new NewProjectSelectGlossariesDialog(Driver);
			_newProjectSetUpPretranslationDialog = new NewProjectSetUpPretranslationDialog(Driver);
			_newProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);
			_newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog(Driver);
		}

		public ProjectsHelper CreateNewProject(
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
			ClickCreateProjectButton();
			FillGeneralProjectInformation(projectName, filePath, sourceLanguage, targetLanguage, useMT: useMachineTranslation);
			ClickNextOnGeneralProjectInformationPage(personalAccount: personalAccount);

			if (!personalAccount)
			{
				BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
				_newProjectSetUpTMDialog = _newProjectSetUpWorkflowDialog.ClickNextButton();
			}

			if (createNewTm)
			{
				BaseObject.InitPage(_newProjectSetUpTMDialog, Driver);
				var translationMemoryName = "TM_" + Guid.NewGuid();

				if (!string.IsNullOrEmpty(tmxFilePath))
				{
					_newProjectSetUpTMDialog
						.ClickUploadTMButton()
						.UploadTmxFile(tmxFilePath)
						.SetNewTMName(translationMemoryName)
						.ClickSaveButton()
						.AssertNewProjectCreateTMDialogDisappeared()
						.AssertTranslationMemoryExist(translationMemoryName);
				}
				else
				{
					_newProjectSetUpTMDialog
						.ClickCreateTMButton()
						.SetNewTMName(translationMemoryName)
						.ClickSaveButton()
						.AssertNewProjectCreateTMDialogDisappeared()
						.AssertTranslationMemoryExist(translationMemoryName);
				}
			}

			if (createGlossary)
			{
				var glossaryUniqueName = glossaryName ?? GlossariesHelper.UniqueGlossaryName();
				//Sleep нужен, иначе кнопка Next некликабельна
				Thread.Sleep(1000);
				_newProjectSetUpTMDialog.ClickNextButton<NewProjectSelectGlossariesDialog>(Driver);
				CreateGllosary(glossaryUniqueName);
			}

			// TODO: Без Sleep тесты падают из-за невозможности нажать на кнопку "ГОТОВО". Необходимо разобраться с проблемой
			Thread.Sleep(3000);

			_newProjectCreateBaseDialog
				.AssertFinishButtonEnabled()
				.ClickFinishButton()
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			return this;
		}

		/// <summary>
		/// Заполняем основную информацию о проекте (1 шаг создания)
		/// </summary>
		public CreateProjectHelper FillGeneralProjectInformation( 
			string projectName,
			string filePath = null,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			Deadline deadline = Deadline.CurrentDate,
			string date = null,
			bool useMT = false)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			if (filePath != null)
			{
				_newProjectGeneralInformationDialog
					.UploadFile(filePath)
					.AssertFileUploaded(Path.GetFileNameWithoutExtension(filePath));
			}

			_newProjectGeneralInformationDialog
				.SetDeadline(deadline, date)
				.ClickSourceLangDropdown()
				.SelectSourceLanguage(sourceLanguage)
				.ClickTargetMultiselect()
				.DeselectAllTargetLanguages()
				.SelectTargetLanguage(targetLanguage)
				.ClickTargetMultiselect()
				.SetProjectName(projectName);

			if (useMT ^ _newProjectGeneralInformationDialog.GetIsMachineTranslationCheckboxSelected())
			{
				_newProjectGeneralInformationDialog.ClickMachineTranslationCheckbox();
			}

			return this;
		}

		public CreateProjectHelper UploadFile(string filePath)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.UploadFile(filePath);

			return this;
		}

		public CreateProjectHelper SelectFirstGlossary()
		{
			BaseObject.InitPage(_newProjectSelectGlossariesDialog, Driver);
			_newProjectSelectGlossariesDialog.ClickFirstGlossary();

			return this;
		}

		public CreateProjectHelper CreateGllosary(string glossaryName)
		{
			BaseObject.InitPage(_newProjectSelectGlossariesDialog, Driver);
			_newProjectSelectGlossariesDialog
				.ClickCreateGlossary()
				.SetGlossaryName(glossaryName)
				.ClickSaveButton();

			return this;
		}

		public CreateProjectHelper AssertErrorFormatDocument()
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.AssertErrorFormatDocument();

			return this;
		}

		public CreateProjectHelper AssertNoErrorFormatDocument()
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.AssertNoErrorFormatDocument();

			return this;
		}

		public CreateProjectHelper FillProjectName(string projectName)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.SetProjectName(projectName);
			
			return this;
		}

		public CreateProjectHelper ClickNextOnWorkflowPage(bool errorExpected = false)
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			if (!errorExpected)
			{
				_newProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>(Driver);
			}
			else
			{
				_newProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpWorkflowDialog>(Driver);
			}

			return this;
		}

		public CreateProjectHelper ClickNextOnSelectGlossaryStep()
		{
			BaseObject.InitPage(_newProjectSelectGlossariesDialog, Driver);
			_newProjectSelectGlossariesDialog.ClickNextButton<NewProjectSetUpPretranslationDialog>(Driver);

			return this;
		}

		public CreateProjectHelper ClickNextOnGeneralProjectInformationPage(bool errorExpected = false, bool personalAccount = false)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);

			if (!errorExpected)
			{
				if (!personalAccount)
				{
					_newProjectGeneralInformationDialog.ClickNext<NewProjectSetUpWorkflowDialog>(Driver);
				}
				else
				{
					_newProjectGeneralInformationDialog.ClickNext<NewProjectSetUpTMDialog>(Driver);
				}
			}
			else
			{
				_newProjectGeneralInformationDialog.ClickNext<NewProjectGeneralInformationDialog>(Driver);
			}

			return this;
		}

		public CreateProjectHelper ClickFinishOnProjectSetUpWorkflowDialog()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			_newProjectSetUpWorkflowDialog
				.ClickFinishButton()
				.WaitCreateProjectDialogDisappear()
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			return this;
		}

		public CreateProjectHelper ClickBackButtonOnWorkflowStep()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			_newProjectSetUpWorkflowDialog.ClickBackButton<NewProjectGeneralInformationDialog>(Driver);

			return this;
		}

		public CreateProjectHelper ClickBackButtonOnGlossaryStep()
		{
			BaseObject.InitPage(_newProjectSelectGlossariesDialog, Driver);
			_newProjectSelectGlossariesDialog.ClickBackButton<NewProjectSetUpTMDialog>(Driver);

			return this;
		}

		public CreateProjectHelper ClickBackButtonOnPreranslationStep()
		{
			BaseObject.InitPage(_newProjectSetUpPretranslationDialog, Driver);
			_newProjectSetUpPretranslationDialog.ClickBackButton<NewProjectSelectGlossariesDialog>(Driver);

			return this;
		}

		public CreateProjectHelper ClickBackButtonOnTMStep()
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog, Driver);
			_newProjectSetUpTMDialog.ClickBackButton<NewProjectSetUpWorkflowDialog>(Driver);

			return this;
		}

		public CreateProjectHelper AssertWorkflowTaskCountMatch(int taskCount)
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			CustomTestContext.WriteLine("Проверить, что количество задач на этапе Workflow = {0}", taskCount);

			Assert.AreEqual(taskCount, _newProjectSetUpWorkflowDialog.WorkflowTaskList().Count,
				"Произошла ошибка:\n неверное количество задач.");

			return this;
		}

		public CreateProjectHelper AssertWorkflowTaskMatch(WorkflowTask task, int taskNumber)
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			CustomTestContext.WriteLine("Проверить, что задача №{0} на этапе Workflow - это {1}.", taskNumber, task);

			Assert.AreEqual(task.ToString(), _newProjectSetUpWorkflowDialog.WorkflowTaskList()[taskNumber - 1],
				"Произошла ошибка:\n задача не соответствует.");

			return this;
		}

		/// <summary>
		/// Проверить, что есть ошибка о повторяющемся имени проекта
		/// </summary>
		public CreateProjectHelper AssertErrorDuplicateName()
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog
				.AssertErrorDuplicateName()
				.AssertNameInputError();

			return this;
		}

		/// <summary>
		/// Проверить, что есть ошибка о совпадении source и target языков
		/// </summary>
		public CreateProjectHelper AssertErrorDuplicateLanguage()
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.AssertDuplicateLanguageError();

			return this;
		}

		/// <summary>
		/// Проверить, что есть ошибка о недопустимых символах в имени проекта
		/// </summary>
		public CreateProjectHelper AssertErrorForbiddenSymbols()
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog
				.AssertErrorForbiddenSymbols()
				.AssertNameInputError();

			return this;
		}

		/// <summary>
		/// Провреить, что есть ошибка о пустом имени проекта
		/// </summary>
		public CreateProjectHelper AssertErrorNoName()
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog
				.AssertErrorNoName()
				.AssertNameInputError();

			return this;
		}

		/// <summary>
		/// Отменить создание проекта
		/// </summary>
		public ProjectsHelper CancelCreateProject()
		{
			BaseObject.InitPage(_newProjectCreateBaseDialog, Driver);
			_newProjectCreateBaseDialog
				.ClickCloseDialog()
				.WaitCreateProjectDialogDisappear()
				.AssertDialogBackgroundDisappeared<ProjectsPage>(Driver);

			return this;
		}

		/// <summary>
		/// Метод добавляет файл в диалоге создания проекта
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public CreateProjectHelper AddFileFromWizard(string filePath)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog
				.UploadFile(filePath)
				.AssertFileUploaded(Path.GetFileName(filePath));

			return this;
		}

		/// <summary>
		/// Метод удаляет файл в диалоге создания проекта
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public CreateProjectHelper DeleteFileFromWizard(string filePath)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog
				.ClickDeleteFile(Path.GetFileName(filePath))
				.AssertFileDeleted(Path.GetFileName(filePath));

			return this;
		}

		/// <summary>
		/// Проверить, что имя проекта совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedProjectName">ожидаемое имя проекта</param>
		public CreateProjectHelper AssertProjectNameMatch(string expectedProjectName)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.AssertProjectNameMatch(expectedProjectName);

			return this;
		}
		
		public CreateProjectHelper AssertTranslationMemoryExist(string translationMemoryName)
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog, Driver);
			_newProjectSetUpTMDialog.AssertTranslationMemoryExist(translationMemoryName);

			return this;
		}

		public CreateProjectHelper AssertTranslationMemoryNotExist(string translationMemoryName)
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog, Driver);
			_newProjectSetUpTMDialog.AssertTranslationMemoryNotExist(translationMemoryName);

			return this;
		}

		public string GetProjectUniqueName()
		{
			return "Test Project" + "-" + Guid.NewGuid();
		}

		public CreateProjectHelper AssertErrorDeadlineDate(string dateFormat)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.AssertErrorDeadlineDate(dateFormat);

			return this;
		}

		public CreateProjectHelper ClickNewTaskButton()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			_newProjectSetUpWorkflowDialog.ClickNewTaskButton();
			
			return this;
		}

		public CreateProjectHelper SelectWorkflowTask(WorkflowTask task, int taskNumber = 1)
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			_newProjectSetUpWorkflowDialog
				.ExpandWorkflowDropdown(taskNumber)
				.SelectTask(task);

			return this;
		}

		public CreateProjectHelper DeleteWorkflowTask(int taskNumber = 1)
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			_newProjectSetUpWorkflowDialog.ClickDeleteButton(taskNumber);

			return this;
		}

		public CreateProjectHelper SelectFirstTM()
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog, Driver);
			_newProjectSetUpTMDialog.ClickFirstTMRow();
			
			return this;
		}

		public CreateProjectHelper AssertEmptyWorkflowErrorDisplayed()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			_newProjectSetUpWorkflowDialog.AssertEmptyWorkflowErrorDisplayed();

			return this;
		}

		public CreateProjectHelper ExpandWorkflowDropdown(int taskNumber)
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);
			_newProjectSetUpWorkflowDialog.ExpandWorkflowDropdown(taskNumber);

			return this;
		}

		public CreateProjectHelper AssertTaskOptionsCountMatch(int taskNumber, int count)
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog, Driver);

			Assert.AreEqual(count, _newProjectSetUpWorkflowDialog.TaskOptionsList(taskNumber).Count,
				"Произошла ошибка:\n неверное количество задач в дропдауне на этапе Workflow.");

			return this;
		}


		public CreateProjectHelper AssertTargetLanguageMatch(Language language)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.AssertTargetLanguageMatch(language);

			return this;
		}

		public CreateProjectHelper AssertSourceLanguageMatch(Language language)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.AssertSourceLanguageMatch(language);

			return this;
		}

		public CreateProjectHelper AssertDeadlineDateMatch(string deadlineDate)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog, Driver);
			_newProjectGeneralInformationDialog.AssertDeadlineDateMatch(deadlineDate);

			return this;
		}

		public CreateProjectHelper AssertFirstTMSelected()
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog, Driver);
			_newProjectSetUpTMDialog.AssertFirstTMSelected();

			return this;
		}

		public CreateProjectHelper ClickNextButtonOnTMStep()
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog, Driver);
			_newProjectSetUpTMDialog.ClickNextButton<NewProjectSelectGlossariesDialog>(Driver);

			return this;
		}

		public CreateProjectHelper AssertFirstGlossarySelected()
		{
			BaseObject.InitPage(_newProjectSelectGlossariesDialog, Driver);
			_newProjectSelectGlossariesDialog.AssertFirstGlossarySelected();

			return this;
		}

		private readonly NewProjectCreateBaseDialog _newProjectCreateBaseDialog;
		private readonly NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog;
		private readonly NewProjectSetUpWorkflowDialog _newProjectSetUpWorkflowDialog;
		private NewProjectSetUpTMDialog _newProjectSetUpTMDialog;
		private readonly NewProjectSelectGlossariesDialog _newProjectSelectGlossariesDialog;
		private readonly NewProjectSetUpPretranslationDialog _newProjectSetUpPretranslationDialog;
	}
}
