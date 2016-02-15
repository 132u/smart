using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class LoginHelper
	{
		public WebDriver Driver { get; private set; }

		public LoginHelper(WebDriver driver)
		{
			Driver = driver;
			_adminHelper = new AdminHelper(Driver);
			_adminSignInPage = new AdminSignInPage(Driver);
			_commonHelper = new CommonHelper(Driver);
			_signInPage = new SignInPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_courseraHomePage = new CourseraHomePage(Driver);
			_courseraSignInDialog = new CourseraSignInDialog(Driver);
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

		public LoginHelper Authorize(StartPage startPage, TestUser user)
		{
			if (ConfigurationManager.Standalone)
			{
				// Тесты запускаются на ОР
				_commonHelper.GoToWorkspaceUrl(user.StandaloneUrl);

				return this;
			}

			switch (startPage)
			{
				case StartPage.Admin:
					_commonHelper.GoToAdminUrl();
					_adminSignInPage.SignIn(user.Login, user.Password);
					break;

				case StartPage.CompanyRegistration:
					_commonHelper.GoToCompanyRegistration();
					break;

				case StartPage.FreelanceRegistration:
					_commonHelper.GoToFreelanceRegistratioin();
					break;

				case StartPage.SignIn:
					_commonHelper.GoToSignInPage();
					break;

				case StartPage.Workspace:
					_commonHelper.GoToSignInPage();
					LogInSmartCat(user.Login, user.NickName, user.Password);
					break;

				case StartPage.PersonalAccount:
					_commonHelper.GoToSignInPage();
					LogInSmartCat(user.Login, user.NickName, user.Password, "Personal");
					break;

				case StartPage.Coursera:
					_commonHelper.GoToCoursera();
					break;

				default:
					_commonHelper.GoToSignInPage();
					LogInSmartCat(user.Login, user.NickName, user.Password);
					break;
			}
			return this;
		}

		private readonly AdminHelper _adminHelper;
		private readonly AdminSignInPage _adminSignInPage;
		private readonly CommonHelper _commonHelper;
		private readonly SignInPage _signInPage;
		private readonly CourseraHomePage _courseraHomePage;
		private readonly WorkspacePage _workspacePage;
		private readonly CourseraSignInDialog _courseraSignInDialog;

		public const string TestAccountName = "TestAccount";
		public const string PersonalAccountName = "Personal";
		public const string PerevedemAccountName = "Perevedem";
		public const string CourseraAccountName = "Coursera";

		public const string PerevedemVenture = "Perevedem.ru";
		public const string SmartCATVenture = "SmartCAT";
		public const string CourseraVenture = "Coursera";
		
		public const string PersonalAccountSurname = "PersAccount";

		public const string EuropeTestServerName = "Europe";
		public const string USATestServerName = "USA";
	}
}
