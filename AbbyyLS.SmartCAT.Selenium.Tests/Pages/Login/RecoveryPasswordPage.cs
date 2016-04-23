using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class RecoveryPasswordPage : IAbstractPage<RecoveryPasswordPage>
	{
		public WebDriver Driver { get; protected set; }

		public RecoveryPasswordPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public RecoveryPasswordPage LoadPage()
		{
			if (!IsRecoveryPasswordPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница RecoveryPasswordPage (страница восстановления пароля).");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть на кнопку смены пароля.
		/// </summary>
		public RecoveryPasswordPage ClickChangePasswordButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку смены пароля.");
			ChangePasswordButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Ввести email для отправки инструкций по смене пароля.
		/// </summary>
		/// <param name="email">email для восстановления пароля</param>
		public RecoveryPasswordPage SetEmailForRecoveryPassword(string email)
		{
			CustomTestContext.WriteLine("Ввести email {0} для отправки инструкций по смене пароля.", email);
			InputEmailField.SetText(email);

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на логотип SmartCat.
		/// </summary>
		public SignInPage ClickOnSmartCatLogo()
		{
			CustomTestContext.WriteLine("Кликнуть на логотип SmartCat.");
			SmartCatLogo.Click();

			return new SignInPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что отображается заметка для восстановления пароля.
		/// </summary>
		public bool IsRecoveryNoticeDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается заметка для восстановления пароля.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(RECOVERY_NOTICE));
		}

		/// <summary>
		/// Проверка отображения сообщения об ошибке.
		/// </summary>
		public bool IsErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверка отображения сообщения об ошибке.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_MESSAGE));
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsRecoveryPasswordPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(PASSWORD_RECOVERY_FROM));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = PASSWORD_RECOVERY_FROM)]
		protected IWebElement PassworRecoveryForm { get; set; }

		[FindsBy(How = How.XPath, Using = INPUT_EMAIL_FIELD)]
		protected IWebElement InputEmailField { get; set; }

		[FindsBy(How = How.XPath, Using = CHANGE_PASSWORD_BUTTON)]
		protected IWebElement ChangePasswordButton { get; set; }

		[FindsBy(How = How.XPath, Using = RECOVERY_NOTICE)]
		protected IWebElement RecoveryNotice { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_MESSAGE)]
		protected IWebElement ErrorMessage { get; set; }

		[FindsBy(How = How.XPath, Using = SMART_CAT_LOGO)]
		protected IWebElement SmartCatLogo { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string PASSWORD_RECOVERY_FROM = "//div[contains(@class, 'g-die')]";
		protected const string INPUT_EMAIL_FIELD = "//div[contains(@class, 'g-form__row')]//input";
		protected const string CHANGE_PASSWORD_BUTTON = "//div[contains(@class, 'g-form__row')]//button[contains(@class, 'g-btn')]";
		protected const string RECOVERY_NOTICE = "//div[contains(@class, 'g-die')]//p[contains(@class, 'g-form__note')]";
		protected const string ERROR_MESSAGE = "//div[contains(@class, 'g-form__error')]";
		protected const string SMART_CAT_LOGO = "//img[contains(@alt, 'SmartCat')]";

		#endregion
	}
}