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
            OpenHomepage();

            // Получить полное имя пользователя после изменения
            string resultUserName = GetFullNameHomepage();

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
            string resultUserName = GetFullNameProfile();

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
            OpenLeaderboardPage();
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
            OpenCoursePage();
            // Получить полное имя после изменения
            string resultUserName = GetFullNameMainMenu();

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
            string translationText = "Example Translation " + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
            // Выйти из редактора
            ClickBackEditor();

            // Перейти в профиль
            OpenUserProfileFromCourse();
            // Открыть форму
            OpenEditProfileForm();
            string newUserName = UserName + DateTime.Now.Ticks;
            string newUserSurname = UserSurname + DateTime.Now.Ticks;
            // Изменить имя и фамилию
            SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);

            // Вернуться в лекцию
            OpenCoursePage();
            OpenCourseByName(courseName);
            OpenLectureByRowNum(lectureRowNumber);
            ClickEditorRowByNum(translationRowNum);
            // Найти добавленный перевод
            int curTranslationNumber = GetSuggestedTranslationRowNum(translationText);
            // Получить имя в списке добавленных переводов
            string resultUserName = GetSuggestedTranslationAuthor(curTranslationNumber);

            // Выйти из редактора
            ClickBackEditor();
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
            string translationText = "Example Translation " + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);

            // Найти добавленный перевод
            int curTranslationNumber = GetSuggestedTranslationRowNum(translationText);
            // Получить имя в списке добавленных переводов
            string resultUserName = GetSuggestedTranslationAuthor(curTranslationNumber);

            // Выйти из редактора
            ClickBackEditor();
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
            string translationText = "Example Translation " + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
            // Выйти из редактора
            ClickBackEditor();
            // Перейти на главную страницу
            OpenHomepage();
            // Ожидание появления события в списке
            WaitEventInEventListByTarget(translationText);
            int rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
            // Имя пользователя в списке событий до изменения имени
            string userNameBefore = GetAuthorEventList(rowNumber);

            // Перейти в профиль
            OpenUserProfileFromHomePage();
            // Открыть форму
            OpenEditProfileForm();
            string newUserName = UserName + DateTime.Now.Ticks;
            string newUserSurname = UserSurname + DateTime.Now.Ticks;
            // Изменить имя и фамилию
            SaveNewUserNameInEditProfileForm(newUserName, newUserSurname);
            
            // Перейти на главную страницу
            OpenHomepage();
            // Ожидание изменения имени в списке событий
            bool isAuthorChanged = WaitEventListChangingAuthor(translationText, HomePageLastEventType.AddTranslationEvent, userNameBefore);
            rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
            // Получить имя в списке событий
            string resultUserName = GetAuthorEventList(rowNumber).Replace("...", "");
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
            string translationText = "Example Translation " + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
            // Выйти из редактора
            ClickBackEditor();
            // Перейти на главную страницу
            OpenHomepage();
            // Ожидание появления события в списке
            WaitEventInEventListByTarget(translationText);
            int rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
            // Имя пользователя в списке событий до изменения имени
            string resultUserName = GetAuthorEventList(rowNumber).Replace("...", "");

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
            OpenCoursePage();

            // Есть ли аватар
            bool isAvatarExist = GetIsAvatarExistMainMenu();
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
            bool isAvatarExist = GetIsAvatarExistProfile();
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
            OpenLeaderboardPage();
            // Есть ли аватар
            bool isAvatarExist = GetIsAvatarExistLeaderboard();
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

            // Перейти к списку лидеров
            OpenEditProfileForm();
            // Есть ли аватар
            bool isAvatarExist = GetIsAvatarExistEditProfileForm();
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
            ChangeNameProfile("");

            Assert.IsTrue(IsElementDisplayed(By.XPath(
                ".//div[contains(@class,'main-fields')]//input[contains(@data-bind,'name')][contains(@class,'errorInput')]")),
                "Ошибка: поле Имя должно быть отмечено ошибкой");
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'errorMsg2')][contains(@data-bind,'name')]")),
                "Ошибка: должна появиться ошибка о неправильном имени");
            Assert.IsTrue(Driver.FindElement(By.XPath(".//input[contains(@class,'js-save')]")).GetAttribute("class").Contains("btn-inactive"),
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
            ChangeNameProfile(" ");

            Assert.IsTrue(IsElementDisplayed(By.XPath(
                ".//div[contains(@class,'main-fields')]//input[contains(@data-bind,'name')][contains(@class,'errorInput')]")),
                "Ошибка: поле Имя должно быть отмечено ошибкой");
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'errorMsg2')][contains(@data-bind,'name')]")),
                "Ошибка: должна появиться ошибка о неправильном имени");
            Assert.IsTrue(Driver.FindElement(By.XPath(".//input[contains(@class,'js-save')]")).GetAttribute("class").Contains("btn-inactive"),
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
            ChangeSurnameProfile("");

            Assert.IsTrue(IsElementDisplayed(By.XPath(
                ".//div[contains(@class,'main-fields')]//input[contains(@data-bind,'surname')][contains(@class,'errorInput')]")),
                "Ошибка: поле Фамилия должно быть отмечено ошибкой");
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'errorMsg2')][contains(@data-bind,'surname')]")),
                "Ошибка: должна появиться ошибка о неправильной фамилии");
            Assert.IsTrue(Driver.FindElement(By.XPath(".//input[contains(@class,'js-save')]")).GetAttribute("class").Contains("btn-inactive"),
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
            ChangeSurnameProfile(" ");

            Assert.IsTrue(IsElementDisplayed(By.XPath(
                ".//div[contains(@class,'main-fields')]//input[contains(@data-bind,'surname')][contains(@class,'errorInput')]")),
                "Ошибка: поле Фамилия должно быть отмечено ошибкой");
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'errorMsg2')][contains(@data-bind,'surname')]")),
                "Ошибка: должна появиться ошибка о неправильной фамилии");
            Assert.IsTrue(Driver.FindElement(By.XPath(".//input[contains(@class,'js-save')]")).GetAttribute("class").Contains("btn-inactive"),
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
            string resultAboutInfo = GetAboutMeInfo();
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
            LogoutUser();

            // Войти в пользователя с новым паролем
            LoginUser(User1.login, newUserPassword);
            // Проверить, что удается войти в пользователя
            Assert.IsFalse(IsExistIncorrectPasswordError(), "Ошибка: не удается войти в пользователя с новым паролем (" + User1NewPass + ")");

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
            LogoutUser();

            // Войти в пользователя с новым паролем
            LoginUser(User1.login, newUserPassword);
            // Проверить, что удается войти в пользователя
            Assert.IsFalse(IsExistIncorrectPasswordError(), "Ошибка: не удается войти в пользователя с новым паролем (" + User1NewPass + ")");

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
            Assert.IsTrue(Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@name='newpassword']")).GetAttribute("class").Contains("errorInput"),
                "Ошибка: поле Новый пароль не отмечено ошибкой");
            Assert.IsTrue(IsElementPresent(By.XPath(".//form[@id='change-password-form']//div[contains(@class,'errorMsg2')][contains(@data-bind,'newpassword')]")),
                "Ошибка: не появилось сообщение о слишком коротком пароле");

            // Проверить, что кнопка Сохранить заблокирована
            Assert.IsTrue(Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@type='submit']")).GetAttribute("class").Contains("btn-inactive"),
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
            LogoutUser();

            // Войти в пользователя с новым паролем
            LoginUser(User1.login, newUserPassword);
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
            Assert.IsTrue(IsElementPresent(By.XPath(".//form[@id='change-password-form']//div[contains(@class,'js-dynamic-errors')]//p[@id='error1']")),
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

            // Проверить, что поле Подтверждение пароля отмечено ошибкой и есть сообщение об ошибке
            Assert.IsTrue(Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@name='confirmpassword']")).GetAttribute("class").Contains("errorInput"),
                "Ошибка: поле Подтверждение пароля не отмечено ошибкой");
            Assert.IsTrue(IsElementPresent(By.XPath(".//form[@id='change-password-form']//div[contains(@class,'errorMsg2')][contains(@data-bind,'confirmpassword')]")),
                "Ошибка: не появилось сообщение о не совпадающих паролях");

            // Проверить, что кнопка Сохранить заблокирована
            Assert.IsTrue(Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@type='submit']")).GetAttribute("class").Contains("btn-inactive"),
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
            OpenCoursePage();
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
            OpenCoursePage();
            // Перейти к списку лидеров
            OpenLeaderboardPage();

            // В списке лидеров найти ссылку на другого пользователя
            string curUserName = GetFullNameLeaderboard();
            string anotherUserRef = ".//div[contains(@class,'rating')]//tr[4]//td[3]/a[contains(@data-bind,'name')]";
            Driver.FindElement(By.XPath(anotherUserRef));
            Assert.IsTrue(IsElementPresent(By.XPath(anotherUserRef)), "Ошибка: список лидеров пуст");
            // Получить имя первого лидера
            string firstLeaderName = Driver.FindElement(By.XPath(anotherUserRef)).Text.Trim();
            if (firstLeaderName != curUserName)
            {
                anotherUserRef = ".//div[contains(@class,'rating')]//tr[5]//td[3]/a[contains(@data-bind,'name')]";
                Assert.IsTrue(IsElementPresent(By.XPath(anotherUserRef)), "Ошибка: в списке нет других пользователей");
            }

            // Перейти в профиль другого пользователя
            Driver.FindElement(By.XPath(anotherUserRef)).Click();
            // Дождаться загрузки страницы
            Wait.Until((d) => d.FindElement(By.ClassName("profile-description")).Displayed);

            // Открыть профиль текущего пользователя
            OpenUserProfileFromCourse();
            Thread.Sleep(1000);
            // Проверить, что открылся профиль именно текущего пользователя
            Assert.AreEqual(curUserName,
                Driver.FindElement(By.XPath(".//div[contains(@class,'profile-title')]")).Text.Trim(),
                "Ошибка: не открылся профиль текущего пользователя");
        }

        /// <summary>
        /// Тест: открытие профиля по ссылке в лидерборде
        /// </summary>
        [Test]
        public void OpenProfileFromLeaderboard()
        {
            // Получить текущее имя пользователя
            string userName = GetUserNameHomepage();

            // Пролистать список лидеров до пользователя
            int numInList = ScrollLeaderboardToUser(userName);

            // Кликнуть по пользователю в списке
            Driver.FindElement(By.XPath(".//div[contains(@class,'rating')]//tr[" + numInList + "]//td[3]/a")).Click();
            Assert.IsTrue(IsElementDisplayed(By.ClassName("profile-description")), "Ошибка: профиль не открылся");
        }

        /// <summary>
        /// Получить полное имя пользователя на главной странице
        /// </summary>
        /// <returns>полное имя - "имя фамилия"</returns>
        protected string GetFullNameHomepage()
        {
            // Получить полное имя пользователя на главной странице
            // Полное имя - "имя фамилия"
            return Driver.FindElement(By.XPath(".//a[contains(@data-bind,'userName')]")).Text;
        }

        /// <summary>
        /// Получить полное имя пользователя на странице профиля пользователя
        /// </summary>
        /// <returns>полное имя - "имя фамилия"</returns>
        protected string GetFullNameProfile()
        {
            // Получить полное имя пользователя на странице профиля пользователя
            // Полное имя - "имя фамилия"
            return Driver.FindElement(By.XPath(".//div[contains(@data-bind,'fullName')]")).Text;
        }

        /// <summary>
        /// Получить полное имя пользователя на странице списка лидеров
        /// </summary>
        /// <returns>полное имя - "имя фамилия"</returns>
        protected string GetFullNameLeaderboard()
        {
            return Driver.FindElement(By.XPath(".//tr[contains(@class,'active')][@style='']//td[contains(@data-bind,'name')]")).Text.Trim();
        }

        /// <summary>
        /// Получить полное имя пользователя в главном меню
        /// </summary>
        /// <returns>полное имя - "имя фамилия"</returns>
        protected string GetFullNameMainMenu()
        {
            return Driver.FindElement(By.XPath(".//div[@id='main-menu']//a[contains(@class,'user-name')]")).Text.Trim();
        }

        /// <summary>
        /// Открыть форму редактирования профиля
        /// </summary>
        protected void OpenEditProfileForm()
        {
            // Открыть форму редактирования
            Driver.FindElement(By.CssSelector("a[data-popup =\"editor-form\"]")).Click();
            Wait.Until((d) => d.FindElement(By.Id("editor-form")).Displayed);
        }

        /// <summary>
        /// Закрыть форму редактирования профиля
        /// </summary>
        protected void CloseEditProfileForm()
        {
            // Открыть форму редактирования
            Driver.FindElement(By.XPath(".//div[@id='editor-form']//div[contains(@class,'cancel')]")).Click();
            WaitUntilDisappearElement(".//div[@id='editor-form']");
        }

        /// <summary>
        /// Открыть форму изменения пароля
        /// </summary>
        protected void OpenEditPasswordForm()
        {
            // Открыть форму редактирования
            OpenEditProfileForm();
            // Перейти на вкладку изменения пароля
            Driver.FindElement(By.XPath(".//div[@id='editor-form']//label[contains(@data-bind,'changePassword')]")).Click();
            Wait.Until((d) => d.FindElement(By.Id("change-password-form")).Displayed);
        }

        /// <summary>
        /// Изменить и сохранить новые имя и фамилию в форме редактирования профиля
        /// </summary>
        /// <param name="newUserName">новое имя</param>
        /// <param name="newUserSurname">ноая фамилия</param>
        protected void SaveNewUserNameInEditProfileForm(string newUserName, string newUserSurname)
        {
            // Ввести новое имя
            ChangeNameProfile(newUserName);
            // Ввести новую фамилию
            ChangeSurnameProfile(newUserSurname);
            // Сохранить
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//input[contains(@class,'js-save')]")).Click();
            // Дождаться, пока форма закроется
            WaitUntilDisappearElement("a[data-popup =\"editor-form\"]");
        }

        /// <summary>
        /// Ввести имя
        /// </summary>
        /// <param name="name">имя</param>
        protected void ChangeNameProfile(string name)
        {
            // Очистить поле Имя
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//input[@name='name']")).Clear();
            // Ввести новое имя
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//input[@name='name']")).SendKeys(name);
        }

        /// <summary>
        /// Ввести фамилию
        /// </summary>
        /// <param name="surname">фамилия</param>
        protected void ChangeSurnameProfile(string surname)
        {
            // Очистить поле Имя
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//input[@name='surname']")).Clear();
            // Ввести новое имя
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//input[@name='surname']")).SendKeys(surname);
        }

        /// <summary>
        /// Добавить информацию о себе
        /// </summary>
        /// <param name="info"></param>
        protected void ChangeAboutMeInfo(string info)
        {
            // Очистить поле Информация о себе
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//textarea[contains(@data-bind,'aboutme')]")).Clear();
            // Добавить информацию
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//textarea[contains(@data-bind,'aboutme')]")).SendKeys(info);
            // Сохранить
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//input[contains(@class,'js-save')]")).Click();
            // Дождаться, пока форма закроется
            WaitUntilDisappearElement("a[data-popup =\"editor-form\"]");
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
            Driver.FindElement(By.XPath(".//form[contains(@id,'edit-profile-form')]//div[contains(@class,'js-upload-btn')]//a")).Click();

            // Заполнить диалог загрузки документа
            FillAddDocumentForm(ImageFile);

            // Сохранить
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//input[contains(@class,'js-save')]")).Click();
            // Дождаться, пока форма закроется
            WaitUntilDisappearElement("a[data-popup =\"editor-form\"]");
        }

        /// <summary>
        /// Проверить, есть ли аватар в главном меню
        /// </summary>
        /// <returns>есть аватар</returns>
        protected bool GetIsAvatarExistMainMenu()
        {
            return Driver.FindElement(By.XPath(".//div[@id='main-menu']//span[contains(@class,'menu-user-link')]/img")).GetAttribute("src").Contains("/avatar/");
        }

        /// <summary>
        /// Проверить, есть ли аватар в профиле
        /// </summary>
        /// <returns>есть аватар</returns>
        protected bool GetIsAvatarExistProfile()
        {
            return Driver.FindElement(By.XPath(".//div[contains(@class,'profile-description')]/..//img")).GetAttribute("src").Contains("/avatar/");
        }

        /// <summary>
        /// Проверить, есть ли аватар в профиле
        /// </summary>
        /// <returns>есть аватар</returns>
        protected bool GetIsAvatarExistLeaderboard()
        {
            return Driver.FindElement(By.XPath(".//tr[contains(@class,'active')][@style='']//td[2]/img")).GetAttribute("src").Contains("/avatar/");
        }

        /// <summary>
        /// Проверить, есть ли аватар в форме редактирования пользователя
        /// </summary>
        /// <returns>есть аватар</returns>
        protected bool GetIsAvatarExistEditProfileForm()
        {
            return Driver.FindElement(By.XPath(
                ".//form[@id='edit-profile-form']//div[contains(@class,'js-avatar')]//img[contains(@data-bind,'attr')]")).GetAttribute("src").Contains("/Files/");
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
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//div[contains(@class,'js-clear-btn')]/a")).Click();

            // Сохранить
            Driver.FindElement(By.XPath(".//form[@id='edit-profile-form']//input[contains(@class,'js-save')]")).Click();
            // Дождаться, пока форма закроется
            WaitUntilDisappearElement("a[data-popup =\"editor-form\"]");
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
            if (GetIsAvatarExistProfile())
            {
                // Удалить аватар
                RecoverAvatarFromProfile();
                // Проверить, что аватара нет
                Assert.IsFalse(GetIsAvatarExistProfile(), "Ошибка: не удаляется аватар перед началом теста");
            }

            // Открыть форму
            OpenEditProfileForm();
            // Добавить аватар
            AddAvatarInProfile();
        }

        /// <summary>
        /// Получить текст информации о себе
        /// </summary>
        /// <returns>информация о себе</returns>
        protected string GetAboutMeInfo()
        {
            return Driver.FindElement(By.XPath(".//div[contains(@data-bind,'about')]")).Text.Trim();
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
            Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@name='password']")).Clear();
            Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@name='password']")).SendKeys(oldPassword);
            // Вести новый пароль
            Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@name='newpassword']")).Clear();
            Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@name='newpassword']")).SendKeys(newPassword);
            // Вести подтверждение нового пароля
            if (confirmPassword.Length == 0)
            {
                confirmPassword = newPassword;
            }
            Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@name='confirmpassword']")).Clear();
            Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[@name='confirmpassword']")).SendKeys(confirmPassword);
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

            // Проверить, что кнопка Сохранить не заблокирована
            Assert.IsTrue(IsElementPresent(By.XPath(".//form[@id='change-password-form']//input[@type='submit'][not(contains(@class,'btn-inactive'))]")),
                "Ошибка: кнопка Сохранить заблокирована");

            Driver.FindElement(By.XPath(".//form[@id='change-password-form']//input[contains(@data-bind,'submitClick')]")).Click();
            // Дождаться, пока форма закроется
            WaitUntilDisappearElement(".//form[@id='change-password-form']");
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
            bool isExistError = false;
            if (!WaitUntilDisappearElement(".//form[@id='login-form-login']"))
            {
                isExistError = IsElementDisplayed(By.XPath(
                                    ".//form[@id='login-form-login']//div[contains(@class,'js-dynamic-errors')]"));
            }
            return isExistError;
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
                FillAuthorizationData(User1.login, pass);
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
                OpenHomepage();
            }
        }

        /// <summary>
        /// Проверить, что пользователь может зайти со своим паролем
        /// </summary>
        protected void CheckUserLastPasswordValid()
        {
            // Выйти из редактора
            Driver.FindElement(By.XPath(".//div[@id='editor-form']//div[@class='cancel']")).Click();
            // Выйти из пользователя
            LogoutUser();
            // Зайти с обычным паролем
            LoginUser(User1);
            Assert.IsFalse(IsExistIncorrectPasswordError(), "Ошибка: пользователь не может зайти со своим старым паролем!");
        }
    }
}
