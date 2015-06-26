using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class SpellcheckDictionaryDialog : EditorPage, IAbstractPage<SpellcheckDictionaryDialog>
	{
		public new SpellcheckDictionaryDialog GetPage()
		{
			var spellcheckDictionaryDialog = new SpellcheckDictionaryDialog();
			InitPage(spellcheckDictionaryDialog);

			return spellcheckDictionaryDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(DICTIONARY_FORM)))
			{
				Assert.Fail("Произошла ошибка:\n не появился словарь.");
			}
		}

		/// <summary>
		/// Нажать кнопку закрытия словаря
		/// </summary>
		public EditorPage ClickCloseDictionaryButton() 
		{
			Logger.Debug("Нажать кнопку закрытия словаря");
			CloseDictionaryButton.Click();

			return new EditorPage().GetPage();
		}

		/// <summary>
		/// Проверить доступность кнопки добавления слова в словарь
		/// </summary>
		public SpellcheckDictionaryDialog AssertAddWordButtinEnabled()
		{
			Logger.Trace("Проверить доступность кнопки добавления слова в словарь");

			Assert.IsTrue(Driver.WaitUntilElementIsEnabled(By.XPath(ADD_WORD_BTN)) && Driver.WaitUntilElementIsDisplay(By.XPath(ADD_WORD_BTN)),
				"Произошла ошибка:\n кнопка добавления слова в словарь недоступна.");

			return GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку добавления слова в словарь
		/// </summary>
		public SpellcheckDictionaryDialog ClickAddWordButton()
		{
			Logger.Debug("Нажать кнопку добавления слова в словарь");
			AddWordButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления слова из словаря
		/// </summary>
		public SpellcheckDictionaryDialog ClickDeleteWordButton(string word)
		{
			Logger.Debug("Нажать кнопку удаления слова {0} из словаря", word);
			DeleteWordButton = Driver.SetDynamicValue(How.XPath, DELETE_WORD_BUTTON, word);
			DeleteWordButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что слово присутствует в словаре
		/// </summary>
		public SpellcheckDictionaryDialog AssertWordExistInDictionary(string word)
		{
			Logger.Trace("Проверить, что слово {0} присутствует в словаре", word);
			
			Assert.IsTrue(GetWordsList().Contains(word),
				"Произошла ошибка:\n слово {0} не найдено в словаре.", word);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что слово не присутствует в словаре
		/// </summary>
		public SpellcheckDictionaryDialog AssertWordNotExistInDictionary(string word)
		{
			Logger.Trace("Проверить, что слово {0} не присутствует в словаре", word);

			Assert.IsFalse(GetWordsList().Contains(word),
				"Произошла ошибка:\n слово {0} найдено в словаре.", word);

			return GetPage();
		}

		/// <summary>
		/// Очистить слово в словаре
		/// </summary>
		public SpellcheckDictionaryDialog CleanWordInDictionary(string word)
		{
			Logger.Debug("Очистить слово {0} в словаре", word);
			Word = Driver.SetDynamicValue(How.XPath, WORD_PATH, word);
			Word.DoubleClick();
			SendKeys.SendWait(@"+{HOME}");
			SendKeys.SendWait(@"{DEL}");

			return GetPage();
		}

		/// <summary>
		/// Добавить слово в словарь
		/// </summary>
		public SpellcheckDictionaryDialog AddWordToDictionary(string word)
		{
			Logger.Debug("Добавить слово {0} в словарь", word);
			InputWordField.SendKeys(word);

			return GetPage();
		}

		/// <summary>
		/// Подтвердить ввод слова в словарь
		/// </summary>
		public T ConfirmWord<T>() where T: class, IAbstractPage<T>, new()
		{
			Logger.Debug("Подтвердить ввод слова в словарь");
			SendKeys.SendWait(@"{Enter}");

			return new T().GetPage();
		}

		/// <summary>
		/// Получить список слов из словаря
		/// </summary>
		/// <returns>Список слов из словаря</returns>
		public List<string> GetWordsList()
		{
			Logger.Trace("Получить список слов из словаря");

			return Driver.GetElementList(By.XPath(WORDS_LIST)).Select(webElement => webElement.Text).ToList();
		}

		[FindsBy(How = How.XPath, Using = INPUT_WORD)]
		protected IWebElement InputWordField { get; set; }

		[FindsBy(How = How.XPath, Using = CLOSE_DICTIONARY_BTN)]
		protected IWebElement CloseDictionaryButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_WORD_BTN)]
		protected IWebElement AddWordButton { get; set; }

		protected IWebElement DeleteWordButton { get; set; }

		protected IWebElement Word { get; set; }

		protected const string DICTIONARY_FORM= "//div[@id='dictionary']";
		protected const string ADD_WORD_BTN = "//div[@id='dictionary']//span[contains(@id, 'btnInnerEl')]";
		protected const string INPUT_WORD = "//div[@id='dictionary']//input[contains(@id, 'textfield')]";
		protected const string DELETE_WORD_BUTTON ="//div[@id='dictionary']//table//tr//td[1]//div[text()='*#*']/../..//td[2]//span[contains(@class, 'fa-trash')]";
		protected const string WORD_PATH = "//div[@id='dictionary']//table//tr//td[1]//div[text()='*#*']";
		protected const string WORDS_LIST = "//div[@id='dictionary']//table//tr//td[1]";
		protected const string CLOSE_DICTIONARY_BTN = "//div[@id='dictionary']//span[contains(@class, 'x-tool-close')]";
	}
}
