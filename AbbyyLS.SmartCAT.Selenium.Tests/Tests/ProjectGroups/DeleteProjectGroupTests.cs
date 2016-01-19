using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.ProjectGroups
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[ProjectGroups]
	class DeleteProjectGroupTests<TWebDriverProvider> : ProjectGroupsBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
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
	}
}
