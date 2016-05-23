using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor.UserPreferences
{
	public class SegmentConfirmationTab : UserPreferencesDialog
	{
		public SegmentConfirmationTab(WebDriver driver): base(driver)
		{
		}

		public new SegmentConfirmationTab LoadPage()
		{
			if (!IsSegmentConfirmationTabOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не произошло переключение во вкладку " +
					" настроек подтверждения сегментов.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть по пункту 'Перейти к следующему сегменту'.
		/// </summary>
		public SegmentConfirmationTab ClickGoToTheNextSegment()
		{
			CustomTestContext.WriteLine("Кликнуть по пункту 'Перейти к следующему сегменту'.");
			GoToTheNextSegment.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по пункту 'Перейти к следующему неподтвержденному сегменту'.
		/// </summary>
		public SegmentConfirmationTab ClickGoToTheUnconfirmedSegment()
		{
			CustomTestContext.WriteLine("Кликнуть по пункту 'Перейти к следующему неподтвержденному сегменту'.");
			GoToTheUnconfirmedSegment.JavaScriptClick();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что вкладка 'Segment Confirmation' открыта.
		/// </summary>
		public bool IsSegmentConfirmationTabOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENT_CONFIRMATION_TAB));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = GO_TO_THE_NEXT_SEGMENT)]
		protected IWebElement GoToTheNextSegment { get; set; }

		[FindsBy(How = How.XPath, Using = GO_TO_THE_UNCONFIRMED_SEGMENT)]
		protected IWebElement GoToTheUnconfirmedSegment { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string SEGMENT_CONFIRMATION_TAB = "//span[contains(text(), 'After confirming a segment:')]";
		protected const string GO_TO_THE_NEXT_SEGMENT = "//div//label[contains(text(), 'Go to the next segment')]//parent::div//input";
		protected const string GO_TO_THE_UNCONFIRMED_SEGMENT = "//div//label[contains(text(), 'Go to the next unconfirmed segment')]//parent::div//input";

		#endregion

	}
}