using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class ErrorsDialog : EditorPage, IAbstractPage<ErrorsDialog>
	{
		public new ErrorsDialog GetPage()
		{
			var errorsDialog = new ErrorsDialog();
			InitPage(errorsDialog);

			return errorsDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(MESSAGEBOX)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог поиска ошибок.");
			}
		}

		protected const string MESSAGEBOX = "//div[@id='messagebox']";
	}
}
