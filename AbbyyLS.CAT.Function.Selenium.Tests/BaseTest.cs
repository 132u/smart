using System;
using System.Threading;
using AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures;
using AbbyyLS.CAT.Function.Selenium.Tests.CommonHelpers;
using AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Imaging;
using NConfiguration;
using System.Text.RegularExpressions;
using NLog;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Базовый тест
	/// </summary>

	[TestFixture("Firefox")]
	//[TestFixture("IE")]
	public class BaseTest
	{

		public static Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Конструктор базового теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public BaseTest(string browserName)
		{

			try
			{
				var cfgAgentSpecific = TestSettingDefinition.Instance.Get<TargetServerConfig>();
				var cfgUserInfo = TestSettingDefinition.Instance.Get<UserInfoConfig>();
				var cfgRoot = TestSettingDefinition.Instance.Get<FilesRootCfg>();

				BrowserName = browserName;
				TestFile = new TestFile(cfgRoot);
				PathTestFiles = cfgRoot.Root;

				
				CreateUniqueNamesByDatetime();
				initializeRelatedToServerFields(cfgAgentSpecific);
				initializeRelatedToUserFields(cfgUserInfo);
				initializeUsersAndCompanyList();
				createDriver();
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Ошибка в конструкторе : " + ex.Message, ex);
				throw;
			}
		}
		

		protected IWebDriver Driver { get; private set; }

		protected WebDriverWait Wait { get; private set; }

		protected string Url { get; private set; }

		protected string WorkspaceUrl { get; private set; }
		protected bool Standalone { get; private set; }
		protected string AdminUrl { get; private set; }

		protected string Login { get; private set; }

		protected string Password { get; private set; }

		protected string UserName { get; private set; }

		protected string Login2 { get; private set; }

		protected string Password2 { get; private set; }

		protected string UserName2 { get; private set; }

		protected List<UserInfo> TestUserList { get; private set; }

		protected List<UserInfo> TestCompanyList { get; private set; }
		protected List<UserInfo> CourseraUserList { get; private set; }

		protected string ProjectName { get; private set; }

		protected string PathTestResults
		{
			get
			{

				return Directory.GetParent(@"..\TestResults\").ToString();
			}
		}

		public string PathTestFiles
		{
			get
			{
				return _pathTestFiles;
			}

			private set
			{
				_pathTestFiles = Directory.GetParent(string.Concat(value, Path.DirectorySeparatorChar)).ToString();
			}
		}
		
		protected TestFile TestFile { get; private set; }

		protected string BrowserName { get; private set; }

		protected ProjectPageHelper ProjectPage { get; private set; }

		protected EditorPageHelper EditorPage { get; private set; }

		protected LoginPageHelper LoginPage { get; private set; }

		protected WorkSpacePageHelper WorkspacePage { get; private set; }

		protected Workspace_CreateProjectDialogHelper WorkspaceCreateProjectDialog { get; private set; }

		protected MainHelper MainHelperClass { get; private set; }

		protected DomainPageHelper DomainPage { get; private set; }

		protected TMPageHelper TMPage { get; private set; }

		protected GlossaryListPageHelper GlossaryListPage { get; private set; }

		protected GlossaryPageHelper GlossaryPage { get; private set; }

		protected SearchPageHelper SearchPage { get; private set; }

		protected ClientPageHelper ClientPage { get; private set; }

		protected AdminPageHelper AdminPage { get; private set; }

		protected GlossaryEditStructureFormHelper GlossaryEditStructureForm { get; private set; }

		protected DictionaryPageHelper DictionaryPage { get; private set; }

		protected Editor_RevisionPageHelper RevisionPage { get; private set; }

		protected UserRightsPageHelper UserRightsPage { get; private set; }

		protected SuggestTermDialogHelper SuggestTermDialog { get; private set; }

		protected GlossaryEditFormHelper GlossaryEditForm { get; private set; }

		protected GlossarySuggestPageHelper GlossarySuggestPage { get; private set; }

		protected CatPanelResultsHelper CatPanel { get; private set; }

		protected ResponsiblesDialogHelper ResponsiblesDialog { get; private set; }

		protected AddTermFormHelper AddTermForm { get; private set; }

		protected RegistrationPageHelper RegistrationPage { get; private set; }
		protected MyAccountPageHelper MyAccountPage { get; private set; }
		protected GlossaryTermFilterHelper GlossaryTermFilterPage { get; private set; }

		protected DateTime TestBeginTime { get; private set; }

		protected bool QuitDriverAfterTest { get; set; }

		public const string DeadlineDate = "03/03/2016";
		public const string ConstTMName = "TestTM";
		public const string GlossaryName = "TestGlossary";
		public const string ProjectNameExportTestOneDoc = "TestProjectTestExportOneDocumentUniqueName";
		public const string ProjectNameExportTestMultiDoc = "TestProjectTestExportMultiDocumentsUniqueName";

		/// <summary>
		/// Начальная подготовка для группы тестов
		/// </summary>
		[TestFixtureSetUp]
		public void SetupAllBase()
		{
		}

		/// <summary>
		/// Начальная подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void SetupBase()
		{
			// По умолчанию выходим из браузера
			QuitDriverAfterTest = true;

			// Вывести время начала теста
			TestBeginTime = DateTime.Now;
			Console.WriteLine(TestContext.CurrentContext.Test.Name + "\nStart: " + TestBeginTime);

			if (Driver == null)
			{
				// Если конструктор заново не вызывался, то надо заполнить Driver
				createDriver();
			}

			// Создание уникального имени проекта
			CreateUniqueNamesByDatetime();
		}

		/// <summary>
		/// Конечные действия для группы тестов
		/// </summary>
		[TestFixtureTearDown]
		public virtual void TeardownAllBase()
		{
			// Закрыть драйвер
			ExitDriver();
		}

		/// <summary>
		/// Конечные действия для каждого теста
		/// </summary>
		[TearDown]
		public void TeardownBase()
		{
			// При вылете браузера возникает ошибка, пытаемся ее словить
			try
			{
				if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
				{
					// Сделать скриншот
					var screenshotDriver = Driver as ITakesScreenshot;
					var screenshot = screenshotDriver.GetScreenshot();

					// Создать папку для скриншотов провалившихся тестов
					var failResultPath = Path.Combine(PathTestResults, "FailedTests");
					Directory.CreateDirectory(failResultPath);

					// Создать имя скриншота по имени теста
					var screenName = TestContext.CurrentContext.Test.Name;
					
					if (screenName.Contains("("))
					{
						// Убрать из названия теста аргументы (файлы)
						screenName = screenName.Substring(0, screenName.IndexOf("("));
					}
					screenName += DateTime.Now.Ticks + ".png";
					// Создать полное имя файла
					screenName = Path.Combine(failResultPath, screenName);
					// Сохранить скриншот
					screenshot.SaveAsFile(screenName, ImageFormat.Png);
				}
			}
			catch (Exception)
			{
				// Закрыть драйвер
				ExitDriver();
			}

			// Выходим из браузера, если нужно
			if (QuitDriverAfterTest)
			{
				ExitDriver();
			}

			// Вывести информацию о прохождении теста
			var testFinishTime = DateTime.Now;
			// Время окончания теста
			Console.WriteLine("Finish: " + testFinishTime);
			// Длительность теста
			var duration = TimeSpan.FromTicks(testFinishTime.Ticks - TestBeginTime.Ticks);
			var durResult = "Duration: ";

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
				// Если тест провалился
				Console.WriteLine("Fail!");
			}
		}

		public bool TestUserFileExist()
		{
			return File.Exists(TestFile.TestUserFile);
		}
		
		/// <summary>
		/// Обновить уникальные имена для нового теста
		/// </summary>
		protected void CreateUniqueNamesByDatetime()
		{
			ProjectName = "Test Project" + "_" + DateTime.UtcNow.Ticks;
		}

		/// <summary>
		/// Обовить страницу
		/// </summary>
		public void RefreshPage()
		{
			Driver.Navigate().Refresh();
		}

		/// <summary>
		/// Метод создания Driver 
		/// </summary>
		private void createDriver()
		{
			if (BrowserName == "Firefox")
			{
				if (Driver == null)
				{
					_profile = new FirefoxProfile
					{
						AcceptUntrustedCertificates = true
					};

					// Изменение языка браузера на английский
					_profile.SetPreference("intl.accept_languages", "en");
					_profile.SetPreference("browser.download.dir", PathTestResults);
					_profile.SetPreference("browser.download.folderList", 2);
					_profile.SetPreference("browser.download.useDownloadDir", false);
					_profile.SetPreference("network.automatic-ntlm-auth.trusted-uris", Url);
					//_profile.SetPreference("browser.download.manager.showWhenStarting", false);
					_profile.SetPreference("browser.helperApps.alwaysAsk.force", false);
					_profile.SetPreference
						("browser.helperApps.neverAsk.saveToDisk", "text/xml, text/csv, text/plain, text/log, application/zip, application/x-gzip, application/x-compressed, application/x-gtar, multipart/x-gzip, application/tgz, application/gnutar, application/x-tar, application/x-xliff+xml,  application/msword.docx, application/pdf, application/x-pdf, application/octetstream, application/x-ttx, application/x-tmx, application/octet-stream");
					//_profile.SetPreference("pdfjs.disabled", true);

					Driver = new FirefoxDriver(_profile);
					//string profiledir = "../../../Profile";
					// string profiledir = "TestingFiles/Profile";
					//_profile = new FirefoxProfile(profiledir);
					//Driver = new FirefoxDriver(_profile);
				}
			}
			else if (BrowserName == "Chrome")
			{
				// драйвер работает некорректно
				Driver = new ChromeDriver();
			}
			else if (BrowserName == "IE")
			{
				Driver = new InternetExplorerDriver();
			}

			setDriverTimeoutDefault();
			Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));

			Driver.Manage().Window.Maximize();

			recreateDrivers();
		}

		/// <summary>
		/// Установить время ожидания драйвера в минимум (для поиска элементов, которых по ожиданию нет)
		/// </summary>
		protected void setDriverTimeoutMinimum()
		{
			Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
		}

		/// <summary>
		/// Установить стандартное время ожидание драйвера
		/// </summary>
		protected void setDriverTimeoutDefault()
		{
			Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
		}

		/// <summary>
		/// Метод назначения задачи на пользователя 
		/// </summary>
		/// <param name="documentRowNum">номер документа</param>
		protected void AssignTask(int documentRowNum = 1)
		{
			// Открываем инфо документа 
			ProjectPage.OpenDocumentInfo(documentRowNum);

			// Открываем окно прав исполнителей
			ProjectPage.ClickAssignRessponsibleBtn();

			//// нажать галочку около документа
			//SelectDocumentInProject(documentRowNum);
			//// нажать на Progress
			//ProjectPage.ClickProgressBtn();
			ProjectPage.WaitProgressDialogOpen();

			// Назначить ответственного в окне Progress
			ProjectPage.ClickUserNameCell();
			ProjectPage.WaitAssignUserList();

			// Выбрать нужное имя
			ProjectPage.ClickAssignUserListUser(UserName);
			ProjectPage.WaitAssignBtnDisplay();

			// Нажать на Assign
			ProjectPage.ClickAssignBtn();
			// Дождаться появления Cancel
			ProjectPage.WaitCancelAssignBtnDisplay();

			// Нажать на Close
			ProjectPage.CloseAssignDialogClick();

			SelectDocumentInProject(documentRowNum);

			// Обновить страницу, чтобы активен был переход в редактор
			Driver.Navigate().Refresh();

			// Нажать на Accept
			ProjectPage.ClickAllAcceptBtns();
		}



		/// <summary>
		/// Создание Тм с импортом файла со страницы проекта
		/// </summary>
		/// <param name="tmName">название новой Тм</param>
		/// <param name="file">файл tmx</param>
		public void ProjectPageAddTmImportTmx(string tmName, string file)
		{
			//Кликнуть кнопку Edit TM
			ProjectPage.ClickEditTMBtn();
			//Дождаться появления диалога изменения ТМ
			ProjectPage.WaitEditTMDialogAppear();
			//Нажать Create в диалоге редактирования TM
			ProjectPage.ClickCreateTMBtn();
			//Заполнить название ТМ в диалоге
			ProjectPage.FillTmNameDialog(tmName);
			//Нажать Импорт в диалоге редактирования TM
			ProjectPage.ClickSaveAndImportTMBtn();
			////Нажать Загрузить файл в диалоге редактирования TM
			ProjectPage.ClickUploadTMBtn();
			//Загрузить документ
			ProjectPage.UploadFileNativeAction(file);
			//Подтвердить импорт
			ProjectPage.ClickConfirmImportBtn();
			Assert.IsTrue(ProjectPage.ClickRadioBtm(),"TM не выбран");
			// Сохранить изменения
			ProjectPage.ClickSaveTMBtn();
			// Дождаться окончания загрузки
			Assert.IsTrue(ProjectPage.WaitDocumentDownloadFinish(),
				"Ошибка: документ загружается слишком долго");
		}

		/// <summary>
		/// Настройка претранслейта c Tm со страницы проекта
		/// </summary>
		/// <param name="tmName">название Тм для претранслейта</param>
		public void SetTmPretranslate(string tmName)
		{
			// нажать кнопку претранслейт
			ProjectPage.ClickPretranslateBtn();
			// создать новое правило
			ProjectPage.ClickNewRuleBtn();
			// открыть меню источника претранслейта
			ProjectPage.ClickSourcePretranslateBtn();
			// выбрать ТМ как источник
			ProjectPage.ClickTmForPretranslateBtn(tmName);
			// сохранить настройки и выполнить претранслейт
			ProjectPage.ClickSavePretranslateBtn();
		}

		/// <summary>
		/// Нажать галочку у документа в проекте
		/// </summary>
		/// <param name="documentNumber"></param>
		protected void SelectDocumentInProject(int documentNumber = 1)
		{
			// Нажать галочку у документа
			Assert.IsTrue(
				ProjectPage.SelectDocument(documentNumber),
				"Ошибка: на странице проекта нет документа");
		}

		/// <summary>
		/// Метод открытия документа в редакторе для выполнения конкретного задания
		/// </summary>
		/// <param name="documentNumber">Номер документа в списке на странице проекта</param>
		/// <param name="taskId">Тип выполняемого задания: 1)перевод 2)редактура 3)корректура</param>
		protected void OpenDocument(int documentNumber = 1, int taskId = 1)
		{
			// Открыть документ
			Assert.IsTrue(
				ProjectPage.OpenDocument(documentNumber),
				"Ошибка: на странице проекта нет документа");

			if (ResponsiblesDialog.WaitUntilChooseTaskDialogDisplay())
			{
				switch (taskId)
				{
					case 1:

						Assert.True(EditorPage.GetTranslationTaskBtnIsExist(), "Ошибка: Неверный этап в окне выбора ");

						//выбрать задачу перевода
						EditorPage.ClickTranslationTaskBtn();
						break;

					case 2:
						Assert.True(EditorPage.GetEditingTaskBtnIsExist(), "Ошибка: Неверный этап в окне выбора ");

						//выбрать задачу редактуры
						EditorPage.ClickEditingTaskBtn();
						break;

					case 3:
						Assert.True(EditorPage.GetProofreadingTaskBtnIsExist(), "Ошибка: Неверный этап в окне выбора ");

						//выбрать задачу корректуры
						EditorPage.ClickProofreadingTaskBtn();
						break;
				}

				EditorPage.ClickContBtn();
			}
			// Дождаться загрузки страницы
			EditorPage.WaitPageLoad();

			Thread.Sleep(1000);

			// Проверить, существует ли хотя бы один сегмент
			Assert.IsTrue(EditorPage.GetSegmentsExist(), "Ошибка: нет сегментов");
		}

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="accountName">аккаунт</param>
		/// <param name="alternativeUser">Использовать альтернативный аккаунт</param>
		/// <param name="dataServer">Расположение сервера</param>
		public void Authorization(
			string accountName = "TestAccount", 
			bool alternativeUser = false, 
			string dataServer = "Europe")
		{
			string authLogin = "";
			string authPassword = "";

			if (!alternativeUser)
			{
				authLogin = Login;
				authPassword = Password;
			}
			else
			{
				authLogin = Login2;
				authPassword = Password2;
			}

			if (Standalone)
			{
				// Перейти на стартовую страницу
				Driver.Navigate().GoToUrl(Url);
			}
			else
			{
				// Перейти на стартовую страницу
				Driver.Navigate().GoToUrl(Url + "/sign-in");

				if (Driver.Url.Contains("pro"))
				{
					// Проверить, загрузилась ли
					Assert.IsTrue(LoginPage.WaitPageLoadLpro(),
						"Не прогрузилась страница Login - возможно, сайт недоступен");

					// Заполнить логин и пароль
					LoginPage.EnterLoginLpro(authLogin);
					LoginPage.EnterPasswordLpro(authPassword);
					Thread.Sleep(1000);
					LoginPage.ClickSubmitLpro();
				}
				else
				{
					// Проверить, загрузилась ли
					Assert.IsTrue(LoginPage.WaitPageLoad(),
						"Не прогрузилась страница Login - возможно, сайт недоступен");
					// Заполнить логин и пароль
					LoginPage.EnterLogin(authLogin);
					LoginPage.EnterPassword(authPassword);
					LoginPage.ClickSubmitCredentials();

					// Проверить, появился ли список аккаунтов
					if (LoginPage.WaitAccountExist(accountName, waitmax: 8))
					{
						// Выбрать аккаунт
						LoginPage.ClickAccountName(accountName);
						// Зайти на сайт
						//LoginPage.ClickSubmitAccount();
					}
					else if (LoginPage.GetIsErrorExist())
					{
						Assert.Fail("Появилась ошибка при входе! М.б.недоступен AOL.");
					}
					// иначе у пользователя только 1 аккаунт
				}
			}

			// Изменили язык на Английский
			Assert.IsTrue(
				WorkspacePage.WaitAppearLocaleBtn(), 
				"Не дождались загрузки страницы со ссылкой для изменения языка");
			WorkspacePage.SelectLocale(WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.English);
		}

		/// <summary>
		/// Переход на вкладку workspace
		/// Если переадресация на стартовую страницу, то авторизация
		/// </summary>
		public void GoToWorkspace(string accountName = "TestAccount")
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу workspace
				Driver.Navigate().GoToUrl(WorkspaceUrl);

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					
					// Проходим процедуру авторизации
					Authorization(accountName);
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Ошибка при переходе на стр WS: " + ex.Message, ex);

				Driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Перейти на страницу workspace второй раз
				Driver.Navigate().GoToUrl(WorkspaceUrl);

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{

					// Проходим процедуру авторизации
					Authorization(accountName);
				}
			}
		}

		/// <summary>
		/// Переход на admin страницу
		/// </summary>
		public void GoToAdminPage()
		{
			try
			{
				// Перейти на  admin страницу
				Driver.Navigate().GoToUrl(AdminUrl);
				//Assert.IsTrue(AdminPage.WaitPageLoad(), "Ошибка: страница админки не загрузилась");
			}
			catch
			{
				Driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToAdminPage();
			}
		}

		/// <summary>
		/// Переход на вкладку глоссариев
		/// Если переадресация на стартовую страницу, то авторизация и затем переход
		/// </summary>
		public void GoToGlossaries()
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу глоссариев
				Driver.Navigate().GoToUrl(Url + "/Enterprise/Glossaries");

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					// Проходим процедуру авторизации
					Authorization();
					// Пробуем перейти на глоссарии еще раз
					GoToGlossaries();
				}
			}
			catch
			{
				Driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				Thread.Sleep(2000);

				// Пробуем перейти на глоссарии еще раз
				GoToGlossaries();
			}
		}

		/// <summary>
		/// Переход на вкладку TM
		/// Если переадресация на стартовую страницу, то авторизация и затем переход
		/// </summary>
		public void GoToTranslationMemories()
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу
				Driver.Navigate().GoToUrl(Url + "/TranslationMemories/Index");

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					// Проходим процедуру авторизации
					Authorization();
					// Пробуем перейти на страницу еще раз
					GoToTranslationMemories();
				}
			}
			catch
			{
				Driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToTranslationMemories();
			}
		}

		/// <summary>
		/// Переход на вкладку Domains
		/// Если переадресация на стартовую страницу, то авторизация и затем переход
		/// </summary>
		public void GoToDomains()
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу
				Driver.Navigate().GoToUrl(Url + "/Domains/Index");

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					// Проходим процедуру авторизации
					Authorization();
					// Пробуем перейти на страницу еще раз
					GoToDomains();
				}
			}
			catch
			{
				Driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToDomains();
			}
		}

		/// <summary>
		/// Переход на вкладку Clients
		/// Если переадресация на стартовую страницу, то авторизация и затем переход
		/// </summary>
		public void GoToClients()
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу
				Driver.Navigate().GoToUrl(Url + "/Clients/Index");

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					// Проходим процедуру авторизации
					Authorization();
					// Пробуем перейти на страницу еще раз
					GoToClients();
				}
			}
			catch
			{
				Driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToClients();
			}
		}

		/// <summary>
		/// Переход на страницу My Account
		/// Если переадресация на стартовую страницу, то авторизация и затем переход по клику MyAccount в панели WS
		/// </summary>
		public void GoToMyAccount()
		{
			// Перейти на страницу
			Driver.Navigate().GoToUrl(Url + "/Billing/LicensePackages/");

			// Если открылась страница логина
			if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
			{
				// Проходим процедуру авторизации
				Authorization();

				// Кликаем MyAccount в панели WS
				MyAccountPage.ClickMyAccountLink();

				// Перешли в новое открытое окно браузера
				Driver.SwitchTo().Window(Driver.WindowHandles[1]);
			}
		}

		/// <summary>
		/// Заполнение первого шага создания проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <param name="useDefaultTargetLanguage">использовать язык target по умолчанию</param>
		/// <param name="srcLang">язык источника</param>
		/// <param name="trgLang">язык перевода</param>
		protected void FirstStepProjectWizard(
			string projectName, 
			bool useDefaultTargetLanguage = true,
			CommonHelper.LANGUAGE srcLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE trgLang = CommonHelper.LANGUAGE.Russian)
		{
			Assert.IsTrue(WorkspacePage.WaitPageLoad(), "Страница workspace не прогрузилась");

			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();

			// Ввести название проекта
			WorkspaceCreateProjectDialog.FillProjectName(projectName);
			// Ввести deadline дату
			WorkspaceCreateProjectDialog.FillDeadlineDate(DeadlineDate);

			// Выбрать Source - en
			WorkspaceCreateProjectDialog.SelectSourceLanguage(srcLang);

			// Проверить, что в Target выставлен русский язык
			SetRusLanguageTarget();

			// Выбрать Target
			if (!useDefaultTargetLanguage)
			{
				// Открыть список Target
				WorkspaceCreateProjectDialog.ClickTargetList();
				// Убираем русский
				WorkspaceCreateProjectDialog.ClickTargetItem(CommonHelper.LANGUAGE.Russian);
				// Добавляем заданный язык
				WorkspaceCreateProjectDialog.ClickTargetItem(trgLang);
				// Закрыть список Target
				WorkspaceCreateProjectDialog.ClickTargetList();
			}
		}

		/// <summary>
		/// Создать проект
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <param name="downloadFile">документ для загрузки</param>
		/// <param name="createNewTM">создать новый TM</param>
		/// <param name="tmFile">файл для загрузки в ТМ</param>
		/// <param name="setGlossary">тип глоссария: новый, первый в списке, по имени</param>
		/// <param name="glossaryName">имя глоссария, если выбирается по имени</param>
		/// <param name="chooseMT">выбрать МТ</param>
		/// <param name="mtType">тип МТ</param>
		/// <param name="isNeedCheckExist">Нужна проверка проекта в списке</param>
		/// <param name="translationTasksNumber">количество заданий перевода</param>
		/// <param name="editingTasksNumber">количество заданий редактуры</param>
		/// <param name="proofreadingTasksNumber">количество заданий корректуры</param>
		protected void CreateProject(
			string projectName,
			string downloadFile = "",
			bool createNewTM = false,
			string tmFile = "",
			Workspace_CreateProjectDialogHelper.SetGlossary setGlossary = Workspace_CreateProjectDialogHelper.SetGlossary.None,
			string glossaryName = "",
			bool chooseMT = false,
			Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.None,
			bool isNeedCheckExist = true,
			int translationTasksNumber = 1,
			int editingTasksNumber = 0,
			int proofreadingTasksNumber = 0)
		{
			// Заполнение полей на первом шаге
			FirstStepProjectWizard(projectName);
			if (downloadFile.Length > 0)
			{
					WorkspaceCreateProjectDialog.UploadFileToNewProject(downloadFile);
					WorkspaceCreateProjectDialog.WaitDocumentAppear(Path.GetFileName(downloadFile));
			}
			WorkspaceCreateProjectDialog.ClickNextStep();

			//2 шаг - выбор ТМ
			if (createNewTM)
			{
				// Создать новую ТМ, c файлом или чистую
				CreateNewTM(tmFile);
				WorkspaceCreateProjectDialog.ClickNextStep();
			}
			else
			{
				if (!WorkspaceCreateProjectDialog.GetIsTMTableNotEmpty())
				{
					// Кликаем Next
					WorkspaceCreateProjectDialog.ClickNextStep();
					// Кликаем Skip
					SkipNotSelectedTM();
				}
				else
				{
					// Выбрать существующую ТМ
					ChooseFirstTMInList();
					WorkspaceCreateProjectDialog.ClickNextStep();
				}
			}

			//3 шаг - выбор глоссария
			switch (setGlossary)
			{
				case Workspace_CreateProjectDialogHelper.SetGlossary.None:

					break;

				case Workspace_CreateProjectDialogHelper.SetGlossary.First:

					WorkspaceCreateProjectDialog.ClickFirstGlossaryInTable();
					break;

				case Workspace_CreateProjectDialogHelper.SetGlossary.New:

					CreateAndAddGlossary();
					break;

				case Workspace_CreateProjectDialogHelper.SetGlossary.ByName:

					WorkspaceCreateProjectDialog.ClickGlossaryByName(glossaryName);
					break;
			}

			WorkspaceCreateProjectDialog.ClickNextStep();

			//4 шаг - выбор МТ
			if (!Standalone && (chooseMT && mtType != Workspace_CreateProjectDialogHelper.MT_TYPE.None))
 			{
				WorkspaceCreateProjectDialog.ChooseMT(mtType);
			}

			WorkspaceCreateProjectDialog.ClickNextStep();

			//5 шаг - настройка этапов workflow
			SetUpWorkflow(translationTasksNumber,
			editingTasksNumber,
			proofreadingTasksNumber);

			Thread.Sleep(500);
			WorkspaceCreateProjectDialog.ClickNextStep();

			//5 шаг - настройка Pretranslate
			//Pretranslate();
			//Finish
			Thread.Sleep(500);
			WorkspaceCreateProjectDialog.ClickFinishCreate();

			// Дождаться проекта в списке проектов
			if (isNeedCheckExist)
			{
				Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(projectName), "Ошибка: проект не появился в списке Workspace");
			}

			// Дождаться, пока документ догрузится
			Assert.IsTrue(WorkspacePage.WaitProjectLoad(projectName), "Ошибка: документ не загрузился");
		}

		/// <summary>
		/// Создать проект и не проверять, создался ли он
		/// </summary>
		/// <param name="projectName">название проекта</param>
		protected void CreateProjectWithoutCheckExist(string projectName)
		{
			CreateProject(projectName, isNeedCheckExist: false);
		}

		/// <summary>
		/// Создание проекта, если проект с таким именем не создан
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <param name="downloadFile">документ для загрузки</param>
		/// <param name="createNewTM">создать новый TM</param>
		/// <param name="tmFile">файл для загрузки в ТМ</param>
		/// <param name="setGlossary">тип глоссария: новый, первый в списке, по имени</param>
		/// <param name="glossaryName">имя глоссария, если выбирается по имени</param>
		/// <param name="chooseMT">выбрать МТ</param>
		/// <param name="mtType">тип МТ</param>
		/// <param name="isNeedCheckExist">Нужна проверка проекта в списке</param>
		protected void CreateProjectIfNotCreated(
			string projectName, 
			string downloadFile = "",
			bool createNewTM = false, 
			string tmFile = "",
			Workspace_CreateProjectDialogHelper.SetGlossary setGlossary = Workspace_CreateProjectDialogHelper.SetGlossary.None,
			string glossaryName = "",
			bool chooseMT = false, 
			Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.None,
			bool isNeedCheckExist = true)
		{
			if (!GetIsExistProject(projectName))
			{
				// Создание проекта
				CreateProject(projectName, downloadFile, createNewTM, tmFile, setGlossary, glossaryName, chooseMT, mtType, isNeedCheckExist);
			}
		}

		/// <summary>
		/// Создать новую ТМ в диалоге создания проекта
		/// </summary>
		/// <param name="TmFileName">файл для загрузки в ТМ</param>
		/// <param name="fromWorkspaceOrProject">true - из workspace, false - из проекта</param>
		public void CreateNewTM(string TmFileName, bool fromWorkspaceOrProject = true)
		{
			string tmName = "TestTM" + DateTime.Now.Ticks;

			//Создание ТМ
			if (TmFileName.Length > 0)
			{
				//Добавить тмх файл
				WorkspaceCreateProjectDialog.ClickUploadTMX();
				WorkspaceCreateProjectDialog.WaitUploadTMXDialog();
				WorkspaceCreateProjectDialog.FillTMNameDialog(tmName);

				WorkspaceCreateProjectDialog.UploadTMInNewProject(TmFileName);

				//Нажать на кнопку Import
				WorkspaceCreateProjectDialog.ClickSaveTMXDialog();

				//Дождаться пока ТМ появится в списке
				WorkspaceCreateProjectDialog.WaitTmxAppear(tmName);

				if (WorkspaceCreateProjectDialog.GetIsExistErrorFileMessage())
				{
					// Диалог загрузки документа не закрылся - закрываем
					TryCloseExternalDialog();

					// кликаем Import снова
					WorkspaceCreateProjectDialog.ClickImportBtn();
				}
				WorkspaceCreateProjectDialog.WaitImportDialogDisappear();
			}
			else
			{
				WorkspaceCreateProjectDialog.ClickCreateTM();
				WorkspaceCreateProjectDialog.WaitCreateTMDialog();
				WorkspaceCreateProjectDialog.FillTMName(tmName);
				WorkspaceCreateProjectDialog.ClickSaveTM();
				WorkspaceCreateProjectDialog.WaitUntilCreateTMDialogDisappear();
				//Дождаться пока ТМ появится в списке
				WorkspaceCreateProjectDialog.WaitTmxAppear(tmName);
			}
		}

		/// <summary>
		/// Выбрать первую ТМ из списка доступных 
		/// </summary>
		public void ChooseFirstTMInList()
		{
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsTMTableNotEmpty(),
				"Ошибка: пустая таблица TM");

			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
		}
		
		/// <summary>
		/// Создание и подключение нового глоссария
		/// </summary>
		public void CreateAndAddGlossary()
		{
			var internalGlossaryName = "InternalGlossaryName" + DateTime.UtcNow.Ticks;

			WorkspaceCreateProjectDialog.ClickCreateGlossary();
			WorkspaceCreateProjectDialog.SetNewGlossaryName(internalGlossaryName);
			WorkspaceCreateProjectDialog.ClickSaveNewGlossary();
			WorkspaceCreateProjectDialog.WaitNewGlossaryAppear(internalGlossaryName);
		}

		/// <summary>
		/// Шаг формирования рабочего процесса диалога создания проекта
		/// </summary>
		/// <param name="translationTasksNumber">количество заданий перевода</param>
		/// <param name="editingTasksNumber">количество заданий редактуры</param>
		/// <param name="proofreadingTasksNumber">количество заданий корректуры</param>
		public void SetUpWorkflow(
			int translationTasksNumber = 1,
			int editingTasksNumber = 0,
			int proofreadingTasksNumber = 0)
		{

			// если в документе нет задачи перевода, поменять стоящий по умолчанию перевод на редактуру
			if (translationTasksNumber == 0 && editingTasksNumber != 0)
			{
				// меняем первый таск на редактуру
				WorkspaceCreateProjectDialog.SetWorkflowEditingTask(1);
				// количество заданий по редактуре уменьшаем на один - одно задание уже поставили
				editingTasksNumber--;
			}
			// если в документе нет задач перевода и редактуры, поменять стоящий по умолчанию перевод на корректуру
			else if (translationTasksNumber == 0 && editingTasksNumber == 0)
			{
				// меняем первый таск на корректуру
				WorkspaceCreateProjectDialog.SetWorkflowProofreadingTask(1);
				// количество заданий по корректуре уменьшаем на один - одно задание уже поставили
				proofreadingTasksNumber--;
			}

			//номер этапа в списке этапов
			int taskNumber = 2;

			//создать необходимое количество Translation
			for (int i = 1; i < translationTasksNumber; i++)
			{
				// кликнуть создание нового задания
				WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
				// установить новое задание - перевод
				WorkspaceCreateProjectDialog.SetWorkflowTranslationTask(taskNumber);

				taskNumber++;
			}

			//создать необходимое количество Editing
			for (int i = 0; i < editingTasksNumber; i++)
			{
				// кликнуть создание нового задания
				WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
				// установить новое задание - редактура
				WorkspaceCreateProjectDialog.SetWorkflowEditingTask(taskNumber);

				taskNumber++;
			}

			//создать необходимое количество Proofreading
			for (int i = 0; i < proofreadingTasksNumber; i++)
			{
				// кликнуть создание нового задания
				WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
				// установить новое задание - корректура
				WorkspaceCreateProjectDialog.SetWorkflowProofreadingTask(taskNumber);

				taskNumber++;
			}
		}

		// TODO
		/// <summary>
		/// Шаг Pretranslate диалога создания проекта
		/// </summary>
		public void Pretranslate()
		{
		}

		/// <summary>
		/// метод открытия настроек проекта и загрузки нового документа с помощью  корпоративного аккаунта
		/// </summary>
		/// <param name="filePath">путь в файлу, импортируемого в проект</param>
		/// <param name="projectName">имя проекта</param>
		protected void ImportDocumentProjectSettings(string filePath, string projectName, string accountType = "TestAccount")
		{
			// Зайти в проект
			OpenProjectPage(projectName);
			// Кликнуть Import
			ProjectPage.ClickImportBtn();
			// ждем, когда загрузится окно для загрузки документа 
			ProjectPage.WaitImportDialogDisplay();
			// Заполнить диалог загрузки
			ProjectPage.UploadFileOnProjectPage(filePath);
			// Нажать Next
			ProjectPage.ClickNextImportDialog();

			Console.WriteLine("кликнули Next");

			// Если появилось сообщение, что не указали файл, значит, Enter не нажался
			if (ProjectPage.GetIsExistNoFileError())
			{
				// Диалог загрузки документа не закрылся - закрываем
				TryCloseExternalDialog();

				// кликнуть Next снова
				ProjectPage.ClickNextImportDialog();
			}

			// Дождаться появления ТМ таблицы
			ProjectPage.WaitImportTMTableDisplay();
			if (accountType != "TestAccount")
			{
				//Клик по кнопке Next
				ProjectPage.ClickNextBtn();
				// Проверка, что следующий шаг не открылся
				Assert.IsFalse(ProjectPage.GetAssigneeTableDisplay(), "Ошибка : Next кнопка активна(кликабельна) в диалоге импорта документа");
				// Нажать Finish
				ProjectPage.ClickFinishImportDialog();

				// Дождаться окончания загрузки
				Assert.IsTrue(ProjectPage.WaitDocumentDownloadFinish(),
					"Ошибка: документ загружается слишком долго");
			}
			else
			{
				// Next
				ProjectPage.ClickNextImportDialog();

				// Нажать Finish
				ProjectPage.ClickFinishImportDialog();

				// Дождаться окончания загрузки
			Assert.IsTrue(
				ProjectPage.WaitDocumentDownloadFinish(),
					"Ошибка: документ загружается слишком долго");
			}
		}

		/// <summary>
		/// Проверить, есть ли проект в списке на странице Workspace
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <returns>есть</returns>
		protected bool GetIsExistProject(string projectName)
		{
			return WorkspacePage.GetIsProjectInList(projectName);
		}

		/// <summary>
		/// Удаление проект на вкладке проектов по имени
		/// </summary>
		/// <param name="ProjectNameToDelete">имя проекта, который надо удалить</param>
		protected void DeleteProjectFromList(string ProjectNameToDelete)
		{
			// Выбрать этот проект
			SelectProjectInList(ProjectNameToDelete);
			// Нажать Удалить
			WorkspacePage.ClickDeleteProjectBtn();
			// Подтвердить
			Assert.IsTrue(
				WorkspacePage.ClickConfirmDelete(), 
				"Ошибка: не появилась форма подтверждения удаления проекта");

			// Дождаться, пока пропадет диалог подтверждения удаления
			Assert.IsTrue(
				WorkspacePage.WaitUntilDeleteConfirmFormDisappear(), 
				"Ошибка: не появилась форма подтверждения удаления проекта");

			Thread.Sleep(500);
		}

		/// <summary>
		/// Закрываем диалог 2
		/// </summary>
		protected void TryCloseExternalDialog2()
		{
			SendKeys.SendWait(@"{Tab}");
			Thread.Sleep(1000);
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Закрываем диалог
		/// </summary>
		protected void TryCloseExternalDialog()
		{
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Работа с диалогом браузера: сохранение документа
		/// </summary>
		/// <param name="documentName">имя документа</param>
		protected void ExternalDialogSelectSaveDocument(string documentName)
		{
			var txt = Regex.Replace(documentName, "[+^%~()]", "{$0}");

			Thread.Sleep(2000);
			// В открывшемся диалоге выбираем "Сохранить"
			SendKeys.SendWait(@"{DOWN}");
			Thread.Sleep(1000);
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(2000);
			// Ввести адрес
			SendKeys.SendWait(txt);
			Thread.Sleep(1000);
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Работа с диалогом браузера: сохранение документа
		/// </summary>
		/// <param name="subFolderName">название папки для выгрузки</param>
		/// <param name="useFileName">использовать название исходного файла</param>
		/// <param name="filePath">название исходного файла</param>
		/// <param name="originalFileExtension">использовать исходное расширение</param>
		/// <param name="fileExtension">расширение</param>
		protected void ExternalDialogSaveDocument(
			string subFolderName,
			bool useFileName = false, 
			string filePath = "",
			bool originalFileExtension = true, 
			string fileExtension = "", 
			string time = "")
		{
			// Заполнить форму для сохранения файла
			var resultPath = Path.Combine(PathTestResults, subFolderName, time);
			Directory.CreateDirectory(resultPath);
			var newFileName = "";

			if (useFileName)
			{
				newFileName = Path.GetFileNameWithoutExtension(filePath) + "_" + DateTime.Now.Ticks.ToString();
			}
			else
			{
				newFileName = DateTime.Now.Ticks.ToString();
			}

			var currentFileExtension = originalFileExtension ? Path.GetExtension(filePath) : fileExtension;

			resultPath = Path.Combine(resultPath, newFileName + currentFileExtension);

			var txt = Regex.Replace(resultPath, "[+^%~()]", "{$0}");

			Thread.Sleep(1000);
			SendKeys.SendWait(txt);
			Thread.Sleep(1000);
			SendKeys.SendWait(@"{Enter}");

			bool isFileExitst;
			var waitSeconds = 0;

			do
			{
				isFileExitst = File.Exists(resultPath);
				Thread.Sleep(1000);
				waitSeconds++;

			} while (!isFileExitst && (waitSeconds <= 6));

			Assert.IsTrue(isFileExitst, "Ошибка: файл не экспортировался\n" + resultPath);
		}

		/// <summary>
		/// Добавить перевод и подтвердить
		/// </summary>
		public void AddTranslationAndConfirm(int segmentRowNumber = 1, string text = "Translation")
		{
			// Написать что-то в target
			EditorPage.AddTextTarget(segmentRowNumber, text);
			// Нажать на кнопку подтвердить
			EditorPage.ClickConfirmBtn();

			// Убедиться что сегмент подтвержден
			Assert.IsTrue(WaitSegmentConfirm(segmentRowNumber),
				"Ошибка: подтверждение (Confirm) не прошло");
		}

		/// <summary>
		/// Дождаться, пока сегмент подтвердится
		/// </summary>
		/// <param name="segmentRowNumber">номер сегмента</param>
		/// <returns>сегмент подтвердился</returns>
		protected bool WaitSegmentConfirm(int segmentRowNumber)
		{
			return EditorPage.WaitSegmentConfirm(segmentRowNumber);
		}

		/// <summary>
		/// Нажать Back в редакторе, проверить возврат в Workspace
		/// </summary>
		public void EditorClickHomeBtn()
		{
			// Нажать кнопку назад
			EditorPage.ClickHomeBtn();

			Assert.IsTrue(
					ProjectPage.WaitPageLoad(),
					"Ошибка: не зашли на страницу проекта");
		}

		/// <summary>
		/// Нажать Source, кликнуть Toggle (кнопку)
		/// </summary>
		public void SourceTargetSwitchButton(int segmentNumber)
		{
			//Выбрать target нужного сегмента
			EditorPage.ClickTargetCell(segmentNumber);
			// Нажать Toggle
			EditorPage.ClickToggleBtn();
		}

		/// <summary>
		/// Кликнуть Toggle (хоткей)
		/// </summary>
		public void SourceTargetSwitchHotkey(int segmentNumber)
		{
			// Нажать хоткей Tab
			EditorPage.TabByHotkey(segmentNumber);
		}

		/// <summary>
		/// Проверить Copy (кнопка)
		/// </summary>
		public void ToTargetButton(int rowNumber = 1)
		{
			// Выбрать source нужного сегмента
			EditorPage.ClickSourceCell(rowNumber);

			// Текст source'a нужного сегмента
			var sourcetxt = EditorPage.GetSourceText(rowNumber);

			// Нажать кнопку копирования
			EditorPage.ClickCopyBtn();

			// Проверить, такой ли текст в target'те
			var targetxt = EditorPage.GetTargetText(rowNumber);

			Assert.AreEqual(
				sourcetxt, 
				targetxt, 
				"Ошибка: после кнопки Copy текст в Source и Target не совпадает");
		}

		/// <summary>
		/// Проверить Copy (хоткей)
		/// </summary>
		public void ToTargetHotkey(int rowNumber = 1)
		{
			//Выбрать source сегмента
			EditorPage.ClickSourceCell(rowNumber);

			// Текст source'a сегмента
			string sourcetxt = EditorPage.GetSourceText(rowNumber);
			// Нажать хоткей копирования
			EditorPage.CopySourceByHotkey(rowNumber);


			// Проверить, такой ли текст в target'те
			string targetxt = EditorPage.GetTargetText(rowNumber);
			Assert.AreEqual(sourcetxt, targetxt, "Ошибка: после хоткея Copy текст в Source и Target не совпадает");
		}

		/// <summary>
		/// Перейти на страницу Domain
		/// </summary>
		protected void SwitchDomainTab()
		{
			MainHelperClass.ClickOpenDomainPage();
			DomainPage.WaitPageLoad();
		}

		/// <summary>
		/// Перейти на страницу TM
		/// </summary>
		protected void SwitchTMTab()
		{
			// Нажать кнопку перехода на страницу Базы Translation memory
			MainHelperClass.ClickOpenTMPage();
			TMPage.WaitPageLoad();
		}

		/// <summary>
		/// Перейти на страницу глоссариев
		/// </summary>
		protected void SwitchGlossaryTab()
		{
			MainHelperClass.ClickOpenGlossaryPage();
			GlossaryListPage.WaitPageLoad();
		}

		/// <summary>
		/// Создать глоссарий и вернуться к списку глоссариев
		/// </summary>
		/// <returns>название глоссария</returns>
		protected string CreateGlossaryAndReturnToGlossaryList(List<CommonHelper.LANGUAGE> languagesList = null, bool bNeedWaitSuccessSave = true)
		{
			// Получить уникальное имя для глоссария
			var glossaryName = GetUniqueGlossaryName();
			// Создать глоссарий
			CreateGlossaryByName(glossaryName,bNeedWaitSuccessSave, languagesList);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();

			return glossaryName;
		}

		/// <summary>
		/// Перейти на страницу поиска
		/// </summary>
		protected void SwitchSearchTab()
		{
			MainHelperClass.ClickOpenSearchPage();
			SearchPage.WaitPageLoad();
		}

		/// <summary>
		/// Перейти на страницу Workspace
		/// </summary>
		protected void SwitchWorkspaceTab()
		{
			MainHelperClass.ClickOpenWorkSpacePage();
			WorkspacePage.WaitPageLoad();
		}

		/// <summary>
		/// Инициировать поиск
		/// </summary>
		/// <param name="searchText"></param>
		protected void InitSearch(string searchText)
		{
			// Ввести слово для поиска
			SearchPage.AddTextSearch(searchText);
			// Нажать Перевести
			SearchPage.ClickTranslateBtn();
			SearchPage.WaitUntilShowResults();
		}

		/// <summary>
		/// Есть ли такой domain
		/// </summary>
		/// <param name="domainName">название</param>
		/// <returns>есть</returns>
		protected bool GetIsDomainExist(string domainName)
		{
			// TODO мб заменить вызов
			return DomainPage.GetIsDomainExist(domainName);
		}

		/// <summary>
		/// Создать Domain
		/// </summary>
		/// <param name="domainName">имя</param>
		/// <param name="shouldCreateOk">должен сохраниться</param>
		protected void CreateDomain(string domainName, bool shouldCreateOk = true)
		{
			// Нажать "Добавить проект"
			DomainPage.ClickCreateDomainBtn();
			// Ввести имя
			DomainPage.EnterNameCreateDomain(domainName);

			// Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
			Driver.Manage().Window.Maximize();

			// Сохранить новый проект
			DomainPage.ClickSaveDomain();
			if (shouldCreateOk)
			{
				Assert.IsTrue(DomainPage.WaitUntilSave(), "Ошибка: domain не сохранился!");
			}
			else
			{
				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Кликнуть галочку около проекта в списке проектов
		/// </summary>
		/// <param name="projectName">название проекта</param>
		protected void SelectProjectInList(string projectName)
		{
			WorkspacePage.SelectProject(projectName);
		}

		/// <summary>
		/// Открыть проект (со страницы Workflow)
		/// </summary>
		/// <param name="projectName">название проекта</param>
		protected void OpenProjectPage(string projectName)
		{
			WorkspacePage.OpenProjectPage(projectName);
			ProjectPage.WaitPageLoad();
		}

		/// <summary>
		/// Открыть свертку проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		protected void OpenProjectInfo(string projectName)
		{
			WorkspacePage.OpenProjectInfo(projectName);
		}

		/// <summary>
		/// Открыть вкладку workflow настроек из окна проекта
		/// </summary>
		protected void OpenWorkflowSettings()
		{
			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Открыть диалог создания глоссария
		/// </summary>
		protected void OpenCreateGlossary()
		{
			GlossaryListPage.ClickCreateGlossary();
			GlossaryEditForm.WaitPageLoad();
		}

		/// <summary>
		/// Создать новый глоссарий
		/// </summary>
		/// <param name="glossaryName">название</param>
		/// <param name="bNeedWaitSuccessSave">ожидается успешное сохранение</param>
		/// <param name="langList">список языков</param>
		protected void CreateGlossaryByName(
			string glossaryName, 
			bool bNeedWaitSuccessSave = true, 
			List<CommonHelper.LANGUAGE> langList = null)
		{
			// Открыть форму создания глоссария
			OpenCreateGlossary();

			// Ввести имя
			GlossaryEditForm.EnterGlossaryName(glossaryName);

			// Добавить комментарий
			GlossaryEditForm.EnterComment("Test Glossary Generated by Selenium");

			// Удалить все языки
			AllLanguagesToDefaultCreateGlossary();

			if (langList != null)
			{
				// Добавить языки
				foreach (var langID in langList)
				{
					AddLanguageCreateGlossary(langID);
				}
			}

			// Нажать сохранить
			GlossaryEditForm.ClickSaveGlossary();

			if (bNeedWaitSuccessSave)
			{
				// Ожидание успешного сохранения
				Assert.IsTrue(
					GlossaryPage.WaitPageLoad(), 
					"Ошибка: глоссарий не сохранился, не перешли на страницу глоссария");
			}
			else
			{
				Thread.Sleep(1000);
			}
		}
		
		/// <summary>
		/// Получить количество терминовв глоссарии
		/// </summary>
		/// <returns></returns>
		protected int GetCountOfItems()
		{
			return GlossaryPage.GetConceptCount();
		}

		/// <summary>
		/// Зайти в глоссарий
		/// </summary>
		/// <param name="glossaryName"></param>
		protected void SwitchCurrentGlossary(string glossaryName)
		{
			// Перейти на страницу глоссария
			GlossaryListPage.ClickGlossaryRow(glossaryName);

			Assert.IsTrue(GlossaryPage.WaitPageLoad(), "Ошибка: не зашли в глоссарий");
		}

		/// <summary>
		/// Открыть свойства глоссария
		/// </summary>
		protected void OpenGlossaryProperties()
		{
			// Нажать Редактирование
			GlossaryPage.OpenEditGlossaryList();
			// Нажать на Properties
			GlossaryPage.ClickOpenProperties();
		}

		/// <summary>
		/// Удаление глоссария
		/// </summary>
		protected void DeleteGlossary()
		{
			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();
			// Нажать Удалить глоссарий 
			GlossaryEditForm.ClickDeleteGlossary();

			// Нажать Да (удалить)
			GlossaryEditForm.ClickConfirmDeleteGlossary();
			GlossaryListPage.WaitPageLoad();
		}

		/// <summary>
		/// Добавить язык при создании глоссария
		/// </summary>
		/// <param name="lang">код языка</param>
		protected void AddLanguageCreateGlossary(CommonHelper.LANGUAGE lang)
		{
			// Кликнуть по Плюсу
			GlossaryEditForm.ClickAddLanguage();
			// Открыть выпадающий список у добавленного языка
			GlossaryEditForm.ClickLastLangOpenCloseList();

			// Выбрать язык
			Assert.IsTrue(GlossaryEditForm.GetIsExistLanguageInList(lang),
				"Ошибка: указанного языка нет в списке");

			GlossaryEditForm.SelectLanguage(lang);
		}

		/// <summary>
		/// Установить дефолтные языки (английский, русский)
		/// </summary>
		protected void AllLanguagesToDefaultCreateGlossary()
		{
			// Удалить все языки
			for (int i = 1; i < GlossaryEditForm.GetGlossaryLanguageCount(); i++)
			{
				GlossaryEditForm.ClickDeleteLanguage();
			}
			// Выставить первый английский
			GlossaryEditForm.ClickLastLangOpenCloseList();

			// Выбрать язык
			Assert.IsTrue(
				GlossaryEditForm.GetIsExistLanguageInList(CommonHelper.LANGUAGE.English),
				"Ошибка: указанного языка нет в списке");

			GlossaryEditForm.SelectLanguage(CommonHelper.LANGUAGE.English);
			// Кликнуть по Плюсу
			GlossaryEditForm.ClickAddLanguage();
			// Выставить второй русский
			GlossaryEditForm.ClickLastLangOpenCloseList();

			// Выбрать язык
			Assert.IsTrue(
				GlossaryEditForm.GetIsExistLanguageInList(CommonHelper.LANGUAGE.Russian),
				"Ошибка: указанного языка нет в списке");

			GlossaryEditForm.SelectLanguage(CommonHelper.LANGUAGE.Russian);
		}

		/// <summary>
		/// Создать проект с документом, открыть документ
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="withTM">с ТМ</param>
		/// <param name="withMT">с МТ</param>
		/// <param name="uploadDocument">путь к документу</param>
		protected void CreateReadyProject(
			string projectName, 
			bool withTM = true, 
			bool withMT = false, 
			string uploadDocument = "",
			Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT, 
			bool chooseGlossary = false, 
			string glossaryName = "")
		{
			// Создание проекта
			CreateProject(projectName, "", withTM, TestFile.EditorTMXFile, Workspace_CreateProjectDialogHelper.SetGlossary.None, glossaryName, withMT, mtType);

			//открытие настроек проекта
			uploadDocument = uploadDocument.Length == 0 ? TestFile.EditorTXTFile : uploadDocument;
			ImportDocumentProjectSettings(uploadDocument, projectName);

			// 3. Назначение задачи на пользователя
			AssignTask();

			// Добавляем созданный глоссарий
			if (glossaryName != "")
			{
				ProjectPage.SetGlossaryByName(glossaryName);
			}

			// 4. Открытие документа по имени созданного проекта
			OpenDocument();
		}

		/// <summary>
		/// Открытие диалога выбора исполнителя
		/// </summary>
		/// <param name="projectName">Имя проекта</param>
		protected void OpenAssignDialog(string projectName)
		{
			// Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(projectName);

			// Открываем инфо документа 
			WorkspacePage.OpenDocumentInfo(1);

			// Открываем окно с правами пользователя через кнопку прав
			WorkspacePage.ClickDocumentAssignBtn();

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(
				ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Выбирает из выпадающего списка пользователя или группу по имени
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <param name="name">Имя пользователя или группы</param>
		/// <param name="isGroup">Выбор группы</param>
		protected void SetResponsible(int rowNumber, string name, bool isGroup)
		{
			var fullName = "";

			// Открыть выпадающий список
			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(rowNumber);

			if (isGroup)
			{
				fullName = "Group: " + name;
			}
			else
			{
				fullName = name;
			}

			ResponsiblesDialog.WaitUntilUsersListDisplay(fullName);

			// Выбрать для заданной задачи имя исполнителя
			ResponsiblesDialog.SetVisibleResponsible(rowNumber, fullName);

			// Кликнуть подтверждение для заданной задачи
			ResponsiblesDialog.ClickAssignBtn(rowNumber);
		}

		/// <summary>
		/// Открывает словарь в редакторе
		/// </summary>
		protected void OpenEditorDictionary()
		{
			// Кликнуть по кнопке
			EditorPage.ClickDictionaryBtn();

			// Проверка, что открылась форма
			Assert.IsTrue(
				EditorPage.WaitDictionaryFormDisplay(),
				"Ошибка: Форма со словарем не открылась.");
		}

		/// <summary>
		/// Ждём, пока словарь прогрузится
		/// </summary>
		protected void WaitLoadDictionary()
		{
			//Проверяем, что загрузка словаря закончилась 
			Assert.IsTrue(
				EditorPage.WaitDictionaryLoadListWords(),
				"Ошибка: Не удалось дождаться окончания загрузки словаря.");
		}
		
		/// <summary>
		/// Создать уникальное имя
		/// </summary>
		/// <returns>имя</returns>
		protected string GetUniqueGlossaryName()
		{
			// Получить уникальное имя глоссария (т.к. добавляется точная дата и время, то не надо проверять, есть ли такой глоссарий в списке)
			// Явное приведение к строке стоит, чтобы не падал ArgumentOutOfRangeException. (неявное приведение даты иногда не отрабатывает корректно)
			return GlossaryName + DateTime.Now.ToString();
		}

		/// <summary>
		/// Вернуть, есть ли глоссарий с таким именем
		/// </summary>
		/// <param name="glossaryName">навзание</param>
		/// <returns>есть</returns>
		protected bool GetIsExistGlossary(string glossaryName)
		{
			// Получить: существует ли глоссарий с таким именем
			return GlossaryListPage.GetIsExistGlossary(glossaryName);
		}

		/// <summary>
		/// Дождаться автосохранения сегментов
		/// </summary>
		protected void AutoSave()
		{
			// Дожидаемся сохранения сегментов
			Assert.IsTrue(
				EditorPage.WaitUntilAllSegmentsSave(),
				"Ошибка: Не проходит автосохранение.");
		}

		/// <summary>
		/// Завершение работы с Driver
		/// </summary>
		protected void ExitDriver()
		{
			if (Driver != null)
			{
				// Закрыть драйвер
				Driver.Quit();
				// Очистить, чтобы при следующем тесте пересоздавалось
				Driver = null;
			}
		}

		/// <summary>
		/// Закрываем модальный диалог
		/// </summary>
		protected void AcceptModalDialog()
		{
			try
			{
				if (Driver.SwitchTo().Alert().Text.
					Contains("Эта страница просит вас подтвердить, что вы хотите уйти — при этом введённые вами данные могут не сохраниться.") ||
					Driver.SwitchTo().Alert().Text.
					Contains("This page is asking you to confirm that you want to leave - data you have entered may not be saved."))
					Driver.SwitchTo().Alert().Accept();

				Thread.Sleep(500);

				if (Driver.SwitchTo().Alert().Text.Contains("Failed to send the request to the server. An error occurred while contacting the server."))
					Driver.SwitchTo().Alert().Accept();

				Thread.Sleep(500);
			}
			catch (NoAlertPresentException)
			{
				TryCloseExternalDialog();
			}
		}

		/// <summary>
		/// Закрываем окно с вопросом о Workflow
		/// </summary>
		protected void AcceptWorkflowModalDialog()
		{
			try
			{
				if (Driver.SwitchTo().Alert().Text.
					Contains("Включение функции workflow для аккаунта необратимо, обратное выключение будет невозможно. Продолжить?"));// ||
					Driver.SwitchTo().Alert().Accept();

				Thread.Sleep(500);
			}
			catch (NoAlertPresentException)
			{
				TryCloseExternalDialog();
			}
		}
		/// <summary>
		/// Задает русский язык в Target при создании проекта
		/// </summary>
		protected void SetRusLanguageTarget()
		{
			// Проверить, что в Target русский язык
			var langList = WorkspaceCreateProjectDialog.GetTargetLanguageList();

			if (langList.Count != 1 || langList[0] != "Russian")
			{
				// Открыть список Target
				WorkspaceCreateProjectDialog.ClickTargetList();
				// Кликаем по всем выбранным языкам, чтобы снять галки
				WorkspaceCreateProjectDialog.ClickAllSelectedTargetItems();
				// Выбираем русский язык
				WorkspaceCreateProjectDialog.ClickTargetItem(CommonHelper.LANGUAGE.Russian);
				// Закрыть список Target
				WorkspaceCreateProjectDialog.ClickTargetList();
			}
		}

		/// <summary>
		/// Пропуск подтверждения отсутствия TM при создании проекта
		/// </summary>
		protected void SkipNotSelectedTM()
		{
			// В случае, если диалог открыт
			if (WorkspaceCreateProjectDialog.WaitUntilConfirmTMDialogDisplay())
			{
				// Жмем Skip
				WorkspaceCreateProjectDialog.ClickSkipBtn();

				// Ждем пока диалог не пропадет
				Assert.IsTrue(
					WorkspaceCreateProjectDialog.WaitUntilConfirmTMDialogDisappear(),
					"Ошибка: после нажатия кнопки Skip диалог подтверждения не выбранной ТМ не закрылся.");
			}
		}
		
		/// <summary>
		/// Метод подстановки из САТ, возвращает номер строки, из которой произведена подстановка
		/// </summary>
		/// <param name="segmentNumber">номер сегмента таргет для подстановки  в него</param>
		/// <param name="catType">тип подстановки из CAT</param>
		/// <returns>номер строки CAT из которой произвели подстановку</returns>

		protected int PasteFromCatReturnCatLineNumber(int segmentNumber, EditorPageHelper.CAT_TYPE catType)
		{

			//Выбираем сегмент
			EditorPage.ClickTargetCell(segmentNumber);

			var catLineNumber = EditorPage.GetCATTranslationRowNumber(catType);

			//Нажать хоткей для подстановки из TM перевода сегмента
			EditorPage.PutCatMatchByHotkey(segmentNumber, catLineNumber);

			// Дождаться автосохранения
			Assert.IsTrue(
				EditorPage.WaitUntilAllSegmentsSave(),
				"Ошибка: Не проходит автосохранение.");

			return catLineNumber;
		}

		/// <summary>
		/// Создание нового глоссария из указанных слов словаря
		/// </summary>
		/// <param name="dict">Словарь</param>
		protected void SetGlossaryByDictinary(Dictionary<string, string> dict)
		{
			foreach (var pair in dict)
			{
				// Нажать New item
				GlossaryPage.ClickNewItemBtn();
				// Дождаться появления строки для ввода
				GlossaryPage.WaitConceptTableAppear();
				// Заполнить термин
				GlossaryPage.FillTerm(1, pair.Key);
				GlossaryPage.FillTerm(2, pair.Value);

				// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
				Driver.Manage().Window.Maximize();
				// Нажать Сохранить
				GlossaryPage.ClickSaveTermin();
				GlossaryPage.WaitConceptGeneralSave();
				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Переход на страницу регистрации
		/// </summary>
		/// <param name="client">компания или пользователь</param>
		public void GoToRegistrationPage(RegistrationType client)
		{
			try
			{
				if (client == RegistrationType.Company)
				{
					Driver.Navigate().GoToUrl(Url + "/corp-reg");
				}
				else
				{
					Driver.Navigate().GoToUrl(Url + "/freelance-reg");
				}
			}
			catch
			{
				Driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToRegistrationPage(client);
			}
		}

		/// <summary>
		/// Добавляет сорс-таргет сегмента в глоссарий для теста
		/// </summary>
		///  <param name="sourceTerm">слово для внесения в словарь</param>
		///  <param name="targetTerm">перевод слова словаря</param>
		public void AddTermGlossary(string sourceTerm, string targetTerm)
		{
			// Добавить сорс
			AddTermForm.TypeSourceTermText(sourceTerm);
			// Добавить термин в таргет
			AddTermForm.TypeTargetTermText(targetTerm);
			// Нажать сохранить
			AddTermForm.ClickAddBtn();

			Thread.Sleep(2000);
			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
		}

		/// <summary>
		/// Пересоздание Helper'ов с новыми Driver, Wait
		/// </summary>
		private void recreateDrivers()
		{
			ProjectPage = new ProjectPageHelper(Driver, Wait);
			EditorPage = new EditorPageHelper(Driver, Wait);
			LoginPage = new LoginPageHelper(Driver, Wait);
			WorkspacePage = new WorkSpacePageHelper(Driver, Wait);
			WorkspaceCreateProjectDialog = new Workspace_CreateProjectDialogHelper(Driver, Wait);
			MainHelperClass = new MainHelper(Driver, Wait);
			DomainPage = new DomainPageHelper(Driver, Wait);
			TMPage = new TMPageHelper(Driver, Wait);
			GlossaryListPage = new GlossaryListPageHelper(Driver, Wait);
			GlossaryPage = new GlossaryPageHelper(Driver, Wait);
			SearchPage = new SearchPageHelper(Driver, Wait);
			ClientPage = new ClientPageHelper(Driver, Wait);
			AdminPage = new AdminPageHelper(Driver, Wait);
			GlossaryEditStructureForm = new GlossaryEditStructureFormHelper(Driver, Wait);
			DictionaryPage = new DictionaryPageHelper(Driver, Wait);
			RevisionPage = new Editor_RevisionPageHelper(Driver, Wait);
			UserRightsPage = new UserRightsPageHelper(Driver, Wait);
			SuggestTermDialog = new SuggestTermDialogHelper(Driver, Wait);
			GlossaryEditForm = new GlossaryEditFormHelper(Driver, Wait);
			GlossarySuggestPage = new GlossarySuggestPageHelper(Driver, Wait);
			CatPanel = new CatPanelResultsHelper(Driver, Wait);
			ResponsiblesDialog = new ResponsiblesDialogHelper(Driver, Wait);
			AddTermForm = new AddTermFormHelper(Driver, Wait);
			RegistrationPage = new RegistrationPageHelper(Driver, Wait);
			MyAccountPage = new MyAccountPageHelper(Driver, Wait);
			GlossaryTermFilterPage = new GlossaryTermFilterHelper(Driver, Wait);
		}

		private void initializeUsersAndCompanyList()
		{
			if (TestUserFileExist())
			{
				var cfgTestUser = TestSettingDefinition.Instance.Get<TestUserConfig>();
				var cfgTestCompany = TestSettingDefinition.Instance.Get<TestUserConfig>();
				var cfgCourseraUser = TestSettingDefinition.Instance.Get<TestUserConfig>();


				TestUserList = new List<UserInfo>();
				TestCompanyList = new List<UserInfo>();
				CourseraUserList = new List<UserInfo>();

				foreach (var user in cfgTestUser.Users)
				{
					TestUserList.Add(
						new UserInfo(
							user.Login, 
							user.Password, 
							user.Activated));
				}


				foreach (var user in cfgTestCompany.Companies)
				{
					TestCompanyList.Add(
						new UserInfo(
							user.Login, 
							user.Password, 
							user.Activated));
				}

				foreach (var user in cfgCourseraUser.CourseraUsers)
				{
					CourseraUserList.Add(
						new UserInfo(
							user.Login, 
							user.Password,
							user.Activated));
				}

			}
		}

		private void initializeRelatedToServerFields(TargetServerConfig cfgAgentSpecific)
		{
			WorkspaceUrl = cfgAgentSpecific.Workspace;
			Standalone = cfgAgentSpecific.Standalone;
			if (Standalone)
			{
				Url = cfgAgentSpecific.Url;
			}
			else
			{
				Url = "https://" + cfgAgentSpecific.Url;
			}
			
			if (string.IsNullOrWhiteSpace(WorkspaceUrl))
			{
				WorkspaceUrl = Url + "/workspace";
			}

			AdminUrl = cfgAgentSpecific.Url + ":81";
		}

		private void initializeRelatedToUserFields(UserInfoConfig cfgUserInfo)
		{
			Login = cfgUserInfo.Login;
			Password = cfgUserInfo.Password;
			UserName = cfgUserInfo.UserName;

			Login2 = cfgUserInfo.Login2;
			Password2 = cfgUserInfo.Password2;
			UserName2 = cfgUserInfo.UserName2;
		}

		private string _pathTestFiles;
		private FirefoxProfile _profile;
	}
}
