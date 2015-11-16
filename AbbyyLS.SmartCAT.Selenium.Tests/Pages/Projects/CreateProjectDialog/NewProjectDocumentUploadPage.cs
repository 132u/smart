using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog 
{
	public class NewProjectDocumentUploadPage : WorkspacePage, IAbstractPage<NewProjectDocumentUploadPage>
	{
		public NewProjectDocumentUploadPage(WebDriver driver)
			: base(driver)
		{
		}

		public new NewProjectDocumentUploadPage GetPage()
		{
			var documentUploadPage = new NewProjectDocumentUploadPage(Driver);
			InitPage(documentUploadPage, Driver);

			return documentUploadPage;
		}

		public new void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!IsNewProjectDocumentUploadPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылась страница загрузки документа при создании проекта.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку 'Пропустить' на странице загрузки документа
		/// </summary>
		public NewProjectSettingsPage ClickSkipDocumentUploadButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Пропустить' на странице загрузки документа");
			SkipDocumentUploadButton.Click();

			return new NewProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Загрузка файла
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public NewProjectDocumentUploadPage UploadDocument(string pathFile)
		{
			CustomTestContext.WriteLine("Загрузить файл: {0}.", pathFile);
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadDocumentInput);
			UploadDocumentInput.SendKeys(pathFile);

			if (!IsFileUploaded(pathFile))
			{
				CustomTestContext.WriteLine("Первая попытка добавить файл была неудачной. Попробовать ещё раз.");
				Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
					"arguments[0].style[\"visibility\"] = \"visible\";",
					UploadDocumentInput);
				UploadDocumentInput.SendKeys(pathFile);
			}

			return GetPage();
		}

		/// <summary>
		/// Загрузка файла с ожиданием ошибки
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public NewProjectDocumentUploadPage UploadDocumentExpectingError(string pathFile)
		{
			CustomTestContext.WriteLine("Загрузить файл: {0}.", pathFile);
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadDocumentInput);
			UploadDocumentInput.SendKeys(pathFile);

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Settings' на странице загрузки документа
		/// </summary>
		public NewProjectSettingsPage ClickSettingsButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Settings' на странице загрузки документа");
			SettingsButton.Click();

			return new NewProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Отмены' на странице загрузки документа
		/// </summary>
		public ProjectsPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Отмены' на странице загрузки документа");
			CancelButton.Click();

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Навести курсор на добавленный документ
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public NewProjectDocumentUploadPage HoverCursorToUploadedDocument(string fileName)
		{
			CustomTestContext.WriteLine("Навести курсор на добавленный документ {0}.", fileName);
			UploadedDocument = Driver.SetDynamicValue(How.XPath, UPLOADED_DOCUMENT, fileName);
			UploadedDocument.HoverElement();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку удаления документа
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public NewProjectDocumentUploadPage ClickDeleteDocumentButton(string fileName)
		{
			CustomTestContext.WriteLine("Нажать на кнопку удаления документа {0}.", fileName);
			DeleteFileButton = Driver.SetDynamicValue(How.XPath, DELETE_DOCUMENT_BUTTON, fileName);
			DeleteFileButton.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Удалить документ
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public NewProjectDocumentUploadPage DeleteDocument(string fileName)
		{
			HoverCursorToUploadedDocument(fileName);
			ClickDeleteDocumentButton(fileName);

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница загрузки документа
		/// </summary>
		public bool IsNewProjectDocumentUploadPageOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылась ли страница загрузки документа при создании проекта");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DOCUMENT_UPLOAD_STEP));
		}

		/// <summary>
		/// Проверить, что файл загрузился
		/// </summary>
		/// <param name="filePath">путь до файла</param>
		public bool IsFileUploaded(string filePath)
		{
			CustomTestContext.WriteLine("Проверить, что документ {0} загрузился.", filePath);
			var fileName = Path.GetFileName(filePath);

			return Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_DOCUMENT.Replace("*#*", fileName)), timeout: 120);
		}

		/// <summary>
		/// Проверить, что файл удалён
		/// </summary>
		/// <param name="filePath">путь до файла</param>
		public bool IsFileDeleted(string filePath)
		{
			CustomTestContext.WriteLine("Проверить, что файл удалён");
			var fileName = Path.GetFileName(filePath);

			return Driver.WaitUntilElementIsDisappeared(By.XPath(UPLOADED_DOCUMENT.Replace("*#*", fileName)));
		}

		/// <summary>
		/// Проверить, что возникла ошибка, указывающая на неверный формат загружаемого файла.
		/// </summary>
		public bool IsWrongDocumentFormatErrorDisplayed(string filePath)
		{
			CustomTestContext.WriteLine("Проверить, что возникла ошибка, указывающая на неверный формат загружаемого файла");
			var fileName = Path.GetFileName(filePath);

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_FORMAT_DOCUMENT_MESSAGE.Replace("*#*", fileName)));
		}

		/// <summary>
		/// Проверить, что имя проекта совпадает с ожидаемым
		/// </summary>
		/// <param name="expectedProjectName">ожидаемое имя проекта</param>
		public bool IsProjectNameMatchExpected(string expectedProjectName)
		{
			CustomTestContext.WriteLine("Проверить, что имя проекта = '{0}'", expectedProjectName);
			var actualProjectName = ProjectNameInput.GetAttribute("value");

			return actualProjectName == expectedProjectName;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SKIP_DOCUMENT_UPLOAD_BUTTON)]
		protected IWebElement SkipDocumentUploadButton { get; set; }

		[FindsBy(How = How.XPath, Using = UPLOAD_DOCUMENT_INPUT)]
		protected IWebElement UploadDocumentInput { get; set; }

		[FindsBy(How = How.XPath, Using = SETTINGS_BUTTON)]
		protected IWebElement SettingsButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_NAME_INPUT)]
		protected IWebElement ProjectNameInput { get; set; }

		protected IWebElement UploadedDocument { get; set; }

		protected IWebElement DeleteFileButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DOCUMENT_UPLOAD_STEP = "//ul[@data-bind='foreach: steps']/li[1][contains(@class,'active')]";
		protected const string SKIP_DOCUMENT_UPLOAD_BUTTON = "//span[@class='skip-step']";
		protected const string UPLOADED_DOCUMENT = "//td[@class='filename']//span[text()='*#*']//..//..//i[not(contains(@class,'loading'))]";
		protected const string UPLOAD_DOCUMENT_INPUT = "//input[contains(@data-bind,'uploadFilesFromFileInput')]";
		protected const string SETTINGS_BUTTON = "//div[@class='btn-icon-wrap']//i[@class='icon-sc-arrow-right']";
		protected const string CANCEL_BUTTON = "//a[contains(@data-bind,'cancel')]";
		protected const string DELETE_DOCUMENT_BUTTON = "//td[@class='filename']//span[text()='*#*']//ancestor::table//preceding-sibling::i[contains(@data-bind,'removeDocument')]";
		protected const string DUPLICATE_NAME_ERROR = "//div[contains(@class,'js-info-popup')]//span[contains(string(),'The following files have already been added to the project')]";
		protected const string ERROR_FORMAT_DOCUMENT_MESSAGE = "//td[@class='filename']//span[@class='errorFileName' and text()='*#*']/../span[@class='mess-err' and @data-bind='if: unsupportedFormat' and text()='Unknown format']";
		protected const string PROJECT_NAME_INPUT = "//div[@class='edit_proj_title']//input[@name='name']";

		#endregion
	}
}
