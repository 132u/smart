using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Vendors
{
	public class VendorPage : WorkspacePage, IAbstractPage<VendorPage>
	{
		public VendorPage(WebDriver driver) : base(driver)
		{
		}

		public new VendorPage LoadPage()
		{
			if (!IsVendorPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка: Не загрузилась страница поставщиков услуг.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Раскрыть вендор
		/// </summary>
		/// <param name="vendorName">название поставщика услуг</param>
		public VendorPage ExpandVendor(string vendorName)
		{
			CustomTestContext.WriteLine("Раскрыть вендор {0}.", vendorName);
			if (!IsVendorExpanded(vendorName))
			{
				VendorName = Driver.SetDynamicValue(How.XPath, VENDOR_NAME, vendorName);
				VendorName.Click();
			}

			if (!Driver.WaitUntilElementIsAppear(By.XPath(EDIT_BUTTON)))
			{
				throw new Exception("Произошла ошибка: Вендор не раскрылся.");
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования.
		/// </summary>
		public EditVendorDialog ClickEditButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования.");
			EditButton.Click();

			return new EditVendorDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Получить название поставщика услуг из таблицы.
		/// </summary>
		/// <param name="vendorName">номер поставщика услуг</param>
		public string GetVendorName(int vendorNumber)
		{
			CustomTestContext.WriteLine("Получить название поставщика №{0} услуг из таблицы.", vendorNumber);
			VendorNameValue = Driver.SetDynamicValue(How.XPath, VENDOR_NAME_VALUE, vendorNumber.ToString());

			return VendorNameValue.Text;
		}

		/// <summary>
		/// Получить название аккаунта поставщика услуг из таблицы.
		/// </summary>
		/// <param name="vendorName">название поставщика услуг</param>
		public string GetVendorAccount(string vendorName)
		{
			CustomTestContext.WriteLine("Получить название аккаунта поставщика услуг '{0}' из таблицы.", vendorName);
			VendorAccountValue = Driver.SetDynamicValue(How.XPath, VENDOR_ACCOUNT_VALUE, vendorName);

			return VendorAccountValue.Text;
		}

		/// <summary>
		/// Получить номер телефона поставщика услуг из таблицы.
		/// </summary>
		/// <param name="vendorName">название поставщика услуг<</param>
		public string GetPhoneNumber(string vendorName)
		{
			CustomTestContext.WriteLine("Получить номер телефона поставщика услуг '{0}' из таблицы.", vendorName);
			PhoneNumberValue = Driver.SetDynamicValue(How.XPath, PHONE_NUMBER_VALUE, vendorName);

			return PhoneNumberValue.Text;
		}

		/// <summary>
		/// Получить email поставщика услуг из таблицы.
		/// </summary>
		/// <param name="vendorName">название поставщика услуг</param>
		public string GetEmail(string vendorName)
		{
			CustomTestContext.WriteLine("Получить email поставщика услуг '{0}' из таблицы.", vendorName);
			EmailValue = Driver.SetDynamicValue(How.XPath, EMAIL_VALUE, vendorName);

			return EmailValue.Text;
		}

		/// <summary>
		/// Получить контакт поставщика услуг из таблицы.
		/// </summary>
		/// <param name="vendorName">название поставщика услуг</param>
		public string GetContactPerson(string vendorName)
		{
			CustomTestContext.WriteLine("Получить контакт поставщика услуг '{0}' из таблицы.", vendorName);
			ContactPersonValue = Driver.SetDynamicValue(How.XPath, CONTACT_PERSON_VALUE, vendorName);

			return ContactPersonValue.Text;
		}
		
		/// <summary>
		/// Получить название права доступа поставщика услуг.
		/// </summary>
		/// <param name="vendorName">название поставщика услуг</param>
		public string GetAccessRightName(string vendorName)
		{
			CustomTestContext.WriteLine("Получить название права доступа поставщика услуг {0}.", vendorName);
			AccessRightName = Driver.SetDynamicValue(How.XPath, ACCESS_RIGHT_NAME, vendorName);

			return AccessRightName.Text;
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Открыть диалог редактирования поставщика услуг.
		/// </summary>
		/// <param name="vendorName">название поставщика услуг</param>
		public EditVendorDialog OpenEditVendorDialog(string vendorName)
		{
			ExpandVendor(vendorName);

			return ClickEditButton();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница поставщиков услуг
		/// </summary>
		public bool IsVendorPageOpened()
		{
			return IsDialogBackgroundDisappeared()
				&& Driver.WaitUntilElementIsAppear(By.XPath(VENDOR_NAME_HEAD_COLUMN));
		}

		/// <summary>
		/// Проверить, что раскрыта панель поставщика услуг
		/// </summary>
		/// <param name="vendorName">имя  поставщика услуг</param>
		public bool IsVendorExpanded(string vendorName)
		{
			CustomTestContext.WriteLine(" Проверить, что раскрыта панель поставщика услуг {0}.", vendorName);
			VendorOpened = Driver.SetDynamicValue(How.XPath, VENDOR_OPENED, vendorName);

			return VendorOpened.GetAttribute("class").Contains("opened");
		}
		
		/// <summary>
		/// Проверить, что вендор присутствует в таблице
		/// </summary>
		/// <param name="vendorName">имя  поставщика услуг</param>
		public bool IsVendorDisplayed(string vendorName)
		{
			CustomTestContext.WriteLine(" Проверить, что поставщика услуг {0} присутствует в таблице.", vendorName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(VENDOR_NAME.Replace("*#*", vendorName)));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = VENDOR_NAME)]
		protected IWebElement VendorName { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_BUTTON)]
		protected IWebElement EditButton { get; set; }
		protected IWebElement VendorNameValue { get; set; }
		protected IWebElement VendorAccountValue { get; set; }
		protected IWebElement ContactPersonValue { get; set; }
		protected IWebElement EmailValue { get; set; }
		protected IWebElement PhoneNumberValue { get; set; }
		protected IWebElement VendorOpened { get; set; }
		protected IWebElement AccessRightName { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string VENDOR_NAME_HEAD_COLUMN = "//th[contains(text(), 'Vendor Name')]";
		protected const string VENDOR_NAME = "//td[contains(@data-bind, 'vendorName') and text()='*#*']";
		protected const string EDIT_BUTTON = "//div[contains(@data-bind, 'showEditPopup')]//a";
		protected const string VENDOR_NAME_VALUE = "//tbody[contains(@data-bind, 'vendors')]//tr[*#*]//td[1][contains(@data-bind, 'vendorName')]";
		protected const string VENDOR_ACCOUNT_VALUE = "//tbody[contains(@data-bind, 'vendors')]//td[text()='*#*']/following-sibling::td[contains(@data-bind, 'vendorAccount')]";
		protected const string PHONE_NUMBER_VALUE = "//tbody[contains(@data-bind, 'vendors')]//td[text()='*#*']/following-sibling::td//p[contains(@data-bind, 'phoneNumber')]";
		protected const string EMAIL_VALUE = "//tbody[contains(@data-bind, 'vendors')]//td[text()='*#*']/following-sibling::td[contains(@data-bind, 'email')]";
		protected const string CONTACT_PERSON_VALUE = "//tbody[contains(@data-bind, 'vendors')]//td[text()='*#*']/following-sibling::td[contains(@data-bind, 'contactPerson')]";
		protected const string VENDOR_OPENED = "//td[1][text()='*#*']/..";
		protected const string ACCESS_RIGHT_NAME = "//td[contains(@data-bind, 'vendorName') and text()='*#*']/../following-sibling::tr[2]//span[contains(@data-bind, 'displayName')]";

		#endregion
	}
}
