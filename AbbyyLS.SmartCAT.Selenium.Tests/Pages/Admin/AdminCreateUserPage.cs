using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminCreateUserPage: AdminLingvoProPage, IAbstractPage<AdminCreateUserPage>
	{
		public AdminCreateUserPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminCreateUserPage LoadPage()
		{
			if (!IsAdminCreateUserPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загружена страница создания пользователя.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Заполнить поле email для нового пользователя
		/// </summary>
		public AdminCreateUserPage FillEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email '{0}' для нового пользователя", email);
			Email.SetText(email);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле Nickname для нового юзера
		/// </summary>
		public AdminCreateUserPage FillNickName(string nickname)
		{
			CustomTestContext.WriteLine("Ввести Nickname '{0}' для нового пользователя", nickname);
			NicknameInput.SetText(nickname);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле пароль для нового юзера
		/// </summary>
		/// <param name="password">пароль</param>
		public AdminCreateUserPage FillPassword(string password)
		{
			CustomTestContext.WriteLine("Ввод пароля '{0}' для нового пользователя", password);
			Password.SetText(password);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле подтверждения пароля для нового юзера
		/// </summary>
		/// <param name="confirmPassword">пароль</param>
		public AdminCreateUserPage FillConfirmPassword(string confirmPassword)
		{
			CustomTestContext.WriteLine("Ввод подтверждения пароля '{0}' для нового пользователя", confirmPassword);
			ConfirmPassword.SetText(confirmPassword);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Submit' при создании нового юзера, ожидая страницу редактирования
		/// </summary>
		public AdminEditUserPage ClickSubmitButtonExpectinEditUserPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Submit'.");
			SaveButton.Click();

			return new AdminEditUserPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Submit' при создании нового юзера
		/// </summary>
		public AdminCreateUserPage ClickSubmitButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Submit'.");
			SaveButton.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Заполнить основную информацию о новом пользователе
		/// </summary>
		/// <param name="email">email</param>
		/// <param name="nickName">ник</param>
		/// <param name="password">пароль</param>
		public AdminCreateUserPage FillGeneralInformationAboutNewUser(
			string email,
			string nickName,
			string password)
		{
			FillEmail(email);
			FillNickName(nickName);
			FillPassword(password);
			FillConfirmPassword(password);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница создания пользователя
		/// </summary>
		public bool IsAdminCreateUserPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(INPUT_EMAIL));
		}

		/// <summary>
		/// Вернуть: сообщение "Пользователь с таким e-mail уже существует в AOL, теперь добавлен и в БД приложения"
		/// </summary>
		public bool IsUserExistMessageDisplayed()
		{
			var isExist = Driver.ElementIsDisplayed(By.XPath(USER_IS_EXIST_MSG));

			if (isExist)
			{
				CustomTestContext.WriteLine("Пользователь уже есть в AOL, добавлен в БД");
			}

			return isExist;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = INPUT_EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = NICKNAME)]
		protected IWebElement NicknameInput { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_PASSWORD)]
		protected IWebElement ConfirmPassword { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string INPUT_EMAIL = "//input[(@id = 'EMail')]";
		protected const string NICKNAME = "//input[@id='Nickname']";
		protected const string PASSWORD = "//div[6]/input[@class='inputField']";
		protected const string CONFIRM_PASSWORD = "//div[8]/input[@class='inputField']";
		protected const string SAVE_BUTTON = "//p[@class='submit-area']/input";
		protected const string USER_IS_EXIST_MSG = "//fieldset//div[2]/span[contains(text(),'таким e-mail уже существует в AOL')]";

		#endregion
	}
}
