using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	public class DocumentUploadGeneralInformationDialog : DocumentUploadBaseDialog, IAbstractPage<DocumentUploadGeneralInformationDialog>
	{
		public DocumentUploadGeneralInformationDialog(WebDriver driver) : base(driver)
		{
		}

		public new DocumentUploadGeneralInformationDialog GetPage()
		{
			var documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			InitPage(documentUploadGeneralInformationDialog, Driver);

			return documentUploadGeneralInformationDialog;
		}

		public new void LoadPage()
		{
			if (!IsDocumentUploadGeneralInformationDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог загрузки файла.");
			}
		}

		/// <summary>
		/// Загрузка файла
		/// </summary>
		/// <param name="pathFiles">путь к файлу</param>
		public DocumentUploadGeneralInformationDialog UploadDocument(IList<string> pathFiles)
		{
			CustomTestContext.WriteLine("Загрузить файл {0}", pathFiles);

			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadFileInput);

			foreach (var pathFile in pathFiles)
			{
				UploadFileInput.SendKeys(pathFile);
			}
			
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Next
		/// </summary>
		public DocumentUploadSetUpTMDialog ClickNextButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Next.");
			NextButton.Click();

			return new DocumentUploadSetUpTMDialog(Driver).GetPage();
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

			WaitUntilDialogBackgroundDisappeared();

			return new ProjectsPage(Driver).GetPage();
		}

		/// <summary>
		/// Дождаться, что диалог добавления файла в проект закрылся.
		/// </summary>
		public bool WaitUntilUploadDocumentDialogClosed()
		{
			CustomTestContext.WriteLine("Проверить, что диалог добавления файла в проект закрылся.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(UPLOAD_DOCUMENT_DIALOG));
		}

		/// <summary>
		/// Проверить, что файл загрузился 
		/// </summary>
		/// <param name="file">имя файла (с расширением)</param>
		public bool IsFileUploaded(string file)
		{
			var fileName = Path.GetFileName(file);

			CustomTestContext.WriteLine("Проверить, что файл {0} загрузился", fileName);

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
		public bool IsDocumentUploadGeneralInformationDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_BTN));
		}

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_INPUT)]
		protected IWebElement UploadFileInput { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BUTTON)]
		protected IWebElement NextButton { get; set; }

		protected const string ADD_BTN = ".//div[contains(@class,'js-popup-import-document')][2]//a[contains(@class,'js-add-file')]";
		protected const string UPLOAD_FILE_INPUT = ".//div[contains(@class,'js-popup-import-document')][2]//input[@type = 'file']";
		protected const string UPLOADED_FILE = ".//div[contains(@class,'js-popup-import-document')][2]//li[@class='js-file-list-item']//span[contains(string(), '*#*')]";
		protected const string DUPLICATE_NAME_ERROR = "//div[contains(@class,'js-info-popup')]//span[contains(string(),'The following files have already been added to the project')]";
		protected const string UPLOAD_DOCUMENT_DIALOG = "//div[contains(@class,'js-popup-import-document')][2]";
		protected const string NEXT_BUTTON = "//div[contains(@class, 'js-popup-import-document')][2]//div[contains(@class, 'g-greenbtn js-next')]//a";
	}
}
