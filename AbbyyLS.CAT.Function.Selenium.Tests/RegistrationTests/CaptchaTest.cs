using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures;
using AbbyyLS.CAT.Function.Selenium.Tests.Driver;
using AbbyyLS.CAT.Function.Selenium.Tests.RegistrationTests;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Registration.Captcha
{
	class CaptchaTest<TWebDriverSettings> : RegistrationBaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Проверка, что после пяти попыток ввода неправильного пароля, появляется captcha на странице Sign-in
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaTestOnSignInPage()
		{
			CaptchaSignIn(4);
			LoginPage.EnterPassword(RegistrationPage.Password + "test");
			RegistrationPage.ClickSignInButton();
			Assert.IsTrue(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: captcha не появилась после 5й попытки ввода неверного пароля");
		}

		/// <summary>
		/// Проверка, что счетчик кол-ва попыток ввода неправильного пароля обнуляется после ввода правильного пароля
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaCounterIsNulledTestOnSignInPage()
		{
			CaptchaSignIn(3);
			LoginPage.EnterPassword(RegistrationPage.Password);
			RegistrationPage.ClickSignInButton();
			RegistrationPage.ClickSignOutBtn();
			LoginPage.EnterLogin(RegistrationPage.Email);
			LoginPage.EnterPassword(RegistrationPage.Password + "i");
			RegistrationPage.ClickSignInButton();
			Assert.IsFalse(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: счетчик количества ввода неправильного пароля не обнулился, captcha появилась");
		}

		/// <summary>
		/// Проверка, что после пяти попыток ввода неправильного пароля, появляется captcha на странице freelance-reg
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaTestOnFreelanceRegPage()
		{
			CaptchaFreelanceReg(4);
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password + "test");
			RegistrationPage.ClickSignInButton();
			Assert.IsTrue(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: captcha не появилась после 5й попытки ввода неверного пароля");
		}

		/// <summary>
		/// Проверка, что счетчик кол-ва попыток ввода неправильного пароля обнуляется после ввода правильного пароля на стр freelance-reg
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaCounterIsNulledTestOnFreelanceRegPage()
		{
			CaptchaFreelanceReg(3);
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password);
			RegistrationPage.ClickSignInButton();
			RegistrationPage.ClickSignOutBtn();
			RegistrationPage.WaitShowPasswordInput();
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password + "test");
			RegistrationPage.ClickSignInButton();
			Assert.IsFalse(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: счетчик количества ввода неправильного пароля не обнулился, captcha появилась");
		}

		/// <summary>
		/// Регистрация фрилансера и count раз ввод неверного пароля
		/// </summary>
		/// <param name="count"> количество попыток войти </param>
		private void CaptchaFreelanceReg(int count)
		{
			RegistrationNewUser(RegistrationPage.Email, RegistrationPage.Password);
			Assert.IsTrue(RegistrationPage.CheckNameSurnameInWSPanel(),
				"Ошибка: имя фрилансера неправильно отображается на странице WS");
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);
			for (int i = 0; i < count; i++)
			{
				RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password + i);
				RegistrationPage.ClickSignInButton();
				Assert.IsFalse(RegistrationPage.GetCaptchaIsDisplayed(),
					"Ошибка: captcha появилась при попытке №" + (i + 1) + " ввода неверного пароля");
			}
		}

		/// <summary>
		/// Регистрация пользователя на freelance-reg и count раз ввод неверного пароля
		/// </summary>
		/// <param name="count"> количество попыток войти </param>
		private void CaptchaSignIn(int count)
		{
			RegistrationNewUser(RegistrationPage.Email, RegistrationPage.Password);
			Assert.IsTrue(RegistrationPage.CheckNameSurnameInWSPanel(),
				"Ошибка: имя фрилансера неправильно отображается на странице WS");

			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
			RefreshPage();
			LoginPage.EnterLogin(RegistrationPage.Email);
			for (int i = 0; i < count; i++)
			{
				LoginPage.EnterPassword(RegistrationPage.Password + i);
				LoginPage.ClickSubmitCredentials();
				Assert.IsFalse(RegistrationPage.GetCaptchaIsDisplayed(),
					"Ошибка: captcha появилась при попытке №" + (i + 1) + " ввода неверного пароля");
			}
		}

		/// <summary>
		/// Проверка, что после пяти попыток ввода неправильного пароля, появляется captcha
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaTestCorpRegPage()
		{
			CaptchaCorpReg(4);
			RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);
			Assert.IsTrue(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: captcha не появилась после 5й попытки ввода неверного пароля");
		}

		/// <summary>
		/// Проверка, что счетчик кол-ва попыток ввода неправильного пароля обнуляется после ввода правильного пароля captcha
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaCounterIsNulledTestCorpRegPage()
		{
			CaptchaCorpReg(3);
			RegisterAsExistUserWithCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);
			RegistrationPage.ClickSignOutBtn();
			RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);
			Assert.IsFalse(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: счетчик не обнулился. Captcha появилась.");
		}

		/// <summary>
		/// Регистрация пользователя на Corp-reg и count раз ввод неверного пароля
		/// </summary>
		/// <param name="count"> количество попыток войти </param>
		private void CaptchaCorpReg(int count)
		{
			Logger.Trace("Регистрация компании и ввод неправильного пароля " + count + " раз");
			RegisterNewUserWithCompanyAndCheckWS(RegistrationPage.Email, RegistrationPage.Password);
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.GoToLoginPageWithExistAccount();
			for (int i = 0; i < count; i++)
			{
				RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);
				Assert.IsFalse(RegistrationPage.GetCaptchaIsDisplayed(),
					"Ошибка: captcha появилась при попытке №" + (i + 1) + " ввода неверного пароля");
			}
			
		}

	}
}
