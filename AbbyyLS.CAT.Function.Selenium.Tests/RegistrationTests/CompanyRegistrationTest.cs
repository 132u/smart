using AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures;
using NUnit.Framework;
using System.Threading;
using System;
using AbbyyLS.CAT.Function.Selenium.Tests.RegistrationTests;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Registration.Company
{
	internal class CompanyRegistrationTest : RegistrationBaseTest
	{

		/// <summary>
		/// Конструктор теста регистрации компании
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public CompanyRegistrationTest(string browserName)
			: base(browserName)
		{
			
		}

		/// <summary>
		/// Тест - регистрация нововoй компании п 1.1
		/// </summary>
		[Test]
		public void RegistrationNewUserCompany()
		{
			RegisterNewUserWithCompanyAndCheckWS(RegistrationPage.Email, RegistrationPage.Password);
		}

		private string _corpAccountNameInAdmin = "";
		/// <summary>
		/// Тест - регистрация пользователя, созданного через админку(с корп.аккаунтом)
		/// </summary>
		[Test]
		public void RegisterUserWithCorpAccount()
		{
			// Переходв  админку и авторизация
			LoginToAdminPage();
			// Создание нового пользователя
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			_corpAccountNameInAdmin = CreateCorpAccount("", true);
			AddUserToCorpAccount(RegistrationPage.Email);
			// Переход на страницу регистрации c cуществующим аккаунтом
			RegisterExistUserAndCheckWS(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.NameCompany, RegistrationPage.DomainName);
		}

		/// <summary>
		/// Тест - проверка,что на одного пользователя можно зарегистрировать две компании
		/// </summary>
		[Test]
		public void RegisterUserAndTwoCompanies()
		{
			RegisterNewUserWithCompanyAndCheckWS(RegistrationPage.Email, RegistrationPage.Password);
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
			string nameCompany2 = "SECOND" + RegistrationPage.NameCompany;
			 string domainName2 = "SECOND" + RegistrationPage.DomainName;
			RegisterExistUserAndCheckWS(RegistrationPage.Email, RegistrationPage.Password, nameCompany2, domainName2);
			Assert.True(WorkspacePage.CheckAccountList(RegistrationPage.NameCompany), "Ошибка: в списке неверное название первой компании");
		}

		/// <summary>
		/// Тест - регистрация пользователя, созданного через админку (без аккаунтов)
		/// </summary>
		[Test]
		public void RegisterUserWithoutAccount()
		{
			// Переходв  админку и авторизация
			LoginToAdminPage();
			// Создание нового пользователя
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			// Переход на страницу регистрации
			RegisterExistUserAndCheckWS(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.NameCompany, RegistrationPage.DomainName);
		}

		/// <summary>
		/// Тест - Регистрация пользователя с активным персональным аккаунтом (создать в админке, добавить перс аккаунт)
		/// </summary>
		[Test]
		public void RegisterUserWithPersAccount()
		{
			// Переходв  админку и авторизация
			LoginToAdminPage();
			// Создание нового пользователя
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			CreateNewPersAcc(RegistrationPage.LastName, true);
			RegisterExistUserAndCheckWS(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.NameCompany, RegistrationPage.DomainName);
		}

		/// <summary>
		/// Тест - Регистрация пользователя с неактивным персональным аккаунтом (создать в админке, добавить перс аккаунт, снять галку)
		/// </summary>
		[Test]
		public void RegisterUserWithInactivePersAccount()
		{
			// Переходв  админку и авторизация
			LoginToAdminPage();
			// Создание нового пользователя
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			CreateNewPersAcc(RegistrationPage.LastName, false);
			RegisterExistUserAndCheckWS(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.NameCompany, RegistrationPage.DomainName);
		}

		/// <summary>
		///Тест регистрации юзера с существующим активным или неактивным аккаунтом в coursera/aol ( or log in with your ABBYY Online account )
		/// </summary>
		[TestCase(0, "active AOL user - 1st row in  TestUsers.xml file")]
		[TestCase(1, "inactive AOL user  - 2nd row in  TestUsers.xml file")]
		[TestCase(2, "active Coursera user - 3th row in  TestUsers.xml file")]
		[TestCase(3, "inactive Coursera user - 4th row in  TestUsers.xml file")]
		public void RegisterUserFromExternalSite(int userNumber, string userTest)
		{
			// если TestUsers.xml отсутствует в папке config, то порпускаем тест
			if (!TestUserFileExist() || (TestCompanyList.Count == 0))
			{
				Assert.Ignore("Файл TestUsers.xml с тестовыми пользователями отсутствует");
			}
			RegisterExistUserAndCheckWS(
				TestCompanyList[userNumber].Login,
				TestCompanyList[userNumber].Password,
				RegistrationPage.NameCompany,
				RegistrationPage.DomainName);
		}

		/// <summary>
		/// Тест - Проверка логина-разлогина, пользователь с корп аккаунтом может выйти и залогиниться
		/// </summary>
		[Test]
		public void LoginRegisteredUser()
		{
			RegisterUserWithCorpAccount();
			WorkspacePage.ClickLogoff();
			RegistrationPage.RefreshLoginPage();
			//Заполнить email
			LoginPage.EnterLogin(RegistrationPage.Email);
			//Заполнить пароль
			LoginPage.EnterPassword(RegistrationPage.Password);
			//Нажать кнопку Sign In
			LoginPage.ClickSubmitCredentials();
			// Проверить, что открылась страница выбора аккаунта
			if (LoginPage.WaitAccountExist(_corpAccountNameInAdmin, 5))
			{
				// Выбрать аккаунт
				LoginPage.ClickAccountName(_corpAccountNameInAdmin);
			}
			else if (LoginPage.GetIsErrorExist())
			{
				Assert.Fail("Появилась ошибка при входе! М.б.недоступен AOL.");
			}

			WorkspacePage.ClickAccount();
			Assert.IsTrue(
				(WorkspacePage.GetCompanyName() == _corpAccountNameInAdmin), "Ошибка: название компании неверно отображается в панели WS");
			Assert.IsTrue(
				(WorkspacePage.GetUserName() == RegistrationPage.FirstName + " " + RegistrationPage.LastName), "Ошибка: имя представителя компании неверно отображается в панели WS");
		}

		///<summary>
		///Тест - Проверка, что кнопка неактивна если не все обязательные поля заполнены на на втором шаге регистрации компании</summary>
		/// <summary>
		/// <param name="field">поле для заполнения</param>
		[TestCase(RegistrationField.FirstName)]
		[TestCase(RegistrationField.LastName)]
		[TestCase(RegistrationField.CompanyName)]
		[TestCase(RegistrationField.DomainName)]
		[TestCase(RegistrationField.PhoneNumber)]

		[Test]
		public void CheckCreateAccBtnDisable(RegistrationField field)
		{
			// Переход на страницу регистрации компании
			GoToRegistrationPage(RegistrationType.Company);
			// Заполняем все поля на первом шаге регистрации компании
			RegistrationPage.FillRegistrationDataInFirstStep(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.Password);
			// Нажимаем кнопку Sign Up button
			RegistrationPage.ClickSignUpButton();
			switch (field)
			{
				case RegistrationField.FirstName:
					FillAllFieldsSecondStepCompanyRegistration(
						string.Empty,
						RegistrationPage.LastName,
						RegistrationPage.NameCompany,
						RegistrationPage.DomainName,
						"123123213123213");
					break;

				case RegistrationField.LastName:
					FillAllFieldsSecondStepCompanyRegistration(
						RegistrationPage.FirstName,
						string.Empty,
						RegistrationPage.NameCompany,
						RegistrationPage.DomainName,
						"123123213123213");
					break;

				case RegistrationField.CompanyName:
					FillAllFieldsSecondStepCompanyRegistration(
						RegistrationPage.FirstName,
						RegistrationPage.LastName,
						string.Empty,
						RegistrationPage.DomainName,
						"123123213123213");
					break;

				case RegistrationField.DomainName:
					FillAllFieldsSecondStepCompanyRegistration(
						RegistrationPage.FirstName,
						RegistrationPage.LastName,
						RegistrationPage.NameCompany,
						string.Empty,
						"123123213123213");
					break;

				case RegistrationField.PhoneNumber:
					FillAllFieldsSecondStepCompanyRegistration(
						RegistrationPage.FirstName,
						RegistrationPage.LastName,
						RegistrationPage.NameCompany,
						RegistrationPage.DomainName,
						string.Empty);
					break;
			}
			Assert.IsTrue(CheckCreateAccBtnDisable(), WARNING_MESSAGE_BUTTON_IS_DISABLE);

		}

		/// <summary>
		/// Проверка email на валидность http://dev.perevedem.ru/wiki/Registration_field
		/// </summary>
		/// <param name="email">email</param>
		///  <param name="valid">валидный или неавлидный</param>
		[TestCase("Em\"ailG1\'23\\23!@#$%^&*()_-|?><,.kh@ads!asdkljoogle.com", false)]
		[TestCase("dfsdfsdf@google.com", true)]
		[TestCase("emailgoogle.com", false)]
		[TestCase("emaifuyyl@goog le.com", false)]
		[TestCase("tab	fyyl@google.com", false)] //tab
		[TestCase("", false)]
		[TestCase(" ", false)]
		[TestCase("asdasddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd@asdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdasdddddddddddddddddddddddddddddddddddddddddddddddsdddd.com", true)] //64 -domen, 253 - local part
		[TestCase("asdasddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd1@asdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdasdddddddddddddddddddddddddddddddddddddddddddddddsdddd1.com", false)] //65 -domen, 254 - local part
		[TestCase("asdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddd@asdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdasdddddddddddddddddddddddddddddddddddddddddddddddsddd.com", true)] //63 -domen, 252 - local part
		[TestCase("email@goo@sfd@ffgl@khhje.com", false)]
		[TestCase("gfgfgfgfgfgfgggfgfgf", false)]
		//[TestCase("dfsd+f-sdf@go-o[]gle.com", true)]
		[Test]
		public void CheckEmailCompanyValidation(string email, bool valid)
		{
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.Company);
			// Заполняем все поля на первом шаге регистрации
			RegistrationPage.FillRegistrationDataInFirstStep(
				email,
				RegistrationPage.Password,
				RegistrationPage.Password);
			if(!valid)
				Assert.IsTrue(RegistrationPage.GetInvalidEmailMsgIsDisplayed(), "Ошибка: Сообщение \"Invalid e-mail\" не появилось, но email невалидный");
			else
				Assert.IsTrue(!RegistrationPage.GetInvalidEmailMsgIsDisplayed(), "Ошибка: Сообщение \"Invalid e-mail\" появилось, но email валидный");
		}

		/// <summary>
		/// Проверка пароля на валидность, все парлои невалидны - кнопка SignUp неактивна 
		/// </summary>
		/// <param name="password">пароль</param>
		/// <param name="confirmPasword">подтверждение пароля</param>
		[TestCase("", "")]
		[TestCase(" ", " ")]
		[TestCase("1", "1")]
		[TestCase("12345", "12345")]
		[TestCase("     ", "     ")] // 5 пробелов
		//[TestCase("      ", "      ")] // 6 пробелов - пока что непонятно
		//[TestCase("qwe qwe qwe", "qwe qwe qwe")] пока что непонятно с требованиями PRX-5858
		[Test]
		public void CheckPasswordCompanyValidation(string password, string confirmPasword)
		{
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.Company);
			// Заполняем все поля на первом шаге регистрации
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				password,
				confirmPasword);
			//TODO проверка , что сообщение "Пароль не может быть короче 6 символов" появилось если пароль короткий
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка: Пароль невалидный, но кнопка активна!");
		}

		/// <summary>
		/// Проверить,что сообешние "Passwords don't match" появилось при вводе разных паролей
		/// </summary>
		[Test]
		public void CheckPassowrdMatch()
		{
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.Company);
			// Заполняем все поля на первом шаге регистрации
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				RegistrationPage.Password,
				RegistrationPage.Password + 1);
			// Нажимаем кнопку Sign Up
			RegistrationPage.ClickSignUpButton();
			Assert.IsTrue(RegistrationPage.GetPasswordMatchMsgIsDisplayed(), "Ошибка: сообщение \"Passwords don't match\" не появилось ");

		}
		/// <summary>
		/// Тест - Проверка, что кнопка активна/неактивна при валидном или невалидном значении в название компании на втором шаге регистрации компании http://dev.perevedem.ru/wiki/Corporate_Registration#.D0.92.D0.B2.D0.BE.D0.B4_.D0.B4.D0.B0.D0.BD.D0.BD.D1.8B.D1.85_.D0.BA.D0.BE.D0.BC.D0.BF.D0.B0.D0.BD.D0.B8.D0.B8
		/// </summary>
		/// <param name="companyName">значение для заполнения навзвания компании</param>
		/// <param name="state">валидное или не валидное значение</param>
		[Test]
		[TestCase("s", false)]
		[TestCase("WwwwwwwwwwwwwwwwwwwwWwwwwwwasdsadsadasdd1", false)] //41 symbols - max
		[TestCase("1kjgkjg", false)]
		[TestCase("'kjgkjg", false)]
		public void CheckCompanyNameValidation(string companyName, bool state) 
		{
			// Переход на страницу регистрации компании
			GoToRegistrationPage(RegistrationType.Company);
			// Заполняем все поля на первом шаге регистрации компании
			RegistrationPage.FillRegistrationDataInFirstStep(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.Password);
			// Нажимаем кнопку Sign Up button
			RegistrationPage.ClickSignUpButton();
			// Заполняем все поля на втором шаге регистрации компании
			FillAllFieldsSecondStepCompanyRegistration(
				RegistrationPage.FirstName,
				RegistrationPage.LastName,
				companyName,
				RegistrationPage.DomainName,
				"123123213123213");
			if(!state)
				Assert.IsTrue(CheckCreateAccBtnDisable(), WARNING_MESSAGE_BUTTON_IS_DISABLE);
			else
				Assert.IsFalse(CheckCreateAccBtnDisable(), WARNING_MESSAGE_BUTTON_IS_ABLE);
		}

		/// <summary>
		/// Тест - Проверка, что кнопка активна/неактивна при валидном или невалидном значении домена компании на втором шаге регистрации компании http://dev.perevedem.ru/wiki/Corporate_Registration#.D0.92.D0.B2.D0.BE.D0.B4_.D0.B4.D0.B0.D0.BD.D0.BD.D1.8B.D1.85_.D0.BA.D0.BE.D0.BC.D0.BF.D0.B0.D0.BD.D0.B8.D0.B8
		/// </summary>
		/// <param name="domain">значение для заполнения  домена компании</param>
		/// <param name="state">валидное или не валидное значение</param>
		[Test]
		[TestCase("asdddddddddddddddddddddddddzzzd", false)] //31
		[TestCase("as", false)]
		[TestCase("wwwasddddddd", false)]
		[TestCase("www.asddddddd", false)]
		[TestCase("-asddddddd", false)]
		[TestCase("_asddddddd", false)]
		[TestCase("2asddddddd", false)]
		public void CheckCompanyDomainValidation(string domain, bool state)
		{
			// Переход на страницу регистрации компании
			GoToRegistrationPage(RegistrationType.Company);
			// Заполняем все поля на первом шаге регистрации компании
			RegistrationPage.FillRegistrationDataInFirstStep(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.Password);
			// Нажимаем кнопку Sign Up button
			RegistrationPage.ClickSignUpButton();
			// Заполняем все поля на втором шаге регистрации компании
			FillAllFieldsSecondStepCompanyRegistration(
				RegistrationPage.FirstName,
				RegistrationPage.LastName,
				RegistrationPage.NameCompany,
				domain,
				"123123213123213");
			if (!state)
				Assert.IsTrue(CheckCreateAccBtnDisable(), WARNING_MESSAGE_BUTTON_IS_DISABLE);
			else
				Assert.IsFalse(CheckCreateAccBtnDisable(), WARNING_MESSAGE_BUTTON_IS_ABLE);
		}

		/// <summary>
		/// Тест - Проверка, что нельзя зарегистрировать две компании с одинаковыми именами
		/// </summary>
		[Test]
		public void CheckTwoTheSameCompanies()
		{
			RegisterNewUserWithCompanyAndCheckWS(RegistrationPage.Email, RegistrationPage.Password);
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegisterAsExistUserWithCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);
			FillAllFieldsSecondStepCompanyRegistration(
				RegistrationPage.FirstName,
				RegistrationPage.LastName,
				RegistrationPage.NameCompany,
				RegistrationPage.DomainName+"1",
				"123123213123213");
			Assert.IsTrue(CheckCreateAccBtnDisable(), WARNING_MESSAGE_BUTTON_IS_DISABLE);
		}

		/// <summary>
		/// Тест - Проверка, что при вводе неверного пароля появляется сообщение "Wrong password" 
		/// </summary>
		[Test]
		public void WrongPasswordTest()
		{
			RegisterNewUserWithCompanyAndCheckWS(RegistrationPage.Email, RegistrationPage.Password);
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.Password);
			RegistrationPage.ClickSignUpButton();
			Assert.IsTrue(RegistrationPage.CheckErrorMessageThatUserIsAlreadyExist(), "Ошибка: сообщение о том, что юзер уже существует не появилось");
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegisterAsExistUserWithInCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);
		}

		/// <summary>
		/// Метод проверки активности кнопки Create Company Account 
		/// </summary>
		private bool CheckCreateAccBtnDisable()
		{
			RegistrationPage.ClickCreateAccountCompanyBtn();
			return !WorkspacePage.WaitPageLoad();
		}

		/// <summary>
		/// Метод - регистрация , как существующий пользователь , заполнение полей на втором шаге регистрации и проверка WS
		/// </summary>
		public void RegisterExistUserAndCheckWS(string email, string password, string nameCompany, string domainName)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(
				email,
				password,
				password);
			RegistrationPage.ClickSignUpButton();
			Assert.IsTrue(RegistrationPage.CheckErrorMessageThatUserIsAlreadyExist(), "Ошибка: сообщение о том, что юзер уже существует не появилось");
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegisterAsExistUserWithCorrectPassword(email, password);
			FillAllFieldsSecondStepCompanyRegistration(
				RegistrationPage.FirstName,
				RegistrationPage.LastName,
				nameCompany,
				domainName,
				"123123213123213");
			RegistrationPage.ClickCreateAccountCompanyBtn();
			Console.WriteLine("WorkspacePage.GetUserName() = " + WorkspacePage.GetUserName() + " ; \n RegistrationPage.firstName & RegistrationPage.lastName = " + RegistrationPage.FirstName + " " + RegistrationPage.LastName);
			WorkspacePage.ClickAccount();
			Thread.Sleep(10);
			Assert.IsTrue(
				(WorkspacePage.GetCompanyName() == nameCompany), "Ошибка: название компании неверно отображается в панели WS");
			Assert.IsTrue(
				(WorkspacePage.GetUserName() == RegistrationPage.FirstName + " " + RegistrationPage.LastName), "Ошибка: имя представителя компании неверно отображается в панели WS");
		}

		/// <summary>
		/// Метод - регистрация , как существующий пользователь через Login из сообщения, заполнение полей на втором шаге регистрации и проверка WS
		/// </summary>
		public void RegisterExistUserThrowLogInMsgAndCheckWS(string email, string password, string nameCompany, string domainName)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(
				email,
				password,
				password);
			RegistrationPage.ClickSignUpButton();
			Assert.IsTrue(RegistrationPage.CheckErrorMessageThatUserIsAlreadyExist(), "Ошибка: сообщение о том, что юзер уже существует не появилось");
			RegistrationPage.ClickLogInFromMsg();
			RegisterAsExistUserWithCorrectPassword(email, password);
			FillAllFieldsSecondStepCompanyRegistration(
				RegistrationPage.FirstName,
				RegistrationPage.LastName,
				nameCompany,
				domainName,
				"123123213123213");
			RegistrationPage.ClickCreateAccountCompanyBtn();
			WorkspacePage.ClickAccount();
			Thread.Sleep(10);
			Assert.IsTrue(
				(WorkspacePage.GetCompanyName() == nameCompany), "Ошибка: название компании неверно отображается в панели WS");
			Assert.IsTrue(
				(WorkspacePage.GetUserName() == RegistrationPage.FirstName + " " + RegistrationPage.LastName), "Ошибка: имя представителя компании неверно отображается в панели WS");
		}

		protected const string WARNING_MESSAGE_BUTTON_IS_DISABLE=
			"Ошибка: кнопка активна (должна быть неактивна), WS стр загрузилась (не должна грузиться)";
		protected const string WARNING_MESSAGE_BUTTON_IS_ABLE =
			"Ошибка: кнопка неактивна (должна быть активна), WS стр не загрузилась (должна грузиться)";
	}
}