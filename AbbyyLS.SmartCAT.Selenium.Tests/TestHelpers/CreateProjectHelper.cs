using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class CreateProjectHelper
	{
		private readonly ProjectsPage _projectsPage = new ProjectsPage();

		private readonly NewProjectGeneralInformationDialog _newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog();
		private readonly NewProjectSetUpTMDialog _newProjectSetUpTMDialog = new NewProjectSetUpTMDialog();

		/// <summary>
		/// Заполняем основную информацию о проекте (1 шаг создания)
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		/// <param name="fileName">имя файла с расширением</param>
		/// <param name="sourceLanguage">исходный язык</param>
		/// <param name="targetLanguage">язык перевода</param>
		/// <param name="projectName">имя проекта</param>
		/// <param name="noFile">надо ли загружать файл</param>
		public CreateProjectHelper FillGeneralProjectInformation(
			string filePath, 
			string fileName, 
			string sourceLanguage,
			string targetLanguage, 
			string projectName, 
			bool noFile = false)
		{
			BaseObject.InitPage(_projectsPage);
			_projectsPage.ClickCreateProjectBtn();
			if (!noFile)
			{
				_newProjectGeneralInformationDialog.UploadFile(filePath)
					.AssertIfFileUploaded(fileName);
			}
			BaseObject.InitPage(_newProjectGeneralInformationDialog);
			_newProjectGeneralInformationDialog.ClickDeadlineDateInput()
				.AssertIsCalendarDisplayed()
				.ClickDeadlineDateCurrent()
				.ClickSourceLangDropdown()
				.SelectSourceLanguage(sourceLanguage)
				.ClickTargetMultiselect()
				.DeselectAllTargetLanguages()
				.SelectTargetLanguage(targetLanguage)
				.ClickTargetMultiselect()
				.SetProjectName(projectName)
				.ClickNextBtn();

			return this;
		}

		/// <summary>
		/// Выбираем существующую, либо создаем новую ТМ
		/// </summary>
		/// <param name="newTMName">имя новой ТМ</param>
		public CreateProjectHelper SelectTM(string newTMName)
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog);

			if (_newProjectSetUpTMDialog.IsTMTableNotEmpty())
			{
				_newProjectSetUpTMDialog.ClickTMTableFirstItem()
					.ClickFinishBtn();
			}
			else
			{
				_newProjectSetUpTMDialog.ClickCreateTMBtn()
					.SetNewTMName(newTMName)
					.ClickSaveBtn()
					.AssertIsTMTableNotEmpty()
					.ClickFinishBtn();
			}

			return this;
		}

		/// <summary>
		/// Переходим на вкладку проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectSettingsHelper GoToProjectSettings(string projectName)
		{
			BaseObject.InitPage(_newProjectSetUpTMDialog);
			_projectsPage.AssertIsProjectLoaded(projectName)
				.ClickProjectRef(projectName);
			var projectSettingsHelper = new ProjectSettingsHelper();

			return projectSettingsHelper;
		}
	}
}
