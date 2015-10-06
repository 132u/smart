using System.Collections.Generic;

using NUnit.Framework;

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

		[Test]
		public void CreateCorpAccount()
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
							Feature.TranslateConnector.ToString(),
							Feature.LingvoDictionaries.ToString(),
							Feature.DocumentUpdate.ToString(),
							Feature.Vendors.ToString()
						},
					unlimitedUseServices: true)
				.CreateAccountAdminIfNotExist(ThreadUser.Login, ThreadUser.Name, ThreadUser.Surname, LoginHelper.TestAccountName);
		}

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
							Feature.TranslateConnector.ToString(),
							Feature.LingvoDictionaries.ToString(),
						}
				);
		}

		[Test]
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
						Feature.TranslateConnector.ToString(),
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

		[Test, Explicit]
		public void CreateSocialNetworksAccounts()
		{
			foreach (var user in ConfigurationManager.SocialNetworksUserList)
			{
				_adminHelper.CreateUserWithPersonalAccount(user.Login, user.Login, user.Password);
			}
		}

		[Test]
		public void CreatePersAccountForThreadUsers()
		{
			foreach (var user in ConfigurationManager.ThreadUsersList)
			{
				_adminHelper.CreateUserWithPersonalAccount(user.Login, user.Login, user.Password);
			}
		}

		[Test]
		public void CreatePersAccountForAdditionalUsers()
		{
			foreach (var user in ConfigurationManager.AdditionalUsersList)
			{
				_adminHelper.CreateUserWithPersonalAccount(user.Login, user.Login, user.Password);
			}
		}

		[Test]
		public void CreateDefaultDictionaryPackage()
		{
			_adminHelper.CreateDictionaryPackageIfNotExist();
		}

		private AdminHelper _adminHelper;
	}
}