using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер формы Add a term
	/// </summary>
	public class AddTermFormHelper : EditorPageHelper
	{
		public AddTermFormHelper(IWebDriver driver, WebDriverWait wait):
			base (driver, wait)
		{
		}
		public void ClickCancelBtn()
		{
			Logger.Debug("Нажать кнопку Отмена");
			ClickElement(By.XPath(CANCEL_BTN_XPATH));
		}

		public void ClickAddBtn()
		{
			Logger.Debug("Нажать кнопку Сохранить");
			ClickElement(By.XPath(ADD_BTN_XPATH));
		}

		public void ClickAddSingleTerm()
		{
			Logger.Debug("Нажать кнопку подтверждения одиночного элемента");
			ClickElement(By.XPath(СONFIRM_SINGLE_TERM_BTN_XPATH));
		}

		public void ClickContainsTermYes()
		{
			Logger.Debug("Нажать кнопку подтверждения добавления термина");
			ClickElement(By.XPath(CONTAINS_TERM_YES_BTN_XPATH));
		}

		public void ClickContainsTermNo()
		{
			Logger.Debug("Нажать кнопку отмены добавления уже существующего термина");
			ClickElement(By.XPath(CONTAINS_TERM_NO_BTN_XPATH));
		}

		public void AssertionIsTextExistInSourceTerm(string text)
		{
			Logger.Trace(string.Format("Проверить, что сработало автозаполнение текста {0} в SourceTerm", text));

			Assert.IsTrue(GetIsElementExist(By.XPath(SOURCE_TERM_VALUE_XPATH.Replace("#", text))),
				"Ошибка: Нет автозаполнения сорса.");
		}

		public void AssertionIsTextExistInTargetTerm(string text)
		{
			Logger.Trace(string.Format("Проверить, что сработало автозаполнение текста {0} в TargetTerm", text));

			Assert.IsTrue(GetIsElementExist(By.XPath(TARGET_TERM_VALUE_XPATH.Replace("#", text))), 
				"Ошибка: Нет автозаполнения таргета.");
		}

		public void TypeTargetTermText(string targetText)
		{
			Logger.Debug(string.Format("Добавить текст {0} в TargetTerm", targetText));
			ClickClearAndAddText(By.XPath(TARGET_TERM_INPUT_XPATH), targetText);
		}

		public void TypeSourceTermText(string sourceText)
		{
			Logger.Trace(string.Format("Набрать текст в  SourceTerm. текст для ввода {0}", sourceText));
			ClickClearAndAddText(By.XPath(SOURCE_TERM_INPUT_XPATH), sourceText);
		}

		public void TypeCommentText(string commentText)
		{
			Logger.Trace(string.Format("Набрать комментарий. текст комментария {0}", commentText));
			SendTextElement(By.XPath(COMMENT_INPUT_XPATH), commentText);
		}

		public void OpenGlossaryList()
		{
			Logger.Trace("Раскрыть выпадающий список");
			ClickElement(By.XPath(TERM_BASE_COMBOBOX_TRIGGER_XPATH));
		}

		public bool CheckGlossaryByName(string glossaryName)
		{
			Logger.Trace(string.Format("Получить, есть ли словарь с заданным именем. имя словаря {0}", glossaryName));
			return WaitUntilDisplayElement(By.XPath(TERM_BASE_BOUNDLIST_XPATH.Replace("#", glossaryName)));		
		}

		public void SelectGlossaryByName(string glossaryName)
		{
			Logger.Trace(string.Format("Выбрать словарь из выпадающего списка. имя словаря {0}", glossaryName));
			ClickElement(By.XPath(TERM_BASE_BOUNDLIST_XPATH.Replace("#", glossaryName)));
		}

		public bool WaitConfirmSingleTermMessage()
		{
			Logger.Trace("Получить, появилось ли сообщение о добавлении одиночного термина.");
			return WaitUntilDisplayElement(By.XPath(CONFIRM_SINGLE_TERM_MESSAGE_XPATH), 5);
		}

		public bool WaitAnyWayTermMessage()
		{
			Logger.Trace("Получить, появилось ли сообщение 'Do you want to add the term anyway?'");
			return WaitUntilDisplayElement(By.XPath(CONFIRM_ANYWAY_ADD_TERM_MESSAGE_XPATH), 5);
		}

		public void CliCkYesBtnInAnyWayTermMessage()
		{
			Logger.Trace("Кликнуть Yes в сообщении 'Do you want to add the term anyway?'");
			ClickElement(By.XPath(YES_BTN_XPATH));
		}

		public bool WaitTermSavedMessage()
		{
			Logger.Trace("Получить, появилось ли сообщение о сохранении термина");
			return WaitUntilDisplayElement(By.XPath(TERM_SAVED_MESSAGE_XPATH), 15);
		}

		public bool WaitContainsTermMessage()
		{
			Logger.Trace("Получить, появилось ли сообщение о повторном добавлении термин");
			return WaitUntilDisplayElement(By.XPath(CONTAINS_TERM_MESSAGE_XPATH), 5);
		}

		protected const string CANCEL_BTN_XPATH = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Cancel')]";
		protected const string ADD_BTN_XPATH = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Add')]";
		protected const string СONFIRM_SINGLE_TERM_BTN_XPATH = CONFIRM_SINGLE_TERM_MESSAGE_XPATH + "//span[contains(string(), 'Yes')]";
		protected const string CONTAINS_TERM_NO_BTN_XPATH = CONTAINS_TERM_MESSAGE_XPATH + "//span[contains(string(), 'No')]";
		protected const string CONTAINS_TERM_YES_BTN_XPATH = CONTAINS_TERM_MESSAGE_XPATH + "//span[contains(string(), 'Yes')]";

		protected const string SOURCE_TERM_VALUE_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm') and contains(@value, '#')]";
		protected const string TARGET_TERM_VALUE_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm') and contains(@value, '#')]";
		protected const string SOURCE_TERM_INPUT_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm')]";
		protected const string TARGET_TERM_INPUT_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm')]";
		protected const string COMMENT_INPUT_XPATH = "//div[contains(@id, 'term-window')]//textarea[contains(@name, 'comment')]";

		protected const string TERM_BASE_COMBOBOX_TRIGGER_XPATH = "//div[contains(@id, 'term-window')]//div[contains(@id, 'trigger-picker')]";
		protected const string TERM_BASE_BOUNDLIST_XPATH = "//ul[contains(@id, 'boundlist')]//li[contains(string(), '#')]";

		protected const string TERM_SAVED_MESSAGE_XPATH = "//div[contains(@id, 'innerCt') and contains(string(), 'The term has been saved')]";
		protected const string CONTAINS_TERM_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'This glossary already contains term(s)')]";
		protected const string CONFIRM_SINGLE_TERM_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'Do you want to add a term without translation?')]";
		protected const string CONFIRM_ANYWAY_ADD_TERM_MESSAGE_XPATH = "//div[@id= 'messagebox' and contains(string(), 'Do you want to add the term anyway?')]";
		protected const string YES_BTN_XPATH = "//div[@id= 'messagebox' and contains(string(), 'Do you want to add the term anyway?')]//span[text()='Yes']";
	}

}
