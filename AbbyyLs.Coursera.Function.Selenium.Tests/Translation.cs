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
    class Translation : BaseTest
    {
        public Translation(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Тест: добавление перевода в сегмент, где нет перевода пользователя
        /// Проверка, что количество переведенных предложений в профиле увеличилось
        /// </summary>
        [Test]
        public void AddTranslationNoTranslations()
        {
            // Зайти в профиль пользователя
            OpenUserProfileFromHomePage();

            // Получить количество переведенных предложений до добавления перевода
            int translationsNumberBefore = GetUserTranslationsNumber();

            // Добавить перевод
            string translationText = "Test" + DateTime.Now.Ticks;
            AddTranslation(translationText);
            // Вернуться из редактора
            ClickBackEditor();
            // Зайти в профиль
            OpenUserProfileFromCourse();

            // Получить количество переведенных предложений после добавления перевода
            int translationsNumberAfter = GetUserTranslationsNumber();
            // Проверить, что рейтинг увеличился
            Assert.IsTrue(translationsNumberAfter > translationsNumberBefore,
                "Ошибка: после добавления перевода количество переведенных предложений пользователя не увеличилось");
        }

        /// <summary>
        /// Тест: добавление перевода в сегмент, где уже есть перевод пользователя
        /// Проверка, что количество переведенных предложений в профиле увеличилось
        /// </summary>
        [Test]
        public void AddTranslationExistTranslations()
        {
            // Добавить перевод
            string translationText = "Test" + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);

            // Вернуться из редактора
            ClickBackEditor();
            // Зайти в профиль
            OpenUserProfileFromCourse();

            // Получить количество переведенных предложений до добавления перевода
            int translationsNumberBefore = GetUserTranslationsNumber();

            // Перейти к списку курсов
            OpenCoursePage();
            // Зайти в курс
            OpenCourseByName(courseName);
            // Перейти в лекцию
            OpenLectureByRowNum(lectureRowNumber);
            string newTranslationText = "Test " + DateTime.Now.Ticks;

            // Добавить новый перевод в ту же ячейку
            AddTranslationByRowNum(translationRowNum, newTranslationText);
            // Вернуться из редактора
            ClickBackEditor();
            // Зайти в профиль
            OpenUserProfileFromCourse();

            // Получить количество переведенных предложений до добавления перевода
            int translationsNumberAfter = GetUserTranslationsNumber();

            // Проверить, что количество переводов увеличилось
            Assert.IsTrue(translationsNumberAfter > translationsNumberBefore, "Ошибка: количество переводов не увеличилось");
        }

        /// <summary>
        /// Тест: добавление перевода в сегмент, где уже есть перевод другого пользователя
        /// Проверка, что количество переведенных предложений в профиле увеличилось
        /// </summary>
        [Test]
        public void AddTranslationExistTranslationsAnotherUsers()
        {
            // Добавить перевод
            string translationText = "Test" + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
            
            // Вернуться из редактора
            ClickBackEditor();
            // Выйти из пользователя
            LogoutUser();
            // Зайти другим пользователем
            LoginUser(User2);

            // Зайти в профиль
            OpenUserProfileFromCourse();

            // Получить количество переведенных предложений до добавления перевода
            int translationsNumberBefore = GetUserTranslationsNumber();

            // Перейти к списку курсов
            OpenCoursePage();
            // Зайти в курс
            OpenCourseByName(courseName);
            // Перейти в лекцию
            OpenLectureByRowNum(lectureRowNumber);
            string newTranslationText = "Test " + DateTime.Now.Ticks;

            // Добавить новый перевод в ту же ячейку
            AddTranslationByRowNum(translationRowNum, newTranslationText);
            // Вернуться из редактора
            ClickBackEditor();
            // Зайти в профиль
            OpenUserProfileFromCourse();

            // Получить количество переведенных предложений до добавления перевода
            int translationsNumberAfter = GetUserTranslationsNumber();

            // Проверить, что количество переводов увеличилось
            Assert.IsTrue(translationsNumberAfter > translationsNumberBefore, "Ошибка: количество переводов не увеличилось");
        }

        /// <summary>
        /// Тест: удаление перевода
        /// Проверка, что количество переведенных предложений в профиле уменьшилось
        /// </summary>
        [Test]
        public void DeleteTranslation()
        {
            // Добавить перевод
            string translationText = "Test" + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);

            // Вернуться из редактора
            ClickBackEditor();
            // Зайти в профиль
            OpenUserProfileFromCourse();

            // Получить количество переведенных предложений до добавления перевода
            int translationsNumberBefore = GetUserTranslationsNumber();

            // Перейти к списку курсов
            OpenCoursePage();
            // Зайти в курс
            OpenCourseByName(courseName);
            // Перейти в лекцию
            OpenLectureByRowNum(lectureRowNumber);
            // Кликнуть по ячейке
            string targetCell = "#segments-body div table tr:nth-child(" + translationRowNum + ") td:nth-child(4) div";
            // Кликнуть по ячейке
            Driver.FindElement(By.CssSelector(targetCell)).Click();
            // Удалить перевод
            DeleteTranslationSuggestedTranslations();
            // Вернуться из редактора
            ClickBackEditor();
            // Зайти в профиль
            OpenUserProfileFromCourse();

            // Получить количество переведенных предложений до добавления перевода
            int translationsNumberAfter = GetUserTranslationsNumber();

            // Проверить, что количество переводов уменьшилось
            Assert.IsTrue(translationsNumberAfter < translationsNumberBefore, "Ошибка: количество переводов не уменьшилось");
        }

        /// <summary>
        /// Тест: редактирование перевода
        /// Проверка, что количество переведенных предложений в профиле не изменилось
        /// </summary>
        [Test]
        public void EditTranslation()
        {
            // Добавить перевод
            string translationText = "Test" + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);

            // Вернуться из редактора
            ClickBackEditor();
            // Зайти в профиль
            OpenUserProfileFromCourse();

            // Получить количество переведенных предложений до добавления перевода
            int translationsNumberBefore = GetUserTranslationsNumber();

            // Перейти к списку курсов
            OpenCoursePage();
            // Зайти в курс
            OpenCourseByName(courseName);
            // Перейти в лекцию
            OpenLectureByRowNum(lectureRowNumber);
            // Кликнуть по ячейке
            string targetCell = "#segments-body div table tr:nth-child(" + translationRowNum + ") td:nth-child(4) div";
            Driver.FindElement(By.CssSelector(targetCell)).Click();
            // Кликнуть редактировать перевод
            int rowNum = GetSuggestedTranslationRowNum(translationText);
            Driver.FindElement(By.XPath(".//div[@id='translations-body']//table//tr[" + rowNum + "]//td[4]//span[contains(@class,'fa-pencil')]")).Click();
            // Ввести текст
            Driver.FindElement(By.CssSelector(targetCell)).SendKeys(DateTime.Now.ToString());
            // Кликнуть Confirm
            Driver.FindElement(By.Id("confirm-btn")).Click();
            // Дождаться Confirm
            AssertConfirmIsDone(translationRowNum);
            // Вернуться из редактора
            ClickBackEditor();
            // Зайти в профиль
            OpenUserProfileFromCourse();

            // Получить количество переведенных предложений до добавления перевода
            int translationsNumberAfter = GetUserTranslationsNumber();

            // Проверить, что количество переводов не изменилось
            Assert.AreEqual(translationsNumberBefore, translationsNumberAfter, "Ошибка: количество переводов изменилось");
        }
    }
}
