using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class QualityAssuranceDialog : WorkspacePage, IAbstractPage<QualityAssuranceDialog>
	{
		public QualityAssuranceDialog(WebDriver driver) : base(driver)
		{
		}

		public QualityAssuranceDialog GetPage()
		{
			var qualityAssuranceDialog = new QualityAssuranceDialog(Driver);
			InitPage(qualityAssuranceDialog, Driver);

			return qualityAssuranceDialog;
		}

		public void LoadPage()
		{
			if (!IsQualityAssuranceDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Диалог контроля качества не открылся.");
			}
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что диалог контроля качества открылся.
		/// </summary>
		public bool IsQualityAssuranceDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DOWNLOAD_REPORT_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CHECK_FOR_ERRORS_BUTTON)]
		protected IWebElement CheckForErrorsButton { get; set; }

		#endregion

		#region Описания XPath элементов страницы

		protected const string CHECK_FOR_ERRORS_BUTTON = "//div[contains(@class, 'qa-check-report-popup')][2]//div[contains(@data-bind, 'checkForErrors')]";
		protected const string DOWNLOAD_REPORT_BUTTON = "//div[contains(@class, 'qa-check-report-popup')][2]//div[contains(@data-bind, 'downloadReport')]";
		#endregion
	}
}
