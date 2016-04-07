using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossaries.ManageGlossariesAndCreateProject
{
	[Parallelizable(ParallelScope.Fixtures)]
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
				new List<RightsType> { RightsType.GlossaryManagement, RightsType.ProjectResourceManagement, RightsType.ProjectCreation });
			
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
