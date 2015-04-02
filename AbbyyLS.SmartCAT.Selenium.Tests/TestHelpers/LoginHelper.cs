using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class LoginHelper
	{
		private readonly SignInPage _signInPage = new SignInPage();

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="login">логин (email)</param>
		/// <param name="password">пароль</param>
		/// <param name="accountName">название аккаунта</param>
		/// <param name="dataServer">расположение сервера</param>
		/// <param name="language">язык локали</param>
		public LoginHelper SignIn(
			string login, 
			string password, 
			string accountName = "TestAccount", 
			string dataServer = "Europe", 
			string language = "English")
		{
			BaseObject.InitPage(_signInPage);

			_signInPage.SetLogin(login)
				.SetPassword(password)
				.ClickSubmitBtn()
				.SelectAccount(accountName, dataServer)
				.SelectLocale(language);

			return this;
		}
	}
}
