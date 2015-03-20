using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.Glossary.UserRights
{
	/// <summary>
	/// Группа тестов для проверки редактирования терминов глоссария
	/// </summary>
	[TestFixture]
	[Category("Standalone")]
	public class ManageTermsTest : GlossaryTest
	{
		public ManageTermsTest(string browserName)
			: base(browserName)
		{
		}

		[TestFixtureSetUp]
		public void SetupGlossaryItemTest()
		{
			Logger.Info("Начало работы метода SetupGlossaryItemTest(). Подготовка перед каждым тест-сетом.");

			try
			{
				Logger.Debug("Значение параметра QuitDriverAfterTest = false. Не закрывать браузер после каждого теста.");
				QuitDriverAfterTest = false;

				GoToUrl(RelativeUrlProvider.Glossaries);
				AddUserRights();
			}
			catch (Exception ex)
			{
				ExitDriver();
				Logger.ErrorException("Ошибка в конструкторе : " + ex.Message, ex);
				throw;
			}
		}

		/// <summary>
		/// Метод тестирования создания Item с обычном режиме
		/// </summary>
		[Test]
		public void CreateItemGeneralTest()
		{
			Logger.Info("Начало работы теста CreateItemGeneralTest().");

			CreateGlossaryByName(GetUniqueGlossaryName());
			CreateItemAndSave();

			// Проверить количество терминов
			Assert.IsTrue(GlossaryPage.GetConceptCount() > 0, 
				"Ошибка: количество терминов должно быть больше 0 (термин не сохранился)");
		}

		/// <summary>
		/// Метод тестирования создания Item с расширенном режиме
		/// </summary>
		[Test]
		public void CreateItemExtendedTest()
		{
			Logger.Info("Начало работы теста CreateItemExtendedTest().");

			// Создать новый глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());

			// Изменить структуру для перехода в расширенный режим
			EditGlossaryStructureAddField();

			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			// Заполнить поля с терминами
			FillNewItemExtended();
			// Сохранить
			GlossaryPage.ClickSaveExtendedConcept();
			GlossaryPage.AssertionConceptSave();
			// Свернуть
			GlossaryPage.ClickTurnOffBtn();

			// Проверить количество терминов
			Assert.IsTrue(GlossaryPage.GetConceptCount() > 0,
				"Ошибка: количество терминов должно быть больше 0 (термин не сохранился)");
		}

		/// <summary>
		/// Метод тестирования создания существующего термина
		/// </summary>
		[Test]
		public void CreateExistingItemTest()
		{
			Logger.Info("Начало работы теста CreateExistingItemTest().");

			// Создать глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());
			var uniqueTerm = "TestTermText" + DateTime.Now;

			// Создать термин
			CreateItemAndSave(uniqueTerm, uniqueTerm);
			// Создать такой же термин
			CreateItemAndSave(uniqueTerm, uniqueTerm, shouldSaveOk: false);
			
			// Проверить, что появилось предупреждение
			GlossaryPage.AssertionDuplicateErrorAppear();
		}

		/// <summary>
		/// Метод тестирования создания пустого термина
		/// </summary>
		[Test]
		public void CreateEmptyItemTest()
		{
			Logger.Info("Начало работы теста CreateEmptyItemTest().");

			// Создать глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());
			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			// Дождаться появления строки для ввода
			GlossaryPage.AssertionConceptTableAppear();
			// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();
			// Нажать Сохранить
			GlossaryPage.ClickSaveTermin();

			// Проверить, что появилось предупреждение
			GlossaryPage.AssertionIsGlossaryErrorExist();
		}

		/// <summary>
		/// Метод тестирования создания термина с синонимами
		/// </summary>
		[Test]
		public void CreateItemSynonymsTest()
		{
			Logger.Info("Начало работы теста CreateItemSynonymsTest().");

			// Создать глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());
			var term1 = "Term1";
			var term2 = "Term2";

			// Открыть форму добавления термина и заполнить поля
			FillCreateItem(term1, term2);

			// Нажать добавить синоним для первого слова
			GlossaryPage.ClickAddSynonym(1);
			term1 += DateTime.Now;
			// Ввести синоним
			GlossaryPage.FillSynonymTermLanguage(1, term1);

			// Нажать добавить синоним для второго слова
			GlossaryPage.ClickAddSynonym(2);
			term2 += DateTime.Now;
			// Ввести синоним
			GlossaryPage.FillSynonymTermLanguage(2, term2);

			// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();
			// Нажать Сохранить
			GlossaryPage.ClickSaveTermin();
			GlossaryPage.AssertionConceptGeneralSave();

			// Проверить количество терминов
			Assert.IsTrue(GlossaryPage.GetConceptCount() > 0, "Ошибка: количество терминов должно быть больше 0 (термин не сохранился)");
		}

		/// <summary>
		/// Метод тестирования создания термина с одинаковыми синонимами
		/// </summary>
		[Test]
		public void CreateItemEqualSynonymsTest()
		{
			Logger.Info("Начало работы теста CreateItemEqualSynonymsTest().");

			// Создать глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());
			var term1 = "Term1";
			var term2 = "Term2";

			// Открыть форму добавления термина и заполнить поля
			FillCreateItem(term1, term2);

			// Нажать добавить синоним для первого слова
			GlossaryPage.ClickAddSynonym(1);
			term1 += DateTime.Now;
			// Ввести синоним
			GlossaryPage.FillSynonymTermLanguage(1, term1);

			// Нажать добавить синоним для второго слова
			GlossaryPage.ClickAddSynonym(2);
			// Ввести синоним
			GlossaryPage.FillSynonymTermLanguage(2, term2);
			
			// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();
			// Нажать Сохранить
			GlossaryPage.ClickSaveTermin();
			Thread.Sleep(1000);

			// Проверить, что поля отмечены красным
			GlossaryPage.AssertionIsTermErrorExist(2);
			GlossaryPage.AssertionIsTermErrorMessageExist(2);
		}

		/// <summary>
		/// Метод тестирования удаления Термина
		/// </summary>
		[Test]
		public void DeleteItemTest()
		{
			Logger.Info("Начало работы теста DeleteItemTest().");

			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Создать термин
			CreateItemAndSave();
			var itemsCount = GlossaryPage.GetConceptCount();

			// Расширить окно, чтобы "корзинка" была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();
			// Выделить ячейку, чтобы "корзинка" появилась
			GlossaryPage.ClickTermRow();
			// Нажать на "корзинку"
			GlossaryPage.ClickDeleteBtn();
			GlossaryPage.AssertionConceptGeneralDelete();

			// Сравнить количество терминов
			Assert.IsTrue(GlossaryPage.GetConceptCount() < itemsCount, 
				"Ошибка: количество терминов не уменьшилось");
		}

		/// <summary>
		/// Метод тестирования кнопки отмены создания термина
		/// </summary>
		[Test]
		public void CancelCreateItemTest()
		{
			Logger.Info("Начало работы теста CancelCreateItemTest().");

			// Создать глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());
			// Открыть форму Создание термина и заполнить ее
			FillCreateItem();
			// Отменить
			GlossaryPage.ClickCancelEditBtn();

			// Проверить, что количество терминов равно нулю
			Assert.IsTrue(GlossaryPage.GetConceptCount() == 0, 
				"Ошибка: количество терминов должно быть равно 0");
		}

		/// <summary>
		/// Метод тестирования поиска термина в глоссарии по слову из первого языка
		/// </summary>
		[Test]
		public void SearchItemGlossaryFirstLangTest()
		{
			Logger.Info("Начало работы теста SearchItemGlossaryFirstLangTest().");

			// Создать глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());
			var uniqueData = DateTime.UtcNow.Ticks + "1Term";
			var firstTerm = "Test First Term " + uniqueData;
			var secondTerm = "Test Second Term " + DateTime.UtcNow;
			// Создать термин
			CreateItemAndSave(firstTerm, secondTerm);
			// Создать другой термин
			CreateItemAndSave();

			// Инициировать поиск по уникальному слову в первом термине
			GlossaryPage.FillSearchField(uniqueData);
			GlossaryPage.ClickSearchBtn();
			// Дождаться окончания поиска
			Thread.Sleep(2000);
			var itemCountAfter = GlossaryPage.GetConceptCount();

			// Проверить, что найден только один термин
			Assert.IsTrue(itemCountAfter == 1, 
				"Ошибка: должен быть найден только один термин");

			// Проверить, что показан нужный термин
			Assert.AreEqual(
				firstTerm, 
				GlossaryPage.GetFirstTermText(), 
				"Ошибка: найден неправильный термин");
		}

		/// <summary>
		/// Метод тестирования поиска термина в глоссарии по слову из второго языка
		/// </summary>
		[Test]
		public void SearchItemGlossarySecondLangTest()
		{
			Logger.Info("Начало работы теста SearchItemGlossarySecondLangTest()");

			// Создать глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());
			var uniqueData = DateTime.UtcNow.Ticks + "2Term";
			var firstTerm = "Test First Term " + DateTime.UtcNow;
			var secondTerm = "Test Second Term " + uniqueData;

			// Создать термин
			CreateItemAndSave(firstTerm, secondTerm);
			// Создать другой термин
			CreateItemAndSave();

			// Инициировать поиск по уникальному слову в первом термине
			GlossaryPage.FillSearchField(uniqueData);
			GlossaryPage.ClickSearchBtn();
			// Дождаться окончания поиска
			Thread.Sleep(2000);
			var itemCountAfter = GlossaryPage.GetConceptCount();

			// Проверить, что найден только один термин
			Assert.IsTrue(itemCountAfter == 1, 
				"Ошибка: должен быть найден только один термин");

			// Проверить, что показан нужный термин
			Assert.AreEqual(
				firstTerm, 
				GlossaryPage.GetFirstTermText(), 
				"Ошибка: найден неправильный термин");
		}

		/// <summary>
		/// Метод тестирования поиска термина из вкладки Поиска
		/// </summary>
		[Test]
		public void SearchItemSearchTabTest()
		{
			Logger.Info("Начало работы теста SearchItemSearchTabTest()");

			// Создать глоссарий
			var firstGlossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(firstGlossaryName);

			var uniqueData = DateTime.UtcNow.Ticks + "SearchTest";
			var firstTerm = "Test First Term " + uniqueData;
			var secondTerm = "Test Second Term ";

			// Создать термин
			CreateItemAndSave(firstTerm, secondTerm + DateTime.UtcNow.Ticks);

			// Перейти на вкладку Глоссарии
			SwitchGlossaryTab();

			// Создать глоссарий
			var secondGlossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(secondGlossaryName);
			// Создать термин
			CreateItemAndSave(firstTerm, secondTerm + DateTime.UtcNow.Ticks);
			// Перейти на вкладку Поиск
			SwitchSearchTab();
			// Найти перевод слова
			InitSearch(uniqueData);

			var glossaryList = new List<string>
			{
				firstGlossaryName, 
				secondGlossaryName
			};

			var glossaryNames = SearchPage.GetGlossaryResultNames();

			// Проверить, что найдено два термина
			Assert.IsTrue(
				glossaryNames.Count == glossaryList.Count, 
				"Ошибка: поиск должен найти только два результата");
			Assert.IsTrue(
				glossaryNames[0].Contains(glossaryList[0]), 
				"Ошибка: в списке нет глоссария:\n" + glossaryList[0]);
			Assert.IsTrue(
				glossaryNames[1].Contains(glossaryList[1]), 
				"Ошибка: в списке нет глоссария:\n" + glossaryList[1]);

			for (var i = 0; i < glossaryNames.Count; ++i)
			{
				// Получить найденный термин
				var itemText = SearchPage.GetGlossaryResultSrcText(i + 1);
				
				// Проверить, что найден и правильный глоссарий, и правильный термин
				Assert.IsTrue(itemText == firstTerm, 
					"Ошибка: найден неправильный термин (" + (i + 1) + "-й найденный результат)");
			}
		}

		/// <summary>
		/// Тест: создать термин с несколькими языками
		/// </summary>
		[Test]
		public void CreateItemMultiLanguageGlossary()
		{
			Logger.Info("Начало работы теста CreateItemMultiLanguageGlossary()");

			// Имя глоссария
			var glossaryName = "TestGlossary" + DateTime.Now.Ticks;
			// Список языков
			var langList = new List<CommonHelper.LANGUAGE>
			{
				CommonHelper.LANGUAGE.German,
				CommonHelper.LANGUAGE.French,
				CommonHelper.LANGUAGE.Japanese,
				CommonHelper.LANGUAGE.Lithuanian
			};

			// Создать глоссарий
			CreateGlossaryByName(glossaryName, true, langList);

			// Создать термин
			CreateItemExtended();

			// Свернуть
			GlossaryPage.ClickTurnOffBtn();

			// Проверить количество терминов
			Assert.IsTrue(GlossaryPage.GetConceptCount() > 0, 
				"Ошибка: количество терминов должно быть больше 0 (термин не сохранился)");

			// Удалить глоссарий, чтобы не было глоссария с многими языками
			DeleteGlossary();
		}

		/// <summary>
		/// Тест: изменение термина (обычный режим)
		/// </summary>
		[Test]
		public void EditItemGeneral()
		{
			Logger.Info("Начало работы теста EditItemGeneral()");

			// Создать глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());
			// Создать термин
			CreateItemAndSave();
			// Нажать на строку с термином
			GlossaryPage.ClickTermRow();
			// Нажать Редактировать
			GlossaryPage.ClickEditTermBtn();

			var newTermText = "New Term " + DateTime.Now;
			GlossaryPage.FillTermGeneralMode(newTermText);
			GlossaryPage.ClickSaveTermin();

			// Проверить, что термин сохранился
			GlossaryPage.AssertionConceptGeneralSave();
			// Проверить, что термин сохранился с новым значением
			AssertionIsTermTextExist(newTermText);
		}

		/// <summary>
		/// Тест: изменение термина (расширенный режим)
		/// </summary>
		[Test]
		public void EditItemExtended()
		{
			Logger.Info("Начало работы теста EditItemExtended()");

			// Имя глоссария
			var glossaryName = "TestGlossary" + DateTime.Now.Ticks;
			
			// Список языков
			var langList = new List<CommonHelper.LANGUAGE>
			{
				CommonHelper.LANGUAGE.German,
				CommonHelper.LANGUAGE.French,
				CommonHelper.LANGUAGE.Japanese,
				CommonHelper.LANGUAGE.Lithuanian
			};

			// Создать глоссарий
			CreateGlossaryByName(glossaryName, true, langList);
			// Создать термин
			CreateItemExtended();
			// Нажать Редактировать
			GlossaryPage.ClickEditBtn();

			var newTermText = "New Term " + DateTime.Now;

			EditAllExtendedItems(newTermText);
			// Сохранить
			GlossaryPage.ClickSaveExtendedConcept();
			// Проверить, что термин сохранился
			GlossaryPage.AssertionConceptSave();
			// Свернуть
			GlossaryPage.ClickTurnOffBtn();
			// Проверить, что термин сохранился с новым значением
			AssertionIsTermTextExist(newTermText);
			// Удалить глоссарий
			DeleteGlossary();
		}

		protected void AssertionIsTermTextExist(string termText)
		{
			Logger.Trace(string.Format("Проверить, что существует термин с текстом {0}", termText));

			Assert.IsTrue(
				GlossaryPage.GetIsTermExistByText(termText),
				"Ошибка: термин не сохранил изменения");
		}

		protected void CreateItemExtended()
		{
			Logger.Trace("Создание термина в расширенном режиме");

			// Изменить структуру для перехода в расширенный режим
			EditGlossaryStructureAddField();
			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			// Заполнить поля с терминами
			FillNewItemExtended();
			// Сохранить
			GlossaryPage.ClickSaveExtendedConcept();
			GlossaryPage.AssertionConceptSave();
		}
	}
}
