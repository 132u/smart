using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class SpecialCharactersForm : EditorPage, IAbstractPage<SpecialCharactersForm>
	{
		public SpecialCharactersForm(WebDriver driver) : base(driver)
		{
		}

		public new SpecialCharactersForm LoadPage()
		{
			if (!IsSpecialCharactersFormOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появилась форма 'Специальные символы'.");
			}

			return this;
		}

		/// <summary>
		/// Проверить, открыта ли форма 'Специальные символы'
		/// </summary>
		public bool IsSpecialCharactersFormOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CHAR_LIST));
		}

		protected const string CHAR_LIST = "//ul[@id='char-list']";
	}
}
