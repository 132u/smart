using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы глоссариев
	/// </summary>
	public class GlossaryPageHelper : CommonHelper
	{
		public GlossaryPageHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
		}

		public void WaitPageLoad()
		{
			Logger.Trace("Проверка успешной загрузки страницы глоссария");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(ADD_CONCEPT_XPATH)), 
				"Ошибка: не зашли в глоссарий");
		}

		public void OpenEditGlossaryList()
		{
			Logger.Debug("Открыть редактирование глоссария");
			ClickElement(By.XPath(OPEN_EDIT_GLOSSARY_LIST_XPATH));
		}

		public void OpenEditStructureForm()
		{
			Logger.Debug("Открыть форму редактирования структуры");
			ClickElement(By.XPath(OPEN_EDIT_STRUCTURE_FORM_BTN_XPATH));
		}

		public void ClickNewItemBtn()
		{
			Logger.Debug("Нажать кнопку New Item");
			ClickElement(By.XPath(ADD_CONCEPT_XPATH));
		}

		public void NewItemClickDomainField()
		{
			Logger.Debug("Кликнуть по полю Domain в новом термине");
			ClickElement(By.XPath(NEW_ITEM_DOMAIN_FIELD_XPATH));
		}


		public void ClickOpenProperties()
		{
			Logger.Debug("Кликнуть кнопку открытия настроеки");
			ClickElement(By.XPath(OPEN_EDIT_PROPERTIES_FORM_BTN_XPATH));
		}

		public void FillItemTermsExtended(string text)
		{
			Logger.Debug(string.Format("Заполнить термины в расширенной версии. Текст термина: {0}", text));

			Logger.Trace("Получение списка языков");
			var termList = GetElementList(By.XPath(ITEM_ADD_EXTENDED_XPATH));

			for (var i = 0; i < termList.Count; ++i)
			{
				Logger.Debug("Нажать кнопку Add");
				termList[i].Click();
				Logger.Debug("Очистить поле и ввести термин");
				ClearAndAddText(By.XPath(ITEM_TERMS_EXTENDED_XPATH + "[" + (i + 1) + "]" + ITEM_EDITOR_INPUT_XPATH), text);
			}
		}

		public void EditTermsExtended(string text)
		{
			Logger.Debug(string.Format("Изменить термины в расширенной версии. Текст термина: {0}", text));

			Logger.Trace("Получение списка языков");
			var termList = GetElementList(By.XPath(ITEM_TERMS_EXTENDED_XPATH + TERM_ROW_XPATH));

			for (var i = 0; i < termList.Count; ++i)
			{
				Logger.Debug("Нажать кнопку Add");
				termList[i].Click();
				Logger.Debug("Очистить поле и ввести термин");
				ClearAndAddText(By.XPath(ITEM_TERMS_EXTENDED_XPATH + "[" + (i + 1) + "]" + ITEM_EDITOR_INPUT_XPATH), text);
			}
		}

		public void AssertionConceptTableAppear()
		{
			Logger.Debug("Проверка появления таблицы для ввода нового термина в нерасширенном режиме");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CONCEPT_TABLE_XPATH)),
				"Ошибка: таблица для ввода нового термина в нерасширенном режиме не появилась");
		}

		public void FillTerm(int termNum, string text)
		{
			Logger.Debug(string.Format("Заполнить термин. Номер (первый язык или второй): {0}; текст: {1}", termNum, text));
			ClearAndAddText(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + termNum + "]" + INPUT_TERM_XPATH), text);
		}

		public void ClickSaveTermin()
		{
			Logger.Debug("Кликнуть кнопку сохранения термина");
			ClickElement(By.XPath(EDIT_CONCEPT_SAVE_BTN_XPATH));
		}

		public void AssertionConceptGeneralSave()
		{
			Logger.Trace("Ожидание сохранения термина в обычном режиме");

			Assert.IsTrue(WaitUntilDisappearElement(By.XPath(EDIT_CONCEPT_SAVE_BTN_XPATH)), 
				"Ошибка: термин не сохранился");
		}

		public void AssertionConceptGeneralDelete()
		{
			Logger.Trace("Проверка успешного удаления термина в обычном режиме");

			Assert.IsTrue(WaitUntilDisappearElement(By.XPath(DELETE_CONCEPT_BTN_XPATH)),
				"Ошибка: термин не был удален");
		}

		public int GetConceptCount()
		{
			Logger.Debug("Получить количество терминов");

			SetDriverTimeoutMinimum();
			var result = GetElementsCount(By.XPath(CONCEPT_ROW_XPATH));
			SetDriverTimeoutDefault();

			return result;
		}

		public void ClickSaveExtendedConcept()
		{
			Logger.Debug("Прокручиваем страницу вверх, чтобы стало видно кнопку сохранения термина.");
			var saveButton = Driver.FindElement(By.XPath(SAVE_EXTENDED_BTN_XPATH));
			((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);  window.scrollBy(0,-200);", saveButton);
			Logger.Debug("Кликнить Сохранить в расширенной форме термина");
			ClickElement(By.XPath(SAVE_EXTENDED_BTN_XPATH));
		}

		public void AssertionConceptSave()
		{
			Logger.Trace("Проверка появления открытого сохраненного термина"); 
			// Не стали использовать WaitUntilDisplayElement,т.к. на тимсити элемент закрывается сползающим хэдером. 
			Assert.IsTrue(GetIsElementExist(By.XPath(OPENED_CONCEPT_ROW_XPATH)), "Ошибка: термин не сохранился");
		}

		public void AssertionDocumentUploaded(string fieldName)
		{
			Logger.Debug(string.Format("Проверить, что документ успешно загружен. Поле, куда записывается документ: {0}", fieldName));

			var xPath = GetCustomFieldImageXPath(fieldName) + FIELD_MEDIA_CONTAINS_XPATH;

			Assert.IsTrue(
				WaitUntilDisplayElement(By.XPath(xPath), maxWait: 60),
				"Ошибка: документ не загрузился");
		}

		public void AssertionIsExistCustomFieldBool(string fieldName)
		{
			Logger.Trace(string.Format("Проверить, что пользовательское поле {0} существует при создании термина", fieldName));
				
			Assert.IsTrue(
				GetIsElementDisplay(By.XPath(GetCustomFieldBoolXPath(fieldName))),
				"Ошибка: поле не появилось");
		}

		public void ClickCustomFieldBool(string fieldName)
		{
			Logger.Debug(string.Format("Нажать checkbox пользовательского поля {0}", fieldName));

			ClickElement(By.XPath(GetCustomFieldBoolXPath(fieldName) + CUSTOM_FIELD_BOOL_CHECKBOX_XPATH));
		}

		public void AssertionIsCustomBooleanChecked(string fieldName)
		{
			Logger.Trace(string.Format("Проверить, что пользовательское поле {0} отмечена галкой", fieldName));

			Assert.IsTrue(GetElementAttribute(By.XPath(GetCustomFieldBoolXPath(fieldName) + INPUT_FIELD_VALUE_XPATH), "value") == "true",
				"Ошибка: в поле неверное значение");
		}

		public void AssertionIsExistCustomField(string fieldName)
		{
			Logger.Trace(string.Format("Проверить, что существует пользовательское поле {0}", fieldName));

			Assert.IsTrue(GetIsElementDisplay(By.XPath(GetCustomFieldXPath(fieldName))),
				"Ошибка: пользовательское поле не появилось");
		}

		public void AssertionIsExistCustomFieldError(string fieldName)
		{
			Logger.Trace(string.Format("Проверить, что поле {0} отмечено ошибкой", fieldName));

			Assert.IsTrue(GetIsElementDisplay(By.XPath(GetCustomFieldErrorXPath(fieldName))),
				"Ошибка: обязательное поле не отмечено ошибкой");
		}

		public void ClickCustomFieldMultiSelect(string fieldName)
		{
			Logger.Debug(string.Format("Кликнуть на поле множественного выбора. Имя поля {0}", fieldName));
			ClickElement(By.XPath(GetCustomFieldXPath(fieldName) +
				MULTISELECT_FIELD_VALUE_XPATH));
		}

		public void SelectItemMultiSelect(string item)
		{
			Logger.Debug(string.Format("Выбрать элемент {0} множественного выбора", item));
			ClickElement(By.XPath(MULTISELECT_ITEM_XPATH + "[text()='" + item + "']"));
		}

		public string GetCustomFieldValue(string fieldName)
		{
			Logger.Debug(string.Format("Получить значение пользовательского поля. Название поля {0}", fieldName));

			return GetTextElement(By.XPath(GetCustomFieldXPath(fieldName) +
				FIELD_DIV_VALUE_XPATH));
		}

		public void FillCustomFieldNumber(string fieldName, string value)
		{
			Logger.Debug(string.Format("Ввести значение в пользовательское поле Number нового термина. Название поля {0}; значение {1}", 
				fieldName, value));
			SendTextElement(By.XPath(GetCustomFieldXPath(fieldName) + INPUT_FIELD_VALUE_XPATH), value);
		}

		public string GetCustomFieldNumberValue(string fieldName)
		{
			Logger.Debug(string.Format("Вернуть значение из пользовательского поля Number. Название поля {0}", fieldName));
			return GetTextElement(By.XPath(GetCustomFieldViewXPath(fieldName) + NUMBER_FIELD_VALUE_XPATH));
		}

		public void ClickCustomFieldChoice(string fieldName)
		{
			Logger.Debug(string.Format("Кликнуть по пользовательскому полю выбора. Название поля {0}", fieldName));
			ClickElement(By.XPath(GetCustomFieldXPath(fieldName) + CHOICE_FIELD_DROPDOWN_XPATH));
		}

		public void SelectChoiceItem(string item)
		{
			Logger.Debug(string.Format("Выбрать элемент {0} списка", item));
			ClickElement(By.XPath(CHOICE_LIST_XPATH + "[@title='" + item + "']"));
		}
		
		public void AssertionIsCustomFieldImageExist(string fieldName)
		{
			Logger.Debug(string.Format("Вернуть, есть ли пользовательское поле Изображение. Имя поля {0}", fieldName));

			Assert.IsTrue(GetIsElementDisplay(By.XPath(GetCustomFieldImageXPath(fieldName))),
				"Ошибка: поле не появилось");
		}

		public void AssertionIsExistCustomFieldImageError(string fieldName)
		{
			Logger.Trace("Проверить, что пользовательское поле изображение отмечено ошибкой");

			Assert.IsTrue(
				GetIsElementDisplay(By.XPath(GetCustomFieldImageErrorXPath(fieldName))),
				"Ошибка: обязательное поле не отмечено ошибкой");
		}

		public void ClickCustomFieldImage(string fieldName)
		{
			Logger.Debug(string.Format("Кликнуть по пользовательскому полю Изображение. Имя поля {0}", fieldName));

			//Работу можно продолжить дальше и визуально клик проходит, но падает исключение про timeout операции.
			//TODO проверить exception
			try
			{
				ClickElement(By.XPath(GetCustomFieldImageXPath(fieldName) 
					+ FIELD_IMAGE_FIELD_XPATH));
			}
			catch (WebDriverException ex)
			{
				// Такая проверка стоит, чтобы обрабатывать только одно конкретное исключение. 
				// Но драйвер не выделяет это исключение в отдельный тип, поэтому приходится делать проверку сообщения.
				if (ex.Message.Contains("timed out after 60 seconds"))
				{
					Logger.Warn(ex.Message);
				}
				else
				{
					throw;
				}
			}
		}

		public bool GetCustomFieldImageFilled(string fieldName)
		{
			Logger.Debug(string.Format("Вернуть, заполнено ли поле Изображение. Имя поля {0}", fieldName));

			var xPath = GetCustomFieldImageXPath(fieldName) + CUSTOM_IMAGE_EXIT_XPATH;

			return GetElementAttribute(By.XPath(xPath), "src").Trim().Length > 0;
		}

		public void ClickCustomFieldMedia(string fieldName)
		{
			Logger.Debug(string.Format("Кликнуть по пользовательскому полю Media. Имя поля {0}", fieldName));

			//Работу можно продолжить дальше и визуально клик проходит, но падает исключение про timeout операции.
			//TODO проверить exception
			try
			{
				ClickElement(By.XPath(GetCustomFieldImageXPath(fieldName) + CUSTOM_FIELD_MEDIA_REF_XPATH));
			}
			catch (WebDriverException ex)
			{
				// Такая проверка стоит, чтобы обрабатывать только одно конкретное исключение. 
				// Но драйвер не выделяет это исключение в отдельный тип, поэтому приходится делать проверку сообщения.
				if (ex.Message.Contains("timed out after 60 seconds"))
				{
					Logger.Warn(ex.Message);
				}
				else
				{
					throw;
				}
			}
		}

		public bool GetIsCustomFieldMediaFilled(string fieldName)
		{
			Logger.Debug(string.Format("Получить, заполнено ли пользовательское поле Media. Имя поля {0}", fieldName));

			var xPath = GetCustomFieldImageXPath(fieldName) + FIELD_MEDIA_CONTAINS_XPATH;

			return GetElementAttribute(By.XPath(xPath), "href").Trim().Length > 0;
		}
		
		public void ClickCustomFieldDate(string fieldName)
		{
			Logger.Debug(string.Format("Кликнуть по пользовательскому полю Дата {0}", fieldName));

			ClickElement(By.XPath(GetCustomFieldXPath(fieldName) + CUSTOM_FIELD_DATE_XPATH));
		}

		public void AssertionIsCalendarExist()
		{
			Logger.Trace("Проверить, что календарь появился");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(CALEDNAR_XPATH)),
				"Ошибка: календарь не появился");
		}
		
		public void SelectCalendarToday()
		{
			Logger.Debug("Выбрать текущую дату");
			ClickElement(By.XPath(CALENDAR_TODAY_XPATH));
		}

		public void FillCustomFieldText(string fieldName, string text)
		{
			Logger.Debug(string.Format("Заполнить пользовательское поле Текст. Название поля: {0}, текст: {1}", fieldName, text));
			SendTextElement(By.XPath(GetCustomFieldXPath(fieldName) + CUSTOM_FIELD_TEXT_XPATH), text);
		}

		public bool GetIsExistTextarea(string fieldName)
		{
			Logger.Debug(string.Format("Получить, есть ли textarea {0}", fieldName));

			return GetIsElementDisplay(By.XPath(GetTextareaXPath(fieldName)));
		}

		public void FillTextarea(string fieldName, string text)
		{
			Logger.Debug(string.Format("Заполнение textarea. Название поля: {0}, текст {1}", fieldName, text));
			SendTextElement(By.XPath(GetTextareaXPath(fieldName)), text);
		}

		public string GetTextareaValue(string fieldName)
		{
			Logger.Debug(string.Format("Получить значение textarea для поля {0}", fieldName));

			return GetTextElement(By.XPath(GetTextareaValueXPath(fieldName)));
		}

		public bool GetIsExistInput(string fieldName)
		{
			Logger.Debug(string.Format("Получить, есть ли input в поле: {0}", fieldName));

			return GetIsElementExist(By.XPath(GetInputXPath(fieldName)));
		}

		public void ClickMediaToImport(string fieldName)
		{
			Logger.Debug("Кликнуть по полю Media для загрузки");
			//Работу можно продолжить дальше и визуально клик проходит, но падает исключение про timeout операции.
			//TODO проверить exception
			try
			{
				ClickElement(By.XPath(GetInputXPath(fieldName) + INPUT_IMPORT_LINK_XPATH));
			}
			catch (WebDriverException ex)
			{
				// Такая проверка стоит, чтобы обрабатывать только одно конкретное исключение. 
				// Но драйвер не выделяет это исключение в отдельный тип, поэтому приходится делать проверку сообщения.
				if (ex.Message.Contains("timed out after 60 seconds"))
				{
					Logger.Warn(ex.Message);
				}
				else
				{
					throw;
				}
			}
		}

		public bool GetIsFieldMediaFilled(string fieldName)
		{
			Logger.Debug(string.Format("Получить, заполнено ли поле Media. Имя поля: {0}", fieldName));

			var xPath = GetInputXPath(fieldName) + FIELD_MEDIA_CONTAINS_XPATH;

			return GetElementAttribute(By.XPath(xPath), "href").Trim().Length > 0;
		}

		/// <summary>
		/// Метод ожидает окончания загрузки файла в поле Media
		/// </summary>
		/// <param name="fieldName">название поля</param>
		public bool WaitFileMediaLoad(string fieldName)
		{
			Logger.Debug(string.Format("Ожидаем окончания загрузки файла в поле Media. Имя поля: {0}", fieldName));
			var xPath = GetInputXPath(fieldName) + FIELD_MEDIA_CONTAINS_XPATH;
			return WaitUntilDisplayElement(By.XPath(xPath));
		}

		public void ClickImageToImport(string fieldName)
		{
			Logger.Debug(string.Format("Кликнуть по полю Image для загрузки. Имя поля: {0}", fieldName));
			//Работу можно продолжить дальше и визуально клик проходит, но падает исключение про timeout операции.
			//TODO проверить exception
			try
			{
				ClickElement(By.XPath(GetInputXPath(fieldName) + FIELD_IMAGE_FIELD_XPATH));
			}
			catch (WebDriverException ex)
			{
				// Такая проверка стоит, чтобы обрабатывать только одно конкретное исключение. 
				// Но драйвер не выделяет это исключение в отдельный тип, поэтому приходится делать проверку сообщения.
				if (ex.Message.Contains("timed out after 60 seconds"))
				{
					Logger.Warn(ex.Message);
				}
				else
				{
					throw;
				}
			}
		}

		public bool GetFieldImageFilled(string fieldName)
		{
			Logger.Debug(string.Format("Вернуть, заполнено ли поле Изображение. Название поля {0}", fieldName));

			var xPath = GetInputXPath(fieldName) + CUSTOM_IMAGE_EXIT_XPATH;

			return GetElementAttribute(By.XPath(xPath), "src").Trim().Length > 0;
		}

		public bool GetIsExistSelect(string fieldName)
		{
			Logger.Debug(string.Format("Вернуть, есть ли поле Select. Название поля {0}", fieldName));

			return GetIsElementExist(By.XPath(GetSelectXPath(fieldName)));
		}

		public void ClickSelectDropdown(string fieldName)
		{
			Logger.Debug(string.Format("Кликнуть Select для выпадения списка. Название поля {0}", fieldName));
			ClickElement(By.XPath(GetSelectXPath(fieldName) + CHOICE_FIELD_DROPDOWN_XPATH));
		}

		public void AssertionIsSelectListVisible()
		{
			Logger.Debug("Проверить, раскрылся ли список выбора домена");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(CHOICE_LIST_XPATH)), "Ошибка: список выбора домена не открылся");
		}

		public string GetSelectValue(string fieldName)
		{
			Logger.Debug(string.Format("Получить значение поля Select. Название поля {0}", fieldName));

			return GetTextElement(By.XPath(GetSelectXPath(fieldName) + FIELD_DIV_VALUE_XPATH));
		}

		public void ClickTopicDropdown(string fieldName)
		{
			Logger.Debug("Кликнуть по полю Topic для выпадения списка");
			ClickElement(By.XPath(GetInputXPath(fieldName) + TOPIC_FIELD_DROPDOWN_XPATH));
		}

		public void AssertionTopicListVisible()
		{
			Logger.Debug("Проверить, что виден список для Topic");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(TOPIC_CHOICE_XPATH)), "Ошибка: список для Topic не открылся");
		}

		public void SelectTopicItem()
		{
			Logger.Debug("Нажать кнопку выбора элемента Topic");
			ClickElement(By.XPath(TOPIC_ITEM_XPATH));
		}

		public string GetTopicValue(string fieldName)
		{
			Logger.Debug(string.Format("Вернуть значение Topic поля {0}", fieldName));

			return GetTextElement(By.XPath(GetInputXPath(fieldName) + FIELD_DIV_VALUE_XPATH));
		}


		public void OpenLanguageAttributes()
		{
			Logger.Debug("Нажать кнопку открытия атрибутов языка");
			ClickElement(By.XPath(LANGUAGE_ROW_XPATH));
		}

		public void AssertionIsExistDetailsTextarea(string fieldName)
		{
			Logger.Debug(string.Format("Получить, отображается ли поле {0} в детальных атрибутах", fieldName));

			Assert.IsTrue(GetIsElementDisplay(By.XPath(GetDetailsTextareaXPath(fieldName))),
				"Ошибка: в детальных атрибутах поле не появилось!");
		}

		public void FillDetailTextarea(string fieldName, string text)
		{
			Logger.Debug(string.Format("Заполнить textarea детальных атрибутов. Название поля {0}, текст {1}", fieldName, text));

			SendTextElement(By.XPath(GetDetailsTextareaXPath(fieldName)), text);
		}

		public string GetDetailTextareaValue(string fieldName)
		{
			Logger.Debug(string.Format("Получить значение textarea детальных атрибутов. Название поля {0}", fieldName));

			return GetTextElement(By.XPath(GetDetailsTextareaXPath(fieldName) + DETAILS_TEXTAREA_VALUE));
		}

		public void OpenTermLevel()
		{
			Logger.Debug("Нажать кнопку открытия атрибутов уровня Term");
			ClickElement(By.XPath(TERM_ROW_XPATH));
		}

		public bool GetIsExistDetailsSelect(string fieldName)
		{
			Logger.Trace(string.Format("Вернуть, отображается ли поле Select в детальных атрибутах. Название поля {0}", fieldName));

			return GetIsElementExist(By.XPath(GetDetailsSelectXPath(fieldName)));
		}

		public string GetDetailsSelectOptionId(string fieldName, int optionNumber)
		{
			Logger.Debug(string.Format("Вернуть id из списка select детальных атрибутов. Название поля {0}, номер элемента списка {1}", 
				fieldName, optionNumber));

			var xPath = GetDetailsSelectXPath(fieldName) + DETAILS_SELECT_OPTION_XPATH + "[" + optionNumber + "]";

			return GetElementAttribute(By.XPath(xPath), "value");
		}

		public void ClickDetailsSelectDropdown(string fieldName)
		{
			Logger.Debug(string.Format("Кликнуть по Select в детальных атрибутах для открытия списка. Название поля {0}", fieldName));
			ClickElement(By.XPath(GetDetailsSelectXPath(fieldName) + CHOICE_FIELD_DROPDOWN_XPATH));
		}

		public void ClickListItemById(string optionId)
		{
			Logger.Debug(string.Format("Кликнуть по элементу списка. Id {0}", optionId));
			ClickElement(By.XPath(GetSelectOptionText(optionId)));
		}

		public string GetListItemText(string optionId)
		{
			Logger.Trace(string.Format("Получить текст элемента списка. Id {0}", optionId));

			return GetTextElement(By.XPath(GetSelectOptionText(optionId)));
		}

		public string GetDetailsSelectValue(string fieldName)
		{
			Logger.Trace(string.Format("Вернуть значение поля Select детальных атрибутов. Название поля {0}", fieldName));

			return GetTextElement(By.XPath(GetDetailsSelectXPath(fieldName) + DETAILS_TEXTAREA_VALUE));
		}

		public bool GetIsTermExistByText(string text)
		{
			Logger.Trace(string.Format("Получить, есть ли термин с названием {0}", text));

			return GetIsElementDisplay(By.XPath(CONCEPT_ROW_XPATH + "//p[contains(text(),'" + text + "')]"));
		}

		public bool GetIsSingleSourceTermExists(string text)
		{
			Logger.Trace(string.Format("Получить, есть ли данный одиночный сорс термин {0}", text));

			if (GetIsExistTerm(text))
			{
				return !GetIsElementDisplay(By.XPath(SINGLE_SOURCE_TERM_XPATH.Replace("#", text)));
			}
			else
			{
				return false;
			}
		}

		public void AssertionIsSingleTargetTermExists(string text)
		{
			Logger.Trace(string.Format("Проверить наличие одиночного таргет термина {0}", text));

			Assert.IsTrue(!GetIsElementDisplay(By.XPath(SINGLE_TARGET_TERM_XPATH.Replace("#", text))),
				"Ошибка: Не добавлен одиночный термин из таргета.");
		}

		public bool GetIsSourceTargetTermExists(string sourceText, string targetText)
		{
			Logger.Debug(string.Format("Получить, есть ли термин с сорсом {0} и таргетом {1}", sourceText, targetText));

			return GetIsElementDisplay(By.XPath(SOURCE_TARGET_TERM_XPATH.Replace("#", sourceText).Replace("**", targetText)));
		}

		public bool GetIsCommentExists(string text)
		{
			Logger.Debug(string.Format("Получить, есть ли термин с комментарием {0}", text));

			return GetIsElementDisplay(By.XPath(COMMENT_XPATH.Replace("#", text)));
		}

		public bool GetAreTwoEqualTermsExist(string sourceText, string targetText)
		{
			Logger.Debug(string.Format("Получить, есть ли два одинаковых термина. Текст сорса {0}, текст таргета {1}", sourceText, targetText));

			var terms = Driver.FindElements(
				By.XPath(
					SOURCE_TARGET_TERM_XPATH
						.Replace("#", sourceText)
						.Replace("**", targetText)))
						.ToList();

			return terms.Count > 1;
		}

		public void ClickEditBtn()
		{
			Logger.Debug("Нажать кнопку Edit");
			ClickElement(By.XPath(EDIT_BTN_XPATH));
		}

		public void ClickTermRow()
		{
			Logger.Debug("Кликнуть на строку термина");
			ClickElement(By.XPath(CONCEPT_ROW_XPATH + TERM_COMMENT_TD));
		}

		public void ClickTermRowByNameOfTerm(string source, string target)
		{
			Logger.Debug("Кликнуть на заданную строку термина");
			ClickElement(By.XPath(SOURCE_TARGET_TERM_COMMENT_RAW_XPATH.Replace("#", source).Replace("**", target)));
		}

		public void ClickEditTermBtn()
		{
			Logger.Debug("Кликнуть на кнопку Edit в строке термина");
			ClickElement(By.XPath(CONCEPT_ROW_XPATH + EDIT_TERM_BTN_XPATH));
		}

		public void FillTermGeneralMode(string text)
		{
			Logger.Debug(string.Format("Заполнить все термины в обычном режиме. Текст термина: {0}", text));
			ClearAndFillElementsList(By.XPath(INPUT_TERM_XPATH), text);
		}

		public void ClickTurnOffBtn()
		{
			Logger.Debug("Кликнуть кнопку Свернуть");
			ClickElement(By.XPath(TURN_OFF_BTN_XPATH));
		}

		public void FillSearchField(string text)
		{
			Logger.Debug(string.Format("Заполнить поле поиск. Текст поля: {0}", text));
			ClearAndAddText(By.XPath(SEARCH_INPUT_XPATH), text);
		}

		public void ClickSearchBtn()
		{
			Logger.Debug("Кликнуть кнопку Search");
			ClickElement(By.XPath(SEARCH_BTN_XPATH));
		}

		public string GetFirstTermText()
		{
			Logger.Debug("Получить текст первого термина");
			return GetTextElement(By.XPath(TERM_TEXT_XPATH)).Trim();
		}

		public bool GetIsExistTerm(string termText)
		{
			Logger.Debug(string.Format("Получить, есть ли термин {0} среди остальных терминов", termText));
			return GetIsElementExist(By.XPath(TERM_TEXT_XPATH + "[contains(text(),'" + termText + "')]"));
		}

		public void ClickCancelEditBtn()
		{
			Logger.Debug("Нажать кнопку Cancel");
			ClickElement(By.XPath(CANCEL_EDIT_CONCEPT_BTN_XPATH));
		}

		public void ClickDeleteBtn()
		{
			Logger.Debug("Нажать кнопку Delete");
			ClickElement(By.XPath(DELETE_CONCEPT_BTN_XPATH));
		}

		public string GetHrefForExport()
		{
			Logger.Debug("Получить сслку для скачивания файла");
			return GetElementAttribute(By.XPath(HREF_EXPORT), "href");
		}

		public void ClickAddSynonym(int langNumber)
		{
			Logger.Debug(string.Format("Нажать кнопку Add для языка #{0}", langNumber));
			ClickElement(By.XPath(CONCEPT_EDITING_TD_XPATH + 
				"[" + langNumber + "]" + ADD_SYNONYM_BTN_XPATH));
		}

		public void FillSynonymTermLanguage(int langNumber, string text)
		{
			Logger.Debug(string.Format("Заполнить язык термина. Язык №{0}, текст {1}", langNumber, text));
			ClearAndAddText(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + langNumber + "]" + SYNONYM_INPUT_XPATH), text);
		}

		public void AssertionIsTermErrorExist(int langNumber)
		{
			Logger.Debug(string.Format("Проверить, что термин отмечен ошибкой. Номер языка: {0}", langNumber));

			Assert.IsTrue(
				GetIsElementExist(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + langNumber + "]" + TERM_ERROR_XPATH)),
				"Ошибка: поле с совпадающим термином не отмечено ошибкой");
		}

		public void AssertionIsTermErrorMessageExist(int langNumber)
		{
			Logger.Debug(string.Format("Проверить, что появилось сообщение об ошибке в термине. Номер языка {0}", langNumber));

			Assert.IsTrue(
				GetIsElementExist(By.XPath(CONCEPT_EDITING_TD_XPATH + "[" + langNumber + "]" + TERM_ERROR_MESSAGE_XPATH)),
				"Ошибка: поле с совпадающим термином не отмечено ошибкой");
		}

		public void AssertionIsGlossaryErrorExist()
		{
			Logger.Trace("Проверить, что появилась ошибка глоссария");

			Assert.IsTrue(GetIsElementExist(By.XPath(GLOSSARY_ERROR_XPATH)),
				"Ошибка: должно появиться предупреждение о добавлении пустого термина");
		}

		public void AssertionDuplicateErrorAppear()
		{
			Logger.Debug("Проверить существование ошибки дубликата");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(DUPLICATE_ERROR_XPATH)),
				"Ошибка: должно появиться предупреждение о добавлении существующего термина");
		}

		public void ClickImportBtn()
		{
			Logger.Debug("Нажать кнопку Import");
			ClickElement(By.XPath(IMPORT_BTN_XPATH));
		}

		public void WaitImportForm()
		{
			Logger.Trace("Проверка появления формы импорта");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(IMPORT_FORM_XPATH)),
				"Ошибка: форма импорта не появилась");
		}

		public void AssertionImportFormDisappear()
		{
			Logger.Trace("Проверить, что форма импорта закрылась");

			Assert.IsTrue(WaitUntilDisappearElement(By.XPath(IMPORT_FORM_XPATH)),
				"Ошибка: форма импорта не была закрыта");
		}

		public void ClickReplaceAll()
		{
			Logger.Debug("Нажать кнопку ReplaceAll");
			ClickElement(By.XPath(REPLACE_ALL_XPATH));
		}

		public void ClickImportFormImportBtn()
		{
			Logger.Debug("Кликнуть импорт в форме импорта");
			ClickElement(By.XPath(IMPORT_FORM_IMPORT_BTN_XPATH));
		}

		public void ClickCloseSuccessResult()
		{
			Logger.Debug("Кликнуть кнопку закрытия сообщения об успешном добавлении");
			ClickElement(By.XPath(SUCCESS_RESULT_CLOSE_BTN_XPATH));
		}

		public void AssertionIsExistNewItemExtendedMode()
		{
			Logger.Trace("Проверить сущестоввание расширенного режима добавления термина");
			
			Assert.IsTrue(GetIsElementDisplay(By.XPath(CONCEPT_EDITING_TD_XPATH)),
				"Ошибка: не появилось расширенного режима добавления термина");
		}

		public void UploadFileInGlossary(string fileName)
		{
			UploadDocNativeAction(fileName);
		}

		public void UploadTerm(string DocumentName)
		{
			var input = Driver.FindElement(By.XPath(IMPORT_TERMS));
			((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].style[\"display\"] = \"block\";" +
				"arguments[0].style[\"visibility\"] = \"visible\";",
				input);
			Driver.FindElement(By.XPath(IMPORT_TERMS)).SendKeys(DocumentName);
			((IJavaScriptExecutor)Driver).ExecuteScript("document.getElementsByClassName('g-iblock g-bold l-editgloss__filelink js-filename-link')[0].innerHTML = '" + Path.GetFileName(DocumentName) + "'");

		}

		public void ClickExportGlossary()
		{
			Logger.Debug("Нажать на кнопку экспорта глоссария");
			ClickElement(By.XPath(HREF_EXPORT));
		}

		public void WaitNewItemOpen()
		{
			Logger.Trace("Проверить, что форма добавления нового термина открылась");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(NEW_ITEM_OPEN)),
				"Ошибка: форма добавления нового термина не была открыта");
		}

		/// <summary>
		/// Получить xPath пользовательского поля (для поля типа Bool)
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldBoolXPath(string fieldName)
		{
			return CUSTOM_FIELD_BOOL_EDIT_CONCEPT_XPATH + 
				"[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить xPath пользовательского поля
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldXPath(string fieldName)
		{
			return CUSTOM_FIELD_EDIT_CONCEPT_XPATH + 
				"[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить xPath пользовательского поля для чтения
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldViewXPath(string fieldName)
		{
			return CUSTOM_FIELD_VIEW_CONCEPT_XPATH + 
				"[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить xPath поля, отмеченного ошибкой
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldErrorXPath(string fieldName)
		{
			return CUSTOM_ERROR_FIELD_XPATH + 
				"[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить XPath пользовательского поля для изоб;ражения
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldImageXPath(string fieldName)
		{
			return CUSTOM_FIELD_IMAGE_XPATH + 
				"[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Получить xPath поля, отмеченного ошибкой
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetCustomFieldImageErrorXPath(string fieldName)
		{
			return CUSTOM_ERROR_FIELD_IMAGE_XPATH + 
				"[contains(text(),'" + fieldName + "')]";
		}

		/// <summary>
		/// Вернуть xPath textarea
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetTextareaXPath(string fieldName)
		{
			return TEXTAREA_FIELD_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть xPath значения textarea
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetTextareaValueXPath(string fieldName)
		{
			return TEXTAREA_FIELD_XPATH + 
				"[@name='" + fieldName + "']" + FIELD_DIV_VALUE_XPATH;
		}

		/// <summary>
		/// Вернуть xPath input
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetInputXPath(string fieldName)
		{
			return INPUT_FIELD_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть xPath Select
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetSelectXPath(string fieldName)
		{
			return SELECT_FIELD_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть xPath для поля textarea в детальных атрибутах
		/// </summary>
		/// <param name="fieldName">название поля</param>
		/// <returns>xPath</returns>
		protected string GetDetailsTextareaXPath(string fieldName)
		{
			return DETAILS_TEXTAREA_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть xPath для поля select в детальных атрибутах
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		protected string GetDetailsSelectXPath(string fieldName)
		{
			return DETAILS_SELECT_XPATH + "[@name='" + fieldName + "']";
		}

		/// <summary>
		/// Вернуть текст списка выбора по id
		/// </summary>
		/// <param name="optionID">id</param>
		/// <returns>текст</returns>
		protected string GetSelectOptionText(string optionID)
		{
			return CHOICE_DROPDOWN_LIST + "[@data-id='" + optionID + "']";
		}

		protected const string IMPORT_TERMS = "//form[contains(@action,'Enterprise/Glossaries/Import')]//input[contains(@class,'js-submit-input')]";

		protected const string ADD_CONCEPT_XPATH = "//span[contains(@class,'js-add-concept')]";
		protected const string OPEN_EDIT_GLOSSARY_LIST_XPATH = "//span[contains(@class,'js-edit-submenu')]";
		protected const string OPEN_EDIT_STRUCTURE_FORM_BTN_XPATH = "//div[contains(@class,'js-edit-structure-btn')]";
		protected const string OPEN_EDIT_PROPERTIES_FORM_BTN_XPATH = "//div[contains(@class,'js-edit-glossary-btn')]";

		protected const string NEW_ITEM_CONCEPT_PART_XPATH = "//div[contains(@class,'js-concept-attrs')]";
		protected const string NEW_ITEM_EDIT_DIV_XPATH = "//div[contains(@class,'js-edit')]";
		protected const string NEW_ITEM_VIEW_DIV_XPATH = "//div[contains(@class,'l-corpr__viewmode__view')]";
		protected const string NEW_ITEM_CONTROL_XPATH = "//div[contains(@class,'js-control')]";

		protected const string NEW_ITEM_DOMAIN_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_EDIT_DIV_XPATH + "//select[@name='Domain']/..//span[contains(@class,'js-dropdown')]";

		protected const string ITEM_TERMS_EXTENDED_XPATH = "//div[contains(@class, 'js-terms-tree')]//div[contains(@class, 'l-corprtree__langbox')]";
		protected const string ITEM_ADD_EXTENDED_XPATH = ITEM_TERMS_EXTENDED_XPATH + "//span[contains(@class,'js-add-term')]";
		protected const string ITEM_EDITOR_INPUT_XPATH = "//span[contains(@class,'js-term-editor')]//input";

		protected const string CONCEPT_TABLE_XPATH = "//table[contains(@class,'js-concepts')]";
		protected const string CONCEPT_EDITING_TD_XPATH = "//tr[contains(@class, 'js-concept')]//td";
		protected const string CONCEPT_EDITING_OPENED = "//tr[contains(@class, 'js-concept-row js-editing opened')]";
		protected const string EDIT_CONCEPT_SAVE_BTN_XPATH = CONCEPT_EDITING_OPENED + "//a[contains(@class, 'js-save-btn')]";
		protected const string CANCEL_EDIT_CONCEPT_BTN_XPATH = CONCEPT_EDITING_OPENED + "//a[contains(@class, 'js-cancel-btn')]";
		protected const string DELETE_CONCEPT_BTN_XPATH = CONCEPT_ROW_XPATH + "//a[contains(@class, 'js-delete-btn')]";
		protected const string CONCEPT_ROW_XPATH = "//tr[contains(@class, 'js-concept-row')]";
		protected const string OPENED_CONCEPT_ROW_XPATH = "//tr[@class='js-concept-panel']/preceding-sibling::tr[1]";
		protected const string SAVE_EXTENDED_BTN_XPATH = CONCEPT_TABLE_XPATH + "//span[contains(@class,'js-save-btn')]";
		
		protected const string CUSTOM_FIELD_BOOL_EDIT_CONCEPT_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_EDIT_DIV_XPATH + "//span[contains(@class,'l-editgloss__name')]";
		protected const string CUSTOM_FIELD_BOOL_CHECKBOX_XPATH = "/..//span[contains(@class,'js-chckbx')]//input[contains(@class,'js-chckbx__orig')]";
		protected const string INPUT_FIELD_VALUE_XPATH = "/..//input[contains(@class,'js-submit-input')]";

		protected const string CUSTOM_FIELD_EDIT_CONCEPT_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_EDIT_DIV_XPATH + "//p";
		protected const string CUSTOM_FIELD_VIEW_CONCEPT_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_VIEW_DIV_XPATH + "//p";
		protected const string CUSTOM_ERROR_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + "//div[contains(@class,'js-edit l-error')]//p";

		protected const string FIELD_DIV_VALUE_XPATH = "/../../div[contains(@class,'js-view')]//div";
		protected const string MULTISELECT_FIELD_VALUE_XPATH = "/..//div[contains(@class,'ui-multiselect')]";
		protected const string MULTISELECT_ITEM_XPATH = "//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')]";
		protected const string NUMBER_FIELD_VALUE_XPATH = "/../div[contains(@class,'js-value')]";
		protected const string CHOICE_FIELD_DROPDOWN_XPATH = "/..//span[contains(@class,'js-dropdown')]";
		protected const string FIELD_IMAGE_FIELD_XPATH = "/..//div[contains(@class,'l-editgloss__imagebox')]//a";
		protected const string CUSTOM_IMAGE_EXIT_XPATH = "/../../div[contains(@class,'l-editgloss__image')]//img[contains(@class,'l-editgloss__imageview')]";
		protected const string CUSTOM_FIELD_IMAGE_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_CONTROL_XPATH + "//p";
		protected const string CUSTOM_ERROR_FIELD_IMAGE_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_CONTROL_XPATH + "[contains(@class,'l-error')]//p";
		protected const string CUSTOM_FIELD_MEDIA_REF_XPATH = "/..//span[contains(@class,'l-editgloss__linkbox')]" + UPLOAD_BTN_XPATH;
		protected const string UPLOAD_BTN_XPATH = "//a[contains(@class,'js-upload-btn')]";
		protected const string FIELD_MEDIA_CONTAINS_XPATH = "/../div[contains(@class,'l-editgloss__filemedia')]//a[contains(@class,'l-editgloss__filelink')]";
		protected const string CUSTOM_FIELD_DATE_XPATH = "/../input[contains(@class,'hasDatepicker')]";
		protected const string CALEDNAR_XPATH = "//table[contains(@class,'ui-datepicker-calendar')]";
		protected const string CALENDAR_TODAY_XPATH = CALEDNAR_XPATH + "//td[contains(@class,'ui-datepicker-today')]";
		protected const string CUSTOM_FIELD_TEXT_XPATH = "/..//textarea";

		protected const string TEXTAREA_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_CONTROL_XPATH + "//textarea";
		protected const string INPUT_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_CONTROL_XPATH + "//input";
		protected const string INPUT_IMPORT_LINK_XPATH = "/..//span[contains(@class,'l-editgloss__linkbox')]//a";
		protected const string SELECT_FIELD_XPATH = NEW_ITEM_CONCEPT_PART_XPATH + NEW_ITEM_EDIT_DIV_XPATH + "//select";
		protected const string TOPIC_FIELD_DROPDOWN_XPATH = "/..//div[contains(@class,'ui-dropdown-treeview-wrapper')]/div[1]//span";

		protected const string CHOICE_DROPDOWN_LIST = "//span[contains(@class,'js-dropdown__list')]//span";
		protected const string CHOICE_LIST_XPATH = CHOICE_DROPDOWN_LIST + "[contains(@class,'js-dropdown__item')]";
		protected const string TOPIC_CHOICE_XPATH = "//div[contains(@class,'ui-dropdown-treeview_dropDown')]";
		protected const string TOPIC_ITEM_XPATH = "//div[contains(@class,'ui-treeview_node')]//div/span";

		protected const string LANGUAGE_ROW_XPATH = "//div[contains(@class,'js-node js-lang-node')]";
		protected const string DETAILS_XPATH = "//td[contains(@class,'js-details-panel')]";
		protected const string DETAILS_TEXTAREA_XPATH = DETAILS_XPATH + "//textarea";
		protected const string DETAILS_TEXTAREA_VALUE = "/../..//div[contains(@class,'js-value')]";

		protected const string DETAILS_SELECT_XPATH = DETAILS_XPATH + "//select";
		protected const string DETAILS_SELECT_OPTION_XPATH = "//option";

		protected const string TERM_ROW_XPATH = "//div[contains(@class,'js-term-node')]";

		protected const string EDIT_BTN_XPATH = "//span[contains(@class,'js-edit-btn')]";
		protected const string EDIT_TERM_BTN_XPATH = "//a[contains(@class,'js-edit-btn')]";
		protected const string TERM_COMMENT_TD = "//td[contains(@class,'glossaryComment')]";
		protected const string INPUT_TERM_XPATH = "//input[contains(@class,'js-term')]";
		protected const string TURN_OFF_BTN_XPATH = "//a[contains(@class,'iconup')]";

		protected const string SEARCH_INPUT_XPATH = "//input[contains(@class,'js-search-term')]";
		protected const string SEARCH_BTN_XPATH = "//a[contains(@class,'js-search-by-term')]";
		protected const string TERM_TEXT_XPATH = CONCEPT_ROW_XPATH + "//td[contains(@class,'glossaryShort')]//p";
		protected const string TERM_ERROR_XPATH = "//p[contains(@class,'l-error')]";
		protected const string TERM_ERROR_MESSAGE_XPATH = "//div[contains(@class,'js-term-box')][contains(@class,'l-error')]";
		protected const string SINGLE_SOURCE_TERM_XPATH = CONCEPT_ROW_XPATH + "//td[1][contains(string(), '#')]/following-sibling::td[1]//p";
		protected const string SINGLE_TARGET_TERM_XPATH = CONCEPT_ROW_XPATH + "//td[1]/p../following-sibling::td[1][contains(string(), '#')]";
		protected const string COMMENT_XPATH = CONCEPT_ROW_XPATH + "//td[contains(@class, 'glossaryComment') and contains(string(), '#')]";
		protected const string SOURCE_TARGET_TERM_XPATH = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]";
		protected const string SOURCE_TARGET_TERM_COMMENT_RAW_XPATH = "//tr[contains(@class, 'js-concept-row') and contains(string(), '#') and contains(string(), '**')]//td[4]";

		protected const string ADD_SYNONYM_BTN_XPATH = "//span[contains(@class,'js-add-term')]";
		protected const string SYNONYM_INPUT_XPATH = "//div" + INPUT_TERM_XPATH;

		protected const string GLOSSARY_ERROR_XPATH = "//tr//td[contains(@class,'glossaryError')]";
		protected const string DUPLICATE_ERROR_XPATH = "//div[contains(@class,'js-popup-confirm')]//form//span[contains(@class,'duplicate-term')]";

		protected const string IMPORT_BTN_XPATH = "//span[contains(@class,'js-import-concepts')]";
		protected const string IMPORT_FORM_XPATH = "//div[contains(@class,'js-popup-import')][2]";

		protected const string EXPORT_BTN_XPATH = "//a[contains(@href,'/Glossaries/Export')]";
		protected const string REPLACE_ALL_XPATH = IMPORT_FORM_XPATH + "//input[contains(@id,'needToClear')][@value='True']";
		protected const string IMPORT_FORM_IMPORT_BTN_XPATH = IMPORT_FORM_XPATH + "//span[contains(@class,'js-import-button')]";
		protected const string SUCCESS_RESULT_CLOSE_BTN_XPATH = "//a[contains(@class,'js-close-link')]";

		protected const string HREF_EXPORT = "//a[contains(@class,'js-export-concepts')]";
		protected const string NEW_ITEM_OPEN = "//div[@class='l-corprtree']";
	}
}