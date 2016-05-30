using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ViewProjectTests<TWebDriverProvider> : ViewProjectBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { PathProvider.DocumentFile, PathProvider.DocumentFile2 });

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.DocumentFile);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.DocumentFile2);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName)
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

			_projectsPage.ClickDocumentRefExpectingEditorPage(_projectUniqueName, PathProvider.DocumentFile);
			
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
				.ClickDocumentRow(_projectUniqueName, PathProvider.DocumentFile);

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(PathProvider.DocumentFile2),
				"Произошла ошибка:\n Задача перевода не отображается для текущего пользователя.");

			_projectsPage.ClickDeclineButton();

			_confirmDeclineTaskDialog.ClickDeclineButtonExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, PathProvider.DocumentFile)
				.ClickDocumentRow(_projectUniqueName, PathProvider.DocumentFile);

			Assert.IsFalse(_projectsPage.IsMyTaskDisplayed(PathProvider.DocumentFile),
				"Произошла ошибка:\n Задача перевода отображается для текущего пользователя.");
		}

		[Test]
		public void TwoTasksTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, PathProvider.DocumentFile)
				.ClickDocumentRow(_projectUniqueName, PathProvider.DocumentFile);

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(PathProvider.DocumentFile),
				"Произошла ошибка:\n Задача перевода не отображается для текущего пользователя.");

			_projectsPage
				.ClickDocumentRow(_projectUniqueName, PathProvider.DocumentFile)
				.HoverDocumentRow(_projectUniqueName, PathProvider.DocumentFile2)
				.ClickDocumentRow(_projectUniqueName, PathProvider.DocumentFile2);

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(PathProvider.DocumentFile2),
				"Произошла ошибка:\n Задача перевода отображается для текущего пользователя.");
		}
	}
}
