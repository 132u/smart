using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Vendors;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageVendorsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_vendorPage = new VendorPage(Driver);
			_editVendorDialog = new EditVendorDialog(Driver);
			_addRightsDialog = new AddRightsDialog(Driver);

			var groupName = Guid.NewGuid().ToString();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_userRightsHelper.CreateGroupWithoutSpecificRight(AdditionalUser.NickName, groupName, RightsType.VendorsManagement);

			_workspacePage.SignOut();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);

				_loginHelper = new LoginHelper(Driver);
				_loginHelper.Authorize(StartPage, AdditionalUser);

				_workspacePage.GoVendorsPage();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[Test]
		public void EditVendorTest()
		{
			var vendor = LoginHelper.TestVendorNames[0];
			var newVendorName = Guid.NewGuid().ToString();
			var contactPerson = Guid.NewGuid().ToString();
			var email = Guid.NewGuid() + "@mailforspam.com";
			var phoneNumber = "123456789";

			_vendorPage.OpenEditVendorDialog(vendor);
			_editVendorDialog.EditVendor(newVendorName, contactPerson, email, phoneNumber);

			Assert.AreEqual(newVendorName, _vendorPage.GetVendorName(vendorNumber: 1),
				"Произошла ошибка: неверное название поставщика услуг.");

			Assert.AreEqual(vendor, _vendorPage.GetVendorAccount(newVendorName),
				"Произошла ошибка: неверное название акканута поставщика услуг.");

			Assert.AreEqual(email, _vendorPage.GetEmail(newVendorName),
				"Произошла ошибка: неверный email поставщика услуг.");

			Assert.AreEqual(phoneNumber, _vendorPage.GetPhoneNumber(newVendorName),
				"Произошла ошибка: неверный номер телефона поставщика услуг.");

			Assert.AreEqual(contactPerson, _vendorPage.GetContactPerson(newVendorName),
				"Произошла ошибка: неверный контакт поставщика услуг.");
		}
		
		[Test]
		public void AddAccessRightVendorTest()
		{
			var vendor = LoginHelper.TestVendorNames[1];

			_vendorPage.OpenEditVendorDialog(vendor);
			_editVendorDialog.ClickAddRightsButton();

			_addRightsDialog
				.ClickRadioButton(RightsType.GlossarySearch)
				.ClickAddButton();

			Assert.AreEqual(RightsType.GlossarySearch.Description(), _editVendorDialog.GetAccessRightName(),
				"Произошла ошибка: право доступа '{0}' не добавилось.", RightsType.GlossarySearch.Description());

			_editVendorDialog.ClickSaveButton();

			_vendorPage.ExpandVendor(vendor);

			Assert.AreEqual(RightsType.GlossarySearch.Description(), _vendorPage.GetAccessRightName(vendor),
				"Произошла ошибка: право доступа '{0}' не добавилось.", RightsType.GlossarySearch.Description());
		}

		[Test]
		public void VendorVisibilityTest()
		{
			for (int i = 1; i < LoginHelper.TestVendorNames.Count; i++)
			{
				Assert.IsTrue(_vendorPage.IsVendorDisplayed(LoginHelper.TestVendorNames[i]),
					"Произошла ошибка: поставщика услуг {0} нет в таблице.", LoginHelper.TestVendorNames[i]);
			}
		}

		protected LoginHelper _loginHelper;
		protected UserRightsHelper _userRightsHelper;
		protected WorkspacePage _workspacePage;
		protected EditVendorDialog _editVendorDialog;
		protected VendorPage _vendorPage;
		protected AddRightsDialog _addRightsDialog;
	}
}
