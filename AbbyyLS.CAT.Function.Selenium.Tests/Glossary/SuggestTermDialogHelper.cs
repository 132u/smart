using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер диалога редактирования термина
	/// </summary>
	public class SuggestTermDialogHelper : CommonHelper
	{
		public SuggestTermDialogHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
		}

		public void WaitPageLoad()
		{
			Logger.Trace("Проверка появления диалога");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(DIALOG_XPATH)),
				"Ошибка: диалог не появился");
		}

		public void WaitPageClose()
		{
			Logger.Trace("Ожидание закрытия формы");
			WaitUntilDisappearElement(By.XPath(DIALOG_XPATH));
			WaitUntilDisappearElement(By.XPath(GRAY_BACKGROUND));
		}

		public void FillTerm(int termNumber, string text)
		{
			Logger.Debug(string.Format("Заполнить термин #{0} текстом {1}", termNumber, text));
			SendTextElement(By.XPath(TERM_XPATH + "[" + termNumber + "]" + TERM_INPUT_XPATH), text);
		}

		public void OpenGlossaryList()
		{
			Logger.Debug("Открыть список глоссариев");
			ClickElement(By.XPath(OPEN_GLOSSARY_LIST_XPATH));

			Logger.Trace("Проверка: открыт список глоссариев");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(LIST_XPATH)),
				"Ошибка: список глоссариев не открылся");
		}

		public void SelectGlossary(string glossaryName)
		{
			Logger.Debug(string.Format("Выбрать глоссарий {0} из списка", glossaryName));
			ClickElement(By.XPath(GLOSSARY_ITEM_XPATH + glossaryName + "']"));
		}

		public void ClickSave()
		{
			Logger.Debug("Нажать кнопку сохранения");
			ClickElement(By.XPath(SAVE_BTN_XPATH));
		}

		public void AssertionIsExistCreateTermError()
		{
			Logger.Trace("Проверка наличия ошибки при создании термина");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(ERROR_MESSAGE_XPATH)),
				"Ошибка: не появилась ошибка пустого термина");
		}

		public string GetLanguageId(int termNumber)
		{
			Logger.Debug(string.Format("Получить id языка для термина #{0}", termNumber));
			return GetElementAttribute(By.XPath(TERM_XPATH + "[" + termNumber + "]" + LANGUAGE_XPATH), "data-id");
		}

		public bool OpenLanguageList(int termNumber)
		{
			Logger.Debug(string.Format("Кликнуть по языку #{0}", termNumber));

			ClickElement(By.XPath(TERM_XPATH + "[" + termNumber + "]" + LANGUAGE_XPATH));

			return WaitUntilDisplayElement(By.XPath(LIST_XPATH));
		}

		public bool SelectLanguage(string id)
		{
			Logger.Debug(string.Format("Выбрать язык. Id языка {0}", id));

			ClickElement(By.XPath(LANGUAGE_ITEM_XPATH + id + "']"));

			return WaitUntilDisappearElement(By.XPath(LIST_XPATH));
		}

		public void AssertionIsExistDuplicateWarning()
		{
			Logger.Trace("Проверка существования предупреждения о существующем термине");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(DUPLICATE_WARNING_XPATH)),
				"Ошибка: не появилось предупреждение о существующем термине");
		}

		public void AssertSuggestTermDialogClosed()
		{
			Logger.Trace("Проверить, что диалог создания термина закрылся");

			Assert.IsTrue(WaitUntilDisappearElement(By.XPath(CANCEL_BTN_XPATH)),
				"Произошла ошибка:\n диалог создания термина не закрылся");
		}

		public void ClickCancel()
		{
			Logger.Debug("Нажать кнопку отмены");
			ClickElement(By.XPath(CANCEL_BTN_XPATH));
		}


		protected const string GRAY_BACKGROUND = "//div[@class='g-popup-bg js-popup-bg']";
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