using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateUsers]
	class CreateSocialNetworksUsers<TWebDriverProvider> : BaseAdminTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateSocialNetworksAccounts()
		{
			foreach (var user in ConfigurationManager.SocialNetworksUserList.ToList())
			{
				_adminHelper.CreateUserWithSpecificAndPersonalAccount(
					user.Login, user.Name, user.Surname, user.Login, user.Password);
			}
		}
	}
}
