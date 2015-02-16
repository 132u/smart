using NUnit.Framework;
using System;
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов на на продление и Upgrade пакета лицензий
	/// </summary>
	public class BillingTest : AdminTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public BillingTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Тест покупка лицензии
		/// </summary>
		[Test]
		public void BuyLicenseTest()
		{
			// Переходим на страницу My Account
			GoToMyAccount();

			// Покупаем лицензию
			BuyLicense();
		}

		/// <summary>
		/// Тест Upgrade лицензии
		/// </summary>
		[Test]
		public void UpgradeLicenseTest()
		{
			// Переходим на страницу My Account
			GoToMyAccount();

			// Расширяем лицензию
			LicenseUpgrade();
		}

		/// <summary>
		/// Тест дважды расширить лицензию
		/// </summary>
		[Test]
		public void DoubleUpgradeLicenseTest()
		{
			LoginToAdminPage();
			// Создем корп аккаунт и авторизуемся на сайте
			string accountName = CreateCorpAccount("", true);
			AddUserToCorpAccount(Login);
			Authorization(accountName);

			// Два раза обновляем один и тот же пакет лицензий
			for (int i = 0; i < 2; i++)
			{
				GoToMyAccount();
				LicenseUpgrade();
			}
		}



		/// <summary>
		/// Тест на продление лицензии
		/// </summary>
		[Test]
		public void ExtendLicenseTest()
		{
			// Переходим на страницу My Account
			GoToMyAccount();

			// Продливаем период лицензии
			ExtendLicense();
		}

		[Test]
		public void DoubleExtendLicenseTest()
		{
			LoginToAdminPage();
			// Создем корп аккаунт и авторизуемся на сайте
			string accountName = CreateCorpAccount("", true);
			AddUserToCorpAccount(Login);
			Authorization(accountName);

			// Два раза продливаем один и тот же пакет лицензий
			for (int i = 0; i < 2; i++)
			{
				GoToMyAccount();
				ExtendLicense();
			}
		}

		/// <summary>
		/// Продление и расширение пакета лицензий из разных локалей
		/// </summary>
		/// <param name="upgrade">Продление или апргрейд </param>
		/// <param name="currency"> Валюта </param>
		[TestCase(true, "RUB", WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.Russian, "(руб.)")] // Rub -> $
		[TestCase(false, "$", WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.English, "$")] // $ -> Rub
		[TestCase(true, "$", WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.English, "$")] // $ -> Rub
		[TestCase(false, "RUB", WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.Russian, "(руб.)")] // Rub -> $
		[Test]
		public void LocaleCurrency(bool upgrade, string currency, WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT language, string sign)
		{

			LoginToAdminPage();
			// Создем корп аккаунт и авторизуемся на сайте
			string accountName = CreateCorpAccount("", true);
			AddUserToCorpAccount(Login);
			Authorization(accountName);

			// Выбираем язык в WS
			WorkspacePage.SelectLocale(language);
			// Переход в личный кабинет
			GoToMyAccount();
			// Проверка, что в таблице покупки лицензий указан верный знак валюты
			Assert.IsTrue(
				MyAccountPage.CheckCurrency(sign),
				"Ошибка: в таблице покупки лицензий указана другая валюта, должно быть " + currency);

			// Покупка лицензии
			BuyLicense();
			// Переход в WS
			GoToUrl(RelativeUrlProvider.Workspace);
			if (currency == "RUB")
			{
				// Выбираем английский язык в WS
				WorkspacePage.SelectLocale(WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.English);
			}
			if (currency == "$")
			{
				// Выбираем русский язык в WS
				WorkspacePage.SelectLocale(WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.Russian);
			}
			// Переход в личный кабинет
			GoToMyAccount();
			if (currency == "RUB")
			{
				// Проверка, что в таблице покупки лицензий указана верная валюта
				Assert.IsTrue(
					MyAccountPage.CheckCurrency("$"),
					"Ошибка: в таблице покупки лицензий указана другая валюта, должен быть $");
			}
			if (currency == "$")
			{
				// Проверка, что в таблице покупки лицензий указана верная валюта
				Assert.IsTrue(
					MyAccountPage.CheckCurrency("(руб.)"),
					"Ошибка: в таблице покупки лицензий указана другая валюта, должен быть RUB");
			}
			if (upgrade)
			{
				// Кликаем Upgrade кнопку
				MyAccountPage.ClickUpgradeBtn();
			}
			else
			{
				// Кликаем Extend кнопку
				MyAccountPage.ClickExtendBtn(1);
			}

			// Проверка валюты
			Assert.IsTrue(
				MyAccountPage.CheckAdditionalPaymentCurrency(currency),
				"Ошибка: в окне Upgrade AdditionalPayment поле сожержит неверное значение валюты (должен быть " + currency);
			Assert.IsTrue(
				MyAccountPage.CheckPackagePriceCurrency(currency),
				"Ошибка: в окне Upgrade AdditionalPayment поле сожержит неверное значение валюты (должен быть " + currency);
			if (upgrade)
			{
				// Апгрейд лицензии
				LicenseUpgrade();
			}else 
			{
				// Продление лицензии
				ExtendLicense();
			}
		}

		/// <summary>
		/// Заполнение данных карты
		/// </summary>
		/// <paraparam name="cardNumber"> Номер карты </paraparam>
		/// <paparam name="cvv"> Cvv </paparam>
		/// <paparam name="date"> Окончание срока действия </paparam>
		public void EnterCardInfo(string cardNumber = "4111111111111111", string cvv = "123", string date = "12/16")
		{
			// Перейти в фрейм
			MyAccountPage.SwitchToPaymentFrame();

			// Ввод номера карты
			MyAccountPage.EnterCardNumber(cardNumber);

			// Ввод CVV
			MyAccountPage.EnterCvv(cvv);

			// Выбрать дату
			MyAccountPage.EnterExpDate(date);

			// Выйти из фрейма
			MyAccountPage.SwitchToDefaultContentFromPaymentIFrame();

		}

		/// <summary>
		/// Купить лицензию
		/// </summary>
		/// <paparam name="licNumber"> Кол-во лицензий </paparam>
		/// <paparam name="period"> Период </paparam>
		public void BuyLicense(string licNumber = "5", int period = 2)
		{
			// Кол-во лицензий в таблице до покупки
			int licQuantityBefore = MyAccountPage.GetLicQuantity();

			// Выбрать кол-во лицензий
			MyAccountPage.SelectLicenseNumber(licNumber);

			// Кликнуть Buy кнопку 
			MyAccountPage.ClickBuyBtn(period);

			// Дождаться появления License Purchase окна
			MyAccountPage.WaitLicPurchasePopUp();

			// Кликнуть Buy кнопку в License Purchase окне
			MyAccountPage.ClickBuyBtnInLicPopUp();

			if (MyAccountPage.GetTrialPopUpDisplayed())
			{
				MyAccountPage.ClickContinueBtnInPopUp();
			}
			// Заполнение данных карты
			EnterCardInfo();

			// Кликнуть Pay кнопку
			MyAccountPage.ClickPayBtnInLicPopUp();

			// Проверить, что сообщение после покупки лицензии появилось
			Assert.IsTrue(MyAccountPage.GetMsgPopUpDisplayed(),
				"Ошибка: сообщение после покупки лицензии не появилось");

			// Кликнуть Close кнопку в сообщении
			MyAccountPage.ClickCloseBtn();

			// Кол-во лицензий в таблице после покупки
			int licQuantityAfter = MyAccountPage.GetLicQuantity();
			Logger.Info("Кол-во лицензий в таблице до покупки = " + licQuantityBefore + "\nКол-во лицензий в таблице после покупки = " + licQuantityAfter);

			// Проверяем, что кол-во лицензий увеличилось на 1 в таблице
			//Assert.AreEqual(licQuantityBefore, licQuantityAfter - 1 , "Ошибка: кол-во лицензий в таблице не увеличилось после покупки");
		}

		/// <summary>
		/// Совершить Upgrade лицензии
		/// </summary>
		/// <paparam name="i"> Номер строки лицензии, если не указано, то первый элемент</paparam>
		public void LicenseUpgrade(int i = 0)
		{
			// Если нет ни одой кнопки Upgrade
			if (MyAccountPage.UpgradeBtnCount() == 0)
			{
				// Купить лицензию
				BuyLicense();
				GoToMyAccount();
			}

			// Информация о кол-ве видимых и невидимых кнопках Upgrade
			Logger.Info("Количество кнопок upgrade  = " + MyAccountPage.GetUpgradeButtons().Count);
			Logger.Info("Количество видимых кнопок upgrade  = " + MyAccountPage.UpgradeBtnCount());

			// Кликнуть кнопку Upgrade 
			MyAccountPage.ClickUpgradeBtn();

			// Cколько уже лицензий в пакете
			int licenseInPackage = MyAccountPage.GetLicNumberInPackage(); 

			if (licenseInPackage != 40)
			{
				// Проверить, что сумма доплаты изменилась в соответствии с объемом пакета
				CheckAdditionalAmountUpgrade();
			}

			// Кликнуть Buy кнопку
			MyAccountPage.ClickBuyBtnInPopUp();

			// Проверить, что платежный iframe загрузился
			Assert.IsTrue(MyAccountPage.GetPaymentFrameDisplayed(), "Ошибка: платежный iframe не загрузился");

			// Заполнить данные карты
			EnterCardInfo();
			MyAccountPage.ClickBuyBtnInLicPopUp();

			// Проверить, что сообщение после апргрейда лицензии появилось
			Assert.IsTrue(MyAccountPage.GetMsgPopUpDisplayed(),
				"Ошибка: сообщение после апргрейда лицензии не появилось");

			// Кликнуть Close кнопку в сообщении
			MyAccountPage.ClickCloseBtn();
		}

		/// <summary>
		/// Проверить, что доплата изменился правильно в окне Upgrade
		/// </summary>
		public void CheckAdditionalAmountUpgrade()
		{
			string additionalPayment = MyAccountPage.GetAdditionalAmountCost(); // доплата

			// Выбираем новое кол-во лицензий
			MyAccountPage.SelectLicAmountUpgrade();

			// Проверить , что доплата изменилась
			string additionalPayment2 = MyAccountPage.GetAdditionalAmountCost();
			Logger.Info("Доплата до смены кол-ва лицензий в пакете = " + additionalPayment
				+ "\nДоплата после смены кол-ва лицензий в пакете = " + additionalPayment2);

			Assert.AreNotEqual(additionalPayment, additionalPayment2,
				"Ошибка: доплата не изменилась после изменения кол-во лицензий в окне Upgrade");
		}

		/// <summary>
		/// Проверить, что доплата изменился правильно в окне Extend
		/// </summary>
		/// <param name="period"> Период на который продливаем </param>
		public void CheckAdditionalAmountExtend(string period = "3")
		{
			string additionalPayment = MyAccountPage.GetAdditionalAmountCost(); // доплата

			// Выбираем новое кол-во лицензий
			MyAccountPage.SelectExtendPeriod(period);

			// Проверить , что доплата изменилась
			string additionalPayment2 = MyAccountPage.GetAdditionalAmountCost();
			Logger.Info("Доплата до смены кол-ва лицензий в пакете = " + additionalPayment
				+ "\nДоплата после смены кол-ва лицензий в пакете = " + additionalPayment2);

			Assert.AreNotEqual(additionalPayment, additionalPayment2,
				"Ошибка: доплата не изменилась после изменения кол-во лицензий в окне Upgrade");
		}

		public void SelestLicenceWithNotMaxPackage()
		{
			// Если нет ни одой видимой кнопки Upgrade
			if (MyAccountPage.UpgradeBtnCount() == 0)
			{
				// Купить лицензию
				BuyLicense();
				GoToMyAccount();
			}

			// Информация о кол-ве видимых и невидимых кнопках Upgrade
			Logger.Info("Количество кнопок upgrade  = " + MyAccountPage.GetUpgradeButtons().Count);
			Logger.Info("Количество видимых кнопок upgrade  = " + MyAccountPage.UpgradeBtnCount());

			for (int i = 0; i < MyAccountPage.UpgradeBtnCount(); i++)
			{
				// Кликнуть кнопку Upgrade 
				MyAccountPage.ClickUpgradeBtn(i);
				int licInPackage = MyAccountPage.GetLicNumberInPackage(); // сколько уже лицензий в пакете
				if (licInPackage < 30)
				{
					// Кликнуть Buy кнопку
					MyAccountPage.ClickBuyBtnInPopUp();

					// Проверить, что платежный iframe загрузился
					Assert.IsTrue(MyAccountPage.GetPaymentFrameDisplayed(), "Ошибка: платежный iframe не загрузился");

					// Заполнить данные карты
					EnterCardInfo();
					MyAccountPage.ClickBuyBtnInLicPopUp();

					// Проверить, что сообщение после апргрейда лицензии появилось
					Assert.IsTrue(MyAccountPage.GetMsgPopUpDisplayed(),
						"Ошибка: сообщение после апргрейда лицензии не появилось");

					// Кликнуть Close кнопку в сообщении
					MyAccountPage.ClickCloseBtn();
					break;
				}
				else
				{
					MyAccountPage.ClickCancelBtnInUpgradePopUp();
					if (MyAccountPage.UpgradeBtnCount() == 1)
					{
						BuyLicense();
					}
				}
			}
		}

		/// <summary>
		/// Продлить лицензию
		/// </summary>
		/// <param name="rowNumber"> Номер строки </param>
		public string ExtendLicense(int rowNumber = 1)
		{
			// Если нет ни одной кнопки Extend, покупаем лицензию
			if (MyAccountPage.GetExtendBtnsCount() == 0)
			{
				BuyLicense();
				GoToMyAccount();
			}
			// Кликнуть по Extend кнопке
			MyAccountPage.ClickExtendBtn(rowNumber);

			// Проверка,что после смены периода продления лицензии, доплата именилась
			CheckAdditionalAmountExtend();

			// Кликнуть Buy кнопку
			MyAccountPage.ClickBuyBtnInPopUp();

			// Проверить, что платежный iframe загрузился
			Assert.IsTrue(MyAccountPage.GetPaymentFrameDisplayed(), "Ошибка: платежный iframe не загрузился");

			// Заполнить данные карты
			EnterCardInfo();
			MyAccountPage.ClickBuyBtnInLicPopUp();

			// Проверить, что сообщение после апргрейда лицензии появилось
			Assert.IsTrue(MyAccountPage.GetMsgPopUpDisplayed(),
				"Ошибка: сообщение после продления лицензии не появилось");

			// Получить дату окончания действия пакета из сообщения
			string expireDate = MyAccountPage.GetDateFromMsgPopUP();

			// Кликнуть Close кнопку в сообщении
			MyAccountPage.ClickCloseBtn();

			Logger.Info("expireDate = " + expireDate);
			return expireDate;
		}
	}
}
