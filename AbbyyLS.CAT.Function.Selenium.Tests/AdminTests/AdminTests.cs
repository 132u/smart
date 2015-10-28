﻿using System;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	public class AdminTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUpCommonTest()
		{
			if (Standalone)
			{
				Assert.Ignore("Тест игнорируется, так как это отделяемое решение");
			}
		}

		/// <summary>
		/// Добавить пользователя в аккаунт
		/// </summary>
		protected void AddUserToAccount(string login)
		{
			AdminPage.OpenUserManagementPage();
			AdminPage.FillUserNameSearch(login);
			AdminPage.ClickSearchUserBtn();
			AdminPage.WaitSearchUserResult();
			AdminPage.ClickAddUser();
			if (AdminPage.CheckAccountAlreadyAdded())
			{
 				Logger.Trace("Попытка повторно добавить в аккаунт пользователя с логином: " + login);
			}
		}

		/// <summary>
		/// Перейти на страницу корпоративных аккаунтов
		/// </summary>
		protected void SwitchEnterpriseAccountList()
		{
			// Зайти в аккаунты
			AdminPage.ClickOpenEntepriseAccounts();
		}

		/// <summary>
		/// Заполнить основные поля создания аккаунта
		/// </summary>
		/// <param name="testAccount"> Аккаунт для бобби или обычный корп аккаунт </param>
		/// <param name="workFlow"> Включение функции workflow для аккаунта </param>
		/// <param name="venture"> Затея </param>
		/// <returns>имя аккаунта</returns>
		public string FillGeneralAccountFields(string testAccount = "", bool workFlow = false, string venture = "SmartCAT")
		{
			// Заполнить форму аккаунта
			var uniqPref = DateTime.Now.Ticks.ToString();
			string accountName = (testAccount == "") ? "TestAccount" + uniqPref : testAccount;

			// Название
			AdminPage.FillAccountName(accountName);

			// Выбрали затею
			AdminPage.SetVenture(venture);

			// Затея
			//AdminPage.SetVenture(Driver.Url.Contains("stage1") ? "Perevedem.ru" : "SmartCAT");

			// Поддомен
			AdminPage.FillSubdomainName("testaccount" + uniqPref);

			if (workFlow)
			{
				AdminPage.CheckWorkflowCheckbox();
				AcceptWorkflowModalDialog();
			}

			// Вернуть имя аккаунта
			return accountName;
		}

		/// <summary>
		/// Зайти в  админку
		/// </summary>
		public void LoginToAdminPage()
		{
			GoToAdminPage();
			AdminPage.FillLogin(Login);
			AdminPage.FillPassword(Password);
			AdminPage.ClickSubmit();
		}

		/// <summary>
		/// Создать новый активный персональный аккаунт в админке для только что созданного юзера
		/// </summary>
		/// <param name="surname">фамилия</param>
		/// <param name="state">статус перс аккаунта (активный или неактивный)</param>
		public void CreateNewPersonalAccount(string surname, bool state)
		{
			if (AdminPage.CheckEditPersonalAccountButtonExists())
			{
				Logger.Trace("Персональный аккаунт для пользователя " + surname + " уже существует.");
				AdminPage.ClickEditPersonalAccountBtn();
			}
			else
			{
				AdminPage.ClickCreatePersonalAccBtnForNewUser();
			}
			AdminPage.FillSurname(surname);
			if (state ^ AdminPage.IsSelectedActiveCheckbox())
			{
				AdminPage.SelectCheckbox();
			}
			AdminPage.ClickSaveBtnPersAcc();
		}

		/// <summary>
		/// Создание нового юзера
		/// </summary>
		/// <param name="emailForNewUser"> email </param>
		/// <param name="nickNameForNewUser"> nickname </param>
		/// <param name="password"> пароль </param>
		/// <param name="admin"> админ или нет </param>
		public void CreateNewUserInAdminPage(string emailForNewUser, string nickNameForNewUser, string password, bool admin = false)
		{
			AdminPage.ClickCreateNewUserBtn();
			AdminPage.FillEmailForNewUser(emailForNewUser);
			AdminPage.FillNickNameForNewUser(nickNameForNewUser);
			AdminPage.FillPasswordForNewUser(password);
			AdminPage.FillConfirmPasswordForNewUser(password);
			AdminPage.ClickSubmitBtnNewUser();
			
			if (AdminPage.GetIsUserIsExistMsgDisplay())
				Logger.Trace("Пользователь " + emailForNewUser + " уже есть в AOL, добавлен в БД");
			else if (admin)
			{
				AdminPage.CheckIsAdminCheckbox();
				AdminPage.ClickSubmitBtnNewUser();
			}
		}

		public string[] Features = {"Clients", "LingvoDictionaries", "Crowd", "Domains", "LiveChat", "TranslateConnector" };
	}
}
