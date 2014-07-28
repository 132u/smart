using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	public class HeaderHelper : CommonHelper
	{
		public HeaderHelper(IWebDriver driver, WebDriverWait wait) :
            base (driver, wait)
        {
        }

		/// <summary>
		/// Залогиниться
		/// </summary>
		/// <param name="login">логин пользователя</param>
		/// <param name="password">пароль пользователя</param>
		public void Login(string login, string password)
		{
			// Открыть форму логина
			if (GetIsElementExist(By.XPath(LOGIN_XPATH)))
				ClickElement(By.XPath(LOGIN_XPATH));
			else 
				ClickElement(By.XPath(LOGIN_HEADER_XPATH));

			// Дождаться открытия формы
			WaitUntilDisplayElement(By.XPath(LOGIN_FORM_LOGIN_XPATH));

			// Заполнить форму
			FillAuthorizationData(login, password);
		}

		/// <summary>
		/// Заполнение формы авторизации пользователя
		/// </summary>
		/// <param name="login">логин</param>
		/// <param name="password">пароль</param>
		public void FillAuthorizationData(string login, string password)
		{
			// Ввести логин и пароль
			ClickClearAndAddText(By.CssSelector(EMAIL_CSS), login);
			ClickClearAndAddText(By.CssSelector(PASSWORD_CSS), password);

			// Нажать Зайти
			ClickElement(By.CssSelector(SUBMIT_CSS));
		}

		/// <summary>
		/// Проверить, открылась ли форма регистрации
		/// </summary>
		/// <returns>Форма открылась</returns>
		public bool GetIsRegistrationFormDisplay()
		{
			return GetIsElementDisplay(By.XPath(LOGIN_FORM_REGISTRATION_XPATH));
		}

		/// <summary>
		/// Разлогиниться
		/// </summary>
		public void LogoutUser()
		{
			ClickElement(By.XPath(LOGOUT_BTN_XPATH));
			WaitUntilDisplayElement(By.XPath(LOGIN_XPATH));
		}

		/// <summary>
		/// Дождаться закрытия формы логина
		/// </summary>
		/// <returns>Форма закрылась</returns>
		public bool WaitUntilLoginFormDisappear()
		{
			// Дождаться закрытия формы
			return WaitUntilDisappearElement(By.XPath(LOGIN_FORM_LOGIN_XPATH));
		}

		/// <summary>
		/// Проверить, присутствуют ли ошибки входа
		/// </summary>
		/// <returns>Ошибка присутствует</returns>
		public bool GetIsLoginErrorPresent()
		{
			return GetIsElementExist(By.XPath(LOGIN_ERROR_XPATH));
		}

		/// <summary>
		/// Проверить, присутствуют ли ошибки пароля
		/// </summary>
		/// <returns>Ошибка присутствует</returns>
		public bool GetIsPasswordErrorPresent()
		{
			return GetIsElementDisplay(By.XPath(PASSWORD_ERROR_XPATH + "//p[@id='error1']"));
		}
		
		/// <summary>
		/// Открыть страницу со списком курсов
		/// </summary>
		public void ClickOpenCoursePage()
		{
			ClickElement(By.XPath(COURSES_BTN_XPATH));
		}

		/// <summary>
		/// Открыть страницу с лидербордом
		/// </summary>
		public void OpenLeaderboardPage()
		{
			ClickElement(By.XPath(LEADERBOARD_BTN_XPATH));
			WaitUntilDisplayElement(By.ClassName(LEADERBOARD_PAGE_CLASSNAME));
		}

		/// <summary>
		/// Открыть домашнюю страницу
		/// </summary>
		public void OpenHomepage()
		{
			ClickElement(By.ClassName(HOME_BTN_CLASSNAME));
			WaitUntilDisplayElement(By.XPath(HOME_PAGE_XPATH));
		}

		/// <summary>
		/// Открыть страницу профиля пользователя
		/// </summary>
		public void OpenProfile()
		{
			ClickElement(By.ClassName(PROFILE_BTN_CLASSNAME));
		}

		/// <summary>
		/// Возвращает имя пользователя
		/// </summary>
		/// <returns>Имя пользователя</returns>
		public string GetName()
		{
			return GetTextElement(By.ClassName(PROFILE_BTN_CLASSNAME));
		}

		/// <summary>
		/// Кликнуть кнопку открытия формы редактора данных пользователя
		/// </summary>
		public void ClickOpenEditorForm()
		{
			ClickElement(By.XPath(EDIT_PROFILE_LINK_XPATH));
		}

		/// <summary>
		/// Дождаться открытия формы редактора данных пользователя
		/// </summary>
		/// <returns>Форма открылась</returns>
		public bool WaitUntilEditorFormDisplay()
		{
			// Дождаться открытия формы
			return WaitUntilDisplayElement(By.XPath(EDITOR_FORM_XPATH));
		}
		
		/// <summary>
		/// Кликнуть кнопку закрытия формы редактора данных пользователя
		/// </summary>
		public void ClickCloseEditorForm()
		{
			ClickElement(By.XPath(EDITOR_CLOSE_XPATH));
		}

		/// <summary>
		/// Дождаться закрытия формы редактора данных пользователя
		/// </summary>
		/// <returns>Форма закрылась</returns>
		public bool WaitUntilEditorFormDisappear()
		{
			// Дождаться закрытия формы
			return WaitUntilDisappearElement(By.XPath(EDITOR_FORM_XPATH));
		}

		/// <summary>
		/// Кликнуть поле смены пароля в форме редактора данных пользователя
		/// </summary>
		public void ClickChangePasswordForm()
		{
			ClickElement(By.XPath(EDITOR_CHANGE_PASSWORD_XPATH));
		}

		/// <summary>
		/// Дождаться открытия формы смены пароля
		/// </summary>
		/// <returns>Форма открылась</returns>
		public bool WaitUntilChangePasswordFormDisplay()
		{
			// Дождаться открытия формы
			return WaitUntilDisplayElement(By.XPath(EDITOR_CHANGE_PASSWORD_FORM_XPATH));
		}

		/// <summary>
		/// Изменяет имя пользователя
		/// </summary>
		/// <param name="name">Новое имя пользователя</param>
		public void ChangeNameProfile(string name)
		{
			ClearAndAddText(By.XPath(EDITOR_PROFILE_EDIT_NAME_XPATH), name);
		}

		/// <summary>
		/// Проверить, что поле ввода имени отмечено ошибкой
		/// </summary>
		/// <returns>Поле ввода отмечено ошибкой</returns>
		public bool GetIsErrorNameInputPresent()
		{
			return GetIsElementDisplay(By.XPath(EDITOR_PROFILE_EDIT_NAME_XPATH + "[contains(@class,'errorInput')]"));
		}

		/// <summary>
		/// Проверить, что присутствует сообщение об ошибке имени
		/// </summary>
		/// <returns>Ошибка присутствует</returns>
		public bool GetIsErrorNameMessagePresent()
		{
			return GetIsElementDisplay(By.XPath(LOGIN_NAME_ERROR_MESSAGE_XPATH));
		}

		/// <summary>
		/// Изменяет фамилия пользователя
		/// </summary>
		/// <param name="surname">Новая фамилия пользователя</param>
		public void ChangeSurnameProfile(string surname)
		{
			ClearAndAddText(By.XPath(EDITOR_PROFILE_EDIT_SURNAME_XPATH), surname);
		}

		/// <summary>
		/// Проверить, что поле ввода фамилии отмечено ошибкой
		/// </summary>
		/// <returns>Поле ввода отмечено ошибкой</returns>
		public bool GetIsErrorSurnameInputPresent()
		{
			return GetIsElementDisplay(By.XPath(EDITOR_PROFILE_EDIT_SURNAME_XPATH + "[contains(@class,'errorInput')]"));
		}

		/// <summary>
		/// Проверить, что присутствует сообщение об ошибке фамилии
		/// </summary>
		/// <returns>Ошибка присутствует</returns>
		public bool GetIsErrorSurnameMessagePresent()
		{
			return GetIsElementDisplay(By.XPath(LOGIN_SURNAME_ERROR_MESSAGE_XPATH));
		}

		/// <summary>
		/// Изменяет инфо пользователя
		/// </summary>
		/// <param name="info">Новое инфо пользователя</param>
		public void ChangeInfoProfile(string info)
		{
			ClearAndAddText(By.XPath(EDITOR_PROFILE_EDIT_ABOUT_XPATH), info);
		}

		/// <summary>
		/// Кликнуть кнопку загрузки аватара
		/// </summary>
		public void ClickAvatarUploadBtn()
		{
			ClickElement(By.XPath(EDITOR_PROFILE_EDIT_UPLOAD_XPATH));
		}

		/// <summary>
		/// Кликнуть кнопку удаления аватара
		/// </summary>
		public void ClickAvatarDeleteBtn()
		{
			ClickElement(By.XPath(EDITOR_PROFILE_EDIT_DELETE_XPATH));
		}

		/// <summary>
		/// Проверить, доступен ли аватар в редакторе профиля
		/// </summary>
		/// <returns>Аватар доступен</returns>
		public bool GetIsAvatarPresentEditorProfile()
		{
			return GetElement(By.XPath(EDITOR_PROFILE_EDIT_AVATAR_XPATH)).GetAttribute("src").Contains("/Files/");
		}

		/// <summary>
		/// Проверить, доступен ли аватар в заголовке
		/// </summary>
		/// <returns>Аватар доступен</returns>
		public bool GetIsAvatarPresentHeader()
		{
			return GetElement(By.XPath(AVATAR_HEADER_XPATH)).GetAttribute("src").Contains("/avatar/");
		}

		/// <summary>
		/// Кликнуть кнопку сохранить данные пользователя
		/// </summary>
		public void ClickSaveProfileBtn()
		{
			ClickElement(By.XPath(EDITOR_PROFILE_EDIT_SAVE_XPATH));
		}

		/// <summary>
		/// Проверить, доступна ли кнопка Сохранить данные профиля
		/// </summary>
		/// <returns>Кнопка доступна</returns>
		public bool GetIsSaveProfileBtnActive()
		{
			return GetIsElementExist(By.XPath(EDITOR_PROFILE_EDIT_SAVE_XPATH + "[not(contains(@class,'btn-inactive'))]"));
		}

		/// <summary>
		/// Изменяет старый пароль пользователя
		/// </summary>
		/// <param name="oldPass">Старый пароль пользователя</param>
		public void ChangeOldPassword(string oldPass)
		{
			ClearAndAddText(By.XPath(EDITOR_CHANGE_PASSWORD_OLD_XPATH), oldPass);
		}

		/// <summary>
		/// Изменяет новый пароль пользователя
		/// </summary>
		/// <param name="newPass">Новый пароль пользователя</param>
		public void ChangeNewPassword(string newPass)
		{
			ClearAndAddText(By.XPath(EDITOR_CHANGE_PASSWORD_NEW_XPATH), newPass);
		}

		/// <summary>
		/// Проверить, что поле ввода нового пароля отмечено ошибкой
		/// </summary>
		/// <returns>Поле ввода отмечено ошибкой</returns>
		public bool GetIsErrorNewPasswordInputPresent()
		{
			return GetIsElementDisplay(By.XPath(EDITOR_CHANGE_PASSWORD_NEW_XPATH + "[contains(@class,'errorInput')]"));
		}

		/// <summary>
		/// Проверить, что присутствует сообщение об ошибке нового пароля
		/// </summary>
		/// <returns>Ошибка присутствует</returns>
		public bool GetIsErrorNewPasswordMessagePresent()
		{
			return GetIsElementDisplay(By.XPath(NEW_PASSWORD_ERROR_MESSAGE_XPATH));
		}

		/// <summary>
		/// Изменяет повтор нового пароля пользователя
		/// </summary>
		/// <param name="reNewPass">Повтор нового пароля пользователя</param>
		public void ChangeReNewPassword(string reNewPass)
		{
			ClearAndAddText(By.XPath(EDITOR_CHANGE_PASSWORD_RENEW_XPATH), reNewPass);
		}

		/// <summary>
		/// Проверить, что поле ввода подтверждения нового пароля отмечено ошибкой
		/// </summary>
		/// <returns>Поле ввода отмечено ошибкой</returns>
		public bool GetIsErrorRenewPasswordInputPresent()
		{
			return GetIsElementDisplay(By.XPath(EDITOR_CHANGE_PASSWORD_RENEW_XPATH + "[contains(@class,'errorInput')]"));
		}

		/// <summary>
		/// Проверить, что присутствует сообщение об ошибке подтверждения нового пароля
		/// </summary>
		/// <returns>Ошибка присутствует</returns>
		public bool GetIsErrorRenewPasswordMessagePresent()
		{
			return GetIsElementDisplay(By.XPath(RENEW_PASSWORD_ERROR_MESSAGE_XPATH));
		}
	
		/// <summary>
		/// Проверить, доступна ли кнопка Сохранить пароль
		/// </summary>
		/// <returns>Кнопка доступна</returns>
		public bool GetIsSavePasswordBtnActive()
		{
			return GetIsElementExist(By.XPath(EDITOR_CHANGE_PASSWORD_SAVE_XPATH + "[not(contains(@class,'btn-inactive'))]"));
		}

		/// <summary>
		/// Кликнуть кнопку сохранить пароль
		/// </summary>
		public void ClickSavePasswordBtn()
		{
			ClickElement(By.XPath(EDITOR_CHANGE_PASSWORD_SAVE_XPATH));
		}



		protected const string LOGIN_XPATH = ".//a[contains(@data-popup,'login-form')]";
		protected const string LOGIN_HEADER_XPATH = ".//button[contains(@data-popup,'login-form')]";

		protected const string LOGIN_FORM_LOGIN_XPATH = ".//form[@id='login-form-login']";
		protected const string LOGIN_FORM_REGISTRATION_XPATH = ".//form[@id='login-form-register']";
		protected const string EDITOR_FORM_XPATH = ".//div[@id='editor-form']";

		protected const string EDITOR_CLOSE_XPATH = EDITOR_FORM_XPATH + "//div[@class='cancel']";

		protected const string EDITOR_CHANGE_PASSWORD_XPATH = EDITOR_FORM_XPATH + "//label[contains(@data-bind,'changePassword')]";
		
		protected const string EDITOR_CHANGE_PASSWORD_FORM_XPATH = EDITOR_FORM_XPATH + "//form[@id='change-password-form']";
		protected const string EDITOR_CHANGE_PASSWORD_OLD_XPATH = EDITOR_CHANGE_PASSWORD_FORM_XPATH + "//input[@name='password']";
		protected const string EDITOR_CHANGE_PASSWORD_NEW_XPATH = EDITOR_CHANGE_PASSWORD_FORM_XPATH + "//input[@name='newpassword']";
		protected const string EDITOR_CHANGE_PASSWORD_RENEW_XPATH = EDITOR_CHANGE_PASSWORD_FORM_XPATH + "//input[@name='confirmpassword']";
		protected const string EDITOR_CHANGE_PASSWORD_SAVE_XPATH = EDITOR_CHANGE_PASSWORD_FORM_XPATH + "//input[contains(@data-bind,'submitClick')]";
	
		protected const string EDITOR_PROFILE_EDIT_FORM_XPATH = EDITOR_FORM_XPATH + "//form[@id='edit-profile-form']";
		protected const string EDITOR_PROFILE_EDIT_NAME_XPATH = EDITOR_PROFILE_EDIT_FORM_XPATH + "//input[@name='name']";
		protected const string EDITOR_PROFILE_EDIT_SURNAME_XPATH = EDITOR_PROFILE_EDIT_FORM_XPATH + "//input[@name='surname']";
		protected const string EDITOR_PROFILE_EDIT_ABOUT_XPATH = EDITOR_PROFILE_EDIT_FORM_XPATH + "//textarea[@name='aboutme']";
		protected const string EDITOR_PROFILE_EDIT_AVATAR_XPATH = EDITOR_PROFILE_EDIT_FORM_XPATH + "//img[contains(@data-bind,'attr')]";
		protected const string EDITOR_PROFILE_EDIT_UPLOAD_XPATH = EDITOR_PROFILE_EDIT_FORM_XPATH + "//div[contains(@class,'js-upload-btn')]";
		protected const string EDITOR_PROFILE_EDIT_DELETE_XPATH = EDITOR_PROFILE_EDIT_FORM_XPATH + "//div[contains(@class,'js-clear-btn')]";
		protected const string EDITOR_PROFILE_EDIT_SAVE_XPATH = EDITOR_PROFILE_EDIT_FORM_XPATH + "//input[contains(@data-bind,'saveClick')]";

		protected const string EMAIL_CSS = "input[name=\"email\"]";
		protected const string PASSWORD_CSS = "input[name=\"password\"]";
		protected const string SUBMIT_CSS = "input[type =\"submit\"]";

		protected const string LOGIN_ERROR_XPATH = LOGIN_FORM_LOGIN_XPATH + "//div[contains(@class,'js-dynamic-errors')]";
		protected const string LOGIN_NAME_ERROR_MESSAGE_XPATH = EDITOR_PROFILE_EDIT_FORM_XPATH + "//div[contains(@class,'errorMsg2')][contains(@data-bind,'name')]";
		protected const string LOGIN_SURNAME_ERROR_MESSAGE_XPATH = EDITOR_PROFILE_EDIT_FORM_XPATH + "//div[contains(@class,'errorMsg2')][contains(@data-bind,'surname')]";

		protected const string PASSWORD_ERROR_XPATH = EDITOR_CHANGE_PASSWORD_FORM_XPATH + "//div[contains(@class,'js-dynamic-errors')]";
		protected const string NEW_PASSWORD_ERROR_MESSAGE_XPATH = EDITOR_CHANGE_PASSWORD_FORM_XPATH + "//div[contains(@class,'errorMsg2')][contains(@data-bind,'newpassword')]";
		protected const string RENEW_PASSWORD_ERROR_MESSAGE_XPATH = EDITOR_CHANGE_PASSWORD_FORM_XPATH + "//div[contains(@class,'errorMsg2')][contains(@data-bind,'confirmpassword')]";

		protected const string LOGOUT_BTN_XPATH = ".//button[contains(@data-bind,'logout')]";

		protected const string EDIT_PROFILE_LINK_XPATH = ".//button[contains(@data-popup,'editor-form')]";

		protected const string AVATAR_HEADER_XPATH = ".//div[@id='main-menu']//span[contains(@class,'menu-user-link')]/img";

		protected const string COURSES_BTN_XPATH = ".//a[contains(@href,'/Courses')]";
		protected const string COURSES_PAGE_CLASSNAME = "projects-list";

		protected const string LEADERBOARD_BTN_XPATH = ".//a[contains(@href,'/Leaderboard')]";
		protected const string LEADERBOARD_PAGE_CLASSNAME = "leaders";

		protected const string HOME_BTN_CLASSNAME = "on-homepage";
		protected const string HOME_PAGE_XPATH = ".//div[contains(@class,'logo')]";

		protected const string PROFILE_BTN_CLASSNAME = "user-name";
	}
}