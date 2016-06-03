using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Projects]
	class ChangeProjectSettingsTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_clientsPage = new ClientsPage(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_dateTimePicker = new DatePicker(Driver);
			_loginHelper = new LoginHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);

			_loginHelper.Authorize(StartPage, ThreadUser);

			_clientName = _clientsPage.GetClientUniqueName();
			_projectGroup = _projectGroupsPage.GetProjectGroupUniqueName();

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);

			_workspacePage.GoToProjectGroupsPage();
			_projectGroupsPage.CreateProjectGroup(_projectGroup);
		}

		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { _document });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);
		}

		[Test, Description("S-29229"), ShortCheckList, Ignore("PRX-17072")]
		public void ChangeClientTest()
		{
			_projectSettingsDialog
				.SelectClient(_clientName)
				.SaveSettingsExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			Assert.AreEqual(_clientName, _projectSettingsDialog.GetClientName(),
				"Произошла ошикба: неверное название клиента.");
		}

		[Test, Description("S-29230"), ShortCheckList]
		public void ChangeProjectGroupTest()
		{
			_projectSettingsDialog
				.SelectProjectGroup(_projectGroup)
				.SaveSettingsExpectingProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			Assert.AreEqual(_projectGroup, _projectSettingsPage.GetProjectGroupName(),
				"Произошла ошикба: неверное название группы проекта.");
		}

		[Test, Description("S-29228"), ShortCheckList]
		public void ChangeDeadlineTest()
		{
			var tomorrow = DateTime.Now.AddDays(1);

			_projectSettingsDialog.ClickDatetimePickerIcon();

			_dateTimePicker.SetDate<ProjectSettingsDialog>(tomorrow.Day);

			_projectSettingsDialog.SaveSettingsExpectingProjectsPage();

			Assert.AreEqual(tomorrow.Date, _projectsPage.GetDeadLine(_projectUniqueName),
				"Произошла ошикба: неверная дата в дедлайне.");
		}

		[Test, Description("S-29231"), ShortCheckList]
		public void ChangeDescriptionTest()
		{
			var description = "description text";
			
			_projectSettingsDialog
				.FillDescription(description)
				.SaveSettingsExpectingProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			Assert.AreEqual(description, _projectSettingsPage.GetProjectDescription(),
				"Произошла ошикба: неверное название группы проекта.");
		}

		protected string _clientName;
		protected string _projectGroup;

		private ProjectGroupsPage _projectGroupsPage;
		private ClientsPage _clientsPage;
		private DatePicker _dateTimePicker;
	}
}
