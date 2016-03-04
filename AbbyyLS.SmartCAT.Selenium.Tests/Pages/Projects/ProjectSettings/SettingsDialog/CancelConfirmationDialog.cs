using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog
{
	public class CancelConfirmationDialog : QualityAssuranceSettings, IAbstractPage<CancelConfirmationDialog>
	{
		public CancelConfirmationDialog(WebDriver driver) : base(driver)
		{
		}

		public new CancelConfirmationDialog GetPage()
		{
			var cancelConfirmationDialog = new CancelConfirmationDialog(Driver);
			InitPage(cancelConfirmationDialog, Driver);

			return cancelConfirmationDialog;
		}

		public new void LoadPage()
		{
			if (!IsCancelConfirmationDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылись настйроки контроля качества.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Close.
		/// </summary>
		public GeneralTab ClickCloseButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Close.");
			CloseButton.Click();

			return new GeneralTab(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel.
		/// </summary>
		public QualityAssuranceSettings ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel.");
			CancelButton.Click();

			return new QualityAssuranceSettings(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог подтверждения
		/// </summary>
		public bool IsCancelConfirmationDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CLOSE_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CLOSE_BUTTON)]
		protected IWebElement CloseButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CANCEL_BUTTON = "//a[contains(@class, 'js-cancel-btn')]";
		protected const string CLOSE_BUTTON = "//div[contains(@class, 'js-ok-btn')]";

		#endregion
	}
}
