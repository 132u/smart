using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	[TestFixture]
	[PriorityMajor]
	class ProjectGroupsTest : BaseTest
	{
		[SetUp]
		public void Setup()
		{
			QuitDriverAfterTest = true;
			_projectGroupHelper = WorkspaceHelper.GoToProjectGroupsPage();
		}

		[Test]
		public void CreateProjectGroupTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.AssertProjectGroupExist(projectGroup);
		}

		[Test]
		public void CreateProjectGroupExistingNameTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.CreateProjectGroup(projectGroup)
				.AssertNameErrorExist();
		}

		[TestCase("")]
		[TestCase("  ")]
		public void CreateProjectGroupInvalidNameTest(string projectGroup)
		{
			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.AssertNewProjectGroupEditMode();
		}

		[Test]
		public void CreateProjectGroupCheckCreateTMTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.GoToTranslationMemoriesPage()
				.AssertProjectGroupExist(projectGroup);
		}

		[Test]
		public void CreateProjectGroupCheckCreateGlossaryTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.GoToGlossariesPage()
				.AssertProjectGroupExist(projectGroup);
		}

		[Test]
		public void ChangeProjectGroupNameTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();
			var newProjectGroupsName = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.RenameProjectGroup(projectGroup, newProjectGroupsName)
				.AssertProjectGroupNotExist(projectGroup)
				.AssertProjectGroupExist(newProjectGroupsName);
		}

		[TestCase("")]
		[TestCase("  ")]
		public void ChangeProjectGroupInvalidNameTest(string projectGroupName)
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.RenameProjectGroup(projectGroup, projectGroupName)
				.AssertIsEditMode();
		}

		[Test]
		public void ChangeProjectGroupExistingNameTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();
			var secondProjectGroupName = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.CreateProjectGroup(secondProjectGroupName)
				.RenameProjectGroup(secondProjectGroupName, projectGroup)
				.AssertNameErrorExist();
		}

		[Test]
		public void DeleteProjectGroupTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.DeleteProjectGroup(projectGroup)
				.AssertProjectGroupNotExist(projectGroup);
		}

		[Test]
		public void DeleteProjectGroupCheckCreateTM()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.DeleteProjectGroup(projectGroup)
				.GoToTranslationMemoriesPage()
				.AssertProjectGroupNotExist(projectGroup);
		}

		[Test]
		public void DeleteProjectGroupsCheckCreateGlossaryTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.DeleteProjectGroup(projectGroup)
				.GoToGlossariesPage()
				.AssertProjectGroupNotExist(projectGroup);
		}

		private ProjectGroupsHelper _projectGroupHelper;
	}
}
