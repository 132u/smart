using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossarySuccessImportDialog : WorkspacePage, IAbstractPage<GlossarySuccessImportDialog>
	{
		public new GlossarySuccessImportDialog GetPage()
		{
			var glossarySuccessImportDialog = new GlossarySuccessImportDialog();
			InitPage(glossarySuccessImportDialog);

			return glossarySuccessImportDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CLOSE_BUTTON), timeout: 30))
			{
				Assert.Fail("Произошла ошибка:\n не открылся диалог с сообщением о успешном импорте глоссария.");
			}
		}

		public GlossaryPage ClickCloseButton()
		{
			Logger.Debug("Нажать кнопку Close в диалоге с сообщением о успешном импорте глоссария.");
			CloseButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(CLOSE_BUTTON));

			return new GlossaryPage().GetPage();
		}
		
		[FindsBy(How = How.XPath, Using = CLOSE_BUTTON)]
		protected IWebElement CloseButton { get; set; }

		protected const string CLOSE_BUTTON = "//a[contains(@class,'js-close-link')]";
	}
}
