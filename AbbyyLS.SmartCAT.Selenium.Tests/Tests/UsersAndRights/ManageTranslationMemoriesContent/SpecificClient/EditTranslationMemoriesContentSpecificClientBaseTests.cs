using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class EditTranslationMemoriesContentSpecificClientBaseTests<TWebDriverProvider> : TranslationMemoriesUserRightsBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var groupName = Guid.NewGuid().ToString();
			_commonClientName = _clientsPage.GetClientUniqueName();
			
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_commonClientName);

			_userRightsHelper.CreateGroupWithSpecificRightsAndSpecificClient(
				AdditionalUser.NickName,
				groupName,
				RightsType.TMContentManagement,
				_commonClientName);

			_workspacePage.SignOut();
		}
	}
}
