using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class AddTermDialog : BaseObject, IAbstractPage<AddTermDialog>
	{
		public AddTermDialog GetPage()
		{
			var addTermDialog = new AddTermDialog();
			InitPage(addTermDialog);

			return addTermDialog;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TERM)))
			{
				Assert.Fail("Произошла ошибка:\n не открылся диалог создания нового термина.");
			}
		}

		/// <summary>
		/// Ввести текст в source
		/// </summary>
		public AddTermDialog FillSourceTerm(string sourceTerm)
		{
			Logger.Debug("Ввести {0} в source.", sourceTerm);
			SourceTerm.SetText(sourceTerm);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в target
		/// </summary>
		public AddTermDialog FillTargetTerm(string targetTerm)
		{
			Logger.Debug("Ввести {0} в target.", targetTerm);
			TargetTerm.SetText(targetTerm);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Add в диалоге создания термина
		/// </summary>
		public T ClickAddButton<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку Add в диалоге создания термина");
			AddButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel в диалоге создания термина
		/// </summary>
		public EditorPage ClickCancel()
		{
			Logger.Debug("Нажать кнопку Cancel в диалоге создания термина");
			CancelButton.Click();

			return new EditorPage().GetPage();
		}

		/// <summary>
		/// Проверить, что сработало автозаполнение текста в SourceTerm в окне добавления термина
		/// </summary>
		public AddTermDialog AssertTextExistInSourceTerm(string text)
		{
			Logger.Trace("Проверить, что сработало автозаполнение текста {0} в SourceTerm", text);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TERM_VALUE.Replace("*#*", text))),
				"Произошла ошибка:\n Нет автозаполнения сорса.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что сработало автозаполнение текста в TargetTerm в окне добавления термина
		/// </summary>
		public AddTermDialog AssertTextExistInTargetTerm(string text)
		{
			Logger.Trace("Проверить, что сработало автозаполнение текста {0} в TargetTerm", text);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TARGET_TERM_VALUE.Replace("*#*", text))),
				"Произошла ошибка:\n Нет автозаполнения таргета.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появился диалог подтверждения сохранения термина без перевода
		/// </summary>
		public AddTermDialog AssertConfirmSingleTermMessageDisplayed()
		{
			Logger.Trace("Проверить, что появился диалог подтверждения сохранения термина без перевода.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_TERM_WITHOUT_TRANSLATION_DIALOG)),
				"Произошла ошибка:\n не появился диалог подтверждения сохранения термина без перевода.");

			return GetPage();
		}

		/// <summary>
		/// Нажать Yes в окне подтверждения добавления термина
		/// </summary>
		public EditorPage Confirm()
		{
			Logger.Debug("Нажать Yes в окне подтверждения.");
			ConfirmYesButton.Click();

			return new EditorPage().GetPage();
		}

		/// <summary>
		/// Ввести текст в поле "Комментарий" в окне добавления текрмина.
		/// </summary>
		/// /// <param name="text">комментарий</param>
		public AddTermDialog EnterComment(string text)
		{
			Logger.Debug("Ввести текст в поле 'Комментарий'.");
			CommentInput.SetText(text);

			return GetPage();
		}

		/// <summary>
		/// Проверить наличие глоссария в выпадающем списке окна добавления термина
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public AddTermDialog AssertGlossaryExistInDropdown(string glossaryName)
		{
			Logger.Trace("Нажать на выпадающий список, чтобы раскрыть его.");
			GlossarySelect.Click();

			Logger.Trace("Проверить наличие глоссария {0} в выпадающем списке.", glossaryName);
			var glossary = Driver.SetDynamicValue(How.XPath, GLOSSARY_SELECT_BOUNDLIST, glossaryName);

			Assert.IsTrue(glossary.Displayed, "Произошла ошибка:\n глоссарий не найден в выпадающем списке.");

			return GetPage();
		}

		/// <summary>
		/// Выбрать глоссарий в выпадающем списке окна добавления термина
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public AddTermDialog SelectGlossaryByName(string glossaryName)
		{
			Logger.Debug("Выбрать глоссарий {0} из выпадающего списка.", glossaryName);
			GlossarySelect.Click();
			var glossary = Driver.SetDynamicValue(How.XPath, GLOSSARY_SELECT_BOUNDLIST, glossaryName);
			glossary.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = SOURCE_TERM)]
		protected IWebElement SourceTerm { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_TERM)]
		protected IWebElement TargetTerm { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddButton { get; set; }

		[FindsBy(How = How.XPath, Using = СONFIRM_YES_BTN)]
		protected IWebElement ConfirmYesButton { get; set; }

		[FindsBy(How = How.XPath, Using = COMMENT_INPUT)]
		protected IWebElement CommentInput { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_SELECT)]
		protected IWebElement GlossarySelect { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		protected const string SOURCE_TERM = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm')]";
		protected const string SOURCE_TERM_VALUE = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm') and contains(@value, '*#*')]";
		protected const string TARGET_TERM_VALUE = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm') and contains(@value, '*#*')]";
		protected const string TARGET_TERM = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm')]";
		protected const string COMMENT_INPUT = "//div[contains(@id, 'term-window')]//textarea[contains(@name, 'comment')]";
		protected const string ADD_BUTTON = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Add')]";
		protected const string CANCEL_BUTTON = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Cancel')]";
		protected const string CONFIRM_TERM_WITHOUT_TRANSLATION_DIALOG = "//div[contains(@id, 'messagebox') and contains(string(), 'Do you want to add a term without translation?')]";
		protected const string СONFIRM_YES_BTN = "//div[contains(@id, 'messagebox')]//span[contains(string(), 'Yes')]";
		protected const string GLOSSARY_SELECT = "//input[@name='glossaryId']";
		protected const string GLOSSARY_SELECT_BOUNDLIST = "//ul[contains(@id, 'boundlist')]//li[contains(string(), '*#*')]";
	}
}