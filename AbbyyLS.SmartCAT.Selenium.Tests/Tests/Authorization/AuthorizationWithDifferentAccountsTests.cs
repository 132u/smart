using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Authorization]
	class AuthorizationWithDifferentAccountsTests<TWebDriverProvider> :
		RegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AuthorizationWithDifferentAccountsTests()
		{
			StartPage = StartPage.Admin;
		}

		[Test, Description("S-7111"), ShortCheckList]
		public void AuthorizationWithSingleCorporateAccountTest()
		{
			_adminHelper.CreateNewUser(_email, _firstAndLastName, _password);

			_adminHelper.AddUserToAdminGroupInAccountIfNotAdded(
				_email,
				_firstName, 
				_lastName, 
				LoginHelper.TestAccountName);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);

			_signInPage.SubmitFormExpectingWorkspacePage(_email, _password);

			Assert.IsTrue(_workspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка:\n После авторизации пользователя с одним аккаунтом " +
				"не произошел автоматический переход на галавную страницу.");
		}

		[Test, Description("S-7046"), ShortCheckList]
		public void AuthorizationWithCorporateAccountAndAccountSortTest()
		{
			_accountList = new List<string>
			{
				"B" + Guid.NewGuid(),
				"A" + Guid.NewGuid(),
				"D" + Guid.NewGuid(),
				"C" + Guid.NewGuid(),
			};

			_adminHelper
				.CreateAllAccountInListIfNotExist(
					_accountList,
					workflow: true,
					features: new List<string>
					{
						Feature.Clients.ToString(),
						Feature.Domains.ToString(),
						Feature.DocumentUpdate.ToString(),
						Feature.Vendors.ToString()
					})
				.CreateNewUser(_email, _firstAndLastName, _password)
				.CreateNewPersonalAccount(_lastName, true)
				.AddUserToAllAccountInList(_accountList, _email, _firstName, _lastName);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);

			_signInPage.SubmitForm(_email, _password);

			_accountList.Add(LoginHelper.PersonalAccountName);

			Assert.IsTrue(_selectAccountForm.IsSortAndContentInAccountsListCorrect(_accountList),
				"Произошла ошибка:\n Список аккаунтов некорректный, не совпадает либо количество, либо сортировка.");
		}

		[Test, Description("S-7115"), ShortCheckList]
		public void WorkInSeveralTabsTest()
		{
			_registrationPage.GetPageExpectingRedirectToSignInPage(ThreadUser.Login);

			_signInPage.SubmitForm(ThreadUser.Login, ThreadUser.Password);

			_selectAccountForm.SelectAccount();

			_workspacePage.Driver.OpenAndSwitchToNewTab(ConfigurationManager.Url);

			Assert.IsTrue(_workspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка:\n После авторизации и повторного заходна на сайт не отобразилась страница workspace.");
		}

		[Test, Description("S-7116"), ShortCheckList]
		public void WorkInSeveralTabsTestAfterSignOutTest()
		{
			_registrationPage.GetPageExpectingRedirectToSignInPage(ThreadUser.Login);

			_signInPage.SubmitForm(ThreadUser.Login, ThreadUser.Password);

			_selectAccountForm.SelectAccount();

			_workspacePage.Driver.OpenAndSwitchToNewTab(ConfigurationManager.Url);
			_workspacePage.ClickSignOutAssumingAlert();

			if (_workspacePage.IsAlertExist())
			{
				_workspacePage.AcceptAlert<SignInPage>();
			}

			_signInPage.Driver.SwitchToPreviousTabFromCurrentTab();
			_workspacePage.ClickProjectsSubmenuExpectingAlert();

			if (_workspacePage.IsAlertExist())
			{
				_workspacePage.AcceptAlert<SignInPage>();
			}

			Assert.IsTrue(_signInPage.IsSignInPageOpened() && !_workspacePage.IsWorkspacePageOpened(),
				"Произошла ошибка:\n Произошел переход на страницу смартката после разлогинивания в соседней вкладке.");
		}

		private List<string> _accountList;
	}
}