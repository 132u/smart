using System;
using System.Threading;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ProjectGroupsHelper : WorkspaceHelper
	{
		public ProjectGroupsHelper RenameProjectGroup(
			string projectGroupsName,
			string newProjectGroupName)
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage
				.ScrollAndClickProjectGroup(projectGroupsName)
				.HoverCursorToProjectGroup(projectGroupsName)
				.AssertEditButtonDisplay(projectGroupsName)
				.ClickEditButton(projectGroupsName)
				.AssertIsEditMode()
				.FillNewName(newProjectGroupName)
				.ClickSaveProjectGroups();

			return this;
		}

		public static string GetProjectGroupUniqueName()
		{
			// Sleep вставлен для того, чтобы избежать вероятности генерации одинаковых имен
			// Такая ситуация возможна, если имена генерируются друг за другом
			Thread.Sleep(500);

			return "TestProjectGroup" + DateTime.UtcNow.Ticks;
		}

		public ProjectGroupsHelper CreateProjectGroup(string projectGroup)
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage
				.ScrollAndClickCreateProjectGroupsButton()
				.АssertGroupProjectEmptyRowDisplayed()
				.FillProjectGroupName(projectGroup)
				.ClickSaveProjectGroups();

			return this;
		}

		public ProjectGroupsHelper AssertSaveButtonDisappear()
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage.AssertSaveButtonDisappear();

			return this;
		}
		
		public ProjectGroupsHelper AssertProjectGroupExist(string projectGroupName)
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage.AssertProjectGroupExist(projectGroupName);

			return this;
		}

		public ProjectGroupsHelper AssertProjectGroupNotExist(string projectGroup)
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage.AssertProjectGroupNotExist(projectGroup);

			return this;
		}

		public ProjectGroupsHelper AssertNameErrorExist()
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage.AssertProjectGrouptNameErrorExist();

			return this;
		}

		public ProjectGroupsHelper AssertIsEditMode()
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage.AssertIsEditMode();

			return this;
		}

		public ProjectGroupsHelper AssertNewProjectGroupEditMode()
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage.AssertNewProjectGroupEditMode();

			return this;
		}

		public ProjectGroupsHelper DeleteProjectGroup(string projectGroupName)
		{
			BaseObject.InitPage(_projectGroupsPage);
			_projectGroupsPage
				.ScrollAndClickProjectGroup(projectGroupName)
				.HoverCursorToProjectGroup(projectGroupName)
				.AssertDeleteButtonDisplay(projectGroupName)
				.ClickDeleteButton(projectGroupName);

			return this;
		}

		private readonly ProjectGroupsPage _projectGroupsPage = new ProjectGroupsPage();
	}
}
