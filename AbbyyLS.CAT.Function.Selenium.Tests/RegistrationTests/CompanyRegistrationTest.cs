using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures;
using AbbyyLS.CAT.Function.Selenium.Tests.Driver;
using AbbyyLS.CAT.Function.Selenium.Tests.RegistrationTests;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Registration.Company
{
	/// <summary>
	/// Тесты регистрации компании
	/// </summary>
	public class CompanyRegistrationTest<TWebDriverSettings> : RegistrationBaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		private string _corpAccountNameInAdmin = "";

		/// <summary>
		/// Тест - регистрация нововoй компании п 1.1
		/// </summary>
		[Test]
		public void RegisterNewUserCompany()
		{
			RegisterNewUserWithCompanyAndCheckWS(RegistrationPage.Email, RegistrationPage.Password);
		}

		/// <summary>
		/// Тест - регистрация пользователя, созданного через админку(с корп.аккаунтом)
		/// </summary>
		[Test]
		public void RegisterUserWithCorpAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			_corpAccountNameInAdmin = CreateCorporateAccount("", true);
			AddUserToCorpAccount(RegistrationPage.Email);
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
			LoginPage.WaitPageLoad();
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
			LoginToAdminPage();
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			RegisterExistUserAndCheckWS(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.NameCompany, RegistrationPage.DomainName);
		}

		/// <summary>
		/// Тест - Регистрация пользователя с активным персональным аккаунтом (создать в админке, добавить перс аккаунт)
		/// </summary>
		[Test]
		public void RegisterUserWithActivePersAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			CreateNewPersonalAccount(RegistrationPage.LastName, true);
			RegisterExistUserAndCheckWS(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.NameCompany, RegistrationPage.DomainName);
		}

		/// <summary>
		/// Тест - Регистрация пользователя с неактивным персональным аккаунтом (создать в админке, добавить перс аккаунт, снять галку)
		/// </summary>
		[Test]
		public void RegisterUserWithInactivePersAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			CreateNewPersonalAccount(RegistrationPage.LastName, false);
			RegisterExistUserAndCheckWS(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.NameCompany, RegistrationPage.DomainName);
		}

		/// <summary>
		/// Тест регистрации юзера с существующим активным или неактивным аккаунтом в coursera/aol ( or log in with your ABBYY Online account )
		/// </summary>
		[Category("ForLocalRun")]
		[TestCase(0, "active AOL user - 1st row in  TestUsers.xml file")]
		[TestCase(1, "inactive AOL user - 2nd row in  TestUsers.xml file")]
		[TestCase(2, "active Coursera user - 3th row in  TestUsers.xml file")]
		[TestCase(3, "inactive Coursera user - 4th row in  TestUsers.xml file")]
		public void RegisterUserFromExternalSite(int userNumber, string userTest)
		{
			// если TestUsers.xml отсутствует в папке config, то пропускаем тест
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
			LoginPage.WaitPageLoad();
			LoginPage.EnterLogin(RegistrationPage.Email);
			LoginPage.EnterPassword(RegistrationPage.Password);
			LoginPage.ClickSubmitCredentials();
			if (LoginPage.WaitAccountExist(_corpAccountNameInAdmin, 5))
				LoginPage.ClickAccountName(_corpAccountNameInAdmin);
			else if (LoginPage.GetIsErrorExist())
				Assert.Fail("Появилась ошибка при входе! М.б.недоступен AOL.");
			WorkspacePage.ClickAccount();
			Assert.IsTrue(
				(WorkspacePage.GetCompanyName() == _corpAccountNameInAdmin), "Ошибка: название компании неверно отображается в панели WS");
			Assert.IsTrue(
				(WorkspacePage.GetUserName() == RegistrationPage.FirstName + " " + RegistrationPage.LastName), "Ошибка: имя представителя компании неверно отображается в панели WS");
		}

		/// <summary>
		/// Тест - Проверка, что кнопка неактивна если не все обязательные поля заполнены на на втором шаге регистрации компании
		/// </summary>
		/// <param name="field">поле для заполнения</param>
		[TestCase(RegistrationField.FirstName)]
		[TestCase(RegistrationField.LastName)]
		[TestCase(RegistrationField.CompanyName)]
		[TestCase(RegistrationField.DomainName)]
		[TestCase(RegistrationField.PhoneNumber)]
		[Test]
		public void CheckCreateAccBtnDisable(RegistrationField field)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.Password);
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
		[TestCase("a@a", true)]
		[TestCase("'@a", false)]
		[TestCase("\\@a", false)]
		[TestCase("!@a", false)]
		[TestCase("@@a", false)]
		[TestCase("\"@a", false)]
		[TestCase("#@a", false)]
		[TestCase("№@a", false)]
		[TestCase(";@a", false)]
		[TestCase("$@a", false)]
		[TestCase(":@a", false)]
		[TestCase("&@a", false)]
		[TestCase("?@a", false)]
		[TestCase("*@a", false)]
		[TestCase("_@a", true)]
		[TestCase("-@a", true)]
		[TestCase("=@a", false)]
		[TestCase("`@a", false)]
		[TestCase("[@a", false)]
		[TestCase("]@a", false)]
		[TestCase("{@a", false)]
		[TestCase("}@a", false)]
		[TestCase("|@a", false)]
		[TestCase("/@a", false)]
		[TestCase(",@a", false)]
		[TestCase(".@a", true)]
		[TestCase("a@'", false)]
		[TestCase("a@\\", false)]
		[TestCase("a@!", false)]
		[TestCase("a@@", false)]
		[TestCase("a@\"", false)]
		[TestCase("a@#", false)]
		[TestCase("a@№", false)]
		[TestCase("a@;", false)]
		[TestCase("a@$", false)]
		[TestCase("a@:", true)]
		[TestCase("a@&", false)]
		[TestCase("a@?", false)]
		[TestCase("a@*", false)]
		[TestCase("a@_", true)]
		[TestCase("a@-", true)]
		[TestCase("a@=", false)]
		[TestCase("a@`", false)]
		[TestCase("a@[", true)]
		[TestCase("a@]", true)]
		[TestCase("a@{", false)]
		[TestCase("a@}", false)]
		[TestCase("a@|", false)]
		[TestCase("a@/", false)]
		[TestCase("a@,", false)]
		[TestCase("a@.", true)]
		[TestCase("dfsdfsdf@google.com", true)]
		[TestCase("emailgoogle.com", false)]
		[TestCase("emaifuyyl@goog le.com", false)]
		[TestCase("tab	fyyl@google.com", false)] //tab
		[TestCase("", false)]
		[TestCase(" ", false)]
		[TestCase("64charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr@253charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr.com", true)] //64 - local part, 253 - domen
		[TestCase("65charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr@253charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr.com", false)] //65 - local part, 253 - domen
		[TestCase("64charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr@254charrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr.com", false)] //64 - local part, 254 - domen
		[TestCase("email@goo@sfd@ffgl@khhje.com", false)]
		[TestCase("gfgfgfgfgfgfgggfgfgf", false)]
		[Test]
		public void CheckEmailCompanyValidation(string email, bool valid)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(
				email,
				RegistrationPage.Password,
				RegistrationPage.Password);
			
			if(!valid)
				Assert.IsTrue(RegistrationPage.GetInvalidEmailMsgIsDisplayed(), "Ошибка: Сообщение \"Invalid e-mail\" не появилось, но email невалидный");
			else
				Assert.IsFalse(RegistrationPage.GetInvalidEmailMsgIsDisplayed(), "Ошибка: Сообщение \"Invalid e-mail\" появилось, но email валидный");
		}

		/// <summary>
		/// Проверка пароля на валидность (минимальная длина пароля 6 символов)
		/// </summary>
		/// <param name="password">пароль</param>
		[TestCase(" ")]
		[TestCase("1")]
		[TestCase("12345")]
		//[TestCase("qwe qwe qwe", "qwe qwe qwe")] пока что непонятно с требованиями PRX-5858
		[Test]
		public void CheckPasswordCompanyValidationMinLenght(string password)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				password,
				password);
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка: Пароль невалидный, но кнопка активна!");
			Assert.IsTrue(RegistrationPage.GetIsMsgWrongPasswordMinLenghtDisplay(), "Ошибка: Не отображается сообщение о невалидности пароля (минимальная длина)!");
		}

		/// <summary>
		/// Проверка пароля на валидность (пустой пароль)
		/// </summary>
		/// <param name="password">пароль</param>
		[TestCase("")]
		[Test]
		public void CheckPasswordCompanyValidationRequired(string password)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				password,
				password);
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка: Пароль невалидный, но кнопка активна!");
			Assert.IsTrue(RegistrationPage.GetIsMsgWrongPasswordRequiredDisplay(), "Ошибка: Не отображается сообщение о невалидности пароля (пароль отсутствует)!");
		}

		/// <summary>
		/// Проверка пароля на валидность (из одних пробелов)
		/// </summary>
		/// <param name="password">пароль</param>
		[TestCase("      ")] // 6 пробелов
		[Test]
		public void CheckPasswordCompanyValidationSpaces(string password)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				password,
				password);
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка: Пароль невалидный, но кнопка активна!");
			Assert.IsTrue(RegistrationPage.GetIsMsgWrongPasswordSpacesDisplay(), "Ошибка: Не отображается сообщение о невалидности пароля (состоит из одних пробелов)!");
		}

		/// <summary>
		/// Проверить,что сообешние "Passwords don't match" появилось при вводе разных паролей
		/// </summary>
		[Test]
		public void CheckPasswordMatch()
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				RegistrationPage.Password,
				RegistrationPage.Password + 1);
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
		[TestCase("1kjgkjg", false)]
		[TestCase("'kjgkjg", false)]
		public void CheckCompanyNameValidation(string companyName, bool state) 
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email, 
				RegistrationPage.Password, 
				RegistrationPage.Password);
			RegistrationPage.ClickSignUpButton();
			FillAllFieldsSecondStepCompanyRegistration(
				RegistrationPage.FirstName,
				RegistrationPage.LastName,
				companyName,
				RegistrationPage.DomainName,
				"123123213123213");
			Assert.IsTrue(CheckCreateAccBtnDisable(), WARNING_MESSAGE_BUTTON_IS_DISABLE);
		}

		
		/// <summary>
		/// Проверяем, что название компании обрезается после ввода
		/// </summary>
		[Test]
		public void CompanyNameMaxLenght()
		{
			var companyName = RandomString.GenerateString(41);
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				RegistrationPage.Password,
				RegistrationPage.Password);
			RegistrationPage.ClickSignUpButton();
			FillAllFieldsSecondStepCompanyRegistration(
				RegistrationPage.FirstName,
				RegistrationPage.LastName,
				companyName,
				RegistrationPage.DomainName,
				"123123213123213");
			Assert.AreEqual(RegistrationPage.GetCompanyName(), companyName.Substring(0, companyName.Length - 1),
				"Ошибка: название компании не обрезается до 40 символов");
			Assert.IsFalse(CheckCreateAccBtnDisable(), WARNING_MESSAGE_BUTTON_IS_ABLE);
		}

		/// <summary>
		/// Тест - Проверка, что кнопка активна/неактивна при валидном или невалидном значении домена компании на втором шаге регистрации компании http://dev.perevedem.ru/wiki/Corporate_Registration#.D0.92.D0.B2.D0.BE.D0.B4_.D0.B4.D0.B0.D0.BD.D0.BD.D1.8B.D1.85_.D0.BA.D0.BE.D0.BC.D0.BF.D0.B0.D0.BD.D0.B8.D0.B8
		/// </summary>
		/// <param name="domain">значение для заполнения  домена компании</param>
		/// <param name="state">валидное или не валидное значение</param>
		[Test]
		[TestCase("as", false)]
		[TestCase("wwwasddddddd", false)]
		[TestCase("www.asddddddd", false)]
		[TestCase("-asddddddd", false)]
		[TestCase("_asddddddd", false)]
		[TestCase("2asddddddd", false)]
		public void CheckCompanyDomainValidation(string domain, bool state)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(RegistrationPage.Email, RegistrationPage.Password, RegistrationPage.Password);
			RegistrationPage.ClickSignUpButton();
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
			LoginPage.WaitPageLoad();
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegisterAsExistUserWithCorrectPassword(RegistrationPage.Email, RegistrationPage.Password);
			RegistrationPage.WaitSecondStepPageLoad();
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
			LoginPage.WaitPageLoad();
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
		/// Метод - регистрация, как существующий пользователь, заполнение полей на втором шаге регистрации и проверка WS
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <param name="nameCompany"></param>
		/// <param name="domainName"></param>
		public void RegisterExistUserAndCheckWS(string email, string password, string nameCompany, string domainName)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegisterAsExistUserWithCorrectPassword(email, password);
			RegistrationPage.WaitSecondStepPageLoad();
			FillAllFieldsSecondStepCompanyRegistration(
				RegistrationPage.FirstName,
				RegistrationPage.LastName,
				nameCompany,
				domainName,
				"123123213123213");
			RegistrationPage.ClickCreateAccountCompanyBtn();
			Assert.IsTrue(WorkspacePage.WaitPageLoad(), "Страница workspace не прогрузилась");
			Logger.Trace("WorkspacePage.GetUserName() = " + WorkspacePage.GetUserName() + " ; \n RegistrationPage.firstName & RegistrationPage.lastName = " + RegistrationPage.FirstName + " " + RegistrationPage.LastName);
			WorkspacePage.ClickAccount();
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