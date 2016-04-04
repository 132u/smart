using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class EditTranslationMemoryDialog : ProjectSettingsPage, IAbstractPage<EditTranslationMemoryDialog>
	{
		public EditTranslationMemoryDialog(WebDriver driver) : base(driver)
		{
		}

		public new EditTranslationMemoryDialog LoadPage()
		{
			if (!IsEditTMDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог редактирования памяти перевода.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку загрузки новой памяти перевода.
		/// </summary>
		public NewTranslationMemoryDialog ClickUploadButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку загрузки новой памяти перевода.");
			UploadButton.Click();

			return new NewTranslationMemoryDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения памяти перевода в диалоге редактирования.
		/// </summary>
		public ProjectSettingsPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения памяти перевода в диалоге редактирования.");
			SaveButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		#endregion
		
		#region Составные методы страницы
		
		#endregion
		
		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог редактирования памяти перевода.
		/// </summary>
		public bool IsEditTMDialogOpened()
		{
			Driver.WaitUntilElementIsDisplay(By.XPath(UPLOAD_BUTTON));

			return UploadButton.Displayed;
		}

		/// <summary>
		/// Проверить, что стоит галочка у память перевода.
		/// </summary>
		/// <param name="translationMemory">имя памяти перевода</param>
		public bool IsTranslationMemoryCheckboxChecked(string translationMemory)
		{
			CustomTestContext.WriteLine("Проверить, что стоит галочка у память перевода {0}.", translationMemory);
			TranslationMemoryCheckbox = Driver.SetDynamicValue(
				How.XPath,
				TRANSLATION_MEMORY_CHECKBOX,
				translationMemory);

			return TranslationMemoryCheckbox.GetIsInputChecked();
		}

		#endregion


		#region Вспомогательные методы


		#endregion


		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = UPLOAD_BUTTON)]
		protected IWebElement UploadButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		protected IWebElement TranslationMemoryCheckbox { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string UPLOAD_BUTTON = "//div[contains(@class,'js-popup-tm')][2]//div[contains(@class,'js-tm-upload')]";
		protected const string SAVE_BUTTON = "//div[contains(@class,'js-popup-tm')][2]//div[contains(@class,'js-submit-btn')]";
		protected const string TRANSLATION_MEMORY_CHECKBOX = "//td/p[@title='*#*']//..//preceding-sibling::td//input";

		#endregion
	}
}
