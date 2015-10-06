using NUnit.Framework;
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

		public DocumentUploadGeneralInformationDialog GetPage()
		{
			var documentUploadGeneralInformationDialog = new DocumentUploadGeneralInformationDialog(Driver);
			InitPage(documentUploadGeneralInformationDialog, Driver);

			return documentUploadGeneralInformationDialog;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ADD_BTN)))
			{
				Assert.Fail("Произошла ошибка:\n не открылся диалог загрузки файла.");
			}
		}

		/// <summary>
		/// Загрузка файла
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public DocumentUploadGeneralInformationDialog UploadDocument(string pathFile)
		{
			CustomTestContext.WriteLine("Загрузить файл {0}", pathFile);

			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadFileInput);
			UploadFileInput.SendKeys(pathFile);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что файл загрузился 
		/// </summary>
		/// <param name="fileName">имя файла (с расширением)</param>
		public DocumentUploadGeneralInformationDialog AssertFileUploaded(string fileName)
		{
			CustomTestContext.WriteLine("Проверить, что файл {0} загрузился", fileName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_FILE.Replace("*#*", fileName))),
				"Произошла ошибка:\n не удалось загрузить файл.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилась ошибка о том, что в проекте уже есть файл с таким именем.
		/// </summary>
		public DocumentUploadGeneralInformationDialog AssertErrorDuplicateDocumentNameExist()
		{
			CustomTestContext.WriteLine("Проверить, что появилась ошибка о том, что в проекте уже есть файл с таким именем.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(DUPLICATE_NAME_ERROR)),
				"Произошла ошибка:\n нет появилась ошибка о том, что в проекте уже есть файл с таким именем.");

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_INPUT)]
		protected IWebElement UploadFileInput { get; set; }

		protected const string ADD_BTN = ".//div[contains(@class,'js-popup-import-document')][2]//a[contains(@class,'js-add-file')]";
		protected const string UPLOAD_FILE_INPUT = ".//div[contains(@class,'js-popup-import-document')][2]//input[@type = 'file']";
		protected const string UPLOADED_FILE = ".//div[contains(@class,'js-popup-import-document')][2]//li[@class='js-file-list-item']//span[contains(string(), '*#*')]";
		protected const string DUPLICATE_NAME_ERROR = "//div[contains(@class,'js-info-popup')]//span[contains(string(),'The following files have already been added to the project')]";
	}
}
