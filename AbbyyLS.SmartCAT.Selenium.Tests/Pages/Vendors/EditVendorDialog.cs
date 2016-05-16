using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Vendors
{
	public class EditVendorDialog : WorkspacePage, IAbstractPage<EditVendorDialog>
	{
		public EditVendorDialog(WebDriver driver) : base(driver)
		{
		}

		public new EditVendorDialog LoadPage()
		{
			if (!IsEditVendorDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка: Не открылся диалог редактирования вендора.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести название вендора.
		/// </summary>
		/// <param name="vendorName">название вендора</param>
		public EditVendorDialog FillVendorName(string vendorName)
		{
			CustomTestContext.WriteLine("Заполнить название вендора {0}.", vendorName);
			VendorName.SetText(vendorName);

			return LoadPage();
		}

		/// <summary>
		/// Ввести контактную информацию.
		/// </summary>
		/// <param name="contactPerson">контактная информация</param>
		public EditVendorDialog FillContactPerson(string contactPerson)
		{
			CustomTestContext.WriteLine("Заполнить контактную информацию {0}.", contactPerson);
			ContactPerson.SetText(contactPerson);

			return LoadPage();
		}

		/// <summary>
		/// Ввести email.
		/// </summary>
		/// <param name="email">email</param>
		public EditVendorDialog FillEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email {0}.", email);
			Email.SetText(email);

			return LoadPage();
		}

		/// <summary>
		/// Ввести номер телефона.
		/// </summary>
		/// <param name="phoneNumber">номер телефона</param>
		public EditVendorDialog FillPhoneNumber(string phoneNumber)
		{
			CustomTestContext.WriteLine("Ввести номер телефона {0}.", PhoneNumber);
			PhoneNumber.SetText(phoneNumber);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения.
		/// </summary>
		public VendorPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения.");
			SaveButton.Click();

			return new VendorPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку добавления прав.
		/// </summary>
		public AddRightsDialog ClickAddRightsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку добавления прав.");
			AddRightsButton.Click();

			return new AddRightsDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Получить название права доступа.
		/// </summary>
		public string GetAccessRightName()
		{
			CustomTestContext.WriteLine("Получить название права доступа.");

			return AccessRightName.Text;
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Редактировать вендор.
		/// </summary>
		/// <param name="vendorName">название вендора</param>
		/// <param name="contactPerson">контактная информация</param>
		/// <param name="email">email</param>
		/// <param name="phoneNumber">номер телефона</param>
		public VendorPage EditVendor(string vendorName, string contactPerson, string email, string phoneNumber)
		{
			FillVendorName(vendorName);
			FillContactPerson(contactPerson);
			FillEmail(email);
			FillPhoneNumber(phoneNumber);
			
			return ClickSaveButton();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог редактирования вендора.
		/// </summary>
		public bool IsEditVendorDialogOpened()
		{
			return Driver.WaitUntilElementIsAppear(By.XPath(SAVE_BUTTON))
				&& Driver.WaitUntilElementIsEnabled(By.XPath(SAVE_BUTTON))
				&& Driver.WaitUntilElementIsEnabled(By.XPath(VENDOR_NAME))
				&& Driver.WaitUntilElementIsEnabled(By.XPath(CONTACT_PERSON))
				&& Driver.WaitUntilElementIsEnabled(By.XPath(PHONE_NUMBER));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = VENDOR_NAME)]
		protected IWebElement VendorName { get; set; }

		[FindsBy(How = How.XPath, Using = CONTACT_PERSON)]
		protected IWebElement ContactPerson { get; set; }

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PHONE_NUMBER)]
		protected IWebElement PhoneNumber { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_RIGHTS_BUTTON)]
		protected IWebElement AddRightsButton { get; set; }

		[FindsBy(How = How.XPath, Using = ACCESS_RIGHT_NAME)]
		protected IWebElement AccessRightName { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string VENDOR_NAME = "(//div[contains(@class, 'edit-vendor-popup')]//input[contains(@data-bind, 'vendorName')])[2]";
		protected const string CONTACT_PERSON = "(//div[contains(@class, 'edit-vendor-popup')]//input[contains(@data-bind, 'contactPerson')])[2]";
		protected const string EMAIL = "(//div[contains(@class, 'edit-vendor-popup')]//input[contains(@data-bind, 'email')])[2]";
		protected const string PHONE_NUMBER = "(//div[contains(@class, 'edit-vendor-popup')]//input[contains(@data-bind, 'phoneNumber')])[2]";
		protected const string SAVE_BUTTON = "(//div[contains(@class, 'edit-vendor-popup')]//div[contains(@data-bind, 'save')])[2]";
		protected const string ADD_RIGHTS_BUTTON = "(//div[contains(@class, 'edit-vendor-popup')]//div[contains(@data-bind, 'showAddAccessRightPopup')]//a)[2]";
		protected const string ACCESS_RIGHT_NAME = "(//div[contains(@class, 'edit-vendor-popup')]//ul[contains(@data-bind, 'vendorAccessRights')])[2]//span[contains(@data-bind, 'displayName')]";

		#endregion
	}
}
