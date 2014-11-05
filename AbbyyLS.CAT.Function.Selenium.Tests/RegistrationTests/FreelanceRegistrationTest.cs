using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Registraion.Freelance
{
	/// <summary>
	/// Тесты регистрации фрилансера
	/// </summary>
	public class FreelanceRegistrationTest : AdminTest
	{
		/// <summary>
		/// Конструктор теста регистрации фрилансера
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public FreelanceRegistrationTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Метод регистрации нового юзера
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
		/// Тест регистрации нового юзера
		/// </summary>
		[Test]

		public void RegistrationNewUserTest()
		{
			RegistrationNewUser(RegistrationPage.Email,RegistrationPage.Password);
			// Проверка ,что имя и фамилия нового фрилансера отображается правильно в хидере
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
			//Нажать на имя юзера,чтоб открыть подменю
			WorkspacePage.ClickAccount();
			//Нажать кнопку выхода
			WorkspacePage.ClickLogoff();
			//Обновить страницу
			RegistrationPage.RefreshLoginPage();
			//Заполнить email
			LoginPage.EnterLogin(RegistrationPage.Email);
			//Заполнить пароль
			LoginPage.EnterPassword(RegistrationPage.Password);
			//Нажать кнопку Sign In
			LoginPage.ClickSubmitCredentials();
			//Проверить что имя и фамилия фрилансера отображается в панели на стр WS
			Assert.IsTrue(
				RegistrationPage.CheckNameSurnameInWSPanel(),
				"Ошибка: имя фрилансера неправильно отображается на странице WS");
		}

		/// <summary>
		/// Тест для проверки,что кнопка Sign up неактивна если пароли разные п.5.1
		/// </summary>
		[Test]
		public void CheckThatSignUpBtnIsDisable()
		{
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.User);
			// Заполняем все поля на первом шаге регистрации
			RegistrationPage.FillRegistrationDataInFirstStep(
				RegistrationPage.Email,
				RegistrationPage.Password,
				"wrong password");
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка: пароли разные, но кнопка активна!");
		}

		/// <summary>
		/// Проверка , что видимости сообщения, о том ,что юзер уже зарегистрирован в системе п. 5.2
		/// </summary>
		[Test]
		public void CheckThatUserIsAlreadyExist()
		{
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.User);
			// Заполняем все поля на первом шаге регистрации
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
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.User);
			// Заполняем все поля на первом шаге регистрации
			RegistrationPage.FillRegistrationDataInFirstStep(RegistrationPage.Email, RegistrationPage.Password, "");
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка: Пароля нет, но кнопка активна!");
		}

		/// <summary>
		/// Проверка, что кнопка SignUp неактивна,  если пароль - пробел п. 5.4
		/// </summary>
		[Test]
		public void CheckThatSignUpBtnIsDisableWhenPasswordIsSpace()
		{
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.User);
			// Заполняем все поля на первом шаге регистрации
			RegistrationPage.FillRegistrationDataInFirstStep(RegistrationPage.Email, " ", " ");
			Assert.IsTrue(RegistrationPage.CheckThatSignUpButtonIsDisable(), "Ошибка:  Пароль - пробел, но кнопка активна!");
		}

		/// <summary>
		/// Тест для проверки, что Load photo отображается в поле для фото файла 6.1.
		/// </summary>
		[Test]
		public void CheckPhotoLabel()
		{
			// Переход на страницу регистрации
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
		public void CheckBtnIsDisableWhenLevelIsNotSet()
		{
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.User);
			// Заполняем все поля на первом шаге регистрации
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
		[TestCase("testsds@dsdom")]
		[Test]
		public void CheckInvalidEmails(string invalidEmail)
		{
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.User);
			// Заполняем все поля на первом шаге регистрации
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
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegistrationPage.TypeTextInEmailFieldSignIn(RegistrationPage.Email);
			RegistrationPage.TypeTextInPasswordFieldSignIn(RegistrationPage.Password);
			RegistrationPage.ClickSignInButton();
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			RegistrationPage.ClickCreateAccountButton();
			// проверка ,что имя и фамилия нового фрилансера отображается правильно в хидере
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
			// Переход на страницу регистрации
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
			// Переход на страницу регистрации пользователя
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
			//процесс добавления фото
			RegistrationPage.ClickLoadPhotoBtn();
			FillAddDocumentForm(photo);
		}

		/// <summary>
		/// Проверка загрузки фото на втором шаге регистрации
		/// </summary>
		/// <param name="format">валидный или невалидный файл</param>
		/// <param name="photo">имя файла фото</param>
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
			// Переход на страницу регистрации
			GoToRegistrationPage(RegistrationType.User);
			// Заполняем все поля на первом шаге регистрации
			RegistrationPage.FillRegistrationDataInFirstStep(
			   RegistrationPage.Email,
			   RegistrationPage.Password,
			   RegistrationPage.Password);
			RegistrationPage.ClickSignUpButton();
			ImportPhoto(PhotoLoad + photo);
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
			// Логинемся в админке
			LoginToAdminPage();
			// Создаем нового юзера
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.NickName, RegistrationPage.Password);
			// Переходим на стр корпоративных аккаунтов
			SwitchEnterpriseAccountList();
			// Нажать Создать
			AdminPage.ClickAddAccount();
			// Переключаемся в новое окно браузера
			Driver.SwitchTo().Window(Driver.WindowHandles[1]);
			bool isWindowWithForm = AdminPage.GetIsAddAccountFormDisplay();
			Assert.IsTrue(isWindowWithForm, "Ошибка: не нашли окно с формой создания аккаунта");
			// Заполняем поля для создания корп аккаунта
			string accountName = FillGeneralAccountFields();
			// Нажать кнопку сохранить
			AdminPage.ClickSaveBtn();
			AddUserToAccount(RegistrationPage.Email);
			// Регистрируем нового фрилансера
			RegisterUserWithExistAccount(RegistrationPage.Email, RegistrationPage.Password);
			WorkspacePage.ClickAccount();
			// Проверка, что список корп аккаунтов верный
			WorkspacePage.CheckAccountList(accountName);
		}

		/// <summary>
		/// Регистрация юзера с активным персональным аккаунтом п 3.8
		/// </summary>
		[Test]
		public void RegisterNewUserWithActivePersAcc()
		{
			LoginToAdminPage();
			// Создать нового юзера
			CreateNewUserInAdminPage(RegistrationPage.Email, RegistrationPage.FirstName, RegistrationPage.Password);
			// Создать персональный аккаунт активный
			CreateNewPersAcc(RegistrationPage.LastName, true);
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
			CreateNewPersAcc(RegistrationPage.LastName, false);
			GoToRegistrationPage(RegistrationType.User);
			LoginAsExistUser();
			// Заполняем все поля на втором шаге регистрации
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			// Нажимаем кнопку Create Account
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
			// Нажимаем кнопку Sign up
			RegistrationPage.ClickSignUpButton();
			Assert.IsTrue(RegistrationPage.CheckErrorMessageThatUserIsAlreadyExist(), "Ошибка: сообщение о том,что юзер уже существует, не появилось (а должно появиться!)");
			RegistrationPage.GoToLoginPageWithExistAccount();
			RegistrationPage.TypeTextInEmailFieldSignIn(email);
			RegistrationPage.TypeTextInPasswordFieldSignIn(password);
			RegistrationPage.ClickSignInButton();
			// Заполняем все поля на втором шаге регистрации
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			// Нажимаем кнопку Create Account
			RegistrationPage.ClickCreateAccountButton();
			Assert.IsTrue(RegistrationPage.CheckNameSurnameInWSPanel(), "Ошибка: имя фрилансера неправильно отображаются на странице WS");
		}



		/// <summary>
		///Тест регистрации юзера с существующим активным или неактивным аккаунтом в coursera/perevedem/aol
		/// </summary>
		[Test]
		public void LoginAsUserWithAccountFromExternalSite()
		{
			// если TestUsers.xml отсутствует в папке config, то порпускаем тест
			if (!TestUserFileExist())
			{
				Assert.Ignore("Файл TestUsers.xml с тестовыми пользователями отсутствует");
			}

			// перебор юзеров из конфига 
			for (int i = 0; i < TestUserList.Count; i++)
			{
				// здесь будет условие , если Activated = false/true  заведен тикет с уточнением требований, пока не решен вопрос - PRX-5519
				//if (TestUserList[i].Activated)
				//{
				RegisterUserWithExistAccount(TestUserList[i].login, TestUserList[i].password);
				//Нажать на имя юзера,чтоб открыть подменю
				WorkspacePage.ClickAccount();
				//Нажать кнопку выхода
				WorkspacePage.ClickLogoff();
				//}
			}
		}
		}

	}
