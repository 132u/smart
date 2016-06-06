using System;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.MachineTranslation
{
	public class FastMTAddFilesPage : WorkspacePage, IAbstractPage<FastMTAddFilesPage>
	{
		public FastMTAddFilesPage(WebDriver driver) : base(driver)
		{
		}

		public new FastMTAddFilesPage LoadPage()
		{
			if (!IsAddFilesPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница 'Machine Translation - загрузка документа'");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Нажать кнопку Add
		/// </summary>
		public FastMTAddFilesSettingsPage ClickAddButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку загрузки файла.");
			AddFileButton.Click();

			return new FastMTAddFilesSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Перейти на вкладку 'Text'
		/// </summary>
		public FastMTTextPage GoToTextTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку 'Text'");
			TextTab.Click();

			return new FastMTTextPage(Driver);
		}

		/// <summary>
		/// Получить процент обработки документа
		/// </summary>
		/// <param name="documentNumber">номер документа (1 - верхний в списке)</param>
		public int GetProgressPercent(int documentNumber = 1)
		{
			CustomTestContext.WriteLine(string.Format(
				"Получить процент обработки файла {0}-го документа", documentNumber));

			FileProgressField = Driver.SetDynamicValue(How.XPath, FILE_PROGRESS_FIELD, documentNumber.ToString());
			var title = FileProgressField.GetAttribute("title");
			title = title.Replace("%", "");

			int percent;
			if (!int.TryParse(title, out percent))
			{
				throw new Exception(
					string.Format("Произошла ошибка:\nне удалось преобразовать процент обработки документа в число: {0}", title));
			}

			return percent;
		}

		/// <summary>
		/// Нажать на кнопку 'Download Translation'
		/// </summary>
		/// <param name="documentNumber">номер документа (1 - верхний в списке)</param>
		public FastMTAddFilesPage ClickDownloadTranslation(int documentNumber = 1)
		{
			CustomTestContext.WriteLine(string.Format(
				"Нажать на кнопку 'Download Translation' для {0}-го документа", documentNumber));
			DownloadTranslationRef = Driver.SetDynamicValue(How.XPath, DOWNLOAD_TRANSLATION_REF, documentNumber.ToString());
			DownloadTranslationRef.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить название документа из списка
		/// </summary>
		/// <param name="documentNumber">номер документа (1 - верхний в списке)</param>
		public string GetDocumentName(int documentNumber = 1)
		{
			CustomTestContext.WriteLine(string.Format("Получить название для {0}-го документа из списка", documentNumber));
			DocumentNameField = Driver.SetDynamicValue(How.XPath, DOCUMENT_NAME, documentNumber.ToString());
			return DocumentNameField.Text;
		}

		/// <summary>
		/// Получить количество документов
		/// </summary>
		public int GetDocumentsCount()
		{
			CustomTestContext.WriteLine("Получить количество документов");
			return Driver.GetElementsCount(By.XPath(DOCUMENT_ROW_BASE));
		}

		/// <summary>
		/// Нажать на строку с документом
		/// </summary>
		/// <param name="documentNumber">номер документа (1 - верхний в списке)</param>
		public FastMTAddFilesPage ClickDocumentRow(int documentNumber = 1)
		{
			CustomTestContext.WriteLine(string.Format("Нажать на строку с {0}-м документом", documentNumber));
			DocumentRow = Driver.SetDynamicValue(How.XPath, DOCUMENT_ROW, documentNumber.ToString());
			DocumentRow.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Download'
		/// </summary>
		/// <param name="documentNumber">номер документа (1 - верхний в списке)</param>
		public FastMTAddFilesPage ClickDownloadFileButton(int documentNumber = 1)
		{
			CustomTestContext.WriteLine(string.Format(
				"Нажать на кнопку 'Download' для {0}-го документа", documentNumber));
			DownloadFileButton = Driver.SetDynamicValue(How.XPath, DOWNLOAD_FILE_BUTTON, documentNumber.ToString());
			DownloadFileButton.Click();

			return LoadPage();
		}
		
		#endregion

		#region Составные методы

		/// <summary>
		/// Скачать документ после перевода
		/// </summary>
		public FastMTAddFilesPage DownloadFileAfterTranslation()
		{
			var percent = 0;

			for (int i = 0; i < 30; ++i)
			{
				percent = GetProgressPercent();

				if (percent == 100)
				{
					DownloadTranslationFile();

					return LoadPage();
				}

				Thread.Sleep(5000);
				RefreshPage<FastMTAddFilesPage>();
			}

			throw new Exception(string.Format("Произошла ошибка:\n документ не перевелся ({0} %)", percent));
		}

		/// <summary>
		/// Скачать перевод документа
		/// </summary>
		/// <param name="documentNumber">номер документа (1 - верхний в списке)</param>
		public FastMTAddFilesPage DownloadTranslationFile(int documentNumber = 1)
		{
			ClickDocumentRow(documentNumber);
			
			if (!WaitDownloadButtonDisplay(documentNumber))
			{
				throw new Exception(string.Format(
					"Произошла ошибка:\n Не удалось дождаться появления кнопки 'Download' {0}-го документа",
					documentNumber));
			}

			ClickDownloadFileButton(documentNumber);
			ClickDownloadTranslation(documentNumber);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница 'Machine Translation - загрузка документа'
		/// </summary>
		public bool IsAddFilesPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SEARCH_BY_NAME_FIELD));
		}

		#endregion

		#region Методы, ожидающие определенного состояния страницы
		
		/// <summary>
		/// Дождаться появления кнопки 'Download'
		/// </summary>
		/// <param name="documentNumber">номер документа (1 - верхний в списке)</param>
		public bool WaitDownloadButtonDisplay(int documentNumber = 1)
		{
			CustomTestContext.WriteLine(string.Format(
				"Дождаться появления кнопки 'Download' для {0}-го документа", documentNumber));

			return Driver.WaitUntilElementIsDisplay(
				Driver.GetValueOfDynamicBy(How.XPath, DOWNLOAD_FILE_BUTTON, documentNumber.ToString()));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddFileButton { get; set; }

		[FindsBy(How = How.XPath, Using = TEXT_TAB)]
		protected IWebElement TextTab { get; set; }

		protected IWebElement FileProgressField { get; set; }

		protected IWebElement DownloadFileButton { get; set; }

		protected IWebElement DownloadTranslationRef { get; set; }

		protected IWebElement DocumentRow { get; set; }

		protected IWebElement DocumentNameField { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string SEARCH_BY_NAME_FIELD = "//div[@class='l-fastmt__b-search']";
		protected const string ADD_BUTTON = "//div[@class='l-fastmt__addfile']";
		protected const string TEXT_TAB = "//a[contains(@data-bind,'textsTab')]";
		protected const string FILE_PROGRESS_FIELD = "//div[@class='l-fastmt__b-day'][*#*]//div[@class='l-fastmt__file-progress']";
		protected const string DOWNLOAD_FILE_BUTTON = "//div[@class='l-fastmt__b-day'][*#*]//i[@title='Download']";
		protected const string DOWNLOAD_TRANSLATION_REF = "//div[@class='l-fastmt__b-day'][*#*]//div[contains(@data-bind, 'downloadDocument(false)')]//a";
		protected const string DOCUMENT_ROW_BASE = "//div[@class='l-fastmt__b-day']";
		protected const string DOCUMENT_ROW = "//div[@class='l-fastmt__b-day'][*#*]//div[@class='l-fastmt__file']";
		protected const string DOCUMENT_NAME = "//div[@class='l-fastmt__b-day'][*#*]//div[@class='l-fastmt__file']//span[contains(@data-bind,'documentName')]";

		#endregion

	}
}
