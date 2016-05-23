using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Projects]
	class WorkflowAssigneeTests<TWebDriverProvider> : WorkflowBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void WorkflowAssigneeTestsSetUp()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.RepetitionsTxtFile });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);
		}

		[Test, Description("ТС-0511 Отказ после подтверждения")]
		public void DeclineTaskTest()
		{
			_projectSettingsHelper.AssignTasksOnDocument(PathProvider.RepetitionsTxtFile, ThreadUser.NickName, _projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.RepetitionsTxtFile);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillTarget(_text, rowNumber: 1)
				.ConfirmSegmentTranslation()
				.FillTarget(_text, rowNumber: 2)
				.ConfirmSegmentTranslation();

			Assert.AreNotEqual(0, _editorPage.GetPercentInProgressBar(),
				"Произошла ошибка:\n Документ не находится в процессе перевода.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToProjectsPage();

			_projectsPage.DeclineTaskInDocumentInfo(_projectUniqueName, PathProvider.RepetitionsTxtFile);

			_taskDeclineDialog.ClickDeclineButton();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName, PathProvider.RepetitionsTxtFile);

			Assert.AreEqual(0, _projectsPage.GetTasksCount(),
				"Произошла ошибка:\n Неверное количество задач.");
		}

		[Test, Description("ТС-053 Отмена участия исполнителя назначенного этапа")]
		public void DeclineAssigneeTest()
		{
			_projectSettingsHelper.AssignTasksOnDocument(PathProvider.RepetitionsTxtFile, ThreadUser.NickName, _projectUniqueName);

			_workspacePage
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName, PathProvider.RepetitionsTxtFile);

			_taskAssignmentPage
				.ClickCancelAssignButton(ThreadUser.NickName)
				.ClickSaveButton();

			_workspacePage
				.GoToProjectsPage()
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName, PathProvider.RepetitionsTxtFile);

			Assert.AreEqual(0, _projectsPage.GetTasksCount(),
				"Произошла ошибка:\n Неверное количество задач.");
		}

		[Test, Description("ТС-51 Работа с репетишенами"), Ignore("PRX-16792")]
		public void AssignRepetionsTest()
		{
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.NickName);

			var text2 = "Translation 2";
			var startRange = 1;
			var endRange = 6;

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.RepetitionsTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.SelectDistributeDocumentAssignmentType();

			_distributeDocumentBetweenAssigneesPage.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(startRange, endRange)
				.ClickSaveButton();

			_distributeDocumentBetweenAssigneesPage
				.SelectAssignee(AdditionalUser.NickName)
				.ClickSelectSegmentsAndAssignLink(assigneeNumber: 2);

			_distributeSegmentsBetweenAssigneesPage.SelectSegmentsRange(endRange + 1 , 12);
			
			_distributeSegmentsBetweenAssigneesPage
				.ClickAssignButton()
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName, PathProvider.RepetitionsTxtFile)
				.ClickDocumentRefExpectingSelectTaskDialog(PathProvider.RepetitionsTxtFile);
				
			_selectTaskDialog.SelectTask();

			_editorPage
				.FillTarget(_text)
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(),
				"Произошла ошибка:\n Сегмент №1не подтвердился");
			//Sleep необходим, так как репетишены подставляются с задержкой (1 секунды не хватает)
			Thread.Sleep(2000);

			Assert.AreEqual(_text, _editorPage.GetTargetText(rowNumber: 4),
				"Произошла ошибка:\n Репетишен не подставился в сегмент №4.");

			Assert.AreEqual(_text, _editorPage.GetTargetText(rowNumber: 7),
				"Произошла ошибка:\n Репетишен не подставился в сегмент №7.");

			Assert.AreEqual(_text, _editorPage.GetTargetText(rowNumber: 10),
				"Произошла ошибка:\n Репетишен не подставился в сегмент №10.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName, PathProvider.RepetitionsTxtFile)
				.ClickDocumentRefExpectingEditorPage(PathProvider.RepetitionsTxtFile);

			_editorPage
				.ClickOnTargetCellInSegment(rowNumber: 9)
				.FillSegmentTargetField(text2, rowNumber: 8)
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 8),
				"Произошла ошибка:\n Сегмент №8 не подтвердился");

			Assert.IsTrue(_editorPage.IsAllSegmentsSavedMessageDisplayed(), "Произошла ошибка:\nТермин не сохранился.");
			//Sleep необходим, так как репетишены подставляются с задержкой (1 секунды не хватает)
			Thread.Sleep(2000);

			Assert.AreEqual(text2, _editorPage.GetTargetText(rowNumber: 11),
				"Произошла ошибка:\n Репетишен не подставился в сегмент №11.");

			Assert.AreEqual(text2, _editorPage.GetTargetText(rowNumber: 5),
				"Произошла ошибка:\n Репетишен не подставился в сегмент №5.");

			Assert.AreEqual(text2, _editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка:\n Репетишен не подставился в сегмент №2.");
		}
	}
}
