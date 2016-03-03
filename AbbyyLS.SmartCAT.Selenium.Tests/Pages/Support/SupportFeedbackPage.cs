using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Support
{
	public class SupportFeedbackPage : WorkspacePage, IAbstractPage<SupportFeedbackPage>
	{
		public SupportFeedbackPage(WebDriver driver)
			: base(driver)
		{
		}

		public new SupportFeedbackPage GetPage()
		{
			var supportFeedbackPage = new SupportFeedbackPage(Driver);
			InitPage(supportFeedbackPage, Driver);

			return supportFeedbackPage;
		}

		public new void LoadPage()
		{
			if (!IsSupportFeedbackPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылась страница ответов техподдержки.");
			}
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница
		/// </summary>
		public bool IsSupportFeedbackPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SUPPORT_FEEDBACK));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SUPPORT_FEEDBACK)]
		protected IWebElement SupportFeedback { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string SUPPORT_FEEDBACK = "//div[contains(@class, 'g-top-nav')]//span['Answers from Support']";

		#endregion
	}
}