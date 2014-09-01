using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;
using System.Threading;

namespace AbbyyLs.CAT.Function.Selenium.Tests
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
		public void ClickSaveBtn()
		{
			ClickElement(By.XPath(SAVE_BTN_XPATH));
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

		/// <summary>
		/// Набрать текст в  SourceTerm		
		/// </summary>
        /// <param name="sourceText">текст для ввода</param>
		public void TypeSourceTermText(string sourceText)
		{
			ClickClearAndAddText(By.XPath(SOURCE_TERM_INPUT_XPATH), sourceText);
		}

		/// <summary>
		/// Набрать комментарий		
		/// </summary>
        /// <param name="commentText">текст комментария</param>
		public void TypeCommentText(string commentText)
		{
			SendTextElement(By.XPath(COMMENT_INPUT_XPATH), commentText);            
		}

        /// <summary>
        /// Раскрыть выпадающий список        
        /// </summary>
        public void OpenGlossaryList()
        {
            ClickElement(By.XPath(TERM_BASE_COMBOBOX_TRIGGER_XPATH));
        }

		/// <summary>
		/// Получить, есть ли словарь с заданным именем		
		/// </summary>
        /// <param name="glossaryName">имя словаря</param>
		/// <returns>есть словарь</returns>
		public bool CheckGlossaryByName(string glossaryName)
		{			
            return WaitUntilDisplayElement(By.XPath(TERM_BASE_BOUNDLIST_XPATH.Replace("#", glossaryName)));		
		}

		/// <summary>
		/// Выбрать словарь из выпадающего списка 		
		/// </summary>
        /// <param name="glossaryName">имя словаря</param>
		public void SelectGlossaryByName(string glossaryName)
		{
			ClickElement(By.XPath(TERM_BASE_BOUNDLIST_XPATH.Replace("#", glossaryName)));
		}

        /// <summary>
        /// Получить, появилось ли сообщение о добавлении одиночного термина      
        /// </summary>
        /// <returns>есть</returns>
        public bool WaitConfirmSingleTermMessage()
        {
            return WaitUntilDisplayElement(By.XPath(CONFIRM_SINGLE_TERM_MESSAGE_XPATH), 5);
        }

        /// <summary>
        /// Получить, появилось ли сообщение о сохранении термина     
        /// </summary>
        /// <returns>есть</returns>
        public bool WaitTermSavedMessage()
        {
            return WaitUntilDisplayElement(By.XPath(TERM_SAVED_MESSAGE_XPATH), 15);
        }

        /// <summary>
        /// Получить, появилось ли сообщение о повторном добавлении термина    
        /// </summary>
        /// <returns>есть</returns>
        public bool WaitContainsTermMessage()
        {
            return WaitUntilDisplayElement(By.XPath(CONTAINS_TERM_MESSAGE_XPATH), 5);
        }

		protected const string CANCEL_BTN_XPATH = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Cancel')]";
		protected const string SAVE_BTN_XPATH = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Save')]";
		protected const string СONFIRM_SINGLE_TERM_BTN_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'Do you want to add a single term?')]//span[contains(string(), 'Yes')]";
		protected const string TERM_SAVED_OK_BTN_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'The term has been saved!')]//span[contains(string(), 'OK')]";
		protected const string CONTAINS_TERM_NO_BTN_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'This glossary already contains term(s)')]//span[contains(string(), 'No')]";
		protected const string CONTAINS_TERM_YES_BTN_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'This glossary already contains term(s)')]//span[contains(string(), 'Yes')]";

		protected const string SOURCE_TERM_VALUE_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm') and contains(@value, '#')]";
        protected const string TARGET_TERM_VALUE_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm') and contains(@value, '#')]";
        protected const string SOURCE_TERM_INPUT_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm')]";
        protected const string TARGET_TERM_INPUT_XPATH = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm')]";
		protected const string COMMENT_INPUT_XPATH = "//div[contains(@id, 'term-window')]//textarea[contains(@name, 'comment')]";

		protected const string TERM_BASE_COMBOBOX_TRIGGER_XPATH = "//div[contains(@id, 'term-window')]//div[contains(@id, 'trigger-picker')]";
		protected const string TERM_BASE_BOUNDLIST_XPATH = "//ul[contains(@id, 'boundlist')]//li[contains(string(), '#')]";

		protected const string TERM_SAVED_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'The term has been saved!')]";
		protected const string CONTAINS_TERM_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'This glossary already contains term(s)')]";
		protected const string CONFIRM_SINGLE_TERM_MESSAGE_XPATH = "//div[contains(@id, 'messagebox') and contains(string(), 'Do you want to add a single term?')]";
	}
}
