﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Vendors
{
	public class AddRightsDialog : WorkspacePage, IAbstractPage<AddRightsDialog>
	{
		public AddRightsDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new AddRightsDialog LoadPage()
		{
			if (!IsAddRightsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка: Не открылся диалог добавления прав в вендор.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Выбрать право доступо.
		/// </summary>
		/// <param name="right">право доступо</param>
		public AddRightsDialog ClickRadioButton(RightsType right)
		{
			CustomTestContext.WriteLine("Выбрать право доступо {0}.", right);
			Driver.WaitUntilElementIsClickable(By.XPath(RADIO_BUTTON.Replace("*#*", right.ToString())));
			RadioButton = Driver.SetDynamicValue(How.XPath, RADIO_BUTTON, right.ToString());
			RadioButton.Click();

			return LoadPage();
		}


		/// <summary>
		/// Нажать кнопку Добавить.
		/// </summary>
		public EditVendorDialog ClickAddButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Добавить.");
			AddButton.Click();

			return new EditVendorDialog(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Дрбавить право доступа
		/// </summary>
		/// <param name="right">право доступа</param>
		public EditVendorDialog AddAccessRight(RightsType right)
		{
			ClickRadioButton(right);
			ClickAddButton();

			return new EditVendorDialog(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась диалог добавления прав в вендор.
		/// </summary>
		public bool IsAddRightsDialogOpened()
		{
			return Driver.WaitUntilElementIsAppear(By.XPath(CANCEL_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = RADIO_BUTTON)]
		protected IWebElement RadioButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string RADIO_BUTTON = "//input[@id='*#*']/following-sibling::em";
		protected const string CANCEL_BUTTON = "//a[contains(@class, 'cancel js-popup-close')]";
		protected const string ADD_BUTTON = "(//div[contains(@data-bind, 'finishWizard')]//a)[2]";

		#endregion

	}
}