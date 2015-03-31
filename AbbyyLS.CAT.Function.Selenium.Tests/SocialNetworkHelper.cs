using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер для работы с Facebook, LinkedIn, Google+
	/// </summary>
	public class SocialNetworkHelper : CommonHelper
	{
		public SocialNetworkHelper(IWebDriver driver, WebDriverWait wait) :
			base (driver, wait)
		{
		}

		#region Методы работы с LinkedIn
		public void ClickSubmitBtnLinkedIn()
		{
			Logger.Trace("Клик по кнопке 'Submit' на странице авторизации LinkedIn");
			ClickElement(By.XPath(SUBMIT_BTN_LINKED_IN));
		}

		public void FillEmailFieldLinkedIn(string email)
		{
			Logger.Trace("Ввод email на странице авторизации LinkedIn");
			ClearAndAddText(By.XPath(EMAIL_FIELD_LINKED_IN), email);
		}

		public void FillPasswordFieldLinkedIn(string password)
		{
			Logger.Trace("Ввод пароля на странице авторизации LinkedIn");
			ClearAndAddText(By.XPath(PASSWORD_FIELD_LINKED_IN), password);
		}
		
		public bool WaitLinkedInIsLoad()
		{
			Logger.Trace("Ожидаем загрузку страницы авторизации LinkedIn");
			return WaitUntilDisplayElement(By.XPath(LINKED_IN_HEADER));
		}
		#endregion

		#region Методы работы с Facebook
		public void FillPasswordFieldFacebook(string password)
		{
			Logger.Trace("Ввод пароля на странице авторизации Facebook");
			ClearAndAddText(By.XPath(PASSWORD_FIELD_FB), password);
		}

		public void ClickSubmitBtnFacebook()
		{
			Logger.Trace("Клик по кнопке 'Submit' на странице авторизации Facebook");
			ClickElement(By.XPath(SUBMIT_BTN_FB));
		}

		public void FillEmailFieldFacebook(string email)
		{
			Logger.Trace("Ввод email на странице авторизации Facebook");
			ClearAndAddText(By.XPath(EMAIL_FIELD_FB), email);
		}

		public bool WaitFacebookIsLoad()
		{
			Logger.Trace("Ожидаем загрузку страницы авторизации Facebook");
			return WaitUntilDisplayElement(By.XPath(FACEBOOK_HEADER));
		}

		#endregion

		#region Методы работы с Google
		public void FillPasswordFieldGoogle(string password)
		{
			Logger.Trace("Ввод пароля на странице авторизации Google");
			ClearAndAddText(By.XPath(PASSWORD_FIELD_GOOGLE), password);
		}

		public void FillEmailFieldGoogle(string email)
		{
			Logger.Trace("Ввод email на странице авторизации Google");
			ClearAndAddText(By.XPath(EMAIL_FIELD_GOOGLE), email);
		}

		public void ClickSubmitBtnGoogle()
		{
			Logger.Trace("Клик по кнопке 'Submit' на странице авторизации Google");
			ClickElement(By.XPath(SUBMIT_BTN_GOOGLE));
		}

		public bool WaitGoogleIsLoad()
		{
			Logger.Trace("Ожидаем загрузку страницы авторизации Google");
			return WaitUntilDisplayElement(By.XPath(GOOGLE_HEADER));
		}
		#endregion

		//XPaths Facebook
		protected const string EMAIL_FIELD_FB = "//input[@id='email']";
		protected const string SUBMIT_BTN_FB = "//input[@id='u_0_1']";
		protected const string PASSWORD_FIELD_FB = "//input[@id='pass']";
		protected const string FACEBOOK_HEADER= "//a[@title='Go to Facebook Home']";

		//XPaths LinkedIn
		protected const string PASSWORD_FIELD_LINKED_IN = "//input[@id='session_password-oauth2SAuthorizeForm']";
		protected const string EMAIL_FIELD_LINKED_IN = "//input[@id='session_key-oauth2SAuthorizeForm']";
		protected const string SUBMIT_BTN_LINKED_IN = "//input[@class='allow']";
		protected const string LINKED_IN_HEADER = "//div[@class='logo' and text()='LinkedIn']";

		//XPaths Google+
		protected const string PASSWORD_FIELD_GOOGLE = "//input[@id='Passwd']";
		protected const string EMAIL_FIELD_GOOGLE = "//input[@id='Email']";
		protected const string SUBMIT_BTN_GOOGLE = "//input[@id='signIn']";
		protected const string GOOGLE_HEADER = "//img[@class='logo']";

	}
}
