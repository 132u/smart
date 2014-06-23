using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	public class CatPanelResultsHelper : CommonHelper
	{
		public CatPanelResultsHelper(IWebDriver driver, WebDriverWait wait) :
            base (driver, wait)
        {
        }
		
		/// <summary>
		/// Получить процент совпадений
		/// </summary>
		/// <param name="rowNumber">Номер строки CAT</param>
		/// <returns>Процент совпадений</returns>
		public int GetCATTranslationProcentMatch(int rowNumber)
		{
			int procentMatch = 0;
			// Получение процента совпадений для всех элементов CAT
			List<string> textList = GetTextListElement(By.XPath(CAT_PANEL_TYPE_COLUMN_MATCH_XPATH));
			// Переводим в int
			procentMatch = Int32.Parse(textList[rowNumber].Remove(textList[rowNumber].IndexOf('%')));
			return procentMatch;
		}

		protected const string CAT_PANEL_TYPE_COLUMN_MATCH_XPATH = ".//div[@id='cat-body']//table//tbody//tr//td[3]//div//span";
	}
}
