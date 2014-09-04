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
using System.Text.RegularExpressions;

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
		/// Страница курсов
		/// </summary>
		private CoursePageHelper _coursePageHelper;
		protected CoursePageHelper CoursePage
		{
			get
			{
				return _coursePageHelper;
			}
		}

		/// <summary>
		/// Страница редактора
		/// </summary>
		private EditorPageHelper _editorPageHelper;
		protected EditorPageHelper EditorPage
		{
			get
			{
				return _editorPageHelper;
			}
		}

		/// <summary>
		/// Шапка страницы
		/// </summary>
		private HeaderHelper _headerHelper;
		protected HeaderHelper Header
		{
			get
			{
				return _headerHelper;
			}
		}

		/// <summary>
		/// Стартовая страница
		/// </summary>
		private HomePageHelper _homePageHelper;
		protected HomePageHelper HomePage
		{
			get
			{
				return _homePageHelper;
			}
		}

		/// <summary>
		/// Страница лидеров
		/// </summary>
		private LeaderboardPageHelper _leaderboardPageHelper;
		protected LeaderboardPageHelper LeaderboardPage
		{
			get
			{
				return _leaderboardPageHelper;
			}
		}
		
		/// <summary>
		/// Страница лекций
		/// </summary>
		private LecturePageHelper _lecturePageHelper;
		protected LecturePageHelper LecturePage
		{
			get
			{
				return _lecturePageHelper;
			}
		}

		/// <summary>
		/// Страница профиля пользователя
		/// </summary>
		private ProfilePageHelper _profilePageHelper;
		protected ProfilePageHelper ProfilePage
		{
			get
			{
				return _profilePageHelper;
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
                    //_profile.SetPreference("browser.download.manager.showWhenStarting", false);
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

			RecreateDrivers();
        }

		/// <summary>
        /// Пересоздание Helper'ов с новыми Driver, Wait
        /// </summary>
		private void RecreateDrivers()
		{
			_coursePageHelper = new CoursePageHelper(Driver, Wait);
			_editorPageHelper = new EditorPageHelper(Driver, Wait);
			_headerHelper = new HeaderHelper(Driver, Wait);
			_homePageHelper = new HomePageHelper(Driver, Wait);
			_leaderboardPageHelper = new LeaderboardPageHelper(Driver, Wait);
			_lecturePageHelper = new LecturePageHelper(Driver, Wait);
			_profilePageHelper = new ProfilePageHelper(Driver, Wait);
		}

        /// <summary>
        /// Устанавливаем ожидание для драйвера в минимум (1 секунда)
        /// </summary>
        protected void setDriverTimeoutMinimum()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Устанавливаем ожидание для драйвера для ожидания (2 секунды)
        /// </summary>
        protected void setDriverTimeoutWait()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
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
        /// <param name="userInfo">Пользователь</param>
        protected void LoginUser(UserInfo userInfo)
        {
			Header.Login(userInfo.login, userInfo.password);
			// Проверить, что не выбросило на форму регистрации
			Assert.IsFalse(Header.GetIsRegistrationFormDisplay(), "Ошибка: При попытке входа открылась форма регистрации.");
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
		} // Удалить после удаления LoginUser

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
        } // Удалить после удаления LoginUser

		/// <summary>
		/// Разлогиниться из этого пользователя
		/// </summary>
		protected void LogoutUser()
		{
			// Выйти из под этого пользователя
			_driver.FindElement(By.XPath(".//a[contains(@data-bind,'logout')]")).Click();
		} // Используется только с ачивками. Заменить на Header.LogoutUser();

		/// <summary>
		/// Перейти на главную страницу
		/// </summary>
		protected void OpenHomepage()
		{
			// Кликнуть по переходу на главную страницу
			_driver.FindElement(By.ClassName("on-homepage")).Click();
			// Дождаться открытия главной страницы
			_wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'logo')]")));
		} // Используется только с ачивками. Заменить на Header.OpenHomepage();

		/// <summary>
		/// Открыть страницу с лидербордом
		/// </summary>
		protected void OpenLeaderboardPage()
		{
			_driver.FindElement(By.XPath(".//a[contains(@href,'/Leaderboard')]")).Click();
			_wait.Until((d) => d.FindElement(By.ClassName("leaders")).Displayed);
		} // Используется только с ачивками. Заменить на Header.OpenLeaderboardPage();

		/// <summary>
		/// Кликнуть по Target в строке в Editor
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		protected void ClickEditorRowByNum(int rowNumber)
		{
			_driver.FindElement(By.XPath(SelectByUrl(".//div[@id='segments']//table//tr[" + rowNumber + "]//td[4]//div",
														".//div[@id='segments']//table[" + rowNumber + "]//td[4]//div"))).Click();
		} // Используется только с ачивками. Заменить на EditorPage.ClickTarget(rowNumber);

		/// <summary>
		/// Получить имя пользователя с главной страницы
		/// </summary>
		/// <returns>имя пользователя</returns>
		protected string GetUserNameHomepage()
		{
			return Driver.FindElement(By.XPath(".//a[contains(@class,'user-link')]")).Text;
		} // Используется только с ачивками. Заменить на HomePage.GetUserName();

		/// <summary>
		/// Получить, учтен ли голос в Editor
		/// </summary>
		/// <param name="isVoteUp">тип голоса</param>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>голос учтен</returns>
		protected bool GetIsVoteConsideredEditor(bool isVoteUp, int rowNumber)
		{
			string voteIcon = isVoteUp ? "fa-thumbs-up" : "fa-thumbs-down";
			return IsElementPresent(By.XPath(SelectByUrl(
					".//div[@id='translations-body']//table//tr[" + rowNumber + "]//td[5]/div//span[contains(@class,'" + voteIcon + "')][contains(@class,'disabled')]",
					".//div[@id='translations-body']//table[" + rowNumber + "]//td[5]/div//span[contains(@class,'" + voteIcon + "')][contains(@class,'disabled')]")));
		} // Используется только с ачивками. Заменить на EditorPage.GetIsVoteConsidered(true, (i + 1)));

		/// <summary>
		/// Возвращает из профиля пользователя рейтинг пользователя
		/// </summary>
		/// <returns></returns>
		protected Decimal GetUserRating()
		{
			return Decimal.Parse(
				_driver.FindElement(By.XPath(".//div[contains(@class,'profile-description')]//span[contains(@data-bind,'rating')]")).Text.Trim().Replace(".", ","));
		} // Используется только с ачивками. Заменить на ProfilePage.GetUserRating();

		/// <summary>
		/// Получить номер строки перевода в предложенных переводах
		/// </summary>
		/// <param name="translationText">перевод</param>
		/// <returns>номер строки</returns>
		protected int GetSuggestedTranslationRowNum(string translationText)
		{
			int rowNumber = 0;
			IList<IWebElement> translationsList = _driver.FindElements(By.XPath(".//div[@id='translations-body']//table//tr//td[3]/div"));
			for (int i = 0; i < translationsList.Count; ++i)
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
		} // Используется только с ачивками. Заменить на EditorPage.GetTranslationRowNumberByTarget(translationText);

		/// <summary>
		/// Вернуть из профиля пользователя номер пользователя в общем зачете
		/// </summary>
		/// <returns></returns>
		protected int GetUserPosition()
		{
			// Получить место пользователя в общем зачете
			return int.Parse(_driver.FindElement(By.XPath(".//span[contains(@data-bind,'position')]")).Text.Trim());
		} // Используется только с ачивками. Заменить на ProfilePage.GetPosition();

		/// <summary>
		/// Вернуть, есть ли пользователь в списке лидерборда в активном списке
		/// (без учета, что пользователь может отображаться до или после списка из 10 человек - активный список из ссылок)
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <returns>есть пользователь</returns>
		protected bool GetIsUserLeaderboardActiveList(string userName)
		{
			return IsElementPresent(By.XPath(".//tr[not(contains(@style,'display: none;'))]//td[3]/a[contains(text(),'" + userName + "')]"));
		} // Используется только с ачивками. Заменить на LeaderboardPage.GetIsUserLeaderboardActiveList(userName);

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
		} // Удалить после устранения всех связей

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
		}  // Удалить после устранения всех связей

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
		}  // Удалить после устранения всех связей

		/// <summary>
		/// Возвращает строку соответствующую действительному адресу (Stage2 или Stage3)
		/// </summary>
		/// <param name="parametrStage2">Строка соответсвующая Stage2</param>
		/// <param name="parametrStage3">Строка соответсвующая Stage2</param>
		/// <returns></returns>
		protected string SelectByUrl(string parametrStage2, string parametrStage3)
		{
			if (Driver.Url.Contains("stage2"))
				return parametrStage2;
			else if (Driver.Url.Contains("stage3"))
				return parametrStage3;

			return "";
		} // Удалить после устранения всех связей

		/// <summary>
		/// В редакторе кликает Back для выхода из него
		/// </summary>
		protected void ClickBackEditor()
		{
			// Back
			EditorPage.ClickBackBtn();
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");
		}

		/// <summary>
		/// Переход в лекцию по номеру строки
		/// </summary>
		/// <param name="rowNumber">номер строки с лекцией</param>
		protected void OpenLectureByRowNum(int rowNumber)
		{
			// Перейти в лекцию
			LecturePage.ClickLectureByRowNum(rowNumber);

			// Ждем перехода в лекцию
			EditorPage.DisplayFirstSegment();
		}

		/// <summary>
		/// Переход в курс по имени курса
		/// </summary>
		protected void OpenCourseByName(string courseName)
		{
			// Перейти в курс
			CoursePage.OpenCourseByName(courseName);
			// Дождаться перехода в курс
			LecturePage.WaitUntilDisplayLecturesList();
		}

		/// <summary>
		/// Открываем страницу курсов
		/// Ждем пока загрузится список курсов
		/// </summary>
		protected bool OpenCoursePage()
		{
			// Открыть страницу курсов
			Header.ClickOpenCoursePage();
			// Дождаться перехода в курс
			return CoursePage.WaitUntilCourseListDisplay();
		} // В Ачивках использовать вот так: Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");

		/// <summary>
		/// Открыть другой курс (имя которого отличается от указанного и не содержит TestProject)
		/// </summary>
		/// <param name="courseNameNotToOpen">имя курса, который не открывать</param>
		/// <returns>имя курса, который открыли</returns>
		protected string OpenAnotherCourse(string courseNameNotToOpen)
		{
			string openCourseName = "";
			// Список курсов
			List<string> courseList = CoursePage.GetCoursesNameList();
			foreach (string el in courseList)
			{
				// Найти курс, название которого отличается от переданного и не содержит TestProject
				if (el != courseNameNotToOpen &&
					!el.Contains("TestProject"))
				{
					openCourseName = el;
					OpenCourseByName(openCourseName);
					break;
				}
			}
			// имя курса
			return openCourseName;
		}

		/// <summary>
		/// Поиск курса с наименьшим прогрессом
		/// </summary>
		/// <returns>возвращает имя курса</returns>
		protected string SelectCourseMinProgress(out Decimal courseProgress)
		{
			Decimal minProgress = 100;
			string courseMinProgress = "";
			// Список курсов для "правого" столбика
			foreach (string course in CoursePage.GetCoursesNameList())
			{
				if (!course.Contains("TestProject"))
				{
					Decimal curProgress = CoursePage.GetCourseProcentByName(course);
					if (curProgress < minProgress)
					{
						// минимальный прогресс
						minProgress = curProgress;
						courseMinProgress = course;
					}
				}
			}

			courseProgress = minProgress;
			return courseMinProgress;
		}

		/// <summary>
		/// Поиск курса с максимальным прогрессом
		/// </summary>
		/// <returns>возвращает имя курса</returns>
		protected string SelectCourseMaxProgress(out Decimal courseProgress)
		{
			Decimal maxProgress = 100;
			string courseMaxProgress = "";
			// Список курсов для "правого" столбика
			foreach (string course in CoursePage.GetCoursesNameList())
			{
				if (!course.Contains("TestProject"))
				{
					Decimal curProgress = CoursePage.GetCourseProcentByName(course);
					if (curProgress > maxProgress)
					{
						// максимальный прогресс
						maxProgress = curProgress;
						courseMaxProgress = course;
					}
				}
			}

			courseProgress = maxProgress;
			return courseMaxProgress;
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
		/// Выбор лекции (с общим и личным прогрессом меньше 100)
		/// </summary>
		/// <returns>номер строки с лекцией</returns>
		protected int SelectLectureGetRowNumber()
		{
			List<string> lectureList = LecturePage.GetLecturesNameList();
			// Проверить, что список лекций не пуст
			Assert.IsTrue(lectureList.Count > 0, "Ошибка: список лекций пуст");

			bool isLectureExist = false;
			int fullProgress = 0, personalProgress = 0;
			int rowNumber = 0;

			for (int i = 0; i < lectureList.Count; ++i)
			{
				personalProgress = LecturePage.GetLecturePersonalProgressByNumber(i + 1);
				fullProgress = LecturePage.GetLectureGeneralProgressByNumber(i + 1);
				if (personalProgress < 100 && fullProgress < 100)
				{
					isLectureExist = true;
					// нумерация строк с 1, а цикл с 0
					rowNumber = i + 1;
					break;
				}

			}

			Assert.IsTrue(isLectureExist, "Ошибка: нет подходящей лекции (с общим и личным прогрессом меньше 100 ");

			// Вернуть номер строки с лекцией
			return rowNumber;
		}

		/// <summary>
		/// Получить: есть ли мой перевод для данного сегмента
		/// </summary>
		/// <returns>перевод пользователя есть</returns>
		protected bool GetIsExistMyTranslationSegment()
		{
			setDriverTimeoutMinimum();
			// Проверить, есть ли среди предложенных переводов - мой перевод (есть ли корзинка!)
			bool isExistUserTranslation = EditorPage.GetIsUserTranslationExist();
			setDriverTimeoutDefault();
			// Вернуть: перевод есть
			return isExistUserTranslation;
		}

		/// <summary>
		/// Удалить переводы
		/// </summary>
		/// <param name="begin">номер строки, начиная с которого удалить переводы</param>
		/// <param name="count">количество удаляемых переводов</param>
		protected void DeleteMyTranslations(int begin, int count)
		{
			IList<IWebElement> list = EditorPage.GetTagetsList();
			int deletedNumber = 0;
			int startIndex = begin > 0 ? (begin - 1) : 0;
			for (int i = startIndex; i < list.Count; ++i)
			{
				// Кликнуть по сегменту в нужной строке
				list[i].Click();
				if (GetIsExistMyTranslationSegment())
				{
					// Удалить перевод из предложенных переводов
					EditorPage.DeleteTranslation();
					++deletedNumber;
					if (deletedNumber >= count)
					{
						break;
					}
				}
			}
		}

		/// <summary>
		/// Очистить лекцию
		/// </summary>
		protected void ClearLecture()
		{
			IList<IWebElement> list = EditorPage.GetTagetsList(); ;
			int countClear = list.Count > maxEditorLinesNum ? maxEditorLinesNum : list.Count;
			// Удалить с 1 сегмента
			DeleteMyTranslations(1, countClear);
		}

		/// <summary>
		/// Получить номер строки в Editor с пустым Target
		/// </summary>
		/// <returns></returns>
		private int GetEmptyRowNumberEditor()
		{
			// Список Target
			IList<IWebElement> sentenceList = EditorPage.GetTagetsList();
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
		/// Проверить, что Confirm прошел
		/// </summary>
		/// <param name="rowNumber"></param>
		protected void AssertConfirmIsDone(int rowNumber)
		{
			// Проверка, что около ячейки с переводом есть галочка
			Assert.IsTrue(EditorPage.GetIsCheckPresent(rowNumber), "Ошибка: около ячейки не появилось галочки");

			// Проверка, что около ячейки с переводом галочка не в рамке (то есть подтверждение прошло)
			Assert.IsTrue(EditorPage.WaitUntilDisappearBorderByRowNumber(rowNumber), "Ошибка: рамка вокруг галочки не пропадает - Confirm не прошел");
		}

		/// <summary>
		/// Добавить переводв строку
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <param name="targetText">текст перевода</param>
		protected void AddTranslationByRowNum(int rowNumber, string targetText)
		{
			EditorPage.AddTextTargetByRowNumber(rowNumber, targetText);
			EditorPage.ClickConfirmBtn();
			// Дождаться Confirm
			AssertConfirmIsDone(rowNumber);
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
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Переход в курс с наименьшим прогрессом
			courseName = OpenCourseMinProgress();
			// Перейти в лекцию
			lectureRowNumber = OpenLectureNotFilled();
			// Найти предложение без перевода
			translationRowNum = GetEmptyTranslationRowNumber();
			// Добавить перевод
			AddTranslationByRowNum(translationRowNum, translationText);
			// Вернуться в строку
			EditorPage.ClickTargetByRowNumber(translationRowNum);
		}

		/// <summary>
		/// Заполнить форму загрузки документа
		/// </summary>
		/// <param name="DocumentName"></param>
		protected void FillAddDocumentForm(string DocumentName)
		{
			Thread.Sleep(3000);
			// Заполнить форму для отправки файла
			string txt = Regex.Replace(DocumentName, "[+^%~()]", "{$0}");
			SendKeys.SendWait(txt);
			Thread.Sleep(2000);
			SendKeys.SendWait(@"{Tab}");
			Thread.Sleep(500);
			SendKeys.SendWait(@"{Tab}");
			Thread.Sleep(500);
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(1000);
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
				isPresent = HomePage.GetIsEventPresentByTarget(targetText, ConvertLastEventTypeToString(evType));
				// Проверить, что условие поиска выполнено или превышен лимит времени
				isNeedStopWait = (!waitAppearEvent ? !isPresent : isPresent) || DateTime.Now.TimeOfDay.Subtract(timeBegin).Seconds > maxDurationWaitEventList;
				if (isNeedStopWait)
				{
					break;
				}
				// Ожидание
				Thread.Sleep(5000);
				// Обновление страницы
				_driver.FindElement(By.XPath(".//div")).SendKeys(OpenQA.Selenium.Keys.F5);
			} while (!isNeedStopWait);
			setDriverTimeoutDefault();
			// Вернуть, есть ли событие в списке
			return isPresent;
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

			// Получаем номер строки нужного перевода 
			return HomePage.GetEventRowNumberByTarget(targetText, ConvertLastEventTypeToString(evType));
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
				string authorName = HomePage.GetEventAuthorByRowNumber(rowNumber);
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
		/// В редакторе проголосовать За/Против перевод/а
		/// </summary>
		/// <param name="isVoteUp">тип голоса</param>
		/// <param name="rowNumber">номер строки в списке предложенных переводов</param>
		protected void VoteFromEditor(bool isVoteUp, int rowNumber)
		{
			// Получить время создания перевода
			string time = EditorPage.GetTranslationTimeByRowNumber(rowNumber);

			// Проголосовать
			EditorPage.VoteByRowNumber(isVoteUp, rowNumber);
			Thread.Sleep(1000);

			// Получаем новый номер строки для перевода
			int rowNumberNew = EditorPage.GetTranslationRowNumberByTime(time);
			
			// Првоеряем, что голос принят
			Assert.IsTrue(EditorPage.GetIsVoteConsidered(isVoteUp, rowNumberNew), "Ошибка: голос не принят");
		}

		/// <summary>
		/// Перейти на страницу профиля пользователя с главной страницы
		/// </summary>
		protected void OpenUserProfileFromHomePage()
		{
			// Перейти на страницу профиля пользователя
			HomePage.OpenProfile();
			// Дождаться загрузки страницы
			Assert.IsTrue(ProfilePage.WaitUntilDisplayProfile(), "Ошибка: Страница профиля не открылась.");
		}

		/// <summary>
		/// Перейти на страницу профиля пользователя со страницы курсов
		/// </summary>
		protected void OpenUserProfileFromCourse()
		{
			// Перейти на страницу профиля пользователя
			Header.OpenProfile();
			// Дождаться загрузки страницы
			Assert.IsTrue(ProfilePage.WaitUntilDisplayProfile(), "Ошибка: Страница профиля не открылась.");
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
		/// Возвращает из профиля пользователя количество голосов За и Против
		/// </summary>
		/// <param name="numVotesUp">OUT: количество голосов За</param>
		/// <param name="numVotesDown">OUT: количество голосов против</param>
		protected void GetUserVotesInfo(out int numVotesUp, out int numVotesDown)
		{
			// Получить количество голосов ЗА
			numVotesUp = ProfilePage.GetVotesUp();
			// Получить количество голосов Против
			numVotesDown = ProfilePage.GetVotesDown();
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
			numTranslated = ProfilePage.GetUserTranslationsNumber();
			userRating = ProfilePage.GetUserRating();
			GetUserVotesInfo(out numVotesUp, out numVotesDown);
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
			string translationText = "Test" + DateTime.Now.Ticks;
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
			Header.LogoutUser();
			// Зайти другим пользователем
			LoginUser(User2);

			if (fromEditor)
			{
				OpenCoursePage();
				// Зайти в курс
				OpenCourseByName(courseName);
				// Зайти в лекцию
				OpenLectureByRowNum(lectureRowNumber);
				// Перейти в строку с добавленным переводом
				EditorPage.ClickTargetByRowNumber(translationRowNum);
				// Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
				int rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);

				// Проголосовать
				VoteFromEditor(isVoteUp, rowNumber);
				// Выйти из редактора
				ClickBackEditor();
			}
			else
			{
				// Проголосовать
				Assert.True(HomePage.GetIsCanVoteEventListByRowNumber(GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent), isVoteUp),
					"Ошибка: не удалось проголосовать");
				Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			}

			// Выйти из этого пользователя
			Header.LogoutUser();
			// Зайти первым пользователем
			LoginUser(User1);
			Thread.Sleep(10000);
			// Открыть профиль пользователя
			OpenUserProfileFromCourse();


			// Получить информацию о пользователе
			GetUserInfo(out numTranslatedAfter, out userRatingAfter, out numVotesUpAfter, out numVotesDownAfter);
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
			setDriverTimeoutMinimum();

			// Проверить событие с таким Target
			if (HomePage.GetIsEventPresentByTarget(targetText))
			{
				// Все события
				List<string> targetList = HomePage.GetEventTargetList();
				for (int i = 0; i < targetList.Count; ++i)
				{
					// Событие с таким Target
					if (targetList[i].Contains(targetText))
					{
						// Тип события
						HomePageLastEventType curEventType = ConvertLastEventFromString(HomePage.GetEventActivityByRowNumber(i + 1));
						if (curEventType != evType)
						{
							continue;
						}

						// Проверить автора
						string curAuthor = HomePage.GetEventAuthorByRowNumber(i + 1);
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
		/// Получить инорфмацию о самоми последнем (самом верхнем/свежем) событии в списке последних событий
		/// </summary>
		/// <param name="targetText">OUT: текст в Target последнего события</param>
		/// <param name="action">OUT: тип последнего события</param>
		/// <param name="author">OUT: автор последнего события</param>
		protected void GetLastEventInfo(out String targetText, out HomePageLastEventType action, out String author)
		{
			// вернуть текст в Target последнего события
			targetText = HomePage.GetEventTargetByRowNumber(1);
			// вернуть тип последнего события
			action = ConvertLastEventFromString(HomePage.GetEventActivityByRowNumber(1));
			// вернуть автора последнего события
			author = HomePage.GetEventAuthorByRowNumber(1);
		}

		/// <summary>
		/// Выбор лекции (с общим и личным прогрессом == 0)
		/// </summary>
		/// <returns>номер строки с лекцией</returns>
		protected int SelectEmptyLectureGetRowNumber()
		{
			// Получить все лекции ккурса
			List<string> lectureList = LecturePage.GetLecturesNameList();
			// Проверить, что список лекций не пуст
			Assert.IsTrue(lectureList.Count > 0, "Ошибка: список лекций пуст");

			bool isLectureExist = false;
			int fullProgress = 0, personalProgress = 0;
			int rowNumber = 0;
			for (int i = 0; i < lectureList.Count; ++i)
			{
				// Выбираем лекцию с личным и общим прогрессом == 0
				personalProgress = LecturePage.GetLecturePersonalProgressByNumber(i + 1);
				if (personalProgress == 0)
				{
					fullProgress = LecturePage.GetLectureGeneralProgressByNumber(i + 1);
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
		/// Удалить переводы
		/// </summary>
		/// <param name="rowList">список номеров строк для удаления переводов</param>
		protected void DeleteMyTranslations(List<int> rowList)
		{
			foreach (int i in rowList)
			{
				// Кликнуть по сегменту в нужной строке
				EditorPage.ClickTargetByRowNumber(i);

				if (GetIsExistMyTranslationSegment())
				{
					// Удалить перевод из предложенных переводов
					EditorPage.DeleteTranslation();
				}
			}
		}

		/// <summary>
		/// Получить список видимых сегментов
		/// </summary>
		/// <param name="lastLastFactRow">IN/OUT: фактический номер последней видимой строки</param>
		/// <param name="isLectureFinished">OUT: лекция закончилась</param>
		/// <param name="startIndex">OUT: номер первой строки для заполнения</param>
		/// <returns>список сегментов</returns>
		protected IList<IWebElement> GetVisibleSegmentList(ref int lastLastFactRow, out bool isLectureFinished, out int startIndex)
		{
			// README : если нужно понять, зачем такой странный алгоритм и к чему непонятные переменные:
			// прочитать описание ниже.

			// При входе в редактор Selenium видит только 34-35 сегментов.
			// Сегменты могут начинаться не с первого:
			// если до этого заходили в редактор и изменяли что-то в какой-то, например, 10 строке,
			// то при следующем входе, курсор будет в 20 строке,
			// а Selenium будет видеть с 3 строки по 38 (например),
			// т.е. при обращении к первой строке, он будет обращаться к фактической 3 строке.
			// Фактический номер строки - тот, который написан в первом столбце.
			// При этом, когда мы обращаемся к какой-то строке по номеру tr:nth-child(N),
			// может произойти ошибка, т.к. Selenium смещает свой видимый список по мере того, как мы заполняем предложения.
			// Т.е. в следующий раз при обращении к первой строке, он уже будет обращаться к 4ой фактической строке (а видеть с 4 по 39).

			// Поэтому беру список видимых строк.
			// Получаю фактический номер первой строки и фактический номер последней строки.
			// (К примеру 1 и 34, соответственно). lastFirstRow = 1, lastLastRow = 34
			// Заполняю все эти видимые строки.
			// Обновляю список видимых сегментов.
			// Снова получаю фактический номер первой и последней строк.
			// (К примеру, 15 и 50, соответственно). curFirstRow = 15, curLastRow = 50
			// Нужно заполнить фактическую 35 строку, но для селениума она сейчас 21я.
			// Чтобы не заполнять с 15 по 34 фактические строки повторно, приходится учитывать предыдущее значение 34,
			// учитывать текущее первое значение 15:
			// фактическая 15 строка - селениум 1 строка
			// фактическиая 35 (34 последняя, нужна следующая - 35) - селениум ? строка
			// => ? = 35 - 15 + 1 = 34 + 1 - 15 + 1
			// => ? = lastLastRow + 1 - curFirstRow + 1
			// А цикл начинается с 0 (номер строки - 1), поэтому start = lastLastRow - curFirstRow + 1


			// Список видимых сегментов
			IList<IWebElement> segmentsList = EditorPage.GetTagetsList();

			// Фактический номер последней видимой строки
			int curLastRow = EditorPage.GetRowPositionByRowNumber(segmentsList.Count);

			Console.WriteLine("curLastRow: " + curLastRow);
			Console.WriteLine("lastLastFactRow: " + lastLastFactRow);

			// Для проверки, закончилась лекция или нет - сравниваем последние фактические номера (текущий и предыдущий),
			// если они совпали - мы зашли в заполненную лекцию
			isLectureFinished = curLastRow == lastLastFactRow;

			startIndex = 0;
			if (!isLectureFinished)
			{
				// Фактический номер первой видимой строки
				int curFirstRow = EditorPage.GetRowPositionByRowNumber(1);
				startIndex = lastLastFactRow - curFirstRow + 1;
			}

			lastLastFactRow = curLastRow;

			return segmentsList;
		}

		/// <summary>
		/// Добавить переводы в видимые сегменты
		/// </summary>
		/// <param name="lastLastFactRow">IN/OUT: фактический номер последней видимой строчки</param>
		/// <returns>лекция закончилась</returns>
		protected bool AddTraslationsVisibleSegments(ref int lastLastFactRow)
		{
			bool isLectureFinished = false;
			int startIndex = 0;
			int position = 0;
			// Список видимых сегментов
			IList<IWebElement> segmentsList = GetVisibleSegmentList(ref lastLastFactRow, out isLectureFinished, out startIndex);
			string translationText = "Test" + DateTime.Now.Ticks;

			if (!isLectureFinished)
			{
				for (int i = startIndex; i < segmentsList.Count; ++i)
				{
					segmentsList[i].Click();
					segmentsList[i].Click();
					// Получение позиции по заданному таргету
					position = EditorPage.GetPositionByTargetElement(segmentsList[i]);
					if (segmentsList[i].Text.Trim().Length == 0)
					{
						segmentsList[i].SendKeys(translationText);
						EditorPage.ClickSegmentConfirmByPosition(position);
						EditorPage.WaitUntilDisappearBorderByPosition(position);
					}
				}
			}

			// Закончилась ли лекция
			return isLectureFinished;
		}

		/// <summary>
		/// Прокрутить лидерборд до позиции пользователя
		/// </summary>
		/// <returns>номер строки с пользователем</returns>
		protected int ScrollLeaderboardToUser(string userName)
		{
			// Открыть профиль пользователя
			HomePage.OpenProfile();
			// Получить место пользователя в профиле
			int userPosition = ProfilePage.GetPosition();

			// Перейти к списку лидеров
			Header.OpenLeaderboardPage();

			// Рассчитать, сколько страниц лидеров надо прокрутить
			int numToScroll = (userPosition % 10 == 0) ? (userPosition / 10 - 1) : (userPosition / 10);
			for (int i = 0; i < numToScroll; ++i)
			{
				LeaderboardPage.OpenNextPage();
			}

			// Получить номер в новом списке
			int numInList = userPosition % 10;
			if (numInList == 0)
			{
				numInList = 10;
			}

			return numInList;
		}

		/// <summary>
		/// Проголосовать за предложенные переводы
		/// </summary>
		/// <param name="isVoteUp"></param>
		/// <param name="voteOnlyUnvoted"></param>
		/// <param name="isNumberLimited"></param>
		/// <param name="votesNumberLeft"></param>
		/// <returns></returns>
		protected int VoteSuggestedTranslations(bool isVoteUp, bool voteOnlyUnvoted, bool isNumberLimited = false, int votesNumberLeft = 0)
		{
			int votedNumber = 0;
			setDriverTimeoutMinimum();
			string voteXPath = isVoteUp ? "fa-thumbs-up" : "fa-thumbs-down";
			IList<IWebElement> translationsList = EditorPage.GetVotesListByVoteType(voteXPath);
			setDriverTimeoutDefault();
			Console.WriteLine(translationsList.Count > 0 ? "переводы есть" : "переводов нет");

			// Пробуем проголосовать за предложенные переводы
			for (int i = 0; i < translationsList.Count; ++i)
			{
				if (isNumberLimited && votedNumber >= votesNumberLeft)
				{
					Console.WriteLine("выходим из голосования");
					break;
				}

				bool canVote = true;
				if (voteOnlyUnvoted)
				{
					setDriverTimeoutMinimum();
					canVote = !EditorPage.GetIsVoteConsidered(true, (i + 1)) && !EditorPage.GetIsVoteConsidered(false, (i + 1));
					setDriverTimeoutDefault();
				}

				if (canVote)
				{
					Console.WriteLine("пытаемся проголосовать " + i);
					translationsList[i].Click();
					if (EditorPage.GetIsVoteConsidered(isVoteUp, (i + 1)))
					{
						++votedNumber;
					}
				}
			}
			Console.WriteLine("вышли из цикла");

			return votedNumber;
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

			_driver.Manage().Window.Maximize();

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

        public void MakeScreen()
        {
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
        }
    }
}
