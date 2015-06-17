using System.IO;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectSettingsHelper : WorkspaceHelper
	{
		public ProjectSettingsHelper ClickDocumentProgress(string filePath)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickDocumentProgress(Path.GetFileNameWithoutExtension(filePath));

			return this;
		}

		public TaskAssignmentDialogHelper ClickAssignButtonInDocumentInfo()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickAssignButtonInDocumentInfo();

			return new TaskAssignmentDialogHelper();
		}

		public ProjectSettingsHelper SelectDocument(string filePath)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickProjectsTableCheckbox(Path.GetFileNameWithoutExtension(filePath));

			return this;
		}

		/// <summary>
		/// Открыть документ в редакторе
		/// </summary>
		/// <param name="filePath">путь до документа</param>
		public EditorHelper OpenDocument<T>(string filePath) where T : class , IAbstractPage<T>, new()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage
				.AssertIsDocumentProcessed()
				.ClickDocument<T>(Path.GetFileNameWithoutExtension(filePath));

			return new EditorHelper();
		}

		/// <summary>
		/// Выбрать движок МТ компрено
		/// </summary>
		public ProjectSettingsHelper SelectDefaultMT()
		{
			BaseObject.InitPage(_projectPage);
			if (!_projectPage.IsDefaultMTSelected())
			{
				_projectPage.ClickDefaultMTCheckbox()
					.ClickSaveMtButton();
			}

			return this;
		}

		/// <summary>
		/// Загрузить файл в проект
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public ProjectSettingsHelper UploadDocument(string filePath)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage
				.ClickDocumentUploadButton()
				.UploadDocument(filePath)
				.AssertFileUploaded(Path.GetFileName(filePath))
				.ClickFinish<ProjectSettingsPage>()
				.WaitUntilUploadDocumentDialogDissapeared();

			return this;
		}

		public UploadDocumentHelper ClickDocumentUploadButton()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickDocumentUploadButton();

			return new UploadDocumentHelper();
		}

		/// <summary>
		/// Проверить, переведен ли документ
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public ProjectSettingsHelper AssertIsDocumentTranslated(string documentName)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.AssertIsStatusCompleted(documentName);

			return this;
		}

		/// <summary>
		/// Выйти из смартката
		/// </summary>
		public LoginHelper LogOff()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage
				.ClickAccount()
				.ClickLogOff();

			return new LoginHelper();;
		}

		/// <summary>
		/// Удалить документ из проекта
		/// </summary>
		/// <param name="fileName">имя документа</param>
		public ProjectSettingsHelper DeleteDocument(string documentName)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage
				.ClickProjectsTableCheckbox(Path.GetFileNameWithoutExtension(documentName))
				.ClickDeleteButton()
				.ConfirmDelete()
				.WaitDeleteDocumentDialogDissappeared()
				.AssertDocumentNotExist(Path.GetFileNameWithoutExtension(documentName));

			return this;
		}

		public ProjectSettingsHelper OpenWorkflowSettings()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage
				.ClickSettingsButton()
				.ClickWorkflowTab();

			return this;
		}

		public ProjectSettingsHelper ClickDeleteTaskButton(int taskNumber = 1)
		{
			BaseObject.InitPage(_settingsDialog);
			_settingsDialog.ClickDeleteTaskButton(taskNumber);

			return this;
		}

		public ProjectSettingsHelper AssertConfirmDeleteDialogDisplay()
		{
			BaseObject.InitPage(_settingsDialog);
			_settingsDialog.AssertConfirmDeleteDialogDislpay();

			return this;
		}

		public ProjectSettingsHelper AcceptAllTasks(string filePath)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.AcceptAllTasksForDocument(Path.GetFileNameWithoutExtension(filePath));

			return this;
		}

		public ProjectSettingsHelper RefreshPage()
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.RefreshPage();

			return this;
		}

		private readonly ProjectSettingsPage _projectPage = new ProjectSettingsPage();
		private readonly SettingsDialog _settingsDialog = new SettingsDialog();
	}
}
