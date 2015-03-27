using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures;

namespace AbbyyLS.CAT.Function.Selenium.Tests.RegistrationTests
{

	public class RegistrationBaseTest : AdminTest
	{
		/// <summary>
		/// Конструктор теста регистрации фрилансера
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public RegistrationBaseTest(string browserName)
			: base(browserName)
		{

		}

		[SetUp]
		public void SetUp()
		{
			if (Standalone)
			{
				Assert.Ignore("Тест игнорируется, так как это отделяемое решение");
			}
		}

		/// <summary>
		/// Метод регистрации нового фрилансера
		/// </summary>
		public void RegistrationNewUser(string email, string password)
		{
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.User);
			// Заполняем все поля на первом шаге регистрации
			RegistrationPage.FillRegistrationDataInFirstStep(email, password, password);
			// Нажимаем кнопку Sign up
			RegistrationPage.ClickSignUpButton();
			// Заполняем все поля на втором шаге регистрации
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			// Нажимаем кнопку Create Account
			RegistrationPage.ClickCreateAccountButton();
		}

		/// <summary>
		/// Ввод неправильного пароля на стр corp-reg
		/// </summary>
		public void RegisterAsExistUserWithInCorrectPassword(string email, string correctPassword)
		{
			RegistrationPage.TypeTextInEmailFieldSignIn(email);
			RegistrationPage.TypeTextInPasswordFieldSignIn(correctPassword + "1");
			RegistrationPage.ClickSignInButton();
			Assert.IsTrue(
				RegistrationPage.GetWrongPasswordMsgIsDisplay(),
				"Ошибка: сообщение \"Wrong password\" не появилось(должно появится, тк пароль неверный)");
		}

		/// <summary>
		/// Метод - регистрация , как существующий пользователь по ссылке "your ABBYY Online account"
		/// </summary>
		public void RegisterAsExistUserWithCorrectPassword(string email, string password)
		{
			RegistrationPage.TypeTextInEmailFieldSignIn(email);
			RegistrationPage.TypeTextInPasswordFieldSignIn(password);
			RegistrationPage.ClickSignInButton();
		}

		/// <summary>
		/// Регистрация новой компании на стр corp-reg
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		public void RegisterNewUserWithCompanyAndCheckWS(string email, string password)
		{
			GoToRegistrationPage(RegistrationType.Company);
			RegistrationPage.FillRegistrationDataInFirstStep(email, password, password);
			RegistrationPage.ClickSignUpButton();
			RegistrationPage.WaitSecondStepPageLoad();
			FillAllFieldsSecondStepCompanyRegistration(
				RegistrationPage.FirstName,
				RegistrationPage.LastName,
				RegistrationPage.NameCompany,
				RegistrationPage.DomainName,
				"123123213123213");
			RegistrationPage.ClickCreateAccountCompanyBtn();
			Assert.IsTrue(WorkspacePage.WaitPageLoad(), "Страница workspace не прогрузилась");
			Logger.Trace("WorkspacePage.GetCompanyName() NEWUSER = " + WorkspacePage.GetCompanyName() + ";\nRegistrationPage.nameCompany NEW USER = " + RegistrationPage.NameCompany);
			Assert.IsTrue(
				(WorkspacePage.GetCompanyName() == RegistrationPage.NameCompany), "Ошибка: название компании неверно отображается в панели WS");
			Assert.IsTrue(
				(WorkspacePage.GetUserName() == RegistrationPage.FirstName + " " + RegistrationPage.LastName), "Ошибка: имя представителя компании неверно отображается в панели WS");
		}


		/// <summary>
		/// Метод - заполнение полей на втором шаге регистрации
		/// </summary>
		/// <param name="firstName">имя</param>
		/// <param name="lastName">фамилия</param>
		/// <param name="companyName">название компании</param>
		/// <param name="domainName">имя домена</param>
		/// <param name="phoneNumber">номер телефона</param>
		/// <param name="companyType">номер опции в комбобоксе тип компании</param>
		public void FillAllFieldsSecondStepCompanyRegistration(
			string firstName, 
			string lastName, 
			string companyName, 
			string domainName, 
			string phoneNumber, 
			CompanyType companyType = CompanyType.LanguageServiceProvider)
		{
			RegistrationPage.FillFirstNameCompany(firstName);
			RegistrationPage.FillLastNameCompany(lastName);
			RegistrationPage.FillNameCompany(companyName);
			RegistrationPage.FillDomainNameCompany(domainName);
			RegistrationPage.FillPhoneNumberCompany(phoneNumber);
			RegistrationPage.SelectCompanyType((int)companyType);
		}
	}
}
