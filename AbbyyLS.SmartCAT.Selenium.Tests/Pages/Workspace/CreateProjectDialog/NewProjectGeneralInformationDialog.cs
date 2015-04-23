using System.Collections.Generic;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.CreateProjectDialog
{
	public class NewProjectGeneralInformationDialog : ProjectsPage, IAbstractPage<NewProjectGeneralInformationDialog>
	{
		
		public new NewProjectGeneralInformationDialog GetPage()
		{
			var newProjectGeneralInformationDialog = new NewProjectGeneralInformationDialog();
			InitPage(newProjectGeneralInformationDialog);
			LoadPage();

			return newProjectGeneralInformationDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.ElementIsDisplayed(By.XPath(ADD_FILE_BTN_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог создания проекта.");
			}
		}

		/// <summary>
		/// Нажать на кнопку "Добавить" (документ)
		/// </summary>
		public NewProjectGeneralInformationDialog ClickAddFileBtn()
		{
			Logger.Debug("Нажать на кнопку 'Добавить' (документ).");
			AddFileBtn.Click();

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
		public NewProjectGeneralInformationDialog SelectSourceLanguage(string lang)
		{
			Logger.Debug("Выбрать исходный язык {0}", lang);
			SourceLangItem = Driver.SetDynamicValue(How.XPath, SOURCE_LANG_ITEM_XPATH, lang);
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
		public NewProjectGeneralInformationDialog SelectTargetLanguage(string lang)
		{
			Logger.Debug("Выбрать язык перевода {0} из списка.", lang);
			TargetLangItem = Driver.SetDynamicValue(How.XPath, TARGET_MULTISELECT_ITEM_XPATH, lang);
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
			ProjectNameInput.SetText(projectName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Далее"
		/// </summary>
		public NewProjectSetUpWorkflowDialog ClickNextBtn()
		{
			Logger.Debug("Нажать кнопку 'Далее'.");
			NextBtn.Click();
			var newProjectSetUpWorkflowDialog = new NewProjectSetUpWorkflowDialog();

			return newProjectSetUpWorkflowDialog.GetPage();
		}

		[FindsBy(How = How.XPath, Using = ADD_FILE_BTN_XPATH)]
		protected IWebElement AddFileBtn { get; set; }

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

		[FindsBy(How = How.XPath, Using = NEXT_BTN_XPATH)]
		protected IWebElement NextBtn { get; set; }

		protected IWebElement SourceLangItem { get; set; }

		protected IWebElement TargetLangItem { get; set; }

		protected IList<IWebElement> TargetLangItemsSelected { get; set; }

		protected const string ADD_FILE_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[contains(@class,'js-files-uploader')]//a";
		protected const string UPLOAD_FILE_INPUT = "//div[contains(@class,'js-popup-create-project')][2]//input[@type = 'file']";
		protected const string UPLOADED_FILE_XPATH = ".//li[@class='js-file-list-item']//span[contains(string(), '*#*')]";
		protected const string DEADLINE_DATE_INPUT_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//input[contains(@class, 'l-project__date')]";
		protected const string DEADLINE_DATE_CURRENT_XPATH = ".//div[contains(@id, 'ui-datepicker-div')]//table[contains(@class, 'ui-datepicker-calendar')]//td[contains(@class, 'ui-datepicker-today')]//a";
		protected const string SOURCE_LANG_DROPDOWN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[select[@id='sourceLanguage']]/span";
		protected const string SOURCE_LANG_ITEM_XPATH = ".//span[contains(@class,'js-dropdown__list')]//span[@title = '*#*']";
		protected const string TARGET_LANG_MULTISELECT_ITEMS_SELECTED_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//li//span[contains(@class,'js-chckbx')]//input[@aria-selected='true']";
		protected const string TARGET_MULTISELECT_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//div[contains(@class,'js-languages-multiselect')]";
		protected const string TARGET_MULTISELECT_ITEM_XPATH = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'js-chckbx')]//input[@title='*#*']";
		protected const string PROJECT_NAME_INPUT_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//input[@name='name']";
		protected const string NEXT_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
	}
}
