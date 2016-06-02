using System;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryImportDialog : WorkspacePage, IAbstractPage<GlossaryImportDialog>
	{
		public GlossaryImportDialog(WebDriver driver) : base(driver)
		{
		}

		public new GlossaryImportDialog LoadPage()
		{
			if (!IsGlossaryImportDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог импорта глоссария");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Нажать кнопку Import в диалоге импорта глоссария и дождаться успешного завершения
		/// </summary>
		public GlossarySuccessImportDialog ClickImportButtonInImportDialogWaitSuccess()
		{
			clickImport();

			CustomTestContext.WriteLine("Дождаться появления диалога успешного импорта");
			return new GlossarySuccessImportDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Import в диалоге импорта глоссария
		/// </summary>
		public GlossaryPage ClickImportInImportInImportDialog()
		{
			clickImport();

			return new GlossaryPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Replace All' в диалоге импорта глоссария
		/// </summary>
		public GlossaryImportDialog ClickReplaceTermsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Replace All' в диалоге импорта глоссария");
			ReplaceAllButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь до файла</param>
		public GlossaryImportDialog SetFileName(string pathFile)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", pathFile);
			ImportGlossaryInput.SendKeys(pathFile);

			return LoadPage();
		}

		/// <summary>
		/// Проверить, отображается ли ошибка структуры загружаемого файла глоссария
		/// </summary>
		public bool IsErrorReportButtonDisplayed(string glossaryName)
		{
			CustomTestContext.WriteLine("Проверить, что импорт глоссария {0} завершен с ошибками.", glossaryName);
			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_REPORT_BUTTON), timeout: 60);
		}
		#endregion

		#region Составные методы

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public GlossaryImportDialog ImportGlossary(string pathFile)
		{
			makeInputDialogVisible();
			SetFileName(pathFile);
			setFileNameForValidation(pathFile);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы
		
		/// <summary>
		/// Проверить, открыт ли диалог импорта глоссария
		/// </summary>
		public bool IsGlossaryImportDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(IMPORT_BUTTON)) &&
				(Driver.WaitUntilElementIsClickable(By.XPath(REPLACE_ALL_BUTTON)) != null) &&
				(Driver.WaitUntilElementIsClickable(By.XPath(IMPORT_BUTTON)) != null) &&
				Driver.WaitUntilElementIsDisappeared(By.XPath(IMPORT_IN_PROGRESS_MESSAGE), timeout: 20);
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста
		/// </summary>
		private GlossaryImportDialog makeInputDialogVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста");
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" + "arguments[0].style[\"visibility\"] = \"visible\";",
			ImportGlossaryInput);

			return LoadPage();
		}

		/// <summary>
		/// Выполнить скрипт для прохождения валидации импорта
		/// </summary>
		/// <param name="pathFile">путь до файла</param>
		private GlossaryImportDialog setFileNameForValidation(string pathFile)
		{
			CustomTestContext.WriteLine("Выполнить скрипт для прохождения валидации импорта");
			Driver.ExecuteScript("document.getElementsByClassName('g-iblock l-editgloss__filelink js-filename-link')[0].innerHTML = '" + Path.GetFileName(pathFile) + "'");

			return LoadPage();
		}

		private void clickImport()
		{
			CustomTestContext.WriteLine("Нажать кнопку Import в диалоге импорта глоссария.");

			ImportButton = Driver.WaitUntilElementIsClickable(By.XPath(IMPORT_BUTTON));

			if (ImportButton != null)
			{
				ImportButton.Click();
			}
			else
			{
				throw new Exception("Кнопка Import в диалоге импорта глоссария не стала кликабельной");
			}
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ERROR_REPORT_BUTTON)]
		protected IWebElement ErrorReportButton { get; set; }

		[FindsBy(How = How.XPath, Using = IMPORT_GLOSSARY_INPUT)]
		protected IWebElement ImportGlossaryInput { get; set; }

		[FindsBy(How = How.XPath, Using = REPLACE_ALL_BUTTON)]
		protected IWebElement ReplaceAllButton { get; set; }

		protected IWebElement ImportButton { get; set; }

		[FindsBy(How = How.XPath, Using = STRUCTURE_ERROR)]
		protected IWebElement StructureError { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string IMPORT_GLOSSARY_INPUT = "//form[contains(@action,'Enterprise/Glossaries/Import')]//input[contains(@class,'js-submit-input')]";
		protected const string IMPORT_BUTTON = "//div[contains(@class,'js-popup-import')][2]//div[contains(@class,'js-import-button')]";
		protected const string REPLACE_ALL_BUTTON = "//div[contains(@class,'js-popup-import')][2]//input[contains(@name,'needToClear')][@value='true']//following-sibling::em";
		protected const string IMPORT_IN_PROGRESS_MESSAGE = "//div[contains(@class, 'js-please-wait')]";
		protected const string STRUCTURE_ERROR = "//div[contains(@class,'g-popupbox')]//div[@class='l-filtersrc__error']";
		//TODO поменять XPath, когда перведут текст на английский в уведомолениях
		protected const string ERROR_REPORT_BUTTON = "//span[text()='Отчет об ошибках']";
		#endregion
	}
}
