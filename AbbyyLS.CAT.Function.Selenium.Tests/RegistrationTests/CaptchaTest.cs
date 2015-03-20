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
			CaptchaSignIn(4);

			// Ввводим неверный пароль 5й раз
			LoginPage.EnterPassword(RegistrationPage.Password + "test");

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Проверяем, что после 5го ввода неправильного пароля появилась captcha
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

			// Ввод правильного пароля
			LoginPage.EnterPassword(RegistrationPage.Password);

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Выходим из системы
			RegistrationPage.ClickSignOutBtn();

			// Заполнить email
			LoginPage.EnterLogin(RegistrationPage.Email);

			// Ввод неправильного пароля
			LoginPage.EnterPassword(RegistrationPage.Password + "i");

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Проверяем, что счетчик обнулился, captcha не появилась
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

			// Ввводим неверный пароль 5й раз
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password + "test");

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Проверяем, что после 5го ввода неправильного пароля появилась captcha
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

			// Ввводим верный пароль 
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password);

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();
			
			// Выходим из системы			
			RegistrationPage.ClickSignOutBtn();

			// Ждём появления поля ввода пароля. Его появление означает,что мы вышли из системы
			RegistrationPage.WaitShowPasswordInput();

			// Переход на страницу регистрации фрилансеров
			GoToRegistrationPage(RegistrationType.User);

			// Переход на стр авторизации для существующих пользователей freelance-reg
			RegistrationPage.GoToLoginPageWithExistAccount();

			// Заполнить поле email
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);

			// Ввводим неправильного пароля 
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password + "test");

			// Кликаем Sign in 
			RegistrationPage.ClickSignInButton();

			// Проверяем, что captcha не появилась
			Assert.IsFalse(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: счетчик количества ввода неправильного пароля не обнулился, captcha появилась");
		}

		/// <summary>
		/// Регистрация фрилансера и count раз ввод неверного пароля
		/// </summary>
		/// <param name="count"> количество попыток войти </param>
		private void CaptchaFreelanceReg(int count)
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

			// count раз вводим неверный пароль 
			for (int i = 0; i < count; i++)
			{
				//Заполнить пароль
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
			// Регистрируем нового фрилансера
			RegistrationNewUser(RegistrationPage.Email, RegistrationPage.Password);

			// Проверка ,что имя и фамилия нового фрилансера отображается правильно в хидере
			Assert.IsTrue(RegistrationPage.CheckNameSurnameInWSPanel(),
				"Ошибка: имя фрилансера неправильно отображается на странице WS");

			WorkspacePage.ClickAccount();
			
			// Нажать кнопку выхода
			WorkspacePage.ClickLogoff();

			// Обновить страницу
			RefreshPage();

			// Заполнить email
			LoginPage.EnterLogin(RegistrationPage.Email);

			// count раз вводим неверный пароль 
			for (int i = 0; i < count; i++)
			{
				//Заполнить пароль
				LoginPage.EnterPassword(RegistrationPage.Password + i);
				//Нажать кнопку Sign In
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
			// Регистрируемся и 4 раза пробуем ввести неправильный пароль
			CaptchaCorpReg(4);

			// Ввод неправильного пароля
			RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);

			// Проверяем, что после 5го ввода неправильного пароля появилась captcha
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
			// Регистрируемся и 3 раза пробуем ввести неправильный пароль
			CaptchaCorpReg(3);

			// Ввод правильного пароля
			RegisterAsExistUserWithCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);

			// Кликаем Sign Out 
			RegistrationPage.ClickSignOutBtn();

			// Ввод неправильного пароля
			RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);

			// Проверяем, что счетчик обнулился, капча не должна появиться
			Assert.IsFalse(RegistrationPage.GetCaptchaIsDisplayed(),
				"Ошибка: счетчик не обнулился. Captcha появилась.");
		}

		/// <summary>
		/// Регистрация пользователя на Corp-reg и count раз ввод неверного пароля
		/// </summary>
		/// <param name="count"> количество попыток войти </param>
		private void CaptchaCorpReg(int count)
		{
			// Регистрируем новую компанию
			RegisterNewUserWithCompanyAndCheckWS(RegistrationPage.Email, RegistrationPage.Password);

			// Разлогиниваемся
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			// Переходим на страницу регистрации компаний
			GoToRegistrationPage(RegistrationType.Company);

			// Переход на страницу входа для компаний
			RegistrationPage.GoToLoginPageWithExistAccount();

			// count раз вводим неверный пароль 
			for (int i = 0; i < count; i++)
			{
				RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);
				Assert.IsFalse(RegistrationPage.GetCaptchaIsDisplayed(),
					"Ошибка: captcha появилась при попытке №" + (i + 1) + " ввода неверного пароля");
			}
			
		}

	}
}
