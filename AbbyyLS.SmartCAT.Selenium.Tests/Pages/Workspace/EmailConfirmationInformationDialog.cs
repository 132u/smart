using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace
{
	public class EmailConfirmationInformationDialog : WorkspacePage, IAbstractPage<EmailConfirmationInformationDialog>
	{
		public EmailConfirmationInformationDialog(WebDriver driver) : base(driver)
		{
		}

		public EmailConfirmationInformationDialog LoadPage()
		{
			if(!IsEmailConfirmationInformationDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка: не открылось информационное сообщение о повторной отправке email");
			}

			return this;
		}

		/// <summary>
		/// Нажать на кнопку Close
		/// </summary>
		public WorkspacePage ClickCloseInformationButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Close");
			CloseInformationButton.Click();

			return new WorkspacePage(Driver).LoadPage();
		}

		public bool IsEmailConfirmationInformationDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(EMAIL_CONFIRMATION_INFORMATION_MESSAGE)) &&
				Driver.WaitUntilElementIsDisplay(By.XPath(CLOSE_INFORMATION_BUTTON));
		}

		[FindsBy(How = How.XPath, Using = EMAIL_CONFIRMATION_INFORMATION_MESSAGE)]
		protected IWebElement EmailConfirmationInformationMessage { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_INFORMATION_BUTTON)]
		protected IWebElement CloseInformationButton { get; set; }

		protected const string EMAIL_CONFIRMATION_INFORMATION_MESSAGE = "//div[text()='An email was sent to you. Please confirm your address using the link in the email.']";
		protected const string CLOSE_INFORMATION_BUTTON = "//div[text()='An email was sent to you. Please confirm your address using the link in the email.']//following-sibling::div//a";
	}
}
