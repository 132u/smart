using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera.CoursePage;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class ProgressTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void CourseraBaseTestsOneTimeSetUp()
		{
			_loginHelper = new LoginHelper(Driver);
			_lectureTab = new LecturesTab(Driver);
			_courseraHomePage = new CourseraHomePage(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_header = new HeaderMenu(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_lectureTab = new LecturesTab(Driver);
			_deleteTranslationDialog = new DeleteTranslationDialog(Driver);

			CourseraReviewerUser = TakeUser(ConfigurationManager.CourseraReviewerUsers);

			_courseraHomePage.GetPage();

			_loginHelper.LogInCoursera(CourseraReviewerUser.Login, CourseraReviewerUser.Password);

			_courseNameForCompleteCourseTest = "CompleteCourseName" + DateTime.UtcNow.Ticks;

			_courseraHomePage.ClickWorkspaceButtonWithoutWaiting();

			if (!_newProjectDocumentUploadPage.IsNewProjectDocumentUploadPageOpened())
			{
				_projectsPage.ClickCreateProjectButton();
			}

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_courseNameForCompleteCourseTest)
				.ClickNextButton();

			_newProjectWorkflowPage
				.ClickClearButton()
				.ClickNewTaskButton(WorkflowTask.CrowdTranslation)
				.ClickNewTaskButton(WorkflowTask.CrowdReview)
				.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectExist(_courseNameForCompleteCourseTest), "Произошла ошибка:\n Не найден проект '{0}'.", _courseNameForCompleteCourseTest);

			_projectsPage.OpenProjectSettingsPage(_courseNameForCompleteCourseTest);

			_projectSettingsHelper.UploadDocument(PathProvider.GetFilesForCompleteProgressTestsCourseraProject());

			_courseraNameProgressTests = "ProgressCourseName" + DateTime.UtcNow.Ticks;

			_workspacePage.GoToProjectsPage();

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_courseraNameProgressTests)
				.ClickNextButton();

			_newProjectWorkflowPage
				.ClickClearButton()
				.ClickNewTaskButton(WorkflowTask.CrowdTranslation)
				.ClickNewTaskButton(WorkflowTask.CrowdReview)
				.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectExist(_courseraNameProgressTests),
				"Произошла ошибка: \nне найден проект '{0}'.", _courseraNameProgressTests);

			_projectsPage.OpenProjectSettingsPage(_courseraNameProgressTests);

			_projectSettingsHelper.UploadDocument(PathProvider.GetFilesForProgressTestsCourseraProject());

			_workspacePage
				.ClickAccount()
				.ClickSignOutExpectingCourseraHomePage();

			ReturnUser(ConfigurationManager.CourseraReviewerUsers, CourseraReviewerUser);
		}

		[SetUp]
		public void ProgressTestsSetUp()
		{
			_translationText = "Test" + Guid.NewGuid();
			
			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
		}

		[Test]
		public void LectureCommonAndPersonalProgressTest()
		{
			var lecture = "1.1 Introduction";

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(2, _lectureTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение персонального поргресс бара для лекции '{0}'.", lecture);

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего поргресс бара для лекции '{0}'.", lecture);

			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText, rowNumber: 2);

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(3, _lectureTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение персонального поргресс бара для лекции '{0}'.", lecture);

			Assert.AreEqual(3, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего поргресс бара для лекции '{0}'.", lecture);
		}

		[Test, Ignore("PRX-15111")]
		public void TwoUsersSameTranslationLectureProgressTest()
		{
			var lecture = "1.3 Concepts";

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);
			
			_header.ClickSignOut();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(0, _lectureTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(0, _lectureTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);
		}

		[Test]
		public void DecreaseLecturePersonalAndCommonProgressTest()
		{
			var lecture = "1.4 Theories";

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(1, _lectureTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(1, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lectureTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ClickDeleteTranslateButton(CourseraCrowdsourceUser.NickName, _translationText);

			_deleteTranslationDialog.ClickYesButton();

			_editorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(0, _lectureTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(0, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);
		}
		
		[Test]
		public void DecreaseLectureCommonProgressTwoUsersTest()
		{
			var lecture = "1.7 Cultural approach";
			var _translationText2 = "Test" + Guid.NewGuid();

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);
			
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);
			
			_header
				.GoToUserProfile()
				.ClickSignOut();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText2);

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lectureTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment(rowNumber: 1)
				.ClickDeleteTranslateButton(_secondUser.NickName, _translationText2);
			
			_deleteTranslationDialog.ClickYesButton();

			_editorPage.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			Assert.AreEqual(0, _lectureTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);
		}

		[Test, Ignore("Неизвестно, когда происходит перерасчет страниц")]
		public void TotalPagesTest()
		{
			var lecture = "3.8 Negotiated Effects";
			var totalPagesCountBefore = _courseraHomePage.GetTotalPagesCount();

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

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

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

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

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage.AddTranslationForCourseraProgress(_translationText);

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(3, _lectureTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);

			_header
				.ClickSignOut()
				.GoToHomePage();

			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_profilePage.GoToCoursesPage();

			_coursesPage
				.ClickCourse(_courseraNameProgressTests)
				.ClickLectureTab();

			_lectureTab.OpenLecture(lecture);

			_editorPage
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_header
				.ClickSignOut()
				.GoToHomePage();

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);

			_coursesPage
				.GoToCoursesPage()
				.ClickCourse(_courseraNameProgressTests)
				.ClickLectureTab();

			Assert.AreEqual(3, _lectureTab.GetPersonalProgressValuebyLectureName(lecture),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture);
		}

		[Test]
		public void LectureProgressNegativeAndPositiveTranslations()
		{
			var lecture = "2.4 Rhetorical Theory";

			_secondUser = TakeUser(ConfigurationManager.CourseraCrowdsourceUsers);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage
				.FillTarget(_translationText)
				.ConfirmSegmentTranslation()
				.AddTranslationForCourseraProgress(_translationText +"2");
		
			_header
				.ClickSignOut()
				.GoToHomePage();
			
			_loginHelper.LogInCoursera(_secondUser.Login, _secondUser.Password);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lectureTab.OpenLecture(lecture);

			_editorPage
				.FillTarget(_translationText + "secondUser")
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
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

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage
				.FillTarget(_translationText)
				.ConfirmSegmentTranslation()
				.FillTarget(secondTranslationVersion)
				.ConfirmSegmentTranslation()
				.AddTranslationForCourseraProgress(thirdTranslationVersion);

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, secondTranslationVersion)
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, thirdTranslationVersion)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_header
				.ClickSignOut()
				.GoToHomePage();
			
			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
					
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			_lectureTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, secondTranslationVersion)
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, thirdTranslationVersion)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_header
				.ClickSignOut()
				.GoToHomePage();
			
			_loginHelper.LogInCoursera(_thirdUser.Login, _thirdUser.Password);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			_lectureTab.OpenLecture(lecture);

			_editorPage
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, secondTranslationVersion)
				.ScrollAndClickVoteUpButton(CourseraCrowdsourceUser.NickName, thirdTranslationVersion)
				.ClickHomeButtonExpectingCourseraCoursesPage();
			
			_coursesPage
				.ClickCourse(_courseraNameProgressTests)
				.ClickLectureTab();

			Assert.AreEqual(2, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);
		}

		[Test]
		public void DecreaseProgressVoteDown()
		{
			var lecture = "2.6 A Renaissance of our field";
			var secondTranslationVersion = _translationText + "2";
			var thirdTranslationVersion = _translationText + "3";

			_courseraHomePage.ClickSelectCourse();

			_coursesPage.ClickCourse(_courseraNameProgressTests);
			
			_lectureTab.OpenLecture(lecture);

			_editorPage
				.FillTarget(_translationText)
				.ConfirmSegmentTranslation()
				.FillTarget(secondTranslationVersion)
				.ConfirmSegmentTranslation()
				.AddTranslationForCourseraProgress(thirdTranslationVersion);

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(6, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Неверное значение общего прогресса для лекции '{0}'.", lecture);

			_lectureTab.OpenLecture(lecture);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, _translationText)
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, secondTranslationVersion)
				.ScrollAndClickVoteDownButton(CourseraCrowdsourceUser.NickName, thirdTranslationVersion)
				.ClickHomeButtonExpectingCourseraCoursesPage();

			_coursesPage.ClickCourse(_courseraNameProgressTests);

			Assert.AreEqual(6, _lectureTab.GetCommonProgressValueByLectureName(lecture),
				"Произошла ошибка:\n Общий прогресс не уменьшился.");
		}

		[Test]
		public void CompleteCourseTest()
		{
			var lecture1 = "1.1 Introduction";
			var lecture2 = "1.2 What is communication";
			var translationText = "Test" + Guid.NewGuid();

			var courseProgress = _coursesPage.GetCourseProgress(_courseNameForCompleteCourseTest);

			_courseraHomePage.ClickSelectCourse();

			_coursesPage
				.WaitCourseNameDisplayed(_courseNameForCompleteCourseTest)
				.ClickCourse(_courseNameForCompleteCourseTest);

			_lectureTab.OpenLecture(lecture1);

			_editorPage
				.FillSegmentTargetField(translationText, rowNumber: 1)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(translationText, rowNumber: 2)
				.ConfirmSegmentTranslation()
				.AddTranslationForCourseraProgress(translationText, rowNumber: 3);

			_coursesPage.WaitCourseProgressChanged(_courseNameForCompleteCourseTest, courseProgress);

			Assert.AreEqual(58.3, _coursesPage.GetCourseProgress(_courseNameForCompleteCourseTest),
				"Произошла ошибка:\n Неверное значение прогресса курса {0}.", _courseNameForCompleteCourseTest);

			_coursesPage
				.ClickCourse(_courseNameForCompleteCourseTest)
				.ClickLectureTab();

			Assert.AreEqual(100, _lectureTab.GetPersonalProgressValuebyLectureName(lecture1),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture1);

			Assert.AreEqual(100, _lectureTab.GetCommonProgressValueByLectureName(lecture1),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture1);

			_lectureTab.OpenLecture(lecture2);

			_editorPage
				.FillSegmentTargetField(translationText, rowNumber: 1)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(translationText, rowNumber: 2)
				.ConfirmSegmentTranslation()
				.AddTranslationForCourseraProgress(translationText, rowNumber: 3);

			_coursesPage.WaitCourseProgressChanged(_courseNameForCompleteCourseTest, 58.3);

			Assert.AreEqual(100.0, _coursesPage.GetCourseProgress(_courseNameForCompleteCourseTest),
				"Произошла ошибка:\n Неверное значение прогресса курса {0}.", _courseNameForCompleteCourseTest);

			_coursesPage
				.ClickCourse(_courseNameForCompleteCourseTest)
				.ClickLectureTab();

			Assert.AreEqual(100, _lectureTab.GetPersonalProgressValuebyLectureName(lecture2),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture1);

			Assert.AreEqual(100, _lectureTab.GetCommonProgressValueByLectureName(lecture2),
				"Произошла ошибка:\n Неверное значение личного прогресса для лекции '{0}'.", lecture1);
		}

		private DeleteTranslationDialog _deleteTranslationDialog;
		private string _translationText;
	}
}
