using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.ProjectSettings;
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
				.ClickAssignBtn()
				.SelectAssignee(userName)
				.ClickAssignBtn()
				.AssertIsUserAssigned()
				.ClickCloseBtn();

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
					.ClickSaveMtBtn();
			}

			return this;
		}

		/// <summary>
		/// Загрузить файл в проект
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		/// <param name="fileName">имя файла с расширением</param>
		public ProjectSettingsHelper UploadDocument(string filePath, string fileName)
		{
			BaseObject.InitPage(_projectPage);
			_projectPage.ClickAddFilesBtn()
				.UploadFile(filePath)
				.AssertIfFileUploaded(fileName)
				.ClickFinishBtn();

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

		private readonly ProjectSettingsPage _projectPage = new ProjectSettingsPage();
	}
}
