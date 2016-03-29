using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	public abstract class DocumentUploadBaseDialog : WorkspacePage
	{
		public DocumentUploadBaseDialog(WebDriver driver) : base(driver)
		{
		}

		/// <summary>
		/// Нажать кнопку "Готово"
		/// </summary>
		public T ClickFinish<T>() where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Готово'");
			FinishButton.ScrollDown();
			FinishButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;
			return instance.GetPage();
		}

		[FindsBy(How = How.XPath, Using = FINISH_BUTTON)]
		protected IWebElement FinishButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON)]
		protected IWebElement NextButton { get; set; }

		protected const string FINISH_BUTTON = "//div[contains(@data-bind, 'completeStep')]//a[contains(text(), 'Finish')]";
		protected const string NEXT_BUTTON = "//div[contains(@data-bind, 'completeStep')]//a[contains(text(), 'Next')]";
	}
}
