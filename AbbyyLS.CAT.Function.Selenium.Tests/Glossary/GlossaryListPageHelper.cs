using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Хелпер страницы списка глоссариев
	/// </summary>
	public class GlossaryListPageHelper : CommonHelper
	{
		public GlossaryListPageHelper(IWebDriver driver, WebDriverWait wait)
			: base(driver, wait)
		{
		}

		public void WaitPageLoad()
		{
			Logger.Trace("Проверка успешной загрузки страницы глоссария");

			Assert.IsTrue(WaitUntilDisplayElement(By.XPath(CREATE_GLOSSARY_BTN_XPATH)), 
				"Ошибка: не удалось перейти на страницу глоссария");
		}

		public void ClickCreateGlossary()
		{
			Logger.Debug("Нажать кнопку создания глоссария");
			ClickElement(By.XPath(CREATE_GLOSSARY_BTN_XPATH));
		}

		public bool GetIsAddSuggestExist()
		{
			Logger.Debug("Получить, есть ли кнопка предложения термина");

			SetDriverTimeoutMinimum();
			var isExist = GetIsElementDisplay(By.XPath(ADD_SUGGEST_BTN_XPATH));
			SetDriverTimeoutDefault();

			return isExist;
		}

		public void ClickAddSuggest()
		{
			Logger.Debug("Кликнуть кнопку добавления предложенного термина");
			ClickElement(By.XPath(ADD_SUGGEST_BTN_XPATH));
		}

		public bool GetIsExistGlossary(string glossaryName)
		{
			Logger.Trace(string.Format("Получить существование глоссария {0}", glossaryName));

			return WaitUntilDisplayElement(By.XPath(GetGlossaryRowXPath(glossaryName)));
		}

		public void ClickGlossaryRow(string glossaryName)
		{
			Logger.Trace(string.Format("Кликнуть по глоссарию с именем {0}", glossaryName));

			var pathToGlossaryName = GetGlossaryRowXPath(glossaryName);

			try
			{
				ClickElement(By.XPath(pathToGlossaryName));
			}
			catch (Exception e)
			{
				var errorMessage = string.Format(
					"Невозможно кликнуть по глоссарию с именем {0}. Текст сообщения: {1}.", glossaryName, e);
				Logger.Error(errorMessage);

				throw new Exception(errorMessage);
			}
		}

		/// <summary>
		/// Метод скроллит до того момента,чтобы глоссарий стал видимым
		/// </summary>
		/// <param name="glossaryName"></param>
		public void ScrollToGlossary(string glossaryName)
		{
			try
			{
				Logger.Trace(string.Format("Скроллим, чтобы глоссарий {0} стал видимым", glossaryName));
				var glossaryItem = Driver.FindElement(By.XPath(GetGlossaryRowXPath(glossaryName)));
				((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true); window.scrollBy(0,-200);", glossaryItem);
			}
			catch (Exception ex)
			{
				Assert.Fail("При попытке скроллинга страницы произошла ошибка: " + ex.Message);
			}
		}

		/// <summary>
		/// Получить автора глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>автор</returns>
		public string GetGlossaryModificationAuthor(string glossaryName)
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
