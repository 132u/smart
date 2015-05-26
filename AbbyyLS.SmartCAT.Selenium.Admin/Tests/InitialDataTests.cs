using AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.InitialData
{
	[TestFixture]
	public class InitialDataTests<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		public InitialDataTests() 
		{
			AdminLoginPage = true;
		}

		[SetUp]
		public void SetUpInitialDataTests()
		{
			_adminHelper = new AdminHelper();
		}

		[Test]
		public void CreateCorpAccount()
		{
			_adminHelper
				.CreateAccountIfNotExist(LoginHelper.SmartCATVenture, LoginHelper.TestAccountName, workflow: true)
				.CreateAccountAdminIfNotExist(Login, UserName, UserSurname);
		}

		[Test]
		public void CreatePersAccountForRingo()
		{
			_adminHelper
				.CreateNewUser(Login2, UserName2, Password2, admin: true)
				.FindUser(Login2)
				.CheckAdminCheckbox()
				.CreateNewPersonalAccount(Login2, state: true)
				.AddUserToSpecificAccount(Login2, LoginHelper.TestAccountName);
		}

		[Test]
		public void CreatePerevedemCorpAccount()
		{
			_adminHelper.CreateAccountIfNotExist(LoginHelper.PerevedemVenture, LoginHelper.PerevedemAccountName, workflow: true);
		}

		[Test]
		public void CreateCourseraUsers()
		{
			_adminHelper.CreateAccountIfNotExist(LoginHelper.CourseraVenture, LoginHelper.CourseraAccountName, workflow: true);

			foreach (var user in CourseraUserList)
			{
				_adminHelper
					.CreateNewUser(user.Login, user.Login, user.Password)
					.AddUserToSpecificAccount(user.Login, LoginHelper.CourseraAccountName);
			}
		}

		[Test]
		public void CreatePersAccountForBobby()
		{
			_adminHelper
				.FindUser(Login)
				.CreateNewPersonalAccount(LoginHelper.PersonalAccountSurname, state: true);
		}

		[Test]
		public void CreatePersAccountForBatman()
		{
			_adminHelper
				.CreateNewUser(RightsTestLogin, RightsTestUserName, RightsTestPassword, admin: true)
				.FindUser(RightsTestLogin)
				.CheckAdminCheckbox()
				.CreateNewPersonalAccount(RightsTestLogin, state: true)
				.AddUserToSpecificAccount(RightsTestLogin, LoginHelper.TestAccountName);
		}

		private AdminHelper _adminHelper;
	}
}