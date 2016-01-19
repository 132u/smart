using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateUsers]
	class CreateAdditionalUsers<TWebDriverProvider> : BaseAdminTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreatePersonalAccountForAdditionalUsers()
		{
			foreach (var user in ConfigurationManager.AdditionalUsersList)
			{
				_adminHelper.CreateUserWithPersonalAccount(user.Login, user.Login, user.Password);
			}
		}
	}
}
