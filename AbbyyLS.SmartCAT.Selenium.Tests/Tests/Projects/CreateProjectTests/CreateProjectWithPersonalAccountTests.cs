using System.IO;
using System.Linq;

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

		[TestCase(true, Description = "S-7169"), ShortCheckList]
		[TestCase(false, Description = "S-7168"), ShortCheckList]
		public void CreateProjectNoFileTest(bool useGreenCreateProjectButton)
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true, useGreenCreateProjectButton: useGreenCreateProjectButton);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);
		}

		[Test, Description("S-7170"), ShortCheckList]
		public void CreateMultiLanguageProjectTest()
		{
			var targetLanguages = new[]
			{
				Language.Russian,
				Language.German
			};

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new []{ PathProvider.DocumentFile },
				targetLanguages: targetLanguages, 
				personalAccount: true);

			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsDocumentInProjectExist(_projectUniqueName, PathProvider.DocumentFile, targetLanguages),
				"Произошла ошибка:\n документ {0} не появился в списке для всех целевых языков.", 
				Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		[Test, Description("S-7171"), ShortCheckList]
		public void AddTargetLanguageToProjectTest()
		{
			var targetLanguages = new[]
			{
				Language.Russian,
				Language.German
			};

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { PathProvider.DocumentFile },
				targetLanguages: new[]{ targetLanguages[0] },
				personalAccount: true);

			_projectsPage.OpenProjectInfo(_projectUniqueName);

			_projectsPage.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.SelectTargetLanguages(targetLanguages[1])
				.SaveSettingsExpectingProjectsPage();

			Assert.IsTrue(_projectsPage.IsDocumentInProjectExist(_projectUniqueName, PathProvider.DocumentFile, targetLanguages),
				"Произошла ошибка:\n документ {0} не появился в списке для всех целевых языков.",
				Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		[TestCase(1, Description = "S-7172"), ShortCheckList]
		[TestCase(2)]
		public void AddDocumentToProjectTest(int targetLanguagesCount)
		{
			var ttxFile = PathProvider.TtxFile;

			var targetLanguages = (new[]
			{
				Language.Russian, 
				Language.German
			})
			.Take(targetLanguagesCount)
			.ToArray();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { ttxFile },
				targetLanguages: targetLanguages,
				personalAccount: true);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", _projectUniqueName);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.UploadDocument(new[] {PathProvider.DocumentFile});

			Assert.IsTrue(_projectSettingsPage.IsDocumentExist(PathProvider.DocumentFile, targetLanguages),
				"Произошла ошибка:\n документ {0} не появился в списке для всех целевых языков.",
				Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		[Test, Description("S-7066"), ShortCheckList]
		public void OpenProjectSettingsDialogOnProjectsPageTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			Assert.IsTrue(_projectSettingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог настроек проекта {0}.", _projectUniqueName);
		}

		[Test]
		public void OpenProjectSettingsDialogOnProjectSettingsPageTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickSettingsButton();

			Assert.IsTrue(_projectSettingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог настроек проекта {0}.", _projectUniqueName);
		}

		[Test, Description("S-7173"), ShortCheckList]
		public void ChangeProjectNameTest()
		{
			string newProjectUniqueName = "changed_" + _projectUniqueName;

			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.FillName(newProjectUniqueName)
				.SaveSettingsExpectingProjectsPage();

			Assert.IsTrue(_projectsPage.IsProjectNotExistInList(_projectUniqueName),
				"Произошла ошибка:\n проект {0} остался в списке.", newProjectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(newProjectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", newProjectUniqueName);

			_projectsPage.OpenProjectSettingsPage(newProjectUniqueName);

			Assert.AreEqual(_projectSettingsPage.GetProjectName(), newProjectUniqueName,
				"Произошла ошибка:\n имя проекта не изменилось на странице настроек проекта.");
		}

		[Test, Description("S-7181"), ShortCheckList]
		public void CancelProjectTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage
				.CancelProject(_projectUniqueName)
				.ClickCancelledProjectsTab();

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
					"Произошла ошибка:\n проект {0} не появился в списке отменённых проектов проектов.", _projectUniqueName);
		}

		[Test, Description("S-7178"), ShortCheckList]
		public void RepetitionsButtonTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			Assert.IsFalse(_projectSettingsPage.IsRepetitionsButtonDisplayed(),
					"Произошла ошибка:\n кнопка 'Repetitions' присутствует на странице проекта.");
		}

		[Test]
		public void CreateProjectDuplicateNameTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

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

			_createProjectHelper.CreateNewProject(longProjectUniqueName, personalAccount: true);

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(longProjectUniqueName.Substring(0, 100)),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", longProjectUniqueName.Substring(0, 100));
		}

		[Test, Description("S-7180"), ShortCheckList]
		public void AssignTasksTest()
		{
			var documentName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);

			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			Assert.IsFalse(_projectsPage.IsMyTasksTabDisplayed(),
				"Произошла ошибка:\n вкладка 'Мои задачи' отображается на странице списка проектов.");

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.UploadDocument(new[] { PathProvider.DocumentFile });

			_projectSettingsPage.HoverDocumentRow(documentName);

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
			_createProjectHelper.CreateNewProject(_projectUniqueName, personalAccount: true);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper
				.UploadDocument(new[] { PathProvider.DocumentFile })
				.DeleteDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}
	}
}
