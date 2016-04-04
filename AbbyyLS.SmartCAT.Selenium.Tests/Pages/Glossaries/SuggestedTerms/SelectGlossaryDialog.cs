using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms
{
	public class SelectGlossaryDialog : SuggestedTermsGlossariesPage, IAbstractPage<SelectGlossaryDialog>
	{
		public SelectGlossaryDialog(WebDriver driver) : base(driver)
		{
		}

		public new SelectGlossaryDialog LoadPage()
		{
			if (!IsSelectGlossaryDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\nНе открылся диалог выбора глоссария");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Нажать на выпадающий список глоссариев в диалоге выбора глоссария
		/// </summary>
		public SelectGlossaryDialog ClickSelectGlossaryDropdownInSelectDialog()
		{
			CustomTestContext.WriteLine("Нажать на выпадающий список глоссариев в диалоге выбора глоссария.");
			SelectGlossaryDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать глоссарий
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		public SelectGlossaryDialog SelectGlossaryInSelectDialog(string glossaryName)
		{
			CustomTestContext.WriteLine("Выбрать {0} глоссарий.", glossaryName);
			Driver.SetDynamicValue(How.XPath, GLOSSARY_IN_SELECT_GLOSSARY_DIALOG, glossaryName).Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Ok
		/// </summary>
		public SuggestedTermsGlossariesPage ClickOkButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Ok.");
			OkButton.Click();

			return new SuggestedTermsGlossariesPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Выбрать глоссарий для предложенного термина
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public SuggestedTermsGlossariesPage SelectGlossaryForSuggestedTerm(string glossaryName)
		{
			ClickSelectGlossaryDropdownInSelectDialog();
			SelectGlossaryInSelectDialog(glossaryName);
			var suggestedTermsPageForAllGlossaries = ClickOkButton();

			return suggestedTermsPageForAllGlossaries.LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли диалог создания терминов
		/// </summary>
		public bool IsSelectGlossaryDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SELECT_GLOSSARY_DROPDOWN));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SELECT_GLOSSARY_DROPDOWN)]
		protected IWebElement SelectGlossaryDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = OK_BUTTON)]
		protected IWebElement OkButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string SELECT_GLOSSARY_DROPDOWN = "//div[contains(@class,'js-select-glossary-popup')]//span[contains(@class,'js-dropdown')]";
		protected const string GLOSSARY_IN_SELECT_GLOSSARY_DIALOG = "//span[contains(@class,'js-dropdown__item')][contains(text(),'*#*')]";
		protected const string OK_BUTTON = "//input[contains(@class, 'js-glossary-selected-button')]";

		#endregion
	}
}
