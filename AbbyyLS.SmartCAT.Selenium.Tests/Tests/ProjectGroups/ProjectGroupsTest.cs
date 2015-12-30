using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.ProjectGroups
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[ProjectGroups]
	class ProjectGroupsTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Setup()
		{
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_newTranslationMemoryDialog = new NewTranslationMemoryDialog(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_newGlossaryDialog = new NewGlossaryDialog(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectGroup = _projectGroupsPage.GetProjectGroupUniqueName();

			_workspacePage.GoToProjectGroupsPage();
		}

		[Test]
		public void CreateProjectGroupTest()
		{
			_projectGroupsPage.CreateProjectGroup(_projectGroup);

			Assert.IsTrue(_projectGroupsPage.IsSaveButtonDisappear(),
				"Произошла ошибка:\n кнопка сохранения группы проектов не исчезла после сохранения.");

			Assert.IsTrue(_projectGroupsPage.IsProjectGroupExist(_projectGroup),
				"Произошла ошибка:\n группа проектов {0} отсутствует в списке", _projectGroup);
		}

		[Test]
		public void CreateProjectGroupExistingNameTest()
		{
			_projectGroupsPage
				.CreateProjectGroup(_projectGroup)
				.CreateProjectGroup(_projectGroup);

			Assert.IsTrue(_projectGroupsPage.IsProjectGrouptNameErrorDisplayed(),
				"Произошла ошибка:\n не появилась ошибка при создании группы проектов с некорректным именем.");
		}

		[TestCase("")]
		[TestCase("  ")]
		public void CreateProjectGroupInvalidNameTest(string projectGroup)
		{
			_projectGroupsPage.CreateProjectGroup(projectGroup);

			Assert.IsTrue(_projectGroupsPage.IsGroupProjectEmptyRowDisplayed(),
				"Произошла ошибка:\n  Произошел выход из режима редактирования только что созданной группы проектов.");
		}

		[Test]
		public void CreateProjectGroupCheckCreateTMTest()
		{
			_projectGroupsPage.CreateProjectGroup(_projectGroup);

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenProjectGroupsList();

			Assert.IsTrue(_newTranslationMemoryDialog.IsProjectGroupsListDisplayed(),
				"Произошла ошибка:\n список групп проектов не открылся при создании ТМ");

			Assert.IsTrue(_newTranslationMemoryDialog.IsProjectGroupExistInList(_projectGroup),
				"Произошла ошибка:\n группа проектов {0} отсутствует в списке при создании ТМ.", _projectGroup);
		}

		[Test]
		public void CreateProjectGroupCheckCreateGlossaryTest()
		{
			_projectGroupsPage.CreateProjectGroup(_projectGroup);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage
				.ClickCreateGlossaryButton()
				.ClickProjectGroupsList();

			Assert.IsTrue(_newGlossaryDialog.IsProjectGroupExistInList(_projectGroup),
				"Произошла ошибка:\n  группа проектов отсутствует в списке при создании глоссария");
		}

		[Test]
		public void ChangeProjectGroupNameTest()
		{
			var newProjectGroupsName = _projectGroupsPage.GetProjectGroupUniqueName();

			_projectGroupsPage
				.CreateProjectGroup(_projectGroup)
				.RenameProjectGroup(_projectGroup, newProjectGroupsName);

			Assert.IsTrue(_projectGroupsPage.IsProjectGroupDisappeared(_projectGroup),
				"Произошла ошибка:\n группа проектов {0} присутствует в списке", _projectGroup);

			Assert.IsTrue(_projectGroupsPage.IsProjectGroupExist(newProjectGroupsName),
				"Произошла ошибка:\n группа проектов {0} отсутствует в списке", newProjectGroupsName);
		}

		[TestCase("")]
		[TestCase("  ")]
		public void ChangeProjectGroupInvalidNameTest(string projectGroupName)
		{
			_projectGroupsPage
				.CreateProjectGroup(_projectGroup)
				.RenameProjectGroup(_projectGroup, projectGroupName);

			Assert.IsTrue(_projectGroupsPage.IsEditModeEnabled(),
				"Произошла ошибка:\n произошел выход из режима редактирования группы проектов.");
		}

		[Test]
		public void ChangeProjectGroupExistingNameTest()
		{
			var secondProjectGroupName = _projectGroupsPage.GetProjectGroupUniqueName();

			_projectGroupsPage
				.CreateProjectGroup(_projectGroup)
				.CreateProjectGroup(secondProjectGroupName)
				.RenameProjectGroup(secondProjectGroupName, _projectGroup);

			Assert.IsTrue(_projectGroupsPage.IsProjectGrouptNameErrorDisplayed(),
				"Произошла ошибка:\n не появилась ошибка при создании группы проектов с некорректным именем.");
		}

		[Test]
		public void DeleteProjectGroupTest()
		{
			_projectGroupsPage
				.CreateProjectGroup(_projectGroup)
				.DeleteProjectGroup(_projectGroup);

			Assert.IsFalse(_projectGroupsPage.IsProjectGroupExist(_projectGroup),
				"Произошла ошибка:\n группа проектов {0} присутствует в списке", _projectGroup);
		}

		[Test]
		public void DeleteProjectGroupCheckCreateTM()
		{
			_projectGroupsPage
				.CreateProjectGroup(_projectGroup)
				.DeleteProjectGroup(_projectGroup);

			Assert.IsFalse(_projectGroupsPage.IsProjectGroupExist(_projectGroup),
				"Произошла ошибка:\n группа проектов {0} присутствует в списке", _projectGroup);

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenProjectGroupsList();

			Assert.IsTrue(_newTranslationMemoryDialog.IsProjectGroupsListDisplayed(),
				"Произошла ошибка:\n список групп проектов не открылся при создании ТМ");

			Assert.IsFalse(_newTranslationMemoryDialog.IsProjectGroupExistInList(_projectGroup),
				"Произошла ошибка:\n группа проектов {0} присутствует в списке при создании ТМ.", _projectGroup);
		}

		[Test]
		public void DeleteProjectGroupsCheckCreateGlossaryTest()
		{
			var projectGroup = _projectGroupsPage.GetProjectGroupUniqueName();

			_projectGroupsPage
				.CreateProjectGroup(projectGroup)
				.DeleteProjectGroup(projectGroup)
				.RefreshPage<WorkspacePage>();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage
				.ClickCreateGlossaryButton()
				.ClickProjectGroupsList();

			Assert.IsFalse(_newGlossaryDialog.IsProjectGroupExistInList(projectGroup),
				"Произошла ошибка:\n Группа проектов присутствует в списке при создании глоссария");
		}

		private ProjectGroupsPage _projectGroupsPage;
		private TranslationMemoriesPage _translationMemoriesPage;
		private NewTranslationMemoryDialog _newTranslationMemoryDialog;
		private GlossariesPage _glossariesPage;
		private NewGlossaryDialog _newGlossaryDialog;
		private WorkspacePage _workspacePage;
		private string _projectGroup;
	}
}
