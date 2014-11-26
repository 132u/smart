using AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures;
using AbbyyLS.CAT.Function.Selenium.Tests.RegistrationTests;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Registration.Captcha
{
	class CaptchaTest : RegistrationBaseTest
	{
		/// <summary>
		/// Конструктор тестов проверки captcha
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public CaptchaTest(string browserName)
			: base(browserName)
		{
			
		}

		/// <summary>
		/// Проверка, что после пяти попыток ввода неправильного пароля, появляется captcha на странице Sign-in
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaTestOnSignInPage()
		{
			CaptchaSignIn();

			// Ввводим неверный пароль 6й раз
			LoginPage.EnterPassword(RegistrationPage.Password + "test");

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Проверяем, что после 6го ввода неправильного пароля появилась captcha
			Assert.IsTrue(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: captcha не появилась после 6й попытки ввода неверного пароля");
		}

		/// <summary>
		/// Проверка, что счетчик кол-ва попыток ввода неправильного пароля обнуляется после ввода правильного пароля
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaCounterIsNulledTestOnSignInPage()
		{
			CaptchaSignIn();

			// Ввод правильного пароля
			LoginPage.EnterPassword(RegistrationPage.Password);

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Кликаем Log off в WS
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			//Заполнить email
			LoginPage.EnterLogin(RegistrationPage.Email);

			// Ввод неправильного пароля
			LoginPage.EnterPassword(RegistrationPage.Password + "i");

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Проверяем, что счетчик обнулился, captcha не появилась
			Assert.IsTrue(!RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: счетчик количества ввода неправильного пароля не обнулился, captcha появилась");
		}

		/// <summary>
		/// Проверка, что после пяти попыток ввода неправильного пароля, появляется captcha на странице freelance-reg
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaTestOnFreelanceRegPage()
		{
			CaptchaFreelanceReg();

			// Ввводим неверный пароль 6й раз
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password + "test");

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Проверяем, что после 6го ввода неправильного пароля появилась captcha
			Assert.IsTrue(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: captcha не появилась после 6й попытки ввода неверного пароля");
		}

		/// <summary>
		/// Проверка, что счетчик кол-ва попыток ввода неправильного пароля обнуляется после ввода правильного пароля на стр freelance-reg
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaCounterIsNulledTestOnFreelanceRegPage()
		{
			CaptchaFreelanceReg();

			// Ввводим верный пароль 
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password);

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Кликнуть кнопку выхода
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			// Переход на страницу регистрации фрилансеров
			GoToRegistrationPage(RegistrationType.User);

			// Переход на стр авторизации для уществующих пользователей freelance-reg
			RegistrationPage.GoToLoginPageWithExistAccount();

			// Заполнить поле email
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);

			// Ввводим неправильного пароля 
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password + "test");

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Проверяем, что captcha не появилась
			Assert.IsTrue(!RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: счетчик количества ввода неправильного пароля не обнулился, captcha появилась");
		}

		private void CaptchaFreelanceReg()
		{
			// Регистрируем нового фрилансера
			RegistrationNewUser(RegistrationPage.Email, RegistrationPage.Password);

			// Проверка ,что имя и фамилия нового фрилансера отображается правильно в хидере
			Assert.IsTrue(RegistrationPage.CheckNameSurnameInWSPanel(),
				"Ошибка: имя фрилансера неправильно отображается на странице WS");

			// Кликнуть кнопку выхода
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			// Переход на страницу регистрации фрилансеров
			GoToRegistrationPage(RegistrationType.User);

			// Переход на стр авторизации для уществующих пользователей freelance-reg
			RegistrationPage.GoToLoginPageWithExistAccount();

			// Заполнить поле email
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);

			// Пять раз вводим неверный пароль 
			for (int i = 1; i < 6; i++)
			{
				//Заполнить пароль
				RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password + i);
				RegistrationPage.ClickSignInButton();
				Assert.IsTrue(!RegistrationPage.GetCaptchaIsDisplayed(),
					"Ошибка: captcha появилась при попытке №" + i + " ввода неверного пароля");
			}

		}

		/// <summary>
		/// Регистрация пользователя на freelance-reg и пять раз ввод неверного пароля
		/// </summary>
		private void CaptchaSignIn()
		{
			// Регистрируем нового фрилансера
			RegistrationNewUser(RegistrationPage.Email, RegistrationPage.Password);

			// Проверка ,что имя и фамилия нового фрилансера отображается правильно в хидере
			Assert.IsTrue(RegistrationPage.CheckNameSurnameInWSPanel(),
				"Ошибка: имя фрилансера неправильно отображается на странице WS");

			WorkspacePage.ClickAccount();
			//Нажать кнопку выхода

			WorkspacePage.ClickLogoff();

			// Обновить страницу
			RegistrationPage.RefreshLoginPage();

			// Заполнить email
			LoginPage.EnterLogin(RegistrationPage.Email);

			// Пять раз вводим неверный пароль 
			for (int i = 1; i < 6; i++)
			{
				//Заполнить пароль
				LoginPage.EnterPassword(RegistrationPage.Password + i);
				//Нажать кнопку Sign In
				LoginPage.ClickSubmitCredentials();
				Assert.IsTrue(!RegistrationPage.GetCaptchaIsDisplayed(),
					"Ошибка: captcha появилась при попытке №" + i + " ввода неверного пароля");
			}
		}

		/// <summary>
		/// Проверка, что после пяти попыток ввода неправильного пароля, появляется captcha
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaTestCorpRegPage()
		{
			CaptchaCorpReg();

			RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);

			// Проверяем, что после 5го ввода неправильного пароля появилась captcha
			Assert.IsTrue(RegistrationPage.GetCaptchaIsDisplayed(),
	"			Ошибка: captcha не появилась после 5й попытки ввода неверного пароля");
		}

		/// <summary>
		/// Проверка, что счетчик кол-ва попыток ввода неправильного пароля обнуляется после ввода правильного пароля captcha
		/// </summary>
		[Category("PRX_6503")]
		[Test]
		public void CaptchaCounterIsNulledTestCorpRegPage()
		{
			CaptchaCorpReg();

			// Ввод правильного пароля
			RegisterAsExistUserWithCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);

			// Кликаем Sign Out 
			RegistrationPage.ClickSignOutBtn();

			// Ввод неправильного пароля
			RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);

			// Проверяем, что счетчик обнулился, капча не должна появиться
			Assert.IsTrue(RegistrationPage.GetCaptchaIsDisplayed(),
	"			Ошибка: счетчик не обнулился");
		}

		/// <summary>
		/// Регистрация пользователя на Corp-reg и пять раз ввод неверного пароля
		/// </summary>
		private void CaptchaCorpReg()
		{
			// Регистрируем новую компанию
			RegisterNewUserWithCompanyAndCheckWS(RegistrationPage.Email, RegistrationPage.Password);

			// Кликаем Log off в WS
			WorkspacePage.ClickLogoff();

			//Переходим на страницу регистрации компаний
			GoToRegistrationPage(RegistrationType.Company);

			// Переход на страницу входа для компаний
			RegistrationPage.GoToLoginPageWithExistAccount();

			// 3 раза вводим неверный пароль 
			for (int i = 1; i < 6; i++)
			{
				RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);
				Assert.IsTrue(!RegistrationPage.GetCaptchaIsDisplayed(),
					"Ошибка: captcha появилась при попытке №" + i + " ввода неверного пароля");
			}
		}

	}
}
