using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class TranslationMemoryAdvancedSettingsSection : AdvancedSettingsSection, IAbstractPage<TranslationMemoryAdvancedSettingsSection>
	{
		public TranslationMemoryAdvancedSettingsSection(WebDriver driver): base(driver)
		{
		}

		public new TranslationMemoryAdvancedSettingsSection GetPage()
		{
			var translationMemoryAdvancedSettingsSection = new TranslationMemoryAdvancedSettingsSection(Driver);
			InitPage(translationMemoryAdvancedSettingsSection, Driver);

			return translationMemoryAdvancedSettingsSection;
		}

		public new void LoadPage()
		{
			if (!IsTranslationMemoryAdvancedSettingsSectionOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылись расширенные настройки 'Translation Memory'.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку 'Select' в расширенных настройках 'Translation Memory'.
		/// </summary>
		public NewProjectSetUpTMDialog ClickSelectTmButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Select' в расширенных настройках 'Translation Memory'.");
			SelectTmButton.JavaScriptClick();

			return new NewProjectSetUpTMDialog(Driver).GetPage();
		}

		#endregion

		#region Описания XPath элементов

		[FindsBy(How = How.XPath, Using = SELECT_TM_BUTTON)]
		protected IWebElement SelectTmButton { get; set; }

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылись расширенные настройки 'Translation Memory'.
		/// </summary>
		/// <returns></returns>
		public bool IsTranslationMemoryAdvancedSettingsSectionOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SELECT_TM_BUTTON));
		}

		#endregion

		#region Объявление XPath элементов

		protected const string SELECT_TM_BUTTON = "//div[@class='g-btn g-greenbtn ' and contains(@data-bind, 'addExistingTM')]//a";

		#endregion
	}
}
