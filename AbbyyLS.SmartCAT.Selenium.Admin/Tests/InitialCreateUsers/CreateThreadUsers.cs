using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateUsers]
	class CreateThreadUsers<TWebDriverProvider> : BaseAdminTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		[ApiIntegration]
		public void CreateThreadUsersWithRequiredAccounts()
		{
			foreach (var user in ConfigurationManager.ThreadUsersList)
			{
				_adminHelper
					.CreateUserWithPersonalAccount(user.Login, user.Login, user.Password)
					.AddUserToSpecificAccount(user.Login, LoginHelper.CourseraAccountName);
			}
		}
	}
}
