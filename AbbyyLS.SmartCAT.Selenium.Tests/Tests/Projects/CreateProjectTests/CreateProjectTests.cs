﻿using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[Projects]
	class CreateProjectTests<TWebDriverProvider>
		: BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7136"), ShortCheckList]
		public void CreateProjectNoFileTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);
		}

		[Test]
		public void CreateProjectSomeFilesTest()
		{
			var ttxFile = PathProvider.TtxFile;
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(
				projectName:_projectUniqueName,
				filesPaths: new[] { document, ttxFile });

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);
		}

		[Test, Description("S-7137"), ShortCheckList]
		public void CreateProjectViaGreenCreateProjectButtonTest()
		{
			var document = PathProvider.DocumentFile;

			_projectsPage.ClickGreenCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { document })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);
		}

		[Test]
		public void CreateProjectDeletedNameTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();
		}

		[Test]
		public void CreateProjectDuplicateNameTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage.FillGeneralProjectInformation(_projectUniqueName);

			Assert.IsTrue(_newProjectSettingsPage.IsDuplicateNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о существующем имени");

			Assert.IsTrue(_newProjectSettingsPage.IsNameInputValidationMarkerDisplayed(),
				"Произошла ошибка:\n поле 'Название' не отмечено ошибкой");
		}

		[Test]
		public void CreateProjectLongNameTest()
		{
			var longProjectUniqueName = _createProjectHelper.GetProjectUniqueName() + _longName;
			_createProjectHelper.CreateNewProject(longProjectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(longProjectUniqueName.Substring(0, 100)),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", longProjectUniqueName.Substring(0, 100));
		}

		[Test]
		public void CreateProjectTwoWordsNameTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName() + " " + "SpacePlusSymbols";

			_createProjectHelper.CreateNewProject(projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", projectUniqueName);
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

		[TestCase(Deadline.CurrentDate)]
		[TestCase(Deadline.NextMonth)]
		[TestCase(Deadline.PreviousMonth)]
		public void CreateProjectWithDeadline(Deadline deadline)
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName, deadline: deadline)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);
		}

		[Test, Description("S-7138"), ShortCheckList]
		public void CreateProjectFewLanguagesTest()
		{
			var document = PathProvider.DocumentFile;
			var documentFileName = Path.GetFileNameWithoutExtension(document);
			var ttxFile = PathProvider.TtxFile;
			var ttxFileName = Path.GetFileNameWithoutExtension(ttxFile);
			var expectedJobList = new List<string>
				(
					new []
					{
						documentFileName + "_fr",
						documentFileName + "_ja",
						documentFileName + "_ru",
						ttxFileName + "_fr",
						ttxFileName + "_ja",
						ttxFileName + "_ru"
					}
				);

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { document, ttxFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillProjectName(_projectUniqueName)
				.SetDeadline(Deadline.NextMonth)
				.SetSourceLanguage(Language.English)
				.SetTargetLanguage(Language.French, deselectAllLanguages: false)
				.SetTargetLanguage(Language.Russian, deselectAllLanguages: false)
				.SetTargetLanguage(Language.Japanese, deselectAllLanguages: false)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName);

			expectedJobList.Sort();
			var jobList = _projectsPage.GetJobList(_projectUniqueName);

			Assert.AreEqual(expectedJobList, jobList,
				"Произошла ошибка: Неверный список джобов.");
		}

		[Test, Description("S-7144"), ShortCheckList]
		public void OpenProjectSettingsPageTest()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[]{ document });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			Assert.IsTrue(_projectSettingsPage.IsProjectSettingsPageOpened(),
				"Произошла ошибка: не открылась страница проекта {0}.", _projectUniqueName);
		}
	}
}
