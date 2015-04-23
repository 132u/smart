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
			LoadPage();

			return translationMemoriesPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ADD_TM_BTN_XPATH)))
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
			CreateNewTmButton.Click();

			return new NewTranslationMemoryDialog().GetPage();
		}

		[FindsBy(How = How.XPath, Using = ADD_TM_BTN_XPATH)]
		protected IWebElement CreateNewTmButton { get; set; }

		protected const string CREATE_TM_DIALOG_XPATH = ".//div[contains(@class,'js-popup-create-tm')][2]";
		protected const string ADD_TM_BTN_XPATH = "//span[contains(@data-bind,'createTm')]";
		protected const string TM_TABLE_BODY_XPATH = "//table[contains(@class,'js-sortable-table')]";
	}
}
