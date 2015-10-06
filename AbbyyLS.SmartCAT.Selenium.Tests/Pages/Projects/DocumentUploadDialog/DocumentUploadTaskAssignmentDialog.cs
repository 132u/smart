using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	class DocumentUploadTaskAssignmentDialog : DocumentUploadBaseDialog, IAbstractPage<DocumentUploadTaskAssignmentDialog>
	{
		public DocumentUploadTaskAssignmentDialog(WebDriver driver) : base(driver)
		{
		}

		public DocumentUploadTaskAssignmentDialog GetPage()
		{
			var documentUploadTaskAssignmentDialog = new DocumentUploadTaskAssignmentDialog(Driver);
			InitPage(documentUploadTaskAssignmentDialog, Driver);

			return documentUploadTaskAssignmentDialog;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(TASK_ASSING_TABLE)))
			{
				Assert.Fail("Произошла ошибка:\n не на шаг назначения пользователей на задачу.");
			}
		}

		protected const string TASK_ASSING_TABLE = ".//div[contains(@class,'js-popup-import-document')][2]//div[@class='js-step last active']";
	}
}
