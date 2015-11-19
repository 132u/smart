using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSettingsPage : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSettingsPage>
	{
		public NewProjectSettingsPage(WebDriver driver): base(driver)
		{
		}

		public new NewProjectSettingsPage GetPage()
		{
			var newProjectSettingPage = new NewProjectSettingsPage(Driver);
			InitPage(newProjectSettingPage, Driver);

			return newProjectSettingPage;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!IsNewProjectSettingsPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылась страница настроек создаваемого проекта");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на вкладку Glossaries в панели 'Advanced Settings'.
		/// </summary>
		public NewProjectSettingsPage ClickGlossariesTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку Glossaries в панели 'Advanced Settings'.");
			GlossariesTab.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Create Glossary' в панели 'Advanced Settings'.
		/// </summary>
		public NewProjectSettingsPage ClickCreateGlossaryButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Create Glossary' в панели 'Advanced Settings'.");
			CreateGlossaryButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Edit Glossary' в панели 'Advanced Settings'.
		/// </summary>
		public NewProjectEditGlossaryDialog ClickEditGlossaryButton(int glossaryNumber)
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Edit Glossary' в панели 'Advanced Settings' для глоссария №{0}.", glossaryNumber);
			Driver.SetDynamicValue(How.XPath, EDIT_GLOSSARY_BUTTON, glossaryNumber.ToString()).Click();

			return new NewProjectEditGlossaryDialog(Driver).GetPage();
		}

		/// <summary>
		/// Навести курсор на глоссарий в панели 'Advanced Settings'.
		/// </summary>
		public NewProjectSettingsPage HoverGlossaryRow(int glossaryNumber)
		{
			CustomTestContext.WriteLine("Навести курсор на глоссарий №{0} в панели 'Advanced Settings'.", glossaryNumber);
			Driver.SetDynamicValue(How.XPath, GLOSSARY_ROW, glossaryNumber.ToString()).HoverElement();

			return GetPage();
		}

		/// <summary>
		/// Ввести им проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public NewProjectSettingsPage FillProjectName(string projectName)
		{
			CustomTestContext.WriteLine("Ввести имя проекта: {0}.", projectName);
			ProjectNameInput.SetText(projectName, projectName.Length > 100 ? projectName.Substring(0, 100) : projectName);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на дропдаун исходного языка, чтобы появился выпадающий список
		/// </summary>
		public NewProjectSettingsPage ClickSourceLangDropdown()
		{
			CustomTestContext.WriteLine("Кликнуть на дропдаун исходного языка, чтобы появился выпадающий список.");
			SourceLangDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбор исходного языка
		/// </summary>
		/// <param name="lang">исходный язык</param>
		public NewProjectSettingsPage SelectSourceLanguageInList(Language lang)
		{
			CustomTestContext.WriteLine("Выбрать исходный язык {0}", lang);
			SourceLangItem = Driver.SetDynamicValue(How.XPath, SOURCE_LANG_ITEM, lang.ToString());
			SourceLangItem.JavaScriptClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Add, чтоб открыть дропдаун языка перевода
		/// </summary>
		public NewProjectSettingsPage ClickAddTargetLanguageButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Add, чтоб открыть дропдаун языка перевода.");
			AddTargetLanguageButton.Click();

			return GetPage();
		}
		
		/// <summary>
		/// Нажать на название дропдаун 'Target Language', чтоб закрыть дропдаун
		/// </summary>
		public NewProjectSettingsPage ClickTargetLanguageDropdownHeader()
		{
			CustomTestContext.WriteLine("Нажать на название дропдаун 'Target Language', чтоб закрыть дропдаун.");
			TargetLanguageDropdownHeader.Click();

			return GetPage();
		}

		/// <summary>
		/// Снять выделение для всех таргет языков
		/// </summary>
		public NewProjectSettingsPage DeselectAllTargetLanguagesInList()
		{
			CustomTestContext.WriteLine("Снять выделение для всех таргет языков.");
			var targetLangItemsSelected = Driver.GetElementList(By.XPath(TARGET_LANG_ITEMS_SELECTED));

			foreach (var e in targetLangItemsSelected)
			{
				e.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык перевода из списка
		/// </summary>
		/// <param name="lang">язык перевода</param>
		public NewProjectSettingsPage SelectTargetLanguageInList(Language lang)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода {0} из списка.", lang);
			TargetLangItem = Driver.SetDynamicValue(How.XPath, TARGET_LANG_ITEM, lang.ToString());
			TargetLangItem.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу 'Use Machine Translation'
		/// </summary>
		public NewProjectSettingsPage ClickMachineTranslationCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу 'Use Machine Translation'.");
			UseMachineTranslationCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку Workflow.
		/// </summary>
		public NewProjectWorkflowPage ClickWorkflowButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Workflow.");
			WorkflowButton.Click();

			return new NewProjectWorkflowPage(Driver).GetPage();
		}

		/// <summary>
		/// Кликнуть на поле для ввода даты, чтобы появился всплывающий календарь
		/// </summary>
		public NewProjectSettingsPage ClickDeadlineDateInput()
		{
			CustomTestContext.WriteLine("Кликнуть на поле для ввода даты, чтобы появился всплывающий календарь.");
			DeadlineDateInput.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать текущую дату дедлайна в календаре
		/// </summary>
		public NewProjectSettingsPage ClickDeadlineDateCurrent()
		{
			CustomTestContext.WriteLine("Выбрать текущую дату дедлайна в календаре.");
			DeadlineDateCurrent.Click();

			return GetPage();
		}

		/// <summary>
		/// Перейти на следующий месяц календаря
		/// </summary>		
		public NewProjectSettingsPage ClickNextMonth()
		{
			CustomTestContext.WriteLine("Перейти на следующий месяц календаря.");
			DeadlineDateNextMonth.Click();

			return GetPage();
		}

		/// <summary>
		/// Перейти на предыдущий месяц календаря
		/// </summary>		
		public NewProjectSettingsPage ClickPreviousMonth()
		{
			CustomTestContext.WriteLine("Перейти на предыдущий месяц календаря.");
			DeadlineDatePrevMonth.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по произвольной дате в календаре
		/// </summary>		
		public NewProjectSettingsPage ClickDeadlineSomeDate()
		{
			CustomTestContext.WriteLine("Кликнуть по произвольной дате в календаре.");
			DeadlineDate.Click();

			return GetPage();
		}

		/// <summary>
		/// Вписать дату дэдлайна
		/// </summary>
		/// <param name="date">дата</param>
		public NewProjectSettingsPage FillDeadlineDate(string date)
		{
			CustomTestContext.WriteLine("Вписать дату дэдлайна: {0}", date);
			DeadlineDateInput.SetText(date, date.Replace(" ", ""));

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Create Project'.
		/// </summary>
		public ProjectsPage ClickCreateProjectButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Create Project'.");
			CreateProjectButton.Click();

			return new ProjectsPage(Driver).GetPage();
			return GetPage();
		}

		/// <summary>
		/// Раскрыть дополнительные настройки, нажав переключатель
		/// </summary>
		public NewProjectSettingsPage ExpandAdvancedSettings()
		{
			CustomTestContext.WriteLine("Раскрыть дополнительные настройки, нажав переключатель");
			if (!IsAdvancedSettingsSectionDisplayed())
			{
				AdvancedSwitch.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Select' в расширенных настройках проекта
		/// </summary>
		public NewProjectSetUpTMDialog ClickSelectTmButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Select' в расширенных настройках проекта");
			SelectTmButton.Click();

			return new NewProjectSetUpTMDialog(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать дату дэдлайна
		/// </summary>
		/// <param name="deadline">тип дэдлайна</param>
		/// <param name="date">дата дэдлайна. Используется только с типом 'FillDeadlineDate'</param>
		public NewProjectSettingsPage SetDeadline(Deadline deadline, string date = null)
		{
			CustomTestContext.WriteLine("Выбрать дату дэдлайна.");
			ClickDeadlineDateInput();

			if (!IsCalendarDisplayed())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не отображается календарь, нельзя выбрать дату дедлайна");
			}

			switch (deadline)
			{
				case Deadline.CurrentDate:
					ClickDeadlineDateCurrent();
					break;

				case Deadline.NextMonth:
					ClickNextMonth();
					ClickDeadlineSomeDate();
					break;

				case Deadline.PreviousMonth:
					ClickPreviousMonth();
					ClickDeadlineSomeDate();
					break;

				case Deadline.FillDeadlineDate:
					FillDeadlineDate(date);
					break;

				default:
					throw new Exception(string.Format(
						"Передан аргумент, который не предусмотрен! Значение аргумента:'{0}'", deadline));
			}

			if (!IsDeadlineDateSelected())
			{
				throw new InvalidElementStateException("Произошла ошибка: \nдата дедлайна не выбрана");
			}

			return GetPage();
		}

		/// <summary>
		/// Выбрать исходный язык
		/// </summary>
		/// <param name="sourceLanguage">исходный язык</param>
		public NewProjectSettingsPage SetSourceLanguage(Language sourceLanguage = Language.English)
		{
			ClickSourceLangDropdown();
			SelectSourceLanguageInList(sourceLanguage);

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		/// <param name="targetLanguage">язык перевода</param>
		public NewProjectSettingsPage SetTargetLanguage(Language targetLanguage = Language.Russian)
		{
			ClickAddTargetLanguageButton();
			DeselectAllTargetLanguagesInList();
			SelectTargetLanguageInList(targetLanguage);
			ClickTargetLanguageDropdownHeader();

			return GetPage();
		}

		/// <summary>
		/// Заполняем основную информацию о проекте
		/// </summary>
		public NewProjectSettingsPage FillGeneralProjectInformation(
			string projectName,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			Deadline deadline = Deadline.CurrentDate,
			string date = null,
			bool useMT = false)
		{
			FillProjectName(projectName);
			SetDeadline(deadline, date);
			SetSourceLanguage(sourceLanguage);
			SetTargetLanguage(targetLanguage);

			if (useMT ^ IsMachineTranslationCheckboxSelected())
			{
				ClickMachineTranslationCheckbox();
			}

			return GetPage();
		}

		/// <summary>
		/// Переименовать глоссарий
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <param name="glossaryNumber">номер глоссария</param>
		public NewProjectEditGlossaryDialog OpenEditGlossaryDialog(int glossaryNumber = 1)
		{
			HoverGlossaryRow(glossaryNumber);
			ClickEditGlossaryButton(glossaryNumber);

			return new NewProjectEditGlossaryDialog(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что секция расширенных настроек открыта
		/// </summary>
		public bool IsAdvancedSettingsSectionDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что секция расширенных настроек открыта.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ADVANCED_SETTINGS_SECTION));
		}

		/// <summary>
		/// Проверить, открылась ли страница настроек создаваемого проекта
		/// </summary>
		public bool IsNewProjectSettingsPageOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылась ли страница настроек создаваемого проекта");

			return Driver.WaitUntilElementIsDisplay(By.XPath(PROJECT_NAME_INPUT));
		}

		/// <summary>
		/// Проверить, появился ли календарь
		/// </summary>
		public bool IsCalendarDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появился ли календарь");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DEADLINE_DATE_CURRENT));
		}

		/// <summary>
		/// Проверить, что дата дэдлайна выбрана
		/// </summary>
		public bool IsDeadlineDateSelected()
		{
			CustomTestContext.WriteLine("Проверить, что дата дэдлайна выбрана.");
			if (string.IsNullOrEmpty(DeadlineDateInput.GetAttribute("value")))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Проверить, выбран ли чекбокс 'Use Machine Translation'
		/// </summary>
		public bool IsMachineTranslationCheckboxSelected()
		{
			CustomTestContext.WriteLine("Проверить, выбран ли чекбокс 'Use Machine Translation'");

			return UseMachineTranslationCheckbox.Selected;
		}

		/// <summary>
		/// Проверить, есть ли ошибка совпадения source и target языков.
		/// </summary>
		public bool IsDuplicateLanguageErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, есть ли ошибка совпадения source и target языков");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_DUPLICATE_LANG));
		}

		/// <summary>
		/// Проверить, что есть ошибка существующего имени
		/// </summary>
		public bool IsDuplicateNameErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что есть ошибка существующего имени");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_NAME_EXISTS));
		}

		/// <summary>
		/// Проверить, что поле 'Название проекта' выделенно ошибкой
		/// </summary>
		public bool IsNameInputValidationMarkerDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что поле 'Название проекта' выделенно ошибкой.");

			return ProjectNameInput.GetElementAttribute("class").Contains("error");
		}

		/// <summary>
		/// Проверить, есть ли ошибка недопустимых символов в имени
		/// </summary>
		public bool IsForbiddenSymbolsInNameErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, есть ли ошибка недопустимых символов в имени");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_FORBIDDEN_SYMBOLS_NAME));
		}

		/// <summary>
		/// Проверить, есть ли ошибка о пустом имени проекта
		/// </summary>
		public bool IsEmptyNameErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, есть ли ошибка о пустом имени проекта");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_NO_NAME));
		}

		/// <summary>
		/// Проверить наличие сообщения о неверном формате даты
		/// </summary>
		public bool IsErrorDeadlineDateMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить наличие сообщения о неверном формате даты");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_DEADLINE_DATE));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = PROJECT_NAME_INPUT)]
		protected IWebElement ProjectNameInput { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_INPUT)]
		protected IWebElement DeadlineDateInput { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_CURRENT)]
		protected IWebElement DeadlineDateCurrent { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_NEXT_MONTH)]
		protected IWebElement DeadlineDateNextMonth { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_PREV_MONTH)]
		protected IWebElement DeadlineDatePrevMonth { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE)]
		protected IWebElement DeadlineDate { get; set; }

		[FindsBy(How = How.XPath, Using = SOURCE_LANG_DROPDOWN)]
		protected IWebElement SourceLangDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_LANG_DROPDOWN)]
		protected IWebElement TargetLanguageDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_TARGET_LANGUAGE_BUTTON)]
		protected IWebElement AddTargetLanguageButton { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_LANGUAGE_DROPDOWN_HEADER)]
		protected IWebElement TargetLanguageDropdownHeader { get; set; }

		[FindsBy(How = How.XPath, Using = USE_MACHINE_TRANSLATION_CHECKBOX)]
		protected IWebElement UseMachineTranslationCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_TM_BUTTON)]
		protected IWebElement SelectTmButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADVANCED_SWITCH)]
		protected IWebElement AdvancedSwitch { get; set; }

		[FindsBy(How = How.XPath, Using = WORKFLOW_BUTTON)]
		protected IWebElement WorkflowButton { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_BUTTON)]
		protected IWebElement CreateProjectButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADVANCED_SETTINGS_SWITCHER)]
		protected IWebElement AdvancedSettingsSwitcher { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARIES_TAB)]
		protected IWebElement GlossariesTab { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_GLOSSARY_BUTTON)]
		protected IWebElement CreateGlossaryButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_GLOSSARY_BUTTON)]
		protected IWebElement EditGlossaryButton { get; set; }

		protected IWebElement SourceLangItem { get; set; }

		protected IWebElement TargetLangItem { get; set; }

		#endregion
		
		#region Описания XPath элементов

		protected const string PROJECT_NAME_INPUT = "//input[@placeholder='Name']";

		protected const string DEADLINE_DATE_CURRENT = "//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//td[contains(@class, 'ui-datepicker-today')]//a";
		protected const string DEADLINE_DATE_INPUT = "//input[contains(@class, 'l-project__date')]";
		protected const string DEADLINE_DATE_NEXT_MONTH = "//div[contains(@id, 'ui-datepicker-div')]//a[contains(@class, 'ui-datepicker-next')]";
		protected const string DEADLINE_DATE_PREV_MONTH = "//div[contains(@id, 'ui-datepicker-div')]//a[contains(@class, 'ui-datepicker-prev')]";
		protected const string DEADLINE_DATE = "//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//tr[1]//td[count(a)!=0][1]";

		protected const string SOURCE_LANG_DROPDOWN = "//div[@class='source_lang']//i";
		protected const string SOURCE_LANG_ITEM = "//div[@class='source_lang']//li[@title = '*#*']";
		protected const string TARGET_LANG_DROPDOWN = "//div[contains(@class,'target_langs')]//i";
		protected const string TARGET_LANG_ITEMS_SELECTED = "//ul//li//input[@checked='checked']";
		protected const string ADD_TARGET_LANGUAGE_BUTTON = "//span[contains(@data-bind, 'openTargetLanguages')]";
		protected const string TARGET_LANGUAGE_DROPDOWN_HEADER = "//div[contains(@class, 'target_langs dropdown')]//p";
		protected const string TARGET_LANG_ITEM = "//div[contains(@class, 'target_langs')]//ul//li[@title = '*#*']";
		protected const string USE_MACHINE_TRANSLATION_CHECKBOX = "//div[contains(@data-bind, 'availableMachineTranslators')]//label//em";
		protected const string WORKFLOW_BUTTON = "//div[@class='btn-icon-wrap']//i[@class='icon-sc-arrow-right']";
		protected const string ADVANCED_SWITCH = "//div[@class='l-switch']//span[@class='mdl-switch__ripple-container mdl-js-ripple-effect mdl-ripple--center']";
		protected const string SELECT_TM_BUTTON = "//div[@class='g-btn g-greenbtn ' and contains(@data-bind, 'addExistingTM')]//a";
		protected const string ERROR_NAME_EXISTS = "//span[@data-message-id='isNameDuplicate']";
		protected const string ERROR_DUPLICATE_LANG = "//span[contains(@data-bind,'targetLang == sourceLanguageId')]";
		protected const string ERROR_FORBIDDEN_SYMBOLS_NAME = "//span[@data-message-id='nameHasInvalidChars']";
		protected const string ERROR_NO_NAME = "//span[@data-message-id='isNameEmpty']";
		protected const string ERROR_DEADLINE_DATE = "//div[@class='proj_deadline pull-right']//span[text()='Specify the deadline in the MM/DD/YYYY format.']";

		protected const string CREATE_PROJECT_BUTTON = "//div[@class='g-btn g-purplebtn g-big-btn icon-btn' and not(contains(@disabled, 'true'))]";

		protected const string ADVANCED_SETTINGS_SECTION = "//div[@class='additional-settings-tabs']";
		protected const string ADVANCED_SETTINGS_SWITCHER = "//label[@for='advancedSettingsSwitch']";
		protected const string GLOSSARIES_TAB = "//ul[contains(@data-bind, 'advancedSettingsTabs')]//li[text()='Glossaries']";
		protected const string CREATE_GLOSSARY_BUTTON = "//div[contains(@data-bind, 'createGlossary')]//a";
		protected const string EDIT_GLOSSARY_BUTTON = "//div[contains(@data-bind, 'switchDetailMode')][*#*]//div[contains(@class, 'right l-settings-icons')]//span[contains(@data-bind, 'edit')]";
		protected const string GLOSSARY_ROW = "//div[contains(@data-bind, 'switchDetailMode')][*#*]";
		
		#endregion
	}
}
