using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class ImportTmxDialog : WorkspacePage, IAbstractPage<ImportTmxDialog>
	{
		public ImportTmxDialog(WebDriver driver) : base(driver)
		{
		}

		public new ImportTmxDialog LoadPage()
		{
			if (!IsImportTmxPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка: \nне открылся диалог импорта TMX файла");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="filePath">путь до файла</param>
		public ImportTmxDialog SetFileName(string filePath)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", filePath);
			ImportFileInput.SendKeys(filePath);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Import' в окне 'Import TMX files'
		/// </summary>
		public TranslationMemoriesPage ClickImportButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Import' в окне 'Import TMX files'");
			ImportButton.Click();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Import' в окне 'Import TMX files' ожидая сообщение об ошибке
		/// </summary>
		public ImportTmxDialog ClickImportButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Import' в окне 'Import TMX files' ожидая сообщение об ошибке");
			ImportButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Import' в окне 'Import TMX files' ожидая появления диалога подтверждения
		/// </summary>
		public ConfirmReplacementDialog ClickImportButtonExpectingReplacementConfirmation()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Import' в окне 'Import TMX files' ожидая появления диалога подтверждения");
			ImportButton.Click();

			return new ConfirmReplacementDialog(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Ввести имя файла в окне 'Import TMX files'
		/// </summary>
		/// <param name="filePath">имя файла</param>
		public ImportTmxDialog EnterFileName(string filePath)
		{
			makeInputDialogVisible();
			SetFileName(filePath);
			setFileNameForValidation(filePath);

			return LoadPage();
		}

		/// <summary>
		/// Импортировать Tmx файл
		/// </summary>
		/// <param name="filePath">путь до файла</param>
		public TranslationMemoriesPage ImportTmxFile(string filePath)
		{
			EnterFileName(filePath);
			var translationMemoriesPage = ClickImportButton();

			return translationMemoriesPage.LoadPage();
		}

		/// <summary>
		/// Импортировать Tmx файл, ожидая диалог подтверждения
		/// </summary>
		/// <param name="filePath">путь до файла</param>
		public ConfirmReplacementDialog ImportTmxFileExpectingConfirmation(string filePath)
		{
			EnterFileName(filePath);
			var confirmReplacementDialog = ClickImportButtonExpectingReplacementConfirmation();

			return confirmReplacementDialog.LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыт ли диалог импорта TMX файлов
		/// </summary>
		public bool IsImportTmxPageOpened()
		{
			return Driver.WaitUntilElementIsClickable(By.XPath(CANCEL_BTN)) != null;
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста
		/// </summary>
		private ImportTmxDialog makeInputDialogVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста");
			Driver.ExecuteScript("$(\"input:file\").removeClass(\"g-hidden\").css(\"opacity\", 100)");

			return LoadPage();
		}

		/// <summary>
		/// Выполнить скрипт для прохождения валидации импорта
		/// </summary>
		/// /// <param name="filePath">путь до файла</param>
		private ImportTmxDialog setFileNameForValidation(string filePath)
		{
			CustomTestContext.WriteLine("Выполнить скрипт для прохождения валидации импорта");
			Driver.ExecuteScript(string.Format("document.getElementsByClassName('g-iblock l-editgloss__filelink js-filename-link')[1].innerHTML='{0}'", Path.GetFileName(filePath)));

			return LoadPage();
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
		protected const string CANCEL_BTN = "(//h2[text()='Import TMX Files']//..//..//a[@class='g-popupbox__cancel js-popup-close'])[2]";

		#endregion
	}
}
