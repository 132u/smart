using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class TranslationMemoriesPage : WorkspacePage, IAbstractPage<TranslationMemoriesPage>
	{
		public TranslationMemoriesPage(WebDriver driver) : base(driver)
		{
		}

		public new TranslationMemoriesPage GetPage()
		{
			var translationMemoriesPage = new TranslationMemoriesPage(Driver);
			InitPage(translationMemoriesPage, Driver);

			return translationMemoriesPage;
		}

		public new void LoadPage()
		{
			if (!IsTranslationMemoriesPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница с памятью переводов");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку создания новой ТМ
		/// </summary>
		public NewTranslationMemoryDialog ClickCreateNewTmButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку создания новой ТМ");
			CreateNewTmButton.JavaScriptClick();

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_TM_BUTTON), timeout: 20))
			{
				CreateNewTmButton.JavaScriptClick();
			}

			return new NewTranslationMemoryDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickEditButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования ТМ.");
			EditButton.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Delete в информации о ТМ
		/// </summary>
		public TranslationMemoriesPage ClickDeleteButtonInTMInfo()
		{
			CustomTestContext.WriteLine("Нажать кнопку Delete.");
			DeleteButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести название ТМ в поле поиска
		/// </summary>
		public TranslationMemoriesPage FillSearch(string translationMemoryName)
		{
			CustomTestContext.WriteLine("Ввести название ТМ {0} в поле поиска.", translationMemoryName);
			SearchField.SetText(translationMemoryName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку поиска ТМ
		/// </summary>
		public TranslationMemoriesPage ClickSearchTMButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку поиска ТМ.");
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
			CustomTestContext.WriteLine("Нажать по строке с ТМ {0}.", translationMemoryName);
			Driver.FindElement(By.XPath(TM_ROW.Replace("*#*", translationMemoryName))).Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести имя в форме изменения ТМ
		/// </summary>
		public TranslationMemoriesPage AddTranslationMemoryName(string tmName)
		{
			CustomTestContext.WriteLine("Ввести имя в форме изменения ТМ.");
			EditNameField.SetText(tmName);

			return GetPage();
		}

		/// <summary>
		/// Очистить имя в форме изменения ТМ
		/// </summary>
		public TranslationMemoriesPage CleanTranslationMemoryName()
		{
			CustomTestContext.WriteLine("Очистить имя в форме изменения ТМ.");
			EditNameField.Clear();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранить в форме редактирования TM
		/// </summary>
		public TranslationMemoriesPage ClickSaveTranslationMemoryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранить в форме редактирования TM.");
			SaveChangesButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Очистить поле комментария
		/// </summary>
		public TranslationMemoriesPage CleanComment()
		{
			CustomTestContext.WriteLine("Очистить поле комментария.");
			EditCommentField.Clear();

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле комментария
		/// </summary>
		public TranslationMemoriesPage AddComment(string text)
		{
			CustomTestContext.WriteLine("Заполнить поле комментария текстом {0}.", text);
			EditCommentField.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Нажать на поле с языком перевода в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToTargetLanguages()
		{
			CustomTestContext.WriteLine("Нажать на поле с языком перевода в форме редактирования ТМ");
			TranslationMemoryTargetLanguagesField.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		public TranslationMemoriesPage SelectTargetLanguage(Language? language)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода {0}", language);
			TargetLanguage = Driver.SetDynamicValue(How.XPath, TARGET_LANG_ITEM, ((int) language).ToString());
			TargetLanguage.Click();

			return GetPage();
		}

		///<summary>
		/// Нажать на поле с клиентами в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToClientsField()
		{
			CustomTestContext.WriteLine("Нажать на поле с клиентами в форме редактирования ТМ");
			ClientsField.Click();

			return GetPage();
		}

		///<summary>
		/// Нажать на поле с темами в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToTopicsField()
		{
			CustomTestContext.WriteLine("Нажать на поле с темами в форме редактирования ТМ");
			TopicsField.Click();

			return GetPage();
		}

		///<summary>
		/// Нажать на поле с группами проектов в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToProjectGroupsField()
		{
			CustomTestContext.WriteLine("Нажать на поле с группами проектов в форме редактирования ТМ");
			ProjectGroupsList.Click();

			return GetPage();
		}

		///<summary>
		/// Выбрать первую группу проектов в списке и вернуть ее имя
		/// </summary>
		/// <param name="projectGroup">Имя выбранной группы проектов</param>
		public TranslationMemoriesPage SelectFirstProjectGroup(out string projectGroup)
		{
			CustomTestContext.WriteLine("Кликнуть на первую группу проектов в списке");
			FirstProjectGroupInInList.Click();
			CustomTestContext.WriteLine("Получить имя первой группы проектов в списке");
			projectGroup = FirstProjectGroupInInList.Text;

			return GetPage();
		}

		///<summary>
		/// Выбрать группу проектов в списке
		/// </summary>
		/// <param name="projectGroup">Имя выбранной группы проектов</param>
		public TranslationMemoriesPage SelectProjectGroup(string projectGroup)
		{
			CustomTestContext.WriteLine("Выбрать группу проектов {0} в списке", projectGroup);
			ProjectGroupInInList = Driver.SetDynamicValue(How.XPath, PROJECT_GROUP_IN_LIST, projectGroup);
			ProjectGroupInInList.Click();

			return GetPage();
		}

		///<summary>
		/// Выбрать клиента в списке
		/// </summary>
		/// <param name="clientName">Имя клиента</param>
		public TranslationMemoriesPage SelectClient(string clientName)
		{
			CustomTestContext.WriteLine("Выбрать клиента {0} в списке", clientName);
			ClientInInList = Driver.SetDynamicValue(How.XPath, CLIENT_IN_LIST, clientName);
			ClientInInList.Click();

			return GetPage();
		}

		///<summary>
		/// Выбрать тему в списке (из видимых)
		/// </summary>
		/// <param name="topicName">тема</param>
		public TranslationMemoriesPage SelectTopic(string topicName)
		{
			CustomTestContext.WriteLine("Выбрать тему {0} в списке", topicName);
			TopicInList = Driver.SetDynamicValue(How.XPath, TOPIC_IN_LIST, topicName);
			TopicInList.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по именам
		/// </summary>
		public TranslationMemoriesPage ClickSortByTMName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по именам");
			TMName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате создания
		/// </summary>
		public TranslationMemoriesPage ClickSortByCreationDate()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате создания.");
			CreationDate.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить количество юнитов
		/// </summary>
		public int GetUnitsCount()
		{
			CustomTestContext.WriteLine("Получить количество юнитов");
			var unitsCountText = SegmentSpan.Text;

			CustomTestContext.WriteLine("Полученное количество юнитов: {0}", unitsCountText);
			int result;

			if (!int.TryParse(unitsCountText, out result))
			{
				throw new Exception(string.Format("Ошибка: невозможно преобразовать в число: {0}", unitsCountText));
			}

			return result;
		}
		
		/// <summary>
		/// Нажать на кнопку 'Update TM'
		/// </summary>
		public TranslationMemoriesPage ClickUpdateTmButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Update TM'");
			UpdateTmButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Export'
		/// </summary>
		public TranslationMemoriesPage ClickExportButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Export'");
			ExportButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку Add TMX
		/// </summary>
		public TranslationMemoriesPage ClickAddTmxButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Add TMX'");
			AddTmxButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку очистки всех фильтров
		/// </summary>
		public TranslationMemoriesPage ClickClearAllFiltersButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку очистки всех фильтров");
			ClearAllFiltersButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Filter'
		/// </summary>
		public TranslationMemoriesFilterDialog ClickFilterButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Filter'");
			FilterButton.Click();

			return new TranslationMemoriesFilterDialog(Driver).GetPage();
		}

		/// <summary>
		/// Вернуть, действуют ли сейчас какие-то фильтры
		/// </summary>
		public bool GetFiltersIsExist()
		{
			CustomTestContext.WriteLine("Вернуть, действуют ли сейчас какие-то фильтры");
			var filtersIsExist = Driver.WaitUntilElementIsDisplay(By.XPath(CLEAR_ALL_FILTERS_BUTTON));
			CustomTestContext.WriteLine("Фильтры обнаружены: {0}", filtersIsExist);

			return filtersIsExist;
		}

		/// <summary>
		/// Нажать кнопку удаления фильтра
		/// </summary>
		/// <param name="filterName">имя фильтра</param>
		public TranslationMemoriesPage ClickRemoveFilterButton(string filterName)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления фильтра {0}", filterName);
			RemoveFilterButton = Driver.SetDynamicValue(How.XPath, REMOVE_FILTER_BUTTON, filterName);
			RemoveFilterButton.Click();

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Открыть окно экспорта TM
		/// </summary>
		/// <param name="translationMemoryName">имя TM</param>
		public TranslationMemoriesPage ExportTM(string translationMemoryName)
		{
			ClickTranslationMemoryRow(translationMemoryName);
			ClickExportButton();

			return GetPage();
		}

		/// <summary>
		/// Открыть информацию о TM
		/// </summary>
		/// <param name="translationMemoryName">имяTM</param>
		public TranslationMemoriesPage OpenTranslationMemoryInformation(string translationMemoryName)
		{
			if (!IsTranslationMemoryInformationOpen(translationMemoryName))
			{
				ClickTranslationMemoryRow(translationMemoryName);
			}

			return GetPage();
		}

		/// <summary>
		/// Закрыть информацию о TM
		/// </summary>
		/// <param name="translationMemoryName">имяTM</param>
		public TranslationMemoriesPage CloseTranslationMemoryInformation(string translationMemoryName)
		{
			if (IsTranslationMemoryInformationOpen(translationMemoryName))
			{
				ClickTranslationMemoryRow(translationMemoryName);
			}

			return GetPage();
		}

		/// <summary>
		/// Очистить все фильтры
		/// </summary>
		public TranslationMemoriesPage ClearFiltersPanelIfExist()
		{
			if (GetFiltersIsExist())
			{
				ClickClearAllFiltersButton();
			}

			return GetPage();
		}

		/// <summary>
		/// Выполнить поиск ТМ
		/// </summary>
		/// <param name="tmName">имя ТМ</param>
		public TranslationMemoriesPage SearchForTranslationMemory(string tmName)
		{
			FillSearch(tmName);
			ClickSearchTMButton();

			return GetPage();
		}

		/// <summary>
		/// Добавить первую группу в списке в память перевода
		/// </summary>
		/// <param name="translationMemoryName"></param>
		/// <param name="projectGroupName">имя группы проекта</param>
		public TranslationMemoriesPage AddFirstProjectGroupToTranslationMemory(
			string translationMemoryName,
			out string projectGroupName)
		{
			ClickEditButton();

			if (!IsEditionFormDisplayed())
			{
				throw new XPathLookupException("Ошибка: не появилась форма редактирования ТМ");
			}

			ClickToProjectGroupsField();
			SelectFirstProjectGroup(out projectGroupName);
			ClickSaveTranslationMemoryButton();

			if (!IsEditionFormDisappeared())
			{
				throw new XPathLookupException("Ошибка: не исчезла форма редактирования ТМ");
			}

			return GetPage();
		}

		/// <summary>
		/// Редактировать ТМ
		/// </summary>
		/// <param name="tmName">имя ТМ</param>
		/// <param name="renameTo">новое имя</param>
		/// <param name="changeCommentTo">новый коментарий</param>
		/// <param name="addTargetLanguage">добавить язык таргета</param>
		/// <param name="addClient">добавить клиента</param>
		/// <param name="addTopic">добавить топик</param>
		/// <param name="addProjectGroup">добавить группу проекта</param>
		public TranslationMemoriesPage EditTranslationMemory(
			string tmName,
			string renameTo = null,
			string changeCommentTo = null,
			Language addTargetLanguage = Language.NoLanguage,
			string addClient = null,
			string addTopic = null,
			string addProjectGroup = null,
			bool isErrorExpecting = false)
		{
			OpenTranslationMemoryInformation(tmName);
			ClickEditButton();
			//Sleep нужен,чтобы форма успела перейти в режим редактирования.
			Thread.Sleep(1000);

			if (renameTo != null)
			{
				CleanTranslationMemoryName();
				AddTranslationMemoryName(renameTo);
			}

			if (changeCommentTo != null)
			{
				CleanComment();
				AddComment(changeCommentTo);
			}

			if (addTargetLanguage != Language.NoLanguage)
			{
				ClickToTargetLanguages();
				SelectTargetLanguage(addTargetLanguage);
			}

			if (addClient != null)
			{
				ClickToClientsField();
				SelectClient(addClient);
			}

			if (addTopic != null)
			{
				ClickToTopicsField();
				SelectTopic(addTopic);
				ClickToTopicsField();
			}

			if (addProjectGroup != null)
			{
				ClickToProjectGroupsField();
				SelectProjectGroup(addProjectGroup);
			}

			ClickSaveTranslationMemoryButton();

			if (!isErrorExpecting)
			{
				if (!IsEditionFormDisappeared())
				{
					throw new XPathLookupException("Ошибка: не исчезла форма редактирования ТМ");
				}

				CloseTranslationMemoryInformation(tmName);
			}

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница ТМ
		/// </summary>
		public bool IsTranslationMemoriesPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_TM_BTN));
		}

		/// <summary>
		/// Проверить, представлена ли ТМ в списке
		/// </summary>
		public bool IsTranslationMemoryExist(string tmName)
		{
			CustomTestContext.WriteLine("Проверить, что ТМ {0} представлена в списке.", tmName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(TM_ROW.Replace("*#*", tmName)));
		}

		/// <summary>
		/// Проверить, открыта ли информация о ТМ
		/// </summary>
		public bool IsTranslationMemoryInformationOpen(string tmName)
		{
			CustomTestContext.WriteLine("Получить, открыта ли информация о ТМ {0}", tmName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(TM_INFORMATION_FORM.Replace("*#*", tmName)));
		}

		/// <summary>
		/// Проверить наличие ошибки о пустом названии при редактировании ТМ
		/// </summary>
		public bool IsEmptyNameErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить наличие ошибки о пустом названии при редактировании ТМ");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EDIT_NO_NAME));
		}

		/// <summary>
		/// Проверить наличие ошибки о существующем имени при некорректном редактировании имени ТМ
		/// </summary>
		public bool IsExistingNameErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить наличие ошибки о существующем имени при некорректном редактировании имени ТМ.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EDIT_EXIST_NAME));
		}

		/// <summary>
		/// Проверить исчезновение формы редактирования ТМ
		/// </summary>
		public bool IsEditionFormDisappeared()
		{
			CustomTestContext.WriteLine("Проверить исчезновение формы редактирования ТМ.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(TM_EDIT_SAVE_BTN));
		}

		/// <summary>
		/// Проверить появление формы редактирования ТМ
		/// </summary>
		public bool IsEditionFormDisplayed()
		{
			CustomTestContext.WriteLine("Проверить появление формы редактирования ТМ.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(TM_EDIT_SAVE_BTN));
		}

		/// <summary>
		/// Проверить, есть ли на странице комментарий
		/// </summary>
		public bool IsCommentTextMatchExpected(string expectedText)
		{
			CustomTestContext.WriteLine("Проверить, есть ли на странице комментарий {0}.", expectedText);

			return EditCommentField.GetAttribute("value") == expectedText;
		}

		/// <summary>
		/// Проверить, что диалог создания ТМ закрылся
		/// </summary>
		public bool IsNewTMDialogDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что диалог создания ТМ закрылся.");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(CREATE_TM_DIALOG));
		}

		/// <summary>
		/// Проверить, указаны ли для ТМ корректные языки
		/// </summary>
		public bool IsLanguagesForTranslationMemoryExists(
			string translationMemoryName,
			string sourceLanguage,
			List<string> targetLanguages)
		{
			CustomTestContext.WriteLine("Проверить, указаны ли для ТМ {0} корректные языки: source = {1}, target = {2}.",
				translationMemoryName, sourceLanguage, string.Join(", ", targetLanguages.ToArray()));
			var TM = Driver.FindElement(By.XPath(TM_LANGUAGES_IN_TABLE.Replace("*#*", translationMemoryName)));
			var languagesColumn = TM.Text;
			var languagesList = languagesColumn.Split(new[] { '>' }).ToList();

			if (languagesList.Count == 2)
			{
				throw new InvalidElementStateException("Произошла ошибка:\n неверное количество элементов в списке с source и target языками");
			}

			var actualSource = languagesList[0].Trim();
			var actualTargetList = languagesList[1].Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

			if (sourceLanguage == actualSource)
			{
				throw new InvalidElementStateException("Произошла ошибка:\n source языки не совпали");
			}

			return targetLanguages.SequenceEqual(actualTargetList);
		}

		/// <summary>
		/// Проверить, что сообщение о окончании импорта TMX файла появилось.
		/// </summary>
		public bool IsFileImportCompleteNotifierDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение о окончании импорта TMX файла появилось");

			return Driver.WaitUntilElementIsDisplay(By.XPath(FILE_IMPORT_NOTIFIER), timeout: 60);
		}

		/// <summary>
		/// Проверить, что сообщение о процессе импорта TMX файла исчезло
		/// </summary>
		public bool IsFileImportAddingNotifierDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение о процессе импорта TMX файла исчезло");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(FILE_IMPORT_ADDING_NOTIFIER), timeout: 60);
		}

		/// <summary>
		/// Проверить, что появилось сообщение об ошибке во время импорта TMX файла
		/// </summary>
		public bool IsFileImportFailedNotifierDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение об ошибке во время импорта TMX файла");

			return Driver.WaitUntilElementIsDisplay(By.XPath(FILE_IMPORT_ERROR_NOTIFIER), timeout: 15);
		}

		/// <summary>
		/// Проверить, что для ТМ указана группа проектов
		/// </summary>
		public bool IsProjectGroupSelectedForTM(string projectGroup)
		{
			CustomTestContext.WriteLine("Проверить, что для ТМ указана группа проектов {1}", projectGroup);

			return ProjectGroupsField.Text.Contains(projectGroup);
		}

		/// <summary>
		/// Проверить появление валидационной ошибки при импорте
		/// </summary>
		public bool IsImportValidationErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить появление валидационной ошибки при импорте");

			return Driver.WaitUntilElementIsDisplay(By.XPath(UPDATE_TM_VALIDATION_ERROR_MESSAGE));
		}

		#endregion

		#region Объявление элементов страницы

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

		[FindsBy(How = How.XPath, Using = SEGMENT_SPAN)]
		protected IWebElement SegmentSpan { get; set; }

		[FindsBy(How = How.XPath, Using = UPDATE_TM_BUTTON)]
		protected IWebElement UpdateTmButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_TMX_BUTTON)]
		protected IWebElement AddTmxButton { get; set; }

		[FindsBy(How = How.XPath, Using = EXPORT_BUTTON)]
		protected IWebElement ExportButton { get; set; }
		
		[FindsBy(How = How.XPath, Using = CLIENTS_FIELD)]
		protected IWebElement ClientsField { get; set; }

		[FindsBy(How = How.XPath, Using = TOPICS_FIELD)]
		protected IWebElement TopicsField { get; set; }

		[FindsBy(How = How.XPath, Using = CLEAR_ALL_FILTERS_BUTTON)]
		protected IWebElement ClearAllFiltersButton { get; set; }

		[FindsBy(How = How.XPath, Using = FILTER_BUTTON)]
		protected IWebElement FilterButton { get; set; }

		protected IWebElement ProjectGroupInInList { get; set; }

		protected IWebElement ClientInInList { get; set; }

		protected IWebElement TopicInList { get; set; }

		protected IWebElement RemoveFilterButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string ADD_TM_BTN = "//div[contains(@data-bind,'createTm')]//a";
		protected const string CREATE_TM_DIALOG = "//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string TM_ROW = "//tr[contains(@class,'l-corpr__trhover clickable')]//span[text()='*#*']";
		protected const string TM_INFORMATION_FORM = "//span[string()='*#*']//..//..//following-sibling::tr[@class='js-tm-panel']//td//div";

		protected const string DELETE_BUTTON = "//tr[@class='js-tm-panel']//div[contains(@data-bind, 'deleteTranslationMemory')]";
		protected const string SAVE_TM_BUTTON = "//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class, 'js-tour-tm-save')]";

		protected const string TM_NAME = "(//th[contains(@data-sort-by,'Name')]//a)[1]";
		protected const string CREATION_DATE = "//th[contains(@data-sort-by,'CreatedDate')]//a";
		protected const string TM_LANGUAGES = "//td[@class='l-corpr__td tm']//span[string()='*#*']/parent::td/parent::tr//td[2]//span[string()='*##*']";
		protected const string TM_LANGUAGES_IN_TABLE = "//td[@class='l-corpr__td tm']//span[string()='*#*']/parent::td/parent::tr//td[2]//span";
		protected const string TARGET_LANG_ITEM = "(//div[contains(@class,'ui-multiselect-menu')])[4]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li//input[@value='*#*']";

		protected const string TM_EDIT_BUTTON = "//tr[@class='js-tm-panel']//div[contains(@data-bind, 'switchToEditing')]//a";
		protected const string TM_EDIT_NAME = "//tr[contains(@class,'js-tm-panel')]//input[contains(@data-bind, 'value: name')]";
		protected const string TM_EDIT_COMMENT = "//tr[contains(@class,'js-tm-panel')]//textarea";
		protected const string TM_EDIT_SAVE_BTN = "//tr[contains(@class,'js-tm-panel')]//div[contains(@data-bind,'click: save')]";
		protected const string TM_EDIT_TARGET_LANGUAGE = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[1]//div[contains(@class,'ui-multiselect')]/div";
		protected const string TM_EDIT_TARGET_LANGUAGE_LIST = "/html/body/div[21]/div";
		protected const string TM_EDIT_PROJECT_GROUPS = "//tr[contains(@class,'js-tm-panel')]//.//span[contains(@class,'ui-multiselect-txtdef')]";

		protected const string PROJECT_GROUPS_FIRST_IN_LIST = "//div[contains(@style, 'block')]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li[2]//label//input";
		protected const string PROJECT_GROUPS_FIELD = "//tr[contains(@class,'js-tm-panel')]//div[contains(@data-bind,'domainNames')]";

		protected const string ERROR_EDIT_NO_NAME = "//tr[contains(@class,'js-tm-panel')]//div[contains(@class,'tmpanel__error')]//p[contains(@data-message-id, 'name-required')]";
		protected const string ERROR_EDIT_EXIST_NAME = "//tr[contains(@class,'js-tm-panel')]//div[contains(@class,'tmpanel__error')]//p[contains(text(),'The name should be unique.')]";

		protected const string SEARCH_BUTTON = "//a[@title='Search TM by name']";
		protected const string SEARCH_TM_FIELD = "//input[@name='searchTMName']";

		protected const string FILE_IMPORT_NOTIFIER = "//div[contains(@class, 'notifications')]//span[contains(text(),'units imported from file')]";
		protected const string FILE_IMPORT_ERROR_NOTIFIER = "//div[contains(@class, 'notifications')]//span[contains(text(),'There was an error while importing translation units')]";
		protected const string FILE_IMPORT_ADDING_NOTIFIER = "//span[contains(text(), 'Adding translation units from the file')]";

		protected const string SEGMENT_SPAN = "//span[contains(@data-bind, 'unitCount')]";
		//UPDATE_TM_BUTTON связан с PRX-11525
		protected const string UPDATE_TM_BUTTON = "//tr[contains(@class,'js-tm-panel')]//a[contains(text(), 'Update ТМ')]";
		//ADD_TMX_BUTTON связан с PRX-11525
		protected const string ADD_TMX_BUTTON = "//tr[contains(@class,'js-tm-panel')]//a[contains(text(), 'Add ТМХ')]";
		protected const string UPDATE_TM_VALIDATION_ERROR_MESSAGE = "(//p[@class='js-error-invalid-file-extension' and text()='Please select a file with TMX extension'])[2]";

		protected const string EXPORT_BUTTON = "//span[contains(@data-bind,'exportTmx')]";

		protected const string PROJECT_GROUP_IN_LIST = "(//ul[contains(@class, 'ui-multiselect-checkboxes')]//span[text()='*#*']//preceding-sibling::span/input)[3]";
		protected const string CLIENTS_FIELD = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[4]/span";
		protected const string CLIENT_IN_LIST = "//span[contains(@class, 'js-dropdown')]/span[contains(text(),'*#*')]";
		protected const string TOPICS_FIELD = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[contains(@data-bind,'topicDropdown')]/div/div[1]";
		protected const string TOPIC_IN_LIST = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[contains(@data-bind,'topicDropdown')]/div/div//span[contains(@class,'nodetext') and text()='*#*']";
		protected const string CLEAR_ALL_FILTERS_BUTTON = "//img[contains(@class, 'filterClear js-clear-filter')]";
		protected const string FILTER_BUTTON = "//span[contains(@class, 'js-set-filter')]";
		protected const string REMOVE_FILTER_BUTTON = "//div[contains(@title, '*#*')]//em//img";

		#endregion
	}
}
