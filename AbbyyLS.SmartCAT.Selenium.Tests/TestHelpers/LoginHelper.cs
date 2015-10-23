using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
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
			_commonHelper = new CommonHelper(Driver);
			_signInPage = new SignInPage(Driver);
		}

		public LoginHelper LogInSmartCat(
			string login,
			string nickName,
			string password,
			string accountName = "TestAccount")
		{
			BaseObject.InitPage(_signInPage, Driver);
			_signInPage.SubmitForm(login, password)
				.SelectAccount(accountName, EuropeTestServerName)
				.CloseHelpIfOpened()
				.SelectLocale();

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
					_adminHelper.SignIn(user.Login, user.Password);
					break;

				case StartPage.CompanyRegistration:
					_commonHelper.GoToCompanyRegistration();
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

				default:
					_commonHelper.GoToSignInPage();
					LogInSmartCat(user.Login, user.NickName, user.Password);
					break;
			}
			return this;
		}

		private readonly AdminHelper _adminHelper;
		private readonly CommonHelper _commonHelper;
		private readonly SignInPage _signInPage;

		public const string TestAccountName = "TestAccount";
		public const string PerevedemAccountName = "Perevedem";
		public const string CourseraAccountName = "Coursera";

		public const string PerevedemVenture = "Perevedem.ru";
		public const string SmartCATVenture = "SmartCAT";
		public const string CourseraVenture = "Coursera";

		public const string LanguageServiceProviderAccountType = "LanguageServiceProvider";

		public const string PersonalAccountSurname = "PersAccount";

		public const string EuropeTestServerName = "Europe";
		public const string USATestServerName = "USA";
	}
}
