using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateAccount]
	class CreateCorporateAccount<TWebDriverProvider> : BaseAdminTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateCorporateAccountIfNotExist()
		{
			for (int i = 0; i < LoginHelper.TestVendorNames.Count; i++)
			{
				_adminHelper.CreateAccountIfNotExist(
					LoginHelper.SmartCATVenture,
					LoginHelper.TestVendorNames[i],
					workflow: true,
					features:
						new List<string>
						{
							Feature.Clients.ToString()
						});
			}

			_adminHelper
				.CreateAccountIfNotExist(
					LoginHelper.SmartCATVenture,
					LoginHelper.TestAccountName,
					workflow: true,
					features: new List<string>
						{
							Feature.Clients.ToString(),
							Feature.Domains.ToString(),
							Feature.LingvoDictionaries.ToString(),
							Feature.DocumentUpdate.ToString(),
							Feature.Vendors.ToString()
						},
					unlimitedUseServices: true)
				.AddUserToAdminGroupInAccountIfNotAdded(ThreadUser.Login, ThreadUser.Name, ThreadUser.Surname, LoginHelper.TestAccountName)
			.OpenEditModeForEnterpriceAccount(LoginHelper.TestAccountName);

			_adminCreateAccountPage.ClickVendorsManagementLink();
			_vendorsManagementPage.AddVendors(LoginHelper.TestVendorNames);
		}
	}
}
