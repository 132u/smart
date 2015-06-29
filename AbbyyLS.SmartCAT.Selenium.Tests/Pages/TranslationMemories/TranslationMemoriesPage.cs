using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class TranslationMemoriesPage : WorkspacePage, IAbstractPage<TranslationMemoriesPage>
	{
		public new TranslationMemoriesPage GetPage()
		{
			var translationMemoriesPage = new TranslationMemoriesPage();
			InitPage(translationMemoriesPage);

			return translationMemoriesPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ADD_TM_BTN)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с памятью переводов.");
			}
		}

		/// <summary>
		/// Нажать кнопку создания новой ТМ"
		/// </summary>
		public NewTranslationMemoryDialog ClickCreateNewTmButton()
		{
			Logger.Debug("Нажать кнопку создания новой ТМ");
			CreateNewTmButton.JavaScriptClick();
			
			return new NewTranslationMemoryDialog().GetPage();
		}

		/// <summary>
		/// Проверить, что ТМ представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryExist(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} представлена в списке.", tmName);
			var translationMemoryRowPath = TM_NAME.Replace("*#*", tmName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(translationMemoryRowPath)),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ.", tmName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что ТМ не представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryNotExist(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} не представлена в списке.", tmName);
			var translationMemoryRowPath = TM_NAME.Replace("*#*", tmName);

			Assert.IsFalse(Driver.ElementIsDisplayed(By.XPath(translationMemoryRowPath)),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ.", tmName);

			return GetPage();
		}

		/// <summary>
		/// Получить, открыта ли информация о ТМ
		/// </summary>
		public bool IsTranslationMemoryInformationOpen(string tmName)
		{
			Logger.Trace("Получить, открыта ли информация о ТМ.");
			var translationMemoryInformationFormPath = TM_INFORMATION_FORM.Replace("*#*", tmName);

			return Driver.ElementIsDisplayed(By.XPath(translationMemoryInformationFormPath));
		}

		/// <summary>
		/// Открыть подробную информацию о ТМ
		/// </summary>
		public TranslationMemoriesPage OpenTranslationMemoryInformation(string tmName)
		{
			Logger.Debug("Открыть подробную информацию о ТМ {0}.", tmName);
			TranslationMemoryName = Driver.SetDynamicValue(How.XPath, TM_NAME, tmName);
			TranslationMemoryName.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickEditButton()
		{
			Logger.Debug("Нажать кнопку редактирования ТМ.");
			EditButton.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что форма редактирования ТМ была открыта
		/// </summary>
		public TranslationMemoriesPage AssertEditionFormOpen()
		{
			Logger.Trace("Проверить, что форма редактирования ТМ была открыта.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TM_EDIT_FORM)),
				"Ошибка: форма редактирования ТМ не была открыта");

			return GetPage();
		}

		/// <summary>
		/// Очистить имя в форме изменения ТМ
		/// </summary>
		public TranslationMemoriesPage CleanTranslationMemoryName()
		{
			Logger.Debug("Очистить имя в форме изменения ТМ.");
			EditNameField.Clear();

			return GetPage();
		}

		/// <summary>
		/// Ввести имя в форме изменения ТМ
		/// </summary>
		public TranslationMemoriesPage AddTranslationMemoryName(string tmName)
		{
			Logger.Debug("Ввести имя в форме изменения ТМ.");
			EditNameField.SetText(tmName);

			return GetPage();
		}

		/// <summary>
		/// Кликнуть кнопку сохранить в форме редактирования TM
		/// </summary>
		public TranslationMemoriesPage ClickSaveTranslationMemoryButton()
		{
			Logger.Debug("Кликнуть кнопку сохранить в форме редактирования TM.");
			SaveChangesButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Дождаться исчезновения формы редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage AssertEditionFormDisappear()
		{
			Logger.Trace("Дождаться исчезновения формы редактирования ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDissapeared(By.XPath(TM_EDIT_SAVE_BTN)),
				"Ошибка: не исчезла форма редактирования ТМ");

			return GetPage();
		}

		/// <summary>
		/// Проверить наличие ошибки при некорректном редактировании имени ТМ
		/// </summary>
		public TranslationMemoriesPage AssertNoNameErrorAppear()
		{
			Logger.Trace("Проверить наличие ошибки при некорректном редактировании имени ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EDIT_NO_NAME)),
				"Ошибка: не появилось сообщение об ошибке в имени при редактировании ТМ.");

			return GetPage();
		}

		/// <summary>
		/// Проверить наличие ошибки о существующем имени при некорректном редактировании имени ТМ
		/// </summary>
		public TranslationMemoriesPage AssertExistingNameErrorAppear()
		{
			Logger.Trace("Проверить наличие ошибки о существующем имени при некорректном редактировании имени ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EDIT_EXIST_NAME)),
				"Ошибка: не появилось сообщение об ошибки имени при редактировании ТМ.");

			return GetPage();
		}

		/// <summary>
		/// Ввести имя ТМ в поле поиска
		/// </summary>
		public TranslationMemoriesPage AddTranslationMemoryNameToSearch(string tmName)
		{
			Logger.Debug("Ввести имя ТМ в поле поиска.");
			SearchTranslationMemoryField.SetText(tmName);

			return GetPage();
		}

		/// <summary>
		/// Очистить поле комментария
		/// </summary>
		public TranslationMemoriesPage CleanComment()
		{
			Logger.Debug("Очистить поле комментария.");
			EditCommentField.Clear();

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле комментария
		/// </summary>
		public TranslationMemoriesPage AddComment(string text)
		{
			Logger.Debug("Заполнить поле комментария текстом {0}.", text);
			EditCommentField.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли на странице комментарий
		/// </summary>
		public TranslationMemoriesPage AssertCommentExist(string text)
		{
			Logger.Trace("Проверить, есть ли на странице комментарий {0}.", text);
			
			Assert.IsTrue(EditCommentField.GetAttribute("value") == text, "Ошибка: комментарий {0} не найден.", text);

			return GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку поиска ТМ
		/// </summary>
		public TranslationMemoriesPage ClickSearchButton()
		{
			Logger.Debug("Нажать кнопку поиска ТМ.");
			SearchTranslationMempryButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть на поле с языком перевода в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToTargetLanguages()
		{
			Logger.Debug("Кликнуть на поле с языком перевода в форме редактирования ТМ");
			TranslationMemoryTargetLanguagesField.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		public TranslationMemoriesPage SelectTargetLanguage(Language language)
		{
			Logger.Debug("Выбрать язык перевода {0}", language);
			TargetLanguage = Driver.SetDynamicValue(How.XPath, TARGET_LANG_ITEM, ((int)language).ToString());
			TargetLanguage.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, указанны ли для ТМ корректные языки
		/// </summary>
		public TranslationMemoriesPage AssertLanguagesForTranslationMemory(
			string translationMemoryName,
			string sourceLanguage,
			string[] targetLanguages)
		{
			var formattedLanguagesString = string.Concat(sourceLanguage, " > ", string.Join(", ", targetLanguages));

			Logger.Trace("Проверить, указанны ли для ТМ {0} корректные языки {1}", translationMemoryName, formattedLanguagesString);
			var translationMemoryLanguages = TM_LANGUAGES
				.Replace("*#*", translationMemoryName)
				.Replace("*##*", formattedLanguagesString);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(translationMemoryLanguages)),
				"Ошибка: для ТМ неверно отображены исходный язык и язык перевода.");

			return GetPage();
		}

		///<summary>
		/// Кликнуть на поле с проектом в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToProjectsFieldAtTmEdditForm()
		{
			Logger.Debug("Кликнуть на поле с проектом в форме редактирования ТМ");
			TranslationMemoryProjectGroupField.Click();

			return GetPage();
		}

		///<summary>
		/// Выбрать первую группу проектов в списке и вернуть ее имя
		/// </summary>
		public TranslationMemoriesPage SelectFirstProjectGroupAndGetName(out string projectGroup)
		{
			Logger.Debug("Кликнуть на первую группу проектов в списке");
			FirstProjectGroupInInList.Click();
			Logger.Trace("Получить имя первой группы проектов в списке");
			projectGroup = FirstProjectGroupInInList.Text;

			return GetPage();
		}

		/// <summary>
		/// Проверить группу проектов для ТМ
		/// </summary>
		public TranslationMemoriesPage AssertProjectGroupExistForTranslationMemory(string translationMemoryName, string projectGroup)
		{
			Logger.Trace("Проверить, что для ТМ {0} указана группа проектов {1}", translationMemoryName, projectGroup);

			Assert.IsTrue(DomainPath.Text.Contains(projectGroup), string.Format("Ошибка: неверно указан проект для ТМ {0}.", translationMemoryName));

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = ADD_TM_BTN)]
		protected IWebElement CreateNewTmButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_BTN)]
		protected IWebElement EditButton { get; set; }

		[FindsBy(How = How.XPath, Using = TM_EDIT_NAME)]
		protected IWebElement EditNameField { get; set; }

		[FindsBy(How = How.XPath, Using = TM_EDIT_COMMENT)]
		protected IWebElement EditCommentField { get; set; }

		[FindsBy(How = How.XPath, Using = TM_EDIT_SAVE_BTN)]
		protected IWebElement SaveChangesButton { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_TM_FIELD)]
		protected IWebElement SearchTranslationMemoryField { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_TM_BTN)]
		protected IWebElement SearchTranslationMempryButton { get; set; }

		[FindsBy(How = How.XPath, Using = TM_EDIT_TARGET_LANGUAGE)]
		protected IWebElement TranslationMemoryTargetLanguagesField { get; set; }

		[FindsBy(How = How.XPath, Using = DOMAN_SPAN)]
		protected IWebElement DomainPath { get; set; }

		[FindsBy(How = How.XPath, Using = FIRST_PROJECT_GROUP_IN_LIST)]
		protected IWebElement FirstProjectGroupInInList { get; set; }

		[FindsBy(How = How.XPath, Using = TM_EDIT_PROJECT_GROUP)]
		protected IWebElement TranslationMemoryProjectGroupField { get; set; }

		protected IWebElement TranslationMemoryName { get; set; }

		protected IWebElement TargetLanguage { get; set; }

		protected const string CREATE_TM_DIALOG = "//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string ADD_TM_BTN = "//span[contains(@data-bind,'createTm')]";
		protected const string TM_TABLE_BODY = "//table[contains(@class,'js-sortable-table')]";
		protected const string TM_NAME = "//span[string()='*#*']";
		protected const string TM_LANGUAGES = "//td[@class='l-corpr__td tm']//span[string()='*#*']/parent::td/parent::tr//td[2]//span[string()='*##*']";
		protected const string TARGET_LANG_ITEM = "//div[contains(@class,'ui-multiselect')]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li//input[@value='*#*']";
		protected const string FIRST_PROJECT_GROUP_IN_LIST = "//div[contains(@class,'ui-multiselect')]//ul//li[2]//label//span[2]";
		
		protected const string TM_INFORMATION_FORM = "//span[string()='*#*']//..//..//following-sibling::tr[@class='js-tm-panel']//td//div";

		protected const string EDIT_BTN = "//tr[@class='js-tm-panel']//span[contains(@data-bind, 'switchToEditing')]//a";

		protected const string TM_EDIT_FORM = "//tr[contains(@class,'js-tm-panel')]";
		protected const string TM_EDIT_NAME = "//tr[contains(@class,'js-tm-panel')]//input[contains(@data-bind, 'value: name')]";
		protected const string TM_EDIT_COMMENT = "//tr[contains(@class,'js-tm-panel')]//textarea";
		protected const string TM_EDIT_SAVE_BTN = "//tr[contains(@class,'js-tm-panel')]//span[contains(@data-bind,'click: save')]";
		protected const string TM_EDIT_TARGET_LANGUAGE = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[1]//div[contains(@class,'ui-multiselect')]/div";
		protected const string DOMAN_SPAN = "//tr[contains(@class,'js-tm-panel')]//div[contains(@data-bind,'domainNames')]";
		protected const string TM_EDIT_PROJECT_GROUP = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[3]//div[contains(@class,'ui-multiselect')]/div";

		protected const string ERROR_EDIT_NO_NAME = "//tr[contains(@class,'js-tm-panel')]//div[contains(@class,'tmpanel__error')]//p[contains(@data-message-id, 'name-required')]";
		protected const string ERROR_EDIT_EXIST_NAME = "//tr[contains(@class,'js-tm-panel')]//div[contains(@class,'tmpanel__error')]//p[contains(text(),'The name should be unique.')]";

		protected const string SEARCH_TM_FIELD = "//input[contains(@class, 'search-tm')]";
		protected const string SEARCH_TM_BTN = "//a[contains(@class, 'search-by-name')]";
	}
}
