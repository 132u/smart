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

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер диалога редактирования термина
	/// </summary>
	public class SuggestTermDialogHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public SuggestTermDialogHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// <summary>
		/// Дождаться появления диалога
		/// </summary>
		/// <returns>появился</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(DIALOG_XPATH));
		}

		/// <summary>
		/// Ждем закрытия формы
		/// </summary>
		/// <returns>закрылась</returns>
		public bool WaitPageClose()
		{
			return WaitUntilDisappearElement(By.XPath(DIALOG_XPATH));
		}

		/// <summary>
		/// Заполнить термин
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		/// <param name="text">текст</param>
		public void FillTerm(int termNumber, string text)
		{
			SendTextElement(By.XPath(TERM_XPATH + "[" + termNumber + "]" + TERM_INPUT_XPATH), text);
		}

		/// <summary>
		/// Открыть список глоссариев
		/// </summary>
		/// <returns>открылся</returns>
		public bool OpenGlossaryList()
		{
			ClickElement(By.XPath(OPEN_GLOSSARY_LIST_XPATH));
			return WaitUntilDisplayElement(By.XPath(LIST_XPATH));
		}

		/// <summary>
		/// Выбрать глоссарий
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		public void SelectGlossary(string glossaryName)
		{
			ClickElement(By.XPath(GLOSSARY_ITEM_XPATH + glossaryName + "']"));
		}

		/// <summary>
		/// Кликнуть Save
		/// </summary>
		public void ClickSave()
		{
			ClickElement(By.XPath(SAVE_BTN_XPATH));
			Console.WriteLine("кликнули сохранить");
		}

		/// <summary>
		/// Вернуть, есть ли ошибка при создании термина
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistCreateTermError()
		{
			return WaitUntilDisplayElement(By.XPath(ERROR_MESSAGE_XPATH));
		}

		/// <summary>
		/// Получить id языка
		/// </summary>
		/// <param name="termNumber">номер термина</param>
		/// <returns>id языка</returns>
		public string GetLanguageId(int termNumber)
		{
			return GetElementAttribute(By.XPath(TERM_XPATH + "[" + termNumber + "]" + LANGUAGE_XPATH), "data-id");
		}

		/// <summary>
		/// Кликнуть по языку
		/// </summary>
		/// <param name="termNumber">номер языка</param>
		/// <returns>открылся список</returns>
		public bool OpenLanguageList(int termNumber)
		{
			ClickElement(By.XPath(TERM_XPATH + "[" + termNumber + "]" + LANGUAGE_XPATH));
			return WaitUntilDisplayElement(By.XPath(LIST_XPATH));
		}

		/// <summary>
		/// Выбрать язык
		/// </summary>
		/// <param name="id">id языка</param>
		/// <returns>закрылся список</returns>
		public bool SelectLanguage(string id)
		{
			ClickElement(By.XPath(LANGUAGE_ITEM_XPATH + id + "']"));
			return WaitUntilDisappearElement(By.XPath(LIST_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли предупреждение о дубликате
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsExistDuplicateWarning()
		{
			return GetIsElementDisplay(By.XPath(DUPLICATE_WARNING_XPATH));
		}

		/// <summary>
		/// Нажать отмену
		/// </summary>
		public void ClickCancel()
		{
			ClickElement(By.XPath(CANCEL_BTN_XPATH));
		}



		protected const string DIALOG_XPATH = "//div[contains(@class,'js-add-suggest-popup')]";
		protected const string TERM_XPATH = "//div[contains(@class, 'l-addsugg__contr lang js-language')]";
		protected const string TERM_INPUT_XPATH = "//input[contains(@class, 'js-addsugg-term')]";
		protected const string OPEN_GLOSSARY_LIST_XPATH = "//span[contains(@class, 'js-dropdown addsuggglos')]";
		protected const string LIST_XPATH = "//span[contains(@class, 'js-dropdown__list')]";
		protected const string ITEM_XPATH = "//span[contains(@class,'js-dropdown__item')]";
		protected const string GLOSSARY_ITEM_XPATH = ITEM_XPATH + "[@title='";
		protected const string LANGUAGE_ITEM_XPATH = ITEM_XPATH + "[@data-id='";
		protected const string SAVE_BTN_XPATH = "//input[contains(@class, 'js-save-btn')]";
		protected const string CANCEL_BTN_XPATH = DIALOG_XPATH + "//a[contains(@class,'g-popupbox__cancel js-popup-close')]";
		
		protected const string ERROR_MESSAGE_XPATH = DIALOG_XPATH + "//div[contains(@class,'js-error-message')]";
		protected const string LANGUAGE_XPATH = "//span[contains(@class,'js-dropdown__text addsugglang')]";
		protected const string DUPLICATE_WARNING_XPATH = "//div[contains(@class,'js-duplicate-warning')]";
	}
}