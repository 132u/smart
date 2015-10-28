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

		public void ClickAddBtn()
		{
			Logger.Debug("Нажать кнопку Сохранить");
			ClickElement(By.XPath(ADD_BTN_XPATH));
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
		protected const string CLOSE_BTN_IN_TERM_SAVED_MESSAGE_XPATH = "//span[contains(@id, 'toolEl') and contains(@class, 'x-tool-close')]";
		protected const string TERM_SAVED_MESSAGE_XPATH = "//div[contains(@id, 'innerCt') and contains(string(), 'The term has been saved')]";
		protected const string CONTAINS_TERM_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'This glossary already contains term(s)')]";
		protected const string CONFIRM_SINGLE_TERM_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'Do you want to add a term without translation?')]";
		protected const string CONFIRM_ANYWAY_ADD_TERM_MESSAGE_XPATH = "//div[@id= 'messagebox' and contains(string(), 'Do you want to add the term anyway?')]";
		protected const string YES_BTN_XPATH = "//div[@id= 'messagebox' and contains(string(), 'Do you want to add the term anyway?')]//span[text()='Yes']";
	}

}
