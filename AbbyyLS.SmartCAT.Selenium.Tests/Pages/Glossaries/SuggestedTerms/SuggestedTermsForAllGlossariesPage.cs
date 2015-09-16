using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class SuggestedTermsPageForAllGlossaries : GlossaryPage, IAbstractPage<SuggestedTermsPageForAllGlossaries>
	{
		public new SuggestedTermsPageForAllGlossaries GetPage()
		{
			var suggestTermsPage = new SuggestedTermsPageForAllGlossaries();
			InitPage(suggestTermsPage);

			return suggestTermsPage;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SUGGEST_TERMS_TABLE)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась общая страница Suggested Terms.");
			}
		}
		
		/// <summary>
		/// Получить количество терминов для глоссария
		/// </summary>
		/// <param name="glossary">название глоссария</param>
		public int TermsByGlossaryNameCount(string glossary)
		{
			Logger.Debug("Получить количество терминов для глоссария {0}.", glossary);
			var allGlossaries = Driver.GetTextListElement(By.XPath(GLOSSARIES_COLUMN_LIST));

			return allGlossaries.Count(glName => glName.Trim() == glossary);
		}

		/// <summary>
		/// Навести курсор на строку термина
		/// </summary>
		public SuggestedTermsPageForAllGlossaries HoverSuggestedTermRow(int rowNumber)
		{
			Logger.Debug("Навести курсор на строку термина №{0}.", rowNumber);
			var term = Driver.SetDynamicValue(How.XPath, SUGGESTED_TERM_ROW, rowNumber.ToString());

			term.Click();
			term.HoverElement();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Accept suggest'
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickAcceptSuggestButton(int termNumber)
		{
			Logger.Debug("Нажать кнопку 'Accept suggest' термина №{0}.", termNumber);
			Driver.SetDynamicValue(How.XPath, ACCEPT_SUGGEST_BUTTON, termNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Редактировать предложенный термин
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		/// <param name="termValue">значение термина</param>
		public SuggestedTermsPageForAllGlossaries FillSuggestedTermInEditMode(int termNumber, string termValue)
		{
			Logger.Debug("Ввести {0} в термин №{1}.", termValue, termNumber);
			var term = Driver.SetDynamicValue(How.XPath, TERM_IN_EDIT_MODE, termNumber.ToString());
			var termInput = Driver.SetDynamicValue(How.XPath, EDIT_TERM_INPUT, termNumber.ToString());

			term.Click();
			termInput.SetText(termValue);

			return GetPage();
		}

		/// <summary>
		/// Проверить,что форма редактирования предложенного термина открылась
		/// </summary>
		public SuggestedTermsPageForAllGlossaries AssertEditFormDisplayed()
		{
			Logger.Debug("Проверить,что форма редактирования предложенного термина открылась.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ACCEPT_TERM_BUTTON_IN_EDIT_MODE)),
				"Произошла ошибка:\nФорма редактирования предложенного термина не открылась.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что форма редактирования предложенного термина закрылась
		/// </summary>
		public SuggestedTermsPageForAllGlossaries AssertEditFormDisappeared()
		{
			Logger.Debug("Проверить, что форма редактирования предложенного термина закрылась.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(ACCEPT_TERM_BUTTON_IN_EDIT_MODE)),
				"Произошла ошибка:\nФорма редактирования предложенного термина не закрылась.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления предложенного термина
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickDeleteSuggestTermButton(int rowNumber)
		{
			Logger.Debug("Нажать кнопку удаления предложенного термина №{0}.", rowNumber);
			Driver.SetDynamicValue(How.XPath, DELETE_SUGGEST_TERM_BUTTON, rowNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования предложенного термина
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickEditSuggestTermButton(int rowNumber)
		{
			Logger.Debug("Нажать кнопку редактирования предложенного термина №{0}.", rowNumber);
			Driver.SetDynamicValue(How.XPath, EDIT_SUGGEST_TERM_BUTTON, rowNumber.ToString()).Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на выпадающий список глоссариев
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickGlossariesDropdown()
		{
			Logger.Debug("Нажать на выпадающий список глоссариев.");
			GlossariesDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать глоссарий в дропдауне
		/// </summary>
		public SuggestedTermsPageForAllGlossaries SelectGlossariesInDropdown(string glossaryName)
		{
			Logger.Debug("Выбрать глоссарий в дропдауне.");
			Driver.SetDynamicValue(How.XPath, GLOSSARY_IN_DROPDOWN, glossaryName).Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог выбора глоссария появился
		/// </summary>
		public SuggestedTermsPageForAllGlossaries AssertSelectGlossaryDialogDisplayed()
		{
			Logger.Trace("Проверить, что диалог выбора глоссария появился");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(SELECT_GLOSSARY_DROPDOWN)),
				"Произошла ошибка:\nДиалог выбора глоссария не появился.");

			return GetPage();
		}

		/// <summary>
		/// Нажать на выпадающий список глоссариев в диалоге выбора глоссария
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickSelectGlossaryDropdownInSelectDialog()
		{
			Logger.Debug("Нажать на выпадающий список глоссариев в диалоге выбора глоссария.");
			SelectGlossaryDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог выбора глоссария появился
		/// </summary>
		public SuggestedTermsPageForAllGlossaries AssertSelectDialogDisplayed()
		{
			Logger.Trace("Проверить, что диалог выбора глоссария появился.");
			
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(SELECT_GLOSSARY_DROPDOWN)),
				"Произошла ошибка:\nДиалог выбора глоссария не появился");

			return GetPage();
		}

		/// <summary>
		/// Выбрать глоссарий
		/// </summary>
		public SuggestedTermsPageForAllGlossaries SelectGlossaryInSelectDialog(string glossaryName)
		{
			Logger.Debug("Выбрать {0} глоссарий.", glossaryName);
			Driver.SetDynamicValue(How.XPath, GLOSSARY_IN_SELECT_GLOSSARY_DIALOG, glossaryName).Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Ok
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickOkButton()
		{
			Logger.Debug("Нажать кнопку Ok.");
			OkButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить количество предложенных терминов
		/// </summary>
		public int SuggestedTermsCount()
		{
			Logger.Trace("Получить количество предложенных терминов.");

			return Driver.GetElementsCount(By.XPath(SUGGESTED_TERMS_ROW_LIST));
		}

		/// <summary>
		/// Получить номер строки термина по названию глоссария
		/// </summary>
		public int TermRowNumberByGlossaryName(string glossaryName)
		{
			Logger.Trace(string.Format("Получить номер строки термина по названию глоссария {0}.", glossaryName));
			var glossaryNameList = Driver.GetTextListElement(By.XPath(ROW_GLOSSARY_NAME));
			
			return glossaryNameList.Select(g => g.Trim()).ToList().IndexOf(glossaryName) + 1;
		}

		/// <summary>
		/// Нажать на кнопку 'Accept term' в режиме редактирования предложенного термина
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickAcceptTermButtonInEditMode()
		{
			Logger.Debug("Нажать на кнопку 'Accept term' в режиме редактирования предложенного термина.");
			AcceptTermButtonInEditMode.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку добавления синонима в термин
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickAddSynonymButton(int termNumber)
		{
			Logger.Debug("Нажать кнопку добавления синонима в термин.");
			Driver.SetDynamicValue(How.XPath, ADD_SYNONYM_BUTTON, termNumber.ToString()).Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = ALL_GLOSSARIES_BUTTON)]
		protected IWebElement AllGlossariesButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = GLOSSARIES_DROPDOWN)]
		protected IWebElement GlossariesDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = OK_BUTTON)]
		protected IWebElement OkButton { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_GLOSSARY_DROPDOWN)]
		protected IWebElement SelectGlossaryDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = ACCEPT_TERM_BUTTON_IN_EDIT_MODE)]
		protected IWebElement AcceptTermButtonInEditMode { get; set; }

		protected const string GLOSSARY_IN_SELECT_GLOSSARY_DIALOG = "//span[contains(@class,'js-dropdown__item')][contains(text(),'*#*')]";
		protected const string SELECT_GLOSSARY_DROPDOWN= "//div[contains(@class,'js-select-glossary-popup')]//span[contains(@class,'js-dropdown')]";
		protected const string OK_BUTTON = "//input[contains(@class, 'js-glossary-selected-button')]";

		protected const string GLOSSARY_IN_DROPDOWN = "//span[contains(@class,'js-dropdown__item')][contains(text(),'*#*')]";
		protected const string GLOSSARIES_DROPDOWN = "//span[contains(@class,'js-dropdown')]";

		protected const string ROW_GLOSSARY_NAME = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]//td[contains(@class, 'js-glossary-cell')]//p";
		protected const string ALL_GLOSSARIES_BUTTON = "//div[contains(@class, 'js-body')]//a[contains(@href, 'Glossaries')]";
		protected const string SUGGEST_TERMS_TABLE = "//table[contains(@class,'js-suggests')]";
		protected const string GLOSSARIES_COLUMN_LIST = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]//td[contains(@class, 'js-glossary-cell')]//p";
		protected const string SUGGESTED_TERM_ROW = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td";
		protected const string ACCEPT_SUGGEST_BUTTON = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td//a[contains(@class, 'accept-suggest')]";
		protected const string SUGGESTED_TERMS_ROW_LIST = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]";
		protected const string DELETE_SUGGEST_TERM_BUTTON = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td//a[contains(@class, 'reject-suggest')]";
		protected const string EDIT_SUGGEST_TERM_BUTTON = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td//a[contains(@class, 'edit-suggest')]";
		protected const string EDIT_TERM_INPUT = "//div[contains(@class,'l-corprtree__langbox')][*#*]//span[contains(@class,'js-term-editor')]//input";
		protected const string ACCEPT_TERM_BUTTON_IN_EDIT_MODE = "//span[@class = 'js-save-text']";
		protected const string TERM_IN_EDIT_MODE = "//div[@class='l-corprtree__langbox'][*#*]//div[2]";
		protected const string ADD_SYNONYM_BUTTON = "//div[contains(@class,'l-corprtree__langbox')][*#*]//span[contains(@class,'js-add-term')]";
	}
}
