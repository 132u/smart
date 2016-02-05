using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms
{
	public class SuggestedTermsGlossaryPage : SuggestedTermsPage, IAbstractPage<SuggestedTermsGlossaryPage>
	{
		public SuggestedTermsGlossaryPage(WebDriver driver) : base(driver)
		{
		}

		public new SuggestedTermsGlossaryPage GetPage()
		{
			var suggestTermsPageForCurrentGlossaries = new SuggestedTermsGlossaryPage(Driver);
			InitPage(suggestTermsPageForCurrentGlossaries, Driver);

			return suggestTermsPageForCurrentGlossaries;
		}

		public new void LoadPage()
		{
			if (!IsSuggestedTermsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница Suggested Terms");
			}
		}

		#region Простые методы

		#endregion

		#region Составные методы

		#endregion

		#region Методы, проверяющие состояние страницы

		#endregion

		#region Объявление элементов страницы

		private IWebElement TermValue { get; set; }

		#endregion

		#region Описания XPath элементов

		#endregion
	}
}
