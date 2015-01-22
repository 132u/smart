﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{


	public class MyAccountPageHelper: CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public MyAccountPageHelper(IWebDriver driver, WebDriverWait wait):
			base (driver, wait)
		{

		}

		/// <summary>
		/// Кликнуть Buy кнопку
		/// </summary>
		/// <param name="i"> Период на который покупается лицензия </param>
		public void ClickBuyBtn(int i)
		{
			ClickElement(By.XPath("//td[" + i + "]" +BUY_ONE_MONTH_BTN));
		}

		/// <summary>
		/// Выбрать кол-во лицензий
		/// </summary>
		/// <param name="i"> Кол-во лицензий</param>
		public void SelectLicenseNumber(string i)
		{
			ClickElement(By.XPath(ADD_LIC_COMBOBOX + "//option[text()='" + i + "']"));
		}

		/// <summary>
		/// Выбрать период для продления лицензии
		/// </summary>
		/// <param name="period"> </param>
		public void SelectExtendPeriod(string period)
		{
			ClickElement(By.XPath(EXTEND_PERIOD_IN_POP_UP + "//option[contains(text(),'" + period + "')]"));
		}

		/// <summary>
		/// Дождаться появления License Purchase окна
		/// </summary>
		public void WaitLicPurchasePopUp()
		{
			WaitUntilDisplayElement(By.XPath(LIC_PURCHASE_POP_UP));
		}

		/// <summary>
		/// Кликнуть Buy кнопку в License Purchase окне
		/// </summary>
		public void ClickBuyBtnInLicPopUp()
		{
			ClickElement(By.XPath(BUY_BTN_IN_LIC_POP_UP));
		}

		/// <summary>
		/// Кликнуть Pay кнопку в License Purchase окне
		/// </summary>
		public void ClickPayBtnInLicPopUp()
		{
			ClickElement(By.XPath(PAY_BTN_IN_LIC_POP_UP));
		}

		/// <summary>
		/// Кликнуть Cancel кнопку в License Purchase окне
		/// </summary>
		public void ClickCancelBtnInLicPopUp()
		{
			ClickElement(By.XPath(CANCEL_BTN_IN_LIC_POP_UP));
		}

		/// <summary>
		/// Ввести номер карты
		/// </summary>
		public void EnterCardNumber(string cardNumber)
		{
			WaitUntilDisplayElement(By.XPath(CREDIT_CARD_NUMBER));
			SendTextElement(By.XPath(CREDIT_CARD_NUMBER), cardNumber);
		}

		/// <summary>
		/// Перейти в iframe платежной системы
		/// </summary>
		public void SwitchToPaymentFrame()
		{
			SwitchToFrame(By.XPath(PAYMENT_IFRAME));
		}

		/// <summary>
		/// Выйти из iframe платежной системы
		/// </summary>
		public void SwitchToDefaultContentFromPaymentIFrame()
		{
			SwitchToDefaultContent();
		}

		/// <summary>
		/// Ввести CVV карты
		/// </summary>
		/// <param name="cvv"> Cvv </param>
		public void EnterCvv(string cvv)
		{
			WaitUntilDisplayElement(By.XPath(CVV));
			SendTextElement(By.XPath(CVV), cvv);
		}

		/// <summary>
		/// Выбрать дату окончания срока действия карты
		/// </summary>
		/// <param name="date"> Дата окончания срока действия </param>
		public void EnterExpDate(string date)
		{
			WaitUntilDisplayElement(By.XPath(EXPIRATION_DATE));
			SendTextElement(By.XPath(EXPIRATION_DATE), date);
		}

		/// <summary>
		/// Получить кол-во всех кнопок Upgrade(видимых и невидимых)
		/// </summary>
		/// <returns> Список кнопок Upgrade </returns>
		public IList<IWebElement> GetUpgradeButtons()
		{
			return GetElementList(By.XPath(UPGRADE_BTNS));
		}

		/// <summary>
		/// Получить номер строки с немаксимальным кол-ом лицензий в пакете
		/// </summary>
		/// <returns> Номер строки </returns>
		public int GetRowNumberPackageInTable()
		{
			IList<IWebElement> tdList = GetElementList(By.XPath(LIC_TITLE));
			List<string> titles = new List<string>();
			int number = -1;
			foreach (IWebElement td in tdList)
			{
				titles.Add(td.Text);
			}

			for (int t = 0; t < titles.Count;t++ )
			{
				if (titles[t].EndsWith(" licenses"))
				{
					titles[t] = titles[t].Replace(" licenses", "");
				}
				if(titles[t].EndsWith(" лицензий"))
				{
					titles[t] = titles[t].Replace(" лицензий", "");
				}
				int n = ParseNumberStrToInt(titles[t]);
				if (n < 30)
				{
					return number = t;
					break;
				}
			}
			return number;
		}

		/// <summary>
		/// Получить номер строки пакета для продления
		/// </summary>
		/// <param name="titleLicense"> Текст из 1й колонки таблицы пакетов</param>
		/// <param name="date"> Expire date </param>
		/// <returns> Номер строки</returns>
		public int GetRowNumberOfLicense(string titleLicense, string date = "")
		{
			IList<IWebElement> titleList = GetElementList(By.XPath(LIC_TITLE));
			IList<IWebElement> dateList = GetElementList(By.XPath(EXPIRE__DATE_COLUMN));

			List<string> rows = new List<string>();
			int number = -1;
			for (int i = 0; i < titleList.Count; i++)
			{
					string text = titleList[i].Text + " = " + dateList[i].Text;
					rows.Add(text);
			}

			for (int i = 0; i < rows.Count; i++)
			{
				if (rows[i].Contains(titleLicense) && rows[i].Contains(date))
				{
					return number = i;
					break;
				}
					
			}
			return number;
		}
		/// <summary>
		/// Получить список видимых Upgrade кнопкок
		/// </summary>
		/// <returns> Cписок видимых Upgrade кнопкок </returns>
		public IList<IWebElement> GetVisibleUpgradeButtons()
		{
			IList<IWebElement> allUpgradeBtns = GetUpgradeButtons();
			IList<IWebElement> vilibleUpgradeBtns = new List<IWebElement>();

			foreach (IWebElement btn in allUpgradeBtns)
			{
				if (btn.Displayed)
					vilibleUpgradeBtns.Add(btn);
			}

			return vilibleUpgradeBtns;
		}

		/// <summary>
		/// Посчитать кол-во видимых Upgrade кнопок
		/// </summary>
		/// <returns> Кол-во Upgrade кнопок </returns>
		public int UpgradeBtnCount()
		{
			return GetVisibleUpgradeButtons().Count;
		}

		/// <summary>
		/// Кликнуть Upgrade кнопку
		/// </summary>
		public void ClickUpgradeBtn(int i = 0)
		{
			IList<IWebElement> buttons = GetVisibleUpgradeButtons();
			buttons[i].Click();

			// Проверить, что Upgrade окно открылось
			Assert.IsTrue(GetUpgradePopUpDisplayed(), "Ошибка: Upgrade окно не появилось");

		}

		/// <summary>
		/// Кликнуть Extend кнопку в таблице с купленными лизензиями
		/// </summary>
		/// <param name="i"> Номер строки </param>
		public void ClickExtendBtn(int i)
		{
			ClickElement(By.XPath(ROW_IN_TABLE + i + "]" + EXTEND_BTN));
			GetExtendPopUpDisplayed();
		}

		/// <summary>
		/// Кликнуть Close кнопку в окне благодарности за покупку лицензии
		/// </summary>
		public void ClickCloseBtn()
		{
			ClickElement(By.XPath(CLOSE_BTN));
		}

		/// <summary>
		/// Вернуть, показано ли окно с сообщением после покупки лицензии
		/// </summary>
		public bool GetMsgPopUpDisplayed()
		{
			return GetIsElementDisplay(By.XPath(MSG_POP_UP));
		}

		/// <summary>
		/// Вернуть, показано ли Upgrade pop up
		/// </summary>
		public bool GetUpgradePopUpDisplayed()
		{
			WaitUntilDisplayElement(By.XPath(UPGRADE_HEADER_POP_UP));
			return GetIsElementDisplay(By.XPath(UPGRADE_HEADER_POP_UP));
		}

		/// <summary>
		/// Вернуть, показано ли Upgrade pop up
		/// </summary>
		public bool GetExtendPopUpDisplayed()
		{
			WaitUntilDisplayElement(By.XPath(EXTEND_HEADER_POP_UP));
			return GetIsElementDisplay(By.XPath(EXTEND_HEADER_POP_UP));
		}

		/// <summary>
		/// Кликнуть Cancel кнопку в Upgrade pop up
		/// </summary>
		public void ClickCancelBtnInUpgradePopUp()
		{
			ClickElement(By.XPath(CANCEL_BTN_IN_UPGRADE_POP_UP));
		}

		/// <summary>
		/// Кликнуть Buy кнопку в pop up окне
		/// </summary>
		public void ClickBuyBtnInPopUp()
		{
			ClickElement(By.XPath(BUY_BTN_IN_UPGRADE_POP_UP));
		}

		/// <summary>
		/// Выбрать новое кол-во лицензий в Upgrade pop up
		/// </summary>
		/// <param name="i"> Кол-во лицензий </param>
		public void SelectNewNumberLicInUpgradePopUp(int i)
		{
			ClickElement(By.XPath(NEW_NUMBER_DROPDOWN + "//option[text() ='" + i + "']"));
		}

		/// <summary>
		/// Вернуть, показано ли Upgrade pop up
		/// </summary>
		public bool GetPaymentFrameDisplayed()
		{
			return GetIsElementDisplay(By.XPath(PAYMENT_IFRAME));
		}

		/// <summary>
		/// Получить сумму доплаты для апргрейда лицензии
		/// </summary>
		public string  GetAdditionalAmountCost()
		{
			return GetTextElement(By.XPath(ADDITIONAL_AMOUNT));
		}

		/// <summary>
		/// Преобразовать текст в число
		/// </summary>
		/// <param name="text"> Входной текст</param>
		/// <returns> Число </returns>
		public int ParseNumberStrToInt(string text)
		{
			return ParseStrToInt(text);
		}

		/// <summary>
		/// Получить кол-во лицензии в пакете в окне апргрейда 
		/// </summary>
		public int GetLicNumberInPackage()
		{
			string licNumber = GetTextElement(By.XPath(LIC_NUMBER_IN_PACKAGE));

			if (licNumber.EndsWith("лицензий"))
			{
				licNumber = licNumber.Replace(" лицензий", ""); 
			}

			if (licNumber.EndsWith("лицензия"))
			{
				licNumber = licNumber.Replace(" лицензия", "");
			}

			if (licNumber.EndsWith(" licenses") )
			{
				licNumber = licNumber.Replace(" licenses", "");
			}

			if (licNumber.EndsWith(" license"))
			{
				licNumber = licNumber.Replace(" license", "");
			}
			return ParseNumberStrToInt(licNumber);
		}

		/// <summary>
		/// Выбрать кол-во лицензий при апргрейде
		/// </summary>
		public void SelectLicAmountUpgrade()
		{
			ClickElement(By.XPath(NEW_LIC_AMOUNT + "//option[@value='1']"));
		}

		/// <summary>
		/// Кликнуть MyAccount в панели WS
		/// </summary>
		public void ClickMyAccountLink()
		{
			ClickElement(By.XPath(MY_ACCOUNT_LINK));
		}

		/// <summary>
		/// Посчитать кол-во лицензий в таблице на стр личного кабинета
		/// </summary>
		/// <returns> Кол-во строк в таблице лицензий </returns>
		public int GetLicQuantity()
		{
			return GetElementsCount(By.XPath(LIC_QUANTITY));
		}

		/// <summary>
		/// Посчитать дату окончания действия пакета лицензии
		/// </summary>
		public string GetDateFromMsgPopUP()
		{
			string msg = GetTextElement(By.XPath(DATE_IN_MSG_POP_UP));
			return msg.Substring(msg.Length - 10, 9);
		}

		/// <summary>
		/// Получить кол-во кнопок Extend
		/// </summary>
		/// <returns>Kол-во кнопок Extend</returns>
		public int GetExtendBtnsCount()
		{
			 return GetElementList(By.XPath(EXTEND_BTN)).Count;
		}

		/// <summary>
		/// Кликнуть кнопку Continue в поп-апе
		/// </summary>
		public void ClickContinueBtnInPopUp()
		{
			ClickElement(By.XPath(CONTINIUE_BTN_IN_POP_UP));
		}

		/// <summary>
		/// Проверить, что сообщение о триале появилось
		/// </summary>
		public bool GetTrialPopUpDisplayed()
		{
			return GetIsElementDisplay(By.XPath(MSG_TRIAL_POP_UP));
		}

		public bool CheckCurrency(string currency)
		{
			List<string> periodList = GetTextListElement(By.XPath(HEAD_COLUMN_BUY_TABLE));
			return periodList.All(p => p.Contains(currency)); 
		}

		/// <summary>
		/// Проверить содержит ли поле Package Price верный занк валюты
		/// </summary>
		/// <param name="currency"> Знак валюты </param>
		public bool CheckPackagePriceCurrency(string currency)
		{
			string packagePrice = GetTextElement(By.XPath(PACKAGE_PRICE));
			return packagePrice.Contains(currency);
		}

		/// <summary>
		/// Проверить содержит ли поле Additional Payment верный занк валюты
		/// </summary>
		/// <param name="currency"> Знак валюты </param>
		public bool CheckAdditionalPaymentCurrency(string currency)
		{
			string payment = GetTextElement(By.XPath(ADDITIONAL_PAYMENT));
			return payment.Contains(currency);
		}
		public const string MY_ACCOUNT_LINK = "//a[contains(@href, 'Billing')]"; // My Account ссылка в панели WS
		public const string ADD_LIC_TABLE = "//table[contains(@class, 'add-lic')]";
		public const string ADD_LIC_COMBOBOX = ADD_LIC_TABLE + "//select[contains(@ng-model, 'selectedOption')]";
		public const string BUY_ONE_MONTH_BTN = "//a[contains(@class, 'danger')]";
		public const string MY_LIC_LINK = "//a[@href='#/licensepackages']";
		public const string STORE_LINK = "//a[@href='#/paidservices']";
		public const string HEAD_COLUMN_BUY_TABLE = "//table[@class='t-licenses add-lic']//th[contains(@ng-repeat, 'period')]";
		public const string LIC_PURCHASE_POP_UP = "//div[contains(@class, 'popup')]";
		public const string BUY_BTN_IN_LIC_POP_UP = "//div[@class='lic-popup ng-scope']//a[contains(@class, 'danger')]"; // кнопка Buy в License Purchase окне
		public const string PAY_BTN_IN_LIC_POP_UP = "//footer[contains(@class, 'clearfix')]//a[contains(@abb-link-click, 'commitPayment')]"; // кнопка Pay в License Purchase окне
		public const string CANCEL_BTN_IN_LIC_POP_UP = "//footer[contains(@class, 'clearfix')]//a[contains(@abb-link-click, 'close')]"; // кнопка Cancel в License Purchase окне

		public const string CREDIT_CARD_NUMBER = "//input[@id='credit-card-number']";
		public const string EXPIRATION_DATE = "//input[@id='expiration']";
		public const string CVV = "//input[@id='cvv']";
		public const string CALENDAR_CONTROL = "//button[contains(@class, 'calendar')]"; // кнопка для открытия календаря
		public const string DATE_FIELD = "//input[contains(@class, 'date')]"; // поле даты
		public const string PAYMENT_IFRAME = "//iframe";

		public const string LIC_TABLE = "//tbody[@class='ng-scope']//tr[@class='ng-scope']";
		public const string LIC_ROW_IN_TABLE = "//td[contains(text(), '"; // строка в таблице с купленными лизензиями
		public const string UPGRADE_BTNS= "//tr[@class='ng-scope']//td[contains(@ng-if, 'ManuallyCreated')]//a[contains(@ng-show, 'ctrl.canIncrease')]";// все кнокпи Upgrade в таблице
		public const string ROW_IN_TABLE = "//tbody//tr[";
		public const string EXTEND_BTN = "//a[contains(@abb-link-click, 'editLicensePackage(package, false)')]";
		public const string CLOSE_BTN = "//a[contains(@abb-link-click, 'close')]"; // Close кнопка в окошке Thank you после покупки лицензии
		public const string MSG_POP_UP = "//div[contains(@class, 'message-popup ng-scope')]//div[contains(@class, 'content') and (contains(text(), 'Thank you') or contains(text(), 'Спасибо'))]";

		public const string POP_UP_WINDOW = "//div[@class='lic-popup ng-scope']";
		public const string UPGRADE_HEADER_POP_UP = "//h3[contains(text(), 'Upgrade') or contains(text(), 'Расширение')]";
		public const string CANCEL_BTN_IN_UPGRADE_POP_UP = POP_UP_WINDOW + "//a[contains(@abb-link-click, 'close')]";
		public const string BUY_BTN_IN_UPGRADE_POP_UP = POP_UP_WINDOW + "//a[contains(@abb-link-click, 'BuyClick')]";
		public const string NEW_NUMBER_DROPDOWN = "//select[contains(@class, 'ng-pristine ng-untouched ng-valid')]";

		public const string NEW_LIC_AMOUNT = "//tr[contains(@ng-if, 'ctrl.isIncrease')]//select[contains(@ng-options, 'option.amount')]"; // Новое кол-во лицензий при апргрейде
		public const string ADDITIONAL_AMOUNT = "//tr[@ng-if='ctrl.isIncrease() || ctrl.isProlongation()']//td[2]"; // Доплата при апргрейде лицензии
		public const string LIC_NUMBER_IN_PACKAGE = "//table[@class='t-licenses']//td[contains(text(),'Количество лицензий') or contains(text(),'Number of Licenses')]/following-sibling::td"; // Кол-во лицензий в пакете в окне Upgrade
		public const string PACKAGE_COST = "//table[@class='t-licenses']//td[contains(text(),'Стоимость пакета') or contains(text(),'Package cost')]/following-sibling::td"; // Стоимость пакета в окне Upgrade
		public const string LIC_TITLE = "//td[contains(text(),'license') or contains(text(),' licenses')]"; 
		public const string LIC_QUANTITY = "//table[@class='t-licenses ng-scope']/tbody/tr"; // Кол-во лицензий в таблице на стр личного кабинета

		public const string EXTEND_HEADER_POP_UP = "//h3[contains(text(), 'Extention') or contains(text(), 'Продление')]";
		public const string EXTEND_PERIOD_IN_POP_UP = "//select[contains(@ng-options, 'optionsPrices')]";
		public const string EXPIRE__DATE_COLUMN = "//td[contains(@ng-class,'willExpireSoon')]";
		public const string DATE_IN_MSG_POP_UP = "//div[@class='b-popup-content ng-binding']";
		public const string CONTINIUE_BTN_IN_POP_UP = "//div[contains(@class,'lic-popup message')]//footer//a[contains(@class,'btn btn-danger')]";
		public const string MSG_TRIAL_POP_UP = "//div[contains(text(),'trial') or contains(text(),'пробные лицензии будут аннулированы')]"; //As soon as the purchased license package comes into effect, the trial licenses will be cancelled.

		public const string PACKAGE_PRICE = "//table[@class='t-licenses']//tr[3]/td[2]";
		public const string ADDITIONAL_PAYMENT = "//div[@ng-hide='ctrl.isBuy() && ctrl.isPaying']//tr[2]/td[2]";

	}
}