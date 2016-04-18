using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class AdminHelper
	{
		public WebDriver Driver { get; private set; }

		public AdminHelper(WebDriver driver)
		{
			Driver = driver;
			_adminFindUserPage = new AdminFindUserPage(Driver);
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
			_accountInvitationPage = new AccountInvitationPage(Driver);
			_adminEmailsSearchPage = new AdminEmailsSearchPage(Driver);
			_adminLetterPage = new AdminLetterPage(Driver);
		}

		public static string PublicDictionaryPackageName
		{
			get
			{
				return _publicDictionaryPackageName;
			}
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

			_adminLingvoProPage.ClickEnterpriseAccountsLink();

			_adminEnterpriseAccountsPage.ChooseVenture(venture);

			if (_adminEnterpriseAccountsPage.IsAccountExists(accountName))
			{
				_adminEnterpriseAccountsPage.ClickEditAccount(accountName);

				var featuresInAccount = _adminCreateAccountPage.GetFeaturesListInAccount();
				assertFeaturesListsMatch(featuresInAccount, features);
				_adminCreateAccountPage.ClickManagementPaidServicesReference();

				if (unlimitedUseServices)
				{
					_adminManagementPaidServicesPage.EnableUnlimitedUseIfNotEnabled();
				}
				else
				{
					_adminManagementPaidServicesPage.DisableUnlimitedUseIfNotDisabled();
				}

				_adminManagementPaidServicesPage.ClickEditAccountReference();

				_adminCreateAccountPage.ClickEnterpriseAccountsLink();

				_adminEnterpriseAccountsPage.ClickManageUsersReference(accountName);

				return this;
			}

			_adminEnterpriseAccountsPage
				.ClickCreateAccount()
				.SwitchToAdminCreateAccountWindow();

			_adminCreateAccountPage.FillAccountCreationForm(
				venture, accountName, workflow, features, packagesNeed, accountType);

			if (unlimitedUseServices)
			{
				_adminCreateAccountPage
					.ClickManagementPaidServicesReference()
					.ClickEnableUnlimitedUseServicesButton();

				if (!_adminManagementPaidServicesPage.IsUnlimitedUseServicesEnabled())
				{
					throw new Exception(
						"Произошла ошибка:\n не удалось включить безлимитное использование услуг");
				}

				_adminManagementPaidServicesPage.ClickEditAccountReference();
			}

			_adminCreateAccountPage
				.ClickEnterpriseAccountsLink()
				.ClickManageUsersReference(accountName);

			return this;
		}

		/// <summary>
		/// Проверить, есть ли пользователь в аккаунте, если нет, добавляем его как администратора (пользователь есть в базе)
		/// </summary>
		/// <param name="userEmail">email пользователя</param>
		/// <param name="userName">имя пользователя</param>
		/// <param name="userSurname">фамилия пользователя</param>
		/// <param name="accountName">имя аккааунта</param>
		public AdminHelper AddUserToAdminGroupInAccountIfNotAdded(
			string userEmail,
			string userName,
			string userSurname,
			string accountName)
		{
			goToManageUsersPage(accountName);
			
			_adminEnterpriseAccountUsersPage.AddExistedUserToAccountIfNotAdded(
				userEmail, userSurname, userName);

			return this;
		}

		/// <summary>
		/// Активировать пользователя
		/// </summary>
		/// <param name="email">email</param>
		public AdminHelper ActivateUser(string email)
		{
			_adminLingvoProPage.ClickAdminLettersSearchReference();
			_adminEmailsSearchPage
				.SetEmail(email)
				.ClickFindButton()
				.OpenLetter(email);

			var link = _adminLetterPage.GetLink();
			Driver.SwitchToNewTab();

			Driver.Navigate().GoToUrl(link);

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
			_adminLingvoProPage.ClickCreateUserReference();

			_adminCreateUserPage.FillGeneralInformationAboutNewUser(email, nickName, password);
			
			if (aolUser)
			{
				_adminCreateUserPage.ClickSubmitButton();
			}
			else
			{
				_adminCreateUserPage.ClickSubmitButtonExpectinEditUserPage();
			}
			
			if (!_adminCreateUserPage.IsUserExistMessageDisplayed() && admin)
			{
				_adminEditUserPage
					.SelectAdminCheckbox()
					.ClickSubmitButton();
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
			if (_adminEditUserPage.IsEditPersonalAccountButtonExists())
			{
				_adminEditUserPage.ClickEditPersonalAccountButton();
			}
			else
			{
				_adminEditUserPage.ClickCreatePersonalAccountButton();
			}

			_adminPersonalAccountPage.FillSurname(surname);

			if (state ^ _adminPersonalAccountPage.IsSelectedActiveCheckbox())
			{
				_adminPersonalAccountPage.SelectActiveCheckbox();
			}

			_adminPersonalAccountPage.ClickSaveButtonPersonalAccount();

			return this;
		}

		public static string GetAccountUniqueName()
		{
			return "AccountUniqueName" + DateTime.UtcNow.Ticks;
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
			_adminLingvoProPage.ClickDictionariesPackagesLink();

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

			if (_adminDictionariesPackages.IsDictionaryPackageExist(packageName))
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
			return _adminDictionaryPackage.GetIncludedDictionariesList();
		}

		public AdminHelper GoToDictionaryPackagePage(string packageName)
		{
			_adminLingvoProPage.ClickDictionariesPackagesLink();

			_adminDictionariesPackages.ClickDictionaryPackageName(packageName);

			return this;
		}

		public AdminHelper OpenEditModeForEnterpriceAccount(string accountName)
		{
			_adminLingvoProPage
				.ClickEnterpriseAccountsLink()
				.ClickEditAccount(accountName);

			return this;
		}

		/// <summary>
		/// Создать пользователя в заданном аккаунте и создать персональный аккаунт для пользователя
		/// </summary>
		/// <param name="email">email</param>
		/// <param name="name">имя</param>
		/// <param name="surname">фамилия</param>
		/// <param name="nickName">ник</param>
		/// <param name="password">пароль</param>
		/// <param name="accountName">название аккаунта</param>
		/// <param name="personalAccountActiveState">активен ли персональный аккаунт</param>
		/// <param name="aolUser">является ли AOL пользователем</param>
		public AdminHelper CreateUserWithSpecificAndPersonalAccount(
			string email,
			string name,
			string surname,
			string nickName,
			string password, 
			string accountName = LoginHelper.TestAccountName,
			bool personalAccountActiveState = true,
			bool aolUser = true)
		{
			CreateNewUser(email, nickName, password, admin: true, aolUser: aolUser);

			_adminFindUserPage.FindUser(email);

			_adminEditUserPage.CheckAdminCheckboxIfNotChecked();

			CreateNewPersonalAccount(email, state: personalAccountActiveState);

			AddUserToAdminGroupInAccountIfNotAdded(email, name, surname, accountName);

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

		/// <summary>
		/// Переход на страницу редактирования пользователей
		/// </summary>
		/// <param name="accountName">имя аккаунта</param>
		private void goToManageUsersPage(string accountName)
		{
			_adminLingvoProPage
					.ClickEnterpriseAccountsLink()
					.ClickManageUsersReference(accountName);
		}

		private const string _publicDictionaryPackageName = "Общедоступные";

		private readonly AdminFindUserPage _adminFindUserPage;
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
		private AdminEmailsSearchPage _adminEmailsSearchPage;
		private AdminLetterPage _adminLetterPage;
		protected AccountInvitationPage _accountInvitationPage;
	}
}
