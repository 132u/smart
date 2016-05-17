using System;
using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class LoginHelper
	{
		public WebDriver Driver { get; set; }

		public LoginHelper(WebDriver driver)
		{
			Driver = driver;
			_adminSignInPage = new AdminSignInPage(Driver);
			_signInPage = new SignInPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_courseraHomePage = new CourseraHomePage(Driver);
			_courseraSignInDialog = new CourseraSignInDialog(Driver);
			_registrationPage = new RegistrationPage(Driver);
		}

		public LoginHelper LogInSmartCat(
			string login,
			string nickName,
			string password,
			string accountName = "TestAccount")
		{
			_signInPage
				.SubmitForm(login, password)
				.SelectAccount(accountName, EuropeTestServerName);
				
			_workspacePage.SetUp(nickName, accountName);

			return this;
		}

		/// <summary>
		/// Авторизоваться в курсере
		/// </summary>
		/// <param name="login">логин</param>
		/// <param name="password">пароль</param>
		public LoginHelper LogInCoursera(
			string login,
			string password)
		{
			_courseraHomePage.ClickJoinButton();
			_courseraSignInDialog
				.LoginInCoursera(login, password)
				.ClickSigInButton();
			
			return this;
		}

		public LoginHelper Authorize(StartPage startPage, TestUser user, string accountName = TestAccountName)
		{
			if (ConfigurationManager.Standalone)
			{
				// Тесты запускаются на ОР
				_workspacePage.GetPage(user.StandaloneUrl);

				return this;
			}

			switch (startPage)
			{
				case StartPage.Admin:
					_adminSignInPage
						.GetPage()
						.SignIn(user.Login, user.Password);
					break;

				case StartPage.Registration:
					_registrationPage.GetPage();
					break;

				case StartPage.SignIn:
					_signInPage.GetPage();
					break;

				case StartPage.Workspace:
					_signInPage.GetPage();
					LogInSmartCat(user.Login, user.NickName, user.Password, accountName);
					break;

				case StartPage.PersonalAccount:
					_signInPage.GetPage();
					LogInSmartCat(user.Login, user.NickName, user.Password, "Personal");
					break;

				case StartPage.Coursera:
					_courseraHomePage.GetPage();
					break;

				default:
					throw new Exception(string.Format("Для данной стартовой страницы {0} в методе не существует сценария авторизации.", startPage));
			}
			return this;
		}
		
		private readonly AdminSignInPage _adminSignInPage;
		private readonly SignInPage _signInPage;
		private readonly CourseraHomePage _courseraHomePage;
		private readonly WorkspacePage _workspacePage;
		private readonly CourseraSignInDialog _courseraSignInDialog;
		private readonly RegistrationPage _registrationPage;

		public const string TestAccountName = "TestAccount";
		public const string PersonalAccountName = "Personal";
		public const string PerevedemAccountName = "Perevedem";
		public const string CourseraAccountName = "Coursera";
		public static List<string> TestVendorNames = new List<string> { "TestVendor1", "TestVendor2", "TestVendor3", "TestVendor4", "TestVendor5" };

		public const string PerevedemVenture = "Perevedem.ru";
		public const string SmartCATVenture = "SmartCAT";
		public const string CourseraVenture = "Coursera";
		
		public const string PersonalAccountSurname = "PersAccount";

		public const string EuropeTestServerName = "Europe";
		public const string USATestServerName = "USA";
	}
}
