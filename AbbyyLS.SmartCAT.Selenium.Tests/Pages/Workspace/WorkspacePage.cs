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
			if (!Driver.WaitUntilElementIsPresent(By.XPath(PROJECTS_BTN_XPATH), 15))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с workspace.");
			}
		}

		/// <summary>
		/// Нажать на кнопку "пользователи и права"
		/// </summary>
		public UsersRightsPage ClickUsersRightsBtn()
		{
			Logger.Trace("Нажимаем кнопку 'Пользователи и права'.");
			UsersRightsBtn.Click();
			var usersRightPage = new UsersRightsPage();
			return usersRightPage.GetPage();
		}

		/// <summary>
		/// Выбрать вкладку "Проекты"
		/// </summary>
		public WorkspacePage ClickProjectsBtn()
		{
			Logger.Trace("Выбираем вкладку 'Проекты'.");
			ProjectsBtn = Driver.SetDynamicValue(How.XPath, PROJECTS_BTN_XPATH, "");
			ProjectsBtn.Click();
			return GetPage();
		}

		/// <summary>
		/// Нажать на имя пользователя и аккаунт, чтобы появилась плашка "Настройки профиля"
		/// </summary>
		public WorkspacePage ClickAccount()
		{
			Logger.Trace("Нажимаем на имя пользователя и аккаунт, чтобы появилась плашка 'Настройки профиля'.");
			Account.Click();
			return GetPage();
		}

		/// <summary>
		/// Выйти из смартката
		/// </summary>
		public SignInPage ClickLogOffRef()
		{
			Logger.Trace("Выходим из смартката.");
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
			Logger.Trace("Меняем язык локали на {0}, если необходимо.", language);
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

		[FindsBy(How = How.XPath, Using = USERS_RIGHTS_BTN_XPATH)]
		protected IWebElement UsersRightsBtn { get; set; }

		[FindsBy(How = How.XPath, Using = ACCOUNT_XPATH)]
		protected IWebElement Account { get; set; }

		[FindsBy(How = How.XPath, Using = LOGOFF_XPATH)]
		protected IWebElement LogOffRef { get; set; }

		protected IWebElement LocaleRef { get; set; }

		protected IWebElement ProjectsBtn { get; set; }

		protected const string PROJECTS_BTN_XPATH = "//a[contains(@href,'/Workspace')]";
		protected const string USERS_RIGHTS_BTN_XPATH = "//a[contains(@href,'/Users/Index')]";

		protected const string LOCALE_REF_XPATH = "//a[contains(@class,'js-set-locale') and contains(@data-locale, '*#*')]";
		protected const string ACCOUNT_XPATH = ".//div[contains(@class,'js-corp-account')]";
		protected const string USER_NAME_XPATH = ACCOUNT_XPATH + "//span[contains(@class,'nameuser')]";
		protected const string LOGOFF_XPATH = ".//a[contains(@href,'Logout')]";
	}
}
