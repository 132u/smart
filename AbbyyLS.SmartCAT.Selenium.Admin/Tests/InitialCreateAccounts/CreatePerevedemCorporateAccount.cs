using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Ignore("PRX-15311")]
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateAccount]
	class CreatePerevedemCorporateAccount<TWebDriverProvider> : BaseAdminTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreatePerevedemCorpAccount()
		{
			_adminHelper.CreateAccountIfNotExist(
				LoginHelper.PerevedemVenture,
				LoginHelper.PerevedemAccountName,
				workflow: true,
				features: new List<string>
						{
							Feature.Clients.ToString(),
							Feature.LingvoDictionaries.ToString(),
						}
			);
		}
	}
}
