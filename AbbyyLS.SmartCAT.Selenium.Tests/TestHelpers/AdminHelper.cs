using System;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class AdminHelper
	{
		/// <summary>
		/// Логинимся в админку
		/// </summary>
		/// <param name="login">логин (email)</param>
		/// <param name="password">пароль</param>
		public AdminHelper SignIn(string login, string password)
		{
			BaseObject.InitPage(_adminSignInPage);
			_adminSignInPage
				.SetLogin(login)
				.SetPassword(password)
				.ClickSubmitButton();

			return this;
		}

		/// <summary>
		/// Проверить, есть нужный аккаунт с заданной затеей, если нет создаем
		/// </summary>
		/// <param name="venture">затея</param>
		/// <param name="accountName">имя аккаунта</param>
		/// <param name="workflow">true - включить функцию Workflow, иначе false</param>
		public AdminHelper CreateAccountIfNotExist(string venture = LoginHelper.SmartCATVenture, string accountName = null, bool workflow = false)
		{
			accountName = accountName ?? "TestAccount" + new Guid();
			string[] features = 
			{
				"Clients",
				"LingvoDictionaries",
				"Crowd",
				"Domains",
				"TranslateConnector",
				"UserActivities"
			};

			BaseObject.InitPage(_adminLingvoProPage);
			_adminLingvoProPage
				.ClickEnterpriseAccountsReference()
				.ChooseVenture(venture);

			BaseObject.InitPage(_adminEnterpriseAccountsPage);

			if (!_adminEnterpriseAccountsPage.IsAccountExists(accountName))
			{
				_adminEnterpriseAccountsPage
					.ClickCreateAccount()
					.SwitchToAdminCreateAccountWindow()
					.FillAccountName(accountName)
					.SetVenture(venture)
					.FillSubdomainName(accountName);

				BaseObject.InitPage(_adminCreateAccountPage);

				if(workflow)
				{
					_adminCreateAccountPage
						.CheckWorkflowCheckbox()
						.AcceptWorkflowModalDialog();
				}

				foreach (var feature in features)
				{
					_adminCreateAccountPage
						.SelectFeature(feature)
						.ClickRightArrowToAddFeature();
				}

				_adminCreateAccountPage
					.SelectExpirationDate()
					.ClickSaveButton();

				return this;
			}

			return this;
		}

		/// <summary>
		/// Проверить, есть ли пользователь в аккаунте, если нет, добавляем его как администратора (пользователь есть в базе)
		/// </summary>
		/// <param name="userEmail">email пользователя</param>
		/// <param name="userName">имя пользователя</param>
		/// <param name="userSurname">фамилия пользователя</param>
		public AdminHelper CreateAccountAdminIfNotExist(string userEmail, string userName, string userSurname)
		{
			BaseObject.InitPage(_adminEnterpriseAccountUsersPage);

			goToManageUsersPage(LoginHelper.TestAccountName);

			if (!_adminEnterpriseAccountUsersPage.IsUserAddedIntoAccount(userEmail))
			{
				_adminEnterpriseAccountUsersPage
					.SetEmailToFindUserInput(userEmail)
					.ClickFindUserButton()
					.AssertUserFound(userEmail)
					.SetUserSurname(userEmail, userSurname)
					.SetUserName(userEmail, userName)
					.ClickAddUserButton();
			}

			return this;
		}

		/// <summary>
		/// Создание нового юзера
		/// </summary>
		/// <param name="email"> email </param>
		/// <param name="nickName"> nickname </param>
		/// <param name="password"> пароль </param>
		/// <param name="admin"> админ или нет </param>
		public AdminHelper CreateNewUser(
			string email,
			string nickName,
			string password,
			bool admin = false)
		{
			BaseObject.InitPage(_adminLingvoProPage);
			_adminLingvoProPage
				.ClickCreateUserReference()
				.FillEmail(email)
				.FillNickName(nickName)
				.FillPassword(password)
				.FillConfirmPassword(password)
				.ClickSubmitButton();

			BaseObject.InitPage(_adminCreateUserPage);

			if (!_adminCreateUserPage.GetIsUserExistMessageDisplay() && admin)
			{
				BaseObject.InitPage(_adminEditUserPage);
				_adminEditUserPage
					.SelectAdminCheckbox()
					.ClickSubmitButton();
			}

			return this;
		}

		/// <summary>
		/// Найти пользователя по email
		/// </summary>
		/// <param name="email"> email </param>
		public AdminHelper FindUser(string email)
		{
			BaseObject.InitPage(_adminLingvoProPage);
			_adminLingvoProPage
				.ClickSearchUserReference()
				.FillUserNameSearch(email)
				.ClickFindButton()
				.ClickEmailInSearchResultTable(email);

			return this;
		}

		/// <summary>
		/// Чекнуть Admin чекбокс
		/// </summary>
		public AdminHelper CheckAdminCheckbox()
		{
			BaseObject.InitPage(_adminEditUserPage);

			if (!_adminEditUserPage.GetIsAdminCheckboxIsChecked())
			{
				_adminEditUserPage.SelectAdminCheckbox();
			}

			return this;
		}

		/// <summary>
		/// Создать новый активный персональный аккаунт в админке для только что созданного юзера
		/// </summary>
		/// <param name="surname">фамилия</param>
		/// <param name="state">статус перс аккаунта (активный или неактивный)</param>
		public AdminHelper CreateNewPersonalAccount(string surname, bool state)
		{
			BaseObject.InitPage(_adminEditUserPage);

			if (_adminEditUserPage.CheckEditPersonalAccountButtonExists())
			{
				_adminEditUserPage.ClickEditPersonalAccountButton();
			}
			else
			{
				_adminEditUserPage.ClickCreatePersonalAccountButton();
			}

			BaseObject.InitPage(_adminPersonalAccountPage);
			_adminPersonalAccountPage.FillSurname(surname);

			if (state ^ _adminPersonalAccountPage.IsSelectedActiveCheckbox())
			{
				_adminPersonalAccountPage.SelectActiveCheckbox();
			}
			_adminPersonalAccountPage.ClickSaveButtonPersonalAccount();

			return this;
		}

		/// <summary>
		/// Добавить пользователя в конкретный корпоративный аккаунт на странице Корпоративных аккаунтов
		/// </summary>
		/// <param name="login"> логин пользователя</param>
		/// <param name="account"> название аккаунта </param>
		public AdminHelper AddUserToSpecificAccount(string login, string account)
		{
			goToManageUsersPage(account);

			BaseObject.InitPage(_adminEnterpriseAccountUsersPage);

			if (!_adminEnterpriseAccountUsersPage.IsUserAddedIntoAccount(login))
			{
				_adminEnterpriseAccountUsersPage
					.SetEmailToFindUserInput(login)
					.ClickFindUserButton()
					.AssertUserFound(login)
					.ClickAddUserButton();
			}

			return this;
		}

		public static string GetAccountUniqueName()
		{
			return "AccountUniqueName" + DateTime.UtcNow.Ticks;
		}

		/// <summary>
		/// Переход на страницу редактирования пользователей
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		private AdminHelper goToManageUsersPage(string accountName)
		{
			BaseObject.InitPage(_adminLingvoProPage);
			_adminLingvoProPage
					.ClickEnterpriseAccountsReference()
					.ClickManageUsersReference(accountName);

			return this;
		}

		private readonly AdminSignInPage _adminSignInPage = new AdminSignInPage();
		private readonly AdminLingvoProPage _adminLingvoProPage = new AdminLingvoProPage();
		private readonly AdminEnterpriseAccountsPage _adminEnterpriseAccountsPage = new AdminEnterpriseAccountsPage();
		private readonly AdminEnterpriseAccountUsersPage _adminEnterpriseAccountUsersPage = new AdminEnterpriseAccountUsersPage();
		private readonly AdminCreateUserPage _adminCreateUserPage = new AdminCreateUserPage();
		private readonly AdminEditUserPage _adminEditUserPage = new AdminEditUserPage();
		private readonly AdminPersonalAccountPage _adminPersonalAccountPage = new AdminPersonalAccountPage();
		private readonly AdminCreateAccountPage _adminCreateAccountPage = new AdminCreateAccountPage();
	}
}
