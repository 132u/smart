using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class SuggestedTermsPageForAllGlossaries : GlossaryPage, IAbstractPage<SuggestedTermsPageForAllGlossaries>
	{
		public SuggestedTermsPageForAllGlossaries(WebDriver driver) : base(driver)
		{
		}

		public new SuggestedTermsPageForAllGlossaries GetPage()
		{
			var suggestTermsPage = new SuggestedTermsPageForAllGlossaries(Driver);
			InitPage(suggestTermsPage, Driver);

			return suggestTermsPage;
		}

		public new void LoadPage()
		{
			if (!IsSuggestedTermsPageForAllGlossariesOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась общая страница Suggested Terms");
			}
		}

		#region Простые методы

		/// <summary>
		/// Получить количество терминов для глоссария
		/// </summary>
		/// <param name="glossary">название глоссария</param>
		public int GetTermsByGlossaryNameCount(string glossary = "")
		{
			CustomTestContext.WriteLine("Получить количество терминов для глоссария {0}.", glossary);
			var allGlossaries = Driver.GetTextListElement(By.XPath(GLOSSARIES_COLUMN_LIST));

			return allGlossaries.Count(glName => glName.Trim() == glossary);
		}

		/// <summary>
		/// Навести курсор на строку термина
		/// </summary>
		public SuggestedTermsPageForAllGlossaries HoverSuggestedTermRow(int rowNumber)
		{
			CustomTestContext.WriteLine("Навести курсор на строку термина №{0}.", rowNumber);
			var term = Driver.SetDynamicValue(How.XPath, SUGGESTED_TERM_ROW, rowNumber.ToString());

			term.Click();
			term.HoverElement();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Accept suggest'
		/// </summary>
		public SelectGlossaryDialog ClickAcceptSuggestButtonExpectingSelectGlossaryDialog(int termNumber)
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Accept suggest' термина №{0}.", termNumber);
			Driver.SetDynamicValue(How.XPath, ACCEPT_SUGGEST_BUTTON, termNumber.ToString()).JavaScriptClick();

			return new SelectGlossaryDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Accept suggest'
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickAcceptSuggestButton(int termNumber)
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Accept suggest' термина №{0}.", termNumber);
			Driver.SetDynamicValue(How.XPath, ACCEPT_SUGGEST_BUTTON, termNumber.ToString()).JavaScriptClick();

			return GetPage();
		}

		/// <summary>
		/// Редактировать предложенный термин
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		/// <param name="termValue">значение термина</param>
		public SuggestedTermsPageForAllGlossaries FillSuggestedTermInEditMode(int termNumber, string termValue)
		{
			CustomTestContext.WriteLine("Ввести {0} в термин №{1}.", termValue, termNumber);
			var term = Driver.SetDynamicValue(How.XPath, TERM_IN_EDIT_MODE, termNumber.ToString());
			var termInput = Driver.SetDynamicValue(How.XPath, EDIT_TERM_INPUT, termNumber.ToString());

			term.Click();
			termInput.SetText(termValue);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления предложенного термина
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickDeleteSuggestTermButton(int rowNumber)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления предложенного термина №{0}.", rowNumber);
			Driver.SetDynamicValue(How.XPath, DELETE_SUGGEST_TERM_BUTTON, rowNumber.ToString()).DoubleClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования предложенного термина
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public SelectGlossaryDialog ClickEditSuggestTermButtonExpectingSelectGlossaryDialog(int rowNumber)
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования предложенного термина №{0}.", rowNumber);
			EditSuggestTermButton = Driver.SetDynamicValue(How.XPath, EDIT_SUGGEST_TERM_BUTTON, rowNumber.ToString());
			EditSuggestTermButton.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(ACCEPT_TERM_BUTTON_IN_EDIT_MODE));

			return new SelectGlossaryDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования предложенного термина
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		public SuggestedTermsPageForAllGlossaries ClickEditSuggestTermButton(int rowNumber)
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования предложенного термина №{0}.", rowNumber);
			EditSuggestTermButton = Driver.SetDynamicValue(How.XPath, EDIT_SUGGEST_TERM_BUTTON, rowNumber.ToString());
			EditSuggestTermButton.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(ACCEPT_TERM_BUTTON_IN_EDIT_MODE));

			return GetPage();
		}

		/// <summary>
		/// Нажать на выпадающий список глоссариев
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickGlossariesDropdown()
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список глоссариев.");
			GlossariesDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать глоссарий в дропдауне
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		public SuggestedTermsPageForAllGlossaries SelectGlossariesInDropdown(string glossaryName)
		{
			CustomTestContext.WriteLine("Выбрать глоссарий в дропдауне.");
			Driver.SetDynamicValue(How.XPath, GLOSSARY_IN_DROPDOWN, glossaryName).Click();

			return GetPage();
		}

		/// <summary>
		/// Получить количество предложенных терминов
		/// </summary>
		public int SuggestedTermsCount()
		{
			CustomTestContext.WriteLine("Получить количество предложенных терминов.");

			return Driver.GetElementsCount(By.XPath(SUGGESTED_TERMS_ROW_LIST));
		}

		/// <summary>
		/// Получить номер строки термина по названию глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		public int GetTermRowNumberByGlossaryName(string glossaryName = "")
		{
			CustomTestContext.WriteLine(string.Format("Получить номер строки термина по названию глоссария {0}.", glossaryName));
			var glossaryNameList = Driver.GetTextListElement(By.XPath(ROW_GLOSSARY_NAME));
			
			return glossaryNameList.Select(g => g.Trim()).ToList().IndexOf(glossaryName) + 1;
		}

		/// <summary>
		/// Нажать на кнопку 'Accept term' в режиме редактирования предложенного термина
		/// </summary>
		public SuggestedTermsPageForAllGlossaries ClickAcceptTermButtonInEditMode()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Accept term' в режиме редактирования предложенного термина.");
			AcceptTermButtonInEditMode.Click();
			Driver.WaitUntilElementIsDisappeared(By.XPath(ACCEPT_TERM_BUTTON_IN_EDIT_MODE));

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку добавления синонима в термин
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		public SuggestedTermsPageForAllGlossaries ClickAddSynonymButton(int termNumber)
		{
			CustomTestContext.WriteLine("Нажать кнопку добавления синонима в термин.");
			Driver.SetDynamicValue(How.XPath, ADD_SYNONYM_BUTTON, termNumber.ToString()).Click();

			return GetPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Редактировать предложенный термин
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		/// <param name="termValue">термин</param>
		/// <param name="chooseGlossary">выбор глоссария</param>
		/// <param name="glossaryToChoose">выбранный глоссарий</param>
		public SelectGlossaryDialog EditSuggestTermInSuggestedTermsPageForAllGlossaries(
			string glossaryName = "")
		{
			var termRowNumber = GetTermRowNumberByGlossaryName(glossaryName);

			HoverSuggestedTermRow(termRowNumber);
			var selectGlossaryDialog = ClickEditSuggestTermButtonExpectingSelectGlossaryDialog(termRowNumber);

			return selectGlossaryDialog;
		}

		/// <summary>
		/// Принять предложенный термин
		/// </summary>
		/// <param name="chooseGlossary">выбор глоссария</param>
		/// <param name="glossaryName">имя глоссария</param>
		public SelectGlossaryDialog AcceptSuggestTermExpectingSelectGlossaryDialog(
			string glossaryName = "")
		{
			var termRowNumber = GetTermRowNumberByGlossaryName(glossaryName);

			HoverSuggestedTermRow(termRowNumber);
			var selectGlossaryDialog = ClickAcceptSuggestButtonExpectingSelectGlossaryDialog(termRowNumber);

			return selectGlossaryDialog.GetPage();
		}

		/// <summary>
		/// Принять предложенный термин
		/// </summary>
		/// <param name="chooseGlossary">выбор глоссария</param>
		/// <param name="glossaryName">имя глоссария</param>
		public SuggestedTermsPageForAllGlossaries AcceptSuggestTermInSuggestedTermsPageForAllGlossaries(
			string glossaryName = "")
		{
			var termRowNumber = GetTermRowNumberByGlossaryName(glossaryName);

			HoverSuggestedTermRow(termRowNumber);
			ClickAcceptSuggestButton(termRowNumber);

			return GetPage();
		}

		/// <summary>
		/// Удалить предложенный термин
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public SuggestedTermsPageForAllGlossaries DeleteSuggestTermInSuggestedTermsPageForAllGlossaries(
			string glossaryName = "")
		{
			var termRowNumber = GetTermRowNumberByGlossaryName(glossaryName);
			var termsCountBeforeDelete = SuggestedTermsCount();

			HoverSuggestedTermRow(termRowNumber);
			ClickDeleteSuggestTermButton(termRowNumber);

			// Sleep не убирать, иначе термин не исчезнет
			Thread.Sleep(1000);

			var termsCountAfterDelete = SuggestedTermsCount();

			if (termsCountBeforeDelete - termsCountAfterDelete != 1)
			{
				throw new InvalidElementStateException("Произошла ошибка:\nПодтвержденный предложенный термин не исчез из списка.");
			}

			return GetPage();
		}

		/// <summary>
		/// Удалить все предложенные термины
		/// </summary>
		public SuggestedTermsPageForAllGlossaries DeleteAllSuggestTerms()
		{
			var termsCount = SuggestedTermsCount();

			while (termsCount != 0)
			{
				HoverSuggestedTermRow(1);
				ClickDeleteSuggestTermButton(1);
				// Sleep не убирать, иначе термин не исчезнет
				Thread.Sleep(1000);
				termsCount = SuggestedTermsCount();
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, загрузилась ли страница Seggested Terms
		/// </summary>
		public bool IsSuggestedTermsPageForAllGlossariesOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(SUGGEST_TERMS_TABLE));
		}

		/// <summary>
		/// Проверить, что строка с термином исчезла.
		/// </summary>
		/// <param name="rowNumber">номер термин</param>
		public bool IsSuggestedTermRowDisappeared(int rowNumber)
		{
			CustomTestContext.WriteLine("Проверить, что строка с термином № исчезла.", rowNumber);

			return Driver.WaitUntilElementIsDisappeared(By.XPath(SUGGESTED_TERM_ROW.Replace("*#*", rowNumber.ToString())));
		}

		/// <summary>
		/// Проверить, что термин исчез.
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		public bool IsAcceptSuggestButtonDisappeared(string glossaryName)
		{
			CustomTestContext.WriteLine("Проверить, что термин глоссария {0} исчез.", glossaryName);

			return Driver.WaitUntilElementIsDisappeared(By.XPath(ACCEPT_SUGGEST_BUTTON_BY_GLOSSARY_NAME.Replace("*#*", glossaryName)));
		}

		#endregion

		#region Объявление элементов страниц

		[FindsBy(How = How.XPath, Using = ALL_GLOSSARIES_BUTTON)]
		protected IWebElement AllGlossariesButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = GLOSSARIES_DROPDOWN)]
		protected IWebElement GlossariesDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = ACCEPT_TERM_BUTTON_IN_EDIT_MODE)]
		protected IWebElement AcceptTermButtonInEditMode { get; set; }

		protected IWebElement EditSuggestTermButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string GLOSSARY_IN_DROPDOWN = "//span[contains(@class,'js-dropdown__item')][contains(text(),'*#*')]";
		protected const string GLOSSARIES_DROPDOWN = "//span[contains(@class,'js-dropdown')]";

		protected const string ROW_GLOSSARY_NAME = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]//td[contains(@class, 'js-glossary-cell')]//p";
		protected const string ALL_GLOSSARIES_BUTTON = "//div[contains(@class, 'js-body')]//a[contains(@href, 'Glossaries')]";
		protected const string SUGGEST_TERMS_TABLE = "//table[contains(@class,'js-suggests')]";
		protected const string GLOSSARIES_COLUMN_LIST = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]//td[contains(@class, 'js-glossary-cell')]//p";
		protected const string SUGGESTED_TERM_ROW = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td";
		protected const string ACCEPT_SUGGEST_BUTTON = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td//i[contains(@class, 'accept-suggest')]";
		protected const string SUGGESTED_TERMS_ROW_LIST = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]";
		protected const string ACCEPT_SUGGEST_BUTTON_BY_GLOSSARY_NAME = "//p[contains(text(), '*#*')]/../following-sibling::td[contains(@class, 'suggestComment')]//i[contains(@class, 'js-accept-suggest')]";
		protected const string DELETE_SUGGEST_TERM_BUTTON = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td//i[contains(@class, 'reject-suggest')]";
		protected const string EDIT_SUGGEST_TERM_BUTTON = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td//i[contains(@class, 'edit-suggest')]";
		protected const string EDIT_TERM_INPUT = "//div[contains(@class,'l-corprtree__langbox')][*#*]//span[contains(@class,'js-term-editor')]//input";
		protected const string ACCEPT_TERM_BUTTON_IN_EDIT_MODE = "//span[contains(@class, 'js-save-text')]";
		protected const string TERM_IN_EDIT_MODE = "//div[@class='l-corprtree__langbox'][*#*]//div[2]";
		protected const string ADD_SYNONYM_BUTTON = "//div[contains(@class,'l-corprtree__langbox')][*#*]//i[contains(@class,'js-add-term')]";

		#endregion
	}
}
