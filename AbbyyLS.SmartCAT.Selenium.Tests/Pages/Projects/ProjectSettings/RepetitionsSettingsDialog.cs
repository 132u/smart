using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class RepetitionsSettingsDialog : WorkspacePage, IAbstractPage<RepetitionsSettingsDialog>
	{
		public RepetitionsSettingsDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new RepetitionsSettingsDialog LoadPage()
		{
			if (!IsRepetitionsSettingsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть диалог настроек повторов .");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть чекбокс 'Auto-propagate repetitions'
		/// </summary>
		public RepetitionsSettingsDialog ClickAutoPropagateRepetitionsCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть чекбокс 'Auto-propagate repetitions'");
			AutoPropagateRepetitionsCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть чекбокс 'Propagate to segments with differences in case'
		/// </summary>
		public RepetitionsSettingsDialog ClickPropagateToSegmentsWithDifferencesInCaseCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть чекбокс 'Propagate to segments with differences in case'");
			PropagateToSegmentsWithDifferencesInCaseCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть чекбокс 'Propagate to confirmed segments'
		/// </summary>
		public RepetitionsSettingsDialog ClickPropagateToConfirmedSegmentsCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть чекбокс 'Propagate to confirmed segments'");
			PropagateToConfirmedSegmentsCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть чекбокс 'Confirm auto-propagated segments'
		/// </summary>
		public RepetitionsSettingsDialog ClickConfirmAutoPropagatedSegmnetsCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть чекбокс 'Confirm auto-propagated segments'");
			ConfirmAutoPropagatedSegmnetsCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть кнопку сохранения настроек
		/// </summary>
		public ProjectSettingsPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Кликнуть кнопку сохранения настроек");
			SaveButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Задать настройки повторов 
		/// </summary>
		/// <param name="autoPropagateRepetitionsChecked">нужно ли отметить чекбокс Auto-propagate repetitions</param>
		/// <param name="propagateToSegmentsWithDifferencesInCaseChecked">нужно ли отметить чекбокс Propagate to segments with differences in case</param>
		/// <param name="propagateToConfirmedSegmentsChecked">нужно ли отметить чекбокс Propagate to confirmed segments</param>
		/// <param name="confirmAutoPropagatedSegmnetsChecked">нужно ли отметить чекбокс Confirm auto-propagated segments</param>
		public RepetitionsSettingsDialog SetRepetitionsSettings(
			bool autoPropagateRepetitionsChecked,
			bool propagateToSegmentsWithDifferencesInCaseChecked,
			bool propagateToConfirmedSegmentsChecked,
			bool confirmAutoPropagatedSegmnetsChecked)
		{
			CustomTestContext.WriteLine("Проверить, что на вкладке 'Повторы' все чекбоксы задизэйблены.");

			if (IsAutoPropagateRepetitionsChecked() ^ autoPropagateRepetitionsChecked)
			{
				ClickAutoPropagateRepetitionsCheckbox();
			}

			if (IsPropagateToSegmentsWithDifferencesInCaseChecked() ^ propagateToSegmentsWithDifferencesInCaseChecked)
			{
				ClickPropagateToSegmentsWithDifferencesInCaseCheckbox();
			}

			if (IsPropagateToConfirmedSegmentsChecked() ^ propagateToConfirmedSegmentsChecked)
			{
				ClickPropagateToConfirmedSegmentsCheckbox();
			}

			if (IsConfirmAutoPropagatedSegmnetsChecked() ^ confirmAutoPropagatedSegmnetsChecked)
			{
				ClickConfirmAutoPropagatedSegmnetsCheckbox();
			}

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что страница настроек повторов открылась
		/// </summary>
		public bool IsRepetitionsSettingsDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(REPETITIONS_SETTINGS_DIALOG));
		}

		/// <summary>
		/// Получить, отмечен ли чекбокс 'Auto-propagate repetitions'
		/// </summary>
		public bool IsAutoPropagateRepetitionsChecked()
		{
			CustomTestContext.WriteLine("Получить, отмечен ли чекбокс 'Auto-propagate repetitions'");

			return AutoPropagateRepetitionsCheckbox.GetIsInputChecked();
		}

		/// <summary>
		/// Получить, отмечен ли чекбокс 'Propagate to segments with differences in case'
		/// </summary>
		public bool IsPropagateToSegmentsWithDifferencesInCaseChecked()
		{
			CustomTestContext.WriteLine("Получить, отмечен ли чекбокс 'Propagate to segments with differences in case'");

			return PropagateToSegmentsWithDifferencesInCaseCheckbox.GetIsInputChecked();
		}

		/// <summary>
		/// Получить, отмечен ли чекбокс 'Propagate to confirmed segments'
		/// </summary>
		public bool IsPropagateToConfirmedSegmentsChecked()
		{
			CustomTestContext.WriteLine("Получить, отмечен ли чекбокс 'Propagate to confirmed segments'");

			return PropagateToConfirmedSegmentsCheckbox.GetIsInputChecked();
		}

		/// <summary>
		/// Получить, отмечен ли чекбокс 'Confirm auto-propagated segments'
		/// </summary>
		public bool IsConfirmAutoPropagatedSegmnetsChecked()
		{
			CustomTestContext.WriteLine("Получить, отмечен ли чекбокс 'Confirm auto-propagated segments'");

			return ConfirmAutoPropagatedSegmnetsCheckbox.GetIsInputChecked();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = AUTO_PROPAGATE_REPETITIONS_CHECKBOX)]
		protected IWebElement AutoPropagateRepetitionsCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = PROPAGATE_TO_SEGMENTS_WITH_DIFFERENCES_IN_CASE_CHECKBOX)]
		protected IWebElement PropagateToSegmentsWithDifferencesInCaseCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = PROPAGATE_TO_CONFIRMED_SEGMENTS_CHECKBOX)]
		protected IWebElement PropagateToConfirmedSegmentsCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_AUTO_PROPAGATED_SEGMENTS_CHECKBOX)]
		protected IWebElement ConfirmAutoPropagatedSegmnetsCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		#endregion

		#region Описания XPath элементов страницы

		protected const string REPETITIONS_SETTINGS_DIALOG = "//div[contains(@class,'js-popup-repetition-settings')][2]";
		protected const string AUTO_PROPAGATE_REPETITIONS_CHECKBOX = "//div[contains(@class,'js-popup-repetition-settings')][2]//input[contains(@data-bind,'checked: autoPropagateRepetitions')]";
		protected const string PROPAGATE_TO_SEGMENTS_WITH_DIFFERENCES_IN_CASE_CHECKBOX = "//div[contains(@class,'js-popup-repetition-settings')][2]//input[contains(@data-bind,'checked: propagateToSegmentsWithDifferencesInCase')]";
		protected const string PROPAGATE_TO_CONFIRMED_SEGMENTS_CHECKBOX = "//div[contains(@class,'js-popup-repetition-settings')][2]//input[contains(@data-bind,'checked: propagateToConfirmedSegments')]";
		protected const string CONFIRM_AUTO_PROPAGATED_SEGMENTS_CHECKBOX = "//div[contains(@class,'js-popup-repetition-settings')][2]//input[contains(@data-bind,'checked: confirmChangedSegments')]";
		protected const string SAVE_BUTTON = "//div[contains(@class,'js-popup-repetition-settings')][2]//div[contains(@data-bind,'click: save')]";

		#endregion
	}
}
