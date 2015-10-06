using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	class DocumentUploadSetUpTMDialog : DocumentUploadBaseDialog, IAbstractPage<DocumentUploadSetUpTMDialog>
	{
		public DocumentUploadSetUpTMDialog(WebDriver driver) : base(driver)
		{
		}

		public DocumentUploadSetUpTMDialog GetPage()
		{
			var documentUploadSetUpTMDialog = new DocumentUploadSetUpTMDialog(Driver);
			InitPage(documentUploadSetUpTMDialog, Driver);

			return documentUploadSetUpTMDialog;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(MACHINE_TRANSLATION_CHECKBOX)))
			{
				Assert.Fail("Произошла ошибка:\n не перешли на шаг назначения TM.");
			}
		}

		protected const string MACHINE_TRANSLATION_CHECKBOX = ".//div[contains(@class,'js-popup-import-document')][2]//div[@class='js-mts-checkbox']";
	}
}
