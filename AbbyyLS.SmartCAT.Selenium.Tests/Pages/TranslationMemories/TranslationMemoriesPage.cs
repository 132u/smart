using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;
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
		/// Нажать кнопку Delete в диалоге подтверждения удаления ТМ
		/// </summary>
		public TranslationMemoriesPage ClickDeleteButtonInConfirmationDialog()
		{
			CustomTestContext.WriteLine("Нажать кнопку Delete в диалоге подтверждения удаления ТМ.");
			DeleteButtonInConfirmationDialog.Click();

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
		public TranslationMemoriesPage SelectTargetLanguage(Language language)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода {0}", language);
			TargetLanguage = Driver.SetDynamicValue(How.XPath, TARGET_LANG_ITEM, ((int) language).ToString());
			TargetLanguage.Click();

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

		/// <summary>
		/// Проверить, что группа представлена в списке
		/// </summary>
		/// <param name="projectGroup">имя группы проектов</param>
		public TranslationMemoriesPage AssertProjectGroupInListDisplay(string projectGroup)
		{
			CustomTestContext.WriteLine("Проверить, что группа {0} представлена в списке", projectGroup);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(PROJECT_GROUP_IN_LIST.Replace("*#*", projectGroup))),
				"Произошла ошибка:\n группа проектов {0} отсутствует в списке.", projectGroup);

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
		/// Проверить, что ТМ представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryExists(string tmName)
		{
			CustomTestContext.WriteLine("Проверить, что ТМ {0} представлена в списке.", tmName);
			Driver.FindElement(By.XPath(TM_ROW.Replace("*#*", tmName))).Scroll();

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TM_ROW.Replace("*#*", tmName))),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ.", tmName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, представлена ли ТМ в списке
		/// </summary>
		public bool TranslationMemoryExists(string tmName)
		{
			CustomTestContext.WriteLine("Проверить, что ТМ {0} представлена в списке.", tmName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(TM_ROW.Replace("*#*", tmName)));
		}

		/// <summary>
		/// Проверить, что ТМ не представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryNotExists(string tmName)
		{
			CustomTestContext.WriteLine("Проверить, что ТМ {0} не представлена в списке.", tmName);

			Assert.IsFalse(Driver.ElementIsDisplayed(By.XPath(TM_ROW.Replace("*#*", tmName))),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ.", tmName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, открыта ли информация о ТМ
		/// </summary>
		public bool IsTranslationMemoryInformationOpen(string tmName)
		{
			CustomTestContext.WriteLine("Получить, открыта ли информация о ТМ.");

			return Driver.ElementIsDisplayed(By.XPath(TM_INFORMATION_FORM.Replace("*#*", tmName)));
		}

		/// <summary>
		/// Проверить, что диалог подтверждения удаления ТМ появился
		/// </summary>
		public TranslationMemoriesPage AssertDeleteConfirmatonDialogPresent()
		{
			CustomTestContext.WriteLine("Проверить, что диалог подтверждения удаления ТМ появился.");
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
			CustomTestContext.WriteLine("Проверить, что диалог подтверждения удаления ТМ закрылся.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_CONFIRMATION_DIALOG)),
				"Произошла ошибка:\n диалог подтверждения удаления ТМ не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Проверить наличие ошибки о пустом названии при редактировании ТМ
		/// </summary>
		public TranslationMemoriesPage AssertNoNameErrorAppear()
		{
			CustomTestContext.WriteLine("Проверить наличие ошибки о пустом названии при редактировании ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EDIT_NO_NAME)),
				"Ошибка: не появилось сообщение о пустом названии при редактировании ТМ.");

			return GetPage();
		}

		/// <summary>
		/// Проверить наличие ошибки о существующем имени при некорректном редактировании имени ТМ
		/// </summary>
		public TranslationMemoriesPage AssertExistingNameErrorAppeared()
		{
			CustomTestContext.WriteLine("Проверить наличие ошибки о существующем имени при некорректном редактировании имени ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EDIT_EXIST_NAME)),
				"Ошибка: не появилось сообщение об ошибки имени при редактировании ТМ.");

			return GetPage();
		}

		/// <summary>
		/// Проверить исчезновение формы редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage AssertEditionFormDisappeared()
		{
			CustomTestContext.WriteLine("Проверить исчезновение формы редактирования ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(TM_EDIT_SAVE_BTN)),
				"Ошибка: не исчезла форма редактирования ТМ");

			return GetPage();
		}

		/// <summary>
		/// Проверить появление формы редактирования ТМ
		/// </summary>
		public TranslationMemoriesPage AssertEditionFormDisplayed()
		{
			CustomTestContext.WriteLine("Проверить появление формы редактирования ТМ.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TM_EDIT_SAVE_BTN)),
				"Ошибка: не появилась форма редактирования ТМ");

			return GetPage();
		}

		/// <summary>
		/// Проверить, есть ли на странице комментарий
		/// </summary>
		public TranslationMemoriesPage AssertCommentExists(string text)
		{
			CustomTestContext.WriteLine("Проверить, есть ли на странице комментарий {0}.", text);

			Assert.IsTrue(EditCommentField.GetAttribute("value") == text, "Ошибка: комментарий {0} не найден.", text);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог создания ТМ закрылся
		/// </summary>
		public TranslationMemoriesPage AssertNewTMDialogDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что диалог создания ТМ закрылся.");

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
			CustomTestContext.WriteLine("Проверить, указаны ли для ТМ {0} корректные языки: source = {1}, target = {2}.",
				translationMemoryName, sourceLanguage, targetLanguages);
			var TM = Driver.FindElement(By.XPath(TM_LANGUAGES_IN_TABLE.Replace("*#*", translationMemoryName)));
			var languagesColumn = TM.Text;
			var languagesList = languagesColumn.Split(new[] {'>'}).ToList();

			Assert.AreEqual(2, languagesList.Count(),
				"Произошла ошибка:\n неверное количество элементов в списке с source и target языками.");

			var actualSource = languagesList[0].Trim();
			var actualTargetList = languagesList[1].Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries).ToList();

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
			CustomTestContext.WriteLine("Проверить, что сообщение о окончании импорта TMX файла появилось.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(FILE_IMPORT_NOTIFIER), timeout: 45),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось.");

			return GetPage();
		}

		public TranslationMemoriesPage AssertFileImportAddingNotifierDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение о процессе импорта TMX файла появилось.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(FILE_IMPORT_ADDING_NOTIFIER), timeout: 45),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение об ошибке во время импорта TMX файла
		/// </summary>
		public TranslationMemoriesPage AssertFileImportFailedNotifierDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение об ошибке во время импорта TMX файла");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(FILE_IMPORT_ERROR_NOTIFIER), timeout: 15),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что для ТМ указана группа проектов
		/// </summary>
		public TranslationMemoriesPage AssertProjectGroupSelectedForTM(string translationMemoryName, string projectGroup)
		{
			CustomTestContext.WriteLine("Проверить, что для ТМ {0} указана группа проектов {1}", translationMemoryName, projectGroup);

			Assert.IsTrue(ProjectGroupsField.Text.Contains(projectGroup),
				string.Format("Произошла ошибка:\n неверно указана группа проектов для ТМ {0}.", translationMemoryName));

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
		public int UnitsCount()
		{
			CustomTestContext.WriteLine("Получить количество юнитов");
			var unitsCountText = SegmentSpan.Text;

			CustomTestContext.WriteLine("Полученное количество юнитов: {0}", unitsCountText);
			int result;

			Assert.IsTrue(int.TryParse(unitsCountText, out result),
				string.Format("Ошибка: невозможно преобразовать в число: {0}", unitsCountText));

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
		/// Ввести имя файла в окне 'Import TMX files'
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public TranslationMemoriesPage EnterFileName(string fileName)
		{
			CustomTestContext.WriteLine("Ввести имя файла в окне 'Import TMX files'");

			try
			{
				Driver.ExecuteScript("$(\"input:file\").removeClass(\"g-hidden\").css(\"opacity\", 100)");
				ImportFileInput.SendKeys(fileName);
				//Чтобы не появилось валидационной ошибки, необходимо,
				//помимо загрузки файла, заполнить следующий элемент
				Driver.ExecuteScript(string.Format("document.getElementsByClassName('g-iblock g-bold l-editgloss__filelink js-filename-link')[1].innerHTML='{0}'", Path.GetFileName(fileName)));
			}
			catch (Exception)
			{
				CustomTestContext.WriteLine("Произошла ошибка:\n не удалось изменить параметры элементов.");
				throw;
			}

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Import' в окне 'Import TMX files'
		/// </summary>
		public TranslationMemoriesPage ClickImportButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Import' в окне 'Import TMX files'");
			ImportButton.Click();

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
		/// Нажать кнопку подтверждения замены ТМ в окне импорта
		/// </summary>
		public TranslationMemoriesPage ClickConfirmReplacementButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку подтверждения замены ТМ в окне импорта");
			ConfirmReplacementButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось окно подтверждения замены при импорте
		/// </summary>
		public TranslationMemoriesPage AssertConfirmReplacementMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось окно подтверждения замены при импорте");

			Assert.IsFalse(Driver.WaitUntilElementIsDisplay(By.XPath(UPDATE_TM_CONFIRM_REPLACEMENT)),
				"Произошла ошибка:\n не появилось окно подтверждения замены.");

			return this;
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
		/// Проверить появление валидационной ошибки при импорте
		/// </summary>
		public TranslationMemoriesPage AssertImportValidationErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить появление валидационной ошибки при импорте");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(UPDATE_TM_VALIDATION_ERROR_MESSAGE)),
				"Произошла ошибка:\n не сработала валидация");

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

			var filtersIsExist = Driver.GetIsElementExist(By.XPath(CLEAR_ALL_FILTERS_BUTTON));

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

		[FindsBy(How = How.XPath, Using = SEGMENT_SPAN)]
		protected IWebElement SegmentSpan { get; set; }

		[FindsBy(How = How.XPath, Using = UPDATE_TM_BUTTON)]
		protected IWebElement UpdateTmButton { get; set; }

		[FindsBy(How = How.XPath, Using = UPDATE_TM_IMPORT_FILE_INPUT)]
		protected IWebElement ImportFileInput { get; set; }

		[FindsBy(How = How.XPath, Using = UPDATE_TM_IMPORT_BUTTON)]
		protected IWebElement ImportButton { get; set; }

		[FindsBy(How = How.XPath, Using = UPDATE_TM_CONFIRM_REPLACEMENT)]
		protected IWebElement ConfirmReplacementButton { get; set; }

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
		protected const string TARGET_LANG_ITEM = "(//div[contains(@class,'ui-multiselect-menu')])[4]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li//input[@value='*#*']";

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
		protected const string FILE_IMPORT_ERROR_NOTIFIER = "//div[contains(@class, 'notifications')]//span[contains(text(),'There was an error while importing translation units')]";
		protected const string FILE_IMPORT_ADDING_NOTIFIER = "//span[contains(text(), 'Adding translation units from the file')]";

		protected const string SEGMENT_SPAN = "//span[contains(@data-bind, 'unitCount')]";
		//UPDATE_TM_BUTTON связан с PRX-11525
		protected const string UPDATE_TM_BUTTON = "//tr[contains(@class,'js-tm-panel')]//a[contains(text(), 'Update ТМ')]";
		protected const string UPDATE_TM_IMPORT_FILE_INPUT = "//h2[text()='Import TMX Files']//..//..//input[@name='file']";
		protected const string UPDATE_TM_IMPORT_BUTTON = "(//h2[text()='Import TMX Files']//..//..//input[@value='Import'])[2]";
		protected const string UPDATE_TM_CONFIRM_REPLACEMENT = "//span[text()='Confirmation']//..//..//..//input[@value='Continue']";
		//ADD_TMX_BUTTON связан с PRX-11525
		protected const string ADD_TMX_BUTTON = "//tr[contains(@class,'js-tm-panel')]//a[contains(text(), 'Add ТМХ')]";
		protected const string UPDATE_TM_VALIDATION_ERROR_MESSAGE = "(//p[@class='js-error-invalid-file-extension' and text()='Please select a file with TMX extension'])[2]";

		protected const string EXPORT_BUTTON = "//span/a[contains(@data-bind,'exportTmx')]";

		protected const string PROJECT_GROUP_IN_LIST = "(//ul[contains(@class, 'ui-multiselect-checkboxes')]//span[text()='*#*']//preceding-sibling::span/input)[3]";
		protected const string CLIENTS_FIELD = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[4]/span";
		protected const string CLIENT_IN_LIST = "//span[contains(@class, 'js-dropdown')]/span[contains(text(),'*#*')]";
		protected const string TOPICS_FIELD = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[contains(@data-bind,'topicDropdown')]/div/div";
		protected const string TOPIC_IN_LIST = "//tr[contains(@class,'js-tm-panel')]//td[2]//div[contains(@data-bind,'topicDropdown')]/div/div//span[contains(@class,'nodetext') and text()='*#*']";
		protected const string CLEAR_ALL_FILTERS_BUTTON = "//img[contains(@class, 'filterClear js-clear-filter')]";
		protected const string FILTER_BUTTON = "//span[contains(@class, 'js-set-filter')]";
		protected const string REMOVE_FILTER_BUTTON = "//div[contains(@title, '*#*')]//em//img";
	}
}
