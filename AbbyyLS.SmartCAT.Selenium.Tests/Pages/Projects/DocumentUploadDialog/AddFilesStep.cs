using System;
using System.Collections.Generic;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	public class AddFilesStep : DocumentUploadBaseDialog, IAbstractPage<AddFilesStep>
	{
		public AddFilesStep(WebDriver driver) : base(driver)
		{
		}

		public new AddFilesStep GetPage()
		{
			var addFilesStep = new AddFilesStep(Driver);
			InitPage(addFilesStep, Driver);

			return addFilesStep;
		}

		public new void LoadPage()
		{
			if (!IsAddFilesStepOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся шаг загрузки файла.");
			}
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

			return GetPage();
		}

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь до файла</param>
		public DublicateFileErrorDialog SetDublicateFileName(string pathFile)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", pathFile);
			UploadFileInput.SendKeys(pathFile);

			return new DublicateFileErrorDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Next
		/// </summary>
		public SettingsResourcesStep ClickNextBurron()
		{
			CustomTestContext.WriteLine("Нажать кнопку Next.");
			NextButton.Click();

			return new SettingsResourcesStep(Driver).GetPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Загрузка файлов
		/// </summary>
		/// <param name="pathFiles">список путей к файлам</param>
		public AddFilesStep UploadDocument(IList<string> filesPaths)
		{
			foreach (var filePath in filesPaths)
			{
				CustomTestContext.WriteLine("Загрузить файл: {0}.", filePath);
				makeInputDialogVisible();
				SetFileName(filePath);
			}
			
			return GetPage();
		}

		/// <summary>
		/// Загрузка файлов
		/// </summary>
		/// <param name="pathFiles">список путей к файлам</param>
		public DublicateFileErrorDialog UploadDublicateDocument(IList<string> filesPaths)
		{
			foreach (var filePath in filesPaths)
			{
				CustomTestContext.WriteLine("Загрузить файл: {0}.", filePath);
				makeInputDialogVisible();
				SetDublicateFileName(filePath);
			}

			return new DublicateFileErrorDialog(Driver).GetPage();
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

			return new ProjectsPage(Driver).GetPage();
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

			return Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_FILE.Replace("*#*", fileName)));
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

			return GetPage();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_INPUT)]
		protected IWebElement UploadFileInput { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_BUTTON)]
		protected IWebElement SelectButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_FILES_TAB)]
		protected IWebElement AddFilesTab { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string SELECT_BUTTON = "//div[contains(@data-bind, 'addFile')]";
		protected const string ADD_FILES_TAB = "//li[contains(@data-bind, 'filesStep')]";
		protected const string UPLOAD_FILE_INPUT = "//input[contains(@data-bind,'uploadFilesFromFileInput')]";
		protected const string UPLOADED_FILE = "//li[contains(@class, 'docs__item')]//span[contains(text(),'*#*')]";
		protected const string DUPLICATE_NAME_ERROR = "//span[contains(string(),'The following files have already been added to the project')]";

		#endregion
	}
}
