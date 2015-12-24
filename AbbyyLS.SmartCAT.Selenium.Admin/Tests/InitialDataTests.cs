using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.InitialData
{
	public class InitialDataTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public InitialDataTests()
		{
			GlobalSetup.SetUp();
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void SetUpInitialDataTests()
		{
			_adminHelper = new AdminHelper(Driver);
		}

		[Test, Category("Admin tests")]
		public void CreateCorporateAccount()
		{
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
				.CreateAccountAdminIfNotExist(ThreadUser.Login, ThreadUser.Name, ThreadUser.Surname, LoginHelper.TestAccountName);
		}

		[Test, Category("Admin tests")]
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

		[Test, Category("Admin tests")]
		public void CreateCourseraUsers()
		{
			_adminHelper.CreateAccountIfNotExist(
				LoginHelper.CourseraVenture,
				LoginHelper.CourseraAccountName,
				workflow: true,
				features: new List<string>
					{
						Feature.Clients.ToString(),
						Feature.Crowd.ToString(),
						Feature.LingvoDictionaries.ToString(),
					}
				);

			foreach (var user in ConfigurationManager.CourseraUserList)
			{
				_adminHelper
					.CreateNewUser(user.Login, user.Login, user.Password, aolUser: true)
					.AddUserToSpecificAccount(user.Login, LoginHelper.CourseraAccountName);
			}
		}

		[Test, Category("Admin tests")]
		public void CreateSocialNetworksAccounts()
		{
			foreach (var user in ConfigurationManager.SocialNetworksUserList)
			{
				_adminHelper.CreateUserWithPersonalAccount(user.Login, user.Login, user.Password);
			}
		}

		[Test, Category("Admin tests")]
		[ApiIntegration]
		public void CreatePersonalAccountForThreadUsers()
		{
			foreach (var user in ConfigurationManager.ThreadUsersList)
			{
				_adminHelper.CreateUserWithPersonalAccount(user.Login, user.Login, user.Password);
			}
		}

		[Test, Category("Admin tests")]
		public void CreatePersonalAccountForAdditionalUsers()
		{
			foreach (var user in ConfigurationManager.AdditionalUsersList)
			{
				_adminHelper.CreateUserWithPersonalAccount(user.Login, user.Login, user.Password);
			}
		}

		[Test, Category("Admin tests")]
		public void CreatePersonalAccountForRingo()
		{
			foreach (var user in ConfigurationManager.UsersInfoList)
			{
				_adminHelper.CreateUserWithPersonalAccount(user.Login, user.Login, user.Password);
			}
		}

		[Test, Category("Admin tests")]
		public void CreateDefaultDictionaryPackage()
		{
			_adminHelper.CreateDictionaryPackageIfNotExist();
		}
		
		private AdminHelper _adminHelper;
	}
}