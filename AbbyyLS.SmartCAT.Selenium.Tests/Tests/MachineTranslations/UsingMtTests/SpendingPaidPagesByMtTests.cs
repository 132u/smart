using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.MachineTranslation;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.MachineTranslations.UsingMTTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class SpendingPaidPagesByMTTests<TWebDriverProvider> : BaseMTTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
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

			_billingPage = new BillingPage(Driver);
			_fastMTTextPage = new FastMTTextPage(Driver);

			_workspacePage.GoToBillingPage();
			_balanceBeforeTest = _billingPage.GetPagesBalance();


			if (_balanceBeforeTest <= 1)
			{
				throw new Exception(
					string.Format("Текущий баланс: {0}, а для прохождения теста должен быть больше 1",
						_balanceBeforeTest));
			}

			_billingPage.ClickLogo();
		}

		[Test, Description("S-7271"), ShortCheckList]
		public void SpendPaidPagesByMTTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new [] {PathProvider.DocumentFile},
				personalAccount: true,
				useMachineTranslation: true,
				useDefaultFreeOrPaidMT: false,
				usePaidMT: true);

			_workspacePage.GoToBillingPage();

			var balanceAfter = _billingPage.GetPagesBalance();

			Assert.IsTrue(balanceAfter < _balanceBeforeTest,
				"Произошла ошибка:\n баланс не уменьшился");
		}

		[Test, Description("S-29775"), ShortCheckList]
		public void SpendPaidPagesByFastMTTest()
		{
			_workspacePage.GoToMachineTranslationPage();

			_fastMTAddFilesPage.GoToTextTab();

			_fastMTTextPage
				.SetTranslationSettings()
				.SetTextToTranslate("test")
				.ClickTranslateButton();

			Assert.IsTrue(_fastMTTextPage.IsTranslationAppeared(),
				"Произошла ошибка:\n текст перевода не появился");

			_workspacePage.GoToBillingPage();

			var balanceAfter = _billingPage.GetPagesBalance();

			Assert.IsTrue(balanceAfter < _balanceBeforeTest,
				"Произошла ошибка:\n баланс не уменьшился");
		}

		private double _balanceBeforeTest;

		private FastMTTextPage _fastMTTextPage;
		private BillingPage _billingPage;
	}
}
