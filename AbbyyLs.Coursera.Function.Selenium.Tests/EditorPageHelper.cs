using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	public class EditorPageHelper : CommonHelper
	{
		public EditorPageHelper(IWebDriver driver, WebDriverWait wait) :
            base (driver, wait)
        {
        }

		/// <summary>
		/// Возвращает отображается ли кнопка Back
		/// </summary>
		/// <returns>Кнопка Back отображается</returns>
		public bool WaitUntilDisplayBackBtn()
		{
			return WaitUntilDisplayElement(By.Id(BACK_BTN_ID));
		}

		/// <summary>
		/// Возвращает отображается ли список сегментов
		/// </summary>
		/// <returns>Список сегментов отображается</returns>
		public bool WaitUntilDisplaySegments()
		{
			return WaitUntilDisplayElement(By.XPath(SEGMENTS_BODY_XPATH));
		}

		/// <summary>
		/// Ожидает пока исчезнет рамочка вокруг галки (прошел Confirm)
		/// Возвращает, исчезла ли рамочка
		/// </summary>
		/// <param name="rowNumber">Номер сегмента</param>
		/// <returns>Рамочка есть</returns>
		public bool WaitUntilDisappearBorderByRowNumber(int rowNumber)
		{
			string xPath = SEGMENTS_BODY_XPATH +
				 "[" + rowNumber + "]" + INFO_BORDER_XPATH;

			return WaitUntilDisappearElement(By.XPath(xPath), 25);
		}
		
		/// <summary>
		/// Ожидает пока исчезнет рамочка вокруг галки (прошел Confirm)
		/// Возвращает, исчезла ли рамочка
		/// </summary>
		/// <param name="position">Позиция сегмента</param>
		/// <returns>Рамочка есть</returns>
		public bool WaitUntilDisappearBorderByPosition(int position)
		{
			string xPath = SEGMENTS_BODY_XPATH + POSITION_XPATH +
				"[text()='" + position + "']/../.." + INFO_BORDER_XPATH;

			return WaitUntilDisappearElement(By.XPath(xPath), 25);
		}

		/// <summary>
		/// Кликнуть Back для выхода из него
		/// </summary>
		public void ClickBackBtn()
		{
			// Back
			ClickElement(By.Id(BACK_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Toggle
		/// </summary>
		public void ClickToggleBtn()
		{
			// Toggle
			ClickElement(By.Id(TOGGLE_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Confirm
		/// </summary>
		public void ClickConfirmBtn()
		{
			// Confirm
			ClickElement(By.Id(CONFIRM_BTN_ID));
		}

		/// <summary>
		/// Кликнуть Copy
		/// </summary>
		public void ClickCopyBtn()
		{
			// Confirm
			ClickElement(By.Id(COPY_BTN_ID));
		}

		/// <summary>
		/// Кликнуть CHANGE CASE
		/// </summary>
		public void ClickChangeCaseBtn()
		{
			// Change Case
			ClickElement(By.Id(CHANGE_CASE_BTN_ID));
		}

		/// <summary>
		/// Кликнуть TAG INSERT
		/// </summary>
		public void ClickTagInsertBtn()
		{
			// Tag Insert
			ClickElement(By.Id(TAG_INSERT_BTN_ID));
		}

		/// <summary>
		/// Кликнуть по переводу в сегменте
		/// </summary>
		public void ClickTargetByRowNumber(int rowNumber)
		{
			string targetCell = SEGMENTS_BODY_XPATH +
				"[" + rowNumber + "]" + TARGET_XPATH;

			ClickElement(By.XPath(targetCell));
		}

		/// <summary>
		/// Кликнуть по галочке Confirm в сегменте
		/// </summary>
		/// <param name="position">Позиция сегмента</param>
		public void ClickSegmentConfirmByPosition(int position)
		{
			string check = SEGMENTS_BODY_XPATH + POSITION_XPATH +
				"[text()='" + position + "']/../.." + INFO_CHECK_XPATH;

			ClickElement(By.XPath(check));
		}

		/// <summary>
		/// Кликнуть по галочке Confirm в сегменте
		/// </summary>
		/// <param name="position">Номер строки сегмента</param>
		public void ClickSegmentConfirmByRowNumber(int rowNumber)
		{
			string check = SEGMENTS_BODY_XPATH +
				"[" + rowNumber + "]" + INFO_CHECK_XPATH;

			ClickElement(By.XPath(check));
		}

		/// <summary>
		/// Возвращает, есть ли перевод пользователя
		/// </summary>
		/// <returns>Перевод есть</returns>
		public bool GetIsUserTranslationExist()
		{
			return GetIsElementExist(By.XPath(TRANSLATION_BODY_XPATH +
				TRASH_BTN_XPATH));
		}

		/// <summary>
		/// Возвращает, есть ли галочка для заданного сегмента
		/// </summary>
		/// <param name="rowNumber">Номер сегмента</param>
		/// <returns>Галочка есть</returns>
		public bool GetIsCheckPresent(int rowNumber)
		{
			string xPath = SEGMENTS_BODY_XPATH +
				"[" + rowNumber + "]" + INFO_CHECK_XPATH;

			return GetIsElementExist(By.XPath(xPath));
		}

		/// <summary>
		/// Возвращает, есть ли tag в переводе для заданного сегмента
		/// </summary>
		/// <param name="rowNumber">Номер сегмента</param>
		/// <returns>Tag есть</returns>
		public bool GetIsTagPresent(int rowNumber)
		{
			string xPath = SEGMENTS_BODY_XPATH +
				"[" + rowNumber + "]" + TAG_TARGET_XPATH;

			return GetIsElementExist(By.XPath(xPath));
		}

		/// <summary>
		/// Возвращает элемент Source по номеру сегмента
		/// </summary>
		/// <param name="rowNumber">Номер сегмента</param>
		/// <returns>Элемент</returns>
		public IWebElement GetSourceByRowNumber(int rowNumber)
		{
			IWebElement source = GetElement(By.XPath(SEGMENTS_BODY_XPATH +
				"[" + rowNumber + "]" + SOURCE_XPATH));
			return source;
		}

		/// <summary>
		/// Возвращает элемент Target по номеру сегмента
		/// </summary>
		/// <param name="rowNumber">Номер сегмента</param>
		/// <returns>Элемент</returns>
		public IWebElement GetTargetByRowNumber(int rowNumber)
		{
			IWebElement target = GetElement(By.XPath(SEGMENTS_BODY_XPATH +
				"[" + rowNumber + "]" + TARGET_XPATH));
			return target;
		}

		/// <summary>
		/// Возвращает список элементов перевода сегмента
		/// </summary>
		/// <returns>Список элементов</returns>
		public IList<IWebElement> GetTagetsList()
		{
			IList<IWebElement> targetList = GetElementList(By.XPath(SEGMENTS_BODY_XPATH +
				TARGET_XPATH));
			return targetList;
		}

		/// <summary>
		/// Возвращает список элементов голосовалок заданного типа
		/// </summary>
		/// <param name="voteType">Тип голоса</param>
		/// <returns>Список элементов</returns>
		public IList<IWebElement> GetVotesListByVoteType(string voteType)
		{
			IList<IWebElement> voteList = GetElementList(By.XPath(TRANSLATION_BODY_XPATH +
				VOTE_XPATH + "//span[contains(@class,'" + voteType + "')]"));
			
			return voteList;
		}

		/// <summary>
		/// Возвращает позицию сегмента по заданному элементу перевода
		/// </summary>
		/// <param name="element">Элемент перевода</param>
		/// <returns>Позиция</returns>
		public int GetPositionByTargetElement(IWebElement element)
		{
			int position = int.Parse(element.FindElement(By.XPath("..")).FindElement(By.XPath("..//td[1]")).Text);

			return position;
		}

		/// <summary>
		/// Удалить перевод
		/// </summary>
		public void DeleteTranslation()
		{
			// Кликнуть удалить перевод
			ClickElement(By.XPath(TRANSLATION_BODY_XPATH + 
				TRASH_BTN_XPATH));

			// Дождаться пока появится окно подтверждения
			WaitUntilDisplayElement(By.XPath(CONFIRM_DIALOG_YES_BTN));
			
			// Нажать ДА в диалоге
			ClickElement(By.XPath(CONFIRM_DIALOG_YES_BTN));

			// Дождаться пока окно подтверждения исчезнет
			WaitUntilDisappearElement(By.XPath(CONFIRM_DIALOG_YES_BTN));
		}

		/// <summary>
		/// Нажать кнопку редактирования перевода заданной строки
		/// </summary>
		/// <param name="rowNumber">Номер строки</param>
		public void ClickEditTranslationByRowNumber(int rowNumber)
		{
			string xPath = TRANSLATION_BODY_XPATH +
				"[" + rowNumber + "]" + PANCIL_BTN_XPATH;
			
			ClickElement(By.XPath(xPath));
		}

		/// <summary>
		/// Добавить перевод в строку
		/// </summary>
		/// <param name="rowNumber">номер строки</param>
		/// <param name="targetText">текст перевода</param>
		public void AddTextTargetByRowNumber(int rowNumber, string targetText)
		{
			string targetCell = SEGMENTS_BODY_XPATH +
				"[" + rowNumber + "]" + TARGET_XPATH;
			
			// Кликнуть по ячейке
			ClickElement(By.XPath(targetCell));
			// Очистит и написать текст в ячейке target
			SendTextElement(By.XPath(targetCell), targetText);
		}

		/// <summary>
		/// Получить количество переводов определенной строки
		/// </summary>
		/// <param name="rowNumber">Номер строки сегмента</param>
		/// <returns>Количество переводов</returns>
		public int GetQuantityTranslationsByRowNumber(int rowNumber)
		{
			string xPath = SEGMENTS_BODY_XPATH +
					"[" + rowNumber + "]" + TRANSLATIONS_XPATH;

			if (GetTextElement(By.XPath(xPath)).Trim() != String.Empty)
				return int.Parse(GetTextElement(By.XPath(xPath)).Trim());
			else
				return 0;
		}

		/// <summary>
		/// Получить номер строки в редакторе по номеру видимой строки в разметке
		/// </summary>
		/// <param name="rowNumber">Видимый номер строки разметки</param>
		/// <returns>Номер строки</returns>
		public int GetRowPositionByRowNumber(int rowNumber)
		{
			return int.Parse(GetTextElement(By.XPath(SEGMENTS_BODY_XPATH +
					"[" + rowNumber + "]" + POSITION_XPATH)));
		}

		/// <summary>
		/// Процедура открытия редактора на первом сегменте
		/// </summary>
		public void DisplayFirstSegment()
		{
			// Ждем отображения кнопки Back
			WaitUntilDisplayBackBtn();

			// Скрипт для очистки кэша, для открытия редатора на первом сегменте
			ExecuteClearScript();

			// Обновляем страницу
			SendTextElement(By.Id(BACK_BTN_ID), OpenQA.Selenium.Keys.F5);

			// Ждем отображения сегментов
			WaitUntilDisplaySegments();
		}

		/// <summary>
		/// Получить рейтинг перевода
		/// </summary>
		/// <param name="rowNumber">номер строки в списке предложенных переводов</param>
		/// <returns>рейтинг перевода</returns>
		public int GetTranslationRatingByRowNumber(int rowNumber)
		{
			return int.Parse(GetTextElement(By.XPath(TRANSLATION_BODY_XPATH +
					"[" + rowNumber + "]" + RATING_XPATH)));
		}

		/// <summary>
		/// Получить автора перевода по номеру строки
		/// </summary>
		/// <param name="rowNumber">Номер строки в списке предложенных переводов</param>
		/// <returns>Автор перевода</returns>
		public string GetTranslationAuthorByRowNumber(int rowNumber)
		{
			string xPath = TRANSLATION_BODY_XPATH +
					"[" + rowNumber + "]" + AUTHOR_XPATH;
			
			return GetTextElement(By.XPath(xPath));
		}

		/// <summary>
		/// Получить время добавления перевода
		/// </summary>
		/// <param name="rowNumber">Номер строки</param>
		/// <returns>Время создания перевода</returns>
		public string GetTranslationTimeByRowNumber(int rowNumber)
		{
			string xPath = TRANSLATION_BODY_XPATH +
				"[" + rowNumber + "]" + TIME_XPATH;

			return GetTextElement(By.XPath(xPath));
		}

		/// <summary>
		/// Получить номер строки перевода по строке времени
		/// </summary>
		/// <param name="time">Время перевода</param>
		/// <returns>Номер строки</returns>
		public int GetTranslationRowNumberByTime(string time)
		{
			// Определяем элемент перевода
			string translationListXPath = TRANSLATION_BODY_XPATH;

			// Получаем список всех элементов перевода
			IList<IWebElement> translationList = GetElementList(By.XPath(translationListXPath));

			// Получаем номер строки в которой находится заданная дата
			for (int i = 0; i < translationList.Count; ++i)
			{
				if (GetTextElement(By.XPath(translationListXPath + 
					"[" + (i + 1).ToString() + "]//td[1]/div")).
					Contains(time))
				{
					return (i + 1);
				}
			}
			return 0;

		}

		/// <summary>
		/// Получить номер строки перевода по строке перевода
		/// </summary>
		/// <param name="target">Перевод</param>
		/// <returns>Номер строки</returns>
		public int GetTranslationRowNumberByTarget(string target)
		{
			// Определяем элемент перевода
			string translationListXPath = TRANSLATION_BODY_XPATH;

			// Получаем список всех элементов перевода
			IList<IWebElement> translationList = GetElementList(By.XPath(translationListXPath));

			// Получаем номер строки в которой находится заданный target
			for (int i = 0; i < translationList.Count; ++i)
			{
				if (GetTextElement(By.XPath(translationListXPath +
					"[" + (i + 1).ToString() + "]//td[3]/div")).
					Contains(target))
				{
					return (i + 1);
				}
			}
			return 0;

		}

		/// <summary>
		/// Получить, учтен ли голос
		/// </summary>
		/// <param name="isVoteUp">тип голоса</param>
		/// <param name="rowNumber">номер строки</param>
		/// <returns>голос учтен</returns>
		public bool GetIsVoteConsidered(bool isVoteUp, int rowNumber)
		{
			string voteIcon = isVoteUp ? "fa-thumbs-up" : "fa-thumbs-down";
			string xPath = TRANSLATION_BODY_XPATH +
				"[" + rowNumber + "]//td[5]/div//span[contains(@class,'" + voteIcon + "')][contains(@class,'disabled')]";

			return GetIsElementExist(By.XPath(xPath));
		}
		
		/// <summary>
		/// Проголосовать
		/// </summary>
		/// <param name="isVoteUp">Тип голоса</param>
		/// <param name="rowNumber">Номер строки</param>
		public void VoteByRowNumber(bool isVoteUp, int rowNumber)
		{
			string voteClass = isVoteUp ? "fa-thumbs-up" : "fa-thumbs-down";

			string xPath = TRANSLATION_BODY_XPATH +
				"[" + rowNumber + "]//td[5]/div//span[contains(@class,'" + voteClass + "')]";

			ClickElement(By.XPath(xPath));
		}



		protected const string BACK_BTN_ID = "back-btn";
		protected const string CONFIRM_BTN_ID = "confirm-btn";
		protected const string TOGGLE_BTN_ID = "toggle-btn";
		protected const string COPY_BTN_ID = "copy-btn";
		protected const string CHANGE_CASE_BTN_ID = "change-case-btn";
		protected const string TAG_INSERT_BTN_ID = "tag-insert-btn";

		protected const string SEGMENTS_BODY_XPATH = ".//div[@id='segments-body']//table";

		protected const string POSITION_XPATH = "//td[contains(@class, 'cell-row-numberer')]//div";
		protected const string SOURCE_XPATH = "//td[contains(@class, 'source-cell')]//div";
		protected const string TARGET_XPATH = "//td[contains(@class, 'target-cell')]//div";
		protected const string TAG_TARGET_XPATH = TARGET_XPATH + "//img[contains(@class,'tag')]";
		protected const string TRANSLATIONS_XPATH = "//td[contains(@class, 'cell-translationcolumn')]//div";
		protected const string INFO_XPATH = "//td[contains(@class, 'info-cell')]//div";

		protected const string INFO_CHECK_XPATH = INFO_XPATH + "//span[contains(@class,'fa-check')]";
		protected const string INFO_BORDER_XPATH = INFO_XPATH + "//span[contains(@class,'fa-border')]";
		
		protected const string TRANSLATION_BODY_XPATH = ".//div[@id='translations-body']//table";
		
		protected const string TRASH_BTN_XPATH = "//td[4]//span[contains(@class,'fa-trash-o')]";
		protected const string PANCIL_BTN_XPATH = "//td[4]//span[contains(@class,'fa-pencil')]";
		protected const string TIME_XPATH = "//td[1]//div";
		protected const string AUTHOR_XPATH = "//td[2]//div";
		protected const string VOTE_XPATH = "//td[5]/div";
		protected const string RATING_XPATH = VOTE_XPATH + "//span[contains(@class,'rating-count')]";

		protected const string CONFIRM_DIALOG_YES_BTN = ".//div[@id='messagebox']//a[contains(@class,'x-btn-blue')]";
	}
}
