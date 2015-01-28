using NUnit.Framework;
using System;

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

		[SetUp]
		public void SetUp()
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
			//проверить, появилось ли сообщение о повторном добавлении пользователя в аккаунт
			if (AdminPage.CheckAccountAlreadyAdded())
			{
				//если пользователь уже есть в аккаунте, пишем об этом в лог.
 				Logger.Trace("Попытка повторно добавить в аккаунт пользователя с логином: " + login);
			}
		}

		/// <summary>
		/// Найти пользователя по email
		/// </summary>
		/// <param name="email"> email </param>
		public void FindUser(string email)
		{
			AdminPage.ClickSearchUserLink();
			AdminPage.FillUserNameSearch(email);
			AdminPage.ClickFindBtn();
			AdminPage.ClickEmailInSearchResultTable(email);
		}

		/// <summary>
		/// Чекнуть Admin чекбокс
		/// </summary>
		public void CheckAdminCheckbox()
		{
			if (!AdminPage.GetIsAdminCheckboxIsChecked())
				AdminPage.ChechIsAdminCheckbox();
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
			var isWindowWithForm = AdminPage.GetIsAddAccountFormDisplay();

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
		/// <param name="testAccount"> Аккаунт для бобби или обычный корп аккаунт </param>
		/// <param name="ventureId"> Имя затеи </param>
		/// <param name="workFlow"> Workflow чекбокс </param>
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
		public void CreateNewPersAcc(string surname, bool state)
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
			if (!AdminPage.GetIsUserIsExistMsgDisplay() && admin)
			{
				AdminPage.ChechIsAdminCheckbox();
				AdminPage.ClickSubmitBtnNewUser();
			}
			else if (AdminPage.GetIsUserIsExistMsgDisplay())
			{
				Logger.Trace("Пользователь "+emailForNewUser+" уже есть в AOL, добавлен в БД");
			}
		}

		/// <summary>
		/// Добавить польователя в корп аккаунт
		/// </summary>
		/// <returns>Имя созданного аккаунта</returns>
		public void AddUserToCorpAccount(string userEmail)
		{
			AddUserToAccount(userEmail);
		}

		/// <summary>
		/// Создание корпоративного аккаунта
		/// </summary>
		/// <param name="testAccount"> Аккаунт для бобби или обычный корп аккаунт </param>
		/// <returns></returns>
		public string CreateCorpAccount(string testAccount = "", bool workflow = false, string venture = "SmartCAT")
		{
			SwitchEnterpriseAccountList();
			// Нажать Создать
			AdminPage.ClickAddAccount();
			// Переключаемся в новое окно браузера
			Driver.SwitchTo().Window(Driver.WindowHandles[1]);
			bool isWindowWithForm = AdminPage.GetIsAddAccountFormDisplay();
			Assert.IsTrue(isWindowWithForm, "Ошибка: не нашли окно с формой создания аккаунта");
			// Заполняем поля для создания корп аккаунта
			string accountName = FillGeneralAccountFields(testAccount, workflow, venture);
			// Выбрать функции
			SelectFeatures();
			// Выбрать дату окончания действия словаря
			SelectExpDate();
			// Нажать кнопку сохранить
			AdminPage.ClickSaveBtn();
			//проверяем,появилось ли сообщение о том,что такой аккаунт уже существует
			if (AdminPage.GetCorpAccountExists())
			{
				//если существует, вызываем метод,который закрывает окно добавления аккаунта и переключается на старое окно
				AdminPage.CloseCurrentWindow();
				//пишем в лог, что аккаунт с таким именем уже есть
				Logger.Trace("Корп. аккаунт с именем " + testAccount + " уже существует.");
			}
			return accountName;
		}

		/// <summary>
		/// Добавить пользователя в конкретный корп аккаунт на стр Корпоративных аккаунтов
		/// </summary>
		/// <param name="login"> логин пользователя</param>
		/// <param name="account"> название аккаунта </param>
		public void AddUserToSpecifyAccount(string login, string account)
		{
			SwitchEnterpriseAccountList();
			AdminPage.ClickUserAddBtnInCorpList(account);
			AddUserToCorpAccount(login);
		}

		public void SelectFeatures()
		{
			foreach (string f in Features)
			{
				if (f == "LiveChat")
					continue;

				AdminPage.SelectFeature(f);
				AdminPage.ClickRightArrowToAddFeature();
			}
		}

		public string[] Features = {"Clients", "LingvoDictionaries", "Crowd", "Domains", "HelpPage", "LiveChat", "TranslateConnector", "UserActivities" };

		public void SelectExpDate()
		{
			AdminPage.ClickCalendar();
			AdminPage.SelectYear("2015");
			AdminPage.SelectDay("14");
		}
	}
}
