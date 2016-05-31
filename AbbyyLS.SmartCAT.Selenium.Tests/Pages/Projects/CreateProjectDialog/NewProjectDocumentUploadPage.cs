using System;
using System.Collections.Generic;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog 
{
	public class NewProjectDocumentUploadPage : NewProjectCreateBaseDialog, IAbstractPage<NewProjectDocumentUploadPage>
	{
		public NewProjectDocumentUploadPage(WebDriver driver)
			: base(driver)
		{
		}

		public new NewProjectDocumentUploadPage LoadPage()
		{
			if (!IsNewProjectDocumentUploadPageOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылась страница загрузки документа при создании проекта.");
			}

			return this;
		}

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь до файла</param>
		public NewProjectDocumentUploadPage SetFileName(string pathFile)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", pathFile);
			UploadDocumentInput.SendKeys(pathFile);

			return LoadPage();
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку 'Пропустить' на странице загрузки документа
		/// </summary>
		public NewProjectSettingsPage ClickSkipDocumentUploadButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Пропустить' на странице загрузки документа");
			SkipDocumentUploadButton.Click();

			return new NewProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Settings' на странице загрузки документа
		/// </summary>
		public NewProjectSettingsPage ClickSettingsButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Settings' на странице загрузки документа");
			Driver.WaitUntilElementIsDisplay(By.XPath(SETTINGS_BUTTON));
			SettingsButton.Click();

			return new NewProjectSettingsPage(Driver).LoadPage();
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

			return LoadPage();
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

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Загрузка документа
		/// </summary>
		/// <param name="filesPaths">путь к файлу</param>
		/// <param name="errorExpecting">ожидание сообщения об ошибке</param>
		public NewProjectDocumentUploadPage UploadDocumentFiles(
			IList<string> filesPaths, bool errorExpecting = false)
		{
			CustomTestContext.WriteLine("Загрузить файл в проект.");

			foreach (var filePath in filesPaths)
			{
				CustomTestContext.WriteLine("Загрузить файл: {0}.", filePath);
				makeInputDialogVisible();
				SetFileName(filePath);

				if (!errorExpecting)
				{
					if (!IsDocumentFileUploaded(filePath))
					{
						throw new Exception("Произошла ошибка: документ не загрузился");
					}
				}
			}

			return LoadPage();
		}

		/// <summary>
		/// Загрузка tmx
		/// </summary>
		/// <param name="filesPaths">путь к файлу</param>
		public NewProjectDocumentUploadPage UploadTmxFiles(IList<string> filesPaths)
		{
			foreach (var filePath in filesPaths)
			{
				CustomTestContext.WriteLine("Загрузить файл: {0}.", filePath);
				makeInputDialogVisible();
				SetFileName(filePath);

				if (!IsTmxFileUploaded(filePath))
				{
					throw new Exception("Произошла ошибка: tmx не загрузился");
				}
			}

			return LoadPage();
		}

		/// <summary>
		/// Удалить документ
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public NewProjectDocumentUploadPage DeleteDocument(string fileName)
		{
			HoverCursorToUploadedDocument(fileName);
			ClickDeleteDocumentButton(fileName);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница загрузки документа
		/// </summary>
		public bool IsNewProjectDocumentUploadPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DOCUMENT_UPLOAD_STEP));
		}

		/// <summary>
		/// Проверить, что файл загрузился
		/// </summary>
		/// <param name="filePath">путь до файла</param>
		public bool IsDocumentFileUploaded(string filePath)
		{
			var fileName = Path.GetFileNameWithoutExtension(filePath);
			var extension = Path.GetExtension(filePath).ToLower();
			CustomTestContext.WriteLine("Проверить, что документ {0} загрузился.", fileName + extension);

			return Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_DOCUMENT.Replace("*#*", fileName + extension)), timeout: 120);
		}

		/// <summary>
		/// Проверить, что файл tmx загрузился
		/// </summary>
		/// <param name="filePath">путь до файла</param>
		public bool IsTmxFileUploaded(string filePath)
		{
			var fileName = Path.GetFileNameWithoutExtension(filePath);
			var extension = Path.GetExtension(filePath).ToLower();
			CustomTestContext.WriteLine("Проверить, что tmx файл {0} загрузился.", fileName + extension);

			return Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_TMX.Replace("*#*", fileName + extension)), timeout: 120);
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

		/// <summary>
		/// Проверить, что есть анимация кнопки выбора документа
		/// </summary>
		public bool IsSelectDocumentButtonAnimationExist()
		{
			CustomTestContext.WriteLine("Проверить, что есть анимация кнопки выбора документа");
			var animationDuration = SelectDocumentButton.GetCssValue("animation-duration");
			var animationName = SelectDocumentButton.GetCssValue("animation-name");

			return animationDuration.Equals("2.8s", StringComparison.OrdinalIgnoreCase) && animationName.Equals("pulse-btn", StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Проверить, что есть анимация кнопки перехода к настройкам проекта
		/// </summary>
		public bool IsSettingsButtonAnimationExist()
		{
			CustomTestContext.WriteLine("Проверить, что есть анимация кнопки перехода к настройкам проекта");
			var animationDuration = SettingsButton.GetCssValue("animation-duration");
			var animationName = SettingsButton.GetCssValue("animation-name");

			return animationDuration.Equals("2.8s", StringComparison.OrdinalIgnoreCase) && animationName.Equals("pulse-btn", StringComparison.OrdinalIgnoreCase);
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста
		/// </summary>
		private NewProjectDocumentUploadPage makeInputDialogVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста");
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";", 
				UploadDocumentInput);

			return LoadPage();
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

		[FindsBy(How = How.XPath, Using = SELECT_DOCUMENT_BUTTON)]
		protected IWebElement SelectDocumentButton { get; set; }

		protected IWebElement UploadedDocument { get; set; }

		protected IWebElement DeleteFileButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DOCUMENT_UPLOAD_STEP = "//ul[@data-bind='foreach: steps']/li[1][contains(@class,'active')]";
		protected const string SKIP_DOCUMENT_UPLOAD_BUTTON = "//span[@class='skip-step']";
		protected const string UPLOADED_DOCUMENT = "//div[contains(@class,'item')]//span[text()='*#*']//..//..//span[not(contains(@class,'loading')) and contains(@data-bind,'loading')]";
		protected const string UPLOAD_DOCUMENT_INPUT = "//input[contains(@data-bind,'uploadFilesFromFileInput')]";
		protected const string UPLOADED_TMX = "//div[contains(@class,'item')]//span[text()='*#*']//..//..//..//..//..//..//..//li[contains(@data-bind , 'stopBubble: true') and contains(@data-bind, 'selectTranslationMemory')]";
		protected const string SETTINGS_BUTTON = "//div[contains(@class,'first-animated-btn') and not(contains(@disabled, 'true'))]";
		protected const string CANCEL_BUTTON = "//a[contains(@data-bind,'cancel')]";
		protected const string DELETE_DOCUMENT_BUTTON = "//span[text()='*#*']/../..//button[contains(@data-bind,'removeDocument')]";
		protected const string DUPLICATE_NAME_ERROR = "//div[contains(@class,'js-info-popup')]//span[contains(string(),'The following files have already been added to the project')]";
		protected const string ERROR_FORMAT_DOCUMENT_MESSAGE = "//span[contains(text(),'*#*')]/following-sibling::span[contains(@data-bind, 'unsupportedFormat')]";
		protected const string PROJECT_NAME_INPUT = "//div[@class='edit_proj_title']//input[@name='name']";
		protected const string SELECT_DOCUMENT_BUTTON = "//div[@data-bind='click: addFile']";

		#endregion
	}
}
