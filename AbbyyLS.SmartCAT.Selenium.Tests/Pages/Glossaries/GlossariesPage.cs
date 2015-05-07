using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossariesPage : WorkspacePage, IAbstractPage<GlossariesPage>
	{
		public new GlossariesPage GetPage()
		{
			var glossariesPage = new GlossariesPage();
			InitPage(glossariesPage);

			return glossariesPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_TABLE_BODY_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с глоссариями.");
			}
		}
		
		/// <summary>
		/// Нажать кнопку создания глоссария
		/// </summary>
		public NewGlossaryDialog ClickCreateGlossaryButton()
		{
			Logger.Debug("Нажать кнопку создания глоссария");
			CreateGlossaryButton.Click();

			return new NewGlossaryDialog().GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREATE_GLOSSARY_BTN_XPATH)]
		protected IWebElement CreateGlossaryButton { get; set; }

		protected const string GLOSSARY_CREATION_DIALOG_XPATH = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string CREATE_GLOSSARY_BTN_XPATH = ".//span[contains(@class,'js-create-glossary-button')]//a";
		protected const string GLOSSARY_TABLE_BODY_XPATH = "//table[contains(@class,'js-sortable-table')]";
	}
}
