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
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public DomainPageHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// <summary>
		/// Дождаться загрузки страницы
		/// </summary>
		/// <returns>загрузилась</returns>
		public bool WaitPageLoad()
		{
			return WaitUntilDisplayElement(By.XPath(ADD_DOMAIN_BTN_XPATH));
		}

		/// <summary>
		/// Получить, есть ли такой domain
		/// </summary>
		/// <param name="domainName">название</param>
		/// <returns>есть</returns>
		public bool GetIsDomainExist(string domainName)
		{
			/*// Получить список всех проектов
			IList<IWebElement> projectsList = Driver.FindElements(By.XPath(DOMAIN_LIST_XPATH));
			bool bProjectExist = false;
			foreach (IWebElement el in projectsList)
			{
				// Проверить имя проекта
				if (el.Text == domainName)
				{
					bProjectExist = true;
					break;
				}
			}

			return bProjectExist;*/
			return GetIsElementDisplay(By.XPath(GetDomainRowXPath(domainName)));
		}

		/// <summary>
		/// Нажать создать domain
		/// </summary>
		public void ClickCreateDomainBtn()
		{
			ClickElement(By.XPath(ADD_DOMAIN_BTN_XPATH));
		}

		/// <summary>
		/// Ввести название нового domain
		/// </summary>
		/// <param name="name">название</param>
		public void EnterNameCreateDomain(string name)
		{
			SendTextElement(By.XPath(NEW_DOMAIN_TD_XPATH), name);
		}

		/// <summary>
		/// Кликнуть Save
		/// </summary>
		public void ClickSaveDomain()
		{
			ClickElement(By.XPath(SAVE_DOMAIN_XPATH));
		}

		/// <summary>
		/// Кликнуть Cancel
		/// </summary>
		public void ClickCancelDomain()
		{
			ClickElement(By.XPath(CANCEL_DOMAIN_XPATH));
		}

		public void WaitUntilSave()
		{
			Logger.Trace("Дождаться сохранения (пропадает кнопка Save)");

			Assert.IsTrue(
					WaitUntilDisappearElement(By.XPath(SAVE_DOMAIN_XPATH)),
					"Ошибка: не пропала кнопка Save");
		}

		/// <summary>
		/// Вернуть, остался ли режим редактирования
		/// </summary>
		/// <returns>режим редактирования</returns>
		public bool GetIsEditMode()
		{
			return GetIsElementDisplay(By.XPath(ENTER_NAME_XPATH));
		}

		/// <summary>
		/// Вернуть, не сохранился новый домен
		/// </summary>
		/// <returns>режим нового домена (не сохранился)</returns>
		public bool GetIsNewDomainEditMode()
		{
			return GetIsElementDisplay(By.XPath(NEW_DOMAIN_TD_XPATH));
		}

		/// <summary>
		/// Проявить кнопку Delete и кликнуть
		/// </summary>
		/// <param name="domainName">название</param>
		/// /// <returns>пропала кнопка</returns>
		public bool ClickDeleteDomain(string domainName)
		{
			var domainXPath = GetDomainRowXPath(domainName);

			// Кликнуть по строке
			ClickElement(By.XPath(domainXPath));
			var deleteXPath = domainXPath + DELETE_DOMAIN_BTN_XPATH;
			
			// Дождаться появления Delete
			WaitUntilDisplayElement(By.XPath(deleteXPath));
			
			// Кликнуть Delete
			ClickElement(By.XPath(deleteXPath));

			return WaitUntilDisappearElement(By.XPath(deleteXPath));
		}

		/// <summary>
		/// Проявить кнопку Edit и кликнуть
		/// </summary>
		/// <param name="domainName">название</param>
		public void ClickEditDomainBtn(string domainName)
		{
			var domainXPath = GetDomainRowXPath(domainName);

			// Кликнуть по строке
			ClickElement(By.XPath(domainXPath));
			var editXPath = domainXPath + EDIT_DOMAIN_BTN_XPATH;
			
			// Дождаться появления Edit
			WaitUntilDisplayElement(By.XPath(editXPath));
			
			// Кликнуть Edit
			ClickElement(By.XPath(editXPath));
		}

		/// <summary>
		/// Ввести новое имя
		/// </summary>
		/// <param name="domainName">название домена</param>
		/// <param name="newName">новое имя</param>
		public void EnterNewName(string domainName, string newName)
		{
			ClearAndAddText(
				By.XPath(ENTER_NAME_XPATH),
				newName);
		}

		/// <summary>
		/// Вернуть, отображается ли строка с ошибкой
		/// </summary>
		/// <returns>отображается</returns>
		public bool GetIsNameErrorExist()
		{
			return WaitUntilDisplayElement(By.XPath(ERROR_NAME_XPATH));
		}

		/// <summary>
		/// Вернуть xPath строки с доменом
		/// </summary>
		/// <param name="domainName">название</param>
		/// <returns>XPath</returns>
		protected string GetDomainRowXPath(string domainName)
		{
			var rowNum = 0;
			var domainList = GetElementList(By.XPath(DOMAIN_LIST_XPATH));
			
			for (int i = 0; i < domainList.Count; ++i)
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