﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminCreateUserPage: AdminLingvoProPage, IAbstractPage<AdminCreateUserPage>
	{
		public new AdminCreateUserPage GetPage()
		{
			var adminCreateUserPage = new AdminCreateUserPage();
			InitPage(adminCreateUserPage);

			return adminCreateUserPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(INPUT_EMAIL)))
			{
				Assert.Fail("Произошла ошибка:\n не загружена страница создания пользователя.");
			}
		}

		/// <summary>
		/// Заполнить поле email для нового пользователя
		/// </summary>
		public AdminCreateUserPage FillEmail(string email)
		{
			Logger.Trace("Ввести email '{0}' для нового пользователя", email);
			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле Nickname для нового юзера
		/// </summary>
		public AdminCreateUserPage FillNickName(string nickname)
		{
			Logger.Trace("Ввести Nickname '{0}' для нового пользователя", nickname);
			NicknameInput.SetText(nickname);

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле пароль для нового юзера
		/// </summary>
		/// <param name="password">пароль</param>
		public AdminCreateUserPage FillPassword(string password)
		{
			Logger.Trace("Ввод пароля '{0}' для нового пользователя", password);
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле подтверждения пароля для нового юзера
		/// </summary>
		/// <param name="confirmPassword">пароль</param>
		public AdminCreateUserPage FillConfirmPassword(string confirmPassword)
		{
			Logger.Trace("Ввод подтверждения пароля '{0}' для нового пользователя", confirmPassword);
			ConfirmPassword.SetText(confirmPassword);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Submit' при создании нового юзера
		/// </summary>
		public AdminEditUserPage ClickSubmitButton()
		{
			Logger.Trace("Нажать кнопку 'Submit'.");
			SaveButton.Click();

			return new AdminEditUserPage();
		}

		/// <summary>
		/// Вернуть: сообщение "Пользователь с таким e-mail уже существует в AOL, теперь добавлен и в БД приложения"
		/// </summary>
		public bool GetIsUserExistMessageDisplay()
		{
			var isExist = Driver.ElementIsDisplayed(By.XPath(USER_IS_EXIST_MSG));

			if (isExist)
			{
				Logger.Trace("Пользователь уже есть в AOL, добавлен в БД");
			}

			return isExist; 
		}

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

		protected const string INPUT_EMAIL = "//input[(@id = 'EMail')]";
		protected const string NICKNAME = "//input[@id='Nickname']";
		protected const string PASSWORD = "//div[6]/input[@class='inputField']";
		protected const string CONFIRM_PASSWORD = "//div[8]/input[@class='inputField']";
		protected const string SAVE_BUTTON = "//p[@class='submit-area']/input";
		protected const string USER_IS_EXIST_MSG = "//fieldset//div[2]/span[contains(text(),'таким e-mail уже существует в AOL')]";
	}
}