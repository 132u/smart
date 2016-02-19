using System.Linq;

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
			foreach (var user in ConfigurationManager.ThreadUsersList.ToList())
			{
				_adminHelper
					.CreateUserWithSpecificAndPersonalAccount(
						user.Login, user.Name, user.Surname, user.Login, user.Password)
					.AddUserToAdminGroupInAccountIfNotAdded(
						user.Login, user.Name, user.Surname, LoginHelper.CourseraAccountName);
			}
		}
	}
}
