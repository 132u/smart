using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminEditUserPage : AdminLingvoProPage, IAbstractPage<AdminEditUserPage>
	{
		public AdminEditUserPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminEditUserPage LoadPage()
		{
			if (!IsAdminEditUserPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загружена страница редактирования пользователя.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Кликнуть по чекбоксу 'Администратор'
		/// </summary>
		public AdminEditUserPage SelectAdminCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу 'Администратор'");
			IsAdminCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Submit' при редактировании пользователя
		/// </summary>
		public AdminEditUserPage ClickSubmitButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Submit'.");
			SaveButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по ссылке 'Редактировать' персонального аккаунта
		/// </summary>
		public AdminPersonalAccountPage ClickEditPersonalAccountButton()
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке 'Редактировать' персонального аккаунта");
			EditPersonalAccount.Click();

			return new AdminPersonalAccountPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Создать перс аккаунт для нового юзера
		/// </summary>
		public AdminPersonalAccountPage ClickCreatePersonalAccountButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Создать персональный аккаунт'");
			CreatePersonalAccount.Click();

			return new AdminPersonalAccountPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Отметить чекбокс Admin, если не отмечен
		/// </summary>
		public AdminEditUserPage CheckAdminCheckboxIfNotChecked()
		{
			if (!IsAdminCheckboxChecked())
			{
				SelectAdminCheckbox();
			}

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница редактирования пользователя
		/// </summary>
		public bool IsAdminEditUserPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(IS_ADMIN_CHECKBOX));
		}

		/// <summary>
		/// Проверить стоит ли галочка в чекбоксе 'Администратор'
		/// </summary>
		public bool IsAdminCheckboxChecked()
		{
			CustomTestContext.WriteLine("Проверить, что стоит ли галочка в чекбоксе 'Администратор'.");

			return IsAdminCheckbox.GetIsInputChecked();
		}

		/// <summary>
		/// Проверить наличие ссылки Редактировать (если ссылка есть, значит перс. аккаунт есть)
		/// </summary>
		public bool IsEditPersonalAccountButtonExists()
		{
			CustomTestContext.WriteLine("Проверить наличие ссылки 'Редактировать'.");
			var isExists = Driver.ElementIsDisplayed(By.XPath(EDIT_PERS_ACCOUNT));

			if (isExists)
			{
				CustomTestContext.WriteLine("Персональный аккаунт для пользователя уже существует.");
			}

			return isExists;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = IS_ADMIN_CHECKBOX)]
		protected IWebElement IsAdminCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_PERS_ACCOUNT)]
		protected IWebElement EditPersonalAccount { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_PERS_ACCOUNT)]
		protected IWebElement CreatePersonalAccount { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string IS_ADMIN_CHECKBOX = "//input[(@id = 'isAdmin')]";
		protected const string SAVE_BUTTON = "//p[@class='submit-area']/input";
		protected const string EDIT_PERS_ACCOUNT = "//div[@class='b-form']/p[1]/a[1]";
		protected const string CREATE_PERS_ACCOUNT = "//form[@action='/Users/CreatePersonalAccount']/input[2]";

		#endregion
	}
}
