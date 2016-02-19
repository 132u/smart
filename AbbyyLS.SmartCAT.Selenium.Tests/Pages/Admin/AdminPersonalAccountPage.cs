using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminPersonalAccountPage : AdminLingvoProPage, IAbstractPage<AdminPersonalAccountPage>
	{
		public AdminPersonalAccountPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminPersonalAccountPage GetPage()
		{
			var adminPersonalAccountPage = new AdminPersonalAccountPage(Driver);
			InitPage(adminPersonalAccountPage, Driver);

			return adminPersonalAccountPage;
		}

		public new void LoadPage()
		{
			if (!IsAdminPersonalAccountPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загружена страница создания аккаунта.");
			}
		}

		#region Простые методы

		/// <summary>
		/// Запонить поле 'Фамилия', когда создается новый перс аккаунт
		/// </summary>
		/// <param name="surname">фамилия</param>
		public AdminPersonalAccountPage FillSurname(string surname)
		{
			CustomTestContext.WriteLine("Ввести фамилию '{0}' при создании персонального аккаунта", surname);
			Surname.SetText(surname);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу Active
		/// </summary>
		public AdminPersonalAccountPage SelectActiveCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу 'Active'");
			ActiveCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Сохранить' при создании перс аккаунт
		/// </summary>
		public AdminPersonalAccountPage ClickSaveButtonPersonalAccount()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Сохранить' при создании персонального аккаунта");
			SaveButton.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница создания персонального аккаунта
		/// </summary>
		public bool IsAdminPersonalAccountPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(INPUT_SURNAME));
		}

		/// <summary>
		/// Проверить, что стоит галочка в чекбоксе 'Active'
		/// </summary>
		public bool IsSelectedActiveCheckbox()
		{
			CustomTestContext.WriteLine("Проверить, что стоит галочка в чекбоксе 'Active'");

			return ActiveCheckbox.GetIsInputChecked();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = INPUT_SURNAME)]
		protected IWebElement Surname { get; set; }

		[FindsBy(How = How.XPath, Using = ACTIVE_CHECKBOX)]
		protected IWebElement ActiveCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON_NEW_PEERS_ACC)]
		protected IWebElement SaveButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string INPUT_SURNAME = "//input[(@id = 'Surname')]";
		protected const string ACTIVE_CHECKBOX = "//input[@type='checkbox' and @id='IsActive']";
		protected const string SAVE_BUTTON_NEW_PEERS_ACC = "//p[@class='submit-area']/input";

		#endregion
	}
}
