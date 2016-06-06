using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.MachineTranslation
{
	public class FastMTTextPage : FastMTSettingsBasePage<FastMTTextPage>
	{
		public FastMTTextPage(WebDriver driver)
			: base(driver)
		{
		}

		override public FastMTTextPage LoadPage()
		{
			if (!IsFastMTTextPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница 'Machine Translation - перевод текста'");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Ввести текст для перевода
		/// </summary>
		/// <param name="text">текст для перевода</param>
		public FastMTTextPage SetTextToTranslate(string text)
		{
			CustomTestContext.WriteLine(string.Format("Ввести текст '{0}' в поле для перевода", text));
			SourceTextArea.SetText(text);

			return LoadPage();
		}

		/// <summary>
		/// Получить текст перевода
		/// </summary>
		public string GetTranslationText()
		{
			CustomTestContext.WriteLine("Получить текст перевода");
			return TargetText.Text;
		}

		/// <summary>
		/// Нажать на кнопку 'Translate'
		/// </summary>
		public FastMTTextPage ClickTranslateButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Translate'");
			TranslateButton.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы
		#endregion

		#region Методы, проверяющие состояние страницы

		protected bool IsFastMTTextPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TEXT_AREA));
		}

		/// <summary>
		/// Появился ли перевод
		/// </summary>
		public bool IsTranslationAppeared()
		{
			if (!WaitLoadingCircleDispay())
			{
				CustomTestContext.WriteLine("Колесо ожидания завершения перевода не появилось");
				return true;
			}

			CustomTestContext.WriteLine("Ждем исчезновения колеса ожидания завершения перевода (ждем 90 секунд)");
			return Driver.WaitUntilElementIsDisappeared(By.XPath(LOADING_CIRCLE), 90);
		}

		#endregion

		#region Методы, ожидающие определенного состояния страницы

		/// <summary>
		/// Дождаться появления колеса ожидания завершения перевода
		/// </summary>
		public bool WaitLoadingCircleDispay()
		{
			CustomTestContext.WriteLine("Дождаться появления колеса ожидания завершения перевода");
			return Driver.WaitUntilElementIsDisplay(By.XPath(LOADING_CIRCLE));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SOURCE_TEXT_AREA)]
		protected IWebElement SourceTextArea { get; set; }

		[FindsBy(How = How.XPath, Using = LOADING_CIRCLE)]
		protected IWebElement LoadingCircle { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_TEXT)]
		protected IWebElement TargetText { get; set; }

		[FindsBy(How = How.XPath, Using = TRANSLATE_BUTTON)]
		protected IWebElement TranslateButton { get; set; }
		
		#endregion

		#region Описание XPath элементов

		protected const string SOURCE_TEXT_AREA = "//textarea[@data-bind='textInput: sourceText']";
		protected const string LOADING_CIRCLE = "//div[contains(@class, 'l-fastmt__loader')]";
		protected const string TARGET_TEXT = "//div[@data-bind='text: targetText']";
		protected const string TRANSLATE_BUTTON = "//div[contains(@data-bind,'beginTextTranslation')]//a";

		#endregion

	}
}
