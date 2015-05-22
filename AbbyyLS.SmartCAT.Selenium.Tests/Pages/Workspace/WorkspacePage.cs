using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

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
			if (!Driver.WaitUntilElementIsEnabled(By.XPath(USER_PICTURE), 15))
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
			openHideMenuIfClosed();
			UsersRightsButton.Click();

			return new UsersRightsPage().GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Проекты"
		/// </summary>
		public WorkspacePage ClickProjectsButton()
		{
			Logger.Debug("Нажать кнопку 'Проекты'.");
			openHideMenuIfClosed();
			ProjectsButton = Driver.SetDynamicValue(How.XPath, PROJECTS_BUTTON, "");
			ProjectsButton.Click();

			return GetPage();
		}


		/// <summary>
		/// Выбрать вкладку "Клиенты"
		/// </summary>
		public ClientsPage ClickClientsButton()
		{
			Logger.Debug("Нажать кнопку 'Клиенты'.");
			openHideMenuIfClosed();
			ClientsButton.Click();

			return new ClientsPage().GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Группы проектов"
		/// </summary>
		public ProjectGroupsPage ClickProjectGroupsButton()
		{
			Logger.Debug("Нажать кнопку 'Группы проектов'.");
			openHideMenuIfClosed();
			ProjectGroupsButton.Click();

			return new ProjectGroupsPage().GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Память переводов"
		/// </summary>
		public TranslationMemoriesPage ClickTranslationMemoriesButton()
		{
			Logger.Debug("Нажать кнопку 'Память переводов'.");
			openHideMenuIfClosed();
			TranslationMemoriesButton.Click();

			return new TranslationMemoriesPage().GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Глоссарии"
		/// </summary>
		public GlossariesPage ClickGlossariesButton()
		{
			Logger.Debug("Нажать кнопку 'Глоссарии'.");
			openHideMenuIfClosed();
			GlossariesButton.Click();

			return new GlossariesPage().GetPage();
		}

		/// <summary>
		/// Развернуть меню ресурсов, если оно свернуто
		/// </summary>
		public WorkspacePage ExpandResourcesIfNotExpanded()
		{
			openHideMenuIfClosed();

			if (!ResourcesMenu.GetAttribute("class").Contains("nested-expanded"))
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
			Account.Click();

			return GetPage();
		}

		/// <summary>
		/// Выйти из смартката
		/// </summary>
		public SignInPage ClickLogOffRef()
		{
			Logger.Debug("Выйти из смартката.");
			LogOffRef.Click();

			return new SignInPage().GetPage();
		}

		/// <summary>
		/// Смена языка локали
		/// </summary>
		/// <param name="language">желаемый язык</param>
		public WorkspacePage SelectLocale(string language)
		{
			Logger.Debug("Сменить язык локали на {0}, если необходимо.", language);
			if (language.ToLower() == "english")
			{
				if (Driver.WaitUntilElementIsDisplay(By.XPath(LOCALE_REF_XPATH.Replace("*#*", "en")), 1))
				{
					LocaleRef = Driver.SetDynamicValue(How.XPath, LOCALE_REF_XPATH, "en");
					LocaleRef.Click();
				}
			}
			else if (language.ToLower() == "russian")
			{
				if (Driver.WaitUntilElementIsDisplay(By.XPath(LOCALE_REF_XPATH.Replace("*#*", "ru")), 1))
				{
					LocaleRef = Driver.SetDynamicValue(How.XPath, LOCALE_REF_XPATH, "ru");
					LocaleRef.Click();
				}
			}

			return GetPage();
		}

		/// <summary>
		/// Закрыть подсказку (если она открыта) сразу после входа в SmartCAT
		/// </summary>
		/// <returns></returns>
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

		private void openHideMenuIfClosed()
		{
			if (!getIsLeftMenuDisplay())
			{
				Logger.Debug("Открыть CAT меню слева.");
				Driver.WaitUntilElementIsDisplay(By.XPath(CAT_MENU_OPEN_BUTTON));
				CatMenuOpenButton.Click();
			}
		}

		/// <summary>
		/// Вернуть раскрыто ли главное меню слева
		/// </summary>
		private bool getIsLeftMenuDisplay()
		{
			Logger.Trace("Вернуть, раскрыто ли главное меню слева.");
			
			return CatMenu.Displayed;
		}

		[FindsBy(How = How.XPath, Using = USERS_RIGHTS_BUTTON)]
		protected IWebElement UsersRightsButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENTS_BUTTON)]
		protected IWebElement ClientsButton { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATION_MEMORIES_BUTTON)]
		protected IWebElement TranslationMemoriesButton { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY)]
		protected IWebElement GlossariesButton { get; set; }

		[FindsBy(How = How.XPath, Using = DOMAIN_REF)]
		protected IWebElement ProjectGroupsButton { get; set; }

		[FindsBy(How = How.XPath, Using = ACCOUNT)]
		protected IWebElement Account { get; set; }

		[FindsBy(How = How.XPath, Using = LOGOFF)]
		protected IWebElement LogOffRef { get; set; }


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

		protected IWebElement LocaleRef { get; set; }

		protected IWebElement ProjectsButton { get; set; }

		protected const string CAT_MENU = "//div[contains(@class, 'js-mainmenu')]";
		protected const string CAT_MENU_OPEN_BUTTON = "//h2[contains(@class,'g-topbox__header')]/a";
		protected const string CLOSE_HELP_BUTTON = "//div[@class='hopscotch-bubble animated']//button[contains(@class,'hopscotch-cta')]";

		protected const string RESOURCES_MENU ="//ul[contains(@class, 'serviceMenu')]//li[contains(@class, 'js-menuitem-Resources')]";
		protected const string EXPAND_RESOURCES_MENU = "//ul[contains(@class, 'serviceMenu')]//li[contains(@class, 'js-menuitem-Resources')]//a";
		protected const string PROJECTS_BUTTON = "//a[contains(@href,'/Workspace')]";
		protected const string USERS_RIGHTS_BUTTON = "//a[contains(@href,'/Users/Index')]";
		protected const string CLIENTS_BUTTON = "//a[contains(@href,'/Clients/Index')]";
		protected const string TRANSLATION_MEMORIES_BUTTON = "//a[contains(@href,'/TranslationMemories/Index')]";
		protected const string GLOSSARY = ".//a[contains(@href,'/Glossaries')]";
		protected const string DOMAIN_REF = ".//a[contains(@href,'/Domains')]";

		protected const string USER_PICTURE = "//i[contains(@class, 'upic')]";
		protected const string LOCALE_REF_XPATH = "//a[contains(@class,'js-set-locale') and contains(@data-locale, '*#*')]";
		protected const string ACCOUNT = "//div[contains(@class,'js-usermenu')]";
		protected const string USER_NAME = "//div[contains(@class,'js-usermenu')]//span[contains(@class,'nameuser')]";
		protected const string LOGOFF = ".//a[contains(@href,'Logout')]";
	}
}
