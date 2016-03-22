using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ViewProjects
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ViewProjectTests<TWebDriverProvider> : ViewProjectBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.DocumentFile);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog
				.UploadDocument(new[] { PathProvider.DocumentFile2 })
				.ClickFihishUploadOnProjectsPage();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false)
				.ClickSaveButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, documentNumber: 2);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false)
				.ClickSaveButton();

			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage, AdditionalUser);
		}

		[Test]
		public void ProjectStatusTest()
		{
			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.AreEqual(ProjectStatus.Created.ToString(), _projectsPage.GetProjectStatusRights(_projectUniqueName),
				"Произошла ошибка:\n Неверный статус проекта {0}.", _projectUniqueName);

			_projectsPage.ClickDocumentRefExpectingEditorPage(PathProvider.DocumentFile);
			
			_editorPage
				.FillTarget("Translation")
				.ConfirmSegmentTranslation()
				.WaitAllSegmentsSavedMessageDisplayed()
				.ClickHomeButtonExpectingProjectsPage();
			
			Assert.AreEqual(ProjectStatus.InProgress.Description(), _projectsPage.GetProjectStatusRights(_projectUniqueName),
				"Произошла ошибка:\n Неверный статус проекта {0}.", _projectUniqueName);
		}

		[Test]
		public void DeclineAssignTaskTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.SelectDocument(_projectUniqueName, PathProvider.DocumentFile);
				
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(_projectUniqueName),
				"Произошла ошибка:\n Задача перевода не отображается для текущего пользователя.");

			_projectsPage.ClickDeclineButton();

			_confirmDeclineTaskDialog.ClickDeclineButtonExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsMyTaskDisplayed(_projectUniqueName),
				"Произошла ошибка:\n Задача перевода отображается для текущего пользователя.");
		}

		[Test]
		public void TwoTasksTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName, documentNumber: 2);

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(_projectUniqueName),
				"Произошла ошибка:\n Задача перевода не отображается для текущего пользователя.");

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(_projectUniqueName, documentNumber: 2),
				"Произошла ошибка:\n Задача перевода отображается для текущего пользователя.");
		}
	}
}
