using System;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	class ImportTmxDialog : WorkspacePage, IAbstractPage<ImportTmxDialog>
	{
		public ImportTmxDialog(WebDriver driver) : base(driver)
		{
		}

		public new ImportTmxDialog GetPage()
		{
			var importTmxDialog = new ImportTmxDialog(Driver);
			InitPage(importTmxDialog, Driver);

			return importTmxDialog;
		}

		public new void LoadPage()
		{
			if (IsImportTmxPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка: \nне открылся диалог импорта TMX файла");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести имя файла в окне 'Import TMX files'
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public ImportTmxDialog EnterFileName(string fileName)
		{
			CustomTestContext.WriteLine("Ввести имя файла в окне 'Import TMX files'");

			try
			{
				Driver.ExecuteScript("$(\"input:file\").removeClass(\"g-hidden\").css(\"opacity\", 100)");
				ImportFileInput.SendKeys(fileName);
				//Чтобы не появилось валидационной ошибки, необходимо,
				//помимо загрузки файла, заполнить следующий элемент
				Driver.ExecuteScript(string.Format("document.getElementsByClassName('l-editgloss__filemedia js-filename-block edit').innerHTML='{0}'", Path.GetFileName(fileName)));
			}
			catch (Exception)
			{
				CustomTestContext.WriteLine("Произошла ошибка:\n не удалось изменить параметры элементов.");
				throw;
			}

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Import' в окне 'Import TMX files'
		/// </summary>
		public TranslationMemoriesPage ClickImportButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Import' в окне 'Import TMX files'");
			ImportButton.Click();

			return new TranslationMemoriesPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Import' в окне 'Import TMX files' ожидая сообщение об ошибке
		/// </summary>
		public ImportTmxDialog ClickImportButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Import' в окне 'Import TMX files' ожидая сообщение об ошибке");
			ImportButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Import' в окне 'Import TMX files' ожидая появления диалога подтверждения
		/// </summary>
		public ConfirmReplacementDialog ClickImportButtonExpectingReplacementConfirmation()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Import' в окне 'Import TMX files' ожидая появления диалога подтверждения");
			ImportButton.Click();

			return new ConfirmReplacementDialog(Driver).GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыт ли диалог импорта TMX файлов
		/// </summary>
		public bool IsImportTmxPageOpened()
		{
			CustomTestContext.WriteLine("Проверить, открыт ли диалог импорта TMX файлов");

			return Driver.WaitUntilElementIsDisplay(By.XPath(UPDATE_TM_IMPORT_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = UPDATE_TM_IMPORT_FILE_INPUT)]
		protected IWebElement ImportFileInput { get; set; }

		[FindsBy(How = How.XPath, Using = UPDATE_TM_IMPORT_BUTTON)]
		protected IWebElement ImportButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string UPDATE_TM_IMPORT_FILE_INPUT = "//h2[text()='Import TMX Files']//..//..//input[@name='file']";
		protected const string UPDATE_TM_IMPORT_BUTTON = "(//h2[text()='Import TMX Files']//..//..//input[@value='Import'])[2]";

		#endregion
	}
}
