using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossarySuccessImportDialog : WorkspacePage, IAbstractPage<GlossarySuccessImportDialog>
	{
		public GlossarySuccessImportDialog(WebDriver driver) : base(driver)
		{
		}

		public new GlossarySuccessImportDialog GetPage()
		{
			var glossarySuccessImportDialog = new GlossarySuccessImportDialog(Driver);
			InitPage(glossarySuccessImportDialog, Driver);

			return glossarySuccessImportDialog;
		}

		public new void LoadPage()
		{
			if (!IsGlossarySuccessImportDialogOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылся диалог с сообщением о успешном импорте глоссария.");
			}
		}

		/// <summary>
		/// Нажать кнопку Close в диалоге с сообщением о успешном импорте глоссария
		/// </summary>
		public GlossaryPage ClickCloseButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Close в диалоге с сообщением о успешном импорте глоссария.");
			CloseButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(CLOSE_BUTTON));

			return new GlossaryPage(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, открылся ли диалог с сообщением о успешном импорте глоссария
		/// </summary>
		public bool IsGlossarySuccessImportDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CLOSE_BUTTON), timeout: 30);
		}

		[FindsBy(How = How.XPath, Using = CLOSE_BUTTON)]
		protected IWebElement CloseButton { get; set; }

		protected const string CLOSE_BUTTON = "//a[contains(@class,'js-close-link')]";
	}
}
