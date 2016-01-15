using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class AdminHelper
	{
		public WebDriver Driver { get; private set; }

		public AdminHelper(WebDriver driver)
		{
			Driver = driver;
			_adminSignInPage = new AdminSignInPage(Driver);
			_adminLingvoProPage = new AdminLingvoProPage(Driver);
			_adminEnterpriseAccountsPage = new AdminEnterpriseAccountsPage(Driver);
			_adminEnterpriseAccountUsersPage = new AdminEnterpriseAccountUsersPage(Driver);
			_adminCreateUserPage = new AdminCreateUserPage(Driver);
			_adminEditUserPage = new AdminEditUserPage(Driver);
			_adminPersonalAccountPage = new AdminPersonalAccountPage(Driver);
			_adminCreateAccountPage = new AdminCreateAccountPage(Driver);
			_adminDictionariesPackages = new AdminDictionariesPackagesPage(Driver);
			_adminDictionaryPackage = new AdminDictionaryPackagePage(Driver);
			_adminManagementPaidServicesPage = new AdminManagementPaidServicesPage(Driver);
		}

		public static string PublicDictionaryPackageName
		{
			get
			{
				return _publicDictionaryPackageName;
			}
		}

		/// <summary>
		/// Логинимся в админку
		/// </summary>
		/// <param name="login">логин (email)</param>
		/// <param name="password">пароль</param>
		public AdminHelper SignIn(string login, string password)
		{
			BaseObject.InitPage(_adminSignInPage, Driver);
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
		/// <param name="features">список фич, значение null означает все фичи из множества Feature</param>
		/// <param name="packagesNeed">пакеты словарей</param>
		/// <param name="unlimitedUseServices">true - включить безлимитное использование услуг, иначе false</param>
		public AdminHelper CreateAccountIfNotExist(
			string venture = LoginHelper.SmartCATVenture,
			string accountName = null,
			bool workflow = false,
			List<string> features = null,
			bool packagesNeed = false,
			bool unlimitedUseServices = false,
			AccountType accountType = AccountType.LanguageServiceProvider)
		{
			accountName = accountName ?? "TestAccount" + new Guid();
			
			features = features ?? Enum.GetNames(typeof(Feature)).ToList();

			BaseObject.InitPage(_adminLingvoProPage, Driver);
			_adminLingvoProPage
				.ClickEnterpriseAccountsLink()
				.ChooseVenture(venture);

			BaseObject.InitPage(_adminEnterpriseAccountsPage, Driver);

			if (_adminEnterpriseAccountsPage.IsAccountExists(accountName))
			{
				_adminEnterpriseAccountsPage.ClickEditAccount(accountName);

				BaseObject.InitPage(_adminCreateAccountPage, Driver);

				var featuresInAccount = _adminCreateAccountPage.FeaturesListInAccount();
				assertFeaturesListsMatch(featuresInAccount, features);
				_adminCreateAccountPage.ClickManagementPaidServicesReference();

				BaseObject.InitPage(_adminManagementPaidServicesPage, Driver);

				if (unlimitedUseServices)
				{
					if (!_adminManagementPaidServicesPage.GetIsUnlimitedUnitsEnable())
					{
						_adminManagementPaidServicesPage
							.ClickEnableUnlimitedUseServicesButton()
							.AssertUnlimitedUseServicesEnabled()
							.ClickEditAccountReference();
					}
				}
				else
				{
					if (_adminManagementPaidServicesPage.GetIsUnlimitedUnitsEnable())
					{
						_adminManagementPaidServicesPage
							.ClickDisableUnlimitedUseServicesButton()
							.AssertUnlimitedUseServicesDisabled()
							.ClickEditAccountReference();
					}
				}

				_adminEnterpriseAccountsPage
					.ClickEnterpriseAccountsLink()
					.ClickManageUsersReference(accountName);

				return this;
			}

			_adminEnterpriseAccountsPage
					.ClickCreateAccount()
					.SwitchToAdminCreateAccountWindow()
					.FillAccountName(accountName)
					.SetVenture(venture)
					.FillSubdomainName(accountName)
					.SetEnterpriseAccountType(accountType);

			BaseObject.InitPage(_adminCreateAccountPage, Driver);

			if (workflow)
			{
				_adminCreateAccountPage
					.CheckWorkflowCheckbox()
					.AcceptWorkflowModalDialog();
			}

			addFeatures(features);
			_adminCreateAccountPage.ClickAddAllDisassemblesButton();

			if (features.Contains(Feature.LingvoDictionaries.ToString()))
			{
				_adminCreateAccountPage.SelectExpirationDate();

				if (packagesNeed)
				{
					_adminCreateAccountPage.AddAllPackages();
				}
			}

			_adminCreateAccountPage.ClickSaveButton();

			if (unlimitedUseServices)
			{
				_adminCreateAccountPage
					.ClickManagementPaidServicesReference()
					.ClickEnableUnlimitedUseServicesButton()
					.AssertUnlimitedUseServicesEnabled()
					.ClickEditAccountReference();
			}

			_adminCreateAccountPage
				.ClickEnterpriseAccountsLink()
				.ClickManageUsersReference(accountName);

			return this;
		}

		private void addFeatures(IList<string> features)
		{
			foreach (var feature in features)
			{
				_adminCreateAccountPage
					.SelectFeature(feature)
					.ClickRightArrowToAddFeature();
			}
		}

		/// <summary>
		/// Проверить, есть ли пользователь в аккаунте, если нет, добавляем его как администратора (пользователь есть в базе)
		/// </summary>
		/// <param name="userEmail">email пользователя</param>
		/// <param name="userName">имя пользователя</param>
		/// <param name="userSurname">фамилия пользователя</param>
		/// <param name="accountName">имя аккааунта</param>
		public AdminHelper CreateAccountAdminIfNotExist(
			string userEmail, 
			string userName, 
			string userSurname, 
			string accountName)
		{
			goToManageUsersPage(accountName);

			BaseObject.InitPage(_adminEnterpriseAccountUsersPage, Driver);

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
			bool admin = false,
			bool aolUser = false)
		{
			BaseObject.InitPage(_adminLingvoProPage, Driver);
			_adminLingvoProPage
				.ClickCreateUserReference()
				.FillEmail(email)
				.FillNickName(nickName)
				.FillPassword(password)
				.FillConfirmPassword(password);

			BaseObject.InitPage(_adminCreateUserPage, Driver);

			if (aolUser)
			{
				_adminCreateUserPage.ClickSubmitButton<AdminCreateUserPage>(Driver);
			}
			else
			{
				_adminCreateUserPage.ClickSubmitButton<AdminEditUserPage>(Driver);
			}
			
			if (!_adminCreateUserPage.GetIsUserExistMessageDisplay() && admin)
			{
				BaseObject.InitPage(_adminEditUserPage, Driver);
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
			BaseObject.InitPage(_adminLingvoProPage, Driver);
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
			BaseObject.InitPage(_adminEditUserPage, Driver);

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
			BaseObject.InitPage(_adminEditUserPage, Driver);

			if (_adminEditUserPage.CheckEditPersonalAccountButtonExists())
			{
				_adminEditUserPage.ClickEditPersonalAccountButton();
			}
			else
			{
				_adminEditUserPage.ClickCreatePersonalAccountButton();
			}

			BaseObject.InitPage(_adminPersonalAccountPage, Driver);
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

			BaseObject.InitPage(_adminEnterpriseAccountUsersPage, Driver);

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
			BaseObject.InitPage(_adminLingvoProPage, Driver);
			_adminLingvoProPage
					.ClickEnterpriseAccountsLink()
					.ClickManageUsersReference(accountName);

			return this;
		}
		
		/// <summary>
		/// Создать пакет словарей, если не существует
		/// </summary>
		/// <param name="dictionariesList">список словарей, null - все бесплатные словари</param>
		/// <param name="packageName">имя пакета</param>
		public AdminHelper CreateDictionaryPackageIfNotExist(
			List<string> dictionariesList = null, 
			string packageName = "Общедоступные")
		{
			goToLingvoDictionariesPage();

			if (dictionariesList == null)
			{
				dictionariesList = new List<string>
				{
					"Economics (De-Ru)", 
					"Economics (Ru-De)",
					"Explanatory (Uk-Uk)",
					"ForumDictionaryEn-Ru",
					"ForumDictionaryGe-Ru",
					"Law (De-Ru)",
					"Law (En-Ru)",
					"Law (Fr-Ru)",
					"Law (Ru-De)",
					"Law (Ru-En)",
					"Law (Ru-Fr)",
					"LingvoComputer (En-Ru)",
					"LingvoComputer (Ru-En)",
					"LingvoEconomics (En-Ru)",
					"LingvoEconomics (Ru-En)",
					"LingvoScience (En-Ru)",
					"LingvoScience (Ru-En)",
					"LingvoThesaurus (Ru-Ru)",
					"LingvoUniversal (En-Ru)",
					"Marketing (En-Ru)",
					"Medical (De-Ru)",
					"Medical (En-Ru)",
					"Medical (Ru-De)",
					"Medical (Ru-En)",
					"Polytechnical (It-Ru)",
					"Polytechnical (Ru-It)",
					"Pronouncing (Uk-Uk)",
					"Universal (De-Ru)",
					"Universal (En-Uk)",
					"Universal (Es-Ru)",
					"Universal (Fr-Ru)",
					"Universal (It-Ru)",
					"Universal (La-Ru)",
					"Universal (Ru-De)",
					"Universal (Ru-En)",
					"Universal (Ru-Es)",
					"Universal (Ru-Fr)",
					"Universal (Ru-It)",
					"Universal (Ru-Uk)",
					"Universal (Uk-En)",
					"Universal (Uk-Ru)"
				};
			}

			if (_adminDictionariesPackages.DictionaryPackageExist(packageName))
			{
				_adminDictionariesPackages
					.ClickDictionaryPackageName(packageName);

				assertDictionariesListsMatch(GetIncludedDictionariesList(), dictionariesList);

				return this;
			}

			_adminDictionariesPackages
					.ClickCreateDictionaryPackageLink()
					.FillDictionaryPackageName(packageName)
					.ClickPublicDictionaryCheckbox()
					.AddDictionariesToPackage(dictionariesList)
					.ClickCreateDictionaryPack();

			return this;
		}

		public List<string> GetIncludedDictionariesList()
		{
			BaseObject.InitPage(_adminDictionaryPackage, Driver);
			return _adminDictionaryPackage.IncludedDictionariesList();
		}

		public AdminHelper GoToDictionaryPackagePage(string packageName)
		{
			goToLingvoDictionariesPage();
			BaseObject.InitPage(_adminDictionariesPackages, Driver);
			_adminDictionariesPackages.ClickDictionaryPackageName(packageName);

			return this;
		}

		public AdminHelper OpenEditModeForEnterpriceAccount(string accountName)
		{
			BaseObject.InitPage(_adminLingvoProPage, Driver);
			_adminLingvoProPage
				.ClickEnterpriseAccountsLink()
				.ClickEditAccount(accountName);

			return this;
		}

		public AdminHelper AddAllDictionariesPackages()
		{
			BaseObject.InitPage(_adminCreateAccountPage, Driver);
			_adminCreateAccountPage
				.AddAllPackages()
				.ClickSaveButton();

			return this;
		}

		public AdminHelper CreateUserWithPersonalAccount(
			string email, 
			string nickName,
			string password, 
			string accountName = LoginHelper.TestAccountName,
			bool activeState = true,
			bool aolUser = true)
		{
			CreateNewUser(email, nickName, password, admin: true, aolUser: aolUser);
			FindUser(email);
			CheckAdminCheckbox();
			CreateNewPersonalAccount(email, state: activeState);
			AddUserToSpecificAccount(email, accountName);

			return this;
		}

		public AdminHelper CreateUserWithSpecificAccount(
			string email,
			string nickName,
			string password,
			string accountName = LoginHelper.TestAccountName)
		{
			CreateNewUser(email, nickName, password, admin: true, aolUser: true);
			FindUser(email);
			CheckAdminCheckbox();
			AddUserToSpecificAccount(email, accountName);

			return this;
		}

		private AdminHelper goToLingvoDictionariesPage()
		{
			BaseObject.InitPage(_adminLingvoProPage, Driver);
			_adminLingvoProPage.ClickDictionariesPackagesLink();

			return this;
		}

		private AdminHelper assertDictionariesListsMatch(
			List<string> firstDictionariesList, 
			List<string> secondDictionariesList)
		{
			Assert.IsTrue(firstDictionariesList.Match(secondDictionariesList),
				"Произошла ошибка:\n списки словарей не совпадают.");

			return this;
		}

		private AdminHelper assertFeaturesListsMatch(
			List<string> firstFeatureList, 
			List<string> secondFeatureList)
		{
			Assert.IsTrue(firstFeatureList.Match(secondFeatureList),
				"Произошла ошибка:\n списки фич не совпадают.");

			return this;
		}

		private const string _publicDictionaryPackageName = "Общедоступные";

		private readonly AdminSignInPage _adminSignInPage;
		private readonly AdminLingvoProPage _adminLingvoProPage;
		private readonly AdminEnterpriseAccountsPage _adminEnterpriseAccountsPage;
		private readonly AdminEnterpriseAccountUsersPage _adminEnterpriseAccountUsersPage;
		private readonly AdminCreateUserPage _adminCreateUserPage;
		private readonly AdminEditUserPage _adminEditUserPage;
		private readonly AdminPersonalAccountPage _adminPersonalAccountPage;
		private readonly AdminCreateAccountPage _adminCreateAccountPage;
		private readonly AdminDictionariesPackagesPage _adminDictionariesPackages;
		private readonly AdminDictionaryPackagePage _adminDictionaryPackage;
		private readonly AdminManagementPaidServicesPage _adminManagementPaidServicesPage;
	}
}
