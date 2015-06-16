using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	public abstract class DocumentUploadBaseDialog : BaseObject
	{
		/// <summary>
		/// Нажать кнопку "Готово"
		/// </summary>
		public T ClickFinish<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку 'Готово'");
			FinishButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Далее'
		/// </summary>
		public T ClickNext<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку 'Далее'.");
			NextButton.Click();

			return new T().GetPage();
		}

		[FindsBy(How = How.XPath, Using = FINISH_BUTTON)]
		protected IWebElement FinishButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON)]
		protected IWebElement NextButton { get; set; }

		protected const string FINISH_BUTTON = ".//div[contains(@class,'js-popup-import-document')][2]//span[contains(@class,'js-finish js-upload-btn')]";
		protected const string NEXT_BUTTON = ".//div[contains(@class,'js-popup-import-document')][2]//span[contains(@class,'js-next')]";
	}
}
