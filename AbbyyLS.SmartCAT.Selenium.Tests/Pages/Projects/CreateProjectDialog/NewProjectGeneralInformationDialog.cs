﻿using System.Collections.Generic;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectGeneralInformationDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectGeneralInformationDialog>
	{
		
		public new NewProjectGeneralInformationDialog GetPage()
		{
			var newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog();
			InitPage(newProjectGeneralInformationDialog);

			return newProjectGeneralInformationDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ADD_FILE_BTN_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог создания проекта.");
			}
		}

		/// <summary>
		/// Нажать на кнопку "Добавить" (документ)
		/// </summary>
		public NewProjectGeneralInformationDialog ClickAddFileButton()
		{
			Logger.Debug("Нажать на кнопку 'Добавить' (документ).");
			AddFileButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Загрузка файла
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public NewProjectGeneralInformationDialog UploadFile(string pathFile)
		{
			Logger.Debug("Загрузить файл: {0}.", pathFile);
			UploadFileInput = Driver.SetDynamicValue(How.XPath, UPLOAD_FILE_INPUT, "");
			Driver.Scripts().ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadFileInput);
			UploadFileInput.SendKeys(pathFile);

			return GetPage();
		}

		/// <summary>
		/// Проверить, загрузился ли файл
		/// </summary>
		/// <param name="fileName">имя файла (с расширением)</param>
		public NewProjectGeneralInformationDialog AssertIfFileUploaded(string fileName)
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_FILE_XPATH.Replace("*#*", fileName))),
				"Произошла ошибка:\n не удалось загрузить файл {0}.", fileName);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на поле для ввода даты, чтобы появился всплывающий календарь
		/// </summary>
		public NewProjectGeneralInformationDialog ClickDeadlineDateInput()
		{
			Logger.Debug("Кликнуть на поле для ввода даты, чтобы появился всплывающий календарь.");
			DeadlineDateInput.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, появился ли календарь
		/// </summary>
		public NewProjectGeneralInformationDialog AssertIsCalendarDisplayed()
		{
			Assert.IsTrue(Driver.ElementIsDisplayed(By.XPath(DEADLINE_DATE_CURRENT_XPATH)),
				"Произошла ошибка:\n не отображается календарь, нельзя выбрать дату дедлайна.");

			return GetPage();
		}

		/// <summary>
		/// Выбрать текущую дату дедлайна в календаре
		/// </summary>
		public NewProjectGeneralInformationDialog ClickDeadlineDateCurrent()
		{
			Logger.Debug("Выбрать текущую дату дедлайна в календаре.");
			DeadlineDateCurrent.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по полю исходного языка, чтобы появился выпадающий список
		/// </summary>
		public NewProjectGeneralInformationDialog ClickSourceLangDropdown()
		{
			Logger.Debug("Кликнуть по полю исходного языка, чтобы появился выпадающий список.");
			SourceLangDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбор исходного языка
		/// </summary>
		/// <param name="lang">исходный язык</param>
		public NewProjectGeneralInformationDialog SelectSourceLanguage(Language lang)
		{
			Logger.Debug("Выбрать исходный язык {0}", lang);
			SourceLangItem = Driver.SetDynamicValue(How.XPath, SOURCE_LANG_ITEM_XPATH, lang.ToString());
			SourceLangItem.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть по полю языка перевода, чтобы выпал появился список
		/// </summary>
		public NewProjectGeneralInformationDialog ClickTargetMultiselect()
		{
			Logger.Debug("Кликнуть по полю языка перевода, чтобы выпал появился список.");
			TargetMultiselect.Click();

			return GetPage();
		}

		/// <summary>
		/// Снять выделение для всех таргет языков
		/// </summary>
		public NewProjectGeneralInformationDialog DeselectAllTargetLanguages()
		{
			Logger.Debug("Снять выделение для всех таргет языков.");
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
			Logger.Debug("Выбрать язык перевода {0} из списка.", lang);
			TargetLangItem = Driver.SetDynamicValue(How.XPath, TARGET_MULTISELECT_ITEM_XPATH, lang.ToString());
			TargetLangItem.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести имя проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public NewProjectGeneralInformationDialog SetProjectName(string projectName)
		{
			Logger.Debug("Ввести имя проекта: {0}.", projectName);
			ProjectNameInput.SetText(projectName, projectName.Length > 100 ? projectName.Substring(0, 100) : projectName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Далее'
		/// </summary>
		public NewProjectSetUpWorkflowDialog ClickNext()
		{
			Logger.Debug("Нажать кнопку 'Далее'.");
			NextButton.Click();

			return new NewProjectSetUpWorkflowDialog().GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Далее', но не инициализировать страницу следующего шага.
		/// </summary>
		public NewProjectGeneralInformationDialog CheckInputs()
		{
			Logger.Debug("Нажать кнопку 'Далее' для проверки валидности полей.");
			NextButton.Click();

			return new NewProjectGeneralInformationDialog().GetPage();
		}

		/// <summary>
		/// Проверить, что есть ошибка существующего имени
		/// </summary>
		public NewProjectGeneralInformationDialog AssertErrorDuplicateName()
		{
			Logger.Trace("Проверить, что есть ошибка существующего имени.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_NAME_EXISTS_XPATH)),
				"Произошла ошибка:\n не появилось сообщение о существующем имени");

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли ошибка совпадения source и target языков.
		/// </summary>
		public NewProjectGeneralInformationDialog AssertDuplicateLanguageError()
		{
			Logger.Trace("Проверить, есть ли ошибка совпадения source и target языков.");

			Assert.True(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_DUPLICATE_LANG_XPATH)),
				"Произошла ошибка:\n не отображается сообщение о том, что source и target языки совпадают.");

			return GetPage();
		}
		
		/// <summary>
		/// Проверить, есть ли ошибка недопустимых символов в имени
		/// </summary>
		public NewProjectGeneralInformationDialog AssertErrorForbiddenSymbols()
		{
			Logger.Trace("Проверить, есть ли ошибка недопустимых символов в имени.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_FORBIDDEN_SYMBOLS_NAME)),
				"Произошла ошибка:\n не появилось сообщение о недопустимых символах в имени");

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли ошибка о пустом имени проекта
		/// </summary>
		public NewProjectGeneralInformationDialog AssertErrorNoName()
		{
			Logger.Trace("Проверить, есть ли ошибка о пустом имени проекта.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_NO_NAME_XPATH)),
				"Произошла ошибка:\n не появилось сообщение о пустом имени проекта.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле 'Название проекта' выделенно ошибкой
		/// </summary>
		public NewProjectGeneralInformationDialog AssertNameInputError()
		{
			Logger.Trace("Проверить, что поле 'Название проекта' выделенно ошибкой.");

			Assert.IsTrue(Driver.FindElement(By.XPath(PROJECT_NAME_INPUT_XPATH)).GetElementAttribute("class").Contains("error"),
				"Произошла ошибка:\n поле Название не отмечено ошибкой");

			return GetPage();
		}

		/// <summary>
		/// Нажать 'Удалить файл'
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public NewProjectGeneralInformationDialog ClickDeleteFile(string fileName)
		{
			Logger.Debug("Нажать 'Удалить файл'.");
			DeleteFileButton = Driver.SetDynamicValue(How.XPath, DELETE_FILE_BTN_XPATH, fileName);
			DeleteFileButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что файл удалён
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public NewProjectGeneralInformationDialog AssertFileDeleted(string fileName)
		{
			Logger.Trace("Проверить, что файл удалён");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(DELETE_FILE_BTN_XPATH.Replace("*#*", fileName))),
				string.Format("Произошла ошибка\n файл {0} не удалился.", fileName));

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = ADD_FILE_BTN_XPATH)]
		protected IWebElement AddFileButton { get; set; }

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_INPUT)]
		protected IWebElement UploadFileInput { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_INPUT_XPATH)]
		protected IWebElement DeadlineDateInput { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_DATE_CURRENT_XPATH)]
		protected IWebElement DeadlineDateCurrent { get; set; }

		[FindsBy(How = How.XPath, Using = SOURCE_LANG_DROPDOWN_XPATH)]
		protected IWebElement SourceLangDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_MULTISELECT_XPATH)]
		protected IWebElement TargetMultiselect { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_NAME_INPUT_XPATH)]
		protected IWebElement ProjectNameInput { get; set; }

		protected IWebElement SourceLangItem { get; set; }

		protected IWebElement TargetLangItem { get; set; }

		protected IWebElement DeleteFileButton { get; set; }

		protected IList<IWebElement> TargetLangItemsSelected { get; set; }

		protected const string UPLOAD_FILE_INPUT = "//div[contains(@class,'js-popup-create-project')][2]//input[@type = 'file']";
		protected const string UPLOADED_FILE_XPATH = ".//li[@class='js-file-list-item']//span[contains(string(), '*#*')]";
		protected const string DEADLINE_DATE_CURRENT_XPATH = ".//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//td[contains(@class, 'ui-datepicker-today')]//a";
		protected const string SOURCE_LANG_ITEM_XPATH = ".//span[contains(@class,'js-dropdown__list')]//span[@title = '*#*']";
		protected const string TARGET_LANG_MULTISELECT_ITEMS_SELECTED_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//li//span[contains(@class,'js-chckbx')]//input[@aria-selected='true']";
		protected const string TARGET_MULTISELECT_ITEM_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'js-chckbx')]//input[@title='*#*']";
		
		protected const string ERROR_DUPLICATE_LANG_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-sourceLanguage-match-targetLanguage')]";
		protected const string ERROR_FORBIDDEN_SYMBOLS_NAME = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-invalid-chars')]";
		protected const string ERROR_NO_NAME_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-required')]";
		protected const string DELETE_FILE_BTN_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//li[contains(@class, 'js-file-list') and contains(string(), '*#*')]//span[contains(@class, 'btn')]//a[contains(@class, 'js-remove-file')]";
		protected const string ADD_FILE_BTN_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-files-uploader')]//a";
		protected const string TARGET_MULTISELECT_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-languages-multiselect')]";
		protected const string SOURCE_LANG_DROPDOWN_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//div[select[@id='sourceLanguage']]/span";
		protected const string DEADLINE_DATE_INPUT_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//input[contains(@class, 'l-project__date')]";
		protected const string PROJECT_NAME_INPUT_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//input[@name='name']";
		protected const string ERROR_NAME_EXISTS_XPATH = "//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-exists')]";
	}
}