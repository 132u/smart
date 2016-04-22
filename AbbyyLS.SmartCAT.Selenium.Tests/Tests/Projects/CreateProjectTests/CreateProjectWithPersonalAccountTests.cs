using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	class CreateProjectWithPersonalAccountTests<TWebDriverProvider>
		: BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public CreateProjectWithPersonalAccountTests()
		{
			StartPage = StartPage.PersonalAccount;
		}

		[Test, Description("S-7168")]
		public void CreateProjectNoFileTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName, personalAccount: true);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", projectUniqueName);
		}

		[Test]
		public void CreateProjectDuplicateNameTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName, personalAccount: true);

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage.FillGeneralProjectInformation(projectUniqueName);

			Assert.IsTrue(_newProjectSettingsPage.IsDuplicateNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о существующем имени");

			Assert.IsTrue(_newProjectSettingsPage.IsNameInputValidationMarkerDisplayed(),
				"Произошла ошибка:\n поле 'Название' не отмечено ошибкой");
		}

		[Test]
		public void CreateProjectLongNameTest()
		{
			var longProjectUniqueName = _createProjectHelper.GetProjectUniqueName() + _longName;

			_createProjectHelper.CreateNewProject(longProjectUniqueName, personalAccount: true);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(longProjectUniqueName.Substring(0, 100)),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", longProjectUniqueName.Substring(0, 100));
		}

		[Test]
		public void AssignTaskButtonTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName, personalAccount: true);

			_projectsPage.ClickProject(projectUniqueName);

			_projectSettingsHelper.UploadDocument(new[] { PathProvider.DocumentFile });

			_projectSettingsPage
				.ClickDocumentProgress(Path.GetFileName(PathProvider.DocumentFile));

			Assert.IsFalse(_projectSettingsPage.IsAssignButtonExist(),
				"Произошла ошибка:\n кнопка 'Назначить задачу' отображается в открытой свёртке документа");
		}

		[Test]
		public void QuitCreateProjectOnFirstStep()
		{
			_projectsPage.ClickCreateProjectButton();

			_workspacePage.ClickProjectsSubmenuExpectingAlert();

			Assert.IsTrue(_workspacePage.IsAlertExist(),
				"Произошла ошибка: \n Не появился алере о несохраненных данных.");

			_workspacePage.AcceptAlert<ProjectsPage>();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(),
				"Произошла ошибка: \n страница со списком проектов не открылась.");
		}

		[Test]
		public void DeleteDocumentFromProjectTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName, personalAccount: true);

			_projectsPage.ClickProject(projectUniqueName);

			_projectSettingsHelper
				.UploadDocument(new[] { PathProvider.DocumentFile })
				.DeleteDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}
	}
}
