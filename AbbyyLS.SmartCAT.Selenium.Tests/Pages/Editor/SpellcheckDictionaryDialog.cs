using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class SpellcheckDictionaryDialog : EditorPage, IAbstractPage<SpellcheckDictionaryDialog>
	{
		public SpellcheckDictionaryDialog(WebDriver driver) : base(driver)
		{
		}

		public new SpellcheckDictionaryDialog LoadPage()
		{
			if (!IsSpellcheckDictionaryDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился словарь");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку закрытия словаря
		/// </summary>
		public EditorPage ClickCloseDictionaryButton() 
		{
			CustomTestContext.WriteLine("Нажать кнопку закрытия словаря");
			CloseDictionaryButton.Click();

			return new EditorPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку добавления слова в словарь
		/// </summary>
		public SpellcheckDictionaryDialog ClickAddWordButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку добавления слова в словарь");
			Thread.Sleep(1000);
			AddWordButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку удаления слова из словаря
		/// </summary>
		public SpellcheckDictionaryDialog ClickDeleteWordButton(string word)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления слова {0} из словаря", word);
			Driver.WaitUntilElementIsDisplay(By.XPath(WORD_PATH.Replace("*#*", word)));
			Word = Driver.SetDynamicValue(How.XPath, WORD_PATH, word);
			Word.HoverElement();

			DeleteWordButton = Driver.SetDynamicValue(How.XPath, DELETE_WORD_BUTTON, word);
			DeleteWordButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выделить слово в словаре
		/// </summary>
		public SpellcheckDictionaryDialog HilightWordInDictionary(string word)
		{
			Word = Driver.SetDynamicValue(How.XPath, WORD_PATH, word);
			CustomTestContext.WriteLine("Установить курсор в конце слова {0} в словаре", word);
			Word.DoubleClick();
			CustomTestContext.WriteLine("Выделить слово {0}", word);
			Word.DoubleClick();
			
			return LoadPage();
		}

		/// <summary>
		/// Добавить слово в словарь
		/// </summary>
		public SpellcheckDictionaryDialog FillWordField(string word)
		{
			CustomTestContext.WriteLine("Добавить слово {0} в словарь", word);
			InputWordField.DoubleClick();
			InputWordField.SendKeys(word);

			return LoadPage();
		}

		/// <summary>
		/// Подтвердить ввод слова в словарь
		/// </summary>
		public T ConfirmWord<T>(WebDriver driver) where T: class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Подтвердить ввод слова в словарь");
			DictionaryForm.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.LoadPage();
		}

		/// <summary>
		/// Получить список слов из словаря
		/// </summary>
		/// <returns>Список слов из словаря</returns>
		public List<string> GetWordsList()
		{
			CustomTestContext.WriteLine("Получить список слов из словаря");
			Driver.WaitUntilElementIsDisplay(By.XPath(WORDS_LIST));

			return Driver.GetElementList(By.XPath(WORDS_LIST)).Select(webElement => webElement.Text).ToList();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Добавить слово в словарь
		/// </summary>
		/// <param name="word">слово</param>
		public EditorPage AddWordToDictionary(string word)
		{
			ClickAddWordButton();
			FillWordField(word);
			ConfirmWord<SpellcheckDictionaryDialog>(Driver);
			var editorPage = ClickCloseDictionaryButton();

			return editorPage;
		}

		/// <summary>
		/// Удалить все слова из словаря
		/// </summary>
		public EditorPage RemoveAllWordsFromDictionary()
		{
			var wordsList = GetWordsList();
			wordsList.ForEach(word => ClickDeleteWordButton(word));
			var editorPage = ClickCloseDictionaryButton();

			return editorPage;
		}

		/// <summary>
		/// Заменить слово в словаре
		/// </summary>
		/// <param name="oldWord">старое слово</param>
		/// <param name="newWord">новое слово</param>
		public EditorPage ReplaceWordInDictionary(string oldWord, string newWord)
		{
			HilightWordInDictionary(oldWord);
			FillWordField(newWord);
			ConfirmWord<SpellcheckDictionaryDialog>(Driver);
			var editorPage = ClickCloseDictionaryButton();

			return editorPage;
		}

		/// <summary>
		/// Удалить слово из словаря
		/// </summary>
		/// <param name="word">слово</param>
		public EditorPage DeleteWordFromDictionary(string word)
		{
			ClickDeleteWordButton(word);
			var editorPage = ClickCloseDictionaryButton();

			return editorPage;
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли словарь
		/// </summary>
		public bool IsSpellcheckDictionaryDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DICTIONARY_FORM));
		}

		/// <summary>
		/// Проверить, что слово присутствует в словаре
		/// </summary>
		public bool IsWordExistInDictionary(string word)
		{
			CustomTestContext.WriteLine("Проверить, что слово {0} присутствует в словаре", word);

			return GetWordsList().Contains(word);
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = INPUT_WORD)]
		protected IWebElement InputWordField { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_DICTIONARY_BTN)]
		protected IWebElement CloseDictionaryButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_WORD_BTN)]
		protected IWebElement AddWordButton { get; set; }

		[FindsBy(How = How.XPath, Using = DICTIONARY_FORM)]
		protected IWebElement DictionaryForm { get; set; }

		protected IWebElement DeleteWordButton { get; set; }

		protected IWebElement Word { get; set; }

		#endregion

		#region Описание  XPath элементов страницы

		protected const string DICTIONARY_FORM = "//div[@id='dictionary_header']";
		protected const string ADD_WORD_BTN = "//div[@id='dictionary']//span[contains(@id, 'btnInnerEl')]";
		protected const string INPUT_WORD = "//div[@id='dictionary']//input[contains(@id, 'textfield')]";
		protected const string DELETE_WORD_BUTTON = "//div[@id='dictionary-body']//table//tr//td[1]//div[text()='*#*']/../..//td[2]//div[contains(@class, 'sci-delete')]";
		protected const string WORD_PATH = "//div[@id='dictionary']//table//tr//td[1]//div[text()='*#*']";
		protected const string WORDS_LIST = "//div[@id='dictionary']//table//tr//td[1]";
		protected const string CLOSE_DICTIONARY_BTN = "//div[@id='dictionary_header']//div[contains(@class, 'x-tool-close')]";

		#endregion
	}
}
