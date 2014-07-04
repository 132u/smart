using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	public class AssignResponsiblesHelper : CommonHelper
	{
		public AssignResponsiblesHelper(IWebDriver driver, WebDriverWait wait) :
            base (driver, wait)
        {
        }

		/// <summary>
		/// Возвращает, виден ли выпадающий список пользователей
		/// </summary>
		/// <returns>Виден ли выпадающий список</returns>
		public bool GetIsResponsiblesSetupDisplayed()
		{
			return GetIsElementDisplay(By.XPath(DROPDOWNLIST_XPATH));
		}

		protected const string RESPONSIBLES_SETUP_XPATH = ".//div[contains(@class,'js-popup-progress')][2]";
		protected const string DROPDOWNLIST_XPATH = "//td[select[@id='responsible']]/span";
	}
}