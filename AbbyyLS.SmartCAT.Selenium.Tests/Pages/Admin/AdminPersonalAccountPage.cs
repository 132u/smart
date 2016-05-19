using System;
using System.Threading;
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

		public new AdminPersonalAccountPage LoadPage()
		{
			if (!IsAdminPersonalAccountPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загружена страница создания аккаунта.");
			}

			return this;
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

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу Active
		/// </summary>
		public AdminPersonalAccountPage SelectActiveCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу 'Active'");
			ActiveCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Навести курсор на кнопку 'Сохранить' при создании персонального аккаунта
		/// </summary>
		public AdminPersonalAccountPage HoverSavePersonalAccountButton()
		{
			CustomTestContext.WriteLine("Навести курсор на кнопку 'Сохранить' при создании персонального аккаунта");
			SaveButton.HoverElement();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Сохранить' при создании перс аккаунт
		/// </summary>
		public AdminPersonalAccountPage ClickSavePersonalAccountButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Сохранить' при создании персонального аккаунта");
			ActiveSaveButton.JavaScriptClick();

			return LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Навести курсор и нажать кнопку 'Сохранить' при создании перс аккаунт
		/// </summary>
		/// <returns></returns>
		public AdminPersonalAccountPage SavePersonalAccount()
		{
			HoverSavePersonalAccountButton();
			ClickSavePersonalAccountButton();

			return LoadPage();
		}

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

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = ACTIVE_SAVE_BUTTON)]
		protected IWebElement ActiveSaveButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string INPUT_SURNAME = "//input[(@id = 'Surname')]";
		protected const string ACTIVE_CHECKBOX = "//input[@type='checkbox' and @id='IsActive']";
		protected const string SAVE_BUTTON = "//p[@class='submit-area']/input";
		protected const string ACTIVE_SAVE_BUTTON = "//p[@class='submit-area']/input[contains(@class, 'ui-state-hover')]";

		#endregion
	}
}
