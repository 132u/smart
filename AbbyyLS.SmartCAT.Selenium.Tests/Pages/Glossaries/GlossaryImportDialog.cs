using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryImportDialog : WorkspacePage, IAbstractPage<GlossaryImportDialog>
	{
		public GlossaryImportDialog(WebDriver driver) : base(driver)
		{
		}

		public new GlossaryImportDialog GetPage()
		{
			var glossaryImportDialog = new GlossaryImportDialog(Driver);
			InitPage(glossaryImportDialog, Driver);

			return glossaryImportDialog;
		}

		public new void LoadPage()
		{
			if (!IsGlossaryImportDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог импорта глоссария");
			}
		}

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public GlossaryImportDialog ImportGlossary(string pathFile)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", pathFile);
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" + "arguments[0].style[\"visibility\"] = \"visible\";",
				ImportGlossaryInput);

			ImportGlossaryInput.SendKeys(pathFile);

			Driver.ExecuteScript("document.getElementsByClassName('g-iblock l-editgloss__filelink js-filename-link')[0].innerHTML = '" + Path.GetFileName(pathFile) + "'");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Import в диалоге импорта глоссария
		/// </summary>
		public GlossarySuccessImportDialog ClickImportButtonInImportDialog()
		{
			CustomTestContext.WriteLine("Нажать кнопку Import в диалоге импорта глоссария.");
			ImportButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(IMPORT_IN_PROGRESS_MESSAGE), timeout: 20);

			return new GlossarySuccessImportDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Replace All' в диалоге импорта глоссария
		/// </summary>
		public GlossaryImportDialog ClickReplaceTermsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Replace All' в диалоге импорта глоссария");
			ReplaceAllButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, открыт ли диалог импорта глоссария
		/// </summary>
		public bool IsGlossaryImportDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(IMPORT_BUTTON));
		}

		[FindsBy(How = How.XPath, Using = IMPORT_GLOSSARY_INPUT)]
		protected IWebElement ImportGlossaryInput { get; set; }

		[FindsBy(How = How.XPath, Using = IMPORT_BUTTON)]
		protected IWebElement ImportButton { get; set; }

		[FindsBy(How = How.XPath, Using = REPLACE_ALL_BUTTON)]
		protected IWebElement ReplaceAllButton { get; set; }

		protected const string IMPORT_GLOSSARY_INPUT = "//form[contains(@action,'Enterprise/Glossaries/Import')]//input[contains(@class,'js-submit-input')]";
		protected const string IMPORT_BUTTON = "//div[contains(@class,'js-popup-import')][2]//div[contains(@class,'js-import-button')]";
		protected const string REPLACE_ALL_BUTTON = "//div[contains(@class,'js-popup-import')][2]//input[contains(@name,'needToClear')][@value='true']//following-sibling::em";
		protected const string IMPORT_IN_PROGRESS_MESSAGE = "//div[contains(@class, 'js-please-wait')]";
	}
}
