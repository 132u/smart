using System;
using System.IO;
using System.Threading;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class CreateProjectHelper : ProjectsHelper
	{

		/// <summary>
		/// Заполняем основную информацию о проекте (1 шаг создания)
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="filePath">путь к файлу</param>
		/// <param name="sourceLanguage">исходный язык</param>
		/// <param name="targetLanguage">язык перевода</param>
		public CreateProjectHelper FillGeneralProjectInformation( 
			string projectName,
			string filePath = null,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian)
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
			if (filePath != null)
			{
				_newProjectGeneralInformationDialog
					.UploadFile(filePath)
					.AssertIfFileUploaded(Path.GetFileNameWithoutExtension(filePath));
			}

			_newProjectGeneralInformationDialog
				.ClickDeadlineDateInput()
				.AssertIsCalendarDisplayed()
				.ClickDeadlineDateCurrent()
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

		public CreateProjectHelper ClickBackButtonOnWorkflowStep()
		{
			BaseObject.InitPage(_newProjectSetUpWorkflowDialog);
			_newProjectSetUpWorkflowDialog.ClickBack();

			return this;
		}

		/// <summary>
		/// Выбрать существующую, либо создаем новую ТМ
		/// </summary>
		/// <param name="newTMName">имя новой ТМ</param>
		public CreateProjectHelper SelectTM(string newTMName)
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog);

			if (_newProjectSetUpTMDialog.IsTMTableEmpty())
			{
				_newProjectSetUpTMDialog
					.ClickTMTableFirstItem()
					.ClickFinishButton();
			}
			else
			{
				_newProjectSetUpTMDialog
					.ClickCreateTMButton()
					.SetNewTMName(newTMName)
					.ClickSaveButton()
					.AssertIsTMTableNotEmpty()
					.ClickFinishButton();
			}

			return this;
		}

		/// <summary>
		/// Создаём новый проект
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="filePath">путь к файлу</param>
		/// <param name="sourceLanguage">исходный язык</param>
		/// <param name="targetLanguage">язык перевода</param>
		public ProjectsHelper CreateNewProject(
			string projectName,
			string filePath = null,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian)
		{
			ClickCreateProjectButton();
			FillGeneralProjectInformation(projectName, filePath, sourceLanguage, targetLanguage);
			ClickNextOnGeneralProjectInformationPage();

			BaseObject.InitPage(_newProjectSetUpWorkflowDialog);
			_newProjectSetUpWorkflowDialog
				.ClickFinishCreate()
				.WaitCreateProjectDialogDissapeared();

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
		/// Открыть даилог создания проекта
		/// </summary>
		public CreateProjectHelper OpenCreateProjectDialog()
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickCreateProjectButton();

			return this;
		}

		/// <summary>
		/// Отменить создание проекта
		/// </summary>
		public ProjectsHelper CancelCreateProject()
		{
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
			_newProjectGeneralInformationDialog
				.ClickCloseDialog()
				.WaitCreateProjectDialogDissapeared();

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
				.AssertIfFileUploaded(Path.GetFileName(filePath));

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

		public string GetProjectUniqueName()
		{
			// Sleep необходим, чтобы имена были уникальными, когда создаём несколько имён подряд. Чтобы не вышло, что кол-во тиков одинаковое.
			Thread.Sleep(10);
			return "Test Project" + "_" + DateTime.UtcNow.Ticks;
		}

		private readonly ProjectsPage _projectsPage = new ProjectsPage();
		private readonly NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog();
		private readonly NewProjectSetUpTMDialog _newProjectSetUpTMDialog = new NewProjectSetUpTMDialog();
		private readonly NewProjectSetUpWorkflowDialog _newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog();
	}
}
