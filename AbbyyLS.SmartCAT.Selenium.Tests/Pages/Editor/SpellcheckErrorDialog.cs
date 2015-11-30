using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class SpellcheckErrorDialog : EditorPage, IAbstractPage<SpellcheckErrorDialog>
	{
		public SpellcheckErrorDialog(WebDriver driver) : base(driver)
		{
		}

		public new SpellcheckErrorDialog GetPage()
		{
			var spellcheckErrorDialog = new SpellcheckErrorDialog(Driver);
			InitPage(spellcheckErrorDialog, Driver);

			return spellcheckErrorDialog;
		}

		public new void LoadPage()
		{
			if (!IsSpellcheckErrorDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появилось окно с предупреждением об ошибке");
			}
		}

		/// <summary>
		/// Нажать кнопку 'ОК'
		/// </summary>
		public SpellcheckDictionaryDialog ClickOkButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'ОК'");
			OkButton.Click();

			return new SpellcheckDictionaryDialog(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, появилось ли окно с предупреждением об ошибке
		/// </summary>
		public bool IsSpellcheckErrorDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(MESSAGEBOX));
		}

		[FindsBy(How = How.XPath, Using = OK_BUTTON)]
		protected IWebElement OkButton { get; set; }

		protected const string MESSAGEBOX = "//div[@id='messagebox']";
		protected const string OK_BUTTON = "//div[@id='messagebox']//a";
	}
}
