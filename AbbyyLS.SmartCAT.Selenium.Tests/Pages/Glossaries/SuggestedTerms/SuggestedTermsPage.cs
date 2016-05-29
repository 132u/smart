using System.Threading;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms
{
	public class SuggestedTermsPage : WorkspacePage, IAbstractPage<SuggestedTermsPage>
	{
		public SuggestedTermsPage(WebDriver driver) : base(driver)
		{
		}

		public new SuggestedTermsPage LoadPage()
		{
			if (!IsSuggestedTermsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница Suggested Terms");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Навести курсор на строку термина
		/// </summary>
		/// <param name="term1">термин на одном языке</param>
		/// <param name="term2">термин на другом языке</param>
		public SuggestedTermsPage HoverSuggestedTermRow(string term1, string term2)
		{
			CustomTestContext.WriteLine("Навести курсор на строку термина {0}-{1}", term1, term2);
			SuggestedTerm = Driver.SetDynamicValue(How.XPath, TERM, term1, term2);

			SuggestedTerm.Click();
			SuggestedTerm.HoverElement();

			return LoadPage();
		}

		/// <summary>
		/// Редактировать предложенный термин
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		/// <param name="termValue">значение термина</param>
		public SuggestedTermsPage FillSuggestedTermInEditMode(int termNumber, string termValue)
		{
			CustomTestContext.WriteLine("Ввести {0} в термин №{1}.", termValue, termNumber);
			var term = Driver.SetDynamicValue(How.XPath, TERM_IN_EDIT_MODE, termNumber.ToString());
			var termInput = Driver.SetDynamicValue(How.XPath, EDIT_TERM_INPUT, termNumber.ToString());

			term.Click();
			termInput.SetText(termValue);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования предложенного термина
		/// </summary>
		/// <param name="term1">термин на одном языке</param>
		/// <param name="term2">термин на другом языке</param>
		public SuggestedTermsPage ClickEditSuggestedTermButton(string term1, string term2)
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования предложенного термина {0}-{1}", term1, term2);
			EditSuggestTermButton = Driver.SetDynamicValue(How.XPath, EDIT_SUGGEST_TERM_BUTTON, term1, term2);
			EditSuggestTermButton.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(ACCEPT_TERM_BUTTON_IN_EDIT_MODE));

			return LoadPage();
		}

		/// <summary>
		/// Нажать на выпадающий список глоссариев
		/// </summary>
		public SuggestedTermsPage ClickGlossariesDropdown()
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список глоссариев.");
			GlossariesDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать глоссарий в дропдауне
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		public SuggestedTermsPage SelectGlossariesInDropdown(string glossaryName)
		{
			CustomTestContext.WriteLine("Выбрать глоссарий в дропдауне.");
			Driver.SetDynamicValue(How.XPath, GLOSSARY_IN_DROPDOWN, glossaryName).Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Accept term' в режиме редактирования предложенного термина
		/// </summary>
		public SuggestedTermsPage ClickAcceptTermButtonInEditMode()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Accept term' в режиме редактирования предложенного термина.");
			AcceptTermButtonInEditMode.Click();
			Driver.WaitUntilElementIsDisappeared(By.XPath(ACCEPT_TERM_BUTTON_IN_EDIT_MODE));

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку добавления синонима в термин
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		public SuggestedTermsPage ClickAddSynonymButton(int termNumber)
		{
			CustomTestContext.WriteLine("Нажать кнопку добавления синонима в термин.");
			Driver.SetDynamicValue(How.XPath, ADD_SYNONYM_BUTTON, termNumber.ToString()).Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить автора предложенного термина
		/// </summary>
		public string GetSuggestedTermAuthor(string term1, string term2)
		{
			CustomTestContext.WriteLine("Получить автора предложенного термина {0}, {1}",
				term1, term2);

			SuggestedTermAuthor = Driver.SetDynamicValue(How.XPath, TERM_AUTHOR, term1, term2);
			return SuggestedTermAuthor.Text;
		}

		/// <summary>
		/// Получить дату предложенного термина
		/// </summary>
		public string GetSuggestedTermDate(string term1, string term2)
		{
			CustomTestContext.WriteLine("Получить дату предложенного термина {0}, {1}",
				term1, term2);

			SuggestedTermDate = Driver.SetDynamicValue(How.XPath, TERM_DATE, term1, term2);
			return SuggestedTermDate.Text;
		}
		#endregion

		#region Составные методы

		/// <summary>
		/// Навести курсор на строку с терминами и нажать кнопку 'Accept suggest' 
		/// ожидая диалог выбора глоссария
		/// </summary>
		/// <param name="term1">термин на одном языке</param>
		/// <param name="term2">термин на другом языке</param>
		public SelectGlossaryDialog AcceptSuggestedTermExpectingSelectGlossaryDialog(string term1, string term2)
		{
			HoverSuggestedTermRow(term1, term2);

			CustomTestContext.WriteLine("Нажать кнопку 'Accept suggest' термина {0}-{1}", term1, term2);
			Driver.SetDynamicValue(How.XPath, ACCEPT_SUGGEST_BUTTON, term1, term2).JavaScriptClick();

			return new SelectGlossaryDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Навести курсор на строку с терминами и нажать кнопку 'Accept suggest'
		/// </summary>
		/// <param name="term1">термин на одном языке</param>
		/// <param name="term2">термин на другом языке</param>
		public SuggestedTermsPage AcceptSuggestedTerm(string term1, string term2)
		{
			HoverSuggestedTermRow(term1, term2);

			CustomTestContext.WriteLine("Нажать кнопку 'Accept suggest' термина {0}-{1}", term1, term2);
			Driver.SetDynamicValue(How.XPath, ACCEPT_SUGGEST_BUTTON, term1, term2).JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Навести курсор на строку с терминами и нажать кнопку редактирования
		/// ожидая диалог выбора глоссария
		/// </summary>
		/// <param name="term1">термин на одном языке</param>
		/// <param name="term2">термин на другом языке</param>
		public SelectGlossaryDialog EditSuggestedTermExpectingSelectGlossaryDialog(string term1, string term2)
		{
			HoverSuggestedTermRow(term1, term2);

			CustomTestContext.WriteLine("Нажать кнопку редактирования предложенного термина {0}-{1}", term1, term2);
			EditSuggestTermButton = Driver.SetDynamicValue(How.XPath, EDIT_SUGGEST_TERM_BUTTON, term1, term2);
			EditSuggestTermButton.Click();

			return new SelectGlossaryDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Навести курсор на строку с терминами и нажать кнопку удаления
		/// </summary>
		/// <param name="term1">термин на одном языке</param>
		/// <param name="term2">термин на другом языке</param>
		public SuggestedTermsPage DeleteSuggestedTerm(string term1, string term2)
		{
			HoverSuggestedTermRow(term1, term2);

			CustomTestContext.WriteLine("Нажать кнопку удаления предложенного термина {0}-{1}", term1, term2);
			Driver.SetDynamicValue(How.XPath, DELETE_SUGGEST_TERM_BUTTON, term1, term2).DoubleClick();

			// Sleep не убирать, иначе термин не исчезнет
			Thread.Sleep(1000);

			return LoadPage();
		}

		/// <summary>
		/// Редактировать предложенный термин
		/// </summary>
		/// <param name="term1">термин на одном языке</param>
		/// <param name="term2">термин на другом языке</param>
		/// <param name="newTermValue1">новый термин на одном языке</param>
		/// <param name="newTermValue2">новый термин на другом языке</param>
		public SuggestedTermsPage EditSuggestedTerm(
			string term1, string term2, string newTermValue1, string newTermValue2)
		{
			HoverSuggestedTermRow(term1, term2);
			ClickEditSuggestedTermButton(term1, term2);
			FillSuggestedTermInEditMode(termNumber: 1, termValue: newTermValue1);
			FillSuggestedTermInEditMode(termNumber: 2, termValue: newTermValue2);
			ClickAcceptTermButtonInEditMode();

			return LoadPage();
		}

		/// <summary>
		/// Добавить синоним для предложенного термина в текущем глоссарии
		/// </summary>
		/// <param name="term1">термин на одном языке</param>
		/// <param name="term2">термин на другом языке</param>
		/// <param name="addButtonNumber">номер кнопки add</param>
		/// <param name="synonymValue">синоним</param>
		public SuggestedTermsPage AddSynonim(
			string term1, string term2, int addButtonNumber, string synonymValue)
		{
			HoverSuggestedTermRow(term1, term2);
			ClickEditSuggestedTermButton(term1, term2);
			ClickAddSynonymButton(addButtonNumber);
			FillSuggestedTermInEditMode(addButtonNumber, synonymValue);
			ClickAcceptTermButtonInEditMode();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать глоссарий в дропдауне
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public SuggestedTermsPage SelectGlossary(string glossaryName)
		{
			ClickGlossariesDropdown();
			SelectGlossariesInDropdown(glossaryName);

			return LoadPage();
		}
		
		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, присутствует ли  предложенный термин в списке
		/// </summary>
		/// <param name="term1">термин в одном языке</param>
		/// /// <param name="term2">термин на другом языке</param>
		public bool IsSuggestedTermDisplayed(string term1, string term2)
		{
			CustomTestContext.WriteLine("Проверить, что в списке есть строка с терминами {0}, {1}",
				term1, term2);

			return Driver.WaitUntilElementIsDisplay(
				By.XPath(TERM.Replace("*#*", term1).Replace("*##*", term2)), 3);
		}

		/// <summary>
		/// Проверить, загрузилась ли страница Seggested Terms
		/// </summary>
		public bool IsSuggestedTermsPageOpened()
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

		protected IWebElement SuggestedTerm { get; set; }
		
		protected IWebElement SuggestedTermAuthor { get; set; }
		
		protected IWebElement SuggestedTermDate { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string GLOSSARY_IN_DROPDOWN = "//span[contains(@class,'js-dropdown__item')][contains(text(),'*#*')]";
		protected const string GLOSSARIES_DROPDOWN = "//span[contains(@class,'js-dropdown')]";

		protected const string ROW_GLOSSARY_NAME = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]//td[contains(@class, 'js-glossary-cell')]//p";
		protected const string ALL_GLOSSARIES_BUTTON = "//div[contains(@class, 'js-body')]//a[contains(@href, 'Glossaries')]";
		protected const string SUGGEST_TERMS_TABLE = "//table[contains(@class,'js-suggests')]";
		protected const string GLOSSARIES_COLUMN_LIST = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]//td[contains(@class, 'js-glossary-cell')]//p";
		protected const string SUGGESTED_TERM_ROW = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td";
		protected const string ACCEPT_SUGGEST_BUTTON = "//tr[contains(@class, 'l-corpr__trhover js-suggest-row') and contains(string(), '*#*') and contains(string(), '*##*')]//i[contains(@class, 'js-accept-suggest')]";
		protected const string SUGGESTED_TERMS_ROW_LIST = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]";
		protected const string ACCEPT_SUGGEST_BUTTON_BY_GLOSSARY_NAME = "//p[contains(text(), '*#*')]/../following-sibling::td[contains(@class, 'suggestComment')]//i[contains(@class, 'js-accept-suggest')]";
		protected const string DELETE_SUGGEST_TERM_BUTTON = "//tr[contains(@class, 'l-corpr__trhover js-suggest-row') and contains(string(), '*#*') and contains(string(), '*##*')]//i[contains(@class, 'reject-suggest')]";
		protected const string EDIT_SUGGEST_TERM_BUTTON = "//tr[contains(@class, 'l-corpr__trhover js-suggest-row') and contains(string(), '*#*') and contains(string(), '*##*')]//i[contains(@class, 'edit-suggest')]";
		protected const string EDIT_TERM_INPUT = "//div[contains(@class,'l-corprtree__langbox')][*#*]//span[contains(@class,'js-term-editor')]//input";
		protected const string ACCEPT_TERM_BUTTON_IN_EDIT_MODE = "//span[contains(@class, 'js-save-text')]";
		protected const string TERM_IN_EDIT_MODE = "//div[@class='l-corprtree__langbox'][*#*]//div[2]";
		protected const string ADD_SYNONYM_BUTTON = "//div[contains(@class,'l-corprtree__langbox')][*#*]//i[contains(@class,'js-add-term')]";
		protected const string TERM = "//tr[contains(@class, 'l-corpr__trhover js-suggest-row') and contains(string(), '*#*') and contains(string(), '*##*')]";
		protected const string TERM_AUTHOR = "//tr[contains(@class, 'l-corpr__trhover js-suggest-row') and contains(string(), '*#*') and contains(string(), '*##*')]//td[5]/p";
		protected const string TERM_DATE = "//tr[contains(@class, 'l-corpr__trhover js-suggest-row') and contains(string(), '*#*') and contains(string(), '*##*')]//td[6]/p";

		#endregion
	}
}
