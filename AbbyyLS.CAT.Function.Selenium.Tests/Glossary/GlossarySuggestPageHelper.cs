using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы предложенных терминов глоссария
	/// </summary>
	public class GlossarySuggestPageHelper : CommonHelper
	{
		public GlossarySuggestPageHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
			buttonsDict = new Dictionary<BUTTON_ID, string>
			{
				{BUTTON_ID.AcceptSuggestTerm, BUTTON_ACCEPT_XPATH},
				{BUTTON_ID.EditSuggestTerm, BUTTON_EDIT_XPATH},
				{BUTTON_ID.RejectSuggestTerm, BUTTON_REJECT_XPATH}
			};
		}

		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(SUGGEST_TABLE_XPATH));
		}

		public void ClickSaveTerm()
		{
			Logger.Debug("Кликнуть кнопку добавления термина");
			ClickElement(By.XPath(SAVE_EDIT_TERM_XPATH));
		}

		/// <summary>
		/// Дождаться окончания сохранения термина.
		/// </summary>
		public void WaitSaveTerm()
		{
			WaitUntilDisappearElement(By.XPath(SAVE_EDIT_TERM_XPATH));
		}

		public enum BUTTON_ID { AcceptSuggestTerm, EditSuggestTerm, RejectSuggestTerm };

		protected Dictionary<BUTTON_ID, string> buttonsDict;
		protected const string BUTTON_ACCEPT_XPATH = "js-accept-suggest";
		protected const string BUTTON_EDIT_XPATH = "js-edit-suggest";
		protected const string BUTTON_REJECT_XPATH = "js-reject-suggest";

		protected const string SUGGEST_TABLE_XPATH = "//table[contains(@class,'js-suggests')]";
		protected const string CURRENT_GLOSSARY_REF_XPATH = "//a[contains(@href,'/Enterprise/Concepts')]";
		protected const string TERM_ROW_XPATH = "//tr[contains(@class,'js-suggest-row') and not(contains(@class,'g-hidden'))]";
		protected const string ROW_GLOSSARY_NAME_XPATH = TERM_ROW_XPATH + "//td[contains(@class, 'js-glossary-cell')]//p";

		protected const string CHOOSE_GLOSSARY_FORM_XPATH = "//div[contains(@class,'js-select-glossary-popup')]";
		protected const string DROPDOWN_XPATH = "//span[contains(@class,'js-dropdown')]";
		protected const string CLICK_GLOSSARY_LIST_DROPDOWN = CHOOSE_GLOSSARY_FORM_XPATH + DROPDOWN_XPATH;
		protected const string DROPDOWN_ITEM = "//span[contains(@class,'js-dropdown__item')]";
		protected const string CHOOSE_GLOSSARY_OK_BTN_XPATH = "//input[contains(@class, 'js-glossary-selected-button')]";

		protected const string EDIT_TERM_BOX = "//div[contains(@class,'l-corprtree__langbox')]";
		protected const string EDITOR_INPUT_XPATH = "//span[contains(@class,'js-term-editor')]//input";
		protected const string VIEWER_XPATH = "//span[contains(@class,'js-term-viewer')]";
		protected const string SAVE_EDIT_TERM_XPATH = "//a[contains(@class, 'js-save-btn')]";
		protected const string ADD_SYNONYM_XPATH = "//span[contains(@class,'js-add-term')]";
		protected const string NEW_TERM_ADDED_XPATH = "//div[@class ='l-corprtree']/div[1]/div[3]/div[3]/span/span[contains(@class,'js-text') and text()='";
		protected const string ACCEPT_TERM_XPATH = "//span[@class = 'js-save-text']";
	}
}