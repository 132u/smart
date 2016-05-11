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
				filesPaths: new[] { PathProvider.EditorTxtFile });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);
		}

		[Test, Description("S-7143"), ShortCheckList]
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

		[Test, ShortCheckList] // TODO: после разделения тест-кейса S-7143 присвоить уникальный ID
		public void ChangeProjectGroupTest()
		{
			_projectSettingsDialog
				.SelectProjectGroup(_projectGroup)
				.SaveSettingsExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			Assert.AreEqual(_projectGroup, _projectSettingsDialog.GetProjectGroupName(),
				"Произошла ошикба: неверное название группы проекта.");
		}

		[Test, ShortCheckList] // TODO: после разделения тест-кейса S-7143 присвоить уникальный ID
		public void ChangeDeadlineTest()
		{
			var tomorrow = DateTime.Now.AddDays(1);

			_projectSettingsDialog.ClickDatetimePickerIcon();

			_dateTimePicker.SetDate<ProjectSettingsDialog>(tomorrow.Day);

			_projectSettingsDialog.SaveSettingsExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			Assert.AreEqual(tomorrow.Date, _projectSettingsDialog.GetDeadine(),
				"Произошла ошикба: неверная дата в дедлайне.");
		}

		[Test, ShortCheckList] // TODO: после разделения тест-кейса S-7143 присвоить уникальный ID
		public void ChangeDescriptionTest()
		{
			var description = "description text";
			
			_projectSettingsDialog
				.FillDescription(description)
				.SaveSettingsExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			Assert.AreEqual(description, _projectSettingsDialog.GetProjectDescription(),
				"Произошла ошикба: неверное название группы проекта.");
		}

		protected string _clientName;
		protected string _projectGroup;

		private ProjectGroupsPage _projectGroupsPage;
		private ClientsPage _clientsPage;
		private DatePicker _dateTimePicker;
	}
}
