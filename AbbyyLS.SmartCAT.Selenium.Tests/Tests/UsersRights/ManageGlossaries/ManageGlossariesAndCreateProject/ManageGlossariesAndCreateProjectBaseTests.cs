﻿using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossaries.ManageGlossariesAndCreateProject
{
	class ManageGlossariesAndCreateProjectBaseTests<TWebDriverProvider> : ManageGlossariesCommonBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
			var groupName = Guid.NewGuid().ToString();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			
			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				groupName,
				RightsType.GlossaryManagement);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);
			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectResourceManagement);
			_groupsAndAccessRightsTab.ClickSaveButton(groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);
			_addAccessRightDialog.AddRightToGroupAnyProject(RightsType.ProjectCreation);
			_groupsAndAccessRightsTab.ClickSaveButton(groupName);

			_workspacePage.SignOut();
		}

		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

				_loginHelper.Authorize(StartPage, AdditionalUser);
				_exportNotification.CancelAllNotifiers<ProjectsPage>();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}
	}
}
