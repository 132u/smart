using System;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration
{
	internal class CompanyRegistrationFirstPage : WorkspacePage, IAbstractPage<CompanyRegistrationFirstPage>
	{
		public CompanyRegistrationFirstPage(WebDriver driver) : base(driver)
		{
		}

		public new CompanyRegistrationFirstPage GetPage()
		{
			var companyRegistrationFirstPage = new CompanyRegistrationFirstPage(Driver);
			InitPage(companyRegistrationFirstPage, Driver);

			return companyRegistrationFirstPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_PASSWORD)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась первая страница регистрации компаний.");
			}
		}

		/// <summary>
		/// Нажать кнопку Continue
		/// </summary>
		public T ClickContinueButton<T>(WebDriver driver) where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать кнопку Continue.");
			ContinueButton.JavaScriptClick();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}
	
		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email</param>
		public CompanyRegistrationFirstPage FillEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле Email.", email);
			Email.SetText(email);

			return GetPage();
		}

		/// <summary>
		/// Ввести пароль
		/// </summary>
		///  <param name="password">пароль</param>
		public CompanyRegistrationFirstPage FillPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле пароля.", password);
			Password.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Ввести подтверждение пароль
		/// </summary>
		///  <param name="password">пароль</param>
		public CompanyRegistrationFirstPage FillConfirmPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле подтверждения пароля.", password);
			ConfirmPassword.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать по ссылке 'or sign in with an existing ABBYY account'
		/// </summary>
		public CompanyRegistrationSignInPage ClickExistingAbbyyAccountLink()
		{
			CustomTestContext.WriteLine("Нажать по ссылке 'or sign in with an existing ABBYY account'.");
			ExistingAbbyyAccountLink.Click();

			return new CompanyRegistrationSignInPage(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, что кнопка Continue неактивна
		/// </summary>
		public CompanyRegistrationFirstPage AssertContinueButtonInactive()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка Continue неактивна.");

			Assert.IsTrue(ContinueButton.GetAttribute("disabled") == "true",
				"Произошла ошибка:\n кнопка Continue активна.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение 'Invalid email' появилось
		/// </summary>
		public CompanyRegistrationFirstPage AssertInvalidEmailMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'Invalid email' появилось.");

			Assert.IsTrue(InvalidEmailMessage.Displayed,
				"Произошла ошибка:\n сообщение 'Invalid email' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение 'The password must have at least 6 characters' появилось
		/// </summary>
		public CompanyRegistrationFirstPage AssertMinimumLenghPasswordMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The password must have at least 6 characters' появилось.");

			Assert.IsTrue(MinimumLenghPasswordMessage.Displayed,
				"Произошла ошибка:\n сообщение 'The password must have at least 6 characters' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение 'The password cannot consist of spaces only' появилось
		/// </summary>
		public CompanyRegistrationFirstPage AssertOnlySpacesPasswordMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The password cannot consist of spaces only' появилось.");

			Assert.IsTrue(OnlySpacesPasswordMessage.Displayed,
				"Произошла ошибка:\n сообщение 'The password cannot consist of spaces only' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение 'The passwords do not match' появилось
		/// </summary>
		public CompanyRegistrationFirstPage AssertPasswordMatchMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The passwords do not match' появилось.");

			Assert.IsTrue(PasswordMatchMessage.Displayed,
				"Произошла ошибка:\n сообщение 'The passwords do not match' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение 'You have already signed up for one of the ABBYY services with this email' появилось
		/// </summary>
		public CompanyRegistrationFirstPage AssertAlreadySignUpMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'You have already signed up for one of the ABBYY services with this email.' появилось.");

			Assert.IsTrue(AlreadySignUpMessage.Displayed,
				"Произошла ошибка:\n сообщение 'You have already signed up for one of the ABBYY services with this email.' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение 'The password must have at least 6 characters.' появилось
		/// </summary>
		public CompanyRegistrationFirstPage AssertInvalidPasswordMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The password must have at least 6 characters.' появилось.");

			Assert.IsTrue(InvalidPasswordMessage.Displayed,
				"Произошла ошибка:\n сообщение 'The password must have at least 6 characters.' не появилось.");

			return GetPage();
		}

		[FindsBy(How = How.Id, Using = CONTINUE_BUTTON)]
		protected IWebElement ContinueButton { get; set; }

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD)]
		protected IWebElement Password { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_PASSWORD)]
		protected IWebElement ConfirmPassword { get; set; }

		[FindsBy(How = How.Id, Using = EXISTING_ABBYY_ACCOUNT_LINK)]
		protected IWebElement ExistingAbbyyAccountLink { get; set; }

		[FindsBy(How = How.XPath, Using = INVALID_EMAIL_MESSAGE)]
		protected IWebElement InvalidEmailMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ONLY_SPACES_PASSWORD_MESSAGE)]
		protected IWebElement OnlySpacesPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = MINIMUM_LENGHT_PASSWORD_MESSAGE)]
		protected IWebElement MinimumLenghPasswordMessage { get; set; }

		[FindsBy(How = How.XPath, Using = ALREADY_SIGN_UP_MESSAGE)]
		protected IWebElement AlreadySignUpMessage { get; set; }
		
		[FindsBy(How = How.XPath, Using = PASSWORD_MATCH_MESSAGE)]
		protected IWebElement PasswordMatchMessage { get; set; }

		[FindsBy(How = How.XPath, Using = INVALID_PASSWORD_MESSAGE)]
		protected IWebElement InvalidPasswordMessage { get; set; }

		protected const string CONTINUE_BUTTON = "btn-sign-up";
		protected const string EMAIL = "//form[@name='signupForm']//input[@id='email']";
		protected const string PASSWORD = "//form[@name='signupForm']//input[@id='password']";
		protected const string EXISTING_ABBYY_ACCOUNT_LINK = "show-sign-in";
		protected const string CONFIRM_PASSWORD = "//form[@name='signupForm']//input[@id='confirm']";

		protected const string INVALID_EMAIL_MESSAGE = "//span[contains(@ng-show,'signupForm.email')]";
		protected const string MINIMUM_LENGHT_PASSWORD_MESSAGE = "//fieldset[contains(@valid,'password')]//div[contains(@ng-message,'minlength')]//span[not(contains(@class,'ng-hide'))]";
		protected const string ONLY_SPACES_PASSWORD_MESSAGE = "//div[contains(@ng-message,'pattern')]//span[not(contains(@class,'ng-hide'))]";
		protected const string PASSWORD_MATCH_MESSAGE = "//span[contains(@ng-show, 'error.match ')]";
		protected const string ALREADY_SIGN_UP_MESSAGE = "//div[@ng-message='already-exists']";
		protected const string INVALID_PASSWORD_MESSAGE = "//span[@translate='PASSWORD-INVALID']";
	}
}
