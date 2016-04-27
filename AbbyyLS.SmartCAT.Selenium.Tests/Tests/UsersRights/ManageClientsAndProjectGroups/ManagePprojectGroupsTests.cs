using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManagePprojectGroupsTests<TWebDriverProvider> : ManageСlientsAndPprojectGroupsBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void BeforeTest()
		{
			_projectGroup = _projectGroupsPage.GetProjectGroupUniqueName();

			_workspacePage.GoToProjectGroupsPage();
			_projectGroupsPage.CreateProjectGroup(_projectGroup);
		}

		[Test]
		public void CreateProjectGroupTest()
		{
			Assert.IsTrue(_projectGroupsPage.IsSaveButtonDisappear(),
				"Произошла ошибка:\n кнопка сохранения группы проектов не исчезла после сохранения.");

			Assert.IsTrue(_projectGroupsPage.IsProjectGroupExist(_projectGroup),
				"Произошла ошибка:\n группа проектов {0} отсутствует в списке", _projectGroup);
		}
		
		[Test]
		public void ChangeProjectGroupNameTest()
		{
			var newProjectGroupsName = _projectGroupsPage.GetProjectGroupUniqueName();

			_projectGroupsPage.RenameProjectGroup(_projectGroup, newProjectGroupsName);

			Assert.IsTrue(_projectGroupsPage.IsProjectGroupDisappeared(_projectGroup),
				"Произошла ошибка:\n группа проектов {0} присутствует в списке", _projectGroup);

			Assert.IsTrue(_projectGroupsPage.IsProjectGroupExist(newProjectGroupsName),
				"Произошла ошибка:\n группа проектов {0} отсутствует в списке", newProjectGroupsName);
		}

		[Test]
		public void DeleteProjectGroupTest()
		{
			_projectGroupsPage.DeleteProjectGroup(_projectGroup);

			Assert.IsFalse(_projectGroupsPage.IsProjectGroupExist(_projectGroup),
				"Произошла ошибка:\n группа проектов {0} присутствует в списке", _projectGroup);
		}
	}
}
