using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search
{
	class ExamplesFromTextsDialog : SearchPage, IAbstractPage<ExamplesFromTextsDialog>
	{
		public ExamplesFromTextsDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new ExamplesFromTextsDialog LoadPage()
		{
			if (!IsExamplesFromTextsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог примеров перевода в тексте.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на стрелку для отображения названия ТМ.
		/// </summary>
		public ExamplesFromTextsDialog ClickOnArrowButton()
		{
			CustomTestContext.WriteLine("Нажать на стрелку для отображения названия ТМ.");
			ArrowButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть на найденный пример перевода.
		/// </summary>
		public SearchPage ClickOnFoundedTranslationWord()
		{
			CustomTestContext.WriteLine("Кликнуть на найденный пример перевода.");
			FoundedTranslationWord.Click();

			return new SearchPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли диалог ответов техподдержки.
		/// </summary>
		public bool IsExamplesFromTextsDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(EXAMPLES_FROM_TEXTS_DIALOG));
		}

		/// <summary>
		/// Проверить, совпадает ли название ТМ с названием ТМ созданного проекта.
		/// </summary>
		/// <param name="uniqueProjectName">имя проекта</param>
		public bool IsTmNameCorrect(string uniqueProjectName)
		{
			CustomTestContext.WriteLine("Проверить, совпадает ли название ТМ с названием ТМ созданного проекта.");
			var tmNameFromDialog = Driver.GetTextListElement(By.XPath(TM_NAME));

			return uniqueProjectName == tmNameFromDialog[0];
		}

		/// <summary>
		/// Проверить, совпадает ли текст из сегмента target с отображенным текстом в диалоге.
		/// </summary>
		/// <param name="targetSentense">фраза из таргет сегмента редактора</param>
		public bool IsTagetTextInEditorAndFindedTextSame(string targetSentense)
		{
			CustomTestContext.WriteLine("Проверить, совпадает ли текст из сегмента target с отображенным текстом в диалоге.");
			var sentenceFromDialog = Driver.GetTextListElement(By.XPath(TARGET_TEXT));

			return targetSentense == sentenceFromDialog[0];
		}

		/// <summary>
		/// Проверить, совпадает ли текст из сегмента source с отображенным текстом в диалоге.
		/// </summary>
		/// <param name="sourceSentence">фраза из сорс сегмента редактора</param>
		public bool IsSourceTextInEditorAndFindedTextSame(string sourceSentence)
		{
			CustomTestContext.WriteLine("Проверить, совпадает ли текст из сегмента source с отображенным текстом в диалоге.");
			var sentenceFromDialog = Driver.GetTextListElement(By.XPath(SOURCE_TEXT));

			return sourceSentence == sentenceFromDialog[0];
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ARROW_BUTTON)]
		protected IWebElement ArrowButton { get; set; }

		[FindsBy(How = How.XPath, Using = FOUND_TRANSLATION)]
		protected IWebElement FoundedTranslationWord { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string ARROW_BUTTON = "//td[contains(@class, 'g-winexamp__tdExamp action')]";
		protected const string EXAMPLES_FROM_TEXTS_DIALOG = "//div[contains(@class, 'g-winexamp__hd')]//h4[contains(text(), 'Examples from texts')]";
		protected const string TM_NAME = "//div[contains(@class, 'g-winexamp__sources')]";
		protected const string SOURCE_TEXT = "//div[contains(@class, 'g-winexamp__text js-second-source-text')]";
		protected const string TARGET_TEXT = "//div[contains(@class, 'g-winexamp__text js-first-source-text')]";
		protected const string FOUND_TRANSLATION = "//div[contains(@class, 'g-winexamp__transl')]//a[contains(@class, 'g-bold g-link')]";

		#endregion
	}
}