using System.Linq;

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
			foreach (var user in ConfigurationManager.AdditionalUsersList.ToList())
			{
				_adminHelper.CreateUserWithSpecificAndPersonalAccount(
					user.Login, user.Name, user.Surname, user.Login, user.Password);
			}
		}
	}
}
