using System;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class VendorsManagementPage : AdminLingvoProPage, IAbstractPage<VendorsManagementPage>
	{
		public VendorsManagementPage(WebDriver driver)
			: base(driver)
		{
		}

		public new VendorsManagementPage LoadPage()
		{
			if (!IsVendorsManagementPageOpened())
			{
				throw new Exception("Произошла ошибка: не открылась страница управления вендорами");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести название вендора в поле поиска
		/// </summary>
		/// <param name="vendorName">название вендора</param>
		public VendorsManagementPage FillSearchButton(string vendorName)
		{
			CustomTestContext.WriteLine("Ввести название вендора в поле поиска");
			SearchField.SetText(vendorName);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку поиска
		/// </summary>
		public VendorsManagementPage ClickSearchButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку поиска");
			SearchButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку добавить
		/// </summary>
		public VendorsManagementPage ClickAddButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку добавить");
			AddButton.Click();

			return LoadPage();
		}
		/// <summary>
		/// Нажать чекбокс вендора
		/// </summary>
		public VendorsManagementPage ClickVendorCheckbox(string vendor)
		{
			CustomTestContext.WriteLine("Нажать чекбокс вендора {0}", vendor);
			VendorCheckbox = Driver.SetDynamicValue(How.XPath, VENDOR_CHECKBOX, vendor);
			VendorCheckbox.Click();

			return LoadPage();
		}
		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Добавить вендоры
		/// </summary>
		/// <param name="vendorsName">список вендоров</param>
		public VendorsManagementPage AddVendors(List<string> vendorsName)
		{
			for (int i = 0; i < vendorsName.Count; i++)
			{
				FillSearchButton(vendorsName[i]);
				ClickSearchButton();
				ClickVendorCheckbox(vendorsName[i]);
				ClickAddButton();
			}
			
			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница управления вендорами
		/// </summary>
		public bool IsVendorsManagementPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SEARCH_BUTTON)]
		protected IWebElement SearchButton { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_FIELD)]
		protected IWebElement SearchField { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddButton { get; set; }
		protected IWebElement VendorCheckbox { get; set; }
		#endregion

		#region Описания XPath элементов

		protected const string SEARCH_BUTTON = "//input[@id='findVendor']";
		protected const string ADD_BUTTON = "//input[@id='addVendorsBtn']";
		protected const string SEARCH_FIELD = "//input[@id='searchText']";
		protected const string VENDOR_CHECKBOX = "//td[text()='*#*']/..//input[@name='vendorAccountIds']";
		#endregion
	}
}
