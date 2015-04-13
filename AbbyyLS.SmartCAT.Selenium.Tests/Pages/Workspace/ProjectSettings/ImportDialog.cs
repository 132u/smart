using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.ProjectSettings
{
	public class ImportDialog : ProjectSettingsPage, IAbstractPage<ImportDialog>
	{
		public new ImportDialog GetPage()
		{
			var importDialog = new ImportDialog();
			InitPage(importDialog);
			LoadPage();

			return importDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(ADD_BTN_XPATH)))
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
			Driver.Scripts().ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
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
			Assert.IsTrue(Driver.WaitUntilElementIsPresent(By.XPath(UPLOADED_FILE_XPATH.Replace("*#*", fileName))),
				"Произошла ошибка:\n не удалось загрузить файл.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Готово"
		/// </summary>
		public ProjectSettingsPage ClickFinishBtn()
		{
			Logger.Debug("Нажать кнопку 'Готово'");
			FinishBtn.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_INPUT_XPATH)]
		protected IWebElement UploadFileInput { get; set; }

		[FindsBy(How = How.XPath, Using = FINISH_BTN_XPATH)]
		protected IWebElement FinishBtn { get; set; }

		protected const string ADD_BTN_XPATH = IMPORT_DIALOG_XPATH + "//a[contains(@class,'js-add-file')]";
		protected const string UPLOAD_FILE_INPUT_XPATH = IMPORT_DIALOG_XPATH + "//input[@type = 'file']";
		protected const string FINISH_BTN_XPATH = IMPORT_DIALOG_XPATH + "//span[contains(@class,'js-finish js-upload-btn')]";
		protected const string UPLOADED_FILE_XPATH = IMPORT_DIALOG_XPATH + "//li[@class='js-file-list-item']//span[contains(string(), '*#*')]";
	}
}
