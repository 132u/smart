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
		/// <param name="accountName">название аккаунта</param>
		/// <param name="dataServer">расположение сервера</param>
		/// <param name="language">язык локали</param>
		public WorkspaceHelper SignIn(
			string login, 
			string password, 
			string accountName = "TestAccount", 
			string dataServer = "Europe", 
			Language language = Language.English)
		{
			BaseObject.InitPage(_signInPage);

			_signInPage
				.SetLogin(login)
				.SetPassword(password)
				.ClickSubmitButton()
				.SelectAccount(accountName, dataServer)
				.SelectLocale(language)
				.ClickCloseHelp();

			return new WorkspaceHelper();
		}

		private readonly SignInPage _signInPage = new SignInPage();

		public const string TestAccountName = "TestAccount";
		public const string PerevedemAccountName = "Perevedem";
		public const string CourseraAccountName = "Coursera";

		public const string PerevedemVenture = "Perevedem.ru";
		public const string SmartCATVenture = "SmartCAT";
		public const string CourseraVenture = "Coursera";

		public const string PersonalAccountSurname = "PersAccount";
		

	}
}
