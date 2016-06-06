using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.MachineTranslation
{
	public abstract class FastMTSettingsBasePage<TPage> : WorkspacePage, IAbstractPage<TPage>
		where TPage :class
	{
		protected FastMTSettingsBasePage(WebDriver driver): base(driver)
		{
		}

		public new abstract TPage LoadPage();

		#region Простые методы

		/// <summary>
		/// Нажать, чтобы открылся выпадающий список для выбора Исходного Языка
		/// </summary>
		public TPage ClickSourceLangDropdown()
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список Исходного Языка");
			SourceLangDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать Исходный Язык
		/// </summary>
		/// <param name="lang">исходный язык</param>
		public TPage SelectSourceLanguageInList(Language lang)
		{
			CustomTestContext.WriteLine("Выбрать исходный язык {0}", lang);
			SourceLangItem = Driver.SetDynamicValue(How.XPath, SOURCE_LANG_ITEM, lang.ToString());
			SourceLangItem.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать, чтобы открылся выпадающий список для выбора Языка Перевода
		/// </summary>
		public TPage ClickTargetLangDropdown()
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список Языка Перевода");
			TargetLangDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать Язык Перевода
		/// </summary>
		/// <param name="lang">язык перевода</param>
		public TPage SelectTargetLangiageInList(Language lang)
		{
			CustomTestContext.WriteLine("Выбрать языка перевода {0}", lang);
			TargetLangItem = Driver.SetDynamicValue(How.XPath, TARGET_LANG_ITEM, lang.ToString());
			TargetLangItem.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на выпадающий список выбора МТ
		/// </summary>
		public TPage ClickMTDropdown()
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список выбора МТ");
			MachineTranslationDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать движок машинного перевода
		/// </summary>
		/// <param name="mtEngine">движок машинного перевода</param>
		public TPage SelectMachineTranslationEngineInList(MachineTranslationType mtEngine)
		{
			CustomTestContext.WriteLine("Выбрать движок машинного перевода {0}", mtEngine);
			MTEngineItem = Driver.SetDynamicValue(How.XPath, MT_ITEM, mtEngine.Description());
			MTEngineItem.JavaScriptClick();

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать исходный язык
		/// </summary>
		/// <param name="sourceLanguage">исходный язык</param>
		public TPage SetSourceLanguage(Language sourceLanguage = Language.English)
		{
			ClickSourceLangDropdown();
			SelectSourceLanguageInList(sourceLanguage);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		/// <param name="targetLanguage">язык перевода</param>
		public TPage SetTargetLanguage(Language targetLanguage = Language.Russian)
		{
			ClickTargetLangDropdown();
			SelectTargetLangiageInList(targetLanguage);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать движок МТ-перевода
		/// </summary>
		/// <param name="mtEngine">движок машинного перевода</param>
		public TPage SetMTEngine(MachineTranslationType mtEngine)
		{
			ClickMTDropdown();
			SelectMachineTranslationEngineInList(mtEngine);

			return LoadPage();
		}

		/// <summary>
		/// Настроить перевод
		/// </summary>
		/// <param name="sourceLanguage">исходный язык</param>
		/// <param name="targetLanguage">язык перевода</param>
		/// <param name="mtEngine">движок МТ-перевода</param>
		public TPage SetTranslationSettings(
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			MachineTranslationType mtEngine = MachineTranslationType.SmartCATTranslator)
		{
			SelectSourceLanguageInList(sourceLanguage);
			SelectTargetLangiageInList(targetLanguage);
			SelectMachineTranslationEngineInList(mtEngine);

			return LoadPage();
		}
		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SOURCE_LANG_DROPDOWN)]
		protected IWebElement SourceLangDropdown { get; set; }

		protected IWebElement SourceLangItem { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_LANG_DROPDOWN)]
		protected IWebElement TargetLangDropdown { get; set; }

		protected IWebElement TargetLangItem { get; set; }

		[FindsBy(How = How.XPath, Using = MT_DROPDOWN)]
		protected IWebElement MachineTranslationDropdown { get; set; }

		protected IWebElement MTEngineItem { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string SOURCE_LANG_DROPDOWN = "//div[@class='source_lang']//input";
		protected const string SOURCE_LANG_ITEM = "//div[@class='source_lang']//li[@title = '*#*']";
		protected const string TARGET_LANG_DROPDOWN = "//div[contains(@class,'target_langs')]//input";
		protected const string TARGET_LANG_ITEM = "//div[contains(@class,'target_langs')]//li[@title = '*#*']";
		protected const string MT_DROPDOWN = "//div[contains(@data-bind,'mtEngine')]//input";
		protected const string MT_ITEM = "//div[contains(@data-bind,'mtEngine')]//li[@title = '*#*']";

		#endregion
	}

}
