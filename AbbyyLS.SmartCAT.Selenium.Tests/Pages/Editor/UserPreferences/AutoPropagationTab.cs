using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor.UserPreferences
{
	public class AutoPropagationTab : UserPreferencesDialog
	{
		public AutoPropagationTab(WebDriver driver)
			: base(driver)
		{
		}

		public new AutoPropagationTab LoadPage()
		{
			if (!IsAutoPropagationTabOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не произошло переключение во вкладку настроек повторов.");
			}

			return this;
		}

		#region Простые методы страницы

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Проверить, что на вкладке 'Повторы' нужные чекбоксы задизэйблены 
		/// </summary>
		/// <param name="autoPropagateRepetitionsDisabled">должен ли быть задизэйблен чекбокс Auto-propagate repetitions</param>
		/// <param name="propagateToSegmentsWithDifferencesInCaseDisabled">должен ли быть задизэйблен чекбокс Propagate to segments with differences in case</param>
		/// <param name="propagateToConfirmedSegmentsDisabled">должен ли быть задизэйблен чекбокс Propagate to confirmed segments</param>
		/// <param name="confirmAutoPropagatedSegmnetsDisabled">должен ли быть задизэйблен чекбокс Confirm auto-propagated segments</param>
		public bool CheckCheckboxesDisabled(
			bool autoPropagateRepetitionsDisabled,
			bool propagateToSegmentsWithDifferencesInCaseDisabled,
			bool propagateToConfirmedSegmentsDisabled,
			bool confirmAutoPropagatedSegmnetsDisabled)
		{
			bool result = true;
			bool actualValue = IsAutoPropagationRepetiotionsCheckboxDisabled();
			if (actualValue ^ autoPropagateRepetitionsDisabled)
			{
				CustomTestContext.WriteLine(
					"Чекбокс 'Auto-propagate repetitions' имеет значение дизэйбла:{0}, но должен иметь значение {1}.",
					actualValue,
					autoPropagateRepetitionsDisabled);

				result = false;
			}

			actualValue = IsPropagateToSegmentsWithDifferencesInCaseCheckboxDisabled();
			if (actualValue ^ propagateToSegmentsWithDifferencesInCaseDisabled)
			{
				CustomTestContext.WriteLine(
					"Чекбокс 'Propagate to segments with differences in case' имеет значение дизэйбла:{0}, но должен иметь значение {1}.",
					actualValue,
					propagateToSegmentsWithDifferencesInCaseDisabled);

				result = false;
			}

			actualValue = IsPropagateToConfirmSegmentsCheckboxDisabled();
			if (actualValue ^ propagateToConfirmedSegmentsDisabled)
			{
				CustomTestContext.WriteLine(
					"Чекбокс 'Propagate to confirmed segments' имеет значение дизэйбла:{0}, но должен иметь значение {1}.",
					actualValue,
					propagateToConfirmedSegmentsDisabled);

				result = false;
			}

			actualValue = IsConfirmAutoPropagatedSegmentCheckboxDisabled();
			if (actualValue ^ confirmAutoPropagatedSegmnetsDisabled)
			{
				CustomTestContext.WriteLine(
					"Чекбокс 'Confirm auto-propagated segments' имеет значение дизэйбла:{0}, но должен иметь значение {1}.",
					actualValue,
					confirmAutoPropagatedSegmnetsDisabled);

				result = false;
			}

			return result;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что вкладка настроек повторов открыта.
		/// </summary>
		public bool IsAutoPropagationTabOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENT_CONFIRMATION_TAB));
		}

		/// <summary>
		/// Проверить, что чекбокс 'Auto-propagate repetitions' задизэйблен
		/// </summary>
		public bool IsAutoPropagationRepetiotionsCheckboxDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что чекбокс 'Auto-propagate repetitions' задизэйблен");
			return AutoPropagationRepetiotionsCheckbox.GetAttribute("disabled") != null;
		}

		/// <summary>
		/// Проверить, что чекбокс 'Propagate to segments with differences in case' задизэйблен
		/// </summary>
		public bool IsPropagateToSegmentsWithDifferencesInCaseCheckboxDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что чекбокс 'Propagate to segments with differences in case' задизэйблен");
			return PropagateToSegmentsWithDifferencesInCaseCheckbox.GetAttribute("disabled") != null;
		}

		/// <summary>
		/// Проверить, что чекбокс 'Propagate to confirmed segments' задизэйблен
		/// </summary>
		public bool IsPropagateToConfirmSegmentsCheckboxDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что чекбокс 'Propagate to confirmed segments' задизэйблен");
			return PropagateToConfirmSegmentsCheckbox.GetAttribute("disabled") != null;
		}

		/// <summary>
		/// Проверить, что чекбокс 'Confirm auto-propagated segments' задизэйблен
		/// </summary>
		public bool IsConfirmAutoPropagatedSegmentCheckboxDisabled()
		{
			CustomTestContext.WriteLine("Проверить, что чекбокс 'Confirm auto-propagated segments' задизэйблен");
			return ConfirmAutoPropagatedSegmentCheckbox.GetAttribute("disabled") != null;
		}

		/// <summary>
		/// Получить, отмечен ли чекбокс 'Propagate to segments with differences in case'
		/// </summary>
		public bool IsPropagateToSegmentsWithDifferencesInCaseChecked()
		{
			CustomTestContext.WriteLine("Получить, отмечен ли чекбокс 'Propagate to segments with differences in case'");

			return PropagateToSegmentsWithDifferencesInCaseCheckbox.GetAttribute("aria-checked") == "true";
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CONFIRM_AUTO_PROPAGATED_SEGMENT_CHECKBOX)]
		protected IWebElement ConfirmAutoPropagatedSegmentCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = PROPAGATE_TO_CONFIRMED_SEGMENTS_CHECKBOX)]
		protected IWebElement PropagateToConfirmSegmentsCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = PROPAGATE_TO_SEGMENTS_WITH_DIFFERENCES_IN_CASE_CHECKBOX)]
		protected IWebElement PropagateToSegmentsWithDifferencesInCaseCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = AUTO_PROPAGATE_REPETITIONS_CHECKBOX)]
		protected IWebElement AutoPropagationRepetiotionsCheckbox { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CONFIRM_AUTO_PROPAGATED_SEGMENT_CHECKBOX = "//label[text()='Confirm auto-propagated segments']//parent::div//input";
		protected const string PROPAGATE_TO_CONFIRMED_SEGMENTS_CHECKBOX = "//label[text()='Propagate to confirmed segments']//parent::div//input";
		protected const string PROPAGATE_TO_SEGMENTS_WITH_DIFFERENCES_IN_CASE_CHECKBOX = "//label[text()='Propagate to segments with differences in case']//parent::div//input";
		protected const string AUTO_PROPAGATE_REPETITIONS_CHECKBOX = "//label[text()='Auto-propagate repetitions']//parent::div//input";
		
		#endregion

	}
}