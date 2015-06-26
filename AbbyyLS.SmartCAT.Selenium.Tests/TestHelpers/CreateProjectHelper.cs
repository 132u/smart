using System;
using System.IO;
using System.Threading;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class CreateProjectHelper : ProjectsHelper
	{
		public ProjectsHelper CreateNewProject(
			string projectName,
			string filePath = null,
			bool createNewTm = false,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian)
		{
			ClickCreateProjectButton();
			FillGeneralProjectInformation(projectName, filePath, sourceLanguage, targetLanguage);
			ClickNextOnGeneralProjectInformationPage();

			BaseObject.InitPage(_newProjectSetUpWorkflowDialog);
			_newProjectSetUpTMDialog = _newProjectSetUpWorkflowDialog.ClickNextButton();

			if (createNewTm)
			{
				_newProjectSetUpTMDialog
					.ClickCreateTMButton()
					.SetNewTMName("TM_" + Guid.NewGuid())
					.ClickSaveButton();
			}

			// TODO: Без Sleep тесты падают из-за невозможности нажать на кнопку "ГОТОВО". Необходимо разобраться с проблемой
			Thread.Sleep(3000);

			BaseObject.InitPage(_newProjectSetUpTMDialog);
			_newProjectSetUpTMDialog
				.AssertFinishButtonEnabled()
				.ClickFinishButton();

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
			string date = null)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
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

			return this;
		}

		public CreateProjectHelper FillProjectName(string projectName)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
			_newProjectGeneralInformationDialog.SetProjectName(projectName);
			
			return this;
		}

		public CreateProjectHelper ClickNextOnWorkflowPage()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog);
			_newProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			return this;
		}

		public CreateProjectHelper ClickNextOnGeneralProjectInformationPage(bool errorExpected = false)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog);

			if (!errorExpected)
			{
				_newProjectGeneralInformationDialog.ClickNext<NewProjectSetUpWorkflowDialog>();
			}
			else
			{
				_newProjectGeneralInformationDialog.ClickNext<NewProjectGeneralInformationDialog>();
			}

			return this;
		}

		public CreateProjectHelper ClickFinishOnProjectSetUpWorkflowDialog()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog);
			_newProjectSetUpWorkflowDialog.ClickFinishButton();

			return this;
		}

		public CreateProjectHelper ClickBackButtonOnWorkflowStep()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog);
			_newProjectSetUpWorkflowDialog.ClickBack();

			return this;
		}

		/// <summary>
		/// Проверить, что есть ошибка о повторяющемся имени проекта
		/// </summary>
		public CreateProjectHelper AssertErrorDuplicateName()
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
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
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
			_newProjectGeneralInformationDialog.AssertDuplicateLanguageError();

			return this;
		}

		/// <summary>
		/// Проверить, что есть ошибка о недопустимых символах в имени проекта
		/// </summary>
		public CreateProjectHelper AssertErrorForbiddenSymbols()
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
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
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
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
			BaseObject.InitPage(_newProjectCreateBaseDialog);
			_newProjectCreateBaseDialog
				.ClickCloseDialog()
				.WaitCreateProjectDialogDissapear();

			return this;
		}

		/// <summary>
		/// Метод добавляет файл в диалоге создания проекта
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public CreateProjectHelper AddFileFromWizard(string filePath)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
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
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
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
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
			_newProjectGeneralInformationDialog.AssertProjectNameMatch(expectedProjectName);

			return this;
		}
		
		public CreateProjectHelper AssertTranslationMemoryExist(string translationMemoryName)
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog);
			_newProjectSetUpTMDialog.AssertTranslationMemoryExist(translationMemoryName);

			return this;
		}

		public string GetProjectUniqueName()
		{
			// Sleep необходим, чтобы имена были уникальными, когда создаём несколько имён подряд. Чтобы не вышло, что кол-во тиков одинаковое.
			Thread.Sleep(10);
			return "Test Project" + "_" + DateTime.UtcNow.Ticks;
		}

		public CreateProjectHelper AssertErrorDeadlineDate(string dateFormat)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
			_newProjectGeneralInformationDialog.AssertErrorDeadlineDate(dateFormat);

			return this;
		}

		public ProjectsHelper WaitCreateProjectDialogDisappear()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog);
			_newProjectSetUpWorkflowDialog.WaitCreateProjectDialogDissapear();

			return new ProjectsHelper();
		}

		public CreateProjectHelper ClickNewTaskButton()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog);
			_newProjectSetUpWorkflowDialog.ClickNewTaskButton();
			
			return this;
		}

		private NewProjectSetUpTMDialog _newProjectSetUpTMDialog = new NewProjectSetUpTMDialog();

		private readonly NewProjectCreateBaseDialog _newProjectCreateBaseDialog = new NewProjectCreateBaseDialog();
		private readonly NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog();
		private readonly NewProjectSetUpWorkflowDialog _newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog();
	}
}
