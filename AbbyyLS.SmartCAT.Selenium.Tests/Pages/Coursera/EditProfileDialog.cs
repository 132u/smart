﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class EditProfileDialog: HeaderMenu, IAbstractPage<EditProfileDialog>
	{
		public WebDriver Driver { get; protected set; }

		public EditProfileDialog(WebDriver driver):base(driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public EditProfileDialog GetPage()
		{
			var editProfileDialog = new EditProfileDialog(Driver);
			InitPage(editProfileDialog, Driver);

			return editProfileDialog;
		}

		public void LoadPage()
		{
			if (!IsEditProfileDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог редактирования профиля.");
			}
		}
	
		#region Простые методы страницы

		/// <summary>
		/// Ввести имя
		/// </summary>
		/// <param name="name">имя</param>
		public EditProfileDialog FillName(string name)
		{
			CustomTestContext.WriteLine("Ввести имя {0}.", name);
			Name.SetText(name);

			return GetPage();
		}

		/// <summary>
		/// Ввести фамилию
		/// </summary>
		/// <param name="surname">фамилия</param>
		public EditProfileDialog FillSurname(string surname)
		{
			CustomTestContext.WriteLine("Ввести фамилию {0}.", surname);
			Surname.SetText(surname);

			return GetPage();
		}

		/// <summary>
		/// Ввести информацию о пользователе
		/// </summary>
		/// <param name="aboutMeInformation">информация о пользователе</param>
		public EditProfileDialog FillAboutMeInformation(string aboutMeInformation)
		{
			CustomTestContext.WriteLine("Ввести информацию о пользователе {0}.", aboutMeInformation);
			AboutMeTextarea.SetText(aboutMeInformation);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения.
		/// </summary>
		public EditProfileDialog ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения.");
			SaveButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на вкладку редактирования пароля
		/// </summary>
		public EditProfileDialog ClickChangePasswordTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку редактирования пароля.");
			ChangePasswordTab.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку закрытия диалога редактирования профиля.
		/// </summary>
		public UserProfilePage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку закрытия диалога редактирования профиля.");
			CancelButton.Click();

			return new UserProfilePage(Driver).GetPage();
		}

		/// <summary>
		/// Ввести новый пароль
		/// </summary>
		/// <param name="surname">пароль</param>
		public EditProfileDialog FillNewPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести новый пароль {0}.", password);
			NewPassword.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Ввести старый пароль
		/// </summary>
		/// <param name="surname">пароль</param>
		public EditProfileDialog FillOldPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести старый пароль {0}.", password);
			OldPassword.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Ввести подтверждение пароля
		/// </summary>
		/// <param name="surname">пароль</param>
		public EditProfileDialog FillConfirmPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести подтверждение пароля {0}.", password);
			ConfirmPassword.SetText(password);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения пароля.
		/// </summary>
		public EditProfileDialog ClickPasswordSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения пароля.");
			SavePasswordButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Загрузить фото пользователя.
		/// </summary>
		/// <param name="pathFile">пусть к файлу</param>
		public EditProfileDialog UploadUserPhoto(string pathFile)
		{
			CustomTestContext.WriteLine("Загрузить фото пользователя.");
			Driver.ExecuteScript(
						"$(\"input:file\").removeClass(\"g-hidden\").css(\"opacity\", 100).css(\"width\", 500)");
			
			UploadPhotoInput.SendKeys(pathFile);

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Поменять пароль
		/// </summary>
		/// <param name="newPassword">новый парль</param>
		/// <param name="oldPassword">старый пароль</param>
		public UserProfilePage ChangePassword(string newPassword, string oldPassword)
		{
			CustomTestContext.WriteLine("Поменять пароль.");
			FillOldPassword(oldPassword);
			FillNewPassword(newPassword);
			FillConfirmPassword(newPassword);
			ClickPasswordSaveButton();

			return new UserProfilePage(Driver).GetPage();
		}

		/// <summary>
		/// Поменять пароль, ожидая ошибку.
		/// </summary>
		/// <param name="newPassword">новый парль</param>
		/// <param name="oldPassword">старый пароль</param>
		public EditProfileDialog ChangePasswordExpectingError(string newPassword, string oldPassword, string confirmPassword = null)
		{
			CustomTestContext.WriteLine("Поменять пароль, ожидая ошибку.");
			FillNewPassword(newPassword);
			FillOldPassword(oldPassword);
			FillConfirmPassword(confirmPassword ?? newPassword);

			return new EditProfileDialog(Driver).GetPage();
		}

		/// <summary>
		/// Редактировать профиль пользователя.
		/// </summary>
		/// <param name="name">имя</param>
		/// <param name="surname">фамилия</param>
		/// <param name="pathUserPhoto">путь к файлу</param>
		public UserProfilePage EditProfile(string name = null, string surname = null, string aboutMe = null, string pathUserPhoto = null)
		{
			CustomTestContext.WriteLine("Редактировать профиль пользователя.");

			if (name != null)
			{
				FillName(name);
			}

			if (surname != null)
			{
				FillSurname(surname);
			}

			if (aboutMe != null)
			{
				FillAboutMeInformation(aboutMe);
			}

			if (pathUserPhoto != null)
			{
				UploadUserPhoto(pathUserPhoto);
			}

			ClickSaveButton();

			return new UserProfilePage(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что диалог реадктирования профиля открылся.
		/// </summary>
		public bool IsEditProfileDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, что диалог реадктирования профиля открылся.");

			return Driver.WaitUntilElementIsAppear(By.XPath(CHANGE_PASSWORD_TAB));

		}

		/// <summary>
		/// Проверить, что фото пользователя отображается в диалоге редактирования профиля.
		/// </summary>
		public bool IsUserPhotoDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что фото пользователя отображается в диалоге редактирования профиля.");

			return UserPhoto.GetAttribute("src").Contains("/avatar/");

		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Password is too short'.
		/// </summary>
		public bool IsShortPasswordErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Password is too short'.");

			return ShortPasswordError.Displayed;
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Invalid password'.
		/// </summary>
		public bool IsInvalidPasswordErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Invalid password'.");
			Driver.WaitUntilElementIsDisplay(By.XPath(INVALID_PASSWORD_ERROR));

			return InvalidPasswordError.Displayed;
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'Passwords do not match'.
		/// </summary>
		public bool IsPasswordMismatchErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'Passwords do not match'.");

			return PasswordMismatchError.Displayed;
		}

		/// <summary>
		/// Проверить, что появилась ошибка 'Please fill in the \"First name\" field'.
		/// </summary>
		public bool IsNameErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилась ошибка 'Please fill in the \"First name\" field'.");

			return NameError.Displayed;
		}

		/// <summary>
		/// Проверить, что появилась красная рамка у поля имени.
		/// </summary>
		public bool IsNameRedBorderDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилась красная рамка у поля имени.");

			return NameRedBorder.Displayed;
		}

		/// <summary>
		/// Проверить, что появилась ошибка 'Please fill in the \"Last name\" field'.
		/// </summary>
		public bool IsSurnameErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилась ошибка 'Please fill in the \"Last name\" field'.");

			return SurnameError.Displayed;
		}

		/// <summary>
		/// Проверить, что появилась красная рамка у поля фамилии.
		/// </summary>
		public bool IsSurnameRedBorderDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилась красная рамка у поля фамилии.");

			return SurnameRedBorder.Displayed;
		}

		/// <summary>
		/// Проверить, что кнопка сохранения неактивна.
		/// </summary>
		public bool IsSaveButtonInactive()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка сохранения неактивна.");

			return InactiveSaveButton.Displayed;
		}

		/// <summary>
		/// Проверить, что кнопка сохранения пароля неактивна.
		/// </summary>
		public bool IsPasswordSaveButtonInactive()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка сохранения пароля неактивна.");

			return InactivePasswordSaveButton.Displayed;
		}

		#endregion
		
		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = NAME)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = SURNAME)]
		protected IWebElement Surname { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = UPLOAD_PHOTO)]
		protected IWebElement UploadPhotoInput { get; set; }

		[FindsBy(How = How.XPath, Using = USER_PHOTO)]
		protected IWebElement UserPhoto { get; set; }

		[FindsBy(How = How.XPath, Using = NAME_ERROR)]
		protected IWebElement NameError { get; set; }

		[FindsBy(How = How.XPath, Using = NAME_RED_BORDER)]
		protected IWebElement NameRedBorder { get; set; }

		[FindsBy(How = How.XPath, Using = SURNAME_ERROR)]
		protected IWebElement SurnameError { get; set; }

		[FindsBy(How = How.XPath, Using = SURNAME_RED_BORDER)]
		protected IWebElement SurnameRedBorder { get; set; }

		[FindsBy(How = How.XPath, Using = INACTIVE_SAVE_BUTTON)]
		protected IWebElement InactiveSaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = INACTIVE_PASSWORD_SAVE_BUTTON)]
		protected IWebElement InactivePasswordSaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = ABOUT_ME_TEXTAREA)]
		protected IWebElement AboutMeTextarea { get; set; }

		[FindsBy(How = How.XPath, Using = OLD_PASSWORD)]
		protected IWebElement OldPassword { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_PASSWORD)]
		protected IWebElement NewPassword { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_PASSWORD)]
		protected IWebElement ConfirmPassword { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_PASSWORD_BUTTON)]
		protected IWebElement SavePasswordButton { get; set; }

		[FindsBy(How = How.XPath, Using = CHANGE_PASSWORD_TAB)]
		protected IWebElement ChangePasswordTab { get; set; }

		[FindsBy(How = How.XPath, Using = SHORT_PASSWORD_ERROR)]
		protected IWebElement ShortPasswordError { get; set; }

		[FindsBy(How = How.XPath, Using = INVALID_PASSWORD_ERROR)]
		protected IWebElement InvalidPasswordError { get; set; }

		[FindsBy(How = How.XPath, Using = PASSWORD_MISMATCH_ERROR)]
		protected IWebElement PasswordMismatchError { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string CHANGE_PASSWORD_TAB = "//label[contains(@data-bind, 'changePassword().hasPassword()')]";
		protected const string NAME = "//form[@id='edit-profile-form']//input[@name='name']";
		protected const string SURNAME = ".//div[@id='editor-form']//form[@id='edit-profile-form']//input[@name='surname']";
		protected const string SAVE_BUTTON = ".//div[@id='editor-form']//form[@id='edit-profile-form']//input[contains(@data-bind,'saveClick')]";
		protected const string UPLOAD_PHOTO = "//input[@type='file']";
		protected const string USER_PHOTO = "//form[@id='edit-profile-form']//img[contains(@data-bind,'attr')]";
		protected const string NAME_RED_BORDER = "//form[@id='edit-profile-form']//input[@name='name'][contains(@class,'errorInput')]";
		protected const string NAME_ERROR = "//form[@id='edit-profile-form']//div[contains(@class,'errorMsg2')][contains(@data-bind,'name')]";
		protected const string INACTIVE_SAVE_BUTTON = ".//div[@id='editor-form']//form[@id='edit-profile-form']//input[contains(@data-bind,'saveClick')][contains(@class,'btn-inactive')]";
		protected const string SURNAME_ERROR = "//form[@id='edit-profile-form']//div[contains(@class,'errorMsg2')][contains(@data-bind,'validationMessage: surname')]";
		protected const string SURNAME_RED_BORDER = "//form[@id='edit-profile-form']//input[@name='surname'][contains(@class,'errorInput')]";
		protected const string ABOUT_ME_TEXTAREA = "//form[@id='edit-profile-form']//textarea[@name='aboutme']";

		protected const string OLD_PASSWORD = ".//div[@id='editor-form']//form[@id='change-password-form']//input[@name='password']";
		protected const string NEW_PASSWORD = ".//div[@id='editor-form']//form[@id='change-password-form']//input[@name='newpassword']";
		protected const string CONFIRM_PASSWORD = ".//div[@id='editor-form']//form[@id='change-password-form']//input[@name='confirmpassword']";
		protected const string SAVE_PASSWORD_BUTTON = ".//div[@id='editor-form']//form[@id='change-password-form']//input[contains(@data-bind,'submitClick')]";
		protected const string INACTIVE_PASSWORD_SAVE_BUTTON = ".//div[@id='editor-form']//form[@id='change-password-form']//input[contains(@data-bind,'submitClick')][contains(@class,'btn-inactive')]";
		protected const string SHORT_PASSWORD_ERROR = ".//div[@id='editor-form']//form[@id='change-password-form']//div[contains(@class,'errorMsg2')][contains(@data-bind,'newpassword')]";
		protected const string INVALID_PASSWORD_ERROR = ".//div[@id='editor-form']//form[@id='change-password-form']//div[contains(@class,'js-dynamic-errors')]//p[@id='error1']";
		protected const string CANCEL_BUTTON = "//div[@id='editor-form']//div[@class='cancel']";
		protected const string PASSWORD_MISMATCH_ERROR = ".//div[@id='editor-form']//form[@id='change-password-form']//div[contains(@class,'errorMsg2')][contains(@data-bind,'confirmpassword')]";

		#endregion
	}
}