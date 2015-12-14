using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryPropertiesDialog : WorkspacePage, IAbstractPage<GlossaryPropertiesDialog>
	{
		public GlossaryPropertiesDialog(WebDriver driver) : base(driver)
		{
		}

		public new GlossaryPropertiesDialog GetPage()
		{
			var glossaryPropertiesDialog = new GlossaryPropertiesDialog(Driver);
			InitPage(glossaryPropertiesDialog, Driver);

			return glossaryPropertiesDialog;
		}

		public new void LoadPage()
		{
			if (!IsGlossaryPropertiesDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n диалог свойств глоссария не открылся");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести имя глоссария в диалоге свойств глоссария
		/// </summary>
		public GlossaryPropertiesDialog FillGlossaryName(string glossaryName)
		{
			CustomTestContext.WriteLine("Ввести имя глоссария {0} в диалоге свойств глоссария.", glossaryName);
			GlossaryName.SetText(glossaryName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Delete Glossary'
		/// </summary>
		public GlossaryPropertiesDialog ClickDeleteGlossaryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Delete Glossary'.");
			DeleteGlossaryButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Delete для подтверждения удаления глоссария
		/// </summary>
		public GlossariesPage ClickConfirmDeleteGlossaryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Delete для подтверждения удаления глоссария.");
			ConfirmDeleteButton.Click();

			return new GlossariesPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Save 
		/// </summary>
		public GlossaryPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save");
			SaveButton.Click();
			WaitUntilDialogBackgroundDisappeared();

			return new GlossaryPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Save, ожидая сообщение об ошибке
		/// </summary>
		public GlossaryPropertiesDialog ClickSaveButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save, ожидая сообщение об ошибке");
			SaveButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить количество языков в диалоге свойств глоссария
		/// </summary>
		public int LanguagesCount()
		{
			CustomTestContext.WriteLine("Получить количество языков в диалоге свойств глоссария.");

			return Driver.GetElementsCount(By.XPath(LANGUAGE_LIST));
		}

		/// <summary>
		/// Нажать на крестик для удаления языка в диалоге свойств глоссария
		/// </summary>
		/// <param name="languageNumber">номер языка</param>
		public GlossaryPropertiesDialog ClickDeleteLanguageButton(int languageNumber)
		{
			CustomTestContext.WriteLine("Нажать на крестик для удаления {0} языка в диалоге свойств глоссария.", languageNumber);
			var deleteButton = Driver.SetDynamicValue(How.XPath, DELETE_LANGUAGE_BUTTON, languageNumber.ToString());
			deleteButton.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(WARNING_DELETE_LANGUAGE));

			return GetPage();
		}

		/// <summary>
		/// Нажать Cancel в сообщении при удалении языка в диалоге свойств глоссария
		/// </summary>
		public GlossaryPropertiesDialog ClickCancelInDeleteLanguageWarning()
		{
			CustomTestContext.WriteLine("Нажать Cancel в сообщении при удалении языка в диалоге свойств глоссария.");
			CancelLanguageDeleteButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Advanced в диалоге свойств глоссария
		/// </summary>
		public GlossaryStructureDialog ClickAdvancedButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Advanced в диалоге свойств глоссария.");
			AdvancedButton.Click();

			return new GlossaryStructureDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку добавления языка
		/// </summary>
		public GlossaryPropertiesDialog ClickAddLanguageButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку добавления языка.");
			AddLanguageButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на последний дропдаун языка
		/// </summary>
		public GlossaryPropertiesDialog ClickLastLanguageDropdown()
		{
			CustomTestContext.WriteLine("Нажать на последний дропдаун языка.");
			LastLanguageDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык в дропдауне
		/// </summary>
		/// <param name="language">язык</param>
		public GlossaryPropertiesDialog SelectLanguage(Language language)
		{
			CustomTestContext.WriteLine("Выбрать язык {0} в дропдауне.", language);
			var languageId = (int)language;
			var languageOption = Driver.SetDynamicValue(How.XPath, LANGUAGE_OPTION, languageId.ToString());
			languageOption.ScrollAndClick();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Добавить язык
		/// </summary>
		/// <param name="language">язык</param>
		public GlossaryPropertiesDialog AddLangauge(Language language)
		{
			ClickAddLanguageButton();
			ClickLastLanguageDropdown();
			SelectLanguage(language);

			return GetPage();
		}

		/// <summary>
		/// Отменить удаление языка в окне настройки
		/// </summary>
		/// <param name="languagesNumber">порядковый номер языка</param>
		public GlossaryPropertiesDialog CancelDeleteLanguageInPropertiesDialog(int languagesNumber = 2)
		{
			ClickDeleteLanguageButton(languagesNumber);
			ClickCancelInDeleteLanguageWarning();

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, появилось ли сообщение с подтверждением удаления глоссария
		/// </summary>
		public bool IsConfirmDeleteMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение с подтверждением удаления глоссария");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_DELETE_MESSAGE));
		}

		/// <summary>
		/// Проверить, открыт ли диалог настроек глоссария
		/// </summary>
		public bool IsGlossaryPropertiesDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_PROPERTIES_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = DELETE_GLOSSARY_BUTTON)]
		protected IWebElement DeleteGlossaryButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_DELETE_BUTTON)]
		protected IWebElement ConfirmDeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_DELETE_MESSAGE)]
		protected IWebElement ConfirmDeleteMessage { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_LANGUAGE_BUTTON)]
		protected IWebElement DeleteLanguageButton{ get; set; }

		[FindsBy(How = How.XPath, Using = WARNING_DELETE_LANGUAGE)]
		protected IWebElement WarningDeleteLanguage { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_LANGUAGE_DELETE)]
		protected IWebElement CancelLanguageDeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_NAME)]
		protected IWebElement GlossaryName { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADVANCED_BUTTON)]
		protected IWebElement AdvancedButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_LANGUAGE_BUTTON)]
		protected IWebElement AddLanguageButton { get; set; }

		[FindsBy(How = How.XPath, Using = LAST_LANGUAGE_DROPDOWN)]
		protected IWebElement LastLanguageDropdown { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string LANGUAGE_OPTION = "//span[contains(@class, 'js-dropdown__item') and @data-id='*#*']";
		protected const string ADD_LANGUAGE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[contains(@data-bind, 'addLanguage')]";
		protected const string LAST_LANGUAGE_DROPDOWN = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[contains(@data-bind, 'addLanguage')]//preceding-sibling::span[@class='g-iblock l-editgloss__control l-editgloss__lang'][1]//span/span";

		protected const string GLOSSARY_PROPERTIES_DIALOG = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string DELETE_GLOSSARY_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[contains(@data-bind, 'click: deleteGlossary')]";
		protected const string CONFIRM_DELETE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@data-bind, 'click: deleteGlossary')]";
		protected const string CONFIRM_DELETE_MESSAGE = "//div[contains(@class, 'popup-edit-glossary')][2]//p[@data-message-id='confirm-delete-glossary']";
		protected const string LANGUAGE_LIST = "//div[@class='l-editgloss__contrbox'][1]//span[@class='g-iblock l-editgloss__control l-editgloss__lang']";
		protected const string DELETE_LANGUAGE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@class='l-editgloss__contrbox'][1]//span[*#*][contains(@class, 'editgloss__control')]//i[contains(@data-bind, 'deleteLanguage')]";
		protected const string WARNING_DELETE_LANGUAGE = "//div[contains(@class, 'popup-edit-glossary')][2]//p[@data-message-id='language-deleted-warning']";
		protected const string CANCEL_LANGUAGE_DELETE = ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@data-bind, 'click: undoDeleteLanguage')]";
		protected const string GLOSSARY_NAME = ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[@class='l-editgloss__nmtext']";
		protected const string SAVE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[contains(@data-bind, 'click: save')]//a";
		protected const string ADVANCED_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@data-bind,'click: saveAndEditStructure')]";
	
		#endregion
	}
}
