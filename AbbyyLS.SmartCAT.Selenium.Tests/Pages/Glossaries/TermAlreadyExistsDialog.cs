using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class TermAlreadyExistsDialog : WorkspacePage, IAbstractPage<TermAlreadyExistsDialog>
	{
		public TermAlreadyExistsDialog(WebDriver driver) : base(driver)
		{
		}

		public new TermAlreadyExistsDialog GetPage()
		{
			var termAlreadyExistsDialog = new TermAlreadyExistsDialog(Driver);
			InitPage(termAlreadyExistsDialog, Driver);

			return termAlreadyExistsDialog;
		}

		public new void LoadPage()
		{
			if (!IsTermAlreadyExistsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог сохранения уже существующего термина");
			}
		}

		/// <summary>
		/// Проверить, открылся диалог сохранения уже существующего термина
		/// </summary>
		public bool IsTermAlreadyExistsDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылся диалог сохранения уже существующего термина");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ALREADY_EXIST_TERM_ERROR));
		}

		protected const string ALREADY_EXIST_TERM_ERROR = "//span[contains(text(),'The term already exists')]";
	}
}
