using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using NUnit.Framework;

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

		public new NewProjectSettingsPage LoadPage()
		{
			if (!IsNewProjectSettingsPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылась страница настроек создаваемого проекта");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести им проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public NewProjectSettingsPage FillProjectName(string projectName)
		{
			CustomTestContext.WriteLine("Ввести имя проекта: {0}.", projectName);
			ProjectNameInput.SetText(projectName, projectName.Length > 100 ? projectName.Substring(0, 100) : projectName);

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на дропдаун исходного языка, чтобы появился выпадающий список
		/// </summary>
		public NewProjectSettingsPage ClickSourceLangDropdown()
		{
			CustomTestContext.WriteLine("Кликнуть на дропдаун исходного языка, чтобы появился выпадающий список.");
			SourceLangDropdown.Click();

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Add, чтоб открыть дропдаун языка перевода
		/// </summary>
		public NewProjectSettingsPage ClickAddTargetLanguageButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Add, чтоб открыть дропдаун языка перевода.");
			AddTargetLanguageButton.Click();

			return LoadPage();
		}
		
		/// <summary>
		/// Нажать на название дропдаун 'Target Language', чтоб закрыть дропдаун
		/// </summary>
		public NewProjectSettingsPage ClickTargetLanguageDropdownHeader()
		{
			CustomTestContext.WriteLine("Нажать на название дропдаун 'Target Language', чтоб закрыть дропдаун.");
			TargetLanguageDropdownHeader.Click();

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Выбрать язык перевода из списка
		/// </summary>
		/// <param name="lang">язык перевода</param>
		public NewProjectSettingsPage SelectTargetLanguageInList(Language lang)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода {0} из списка.", lang);
			TargetLangItem = Driver.SetDynamicValue(How.XPath, TARGET_LANG_ITEM, lang.ToString());
			TargetLangItem.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу 'Use Machine Translation'
		/// </summary>
		public NewProjectSettingsPage ClickUseMachineTranslationCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу 'Use Machine Translation'.");
			UseMachineTranslationCheckbox.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Next.
		/// </summary>
		public NewProjectWorkflowPage ClickNextButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Next.");
			NextButton.Click();

			return new NewProjectWorkflowPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Next, ожидая ошибку.
		/// </summary>
		public NewProjectSettingsPage ClickNextButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Next, ожидая ошибку.");
			NextButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на поле для ввода даты, чтобы появился всплывающий календарь
		/// </summary>
		public NewProjectSettingsPage OpenCalendar()
		{
			CustomTestContext.WriteLine("Кликнуть на поле для ввода даты, чтобы появился всплывающий календарь.");
			DeadlineDateField.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать текущую дату дедлайна в календаре
		/// </summary>
		public NewProjectSettingsPage ClickDeadlineDateCurrent()
		{
			CustomTestContext.WriteLine("Выбрать текущую дату дедлайна в календаре.");
			DeadlineDateCurrent.Click();

			return LoadPage();
		}

		/// <summary>
		/// Перейти на следующий месяц календаря
		/// </summary>		
		public NewProjectSettingsPage ClickNextMonth()
		{
			CustomTestContext.WriteLine("Перейти на следующий месяц календаря.");
			DeadlineDateNextMonth.Click();

			return LoadPage();
		}

		/// <summary>
		/// Перейти на предыдущий месяц календаря
		/// </summary>		
		public NewProjectSettingsPage ClickPreviousMonth()
		{
			CustomTestContext.WriteLine("Перейти на предыдущий месяц календаря.");
			DeadlineDatePrevMonth.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по произвольной дате в календаре
		/// </summary>		
		public NewProjectSettingsPage ClickDeadlineSomeDate()
		{
			CustomTestContext.WriteLine("Кликнуть по произвольной дате в календаре.");
			DeadlineDate.Click();

			return LoadPage();
		}

		/// <summary>
		/// Вписать дату дэдлайна
		/// </summary>
		/// <param name="date">дата</param>
		public NewProjectSettingsPage FillDeadlineDate(string date)
		{
			CustomTestContext.WriteLine("Вписать дату дэдлайна: {0}", date);
			DeadlineDateInput.SetText(date, date.Replace(" ", ""));

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Create Project'.
		/// </summary>
		public ProjectsPage ClickCreateProjectButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Create Project'.");
			CreateProjectButton.Click();

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перевести память перевода в режим запись.
		/// </summary>
		/// <param name="translationMemory">память перевода</param>
		public NewProjectSettingsPage ClickWriteTMRadioButton(string translationMemory)
		{
			CustomTestContext.WriteLine("Перевести память перевода {0} в режим запись.", translationMemory);
			WriteTranslationMemoryRadioButton = Driver.SetDynamicValue(How.XPath, WRITE_TRANSLATION_MEMORY_RADIO_BUTTON, translationMemory);
			WriteTranslationMemoryRadioButton.ScrollDown();
			WriteTranslationMemoryRadioButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку удаления памяти перевода.
		/// </summary>
		public NewProjectSettingsPage ClickRemoveTranslationMemoryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления памяти перевода.");
			RemoveTranslationMemoryButton.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку создания памяти перевода.
		/// </summary>
		public NewProjectSettingsPage ClickCreateTranslationMemoryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку создания памяти перевода.");
			CreateTranslationMemoryButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Навести курсор на строку памяти перевода.
		/// </summary>
		/// <param name="translationMemory">память перевода</param>
		public NewProjectSettingsPage HoverTranslationMemoryRow(string translationMemory)
		{
			CustomTestContext.WriteLine("Навести курсор на строку памяти перевода. {0}.", translationMemory);
			TranslationMemoryRow = Driver.SetDynamicValue(How.XPath, TRANSLATION_MEMORY_ROW, translationMemory);
			TranslationMemoryRow.HoverElement();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун клиента.
		/// </summary>
		public NewProjectSettingsPage ClickClientDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун клиента.");
			ClientDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать группу проектов.
		/// </summary>
		/// <param name="projectGroup">группа проектов</param>
		public NewProjectSettingsPage ClickProjectGroupOptioin(string projectGroup)
		{
			CustomTestContext.WriteLine("Выбрать группу проектов.");
			ProjectGroupOption = Driver.SetDynamicValue(How.XPath, PROJECT_GROUP_OPTION, projectGroup);
			ProjectGroupOption.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун группы проектов.
		/// </summary>
		public NewProjectSettingsPage ClickProjectGroupDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун группы проектов.");
			ProjectGroupDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать клиента.
		/// </summary>
		/// <param name="client">клиент</param>
		public NewProjectSettingsPage ClickClientOptioin(string client)
		{
			CustomTestContext.WriteLine("Выбрать клиента.");
			ClientOption = Driver.SetDynamicValue(How.XPath, CLIENT_OPTION, client);
			ClientOption.Click();

			return LoadPage();
		}
		
		/// <summary>
		/// Раскрыть дополнительные настройки, нажав переключатель
		/// </summary>
		public AdvancedSettingsSection ExpandAdvancedSettings()
		{
			CustomTestContext.WriteLine("Раскрыть дополнительные настройки, нажав переключатель");
			if (!IsAdvancedSettingsSectionExist())
			{
				AdvancedSwitch.JavaScriptClick();
			}

			return new AdvancedSettingsSection(Driver).LoadPage();
		}
		
		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Удалить память перевода
		/// </summary>
		/// <param name="translationMemory">память перевода</param>
		public NewProjectSettingsPage RemoveTranslationMemory(string translationMemory)
		{
			HoverTranslationMemoryRow(translationMemory);
			ClickRemoveTranslationMemoryButton();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать дату дэдлайна
		/// </summary>
		/// <param name="deadline">тип дэдлайна</param>
		/// <param name="date">дата дэдлайна. Используется только с типом 'FillDeadlineDate'</param>
		public NewProjectSettingsPage SetDeadline(Deadline deadline)
		{
			CustomTestContext.WriteLine("Выбрать дату дэдлайна.");
			OpenCalendar();

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
					
				default:
					throw new Exception(string.Format(
						"Передан аргумент, который не предусмотрен! Значение аргумента:'{0}'", deadline));
			}

			if (!IsDeadlineDateSelected())
			{
				throw new InvalidElementStateException("Произошла ошибка: \nдата дедлайна не выбрана");
			}

			return LoadPage();
		}

		/// <summary>
		/// Выбрать исходный язык
		/// </summary>
		/// <param name="sourceLanguage">исходный язык</param>
		public NewProjectSettingsPage SetSourceLanguage(Language sourceLanguage = Language.English)
		{
			ClickSourceLangDropdown();
			SelectSourceLanguageInList(sourceLanguage);

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Заполняем основную информацию о проекте
		/// </summary>
		public NewProjectSettingsPage FillGeneralProjectInformation(
			string projectName,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			Deadline deadline = Deadline.CurrentDate,
			bool useMachineTranslation = false)
		{
			FillProjectName(projectName);
			SetDeadline(deadline);
			SetSourceLanguage(sourceLanguage);
			SetTargetLanguage(targetLanguage);

			if (useMachineTranslation ^ IsUseMachineTranslationInutSelected())
			{
				ClickUseMachineTranslationCheckbox();
			}

			return LoadPage();
		}

		/// <summary>
		/// Выбрать клиента
		/// </summary>
		/// <param name="client">клиент</param>
		public NewProjectSettingsPage SelectClient(string client)
		{
			ClickClientDropdown();
			ClickClientOptioin(client);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать группу проектов
		/// </summary>
		/// <param name="projectGroup">группа проектов</param>
		public NewProjectSettingsPage SelectProjectGroup(string projectGroup)
		{
			ClickProjectGroupDropdown();
			ClickProjectGroupOptioin(projectGroup);

			return LoadPage();
		}
		
		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что имя проекта совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedProjectName">ожидаемое имя проекта</param>
		public bool IsProjectNameMatchExpected(string expectedProjectName)
		{
			CustomTestContext.WriteLine("Проверить, что имя проекта = '{0}'", expectedProjectName);
			var actualProjectName = ProjectNameInput.GetAttribute("value");

			return actualProjectName == expectedProjectName;
		}

		/// <summary>
		/// Проверить, что в таргет-языке указан правильный язык
		/// </summary>
		public bool IsTargetLanguageMatchExpected(Language expectedLanguage)
		{
			CustomTestContext.WriteLine("Проверить, что в таргет-языке указан {0}", expectedLanguage);
			var selected_target = Driver.SetDynamicValue(How.XPath, TARGET_LANG_SELECTED, expectedLanguage.ToString());

			return selected_target.Displayed;
		}

		/// <summary>
		/// Проверить, что в сорс-языке указан правильный язык
		/// </summary>
		public bool IsSourceLanguageMatchExpected(Language expectedLanguage)
		{
			CustomTestContext.WriteLine("Проверить, что в сорс-языке указан {0}", expectedLanguage);
			var selected_source = Driver.SetDynamicValue(How.XPath, SOURCE_LANG_SELECTED, expectedLanguage.ToString());

			return selected_source.Displayed;
		}

		/// <summary>
		/// Получить значение в поле дэдлайна.
		/// </summary>
		public DateTime? GetDeadlineDate()
		{
			CustomTestContext.WriteLine("Получить значение в поле дэдлайна.");
			var date = DeadlineValue.Text.Split(new[] { ' ' })[0].Trim();

			return DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;
		}

		/// <summary>
		/// Проверить, что секция расширенных настроек открыта
		/// </summary>
		public bool IsAdvancedSettingsSectionExist()
		{
			CustomTestContext.WriteLine("Проверить, что секция расширенных настроек открыта.");

			return Driver.GetIsElementExist(By.XPath(ADVANCED_SETTINGS_SECTION));
		}

		/// <summary>
		/// Проверить, открылась ли страница настроек создаваемого проекта
		/// </summary>
		public bool IsNewProjectSettingsPageOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(PROJECT_NAME_INPUT));
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
			if (string.IsNullOrEmpty(DeadlineSetValue.Text))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Проверить, выбран ли чекбокс 'Use Machine Translation'
		/// </summary>
		public bool IsUseMachineTranslationInutSelected()
		{
			CustomTestContext.WriteLine("Проверить, стоит ли галочка в чекбоксе 'Use Machine Translation'");

			return Driver.GetIsElementExist(By.XPath(USE_MACHINE_TRANSLATION_INPUT)) && UseMachineTranslationInput.Selected;
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

			return Driver.FindElement(By.XPath(ERROR_NO_NAME)).GetAttribute("class").Contains("error");
		}

		/// <summary>
		/// Проверить наличие сообщения о неверном формате даты
		/// </summary>
		public bool IsErrorDeadlineDateMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить наличие сообщения о неверном формате даты");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_DEADLINE_DATE));
		}

		/// <summary>
		/// Проверить наличие памяти перевода.
		/// </summary>
		/// <param name="translationMemory">память перевода</param>
		public bool IsTranslationMemoryDisplayed(string translationMemory)
		{
			CustomTestContext.WriteLine("Проверить наличие памяти перевода {0}.", translationMemory);

			return Driver.GetIsElementExist(By.XPath(TRANSLATION_MEMORY_ROW.Replace("*#*", translationMemory)));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = PROJECT_NAME_INPUT)]
		protected IWebElement ProjectNameInput { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_FIELD)]
		protected IWebElement DeadlineDateField { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_INPUT)]
		protected IWebElement DeadlineDateInput { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_VALUE)]
		protected IWebElement DeadlineValue { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_SET_VALUE)]
		protected IWebElement DeadlineSetValue { get; set; }

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

		[FindsBy(How = How.XPath, Using = USE_MACHINE_TRANSLATION_INPUT)]
		protected IWebElement UseMachineTranslationInput { get; set; }
		
		[FindsBy(How = How.XPath, Using = ADVANCED_SWITCH)]
		protected IWebElement AdvancedSwitch { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON)]
		protected IWebElement NextButton { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_PROJECT_BUTTON)]
		protected IWebElement CreateProjectButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADVANCED_SETTINGS_SWITCHER)]
		protected IWebElement AdvancedSettingsSwitcher { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARIES_TAB)]
		protected IWebElement GlossariesTab { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_MULTISELECT_XPATH)]
		protected IWebElement TargetMultiselect { get; set; }
		
		[FindsBy(How = How.XPath, Using = CLIENT_DROPDOWN)]
		protected IWebElement ClientDropdown { get; set; }
		protected IWebElement ClientOption { get; set; }
		[FindsBy(How = How.XPath, Using = ADD_EXISTING_TM)]
		protected IWebElement AddExistingTMButton { get; set; }

		[FindsBy(How = How.XPath, Using = REMOVE_TRANSLATION_MEMORY_BUTTON)]
		protected IWebElement RemoveTranslationMemoryButton { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_TRANSLATION_MEMORY_BUTTON)]
		protected IWebElement CreateTranslationMemoryButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = PROJECT_GROUP_DROPDOWN)]
		protected IWebElement ProjectGroupDropdown { get; set; }
		protected IWebElement ProjectGroupOption { get; set; }

		protected IWebElement WriteTranslationMemoryRadioButton { get; set; }
		protected IWebElement TranslationMemoryRow { get; set; }
		protected IWebElement SourceLangItem { get; set; }

		protected IWebElement TargetLangItem { get; set; }

		#endregion
		
		#region Описания XPath элементов

		protected const string PROJECT_NAME_INPUT = "//input[contains(@placeholder, 'name')]";

		protected const string DEADLINE_DATE_CURRENT = "//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//td[contains(@class, 'ui-datepicker-today')]//a";
		protected const string DEADLINE_DATE_INPUT = "//div[contains(@class, 'datetimepicker')]//input";
		protected const string DEADLINE_DATE_NEXT_MONTH = "//div[contains(@id, 'ui-datepicker-div')]//a[contains(@class, 'ui-datepicker-next')]";
		protected const string DEADLINE_DATE_FIELD = "//datetimepicker//span[contains(@data-bind, 'watermark')]";
		protected const string DEADLINE_DATE_PREV_MONTH = "//div[contains(@id, 'ui-datepicker-div')]//a[contains(@class, 'ui-datepicker-prev')]";
		protected const string DEADLINE_DATE = "//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//tr[1]//td[count(a)!=0][1]";
		protected const string DEADLINE_VALUE = "//div[contains(@class, 'datetimepicker')]//span//span[contains(@data-bind,'formatDateTime')]";
		protected const string DEADLINE_SET_VALUE = "//datetimepicker//span[contains(@class, 'datetimepicker__date')]";
		protected const string SOURCE_LANG_DROPDOWN = "//div[@class='source_lang']//i";
		protected const string SOURCE_LANG_SELECTED = "//div[@class='source_lang']//input[@title='*#*']";
		protected const string SOURCE_LANG_ITEM = "//div[@class='source_lang']//li[@title = '*#*']";
		protected const string TARGET_LANG_DROPDOWN = "//div[contains(@class,'target_langs')]//i";
		protected const string TARGET_LANG_ITEMS_SELECTED = "//ul//li//input[@checked='checked']";
		protected const string TARGET_MULTISELECT_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-languages-multiselect')]";
		protected const string TARGET_LANG_SELECTED = "//div[contains(@class, 'target_langs')]//span[contains(@class, 'ui-multiselect-value') and text()='*#*']";
		protected const string ADD_TARGET_LANGUAGE_BUTTON = "//div[contains(@data-bind, 'allTargetLanguagesList')]//textarea";
		protected const string TARGET_LANGUAGE_DROPDOWN_HEADER = "//div[contains(@class, 'target_langs dropdown')]//p";
		protected const string TARGET_LANG_ITEM = "//div[contains(@class, 'target_langs')]//ul//li[@title = '*#*']";
		protected const string USE_MACHINE_TRANSLATION_CHECKBOX = "//div[contains(@data-bind, 'availableMachineTranslators')]//label//em";
		protected const string NEXT_BUTTON = "//div[contains(@data-bind, 'click: $parent.completeStep')]//i[@class='icon-sc-arrow-right']";
		protected const string USE_MACHINE_TRANSLATION_INPUT = "//div[contains(@data-bind, 'availableMachineTranslators')]//label//input";
		protected const string ADVANCED_SWITCH = "//div[@class='l-switch']//span[@class='mdl-switch__ripple-container mdl-js-ripple-effect mdl-ripple--center']";
		protected const string ERROR_NAME_EXISTS = "//span[@data-message-id='isNameDuplicate']";
		protected const string ERROR_DUPLICATE_LANG = "//span[contains(text(), 'The target language must be different from the source language.')]";
		protected const string ERROR_FORBIDDEN_SYMBOLS_NAME = "//span[@data-message-id='nameHasInvalidChars']";
		protected const string ERROR_NO_NAME = "//input[@placeholder='Enter the project name']";
		protected const string ERROR_DEADLINE_DATE = "//div[@class='proj_deadline pull-right']//span[text()='Specify the deadline in the MM/DD/YYYY format.']";

		protected const string CREATE_PROJECT_BUTTON = "//div[contains(@data-bind,'complete')]//a[contains(@class, 'g-greenbtn')]";

		protected const string ADVANCED_SETTINGS_SECTION = "//div[@class='additional-settings-tabs']";
		protected const string ADVANCED_SETTINGS_SWITCHER = "//label[@for='advancedSettingsSwitch']";
		protected const string GLOSSARIES_TAB = "//ul[contains(@data-bind, 'advancedSettingsTabs')]//li[text()='Glossaries']";

		protected const string CLIENT_DROPDOWN = "//input[contains(@placeholder, 'Select a client')]";
		protected const string CLIENT_OPTION = "//li[@title='*#*']";

		protected const string ADD_EXISTING_TM = "//div[contains(@data-bind, 'addExistingTM')]";
		protected const string WRITE_TRANSLATION_MEMORY_RADIO_BUTTON = "//div[contains(text(),'*#*')]/../../../..//div[contains(@class, 'radiobtn-wrap')]";
		protected const string TRANSLATION_MEMORY_ROW = "//div[contains(text(),'*#*')]";
		protected const string REMOVE_TRANSLATION_MEMORY_BUTTON = "//span[contains(@data-bind,'removeProjectTM')]";
		protected const string CREATE_TRANSLATION_MEMORY_BUTTON = "//div[contains(@data-bind, 'createTM')]//a";

		protected const string PROJECT_GROUP_DROPDOWN = "//div[contains(@data-bind, 'domainId')]//input[contains(@placeholder, 'Select a project group')]";
		protected const string PROJECT_GROUP_OPTION = "//li[@title='*#*']";
		protected const string TM_SOURCE_LANGUAGE = "(//div[contains(text(),'*#*')]/../../..//span[contains(@data-bind, 'sourceLanguageId')])[1]";

		#endregion
	}
}
