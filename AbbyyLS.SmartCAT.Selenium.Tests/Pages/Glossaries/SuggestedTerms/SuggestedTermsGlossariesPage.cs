﻿using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms
{
	public class SuggestedTermsGlossariesPage : SuggestedTermsPage, IAbstractPage<SuggestedTermsGlossariesPage>
	{
		public SuggestedTermsGlossariesPage(WebDriver driver) : base(driver)
		{
		}

		public new SuggestedTermsGlossariesPage GetPage()
		{
			var suggestTermsPage = new SuggestedTermsGlossariesPage(Driver);
			InitPage(suggestTermsPage, Driver);

			return suggestTermsPage;
		}

		public new void LoadPage()
		{
			if (!IsSuggestedTermsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась общая страница Suggested Terms");
			}
		}

		#region Простые методы
		
		#endregion

		#region Составные методы
		
		#endregion

		#region Методы, проверяющие состояние страницы
		
		#endregion

		#region Объявление элементов страниц

		#endregion

		#region Описания XPath элементов

		#endregion
	}
}