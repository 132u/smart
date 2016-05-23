using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	public class SettingsResourcesStep : DocumentUploadBaseDialog, IAbstractPage<SettingsResourcesStep>
	{
		public SettingsResourcesStep(WebDriver driver): base(driver)
		{
		}

		public new SettingsResourcesStep LoadPage()
		{
			if (!IsSettingsResourcesStepOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся шаг настройки ресурсосв.");
			}

			return this;
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся шаг настройки ресурсов.
		/// </summary>
		public bool IsSettingsResourcesStepOpened()
		{
			CustomTestContext.WriteLine("Проверить, что открылся шаг настройки ресурсов.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(FINISH_BUTTON));
		}

		/// <summary>
		/// Проверить, что TM выбрана.
		/// </summary>
		/// <param name="tmName">TM</param>
		public bool IsTMChecked(string tmName)
		{
			CustomTestContext.WriteLine("Проверить, что TM выбрана.");
			TMCheckboxByName = Driver.SetDynamicValue(How.XPath, TM_CHECKBOX_BY_NAME, tmName);

			return TMCheckboxByName.Selected;
		}

		/// <summary>
		/// Проверить, что глоссарий выбран.
		/// </summary>
		/// <param name="glossary">глоссарий</param>
		public bool IsGlossaryChecked(string glossary)
		{
			CustomTestContext.WriteLine("Проверить, что глоссарий выбран.");
			GlossaryCheckboxByName = Driver.SetDynamicValue(How.XPath, GLOSSARY_CHECKBOX_BY_NAME, glossary);

			return GlossaryCheckboxByName.Selected;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = USE_MACHINE_TRANSLATION_CHECKBOX)]
		protected IWebElement UseMachineTranslationheckbox { get; set; }

		protected IWebElement GlossaryCheckboxByName { get; set; }

		protected IWebElement TMCheckboxByName { get; set; }

		#endregion

		#region Описания XPath элементов
		
		protected const string USE_MACHINE_TRANSLATION_CHECKBOX = "//input[@name='mts-checkbox']";
		protected const string GLOSSARY_CHECKBOX_BY_NAME = "//td//p[text()='*#*']/../preceding-sibling::td//input";
		protected const string TM_CHECKBOX_BY_NAME = "//table[contains(@class,'js-tms-table')]//td[@title='*#*']//following-sibling::td//input";

		#endregion
	}
}
