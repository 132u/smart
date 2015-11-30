using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class AddTermDialog : EditorPage, IAbstractPage<AddTermDialog>
	{
		public AddTermDialog(WebDriver driver) :base(driver)
		{
		}

		public new AddTermDialog GetPage()
		{
			var addTermDialog = new AddTermDialog(Driver);
			InitPage(addTermDialog, Driver);

			return addTermDialog;
		}

		public new void LoadPage()
		{
			if (!IsAddTermDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог создания нового термина");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести текст в source
		/// </summary>
		public AddTermDialog FillSourceTerm(string sourceTerm)
		{
			CustomTestContext.WriteLine("Ввести {0} в source.", sourceTerm);
			SourceTerm.SetText(sourceTerm);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в target
		/// </summary>
		public AddTermDialog FillTargetTerm(string targetTerm)
		{
			CustomTestContext.WriteLine("Ввести {0} в target.", targetTerm);
			TargetTerm.SetText(targetTerm);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Add в диалоге создания термина
		/// </summary>
		public EditorPage ClickAddButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Add в диалоге создания термина");
			AddButton.Click();

			return new EditorPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Add в диалоге создания термина, ожидая окно подтверждения
		/// </summary>
		public ConfirmTermWithoutTranskationDialog ClickAddButtonExpectingConfirmation()
		{
			CustomTestContext.WriteLine("Нажать кнопку Add в диалоге создания термина, ожидая окно подтверждения");
			AddButton.Click();

			return new ConfirmTermWithoutTranskationDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel в диалоге создания термина
		/// </summary>
		public EditorPage ClickCancelAddTerm()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel в диалоге создания термина");
			CancelButton.Click();

			return new EditorPage(Driver).GetPage();
		}

		/// <summary>
		/// Ввести текст в поле "Комментарий" в окне добавления текрмина.
		/// </summary>
		/// <param name="text">комментарий</param>
		public AddTermDialog EnterComment(string text)
		{
			CustomTestContext.WriteLine("Ввести текст в поле 'Комментарий'.");
			CommentInput.SetText(text);

			return GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Закрыть выпадающий список глоссария.
		/// </summary>
		public AddTermDialog CloseGlossarySelect()
		{
			CustomTestContext.WriteLine("Закрыть выпадающий список глоссария.");
			if (IsGlossaryListDisplayed())
			{
				GlossarySelect.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Открыть выпадающий список глоссария.
		/// </summary>
		public AddTermDialog OpenGlossarySelect()
		{
			CustomTestContext.WriteLine("Открыть выпадающий список глоссария.");
			if (!IsGlossaryListDisplayed())
			{
				GlossarySelect.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Выбрать глоссарий в выпадающем списке окна добавления термина
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public AddTermDialog SelectGlossaryByName(string glossaryName)
		{
			CustomTestContext.WriteLine("Выбрать глоссарий {0} из выпадающего списка.", glossaryName);
			GlossarySelect.Click();
			var glossary = Driver.SetDynamicValue(How.XPath, GLOSSARY_SELECT_BOUNDLIST, glossaryName);
			glossary.Click();

			return GetPage();
		}

		/// <summary>
		/// Добавить новый термин
		/// </summary>
		/// <param name="source">исходное слово</param>
		/// <param name="target">перевод</param>
		/// <param name="comment">комментарий</param>
		/// <param name="glossaryName">имя глоссария</param>
		public EditorPage AddNewTerm(string source, string target, string comment = null, string glossaryName = null)
		{
			FillSourceTerm(source);
			FillTargetTerm(target);

			if (comment != null)
			{
				EnterComment(comment);
			}

			if (glossaryName != null)
			{
				SelectGlossaryByName(glossaryName);
			}

			var editorPage = ClickAddButton();

			return editorPage;
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Проверить, открылся ли диалог создания нового термина
		/// </summary>
		public bool IsAddTermDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TERM));
		}

		/// <summary>
		/// Проверить, что сработало автозаполнение текста в SourceTerm в окне добавления термина
		/// </summary>
		public bool IsTextExistInSourceTerm(string text)
		{
			CustomTestContext.WriteLine("Проверить, что сработало автозаполнение текста {0} в SourceTerm", text);

			return Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TERM_VALUE.Replace("*#*", text)));
		}

		/// <summary>
		/// Проверить, что сработало автозаполнение текста в TargetTerm в окне добавления термина
		/// </summary>
		public bool IsTextExistInTargetTerm(string text)
		{
			CustomTestContext.WriteLine("Проверить, что сработало автозаполнение текста {0} в TargetTerm", text);

			return Driver.WaitUntilElementIsDisplay(By.XPath(TARGET_TERM_VALUE.Replace("*#*", text)));
		}

		/// <summary>
		/// Проверить наличие глоссария в выпадающем списке окна добавления термина
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public bool IsGlossaryExistInDropdown(string glossaryName)
		{
			CustomTestContext.WriteLine("Проверить наличие глоссария {0} в выпадающем списке.", glossaryName);
			var glossary = Driver.SetDynamicValue(How.XPath, GLOSSARY_SELECT_BOUNDLIST, glossaryName);

			return glossary.Displayed;
		}

		/// <summary>
		/// Проверить, открыт ли выпадающий список глоссария.
		/// </summary>
		public bool IsGlossaryListDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что открыт ли выпадающий список глоссария.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_LIST));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SOURCE_TERM)]
		protected IWebElement SourceTerm { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_TERM)]
		protected IWebElement TargetTerm { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddButton { get; set; }

		[FindsBy(How = How.XPath, Using = COMMENT_INPUT)]
		protected IWebElement CommentInput { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_SELECT)]
		protected IWebElement GlossarySelect { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		#endregion

		#region Описания XPath элементов страницы

		protected const string SOURCE_TERM = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm')]";
		protected const string SOURCE_TERM_VALUE = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm') and contains(@value, '*#*')]";
		protected const string TARGET_TERM_VALUE = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm') and contains(@value, '*#*')]";
		protected const string TARGET_TERM = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm')]";
		protected const string COMMENT_INPUT = "//div[contains(@id, 'term-window')]//textarea[contains(@name, 'comment')]";
		protected const string ADD_BUTTON = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Add')]";
		protected const string CANCEL_BUTTON = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Cancel')]";
		protected const string GLOSSARY_SELECT = "//input[@name='glossaryId']";
		protected const string GLOSSARY_LIST = "//ul[contains(@id, 'boundlist')]";
		protected const string GLOSSARY_SELECT_BOUNDLIST = "//ul[contains(@id, 'boundlist')]//li[contains(string(), '*#*')]";

		#endregion
	}
}
