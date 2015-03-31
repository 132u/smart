﻿using AbbyyLS.CAT.Function.Selenium.Tests;
using NUnit.Framework;
using NLog;

namespace AbbyyLS.CAT.Function.Selenium.Admin
{
	public class Admin : AdminTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary> 
		/// <param name="browserName">Название браузера</param>
		public Admin(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Старт тестов
		/// </summary>
		[SetUp]
		public void SetupTest()
		{
			LoginToAdminPage();
		}

		/// <summary>
		/// Создание корпоративного аккаунта TestAccount и добавление бобби и ринго в TestAccount 
		/// </summary>
		[Test]
		public void CreateCorpAccount()
		{
			CreateCorporateAccount("TestAccount", true);
			AddUserToCorpAccount(Login);

		}

		/// <summary>
		/// Создание персонального аккаунта для ringo
		/// </summary>
		[Test]
		public void CreatePersAccountForRingo()
		{
			// Создаем ринго пользователя
			CreateNewUserInAdminPage(Login2, UserName2, Password2, true);
			FindUser(Login2);
			CheckAdminCheckbox();
			CreateNewPersonalAccount(Login2, true);
			AddUserToSpecifyAccount(Login2, "TestAccount");
		}

		/// <summary>
		/// Создание корпоративного аккаунта Perevedem
		/// </summary>
		[Test]
		public void CreatePerevedemCorpAccount()
		{
			CreateCorporateAccount("Perevedem", true, "Perevedem.ru");
		}

		/// <summary>
		/// Создание аккаунта Coursera и пользователей Coursera
		/// </summary>
		[Test]
		public void CreateCourseraUsers()
		{
			CreateCorporateAccount("Coursera", true, "Coursera");
			foreach(var user in CourseraUserList)
			{
				CreateNewUserInAdminPage(user.Login, user.Login, user.Password);
				AddUserToSpecifyAccount(user.Login, "Coursera");
				Logger.Trace("Пользователь " + user.Login + " добавлен в аккаунт Coursera;\n");
			}
		}

		/// <summary>
		/// Создание персонального аккаунта для бобби
		/// </summary>
		[Test]
		public void CreatePersAccountForBobby()
		{
			FindUser(Login);
			CreateNewPersonalAccount("Personal", true);
		}

		/// <summary>
		/// Создание персонального аккаунта для batman
		/// </summary>
		[Test]
		public void CreatePersAccountForBatman()
		{
			// Создаем ринго пользователя
			CreateNewUserInAdminPage(TestRightsLogin, TestRightsUserName, TestRightsPassword, true);
			FindUser(TestRightsLogin);
			CheckAdminCheckbox();
			CreateNewPersonalAccount(TestRightsLogin, true);
			AddUserToSpecifyAccount(TestRightsLogin, "TestAccount");
		}
		
	}
}
