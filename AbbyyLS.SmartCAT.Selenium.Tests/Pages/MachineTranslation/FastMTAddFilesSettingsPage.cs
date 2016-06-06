using System;
using System.Collections.Generic;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.MachineTranslation
{
	public class FastMTAddFilesSettingsPage : FastMTSettingsBasePage<FastMTAddFilesSettingsPage>
	{
		public FastMTAddFilesSettingsPage(WebDriver driver) : base (driver)
		{
		}

		override public FastMTAddFilesSettingsPage LoadPage()
		{
			if (!IsAddFilesSettingsPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылась страница загрузки и настройки файлов для быстрого МТ.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Нажать на кнопку 'Translate'
		/// </summary>
		public FastMTAddFilesPage ClickTranslateButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Translate'");
			TranslateFileButton.Click();

			return new FastMTAddFilesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь до файла</param>
		public FastMTAddFilesSettingsPage SetFileName(string pathFile)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", pathFile);
			UploadDocumentInput.SendKeys(pathFile);

			return new FastMTAddFilesSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Проверить, загрузился ли файл
		/// </summary>
		/// <param name="fileName">название файла</param>
		public bool IsFileUploaded(string fileName)
		{
			CustomTestContext.WriteLine(string.Format("Проверить, загрузился ли файл '{0}'", fileName));
			return Driver.WaitUntilElementIsDisplay(Driver.GetValueOfDynamicBy(How.XPath, UPLOADED_FILE, fileName));
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Загрузка файла
		/// </summary>
		/// <param name="filesPaths">массив путей к файлам</param>
		public FastMTAddFilesSettingsPage UploadDocumentFiles(IList<string> filesPaths)
		{
			foreach (var filePath in filesPaths)
			{
				CustomTestContext.WriteLine("Загрузить файл: {0}.", filePath);
				makeInputDialogVisible();
				SetFileName(filePath);

				var fileName = Path.GetFileName(filePath);
				if (!IsFileUploaded(fileName))
				{
					throw new Exception("Произошла ошибка: документ не загрузился");
				}
			}

			return new FastMTAddFilesSettingsPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница с настройками загрузки файлов
		/// </summary>
		public bool IsAddFilesSettingsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_FILES_TITLE));
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста
		/// </summary>
		private void makeInputDialogVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста");
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadDocumentInput);
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SETTINGS_FIELD)]
		protected IWebElement SettingsField { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATE_FILE_BUTTON)]
		protected IWebElement TranslateFileButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_FILES_TITLE)]
		protected IWebElement AddFilesTitle { get; set; }
		
		[FindsBy(How = How.XPath, Using = UPLOAD_DOCUMENT_INPUT)]
		protected IWebElement UploadDocumentInput { get; set; }

		protected IWebElement UploadedFile;

		#endregion

		#region Описание XPath элементов

		protected const string SETTINGS_FIELD = "//div[contains(@class,'js-fastmachinetranslation-add-files-page')]//div[@class='param-box']";
		protected const string TRANSLATE_FILE_BUTTON = "//div[contains(@data-bind,'translationButtonTitle')]//a";
		protected const string ADD_FILES_TITLE = "//div[@class='g-topbox__header']//span[text()='Add Files']";
		protected const string UPLOAD_DOCUMENT_INPUT = "//input[contains(@data-bind,'uploadFilesFromFileInput')]";
		protected const string UPLOADED_FILE = "//ul[@class='g-docs__list']//li//span[text()='*#*']";

		#endregion
	}
}
