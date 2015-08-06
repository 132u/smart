﻿using System;
using System.Collections.Generic;
using System.Linq;

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
		/// Нажать кнопку создания новой ТМ
		/// </summary>
		public NewTranslationMemoryDialog ClickCreateNewTmButton()
		{
			Logger.Debug("Нажать кнопку создания новой ТМ");
			CreateNewTmButton.JavaScriptClick();

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_TM_BUTTON), timeout: 20))
			{
				CreateNewTmButton.JavaScriptClick();
			}

			return new NewTranslationMemoryDialog().GetPage();
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
		/// Нажать кнопку Delete в информации о ТМ
		/// </summary>
		public TranslationMemoriesPage ClickDeleteButtonInTMInfo()
		{
			Logger.Debug("Нажать кнопку Delete.");
			DeleteButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Delete в диалоге подтверждения удаления ТМ
		/// </summary>
		public TranslationMemoriesPage ClickDeleteButtonInConfirmationDialog()
		{
			Logger.Debug("Нажать кнопку Delete в диалоге подтверждения удаления ТМ.");
			DeleteButtonInConfirmationDialog.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести название ТМ в поле поиска
		/// </summary>
		public TranslationMemoriesPage FillSearch(string translationMemoryName)
		{
			Logger.Debug("Ввести название ТМ {0} в поле поиска.", translationMemoryName);
			SearchField.SetText(translationMemoryName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку поиска ТМ
		/// </summary>
		public TranslationMemoriesPage ClickSearchTMButton()
		{
			Logger.Debug("Нажать кнопку поиска ТМ.");
			SearchButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на строку с ТМ
		/// </summary>
		/// <remarks>
		/// Нажатие на строку открывает/закрывает информацию о ТМ
		/// </remarks>
		public TranslationMemoriesPage ClickTranslationMemoryRow(string translationMemoryName)
		{
			Logger.Debug("Нажать по строке с ТМ {0}.", translationMemoryName);
			Driver.FindElement(By.XPath(TM_ROW.Replace("*#*", translationMemoryName))).Click();

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
		/// Очистить имя в форме изменения ТМ
		/// </summary>
		public TranslationMemoriesPage CleanTranslationMemoryName()
		{
			Logger.Debug("Очистить имя в форме изменения ТМ.");
			EditNameField.Clear();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранить в форме редактирования TM
		/// </summary>
		public TranslationMemoriesPage ClickSaveTranslationMemoryButton()
		{
			Logger.Debug("Нажать кнопку сохранить в форме редактирования TM.");
			SaveChangesButton.Click();
			
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
		/// Нажать на поле с языком перевода в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToTargetLanguages()
		{
			Logger.Debug("Нажать на поле с языком перевода в форме редактирования ТМ");
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

		///<summary>
		/// Нажать на поле с группами проектов в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToProjectGroupsField()
		{
			Logger.Debug("Нажать на поле с группами проектов в форме редактирования ТМ");
			ProjectGroupsList.Click();

			return GetPage();
		}

		///<summary>
		/// Выбрать первую группу проектов в списке и вернуть ее имя
		/// </summary>
		/// <param name="projectGroup">Имя выбранной группы проектов</param>
		public TranslationMemoriesPage SelectFirstProjectGroup(out string projectGroup)
		{
			Logger.Debug("Кликнуть на первую группу проектов в списке");
			FirstProjectGroupInInList.Click();
			Logger.Trace("Получить имя первой группы проектов в списке");
			projectGroup = FirstProjectGroupInInList.Text;

			return GetPage();
		}
		
		/// <summary>
		/// Проверить, что ТМ представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryExists(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} представлена в списке.", tmName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TM_ROW.Replace("*#*", tmName))),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ.", tmName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, представлена ли ТМ в списке
		/// </summary>
		public bool TranslationMemoryExists(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} представлена в списке.", tmName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(TM_ROW.Replace("*#*", tmName)));
		}

		/// <summary>
		/// Проверить, что ТМ не представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryNotExists(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} не представлена в списке.", tmName);

			Assert.IsFalse(Driver.ElementIsDisplayed(By.XPath(TM_ROW.Replace("*#*", tmName))),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ.", tmName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, открыта ли информация о ТМ
		/// </summary>
		public bool IsTranslationMemoryInformationOpen(string tmName)
		{
			Logger.Trace("Получить, открыта ли информация о ТМ.");

			return Driver.ElementIsDisplayed(By.XPath(TM_INFORMATION_FORM.Replace("*#*", tmName)));
		}

		/// <summary>
		/// Проверить, что диалог подтверждения удаления ТМ появился
		/// </summary>
		public TranslationMemoriesPage AssertDeleteConfirmatonDialogPresent()
		{
			Logger.Trace("Проверить, что диалог подтверждения удаления ТМ появился.");
			Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_CONFIRMATION_DIALOG));

			Assert.IsTrue(DeleteConfirmationDialog.Displayed,
				"Произошла ошибка:\n диалог подтверждения удаления ТМ не появился.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог подтверждения удаления ТМ закрылся
		/// </summary>
		public TranslationMemoriesPage AssertDeleteConfirmatonDialogDisappear()
		{
			Logger.Trace("Проверить, что диалог подтверждения удаления ТМ закрылся.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_CONFIRMATION_DIALOG)),
				"Произошла ошибка:\n диалог подтверждения удаления ТМ не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Проверить наличие ошибки о пустом названии при редактировании ТМ
		/// </summary>
		public TranslationMemoriesPage AssertNoNameErrorAppear()
		{
			Logger.Trace("Проверить наличие ошибки о пустом названии при редактировании ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EDIT_NO_NAME)),
				"Ошибка: не появилось сообщение о пустом названии при редактировании ТМ.");

			return GetPage();
		}

		/// <summary>
		/// Проверить наличие ошибки о существующем имени при некорректном редактировании имени ТМ
		/// </summary>
		public TranslationMemoriesPage AssertExistingNameErrorAppeared()
		{
			Logger.Trace("Проверить наличие ошибки о существующем имени при некорректном редактировании имени ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EDIT_EXIST_NAME)),
				"Ошибка: не появилось сообщение об ошибки имени при редактировании ТМ.");

			return GetPage();
		}

		/// <summary>
		/// Проверить исчезновение формы редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage AssertEditionFormDisappeared()
		{
			Logger.Trace("Дождаться исчезновения формы редактирования ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(TM_EDIT_SAVE_BTN)),
				"Ошибка: не исчезла форма редактирования ТМ");

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли на странице комментарий
		/// </summary>
		public TranslationMemoriesPage AssertCommentExists(string text)
		{
			Logger.Trace("Проверить, есть ли на странице комментарий {0}.", text);

			Assert.IsTrue(EditCommentField.GetAttribute("value") == text, "Ошибка: комментарий {0} не найден.", text);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог создания ТМ закрылся
		/// </summary>
		public TranslationMemoriesPage AssertNewTMDialogDisappeared()
		{
			Logger.Trace("Проверить, что диалог создания ТМ закрылся.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(CREATE_TM_DIALOG)),
				"Произошла ошибка:\n диалог создания ТМ не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, указаны ли для ТМ корректные языки
		/// </summary>
		public TranslationMemoriesPage AssertLanguagesForTranslationMemory(
			string translationMemoryName,
			string sourceLanguage,
			List<string> targetLanguages)
		{
			Logger.Trace("Проверить, указаны ли для ТМ {0} корректные языки: source = {1}, target = {2}.",
				translationMemoryName, sourceLanguage, targetLanguages);
			var TM = Driver.FindElement(By.XPath(TM_LANGUAGES_IN_TABLE.Replace("*#*", translationMemoryName)));
			var languagesColumn = TM.Text;
			var languagesList = languagesColumn.Split(new[] { '>' }).ToList();

			Assert.AreEqual(2, languagesList.Count(), "Произошла ошибка:\n неверное количество элементов в списке с source и target языками.");
			
			var actualSource = languagesList[0].Trim();
			var actualTargetList = languagesList[1].Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
			
			Assert.AreEqual(sourceLanguage, actualSource,
				"Произошла ошибка:\n source языки не совпали.");

			Assert.IsFalse(targetLanguages.Except(actualTargetList).Any(),
				"Произошла ошибка:\nСписки target языков не совпали. Ожидаемый список target языков содержит больше элементов чем фактический список.");

			Assert.IsFalse(actualTargetList.Except(targetLanguages).Any(),
				"Произошла ошибка:\nСписки target языков не совпали. Фактический список target языков содержит больше элементов чем ожидаемый список.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение о окончании импорта TMX файла появилось.
		/// </summary>
		public TranslationMemoriesPage AssertFileImportCompleteNotifierDisplayed()
		{
			Logger.Trace("Проверить, что сообщение о окончании импорта TMX файла появилось.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(FILE_IMPORT_NOTIFIER), timeout: 45),
				"Произошла ошибка:\n сообещние о окончании импорта TMX файла не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить группу проектов для ТМ
		/// </summary>
		public TranslationMemoriesPage AssertProjectGroupSelectedForTM(string translationMemoryName, string projectGroup)
		{
			Logger.Trace("Проверить, что для ТМ {0} указана группа проектов {1}", translationMemoryName, projectGroup);

			Assert.IsTrue(ProjectGroupsField.Text.Contains(projectGroup), string.Format("Произошла ошибка:\n неверно указан проект для ТМ {0}.", translationMemoryName));
			
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по именам
		/// </summary>
		public TranslationMemoriesPage ClickSortByTMName()
		{
			Logger.Debug("Нажать кнопку сортировки по именам");
			TMName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате создания
		/// </summary>
		public TranslationMemoriesPage ClickSortByCreationDate()
		{
			Logger.Debug("Нажать кнопку сортировки по дате создания.");
			CreationDate.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = TM_NAME)]
		protected IWebElement TMName { get; set; }

		[FindsBy(How = How.XPath, Using = CREATION_DATE)]
		protected IWebElement CreationDate { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_TM_BTN)]
		protected IWebElement CreateNewTmButton { get; set; }


		[FindsBy(How = How.XPath, Using = SEARCH_BUTTON)]
		protected IWebElement SearchButton { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_TM_FIELD)]
		protected IWebElement SearchField { get; set; }


		[FindsBy(How = How.XPath, Using = DELETE_BUTTON)]
		protected IWebElement DeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_CONFIRMATION_DIALOG)]
		protected IWebElement DeleteConfirmationDialog { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_BUTTON_IN_CONFIRMATION_DIALOG)]
		protected IWebElement DeleteButtonInConfirmationDialog { get; set; }


		[FindsBy(How = How.XPath, Using = TM_EDIT_BUTTON)]
		protected IWebElement EditButton { get; set; }

		[FindsBy(How = How.XPath, Using = TM_EDIT_NAME)]
		protected IWebElement EditNameField { get; set; }

		[FindsBy(How = How.XPath, Using = TM_EDIT_COMMENT)]
		protected IWebElement EditCommentField { get; set; }

		[FindsBy(How = How.XPath, Using = TM_EDIT_SAVE_BTN)]
		protected IWebElement SaveChangesButton { get; set; }


		[FindsBy(How = How.XPath, Using = TM_EDIT_PROJECT_GROUPS)]
		protected IWebElement ProjectGroupsList { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUPS_FIRST_IN_LIST)]
		protected IWebElement FirstProjectGroupInInList { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUPS_FIELD)]
		protected IWebElement ProjectGroupsField { get; set; }


		[FindsBy(How = How.XPath, Using = TM_EDIT_TARGET_LANGUAGE)]
		protected IWebElement TranslationMemoryTargetLanguagesField { get; set; }

		[FindsBy(How = How.XPath, Using = TM_EDIT_TARGET_LANGUAGE_LIST)]
		protected IWebElement TranslationMemoryTargetLanguagesList { get; set; }
		protected IWebElement TargetLanguage { get; set; }

		protected const string ADD_TM_BTN = "//span[contains(@data-bind,'createTm')]//a";
		protected const string CREATE_TM_DIALOG = "//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string TM_ROW = "//tr[contains(@class,'l-corpr__trhover clickable')]//span[text()='*#*']";
		protected const string TM_INFORMATION_FORM = "//span[string()='*#*']//..//..//following-sibling::tr[@class='js-tm-panel']//td//div";

		protected const string DELETE_BUTTON = "//tr[@class='js-tm-panel']//span[contains(@data-bind, 'deleteTranslationMemory')]";
		protected const string DELETE_CONFIRMATION_DIALOG = "//form[contains(@action,'Delete')]";
		protected const string DELETE_BUTTON_IN_CONFIRMATION_DIALOG = "//form[contains(@action,'Delete')]//input[@value='Delete']";
		protected const string SAVE_TM_BUTTON = ".//div[contains(@class,'js-popup-create-tm')][2]//span[contains(@data-bind, 'click: save')]";

		protected const string TM_NAME = "(//th[contains(@data-sort-by,'Name')]//a)[1]";
		protected const string CREATION_DATE = "//th[contains(@data-sort-by,'CreatedDate')]//a";
		protected const string TM_LANGUAGES = "//td[@class='l-corpr__td tm']//span[string()='*#*']/parent::td/parent::tr//td[2]//span[string()='*##*']";
		protected const string TM_LANGUAGES_IN_TABLE = "//td[@class='l-corpr__td tm']//span[string()='*#*']/parent::td/parent::tr//td[2]//span";
		protected const string TARGET_LANG_ITEM = "(//div[contains(@class,'ui-multiselect-menu')])[2]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li//input[@value='*#*']";

		protected const string TM_EDIT_BUTTON = "//tr[@class='js-tm-panel']//span[contains(@data-bind, 'switchToEditing')]//a";
		protected const string TM_EDIT_NAME = "//tr[contains(@class,'js-tm-panel')]//input[contains(@data-bind, 'value: name')]";
		protected const string TM_EDIT_COMMENT = "//tr[contains(@class,'js-tm-panel')]//textarea";
		protected const string TM_EDIT_SAVE_BTN = "//tr[contains(@class,'js-tm-panel')]//span[contains(@data-bind,'click: save')]";
		protected const string TM_EDIT_TARGET_LANGUAGE = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[1]//div[contains(@class,'ui-multiselect')]/div";
		protected const string TM_EDIT_TARGET_LANGUAGE_LIST = "/html/body/div[21]/div";
		protected const string TM_EDIT_PROJECT_GROUPS = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[3]//div[contains(@class,'ui-multiselect')]/div";

		protected const string PROJECT_GROUPS_FIRST_IN_LIST = "//div[contains(@style, 'block')]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li[2]//label//input";
		protected const string PROJECT_GROUPS_FIELD = "//tr[contains(@class,'js-tm-panel')]//div[contains(@data-bind,'domainNames')]";

		protected const string ERROR_EDIT_NO_NAME = "//tr[contains(@class,'js-tm-panel')]//div[contains(@class,'tmpanel__error')]//p[contains(@data-message-id, 'name-required')]";
		protected const string ERROR_EDIT_EXIST_NAME = "//tr[contains(@class,'js-tm-panel')]//div[contains(@class,'tmpanel__error')]//p[contains(text(),'The name should be unique.')]";

		protected const string SEARCH_BUTTON = "//a[contains(@class, 'search-by-name')]";
		protected const string SEARCH_TM_FIELD = "//input[contains(@class, 'search-tm')]";

		protected const string FILE_IMPORT_NOTIFIER = "//div[contains(@class, 'notifications')]//span[contains(text(),'units imported from file')]";
	}
}
