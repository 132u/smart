using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class ImportDialog : ProjectSettingsPage, IAbstractPage<ImportDialog>
	{
		public new ImportDialog GetPage()
		{
			var importDialog = new ImportDialog();
			InitPage(importDialog);

			return importDialog;
		}

		public new void LoadPage()
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
		public ImportDialog UploadFile(string pathFile)
		{
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				UploadFileInput);
			UploadFileInput.SendKeys(pathFile);

			return GetPage();
		}

		/// <summary>
		/// Проверить, загрузился ли файл
		/// </summary>
		/// <param name="fileName">имя файла (с расширением)</param>
		public ImportDialog AssertIfFileUploaded(string fileName)
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(UPLOADED_FILE.Replace("*#*", fileName))),
				"Произошла ошибка:\n не удалось загрузить файл.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Готово"
		/// </summary>
		public ProjectSettingsPage ClickFinishButton()
		{
			Logger.Debug("Нажать кнопку 'Готово'");
			FinishButton.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_INPUT)]
		protected IWebElement UploadFileInput { get; set; }

		[FindsBy(How = How.XPath, Using = FINISH_BTN)]
		protected IWebElement FinishButton { get; set; }

		protected const string ADD_BTN = ".//div[contains(@class,'js-popup-import-document')][2]//a[contains(@class,'js-add-file')]";
		protected const string UPLOAD_FILE_INPUT = ".//div[contains(@class,'js-popup-import-document')][2]//input[@type = 'file']";
		protected const string FINISH_BTN = ".//div[contains(@class,'js-popup-import-document')][2]//span[contains(@class,'js-finish js-upload-btn')]";
		protected const string UPLOADED_FILE = ".//div[contains(@class,'js-popup-import-document')][2]//li[@class='js-file-list-item']//span[contains(string(), '*#*')]";
	}
}
