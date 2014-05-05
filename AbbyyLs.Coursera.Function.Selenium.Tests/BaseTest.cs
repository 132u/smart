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
    [TestFixture("StageUrl2", "StageWorkspace2", "Firefox")]
    [TestFixture("StageUrl3", "StageWorkspace2", "Firefox")]
    public class BaseTest
    {
        protected const int maxEditorLinesNum = 16;
        protected const int numberSentencesGrowProgress = 5;
        protected const int maxDurationWaitEventList = 50;

        private IWebDriver _driver;
        protected IWebDriver Driver
        {
            get
            {
                return _driver;
            }

        }
        private WebDriverWait _wait;
        protected WebDriverWait Wait
        {
            get
            {
                return _wait;
            }
        }
        private FirefoxProfile _profile;
        private string _url;
        protected string Url
        {
            get
            {
                return _url;
            }
        }

        protected struct UserInfo
        {
            public string login;
            public string password;

            public UserInfo (string l, string p)
            {
                login = l;
                password = p;
            }
        }

        private UserInfo _user1;
        protected UserInfo User1
        {
            get
            {
                return _user1;
            }
        }

        private UserInfo _user2;
        protected UserInfo User2
        {
            get
            {
                return _user2;
            }
        }

        private List<UserInfo> _testUserList;
        protected List<UserInfo> TestUserList
        {
            get
            {
                return _testUserList;
            }
        }


        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
        }

        private string _userSurname;
        public string UserSurname
        {
            get
            {
                return _userSurname;
            }
        }

        // Возможные типы событий в списке последних событий на главной странице
        public enum HomePageLastEventType { AddTranslationEvent, VoteUpEvent}

        protected string PathTestResults
        {
            get
            {
                System.IO.DirectoryInfo directoryInfo =
                    System.IO.Directory.GetParent(@"..\..\..\TestResults\");

                return directoryInfo.ToString();
            }
        }

        private string _imageFile;
        public string ImageFile
        {
            get
            {
                return _imageFile;
            }
        }

        private string _browserName;

        private string BrowserName
        {
            get
            {
                return _browserName;
            }
        }

        private string _user1NewPass;
        public string User1NewPass
        {
            get
            {
                return _user1NewPass;
            }
        }

        private string _user1ForbiddenPass;
        public string User1ForbiddenPass
        {
            get
            {
                return _user1ForbiddenPass;
            }
        }

        private string _user1LimitPass;
        public string User1LimitPass
        {
            get
            {
                return _user1LimitPass;
            }
        }

        /// <summary>
        /// Конструктор BaseTest
        /// </summary>
        public BaseTest(string url, string workspaceUrl, string browserName)
        {
            _browserName = browserName;
            _url = ConfigurationManager.AppSettings[url];
            CreateDriver();

            _user1.login = ConfigurationManager.AppSettings["LoginUser1"];
            _user1.password = ConfigurationManager.AppSettings["PasswordUser1"];

            _user2.login = ConfigurationManager.AppSettings["LoginUser2"];
            _user2.password = ConfigurationManager.AppSettings["PasswordUser2"];

            _userName = ConfigurationManager.AppSettings["userName1"];
            _userSurname = ConfigurationManager.AppSettings["userSurname1"];

            _imageFile = Path.GetFullPath(ConfigurationManager.AppSettings["TestImageFile"]);

            _user1NewPass = ConfigurationManager.AppSettings["PasswordUser1newPass"];
            _user1ForbiddenPass = ConfigurationManager.AppSettings["PasswordUser1forbiddenPass"];
            _user1LimitPass = ConfigurationManager.AppSettings["PasswordUser1limitPass"];

            _testUserList = new List<UserInfo>();
            
            // Заполнение данных тестовых пользователей
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser1"], ConfigurationManager.AppSettings["PasswordTestUser1"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser2"], ConfigurationManager.AppSettings["PasswordTestUser2"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser3"], ConfigurationManager.AppSettings["PasswordTestUser3"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser4"], ConfigurationManager.AppSettings["PasswordTestUser4"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser5"], ConfigurationManager.AppSettings["PasswordTestUser5"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser6"], ConfigurationManager.AppSettings["PasswordTestUser6"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser7"], ConfigurationManager.AppSettings["PasswordTestUser7"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser8"], ConfigurationManager.AppSettings["PasswordTestUser8"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser9"], ConfigurationManager.AppSettings["PasswordTestUser9"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser10"], ConfigurationManager.AppSettings["PasswordTestUser10"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser11"], ConfigurationManager.AppSettings["PasswordTestUser11"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser12"], ConfigurationManager.AppSettings["PasswordTestUser12"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser13"], ConfigurationManager.AppSettings["PasswordTestUser13"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser14"], ConfigurationManager.AppSettings["PasswordTestUser14"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser15"], ConfigurationManager.AppSettings["PasswordTestUser15"]));
            _testUserList.Add(new UserInfo(ConfigurationManager.AppSettings["LoginTestUser16"], ConfigurationManager.AppSettings["PasswordTestUser16"]));
        }

        /// <summary>
        /// Метод создания _driver 
        /// </summary>
        private void CreateDriver()
        {
            if (BrowserName == "Firefox")
            {
                if (_driver == null)
                {
                    _profile = new FirefoxProfile();
                    _profile.AcceptUntrustedCertificates = true;
                    _profile.SetPreference("browser.download.dir", PathTestResults);
                    _profile.SetPreference("browser.download.folderList", 2);
                    _profile.SetPreference("browser.download.useDownloadDir", false);
                    _profile.SetPreference("browser.download.manager.showWhenStarting", false);
                    _profile.SetPreference("browser.helperApps.alwaysAsk.force", false);
                    _profile.SetPreference
                        ("browser.helperApps.neverAsk.saveToDisk", "text/xml, text/csv, text/plain, text/log, application/zip, application/x-gzip, application/x-compressed, application/x-gtar, multipart/x-gzip, application/tgz, application/gnutar, application/x-tar, application/x-xliff+xml,  application/msword.docx, application/pdf, application/x-pdf, application/octetstream, application/x-ttx, application/x-tmx, application/octet-stream");
                    //_profile.SetPreference("pdfjs.disabled", true);

                    _driver = new FirefoxDriver(_profile);
                    //string profiledir = "../../../Profile";
                    // string profiledir = "TestingFiles/Profile";
                    //_profile = new FirefoxProfile(profiledir);
                    //_driver = new FirefoxDriver(_profile);
                }
            }
            else if (BrowserName == "Chrome")
            {
                _driver = new ChromeDriver();
            }
            else if (BrowserName == "IE")
            {
                //TODO: Сделать запуск из IE
            }

            setDriverTimeoutDefault();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        }

        /// <summary>
        /// Устанавливаем ожидание для драйвера в минимум (1 секунда)
        /// </summary>
        protected void setDriverTimeoutMinimum()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Устанавливаем ожидание для драйвера значение по умолчанию (10 секунд)
        /// </summary>
        protected void setDriverTimeoutDefault()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
        }

        /// <summary>
        /// Устанавливаем ожидание для драйвера в максимум (20 секунд)
        /// </summary>
        protected void setDriverTimeoutMaximum()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Метод авторизации пользователя
        /// </summary>
        /// <param name="login">логин пользователя</param>
        /// <param name="password">пароль пользователя</param>
        protected void LoginUser(string login, string password)
        {
            // Открыть форму логина
            WaitAndClickElement(".//a[contains(@data-popup,'login-form')]");
            // Дождаться открытия формы
            _wait.Until((d) => d.FindElement(By.Id("login-form")).Displayed);
            // Заполнить форму
            FillAuthorizationData(login, password);
        }

        /// <summary>
        /// Метод авторизации пользователя
        /// </summary>
        /// <param name="userInfo">Пользователь</param>
        protected void LoginUser(UserInfo userInfo)
        {
            // Открыть форму логина
            WaitAndClickElement(".//a[contains(@data-popup,'login-form')]");
            // Дождаться открытия формы
            _wait.Until((d) => d.FindElement(By.Id("login-form")).Displayed);
            // Заполнить форму
            FillAuthorizationData(userInfo.login, userInfo.password);
        }

        /// <summary>
        /// Заполнение формы авторизации пользователя
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль</param>
        protected void FillAuthorizationData(string login, string password)
        {
            // Ввести логин и пароль
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).SendKeys(login);
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).SendKeys(password);
            // Нажать Зайти
            _driver.FindElement(By.CssSelector("input[type =\"submit\"]")).Click();
        }

        /// <summary>
        /// Ожидание события в списке событий
        /// </summary>
        /// <param name="targetText">тест в Target события</param>
        /// <param name="evType">тип события</param>
        /// <param name="waitAppearEvent">дождаться, пока появится или пока пропадет событие (true - пока появится, false - пока пропадет)</param>
        /// <returns>Событие есть в списке</returns>
        protected bool WaitEventInEventListByTarget(string targetText, HomePageLastEventType evType = HomePageLastEventType.AddTranslationEvent, bool waitAppearEvent = true)
        {
            bool isNeedStopWait = true;
            bool isPresent = false;
            TimeSpan timeBegin = DateTime.Now.TimeOfDay;
            setDriverTimeoutMinimum();
            do
            {
                // Проверить, есть ли событие
                isPresent = IsElementPresent(By.XPath(
                    ".//table[@class='last-events']//tr//td/span[contains(@data-bind,'target')][contains(text(),'"
                        + targetText + "')]/../..//td[4]//div[contains(@class,'" + ConvertLastEventTypeToString(evType) + "')]"));
                // Проверить, что условие поиска выполнено или превышен лимит времени
                isNeedStopWait = (!waitAppearEvent ? !isPresent : isPresent) || DateTime.Now.TimeOfDay.Subtract(timeBegin).Seconds > maxDurationWaitEventList;
                if (isNeedStopWait)
                {
                    break;
                }
                // Ожидание
                Console.WriteLine("start sleep");
                Thread.Sleep(1000);
                Console.WriteLine("end sleep");
                // Обновление страницы
                _driver.FindElement(By.XPath(".//div")).SendKeys(OpenQA.Selenium.Keys.F5);
            } while (!isNeedStopWait);
            setDriverTimeoutDefault();
            // Вернуть, есть ли событие в списке
            return isPresent;
        }

        /// <summary>
        /// Дождаться, пока изменится имя автора события
        /// </summary>
        /// <param name="targetText">текст в Target события</param>
        /// <param name="evType">тип события</param>
        /// <param name="oldAuthor">старый автор</param>
        /// <returns>Имя автора события изменилось</returns>
        protected bool WaitEventListChangingAuthor(string targetText, HomePageLastEventType evType, string oldAuthor)
        {
            bool isNeedStopWait = true;
            bool isChanged = false;
            TimeSpan timeBegin = DateTime.Now.TimeOfDay;
            setDriverTimeoutMinimum();
            do
            {
                int rowNumber = GetEventRowNum(targetText, evType);
                // Имя автора
                string authorName = _driver.FindElement(By.XPath(".//table[@class='last-events']//tr[" + rowNumber + "]//td[4]//a[contains(@data-bind,'userName')]")).Text.Trim();
                // Автор изменился?
                isChanged = authorName != oldAuthor;
                // Проверить, выполнено ли условие поиска или превышен лимит времени
                isNeedStopWait = isChanged || DateTime.Now.TimeOfDay.Subtract(timeBegin).Seconds > maxDurationWaitEventList;
                if (isNeedStopWait)
                {
                    break;
                }
                Thread.Sleep(1000);
                // Обновить страницу
                _driver.FindElement(By.XPath(".//div")).SendKeys(OpenQA.Selenium.Keys.F5);
            } while (!isNeedStopWait);
            setDriverTimeoutDefault();
            // Вернуть, изменилось ли имя
            return isChanged;
        }

        /// <summary>
        /// Получить номер строки конретного события
        /// </summary>
        /// <param name="targetText">текст в Target события</param>
        /// <param name="evType">тип события</param>
        /// <returns>номер строки с событием</returns>
        protected int GetEventRowNum(string targetText, HomePageLastEventType evType)
        {
            // Дождаться появления события
            Assert.IsTrue(WaitEventInEventListByTarget(targetText, evType), "Ошибка: событие не появилось");
            int rowNumber = 0;
            // Список событий
            IList<IWebElement> eventsList = _driver.FindElements(By.XPath(".//table[@class='last-events']//tr//td[3]/span"));
            for (int i = 0; i < eventsList.Count; ++i)
            {
                if (eventsList[i].Text.Contains(targetText))
                {
                    // Событие с тем же target
                    if (_driver.FindElement(By.XPath(".//table[@class='last-events']//tr[" + (i + 2) + "]//td[4]//div[contains(@data-bind,'actionStyle')]")).GetAttribute("class")
                        == ConvertLastEventTypeToString(evType))
                    {
                        // и тип события совпадает
                        rowNumber = (i + 2);
                        break;
                    }
                }
            }
            // Возвращаем номер строки с событием
            return rowNumber;
        }

        /// <summary>
        /// Получить рейтинг в списке событий
        /// </summary>
        /// <param name="rowNumber">номер строки с событием</param>
        /// <returns>рейтинг</returns>
        protected int GetRatingEventList(int rowNumber)
        {
            return int.Parse(
                _driver.FindElement(By.XPath(".//table[@class='last-events']//tr[" + rowNumber + "]//td[5]//span[contains(@data-bind,'rating')]")).Text.Trim());
        }

        /// <summary>
        /// Получить автора в списке событий
        /// </summary>
        /// <param name="rowNumber">номер строки с событием</param>
        /// <returns>автор</returns>
        protected string GetAuthorEventList(int rowNumber)
        {
            return _driver.FindElement(By.XPath(".//table[@class='last-events']//tr[" + rowNumber + "]//td[4]//a[contains(@data-bind,'userName')]")).Text.Trim();
        }
        
        /// <summary>
        /// Проголосовать в списке событий
        /// </summary>
        /// <param name="rowNumber">номер строки с событием</param>
        /// <param name="voteUp">тип голоса (true: ЗА, false: Против)</param>
        /// <returns>можно ли проголосовать (возвращает false, если нельзя проголосовать - кнопка заблокирована)</returns>
        protected bool VoteEventListByRowNum(int rowNumber, bool voteUp)
        {
            string voteClass = voteUp ? "like" : "dislike";
            // Проверить, что можно проголосовать
            bool canVote = IsElementPresent(By.XPath(".//table[@class='last-events']//tr[" + rowNumber + "]//td[5]//div[@class='" + voteClass + "']"));
            if (canVote)
            {
                // Проголосовать
                _driver.FindElement(By.XPath(".//table[@class='last-events']//tr[" + rowNumber + "]//td[5]//div[@class='" + voteClass + "']")).Click();
                // Дождаться, пока голос будет учтен
                _wait.Until((d) => d.FindElement(By.XPath(".//table[@class='last-events']//tr[" + rowNumber + "]//td[5]//div[@class='" + voteClass + "d']")));
            }
            return canVote;
        }

        /// <summary>
        /// В редакторе проголосовать За/Против перевод/а
        /// </summary>
        /// <param name="rowNumber">номер строки в списке предложенных переводов</param>
        protected void VoteFromEditor(bool isVoteUp, int rowNumber)
        {
            string voteClass = isVoteUp ? "fa-thumbs-up" : "fa-thumbs-down";
            Driver.FindElement(By.XPath(".//div[@id='translations-body']//table//tr[" + rowNumber + "]//td[5]/div//span[contains(@class,'" + voteClass + "')]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetIsVoteConsideredEditor(isVoteUp, rowNumber), "Ошибка: голос не принят");
        }

        /// <summary>
        /// Проголосовать в списке событий за событие, за которое уже голосовали
        /// </summary>
        /// <param name="rowNumber">номер строки с событием</param>
        /// <param name="voteUp">тип голоса (true: За, false: против)</param>
        protected void TryVoteVotedEventListByRowNum(int rowNumber, bool voteUp)
        {
            // т.к. за событие уже голосовали, то class будет как уже проголосованный
            string voteClass = voteUp ? "liked" : "disliked";
            // Проголосовать
            _driver.FindElement(By.XPath(".//table[@class='last-events']//tr[" + rowNumber + "]//td[5]//div[@class='" + voteClass + "']")).Click();
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Вернуть, есть ли событие в списке последних событий
        /// </summary>
        /// <param name="targetText">текст в Target события</param>
        /// <param name="evType">тип события</param>
        /// <param name="author">автор события</param>
        /// <returns>есть ли событие</returns>
        protected bool GetIsExistInLastEventList(string targetText, HomePageLastEventType evType, string author)
        {
            bool isPresent = false;
            TimeSpan timeBegin = DateTime.Now.TimeOfDay;
            setDriverTimeoutMinimum();

            // Проверить событие с таким Target
            if (IsElementPresent(By.XPath(".//table[@class='last-events']//tr//td[3]/span[contains(text(),'" + targetText + "')]")))
            {
                // Все события
                IList<IWebElement> targetList = _driver.FindElements(By.XPath(".//table[@class='last-events']//tr//td[3]/span"));
                for (int i = 0; i < targetList.Count; ++i)
                {
                    // Событие с таким Target
                    if (targetList[i].Text.Contains(targetText))
                    {
                        // Тип события
                        HomePageLastEventType curEventType = ConvertLastEventFromString(
                            _driver.FindElement(By.XPath(".//table[@class='last-events']//tr[" + (i + 1) + "]//td[4]//div[contains(@data-bind,'actionStyle')]"))
                            .GetAttribute("class"));
                        if (curEventType != evType)
                        {
                            continue;
                        }

                        // Проверить автора
                        string curAuthor = _driver.FindElement(By.XPath(".//table[@class='last-events']//tr[" + (i + 1) + "]//td[4]//a[contains(@data-bind,'userName')]")).Text;
                        if (curAuthor == author)
                        {
                            isPresent = true;
                            break;
                        }
                    }
                }
            }
            setDriverTimeoutDefault();
            // Есть ли событие
            return isPresent;
        }

        /// <summary>
        /// Добавить перевод
        /// (выбрать курс, выбрать лекцию, добавить перевод в сегмент)
        /// </summary>
        /// <param name="translationText">текст перевода</param>
        protected void AddTranslation(string translationText)
        {
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
        }
        
        /// <summary>
        /// Добавить перевод и вернуть курс, лекцию и сегмент
        /// </summary>
        /// <param name="translationText">текст перевода</param>
        /// <param name="courseName">OUT: имя курса, в который добавлен перевод</param>
        /// <param name="lectureRowNumber">OUT: номер строки лекции, в которую добавлен перевод</param>
        /// <param name="translationRowNum">OUT: номер строки сегмента, в который добавлен перевод</param>
        protected void AddTranslation(string translationText, out string courseName, out int lectureRowNumber, out int translationRowNum)
        {
            courseName = "";
            lectureRowNumber = 0;
            translationRowNum = 0;

            // Перейти к списку доступных курсов
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            courseName = OpenCourseMinProgress();
            // Перейти в лекцию
            lectureRowNumber = 15;// OpenLectureNotFilled();
            OpenLectureByRowNum(lectureRowNumber);
            // Найти предложение без перевода
            translationRowNum = GetEmptyTranslationRowNumber();

            // Добавить перевод
            AddTranslationByRowNum(translationRowNum, translationText);
            // Вернуться в строку
            ClickEditorRowByNum(translationRowNum);
        }

        /// <summary>
        /// Получить инорфмацию о самоми последнем (самом верхнем/свежем) событии в списке последних событий
        /// </summary>
        /// <param name="targetText">OUT: текст в Target последнего события</param>
        /// <param name="action">OUT: тип последнего события</param>
        /// <param name="author">OUT: автор последнего события</param>
        protected void GetLastEventInfo(out String targetText, out HomePageLastEventType action, out String author)
        {
            // вернуть текст в Target последнего события
            targetText = _driver.FindElement(By.XPath(".//table[@class='last-events']//tr[2]//td[3]/span")).Text;
            // вернуть тип последнего события
            action = ConvertLastEventFromString(_driver.FindElement(By.XPath(".//table[@class='last-events']//tr[2]//td[4]/span/div")).GetAttribute("class"));
            // вернуть автора последнего события
            author = _driver.FindElement(By.XPath(".//table[@class='last-events']//tr[2]//td[4]//a")).Text;
        }

        /// <summary>
        /// Преобразование типа в строковый эквивалент (класс тега)
        /// </summary>
        /// <param name="evType">тип события</param>
        /// <returns>строковый эквивалент</returns>
        protected string ConvertLastEventTypeToString(HomePageLastEventType evType)
        {
            string evString = "";
            switch (evType)
            {
                case HomePageLastEventType.AddTranslationEvent:
                    evString = "arrow";
                    break;
                case HomePageLastEventType.VoteUpEvent:
                    evString = "tick";
                    break;
            }

            return evString;
        }

        /// <summary>
        /// Преобразование строкового класса тега в тип события
        /// </summary>
        /// <param name="eventStr">строковый класс тега</param>
        /// <returns>тип события</returns>
        protected HomePageLastEventType ConvertLastEventFromString(string eventStr)
        {
            HomePageLastEventType evType = HomePageLastEventType.AddTranslationEvent;
            switch (eventStr)
            {
                case "arrow":
                    evType = HomePageLastEventType.AddTranslationEvent;
                    break;
                case "tick":
                    evType = HomePageLastEventType.VoteUpEvent;
                    break;
            }

            return evType;
        }

        /// <summary>
        /// В редакторе кликает Back для выхода из него
        /// </summary>
        protected void ClickBackEditor()
        {
            // Back
            _driver.FindElement(By.Id("back-btn")).Click();
            _wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'lectures')]")));
        }

        /// <summary>
        /// Разлогиниться из этого пользователя
        /// </summary>
        protected void LogoutUser()
        {
            // Выйти из под этого пользователя
            _driver.FindElement(By.XPath(".//a[contains(@data-bind,'logout')]")).Click();
        }

        /// <summary>
        /// Поиск курса с наименьшим прогрессом
        /// </summary>
        /// <returns>возвращает имя курса</returns>
        protected string SelectCourseMinProgress(out Decimal courseProgress)
        {
            Decimal minProgressRight = 100;
            // Список курсов для "правого" столбика
            IList<IWebElement> courseRightList = _driver.FindElements(By.XPath(".//ul[contains(@data-bind,'projectsRight')]//table//td[2]//div[contains(@data-bind,'name')]"));
            for (int i = 0; i < courseRightList.Count; ++i)
            {
                // Не учитываем курсы, в названии которых есть TestProject
                if (!_driver.FindElement(By.XPath(
                        ".//ul[contains(@data-bind,'projectsRight')]//table[" + (i + 1) + "]//td[2]//div[contains(@data-bind,'name')]")).Text.Contains("TestProject"))
                {
                    Decimal curProgress = Decimal.Parse(_driver.FindElement(By.XPath(
                        ".//ul[contains(@data-bind,'projectsRight')]//table[" + (i + 1) + "]//td[1]//span[contains(@class,'percent')]")).Text.Replace("%", "").Replace(".", ","));
                    if (curProgress < minProgressRight)
                    {
                        // минимальный прогресс
                        minProgressRight = curProgress;
                    }
                }
            }

            Decimal minProgressLeft = 100;
            // Список курсов для "левого" столбика"
            IList<IWebElement> courseLeftList = _driver.FindElements(By.XPath(".//ul[contains(@data-bind,'projectsLeft')]//table//td[2]//div[contains(@data-bind,'name')]"));
            for (int i = 0; i < courseRightList.Count; ++i)
            {
                // Не учитываем курсы, в названии которых есть TestProject
                if (!_driver.FindElement(By.XPath(
                        ".//ul[contains(@data-bind,'projectsLeft')]//table[" + (i + 1) + "]//td[2]//div[contains(@data-bind,'name')]")).Text.Contains("TestProject"))
                {
                    Decimal curProgress = Decimal.Parse(_driver.FindElement(By.XPath(
                        ".//ul[contains(@data-bind,'projectsLeft')]//table[" + (i + 1) + "]//td[1]//span[contains(@class,'percent')]")).Text.Replace("%", "").Replace(".", ","));
                    if (curProgress < minProgressLeft)
                    {
                        // минимальный прогресс
                        minProgressLeft = curProgress;
                    }
                }
            }

            // Выбрать, какой прогресс меньше (в левом или правом столбике)
            courseProgress = minProgressLeft < minProgressRight ? minProgressLeft : minProgressRight;

            // Нажимаем на ссылку курса с наименьшим прогрессом
            string courseXPath = ".//td[contains(@class,'coursePercents')]/span[contains(@class,'percent')][text()='" + courseProgress.ToString().Replace(",", ".") + "%']/../..//td/a";
            string courseName = "";

            // Все курсы с этим прогрессом
            IList<IWebElement> courseElements = 
                _driver.FindElements(By.XPath(
                ".//td[contains(@class,'coursePercents')]/span[contains(@class,'percent')][text()='" + courseProgress.ToString().Replace(",", ".") + "%']/../..//td[2]//div"));
            foreach (IWebElement el in courseElements)
            {
                // Не учитываем курсы, в названии которых есть TestProject
                if (!el.Text.Contains("TestProject"))
                {
                    courseName = el.Text;
                    break;
                }
            }
            // Вернуть имя курса
            return courseName;
        }

        /// <summary>
        /// Поиск курса с максимальным прогрессом
        /// </summary>
        /// <returns>возвращает имя курса</returns>
        protected string SelectCourseMaxProgress(out Decimal courseProgress)
        {
            Decimal maxProgressRight = 100;
            // Список курсов для "правого" столбика
            IList<IWebElement> courseRightList = _driver.FindElements(By.XPath(".//ul[contains(@data-bind,'projectsRight')]//table//td[2]//div[contains(@data-bind,'name')]"));
            for (int i = 0; i < courseRightList.Count; ++i)
            {
                // Не учитываем курсы, в названии которых есть TestProject
                if (!_driver.FindElement(By.XPath(
                        ".//ul[contains(@data-bind,'projectsRight')]//table[" + (i + 1) + "]//td[2]//div[contains(@data-bind,'name')]")).Text.Contains("TestProject"))
                {
                    Decimal curProgress = Decimal.Parse(_driver.FindElement(By.XPath(
                        ".//ul[contains(@data-bind,'projectsRight')]//table[" + (i + 1) + "]//td[1]//span[contains(@class,'percent')]")).Text.Replace("%", "").Replace(".", ","));
                    if (curProgress > maxProgressRight)
                    {
                        // максимальный прогресс
                        maxProgressRight = curProgress;
                    }
                }
            }

            Decimal maxProgressLeft = 100;
            // Список курсов для "левого" столбика"
            IList<IWebElement> courseLeftList = _driver.FindElements(By.XPath(".//ul[contains(@data-bind,'projectsLeft')]//table//td[2]//div[contains(@data-bind,'name')]"));
            for (int i = 0; i < courseRightList.Count; ++i)
            {
                // Не учитываем курсы, в названии которых есть TestProject
                if (!_driver.FindElement(By.XPath(
                        ".//ul[contains(@data-bind,'projectsLeft')]//table[" + (i + 1) + "]//td[2]//div[contains(@data-bind,'name')]")).Text.Contains("TestProject"))
                {
                    Decimal curProgress = Decimal.Parse(_driver.FindElement(By.XPath(
                        ".//ul[contains(@data-bind,'projectsLeft')]//table[" + (i + 1) + "]//td[1]//span[contains(@class,'percent')]")).Text.Replace("%", "").Replace(".", ","));
                    if (curProgress > maxProgressLeft)
                    {
                        // максимальный прогресс
                        maxProgressLeft = curProgress;
                    }
                }
            }

            // Выбрать, какой прогресс больше (в левом или правом столбике)
            courseProgress = maxProgressLeft > maxProgressRight ? maxProgressLeft : maxProgressRight;

            // Нажимаем на ссылку курса с наименьшим прогрессом
            string courseXPath = ".//td[contains(@class,'coursePercents')]/span[contains(@class,'percent')][text()='" + courseProgress.ToString().Replace(",", ".") + "%']/../..//td/a";
            string courseName = "";

            // Все курсы с этим прогрессом
            IList<IWebElement> courseElements =
                _driver.FindElements(By.XPath(
                ".//td[contains(@class,'coursePercents')]/span[contains(@class,'percent')][text()='" + courseProgress.ToString().Replace(",", ".") + "%']/../..//td[2]//div"));
            foreach (IWebElement el in courseElements)
            {
                // Не учитываем курсы, в названии которых есть TestProject
                if (!el.Text.Contains("TestProject"))
                {
                    courseName = el.Text;
                    break;
                }
            }
            // Вернуть имя курса
            return courseName;
        }

        /// <summary>
        /// Переход в курс по имени курса
        /// </summary>
        protected void OpenCourseByName(string courseName)
        {
            // Перейти в курс
            _driver.FindElement(By.XPath(".//div[contains(@class,'name')][contains(text(),'" + courseName.Trim() + "')]/../../a")).Click();
            // Дождаться перехода в курс
            _wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'tabs_cont')]")).Displayed);
        }

        /// <summary>
        /// Открыть другой курс (имя которого отличается от указанного и не содержит TestProject)
        /// </summary>
        /// <param name="courseNameNotToOpen">имя курса, который не открывать</param>
        /// <returns>имя курса, который открыли</returns>
        protected string OpenAnotherCourse(string courseNameNotToOpen)
        {
            string openCourseName = "";
            // Список курсов
            IList<IWebElement> courseList = Driver.FindElements(By.XPath(".//div[contains(@class,'name')]"));
            foreach (IWebElement el in courseList)
            {
                // Найти курс, название которого отличается от переданного и не содержит TestProject
                if (el.Text != courseNameNotToOpen &&
                    !el.Text.Contains("TestProject"))
                {
                    openCourseName = el.Text;
                    OpenCourseByName(openCourseName);
                    break;
                }
            }
            // имя курса
            return openCourseName;
        }

        // Перейти на главную страницу
        protected void OpenHomepage()
        {
            // Кликнуть по переходу на главную страницу
            _driver.FindElement(By.ClassName("on-homepage")).Click();
            // Дождаться открытия главной страницы
            _wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'logo')]")));
        }

        /// <summary>
        /// Выбор лекции (с общим и личным прогрессом меньше 100)
        /// </summary>
        /// <returns>номер строки с лекцией</returns>
        protected int SelectLectureGetRowNumber()
        {
            // Получить проценты личного прогресса всех лекций
            IList<IWebElement> percentElements = _driver.FindElements(By.XPath(".//div[contains(@data-bind,'personalProgressView')]"));
            // Проверить, что список лекций не пуст
            Assert.IsTrue(percentElements.Count > 0, "Ошибка: список лекций пуст");
            bool isLectureExist = false;
            int fullProgress = 0, personalProgress = 0;
            int rowNumber = 0;
            for (int i = 0; i < percentElements.Count; ++i)
            {
                // Выбираем лекцию с личным и общим прогрессом меньше 100
                personalProgress = int.Parse(percentElements[i].Text.Replace("%", ""));
                if (personalProgress < 100)
                {
                    fullProgress = int.Parse(
                        _driver.FindElement(By.XPath(
                        ".//tbody[contains(@data-bind,'lectures')]//tr[" + (i + 1) + "]//td/div[contains(@data-bind,'progressView')]"))
                        .Text.Replace("%", ""));
                    if (fullProgress < 100)
                    {
                        isLectureExist = true;
                        // нумерация строк с 1, а цикл с 0
                        rowNumber = i + 1;
                        break;
                    }
                }
            }
            Assert.IsTrue(isLectureExist, "Ошибка: нет подходящей лекции (с общим и личным прогрессом меньше 100 ");

            // Вернуть номер строки с лекцией
            return rowNumber;
        }

        /// <summary>
        /// Выбор лекции (с общим и личным прогрессом == 0)
        /// </summary>
        /// <returns>номер строки с лекцией</returns>
        protected int SelectEmptyLectureGetRowNumber()
        {
            // Получить проценты личного прогресса всех лекций
            IList<IWebElement> percentElements = _driver.FindElements(By.XPath(".//div[contains(@data-bind,'personalProgressView')]"));
            // Проверить, что список лекций не пуст
            Assert.IsTrue(percentElements.Count > 0, "Ошибка: список лекций пуст");

            bool isLectureExist = false;
            int fullProgress = 0, personalProgress = 0;
            int rowNumber = 0;
            for (int i = 0; i < percentElements.Count; ++i)
            {
                // Выбираем лекцию с личным и общим прогрессом == 0
                personalProgress = int.Parse(percentElements[i].Text.Replace("%", ""));
                if (personalProgress == 0)
                {
                    fullProgress = int.Parse(
                        _driver.FindElement(By.XPath(
                        ".//tbody[contains(@data-bind,'lectures')]//tr[" + (i + 1) + "]//td/div[contains(@data-bind,'progressView')]"))
                        .Text.Replace("%", ""));
                    if (fullProgress == 0)
                    {
                        isLectureExist = true;
                        rowNumber = i + 1;
                        break;
                    }
                }
            }
            Assert.IsTrue(isLectureExist, "Ошибка: нет подходящей лекции (с общим и личным прогрессом == 0");

            // Вернуть номер строки с лекцией
            return rowNumber;
        }

        /// <summary>
        /// Переход в лекцию по номеру строки
        /// </summary>
        /// <param name="rowNumber">номер строки с лекцией</param>
        protected void OpenLectureByRowNum(int rowNumber)
        {
            // Перейти в лекцию
            _driver.FindElement(By.XPath(".//tbody[contains(@data-bind,'lectures')]//tr[" + rowNumber + "]//a")).Click();
            // Ждем перехода в лекцию
            _wait.Until((d) => d.FindElement(By.Id("back-btn")).Displayed);

            Scripts(_driver).ExecuteScript("localStorage.clear()");
            _driver.FindElement(By.Id("back-btn")).SendKeys(OpenQA.Selenium.Keys.F5);
            _wait.Until((d) => d.FindElement(By.XPath(".//div[@id='segments-body']//tbody//tr")).Displayed);

        }

        public static IJavaScriptExecutor Scripts (IWebDriver driver)
        {
            return (IJavaScriptExecutor)driver;
        }

        /// <summary>
        /// Добавить переводв строку
        /// </summary>
        /// <param name="rowNumber">номер строки</param>
        /// <param name="targetText">текст перевода</param>
        protected void AddTranslationByRowNum(int rowNumber, string targetText)
        {
            string targetCell = "#segments-body div table tr:nth-child(" + rowNumber + ") td:nth-child(4) div";
            // Кликнуть по ячейке
            _driver.FindElement(By.CssSelector(targetCell)).Click();
            // Очистить
            _driver.FindElement(By.CssSelector(targetCell)).Clear();
            // Написать текст в ячейке target
            _driver.FindElement(By.CssSelector(targetCell)).SendKeys(targetText);
            // Кликнуть Confirm
            _driver.FindElement(By.Id("confirm-btn")).Click();
            // Дождаться Confirm
            AssertConfirmIsDone(rowNumber);
        }

        /// <summary>
        /// Получить номер строки с пустым Target
        /// </summary>
        /// <returns>номер строки</returns>
        protected int GetEmptyTranslationRowNumber()
        {
            // Получить номер строки с пустым Target в редакторе
            int rowNumber = GetEmptyRowNumberEditor();

            // Если нет пустых строк или строка дальше 16й (около 17-18й строки начинаются проблемы с селениумом - он видит только 34 строки)
            if (rowNumber == 0 || rowNumber > maxEditorLinesNum)
            {
                // Очистить лекцию
                ClearLecture();
                // Обновить номер строки
                rowNumber = GetEmptyRowNumberEditor();
            }

            // Вернуть номер строки
            return rowNumber;
        }

        /// <summary>
        /// Получить номер строки в Editor с пустым Target
        /// </summary>
        /// <returns></returns>
        private int GetEmptyRowNumberEditor()
        {
            // Список Target
            IList<IWebElement> sentenceList = _driver.FindElements(By.CssSelector("#segments-body div table tr td:nth-child(4) div"));
            int rowNumber = 0;
            for (int i = 0; i < sentenceList.Count; ++i)
            {
                // Target пуст
                if (sentenceList[i].Text.Trim().Length == 0)
                {
                    rowNumber = i + 1;
                    break;
                }
            }
            // номер строки
            return rowNumber;
        }

        /// <summary>
        /// Отдает количество вариантов перевода для конкретного предложения
        /// </summary>
        /// <param name="translationRowNum">номер строки с предложением</param>
        /// <returns>количество вариантов перевода</returns>
        protected int GetTranslationVariantsNum(int translationRowNum)
        {
            // Получить количество вариантов перевода
            _driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + translationRowNum + ") td:nth-child(4) div")).Click();
            string variantsNumberStr = _driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + translationRowNum + ") td:nth-child(6) div")).Text.Trim();
            int variantsNumber = 0;
            // Если поле пустое - то вариантов 0
            if (variantsNumberStr.Length > 0)
            {
                // Количество вариантов перевода
                variantsNumber = int.Parse(variantsNumberStr);
            }
            return variantsNumber;
        }

        /// <summary>
        /// Очистить лекцию
        /// </summary>
        protected void ClearLecture()
        {
            IList<IWebElement> list = _driver.FindElements(By.XPath(".//div[@id='segments']//table//td[4]//div"));
            int countClear = list.Count > maxEditorLinesNum ? maxEditorLinesNum : list.Count;
            // Удалить с 1 сегмента
            DeleteTranslations(1, countClear);
        }

        /// <summary>
        /// Удалить переводы
        /// </summary>
        /// <param name="begin">номер строки, начиная с которого удалить переводы</param>
        /// <param name="count">количество удаляемых переводов</param>
        protected void DeleteTranslations(int begin, int count)
        {
            IList<IWebElement> list = _driver.FindElements(By.XPath(".//div[@id='segments']//table//td[4]//div"));
            for (int i = begin; i < (begin + count); ++i)
            {
                // Кликнуть по сегменту в нужной строке
                _driver.FindElement(By.XPath(".//div[@id='segments']//table//tr[" + i + "]//td[4]//div")).Click();

                if (list[(i - 1)].Text.Trim().Length > 0)
                {
                    // Удалить перевод из предложенных переводов
                    DeleteTranslationSuggestedTranslations(list[(i - 1)].Text.Trim());
                }
            }
        }

        /// <summary>
        /// Удалить переводы
        /// </summary>
        /// <param name="rowList">список номеров строк для удаления переводов</param>
        protected void DeleteTranslations(List<int> rowList)
        {
            IList<IWebElement> list = _driver.FindElements(By.XPath(".//div[@id='segments']//table//td[4]//div"));
            foreach (int i in rowList)
            {
                // Кликнуть по сегменту в нужной строке
                _driver.FindElement(By.XPath(".//div[@id='segments']//table//tr[" + i + "]//td[4]//div")).Click();
                if (list[i].Text.Trim().Length > 0)
                {
                    // Удалить перевод из предложенных переводов
                    DeleteTranslationSuggestedTranslations(list[i].Text.Trim());
                }
            }
        }

        /// <summary>
        /// Удалить конкретный перевод
        /// </summary>
        /// <param name="translationText">текст перевода, который надо удалить</param>
        protected void DeleteTranslationSuggestedTranslations(string translationText)
        {
            // Получить номер строки в переводом в предложенных переводах
            int rowNum = GetSuggestedTranslationRowNum(translationText);
            // Нажать на Удалить
            Driver.FindElement(By.XPath(".//div[@id='translations-body']//table//tr[" + rowNum + "]//td[4]//span[contains(@class,'fa-trash-o')]")).Click();
            // Подтвердить
            Driver.FindElement(By.XPath(".//div[@id='messagebox']//a[contains(@class,'x-btn-blue')]")).Click();
            // Дождаться, пока диалог подтверждения пропадет
            WaitUntilDisappearElement(".//div[@id='messagebox']//a[contains(@class,'x-btn-blue')]");
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Кликнуть по Target в строке в Editor
        /// </summary>
        /// <param name="rowNumber">номер строки</param>
        protected void ClickEditorRowByNum(int rowNumber)
        {
            _driver.FindElement(By.XPath(".//div[@id='segments']//table//tr[" + rowNumber + "]//td[4]//div")).Click();
        }

        /// <summary>
        /// Получить номер строки перевода в предложенных переводах
        /// </summary>
        /// <param name="translationText">перевод</param>
        /// <returns>номер строки</returns>
        protected int GetSuggestedTranslationRowNum(string translationText)
        {
            int rowNumber = 0;
            IList<IWebElement> translationsList = _driver.FindElements(By.XPath(".//div[@id='translations-body']//table//tr//td[3]/div"));
            for(int i = 0; i < translationsList.Count; ++i)
            {
                // Текст перевода содержит нужный перевод?
                if (translationsList[i].Text.Contains(translationText))
                {
                    rowNumber = i + 1;
                    break;
                }
            }

            // Номер строки
            return rowNumber;
        }

        /// <summary>
        /// Получить автора предложенного перевода
        /// </summary>
        /// <param name="rowNumber">номер строки с предложенным переводом</param>
        /// <returns>имя автора</returns>
        protected string GetSuggestedTranslationAuthor(int rowNumber)
        {
            return _driver.FindElement(By.XPath(".//div[@id='translations-body']//table//tr[" + rowNumber + "]//td[2]/div")).Text.Trim();
        }

        /// <summary>
        /// Дождаться появления элемента и кликнуть
        /// </summary>
        /// <param name="xPath"></param>
        protected void WaitAndClickElement(string xPath)
        {
            // Дождаться появления элемента
            _wait.Until((d) => d.FindElement(By.XPath(xPath)).Displayed);
            // Кликнуть по элементу
            _driver.FindElement(By.XPath(xPath)).Click();
        }

        /// <summary>
        /// Перейти на страницу профиля пользователя с главной страницы
        /// </summary>
        protected void OpenUserProfileFromHomePage()
        {
            // Перейти на страницу профиля пользователя
            _driver.FindElement(By.ClassName("user-link")).Click();
            // Дождаться загрузки страницы
            _wait.Until((d) => d.FindElement(By.ClassName("profile-description")).Displayed);
        }

        /// <summary>
        /// Перейти на страницу профиля пользователя со страницы курсов
        /// </summary>
        protected void OpenUserProfileFromCourse()
        {
            // Перейти на страницу профиля пользователя
            _driver.FindElement(By.ClassName("user-name")).Click();
            // Дождаться загрузки страницы
            _wait.Until((d) => d.FindElement(By.ClassName("profile-description")).Displayed);
            Thread.Sleep(5000); // TODO  убрать
        }

        /// <summary>
        /// Открыть страницу со списком курсов
        /// </summary>
        protected void OpenCoursePage()
        {
            _driver.FindElement(By.XPath(".//a[contains(@href,'/Courses')]")).Click();
            _wait.Until((d) => d.FindElement(By.XPath(".//ul[contains(@class,'projects-list')]")).Displayed);
        }
        
        /// <summary>
        /// Открыть страницу с лидербордом
        /// </summary>
        protected void OpenLeaderboardPage()
        {
            _driver.FindElement(By.XPath(".//a[contains(@href,'/Leaderboard')]")).Click();
            _wait.Until((d) => d.FindElement(By.ClassName("leaders")).Displayed);
        }


        /// <summary>
        /// Открыть курс с наименьшим прогрессом
        /// </summary>
        /// <returns>имя курса</returns>
        protected string OpenCourseMinProgress()
        {
            Decimal courseProgress;
            // Выбрать курс с минимальным прогрессом
            string courseName = SelectCourseMinProgress(out courseProgress);
            OpenCourseByName(courseName);
            return courseName;
        }

        /// <summary>
        /// Открыть лекцию с неполным прогрессом
        /// </summary>
        /// <returns>номер строки с лекцией</returns>
        protected int OpenLectureNotFilled()
        {
            // Выбрать незаполненную лекцию
            int lectureRowNumber = SelectLectureGetRowNumber();
            OpenLectureByRowNum(lectureRowNumber);
            return lectureRowNumber;
        }

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        /// <param name="numTranslated">OUT: количество переведенных предложений</param>
        /// <param name="userRating">OUT: рейтинг пользователя</param>
        /// <param name="numVotesUp">OUT: количество голосов За</param>
        /// <param name="numVotesDown">OUT: количество голосов Против</param>
        protected void GetUserInfo(out int numTranslated, out Decimal userRating, out int numVotesUp, out int numVotesDown)
        {
            numTranslated = GetUserTranslationsNumber();
            userRating = GetUserRating();
            GetUserVotesInfo(out numVotesUp, out numVotesDown);
        }

        /// <summary>
        /// Возвращает из профиля пользователя количество переведенных предложений
        /// </summary>
        /// <returns>количество переведенных предложений</returns>
        protected int GetUserTranslationsNumber()
        {
            return int.Parse(
                _driver.FindElement(By.XPath(".//div[contains(@class,'profile-description')]//span[contains(@data-bind,'translated')]")).Text.Trim());
        }

        /// <summary>
        /// Возвращает из профиля пользователя рейтинг пользователя
        /// </summary>
        /// <returns></returns>
        protected Decimal GetUserRating()
        {
            return Decimal.Parse(
                _driver.FindElement(By.XPath(".//div[contains(@class,'profile-description')]//span[contains(@data-bind,'rating')]")).Text.Trim().Replace(".", ","));
        }

        /// <summary>
        /// Возвращает из профиля пользователя количество голосов За и Против
        /// </summary>
        /// <param name="numVotesUp">OUT: количество голосов За</param>
        /// <param name="numVotesDown">OUT: количество голосов против</param>
        protected void GetUserVotesInfo(out int numVotesUp, out int numVotesDown)
        {
            // Получить количество голосов ЗА
            numVotesUp = int.Parse(
                _driver.FindElement(By.XPath(".//div[contains(@class,'profile-description')]//div[contains(@data-bind,'votesUp')]")).Text.Trim());
            // Получить количество голосов Против
            numVotesDown = int.Parse(
                _driver.FindElement(By.XPath(".//div[contains(@class,'profile-description')]//div[contains(@data-bind,'votesDown')]")).Text.Trim());
        }

        /// <summary>
        /// Вернуть из профиля пользователя номер пользователя в общем зачете
        /// </summary>
        /// <returns></returns>
        protected int GetUserPosition()
        {
            // Получить место пользователя в общем зачете
            return int.Parse(_driver.FindElement(By.XPath(".//span[contains(@data-bind,'position')]")).Text.Trim());
        }

        /// <summary>
        /// Вернуть, есть ли пользователь в списке лидерборда в активном списке
        /// (без учета, что пользователь может отображаться до или после списка из 10 человек - активный список из ссылок)
        /// </summary>
        /// <param name="userName">имя пользователя</param>
        /// <returns>есть пользователь</returns>
        protected bool GetIsUserLeaderboardActiveList(string userName)
        {
            return IsElementPresent(By.XPath(".//tr[not(contains(@style,'display: none;'))]//td[3]/a[contains(text(),'" + userName + "')]"));
        }

        /// <summary>
        /// Есть ли элемент
        /// </summary>
        /// <param name="by">By локатор элемента</param>
        /// <returns>элемент есть</returns>
        protected bool IsElementPresent(By by)
        {
            try
            {
                _driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка на наличие элемента на экране
        /// </summary>
        protected bool IsElementDisplayed(By by)
        {
            try
            {
                return Driver.FindElement(by).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Дождаться, пока элемент пропадет
        /// </summary>
        /// <param name="xPath">xPath элемента</param>
        /// <param name="maxWaitSeconds">максимальное время ожидание пропадания элемента</param>
        /// <returns>элемент пропал</returns>
        protected bool WaitUntilDisappearElement(string xPath, int maxWaitSeconds = 10)
        {
            bool isDisplayed = false;
            TimeSpan timeBegin = DateTime.Now.TimeOfDay;
            setDriverTimeoutMinimum();
            do
            {
                // Элемент отображается?
                isDisplayed = IsElementDisplayed(By.XPath(xPath));
                if (!isDisplayed || DateTime.Now.TimeOfDay.Subtract(timeBegin).Seconds > maxWaitSeconds)
                {
                    // Пропал или время истекло
                    break;
                }
                Thread.Sleep(500);
            } while (isDisplayed);
            setDriverTimeoutDefault();
            return !isDisplayed;
        }

        /// <summary>
        /// Проголосовать и вернуть информацию из профиля пользователя
        /// </summary>
        /// <param name="fromEditor">голосование из редактора или из списка последних событий (true: редактор, false: список последних событий)</param>
        /// <param name="isVoteUp">голосование за или против (true: за, false: против)</param>
        /// <param name="userRatingBefore">OUT: рейтинг пользователя до голосования</param>
        /// <param name="userRatingAfter">OUT: рейтинг пользователя после голосования</param>
        /// <param name="numVotesUpBefore">OUT: количество голосов За до голосования</param>
        /// <param name="numVotesUpAfter">OUT: количество голосов За после голосования</param>
        /// <param name="numVotesDownBefore">OUT: количество голосов Против до голосования</param>
        /// <param name="numVotesDownAfter">OUT: количество голосов Против после голосования</param>
        /// <param name="numTranslatedBefore">OUT: количество переведенных предложений до голосования</param>
        /// <param name="numTranslatedAfter">OUT: количество переведенных предложений после голосования</param>
        protected void VoteCheckUserProfile(bool fromEditor, bool isVoteUp, out Decimal userRatingBefore, out Decimal userRatingAfter,
                                out int numVotesUpBefore, out int numVotesUpAfter,
                                out int numVotesDownBefore, out int numVotesDownAfter,
                                out int numTranslatedBefore, out int numTranslatedAfter)
        {
            // Добавить перевод
            string translationText = "Example Translation " + DateTime.Now.Ticks;
            string courseName;
            int lectureRowNumber, translationRowNum;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
            // Выйти из редактора
            ClickBackEditor();

            // Открыть профиль пользователя
            OpenUserProfileFromCourse();
            // Получить информацию о пользователе
            GetUserInfo(out numTranslatedBefore, out userRatingBefore, out numVotesUpBefore, out numVotesDownBefore);

            // Выйти из этого пользователя
            LogoutUser();
            // Зайти другим пользователем
            LoginUser(User2);

            if (fromEditor)
            {
                // Зайти в курс
                OpenCourseByName(courseName);
                // Зайти в лекцию
                OpenLectureByRowNum(lectureRowNumber);
                // Перейти в строку с добавленным переводом
                ClickEditorRowByNum(translationRowNum);
                // Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
                int rowNumber = GetSuggestedTranslationRowNum(translationText);

                // Проголосовать
                VoteFromEditor(isVoteUp, rowNumber);
                // Выйти из редактора
                ClickBackEditor();
            }
            else
            {
                // Проголосовать
                Assert.True(VoteEventListByRowNum(GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent), isVoteUp),
                    "Ошибка: не удалось проголосовать");
                OpenCoursePage();
            }

            // Выйти из этого пользователя
            LogoutUser();
            // Зайти первым пользователем
            LoginUser(User1);
            Thread.Sleep(10000);
            // Открыть профиль пользователя
            OpenUserProfileFromCourse();

            
            // Получить информацию о пользователе
            GetUserInfo(out numTranslatedAfter, out userRatingAfter, out numVotesUpAfter, out numVotesDownAfter);
        }

        /// <summary>
        /// Заполнить форму загрузки документа
        /// </summary>
        /// <param name="DocumentName"></param>
        protected void FillAddDocumentForm(string DocumentName)
        {
            Thread.Sleep(3000);
            // Заполнить форму для отправки файла
            SendKeys.SendWait(DocumentName);
            Thread.Sleep(2000);
            SendKeys.SendWait(@"{Tab}");
            Thread.Sleep(500);
            SendKeys.SendWait(@"{Tab}");
            Thread.Sleep(500);
            SendKeys.SendWait(@"{Enter}");
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Проверить, что Confirm прошел
        /// </summary>
        /// <param name="rowNumber"></param>
        protected void AssertConfirmIsDone(int rowNumber)
        {
            // Проверка, что около ячейки с переводом есть галочка
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[@id='segments-body']//table//tr[" + rowNumber + "]//td[5]//span[contains(@class,'fa-check')]")),
                "Ошибка: около ячейки не появилось галочки");
            // Проверка, что около ячейки с переводом галочка не в рамке (то есть подтверждение прошло)
            Assert.IsTrue(WaitUntilDisappearElement(
                ".//div[@id='segments-body']//table//tr[" + rowNumber + "]//td[5]//span[contains(@class,'fa-border')]", 25),
                "Ошибка: рамка вокруг галочки не пропадает - Confirm не прошел");
        }

        /// <summary>
        /// Получить, учтен ли голос в Editor
        /// </summary>
        /// <param name="isVoteUp">тип голоса</param>
        /// <param name="rowNumber">номер строки</param>
        /// <returns>голос учтен</returns>
        protected bool GetIsVoteConsideredEditor(bool isVoteUp, int rowNumber)
        {
            string voteIcon = isVoteUp ? "fa-thumbs-up" : "fa-thumbs-down";
            return IsElementPresent(By.XPath(
                    ".//div[@id='translations-body']//table//tr["
                    + rowNumber + "]//td[5]/div//span[contains(@class,'"
                    + voteIcon + "')][contains(@class,'disabled')]"));
        }

        /// <summary>
        /// Получить имя пользователя с главной страницы
        /// </summary>
        /// <returns>имя пользователя</returns>
        protected string GetUserNameHomepage()
        {
            return Driver.FindElement(By.XPath(".//a[contains(@class,'user-link')]")).Text;
        }

        /// <summary>
        /// Прокрутить лидерборд до позиции пользователя
        /// </summary>
        /// <returns>номер строки с пользователем</returns>
        protected int ScrollLeaderboardToUser(string userName)
        {
            // Открыть профиль пользователя
            OpenUserProfileFromHomePage();
            // Получить место пользователя в профиле
            int userPosition = GetUserPosition();

            // Перейти к списку лидеров
            OpenLeaderboardPage();

            // Рассчитать, сколько страниц лидеров надо прокрутить
            int numToScroll = (userPosition % 10 == 0) ? (userPosition / 10 - 1) : (userPosition / 10);
            string currentPageRange = "";
            for (int i = 0; i < numToScroll; ++i)
            {
                currentPageRange = Driver.FindElement(By.XPath(".//div[contains(@data-bind,'currentPageRange')]")).Text;
                // Нажать на Следующую страницу
                Driver.FindElement(By.XPath(".//a[contains(@data-bind,'nextPage')]")).Click();
                // Дождаться обновления списка
                Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@data-bind,'currentPageRange')][not(contains(text(),'" + currentPageRange + "'))]")));
            }

            // Получить номер в новом списке
            int numInList = userPosition % 10;
            if (numInList == 0)
            {
                numInList = 10;
            }
            // В списке есть лишние tr, пропускаем их
            numInList += 3;

            return numInList;
        }

        /// <summary>
        /// Получить личный прогресс для лекции
        /// </summary>
        /// <param name="lectureRowNumber">номер строки с лекцией</param>
        /// <returns>личный прогресс</returns>
        protected int GetPersonalProgress(int lectureRowNumber)
        {
            return int.Parse(Driver.FindElement(By.XPath(
                ".//tbody[contains(@data-bind,'lectures')]//tr[" + lectureRowNumber + "]//div[contains(@data-bind,'personalProgressView')]")).Text.Replace("%", "").Trim());
        }

        DateTime testBeginTime;

        [SetUp]
        public void Setup()
        {
            testBeginTime = DateTime.Now;
            Console.WriteLine(TestContext.CurrentContext.Test.Name + "\nStart: " + testBeginTime.ToString());

            if (_driver == null)
            {
                // Если конструктор заново не вызывался, то надо заполнить _driver
                CreateDriver();
            }

            _driver.Navigate().GoToUrl(_url);
            // Зайти под первым пользователем
            LoginUser(_user1);
        }

        [TearDown]
        public void Teardown()
        {
            // Если тест провалился
            //if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
            {
                // Сделать скриншот
                ITakesScreenshot screenshotDriver = _driver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();

                // Создать папку для скриншотов провалившихся тестов
                string failResultPath = System.IO.Path.Combine(PathTestResults, "FailedTests");
                System.IO.Directory.CreateDirectory(failResultPath);
                // Создать имя скриншота по имени теста
                string screenName = TestContext.CurrentContext.Test.Name;
                if (screenName.Contains("("))
                {
                    // Убрать из названия теста аргументы (файлы)
                    screenName = screenName.Substring(0, screenName.IndexOf("("));
                }
                screenName += DateTime.Now.Ticks.ToString() + ".png";
                // Создать полное имя файла
                screenName = System.IO.Path.Combine(failResultPath, screenName);
                // Сохранить скриншот
                screenshot.SaveAsFile(screenName, ImageFormat.Png);
            }

            // Закрыть драйвер
            _driver.Quit();
            // Очистить, чтобы при следующем тесте пересоздавалось
            _driver = null;

            DateTime testFinishTime = DateTime.Now;
            Console.WriteLine("Finish: " + testFinishTime.ToString());
            TimeSpan duration = TimeSpan.FromTicks(testFinishTime.Ticks - testBeginTime.Ticks);
            string durResult = "Duration: ";
            if (duration.TotalMinutes > 1)
            {
                durResult += duration.TotalMinutes + "min";
            }
            else
            {
                durResult += duration.TotalSeconds + "sec";
            }

            durResult += " (" + duration.TotalMilliseconds + "ms).";
            Console.WriteLine(durResult);

            if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
            {
                Console.WriteLine("Fail!");
            }
        }
    }
}
