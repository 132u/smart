using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace
{
	public class WorkspacePage : BaseObject, IAbstractPage<WorkspacePage>
	{

		public WorkspacePage GetPage()
		{
			var workspacePage = new WorkspacePage();
			InitPage(workspacePage);
			LoadPage();
			return workspacePage;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(ACCOUNT_XPATH), 15))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с workspace.");
			}
		}

		/// <summary>
		/// Нажать на кнопку "пользователи и права"
		/// </summary>
		public UsersRightsPage ClickUsersRightsBtn()
		{
			Logger.Debug("Нажать кнопку 'Пользователи и права'.");
			openHideMenuIfClosed();
			UsersRightsBtn.Click();
			var usersRightPage = new UsersRightsPage();

			return usersRightPage.GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Проекты"
		/// </summary>
		public WorkspacePage ClickProjectsBtn()
		{
			Logger.Debug("Нажать кнопку 'Проекты'.");
			openHideMenuIfClosed();
			ProjectsBtn = Driver.SetDynamicValue(How.XPath, PROJECTS_BTN_XPATH, "");
			ProjectsBtn.Click();

			return GetPage();
		}


		/// <summary>
		/// Выбрать вкладку "Клиенты"
		/// </summary>
		public ClientsPage ClickClientsBtn()
		{
			Logger.Debug("Нажать кнопку 'Клиенты'.");
			openHideMenuIfClosed();
			ClientsBtn.Click();
			var clientsPage = new ClientsPage();

			return clientsPage.GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Память переводов"
		/// </summary>
		public TranslationMemoriesPage ClickTranslationMemoriesBtn()
		{
			Logger.Debug("Нажать кнопку 'Память переводов'.");
			openHideMenuIfClosed();
			TranslationMemoriesBtn.Click();
			var translationMemoriesPage = new TranslationMemoriesPage();

			return translationMemoriesPage.GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Глоссарии"
		/// </summary>
		public GlossariesPage ClickGlossariesBtn()
		{
			Logger.Debug("Нажать кнопку 'Глоссарии'.");
			openHideMenuIfClosed();
			GlossariesBtn.Click();
			var glossariesPage = new GlossariesPage();

			return glossariesPage.GetPage();
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
			var signInPage = new SignInPage();

			return signInPage.GetPage();
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
				if (Driver.WaitUntilElementIsPresent(By.XPath(LOCALE_REF_XPATH.Replace("*#*", "en")), 1))
				{
					LocaleRef = Driver.SetDynamicValue(How.XPath, LOCALE_REF_XPATH, "en");
					LocaleRef.Click();
				}
			}
			else if (language.ToLower() == "russian")
			{
				if (Driver.WaitUntilElementIsPresent(By.XPath(LOCALE_REF_XPATH.Replace("*#*", "ru")), 1))
				{
					LocaleRef = Driver.SetDynamicValue(How.XPath, LOCALE_REF_XPATH, "ru");
					LocaleRef.Click();
				}
			}

			return GetPage();
		}

		private void openHideMenuIfClosed()
		{
			if (!getIsLeftMenuDisplay())
			{
				Logger.Debug("Открыть CAT меню слева.");
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

		[FindsBy(How = How.XPath, Using = USERS_RIGHTS_BTN_XPATH)]
		protected IWebElement UsersRightsBtn { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENTS_BTN_XPATH)]
		protected IWebElement ClientsBtn { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATION_MEMORIES_BTN_XPATH)]
		protected IWebElement TranslationMemoriesBtn { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_XPATH)]
		protected IWebElement GlossariesBtn { get; set; }

		[FindsBy(How = How.XPath, Using = ACCOUNT_XPATH)]
		protected IWebElement Account { get; set; }

		[FindsBy(How = How.XPath, Using = LOGOFF_XPATH)]
		protected IWebElement LogOffRef { get; set; }


		[FindsBy(How = How.XPath, Using = RESOURCES_MENU_XPATH)]
		protected IWebElement ResourcesMenu { get; set; }

		[FindsBy(How = How.XPath, Using = EXPAND_RESOURCES_MENU_XPATH)]
		protected IWebElement ExpandResourcesMenuButton { get; set; }

		[FindsBy(How = How.XPath, Using = CAT_MENU)]
		protected IWebElement CatMenu { get; set; }

		[FindsBy(How = How.XPath, Using = CAT_MENU_OPEN_BTN)]
		protected IWebElement CatMenuOpenButton { get; set; }

		protected IWebElement LocaleRef { get; set; }

		protected IWebElement ProjectsBtn { get; set; }

		protected const string CAT_MENU = "//div[contains(@class, 'js-mainmenu')]";
		protected const string CAT_MENU_OPEN_BTN = "//h2[@class='g-topbox__header']/a";

		protected const string RESOURCES_MENU_XPATH ="//ul[contains(@class, 'serviceMenu')]//li[contains(@class, 'js-menuitem-Resources')]";
		protected const string EXPAND_RESOURCES_MENU_XPATH = RESOURCES_MENU_XPATH + "//a";
		protected const string PROJECTS_BTN_XPATH = "//a[contains(@href,'/Workspace')]";
		protected const string USERS_RIGHTS_BTN_XPATH = "//a[contains(@href,'/Users/Index')]";
		protected const string CLIENTS_BTN_XPATH = "//a[contains(@href,'/Clients/Index')]";
		protected const string TRANSLATION_MEMORIES_BTN_XPATH = "//a[contains(@href,'/TranslationMemories/Index')]";
		protected const string GLOSSARY_XPATH = ".//a[contains(@href,'/Glossaries')]";

		protected const string LOCALE_REF_XPATH = "//a[contains(@class,'js-set-locale') and contains(@data-locale, '*#*')]";
		protected const string ACCOUNT_XPATH = "//div[contains(@class,'js-usermenu')]";
		protected const string USER_NAME_XPATH = ACCOUNT_XPATH + "//span[contains(@class,'nameuser')]";
		protected const string LOGOFF_XPATH = ".//a[contains(@href,'Logout')]";
	}
}
