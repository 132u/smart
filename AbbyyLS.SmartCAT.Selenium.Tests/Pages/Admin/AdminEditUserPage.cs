using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminEditUserPage : AdminLingvoProPage, IAbstractPage<AdminEditUserPage>
	{
		public new AdminEditUserPage GetPage()
		{
			var adminEditUserPage = new AdminEditUserPage();
			InitPage(adminEditUserPage);

			return adminEditUserPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(IS_ADMIN_CHECKBOX)))
			{
				Assert.Fail("Произошла ошибка:\n не загружена страница редактирования пользователя.");
			}
		}

		/// <summary>
		/// Кликнуть по чекбоксу 'Администратор'
		/// </summary>
		public AdminEditUserPage SelectAdminCheckbox()
		{
			Logger.Trace("Кликнуть по чекбоксу 'Администратор'");
			IsAdminCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить стоит ли галочка в чекбоксе 'Администратор'
		/// </summary>
		public bool GetIsAdminCheckboxIsChecked()
		{
			Logger.Trace("Проверить, что стоит ли галочка в чекбоксе 'Администратор'.");
			
			return IsAdminCheckbox.GetIsInputChecked();
		}

		/// <summary>
		/// Нажать кнопку 'Submit' при редактировании пользователя
		/// </summary>
		public AdminEditUserPage ClickSubmitButton()
		{
			Logger.Trace("Нажать кнопку 'Submit'.");
			SaveButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить наличие ссылки Редактировать (если ссылка есть, значит перс. аккаунт есть)
		/// </summary>
		public bool CheckEditPersonalAccountButtonExists()
		{
			Logger.Trace("Проверить наличие ссылки 'Редактировать'.");
			var isExists = Driver.ElementIsDisplayed(By.XPath(EDIT_PERS_ACCOUNT));

			if (isExists)
			{
				Logger.Trace("Персональный аккаунт для пользователя уже существует.");
			}

			return isExists;
		}

		/// <summary>
		/// Кликнуть по ссылке 'Редактировать' персонального аккаунта
		/// </summary>
		public AdminPersonalAccountPage ClickEditPersonalAccountButton()
		{
			Logger.Trace("Кликнуть по ссылке 'Редактировать' персонального аккаунта");
			EditPersonalAccount.Click();

			return new AdminPersonalAccountPage().GetPage();
		}

		/// <summary>
		/// Нажать кнопку Создать перс аккаунт для нового юзера
		/// </summary>
		public AdminPersonalAccountPage ClickCreatePersonalAccountButton()
		{
			Logger.Trace("Нажать кнопку 'Создать персональный аккаунт'");
			CreatePersonalAccount.Click();

			return new AdminPersonalAccountPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = IS_ADMIN_CHECKBOX)]
		protected IWebElement IsAdminCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_PERS_ACCOUNT)]
		protected IWebElement EditPersonalAccount { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_PERS_ACCOUNT)]
		protected IWebElement CreatePersonalAccount { get; set; }

		protected const string IS_ADMIN_CHECKBOX = "//input[(@id = 'isAdmin')]";
		protected const string SAVE_BUTTON = "//p[@class='submit-area']/input";
		protected const string EDIT_PERS_ACCOUNT = "//div[@class='b-form']/p[1]/a[1]";
		protected const string CREATE_PERS_ACCOUNT = "//form[@action='/Users/CreatePersonalAccount']/input[2]";
	}
}
