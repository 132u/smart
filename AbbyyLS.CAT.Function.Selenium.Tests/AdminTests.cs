using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	public class AdminTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public AdminTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Добавить пользователя в аккаунт
		/// </summary>
		protected void AddUserToAccount(string login)
		{
			// Перейти в управление пользователями
			AdminPage.OpenUserManagementPage();
			// Ввести логин пользователя
			AdminPage.FillUserNameSearch(login);
			// Найти
			AdminPage.ClickSearchUserBtn();
			// Дождаться появления пользователя
			AdminPage.WaitSearchUserResult();
			// Добавить
			AdminPage.ClickAddUser();
		}

		/// <summary>
		/// Открыть форму создания корпоративного аккаунта
		/// </summary>
		public void OpenCreateAccountForm()
		{
			LoginToAdminPage();
			// Зайти в корпоративные аккаунты
			SwitchEnterpriseAccountList();

			// Нажать Создать
			AdminPage.ClickAddAccount();
			Driver.SwitchTo().Window(Driver.WindowHandles[1]);
			bool isWindowWithForm = AdminPage.GetIsAddAccountFormDisplay();
			Assert.IsTrue(isWindowWithForm, "Ошибка: не нашли окно с формой создания аккаунта");
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
		/// <returns>имя аккаунта</returns>
		public string FillGeneralAccountFields()
		{
			// Заполнить форму аккаунта
			string uniqPref = DateTime.Now.Ticks.ToString();
			string accountName = "TestAccount" + uniqPref;
			// Название
			AdminPage.FillAccountName(accountName);

			// Затея		
			if (Driver.Url.Contains("stage1"))
				AdminPage.SetVenture("Perevedem.ru");
			else
				AdminPage.SetVenture("SmartCAT");

			// Поддомен
			AdminPage.FillSubdomainName("testaccount" + uniqPref);

			// Дата
			AdminPage.FillDeadLineDate(DateTime.Now.AddDays(10));

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
		public void CreateNewPersAcc(string surname, bool state)
		{
			AdminPage.ClickCreatePersonalAccBtnForNewUser();
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
		/// <param name="nickNameForNewUser"> никнэйм </param>
		/// <param name="password"> пароль </param>
		public void CreateNewUserInAdminPage(string emailForNewUser, string nickNameForNewUser, string password)
		{
			AdminPage.ClickCreateNewUserBtn();
			AdminPage.FillEmailForNewUser(emailForNewUser);
			AdminPage.FillNickNameForNewUser(nickNameForNewUser);
			AdminPage.FillPasswordForNewUser(password);
			AdminPage.FillConfirmPasswordForNewUser(password);
			AdminPage.ClickSubmitBtnNewUser();
		}

		/// <summary>
		/// Добавить польователя в корп аккаунт
		/// </summary>
		/// <returns>Имя созданного аккаунта</returns>
		public  string AddUserToCorpAccount()
		{
			SwitchEnterpriseAccountList();
			// Нажать Создать
			AdminPage.ClickAddAccount();
			// Переключаемся в новое окно браузера
			Driver.SwitchTo().Window(Driver.WindowHandles[1]);
			bool isWindowWithForm = AdminPage.GetIsAddAccountFormDisplay();
			Assert.IsTrue(isWindowWithForm, "Ошибка: не нашли окно с формой создания аккаунта");
			// Заполняем поля для создания корп аккаунта
			string accountName = FillGeneralAccountFields();
			// Нажать кнопку сохранить
			AdminPage.ClickSaveBtn();
			AddUserToAccount(RegistrationPage.Email);
			return accountName;
		}
	}
}
