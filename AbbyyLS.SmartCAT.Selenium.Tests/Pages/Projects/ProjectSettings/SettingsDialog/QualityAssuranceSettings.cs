using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog
{
	public class QualityAssuranceSettings : ProjectSettingsDialog, IAbstractPage<QualityAssuranceSettings>
	{
		public QualityAssuranceSettings(WebDriver driver) : base(driver)
		{
		}

		public new QualityAssuranceSettings LoadPage()
		{
			if (!IsQualityAssuranceSettingsOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылись настйроки контроля качества.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Снять галочку Error Title
		/// </summary>
		public QualityAssuranceSettings UncheckErrotTitleCheckbox()
		{
			CustomTestContext.WriteLine("Снять галочку Error Title.");
			EditorTitleCheckbox.Click();
			EditorTitleCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Apply.
		/// </summary>
		public GeneralTab ClickApplyButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Apply.");
			ApplyButton.Click();

			return new GeneralTab(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel без изменений настроек.
		/// </summary>
		public GeneralTab ClickCancelButtonWithoutChanges()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel без изменений настроек.");
			CancelButton.Click();

			return new GeneralTab(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel после изменений настроек.
		/// </summary>
		public CancelConfirmationDialog ClickCancelButtonWithChanges()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel после изменений настроек.");
			CancelButton.Click();

			return new CancelConfirmationDialog(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылись настйроки контроля качества.
		/// </summary>
		public bool IsQualityAssuranceSettingsOpened()
		{
			return Driver.WaitUntilElementIsDisappeared(By.XPath(CLOSE_BUTTON)) 
				&& Driver.WaitUntilElementIsDisplay(By.XPath(APPLY_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = APPLY_BUTTON)]
		protected IWebElement ApplyButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDITOR_TITLE_CHECKBOX)]
		protected IWebElement EditorTitleCheckbox { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CLOSE_BUTTON = "//div[contains(@class, 'js-ok-btn')]";
		protected const string APPLY_BUTTON = "//div[contains(@class, 'qa-settings-popup')][2]//div[contains(@data-bind, 'apply')]";
		protected const string CANCEL_BUTTON = "//div[contains(@class, 'qa-settings-popup')][2]//a[contains(@class, 'cancel')]";
		protected const string EDITOR_TITLE_CHECKBOX = "//div[contains(@class, 'qa-settings-popup')][2]//thead//input[contains(@data-bind, 'allSettingsEnabled')]";

		#endregion
	}
}
