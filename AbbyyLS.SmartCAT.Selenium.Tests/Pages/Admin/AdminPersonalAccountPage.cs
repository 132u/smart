using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminPersonalAccountPage : AdminLingvoProPage, IAbstractPage<AdminPersonalAccountPage>
	{
		public new AdminPersonalAccountPage GetPage()
		{
			var adminPersonalAccountPage = new AdminPersonalAccountPage();
			InitPage(adminPersonalAccountPage);

			return adminPersonalAccountPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(INPUT_SURNAME)))
			{
				Assert.Fail("Произошла ошибка:\n не загружена страница создания аккаунта.");
			}
		}

		/// <summary>
		/// Запонить поле 'Фамилия', когда создается новый перс аккаунт
		/// </summary>
		/// <param name="surname">фамилия</param>
		public AdminPersonalAccountPage FillSurname(string surname)
		{
			Logger.Trace("Ввести фамилию '{0}' при создании персонального аккаунта", surname);
			Surname.SetText(surname);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что стоит галочка в чекбоксе 'Active'
		/// </summary>
		public bool IsSelectedActiveCheckbox()
		{
			Logger.Trace("Проверить, что стоит галочка в чекбоксе 'Active'");
			return ActiveCheckbox.GetIsInputChecked();
		}

		/// <summary>
		/// Кликнуть по чекбоксу Active
		/// </summary>
		public AdminPersonalAccountPage SelectActiveCheckbox()
		{
			Logger.Trace("Кликнуть по чекбоксу 'Active'");
			ActiveCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Сохранить' при создании перс аккаунт
		/// </summary>
		public AdminPersonalAccountPage ClickSaveButtonPersonalAccount()
		{
			Logger.Trace("Нажать кнопку 'Сохранить' при создании персонального аккаунта");
			SaveButton.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = INPUT_SURNAME)]
		protected IWebElement Surname { get; set; }

		[FindsBy(How = How.XPath, Using = ACTIVE_CHECKBOX)]
		protected IWebElement ActiveCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON_NEW_PEERS_ACC)]
		protected IWebElement SaveButton { get; set; }

		protected const string INPUT_SURNAME = "//input[(@id = 'Surname')]";
		protected const string ACTIVE_CHECKBOX = "//input[@type='checkbox' and @id='IsActive']";
		protected const string SAVE_BUTTON_NEW_PEERS_ACC = "//p[@class='submit-area']/input";
	}
}
