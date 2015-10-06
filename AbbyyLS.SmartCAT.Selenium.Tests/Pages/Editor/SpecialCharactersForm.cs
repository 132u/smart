using NUnit.Framework;
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

		public new SpecialCharactersForm GetPage()
		{
			var specialCharactersForm = new SpecialCharactersForm(Driver);
			InitPage(specialCharactersForm, Driver);

			return specialCharactersForm;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CHAR_LIST)))
			{
				Assert.Fail("Произошла ошибка:\n не появилась форма 'Специальные символы'.");
			}
		}

		protected const string CHAR_LIST = "//ul[@id='char-list']";
	}
}
