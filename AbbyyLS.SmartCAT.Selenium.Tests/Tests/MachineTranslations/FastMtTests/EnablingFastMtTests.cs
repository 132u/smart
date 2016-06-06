using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
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
			_adminHelper = new AdminHelper(Driver);
			_selectAccountForm = new SelectAccountForm(Driver);
			_signInPage = new SignInPage(Driver);

			var accountUniqueName = AdminHelper.GetAccountUniqueName();
			_adminHelper
				.CreateAccountIfNotExist(
					LoginHelper.SmartCATVenture,
					accountName: accountUniqueName,
					features: new List<string>() { Feature.FastMT.ToString() })
				.AddUserToAdminGroupInAccountIfNotAdded(ThreadUser.Login, ThreadUser.Name, ThreadUser.Surname, accountUniqueName);

			_signInPage
				.GetPage()
				.SubmitForm(ThreadUser.Login, ThreadUser.Password);

			_selectAccountForm.SelectAccount(accountUniqueName);

			_workspacePage.SetLocale();
		}

		[Test, Description("S-7273"), ShortCheckList]
		public void EnableFastMTByAdmin()
		{
			_workspacePage.GoToMachineTranslationPage();

			Assert.IsTrue(_fastMTAddFilesPage.IsAddFilesPageOpened(),
						"Произошла ошибка:\n страница 'Machine Translation не открылась");
		}

		private AdminHelper _adminHelper;
		private SelectAccountForm _selectAccountForm;
		private SignInPage _signInPage;
	}
}
