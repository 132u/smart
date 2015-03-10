using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер формы Add a term
	/// </summary>
	public class AddTermFormHelper : EditorPageHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public AddTermFormHelper(IWebDriver driver, WebDriverWait wait):
			base (driver, wait)
		{

		}

		/// <summary>
		/// Нажать кнопку "Отмена"
		/// </summary>
		public void ClickCancelBtn()
		{
			ClickElement(By.XPath(CANCEL_BTN_XPATH));
		}

		/// <summary>
		/// Нажать кнопку "Сохранить"
		/// </summary>
		public void ClickAddBtn()
		{
			ClickElement(By.XPath(ADD_BTN_XPATH));
		}

		/// <summary>
		/// Подтвердить добавление одиночного термина
		/// </summary>
		public void ClickAddSingleTerm()
		{
			ClickElement(By.XPath(СONFIRM_SINGLE_TERM_BTN_XPATH));
		}

		/// <summary>
		/// Форма "Термин добавлен", нажать ОК
		/// </summary>
		public void ClickTermSaved()
		{
			ClickElement(By.XPath(TERM_SAVED_OK_BTN_XPATH));
		}

		/// <summary>
		/// Подтвердить добавление уже существующего термина
		/// </summary>
		public void ClickContainsTermYes()
		{
			ClickElement(By.XPath(CONTAINS_TERM_YES_BTN_XPATH));
		}

		/// <summary>
		/// Отменить добавление уже существующего термина
		/// </summary>
		public void ClickContainsTermNo()
		{
			ClickElement(By.XPath(CONTAINS_TERM_NO_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть присуствует ли заданный текст в SourceTerm	
		/// </summary>
		/// <param name="text">текст</param>
		/// <returns>есть</returns>
		public bool GetSourceTermText(string text)
		{
			return GetIsElementExist(By.XPath(SOURCE_TERM_VALUE_XPATH.Replace("#", text)));
		}

		/// <summary>
		/// Вернуть присуствует ли заданный текст в TargetTerm
		/// </summary>
		/// <param name="text">текст</param>
		/// <returns>есть</returns>
		public bool GetTargetTermText(string text)
		{
			return GetIsElementExist(By.XPath(TARGET_TERM_VALUE_XPATH.Replace("#", text)));
		}

		/// <summary>
		/// Набрать текст в  TargetTerm		
		/// </summary>
		/// <param name="targetText">текст для ввода</param>
		public void TypeTargetTermText(string targetText)
		{
			ClickClearAndAddText(By.XPath(TARGET_TERM_INPUT_XPATH), targetText);
		}

		public void TypeSourceTermText(string sourceText)
		{
			Log.Trace(string.Format("Набрать текст в  SourceTerm. текст для ввода {0}", sourceText));
			ClickClearAndAddText(By.XPath(SOURCE_TERM_INPUT_XPATH), sourceText);
		}

		public void TypeCommentText(string commentText)
		{
			Log.Trace(string.Format("Набрать комментарий. текст комментария {0}", commentText));
			SendTextElement(By.XPath(COMMENT_INPUT_XPATH), commentText);
		}

		public void OpenGlossaryList()
		{
			Log.Trace("Раскрыть выпадающий список");
			ClickElement(By.XPath(TERM_BASE_COMBOBOX_TRIGGER_XPATH));
		}

		public bool CheckGlossaryByName(string glossaryName)
		{
			Log.Trace(string.Format("Получить, есть ли словарь с заданным именем. имя словаря {0}", glossaryName));
			return WaitUntilDisplayElement(By.XPath(TERM_BASE_BOUNDLIST_XPATH.Replace("#", glossaryName)));		
		}

		public void SelectGlossaryByName(string glossaryName)
		{
			Log.Trace(string.Format("Выбрать словарь из выпадающего списка. имя словаря {0}", glossaryName));
			ClickElement(By.XPath(TERM_BASE_BOUNDLIST_XPATH.Replace("#", glossaryName)));
		}

		public bool WaitConfirmSingleTermMessage()
		{
			Log.Trace("Получить, появилось ли сообщение о добавлении одиночного термина");
			return WaitUntilDisplayElement(By.XPath(CONFIRM_SINGLE_TERM_MESSAGE_XPATH), 5);
		}

		public bool WaitAnyWayTermMessage()
		{
			Log.Trace("Получить, появилось ли сообщение 'Do you want to add the term anyway?'");
			return WaitUntilDisplayElement(By.XPath(CONFIRM_ANYWAY_ADD_TERM_MESSAGE_XPATH), 5);
		}

		public void CliCkYesBtnInAnyWayTermMessage()
		{
			Log.Trace("Кликнуть Yes в сообщении 'Do you want to add the term anyway?'");
			ClickElement(By.XPath(YES_BTN_XPATH));
		}

		public bool WaitTermSavedMessage()
		{
			Log.Trace("Получить, появилось ли сообщение о сохранении термина");
			return WaitUntilDisplayElement(By.XPath(TERM_SAVED_MESSAGE_XPATH), 15);
		}

		public bool WaitContainsTermMessage()
		{
			Log.Trace("Получить, появилось ли сообщение о повторном добавлении термин");
			return WaitUntilDisplayElement(By.XPath(CONTAINS_TERM_MESSAGE_XPATH), 5);
		}

		protected const string CANCEL_BTN_XPATH = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Cancel')]";
		protected const string ADD_BTN_XPATH = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Add')]";
		protected const string СONFIRM_SINGLE_TERM_BTN_XPATH = CONFIRM_SINGLE_TERM_MESSAGE_XPATH + "//span[contains(string(), 'Yes')]";
		protected const string TERM_SAVED_OK_BTN_XPATH = TERM_SAVED_MESSAGE_XPATH + "//span[contains(string(), 'OK')]";
		protected const string CONTAINS_TERM_NO_BTN_XPATH = CONTAINS_TERM_MESSAGE_XPATH + "//span[contains(string(), 'No')]";
		protected const string CONTAINS_TERM_YES_BTN_XPATH = CONTAINS_TERM_MESSAGE_XPATH + "//span[contains(string(), 'Yes')]";

		protected const string SOURCE_TERM_VALUE_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm') and contains(@value, '#')]";
		protected const string TARGET_TERM_VALUE_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm') and contains(@value, '#')]";
		protected const string SOURCE_TERM_INPUT_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm')]";
		protected const string TARGET_TERM_INPUT_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm')]";
		protected const string COMMENT_INPUT_XPATH = "//div[contains(@id, 'term-window')]//textarea[contains(@name, 'comment')]";

		protected const string TERM_BASE_COMBOBOX_TRIGGER_XPATH = "//div[contains(@id, 'term-window')]//div[contains(@id, 'trigger-picker')]";
		protected const string TERM_BASE_BOUNDLIST_XPATH = "//ul[contains(@id, 'boundlist')]//li[contains(string(), '#')]";

		protected const string TERM_SAVED_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'The term has been saved')]";
		protected const string CONTAINS_TERM_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'This glossary already contains term(s)')]";
		protected const string CONFIRM_SINGLE_TERM_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'Do you want to add a term without translation?')]";
		protected const string CONFIRM_ANYWAY_ADD_TERM_MESSAGE_XPATH = "//div[@id= 'messagebox' and contains(string(), 'Do you want to add the term anyway?')]";
		protected const string YES_BTN_XPATH = "//div[@id= 'messagebox' and contains(string(), 'Do you want to add the term anyway?')]//span[text()='Yes']";

		private static readonly Logger Log = LogManager.GetCurrentClassLogger();
	}

}
