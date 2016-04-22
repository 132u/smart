using System.Collections.Generic;
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
		[Test, Description("S-7136")]
		public void CreateProjectNoFileTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);
		}

		[Test]
		public void CreateProjectSomeFilesTest()
		{
			_createProjectHelper.CreateNewProject(
				projectName:_projectUniqueName,
				filesPaths: new[] { PathProvider.DocumentFile, PathProvider.TtxFile });

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);
		}

		[Test, Description("S-7137")]
		public void CreateProjectViaGreenCreateProjectButtonTest()
		{
			_projectsPage.ClickGreenCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
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

		[Test, Description("S-7142")]
		public void ImportDocumentAfterCreationTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper.UploadDocument(new[] { PathProvider.DocumentFile });

			Assert.IsTrue(_projectSettingsPage.IsDocumentExist(PathProvider.DocumentFile),
				"Произошла ошибка:\n документ {0} отсутствует в проекте.", PathProvider.DocumentFile);
		}

		[Test, Description("S-7138")]
		public void CreateProjectFewLanguagesTest()
		{
			var documentFileName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);
			var ttxFileName = Path.GetFileNameWithoutExtension(PathProvider.TtxFile);
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
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile, PathProvider.TtxFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillProjectName(_projectUniqueName)
				.SetDeadline(Deadline.NextMonth)
				.SetSourceLanguage(Language.English)
				.SetTargetLanguage(Language.French)
				.SetTargetLanguage(Language.Russian)
				.SetTargetLanguage(Language.Japanese)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName, documentNumber: 1)
				.OpenDocumentInfoForProject(_projectUniqueName, documentNumber: 2);

			expectedJobList.Sort();
			var jobList = _projectsPage.GetJobList(_projectUniqueName);

			Assert.AreEqual(expectedJobList, jobList,
				"Произошла ошибка: Неверный список джобов.");
		}

		[Test, Description("S-7144")]
		public void OpenProjectSettingsPageTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[]{ PathProvider.DocumentFile});

			_projectsPage.ClickProject(_projectUniqueName);

			Assert.IsTrue(_projectSettingsPage.IsProjectSettingsPageOpened(),
				"Произошла ошибка: не открылась страница проекта {0}.", _projectUniqueName);
		}
	}
}
