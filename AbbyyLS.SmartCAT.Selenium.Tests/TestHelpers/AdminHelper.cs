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
			_adminSignInPage.SetLogin(login).SetPassword(password).ClickSubmitBtn();

			return this;
		}

		/// <summary>
		/// Проверяем, есть нужный аккаунт с заданной затеей, если нет создаем
		/// </summary>
		/// <param name="venture">затея (Perevedem)</param>
		/// <param name="accountName">имя аккаунта</param>
		public AdminHelper CheckOrCreateAccount(string venture, string accountName)
		{
			BaseObject.InitPage(_adminLingvoProPage);
			_adminLingvoProPage.ClickEnterpriseAccountsRef().ChooseVenture(venture);

			if (!_adminEnterpriseAccountsPage.IsAccountExists(accountName))
			{
				//TODO: CreateAccount
			}
			_adminEnterpriseAccountsPage.ClickManageUsersRef(accountName);

			return this;
		}

		/// <summary>
		/// Проверяем, есть ли пользователь в аккаунте, если нет, добавляем его как администратора (пользователь есть в базе)
		/// </summary>
		/// <param name="userEmail">email пользователя</param>
		/// <param name="userName">имя пользователя</param>
		/// <param name="userSurname">фамилия пользователя</param>
		public AdminHelper CheckOrCreateAccountAdmin(string userEmail, string userName, string userSurname)
		{
			BaseObject.InitPage(_adminEnterpriseAccountUsersPage);
			if (!_adminEnterpriseAccountUsersPage.IsUserAddedIntoAccount(userEmail))
			{
				_adminEnterpriseAccountUsersPage.SetEmailToFindUserInput(userEmail)
					.ClickFindUserBtn()
					.AssertUserFound(userEmail)
					.SetUserSurname(userEmail, userSurname)
					.SetUserName(userEmail, userName)
					.ClickAddUserBtn();
			}

			return this;
		}

		private readonly AdminSignInPage _adminSignInPage = new AdminSignInPage();
		private readonly AdminLingvoProPage _adminLingvoProPage = new AdminLingvoProPage();
		private readonly AdminEnterpriseAccountsPage _adminEnterpriseAccountsPage = new AdminEnterpriseAccountsPage();
		private readonly AdminEnterpriseAccountUsersPage _adminEnterpriseAccountUsersPage = new AdminEnterpriseAccountUsersPage();
	}
}
