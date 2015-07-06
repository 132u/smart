using System;
using System.Globalization;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_TABLE)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница с глоссариями.");
			}
		}

		/// <summary>
		/// Проверить, что глоссарий присутствует в списке
		/// </summary>
		public GlossariesPage AssertGlossaryExist(string glossaryName)
		{
			Logger.Trace("Проверить, что глоссарий {0} присутствует в списке.", glossaryName);
			var glossary = Driver.SetDynamicValue(How.XPath, GLOSSARY_ROW, glossaryName);

			Assert.IsTrue(glossary.Displayed, "Произошла ошибка:\n глоссарий {0} отсутствует в списке.", glossaryName);
			
			return GetPage();
		}

		///<summary>
		 ///Проверить, что глоссарий отсутствует в списке
		 ///</summary>
		public GlossariesPage AssertGlossaryNotExist(string glossaryName)
		{
			Logger.Trace("Проверить, что глоссарий {0} отсутствует в списке.", glossaryName);

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(GLOSSARY_ROW.Replace("*#*", glossaryName))),
				"Произошла ошибка:\n глоссарий {0} присутствует в списке.", glossaryName);

			return GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку создания глоссария
		/// </summary>
		public NewGlossaryDialog ClickCreateGlossaryButton()
		{
			Logger.Debug("Нажать кнопку создания глоссария.");
			CreateGlossaryButton.JavaScriptClick();

			return new NewGlossaryDialog().GetPage();
		}

		/// <summary>
		/// Нажать по имени глоссария
		/// </summary>
		public GlossaryPage ClickGlossaryRow(string glossaryName)
		{
			Logger.Debug("Нажать по имени глоссария {0}.", glossaryName);
			var glossaryRow = Driver.SetDynamicValue(How.XPath, GLOSSARY_ROW, glossaryName);

			glossaryRow.ScrollAndClick();

			return new GlossaryPage().GetPage();
		}

		/// <summary>
		/// Получить дату изменения глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>дата изменения</returns>
		public DateTime GlossaryDateModified(string glossaryName)
		{
			Logger.Trace("Получить дату изменения глоссария {0}.", glossaryName);

			return DateTime.ParseExact(Driver.SetDynamicValue(How.XPath, MODIFIED_DATE, glossaryName).Text,
				"M/d/yyyy h:mm tt",CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Получить имя автора глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>имя автор</returns>
		public string GetModifiedByAuthor(string glossaryName)
		{
			Logger.Trace("Получить имя автора глоссария.");
			var author = Driver.SetDynamicValue(How.XPath, AUTHOR, glossaryName);

			return author.Text;
		}

		[FindsBy(How = How.XPath, Using = CREATE_GLOSSARY_BUTTON)]
		protected IWebElement CreateGlossaryButton { get; set; }

		protected const string GLOSSARY_CREATION_DIALOG_XPATH = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string CREATE_GLOSSARY_BUTTON = ".//span[contains(@class,'js-create-glossary-button')]//a";
		protected const string GLOSSARY_TABLE = "//table[contains(@class,'js-sortable-table') and contains(@data-sort-action, 'Glossaries')]";
		protected const string GLOSSARY_ROW = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text()='*#*']";
		protected const string MODIFIED_DATE = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '*#*']/../../td[8]";
		protected const string AUTHOR = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '*#*']/../../td[9]/p";
	}
}
