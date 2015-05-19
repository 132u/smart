using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

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
			Logger.Trace("Кликнуть Buy кнопку");
			ClickElement(By.XPath("//td[" + i + "]" +BUY_ONE_MONTH_BTN));
		}

		/// <summary>
		/// Выбрать количество лицензий
		/// </summary>
		/// <param name="i"> количество лицензий</param>
		public void SelectLicenseNumber(string i)
		{
			Logger.Trace("Выбираем " + i + " лицензий");
			ClickElement(By.XPath(ADD_LIC_COMBOBOX + "//option[text()='" + i + "']"));
		}

		/// <summary>
		/// Выбрать период для продления лицензии
		/// </summary>
		/// <param name="period"> </param>
		public void SelectExtendPeriod(string period)
		{
			Logger.Trace("Выбираем период продления лицензии = " + period);
			ClickElement(By.XPath(EXTEND_PERIOD_IN_POP_UP + "//option[contains(text(),'" + period + "')]"));
		}

		/// <summary>
		/// Дождаться появления License Purchase окна
		/// </summary>
		public void WaitLicPurchasePopUp()
		{
			Logger.Trace("Ожидание появления License Purchase окна");
			WaitUntilDisplayElement(By.XPath(LIC_PURCHASE_POP_UP));
		}

		/// <summary>
		/// Кликнуть Buy кнопку в License Purchase окне
		/// </summary>
		public void ClickBuyBtnInLicPopUp()
		{
			Logger.Trace("Кликнуть Buy кнопку в License Purchase окне");
			ClickElement(By.XPath(BUY_BTN_IN_LIC_POP_UP));
		}

		/// <summary>
		/// Кликнуть Pay кнопку в License Purchase окне
		/// </summary>
		public void ClickPayBtnInLicPopUp()
		{
			Logger.Trace("Кликнуть Pay кнопку в License Purchase окне");
			ClickElement(By.XPath(PAY_BTN_IN_LIC_POP_UP));
		}

		/// <summary>
		/// Кликнуть Cancel кнопку в License Purchase окне
		/// </summary>
		public void ClickCancelBtnInLicPopUp()
		{
			Logger.Trace("Кликнуть Cancel кнопку в License Purchase окне");
			ClickElement(By.XPath(CANCEL_BTN_IN_LIC_POP_UP));
		}

		/// <summary>
		/// Ввести номер карты
		/// </summary>
		public void EnterCardNumber(string cardNumber)
		{
			Logger.Trace("Ожидание появления поля для ввода номера карты");
			WaitUntilDisplayElement(By.XPath(CREDIT_CARD_NUMBER));
			Logger.Trace("Ввод номера карты " + cardNumber);
			SendTextElement(By.XPath(CREDIT_CARD_NUMBER), cardNumber);
		}

		/// <summary>
		/// Перейти в Frame платежной системы
		/// </summary>
		public void SwitchToPaymentFrame()
		{
			Logger.Trace("Переключение в Frame платежной системы");
			SwitchToFrame(By.XPath(PAYMENT_IFRAME));
		}

		/// <summary>
		/// Выйти из iframe платежной системы
		/// </summary>
		public void SwitchToDefaultContentFromPaymentIFrame()
		{
			Logger.Trace("Переключение к основному контенту");
			SwitchToDefaultContent();
		}

		/// <summary>
		/// Ввести CVV карты
		/// </summary>
		/// <param name="cvv"> Cvv </param>
		public void EnterCvv(string cvv)
		{
			Logger.Trace("Ожидание появления поля для ввода CVV");
			WaitUntilDisplayElement(By.XPath(CVV));
			Logger.Trace("Ввод CVV");
			SendTextElement(By.XPath(CVV), cvv);
		}

		/// <summary>
		/// Выбрать дату окончания срока действия карты
		/// </summary>
		/// <param name="date"> Дата окончания срока действия </param>
		public void EnterExpDate(string date)
		{
			Logger.Trace("Ожидание появления поля даты окончания срока действия карты");
			WaitUntilDisplayElement(By.XPath(EXPIRATION_DATE));
			Logger.Trace("Ввод даты окончания срока действия карты");
			SendTextElement(By.XPath(EXPIRATION_DATE), date);
		}

		/// <summary>
		/// Получитьсписок всех кнопок Upgrade(видимых и невидимых)
		/// </summary>
		/// <returns> Список кнопок Upgrade </returns>
		public IList<IWebElement> GetUpgradeButtons()
		{
			Logger.Trace("Получить список кнопок Upgrade(видимых и невидимых)");
			return GetElementList(By.XPath(UPGRADE_BTNS));
		}

		/// <summary>
		/// Получить номер строки с немаксимальным кол-ом лицензий в пакете
		/// </summary>
		/// <returns> Номер строки </returns>
		public int GetRowNumberPackageInTable()
		{
			var tdList = GetElementList(By.XPath(LIC_TITLE));
			var titles = tdList.Select(td => td.Text).ToList();

			for (var t = 0; t < titles.Count;t++ )
			{
				if (titles[t].EndsWith(" licenses"))
				{
					titles[t] = titles[t].Replace(" licenses", "");
				}
				if(titles[t].EndsWith(" лицензий"))
				{
					titles[t] = titles[t].Replace(" лицензий", "");
				}
				
				if (ParseNumberStrToInt(titles[t]) < 30)
					return t;
			}
			return -1;
		}

		/// <summary>
		/// Получить номер строки пакета для продления
		/// </summary>
		/// <param name="titleLicense"> Текст из 1й колонки таблицы пакетов</param>
		/// <param name="date"> Expire date </param>
		/// <returns> Номер строки</returns>
		public int GetRowNumberOfLicense(string titleLicense, string date = "")
		{
			var titleList = GetElementList(By.XPath(LIC_TITLE));
			var dateList = GetElementList(By.XPath(EXPIRE__DATE_COLUMN));

			var rows = titleList.Select((t, i) => t.Text + " = " + dateList[i].Text).ToList();

			for (int i = 0; i < rows.Count; i++)
			{
				if (rows[i].Contains(titleLicense) && rows[i].Contains(date))
					return i;
			}

			return -1;
		}

		/// <summary>
		/// Получить список видимых Upgrade кнопкок
		/// </summary>
		/// <returns> Cписок видимых Upgrade кнопкок </returns>
		public IList<IWebElement> GetVisibleUpgradeButtons()
		{
			Logger.Trace("Получить список видимых кнопок Upgrade");
			return GetUpgradeButtons().Where(btn => btn.Displayed).ToList();
		}

		/// <summary>
		/// Посчитать количество видимых Upgrade кнопок
		/// </summary>
		/// <returns> количество Upgrade кнопок </returns>
		public int GetUpgradeBtnCount()
		{
			Logger.Trace("Получить количество всех видимых кнопок Upgrade");
			return GetVisibleUpgradeButtons().Count;
		}

		/// <summary>
		/// Кликнуть Upgrade кнопку
		/// </summary>
		public void ClickUpgradeBtn(int i = 0)
		{
			var buttons = GetVisibleUpgradeButtons();
			Logger.Trace("Кликнуть Upgrade кнопку №" + i);
			buttons[i].Click();

			// Проверить, что Upgrade окно открылось
			Assert.IsTrue(GetUpgradePopUpDisplayed(), "Ошибка: Upgrade окно не появилось");

		}

		/// <summary>
		/// Кликнуть Extend кнопку в таблице с купленными лизензиями
		/// </summary>
		/// <param name="i"> Номер строки </param>
		public void OpenExtendPopUp(int i)
		{
			Logger.Trace("Кликнуть Extend кнопку в таблице с купленными лизензиями");
			ClickElement(By.XPath(ROW_IN_TABLE + i + "]" + EXTEND_BTN));
			GetExtendPopUpDisplayed();
		}

		/// <summary>
		/// Кликнуть Close кнопку в окне благодарности за покупку лицензии
		/// </summary>
		public void ClickCloseBtn()
		{
			Logger.Trace("Кликнуть Close кнопку в окне благодарности за покупку лицензии");
			ClickElement(By.XPath(CLOSE_BTN));
		}

		public bool CloseButtonDisplay()
		{
			Logger.Trace("проверить, что кнопка закрытия появилась");
			return WaitUntilDisplayElement(By.XPath(CLOSE_BTN));
		}

		/// <summary>
		/// Вернуть, показано ли окно с сообщением после покупки лицензии
		/// </summary>
		public bool GetPaymentPopUpDisplayed()
		{
			Logger.Trace("Определить, показано ли окно с сообщением после покупки лицензии");
			return GetIsElementDisplay(By.XPath(MSG_POP_UP));
		}

		/// <summary>
		/// Вернуть, показано ли Upgrade pop-up
		/// </summary>
		public bool GetUpgradePopUpDisplayed()
		{
			Logger.Trace("Ожидание открытия Upgrade pop-up");
			return WaitUntilDisplayElement(By.XPath(UPGRADE_HEADER_POP_UP));
		}

		/// <summary>
		/// Вернуть, показано ли Extend pop-up
		/// </summary>
		public bool GetExtendPopUpDisplayed()
		{
			Logger.Trace("Ожидание открытия Extend pop-up");
			return WaitUntilDisplayElement(By.XPath(EXTEND_HEADER_POP_UP));
		}

		public void WaitPopUpClosed()
		{
			WaitUntilDisappearElement(By.XPath(POP_UP_WINDOW));
		}

		/// <summary>
		/// Кликнуть Cancel кнопку в Upgrade pop-up
		/// </summary>
		public void ClickCancelBtnInUpgradePopUp()
		{
			Logger.Trace("Кликнуть Cancel кнопку в Upgrade pop-up");
			ClickElement(By.XPath(CANCEL_BTN_IN_UPGRADE_POP_UP));
		}

		/// <summary>
		/// Кликнуть Buy кнопку в pop-up окне
		/// </summary>
		public void ClickBuyBtnInPopUp()
		{
			Logger.Trace("Кликнуть Buy кнопку в pop-up");
			ClickElement(By.XPath(BUY_BTN_IN_UPGRADE_POP_UP));
		}

		/// <summary>
		/// Выбрать новое количество лицензий в Upgrade pop-up
		/// </summary>
		/// <param name="i"> количество лицензий </param>
		public void SelectNewNumberLicInUpgradePopUp(int i)
		{
			Logger.Trace("Выбрать новое количество лицензий в Upgrade pop-up");
			ClickElement(By.XPath(NEW_NUMBER_DROPDOWN + "//option[text() ='" + i + "']"));
		}

		/// <summary>
		/// Вернуть, показано ли Upgrade pop-up
		/// </summary>
		public bool GetPaymentFrameDisplayed()
		{
			Logger.Trace("Проверить показано ли Upgrade pop-up");
			return GetIsElementDisplay(By.XPath(PAYMENT_IFRAME));
		}

		/// <summary>
		/// Получить сумму доплаты для Upgrade лицензии
		/// </summary>
		public string  GetAdditionalAmountCost()
		{
			Logger.Trace("Получить сумму доплаты для Upgrade лицензии");
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
		/// Получить количество лицензии в пакете в окне Upgrade 
		/// </summary>
		public int GetLicNumberInPackage()
		{
			Logger.Trace("Получить количество лицензии в пакете в окне Upgrade");
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
		/// Выбрать количество лицензий при Upgrade
		/// </summary>
		public void SelectLicAmountUpgrade()
		{
			Logger.Trace("Выбрать количество лицензий при Upgrade");
			ClickElement(By.XPath(NEW_LIC_AMOUNT + "//option[@value='1']"));
		}

		/// <summary>
		/// Кликнуть MyAccount в панели WS
		/// </summary>
		public void ClickMyAccountLink()
		{
			Logger.Trace("Кликнуть MyAccount в панели WS");
			ClickElement(By.XPath(MY_ACCOUNT_LINK));
		}

		/// <summary>
		/// Посчитать количество лицензий в таблице на стр личного кабинета
		/// </summary>
		/// <returns> количество строк в таблице лицензий </returns>
		public int GetLicQuantity()
		{
			Logger.Trace("Посчитать количество лицензий в таблице на стр личного кабинета");
			return GetElementsCount(By.XPath(LIC_QUANTITY));
		}

		/// <summary>
		/// Посчитать дату окончания действия пакета лицензии
		/// </summary>
		public string GetDateFromMsgPopUP()
		{
			Logger.Trace("Получить дату окончания действия пакета лицензии");
			string msg = GetTextElement(By.XPath(DATE_IN_MSG_POP_UP));
			return msg.Substring(msg.Length - 10, 9);
		}

		/// <summary>
		/// Получить количество кнопок Extend
		/// </summary>
		/// <returns>Kол-во кнопок Extend</returns>
		public int GetExtendBtnsCount()
		{
			Logger.Trace("Получить количество кнопок Extend");
			 return GetElementList(By.XPath(EXTEND_BTN)).Count;
		}

		/// <summary>
		/// Кликнуть кнопку Continue в pop-up
		/// </summary>
		public void ClickContinueBtnInPopUp()
		{
			Logger.Trace("Кликнуть кнопку Continue в pop-up");
			ClickElement(By.XPath(CONTINIUE_BTN_IN_POP_UP));
		}

		/// <summary>
		/// Проверить, что сообщение о триале появилось
		/// </summary>
		public bool GetTrialPopUpDisplayed()
		{
			Logger.Trace("Проверить, что сообщение о триале появилось");
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
			Logger.Trace("Проверить, содержит ли поле Package Price занк валюты " + currency);
			string packagePrice = GetTextElement(By.XPath(PACKAGE_PRICE));
			return packagePrice.Contains(currency);
		}

		/// <summary>
		/// Проверить содержит ли поле Additional Payment верный занк валюты
		/// </summary>
		/// <param name="currency"> Знак валюты </param>
		public bool CheckAdditionalPaymentCurrency(string currency)
		{
			Logger.Trace("Проверить, содержит ли поле Additional Payment занк валюты " + currency);
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
		public const string PAYMENT_IFRAME = "//form[@id='checkoutForm']//iframe";

		public const string LIC_TABLE = "//tbody[@class='ng-scope']//tr[@class='ng-scope']";
		public const string LIC_ROW_IN_TABLE = "//td[contains(text(), '"; // строка в таблице с купленными лизензиями
		public const string UPGRADE_BTNS= "//tr[@class='ng-scope']//td[contains(@ng-if, 'ManuallyCreated')]//a[contains(@ng-show, 'ctrl.canIncrease')]";// все кнокпи Upgrade в таблице
		public const string ROW_IN_TABLE = "//tbody//tr[";
		public const string EXTEND_BTN = "//a[contains(@abb-link-click, 'editLicensePackage(package, false)')]";
		public const string CLOSE_BTN = "//a[contains(@abb-link-click, 'close')]"; // Close кнопка в окошке Thank you после покупки лицензии
		public const string MSG_POP_UP = "//div[contains(@class, 'message-popup ng-scope')]//div[contains(@class, 'content') and (contains(text(), 'Thank you') or contains(text(), 'Спасибо'))]";

		public const string POP_UP_WINDOW = "//div[@class='lic-popup ng-scope']";
		public const string UPGRADE_HEADER_POP_UP = "//h3[contains(text(), 'Upgrade') or contains(text(), 'Расширение')]";
		public const string CANCEL_BTN_IN_UPGRADE_POP_UP = POP_UP_WINDOW + "//a[contains(@abb-link-click, 'close') and contains(@class, 'btn')]";
		public const string BUY_BTN_IN_UPGRADE_POP_UP = POP_UP_WINDOW + "//a[contains(@abb-link-click, 'BuyClick')]";
		public const string NEW_NUMBER_DROPDOWN = "//select[contains(@class, 'ng-pristine ng-untouched ng-valid')]";

		public const string NEW_LIC_AMOUNT = "//tr[contains(@ng-if, 'ctrl.isIncrease')]//select[contains(@ng-options, 'option.amount')]"; // Новое количество лицензий при Upgrade
		public const string ADDITIONAL_AMOUNT = "//tr[@ng-if='ctrl.isIncrease() || ctrl.isProlongation()']//td[2]"; // Доплата при Upgrade лицензии
		public const string LIC_NUMBER_IN_PACKAGE = "//table[@class='t-licenses']//td[contains(text(),'Количество лицензий') or contains(text(),'Number of Licenses')]/following-sibling::td"; // количество лицензий в пакете в окне Upgrade
		public const string PACKAGE_COST = "//table[@class='t-licenses']//td[contains(text(),'Стоимость пакета') or contains(text(),'Package cost')]/following-sibling::td"; // Стоимость пакета в окне Upgrade
		public const string LIC_TITLE = "//td[contains(text(),'license') or contains(text(),' licenses')]"; 
		public const string LIC_QUANTITY = "//table[@class='t-licenses ng-scope']/tbody/tr"; // количество лицензий в таблице на стр личного кабинета

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
