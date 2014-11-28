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
			ClickElement(By.XPath(WORKSPACE_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на Domain
		/// </summary>
		public void ClickOpenDomainPage()
		{
			ClickElement(By.XPath(DOMAIN_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на TM
		/// </summary>
		public void ClickOpenTMPage()
		{
			ClickElement(By.XPath(TM_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на глоссарии
		/// </summary>
		public void ClickOpenGlossaryPage()
		{
			ClickElement(By.XPath(GLOSSARY_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на страницу предложенных терминов
		/// </summary>
		public void ClickOpenSuggestTermsPage()
		{
			ClickElement(By.XPath(SUGGEST_TERMS_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на страницу поиска
		/// </summary>
		public void ClickOpenSearchPage()
		{
			ClickElement(By.XPath(SEARCH_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на страницу клиентов
		/// </summary>
		public void ClickOpenClientPage()
		{
			ClickElement(By.XPath(CLIENT_REF_XPATH));
		}

		/// <summary>
		/// Кликнуть для перехода на страницу словарей
		/// </summary>
		public void ClickOpenDictionariesPage()
		{
			ClickElement(By.XPath(DICTIONARIES_REF_XPATH));
		}

		/// <summary>
		/// Вернуть, видна ли ссылка на страницу словарей
		/// </summary>
		/// <returns>видна</returns>
		public bool GetIsRefDictionariesVisible()
		{
			return GetIsElementDisplay(By.XPath(DICTIONARIES_REF_XPATH));
		}

		/// <summary>
		/// Открыть профиль
		/// </summary>
		public void OpenProfile()
		{
			ClickElement(By.XPath(OPEN_PROFILE_XPATH));
		}

		/// <summary>
		/// Дождаться открытия профила
		/// </summary>
		/// <returns>открылся</returns>
		public bool WaitProfileOpen()
		{
			return WaitUntilDisplayElement(By.XPath(PROFILE_FORM_XPATH));
		}

		/// <summary>
		/// Получить имя пользователя из профиля
		/// </summary>
		/// <returns>имя</returns>
		public string GetUserNameProfile()
		{
			return GetElementAttribute(By.XPath(PROFILE_USER_NAME_XPATH), "value");
		}

		/// <summary>
		/// Закрыть профиль
		/// </summary>
		public void CloseProfile()
		{
			ClickElement(By.XPath(CLOSE_PROFILE_XPATH));
		}



		protected const string WORKSPACE_REF_XPATH = ".//a[contains(@href,'/Workspace')]";
		protected const string DOMAIN_REF_XPATH = ".//a[contains(@href,'/Domains')]";
		protected const string TM_REF_XPATH = ".//a[contains(@href,'/TranslationMemories/Index')]"; // TODO проверить ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Enterprise/TranslationMemories')]
		protected const string GLOSSARY_REF_XPATH = ".//a[contains(@href,'/Glossaries')]";
		protected const string SUGGEST_TERMS_REF_XPATH = ".//a[contains(@href,'/Suggests')]";
		protected const string SEARCH_REF_XPATH = ".//a[contains(@href,'/Start')]";
		protected const string CLIENT_REF_XPATH = ".//a[contains(@href,'/Clients')]";
		protected const string DICTIONARIES_REF_XPATH = ".//a[contains(@href,'/LingvoDictionaries')]";

		protected const string OPEN_PROFILE_XPATH = "//a[@class='js-link-profile g-redbtn__text g-btn__text']";
		protected const string PROFILE_FORM_XPATH = "//div[contains(@class,'g-popupbox g-profile')]";
		protected const string PROFILE_USER_NAME_XPATH = PROFILE_FORM_XPATH + "//p[contains(@class, 'name')]//input";
		protected const string CLOSE_PROFILE_XPATH = PROFILE_FORM_XPATH + "//span[contains(@class, 'js-popup-close')]//a";
	}
}