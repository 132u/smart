﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

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
			_workspaceHelper = new WorkspaceHelper(Driver);
			_projectGroupHelper = _workspaceHelper.GoToProjectGroupsPage();
		}

		[Test]
		public void CreateProjectGroupTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.AssertSaveButtonDisappear()
				.AssertProjectGroupExist(projectGroup);
		}

		[Test]
		public void CreateProjectGroupExistingNameTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.AssertSaveButtonDisappear()
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
				.AssertSaveButtonDisappear()
				.GoToTranslationMemoriesPage()
				.AssertProjectGroupExist(projectGroup);
		}

		[Test]
		public void CreateProjectGroupCheckCreateGlossaryTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.AssertSaveButtonDisappear()
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
				.AssertSaveButtonDisappear()
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
				.AssertSaveButtonDisappear()
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
				.AssertSaveButtonDisappear()
				.CreateProjectGroup(secondProjectGroupName)
				.AssertSaveButtonDisappear()
				.RenameProjectGroup(secondProjectGroupName, projectGroup)
				.AssertNameErrorExist();
		}

		[Test]
		public void DeleteProjectGroupTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.AssertSaveButtonDisappear()
				.DeleteProjectGroup(projectGroup)
				.AssertProjectGroupNotExist(projectGroup);
		}

		[Test]
		public void DeleteProjectGroupCheckCreateTM()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.AssertSaveButtonDisappear()
				.DeleteProjectGroup(projectGroup)
				.AssertProjectGroupNotExist(projectGroup)
				.GoToTranslationMemoriesPage()
				.AssertProjectGroupNotExist(projectGroup);
		}

		[Test]
		public void DeleteProjectGroupsCheckCreateGlossaryTest()
		{
			var projectGroup = ProjectGroupsHelper.GetProjectGroupUniqueName();

			_projectGroupHelper
				.CreateProjectGroup(projectGroup)
				.AssertSaveButtonDisappear()
				.DeleteProjectGroup(projectGroup)
				.RefreshPage<WorkspacePage, WorkspaceHelper>()
				.GoToGlossariesPage()
				.AssertProjectGroupNotExist(projectGroup);
		}

		private ProjectGroupsHelper _projectGroupHelper;
		private WorkspaceHelper _workspaceHelper;
	}
}
