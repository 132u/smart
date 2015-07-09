using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class TranslationMemoriesPage : WorkspacePage, IAbstractPage<TranslationMemoriesPage>
	{
		public new TranslationMemoriesPage GetPage()
		{
			var translationMemoriesPage = new TranslationMemoriesPage();
			InitPage(translationMemoriesPage);

			return translationMemoriesPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ADD_TM_BTN)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с памятью переводов.");
			}
		}

		/// <summary>
		/// Нажать кнопку создания новой ТМ
		/// </summary>
		public NewTranslationMemoryDialog ClickCreateNewTmButton()
		{
			Logger.Debug("Нажать кнопку создания новой ТМ");
			CreateNewTmButton.JavaScriptClick();

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_TM_BUTTON), timeout: 20))
			{
				CreateNewTmButton.JavaScriptClick();
			}

			return new NewTranslationMemoryDialog().GetPage();
		}

		/// <summary>
		/// Проверить, что ТМ представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryExist(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} представлена в списке.", tmName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TM_ROW.Replace("*#*", tmName))),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ.",tmName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что ТМ представлена в списке
		/// </summary>
		public bool TranslationMemoryExists(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} представлена в списке.", tmName);
			var translationMemoryRowPath = TM_ROW.Replace("*#*", tmName);

			return Driver.WaitUntilElementIsDisplay(By.XPath(translationMemoryRowPath));

		}

		/// <summary>
		/// Проверить, что ТМ не представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryNotExist(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} не представлена в списке.", tmName);

			Assert.IsFalse(Driver.ElementIsDisplayed(By.XPath(TM_ROW.Replace("*#*", tmName))),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ.",tmName);

			return GetPage();
		}

		/// <summary>
		/// Ввести название ТМ в поле поиска
		/// </summary>
		public TranslationMemoriesPage FillSearch(string translationMemoryName)
		{
			Logger.Debug("Ввести название ТМ {0} в поле поиска.", translationMemoryName);

			Search.SetText(translationMemoryName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку поиска
		/// </summary>
		public new TranslationMemoriesPage ClickSearchButton()
		{
			Logger.Debug("Нажать кнопку поиска.");

			SearchButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Delete
		/// </summary>
		public TranslationMemoriesPage ClickDeleteButtonInTMInfo()
		{
			Logger.Debug("Нажать кнопку Delete.");

			DeleteButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать по строке с ТМ
		/// </summary>
		public TranslationMemoriesPage ClickTranslationMemoryRow(string translationMemoryName)
		{
			var translationMemoryRow = TM_ROW.Replace("*#*", translationMemoryName);

			if (!translationMemoryInfoExpanded(translationMemoryName))
			{
				Logger.Debug("Нажать по строке с ТМ {0}.", translationMemoryName);
				Driver.FindElement(By.XPath(translationMemoryRow)).Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог подтверждения удаления ТМ появился
		/// </summary>
		public TranslationMemoriesPage AssertDeleteConfirmatonDialogPresent()
		{
			Logger.Trace("Проверить, что диалог подтверждения удаления ТМ появился.");
			Driver.WaitUntilElementIsDisplay(By.XPath(DELETE_CONFIRMATION_DIALOG));

			Assert.IsTrue(DeleteConfirmationDialog.Displayed,
				"Произошла ошибка:\n диалог подтверждения удаления ТМ не появился.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог подтверждения удаления ТМ закрылся
		/// </summary>
		public TranslationMemoriesPage AssertDeleteConfirmatonDialogDisappear()
		{
			Logger.Trace("Проверить, что диалог подтверждения удаления ТМ закрылся.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(DELETE_CONFIRMATION_DIALOG)),
				"Произошла ошибка:\n диалог подтверждения удаления ТМ не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Delete в диалоге подтверждения удаления ТМ
		/// </summary>
		public TranslationMemoriesPage ClickDeleteButtonInConfirmationDialog()
		{
			Logger.Debug("Нажать кнопку Delete в диалоге подтверждения удаления ТМ.");

			DeleteButtonInConfirmationDialog.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог создания ТМ закрылся.
		/// </summary>
		public TranslationMemoriesPage AssertNewTranslationMemoryDialogDisappear()
		{
			Logger.Trace("Проверить, что диалог создания ТМ закрылся.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(CREATE_TM_DIALOG)),
				"Произошла ошибка:\n диалог создания ТМ не закрылся.");

			return GetPage();
		}

		/// <summary>
		/// Вернуть, открыта ли информация о ТМ
		/// </summary>
		private bool translationMemoryInfoExpanded(string tmName)
		{
			Logger.Trace("Вернуть, открыта ли информация о ТМ {0}", tmName);
			var translationMemoryRow = TM_ROW.Replace("*#*", tmName);

			return Driver.FindElement(By.XPath(translationMemoryRow)).GetElementAttribute("class").Contains("opened");
		}

		/// <summary>
		/// Нажать кнопку сортировки по именам
		/// </summary>
		public TranslationMemoriesPage ClickSortByTMName()
		{
			Logger.Debug("Нажать кнопку сортировки по именам");
			TMName.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по языкам
		/// </summary>
		public TranslationMemoriesPage ClickSortByLanguages()
		{
			Logger.Debug("Нажать кнопку сортировки по языкам");
			Languages.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по авторам
		/// </summary>
		public TranslationMemoriesPage ClickSortByAuthor()
		{
			Logger.Debug("Нажать кнопку сортировки по авторам");
			Author.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате создания
		/// </summary>
		public TranslationMemoriesPage ClickSortByCreationDate()
		{
			Logger.Debug("Нажать кнопку сортировки по дате создания.");
			CreationDate.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = TM_NAME)]
		protected IWebElement TMName { get; set; }

		[FindsBy(How = How.XPath, Using = LANGUAGES)]
		protected IWebElement Languages { get; set; }

		[FindsBy(How = How.XPath, Using = AUTHOR)]
		protected IWebElement Author { get; set; }

		[FindsBy(How = How.XPath, Using = CREATION_DATE)]
		protected IWebElement CreationDate { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_TM_BTN)]
		protected IWebElement CreateNewTmButton { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH)]
		protected IWebElement Search { get; set; }

		[FindsBy(How = How.XPath, Using = SEARCH_BUTTON)]
		protected IWebElement SearchButton { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_BUTTON)]
		protected IWebElement DeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_CONFIRMATION_DIALOG)]
		protected IWebElement DeleteConfirmationDialog { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_BUTTON_IN_CONFIRMATION_DIALOG)]
		protected IWebElement DeleteButtonInConfirmationDialog { get; set; }

		protected const string CREATE_TM_DIALOG = "//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string ADD_TM_BTN = "//span[contains(@data-bind,'createTm')]//a";
		protected const string TM_TABLE_BODY = "//table[contains(@class,'js-sortable-table')]";
		protected const string TM_ROW = "//tr[@class='l-corpr__trhover clickable']//span[text()='*#*']";
		protected const string SEARCH = "//input[contains(@class, 'search-tm')]";
		protected const string SEARCH_BUTTON = "//a[contains(@class, 'search-by-name')]";
		protected const string DELETE_BUTTON = "//tr[@class='js-tm-panel']//span[contains(@data-bind, 'deleteTranslationMemory')]";
		protected const string DELETE_CONFIRMATION_DIALOG = "//form[contains(@action,'Delete')]";
		protected const string DELETE_BUTTON_IN_CONFIRMATION_DIALOG = "//form[contains(@action,'Delete')]//input[@value='Delete']";
		protected const string SAVE_TM_BUTTON = ".//div[contains(@class,'js-popup-create-tm')][2]//span[contains(@data-bind, 'click: save')]";

		protected const string TM_NAME = "(//th[contains(@data-sort-by,'Name')]//a)[1]";
		protected const string LANGUAGES = "//th[contains(@data-sort-by,'Languages')]//a";
		protected const string AUTHOR = "//th[contains(@data-sort-by,'CreatorName')]//a";
		protected const string CREATION_DATE = "//th[contains(@data-sort-by,'CreatedDate')]//a";
	}
}
