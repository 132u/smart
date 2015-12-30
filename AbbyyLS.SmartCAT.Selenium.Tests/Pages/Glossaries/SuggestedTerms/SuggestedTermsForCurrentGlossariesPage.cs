using System.Threading;

using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class SuggestedTermsPageForCurrentGlossaries : SuggestedTermsPageForAllGlossaries, IAbstractPage<SuggestedTermsPageForCurrentGlossaries>
	{
		public SuggestedTermsPageForCurrentGlossaries(WebDriver driver) : base(driver)
		{
		}

		public new SuggestedTermsPageForCurrentGlossaries GetPage()
		{
			var suggestTermsPageForCurrentGlossaries = new SuggestedTermsPageForCurrentGlossaries(Driver);
			InitPage(suggestTermsPageForCurrentGlossaries, Driver);

			return suggestTermsPageForCurrentGlossaries;
		}

		public new void LoadPage()
		{
			if (!IsSuggestedTermsPageForCurrentGlossariesOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница Suggested Terms");
			}
		}

		#region Простые методы

		/// <summary>
		/// Получить количество терминов
		/// </summary>
		public int GetSuggestedTermsCount()
		{
			CustomTestContext.WriteLine("Получить количество терминов.");

			return Driver.GetElementsCount(By.XPath(TERM_ROW));
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Редактировать предложенный термин для текущего глоссария
		/// </summary>
		/// <param name="termRowNumber">номер строки</param>
		/// <param name="termValue">термин</param>
		public SuggestedTermsPageForCurrentGlossaries EditSuggestTermInSuggestedTermsPageForCurrentGlossary(
			int termRowNumber, string termValue)
		{
			HoverSuggestedTermRow(termRowNumber);
			ClickEditSuggestTermButton(termRowNumber);
			FillSuggestedTermInEditMode(termNumber: 1, termValue: termValue);
			FillSuggestedTermInEditMode(termNumber: 2, termValue: termValue);
			ClickAcceptTermButtonInEditMode();

			return GetPage();
		}

		/// <summary>
		/// Добавить синоним для предложенного термина в текущем глоссарии
		/// </summary>
		/// <param name="termRowNumber">номер строки</param>
		/// <param name="addButtonNumber">номер кнопки add</param>
		/// <param name="synonymValue">синоним</param>
		public SuggestedTermsPageForCurrentGlossaries AddSynonimInSuggestedTermsPageForCurrentGlossary(int termRowNumber,
			int addButtonNumber, string synonymValue)
		{
			HoverSuggestedTermRow(termRowNumber);
			ClickEditSuggestTermButton(termRowNumber);
			ClickAddSynonymButton(addButtonNumber);
			FillSuggestedTermInEditMode(addButtonNumber, synonymValue);
			ClickAcceptTermButtonInEditMode();

			return GetPage();
		}

		/// <summary>
		/// Выбрать глоссарий в дропдауне
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public SuggestedTermsPageForCurrentGlossaries SelectGlossaryInSuggestedTermsPageForCurrentGlossary(
			string glossaryName)
		{
			ClickGlossariesDropdown();
			SelectGlossariesInDropdown(glossaryName);

			return GetPage();
		}

		/// <summary>
		/// Подтвердить предложенный термин
		/// </summary>
		/// <param name="termRowNumber">номер строки</param>
		public SuggestedTermsPageForCurrentGlossaries AcceptSuggestTermInSuggestedTermsPageForCurrentGlossary(int termRowNumber)
		{
			var termsCountBeforeAccept = GetSuggestedTermsCount();

			HoverSuggestedTermRow(termRowNumber);
			ClickAcceptSuggestButton(termRowNumber);

			// Sleep не убирать, иначе термин не исчезнет
			Thread.Sleep(3000);

			var termsCountAfterAccept = GetSuggestedTermsCount();

			if (termsCountBeforeAccept - termsCountAfterAccept != 1)
			{
				throw new InvalidElementStateException(
					"Произошла ошибка:\nПодтвержденный предложенный термин не исчез из списка.");
			}

			return GetPage();
		}

		/// <summary>
		/// Удалить предложенный термин
		/// </summary>
		/// <param name="termRowNumber">номер строки</param>
		public SuggestedTermsPageForCurrentGlossaries DeleteSuggestTermInSuggestedTermsPageForCurrentGlossary(int termRowNumber)
		{
			var termsCountBeforeDelete = GetSuggestedTermsCount();

			HoverSuggestedTermRow(termRowNumber);
			ClickDeleteSuggestTermButton(termRowNumber);

			// Sleep не убирать, иначе термин не исчезнет
			Thread.Sleep(1000);

			var termsCountAfterDelete = GetSuggestedTermsCount();

			if (termsCountBeforeDelete - termsCountAfterDelete != 1)
			{
				throw new InvalidElementStateException(
					"Произошла ошибка:\nПодтвержденный предложенный термин не исчез из списка.");
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница SuggestedTerms
		/// </summary>
		public bool IsSuggestedTermsPageForCurrentGlossariesOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SUGGEST_TERMS_TABLE));
		}

		/// <summary>
		/// Проверить, что в предложенном термине верное значение
		/// </summary>
		/// <param name="expectedTermValue">ожидаемое значение</param>
		/// <param name="rowNumber">номер строки термина</param>
		/// <param name="columnNumber">номер столбца</param>
		public bool IsTermValueMatchExpected(string expectedTermValue, int rowNumber, int columnNumber)
		{
			CustomTestContext.WriteLine("Проверить, что в предложенном термине (строка №{0}, колонка №{1}) верное значение {2}.",
				rowNumber, columnNumber, expectedTermValue);
			TermValue = Driver.FindElement(By.XPath(TERM_VALUE.Replace("*#*", rowNumber.ToString()).Replace("##", columnNumber.ToString())));
			
			return expectedTermValue == TermValue.Text;
		}

		#endregion

		#region Объявление элементов страницы

		private IWebElement TermValue { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string TERM_VALUE = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td[##]//p";
		protected const string TERM_ROW = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]";

		#endregion
	}
}
