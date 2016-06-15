using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.MachineTranslations.FastMTTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class EnablingFastMTTests<TWebDriverProvider> : BaseMTTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public EnablingFastMTTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void SetUp()
		{
			_adminSignInPage = new AdminSignInPage(Driver);
			_adminLingvoProPage = new AdminLingvoProPage(Driver);
			_adminEnterpriseAccountsPage = new AdminEnterpriseAccountsPage(Driver);
			_adminCreateAccountPage = new AdminCreateAccountPage(Driver);
		}

		[Test, Description("S-7273"), ShortCheckList]
		public void EnableFastMTByAdmin()
		{
			var accountUniqueName = AdminHelper.GetAccountUniqueName();
			_adminHelper
				.CreateAccountIfNotExist(
					LoginHelper.SmartCATVenture,
					accountName: accountUniqueName,
					features: new List<string>() {  })
				.AddUserToAdminGroupInAccountIfNotAdded(ThreadUser.Login, ThreadUser.Name, ThreadUser.Surname, accountUniqueName);

			signInSmartCatAccount(accountUniqueName);

			Assert.IsFalse(_workspacePage.IsMachineTranslationExistInMenu(),
				"Произошла ошибка:\n ссылки 'Machine Translation' не должно быть в меню");

			_workspacePage.SignOut();

			_adminSignInPage
				.GetPage()
				.SignIn(ThreadUser.Login, ThreadUser.Password);

			_adminLingvoProPage.ClickEnterpriseAccountsLink();

			_adminEnterpriseAccountsPage.ClickEditAccount(accountUniqueName);

			_adminCreateAccountPage
				.AddFeatures(new List<string>() {Feature.FastMT.ToString()})
				.ClickSaveButton();

			signInSmartCatAccount(accountUniqueName);

			Assert.IsTrue(_workspacePage.IsMachineTranslationExistInMenu(),
				"Произошла ошибка:\n ссылка 'Machine Translation' должна быть в меню");

			_workspacePage.GoToMachineTranslationPage();

			Assert.IsTrue(_fastMTAddFilesPage.IsAddFilesPageOpened(),
						"Произошла ошибка:\n страница 'Machine Translation не открылась");
		}

		private void signInSmartCatAccount(string accountName)
		{
			_signInPage
				.GetPage()
				.SubmitForm(ThreadUser.Login, ThreadUser.Password);

			_selectAccountForm.SelectAccount(accountName);

			_workspacePage.SetLocale();
		}

		protected AdminSignInPage _adminSignInPage;
		protected AdminLingvoProPage _adminLingvoProPage;
		protected AdminEnterpriseAccountsPage _adminEnterpriseAccountsPage;
		protected AdminCreateAccountPage _adminCreateAccountPage;
	}
}
