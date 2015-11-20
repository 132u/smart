using System;
using System.Collections.Generic;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectGeneralInformationDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectGeneralInformationDialog>
	{
		public NewProjectGeneralInformationDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectGeneralInformationDialog GetPage()
		{
			var newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog(Driver);
			InitPage(newProjectGeneralInformationDialog, Driver);

			return newProjectGeneralInformationDialog;
		}

		public new void LoadPage()
		{
			if (!IsNewProjectGeneralInformationDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог создания проекта");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку "Добавить" (документ)
		/// </summary>
		public NewProjectGeneralInformationDialog ClickAddFileButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Добавить' (документ).");
			AddFileButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Загрузка файла
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public NewProjectGeneralInformationDialog UploadFile(string pathFile)
		{
			CustomTestContext.WriteLine("Загрузить файл: {0}.", pathFile);
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadFileInput);
			UploadFileInput.SendKeys(pathFile);

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_FILE_XPATH.Replace(
				"*#*", 
				Path.GetFileName(pathFile))), 90))
			{
				CustomTestContext.WriteLine("Первая попытка добавить файл была неудачной. Попробовать ещё раз.");

				Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
					"arguments[0].style[\"visibility\"] = \"visible\";",
					UploadFileInput);
				UploadFileInput.SendKeys(pathFile);

				if (fileAlreadyAddedErrorDisplayed())
				{
					CloseButtonErrorMessage.Click();
				}
			}

			return GetPage();
		}

		/// <summary>
		/// Загрузка файла с ожиданием ошибки
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public NewProjectGeneralInformationDialog UploadFileExpectingError(string pathFile)
		{
			CustomTestContext.WriteLine("Загрузить файл: {0}.", pathFile);
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadFileInput);
			UploadFileInput.SendKeys(pathFile);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по полю исходного языка, чтобы появился выпадающий список
		/// </summary>
		public NewProjectGeneralInformationDialog ClickSourceLangDropdown()
		{
			CustomTestContext.WriteLine("Кликнуть по полю исходного языка, чтобы появился выпадающий список.");
			SourceLangDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбор исходного языка
		/// </summary>
		/// <param name="lang">исходный язык</param>
		public NewProjectGeneralInformationDialog SelectSourceLanguage(Language lang)
		{
			CustomTestContext.WriteLine("Выбрать исходный язык {0}", lang);
			SourceLangItem = Driver.SetDynamicValue(How.XPath, SOURCE_LANG_ITEM_XPATH, lang.ToString());
			SourceLangItem.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по полю языка перевода, чтобы выпал появился список
		/// </summary>
		public NewProjectGeneralInformationDialog ClickTargetMultiselect()
		{
			CustomTestContext.WriteLine("Кликнуть по полю языка перевода, чтобы выпал появился список.");
			TargetMultiselect.Click();

			return GetPage();
		}

		/// <summary>
		/// Снять выделение для всех таргет языков
		/// </summary>
		public NewProjectGeneralInformationDialog DeselectAllTargetLanguages()
		{
			CustomTestContext.WriteLine("Снять выделение для всех таргет языков.");
			TargetLangItemsSelected = Driver.GetElementList(By.XPath(TARGET_LANG_MULTISELECT_ITEMS_SELECTED_XPATH));

			foreach (var e in TargetLangItemsSelected)
			{
				e.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык перевода из списка
		/// </summary>
		/// <param name="lang">язык перевода</param>
		public NewProjectGeneralInformationDialog SelectTargetLanguage(Language lang)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода {0} из списка.", lang);
			TargetLangItem = Driver.SetDynamicValue(How.XPath, TARGET_MULTISELECT_ITEM_XPATH, lang.ToString());
			TargetLangItem.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести имя проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public NewProjectGeneralInformationDialog FillProjectName(string projectName)
		{
			CustomTestContext.WriteLine("Ввести имя проекта: {0}.", projectName);
			ProjectNameInput.SetText(projectName, projectName.Length > 100 ? projectName.Substring(0, 100) : projectName);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по чекбоксу 'Use Machine Translation'
		/// </summary>
		public NewProjectGeneralInformationDialog ClickMachineTranslationCheckbox()
		{
			CustomTestContext.WriteLine("Кликнуть по чекбоксу 'Use Machine Translation'.");

			UseMachineTranslationCheckbox.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать дату дэдлайна
		/// </summary>
		/// <param name="deadline">тип дэдлайна</param>
		/// <param name="date">дата дэдлайна. Используется только с типом 'FillDeadlineDate'</param>
		public NewProjectGeneralInformationDialog SetDeadline(Deadline deadline, string date = null)
		{
			CustomTestContext.WriteLine("Выбрать дату дэдлайна.");

			clickDeadlineDateInput();

			if (!IsCalendarDisplayed())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не отображается календарь, нельзя выбрать дату дедлайна");
			}

			switch (deadline)
			{
 				case Deadline.CurrentDate:
					clickDeadlineDateCurrent();
					break;

				case Deadline.NextMonth:
					clickNextMonth();
					clickDeadlineSomeDate();
					break;

				case Deadline.PreviousMonth:
					clickPreviousMonth();
					clickDeadlineSomeDate();
					break;

				case Deadline.FillDeadlineDate:
					fillDeadlineDate(date);
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
		/// Заполняем основную информацию о проекте (1 шаг создания)
		/// </summary>
		public NewProjectGeneralInformationDialog FillGeneralProjectInformation(
			string projectName,
			string filePath = null,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			Deadline deadline = Deadline.CurrentDate,
			string date = null,
			bool useMT = false)
		{
			if (filePath != null)
			{
				UploadFile(filePath);

				if (!IsFileUploaded(Path.GetFileNameWithoutExtension(filePath)))
				{
					throw new Exception("Произошла ошибка: \n не загрузился файл");
				}
			}

			SetDeadline(deadline, date);
			ClickSourceLangDropdown();
			SelectSourceLanguage(sourceLanguage);
			ClickTargetMultiselect();
			DeselectAllTargetLanguages();
			SelectTargetLanguage(targetLanguage);
			ClickTargetMultiselect();
			FillProjectName(projectName);

			if (useMT ^ IsMachineTranslationCheckboxSelected())
			{
				ClickMachineTranslationCheckbox();
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли диалог создания проекта
		/// </summary>
		public bool IsNewProjectGeneralInformationDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылся ли диалог создания проекта");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_FILE_BTN_XPATH));
		}

		/// <summary>
		/// Проверить, что файл загрузился
		/// </summary>
		/// <param name="filePath">имя файла (с расширением)</param>
		public bool IsFileUploaded(string filePath)
		{
			CustomTestContext.WriteLine("Проверить, что документ {0} загрузился.", filePath);
			var fileName = Path.GetFileName(filePath);

			return Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_FILE_XPATH.Replace("*#*", fileName)), timeout: 120);
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
		/// Проверить, что есть ошибка существующего имени
		/// </summary>
		public bool IsDuplicateNameErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что есть ошибка существующего имени");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_NAME_EXISTS_XPATH));
		}

		/// <summary>
		/// Вернуть, выбран ли чекбокс 'Use Machine Translation'
		/// </summary>
		public bool IsMachineTranslationCheckboxSelected()
		{
			CustomTestContext.WriteLine("Вернуть, выбран ли чекбокс 'Use Machine Translation'");

			var machineTranslationCheckboxSelected = UseMachineTranslationCheckbox.Selected;

			CustomTestContext.WriteLine("Чекбокс 'Use Machine Translation' выбран: {0}", machineTranslationCheckboxSelected);

			return machineTranslationCheckboxSelected;
		}

		/// <summary>
		/// Проверить, появился ли календарь
		/// </summary>
		public bool IsCalendarDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появился ли календарь");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DEADLINE_DATE_CURRENT_XPATH));
		}

		#endregion

		#region Вспомогательные методы страницы

		/// <summary>
		/// Кликнуть на поле для ввода даты, чтобы появился всплывающий календарь
		/// </summary>
		private void clickDeadlineDateInput()
		{
			CustomTestContext.WriteLine("Кликнуть на поле для ввода даты, чтобы появился всплывающий календарь.");
			DeadlineDateInput.Click();
		}

		/// <summary>
		/// Выбрать текущую дату дедлайна в календаре
		/// </summary>
		private void clickDeadlineDateCurrent()
		{
			CustomTestContext.WriteLine("Выбрать текущую дату дедлайна в календаре.");
			DeadlineDateCurrent.Click();
		}

		/// <summary>
		/// Перейти на следующий месяц календаря
		/// </summary>		
		private void clickNextMonth()
		{
			CustomTestContext.WriteLine("Перейти на следующий месяц календаря.");
			DeadlineDateNextMonth.Click();
		}

		/// <summary>
		/// Перейти на предыдущий месяц календаря
		/// </summary>		
		private void clickPreviousMonth()
		{
			CustomTestContext.WriteLine("Перейти на предыдущий месяц календаря.");
			DeadlineDatePrevMonth.Click();
		}

		/// <summary>
		/// Кликнуть по произвольной дате в календаре
		/// </summary>		
		private void clickDeadlineSomeDate()
		{
			CustomTestContext.WriteLine("Кликнуть по произвольной дате в календаре.");
			DeadlineDate.Click();
		}

		/// <summary>
		/// Вписать дату дэдлайна
		/// </summary>
		/// <param name="date">дата</param>
		private void fillDeadlineDate(string date)
		{
			CustomTestContext.WriteLine("Вписать дату дэдлайна: {0}", date);
			DeadlineDateInput.SetText(date, date.Replace(" ", ""));
		}

		private bool fileAlreadyAddedErrorDisplayed()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_FILE_ALREADY_ADDED));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ADD_FILE_BTN_XPATH)]
		protected IWebElement AddFileButton { get; set; }

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_INPUT)]
		protected IWebElement UploadFileInput { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_INPUT_XPATH)]
		protected IWebElement DeadlineDateInput { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_CURRENT_XPATH)]
		protected IWebElement DeadlineDateCurrent { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_NEXT_MONTH_XPATH)]
		protected IWebElement DeadlineDateNextMonth { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_XPATH)]
		protected IWebElement DeadlineDate { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_PREV_MONTH_XPATH)]
		protected IWebElement DeadlineDatePrevMonth { get; set; }

		[FindsBy(How = How.XPath, Using = SOURCE_LANG_DROPDOWN_XPATH)]
		protected IWebElement SourceLangDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_MULTISELECT_XPATH)]
		protected IWebElement TargetMultiselect { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_NAME_INPUT_XPATH)]
		protected IWebElement ProjectNameInput { get; set; }

		[FindsBy(How = How.XPath, Using = USE_MACHINE_TRANSLATION_CHECKBOX)]
		protected IWebElement UseMachineTranslationCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_BUTTON_ERROR_MESSAGE)]
		protected IWebElement CloseButtonErrorMessage { get; set; }

		protected IWebElement SourceLangItem { get; set; }

		protected IWebElement TargetLangItem { get; set; }

		protected IList<IWebElement> TargetLangItemsSelected { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string UPLOAD_FILE_INPUT = "//div[contains(@class,'js-popup-create-project')][2]//input[@type = 'file']";
		protected const string UPLOADED_FILE_XPATH = ".//li[@class='js-file-list-item']//span[contains(string(), '*#*')]";
		protected const string SOURCE_LANG_ITEM_XPATH = ".//span[contains(@class,'js-dropdown__list')]//span[@title = '*#*']";
		protected const string TARGET_LANG_MULTISELECT_ITEMS_SELECTED_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//li//span[contains(@class,'js-chckbx')]//input[@aria-selected='true']";
		protected const string TARGET_MULTISELECT_ITEM_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'js-chckbx')]//input[@title='*#*']";
		protected const string ERROR_FORMAT_DOCUMENT_MESSAGE_XPATH = "//div[contains(@class,'js-info-popup')]//span[contains(string(),'These files are not supported')]";

		protected const string ERROR_DUPLICATE_LANG_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-sourceLanguage-match-targetLanguage')]";
		protected const string ERROR_FORBIDDEN_SYMBOLS_NAME = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-invalid-chars')]";
		protected const string ERROR_NO_NAME_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-required')]";
		protected const string ADD_FILE_BTN_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-files-uploader')]//a";
		protected const string TARGET_MULTISELECT_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-languages-multiselect')]";
		protected const string SOURCE_LANG_DROPDOWN_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//div[select[@id='sourceLanguage']]/span";
		protected const string DEADLINE_DATE_INPUT_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//input[contains(@class, 'l-project__date')]";
		protected const string PROJECT_NAME_INPUT_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//input[@name='name']";
		protected const string ERROR_NAME_EXISTS_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-exists')]";
		protected const string ERROR_FILE_ALREADY_ADDED = "//span[contains(text(), 'The following files have already been added to the project')]";
		protected const string CLOSE_BUTTON_ERROR_MESSAGE = "//a[contains(@class, 'js-close-link')]";

		protected const string DEADLINE_DATE_NEXT_MONTH_XPATH = "//div[contains(@id, 'ui-datepicker-div')]//a[contains(@class, 'ui-datepicker-next')]";
		protected const string DEADLINE_DATE_XPATH = "//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//tr[1]//td[count(a)!=0][1]";
		protected const string DEADLINE_DATE_CURRENT_XPATH = ".//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//td[contains(@class, 'ui-datepicker-today')]//a";
		protected const string DEADLINE_DATE_PREV_MONTH_XPATH = "//div[contains(@id, 'ui-datepicker-div')]//a[contains(@class, 'ui-datepicker-prev')]";

		protected const string USE_MACHINE_TRANSLATION_CHECKBOX = "//div[@class='g-popup-bd js-popup-bd js-popup-create-project'][2]//input[@id='mts-checkbox']";

		#endregion
	}
}
