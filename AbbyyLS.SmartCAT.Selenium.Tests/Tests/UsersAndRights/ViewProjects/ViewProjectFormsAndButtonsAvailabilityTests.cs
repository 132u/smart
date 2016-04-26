using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ViewProjectFormsAndButtonsAvailabilityTests<TWebDriverProvider> : ViewProjectBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.DocumentFile });
			
			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, false)
				.ClickSaveButton();

			_workspacePage.SignOut();
		}

		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_loginHelper.Authorize(StartPage, AdditionalUser);
				_exportNotification.CancelAllNotifiers<ProjectsPage>();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[Test]
		public void ProjectPageTest()
		{
			_projectsPage.ClickProjectWithoutProjectSettingPageOpened(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectPanelExpanded(_projectUniqueName),
				"Произошла ошибка: Не открылась вкладка проекта {0}.", _projectUniqueName);
		}

		[Test]
		public void GreenCreateProjectButtonTest()
		{
			Assert.IsFalse(_projectsPage.IsGreenCreateProjectButtonDisplayed(),
				"Произошла ошибка:\n Зеленая кнопка создания поекта отображается.");
		}

		[Test]
		public void CreateProjectButtonTest()
		{
			Assert.IsFalse(_projectsPage.IsCreateProjectButtonDisplayed(),
				"Произошла ошибка:\n Кнопка создания поекта отображается.");
		}

		[Test]
		public void DeleteFileButtonTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.SelectDocument(_projectUniqueName, PathProvider.DocumentFile);

			Assert.IsFalse(_projectsPage.IsDeleteFileButtonDisplayed(_projectUniqueName),
				"Произошла ошибка: Кнопка удаления файла отображается.");
		}

		[Test]
		public void DeleteFileButtonWithoutSelectDocumentTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsDeleteFileButtonDisplayed(_projectUniqueName),
				"Произошла ошибка: Кнопка удаления файла отображается.");
		}

		[Test]
		public void CheckedCheckboxesTest()
		{
			_projectsPage.ClickMainCheckbox();

			Assert.IsFalse(_projectsPage.AreAllCheckboxesChecked(),
				"Произошла ошибка: Чекбоксы не чекнуты.");
		}

		[Test]
		public void UncheckedCheckboxesTest()
		{
			_projectsPage
				.ClickMainCheckbox()
				.ClickMainCheckbox();

			Assert.IsFalse(_projectsPage.AreAllCheckboxesChecked(),
				"Произошла ошибка: Чекбоксы чекнуты.");
		}

		[Test]
		public void SearchProjectTest()
		{
			Assert.IsTrue(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка: Не найден проект.");
		}

		[Test]
		public void AddFilesButtonTest()
		{
			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsAddFilesButtonDisplayed(),
				"Произошла ошибка: Кнопка 'Add Files' отображается для проекта {0}.", _projectUniqueName);
		}

		[Test]
		public void DeleteProjectButtonTest()
		{
			Assert.IsFalse(_projectsPage.IsProjectDeleteButtonDisplayed(),
				"Произошла ошибка: Кнопка удаления проекта отображается в панели.");
		}

		[Test]
		public void QACheckButtonTest()
		{
			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsQualityAssuranceCheckButtonDisplayed(_projectUniqueName),
				"Произошла ошибка:\n кнопка 'QA Check' у проекта '{0}' отсутствует", _projectUniqueName);

			_projectsPage.ClickQACheckButton(_projectUniqueName);

			Assert.IsTrue(_qualityAssuranceDialog.IsQualityAssuranceDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог контроля качества.");
		}

		[Test]
		public void StatisticsButtonTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectStatisticsButtonExpectingBuildStatisticsPage(_projectUniqueName);

			Assert.IsTrue(_buildStatisticsPage.IsBuildStatisticsPageOpened(),
				"Произошла ошибка:\n Не открылась страница построения статистики.");
		}
	}
}
