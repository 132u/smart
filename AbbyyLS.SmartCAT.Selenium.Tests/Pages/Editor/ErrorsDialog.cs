using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class ErrorsDialog : EditorPage, IAbstractPage<ErrorsDialog>
	{
		public ErrorsDialog(WebDriver driver) : base(driver)
		{
		}

		public new ErrorsDialog LoadPage()
		{
			if (!IsErrorsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог поиска ошибок");
			}

			return this;
		}

		/// <summary>
		/// Проверить, открылся ли диалог поиска ошибок
		/// </summary>
		public bool IsErrorsDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(MESSAGEBOX));
		}

		protected const string MESSAGEBOX = "//div[@id='messagebox']";
	}
}
