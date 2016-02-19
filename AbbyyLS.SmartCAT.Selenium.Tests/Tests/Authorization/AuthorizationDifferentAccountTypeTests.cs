using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Authorization
{
	[Parallelizable(ParallelScope.Fixtures)]
	class AuthorizationDifferentAccountTypeTests<TWebDriverProvider> : AuthorizationBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public AuthorizationDifferentAccountTypeTests()
		{
			StartPage = StartPage.Admin;
		}

		[TestCase(AccountType.AbbyyLs)]
		[TestCase(AccountType.Department)]
		[TestCase(AccountType.EasyTranslateApi)]
		[TestCase(AccountType.GenericCorporate)]
		[TestCase(AccountType.LanguageServiceProvider)]
		[TestCase(AccountType.Test)]
		[TestCase(AccountType.VmClient)]
		public void AuthorizationDifferentAccountTypeTest(AccountType accountType)
		{
			var _accountUniqueName = AdminHelper.GetAccountUniqueName();
			
			_adminHelper
				.CreateAccountIfNotExist(
					LoginHelper.SmartCATVenture,
					_accountUniqueName,
					workflow: true,
					features: new List<string>
						{
							Feature.Clients.ToString(),
							Feature.Domains.ToString(),
							Feature.LingvoDictionaries.ToString(),
							Feature.DocumentUpdate.ToString(),
							Feature.Vendors.ToString()
						},
					unlimitedUseServices: true,
					accountType: accountType)
				.AddUserToAdminGroupInAccountIfNotAdded(ThreadUser.Login, ThreadUser.Name, ThreadUser.Surname, _accountUniqueName);

			_commonHelper.GoToSignInPage();

			_signInPage.SubmitForm(ThreadUser.Login, ThreadUser.Password);

			_selectAccountForm.SelectAccount(_accountUniqueName);

			_workspacePage.SetLocale();

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_accountUniqueName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
		}
	}
}
