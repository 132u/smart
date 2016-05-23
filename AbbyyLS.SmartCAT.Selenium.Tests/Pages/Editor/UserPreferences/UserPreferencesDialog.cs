using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor.UserPreferences;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class UserPreferencesDialog : EditorPage, IAbstractPage<UserPreferencesDialog>
	{
		public UserPreferencesDialog(WebDriver driver): base(driver)
		{
		}

		public new UserPreferencesDialog LoadPage()
		{
			if (!IsUserPreferencesDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог пользовательских настроек.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть на кнопку 'Сохранить.'
		/// </summary>
		public EditorPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Сохранить.'");
			SaveButton.Click();

			return new EditorPage(Driver).LoadPage();
		}

		/// <summary>
		/// Проверить выключен ли чекбокс 'Confirm Segment', если включен, выключить.
		/// </summary>
		public UserPreferencesDialog CheckConfirmSegmentCheckbox()
		{
			CustomTestContext.WriteLine("Проверить выключен ли чекбокс 'Confirm Segment', если включен, выключить.");
			if (!IsCofirmSegmentUnchecked())
			{
				ClickConfirmSegmentCheckbox();
			}

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу 'Confirm Segment'.
		/// </summary>
		public UserPreferencesDialog ClickConfirmSegmentCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу 'Confirm Segment'.");
			ConfirmSegmentCheckbox.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Переключиться во вкладку 'Segment Confirmation'.
		/// </summary>
		public SegmentConfirmationTab SwitchToSegmentConfirmationTab()
		{
			CustomTestContext.WriteLine("Переключиться во вкладку 'Segment Confirmation'.");
			SegmentConfirmationTab.Click();

			return new SegmentConfirmationTab(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, появился ли диалог пользовательских настроек.
		/// </summary>
		public bool IsUserPreferencesDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(USER_PREFERENCES));
		}

		/// <summary>
		/// Проверить что чекбокс 'Confirm Segment' не отмечен.
		/// true если чекбокс не отмечен, false если отмечен
		/// </summary>
		public bool IsCofirmSegmentUnchecked()
		{
			CustomTestContext.WriteLine("Проверить что чекбокс 'Confirm Segment' не отмечен.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(UNCHECKED_CONFIRM_SEGMENT_CHECKBOX));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SAVE_BTN)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_SEGMENT_CHECKBOX)]
		protected IWebElement ConfirmSegmentCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SEGMENT_CONFIRMATION_TAB)]
		protected IWebElement SegmentConfirmationTab { get; set; }

		protected IWebElement UncheckedConfirmSegmentCheckbox { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string USER_PREFERENCES = "//div[contains(@class, 'x-title-text') and contains(text(), 'User Preferences')]";
		protected const string SAVE_BTN = "//span[contains(text(), 'Save')]";
		protected const string UNCHECKED_CONFIRM_SEGMENT_CHECKBOX = "//div//label[contains(text(), 'Confirm segment')]//parent::div//input[contains(@aria-checked, 'false')]";
		protected const string CONFIRM_SEGMENT_CHECKBOX = "//div//label[contains(text(), 'Confirm segment')]//parent::div//input";
		protected const string SEGMENT_CONFIRMATION_TAB = "//span[contains(text(), 'Segment Confirmation')]";

		#endregion
	}
}