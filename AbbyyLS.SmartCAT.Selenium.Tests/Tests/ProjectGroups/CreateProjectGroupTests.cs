using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.ProjectGroups
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[ProjectGroups]
	class CreateProjectGroupTests<TWebDriverProvider> : ProjectGroupsBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
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
	}
}
