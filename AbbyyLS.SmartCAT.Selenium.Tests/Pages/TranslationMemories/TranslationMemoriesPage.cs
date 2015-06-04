using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

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
		/// Нажать кнопку создания новой ТМ"
		/// </summary>
		public NewTranslationMemoryDialog ClickCreateNewTmButton()
		{
			Logger.Debug("Нажать кнопку создания новой ТМ");
			CreateNewTmButton.AdvancedClick();
			
			return new NewTranslationMemoryDialog().GetPage();
		}

		/// <summary>
		/// Проверить, что ТМ представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryExist(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} представлена в списке.", tmName);
			var translationMemoryRowPath = TM_ROW.Replace("*#*", tmName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(translationMemoryRowPath)),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ.", tmName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что ТМ не представлена в списке
		/// </summary>
		public TranslationMemoriesPage AssertTranslationMemoryNotExist(string tmName)
		{
			Logger.Trace("Проверить, что ТМ {0} не представлена в списке.", tmName);
			var translationMemoryRowPath = TM_ROW.Replace("*#*", tmName);

			Assert.IsFalse(Driver.ElementIsDisplayed(By.XPath(translationMemoryRowPath)),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ.", tmName);

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = ADD_TM_BTN)]
		protected IWebElement CreateNewTmButton { get; set; }

		protected const string CREATE_TM_DIALOG = "//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string ADD_TM_BTN = "//span[contains(@data-bind,'createTm')]";
		protected const string TM_TABLE_BODY = "//table[contains(@class,'js-sortable-table')]";
		protected const string TM_ROW = "//tr[@class='l-corpr__trhover clickable']//span[text()='*#*']";
	}
}
