using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class ConfirmReplacementDialog : ImportTmxDialog, IAbstractPage<ConfirmReplacementDialog>
	{
		public ConfirmReplacementDialog(WebDriver driver) : base(driver)
		{
		}

		public new ConfirmReplacementDialog GetPage()
		{
			var confirmReplacementDialog = new ConfirmReplacementDialog(Driver);
			InitPage(confirmReplacementDialog, Driver);

			return confirmReplacementDialog;
		}

		public new void LoadPage()
		{
			if (IsConfirmReplacementMessageDisplayed())
			{
				throw new XPathLookupException("Произошла ошибка: \nне открылся диалог подтверждения замены при импорте");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку подтверждения замены ТМ в окне импорта
		/// </summary>
		public TranslationMemoriesPage ClickConfirmReplacementButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку подтверждения замены ТМ в окне импорта");
			ConfirmReplacementButton.Click();

			return new TranslationMemoriesPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку подтверждения замены ТМ в окне импорта
		/// </summary>
		public ImportTmxDialog ClickConfirmReplacementButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку подтверждения замены ТМ в окне импорта");
			ConfirmReplacementButton.Click();

			return new ImportTmxDialog(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что появилось окно подтверждения замены при импорте
		/// </summary>
		public bool IsConfirmReplacementMessageDisplayed()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(UPDATE_TM_CONFIRM_REPLACEMENT));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = UPDATE_TM_CONFIRM_REPLACEMENT)]
		protected IWebElement ConfirmReplacementButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string UPDATE_TM_CONFIRM_REPLACEMENT = "//span[text()='Confirmation']//..//..//..//input[@value='Continue']";

		#endregion
	}
}
