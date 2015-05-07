using System.IO;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectSettingsHelper : WorkspaceHelper
	{
		/// <summary>
		/// Назначить ответственного на документ
		/// </summary>
		/// <param name="documentName">имя документа</param>
		/// <param name="userName">имя назначаемого пользователя</param>
		public ProjectSettingsHelper AssignResponsible(string documentName, string userName)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.UncheckAllChecboxesDocumentsTable()
				.ClickProjectsTableCheckbox(documentName)
				.ClickAssignButton()
				.SelectAssignee(userName)
				.ClickAssignButton()
				.AssertIsUserAssigned()
				.ClickCloseButton();

			return this;
		}

		/// <summary>
		/// Открыть документ в редакторе
		/// </summary>
		/// <param name="documentName">имя документа</param>
		public EditorHelper OpenDocument(string documentName)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.AssertIsDocumentProcessed()
				.ClickDocumentRef(documentName);
			var editorHelper = new EditorHelper();

			return editorHelper;
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
			_projectPage.ClickAddFilesButton()
				.UploadFile(filePath)
				.AssertIfFileUploaded(Path.GetFileName(PathProvider.DocumentFile))
				.ClickFinishButton();

			return this;
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
			_projectPage.ClickAccount()
				.ClickLogOffRef();
			var loginHelper = new LoginHelper();

			return loginHelper;
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

		private readonly ProjectSettingsPage _projectPage = new ProjectSettingsPage();
	}
}
