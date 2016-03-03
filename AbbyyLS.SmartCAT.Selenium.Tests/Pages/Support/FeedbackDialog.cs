using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Support
{
	public class FeedbackDialog : WorkspacePage, IAbstractPage<FeedbackDialog>
	{
		public FeedbackDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new FeedbackDialog GetPage()
		{
			var feedbackDialog = new FeedbackDialog(Driver);
			InitPage(feedbackDialog, Driver);

			return feedbackDialog;
		}

		public new void LoadPage()
		{
			if (!IsSupportFeedbackDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог ответов техподдержки.");
			}
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли диалог ответов техподдержки.
		/// </summary>
		public bool IsSupportFeedbackDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(FEEDBACK_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = FEEDBACK_DIALOG)]
		protected IWebElement SupportFeedbackDialog { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string FEEDBACK_DIALOG = "//div[contains(@class, 'js-feedback-popup')]";

		#endregion
	}
}