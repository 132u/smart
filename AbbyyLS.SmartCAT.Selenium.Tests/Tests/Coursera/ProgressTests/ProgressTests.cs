using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera.CoursePage;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class ProgressTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public override void OneTimeSetUp()
		{
			_completeCourseraProject = "CourseraProject" + Guid.NewGuid();
			_progressCourseraProject = "CourseraProject" + Guid.NewGuid();

			_createProjectHelper = new CreateProjectHelper(Driver);
			_loginHelper = new LoginHelper(Driver);
			_lecturesTab = new LecturesTab(Driver);
			_courseraHomePage = new CourseraHomePage(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_header = new HeaderMenu(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_lecturesTab = new LecturesTab(Driver);
			_deleteTranslationDialog = new DeleteTranslationDialog(Driver);
			_coursesPage = new CoursesPage(Driver);
			_leaderboardPage = new LeaderboardPage(Driver);
			_coursePage = new CoursePage(Driver);
			_profilePage = new UserProfilePage(Driver);
			_editProfileDialog = new EditProfileDialog(Driver);
			_courseraEditorPage = new CourseraEditorPage(Driver);

			CourseraReviewerUser = TakeUser(ConfigurationManager.CourseraReviewerUsers);
			
			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(CourseraReviewerUser.Login, CourseraReviewerUser.Password);

			_courseraHomePage.ClickWorkspaceButton();

			_createProjectHelper.CreateNewProject(
				projectName: _completeCourseraProject,
				filesPaths: PathProvider.GetFilesForCompleteProgressTestsCourseraProject(),
				tasks: new[] { WorkflowTask.CrowdTranslation, WorkflowTask.CrowdReview });
			
			_createProjectHelper.CreateNewProject(
				projectName: _progressCourseraProject,
				filesPaths: PathProvider.GetFilesForProgressTestsCourseraProject(),
				tasks: new[] { WorkflowTask.CrowdTranslation, WorkflowTask.CrowdReview });

			_workspacePage
				.ClickAccount()
				.ClickSignOutExpectingCourseraHomePage();

			ReturnUser(ConfigurationManager.CourseraReviewerUsers, CourseraReviewerUser);
		}
		
		[Test]
		public void LectureCommonAndPersonalProgressTest()
		{
			var lecture = "1.1 Introduction";

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText);

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(2, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение персонального поргресс бара для лекции '{0}'.", lecture);

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего поргресс бара для лекции '{0}'.", lecture);

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText, rowNumber: 2);

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(3, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение персонального поргресс бара для лекции '{0}'.", lecture);

			Assert.AreEqual(3, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего поргресс бара для лекции '{0}'.", lecture);
		}

		[Test, Ignore("PRX-15111")]
		public void TwoUsersSameTranslationLectureProgressTest()
		{
			var lecture = "1.3 Concepts";

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText);
			
			_header.ClickSignOut();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(0, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText);

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(0, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);
		}

		[Test]
		public void DecreaseLecturePersonalAndCommonProgressTest()
		{
			var lecture = "1.4 Theories";

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText);

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(1, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(1, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.DeleteTranslation(_translationText, CourseraCrowdsourceUser.NickName);

			_deleteTranslationDialog.ClickYesButton();

			_courseraEditorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(0, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(0, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);
		}
		
		[Test, Ignore("PRX-17445")]
		public void DecreaseLectureCommonProgressTwoUsersTest()
		{
			var lecture = "1.7 Cultural approach";
			var _translationText2 = "Test" + Guid.NewGuid();

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);
			
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText);
			
			_header
				.GoToUserProfile()
				.ClickSignOut();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_progressCourseraProject);

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText2);

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.DeleteTranslation(_translationText2, _secondUser.NickName);
			
			_deleteTranslationDialog.ClickYesButton();

			_courseraEditorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(0, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);
		}

		[Test, Ignore("Неизвестно, когда происходит перерасчет страниц")]
		public void TotalPagesTest()
		{
			var lecture = "3.8 Negotiated Effects";
			var totalPagesCountBefore = _courseraHomePage.GetTotalPagesCount();

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText);

			_header.GoToHomePage();

			_courseraHomePage.WaitPagesCounterChanged(totalPagesCountBefore);
			
			Assert.Less(totalPagesCountBefore, _courseraHomePage.GetTotalPagesCount(),
				"Произошла ошибка:\n Количество страниц не увеличилось.");
		}

		[Test, Ignore("Неизвестно, когда происходит перерасчет слов")]
		public void TotalWordsTest()
		{
			var lecture = "2.1 A short history of Communication Science";
			var totalWordsCountBefore = _courseraHomePage.GetTotalWordsCount();

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText);

			_header.GoToHomePage();

			_courseraHomePage.WaitWordsCounterChanged(totalWordsCountBefore);
		
			Assert.LessOrEqual(totalWordsCountBefore + 7, _courseraHomePage.GetTotalWordsCount(),
				"Произошла ошибка:\n Количество слов не увеличилось.");
		}
		
		[Test]
		public void PersonalProgressVoteDownBySecondUserTest()
		{
			var lecture = "2.3 Two Schools of Classical Communication Science";

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage.AddTranslationForCourseraProgress(_translationText);

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(3, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			_header
				.ClickSignOut()
				.GoToHomePage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage
				.ClickCourse(_progressCourseraProject)
				.ClickLectureTab();

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header
				.ClickSignOut()
				.GoToHomePage();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);

			_coursesPage
				.GoToCoursesPage()
				.ClickCourse(_progressCourseraProject)
				.ClickLectureTab();

			Assert.AreEqual(3, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);
		}

		[Test]
		public void LectureProgressNegativeAndPositiveTranslations()
		{
			var lecture = "2.4 Rhetorical Theory";

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage
				.FillAndConfirmTarget(_translationText)
				.AddTranslationForCourseraProgress(_translationText +"2");
		
			_header
				.ClickSignOut()
				.GoToHomePage();
			
			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage
				.FillAndConfirmTarget(_translationText + "secondUser")
				.ClickOnTargetAndScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);
		}

		[Test]
		public void LectureProgressMakeTranslationsPositive()
		{
			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);
			_thirdUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			var lecture = "2.5 The Dark Ages of Communication";
			var secondTranslationVersion = _translationText + "2";
			var thirdTranslationVersion = _translationText + "3";
		
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage
				.FillAndConfirmTarget(_translationText)
				.FillAndConfirmTarget(secondTranslationVersion)
				.AddTranslationForCourseraProgress(thirdTranslationVersion);

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage
				.ClickOnTargetAndScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, secondTranslationVersion)
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, thirdTranslationVersion)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_header
				.ClickSignOut()
				.GoToHomePage();
			
			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
					
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage
				.ClickOnTargetAndScrollAndClickVoteUpButton(author: CourseraCrowdsourceUser.NickName, translation: _translationText)
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, secondTranslationVersion)
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, thirdTranslationVersion)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_header
				.ClickSignOut()
				.GoToHomePage();
			
			_loginHelper.LogInCoursera(_thirdUser.Login, _thirdUser.Password);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, secondTranslationVersion)
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, thirdTranslationVersion)
				.ClickHomeButtonExpectingCourseraCoursesPage();
			
			_coursesPage
				.ClickCourse(_progressCourseraProject)
				.ClickLectureTab();

			Assert.AreEqual(2, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);
		}

		[Test]
		public void DecreaseProgressVoteDown()
		{
			var lecture = "2.6 A Renaissance of our field";
			var secondTranslationVersion = _translationText + "2";
			var thirdTranslationVersion = _translationText + "3";

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_progressCourseraProject);
			
			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage
				.FillAndConfirmTarget(_translationText)
				.FillAndConfirmTarget(secondTranslationVersion)
				.AddTranslationForCourseraProgress(thirdTranslationVersion);

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(6, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lecturesTab.OpenLecture(lecture);

			_courseraEditorPage
				.ClickOnTargetAndScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, secondTranslationVersion)
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, thirdTranslationVersion)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_progressCourseraProject);

			Assert.AreEqual(6, _lecturesTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Общий прогресс не уменьшился.");
		}

		[Test]
		public void CompleteCourseTest()
		{
			var lecture1 = "1.1 Introduction";
			var lecture2 = "1.2 What is communication";
			var translationText = "Test" + Guid.NewGuid();

			var courseProgress = _coursesPage.GetCourseProgress(_completeCourseraProject);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage
				.WaitCourseNameDisplayed(_completeCourseraProject)
				.ClickCourse(_completeCourseraProject);

			_lecturesTab.OpenLecture(lecture1);

			_courseraEditorPage
				.FillAndConfirmTarget(translationText)
				.FillAndConfirmTarget(translationText, segmentNumber: 2)
				.AddTranslationForCourseraProgress(translationText, rowNumber: 3);

			_coursesPage.WaitCourseProgressChanged(_completeCourseraProject, courseProgress);

			Assert.AreEqual(58.3, _coursesPage.GetCourseProgress(_completeCourseraProject),
				"Произошла ошибка:\n Неверное значение прогресса курса {0}.", _completeCourseraProject);

			_coursesPage
				.ClickCourse(_completeCourseraProject)
				.ClickLectureTab();

			Assert.AreEqual(100, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture1),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture1);

			Assert.AreEqual(100, _lecturesTab.GetCommonProgressValueByLectureName(lecture1),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture1);

			_lecturesTab.OpenLecture(lecture2);

			_courseraEditorPage
				.FillAndConfirmTarget(translationText, segmentNumber: 1)
				.FillAndConfirmTarget(translationText, segmentNumber: 2)
				.AddTranslationForCourseraProgress(translationText, rowNumber: 3);

			_coursesPage.WaitCourseProgressChanged(_completeCourseraProject, 58.3);

			Assert.AreEqual(100.0, _coursesPage.GetCourseProgress(_completeCourseraProject),
				"Произошла ошибка:\n Неверное значение прогресса курса {0}.", _completeCourseraProject);

			_coursesPage
				.ClickCourse(_completeCourseraProject)
				.ClickLectureTab();

			Assert.AreEqual(100, _lecturesTab.GetPersonalProgressValuebyLectureName(lecture2),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture1);

			Assert.AreEqual(100, _lecturesTab.GetCommonProgressValueByLectureName(lecture2),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture1);
		}

		private string _completeCourseraProject;
		private string _progressCourseraProject;
	}
}
