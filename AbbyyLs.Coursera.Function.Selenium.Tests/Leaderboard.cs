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
    class Leaderboard : BaseTest
    {
        public Leaderboard(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Тест: Проверка отображения пользователя в общем списке
        /// т.е. даже если пользователь не входит в 10ку юзеров, он все равно должен быть отображен в конце таблицы
        /// </summary>
        [Test]
        public void ExistUserMainList()
        {
            // Получить текущее имя пользователя
            string userName = GetUserNameHomepage();

            // Перейти в список лидеров
            OpenCoursePage();
            OpenLeaderboardPage();

            // Проверить, что пользователь есть в списке
            Assert.IsTrue(GetIsUserExistLeaderboard(userName), "Ошибка: пользователя нет в списке");
        }

        /// <summary>
        /// Тест: Проверка, что место пользователя из Профиля Пользователя совпадает с местом в Лидерборде
        /// </summary>
        [Test]
        public void UserPositionLeaderboard()
        {
            // Получить текущее имя пользователя
            string userName = GetUserNameHomepage();

            // Пролистать список лидеров до пользователя
            int numInList = ScrollLeaderboardToUser(userName);

            // Имя пользователя по этому номеру:
            string userCurrentPositionName = Driver.FindElement(By.XPath(".//div[contains(@class,'rating')]//tr[" + numInList + "]//td[3]")).Text;

            Assert.AreEqual(userName, userCurrentPositionName, "Ошибка: на этом месте пользователь с другим именем");
        }

        /// <summary>
        /// Тест: Проверка, что рейтинг из Профиля Пользователя совпадает с рейтингом в Лидерборде
        /// </summary>
        [Test]
        public void UserRatingLeaderboard()
        {
            // Получить текущее имя пользователя
            string userName = GetUserNameHomepage();

            // Открыть профиль пользователя
            OpenUserProfileFromHomePage();
            // Получить рейтинг пользователя в профиле
            Decimal userRating = GetUserRating();
            // Перейти к списку лидеров
            OpenLeaderboardPage();
            // Получить рейтинг пользователя в лидерборде
            Decimal leaderboardRating = GetRatingLeaderboard();
            // Сравнить рейтинг
            Assert.AreEqual(userRating, leaderboardRating, "Ошибка: рейтинг в лидерборде не совпадает с рейтингом в профиле");
        }

        /// <summary>
        /// Тест: Проверка пользователя в лидерборде для курса - пользователь есть в лидерах для курса
        /// </summary>
        [Test]
        public void CourseLeaderboardUserExist()
        {
            // Получить текущее имя пользователя
            string userName = GetUserNameHomepage();
            // Добавить перевод
            string translationText = "Example Translation " + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
            ClickBackEditor();

            // Перейти к списку лидеров
            OpenLeaderboardPage();

            // Открыть курс в лидерборде
            OpenLeaderboardCourse(courseName);

            // Проверка, есть ли пользователь
            Assert.IsTrue(GetIsUserExistLeaderboard(userName), "Ошибка: пользователя нет в списке");
        }

        /// <summary>
        /// Тест: Проверка пользователя в лидерборде для курса - рейтинг для курса меньше общего рейтинга
        /// </summary>
        [Test]
        public void CourseLeaderboardCompareRating()
        {
            // Получить текущее имя пользователя
            string userName = GetUserNameHomepage();
            // Добавить перевод
            string translationText = "Example Translation " + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
            ClickBackEditor();
            // Перейти в список курсов
            OpenCoursePage();
            // Открыть другой курс
            OpenAnotherCourse(courseName);
            // Открыть первую лекцию
            OpenLectureByRowNum(1);
            // Добавить перевод            
            AddTranslationByRowNum(GetEmptyTranslationRowNumber(), translationText);
            ClickBackEditor();
            
            // Перейти к списку лидеров
            OpenLeaderboardPage();
            // Получить рейтинг пользователя в лидерборде
            Decimal leaderboardRating = GetRatingLeaderboard();

            // Открыть курс в лидерборде
            OpenLeaderboardCourse(courseName);

            // Получить рейтинг для курса
            Decimal courseRating = GetRatingLeaderboard();
            // Сравнить рейтинг общий и для курса
            Assert.IsTrue(courseRating < leaderboardRating, "Ошибка: рейтинг для курса должен быть меньше общего рейтинга");
        }

        /// <summary>
        /// Вернуть, есть ли пользователь в списке лидеров
        /// </summary>
        /// <returns>есть ли пользователь в списке</returns>
        protected bool GetIsUserExistLeaderboard(string userName)
        {
            setDriverTimeoutMinimum();
            bool isExist = IsElementPresent(By.XPath(".//tr[not(contains(@style,'display: none;'))]//td[3][contains(text(),'" + userName + "')]")) ||
                IsElementPresent(By.XPath(".//tr[not(contains(@style,'display: none;'))]//td[3]/a[contains(text(),'" + userName + "')]"));
            setDriverTimeoutDefault();
            return isExist;
        }

        /// <summary>
        /// Вернуть рейтинг пользователя в лидерборде
        /// </summary>
        /// <returns>рейтинг</returns>
        protected Decimal GetRatingLeaderboard()
        {
            return Decimal.Parse(
                Driver.FindElement(By.XPath(
                ".//tr[contains(@class,'active')][not(contains(@style,'display: none'))]//td[contains(@data-bind,'rating')]")).Text.Trim().Replace(".", ","));
        }

        /// <summary>
        /// Получить номер курса в выпадающем списке на странице лидерборда
        /// </summary>
        /// <param name="courseName">имя курса</param>
        /// <returns>индекс</returns>
        private int GetCourseIndexLeaderboardlist(string courseName)
        {
            int courseIndex = 0;
            IList<IWebElement> courseList = Driver.FindElements(By.XPath(".//select[@id='select_courses_rat']//option"));
            for (int i = 0; i < courseList.Count; ++i)
            {
                if (courseList[i].Text == courseName)
                {
                    courseIndex = i;
                    break;
                }
            }

            Assert.IsTrue(courseIndex > 0, "Ошибка: курса нет в выпадающем списке");
            return courseIndex;
        }

        /// <summary>
        /// В лидерборде открыть конкретный курс
        /// </summary>
        /// <param name="courseName">название курса</param>
        private void OpenLeaderboardCourse(string courseName)
        {
            string totalNum = Driver.FindElement(By.XPath(".//div[contains(@data-bind,'total')]")).Text;
            // Открыть список курсов
            Driver.FindElement(By.Id("select_courses_rat")).Click();
            // Получить индекс курса в списке
            int courseIndex = GetCourseIndexLeaderboardlist(courseName);
            for (int i = 0; i < courseIndex; ++i)
            {
                SendKeys.SendWait(@"{Down}");
            }
            // Выбрать нужный курс
            SendKeys.SendWait(@"{Enter}");
            // Дождаться обновления
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@data-bind,'total')][not(contains(text()," + totalNum + "))]")));
        }
    }
}
