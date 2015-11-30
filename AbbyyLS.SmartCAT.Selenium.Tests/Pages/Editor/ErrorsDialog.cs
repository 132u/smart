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

		public new ErrorsDialog GetPage()
		{
			var errorsDialog = new ErrorsDialog(Driver);
			InitPage(errorsDialog, Driver);

			return errorsDialog;
		}

		public new void LoadPage()
		{
			if (!IsErrorsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог поиска ошибок");
			}
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
