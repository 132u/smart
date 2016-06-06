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
			_document1 = PathProvider.DocumentFile;
			_document2 = PathProvider.DocumentFile2;
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { _document1, _document2 });

			_projectsPage.OpenAssignDialog(_projectUniqueName, _document1);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, _document2);

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

			_projectsPage.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _document1);
			
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
				.SelectDocument(_projectUniqueName, _document1);
				
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRow(_projectUniqueName, _document1);

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(_document1),
				"Произошла ошибка:\n Задача перевода не отображается для текущего пользователя.");

			_projectsPage.ClickDeclineButton();

			_confirmDeclineTaskDialog.ClickDeclineButtonExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _document1)
				.ClickDocumentRow(_projectUniqueName, _document1);

			Assert.IsFalse(_projectsPage.IsMyTaskDisplayed(_document1),
				"Произошла ошибка:\n Задача перевода отображается для текущего пользователя.");
		}

		[Test]
		public void TwoTasksTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.HoverDocumentRow(_projectUniqueName, _document1)
				.ClickDocumentRow(_projectUniqueName, _document1);

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(PathProvider.DocumentFile),
				"Произошла ошибка:\n Задача перевода не отображается для текущего пользователя.");

			_projectsPage
				.ClickDocumentRow(_projectUniqueName, _document1)
				.HoverDocumentRow(_projectUniqueName, _document2)
				.ClickDocumentRow(_projectUniqueName, _document2);

			Assert.IsTrue(_projectsPage.IsMyTaskDisplayed(_document2),
				"Произошла ошибка:\n Задача перевода отображается для текущего пользователя.");
		}
	}
}
