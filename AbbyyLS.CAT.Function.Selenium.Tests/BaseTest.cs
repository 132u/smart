using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using NConfiguration;
using NLog;

using AbbyyLS.CAT.Function.Selenium.Tests.CheckRights;
using AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures;
using AbbyyLS.CAT.Function.Selenium.Tests.Driver;
using AbbyyLS.CAT.Function.Selenium.Tests.Editor.Panel;
using AbbyyLS.CAT.Function.Selenium.Tests.Workspace.Domains;
using AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Базовый тест
	/// </summary>
	[TestFixture(typeof(ChromeWebDriverSettings))]
	//[TestFixture(typeof(IEWebDriverSettings))]
	//[TestFixture(typeof(FirefoxWebDriverSettings))]
	public class BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Конструктор базового теста
		/// </summary>
		public BaseTest()
		{
			try
			{
				var cfgAgentSpecific = TestSettingDefinition.Instance.Get<TargetServerConfig>();
				var cfgUserInfo = TestSettingDefinition.Instance.Get<UserInfoConfig>();

				CreateUniqueNamesByDatetime();
				initializeRelatedToServerFields(cfgAgentSpecific);
				initializeRelatedToUserFields(cfgUserInfo);
				initializeUsersAndCompanyList();
			}
			catch (Exception ex)
			{
				ExitDriver();
				Logger.ErrorException("Ошибка в конструкторе : " + ex.Message, ex);

				throw;
			}
		}
		
		protected static IWebDriver Driver { get; private set; }

		protected WebDriverWait Wait { get; private set; }

		protected static string[] ProcessNames { get; private set; }

		protected string Url { get; private set; }

		protected string WorkspaceUrl { get; private set; }

		protected bool Standalone { get; private set; }

		protected bool EmailAuth { get; private set; }

		protected string AdminUrl { get; private set; }

		protected string Login { get; private set; }

		protected string Password { get; private set; }

		protected string UserName { get; private set; }

		protected string Login2 { get; private set; }

		protected string Password2 { get; private set; }

		protected string UserName2 { get; private set; }

		protected string TestRightsLogin { get; private set; }

		protected string TestRightsPassword { get; private set; }

		protected string TestRightsUserName { get; private set; }

		protected List<UserInfo> TestUserList { get; private set; }

		protected List<UserInfo> TestCompanyList { get; private set; }

		protected List<UserInfo> CourseraUserList { get; private set; }
		protected List<UserInfo> AolUserList { get; private set; }
		protected string ProjectUniqueName { get; private set; }
		
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
		protected SocialNetworkHelper SocialNetworkPage { get; private set; }
		protected CheckCreateProjectRightHelper CheckCreateProjectRightHelper { get; private set; }

		protected DateTime TestBeginTime { get; private set; }

		protected bool QuitDriverAfterTest { get; set; }

		public const string DeadlineDate = "03/03/2016";
		public const string ConstTMName = "TestTM";
		public const string GlossaryName = "TestGlossary";
		public const string GlossaryUniqueName = "TestGlossaryEditStructureTermLevelUniqueName";
		public const string ProjectNameExportTestOneDoc = "TestProjectTestExportOneDocumentUniqueName";
		public const string ProjectNameExportTestMultiDoc = "TestProjectTestExportMultiDocumentsUniqueName";

		/// <summary>
		/// Начальная подготовка для группы тестов
		/// </summary>
		[TestFixtureSetUp]
		public void SetupAllBase()
		{
			createDriver();
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
			Logger.Info(TestContext.CurrentContext.Test.Name + "\nStart: " + TestBeginTime);

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
			if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
			{
				// Если при попытке сделать скриншот вылетела ошибка, закрываем драйвер
				if (!takeScreenshot())
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
			Logger.Info("Finish: " + testFinishTime);
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
			Logger.Info(durResult);

			if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
			{
				// Если тест провалился
				Logger.Info("Fail!");
			}
		}

		/// <summary>
		/// Снимаем скриншот
		/// </summary>
		/// <returns>Возвращает: получилось или нет сделать скриншот</returns>
		private static bool takeScreenshot()
		{
			try
			{
				var screenshotDriver = Driver as ITakesScreenshot;
				
				var failResultPath = Path.Combine(PathProvider.ResultsFolderPath, "FailedTests");
				Directory.CreateDirectory(failResultPath);

				var nameParts = TestContext.CurrentContext.Test.FullName.Split('.');
				var className = nameParts[nameParts.Length - 2].Replace('<', '(').Replace('>', ')');

				var screenName = TestContext.CurrentContext.Test.Name;
				// Убрать из названия теста аргументы (файлы)
				if (screenName.Contains("("))
					screenName = screenName.Substring(0, screenName.IndexOf("("));

				screenName = className + "." + screenName + DateTime.Now.ToString(" yyyy.MM.dd HH.mm.ss") + ".png";
				screenName = Path.Combine(failResultPath, screenName);

				var screenshot = screenshotDriver.GetScreenshot();
				screenshot.SaveAsFile(screenName, ImageFormat.Png);

				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorException(String.Format("Произошла ошибка при попытке сделать скриншот. Тест: {0} Ошибка: {1}",
					TestContext.CurrentContext.Test.Name,
					ex.Message), 
					ex);

				return false;
			}
		}

		public bool TestUserFileExist()
		{
			return File.Exists(PathProvider.TestUserFile);
		}

		public bool CourseraUserFileExist()
		{
			return File.Exists(PathProvider.CourseraUserFile);
		}

		public bool AolUserFileExist()
		{
			return File.Exists(PathProvider.AolUserFile);
		}

		/// <summary>
		/// Обновить уникальные имена для нового теста
		/// </summary>
		protected void CreateUniqueNamesByDatetime()
		{
			ProjectUniqueName = "Test Project" + "_" + DateTime.UtcNow.Ticks;
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
			if (Driver == null)
			{
				var webDriverSettings = new TWebDriverSettings();
				Driver = webDriverSettings.Driver;
				ProcessNames = webDriverSettings.ProcessNames;
			}
			
			SetDriverTimeoutDefault();
			Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));

			Driver.Manage().Window.Maximize();

			recreateDrivers();
		}

		/// <summary>
		/// Установить время ожидания драйвера в минимум (для поиска элементов, которых по ожиданию нет)
		/// </summary>
		protected void SetDriverTimeoutMinimum()
		{
			Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
		}

		/// <summary>
		/// Установить стандартное время ожидание драйвера
		/// </summary>
		protected void SetDriverTimeoutDefault()
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
			
			// Ожидаем появления диалога Progress
			ProjectPage.WaitProgressDialogOpen();

			// Назначить ответственного в окне Progress
			ProjectPage.ClickUserNameCell();
			
			// Выбрать нужное имя
			ProjectPage.ClickAssignUserListUser(UserName);
			ProjectPage.WaitAssignBtnDisplay();

			// Нажать на Assign
			ProjectPage.ClickAssignBtn();
			// Дождаться появления Cancel
			ProjectPage.WaitCancelAssignBtnDisplay();

			// Нажать на Close
			ProjectPage.CloseAssignDialogClick();
			Thread.Sleep(1000);// Sleep не убирать, необходим для корректной работы в Chrome

			ProjectPage.SelectDocument(documentRowNum);
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
			ProjectPage.ClickUploadTmButton();
			//Заполнить название ТМ в диалоге
			ProjectPage.FillTmNameDialog(tmName);
			//Загрузить документ
			ProjectPage.UploadTmxFile(file);
			//Подтвердить создание ТМ
			ProjectPage.ClickConfirmTmCreation();
			Assert.IsTrue(ProjectPage.ClickRadioBtm(),"TM не выбран");
			// Сохранить изменения
			ProjectPage.ClickSaveTMBtn();
			// Дождаться окончания загрузки
			ProjectPage.WaitDocumentDownloadFinish();
		}

		/// <summary>
		/// Настройка претранслейта со страницы проекта.
		/// </summary>
		/// <remarks>
		/// tmName == null => выбирается деволтный МТ
		/// tmName != null => выбирается TM с именем tmName
		/// </remarks>
		/// <param name="tmName">имя tm для претранслейта</param>
		public void SetPretranslate(string tmName = null)
		{
			// нажать кнопку претранслейт
			ProjectPage.ClickPretranslateBtn();
			// создать новое правило
			ProjectPage.ClickNewRuleBtn();
			// открыть меню источника претранслейта
			ProjectPage.ClickSourcePretranslateBtn();

			if (tmName == null)
			{
				ProjectPage.ClickAbbyyMtForPretranslate();
			}
			else
			{
				// выбрать ТМ как источник
				ProjectPage.ClickTmForPretranslateBtn(tmName);
			}
			
			// сохранить настройки и выполнить претранслейт
			ProjectPage.ClickSavePretranslateBtn();
		}

		/// <summary>
		/// Метод открытия документа в редакторе для выполнения конкретного задания
		/// </summary>
		/// <param name="documentNumber">Номер документа в списке на странице проекта</param>
		/// <param name="taskId">Тип выполняемого задания: 1)перевод 2)редактура 3)корректура</param>
		protected void OpenDocument(int documentNumber = 1, int taskId = 1)
		{
			ProjectPage.OpenDocument(documentNumber);

			if (ResponsiblesDialog.WaitUntilChooseTaskDialogDisplay())
			{
				switch (taskId)
				{
					case 1:

						EditorPage.AssertionTranslationTaskBtnIsExist();
						EditorPage.ClickTranslationTaskBtn();
						break;

					case 2:
						EditorPage.AssertionEditingTaskBtnIsExist();
						EditorPage.ClickEditingTaskBtn();
						break;

					case 3:
						EditorPage.AssertionProofreadingTaskBtnIsExist();
						EditorPage.ClickProofreadingTaskBtn();
						break;
				}

				EditorPage.ClickContBtn();
			}

			EditorPage.AssertionIsPageLoad();
			Thread.Sleep(1000);

			EditorPage.CloseTutorial();
			// Проверить, существует ли хотя бы один сегмент
			Assert.IsTrue(EditorPage.GetSegmentsExist(), "Ошибка: нет сегментов");
		}

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="authLogin">логин пользователя</param>
		/// <param name="authPassword">пароль пользователя</param>
		/// <param name="accountName">Имя аккаунта</param>
		/// <param name="dataServer">Расположение сервера</param>
		public void Authorization(
			string authLogin,
			string authPassword,
			string accountName = "TestAccount",
			string dataServer = "Europe")
		{
			Logger.Debug("Прохождение процедуры авторизации пользователя");

			if (Standalone)
			{
				if (EmailAuth)
				{
					Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.LogIn);

					LoginPage.EnterLoginAuthEmail(authLogin);
					LoginPage.EnterPasswordAuthEmail(authPassword);
					LoginPage.ClickSubmitAuthEmail();
				}
				else
				{
					Driver.Navigate().GoToUrl(Url);
				}
			}
			else
			{
				Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.SingIn);

				// Проверить, загрузилась ли
				Assert.IsTrue(LoginPage.WaitPageLoad(),
					"Не прогрузилась страница Login - возможно, сайт недоступен");
				LoginPage.SelectLocale(LOCALE_LANGUAGE_SELECT.English);
				// Заполнить логин и пароль
				LoginPage.EnterLogin(authLogin);
				LoginPage.EnterPassword(authPassword);
				LoginPage.ClickSubmitCredentials();

				if (LoginPage.GetIsErrorExist())
				{
					Assert.Fail("Появилась ошибка при входе! М.б.недоступен AOL.");
				}

				if (LoginPage.GetAccountsCount() == 1 && !LoginPage.IsOneOfServersNotRespondingErrorExist())
				{
					Assert.Fail("Если аккаунт единственный, должно сразу осуществляться перенапралвение  в него.");
				}
					
				if (!WorkspacePage.WaitPageLoad())
				{
					if (!LoginPage.WaitAccountExist(accountName, waitmax: 8, dataServer: dataServer))
					{
						Assert.Fail("В списке аккаунтов нет нужного " + accountName);
					}
					// Выбрать аккаунт
					LoginPage.ClickAccountName(accountName);
					WorkspacePage.WaitPageLoad();
				}
					
				//проверка того,что мы в нужном аккаунте
				//(на случай,когда у пользователя один аккаунт и это не нужный нам аккаунт)
				var currentAccountName = WorkspacePage.GetCompanyName();
				if (currentAccountName != accountName)
				{
					Assert.Fail("Ошибка: не зашли в аккаунт:" + accountName + ". У пользователя один аккаунт (" + currentAccountName + ") и произошло автоматическое перенаправление в него.");
				}
				// иначе у пользователя только 1 аккаунт
			}

			// Проверяем, что выставлен Английский язык
			WorkspacePage.WaitAppearLocaleBtn();
			Assert.AreEqual(LOCALE_LANGUAGE_SELECT.English, WorkspacePage.GetCurrentLocale(),
				string.Format("Ошибка: Должен быть выставлен {0} язык.", LOCALE_LANGUAGE_SELECT.English));
		}

		/// <summary>
		/// Переход на admin страницу
		/// </summary>
		public void GoToAdminPage()
		{
			try
			{
				Logger.Trace("Переход в админку");
				Driver.Navigate().GoToUrl(AdminUrl);
			}
			catch
			{
				Logger.Trace("Обновляем страницу");
				Driver.Navigate().Refresh();
				Logger.Trace("Закрываем модальное диалоговое окно");
				AcceptModalDialog();
				GoToAdminPage();
			}
		}

		/// <summary>
		/// Переход по переданному relativeUrl, который указывается без базовой части.
		/// т.е. для перехода на smartcat.stageN.als.local/Workspace передаём аргумент "/Workspace".
		/// Если переадресация на стартовую страницу, то авторизация и затем переход
		/// </summary>
		/// <param name="relativeUrl">адрес для перехода</param>
		/// <param name="accountName">аккаунт для авторизации, если требуется авторизоваться</param>
		public void GoToUrl(string relativeUrl, string accountName = "TestAccount")
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				Logger.Info("Переходим на страницу:" + Url + relativeUrl);

				Driver.Navigate().GoToUrl(Url + relativeUrl);

				AcceptModalDialog();
				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					Authorization(Login, Password, accountName);
					Driver.Navigate().GoToUrl(Url + relativeUrl);
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Ошибка при переходе на стр " + Url + relativeUrl + ": " + ex.Message, ex);

				//// Если обновить в этом месте страницу в Google Chrome, тест упадёт, 
				//// т.к. Google Chrome сразу требует закрыть модальный диалог.
				if (typeof(TWebDriverSettings) != typeof(ChromeWebDriverSettings))
				{
					Driver.Navigate().Refresh();
				}

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Перейти на страницу второй раз
				Driver.Navigate().GoToUrl(Url + relativeUrl);

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					// Проходим процедуру авторизации
					Authorization(Login, Password, accountName);
					// Переходим на страницу
					Driver.Navigate().GoToUrl(Url + relativeUrl);
				}
			}
		}

		/// <summary>
		/// Переход на страницу My Account
		/// Если переадресация на стартовую страницу, то авторизация и затем переход по клику MyAccount в панели WS
		/// </summary>
		public void GoToMyAccount()
		{
			// Перейти на страницу
			Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.LicensePackages);

			// Если открылась страница логина
			if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
			{
				// Проходим процедуру авторизации
				Authorization(Login, Password);

				// Переходим к лицензиям
				WorkspacePage.ClickAccount();
				WorkspacePage.ClickLicensesAndServices();

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
			Logger.Debug("Заполнение полей на первой странице создания проекта.");
			Assert.IsTrue(WorkspacePage.WaitPageLoad(), "Страница workspace не прогрузилась");

			WorkspacePage.ClickCreateProject();
			Assert.IsTrue(WorkspaceCreateProjectDialog.WaitDialogDisplay(), "Произошла ошибка:\n диалог создания проекта не открылся.");
			WorkspaceCreateProjectDialog.FillProjectName(projectName);
			WorkspaceCreateProjectDialog.FillDeadlineDate(DeadlineDate);
			WorkspaceCreateProjectDialog.SelectSourceLanguage(srcLang);
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
		/// <param name="isNeedCheckProjectAppearInList">Нужна проверка проекта в списке</param>
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
			bool isNeedCheckProjectAppearInList = true,
			int translationTasksNumber = 1,
			int editingTasksNumber = 0,
			int proofreadingTasksNumber = 0)
		{
			Logger.Info(string.Format("Создание нового проекта {0}", projectName));
			
			FirstStepProjectWizard(projectName);
			
			if (downloadFile.Length > 0)
			{
					WorkspaceCreateProjectDialog.UploadFileToNewProject(downloadFile);
					WorkspaceCreateProjectDialog.WaitDocumentAppear(Path.GetFileName(downloadFile));
			}
			if (WorkspaceCreateProjectDialog.GetProjectName() != projectName)
				WorkspaceCreateProjectDialog.FillProjectName(projectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			if (WorkspaceCreateProjectDialog.IsWorkflowStepPresented())
			{
				Logger.Debug("Вторая страница создания проекта - этап workflow.");
				SetUpWorkflow(
						translationTasksNumber,
						editingTasksNumber,
						proofreadingTasksNumber);
				WorkspaceCreateProjectDialog.ClickNextStep();
			}

			Logger.Debug("Третяя страница создания проекта - выбор ТМ.");
			if (createNewTM)
			{
				CreateNewTM(tmFile);
				WorkspaceCreateProjectDialog.ClickNextStep();
			}
			else
			{
				if (!WorkspaceCreateProjectDialog.GetIsTMTableNotEmpty())
				{
					WorkspaceCreateProjectDialog.ClickNextStep();
					SkipNotSelectedTM();
				}
				else
				{
					ChooseFirstTMInList();
					WorkspaceCreateProjectDialog.ClickNextStep();
				}
			}

			Logger.Debug("Четвертая страница создания проекта - выбор глоссария.");
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
			
			Thread.Sleep(2000); // Периодически глоссарий не успевает подгружаться
			WorkspaceCreateProjectDialog.ClickNextStep();

			Logger.Debug("Пятая страница создания проекта - выбор МТ.");
			if (!Standalone && (chooseMT && mtType != Workspace_CreateProjectDialogHelper.MT_TYPE.None))
			{
				WorkspaceCreateProjectDialog.ChooseMT(mtType);
			}

			WorkspaceCreateProjectDialog.ClickNextStep();


			Logger.Debug("Шестая страница создания проекта - настройка Pretranslate.");
			//Pretranslate();
			
			Thread.Sleep(500);
			WorkspaceCreateProjectDialog.ClickFinishCreate();

			if (isNeedCheckProjectAppearInList)
			{
				WorkspacePage.WaitProjectAppearInList(projectName);
			}

			WorkspacePage.WaitProjectLoad(projectName);

			// НЕ удалять! требует дальнейшей отладки, которая возможна,
			// только когда МТ/ТМ/TB не будут загружаться в документ
			//Assert.IsTrue(
			//	!WorkspacePage.GetWarningIsDisplayForProject(projectName),
			//	"Ошибка: при создании проекта не загрузился TB/TM/MT(появился Warning треугольник около названия проекта)");
		}

		/// <summary>
		/// Создать проект и не проверять, создался ли он
		/// </summary>
		/// <param name="projectName">название проекта</param>
		protected void CreateProjectWithoutCheckExist(string projectName)
		{
			CreateProject(projectName, isNeedCheckProjectAppearInList: false);
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
		/// <param name="tmFileName">файл для загрузки в ТМ</param>
		/// <param name="fromWorkspaceOrProject">true - из workspace, false - из проекта</param>
		public void CreateNewTM(string tmFileName, bool fromWorkspaceOrProject = true)
		{
			Logger.Debug("Создание новой ТМ в диалоге создания проекта");

			var tmName = "TestTM" + DateTime.Now.Ticks;

			if (tmFileName.Length > 0)
			{
				WorkspaceCreateProjectDialog.ClickUploadTMX();
				WorkspaceCreateProjectDialog.WaitCreateTMDialog();
				WorkspaceCreateProjectDialog.UploadTMInNewProject(tmFileName);
				WorkspaceCreateProjectDialog.FillTMName(tmName);
				WorkspaceCreateProjectDialog.ClickSaveTM();
				WorkspaceCreateProjectDialog.WaitTmxAppear(tmName);

				if (WorkspaceCreateProjectDialog.GetIsExistErrorFileMessage())
				{
					// Диалог загрузки документа не закрылся - закрываем
					TryCloseExternalDialog();
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
				WorkspaceCreateProjectDialog.WaitTmxAppear(tmName);
			}
		}

		public void ChooseFirstTMInList()
		{
			Logger.Debug("Выбрать первую ТМ из списка доступных ");

			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsTMTableNotEmpty(),
				"Ошибка: пустая таблица TM");

			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
		}
		
		public void CreateAndAddGlossary()
		{
			Logger.Debug("Создание и подключение нового глоссария");

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
				Logger.Debug("Меняем первую задачу на редактуру");
				WorkspaceCreateProjectDialog.SetWorkflowEditingTask(1);

				Logger.Trace("Количество заданий по редактуре уменьшаем на один - одно задание уже поставили");
				editingTasksNumber--;
			}
			// если в документе нет задач перевода и редактуры, поменять стоящий по умолчанию перевод на корректуру
			else if (translationTasksNumber == 0 && editingTasksNumber == 0)
			{
				Logger.Debug("Меняем первую задачу на корректуру");
				WorkspaceCreateProjectDialog.SetWorkflowProofreadingTask(1);

				Logger.Trace("Количество заданий по корректуре уменьшаем на один - одно задание уже поставили");
				proofreadingTasksNumber--;
			}

			//номер этапа в списке этапов
			var taskNumber = 2;

			Logger.Debug(string.Format("Cоздать необходимое количество заданий перевода: {0}", translationTasksNumber));
			for (var i = 1; i < translationTasksNumber; i++)
			{
				WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
				WorkspaceCreateProjectDialog.SetWorkflowTranslationTask(taskNumber);

				taskNumber++;
			}

			Logger.Debug(string.Format("Cоздать необходимое количество заданий редактирования: {0}", editingTasksNumber));
			for (var i = 0; i < editingTasksNumber; i++)
			{
				WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
				WorkspaceCreateProjectDialog.SetWorkflowEditingTask(taskNumber);

				taskNumber++;
			}

			Logger.Debug(string.Format("Cоздать необходимое количество заданий Proofreading: {0}", proofreadingTasksNumber));
			for (var i = 0; i < proofreadingTasksNumber; i++)
			{
				WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
				WorkspaceCreateProjectDialog.SetWorkflowProofreadingTask(taskNumber);

				taskNumber++;
			}
		}

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
			OpenProjectPage(projectName);

			ProjectPage.ClickImportBtn();

			ProjectPage.WaitImportDialogDisplay();

			ProjectPage.UploadFileOnProjectPage(filePath);

			ProjectPage.ClickNextImportDialog();

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
				ProjectPage.WaitDocumentDownloadFinish();
			}
			else
			{
				// Next
				ProjectPage.ClickNextImportDialog();

				// Нажать Finish
				ProjectPage.ClickFinishImportDialog();

				// Дождаться окончания загрузки
				ProjectPage.WaitDocumentDownloadFinish();
			}
		}

		/// <summary>
		/// Проверить, есть ли проект в списке на странице Workspace
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <returns>есть</returns>
		protected bool GetIsExistProject(string projectName)
		{
			Logger.Trace(string.Format("Проверка, есть ли проект {0} в списке на странице Workspace.", projectName));
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
			WorkspacePage.ClickConfirmDelete();

			// Дождаться, пока пропадет диалог подтверждения удаления
			WorkspacePage.WaitUntilDeleteConfirmFormDisappear();

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

		protected void TryCloseExternalDialog()
		{
			Logger.Debug("Закрываем открытый диалог");
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
		/// Проверка сохраненного документа и перемещение его в папку для выгрузки
		/// </summary>
		/// <param name="subFolderName">название папки для выгрузки</param>
		/// <param name="fileExtension">расширение</param>
		/// <param name="time">время выгрузки</param>
		protected void CheckAndMoveUserLogs(
			string subFolderName,
			string fileExtension, 
			string time)
		{
			string logFile = null;
			
			for (var i = 0; i < 5; i++)
			{
				var logFiles = Directory.GetFiles(PathProvider.ResultsFolderPath, "UserActivitiesLog*");

				if (logFiles.Any())
				{
					logFile = logFiles.First();
					break;
				}

				Thread.Sleep(1000);
			}

			Assert.IsTrue(logFile != null, "Ошибка: файл не экспортировался после пяти секунд ожидания");

			var newFileName = DateTime.Now.Ticks.ToString();
			var resultPath = Path.Combine(PathProvider.ResultsFolderPath, subFolderName, time);

			Directory.CreateDirectory(resultPath);
			
			resultPath = Path.Combine(resultPath, newFileName + fileExtension);
			
			File.Move(logFile, resultPath);
			File.Delete(logFile);
		}

		public void CleanResultFolderFromActivityLogs()
		{
			var activityLogs = Directory.GetFiles(PathProvider.ResultsFolderPath, "UserActivitiesLog*");

			foreach (var log in activityLogs)
			{
				File.Delete(log);
			}
		}

		public void AddTranslationAndConfirm(int segmentRowNumber = 1, string text = "Translation")
		{
			Logger.Debug("Добавить перевод и подтвердить");

			EditorPage.AddTextTarget(segmentRowNumber, text);
			
			EditorPage.ClickConfirmBtn();

			if (EditorPage.IsMessageBoxDisplay())
			{
				EditorPage.ClickConfirmButtonInMessageBox();
			}

			Assert.IsTrue(WaitSegmentConfirm(segmentRowNumber),
				"Ошибка: подтверждение (Confirm) не прошло. Возле сегмента не появилась зеленая галка.");
		}

		protected bool WaitSegmentConfirm(int segmentRowNumber)
		{
			Logger.Trace(string.Format("Дожидаемся подтверждения сегмента #{0}", segmentRowNumber));
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
			var sourcetxt = EditorPage.GetSourceText(rowNumber);
			// Нажать хоткей копирования
			EditorPage.CopySourceByHotkey(rowNumber);


			// Проверить, такой ли текст в target'те
			var targetxt = EditorPage.GetTargetText(rowNumber);

			Assert.AreEqual(sourcetxt, targetxt, "Ошибка: после хоткея Copy текст в Source и Target не совпадает");
		}

		/// <summary>
		/// Перейти на страницу TM
		/// </summary>
		protected void SwitchTMTab()
		{
			// Раскрыть выпадающее меню слева
			WorkspacePage.ClickOpenResourcesInMenu();
			// Нажать кнопку перехода на страницу Базы Translation memory
			MainHelperClass.ClickOpenTMPage();
			TMPage.WaitPageLoad();
		}

		protected void SwitchGlossaryTab()
		{
			Logger.Debug("Переход на старницу глоссария.");
			MainHelperClass.ClickResourcesRef();
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

		protected void SwitchSearchTab()
		{
			MainHelperClass.OpenHideMenu();
			Logger.Trace("Переход на стараницу поиска");
			MainHelperClass.ClickOpenSearchPage();
			SearchPage.WaitPageLoad();
		}

		/// <summary>
		/// Перейти на страницу Workspace
		/// </summary>
		protected void SwitchWorkspaceTab()
		{
			Logger.Trace("Переходим на страницу Workspace.");
			if (!WorkspacePage.GetIsLeftMenuDisplay())
				WorkspacePage.OpenHideMenu();
			MainHelperClass.ClickOpenWorkSpacePage();
			WorkspacePage.WaitPageLoad();
		}

		protected void InitSearch(string searchText)
		{
			Logger.Debug(string.Format("Инициировать поиск текста: {0}", searchText));

			SearchPage.AddTextSearch(searchText);
			SearchPage.ClickTranslateBtn();
			SearchPage.WaitUntilShowResults();
		}

		protected void CreateDomain(string domainName, bool shouldCreateOk = true)
		{
			Logger.Debug(string.Format("Создание домена. Имя домена: {0}, домен должен сохраниться: {1}", domainName, shouldCreateOk));
			
			DomainPage.ClickCreateDomainBtn();
			DomainPage.EnterNameCreateDomain(domainName);

			// Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
			Driver.Manage().Window.Maximize();

			DomainPage.ClickSaveDomain();
			
			if (shouldCreateOk)
			{
				DomainPage.WaitUntilSave();
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
			Assert.IsTrue(ProjectPage.WaitPageLoad(), string.Format("Ошибка: страница проекта {0} не открылась", projectName));
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

		protected void OpenCreateGlossary()
		{
			Logger.Debug("Открытие диалога создания глоссария");
			GlossaryListPage.ClickCreateGlossary();

			GlossaryEditForm.AssertionPageLoad();
		}

		protected void CreateGlossaryByName(
			string glossaryName, 
			bool bNeedWaitSuccessSave = true, 
			List<CommonHelper.LANGUAGE> langList = null)
		{
			var langListString = langList == null ? "null" : string.Join(", ", langList);

			Logger.Debug(string.Format("Создание нового глоссария. Имя: {0}, ожидание успешного сохранения: {1}, список языков: {2}",
				glossaryName, bNeedWaitSuccessSave, langListString));

			OpenCreateGlossary();

			GlossaryEditForm.EnterGlossaryName(glossaryName);
			GlossaryEditForm.EnterComment("Test Glossary Generated by Selenium");

			AllLanguagesToDefaultCreateGlossary();

			if (langList != null)
			{
				// Добавить языки
				foreach (var langID in langList)
				{
					AddLanguageCreateGlossary(langID);
				}
			}

			GlossaryEditForm.ClickSaveGlossary();

			if (bNeedWaitSuccessSave)
			{
				GlossaryPage.WaitPageLoad();
			}
			else
			{
				Thread.Sleep(1000);
			}
		}
		
		protected void SwitchCurrentGlossary(string glossaryName)
		{
			Logger.Debug(string.Format("Зайти в глоссарий {0}", glossaryName));
			GlossaryListPage.ScrollAndClickGlossaryRow(glossaryName);
			GlossaryPage.WaitPageLoad();
		}

		protected void OpenGlossaryProperties()
		{
			Logger.Debug("Открытие свойства глоссария");
			GlossaryPage.OpenEditGlossaryList();
			GlossaryPage.ClickOpenProperties();
			GlossaryPage.WaitOpenGlossaryProperties();
		}

		protected void DeleteGlossary()
		{
			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();
			// Нажать Удалить глоссарий 
			GlossaryEditForm.ClickDeleteGlossary();

			// Нажать Да (удалить)
			GlossaryEditForm.WaitUntilDeleteGlossaryButtonDisplay();
			GlossaryEditForm.ClickConfirmDeleteGlossary();
			GlossaryListPage.WaitPageLoad();
		}

		protected void AddLanguageCreateGlossary(CommonHelper.LANGUAGE lang)
		{
			Logger.Debug(string.Format("Добавление языка {0} при создании глоссария", lang));
			
			GlossaryEditForm.ClickAddLanguage();
			GlossaryEditForm.ClickLastLangOpenCloseList();

			// Выбрать язык
			GlossaryEditForm.AssertionIsLanguageExistInList(lang);
			GlossaryEditForm.SelectLanguage(lang);
		}

		protected void AllLanguagesToDefaultCreateGlossary()
		{
			Logger.Debug("Установить дефолтные языки (английский, русский)");

			// Удалить все языки
			for (var i = 1; i < GlossaryEditForm.GetGlossaryLanguageCount(); i++)
			{
				GlossaryEditForm.ClickDeleteLanguage();
			}

			// Выставить первый английский
			GlossaryEditForm.ClickLastLangOpenCloseList();
			// Выбрать язык
			GlossaryEditForm.AssertionIsLanguageExistInList(CommonHelper.LANGUAGE.English);
			GlossaryEditForm.SelectLanguage(CommonHelper.LANGUAGE.English);
			// Кликнуть по Плюсу
			GlossaryEditForm.ClickAddLanguage();
			// Выставить второй русский
			GlossaryEditForm.ClickLastLangOpenCloseList();
			// Выбрать язык
			GlossaryEditForm.AssertionIsLanguageExistInList(CommonHelper.LANGUAGE.Russian);
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
			Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.ABBYY, 
			bool chooseGlossary = false, 
			string glossaryName = "")
		{
			// Создание проекта
			CreateProject(projectName, "", withTM, PathProvider.EditorTmxFile, Workspace_CreateProjectDialogHelper.SetGlossary.None, glossaryName, withMT, mtType);

			//открытие настроек проекта
			uploadDocument = uploadDocument.Length == 0 ? PathProvider.EditorTxtFile : uploadDocument;
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

		protected void OpenAssignDialog(string projectName)
		{
			Logger.Debug(string.Format("Открытие диалога выбора исполнителя в проекте {0}", projectName));
			// Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(projectName);

			// Открываем инфо документа 
			WorkspacePage.OpenDocumnetInfoForProject(projectName);

			// Открываем окно с правами пользователя через кнопку прав
			WorkspacePage.ClickDocumentAssignBtn(projectName);

			// Ожидание открытия диалога выбора исполнителя
			ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay();
		}

		/// <summary>
		/// Выбирает из выпадающего списка пользователя или группу по имени
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <param name="name">Имя пользователя или группы</param>
		/// <param name="isGroup">Выбор группы</param>
		protected void SetResponsible(int rowNumber, string name, bool isGroup)
		{
			Logger.Debug(string.Format("Выбор в качестве исполнителя для задачи #{0} юзера {1}. Выбор группы: {2}", rowNumber, name, isGroup));

			var fullName = string.Empty;
			var rightFullName = string.Empty;

			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(rowNumber);

			if (isGroup)
			{
				fullName = "Group: " + name;
			}
			else
			{
				fullName = name;
			}

			if (!ResponsiblesDialog.WaitUntilUserInListDisplay(fullName))
			{
				if (!ResponsiblesDialog.WaitUntilUserInListDisplay(fullName.Replace(" ", "  ")))
					Assert.Fail("Ошибка: пользователь " + fullName +
					            " не найден в выпадающем списке при назначении исполнителя на задачу");
				else
					rightFullName = fullName.Replace(" ", "  ");
			}
			else
				rightFullName = fullName;

			// Выбрать для заданной задачи имя исполнителя
			ResponsiblesDialog.SetVisibleResponsible(rowNumber, rightFullName);

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
			EditorPage.AssertionIsDictionaryFormDisplayed();
		}

		/// <summary>
		/// Ждём, пока словарь прогрузится
		/// </summary>
		protected void WaitLoadDictionary()
		{
			//Проверяем, что загрузка словаря закончилась 
			EditorPage.AssertionIsDictionaryListLoad();
		}
		
		/// <summary>
		/// Создать уникальное имя
		/// </summary>
		/// <returns>имя</returns>
		protected string GetUniqueGlossaryName()
		{
			// Получить уникальное имя глоссария (т.к. добавляется точная дата и время, то не надо проверять, есть ли такой глоссарий в списке)
			// Явное приведение к строке стоит, чтобы не падал ArgumentOutOfRangeException. (неявное приведение даты иногда не отрабатывает корректно)
			return GlossaryName + DateTime.Now.ToString("MM.dd.yyyy HH:mm:ss", new CultureInfo("en-US"));
		}

		/// <summary>
		/// Дождаться автосохранения сегментов
		/// </summary>
		protected void AutoSave()
		{
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.ManualInput,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неверный тип последней ревизии после автосохранения.");
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

			foreach (var item in ProcessNames.Select(Process.GetProcessesByName).SelectMany(processArray => processArray))
			{
				try
				{
					if (!item.HasExited)
						item.Kill();
				}
				catch (Exception ex)
				{
					Logger.ErrorException("Ошибка при завершении процесса (" + item.ProcessName + "): "  + ex.Message, ex);
				}
			}

			Logger.Info("Работа драйвера и браузера завершена.");
		}

		/// <summary>
		/// Закрываем модальный диалог
		/// </summary>
		protected void AcceptModalDialog()
		{
			try
			{
				if (IsModalDialogConfirmation() || IsModalDialogChangesNotSaved() || IsModalDialogTMNotSaved()) 
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
		/// Метод проверяет сообщение в модальном диалоге на содержание фразы:
		/// "Эта страница просит вас подтвердить, что вы хотите уйти — при этом введённые вами данные могут не сохраниться."
		/// (англ./рус.)
		/// </summary>
		protected bool IsModalDialogConfirmation()
		{
			return	Driver.SwitchTo().Alert().Text.
						Contains("Эта страница просит вас подтвердить, что вы хотите уйти — при этом введённые вами данные могут не сохраниться.") ||
					Driver.SwitchTo().Alert().Text.
						Contains("This page is asking you to confirm that you want to leave - data you have entered may not be saved.");
		}

		/// <summary>
		/// Метод проверяет сообщение в модальном диалоге на содержание фразы:
		/// "Изменения не были сохранены."
		/// (англ./рус.)
		/// </summary>
		protected bool IsModalDialogChangesNotSaved()
		{
			return Driver.SwitchTo().Alert().Text.Contains("Изменения не были сохранены.")
				|| Driver.SwitchTo().Alert().Text.Contains("Changes have not been saved.");
		}

		/// <summary>
		/// Метод проверяет сообщение в модальном диалоге на содержание фразы:
		/// "Some TMs are being edited. Please save TMs or cancel editing."
		/// "Are you sure you want to leave this page?"
		/// </summary>
		protected bool IsModalDialogTMNotSaved()
		{
			return Driver.SwitchTo().Alert().Text.
						Contains("Some TMs are being edited. Please save TMs or cancel editing.");
		}

		/// <summary>
		/// Закрываем окно с вопросом о Workflow
		/// </summary>
		protected void AcceptWorkflowModalDialog()
		{
			try
			{
				if (Driver.SwitchTo().Alert().Text
						.Contains("Включение функции workflow для аккаунта необратимо, обратное выключение будет невозможно. Продолжить?"))
				{
					Logger.Trace("Закрыть модальное диалоговое окно");
					Driver.SwitchTo().Alert().Accept();
				}


				Thread.Sleep(500);
			}
			catch (NoAlertPresentException)
			{
				TryCloseExternalDialog();
			}
		}

		protected void SetRusLanguageTarget()
		{
			Logger.Debug("Задание русского языка перевода (target) при создании проекта.");
			
			var langList = WorkspaceCreateProjectDialog.GetTargetLanguageList();

			if (langList.Count != 1 || langList[0] != "Russian")
			{
				WorkspaceCreateProjectDialog.ClickTargetList();
				WorkspaceCreateProjectDialog.ClickAllSelectedTargetItems();
				WorkspaceCreateProjectDialog.ClickTargetItem(CommonHelper.LANGUAGE.Russian);
				WorkspaceCreateProjectDialog.ClickTargetList();
			}
		}

		protected void SkipNotSelectedTM()
		{
			Logger.Debug("Пропуск подтверждения отсутствия TM при создании проекта");

			if (WorkspaceCreateProjectDialog.WaitUntilConfirmTMDialogDisplay())
			{
				WorkspaceCreateProjectDialog.ClickSkipBtn();

				// Ждем пока диалог не пропадет
				WorkspaceCreateProjectDialog.AssertionConfirmTMDialogDisappear();
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
			Logger.Trace(string.Format("Метод подстановки из САТ. Номер сегмента {0}, тип подстановки {1}", segmentNumber, catType));
			
			EditorPage.ClickTargetCell(segmentNumber);

			var catLineNumber = EditorPage.GetCatTranslationRowNumber(catType);

			EditorPage.PutCatMatchByHotkey(segmentNumber, catLineNumber);

			Assert.IsTrue(EditorPage.GetTargetText(segmentNumber) == EditorPage.GetCatPanelText(catLineNumber),
				"Ошибка: хоткей подстановки " + catType
				+ " не сработал, текст в таргет и текст в панели CAT не совпадает");

			EditorPage.ClickConfirmBtn();

			// Дождаться сохранения
			Assert.IsTrue(
				EditorPage.WaitUntilAllSegmentsSave(),
				"Ошибка: Не проходит сохранение через Confirm в редакторе.");

			// Вернуться в сегмент
			EditorPage.ClickTargetCell(segmentNumber);

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
				GlossaryPage.AssertionConceptTableAppear();
				// Заполнить термин
				GlossaryPage.FillTerm(1, pair.Key);
				GlossaryPage.FillTerm(2, pair.Value);

				// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
				Driver.Manage().Window.Maximize();
				// Нажать Сохранить
				GlossaryPage.ClickSaveTermin();
				GlossaryPage.AssertionConceptGeneralSave();
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
					Logger.Trace("Переход на страницу регистарции компаний");
					Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.CorpReg);
				}
				else
				{
					Logger.Trace("Переход на страницу регистарции фрилансеров");
					Driver.Navigate().GoToUrl(Url + RelativeUrlProvider.FreelanceReg);
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

		public void AddTermGlossary(string sourceTerm, string targetTerm)
		{
			Logger.Debug(string.Format("Добавляем сорс-таргет сегмент в глоссарий для теста. Слово для внесения в словарь - {0}, перевод слова словаря - {1}", sourceTerm, targetTerm));
			
			AddTermForm.TypeSourceTermText(sourceTerm);
			// Добавить термин в таргет
			AddTermForm.TypeTargetTermText(targetTerm);
			// Нажать сохранить
			AddTermForm.ClickAddBtn();
		}

		/// <summary>
		/// Метод возвращает список файлов, которые содержатся в папке dirName и подходят под маску mask
		/// </summary>
		/// <param name="mask">маска имени файла</param>
		/// <param name="waitTime">время ождиания появления айлфа</param>
		/// <param name="dirName">папка,в которой надо искать файлы</param>
		/// <returns>Список файлов. Пустой список, если нет подходящих под маску файлов.</returns>
		public string[] GetDownloadFiles(string mask, int waitTime, string dirName)
		{
			string[] files = null;
			for (int i = 0; i < waitTime; i++)
			{
				files = Directory.GetFiles(dirName, mask, SearchOption.TopDirectoryOnly);
				if (files.Length > 0)
				{
					break;
				}
				Thread.Sleep(1000);//Ждём секунду
			}
			return files;
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
			CheckCreateProjectRightHelper = new CheckCreateProjectRightHelper(Driver, Wait);
			SocialNetworkPage = new SocialNetworkHelper(Driver, Wait);
		}

		private void initializeUsersAndCompanyList()
		{
			if (TestUserFileExist())
			{
				var cfgTestUser = TestSettingDefinition.Instance.Get<TestUserConfig>();

				TestUserList = new List<UserInfo>();
				TestCompanyList = new List<UserInfo>();
				CourseraUserList = new List<UserInfo>();
				AolUserList = new List<UserInfo>();

				foreach (var user in cfgTestUser.Users)
				{
					TestUserList.Add(
						new UserInfo(
							user.Login, 
							user.Password, 
							user.Activated));
				}

				foreach (var user in cfgTestUser.Companies)
				{
					TestCompanyList.Add(
						new UserInfo(
							user.Login, 
							user.Password, 
							user.Activated));
				}

				foreach (var user in cfgTestUser.CourseraUsers)
				{
					CourseraUserList.Add(
						new UserInfo(
							user.Login, 
							user.Password,
							user.Activated));
				}

				foreach (var user in cfgTestUser.AolUsers)
				{
					AolUserList.Add(
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
			EmailAuth = cfgAgentSpecific.EmailAuth;

			Url = Standalone ? "http://" + cfgAgentSpecific.Url : "https://" + cfgAgentSpecific.Url;

			if (string.IsNullOrWhiteSpace(WorkspaceUrl))
			{
				WorkspaceUrl = Url + "/workspace";
			}

			AdminUrl = "http://" + cfgAgentSpecific.Url + ":81";
		}

		private void initializeRelatedToUserFields(UserInfoConfig cfgUserInfo)
		{
			Login = cfgUserInfo.Login;
			Password = cfgUserInfo.Password;
			UserName = cfgUserInfo.UserName;

			Login2 = cfgUserInfo.Login2;
			Password2 = cfgUserInfo.Password2;
			UserName2 = cfgUserInfo.UserName2;

			TestRightsLogin = cfgUserInfo.TestRightsLogin;
			TestRightsPassword = cfgUserInfo.TestRightsPassword;
			TestRightsUserName = cfgUserInfo.TestRightsUserName;
		}
	}
}
