using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	class DocumentUploadAssignResponsiblesDialog : DocumentUploadBaseDialog, IAbstractPage<DocumentUploadAssignResponsiblesDialog>
	{
		public DocumentUploadAssignResponsiblesDialog GetPage()
		{
			var documentUploadAssignResponsiblesDialog = new DocumentUploadAssignResponsiblesDialog();
			InitPage(documentUploadAssignResponsiblesDialog);

			return documentUploadAssignResponsiblesDialog;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ASSING_RESPONSIBLES_TABLE)))
			{
				Assert.Fail("Произошла ошибка:\n не на шаг назначения пользователей на задачу.");
			}
		}

		protected const string ASSING_RESPONSIBLES_TABLE = ".//div[contains(@class,'js-popup-import-document')][2]//div[@class='js-step last active']";
	}
}
