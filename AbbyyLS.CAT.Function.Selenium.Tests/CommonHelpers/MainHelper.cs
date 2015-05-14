using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Основной хелпер
	/// </summary>
	public class MainHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор основного хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public MainHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
		}

		/// <summary>
		/// Кликнуть для перехода на Workspace
		/// </summary>
		public void ClickOpenWorkSpacePage()
		{
			if (!GetIsLeftMenuDisplay())
				OpenHideMenu();
			Logger.Trace("Клик по 'Projects' в главном меню слева");
			ClickElement(By.XPath(WORKSPACE_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на Domain
		/// </summary>
		public void ClickOpenDomainPage()
		{
			if (!GetIsLeftMenuDisplay())
				OpenHideMenu();
			Logger.Trace("Клик по 'Project Groups' в главном меню слева");
			ClickElement(By.XPath(DOMAIN_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на TM
		/// </summary>
		public void ClickOpenTMPage()
		{
			Logger.Trace("Проверка, что подменю Resources раскрыто");
			if (!GetIsElementDisplay(By.XPath(TM_REF_XPATH)))
				ClickResourcesRef();
			Logger.Trace("Клик по 'Translation Memories' в главном меню слева");
			ClickElement(By.XPath(TM_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть на ссылку "Ресурсы"
		/// </summary>
		public void ClickResourcesRef()
		{
			Logger.Trace("Кликаем на ссылку Ресурсы.");
			if (!GetIsLeftMenuDisplay())
				OpenHideMenu();
			ExpandResourcesMenu(GLOSSARY_REF_XPATH);
		}

		/// <summary>
		/// Кликнуть для перехода на глоссарии
		/// </summary>
		public void ClickOpenGlossaryPage()
		{
			if (!GetIsLeftMenuDisplay())
				OpenHideMenu();
			Logger.Trace("Клик по 'Glossaries' в главном меню слева");
			ClickElement(By.XPath(GLOSSARY_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на страницу предложенных терминов
		/// </summary>
		public void ClickOpenSuggestTermsPage()
		{
			Logger.Trace("Клик для перехода на страницу предложенных терминов");
			ClickElement(By.XPath(SUGGEST_TERMS_REF_XPATH));
		}

		public void ClickOpenSearchPage()
		{
			Logger.Trace("Проверка, что подменю Resources раскрыто");
			if (!GetIsElementDisplay(By.XPath(SEARCH_REF_XPATH)))
			ExpandResourcesMenu(SEARCH_REF_XPATH);

			ClickElement(By.XPath(SEARCH_REF_XPATH));
		}

		protected void ExpandResourcesMenu(string xpath)
		{
			if (!GetIsElementDisplay(By.XPath(xpath)))
			{
				Logger.Trace("Клик по 'Resources' в главном меню слева");
				ClickElement(By.XPath(RESOURCES_REF_XPATH));
			}
		}

		/// <summary>
		/// Кликнуть для перехода на страницу клиентов
		/// </summary>
		public void ClickOpenClientPage()
		{
			if (!GetIsLeftMenuDisplay())
				OpenHideMenu();
			Logger.Trace("Клик по 'Clients' в главном меню слева");
			ClickElement(By.XPath(CLIENT_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на страницу словарей
		/// </summary>
		public void ClickOpenDictionariesPage()
		{
			if (!GetIsElementDisplay(By.XPath(DICTIONARIES_REF_XPATH)))
				ClickResourcesRef();
			ExpandResourcesMenu(DICTIONARIES_REF_XPATH);

			Logger.Trace("Клик по 'Lingvo Dictionaries' в главном меню слева");
			ClickElement(By.XPath(DICTIONARIES_REF_XPATH));
		}

		/// <summary>
		/// Вернуть, видна ли ссылка на странице словарей
		/// </summary>
		/// <returns>видна</returns>
		public bool GetIsRefDictionariesVisible()
		{
			if (!GetIsLeftMenuDisplay())
				OpenHideMenu();
			ExpandResourcesMenu(DICTIONARIES_REF_XPATH);

			Logger.Trace("Вернуть, видна ли ссылка на странице словарей");
			return GetIsElementDisplay(By.XPath(DICTIONARIES_REF_XPATH));
		}

		/// <summary>
		/// Открыть профиль
		/// </summary>
		public void OpenProfile()
		{
			Logger.Trace("Клик по 'Profile settings'");
			ClickElement(By.XPath(OPEN_PROFILE_XPATH));
		}

		/// <summary>
		/// Дождаться открытия профиля
		/// </summary>
		/// <returns>открылся</returns>
		public bool WaitProfileOpen()
		{
			Logger.Trace("Вернуть открылось ли диалоговое окно настройки профиля");
			return WaitUntilDisplayElement(By.XPath(PROFILE_FORM_XPATH));
		}

		/// <summary>
		/// Получить имя пользователя из профиля
		/// </summary>
		/// <returns>имя</returns>
		public string GetUserNameProfile()
		{
			Logger.Trace("Получить имя пользователя из профиля");
			return GetElementAttribute(By.XPath(PROFILE_USER_NAME_XPATH), "value");
		}

		/// <summary>
		/// Закрыть профиль
		/// </summary>
		public void CloseProfile()
		{
			Logger.Trace("Клик по кнопке 'Close' в диалоговом окне настройки профиля");
			ClickElement(By.XPath(CLOSE_PROFILE_XPATH));
		}

		public bool GetIsLeftMenuDisplay()
		{
			Logger.Trace("Вернуть раскрыто ли главное меню слева");
			return GetIsElementDisplay(By.XPath(LEFT_MENU));
		}

		public void OpenHideMenu()
		{
			Logger.Trace("Главное меню слева скрыто. Клик по кнопке открытия меню");
			Driver.FindElement(By.XPath(MENU_OPEN_BTN)).Click();
		}

		protected const string MENU_OPEN_BTN = "//h2[contains(@class,'g-topbox__header')]/a";
		protected const string LEFT_MENU = "//div[contains(@class, 'js-mainmenu')]";

		protected const string WORKSPACE_REF_XPATH = ".//a[contains(@href,'/Workspace')]";
		protected const string DOMAIN_REF_XPATH = ".//a[contains(@href,'/Domains')]";
		protected const string TM_REF_XPATH = ".//a[contains(@href,'/TranslationMemories/Index')]"; // TODO проверить ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Enterprise/TranslationMemories')]
		protected const string GLOSSARY_REF_XPATH = ".//a[contains(@href,'/Glossaries')]";
		protected const string RESOURCES_REF_XPATH = "//a[contains(@class, 'menuitem-Resources')]";
		protected const string SUGGEST_TERMS_REF_XPATH = ".//a[contains(@href,'/Suggests')]";
		protected const string SEARCH_REF_XPATH = "//div[contains(@class, 'menu-wrapper')]//a[contains(@href,'/Start')]";
		protected const string CLIENT_REF_XPATH = ".//a[contains(@href,'/Clients')]";
		protected const string DICTIONARIES_REF_XPATH = ".//a[contains(@href,'/LingvoDictionaries')]";

		protected const string OPEN_PROFILE_XPATH = "//a[contains(@class,'js-menuitem-profile')]";
		protected const string PROFILE_FORM_XPATH = "//div[contains(@class,'g-popupbox g-profile')]";
		protected const string PROFILE_USER_NAME_XPATH = PROFILE_FORM_XPATH + "//p[contains(@class, 'name')]//input";
		protected const string CLOSE_PROFILE_XPATH = PROFILE_FORM_XPATH + "//span[contains(@class, 'js-popup-close')]//a";
	}
}