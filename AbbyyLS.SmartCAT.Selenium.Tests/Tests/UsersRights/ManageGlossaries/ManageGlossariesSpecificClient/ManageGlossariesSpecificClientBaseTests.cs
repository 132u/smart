using System;
using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossaries;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossariesCompoundRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class ManageGlossariesSpecificClientBaseTests<TWebDriverProvider> : ManageGlossariesCommonBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_groupName = Guid.NewGuid().ToString();
			_commonClientName = _clientsPage.GetClientUniqueName();
			_commonClientName2 = _clientsPage.GetClientUniqueName();
			_commonGlossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_commonClientName);
			_clientsPage.CreateNewClient(_commonClientName2);

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_commonGlossaryUniqueName, client: _commonClientName2);

			_userRightsHelper.CreateGroupWithSpecificRightsAndSpecificClient(
				AdditionalUser.NickName,
				_groupName,
				RightsType.GlossaryManagement,
				_commonClientName);
		}
	}
}
