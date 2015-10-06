using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class AddAccessRightDialog : UsersRightsPage, IAbstractPage<AddAccessRightDialog>
	{
		public AddAccessRightDialog(WebDriver driver) : base(driver)
		{
		}

		public new AddAccessRightDialog GetPage()
		{
			var addAccessRightPage = new AddAccessRightDialog(Driver);
			InitPage(addAccessRightPage, Driver);

			return addAccessRightPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ADD_ACCESS_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти на вкладку 'Пользователи и права'.");
			}
		}

		/// <summary>
		/// Выбрать из списка право на создание проектов
		/// </summary>
		/// <param name="right">право</param>
		public AddAccessRightDialog ClickRightRadio(RightsType right)
		{
			CustomTestContext.WriteLine("Выбрать из списка право {0}.", right);

			RightRadio = Driver.SetDynamicValue(How.XPath, RIGHT_RADIO, right.ToString());
			RightRadio.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать набор для всех проектов
		/// </summary>
		public AddAccessRightDialog ClickForAnyProjectRadio()
		{
			CustomTestContext.WriteLine("Выбрать набор 'для всех проектов'.");
			ForAnyProjectRadio.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку далее (при добавлении прав пользователя)
		/// </summary>
		public AddAccessRightDialog ClickNextButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку далее (при добавлении прав пользователя).");
			NextButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Добавить" (право)
		/// </summary>
		public UsersRightsPage ClickAddRightButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Добавить' (право).");
			AddRightButton.Click();

			return new UsersRightsPage(Driver).GetPage();
		}

		[FindsBy(How = How.XPath, Using = ADD_RIGHT_BTN_XPATH)]
		protected IWebElement AddRightButton { get; set; }

		[FindsBy(How = How.XPath, Using = FOR_ANY_PROJECT_RADIO_XPATH)]
		protected IWebElement ForAnyProjectRadio { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BTN_XPATH)]
		protected IWebElement NextButton { get; set; }

		protected IWebElement RightRadio { get; set; }

		protected const string ADD_ACCESS_DIALOG = "//div[contains(@class,'js-add-access-right-popup')][2]";
		protected const string ADD_RIGHT_BTN_XPATH = "//div[contains(@class, 'add-access-right-popup')][2]//span[contains(@data-bind, 'visible : canFinishWizard, click : finishWizard')]//a[string() = 'Add']";
		protected const string NEXT_BTN_XPATH = "//div[contains(@class, 'add-access-right-popup')][2]//span[contains(@data-bind, 'click : moveToNextStep')]//a[string() = 'Next']";
		protected const string FOR_ANY_PROJECT_RADIO_XPATH = "//div[contains(@class, 'add-access-right-popup')][2]//div[contains(@data-bind, 'hasUnrestrictedAccessScope')]//input";
		protected const string RIGHT_RADIO = "//input[@id='*#*']";
	}
}
