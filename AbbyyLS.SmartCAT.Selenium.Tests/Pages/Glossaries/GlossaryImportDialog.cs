using System.IO;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryImportDialog : WorkspacePage, IAbstractPage<GlossaryImportDialog>
	{
		public new GlossaryImportDialog GetPage()
		{
			var glossaryImportDialog = new GlossaryImportDialog();
			InitPage(glossaryImportDialog);

			return glossaryImportDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(IMPORT_BUTTON)))
			{
				Assert.Fail("Произошла ошибка:\n не открылся диалог импорта глоссария.");
			}
		}

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public GlossaryImportDialog ImportGlossary(string pathFile)
		{
			Logger.Trace("Ввести путь к файлу {0} в поле импорта.", pathFile);
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" + "arguments[0].style[\"visibility\"] = \"visible\";",
				ImportGlossaryInput);

			ImportGlossaryInput.SendKeys(pathFile);

			Driver.ExecuteScript("document.getElementsByClassName('g-iblock g-bold l-editgloss__filelink js-filename-link')[0].innerHTML = '" + Path.GetFileName(pathFile) + "'");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Import в диалоге импорта глоссария
		/// </summary>
		public GlossarySuccessImportDialog ClickImportButtonInImportDialog()
		{
			Logger.Debug("Нажать кнопку Import в диалоге импорта глоссария.");
			ImportButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(IMPORT_IN_PROGRESS_MESSAGE), timeout: 20);

			return new GlossarySuccessImportDialog().GetPage();
		}

		public GlossaryImportDialog ClickReplaceTermsButton()
		{
			Logger.Debug("Нажать кнопку 'Replace All' в диалоге импорта глоссария.");
			ReplaceAllButton.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = IMPORT_GLOSSARY_INPUT)]
		protected IWebElement ImportGlossaryInput { get; set; }

		[FindsBy(How = How.XPath, Using = IMPORT_BUTTON)]
		protected IWebElement ImportButton { get; set; }

		[FindsBy(How = How.XPath, Using = REPLACE_ALL_BUTTON)]
		protected IWebElement ReplaceAllButton { get; set; }

		protected const string IMPORT_GLOSSARY_INPUT = "//form[contains(@action,'Enterprise/Glossaries/Import')]//input[contains(@class,'js-submit-input')]";
		protected const string IMPORT_BUTTON = "//div[contains(@class,'js-popup-import')][2]//span[contains(@class,'js-import-button')]";
		protected const string REPLACE_ALL_BUTTON = "//div[contains(@class,'js-popup-import')][2]//input[contains(@id,'needToClear')][@value='True']";
		protected const string IMPORT_IN_PROGRESS_MESSAGE = "//div[contains(@class, 'js-please-wait')]";
	}
}
