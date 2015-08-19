﻿using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.InitialData
{
	[TestFixture]
	public class InitialDataTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public InitialDataTests() 
		{
			StartPage = StartPage.Admin;
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
							Feature.DocumentUpdate.ToString()
						})
					.CreateAccountAdminIfNotExist(Login, UserName, UserSurname, LoginHelper.TestAccountName);
		}

		[Test]
		public void CreatePersAccountForRingo()
		{
			_adminHelper
				.CreateNewUser(Login2, NickName2, Password2, admin: true, aolUser: true)
				.FindUser(Login2)
				.CheckAdminCheckbox()
				.CreateNewPersonalAccount(Login2, state: true)
				.AddUserToSpecificAccount(Login2, LoginHelper.TestAccountName);
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

			foreach (var user in CourseraUserList)
			{
				_adminHelper
					.CreateNewUser(user.Login, user.Login, user.Password, aolUser: true)
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
				.CreateNewUser(RightsTestLogin, RightsTestNickName, RightsTestPassword, admin: true, aolUser: true)
				.FindUser(RightsTestLogin)
				.CheckAdminCheckbox()
				.CreateNewPersonalAccount(RightsTestLogin, state: true)
				.AddUserToSpecificAccount(RightsTestLogin, LoginHelper.TestAccountName);
		}

		[Test]
		public void CreateDefaultDictionaryPackage()
		{
			_adminHelper.CreateDictionaryPackageIfNotExist();
		}

		private AdminHelper _adminHelper;
	}
}