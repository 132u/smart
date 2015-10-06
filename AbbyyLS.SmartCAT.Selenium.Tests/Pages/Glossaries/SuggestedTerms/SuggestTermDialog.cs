using System;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class SuggestTermDialog : WorkspacePage, IAbstractPage<SuggestTermDialog>
	{
		public SuggestTermDialog(WebDriver driver) : base(driver)
		{
		}

		public new SuggestTermDialog GetPage()
		{
			var suggestTermDialog = new SuggestTermDialog(Driver);
			InitPage(suggestTermDialog, Driver);

			return suggestTermDialog;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SUGGEST_TERM_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\nНе открылся диалог создания терминов.");
			}
		}

		/// <summary>
		/// Заполнить название термина
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		/// <param name="term">термин</param>
		public SuggestTermDialog FillTerm(int termNumber, string term)
		{
			CustomTestContext.WriteLine("Ввести {0} в термин №{1}.", term, termNumber);
			Driver.SetDynamicValue(How.XPath, TERM_INPUT, termNumber.ToString()).SetText(term);

			return GetPage();
		}

		/// <summary>
		/// Нажать на выпадающий список глоссариев
		/// </summary>
		public SuggestTermDialog ClickGlossariesDropdown()
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список глоссариев.");
			GlossaryDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать глоссарий в дропдауне
		/// </summary>
		/// <param name="glossary">название глоссария</param>
		public SuggestTermDialog SelectGlossariesInDropdown(string glossary)
		{
			CustomTestContext.WriteLine("Выбрать глоссарий {0} в дропдауне.", glossary);
			Driver.SetDynamicValue(How.XPath, GLOSSARU_IN_LIST, glossary).Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Save
		/// </summary>
		public T ClickSaveButton<T>(WebDriver driver) where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать кнопку Save.");
			SaveButon.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Нажать на выпадающий список языков
		/// </summary>
		/// <param name="languageNumber">номер языка</param>
		public SuggestTermDialog ClickLanguageList(int languageNumber)
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список языков №{0}.", languageNumber);
			Driver.SetDynamicValue(How.XPath, LANGUAGE_LIST, languageNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Вернуть название языка, установленного для термина
		/// </summary>
		public string LanguageText(int languageNumber)
		{
			CustomTestContext.WriteLine("Вернуть название языка, установленного для термина №{0}.");

			return Driver.SetDynamicValue(How.XPath, LANGUAGE_LIST, languageNumber.ToString()).Text.Trim();
		}

		/// <summary>
		/// Выбрать язык
		/// </summary>
		public SuggestTermDialog SelectLanguageInList(Language language)
		{
			CustomTestContext.WriteLine("Выбрать язык {0}.", language);
			Driver.SetDynamicValue(How.XPath, LANGUAGE_OPTION, language.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение о том, что такой термин уже существует, появилось
		/// </summary>
		public SuggestTermDialog AssertDublicateErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение о том, что такой термин уже существует, появилось.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(DUPLICATE_ERROR)),
				"Произошла ошибка:\n сообщение о том, что такой термин уже существует, не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение 'Enter at least one term.' появилось
		/// </summary>
		public SuggestTermDialog AssertEmptyTermErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'Enter at least one term.' появилось.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(EMPTY_TERM_ERROR_MESSAGE)),
				"Произошла ошибка:\nCообщение 'Enter at least one term.' не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel
		/// </summary>
		public T ClickCancelButton<T>(WebDriver driver) where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel.");
			CancelButon.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку 'Save term anyway'
		/// </summary>
		public T ClickSaveTermAnywayButton<T>(WebDriver driver) where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Save term anyway'.");
			SaveTermAnywayButon.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}

		[FindsBy(How = How.XPath, Using = GLOSSARY_DROPDOWN)]
		protected IWebElement GlossaryDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTON)]
		protected IWebElement SaveButon { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButon { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_TERM_ANYWAY_BUTON)]
		protected IWebElement SaveTermAnywayButon { get; set; }

		protected const string SUGGEST_TERM_DIALOG = "//div[contains(@class,'js-add-suggest-popup')]";
		protected const string TERM_INPUT = "//div[contains(@class, 'l-addsugg__contr lang js-language')][*#*]//input[contains(@class, 'js-addsugg-term')]";
		protected const string GLOSSARY_DROPDOWN = "//span[contains(@class, 'js-dropdown addsuggglos')]";
		protected const string GLOSSARU_IN_LIST = "//span[contains(@class,'js-dropdown__item')][@title='*#*']";
		protected const string SAVE_BUTON = "//input[contains(@class, 'js-save-btn')]";
		protected const string LANGUAGE_LIST = "//div[contains(@class, 'l-addsugg__contr lang js-language')][*#*]//span[contains(@class,'js-dropdown__text addsugglang')]";
		protected const string LANGUAGE_OPTION = "//span[contains(@class,'js-dropdown__item')][@title='*#*']";
		protected const string CANCEL_BUTTON = "//div[contains(@class,'js-add-suggest-popup')]//a[contains(@class,'g-popupbox__cancel js-popup-close')]";
		protected const string DUPLICATE_ERROR = "//div[contains(@class,'js-duplicate-warning')]";
		protected const string SAVE_TERM_ANYWAY_BUTON = "//input[contains(@value, 'anyway')]";
		protected const string EMPTY_TERM_ERROR_MESSAGE = "//div[contains(@class,'js-add-suggest-popup')]//div[contains(@class,'js-error-message')]";
	}
}
