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

		public new TranslationMemoriesPage LoadPage()
		{
			if (!IsTranslationMemoriesPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница с памятью переводов");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Получить название клиента.
		/// </summary>
		public string GetClientName()
		{
			CustomTestContext.WriteLine(" Получить название клиента.");

			return ClientViewMode.Text;
		}

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

			return new NewTranslationMemoryDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickEditButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования ТМ.");
			EditButton.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Delete в информации о ТМ
		/// </summary>
		public DeleteTmDialog ClickDeleteButtonInTMInfo()
		{
			CustomTestContext.WriteLine("Нажать кнопку Delete.");
			DeleteButton.Click();

			return new DeleteTmDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Ввести название ТМ в поле поиска
		/// </summary>
		public TranslationMemoriesPage FillSearch(string translationMemoryName)
		{
			CustomTestContext.WriteLine("Ввести название ТМ {0} в поле поиска.", translationMemoryName);
			SearchField.SetText(translationMemoryName);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку поиска ТМ
		/// </summary>
		public TranslationMemoriesPage ClickSearchTMButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку поиска ТМ.");
			SearchButton.Click();

			return LoadPage();
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
			Driver.FindElement(By.XPath(TM_ROW.Replace("*#*", translationMemoryName))).ScrollDown();
			Driver.FindElement(By.XPath(TM_ROW.Replace("*#*", translationMemoryName))).ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Ввести имя в форме изменения ТМ
		/// </summary>
		public TranslationMemoriesPage AddTranslationMemoryName(string tmName)
		{
			CustomTestContext.WriteLine("Ввести имя в форме изменения ТМ.");
			EditNameField.SetText(tmName);

			return LoadPage();
		}

		/// <summary>
		/// Очистить имя в форме изменения ТМ
		/// </summary>
		public TranslationMemoriesPage CleanTranslationMemoryName()
		{
			CustomTestContext.WriteLine("Очистить имя в форме изменения ТМ.");
			EditNameField.Clear();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранить в форме редактирования TM
		/// </summary>
		public TranslationMemoriesPage ClickSaveTranslationMemoryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранить в форме редактирования TM.");
			// Обычный клик не работает по непонятным причинам, руками кликается нормально (javascript клик тоже не работает)
			SaveChangesButton.Scroll();
			SaveChangesButton.AdvancedClick();
			SaveChangesButton.DoubleClick();

			if (!IsEditionFormDisappeared())
			{
				throw new XPathLookupException("Ошибка: не исчезла форма редактирования ТМ");
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранить в форме редактирования TM
		/// </summary>
		public TranslationMemoriesPage ClickSaveTranslationMemoryButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранить в форме редактирования TM.");
			// Обычный клик не работает по непонятным причинам, руками кликается нормально (javascript клик тоже не работает)
			SaveChangesButton.Scroll();
			SaveChangesButton.AdvancedClick();
			SaveChangesButton.DoubleClick();

			return LoadPage();
		}

		/// <summary>
		/// Очистить поле комментария
		/// </summary>
		public TranslationMemoriesPage CleanComment()
		{
			CustomTestContext.WriteLine("Очистить поле комментария.");
			EditCommentField.Clear();

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле комментария
		/// </summary>
		public TranslationMemoriesPage AddComment(string text)
		{
			CustomTestContext.WriteLine("Заполнить поле комментария текстом {0}.", text);
			EditCommentField.SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Нажать на поле с языком перевода в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToTargetLanguages()
		{
			CustomTestContext.WriteLine("Нажать на поле с языком перевода в форме редактирования ТМ");
			TranslationMemoryTargetLanguagesField.Click();

			return LoadPage();
		}
		
		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		public TranslationMemoriesPage ClickTargetLanguageOption(Language? language)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода {0}", language);
			var languageValue = (int)language;
			var TargetLanguages = Driver.FindElements(By.XPath(TARGET_LANG_ITEM.Replace("*#*", languageValue.ToString())));
			for (int i = 0; i < TargetLanguages.Count; i++)
			{
				TargetLanguages[i].ScrollDown();
				try
				{
					TargetLanguages[i].Click();
				}
				catch (ElementNotVisibleException)
				{
					continue;
				}
			}
			
			return LoadPage();
		}

		///<summary>
		/// Нажать на поле с клиентами в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToClientsField()
		{
			CustomTestContext.WriteLine("Нажать на поле с клиентами в форме редактирования ТМ");
			ClientsField.Click();

			return LoadPage();
		}

		///<summary>
		/// Нажать на поле с темами в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToTopicsField()
		{
			CustomTestContext.WriteLine("Нажать на поле с темами в форме редактирования ТМ");
			TopicsField.Click();

			return LoadPage();
		}

		///<summary>
		/// Нажать на поле с группами проектов в форме редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage ClickToProjectGroupsField()
		{
			CustomTestContext.WriteLine("Нажать на поле с группами проектов в форме редактирования ТМ");
			ProjectGroupsList.Click();

			return LoadPage();
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

			return LoadPage();
		}

		///<summary>
		/// Выбрать группу проектов в списке
		/// </summary>
		/// <param name="projectGroup">Имя выбранной группы проектов</param>
		public TranslationMemoriesPage SelectProjectGroup(string projectGroup)
		{
			CustomTestContext.WriteLine("Выбрать группу проектов {0} в списке", projectGroup);
			ProjectGroupInInList = Driver.SetDynamicValue(How.XPath, PROJECT_GROUP_IN_LIST, projectGroup);
			ProjectGroupInInList.Scroll();
			ProjectGroupInInList.Click();

			return LoadPage();
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

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по именам, ожидая алерт.
		/// </summary>
		public void ClickSortByTMNameAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по именамм, ожидая алерт.");
			TMName.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате созданиям, ожидая алерт.
		/// </summary>
		public void ClickSortByCreationDateAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате созданиям, ожидая алерт.");
			CreationDate.Click();
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
		public ImportTmxDialog ClickUpdateTmButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Update TM'");
			UpdateTmButton.Click();

			return new ImportTmxDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Export'
		/// </summary>
		public TranslationMemoriesPage ClickExportButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Export'");
			ExportButton.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Add TMX
		/// </summary>
		public ImportTmxDialog ClickAddTmxButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Add TMX'");
			AddTmxButton.Click();

			return new ImportTmxDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку очистки всех фильтров
		/// </summary>
		public TranslationMemoriesPage ClickClearAllFiltersButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку очистки всех фильтров");
			ClearAllFiltersButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Filter'
		/// </summary>
		public TranslationMemoriesFilterDialog ClickFilterButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Filter'");
			FilterButton.Click();

			return new TranslationMemoriesFilterDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун исходного языка.
		/// </summary>
		public TranslationMemoriesPage ClickSourceLanguageDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун исходного языка.");
			SourceLanguageDropdown.WaitTargetAndClick();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун целевого языка.
		/// </summary>
		public TranslationMemoriesPage ClickTargetLanguageDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун целевого языка.");
			TargetLanguageDropdown.Scroll();
			TargetLanguageDropdown.WaitTargetAndClick();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун целевого языка.
		/// </summary>
		public TranslationMemoriesPage ClickTarget()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун целевого языка.");
			TargetLanguageDropdown.Click();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на исходный язык. 
		/// </summary>
		/// <param name="language">исходный язык</param>
		public TranslationMemoriesPage ClickSourceLanguageOption(Language language)
		{
			CustomTestContext.WriteLine("Нажать на исходный язык.");
			var languageId = (int)language;
			SourceLanguageOption = Driver.SetDynamicValue(How.XPath, SOURCE_LANGUAGE_OPTION, languageId.ToString());
			SourceLanguageOption.ScrollAndClick();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Вернуть, действуют ли сейчас какие-то фильтры
		/// </summary>
		public bool GetFiltersIsExist()
		{
			CustomTestContext.WriteLine("Вернуть, действуют ли сейчас какие-то фильтры");
			var filtersIsExist = Driver.WaitUntilElementIsDisplay(By.XPath(CLEAR_ALL_FILTERS_BUTTON), timeout: 3);
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

			return LoadPage();
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

			return LoadPage();
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

			return LoadPage();
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

			return LoadPage();
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

			return LoadPage();
		}

		/// <summary>
		/// Выполнить поиск ТМ
		/// </summary>
		/// <param name="tmName">имя ТМ</param>
		public TranslationMemoriesPage SearchForTranslationMemory(string tmName)
		{
			FillSearch(tmName);
			ClickSearchTMButton();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать исходный язык.
		/// </summary>
		/// <param name="language">исходный язык</param>
		public TranslationMemoriesPage SelectSourceLanguage(Language language)
		{
			ClickSourceLanguageDropdown();
			ClickSourceLanguageOption(language);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать целевой язык.
		/// </summary>
		/// <param name="language">исходный язык</param>
		public TranslationMemoriesPage SelectTargetLanguage(List<Language> languages)
		{
			ClickTargetLanguageDropdown();
			for (int i = 0; i < languages.Count; i++)
			{
				ClickTargetLanguageOption(languages[i]);
			}
			ClickTargetLanguageDropdown();

			return LoadPage();
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

			return LoadPage();
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
				ClickTargetLanguageOption(addTargetLanguage);
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

			if (isErrorExpecting)
			{
				ClickSaveTranslationMemoryButtonExpectingError();
			}
			else
			{
				ClickSaveTranslationMemoryButton();
				CloseTranslationMemoryInformation(tmName);
			}

			return LoadPage();
		}

		/// <summary>
		/// Открыть диалог импорта Tmx (режим обновления TM)
		/// </summary>
		/// <param name="tmName">имя памяти перевода</param>
		/// <param name="update">true - обновление (замена), false - добавление</param>
		public ImportTmxDialog OpenImportTmxDialog(string tmName, bool update)
		{
			OpenTranslationMemoryInformation(tmName);

			if (update)
			{
				ClickUpdateTmButton();
			}
			else
			{
				ClickAddTmxButton();
			}

			return new ImportTmxDialog(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница ТМ
		/// </summary>
		public bool IsTranslationMemoriesPageOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(TRANSLATION_MEMORIES_TABLE));
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

			return Driver.ElementIsDisplayed(By.XPath(TM_INFORMATION_FORM.Replace("*#*", tmName)));
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
		/// Получить целевые языки памяти перевода.
		/// </summary>
		/// <param name="translationMemoryName">имя памяти перевода</param>
		public List<string> GetTranslationMemoryTargetLanguages(string translationMemoryName)
		{
			CustomTestContext.WriteLine("Получить целевые языки памяти перевода {0}.", translationMemoryName);
			TranslationMemoryColumn = Driver.SetDynamicValue(How.XPath, TM_LANGUAGES_IN_TABLE, translationMemoryName);
			var languagesColumn = TranslationMemoryColumn.Text;
			var languagesList = languagesColumn.Split(new[] { '>' }).ToList();

			if (languagesList.Count < 2)
			{
				throw new InvalidElementStateException("Произошла ошибка: Неверное количество языков.");
			}
			var targetLanguages = languagesList[1].Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
			targetLanguages.Sort();

			return targetLanguages;
		}

		/// <summary>
		/// Получить исходный язык памяти перевода.
		/// </summary>
		/// <param name="translationMemoryName">имя памяти перевода</param>
		public string GetTranslationMemorySourceLanguage(string translationMemoryName)
		{
			CustomTestContext.WriteLine("Получить исходный язык памяти перевода {0}.", translationMemoryName);
			TranslationMemoryColumn = Driver.SetDynamicValue(How.XPath, TM_LANGUAGES_IN_TABLE, translationMemoryName);
			var languagesColumn = TranslationMemoryColumn.Text;
			var languagesList = languagesColumn.Split(new[] { '>' }).ToList();

			if (languagesList.Count < 2)
			{
				throw new InvalidElementStateException("Произошла ошибка: Неверное количество языков.");
			}

			return languagesList[0].Trim();
		}

		/// <summary>
		/// Проверить, что сообщение о окончании импорта TMX файла появилось.
		/// </summary>
		public bool IsFileImportCompleteNotifierDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение о окончании импорта TMX файла появилось");

			return Driver.WaitUntilElementIsDisplay(By.XPath(FILE_IMPORT_NOTIFIER));
		}

		/// <summary>
		/// Проверить, что сообщение о процессе импорта TMX файла исчезло
		/// </summary>
		public bool IsFileImportAddingNotifierDisappeared(int timeout = 60)
		{
			CustomTestContext.WriteLine("Проверить, что сообщение о процессе импорта TMX файла исчезло");

			if (!Driver.WaitUntilElementIsDisappeared(By.XPath(FILE_IMPORT_ADDING_NOTIFIER), timeout))
			{
				RefreshPage<WorkspacePage>();
			}

			return Driver.WaitUntilElementIsDisappeared(By.XPath(FILE_IMPORT_ADDING_NOTIFIER), timeout);
		}

		/// <summary>
		/// Проверить, что появилось сообщение об ошибке во время импорта TMX файла
		/// </summary>
		public bool IsFileImportFailedNotifierDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение об ошибке во время импорта TMX файла");

			return Driver.WaitUntilElementIsDisplay(By.XPath(FILE_IMPORT_ERROR_NOTIFIER), timeout: 45);
		}

		/// <summary>
		/// Проверить, что для ТМ указана группа проектов
		/// </summary>
		public bool IsProjectGroupSelectedForTM(string projectGroup)
		{
			CustomTestContext.WriteLine("Проверить, что для ТМ указана группа проектов {0}", projectGroup);

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

		/// <summary>
		/// Проверить, что появилось сообщение 'The language cannot be changed because TM is already used in SmartCAT.'.
		/// </summary>
		public bool IsImpossibleToChangeLanguageErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'The language cannot be changed because TM is already used in SmartCAT.'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(IMPOSSIBLE_TO_CHANGE_LANGUAGE_ERROR));
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

		[FindsBy(How = How.XPath, Using = TAGET_LANGUAGE_DROPDOWN)]
		protected IWebElement TargetLanguageDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = FILTER_BUTTON)]
		protected IWebElement FilterButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENT_VIEW_MODE)]
		protected IWebElement ClientViewMode { get; set; }
		[FindsBy(How = How.XPath, Using = SOURCE_LANGUAGE_DROPDOWN)]
		protected IWebElement SourceLanguageDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = SOURCE_LANGUAGE_OPTION)]
		protected IWebElement SourceLanguageOption { get; set; }

		protected IWebElement ProjectGroupInInList { get; set; }

		protected IWebElement ClientInInList { get; set; }

		protected IWebElement TopicInList { get; set; }
		protected IWebElement RemoveFilterButton { get; set; }
		protected IWebElement TranslationMemoryColumn { get; set; }
		protected IWebElement TmInformationForm { get; set; }

		#endregion

		#region Описания XPath элементов
		protected const string TAGET_LANGUAGE_DROPDOWN = "//span[contains(@class, 'trgtlang')]//div";
		protected const string TRANSLATION_MEMORIES_TABLE = "//table[contains(@class, 'translationmemories')]";
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
		protected const string TARGET_LANG_ITEM = "//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li//input[@value='*#*']";

		protected const string SOURCE_LANGUAGE_DROPDOWN = "//select[contains(@data-bind,'allSourceLanguagesList')]//following-sibling::span";
		protected const string SOURCE_LANGUAGE_OPTION = "//span[@data-id='*#*']";

		protected const string TM_EDIT_BUTTON = "//tr[@class='js-tm-panel']//div[contains(@data-bind, 'switchToEditing')]//a";
		protected const string TM_EDIT_NAME = "//tr[contains(@class,'js-tm-panel')]//input[contains(@data-bind, 'value: name')]";
		protected const string TM_EDIT_COMMENT = "//tr[contains(@class,'js-tm-panel')]//textarea";
		protected const string TM_EDIT_SAVE_BTN = "//tr[contains(@class,'js-tm-panel')]//div[contains(@data-bind,'click: save')]//a";
		protected const string TM_EDIT_TARGET_LANGUAGE = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[1]//div[contains(@class,'ui-multiselect')]/div";
		protected const string TM_EDIT_TARGET_LANGUAGE_LIST = "/html/body/div[21]/div";
		protected const string TM_EDIT_PROJECT_GROUPS = "//tr[contains(@class,'js-tm-panel')]//.//span[contains(@class,'ui-multiselect-txtdef')]";

		protected const string PROJECT_GROUPS_FIRST_IN_LIST = "//div[contains(@style, 'block')]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li[1]//label//input";
		protected const string PROJECT_GROUPS_FIELD = "//tr[contains(@class,'js-tm-panel')]//div[contains(@data-bind,'domainNames')]";

		protected const string ERROR_EDIT_NO_NAME = "//tr[contains(@class,'js-tm-panel')]//div[contains(@class,'tmpanel__error')]//p[contains(@data-message-id, 'name-required')]";
		protected const string ERROR_EDIT_EXIST_NAME = "//tr[contains(@class,'js-tm-panel')]//div[contains(@class,'tmpanel__error')]//p[contains(text(),'A translation memory with this name already exists.')]";

		protected const string SEARCH_BUTTON = "//a[@title='Search TM by name']";
		protected const string SEARCH_TM_FIELD = "//input[@name='searchTMName']";

		protected const string FILE_IMPORT_NOTIFIER = "//div[contains(@class, 'notifications')]//span[contains(text(),'units imported from file')]";
		protected const string FILE_IMPORT_ERROR_NOTIFIER = "//div[contains(@class, 'notifications')]//span[contains(text(),'error') and contains(text(),'importing')]";
		protected const string FILE_IMPORT_ADDING_NOTIFIER = "//span[contains(text(), 'Adding translation units from the file')]";

		protected const string SEGMENT_SPAN = "//span[contains(@data-bind, 'unitCount')]";
		//UPDATE_TM_BUTTON связан с PRX-11525
		protected const string UPDATE_TM_BUTTON = "//tr[contains(@class,'js-tm-panel')]//a[contains(text(), 'Update ТМ')]";
		//ADD_TMX_BUTTON связан с PRX-11525
		protected const string ADD_TMX_BUTTON = "//tr[contains(@class,'js-tm-panel')]//a[contains(text(), 'Add ТМХ')]";
		protected const string UPDATE_TM_VALIDATION_ERROR_MESSAGE = "(//p[@class='js-error-invalid-file-extension' and text()='Please select a file with TMX extension'])[2]";

		protected const string EXPORT_BUTTON = "//span[contains(@data-bind,'exportTmx')]";

		protected const string PROJECT_GROUP_IN_LIST = "(//ul[contains(@class, 'ui-multiselect-checkboxes')]//span[text()='*#*']//preceding-sibling::span/input)[2]";
		protected const string CLIENTS_FIELD = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[5]/span";
		protected const string CLIENT_IN_LIST = "//span[contains(@class, 'js-dropdown')]/span[contains(text(),'*#*')]";
		protected const string TOPICS_FIELD = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[contains(@data-bind,'topicDropdown')]/div/div[1]";
		protected const string TOPIC_IN_LIST = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[contains(@data-bind,'topicDropdown')]/div/div//span[contains(@class,'nodetext') and text()='*#*']";
		protected const string CLEAR_ALL_FILTERS_BUTTON = "//i[contains(@class, 'filterClear js-clear-filter')]";
		protected const string FILTER_BUTTON = "//div[contains(@class, 'js-set-filter')]";
		protected const string REMOVE_FILTER_BUTTON = "//div[contains(@title, '*#*')]//em//i";
		protected const string CLIENT_VIEW_MODE = "//div[contains(@data-bind,'clientName')]";
		protected const string IMPOSSIBLE_TO_CHANGE_LANGUAGE_ERROR = "//p[contains(text(), 'The language cannot be changed because TM is already used in SmartCAT')]";
		#endregion
	}
}
