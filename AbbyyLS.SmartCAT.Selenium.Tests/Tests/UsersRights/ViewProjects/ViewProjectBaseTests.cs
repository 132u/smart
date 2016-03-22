﻿using System;

﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
﻿using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ViewProjects
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ViewProjectBaseTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_exportNotification = new ExportNotification(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_editorPage = new EditorPage(Driver);
			_qualityAssuranceDialog = new QualityAssuranceDialog(Driver);
			_confirmDeclineTaskDialog = new ConfirmDeclineTaskDialog(Driver);
			_buildStatisticsPage = new BuildStatisticsPage(Driver);

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				groupName,
				RightsType.ProjectView);
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		protected string _projectUniqueName;
		protected UserRightsHelper _userRightsHelper;
		protected DocumentUploadGeneralInformationDialog _documentUploadGeneralInformationDialog;
		protected CreateProjectHelper _createProjectHelper;
		protected WorkspacePage _workspacePage;
		protected ProjectsPage _projectsPage;
		protected ExportNotification _exportNotification;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected EditorPage _editorPage;
		protected QualityAssuranceDialog _qualityAssuranceDialog;
		protected ConfirmDeclineTaskDialog _confirmDeclineTaskDialog;
		protected BuildStatisticsPage _buildStatisticsPage;
	}
}
