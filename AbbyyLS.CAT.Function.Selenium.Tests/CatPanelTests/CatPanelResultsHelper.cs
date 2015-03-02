using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Panel
{
	/// <summary>
	/// Хелпер панели выдачи переводов
	/// </summary>
	public class CatPanelResultsHelper : AddTermFormHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public CatPanelResultsHelper(IWebDriver driver, WebDriverWait wait) :
			base (driver, wait)
		{
		}
		
		/// <summary>
		/// Возвращает процент совпадений в CAT
		/// </summary>
		/// <param name="rowNumber">Номер строки CAT</param>
		/// <returns>Процент совпадений</returns>
		public int GetCATTranslationProcentMatch(int rowNumber)
		{
			int procentMatch = 0;

			// Получение процента совпадений для всех элементов CAT
			List<string> textList = GetTextListElement(By.XPath(CAT_PANEL_PROCENT_MATCH_XPATH));

			// Переводим в int
			procentMatch = Int32.Parse(textList[rowNumber].Remove(textList[rowNumber].IndexOf('%')));
			return procentMatch;
		}

		/// <summary>
		/// Возвращает список терминов в САТ панели
		/// </summary>
		/// <returns>Список терминов САТ панели</returns>
		public List<string> GetCATTerms()
		{
			// Получение списка терминов в CAT в нижнем регистре
			List <string> terms = new List<string>();

			foreach (string term in GetTextListElement(By.XPath(CAT_PANEL_TERM_XPATH)))
			{
				terms.Add(term.ToLower());
			}
			return terms;
		}



		protected const string CAT_PANEL_TERM_XPATH = ".//div[@id='cat-body']//table//tbody//tr//td[2]//div";
		protected const string CAT_PANEL_PROCENT_MATCH_XPATH = ".//div[@id='cat-body']//table//tbody//tr//td[3]//div//span";
	}
}
