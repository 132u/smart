using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog
{
	public class InformationFromWFSetupTabDialog : ProjectSettingsDialog, IAbstractPage<InformationFromWFSetupTabDialog>
	{
		public InformationFromWFSetupTabDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new InformationFromWFSetupTabDialog LoadPage()
		{
			if (!IsInformationFromWFSetupTabDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылся информационный диалог.");
			}

			return this;
		}

		/// <summary>
		/// Кликнуть на кнопку закрытия диалога.
		/// </summary>
		public WorkflowSetUpTab СlickCloseButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку закрытия диалога.");
			CloseButton.Click();

			return new WorkflowSetUpTab(Driver).LoadPage();
		}

		/// <summary>
		/// Проверить, открылся ли информационный диалог.
		/// </summary>
		public bool IsInformationFromWFSetupTabDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылся ли информационный диалог.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(INFO_DIALOG));
		}

		[FindsBy (How = How.XPath, Using = CLOSE_BUTTON)]
		protected IWebElement CloseButton { get; set; }

		protected const string INFO_DIALOG = "//div[contains(@class, 'js-popup-text g-popup-info-text')]";
		protected const string CLOSE_BUTTON = "//div[contains(@class, 'js-popup-close')]";
	}
}
