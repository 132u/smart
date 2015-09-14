using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Billing;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
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
		public WorkspacePage GetPage()
		{
			var workspacePage = new WorkspacePage();
			InitPage(workspacePage);

			return workspacePage;
		}

		public void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(USER_PICTURE), 30))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с workspace.");
			}
		}

		/// <summary>
		/// Нажать на кнопку "пользователи и права"
		/// </summary>
		public UsersRightsPage ClickUsersRightsButton()
		{
			Logger.Debug("Нажать кнопку 'Пользователи и права'.");
			UsersRightsButton.Click();

			return new UsersRightsPage().GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Проекты"
		/// </summary>
		public ProjectsPage ClickProjectsButton()
		{
			Logger.Debug("Нажать кнопку 'Проекты'.");
			ProjectsButton.Click();

			return new ProjectsPage().GetPage();
		}


		/// <summary>
		/// Выбрать вкладку "Клиенты"
		/// </summary>
		public ClientsPage ClickClientsButton()
		{
			Logger.Debug("Нажать кнопку 'Клиенты'.");
			ClientsButton.Click();

			return new ClientsPage().GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Группы проектов"
		/// </summary>
		public ProjectGroupsPage ClickProjectGroupsButton()
		{
			Logger.Debug("Нажать кнопку 'Группы проектов'.");
			ProjectGroupsButton.Click();

			return new ProjectGroupsPage().GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Память переводов"
		/// </summary>
		public TranslationMemoriesPage ClickTranslationMemoriesButton()
		{
			Logger.Debug("Нажать кнопку 'Память переводов'.");
			TranslationMemoriesButton.Click();

			return new TranslationMemoriesPage().GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Глоссарии"
		/// </summary>
		public GlossariesPage ClickGlossariesButton()
		{
			Logger.Debug("Нажать кнопку 'Глоссарии'.");
			GlossariesButton.Click();

			return new GlossariesPage().GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Lingvo Dictionaries'
		/// </summary>
		public LingvoDictionariesPage ClickLingvoDictionariesButton()
		{
			Logger.Debug("Нажать кнопку 'Lingvo Dictionaries'.");
			LingvoDictionaries.Click();

			return new LingvoDictionariesPage().GetPage();
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
				Logger.Warn("StaleElementReferenceException: Не удалось найти элемент. Предпринять повторную попытку.");
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
			Logger.Debug("Нажать на имя пользователя и аккаунт, чтобы появилась плашка 'Настройки профиля'.");
			Driver.WaitUntilElementIsDisplay(By.XPath(ACCOUNT));
			Account.JavaScriptClick();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что настройки профиля открылись.
		/// </summary>
		public WorkspacePage AssertAccountProfileDisplayed()
		{
			Logger.Debug("Проверить, что настройки профиля открылись.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(LOGOFF)),
				"Произошла ошибка:\n настройки профиля не открылись.");

			return GetPage();
		}

		/// <summary>
		/// Нажать на 'Licenses and Services'
		/// </summary>
		public BillingPage ClickLicenseAndServices()
		{
			Logger.Debug("Нажать на 'Licenses and Services'.");
			Driver.WaitUntilElementIsDisplay(By.XPath(LICENSES_AND_SERVICES));
			LicenseAndServices.Click();

			return new BillingPage();
		}

		public BillingPage SwitchToLicenseAndServicesWindow()
		{
			Logger.Trace("Переключиться  в окно 'Управление лицензиями'");
			// Sleep нужен, чтоб вторая вкладка успела открыться, иначе количество открытых вкладок посчитается неправильно 
			Thread.Sleep(1000);
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles.First()).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles.Last());
			}

			return new BillingPage();
		}
		/// <summary>
		/// Выйти из смартката
		/// </summary>
		public SignInPage ClickSignOut()
		{
			Logger.Debug("Выйти из смартката.");
			SignOutButton.JavaScriptClick();

			return new SignInPage().GetPage();
		}

		/// <summary>
		/// Смена языка локали
		/// </summary>
		public WorkspacePage SelectLocale(Language language)
		{
			Logger.Debug("Сменить язык локали на {0}, если необходимо.", language);
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
		public WorkspacePage ClickCloseHelp()
		{
			Logger.Trace("Проверить, открыта ли подсказка при входе в SmartCAT.");

			if (Driver.WaitUntilElementIsDisplay(By.XPath(CLOSE_HELP_BUTTON), timeout:3))
			{
				Logger.Debug("Закрыть окно подсказки сразу после входа в SmartCAT");
				CloseHelpButton.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Вернуть количество уведомлений об экспорте
		/// </summary>
		public int GetCountExportNotifiers()
		{
			Logger.Trace("Узнать, сколько уведомлений есть на данный момент.");
			var countExportNotifiers = Driver.GetElementList(By.XPath(NOTIFIER_LIST)).Count;
			Logger.Trace("Обнаружено {0} уведомлений.", countExportNotifiers);

			return countExportNotifiers;
		}

		public T AssertDialogBackgroundDisappeared<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Trace("Дождаться закрытия фона диалога.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DIALOG_BACKGROUND)),
				"Ошибка: фон диалога не закрылся.");
			
			return new T().GetPage();
		}

		/// <summary>
		/// Проверить, что 'Lingvo Dictionaries' отсутствует в меню
		/// </summary>
		public WorkspacePage AssertLingvoDictionariesMenuIsNotDisplayed()
		{
			Logger.Trace("Проверить, что 'Lingvo Dictionaries' отсутствует в меню.");

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(LINGVO_DICTIONARIES_MENU)),
				"Произошла ошибка:\n 'Lingvo Dictionaries' присутствует в меню.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что 'Lingvo Dictionaries' присутствует в меню
		/// </summary>
		public WorkspacePage AssertLingvoDictionariesDisplayed()
		{
			Logger.Trace("Проверить, существует ли 'Lingvo Dictionaries' в меню.");

			Assert.IsTrue(LingvoDictionaries.Displayed,
				"Произошла ошибка:\n в меню отсутствует 'Lingvo Dictionaries'.");

			return GetPage();
		}

		/// <summary>
		/// Нажать Search в меню.
		/// </summary>
		public SearchPage ClickSearchButton()
		{
			Logger.Debug("Нажать Search в меню.");
			SearchMenu.Click();

			return new SearchPage().GetPage();
		}

		/// <summary>
		/// Проверить, что имя пользователя в черной плашке совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedUserName">ожидаемое имя</param>
		public WorkspacePage AssertUserNameMatch(string expectedUserName)
		{
			Logger.Trace("Проверить, что имя {0} пользователя в черной плашке совпадает с ожидаемым именем {1}.",
				currentUserName(), expectedUserName);

			Assert.AreEqual(expectedUserName, currentUserName(),
				"Произошла ошибка:\n имя пользователя в черной плашке не совпадает с ожидаемым именем.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что название аккаунта в черной плашке совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedAccountName">ожидаемое имя</param>
		public WorkspacePage AssertAccountNameMatch(string expectedAccountName)
		{
			Logger.Trace("Проверить, что название {0} аккаунта в черной плашке совпадает с ожидаемым именем {1}.",
				currentAccount(), expectedAccountName);

			Assert.AreEqual(expectedAccountName, currentAccount(),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что список аккаунтов содержит нужный аккаунт
		/// </summary>
		/// <param name="accountName">имя аккаунт</param>
		public WorkspacePage AssertAccountListContainsAccountName(string accountName)
		{
			Logger.Trace("Проверить, что список аккаунтов содержит {0} аккаунт.", accountName);

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

				Logger.Debug("Открыть CAT меню слева.");
				CatMenuOpenButton.JavaScriptClick();
			}

			return GetPage();
		}
		
		/// <summary>
		/// Обновить страницу
		/// </summary>
		public T RefreshPage<T>() where T: class, IAbstractPage<T>, new()
		{
			Logger.Debug("Обновить страницу.");
			Driver.Navigate().Refresh();

			return new T().GetPage();
		}

		/// <summary>
		/// Подтвердить, что на странице нет алертов.
		/// </summary>
		public WorkspacePage AssertAlertNoExist()
		{
			Logger.Trace("Подтвердить, что на странице нет алертов.");

			Assert.Throws<NoAlertPresentException>(() => Driver.SwitchTo().Alert().Accept(), 
				"Произошла ошибка:\n алерт не должен появляться.");

			return GetPage();
		}

		/// <summary>
		/// Закрыть все показанные уведомления
		/// </summary>
		public T CloseAllNotifications<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Закрыть все показанные уведомления.");
			Driver.WaitUntilElementIsDisplay(By.XPath(ALL_NOTIFICATIONS), timeout: 30);
			var notificationsCount = Driver.GetElementList(By.XPath(ALL_NOTIFICATIONS)).Count;

			for (var i = notificationsCount; i > 0; i--)
			{
				Logger.Debug("Закрыть сообщение №{0}.", i);
				Notification = Driver.SetDynamicValue(How.XPath, NOTIFICATION, i.ToString());
				Notification.Click();
			}

			return new T().GetPage();
		}

		/// <summary>
		/// Вернуть раскрыто ли главное меню слева
		/// </summary>
		private bool getIsLeftMenuDisplay()
		{
			Logger.Trace("Вернуть, раскрыто ли главное меню слева.");

			return CatMenu.Displayed;
		}

		private WorkspacePage assertCatMenuButtonDisplay()
		{
			Logger.Trace("Проверить, что кнопка открытия меню отображается на странице.");

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
			Logger.Trace("Получить имя пользователя из черной плашки.");

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
			Logger.Trace("Получить имя текущего аккаунта.");

			return CurrentAccountName.Text;
		}

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

		[FindsBy(How = How.XPath, Using = ACCOUNT_NAME_IN_LIST)]
		protected IWebElement AccountNameInList { get; set; }

		protected IWebElement Notification { get; set; }
		
		protected const string CAT_MENU = "//div[contains(@class, 'js-mainmenu')]";
		protected const string CAT_MENU_OPEN_BUTTON = "//h2[contains(@class,'g-topbox__header')]/a";
		protected const string CLOSE_HELP_BUTTON = "//div[@class='popup-wrap']//img[@title='Close']";

		protected const string RESOURCES_MENU ="//ul[contains(@class, 'serviceMenu')]//li[contains(@class, 'js-menuitem-Resources')]";
		protected const string EXPAND_RESOURCES_MENU = "//ul[contains(@class, 'serviceMenu')]//li[contains(@class, 'js-menuitem-Resources')]//a";
		protected const string PROJECTS_BUTTON = "//a[contains(@href,'/Workspace')]";
		protected const string USERS_RIGHTS_BUTTON = "//a[contains(@href,'/Users/Index')]";
		protected const string CLIENTS_BUTTON = "//a[contains(@href,'/Clients/Index')]";
		protected const string TRANSLATION_MEMORIES_BUTTON = "//a[contains(@href,'/TranslationMemories/Index')]";
		protected const string GLOSSARY = ".//a[contains(@href,'/Glossaries')]";
		protected const string DOMAIN_REF = ".//a[contains(@href,'/Domains')]";
		protected const string LINGVO_DICTIONARIES_MENU = ".//a[contains(@href,'/LingvoDictionaries')]";
		protected const string SEARCH_MENU = "//div[contains(@class, 'menu-wrapper')]//a[contains(@href,'/Start')]";

		protected const string USER_PICTURE = "//i[contains(@class, 'upic')]";
		protected const string LOCALE_REF = "//a[contains(@class,'js-set-locale') and contains(@data-locale, '*#*')]";
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
	}
}
