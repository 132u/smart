using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class NewTranslationMemoryDialog: ProjectSettingsPage, IAbstractPage<NewTranslationMemoryDialog>
	{
		public NewTranslationMemoryDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewTranslationMemoryDialog GetPage()
		{
			var newNewTranslationMemoryDialog = new NewTranslationMemoryDialog(Driver);
			InitPage(newNewTranslationMemoryDialog, Driver);

			return newNewTranslationMemoryDialog;
		}

		public new void LoadPage()
		{
			if (!IsNewTranslationMemoryDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог создания новой памяти перевода.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести имя новой памяти перевода.
		/// </summary>
		/// <param name="translationMemoryName">имя новой памяти перевода</param>
		public NewTranslationMemoryDialog FillName(string translationMemoryName)
		{
			CustomTestContext.WriteLine("Ввести имя {0} новой памяти перевода.", translationMemoryName);
			Name.SetText(translationMemoryName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения новой памяти перевода.
		/// </summary>
		public EditTranslationMemoryDialog ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения новой памяти перевода.");
			SaveButton.Click();

			return new EditTranslationMemoryDialog(Driver).GetPage();
		}

		#endregion
		
		#region Составные методы страницы

		/// <summary>
		/// Создать новую память превода
		/// </summary>
		/// <param name="translationMemoryName">имя памяти перевода</param>
		/// <param name="filePath">путьк файлу</param>
		public EditTranslationMemoryDialog CreateNewTranslationMemory(string translationMemoryName, string filePath)
		{
			UploadTM(filePath);
			FillName(translationMemoryName);
			ClickSaveButton();

			return new EditTranslationMemoryDialog(Driver).GetPage();
		}

		/// <summary>
		/// Загрузить память перевода
		/// </summary>
		/// <param name="filePath">путь к файлу</param>
		public NewTranslationMemoryDialog UploadTM(string filePath)
		{
			CustomTestContext.WriteLine("Загрузить память перевода");
			try
			{
				Driver.ExecuteScript("$(\"input:file\").removeClass(\"g-hidden\").css(\"opacity\", 100)");
				Driver.FindElement(By.XPath(IMPORT_FILE_INPUT)).SendKeys(filePath);
				Driver.ExecuteScript("$(\".js-import-file-form .js-control\").data(\"controller\").filenameLink.text($(\".js-import-file-form .js-control\").data(\"controller\").fileInput.val());");
				Driver.ExecuteScript("$(\".js-import-file-form .js-control\").data(\"controller\").trigger(\"valueChanged\");");
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Произошла ошибка при попытке загрузки файла: {0}", ex.Message));
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог создания новой памяти перевода
		/// </summary>
		public bool IsNewTranslationMemoryDialogOpened()
		{
			Driver.WaitUntilElementIsDisplay(By.XPath(NAME));

			return Name.Displayed;
		}

		#endregion

		#region Вспомогательные методы
		
		#endregion
		
		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = NAME)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = IMPORT_FILE_INPUT)]
		protected IWebElement ImportFileButton { get; set; }

		#endregion
		
		#region Описание XPath элементов

		protected const string NAME = "//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@class,'l-createtm')]";
		protected const string IMPORT_FILE_INPUT = "//div[contains(@class,\"js-popup-create-tm\")][2]//input[@type=\"file\"]";
		protected const string SAVE_BUTTON = "//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@data-bind, 'click: save')]";

		#endregion
	}
}
