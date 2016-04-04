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

		public new GlossaryPropertiesDialog LoadPage()
		{
			if (!IsGlossaryPropertiesDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n диалог свойств глоссария не открылся");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести имя глоссария в диалоге свойств глоссария
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public GlossaryPropertiesDialog FillGlossaryName(string glossaryName)
		{
			CustomTestContext.WriteLine("Ввести имя глоссария {0} в диалоге свойств глоссария.", glossaryName);
			GlossaryName.SetText(glossaryName);

			return LoadPage();
		}

		/// <summary>
		/// Ввести комментарий в диалоге свойств глоссария
		/// </summary>
		/// <param name="comment">комментарий</param>
		public GlossaryPropertiesDialog FillGlossaryComment(string comment)
		{
			CustomTestContext.WriteLine("Ввести комментарий в диалоге свойств глоссария.", comment);
			Comment.SetText(comment);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Delete Glossary'
		/// </summary>
		public GlossaryPropertiesDialog ClickDeleteGlossaryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Delete Glossary'.");
			DeleteGlossaryButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Delete для подтверждения удаления глоссария
		/// </summary>
		public GlossariesPage ClickConfirmDeleteGlossaryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Delete для подтверждения удаления глоссария.");
			ConfirmDeleteButton.Click();

			return new GlossariesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Save 
		/// </summary>
		public GlossaryPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save");
			SaveButton.Click();

			return new GlossaryPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Save, ожидая сообщение об ошибке
		/// </summary>
		public GlossaryPropertiesDialog ClickSaveButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save, ожидая сообщение об ошибке");
			SaveButton.Click();

			return LoadPage();
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
		/// Получить текст комментария в диалоге свойств глоссария
		/// </summary>
		public string GetGlossaryComment()
		{
			CustomTestContext.WriteLine("Получить текст комментария в диалоге свойств глоссария.");

			return Comment.GetAttribute("value");
		}

		/// <summary>
		/// Получить название клиента в диалоге свойств глоссария
		/// </summary>
		public string GetGlossaryClient()
		{
			CustomTestContext.WriteLine("Получить название клиента в диалоге свойств глоссария.");
		
			return ClientDropdown.Text;
		}

		/// <summary>
		/// Получить язык
		/// </summary>
		/// <param name="languageNumber">номер языка</param>
		public string GetLanguage(int languageNumber)
		{
			CustomTestContext.WriteLine("Получить язык №{0} в диалоге свойств глоссария.", languageNumber);
			LanguageDropdown = Driver.SetDynamicValue(How.XPath, LANGUAGE_DROPDOWN, languageNumber.ToString());
			
			return LanguageDropdown.Text;
		}

		/// <summary>
		/// Получить название группы поректа в диалоге свойств глоссария
		/// </summary>
		public string GetGlossaryProjectGroup()
		{
			CustomTestContext.WriteLine("Получить название группы поректа в диалоге свойств глоссария.");

			return ProjectGroupDropdown.Text;
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

			return LoadPage();
		}

		/// <summary>
		/// Нажать Cancel в сообщении при удалении языка в диалоге свойств глоссария
		/// </summary>
		public GlossaryPropertiesDialog ClickCancelInDeleteLanguageWarning()
		{
			CustomTestContext.WriteLine("Нажать Cancel в сообщении при удалении языка в диалоге свойств глоссария.");
			CancelLanguageDeleteButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Advanced в диалоге свойств глоссария
		/// </summary>
		public GlossaryStructureDialog ClickAdvancedButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Advanced в диалоге свойств глоссария.");
			AdvancedButton.Click();

			return new GlossaryStructureDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку добавления языка
		/// </summary>
		public GlossaryPropertiesDialog ClickAddLanguageButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку добавления языка.");
			AddLanguageButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на последний дропдаун языка
		/// </summary>
		public GlossaryPropertiesDialog ClickLastLanguageDropdown()
		{
			CustomTestContext.WriteLine("Нажать на последний дропдаун языка.");
			LastLanguageDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун выбора клиента
		/// </summary>
		public GlossaryPropertiesDialog ClickClientDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун выбора клиента.");
			ClientDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун выбора группы проекта клиента
		/// </summary>
		public GlossaryPropertiesDialog ClickProjectGroupDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун выбора группы проекта.");
			ProjectGroupDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун выбора клиента
		/// </summary>
		/// <param name="client">клиент</param>
		public GlossaryPropertiesDialog ClickClientOption(string client)
		{
			CustomTestContext.WriteLine("Нажать на дропдаун выбора клиента.");
			ClientOption = Driver.SetDynamicValue(How.XPath, CLIENT_OPTION, client);
			ClientOption.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун выбора группы проекта
		/// </summary>
		/// <param name="projectGroup">группа проекта</param>
		public GlossaryPropertiesDialog ClickProjectGroupOption(string projectGroup)
		{
			CustomTestContext.WriteLine("Нажать на дропдаун выбора группы проекта.");
			ProjectGroupOption = Driver.SetDynamicValue(How.XPath, PROJECT_GROUP_OPTION, projectGroup);
			ProjectGroupOption.ScrollAndClick();

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Раскрыть дропдаун языка
		/// </summary>
		/// <param name="languageNumber">номер языка</param>
		public GlossaryPropertiesDialog ClickLanguageDropdown(int languageNumber)
		{
			CustomTestContext.WriteLine("Раскрыть дропдаун языка №{0}.", languageNumber);
			LanguageDropdown = Driver.SetDynamicValue(How.XPath, LANGUAGE_DROPDOWN, languageNumber.ToString());
			LanguageDropdown.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать клиента в дропдауне
		/// </summary>
		/// <param name="client">клиент</param>
		public GlossaryPropertiesDialog SelectClient(string client)
		{
			ClickClientDropdown();
			ClickClientOption(client);
			GlossaryName.Click();

			return LoadPage();
		}
		
		/// <summary>
		/// Выбрать группу проекта в дропдауне
		/// </summary>
		/// <param name="projectGroup">группа проекта</param>
		public GlossaryPropertiesDialog SelectProjectGroup(string projectGroup)
		{
			ClickProjectGroupDropdown();
			ClickProjectGroupOption(projectGroup);
			ClickProjectGroupDropdown();

			return LoadPage();
		}

		/// <summary>
		/// Добавить язык
		/// </summary>
		/// <param name="language">язык</param>
		public GlossaryPropertiesDialog AddLangauge(Language language)
		{
			ClickAddLanguageButton();
			ClickLastLanguageDropdown();
			SelectLanguage(language);

			return LoadPage();
		}

		/// <summary>
		/// Изменить язык
		/// </summary>
		/// <param name="language">язык</param>
		/// <param name="languageNumber">номер языка</param>
		public GlossaryPropertiesDialog EditLangauge(Language language, int languageNumber = 1)
		{
			ClickLanguageDropdown(languageNumber);
			SelectLanguage(language);

			return LoadPage();
		}

		/// <summary>
		/// Отменить удаление языка в окне настройки
		/// </summary>
		/// <param name="languagesNumber">порядковый номер языка</param>
		public GlossaryPropertiesDialog CancelDeleteLanguageInPropertiesDialog(int languagesNumber = 2)
		{
			ClickDeleteLanguageButton(languagesNumber);
			ClickCancelInDeleteLanguageWarning();

			return LoadPage();
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

		[FindsBy(How = How.XPath, Using = LAST_LANGUAGE_DROPDOWN_ARROW)]
		protected IWebElement LastLanguageDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENT_DROPDOWN)]
		protected IWebElement ClientDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUP_DROPDOWN)]
		protected IWebElement ProjectGroupDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = COMMENT)]
		protected IWebElement Comment { get; set; }
		protected IWebElement ClientOption { get; set; }
		protected IWebElement LanguageDropdown { get; set; }
		protected IWebElement ProjectGroupOption { get; set; }
		#endregion

		#region Описания XPath элементов

		protected const string COMMENT = ".//div[contains(@class,'js-popup-edit-glossary')][2]//textarea[contains(@data-bind, 'comment')]";
		protected const string LANGUAGE_OPTION = "//span[contains(@class, 'js-dropdown__item') and @data-id='*#*']";
		protected const string ADD_LANGUAGE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[contains(@data-bind, 'addLanguage')]";
		protected const string LAST_LANGUAGE_DROPDOWN_ARROW = "(//span[contains(@class, 'editgloss__control l-editgloss__lang')])[last()]//span[contains(@class, 'dropdown')]//i";
		protected const string CLIENT_DROPDOWN = "//span[contains(@class, 'dropdown boxtype')]";
		protected const string CLIENT_OPTION = "//span[contains(@class, 'dropdown__list boxtype')]//span[@title='*#*']";
		protected const string PROJECT_GROUP_OPTION = "//ul[contains(@class, 'multiselect-checkboxes')]//li//span[text()='*#*']/preceding-sibling::span";
		protected const string PROJECT_GROUP_DROPDOWN = "//div[contains(@class, 'multiselect-text')]";
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
		protected const string LANGUAGE_DROPDOWN = "//span[contains(@class, 'g-iblock l-editgloss__control l-editgloss__lang')][*#*]//span[contains(@class, 'dropdown  g-drpdwn g-iblock')]//span";

		#endregion
	}
}
