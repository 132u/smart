using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace AbbyyLS.PEMT.Function.Selenium.Tests
{
    public class BaseTest
	{
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

	    private string _adminUrl;

	    protected string AdminUrl
	    {
		    get { return _adminUrl; }
	    }

		protected struct UserInfo
		{
			public string login;
			public string password;

			public UserInfo(string l, string p)
			{
				login = l;
				password = p;
			}
		}

	    private UserInfo _userInfo;

	    protected UserInfo MainUserInfo
	    {
		    get { return _userInfo; }
	    }

	    private string _pemtAccountName;

	    protected string PemtAccountName
	    {
			get { return _pemtAccountName; }
	    }

		/// <summary>
		/// Страница логина
		/// </summary>
		private LoginPageHelper _loginPageHelper;
		protected LoginPageHelper LoginPage
		{
			get { return _loginPageHelper;}
		}

	    /// <summary>
	    /// Страница админки
	    /// </summary>
	    private AdminPageHelper _adminPageHelper;

	    protected AdminPageHelper AdminPage
	    {
		    get { return _adminPageHelper; }
	    }

		private string _browserName;

		private string BrowserName
		{
			get
			{
				return _browserName;
			}
		}

		/// <summary>
		/// Path результатов теста
		/// </summary>
		protected string PathTestResults
		{
			get
			{
				System.IO.DirectoryInfo directoryInfo =
					System.IO.Directory.GetParent(@"..\TestResults\");

				return directoryInfo.ToString();
			}
		}

		/// <summary>
		/// Конструктор BaseTest
		/// </summary>
		public BaseTest(/*string browserName*/)
		{
			_browserName = "Firefox";//browserName;
			_url = ConfigurationManager.AppSettings["url"];
			_adminUrl = ConfigurationManager.AppSettings["adminUrl"];
			_pemtAccountName = ConfigurationManager.AppSettings["PemtAccountName"];
			CreateDriver();

			_userInfo = new UserInfo
			{
				login = ConfigurationManager.AppSettings["Login"],
				password = ConfigurationManager.AppSettings["Password"],
			};
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
			_wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

			RecreateDrivers();
		}

		/// <summary>
		/// Пересоздание Helper'ов с новыми Driver, Wait
		/// </summary>
		private void RecreateDrivers()
		{
			_loginPageHelper = new LoginPageHelper(Driver, Wait);
			_adminPageHelper = new AdminPageHelper(Driver, Wait);
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
			_driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
		}

		/// <summary>
		/// Устанавливаем ожидание для драйвера в максимум (20 секунд)
		/// </summary>
		protected void setDriverTimeoutMaximum()
		{
			_driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
		}

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="accountName">аккаунт</param>
		public void Authorization(string accountName = "TestAccount")
		{
			// Перейти на стартовую страницу
			_driver.Navigate().GoToUrl(_url);

			// Проверить, загрузилась ли
			Assert.IsTrue(LoginPage.WaitPageLoad(),
				"Не прогрузилась страница Login - возможно, сайт недоступен");
			// Заполнить логин и пароль
			LoginPage.EnterLogin(TestUserInfo.login);
			LoginPage.EnterPassword(TestUserInfo.password);
			LoginPage.ClickSubmit();

			// Проверить, появился ли список аккаунтов
			if (LoginPage.WaitAccountExist(accountName, 5))
			{
				// Выбрать аккаунт
				LoginPage.ClickAccountName(accountName);
				// Зайти на сайт
				LoginPage.ClickSubmit();
			}
			else if (LoginPage.GetIsErrorExist())
			{
				Assert.Fail("Появилась ошибка при входе! М.б.недоступен AOL.");
			}
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

			// Зайти под пользователем
			Authorization();
		}

		[TearDown]
		public void Teardown()
		{
			// Если тест провалился
			if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
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
