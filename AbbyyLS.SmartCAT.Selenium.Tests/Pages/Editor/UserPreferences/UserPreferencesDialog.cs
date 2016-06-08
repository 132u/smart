using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
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
		/// Кликнуть по чекбоксу блока 'Copy source'.
		/// </summary>
		public UserPreferencesDialog ClickCopySourceCheckBox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу блока 'Copy source'.");
			CopySourceCheckBox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу блока 'Insert Translation'.
		/// </summary>
		public UserPreferencesDialog ClickInsertTranslationCheckBox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу блока 'Insert Translation'.");
			InserTranslationCheckBox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу 'Confirm Segment'.
		/// </summary>
		public UserPreferencesDialog ClickConfirmSegmentCheckBox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу 'Confirm Segment'.");
			ConfirmSegmentCheckbox.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на чекбокс 'Confirm matches that are 100% and higher'.
		/// </summary>
		public UserPreferencesDialog ClickConfirmMatcheshCheckBox()
		{
			CustomTestContext.WriteLine("Нажать на чекбокс 'Confirm matches that are 100% and higher'.");
			ConfirmMatchesCheckBox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на чекбокс 'Insert machine translation'.
		/// </summary>
		public UserPreferencesDialog ClickInsertMtCheckBox()
		{
			CustomTestContext.WriteLine("Нажать на чекбокс 'Insert machine translation'.");
			InsertMtCheckBox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на чекбокс 'Copy source to target'.
		/// </summary>
		public UserPreferencesDialog ClickCopySourceToTargetCheckBox()
		{
			CustomTestContext.WriteLine("Нажать на чекбокс 'Copy source to target'.");
			CopySourceToTargetCheckBox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по дропдауну с процентами совпадений подстановки из ТМ.
		/// </summary>
		public UserPreferencesDialog ClickDropDownWithPercents()
		{
			CustomTestContext.WriteLine("Кликнуть по дропдауну с процентами совпадений подстановки из ТМ.");
			DropDownWithPercents.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать процент совпадений в ТМ.
		/// </summary>
		/// <param name="percents">проценты совпадений в ТМ</param>
		public UserPreferencesDialog SelectMatchesPercents(TMMatchesPercents percents)
		{
			CustomTestContext.WriteLine("Выбрать процент {0} совпадений в ТМ.", percents.Description());
			Driver.WaitUntilElementIsDisplay(By.XPath(PERCENTS_IN_DROP_DOWN_LIST.Replace("*#*", percents.Description())));
			PercentsInDropDownList = Driver.SetDynamicValue(How.XPath, PERCENTS_IN_DROP_DOWN_LIST, percents.Description());
			PercentsInDropDownList.JavaScriptClick();

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

		#region Составные методы страницы

		/// <summary>
		/// Установтиь проценты совпадений в ТМ.
		/// </summary>
		/// <param name="percents">проценты совпадений в ТМ</param>
		public UserPreferencesDialog SetMatchesInPercents(TMMatchesPercents percents)
		{
			ClickDropDownWithPercents();
			SelectMatchesPercents(percents);

			return LoadPage();
		}

		/// <summary>
		/// Снять галки со всех чекбоксов.
		/// </summary>
		public UserPreferencesDialog UncheckAllCheckboxes()
		{
			CustomTestContext.WriteLine("Снять галки со всех чекбоксов.");
			var allElements = Driver.GetElementList(By.XPath(ALL_CHECKED_ELEMENTS));
			CustomTestContext.WriteLine("Количество обнаруженных элементов - {0}", allElements.Count);

			if (IsConfirmSegmentChecked())
			{
				ClickConfirmSegmentCheckBox();
			}

			if (IsConfirmMatchesChecked())
			{
				ClickConfirmMatcheshCheckBox();
			}

			foreach (var element in allElements)
			{
				element.Click();
			}

			return LoadPage();
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
		/// Проверить что чекбокс 'Confirm Segment' отмечен.
		/// </summary>
		public bool IsConfirmSegmentChecked()
		{
			CustomTestContext.WriteLine("Проверить что чекбокс 'Confirm Segment' отмечен.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_SEGMENT_CHECKED), timeout: 3);
		}

		/// <summary>
		/// Проверить что чекбокс 'Confirm Matches' отмечен.
		/// </summary>
		public bool IsConfirmMatchesChecked()
		{
			CustomTestContext.WriteLine("Проверить что чекбокс 'Confirm Matches' отмечен.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_MATCHES_CHECKED), timeout: 3);
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CONFIRM_MATCHES_CHECKBOX)]
		protected IWebElement ConfirmMatchesCheckBox { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_SEGMENT_CHECKBOX)]
		protected IWebElement ConfirmSegmentCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = DROP_DOWN)]
		protected IWebElement DropDownWithPercents { get; set; }

		[FindsBy(How = How.XPath, Using = INSERT_MT_CHECKBOX)]
		protected IWebElement InsertMtCheckBox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BTN)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = SEGMENT_CONFIRMATION_TAB)]
		protected IWebElement SegmentConfirmationTab { get; set; }

		[FindsBy(How = How.XPath, Using = COPY_SOURCE_CHECK_BOX)]
		protected IWebElement CopySourceCheckBox { get; set; }

		[FindsBy(How = How.XPath, Using = COPY_SOURCE_TO_TARGET_CHECK_BOX)]
		protected IWebElement CopySourceToTargetCheckBox { get; set; }

		[FindsBy(How = How.XPath, Using = INSERT_TRANSLATION_CHECK_BOX)]
		protected IWebElement InserTranslationCheckBox { get; set; }

		protected IWebElement AllCheckedElements { get; set; }
		protected IWebElement PercentsInDropDownList { get; set; }
		protected IWebElement UncheckedConfirmSegmentCheckbox { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string USER_PREFERENCES = "//div[contains(@class, 'x-title-text') and contains(text(), 'User Preferences')]";
		protected const string ALL_CHECKED_ELEMENTS = "//div[contains(@class, 'x-user-settings-window')]//input[@aria-checked='true']//parent::div//span";

		protected const string COPY_SOURCE_CHECK_BOX = "(//div[contains(@class, 'x-field x-form-item x-form-item-rule x-form-type-checkbox')])[1]//input//following-sibling::span";
		protected const string INSERT_TRANSLATION_CHECK_BOX = "(//div[contains(@class, 'x-field x-form-item x-form-item-rule x-form-type-checkbox')])[2]//input//following-sibling::span";
		protected const string INSERT_MT_CHECKBOX = "(//div[contains(@class, 'x-field x-form-item x-form-item-rule x-form-type-checkbox')])[3]//input//following-sibling::span";
		protected const string COPY_SOURCE_TO_TARGET_CHECK_BOX = "(//div[contains(@class, 'x-field x-form-item x-form-item-rule x-form-type-checkbox')])[4]//input//following-sibling::span";

		protected const string CONFIRM_SEGMENT_CHECKBOX = "//div//label[contains(text(), 'Confirm segment')]//parent::div//input";
		protected const string CONFIRM_MATCHES_CHECKBOX = "//div[contains(@class, 'x-panel x-window-item x-panel-default')]//div//label[contains(text(), 'Confirm matches')]//parent::div//span";
		protected const string CONFIRM_MATCHES_CHECKED = "//div[contains(@class, 'x-panel x-window-item x-panel-default')]//div//label[contains(text(), 'Confirm matches')]//parent::div//input[@aria-checked='true']//parent::div//span";
		protected const string CONFIRM_SEGMENT_CHECKED = "//div//label[contains(text(), 'Confirm segment')]//parent::div//input[contains(@aria-checked, 'true')]";

		protected const string DROP_DOWN = "(//div[contains(@class, 'x-container x-container-default x-box-layout-ct')])[2]//div[contains(@class, 'x-box-layout-ct')]//input//parent::div";
		protected const string PERCENTS_IN_DROP_DOWN_LIST = "//div[contains(@class, 'x-boundlist')]//ul//li[contains(text(), '*#*%')]";

		protected const string SEGMENT_CONFIRMATION_TAB = "//span[contains(text(), 'Segment Confirmation')]";

		protected const string SAVE_BTN = "//span[contains(text(), 'Save')]";

		#endregion
	}
}