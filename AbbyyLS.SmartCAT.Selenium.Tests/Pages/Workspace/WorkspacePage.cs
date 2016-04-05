using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.LingvoDictionaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace
{
	public class WorkspacePage : IAbstractPage<WorkspacePage>
	{
		public WebDriver Driver { get; protected set; }

		public WorkspacePage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public void GetPage(string workspaceUrl)
		{
			CustomTestContext.WriteLine("Переход на страницу Workspace: {0}.", workspaceUrl);

			Driver.Navigate().GoToUrl(workspaceUrl);
		}

		public WorkspacePage LoadPage()
		{
			if (!IsWorkspacePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница с workspace");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку "пользователи и права"
		/// </summary>
		public UsersTab ClickUsersRightsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Пользователи и права'.");
			UsersRightsButton.Click();

			return new UsersTab(Driver).LoadPage();
		}

		/// <summary>
		/// Выбрать вкладку "Клиенты"
		/// </summary>
		public ClientsPage ClickClientsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Клиенты'.");
			ClientsButton.Click();

			return new ClientsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Выбрать вкладку "Группы проектов"
		/// </summary>
		public ProjectGroupsPage ClickProjectGroupsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Группы проектов'.");
			ProjectGroupsButton.Click();

			return new ProjectGroupsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Выбрать вкладку "Память переводов"
		/// </summary>
		public TranslationMemoriesPage ClickTranslationMemoriesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Память переводов'.");
			TranslationMemoriesButton.JavaScriptClick();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Выбрать вкладку "Глоссарии"
		/// </summary>
		public GlossariesPage ClickGlossariesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Глоссарии'.");
			GlossariesButton.JavaScriptClick();

			return new GlossariesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Lingvo Dictionaries'
		/// </summary>
		public LingvoDictionariesPage ClickLingvoDictionariesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Lingvo Dictionaries'.");
			LingvoDictionaries.Click();

			return new LingvoDictionariesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на имя пользователя и аккаунт, чтобы появилась плашка "Настройки профиля"
		/// </summary>
		public WorkspacePage ClickAccount()
		{
			CustomTestContext.WriteLine("Нажать на имя пользователя и аккаунт, чтобы появилась плашка 'Настройки профиля'.");
			Account.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на 'Licenses and Services'
		/// </summary>
		public BillingPage ClickLicenseAndServices()
		{
			CustomTestContext.WriteLine("Нажать на 'Licenses and Services'.");
			LicenseAndServices.Click();

			return new BillingPage(Driver);
		}

		/// <summary>
		/// Переключиться  в окно 'Управление лицензиями'
		/// </summary>
		public BillingPage SwitchToLicenseAndServicesWindow()
		{
			CustomTestContext.WriteLine("Переключиться  в окно 'Управление лицензиями'");
			// Sleep нужен, чтоб вторая вкладка успела открыться, иначе количество открытых вкладок посчитается неправильно 
			Thread.Sleep(1000);
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles.First()).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return new BillingPage(Driver);
		}

		/// <summary>
		/// Выйти из смартката
		/// </summary>
		public SignInPage ClickSignOut()
		{
			CustomTestContext.WriteLine("Выйти из смартката.");
			SignOutButton.JavaScriptClick();

			return new SignInPage(Driver).LoadPage();
		}

		/// <summary>
		/// Выйти из смартката
		/// </summary>
		public void ClickSignOutAssumingAlert()
		{
			CustomTestContext.WriteLine("Выйти из смартката, ожидая алерт.");

			SignOutButton.JavaScriptClick();
		}

		/// <summary>
		/// Нажать на кнопку информация(HelpMenu)
		/// </summary>
		public WorkspacePage ClickHelpMenuButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку информация.");
			HelpMenuButton.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Перейти на страницу 'Справка'
		/// </summary>
		public HelpPage ClickHelpPageInNewTab()
		{
			CustomTestContext.WriteLine("Перейти на страницу 'Справка'.");
			HelpPage.JavaScriptClick();

			Driver.SwitchToNewBrowserTab();

			return new HelpPage(Driver).LoadPage();

		}

		/// <summary>
		/// Перейти на страницу 'Видеоуроки'
		/// </summary>
		public VideoLessonsPage ClickVideoLessonsPage()
		{
			CustomTestContext.WriteLine("Перейти на страницу 'Видеоуроки'.");
			VideoLessonsPage.JavaScriptClick();

			return new VideoLessonsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на страницу 'Ответы техподержки'
		/// </summary>
		public SupportFeedbackPage ClickSupportFeedbackPage()
		{
			CustomTestContext.WriteLine("Перейти на страницу 'Ответы техподержки'.");
			SupportFeedbackPage.JavaScriptClick();

			return new SupportFeedbackPage(Driver).LoadPage();
		}

		/// <summary>
		/// Открыть диалоговое окно 'Обратиться в техподдержку'
		/// </summary>
		public FeedbackDialog ClickSupportFeedbackDialog()
		{
			CustomTestContext.WriteLine("Открыть диалоговое окно 'Обратиться в техподдержку'.");
			FeedbackDialog.JavaScriptClick();

			return new FeedbackDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Вернуть количество уведомлений об экспорте
		/// </summary>
		public int GetCountExportNotifiers()
		{
			CustomTestContext.WriteLine("Узнать, сколько уведомлений есть на данный момент.");
			var countExportNotifiers = Driver.GetElementList(By.XPath(NOTIFIER_LIST)).Count;
			CustomTestContext.WriteLine("Обнаружено {0} уведомлений.", countExportNotifiers);

			return countExportNotifiers;
		}

		/// <summary>
		/// Нажать Search в меню.
		/// </summary>
		public SearchPage ClickSearchButton()
		{
			CustomTestContext.WriteLine("Нажать Search в меню.");
			SearchMenu.Click();

			return new SearchPage(Driver).LoadPage();
		}

		/// <summary>
		/// Обновить страницу
		/// </summary>
		public T RefreshPage<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Обновить страницу.");
			Driver.Navigate().Refresh();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.LoadPage();
		}

		/// <summary>
		/// Закрыть алерт
		/// </summary>
		public T AcceptAlert<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Закрыть алерт");
			Driver.SwitchTo().Alert().Accept();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.LoadPage();
		}

		/// <summary>
		/// Закрыть все показанные уведомления
		/// </summary>
		public T CloseAllNotifications<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Закрыть все показанные уведомления.");
			Driver.WaitUntilElementIsDisplay(By.XPath(ALL_NOTIFICATIONS), timeout: 3);
			var notificationsCount = Driver.GetElementList(By.XPath(ALL_NOTIFICATIONS)).Count;

			for (var i = notificationsCount; i > 0; i--)
			{
				CustomTestContext.WriteLine("Закрыть сообщение №{0}.", i);
				Driver.WaitUntilElementIsDisplay(By.XPath(CLOSE_NOTIFICATION_BUTTON.Replace("*#*", i.ToString())));
				Notification = Driver.SetDynamicValue(How.XPath, CLOSE_NOTIFICATION_BUTTON, i.ToString());
				Notification.Click();
			}

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Выйти из SmartCAT, закрыв alert
		/// </summary>
		public SignInPage SignOutExpectingAlert()
		{
			ClickAccount();
			ClickSignOutAssumingAlert();
			Driver.SwitchTo().Alert().Accept();

			return new SignInPage(Driver).LoadPage();
		}


		/// <summary>
		/// Перейти на страницу со списком клиентов
		/// </summary>
		public ClientsPage GoToClientsPage()
		{
			OpenHideMenuIfClosed();
			ClickClientsButton();

			return new ClientsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на страницу со списком TM
		/// </summary>
		public TranslationMemoriesPage GoToTranslationMemoriesPage()
		{
			OpenHideMenuIfClosed();
			ExpandResourcesIfNotExpanded();
			ClickTranslationMemoriesButton();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на страницу со списком глоссариев
		/// </summary>
		public GlossariesPage GoToGlossariesPage()
		{
			OpenHideMenuIfClosed();
			ExpandResourcesIfNotExpanded();
			ClickGlossariesButton();

			return new GlossariesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на страницу со списком групп проектов
		/// </summary>
		public ProjectGroupsPage GoToProjectGroupsPage()
		{
			OpenHideMenuIfClosed();
			ClickProjectGroupsButton();

			return new ProjectGroupsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на страницу "Пользователи и права"
		/// </summary>
		public UsersTab GoToUsersPage()
		{
			OpenHideMenuIfClosed();
			ClickUsersRightsButton();

			return new UsersTab(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на страницу поиска
		/// </summary>
		public SearchPage GoToSearchPage()
		{
			OpenHideMenuIfClosed();
			ExpandResourcesIfNotExpanded();
			ClickSearchButton();

			return new SearchPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на страницу со словарями Lingvo
		/// </summary>
		public LingvoDictionariesPage GoToLingvoDictionariesPage()
		{
			OpenHideMenuIfClosed();
			ExpandResourcesIfNotExpanded();
			ClickLingvoDictionariesButton();

			return new LingvoDictionariesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на страницу со списком проектов
		/// </summary>
		public ProjectsPage GoToProjectsPage()
		{
			OpenHideMenuIfClosed();
			ClickProjectsSubmenu();

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Выйти из аккаунта
		/// </summary>
		public SignInPage SignOut()
		{
			ClickAccount();
			ClickSignOut();

			return new SignInPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на страницу биллинга
		/// </summary>
		public BillingPage GoToBillingPage()
		{
			ClickAccount();
			ClickLicenseAndServices();
			SwitchToLicenseAndServicesWindow();

			return new BillingPage(Driver).LoadPage();
		}

		/// <summary>
		/// Выбрать вкладку "Проекты"
		/// </summary>
		public ProjectsPage ClickProjectsSubmenu()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Проекты'.");
			ProjectsButton.JavaScriptClick();

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на пункт меню "Проекты", без ожидания загрузки каких-либо страниц
		/// </summary>
		public void ClickProjectsSubmenuExpectingAlert()
		{
			CustomTestContext.WriteLine("Нажать на пункт меню 'Проекты', без ожидания загрузки каких-либо страниц");
			OpenHideMenuIfClosed();
			ProjectsButton.JavaScriptClick();
		}

		/// <summary>
		/// Развернуть меню ресурсов, если оно свернуто
		/// </summary>
		public WorkspacePage ExpandResourcesIfNotExpanded()
		{
			if (!ResourcesMenu.GetAttribute("class").Contains("nested-expanded"))
			{
				ExpandResourcesMenuButton.Click();
			}

			return LoadPage();
		}
		
		/// <summary>
		/// Смена языка локали
		/// </summary>
		public WorkspacePage SetLocale(Language language = Language.English)
		{
			CustomTestContext.WriteLine("Сменить язык локали на {0}, если необходимо.", language);
			// Sleep нужен для предотвращения ошибки "unknown error: Element is not clickable at point"
			Thread.Sleep(1000);
			LanguageButton.Click();
			switch (language)
			{
				case Language.English:
					if (Driver.WaitUntilElementIsDisplay(By.XPath(LOCALE_REF.Replace("*#*", "en")), timeout: 1))
					{
						LocaleRef = Driver.SetDynamicValue(How.XPath, LOCALE_REF, "en");
						LocaleRef.Click();
						Driver.WaitUntilElementIsDisplay(By.XPath(LANGUAGE_PICTURE.Replace("*#*", "en")));
					}
					break;
				case Language.Russian:
					if (Driver.WaitUntilElementIsDisplay(By.XPath(LOCALE_REF.Replace("*#*", "ru")), timeout: 1))
					{
						LocaleRef = Driver.SetDynamicValue(How.XPath, LOCALE_REF, "ru");
						LocaleRef.Click();
						Driver.WaitUntilElementIsDisplay(By.XPath(LANGUAGE_PICTURE.Replace("*#*", "ru")));
					}
					break;
				default:
					throw new InvalidEnumArgumentException("Произошла ошибка:\n локализация может быть только русской или английской.");
			}

			return LoadPage();
		}

		/// <summary>
		/// Закрыть подсказку (если она открыта) сразу после входа в SmartCAT
		/// </summary>
		public WorkspacePage CloseHelp()
		{
			CustomTestContext.WriteLine("Проверить, открыта ли подсказка при входе в SmartCAT.");

			CloseHelpButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Открыть CAT меню слева
		/// </summary>
		public WorkspacePage OpenHideMenuIfClosed()
		{
			if (!CatMenu.Displayed)
			{
				CustomTestContext.WriteLine("Открыть CAT меню слева.");
				Driver.WaitUntilElementIsDisplay(By.XPath(CAT_MENU_OPEN_BUTTON), 20);
				CatMenuOpenButton.JavaScriptClick();
			}

			return LoadPage();
		}

		/// <summary>
		/// Выбрать язык интерфейса
		/// </summary>
		/// <param name="language">язык</param>
		public WorkspacePage SelectLocale(Language language)
		{
			SetLocale(language);

			if (language == Language.Russian)
			{
				RefreshPage<WorkspacePage>();
			}

			return LoadPage();
		}

		/// <summary>
		/// Предварительная настройка workspace: закрытие тура, выбор языка, проверка пользователя
		/// </summary>
		/// <param name="nickName">имя пользователя</param>
		/// <param name="accountName">имя аккаунта</param>
		/// <param name="language">язык интерфейса</param>
		public WorkspacePage SetUp(
			string nickName,
			string accountName = LoginHelper.TestAccountName,
			Language language = Language.English)
		{
			if (!IsUserNameMatchExpected(nickName))
			{
				throw new Exception(
					"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");
			}

			if (!IsAccountNameMatchExpected(accountName))
			{
				throw new Exception(
					"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");
			}

			SetLocale(language);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница
		/// </summary>
		public bool IsWorkspacePageOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(USER_PICTURE));
		}

		/// <summary>
		/// Проверить, раскрыт ли пункт меню Проекты.
		/// </summary>
		public bool IsProjectsMenuExpanded()
		{
			CustomTestContext.WriteLine("Проверить, раскрыт ли пункт меню Проекты.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(PROJECTS_BUTTON));
		}

		/// <summary>
		/// Проверить, что пункт 'Lingvo Dictionaries' присутствует в меню и видим
		/// </summary>
		public bool IsLingvoDictionariesMenuItemDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что пункт 'Lingvo Dictionaries' присутствует в меню и видим");

			return Driver.GetIsElementExist(By.XPath(LINGVO_DICTIONARIES_MENU)) && LingvoDictionaries.Displayed;
		}

		/// <summary>
		/// Проверить, что имя пользователя в черной плашке совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedUserName">ожидаемое имя</param>
		public bool IsUserNameMatchExpected(string expectedUserName)
		{
			CustomTestContext.WriteLine(
				"Проверить, что имя пользователя в черной плашке совпадает с ожидаемым именем {0}",
				expectedUserName);

			var userName = UserName.Text;
			var index = userName.IndexOf("\r", StringComparison.Ordinal);

			if (index > 0)
			{
				userName = userName.Substring(0, index);
			}

			return userName == expectedUserName;
		}

		/// <summary>
		/// Проверить, что название аккаунта в черной плашке совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedAccountName">ожидаемое имя</param>
		public bool IsAccountNameMatchExpected(string expectedAccountName)
		{
			CustomTestContext.WriteLine(
				"Проверить, что название аккаунта в черной плашке совпадает с ожидаемым именем {0}",
				expectedAccountName);

			return CurrentAccountName.Text == expectedAccountName;
		}

		/// <summary>
		/// Проверить, что список аккаунтов содержит нужный аккаунт
		/// </summary>
		/// <param name="accountName">имя аккаунт</param>
		public bool IsAccountListContainsAccountName(string accountName)
		{
			CustomTestContext.WriteLine("Проверить, что список аккаунтов содержит {0} аккаунт.", accountName);
			AccountNameInList = Driver.SetDynamicValue(How.XPath, ACCOUNT_NAME_IN_LIST, accountName);

			return AccountNameInList.Displayed;
		}

		/// <summary>
		/// Проверить, есть ли на странице алерты
		/// </summary>
		public bool IsAlertExist()
		{
			CustomTestContext.WriteLine("Проверить, есть ли на странице алерты");

			try
			{
				Driver.SwitchTo().Alert();
				return true;
			}
			catch (NoAlertPresentException e)
			{
				return false;
			}
		}

		/// <summary>
		/// Проверить, что фон диалога исчез
		/// </summary>
		public bool IsDialogBackgroundDisappeared()
		{
			return Driver.WaitUntilElementIsDisappeared(By.XPath(DIALOG_BACKGROUND));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = USERS_RIGHTS_BUTTON)]
		protected IWebElement UsersRightsButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENTS_BUTTON)]
		protected IWebElement ClientsButton { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATION_MEMORIES_BUTTON)]
		protected IWebElement TranslationMemoriesButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECTS_BUTTON)]
		protected IWebElement ProjectsButton { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY)]
		protected IWebElement GlossariesButton { get; set; }

		[FindsBy(How = How.XPath, Using = DOMAIN_REF)]
		protected IWebElement ProjectGroupsButton { get; set; }

		[FindsBy(How = How.XPath, Using = ACCOUNT)]
		protected IWebElement Account { get; set; }

		[FindsBy(How = How.XPath, Using = SIGN_OUT_BUTTON)]
		protected IWebElement SignOutButton { get; set; }

		[FindsBy(How = How.XPath, Using = HELP_MENU)]
		protected IWebElement HelpMenuButton { get; set; }

		[FindsBy(How = How.XPath, Using = HELP_PAGE)]
		protected IWebElement HelpPage { get; set; }

		[FindsBy(How = How.XPath, Using = VIDEO_LESSONS_PAGE)]
		protected IWebElement VideoLessonsPage { get; set; }

		[FindsBy(How = How.XPath, Using = SUPPORT_FEEDBACK_PAGE)]
		protected IWebElement SupportFeedbackPage { get; set; }

		[FindsBy(How = How.XPath, Using = FEEDBACK_DIALOG)]
		protected IWebElement FeedbackDialog { get; set; }

		[FindsBy(How = How.XPath, Using = RESOURCES_MENU)]
		protected IWebElement ResourcesMenu { get; set; }

		[FindsBy(How = How.XPath, Using = EXPAND_RESOURCES_MENU)]
		protected IWebElement ExpandResourcesMenuButton { get; set; }

		[FindsBy(How = How.XPath, Using = CAT_MENU)]
		protected IWebElement CatMenu { get; set; }

		[FindsBy(How = How.XPath, Using = CAT_MENU_OPEN_BUTTON)]
		protected IWebElement CatMenuOpenButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_HELP_BUTTON)]
		protected IWebElement CloseHelpButton { get; set; }

		[FindsBy(How = How.XPath, Using = LICENSES_AND_SERVICES)]
		protected IWebElement LicenseAndServices { get; set; }

		[FindsBy(How = How.XPath, Using = LINGVO_DICTIONARIES_MENU)]
		protected IWebElement LingvoDictionaries { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_MENU)]
		protected IWebElement SearchMenu { get; set; }

		[FindsBy(How = How.XPath, Using = LANGUAGE_BUTTON)]
		protected IWebElement LanguageButton { get; set; }

		protected IWebElement LocaleRef { get; set; }

		[FindsBy(How = How.XPath, Using = CURRENT_ACCOUNT_NAME)]
		protected IWebElement CurrentAccountName { get; set; }

		[FindsBy(How = How.XPath, Using = USER_NAME)]
		protected IWebElement UserName { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECTS_MENU)]
		protected IWebElement ProjectsMenu { get; set; }

		protected IWebElement AccountNameInList { get; set; }

		protected IWebElement Notification { get; set; }

		#endregion

		#region Описания XPath элементов
		
		protected const string CAT_MENU = "//div[contains(@class, 'js-mainmenu')]";
		protected const string CAT_MENU_OPEN_BUTTON = "//div[contains(@class,'g-topbox__burger')]//span";
		protected const string CLOSE_HELP_BUTTON = "//div[@class='popup-wrap']//img[@title='Close']";

		protected const string RESOURCES_MENU ="//li[contains(@class, 'js-menuitem-Resources')]";
		protected const string EXPAND_RESOURCES_MENU = "//li[contains(@class, 'js-menuitem-Resources')]//a";
		protected const string PROJECTS_BUTTON = "//a[@href ='/Workspace']";
		protected const string PROJECTS_MENU = "//li//a//span[text()='Projects']";
		protected const string USERS_RIGHTS_BUTTON = "//a[contains(@href,'/Users/Index') and contains(@class,'mainmenu')]";
		protected const string CLIENTS_BUTTON = "//a[contains(@href,'/Clients/Index')]";
		protected const string TRANSLATION_MEMORIES_BUTTON = "//a[contains(@href,'/TranslationMemories/Index')]";
		protected const string GLOSSARY = ".//a[contains(@href,'/Glossaries')]";
		protected const string DOMAIN_REF = ".//a[contains(@href,'/Domains')]";
		protected const string LINGVO_DICTIONARIES_MENU = ".//a[contains(@href,'/LingvoDictionaries')]";
		protected const string SEARCH_MENU = "//div[contains(@class, 'menu-wrapper')]//a[contains(@href,'/Start')]";

		protected const string USER_PICTURE = "//i[contains(@class, 'upic')]";
		protected const string LOCALE_REF = "//div[contains(@class, 'langTools')]//i[contains(@class, '*#*')]";
		protected const string LANGUAGE_BUTTON = "//div[contains(@class, 'language-menu')]//button[contains(@class, 'language-button')]//i";
		protected const string ACCOUNT = "//div[contains(@class,'js-usermenu')]";
		protected const string USER_NAME = "//div[contains(@class,'js-usermenu')]//span[contains(@class,'nameuser')]";
		protected const string LOGOFF = ".//a[contains(@href,'Logout')]";
		protected const string NOTIFIER_LIST = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]";
		protected const string SIGN_OUT_BUTTON = ".//a[contains(@href,'Logout')]";
		protected const string LICENSES_AND_SERVICES = "//a[contains(@class,'billing')]";
		protected const string HELP_MENU = "//div[contains(@class, 'js-menu-help')]";
		protected const string HELP_PAGE = "//div[contains(@class, 'js-help-submenu')]//a[contains(@href,'/Support/ShowHelp')]";
		protected const string VIDEO_LESSONS_PAGE = "//div[contains(@class, 'js-help-submenu')]//a[contains(@href,'/Support/HelpPage')]";
		protected const string SUPPORT_FEEDBACK_PAGE = "//div[contains(@class, 'js-help-submenu')]//a[contains(@href,'/Support/FeedbackList')]";
		protected const string FEEDBACK_DIALOG = "//div[contains(@class, 'js-help-submenu')]//a[contains(@href,'#')]";

		protected const string LANGUAGE_PICTURE = "//i[text()='*#*']";
		protected const string DIALOG_BACKGROUND = "//div[contains(@class,'js-popup-bg')]";
		protected const string CURRENT_ACCOUNT_NAME = "//span[contains(@class,'g-topbox__nameuser')]//small";
		protected const string ACCOUNT_NAME_IN_LIST = "//li[@class='g-topbox__corpitem' and @title='*#*']";

		protected const string ALL_NOTIFICATIONS = "//div[@class='g-notifications-item']";
		protected const string NOTIFICATION = "//div[@class='g-notifications-item'][*#*]//span[2]/a";
		protected const string CLOSE_NOTIFICATION_BUTTON = "//div[@class='g-notifications-item'][*#*]//button[contains(@data-bind, 'stopBubble')]//span";

		#endregion
	}
}
