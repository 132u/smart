using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки словарей
	/// </summary>
	public class DictionaryTest<TWebDriverSettings> : AdminTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUp()
		{
			if (Standalone)
				Assert.Ignore("Тест игнорируется, так как это отделяемое решение");
		}

		[SetUp]
		public void Setup()
		{
			LoginToAdminPage();
			CreateDictionaryPackIfItNotExist();
		}

		/// <summary>
		/// Тест: проверка перевода со словарями
		/// </summary>
		[Test]
		public void CheckResultTranslation()
		{
			AddAccountWithDictionaries();

			// Найти перевод слова
			InitSearch("tester");

			// Проверить результат: должен быть показан результат из словаря
			Assert.IsTrue(
				SearchPage.GetIsDictionarySearchResultExist(),
				"Ошибка: не показаны результаты поиска по словарю");

			// Проверить словарь
			string resultText = SearchPage.GetDictionaryName();

			Assert.AreEqual(
				"ABBYY Lingvo dictionaries (En-Ru)", resultText,
				"Ошибка: не тот словарь: " + resultText);
		}

		/// <summary>
		/// Тест: проверка, что перевод является ссылкой
		/// </summary>
		[Test]
		public void CheckTranslationReference()
		{
			AddAccountWithDictionaries();

			// Найти перевод слова
			InitSearch("tester");
			
			// Проверить, что перевод - ссылка
			Assert.IsTrue(
				SearchPage.GetIsTranslationRefExist(),
				"Ошибка: перевод - не ссылка");
		}

		/// <summary>
		/// Тест: проверка обратного перевода
		/// </summary>
		[Test]
		public void CheckReverseTranslation()
		{
			AddAccountWithDictionaries();
			
			// Найти перевод слова
			InitSearch("tester");

			// Нажать на перевод
			SearchPage.ClickTranslation();

			// Дождаться открытия форму с переводом
			SearchPage.WaitUntilTranslationFormAppear();
			// Кликнуть по переводу
			SearchPage.ClickTranslationFormRef();

			// Дождаться появления результата (перевод по каждому слову)
			SearchPage.WaitWordByWordTranslationAppear();

			// Проверить, что Source теперь Русский
			Assert.IsTrue(
				SearchPage.GetIsSourceRussian(),
				"Ошибка: язык Source не изменился на русский");

			// Проверить, что появились переводы слов (со ссылками)
			Assert.IsTrue(
				SearchPage.GetIsReverseTranslationListExist(),
				"Ошибка: нет обратных переводов");
		}

		/// <summary>
		/// Тест: проверка, что автоматически изменяется пара Source-Target языков
		/// </summary>
		[Test]
		public void CheckAutoReverse()
		{
			AddAccountWithDictionaries();

			// Поменять языки на странице поиска, если надо
			ChangeLanguages();

			// Найти перевод слова
			InitSearch("tester");

			// Проверить, что появилось сообщение об автоматическом изменении языков
			Assert.IsTrue(
				SearchPage.GetIsExistAutoreversedMessage(),
				"Ошибка: не появилось сообщение об автоматическом изменении языков");
			// Проверить ссылку для возврата языков
			Assert.IsTrue(
				SearchPage.GetIsExistAutoreversedRef(),
				"Ошибка: не появилось ссылки для возврата языков");
		}

		/// <summary>
		/// Тест: проверка создания аккаунта со списком словарей
		/// </summary>
		[Test]
		public void AccountExistDictionaryList()
		{
			OpenCreateAccountForm();
			string accountName = FillGeneralAccountFields();
			AddDictionaryAccount();
			AdminPage.ClickSubmit();
			AdminPage.WaitSuccessAnswer();
			AddUserToAccount(Login);
			Logger.Trace("Переход  в SmartCat");
			Driver.Navigate().GoToUrl(Url);
			Authorization(Login, Password, accountName);
			Assert.IsTrue(
				MainHelperClass.GetIsRefDictionariesVisible(),
				"Ошибка: не показывается вкладка Lingvo Dictionaries");

			MainHelperClass.ClickOpenDictionariesPage();

			Assert.IsTrue(
				DictionaryPage.GetDictionaryListCount() > 0, 
				"Ошибка: список словарей пуст");
		}

		/// <summary>
		/// Тест: создание аккаунта без функции Словари
		/// </summary>
		[Test]
		public void AccountWithoutDictionaryTab()
		{
			// Открыть форму создания аккаунта
			OpenCreateAccountForm();

			// Заполнить форму аккаунта
			var accountName = FillGeneralAccountFields();

			// Сохранить
			AdminPage.ClickSubmit();

			// Дождаться успеха
			AdminPage.WaitSuccessAnswer();

			// Добавить пользователя в аккаунт
			AddUserToAccount(Login);

			// Перейти в CAT
			Driver.Navigate().GoToUrl(Url);
			// Зайти пользователем
			Authorization(Login, Password, accountName);

			// Проверка, что вкладка LingvoDictionaries не видна
			Assert.IsFalse(
				MainHelperClass.GetIsRefDictionariesVisible(),
				"Ошибка: вкладка Lingvo Dictionaries видна (не должно быть видно)");
		}

		/// <summary>
		/// Тест: создание аккаунта с Фукнцией Словари, но без списка словарей
		/// </summary>
		[Test]
		public void AccountDictionaryEmptyList()
		{
			// Открыть форму создания аккаунта
			OpenCreateAccountForm();

			// Заполнить форму аккаунта
			var accountName = FillGeneralAccountFields();

			// Добавить функцию Словари, но не добавлять список словарей
			AddDictionaryAccount(false);

			// Сохранить
			AdminPage.ClickSubmit();

			// Дождаться успеха
			AdminPage.WaitSuccessAnswer();

			// Добавить пользователя в аккаунт
			AddUserToAccount(Login);

			var listOfDictionariesForSpecificPack = GetDictionaryList(DictionaryPackName);

			Logger.Trace("Переход в SmartCat");
			Driver.Navigate().GoToUrl(Url);
			// Зайти пользователем
			Authorization(Login, Password, accountName);

			// Проверка, что вкладка LingvoDictionaries видна
			Assert.IsTrue(
				MainHelperClass.GetIsRefDictionariesVisible(),
				"Ошибка: не показывается вкладка Lingvo Dictionaries");

			// Перейти на вкладку со словарями
			MainHelperClass.ClickOpenDictionariesPage();

			var listOfDictionaries = DictionaryPage.GetDictionaryList();

			Assert.IsTrue(
				listOfDictionariesForSpecificPack.Count == listOfDictionaries.Count,
				"Количество словарей в админке и в кате различное.");

			Assert.IsTrue(
				listOfDictionariesForSpecificPack.Except(listOfDictionaries).ToList().Count == 0,
				"Некоторые из словарей стандартного пакета не найдены.");

			Assert.IsTrue(
				listOfDictionaries.Except(listOfDictionariesForSpecificPack).ToList().Count == 0,
				"Некоторые из словарей найденных в кате не являются словарями из стандартного пакета.");
		}

		/// <summary>
		/// Тест: создание аккаунта, добавление словарей при редактировании
		/// </summary>
		[Test]
		public void AccountAddDictionaries()
		{
			// Открыть форму создания аккаунта
			OpenCreateAccountForm();

			// Заполнить форму аккаунта
			string accountName = FillGeneralAccountFields();
			// Добавить функцию Словари, но не добавлять список словарей
			AddDictionaryAccount(false);

			// Сохранить
			AdminPage.ClickSubmit();

			// Дождаться успеха
			AdminPage.WaitSuccessAnswer();

			// Добавить пользователя в аккаунт
			AddUserToAccount(Login);

			// Перейти в корпоративные аккаунты
			SwitchEnterpriseAccountList();
			// Нажать редактирование созданного аккаунта
			AdminPage.ClickEditAccount(accountName);

			// Добавить словари
			AddDictionaryAccount();
			// Сохранить
			AdminPage.ClickSubmit();

			// Перейти в CAT
			Driver.Navigate().GoToUrl(Url);
			// Зайти пользователем
			Authorization(Login, Password, accountName);

			// Проверка, что вкладка LingvoDictionaries видна
			Assert.IsTrue(
				MainHelperClass.GetIsRefDictionariesVisible(),
				"Ошибка: не показывается вкладка Lingvo Dictionaries");

			// Перейти на вкладку со словарями
			MainHelperClass.ClickOpenDictionariesPage();

			// Проверить, что есть словари
			Assert.IsTrue(
				DictionaryPage.GetDictionaryListCount() > 0, 
				"Ошибка: список словарей пуст");
		}

		/// <summary>
		/// Тест: проверка перехода на Definitions, если Source и Target языки совпадают
		/// </summary>
		[Test]
		public void CheckDefinitions()
		{
			// Зайти
			Authorization(Login, Password);
			SwitchSearchTab();
			SearchPage.SelectEnSourceLanguage();
			SearchPage.SelectEnTargetLanguage();
			
			// Найти перевод слова
			InitSearch("tester");
			SearchPage.WaitSearchResult();

			// Проверить, что вкладка Definitions активна
			Assert.IsTrue(
				SearchPage.GetIsDefinitionTabActive(), 
				"Ошибка: не перешли на вкладку Definitions");
		}

		/// <summary>
		/// Добавить словари в аккаунт
		/// </summary>
		/// <param name="needAddAllDictionaries">добавить все словари из списка (false - не добавлять)</param>
		protected void AddDictionaryAccount(bool needAddAllDictionaries = true)
		{
			// Выбрать Функцию Словари
			if (AdminPage.GetAvailableAddDictionaryFeature())
			{
				AdminPage.ClickDictioaryInFeatures();
				AdminPage.ClickToRightFeatureTable();
			}

			if (needAddAllDictionaries)
			{
				// Добавить словари
				AdminPage.AddAllDictionaries();
			}

			// Добавить дату
			AdminPage.FillDictionaryDeadlineDate(DateTime.Now.AddDays(10));
		}

		/// <summary>
		/// Создать аккаунт со словарями
		/// </summary>
		protected void AddAccountWithDictionaries()
		{
			// Открыть форму создания аккаунта
			OpenCreateAccountForm();

			// Заполнить форму аккаунта
			string accountName = FillGeneralAccountFields();
			// Добавить словари
			AddDictionaryAccount();

			// Сохранить
			AdminPage.ClickSubmit();

			// Дождаться успеха
			AdminPage.WaitSuccessAnswer();

			// Добавить пользователя в аккаунт
			AddUserToAccount(Login);

			// Перейти в CAT
			Driver.Navigate().GoToUrl(Url);
			// Зайти пользователем
			Authorization(Login, Password, accountName);

			// Проверка, что вкладка LingvoDictionaries видна
			Assert.IsTrue(
				MainHelperClass.GetIsRefDictionariesVisible(),
				"Ошибка: не показывается вкладка Lingvo Dictionaries");

			// Перейти на вкладку со словарями
			MainHelperClass.ClickOpenDictionariesPage();

			Assert.IsTrue(
				DictionaryPage.GetDictionaryListCount() > 0, 
				"Ошибка: список словарей пуст");

			// Перейти на вкладку Search
			SwitchSearchTab();
		}

		/// <summary>
		/// Поменять языки на вкладке поиска (сорс - русский, таргет - английский)
		/// </summary>
		protected void ChangeLanguages()
		{
			if (!SearchPage.GetIsSourceRussian())
			{
				SearchPage.SelectRuSourceLanguage();
				SearchPage.SelectEnTargetLanguage();
			}
		}

		/// <summary>
		/// Получить список словарей для пакета pack
		/// </summary>
		protected List<String> GetDictionaryList(string pack)
		{
			AdminPage.GotoDictionaryPackPage();
			AdminPage.SelectDictionaryPack(pack);

			return AdminPage.GetListOfDictionaries();
		}

		/// <summary>
		/// Создать общедоступный пакет словарей если он не существует
		/// </summary>
		protected void CreateDictionaryPackIfItNotExist()
		{
			AdminPage.GotoDictionaryPackPage();

			if (!AdminPage.IsRequiredDictionaryPackExist(DictionaryPackName))
			{
				AdminPage.GoToDictionaryPackCreationPage();
				AdminPage.AddDictionaryPackName(DictionaryPackName);
				AdminPage.MakePublicDictionatyPack();
				AdminPage.AddDictionariesToPack(PublicDictionaryList);
				AdminPage.CreateDictionaryPack();
			}
		}

		private const string DictionaryPackName = "Общедоступные";
		private static readonly List<string> PublicDictionaryList = new List<string>
		{
			"Economics (De-Ru)", 
			"Economics (Ru-De)",
			"Explanatory (Uk-Uk)",
			"ForumDictionaryEn-Ru",
			"ForumDictionaryGe-Ru",
			"Law (De-Ru)",
			"Law (En-Ru)",
			"Law (Fr-Ru)",
			"Law (Ru-De)",
			"Law (Ru-En)",
			"Law (Ru-Fr)",
			"LingvoComputer (En-Ru)",
			"LingvoComputer (Ru-En)",
			"LingvoEconomics (En-Ru)",
			"LingvoEconomics (Ru-En)",
			"LingvoScience (En-Ru)",
			"LingvoScience (Ru-En)",
			"LingvoThesaurus (Ru-Ru)",
			"LingvoUniversal (En-Ru)",
			"Marketing (En-Ru)",
			"Medical (De-Ru)",
			"Medical (En-Ru)",
			"Medical (Ru-De)",
			"Medical (Ru-En)",
			"Polytechnical (It-Ru)",
			"Polytechnical (Ru-It)",
			"Pronouncing (Uk-Uk)",
			"Universal (De-Ru)",
			"Universal (En-Uk)",
			"Universal (Es-Ru)",
			"Universal (Fr-Ru)",
			"Universal (It-Ru)",
			"Universal (La-Ru)",
			"Universal (Ru-De)",
			"Universal (Ru-En)",
			"Universal (Ru-Es)",
			"Universal (Ru-Fr)",
			"Universal (Ru-It)",
			"Universal (Ru-Uk)",
			"Universal (Uk-En)",
			"Universal (Uk-Ru)"
		};
	}
}
