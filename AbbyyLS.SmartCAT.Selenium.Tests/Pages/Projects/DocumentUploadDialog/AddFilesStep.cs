using System;
using System.Collections.Generic;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	public class AddFilesStep : DocumentUploadBaseDialog, IAbstractPage<AddFilesStep>
	{
		public AddFilesStep(WebDriver driver) : base(driver)
		{
		}

		public new AddFilesStep LoadPage()
		{
			if (!IsAddFilesStepOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся шаг загрузки файла.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь до файла</param>
		public AddFilesStep SetFileName(string pathFile)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", pathFile);
			UploadFileInput.SendKeys(pathFile);

			return LoadPage();
		}

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь до файла</param>
		public DublicateFileErrorDialog SetDublicateFileName(string pathFile)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", pathFile);
			UploadFileInput.SendKeys(pathFile);

			return new DublicateFileErrorDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Next
		/// </summary>
		public SettingsResourcesStep ClickNextButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Next.");
			NextButton.ScrollDown();
			NextButton.Click();

			return new SettingsResourcesStep(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку удаления таргет языка
		/// </summary>
		/// <param name="language">язык</param>
		public AddFilesStep ClickDeleteTargetLanguageButton(Language language)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления таргет языка {0}.", language);
			DeleteTargetLanguageButton = Driver.SetDynamicValue(How.XPath, DELETE_TARGET_LANGUAGE_BUTTON, language.ToString());
			DeleteTargetLanguageButton.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Загрузка файлов
		/// </summary>
		/// <param name="filesPaths">список путей к файлам</param>
		public AddFilesStep UploadDocument(IList<string> filesPaths)
		{
			foreach (var filePath in filesPaths)
			{
				CustomTestContext.WriteLine("Загрузить файл: {0}.", filePath);
				makeInputDialogVisible();
				SetFileName(filePath);
			}

			foreach (var filePath in filesPaths)
			{
				if (!IsFileUploaded(filePath))
				{
					throw new Exception("Произошла ошибка: '\nдокумент " + filePath + " не загружен");
				}
			}

			return LoadPage();
		}

		/// <summary>
		/// Загрузка файлов
		/// </summary>
		/// <param name="filesPaths">список путей к файлам</param>
		public DublicateFileErrorDialog UploadDublicateDocument(IList<string> filesPaths)
		{
			foreach (var filePath in filesPaths)
			{
				CustomTestContext.WriteLine("Загрузить файл: {0}.", filePath);
				makeInputDialogVisible();
				SetDublicateFileName(filePath);
			}

			return new DublicateFileErrorDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Finish
		/// </summary>
		public ProjectsPage ClickFihishUploadOnProjectsPage()
		{
			ClickFinish<ProjectsPage>();

			if (!WaitUntilUploadDocumentDialogClosed())
			{
				throw new Exception("Произошла ошибка: \n не удалось дождаться закрытия диалога добавления файла в проект.");
			}

			return new ProjectsPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Дождаться, что диалог добавления файла в проект закрылся.
		/// </summary>
		public bool WaitUntilUploadDocumentDialogClosed()
		{
			CustomTestContext.WriteLine("Проверить, что диалог добавления файла в проект закрылся.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(ADD_FILES_TAB));
		}

		/// <summary>
		/// Проверить, что файл загрузился 
		/// </summary>
		/// <param name="file">имя файла (с расширением)</param>
		public bool IsFileUploaded(string file)
		{
			var fileName = Path.GetFileName(file);

			CustomTestContext.WriteLine("Проверить, что файл {0} загрузился", fileName);
			Driver.SetDynamicValue(How.XPath, UPLOADED_FILE, fileName).Scroll();

			return Driver.WaitUntilElementIsDisappeared(By.XPath(UPLOAD_ICON.Replace("*#*", fileName)))
				&& Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_FILE.Replace("*#*", fileName)));
		}

		/// <summary>
		/// Проверить, что появилась ошибка о том, что в проекте уже есть файл с таким именем.
		/// </summary>
		public bool IsDuplicateDocumentNameErrorExist()
		{
			CustomTestContext.WriteLine("Проверить, что появилась ошибка о том, что в проекте уже есть файл с таким именем.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DUPLICATE_NAME_ERROR));
		}

		/// <summary>
		/// Проверить, открыт ли диалог загрузки файлов
		/// </summary>
		public bool IsAddFilesStepOpened()
		{
			AddFilesTab.Scroll();

			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_FILES_TAB));
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста
		/// </summary>
		private AddFilesStep makeInputDialogVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста");
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadFileInput);

			return LoadPage();
		}

		/// <summary>
		/// Проверить, что все переданные языки есть в списке таргет языков
		/// </summary>
		/// <param name="languages">массив языков</param>
		public bool IsLanguagesExist(Language[] languages)
		{
			CustomTestContext.WriteLine("Проверить, что все переданные языки есть в списке таргет языков.");

			foreach (var language in languages)
			{
				bool added = Driver.WaitUntilElementIsDisplay(By.XPath(ADDED_TARGET_LANGUAGE.Replace("*#*", language.ToString())));

				CustomTestContext.WriteLine("Результат проверки для языка {0}: {1}", language, added);

				if (!added)
				{
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_INPUT)]
		protected IWebElement UploadFileInput { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_BUTTON)]
		protected IWebElement SelectButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_FILES_TAB)]
		protected IWebElement AddFilesTab { get; set; }

		protected IWebElement DeleteTargetLanguageButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string SELECT_BUTTON = "//div[contains(@data-bind, 'addFile')]";
		protected const string ADD_FILES_TAB = "//li[contains(@data-bind, 'filesStep')]";
		protected const string UPLOAD_FILE_INPUT = "//input[contains(@data-bind,'uploadFilesFromFileInput')]";
		protected const string UPLOADED_FILE = "//li[contains(@class, 'docs__item')]//span[contains(text(),'*#*')]";
		protected const string DUPLICATE_NAME_ERROR = "//span[contains(string(),'The following files have already been added to the project')]";
		protected const string UPLOAD_ICON = "//li[contains(@class, 'docs__item')]//span[contains(text(),'*#*')]//preceding-sibling::span[contains(@class, 'icon_file_loading')]";
		protected const string ADDED_TARGET_LANGUAGE = "//div[@data-bind='foreach: selectedOptions']//span[text()='*#*']";
		protected const string DELETE_TARGET_LANGUAGE_BUTTON = "//div[@data-bind='foreach: selectedOptions']//span[text()='*#*']/following-sibling::span/span";

		#endregion
	}
}
