using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class LoginHelper
	{
		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="login">логин (email)</param>
		/// <param name="password">пароль</param>
		public LoginHelper SignIn(
			string login,
			string password)
		{
			BaseObject.InitPage(_signInPage);

			_signInPage
				.SetLogin(login)
				.SetPassword(password)
				.ClickSubmitButton<SelectAccountForm>();

			return this;
		}

		public LoginHelper TryToSignIn(
			string login,
			string password)
		{
			BaseObject.InitPage(_signInPage);

			_signInPage
				.SetLogin(login)
				.SetPassword(password)
				.ClickSubmitButton<SignInPage>();

			return this;
		}

		public WorkspaceHelper SelectAccount(
			string accountName = TestAccountName, 
			string dataServer = EuropeTestServerName)
		{
			BaseObject.InitPage(_selectAccountForm);
			_selectAccountForm
				.AssertEuropeServerRespond()
				.SelectAccount(accountName, dataServer);

			return new WorkspaceHelper();
		}

		public LoginHelper CheckWrongPasswordMessageDisplayed()
		{
			BaseObject.InitPage(_signInPage);
			_signInPage.CheckWrongPasswordMessageDisplayed();

			return this;
		}

		public LoginHelper CheckUserNotFoundMessageDisplayed()
		{
			BaseObject.InitPage(_signInPage);
			_signInPage.CheckUserNotFoundMessageDisplayed();

			return this;
		}

		public LoginHelper CheckEmptyPasswordMessageDisplayed()
		{
			BaseObject.InitPage(_signInPage);
			_signInPage.CheckEmptyPasswordMessageDisplayed();

			return this;
		}

		public LoginHelper CheckInvalidEmailMessageDisplayed()
		{
			BaseObject.InitPage(_signInPage);
			_signInPage.CheckInvalidEmailMessageDisplayed();

			return this;
		}

		public WorkspaceHelper SignInViaFacebook(
			string email,
			string password)
		{
			BaseObject.InitPage(_signInPage);

			_signInPage
				.ClickFacebookIcon()
				.SetEmail(email)
				.SetPassword(password)
				.ClickSubmitButton();

			return new WorkspaceHelper();
		}

		public WorkspaceHelper SignInViaGooglePlus(
			string email,
			string password)
		{
			BaseObject.InitPage(_signInPage);

			_signInPage
				.ClickGooglePlusIcon()
				.SetEmail(email)
				.ClickNextButton()
				.SetPassword(password)
				.ClickSubmitButton();

			return new WorkspaceHelper();
		}

		public WorkspaceHelper SignInViaLinkedIn(
			string email,
			string password)
		{
			BaseObject.InitPage(_signInPage);

			_signInPage
				.ClickLinkedInIcon()
				.SetEmail(email)
				.SetPassword(password)
				.ClickSubmitButton();

			return new WorkspaceHelper();
		}

		public LoginHelper AssertAccountNotFoundMessageDisplayed()
		{
			BaseObject.InitPage(_selectAccountForm);
			_selectAccountForm.CheckAccountNotFoundMessageDisplayed();

			return this;
		}

		public LoginHelper LogInSmartCat(
			string login,
			string nickName,
			string password,
			string accountName = "TestAccount")
		{
			SignIn(login, password)
				.SelectAccount(accountName)
				.SetUp(nickName, accountName);

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
					LogInSmartCat(
						user.Login,
						user.NickName,
						user.Password);
					break;

				case StartPage.PersonalAccount:
					_commonHelper.GoToSignInPage();
					LogInSmartCat(
						user.Login,
						user.NickName,
						user.Password,
						"Personal");
					break;

				default:
					_commonHelper.GoToSignInPage();
					LogInSmartCat(
						user.Login,
						user.NickName,
						user.Password);
					break;
			}

			return this;
		}

		private readonly AdminHelper _adminHelper = new AdminHelper();
		private readonly CommonHelper _commonHelper = new CommonHelper();
		private readonly SignInPage _signInPage = new SignInPage();
		private readonly SelectAccountForm _selectAccountForm = new SelectAccountForm();

		public const string TestAccountName = "TestAccount";
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
