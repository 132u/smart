using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.Domains
{
	/// <summary>
	/// Хелпер вкладки домены
	/// </summary>
	public class DomainPageHelper : CommonHelper
	{
		public DomainPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		public bool GetIsDomainExist(string domainName)
		{
			Logger.Trace(string.Format("Определение существования домена {0}", domainName));
			return GetIsElementDisplay(By.XPath(GetDomainRowXPath(domainName)));
		}

		public void ClickCreateDomainBtn()
		{
			Logger.Debug("Нажать кнопку создать domain");
			ClickElement(By.XPath(ADD_DOMAIN_BTN_XPATH));
		}

		public void EnterNameCreateDomain(string name)
		{
			Logger.Debug(string.Format("Вводим имя нового домена {0}", name));
			SendTextElement(By.XPath(NEW_DOMAIN_TD_XPATH), name);
		}

		public void ClickSaveDomain()
		{
			Logger.Debug("Кликаем кнопку Save");
			ClickElement(By.XPath(SAVE_DOMAIN_XPATH));
		}

		public void WaitUntilSave()
		{
			Logger.Trace("Дождаться сохранения (пропадает кнопка Save)");
			
			Assert.IsTrue(
				WaitUntilDisappearElement(By.XPath(SAVE_DOMAIN_XPATH)),
				"Ошибка: не пропала кнопка Save после сохранения домена");
		}

		protected string GetDomainRowXPath(string domainName)
		{
			Logger.Trace(string.Format("Получить xPath строки с доменом {0}.", domainName));

			var rowNum = 0;
			var domainList = GetElementList(By.XPath(DOMAIN_LIST_XPATH));
			
			for (var i = 0; i < domainList.Count; ++i)
			{
				if (domainList[i].Text.Contains(domainName))
				{
					Logger.Trace(domainList[i].Text);
					Logger.Trace(i);
					rowNum = i + 1;
					break;
				}
			}

			return NOT_HIDDEN_TR + "[contains(@class,'js-row')][" + rowNum + "]";
		}

		protected const string ADD_DOMAIN_BTN_XPATH = ".//span[contains(@class,'js-add-domain')]";
		protected const string SAVE_DOMAIN_BTN_XPATH = "//a[contains(@class,'js-save-domain')]";
		protected const string CANCEL_DOMAIN_BTN_XPATH = "//a[contains(@class,'js-revert-domain')]";
		protected const string DELETE_DOMAIN_BTN_XPATH = "//a[contains(@class,'js-delete-domain')]";
		protected const string EDIT_DOMAIN_BTN_XPATH = "//a[contains(@class,'js-edit-domain')]";

		protected const string NEW_DOMAIN_TD_XPATH = NOT_HIDDEN_TR + "//td[contains(@class,'domainNew')]//div[contains(@class,'" + EDIT_DOMAIN_CLASS + "')]" + NAME_INPUT_XPATH;
		protected const string SAVE_DOMAIN_XPATH = NOT_HIDDEN_TR + "//div[contains(@class,'" + EDIT_DOMAIN_CLASS + "') and not(contains(@class,'g-hidden'))]" + SAVE_DOMAIN_BTN_XPATH;
		protected const string CANCEL_DOMAIN_XPATH = NOT_HIDDEN_TR + "//tr[not(contains(@class,'g-hidden'))]//div[contains(@class,'" + EDIT_DOMAIN_CLASS + "') and not(contains(@class,'g-hidden'))]//a[contains(@class,'js-revert-domain')]" + CANCEL_DOMAIN_BTN_XPATH;

		protected const string EDIT_DOMAIN_CLASS = "js-edit-mode";

		protected const string NAME_INPUT_XPATH = "//input[contains(@class,'js-domain-name-input')]";
		protected const string ENTER_NAME_XPATH = "//td[contains(@class,'domainEdit')]//div[contains(@class,'" + EDIT_DOMAIN_CLASS + "')]" + NAME_INPUT_XPATH;

		protected const string ERROR_NAME_XPATH = "//div[contains(@class,'js-error-text g-hidden')]";

		protected const string DOMAIN_LIST_XPATH = ".//table[contains(@class,'js-domains')]//tr[contains(@class,'js-row') and not(contains(@class,'g-hidden'))]";
		protected const string NOT_HIDDEN_TR = "//tr[not(contains(@class,'g-hidden'))]";
	}
}