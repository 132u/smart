using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using OpenQA.Selenium.Interactions;

namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	class UserProfile : BaseTest
	{
		public UserProfile(string url, string workspaceUrl, string browserName)
			: base(url, workspaceUrl, browserName)
		{

		}

		[SetUp]
		public void Setup()
		{
			// Проверка входа в пользователя - есть ли ошибка неправильного пароля
			if (IsExistIncorrectPasswordError())
			{
				// В тестах проверяется изменение пароля, в конце теста пароль восстанавливается
				// Но при прохождении теста может возникнуть ошибка и он может не дойти до восстановления пароля
				// Тогда при следующем входе в пользователя появится ошибка, что пароль неправильный
				// Поэтому проверяем пароли, на которые в тестах мы меняли изначальный пароль
				// И восстаналиваем нужный пароль
				TryRecoverPassword();
			}
		}

		/// <summary>
		/// Тест: изменение имени и фамилии пользователя, проверка изменения на главной странице
		/// </summary>
		[Test]
		public void ChangeUserNameCheckHomepage()
		{
			// Открыть профиль пользователя
			OpenUserProfileFromHomePage();
			// Открыть форму редактирования
			OpenEditProfileForm();

			string newUserName = UserName + DateTime.Now.Ticks;
			string newUserSurname = UserSurname + DateTime.Now.Ticks;
			// Изменить имя и фамилию
			SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);

			// Вернуться на главную страницу
			Header.OpenHomepage();

			// Получить полное имя пользователя после изменения
			string resultUserName = HomePage.GetUserName();

			// Восстановить имя пользователя
			RecoverUserNameFromHomePage();

			// Проверить изменение имени - ASSERT внутри
			AssertUserName(resultUserName, newUserName, newUserSurname);
		}

		/// <summary>
		/// Тест: изменение имени и фамилии пользователя, проверка изменения на странице профиля пользователя
		/// </summary>
		[Test]
		public void ChangeUserNameCheckProfile()
		{
			// Открыть профиль
			OpenUserProfileFromHomePage();
			// Открыть форму редактирования
			OpenEditProfileForm();

			string newUserName = UserName + DateTime.Now.Ticks;
			string newUserSurname = UserSurname + DateTime.Now.Ticks;
			// Изменить имя и фамилию
			SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);

			// Получить полное имя после изменения
			string resultUserName = ProfilePage.GetFullUserName();

			// Восстановить имя
			RecoverUserNameFromProfile();

			// Проверить изменение имени - ASSERT внутри
			AssertUserName(resultUserName, newUserName, newUserSurname);
		}

		/// <summary>
		/// Тест: изменение имени и фамилии пользователя, проверка изменения в списке лидеров
		/// </summary>
		[Test]
		public void ChangeUserNameCheckLeaderboard()
		{
			// Открыть профиль
			OpenUserProfileFromHomePage();

			// Открыть форму
			OpenEditProfileForm();

			string newUserName = UserName + DateTime.Now.Ticks;
			string newUserSurname = UserSurname + DateTime.Now.Ticks;

			// Изменить имя и фамилию
			SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);

			// Перейти к списку лидеров
			Header.OpenLeaderboardPage();

			// Получить полное имя после изменения
			string resultUserName = GetFullNameLeaderboard();

			// Восстановить имя
			RecoverUserNameFromCourse();

			// Проверить изменение имени - ASSERT внутри
			AssertUserName(resultUserName, newUserName, newUserSurname);
		}

		/// <summary>
		/// Тест: изменение имени и фамилии пользователя, проверка изменения в главном меню
		/// </summary>
		[Test]
		public void ChangeUserNameCheckMainMenu()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму
			OpenEditProfileForm();
			string newUserName = UserName + DateTime.Now.Ticks;
			string newUserSurname = UserSurname + DateTime.Now.Ticks;
			// Изменить имя и фамилию
			SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);

			// Перейти на страницу курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Получить полное имя после изменения
			string resultUserName = Header.GetName();

			// Восстановить имя
			RecoverUserNameFromCourse();

			// Проверить изменение имени - ASSERT внутри
			AssertUserName(resultUserName, newUserName, newUserSurname);
		}

		/// <summary>
		/// Тест: изменение имени и фамилии пользователя, проверка изменения имени в уже добавленном переводе
		/// </summary>
		[Test]
		public void ChangeUserNameCheckTranslationBeforeEdit()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			// Выйти из редактора
			ClickHomeEditor();

			// Перейти в профиль
			OpenUserProfileFromCourse();
			// Открыть форму
			OpenEditProfileForm();
			string newUserName = UserName + DateTime.Now.Ticks;
			string newUserSurname = UserSurname + DateTime.Now.Ticks;
			// Изменить имя и фамилию
			SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);

			// Вернуться в лекцию
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			OpenCourseByName(courseName);
			OpenLectureByRowNum(lectureRowNumber);
			EditorPage.ClickTargetByRowNumber(translationRowNum);
			// Найти добавленный перевод
			int curTranslationNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);
			// Получить имя в списке добавленных переводов
			string resultUserName = EditorPage.GetTranslationAuthorByRowNumber(curTranslationNumber);

			// Выйти из редактора
			ClickHomeEditor();
			// Восстановить имя
			RecoverUserNameFromCourse();

			// Проверить изменение имени - ASSERT внутри
			AssertUserName(resultUserName, newUserName, newUserSurname);
		}

		/// <summary>
		/// Тест: изменение имени и фамилии пользователя, проверка изменения имени при добавлении перевода после изменения имени
		/// </summary>
		[Test]
		public void ChangeUserNameCheckTranslationAfterEdit()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму
			OpenEditProfileForm();
			string newUserName = UserName + DateTime.Now.Ticks;
			string newUserSurname = UserSurname + DateTime.Now.Ticks;
			// Изменить имя и фамилию
			SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);

			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);

			// Найти добавленный перевод
			int curTranslationNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);
			// Получить имя в списке добавленных переводов
			string resultUserName = EditorPage.GetTranslationAuthorByRowNumber(curTranslationNumber);

			// Выйти из редактора
			ClickHomeEditor();
			// Восстановить имя
			RecoverUserNameFromCourse();

			// Проверить изменение имени - ASSERT внутри
			AssertUserName(resultUserName, newUserName, newUserSurname);
		}

		/// <summary>
		/// Тест: изменение имени и фамилии пользователя, проверка изменения имени в списке последних событий (событие было до изменения имени)
		/// </summary>
		[Test]
		public void ChangeUserNameCheckEventListBeforeEdit()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			// Выйти из редактора
			ClickHomeEditor();
			// Перейти на главную страницу
			Header.OpenHomepage();
			// Ожидание появления события в списке
			WaitEventInEventListByTarget(translationText);
			int rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
			// Имя пользователя в списке событий до изменения имени
			string userNameBefore = HomePage.GetEventAuthorByRowNumber(rowNumber);

			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму
			OpenEditProfileForm();
			string newUserName = UserName + DateTime.Now.Ticks;
			string newUserSurname = UserSurname + DateTime.Now.Ticks;
			// Изменить имя и фамилию
			SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);

			// Перейти на главную страницу
			Header.OpenHomepage();
			// Ожидание изменения имени в списке событий
			bool isAuthorChanged = WaitEventListChangingAuthor(translationText, HomePageLastEventType.AddTranslationEvent, userNameBefore);
			rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
			// Получить имя в списке событий
			string resultUserName = HomePage.GetEventAuthorByRowNumber(rowNumber).Replace("...", "");
			// Восстановить имя
			RecoverUserNameFromHomePage();

			// Проверить изменение имени
			Assert.IsTrue(isAuthorChanged, "Ошибка: не изменилось имя");
			Assert.IsTrue((newUserName + " " + newUserSurname).Contains(resultUserName),
				"Ошибка: неправильное имя\nДолжно было быть: " + (newUserName + " " + newUserSurname) + "\nА сейчас: " + resultUserName);
		}

		/// <summary>
		/// Тест: изменение имени и фамилии пользователя, проверка изменения имени в списке последних событий (событие создали после изменения имени)
		/// </summary>
		[Test]
		public void ChangeUserNameCheckEventListAfterEdit()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму
			OpenEditProfileForm();
			string newUserName = UserName + DateTime.Now.Ticks;
			string newUserSurname = UserSurname + DateTime.Now.Ticks;
			// Изменить имя и фамилию
			SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);

			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			// Выйти из редактора
			ClickHomeEditor();
			// Перейти на главную страницу
			Header.OpenHomepage();
			// Ожидание появления события в списке
			WaitEventInEventListByTarget(translationText);
			int rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
			// Имя пользователя в списке событий до изменения имени
			string resultUserName = HomePage.GetEventAuthorByRowNumber(rowNumber).Replace("...", "");
			// Восстановить имя
			RecoverUserNameFromHomePage();

			// Проверить изменение имени
			Assert.IsTrue((newUserName + " " + newUserSurname).Contains(resultUserName),
				"Ошибка: неправильное имя\nДолжно было быть: " + (newUserName + " " + newUserSurname) + "\nА сейчас: " + resultUserName);
		}

		/// <summary>
		/// Добавление аватора пользователя, проверка в главном меню
		/// </summary>
		[Test]
		public void AddAvatarCheckMainMenu()
		{
			// Добавить аватар пользователю
			AddAvatarUser();

			// Перейти в список курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");

			// Есть ли аватар
			bool isAvatarExist = Header.GetIsAvatarPresentHeader();
			// Восстановить аватар
			RecoverAvatarFromCourse();
			// Проверить аватар
			Assert.IsTrue(isAvatarExist, "Ошибка: аватар не появился");
		}

		/// <summary>
		/// Добавление аватора пользователя, проверка в профиле
		/// </summary>
		[Test]
		public void AddAvatarCheckProfile()
		{
			// Добавить аватар пользователю
			AddAvatarUser();

			// Есть ли аватар
			bool isAvatarExist = ProfilePage.GetIsAvatarPresentProfile();
			// Восстановить аватар
			RecoverAvatarFromProfile();
			// Проверить аватар
			Assert.IsTrue(isAvatarExist, "Ошибка: аватар не появился");
		}

		/// <summary>
		/// Добавление аватора пользователя, проверка в списке лидеров
		/// </summary>
		[Test]
		public void AddAvatarCheckLeaderboard()
		{
			// Добавить аватар пользователю
			AddAvatarUser();

			// Перейти к списку лидеров
			Header.OpenLeaderboardPage();
			// Есть ли аватар
			bool isAvatarExist = LeaderboardPage.GetIsAvatarPresentLeaderboard();
			// Восстановить аватар
			RecoverAvatarFromCourse();

			// Проверить аватар
			Assert.IsTrue(isAvatarExist, "Ошибка: аватар не появился");
		}

		/// <summary>
		/// Добавление аватора пользователя, проверка в форме изменения профиля
		/// </summary>
		[Test]
		public void AddAvatarCheckEditProfileForm()
		{
			// Добавить аватар пользователю
			AddAvatarUser();
			ProfilePage.WaitUntilDisplayProfile();
			// Перейти к списку лидеров
			OpenEditProfileForm();
			// Есть ли аватар
			bool isAvatarExist = Header.GetIsAvatarPresentEditorProfile();
			// Закрыть редактирование профиля
			CloseEditProfileForm();
			// Восстановить аватар
			RecoverAvatarFromCourse();

			// Проверить аватар
			Assert.IsTrue(isAvatarExist, "Ошибка: аватар не появился");
		}

		/// <summary>
		/// Изменить имя пользователя на пустое
		/// </summary>
		[Test]
		public void ChangeNameEmpty()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму
			OpenEditProfileForm();
			// Изменить имя на пустое
			Header.ChangeNameProfile("");

			Assert.IsTrue(Header.GetIsErrorNameInputPresent(),
				"Ошибка: поле Имя должно быть отмечено ошибкой");
			Assert.IsTrue(Header.GetIsErrorNameMessagePresent(),
				"Ошибка: должна появиться ошибка о неправильном имени");
			Assert.IsFalse(Header.GetIsSaveProfileBtnActive(),
				"Ошибка: кнопка сохранения должна быть неактивной");
		}

		/// <summary>
		/// Изменить имя пользователя на пробельное
		/// </summary>
		[Test]
		public void ChangeNameSpace()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму
			OpenEditProfileForm();
			// Изменить имя на пустое
			Header.ChangeNameProfile(" ");

			Assert.IsTrue(Header.GetIsErrorNameInputPresent(),
				"Ошибка: поле Имя должно быть отмечено ошибкой");
			Assert.IsTrue(Header.GetIsErrorNameMessagePresent(),
				"Ошибка: должна появиться ошибка о неправильном имени");
			Assert.IsFalse(Header.GetIsSaveProfileBtnActive(),
				"Ошибка: кнопка сохранения должна быть неактивной");
		}

		/// <summary>
		/// Изменить фамилию пользователя на пустую
		/// </summary>
		[Test]
		public void ChangeSurnameEmpty()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму
			OpenEditProfileForm();
			// Изменить имя на пустое
			Header.ChangeSurnameProfile("");

			Assert.IsTrue(Header.GetIsErrorSurnameInputPresent(),
				"Ошибка: поле Фамилия должно быть отмечено ошибкой");
			Assert.IsTrue(Header.GetIsErrorSurnameMessagePresent(),
				"Ошибка: должна появиться ошибка о неправильной фамилии");
			Assert.IsFalse(Header.GetIsSaveProfileBtnActive(),
				"Ошибка: кнопка сохранения должна быть неактивной");
		}

		/// <summary>
		/// Изменить фамилию пользователя на пробельную
		/// </summary>
		[Test]
		public void ChangeSurnameSpace()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму
			OpenEditProfileForm();
			// Изменить имя на пустое
			Header.ChangeSurnameProfile(" ");

			Assert.IsTrue(Header.GetIsErrorSurnameInputPresent(),
				"Ошибка: поле Фамилия должно быть отмечено ошибкой");
			Assert.IsTrue(Header.GetIsErrorSurnameMessagePresent(),
				"Ошибка: должна появиться ошибка о неправильной фамилии");
			Assert.IsFalse(Header.GetIsSaveProfileBtnActive(),
				"Ошибка: кнопка сохранения должна быть неактивной");
		}

		/// <summary>
		/// Тест: добавление информации о себе (проверка на странице профиля)
		/// </summary>
		[Test]
		public void AddInfoAboutMe()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму
			OpenEditProfileForm();
			// Добавить информацию о себе
			string aboutInfo = "About Me Info " + DateTime.Now.Ticks;
			ChangeAboutMeInfo(aboutInfo);
			// Получить текст информации о себе
			string resultAboutInfo = ProfilePage.GetInfo();
			// Проверить, что сохранилась правильная информация
			Assert.AreEqual(resultAboutInfo, aboutInfo, "Ошибка: текст о себе не сохранился");
		}

		/// <summary>
		/// Тест: изменение пароля на новый
		/// </summary>
		[Test]
		public void ChangePasswordNew()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму изменения пароля
			OpenEditPasswordForm();
			string newUserPassword = User1NewPass;

			// Изменить пароль
			ChangePasswordAndSave(User1.password, newUserPassword);
			// Выйти из пользователя
			Header.LogoutUser();

			// Войти в пользователя с новым паролем
			Header.Login(User1.login, newUserPassword);
			//LoginUser(User1);

			// Проверить, что удается войти в пользователя
			Assert.IsFalse(IsExistIncorrectPasswordError(), "Ошибка: не удается войти в пользователя с новым паролем (" + newUserPassword + ")");

			// Открыть профиль
			OpenUserProfileFromHomePage();
			// Восстановить пароль
			RecoverUserPassword(newUserPassword);
		}

		/// <summary>
		/// Тест: изменение пароля на пограничный
		/// </summary>
		[Test]
		public void ChangePasswordLimit()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму изменения пароля
			OpenEditPasswordForm();
			string newUserPassword = User1LimitPass;

			// Изменить пароль
			ChangePasswordAndSave(User1.password, newUserPassword);
			// Выйти из пользователя
			Header.LogoutUser();

			// Войти в пользователя с новым паролем
			Header.Login(User1.login, newUserPassword);
			//LoginUser(User1.login, User1NewPass);
			// Проверить, что удается войти в пользователя
			Assert.IsFalse(IsExistIncorrectPasswordError(), "Ошибка: не удается войти в пользователя с новым паролем (" + newUserPassword + ")");

			// Открыть профиль
			OpenUserProfileFromHomePage();
			// Восстановить пароль
			RecoverUserPassword(newUserPassword);
		}

		/// <summary>
		/// Тест: изменение пароля на запрещенный (менее 6 символов)
		/// </summary>
		[Test]
		public void ChangePasswordForbidden()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму изменения пароля
			OpenEditPasswordForm();
			string newUserPassword = User1ForbiddenPass;

			// Изменить пароль - ввести запрещенный новый пароль
			ChangePassword(User1.password, newUserPassword);

			// Проверить, что поле Новый пароль отмечено ошибкой и есть сообщение об ошибке
			Assert.IsTrue(Header.GetIsErrorNewPasswordInputPresent(),
				"Ошибка: поле Новый пароль не отмечено ошибкой");
			Assert.IsTrue(Header.GetIsErrorNewPasswordMessagePresent(),
				"Ошибка: не появилось сообщение о слишком коротком пароле");

			// Проверить, что кнопка Сохранить заблокирована
			Assert.IsFalse(Header.GetIsSavePasswordBtnActive(),
				"Ошибка: кнопка Сохранить не заблокирована");

			// Проверить, что пользователь заходит со старым паролем
			CheckUserLastPasswordValid();
		}

		/// <summary>
		/// Тест: изменение пароля, новый совпадает со старым
		/// </summary>
		[Test]
		public void ChangePasswordNewEqualOld()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму изменения пароля
			OpenEditPasswordForm();
			// Новый пароль совпадает со старым
			string newUserPassword = User1.password;

			// Изменить пароль
			ChangePasswordAndSave(User1.password, newUserPassword);
			// Выйти из пользователя
			Header.LogoutUser();

			// Войти в пользователя с новым паролем
			LoginUser(User1);
			// Проверить, что удается войти в пользователя
			Assert.IsFalse(IsExistIncorrectPasswordError(), "Ошибка: не удается войти в пользователя с новым паролем (" + User1NewPass + ")");
			// Восстаналивать пароль не надо, т.к. пароль изменили на тот же самый
		}

		/// <summary>
		/// Тест: изменение пароля, старый указан неверно
		/// </summary>
		[Test]
		public void ChangePasswordIncorrectOld()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму изменения пароля
			OpenEditPasswordForm();

			// Указать неправильный старый пароль и сохранить
			ChangePasswordAndSave(User1NewPass, User1.password);

			// Проверить, что появилось сообщение о неверном пароле
			Assert.IsTrue(Header.GetIsPasswordErrorPresent(),
				"Ошибка: не появилось сообщение о неверном пароле");

			CheckUserLastPasswordValid();
		}

		/// <summary>
		/// Тест: изменение пароля, новый и подтверждение не совпадают
		/// </summary>
		[Test]
		public void ChangePasswordNotEqualNewConfirm()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			// Открыть форму изменения пароля
			OpenEditPasswordForm();

			// Ввести разные пароли: новый и подтверждение
			ChangePassword(User1.password, User1NewPass, User1LimitPass);

			// Проверить, что поле Новый пароль отмечено ошибкой и есть сообщение об ошибке
			Assert.IsTrue(Header.GetIsErrorRenewPasswordInputPresent(),
				"Ошибка: поле Подтверждение пароля не отмечено ошибкой");
			Assert.IsTrue(Header.GetIsErrorRenewPasswordMessagePresent(),
				"Ошибка: не появилось сообщение о не совпадающих паролях");

			// Проверить, что кнопка Сохранить заблокирована
			Assert.IsFalse(Header.GetIsSavePasswordBtnActive(),
				"Ошибка: кнопка Сохранить не заблокирована");

			CheckUserLastPasswordValid();
		}

		/// <summary>
		/// Проверка открытия страницы Мой профиль с главной страницы
		/// </summary>
		[Test]
		public void OpenProfileHomePage()
		{
			// Открыть профиль с главной страницы
			OpenUserProfileFromHomePage();
		}

		/// <summary>
		/// Проверка открытия страницы Мой профиль в главном меню на странице со списком курсов
		/// </summary>
		[Test]
		public void OpenProfileFromMainMenuCoursePage()
		{
			// Перейти на страницу со списком курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Открыть профиль по ссылке в главном меню на странице со списком курсов
			OpenUserProfileFromCourse();
		}

		/// <summary>
		/// Тест: открыть профиль по нажатию в верхнем меню со страницы другого пользователя
		/// </summary>
		[Test]
		public void OpenProfileFromAnotherUserPage()
		{
			// Перейти на страницу курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Перейти к списку лидеров
			Header.OpenLeaderboardPage();
			Assert.IsTrue(LeaderboardPage.GetIsNamePresentByRowNumber(1), "Ошибка: список лидеров пуст");

			// В списке лидеров найти ссылку на другого пользователя
			string curUserName = GetFullNameLeaderboard();
			// Получить имя первого лидера
			string firstLeaderName = LeaderboardPage.GetNameByRowNumber(1);

			if (firstLeaderName != curUserName)
			{
				Assert.IsTrue(LeaderboardPage.GetIsNamePresentByRowNumber(2), "Ошибка: в списке нет других пользователей");
				LeaderboardPage.ClickNameByRowNumber(2);
			}
			else
			{
				LeaderboardPage.ClickNameByRowNumber(1);
			}

			// Дождаться загрузки страницы
			Assert.IsTrue(ProfilePage.WaitUntilDisplayProfile(), "Ошибка: Страница профиля не открылась.");

			// Открыть профиль текущего пользователя
			OpenUserProfileFromCourse();
			Thread.Sleep(1000);
			// Проверить, что открылся профиль именно текущего пользователя
			Assert.AreEqual(curUserName, ProfilePage.GetFullUserName(),
				"Ошибка: не открылся профиль текущего пользователя");
		}

		/// <summary>
		/// Тест: открытие профиля по ссылке в лидерборде
		/// </summary>
		[Test]
		public void OpenProfileFromLeaderboard()
		{
			// Получить текущее имя пользователя
			string userName = HomePage.GetUserName();

			// Пролистать список лидеров до пользователя
			int numInList = ScrollLeaderboardToUser(userName);

			// Кликнуть по пользователю в списке
			LeaderboardPage.ClickNameByRowNumber(numInList);
			// Дождаться загрузки страницы
			Assert.IsTrue(ProfilePage.WaitUntilDisplayProfile(), "Ошибка: Страница профиля не открылась.");
		}



		/// <summary>
		/// Получить полное имя пользователя на странице списка лидеров
		/// </summary>
		/// <returns>полное имя - "имя фамилия"</returns>
		protected string GetFullNameLeaderboard()
		{
			string userName = "";

			setDriverTimeoutMinimum();
			// Получение имени пользователя, проверка всех списков
			userName = LeaderboardPage.GetUserName();

			setDriverTimeoutDefault();

			return userName;
		}

		/// <summary>
		/// Открыть форму редактирования профиля
		/// </summary>
		protected void OpenEditProfileForm()
		{
			// Открыть форму редактирования
			Header.ClickOpenEditorForm();
			// Формирование ошибки, если форма не открылась
			Assert.IsTrue(Header.WaitUntilEditorFormDisplay(), "Ошибка: Форма редактирования данных пользователя не открылась.");
		}

		/// <summary>
		/// Закрыть форму редактирования профиля
		/// </summary>
		protected void CloseEditProfileForm()
		{
			// Закрыть форму редактирования
			Header.ClickCloseEditorForm();
			Assert.IsTrue(Header.WaitUntilEditorFormDisappear(), "Ошибка: Форма редактирования данных пользователя не закрылась.");
		}

		/// <summary>
		/// Сохранить данные пользователя
		/// </summary>
		protected void SaveProfile()
		{
			// Закрыть форму редактирования
			Header.ClickSaveProfileBtn();
			Assert.IsTrue(Header.WaitUntilEditorFormDisappear(), "Ошибка: Форма редактирования данных пользователя не закрылась.");
		}

		/// <summary>
		/// Сохранить пароль пользователя
		/// </summary>
		protected void SavePassword()
		{
			// Проверить, что кнопка Сохранить не заблокирована
			Assert.IsTrue(Header.GetIsSavePasswordBtnActive(),
				"Ошибка: кнопка Сохранить заблокирована");

			Header.ClickSavePasswordBtn();

			// Дождаться, пока форма закроется
			Header.WaitUntilEditorFormDisappear();
		}

		/// <summary>
		/// Открыть форму изменения пароля
		/// </summary>
		protected void OpenEditPasswordForm()
		{
			// Открыть форму редактирования
			OpenEditProfileForm();
			// Перейти на вкладку изменения пароля
			Header.ClickChangePasswordForm();
			// Дождаться пока откроется форма изменения пароля
			Assert.IsTrue(Header.WaitUntilChangePasswordFormDisplay(), "Ошибка: Форма смены пароля не открылась.");
		}

		/// <summary>
		/// Изменить и сохранить новые имя и фамилию в форме редактирования профиля
		/// </summary>
		/// <param name="newUserName">новое имя</param>
		/// <param name="newUserSurname">ноая фамилия</param>
		protected void SaveNewUserNameInEditProfileForm(string newUserName, string newUserSurname)
		{
			// Ввести новое имя
			Header.ChangeNameProfile(newUserName);
			// Ввести новую фамилию
			Header.ChangeSurnameProfile(newUserSurname);
			// Сохранить
			SaveProfile();
		}

		/// <summary>
		/// Добавить информацию о себе
		/// </summary>
		/// <param name="info"></param>
		protected void ChangeAboutMeInfo(string info)
		{
			// Очистить поле Информация о себе
			Header.ChangeInfoProfile(info);
			// Сохранить
			SaveProfile();
		}

		/// <summary>
		/// Восстановление имени пользователя до выхода из теста (со страницы профиля)
		/// </summary>
		protected void RecoverUserNameFromProfile()
		{
			// Открыть форму редактирования
			OpenEditProfileForm();

			// Изменить имя и фамилию
			SaveNewUserNameInEditProfileForm(UserName, UserSurname);
		}

		/// <summary>
		/// Восстановление имени пользователя до выхода из теста (переход с главной страницы на страницу профиля)
		/// </summary>
		protected void RecoverUserNameFromHomePage()
		{
			// Открыть профиль пользователя
			OpenUserProfileFromHomePage();
			// Восстановить имя
			RecoverUserNameFromProfile();
		}

		/// <summary>
		/// Восстановление имени пользователя до выхода из теста (переход со страницы курса или лидеров)
		/// </summary>
		protected void RecoverUserNameFromCourse()
		{
			// Открыть профиль пользователя
			OpenUserProfileFromCourse();
			// Восстановить имя
			RecoverUserNameFromProfile();
		}

		/// <summary>
		/// Проверка изменения имени - ASSERT-ы
		/// </summary>
		/// <param name="resultUserName">имя пользователя, которое в итоге получилось</param>
		/// <param name="newName">новое имя пользователя</param>
		/// <param name="newSurname">новая фамилия пользователя</param>
		protected void AssertUserName(string resultUserName, string newName, string newSurname)
		{
			Assert.IsTrue(resultUserName == (newName + " " + newSurname),
				"Ошибка: новое имя отображается неправильно:\nДолжно быть: " + (newName + " " + newSurname) + "\nА есть: " + resultUserName);
		}

		/// <summary>
		/// Добавить аватар в форме редактирования пользователя
		/// </summary>
		protected void AddAvatarInProfile()
		{
			// Открыть форму редактирования профиля
			OpenEditProfileForm();

			// Кликнуть на открытие диалога загрузки файла
			Header.ClickAvatarUploadBtn();

			// Заполнить диалог загрузки документа
			FillAddDocumentForm(ImageFile);

			// Сохранить
			SaveProfile();
		}

		/// <summary>
		/// Восстановить аватар со страницы курсов
		/// </summary>
		protected void RecoverAvatarFromCourse()
		{
			// Открыть профиль пользователя
			OpenUserProfileFromCourse();
			// Восстановить имя
			RecoverAvatarFromProfile();
		}

		/// <summary>
		/// Восстановить аватар с главной страницы
		/// </summary>
		protected void RecoverAvatarFromHomePage()
		{
			// Открыть профиль пользователя
			OpenUserProfileFromHomePage();
			// Восстановить имя
			RecoverAvatarFromProfile();
		}

		/// <summary>
		/// Восстановить аватар со страницы профиля
		/// </summary>
		protected void RecoverAvatarFromProfile()
		{
			// Открыть форму редактирования
			OpenEditProfileForm();

			// Удалить аватар
			Header.ClickAvatarDeleteBtn();

			// Сохранить
			SaveProfile();
		}

		/// <summary>
		/// Проверка на странице пользователя - есть ли аватар,
		/// если есть - удаление аватара,
		/// если не удалился - Assert.Fail.
		/// Добавление аватара.
		/// </summary>
		protected void AddAvatarUser()
		{
			// Перейти в профиль
			OpenUserProfileFromHomePage();
			Driver.Manage().Window.Maximize();

			// Проверить, что аватара нет
			if (ProfilePage.GetIsAvatarPresentProfile())
			{
				// Удалить аватар
				RecoverAvatarFromProfile();
				// Проверить, что аватара нет
				Assert.IsFalse(ProfilePage.GetIsAvatarPresentProfile(), "Ошибка: не удаляется аватар перед началом теста");
			}

			// Открыть форму
			OpenEditProfileForm();
			// Добавить аватар
			AddAvatarInProfile();
		}

		/// <summary>
		/// Заполнить форму изменения пароля
		/// </summary>
		/// <param name="oldPassword">старый пароль</param>
		/// <param name="newPassword">новый пароль</param>
		/// <param name="confirmPassword">подтверждение нового пароля, если пустое - вставляется новый пароль</param>
		protected void ChangePassword(string oldPassword, string newPassword, string confirmPassword = "")
		{
			// Ввести старый пароль
			Header.ChangeOldPassword(oldPassword);

			// Вести новый пароль
			Header.ChangeNewPassword(newPassword);

			// Вести подтверждение нового пароля
			if (confirmPassword.Length == 0)
			{
				confirmPassword = newPassword;
			}

			Header.ChangeReNewPassword(confirmPassword);
		}

		/// <summary>
		/// Заполнить форму изменения пароля и сохранить
		/// </summary>
		/// <param name="oldPassword">старый пароль</param>
		/// <param name="newPassword">новый пароль</param>
		/// <param name="confirmPassword">подтверждение нового пароля, если пустое - вставляется новый пароль</param>
		protected void ChangePasswordAndSave(string oldPassword, string newPassword, string confirmPassword = "")
		{
			// Заполнить форму изменения пароля
			ChangePassword(oldPassword, newPassword, confirmPassword);

			// Закрыть форму
			SavePassword();
		}

		/// <summary>
		/// Восставновить пароль первого пользователя
		/// </summary>
		/// <param name="currentPassword">сейчас установленный пароль пользователя</param>
		protected void RecoverUserPassword(string currentPassword)
		{
			// Открыть форму изменения пароля
			OpenEditPasswordForm();
			// Изменить пароль на изначальный
			ChangePasswordAndSave(currentPassword, User1.password);
		}

		/// <summary>
		/// Проверка, есть ли ошибка неверного пароля
		/// </summary>
		/// <returns>есть ошибка</returns>
		private bool IsExistIncorrectPasswordError()
		{
			if (!Header.WaitUntilLoginFormDisappear())
			{
				return Header.GetIsLoginErrorPresent();
			}
			return false;
		}

		/// <summary>
		/// Восстановить пароль
		/// Метод пробует зайти в пользователя с другими паролями
		/// Если не удается - выдает ошибку
		/// Если удается - восстанавливает пароль
		/// </summary>
		protected void TryRecoverPassword()
		{
			string currentPassword = "";
			// Возможные варианты сохранившегося пароля
			List<string> passwordList = new List<string>();
			passwordList.Add(User1NewPass);
			passwordList.Add(User1LimitPass);
			passwordList.Add(User1ForbiddenPass);

			// Попробовать разные пароли
			foreach (string pass in passwordList)
			{
				// Заполнить форму с другим паролем
				Header.FillAuthorizationData(User1.login, pass);
				// Проверить, прошел ли пароль
				if (!IsExistIncorrectPasswordError())
				{
					currentPassword = pass;
					break;
				}
			}

			// Если пароль так и не подобран
			if (currentPassword.Length == 0)
			{
				Assert.Fail("Ошибка: не удается войти в пользователя. Возможно, надо сменить/восстановить пароль вручную\nЛогин: " +
					User1.login + "\nПароли: " + User1.password + "; " + User1NewPass + "; " + User1LimitPass + "; " + User1ForbiddenPass + ".");
			}
			else
			{
				// Заходим в профиль
				OpenUserProfileFromHomePage();
				// Восстанавливаем пароль
				RecoverUserPassword(currentPassword);
				// Возвращаемся на главную страницу
				Header.OpenHomepage();
			}
		}

		/// <summary>
		/// Проверить, что пользователь может зайти со своим паролем
		/// </summary>
		protected void CheckUserLastPasswordValid()
		{
			// Выйти из редактора
			Header.ClickCloseEditorForm();

			// Выйти из пользователя
			Header.LogoutUser();
			// Зайти с обычным паролем
			LoginUser(User1);
			Assert.IsFalse(IsExistIncorrectPasswordError(), "Ошибка: пользователь не может зайти со своим старым паролем!");
		}
	}
}
