using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.LingvoDictionaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace
{
	public class WorkspacePage : BaseObject, IAbstractPage<WorkspacePage>
	{
		public WebDriver Driver { get; protected set; }

		public WorkspacePage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public WorkspacePage GetPage()
		{
			var workspacePage = new WorkspacePage(Driver);
			InitPage(workspacePage, Driver);

			return workspacePage;
		}

		public void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!IsWorkspacePageOpened())
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с workspace.");
			}
		}

		/// <summary>
		/// Проверить, открылась ли страница
		/// </summary>
		public bool IsWorkspacePageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(USER_PICTURE));
		}

		/// <summary>
		/// Нажать на кнопку "пользователи и права"
		/// </summary>
		public UsersRightsPage ClickUsersRightsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Пользователи и права'.");
			UsersRightsButton.Click();

			return new UsersRightsPage(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Проекты"
		/// </summary>
		public ProjectsPage ClickProjectsSubmenu()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Проекты'.");
			if (!IsProjectsMenuExpanded())
			{
				ExpandProjectMenu();
			}

			ProjectsButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Раскрыть пункт меню Проекты
		/// </summary>
		public WorkspacePage ExpandProjectMenu()
		{
			CustomTestContext.WriteLine("Раскрыть пункт меню Проекты");
			ProjectsMenu.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Клиенты"
		/// </summary>
		public ClientsPage ClickClientsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Клиенты'.");
			ClientsButton.Click();

			return new ClientsPage(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Группы проектов"
		/// </summary>
		public ProjectGroupsPage ClickProjectGroupsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Группы проектов'.");
			ProjectGroupsButton.Click();

			return new ProjectGroupsPage(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Память переводов"
		/// </summary>
		public TranslationMemoriesPage ClickTranslationMemoriesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Память переводов'.");
			TranslationMemoriesButton.Click();

			return new TranslationMemoriesPage(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Глоссарии"
		/// </summary>
		public GlossariesPage ClickGlossariesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Глоссарии'.");
			GlossariesButton.Click();

			return new GlossariesPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Lingvo Dictionaries'
		/// </summary>
		public LingvoDictionariesPage ClickLingvoDictionariesButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Lingvo Dictionaries'.");
			LingvoDictionaries.Click();

			return new LingvoDictionariesPage(Driver).GetPage();
		}

		/// <summary>
		/// Развернуть меню ресурсов, если оно свернуто
		/// </summary>
		public WorkspacePage ExpandResourcesIfNotExpanded()
		{
			IWebElement resourcesMenu;
			try
			{
				resourcesMenu = Driver.FindElement(By.XPath(RESOURCES_MENU));
			}
			catch (StaleElementReferenceException)
			{
				CustomTestContext.WriteLine("StaleElementReferenceException: Не удалось найти элемент. Предпринять повторную попытку.");
				resourcesMenu = Driver.FindElement(By.XPath(RESOURCES_MENU));
			}

			if (!resourcesMenu.GetAttribute("class").Contains("nested-expanded"))
			{
				ExpandResourcesMenuButton.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Нажать на имя пользователя и аккаунт, чтобы появилась плашка "Настройки профиля"
		/// </summary>
		public WorkspacePage ClickAccount()
		{
			CustomTestContext.WriteLine("Нажать на имя пользователя и аккаунт, чтобы появилась плашка 'Настройки профиля'.");
			Driver.WaitUntilElementIsDisplay(By.XPath(ACCOUNT));
			Account.JavaScriptClick();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что настройки профиля открылись.
		/// </summary>
		public WorkspacePage AssertAccountProfileDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что настройки профиля открылись.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(LOGOFF)),
				"Произошла ошибка:\n настройки профиля не открылись.");

			return GetPage();
		}

		/// <summary>
		/// Нажать на 'Licenses and Services'
		/// </summary>
		public BillingPage ClickLicenseAndServices()
		{
			CustomTestContext.WriteLine("Нажать на 'Licenses and Services'.");
			Driver.WaitUntilElementIsDisplay(By.XPath(LICENSES_AND_SERVICES));
			LicenseAndServices.Click();

			return new BillingPage(Driver);
		}

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

			return new SignInPage(Driver).GetPage();
		}

		/// <summary>
		/// Смена языка локали
		/// </summary>
		public WorkspacePage SelectLocale(Language language = Language.English)
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

			return GetPage();
		}

		/// <summary>
		/// Закрыть подсказку (если она открыта) сразу после входа в SmartCAT
		/// </summary>
		public WorkspacePage CloseHelpIfOpened()
		{
			CustomTestContext.WriteLine("Проверить, открыта ли подсказка при входе в SmartCAT.");

			if (Driver.WaitUntilElementIsDisplay(By.XPath(CLOSE_HELP_BUTTON), timeout:3))
			{
				CustomTestContext.WriteLine("Закрыть окно подсказки сразу после входа в SmartCAT");
				CloseHelpButton.Click();
				Driver.WaitUntilElementIsDisappeared(By.XPath(DIALOG_BACKGROUND));
			}

			
			return GetPage();
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
		/// Проверить, что фон диалога исчез
		/// </summary>
		public T AssertDialogBackgroundDisappeared<T>(WebDriver driver) where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Дождаться закрытия фона диалога.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DIALOG_BACKGROUND)),
				"Ошибка: фон диалога не закрылся.");

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Дождаться, что фон диалога исчез
		/// </summary>
		public void WaitUntilDialogBackgroundDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что фон диалога исчез.");

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(DIALOG_BACKGROUND)))
			{
				throw new XPathLookupException("Произошла ошибка: фон диалога не закрылся");
			}
		}

		/// <summary>
		/// Проверить, что 'Lingvo Dictionaries' отсутствует в меню
		/// </summary>
		public WorkspacePage AssertLingvoDictionariesMenuIsNotDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что 'Lingvo Dictionaries' отсутствует в меню.");

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(LINGVO_DICTIONARIES_MENU)),
				"Произошла ошибка:\n 'Lingvo Dictionaries' присутствует в меню.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что 'Lingvo Dictionaries' присутствует в меню
		/// </summary>
		public WorkspacePage AssertLingvoDictionariesDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, существует ли 'Lingvo Dictionaries' в меню.");

			Assert.IsTrue(LingvoDictionaries.Displayed,
				"Произошла ошибка:\n в меню отсутствует 'Lingvo Dictionaries'.");

			return GetPage();
		}

		/// <summary>
		/// Нажать Search в меню.
		/// </summary>
		public SearchPage ClickSearchButton()
		{
			CustomTestContext.WriteLine("Нажать Search в меню.");
			SearchMenu.Click();

			return new SearchPage(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, что имя пользователя в черной плашке совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedUserName">ожидаемое имя</param>
		public WorkspacePage AssertUserNameMatch(string expectedUserName)
		{
			CustomTestContext.WriteLine("Проверить, что имя {0} пользователя в черной плашке совпадает с ожидаемым именем {1}.",
				currentUserName(), expectedUserName);

			Assert.AreEqual(expectedUserName, currentUserName(),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что имя пользователя в черной плашке совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedUserName">ожидаемое имя</param>
		public bool IsNickNameMatch(string expectedUserName)
		{
			var userName = currentUserName();

			CustomTestContext.WriteLine("Проверить, что имя {0} пользователя в черной плашке совпадает с ожидаемым именем {1}.",
				userName, expectedUserName);

			return userName == expectedUserName;
		}

		/// <summary>
		/// Проверить, что название аккаунта в черной плашке совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedAccountName">ожидаемое имя</param>
		public WorkspacePage AssertAccountNameMatch(string expectedAccountName)
		{
			CustomTestContext.WriteLine("Проверить, что название {0} аккаунта в черной плашке совпадает с ожидаемым именем {1}.",
				currentAccount(), expectedAccountName);

			Assert.AreEqual(expectedAccountName, currentAccount(),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что название аккаунта в черной плашке совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedAccountName">ожидаемое имя</param>
		public bool IsAccountNameMatch(string expectedAccountName)
		{
			CustomTestContext.WriteLine("Проверить, что название {0} аккаунта в черной плашке совпадает с ожидаемым именем {1}.",
				currentAccount(), expectedAccountName);

			return currentAccount() == expectedAccountName;
		}

		/// <summary>
		/// Проверить, что список аккаунтов содержит нужный аккаунт
		/// </summary>
		/// <param name="accountName">имя аккаунт</param>
		public WorkspacePage AssertAccountListContainsAccountName(string accountName)
		{
			CustomTestContext.WriteLine("Проверить, что список аккаунтов содержит {0} аккаунт.", accountName);

			Assert.IsTrue(Driver.SetDynamicValue(How.XPath, ACCOUNT_NAME_IN_LIST, accountName).Displayed,
				"Произошла ошибка:\n список аккаунтов не содержит {0} аккаунт.", accountName);

			return GetPage();
		}

		public WorkspacePage OpenHideMenuIfClosed()
		{
			Driver.WaitPageTotalLoad();

			if (!getIsLeftMenuDisplay())
			{
				assertCatMenuButtonDisplay();

				CustomTestContext.WriteLine("Открыть CAT меню слева.");
				CatMenuOpenButton.JavaScriptClick();
			}

			return GetPage();
		}

		/// <summary>
		/// Обновить страницу
		/// </summary>
		public T RefreshPage<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Обновить страницу.");
			Driver.Navigate().Refresh();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.GetPage();
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
		/// Проверить, раскрыт ли пункт меню Проекты.
		/// </summary>
		public bool IsProjectsMenuExpanded()
		{
			CustomTestContext.WriteLine("Проверить, раскрыт ли пункт меню Проекты.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(PROJECTS_BUTTON));
		}

		/// <summary>
		/// Закрыть все показанные уведомления
		/// </summary>
		public T CloseAllNotifications<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Закрыть все показанные уведомления.");
			Driver.WaitUntilElementIsDisplay(By.XPath(ALL_NOTIFICATIONS), timeout: 30);
			var notificationsCount = Driver.GetElementList(By.XPath(ALL_NOTIFICATIONS)).Count;

			for (var i = notificationsCount; i > 0; i--)
			{
				CustomTestContext.WriteLine("Закрыть сообщение №{0}.", i);
				Notification = Driver.SetDynamicValue(How.XPath, NOTIFICATION, i.ToString());
				Notification.Click();
			}

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Вернуть раскрыто ли главное меню слева
		/// </summary>
		private bool getIsLeftMenuDisplay()
		{
			CustomTestContext.WriteLine("Вернуть, раскрыто ли главное меню слева.");

			return CatMenu.Displayed;
		}

		private WorkspacePage assertCatMenuButtonDisplay()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка открытия меню отображается на странице.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CAT_MENU_OPEN_BUTTON), 20),
				"Произошла ошибка:\n кнопка открытия меню не отображается на странице.");

			return GetPage();
		}

		/// <summary>
		/// Получить имя пользователя из черной плашки
		/// </summary>
		/// <returns>имя пользователя</returns>
		private string currentUserName()
		{
			CustomTestContext.WriteLine("Получить имя пользователя из черной плашки.");

			var accountName = UserName.Text;
			var index = accountName.IndexOf("\r", StringComparison.Ordinal);

			if (index > 0)
			{
				accountName = accountName.Substring(0, index);
			}

			return accountName;
		}

		/// <summary>
		/// Получить имя текущего аккаунта
		/// </summary>
		/// <returns>имя текущего аккаунта</returns>
		private string currentAccount()
		{
			CustomTestContext.WriteLine("Получить имя текущего аккаунта.");

			return CurrentAccountName.Text;
		}

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
		
		[FindsBy(How = How.XPath, Using = ACCOUNT_NAME_IN_LIST)]
		protected IWebElement AccountNameInList { get; set; }

		protected IWebElement Notification { get; set; }

        #endregion

        #region Описания XPath элементов
		
		protected const string CAT_MENU = "//div[contains(@class, 'js-mainmenu')]";
		protected const string CAT_MENU_OPEN_BUTTON = "//div[contains(@class,'g-topbox__header')]/a";
		protected const string CLOSE_HELP_BUTTON = "//div[@class='popup-wrap']//img[@title='Close']";

		protected const string RESOURCES_MENU ="//li[contains(@class, 'js-menuitem-Resources')]";
		protected const string EXPAND_RESOURCES_MENU = "//li[contains(@class, 'js-menuitem-Resources')]//a";
		protected const string PROJECTS_BUTTON = "//a[contains(@href,'/Workspace')]";
		protected const string PROJECTS_MENU = "//li[contains(@class, 'first has-nested-items js-menuitem-SmartCAT')]//a//span[text()='Projects']";
		protected const string USERS_RIGHTS_BUTTON = "//a[contains(@href,'/Users/Index')]";
		protected const string CLIENTS_BUTTON = "//a[contains(@href,'/Clients/Index')]";
		protected const string TRANSLATION_MEMORIES_BUTTON = "//a[contains(@href,'/TranslationMemories/Index')]";
		protected const string GLOSSARY = ".//a[contains(@href,'/Glossaries')]";
		protected const string DOMAIN_REF = ".//a[contains(@href,'/Domains')]";
		protected const string LINGVO_DICTIONARIES_MENU = ".//a[contains(@href,'/LingvoDictionaries')]";
		protected const string SEARCH_MENU = "//div[contains(@class, 'menu-wrapper')]//a[contains(@href,'/Start')]";

		protected const string USER_PICTURE = "//i[contains(@class, 'upic')]";
		protected const string LOCALE_REF = "//div[contains(@class, 'langTools')]//i[contains(@class, '*#*')]";
		protected const string LANGUAGE_BUTTON = "//div[contains(@class, 'language-menu')]//span[contains(@class, 'language-button')]//i";
		protected const string ACCOUNT = "//div[contains(@class,'js-usermenu')]";
		protected const string USER_NAME = "//div[contains(@class,'js-usermenu')]//span[contains(@class,'nameuser')]";
		protected const string LOGOFF = ".//a[contains(@href,'Logout')]";
		protected const string NOTIFIER_LIST = "//div[@id='notifications-block']//div[contains(@class,'notifications-item')]";
		protected const string SIGN_OUT_BUTTON = ".//a[contains(@href,'Logout')]";
		protected const string LICENSES_AND_SERVICES = "//a[contains(@class,'billing')]";

		protected const string LANGUAGE_PICTURE = "//i[text()='*#*']";
		protected const string DIALOG_BACKGROUND = "//div[contains(@class,'js-popup-bg')]";
		protected const string CURRENT_ACCOUNT_NAME = "//span[contains(@class,'g-topbox__nameuser')]//small";
		protected const string ACCOUNT_NAME_IN_LIST = "//li[@class='g-topbox__corpitem' and @title='*#*']";
		
		protected const string ALL_NOTIFICATIONS = "//div[@class='g-notifications-item']//span[2]/a";
		protected const string NOTIFICATION = "//div[@class='g-notifications-item'][*#*]//span[2]/a";

        #endregion
	}
}
