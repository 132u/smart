using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	[SetUpFixture]
	public class GlobalSetup
	{
		[SetUp]
		public static void SetUp()
		{
			ConfigurationManager.InitializeRelatedToUserFields();
			ConfigurationManager.InitializeUsersAndCompanyList();
			ConfigurationManager.InitializeRelatedToServerFields();
		}
	}
}
