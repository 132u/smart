using NUnit.Framework;

using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class SuggestedTermsPageForCurrentGlossaries : SuggestedTermsPageForAllGlossaries,
	                                                      IAbstractPage<SuggestedTermsPageForCurrentGlossaries>
	{
		public new SuggestedTermsPageForCurrentGlossaries GetPage()
		{
			var suggestTermsPageForCurrentGlossaries = new SuggestedTermsPageForCurrentGlossaries();
			InitPage(suggestTermsPageForCurrentGlossaries);

			return suggestTermsPageForCurrentGlossaries;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SUGGEST_TERMS_TABLE)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница Suggested Terms.");
			}
		}

		/// <summary>
		/// Проверить, что в предложенном термине верное значение
		/// </summary>
		/// <param name="expectedTermValue">ожидаемое значение</param>
		/// <param name="rowNumber">номер строки термина</param>
		/// <param name="columnNumber">номер столбца</param>
		/// <returns></returns>
		public SuggestedTermsPageForCurrentGlossaries AssertTermValueMatch(
			string expectedTermValue,
			int rowNumber,
			int columnNumber)
		{
			Logger.Trace("Проверить, что в предложенном термине (стркоа №{0}, колонка №{1}) верное значение {2}.",
				rowNumber, columnNumber, expectedTermValue);
			var actualTermValue = Driver.FindElement(By.XPath(TERM_VALUE.Replace("*#*", rowNumber.ToString()).Replace("##", columnNumber.ToString()))).Text;

			Assert.AreEqual(expectedTermValue, actualTermValue,
				"Произошла ошибка:\nНеверное значение в термине (строка №{0} колонка №{1}).", rowNumber, columnNumber);

			return GetPage();
		}

		/// <summary>
		/// Получить количество терминов
		/// </summary>
		public int SuggestedTermsCount()
		{
			Logger.Trace("Получить количество терминов.");

			return Driver.GetElementsCount(By.XPath(TERM_ROW));
		}

		protected const string TERM_VALUE = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))][*#*]//td[##]//p";
		protected const string TERM_ROW = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]";
	}
}
