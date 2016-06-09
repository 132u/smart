using System;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog.AdvancedSettings
{
	public class PretranslateSettingsSection : AdvancedSettingsSection, IAbstractPage<PretranslateSettingsSection>
	{
		public PretranslateSettingsSection(WebDriver driver)
			: base(driver)
		{
		}

		public new PretranslateSettingsSection LoadPage()
		{
			if (!IsPretranslateSettingsSectionOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылись настройки предварительного перевода.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть на кнопку предварительного перевода.
		/// </summary>
		public PretranslateSettingsSection ClickPretranslateButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку предварительного перевода.");
			PreTranslateButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку памяти переводов.
		/// </summary>
		public PretranslateSettingsSection ClickTmButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку памяти переводов.");
			TmButton.ScrollAndClickViaElementBlock();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун подтверждения сегмента для правила предварительного перевода.
		/// </summary>
		/// <param name="rule">правило предварительного перевода</param>
		public PretranslateSettingsSection ClickConfirmDropdown(PreTranslateRulles rule)
		{
			CustomTestContext.WriteLine("Нажать на дропдаун подтверждения сегмента для правила предварительного перевода {0}.", rule);
			ConfirmDropdown = Driver.SetDynamicValue(How.XPath, CONFIRM_DROPDOWN, rule.Description());
			ConfirmDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать опцию подтверждения сегмента для правила предварительного перевода.
		/// </summary>
		/// <param name="rule">правило предварительного перевода</param>
		/// <param name="task">задача</param>
		public PretranslateSettingsSection ClickConfirmOprtion(PreTranslateRulles rule, WorkflowTask task)
		{
			CustomTestContext.WriteLine("Выбрать опцию подтверждения сегмента для правила предварительного перевода. {0} на этапе {1}.", rule, task);
			ConfirmOption = Driver.SetDynamicValue(How.XPath, CONFIRM_OPTION, rule.Description(), task.ToString());
			ConfirmOption.ScrollAndClickViaElementBlock();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку 'Оригинал(только цифры)'.
		/// </summary>
		public PretranslateSettingsSection ClickSourceButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Оригинал(только цифры)'.");
			SourceButton.ScrollAndClickViaElementBlock();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку 'Машинный перевод'.
		/// </summary>
		public PretranslateSettingsSection ClickMachineTranslationButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Машинный перевод'.");
			MachineTranslationButton.ScrollAndClickViaElementBlock();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на кнопку 'Добавить правило'.
		/// </summary>
		public PretranslateSettingsSection ClickAddRulleButton()
		{
			CustomTestContext.WriteLine("Кликнуть на кнопку 'Добавить правило'.");
			AddRulleButton.ScrollAndClickViaElementBlock();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть чекбокс 'Confirm segments'.
		/// </summary>
		public PretranslateSettingsSection ClickConfirmSegmentsCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть чекбокс 'Confirm segments'.");
			ConfirmSegmentsCheckbox.ScrollAndClickViaElementBlock();

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Настроить подтверждение сегмента для правила предварительного перевода.
		/// </summary>
		/// <param name="rule">тип предварительного перевода</param>
		/// <param name="workflowTask">этап для подтверждения</param>
		public PretranslateSettingsSection SelectConfirmOption(PreTranslateRulles rule, WorkflowTask workflowTask)
		{
			ClickConfirmDropdown(rule);
			ClickConfirmOprtion(rule, workflowTask);
			
			return LoadPage();
		}

		/// <summary>
		/// Задать правило предварительного перевода.
		/// </summary>
		/// <param name="rulles">тип предварительного перевода и этап</param>
		/// <param name="personalAccount">персональный аккаунт</param>
		public PretranslateSettingsSection SetTranslationsRulles(IList<KeyValuePair<PreTranslateRulles, WorkflowTask?>> rulles, bool personalAccount = false)
		{
			CustomTestContext.WriteLine("Задать правило предварительного перевода.");
			
			foreach (var rule in rulles)
			{
				switch (rule.Key)
				{
					case PreTranslateRulles.MT:
						ClickMachineTranslationButton();
						break;

					case PreTranslateRulles.TM:
						ClickTmButton();
						break;

					case PreTranslateRulles.SRC:
						ClickSourceButton();
						break;

					default:
						throw new Exception("Произошла ошибка: Передан неизвестный параметр для назначения правила предварительного перевода: " + rule.Key.ToString());
				}

				if (rule.Value.HasValue)
				{
					if (!personalAccount)
					{

						SelectConfirmOption(rule.Key, rule.Value.Value);
					}
					else
					{
						ClickConfirmSegmentsCheckbox();
					}
				}

				ClickAddRulleButton();
			}
			
			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылись ли настройки предварительного перевода.
		/// </summary>
		public bool IsPretranslateSettingsSectionOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(PRETRANSLATE_REMARK));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = PRETRANSLATE_BUTTON)]
		protected IWebElement PreTranslateButton { get; set; }

		[FindsBy(How = How.XPath, Using = TM_BUTTON)]
		protected IWebElement TmButton { get; set; }

		[FindsBy(How = How.XPath, Using = SOURCE_BUTTON)]
		protected IWebElement SourceButton { get; set; }

		[FindsBy(How = How.XPath, Using = MACHINE_TRANSLATION_BUTTON)]
		protected IWebElement MachineTranslationButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_RULLE_BUTTON)]
		protected IWebElement AddRulleButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_SEGMENTS_CHECKBOX)]
		protected IWebElement ConfirmSegmentsCheckbox { get; set; }

		protected IWebElement ConfirmDropdown { get; set; }

		protected IWebElement ConfirmOption { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string PRETRANSLATE_REMARK = "//p[contains(text(), 'pretranslation')]";
		protected const string PRETRANSLATE_BUTTON = "//div[contains(@class, 'main-panel-content')]//span[contains(@class, 'mdl-switch__ripple-container')]";
		protected const string TM_BUTTON = "//div[contains(@class, 'item-title')]//a[contains(text(), 'Translation Memory')]";
		protected const string ADD_RULLE_BUTTON = "//div[contains(@class, 'add-btn')]//a[contains(text(), 'Add Rule')]";
		protected const string SOURCE_BUTTON = "//div[contains(@class, 'item-title')]//a[contains(text(), 'Source (Numbers Only)')]";
		protected const string MACHINE_TRANSLATION_BUTTON = "//div[contains(@class, 'item-title')]//a[contains(text(), 'Machine Translation')]";
		protected const string CONFIRM_DROPDOWN = "//div[@class='item-title'][contains(string(),'*#*')]/ancestor::div[contains(@class, 'item')]//div[contains(@data-bind, 'stepsToConfirm')]";
		protected const string CONFIRM_OPTION = "//div[@class='item-title'][contains(string(),'*#*')]/ancestor::div[contains(@class, 'item')]//div[contains(@data-bind, 'stepsToConfirm')]//li[@title='At the \"*##*\" stage']";
		protected const string CONFIRM_SEGMENTS_CHECKBOX = "//input[contains(@data-bind, 'confirmSegments')]";

		#endregion
	}
}
