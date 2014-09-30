using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы списка глоссариев
	/// </summary>
	public class GlossaryListPageHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public GlossaryListPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(CREATE_GLOSSARY_BTN_XPATH));
		}

		/// <summary>
		/// Кликнуть Создать глоссарий
		/// </summary>
		public void ClickCreateGlossary()
		{
			ClickElement(By.XPath(CREATE_GLOSSARY_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли кнопка предложения термина
		/// </summary>
		/// <returns>есть</returns>
		public bool GetIsAddSuggestExist()
		{
			SetDriverTimeoutMinimum();
			bool isExist = GetIsElementDisplay(By.XPath(ADD_SUGGEST_BTN_XPATH));
			SetDriverTimeoutDefault();

			return isExist;
		}

		/// <summary>
		/// Кликнуть Добавить предложенный термин
		/// </summary>
		public void ClickAddSuggest()
		{
			ClickElement(By.XPath(ADD_SUGGEST_BTN_XPATH));
		}

		/// <summary>
		/// Вернуть, есть ли глоссарий с таким именем
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>есть</returns>
		public bool GetIsExistGlossary(string glossaryName)
		{
			
			//bool isExist = GetIsElementExist(By.XPath(GetGlossaryRowXPath(glossaryName)));
		   // SetDriverTimeoutDefault();
			return WaitUntilDisplayElement(By.XPath(GetGlossaryRowXPath(glossaryName)));
		}

		/// <summary>
		/// Кликнуть по строке глоссария
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public void ClickGlossaryRow(string glossaryName)
		{
			ClickElement(By.XPath(GetGlossaryRowXPath(glossaryName)));
		}

		/// <summary>
		/// Получить автора глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>автор</returns>
		public string GetGlossaryAuthor(string glossaryName)
		{
			return GetTextElement(By.XPath(GLOSSARY_NAME_XPATH + "[text() = '" + glossaryName + "']/../../td[9]/p"));
		}

		/// <summary>
		/// Получить дату изменения глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>дата изменения</returns>
		public string GetGlossaryDateModified(string glossaryName)
		{
			return GetTextElement(By.XPath(GLOSSARY_NAME_XPATH + "[text() = '" + glossaryName + "']/../../td[8]"));
		}

		/// <summary>
		/// Получить xPath глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>xPath</returns>
		protected string GetGlossaryRowXPath(string glossaryName)
		{
			return GLOSSARY_NAME_XPATH + "[text()='" + glossaryName + "']";
		}



		protected const string CREATE_GLOSSARY_BTN_XPATH = ".//span[contains(@class,'js-create-glossary-button')]//a";
		protected const string ADD_SUGGEST_BTN_XPATH = "//span[contains(@class,'js-add-suggest')]";
		protected const string GLOSSARY_NAME_XPATH = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p";	   
	}
}
