using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class QualityAssuranceAdvancedSettingsSection : AdvancedSettingsSection, IAbstractPage<QualityAssuranceAdvancedSettingsSection>
	{
		public QualityAssuranceAdvancedSettingsSection(WebDriver driver)
			: base(driver)
		{
		}

		public new QualityAssuranceAdvancedSettingsSection LoadPage()
		{
			if (!IsQualityAssuranceAdvancedSettingsSectionOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылись расширенные настройки 'Quality Assurance'.");
			}

			return this;
		}
		
		#region Простые методы страницы
		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылись расширенные настройки 'Quality Assurance'.
		/// </summary>
		/// <returns></returns>
		public bool IsQualityAssuranceAdvancedSettingsSectionOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_SETTINGS));
		}

		#endregion


		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ERROR_SETTINGS)]
		protected IWebElement ErrorSettings { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string ERROR_SETTINGS = "//div[contains(@data-bind, 'errorSettings')]";

		#endregion
	}
}
