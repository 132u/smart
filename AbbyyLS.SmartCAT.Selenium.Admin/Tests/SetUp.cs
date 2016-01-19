using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Tests;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[SetUpFixture]
	class SetUp
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			GlobalSetup.SetUp();
		}
	}
}
