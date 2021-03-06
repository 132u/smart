﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog.AdvancedSettings;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class AdvancedSettingsSection : NewProjectCreateBaseDialog, IAbstractPage<AdvancedSettingsSection>
	{
		public AdvancedSettingsSection(WebDriver driver): base(driver)
		{
		}

		public new AdvancedSettingsSection LoadPage()
		{
			if (!IsAdvancedSettingsSectionOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылись расширенные настройки.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку 'Select' в расширенных настройках проекта
		/// </summary>
		public TranslationMemoryAdvancedSettingsSection ClickSelectTranslationMemoryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Select' в расширенных настройках проекта");
			SelectTmButton.JavaScriptClick();

			return new TranslationMemoryAdvancedSettingsSection(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на вкладку Pretranslation.
		/// </summary>
		public PretranslateSettingsSection ClickPretranslationTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку Pretranslation.");
			PretranslationTab.Click();

			return new PretranslateSettingsSection(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на вкладку 'Translation Memory' в секции расширенных настройках.
		/// </summary>
		public TranslationMemoryAdvancedSettingsSection ClickTranslationMemoryTab()
		{
			CustomTestContext.WriteLine("Перейти на вкладку 'Translation Memory' в секции расширенных настройках.");
			TranslationMemoryTab.Click();

			return new TranslationMemoryAdvancedSettingsSection(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на вкладку Glossaries в секции расширенных настройках.
		/// </summary>
		public GlossariesAdvancedSettingsSection ClickGlossariesTab()
		{
			CustomTestContext.WriteLine("Перейти на вкладку Glossaries в секции расширенных настройках.");
			GlossariesTab.Click();

			return new GlossariesAdvancedSettingsSection(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на вкладку 'Quality Assurance' в секции расширенных настройках.
		/// </summary>
		public QualityAssuranceAdvancedSettingsSection ClickQualityAssuranceTab()
		{
			CustomTestContext.WriteLine("Перейти на вкладку 'Quality Assurance' в секции расширенных настройках.");
			QualityAssuranceTab.Click();

			return new QualityAssuranceAdvancedSettingsSection(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылись расширенные настройки.
		/// </summary>
		public bool IsAdvancedSettingsSectionOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(TRANSLATION_MEMORY_TAB));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = TRANSLATION_MEMORY_TAB)]
		protected IWebElement TranslationMemoryTab { get; set; }

		[FindsBy(How = How.XPath, Using = PRETRANSLATION_TAB)]
		protected IWebElement PretranslationTab { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARIES_TAB)]
		protected IWebElement GlossariesTab { get; set; }
		
		[FindsBy(How = How.XPath, Using = QUALITY_ASSURANCE_TAB)]
		protected IWebElement QualityAssuranceTab { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_TM_BUTTON)]
		protected IWebElement SelectTmButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DEADLINE_DATE_CURRENT = "//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//td[contains(@class, 'ui-datepicker-today')]//a";
		protected const string PRETRANSLATION_TAB = "//li[text()='Pretranslation']";
		protected const string SELECT_TM_BUTTON = "//div[contains(@class,'greenbtn') and contains(@data-bind, 'addExistingTM')]//a";
		protected const string TRANSLATION_MEMORY_TAB = "//ul[contains(@data-bind, 'advancedSettingsTabs')]//li[1]";
		protected const string GLOSSARIES_TAB = "//ul[contains(@data-bind, 'advancedSettingsTabs')]//li[2]";
		protected const string QUALITY_ASSURANCE_TAB = "//ul[contains(@data-bind, 'advancedSettingsTabs')]//li[3]";

		#endregion

	}
}
