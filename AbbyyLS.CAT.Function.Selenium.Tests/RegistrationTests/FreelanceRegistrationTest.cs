using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures;
using AbbyyLS.CAT.Function.Selenium.Tests.Driver;
using AbbyyLS.CAT.Function.Selenium.Tests.RegistrationTests;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Registration.Freelance
{
	/// <summary>
	/// Тесты регистрации фрилансера
	/// </summary>
	public class FreelanceRegistrationTest<TWebDriverSettings> : RegistrationBaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Тест регистрации нового юзера
		/// </summary>
		[Test]
		public void RegistrationNewUserTest()
		{
			RegistrationNewUser(RegistrationPage.Email,RegistrationPage.Password);
			Assert.IsTrue(
				RegistrationPage.CheckNameSurnameInWSPanel(), "Ошибка: имя фрилансера неправильно отображается на странице WS");
		}

		/// <summary>
		/// Тест для проверки, что новый фрилансер может залогиниться после регистрации
		/// </summary>
		[Test]
		public void LoginNewUserAfterRegistration()
		{
			RegistrationNewUser(RegistrationPage.Email,RegistrationPage.Password);
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
			Logger.Trace("Переход на страницу регистрации фрилансеров");
			Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.SingIn);
			LoginPage.EnterLogin(RegistrationPage.Email);
			LoginPage.EnterPassword(RegistrationPage.Password);
			LoginPage.ClickSubmitCredentials();
			if (LoginPage.CheckEuropeServerIsDisplayed() || LoginPage.CheckUsaServerIsDisplayed())
				LoginPage.ClickAccountName("Personal");

			Assert.IsTrue(
				RegistrationPage.CheckNameSurnameInWSPanel(),
				"Ошибка: имя фрилансера неправильно отображается на странице WS");
		}

		/// <summary>
		/// Тест для проверки,что кнопка Sign up неактивна если пароли разные п.5.1
		/// </summary>
		[Test]
		public void GetPasswordMismatchMsgIsDisplayed()
		{
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				RegistrationPage.Password,
				"wrong password");
			RegistrationPage.ClickSignUpButton();
			Assert.IsTrue(RegistrationPage.GetPasswordMatchMsgIsDisplayed(), "Ошибка: сообщение о том, что пароли разные не появилось!");
		}

		/// <summary>
		/// Проверка , что видимости сообщения, о том ,что юзер уже зарегистрирован в системе п. 5.2
		/// </summary>
		[Test]
		public void CheckThatUserIsAlreadyExist()
		{
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.FillRegistrationDataInFirstStep(Login, RegistrationPage.Password, RegistrationPage.Password);
			RegistrationPage.ClickSignUpButton();
			RegistrationPage.CheckErrorMessageThatUserIsAlreadyExist();
		}

		/// <summary>
		/// Проверка, что кнопка Sign Up неактивна,  без пароля п. 5.3
		/// </summary>
		[Test]
		public void CheckThatSignUpBtnIsDisableWhenNoPassword()
		{
			GoToRegistrationPage(RegistrationType.User);
			FillEmailAndPasswordInFirstStep(RegistrationPage.Email, RegistrationPage.Password);
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка: Пароля нет, но кнопка активна!");
		}

		/// <summary>
		/// Проверка, что кнопка SignUp неактивна,  если пароль - пробел п. 5.4
		/// </summary>
		[Test]
		public void CheckThatSignUpBtnIsDisableWhenPasswordIsSpace()
		{
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.FillRegistrationDataInFirstStep(RegistrationPage.Email, " ", " ");
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка:  Пароль - пробел, но кнопка активна!");
		}

		/// <summary>
		/// Тест для проверки, что Load photo отображается в поле для фото файла 6.1.
		/// </summary>
		[Test]
		public void CheckPhotoLabel()
		{
			Assert.Ignore("Тест инорируем, так как поле для загрузки фото убрали со стр регистрации фрилансеров");

			GoToRegistrationPage(RegistrationType.User);
			// Заполняем все поля на первом шаге регистрации
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				RegistrationPage.Password,
				RegistrationPage.Password);
			RegistrationPage.ClickSignUpButton();
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			Assert.IsTrue(RegistrationPage.CheckLabelForPhoto(), "Ошибка: Load photo не отображается в секции для фото");
		}

		/// <summary>
		/// Тест для проверки, что кнопка SignUp неактивна, если этап для второй пары языков, не указан  PRX-5503 п 6.3.
		/// </summary>
		[Test]
		[Category("PRX_6718")]
		public void CheckBtnIsDisableWhenLevelIsNotSet()
		{
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				RegistrationPage.Password,
				RegistrationPage.Password);
			RegistrationPage.ClickSignUpButton();
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			RegistrationPage.SelectSecondService();
			Assert.IsTrue(RegistrationPage.CheckThatCreateAccountBtnIsDisable(), "Ошибка: Этап для второй пары языков не указан, но кнопка активна!");
		}

		/// <summary>
		/// Проверка email на валидность, все email невалидны - кнопка SignUp неактивна  п.8 
		/// </summary>
		[TestCase("66!/.6@mailforspam.com ")]
		[TestCase("%%%/%%%")]
		[TestCase("‘ or ‘a’ = ‘a'; DROP TABLE user; SELECT * FROM blog WHERE code LIKE ‘a%’;")]
		[TestCase("<script>alert(“Hello, world!”)</alert>, <script>document.getElementByID(“…”).disabled=true</script>")]
		[TestCase("<form action=”http://live.hh.ru”><input type=”submit”></form>")]
		[TestCase("testsdsdsd.com")]
		[TestCase("“♣☺♂” , “”‘~!@#$%^&*()?>,.//*<!–“”, “${code}”;–>")]
		[TestCase("asadsdsa,asddsa@asd.asd")]
		[TestCase("ывавыааыва@ывааываыв.com")]
		[Test]
		public void CheckInvalidEmails(string invalidEmail)
		{
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.FillRegistrationDataInFirstStep(
				invalidEmail,
				RegistrationPage.Password,
				RegistrationPage.Password);
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка: Email невалидный, но кнопка активна!");
		}

		/// <summary>
		/// Создание юзера через админку и авторизация под созданным юзером п 4.1
		/// </summary>
		[Test]
		public void CreateAccountInAdminAndChechRegistration()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password);
			RegistrationPage.ClickSignInButton();
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			RegistrationPage.ClickCreateAccountButton();
			Assert.IsTrue(
				RegistrationPage.CheckNameSurnameInWSPanel(),
				"Ошибка: имя фрилансера неправильно отображается на странице WS");
		}
		
		/// <summary>
		/// Тест - проверка, что сообщение 'Неверный парлоль'  появляется, если пароль неверен - Sign In форма
		/// </summary>
		[Test]
		public void LoginWithExistEmailAndWrongPassword()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);
			RegistrationPage.TypeTextInPasswordFieldSignIn("wrong password");
			RegistrationPage.ClickSignInButton();
			Assert.IsTrue(RegistrationPage.CheckThatErrorMeassageThatPasswordIsWrongDisplay(), "Ошибка: Сообщение 'Пароль неправильный' не появилось");
		}

		/// <summary>
		///Тест - проверка, что сообщение 'Пользователя не существует'  появляется, если исользовать рандомный email - Sign In форма
		/// </summary>
		[Test]
		public void LoginWithNotExitEmail()
		{
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password);
			RegistrationPage.ClickSignInButton();
			Assert.IsTrue(RegistrationPage.CheckThatErrorMeassageThatUserIsNotExistIsDisplay(), "Ошибка: Сообщение 'Пользователя не существует' не появилось");

		}

		/// <summary>
		/// Метод загрузки фото на втором шаге регистрации
		/// </summary>
		/// <param name="photo">фото</param>
		protected void ImportPhoto(string photo)
		{
			RegistrationPage.ClickLoadPhotoBtn();
			RegistrationPage.UploadPhoto(photo);
		}

		/// <summary>
		/// Проверка загрузки фото на втором шаге регистрации
		/// </summary>
		/// <param name="format">валидный или невалидный файл</param>
		/// <param name="photo">имя файла фото</param>
		[Ignore("Функционал загрузки фото закрыли")]
		[TestCase(false, "txtfile.txt")]
		[TestCase(true, "bmpfile.bmp")]
		[TestCase(true, "giffile.gif")]
		[TestCase(true, "dibfile.dib")]
		[TestCase(true, "jpgfile.jpg")]
		[TestCase(true, "jpgfile2.jpg")]
		[TestCase(true, "jpgfile3.jpg")]
		[TestCase(true, "pngfile.png")]
		[Test]
		public void LoadPhotoTest(bool format, string photo)
		{
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.FillRegistrationDataInFirstStep(
			   RegistrationPage.Email,
			   RegistrationPage.Password,
			   RegistrationPage.Password);
			RegistrationPage.ClickSignUpButton();
			ImportPhoto(PathProvider.PhotoLoadFolder + photo);
			if (format)
			{
				Assert.IsFalse(RegistrationPage.WrongFormatLabelISDisplay(), "Ошибка: Надпись Wrong Format появилась(не должна появляться)");
			}
			else
			{
				Assert.IsTrue(RegistrationPage.WrongFormatLabelISDisplay(), "Ошибка: Надпись Wrong Format не появилась(должна появляться)");
			}
		}

		/// <summary>
		///  п.3.7. Проверка,что Пользователь с существующим корп аккаунтом может авторизоваться и список соответсвущих корп аккаунтов отображается на стр WS
		/// </summary>
		[Test]
		public void CreateNewUserInAdminWithEnterAccount()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			SwitchEnterpriseAccountList();
			AdminPage.ClickAddAccount();
			Driver.SwitchTo().Window(Driver.WindowHandles[1]);
			bool isWindowWithForm = AdminPage.GetIsAddAccountFormDisplay();
			Assert.IsTrue(isWindowWithForm, "Ошибка: не нашли окно с формой создания аккаунта");
			string accountName = FillGeneralAccountFields();
			AdminPage.ClickSaveBtn();
			AddUserToAccount(RegistrationPage.Email);
			RegisterUserWithExistAccount(RegistrationPage.Email, RegistrationPage.Password);
			WorkspacePage.ClickAccount();
			WorkspacePage.CheckAccountList(accountName);
		}

		/// <summary>
		/// Регистрация юзера с активным персональным аккаунтом п 3.8
		/// </summary>
		[Test]
		public void RegisterNewUserWithActivePersAcc()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.FirstName, RegistrationPage.Password);
			CreateNewPersonalAccount(RegistrationPage.LastName, true);
			GoToRegistrationPage(RegistrationType.User);
			LoginAsExistUser();
			Assert.True(RegistrationPage.CheckNameInWSPanel(), "Ошибка: имя фрилансера неправильно отображается на странице WS");
		}

		/// </summary>
		///<summary> Mетод авторизации юзера с существующи аккаунтом</summary>
		/// </summary>
		public void LoginAsExistUser()
		{
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password);
			RegistrationPage.ClickSignInButton();
		}

		/// <summary>
		/// Регистрация юзера с неактивным персональным аккаунтом п 3.10
		/// </summary>
		[Test]
		public void RegisterNewUserWithInActivePersAcc()
		{
			LoginToAdminPage();
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			CreateNewPersonalAccount(RegistrationPage.LastName, false);
			GoToRegistrationPage(RegistrationType.User);
			LoginAsExistUser();
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			RegistrationPage.ClickCreateAccountButton();
			//TODO ЕЩЕ НЕ РЕАЛИЗОВАНО, должно появится сообщение, есть тикет, но сейчас не сделано так - PRX-5533
		}

		/// <summary>
		/// Метод для регистрации юзера с существующим аккаунтом
		/// </summary>
		public void RegisterUserWithExistAccount(string email, string password)
		{
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.FillRegistrationDataInFirstStep(email, password, password);
			RegistrationPage.ClickSignUpButton();
			Assert.IsTrue(RegistrationPage.CheckErrorMessageThatUserIsAlreadyExist(), "Ошибка: сообщение о том,что юзер уже существует, не появилось (а должно появиться!)");
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegistrationPage.TypeTextInEmailFieldSignIn(email);
			RegistrationPage.TypeTextInPasswordFieldSignIn(password);
			RegistrationPage.ClickSignInButton();
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			RegistrationPage.ClickCreateAccountButton();
			Assert.IsTrue(RegistrationPage.CheckNameSurnameInWSPanel(), "Ошибка: имя фрилансера неправильно отображаются на странице WS");
		}

		/// <summary>
		/// Заполнить email и пароль на певом шаге регистрации фрилансера
		/// </summary>
		/// <param name="email">email фрилансера</param>
		/// <param name="password">пароль</param>
		public void FillEmailAndPasswordInFirstStep(string email, string password)
		{
			RegistrationPage.TypeTextInEmailField(email);
			RegistrationPage.TypeTextInPasswordField(password);
		}

		/// <summary>
		///Тест регистрации юзера с существующим активным или неактивным аккаунтом в coursera/perevedem/aol
		/// </summary>
		[Category("ForLocalRun")]
		[TestCase(0, "active AOL user (1st row in TestUsers.xml file)")]
		[TestCase(1, "inactive AOL user (2nd row in TestUsers.xml file)")]
		[TestCase(2, "active Coursera user (3th row in TestUsers.xml in file)")]
		[TestCase(3, "inactive Coursera user (4th row in TestUsers.xml in file)")]
		[TestCase(4, "active Perevedem user (5th row in TestUsers.xml file)")]
		[TestCase(5, "inactive Perevedem user (6th row in TestUsers.xml file)")]
		public void LoginAsUserWithAccountFromExternalSite(int userNumber, string userTest)
		{
			// если TestUsers.xml отсутствует в папке config, то порпускаем тест
			if (!TestUserFileExist() || (TestUserList.Count == 0))
			{
				Assert.Ignore("Файл TestUsers.xml с тестовыми пользователями отсутствует или нет данных о юзере");
			}
				RegisterUserWithExistAccount(TestUserList[userNumber].Login, TestUserList[userNumber].Password);
		}

	}
}
