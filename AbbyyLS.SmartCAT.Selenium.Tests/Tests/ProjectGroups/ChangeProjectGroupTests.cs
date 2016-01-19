using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.ProjectGroups
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[ProjectGroups]
	class ChangeProjectGroupTests<TWebDriverProvider> : ProjectGroupsBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
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
	}
}
