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

		public void WaitPageLoad()
		{
			Logger.Trace("Ожидание загрузки страницы domain");

			Assert.IsTrue(
				WaitUntilDisplayElement(By.XPath(ADD_DOMAIN_BTN_XPATH)),
				"Ошибка: страница domain не загрузилась");
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

		public void ClickCancelDomain()
		{
			Logger.Debug("Кликаем кнопку Cancel");
			ClickElement(By.XPath(CANCEL_DOMAIN_XPATH));
		}

		public void WaitUntilSave()
		{
			Logger.Trace("Дождаться сохранения (пропадает кнопка Save)");
			
			Assert.IsTrue(
				WaitUntilDisappearElement(By.XPath(SAVE_DOMAIN_XPATH)),
				"Ошибка: не пропала кнопка Save после сохранения домена");
		}

		public void AssertionIsEditMode()
		{
			Logger.Trace("Проверка, что мы находимся в режиме редактирования");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(ENTER_NAME_XPATH)),
				"Ошибка: после попытки изменения имени на недопустимое мы вышли из режима редактирования (должны остаться).");
		}

		public void AssertionIsNewDomainEditMode()
		{
			Logger.Trace("Проверка, что домен не сохранился, а остался в режиме редактирования");

			Assert.IsTrue(GetIsElementDisplay(By.XPath(NEW_DOMAIN_TD_XPATH)),
				"Ошибка: после попытки создания домена с недопустимым именем мы вышли из режима редактирования (должны остаться).");
		}

		public void ClickDeleteDomain(string domainName)
		{
			Logger.Debug(string.Format("Удаление домена {0}...", domainName));

			var domainXPath = GetDomainRowXPath(domainName);

			Logger.Debug(string.Format("Кликнуть по строке с доменом {0}", domainName));
			ClickElement(By.XPath(domainXPath));

			var deleteXPath = domainXPath + DELETE_DOMAIN_BTN_XPATH;

			Logger.Trace("Дождаться появления кнопки Delete");
			WaitUntilDisplayElement(By.XPath(deleteXPath));
			
			Logger.Debug("Кликнуть кнопку Delete");
			ClickElement(By.XPath(deleteXPath));

			Assert.IsTrue(WaitUntilDisappearElement(By.XPath(deleteXPath)),
				"Кнопка DELETE не пропала после удаления домена");
			
		}

		public void ClickEditDomainBtn(string domainName)
		{
			Logger.Debug(string.Format("Редактирование домена {0}...", domainName));

			var domainXPath = GetDomainRowXPath(domainName);

			Logger.Debug(string.Format("Кликнуть по строке с доменом {0}", domainName));
			ClickElement(By.XPath(domainXPath));

			var editXPath = domainXPath + EDIT_DOMAIN_BTN_XPATH;

			Logger.Trace("Дождаться появления кнопки Edit");
			WaitUntilDisplayElement(By.XPath(editXPath));

			Logger.Debug("Нажать кнопку Edit");
			ClickElement(By.XPath(editXPath));
		}

		public void EnterNewName(string domainName, string newName)
		{
			Logger.Debug(string.Format("Ввести новое имя домена. Старое имя: {0}, новое имя: {1}", domainName, newName));
			ClearAndAddText(By.XPath(ENTER_NAME_XPATH), newName);
		}

		public void AssertionIsNameErrorExist()
		{
			Logger.Trace("Проверить, появилась ли ошибка существующего имени");

			Assert.IsTrue(
				WaitUntilDisplayElement(By.XPath(ERROR_NAME_XPATH)),
				"Ошибка: не появилась ошибка существующего имени");
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