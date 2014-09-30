using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки терминов глоссария
	/// </summary>
	public class GlossarySuggestedTermsTest : GlossaryTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Названеи браузера</param>
		public GlossarySuggestedTermsTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Предварительная подготовка группы тестов
		/// </summary>
		[SetUp]
		public void Setup()
		{
		}

		/// <summary>
		/// Метод тестирования предложения термина без указания глоссария со страницы со списком глоссариев
		/// </summary>
		[Test]
		public void SuggestWithoutGlossaryFromGlossaryListTest()
		{
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без указанного глоссария
			int unglossaryTermsCountBefore = GetCountSuggestTermsWithoutGlossary();
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Предложить термин
			CreateSuggestTerm();

			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без глоссария
			int unglossaryTermsCountAfter = GetCountSuggestTermsWithoutGlossary();
			// Проверить, что таких терминов стало больше
			Assert.IsTrue(unglossaryTermsCountAfter > unglossaryTermsCountBefore, "Ошибка: предложенный термин не сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения термина для глоссария со страницы со списком глоссариев
		/// </summary>
		[Test]
		public void SuggestWithGlossaryFromGlossaryListTest()
		{
			// Создать глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Предложить термин для этого глоссария
			SuggestTermSetGlossary(glossaryName);

			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Проверить, что терминов для этого глоссария больше нуля
			Assert.IsTrue(GetCountSuggestTermsGlossary(glossaryName) > 0, "Ошибка: нет предложенного термина для этого глоссария");
		}

		/// <summary>
		/// Метод тестирования предложения термина со страницы глоссария
		/// </summary>
		[Test]
		public void SuggestWithGlossaryFromGlossaryTest()
		{
			// Создать глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);
			// Предложить термин
			CreateSuggestTerm();

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти к предложенным терминам
			SwitchSuggestTermsTab();
			// Проверить, что терминов для этого глоссария больше нуля
			Assert.IsTrue(GetCountSuggestTermsGlossary(glossaryName) > 0, "Ошибка: нет предложенного термина для этого глоссария");
		}

		/// <summary>
		/// Метод тестирования предложения термина со страницы другого глоссария
		/// </summary>
		[Test]
		public void SuggestWithGlossaryFromAnotherGlossaryTest()
		{
			// Создать один глоссарий
			string firstGlossaryName = CreateGlossaryAndReturnToGlossaryList();
			// Создать другой глоссарий
			string secondGlossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(secondGlossaryName);

			// Предложить термин для первого глоссария на странице второго глоссария
			SuggestTermSetGlossary(firstGlossaryName);

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Проверить, что терминов для первого глоссария больше нуля
			Assert.IsTrue(GetCountSuggestTermsGlossary(firstGlossaryName) > 0, "Ошибка: нет предложенного термина для первого глоссария");
		}

		/// <summary>
		/// Метод тестирования предложения термина со страницы глоссария без привязки к глоссарию
		/// </summary>
		[Test]
		public void SuggestWithoutGlossaryFromAnotherGlossaryTest()
		{
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без указанного глоссария
			int unglossaryTermsCountBefore = GetCountSuggestTermsWithoutGlossary();

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Создать  глоссарий
			CreateGlossaryByName(GetUniqueGlossaryNameF());

			// Предложить термин с отсутствием глоссария
			SuggestTermSetGlossary("");

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без указанного глоссария
			int unglossaryTermsCountAfter = GetCountSuggestTermsWithoutGlossary();
			// Проверить, что количество терминов без глоссария увеличилось
			Assert.IsTrue(unglossaryTermsCountAfter > unglossaryTermsCountBefore, "Ошибка: термин без указанного глоссария не сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения существующего термина со страницы глоссария, проверка появления предупреждения
		/// </summary>
		[Test]
		public void SuggestExistingTermWarningFromGlossaryTest()
		{
			// Создать  глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);

			string uniquePrefix = DateTime.Now.ToString();
			string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
			string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
			// Создать термин
			CreateItemAndSave(SuggestTerm1, SuggestTerm2);
			// Предложить термин
			SuggestTermAndSave(SuggestTerm1, SuggestTerm2);
			Thread.Sleep(2000);
			// Проверить, что появилось предупреждение
			Assert.IsTrue(SuggestTermDialog.GetIsExistDuplicateWarning(),
				"Ошибка: не появилось предупреждение о существующем термине");
			// Нажать отмену
			SuggestTermDialog.ClickCancel();
			// Перейти в предложенные термины
			SwitchSuggestTermsTab();
			// Проверить, что нет предложенных терминов
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCount() == 0, "Ошибка: предложенный термин сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения существующего термина со страницы глоссария, одобрение
		/// </summary>
		[Test]
		public void SuggestExistingTermAcceptFromGlossaryTest()
		{
			// Создать  глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);

			string uniquePrefix = DateTime.Now.ToString();
			string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
			string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
			// Создать термин
			CreateItemAndSave(SuggestTerm1, SuggestTerm2);
			// Предложить термин
			SuggestTermAndSave(SuggestTerm1, SuggestTerm2);
			Thread.Sleep(2000);
			// Согласиться
			SuggestTermDialog.ClickSave();
			Thread.Sleep(2000);
			// Перейти в предложенные термины
			SwitchSuggestTermsTab();
			Thread.Sleep(2000);
			// Проверить, что предложенный термин сохранился
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCount() > 0, "Ошибка: предложенный термин не сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения существующего термина из списка глоссариев, проверка появления предупреждения
		/// </summary>
		[Test]
		public void SuggestExistingTermWarningFromGlossaryListTest()
		{
			// Создать  глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);

			string uniquePrefix = DateTime.Now.ToString();
			string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
			string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
			// Создать термин
			CreateItemAndSave(SuggestTerm1, SuggestTerm2);

			// Перейти в список глоссариев
			SwitchGlossaryTab();

			// Предложить термин  с указанием глоссария
			SuggestTermAndSave(SuggestTerm1, SuggestTerm2, true, glossaryName);
			Thread.Sleep(2000);
			// Проверить, что появилось предупреждение
			Assert.IsTrue(SuggestTermDialog.GetIsExistDuplicateWarning(),
				"Ошибка: не появилось предупреждение о существующем термине");
			// Нажать отмену
			SuggestTermDialog.ClickCancel();
			Thread.Sleep(2000);
			// Перейти в глоссарий
			SwitchCurrentGlossaryF(glossaryName);
			Thread.Sleep(2000);
			// Перейти в предложенные термины
			SwitchSuggestTermsTab();
			// Проверить, что нет предложенных терминов
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCount() == 0, "Ошибка: предложенный термин сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения существующего термина из списка глоссариев, одобрение
		/// </summary>
		[Test]
		public void SuggestExistingTermAcceptFromGlossaryListTest()
		{
			// Создать  глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);

			string uniquePrefix = DateTime.Now.ToString();
			string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
			string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
			// Создать термин
			CreateItemAndSave(SuggestTerm1, SuggestTerm2);

			// Перейти в список глоссариев
			SwitchGlossaryTab();
			// Предложить термин  с указанием глоссария
			SuggestTermAndSave(SuggestTerm1, SuggestTerm2, true, glossaryName);
			Thread.Sleep(2000);
			// Согласиться
			SuggestTermDialog.ClickSave();
			Thread.Sleep(2000);
			// Перейти в глоссарий
			SwitchCurrentGlossaryF(glossaryName);
			Thread.Sleep(2000);
			// Перейти в предложенные термины
			SwitchSuggestTermsTab();
			// Проверить, что предложенный термин сохранился
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCount() > 0, "Ошибка: предложенный термин не сохранился");
		}

		/// <summary>
		/// Метод тестирования одобрения термина с указанным глоссарием со страницы со списком глоссариев
		/// </summary>
		[Test]
		public void AcceptWithGlossaryFromGlossaryListTest()
		{
			// Создать  глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);
			// Предложить термин
			CreateSuggestTerm();

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// В строке с термином для текущего глоссария нажать "Принять"
			ClickButtonSuggestTermRowByGlossary(glossaryName, GlossarySuggestPageHelper.BUTTON_ID.AcceptSuggestTerm);

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти в глоссарий
			SwitchCurrentGlossaryF(glossaryName);

			// Проверить количество терминов
			Assert.IsTrue(GetCountOfItemsGP() > 0, "Ошибка: термин не добавился");
		}

		/// <summary>
		/// Метод тестирования одобрения термина с указанным глоссарием со страницы этого глоссария
		/// </summary>
		[Test]
		public void AcceptWithGlossaryFromGlossaryTest()
		{
			// Создать  глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);
			// Предложить термин
			CreateSuggestTerm();
			// Перейти в предложенные термины для этого глоссария
			SwitchSuggestTermsTab();
			// Одобрить термин
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.AcceptSuggestTerm);
			// Перейти в этот глоссарий
			SwitchGlossaryFromSuggestedTerm();

			// Проверить количество терминов
			Assert.IsTrue(GetCountOfItemsGP() > 0, "Ошибка: термин не добавился");
		}

		/// <summary>
		/// Метод тестирования одобрения термина с указанным глоссарием со страницы другого глоссария
		/// </summary>
		[Test]
		public void AcceptWithGlossaryFromAnotherGlossaryTest()
		{
			// Создать глоссарий
			string glossaryNameWithSuggestTerm = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryNameWithSuggestTerm);
			// Предложить термин
			CreateSuggestTerm();

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Создать другой глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);
			// Перейти в предложенные термины для этого глоссария
			SwitchSuggestTermsTab();

			// Открыть выпадающий список с выбором глоссария
			GlossarySuggestPage.ClickDropdown();
			GlossarySuggestPage.SelectDropdownItem(glossaryNameWithSuggestTerm);
			Thread.Sleep(500);

			// Одобрить термин
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.AcceptSuggestTerm);
			
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти в наш первый глоссарий
			SwitchCurrentGlossaryF(glossaryNameWithSuggestTerm);

			// Проверить количество терминов
			Assert.IsTrue(GetCountOfItemsGP() > 0, "Ошибка: термин не добавился");
		}

		/// <summary>
		/// Метод тестирования одобрения термина без глоссария со страницы со списком глоссариев
		/// </summary>
		[Test]
		public void AcceptWithoutGlossaryFromGlossaryListTest()
		{
			// Создать глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();

			// Предложить термин
			CreateSuggestTerm();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();

		   // Найти термин без указанного глоссария
			int termNumber = GlossarySuggestPage.GetTermRowNumberByGlossaryName("");
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.AcceptSuggestTerm, termNumber);
			// Дождаться появления формы выбора глоссария
			GlossarySuggestPage.WaitChooseGlossaryForm();
			// Выбрать нужный глоссарий
			GlossarySuggestPage.ClickChooseGlossaryFormDropdownGlossaryList();
			Console.WriteLine("выбрали глоссарий");
			GlossarySuggestPage.SelectDropdownItem(glossaryName);
			// Сохранить
			GlossarySuggestPage.ClickOkChooseGlossary();
			// TODO никак не убрать
			Thread.Sleep(5000);

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти в глоссарий
			SwitchCurrentGlossaryF(glossaryName);

			// Проверить количество терминов
			Assert.IsTrue(GetCountOfItemsGP() > 0, "Ошибка: термин не добавился");
		}

		/// <summary>
		/// Метод тестирования удаления предложенного термина без глоссария
		/// </summary>
		[Test]
		public void DeleteWithoutGlossaryTest()
		{
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без указанного глоссария
			int unglossaryTermsCount = GetCountSuggestTermsWithoutGlossary();
			if (unglossaryTermsCount == 0)
			{
				// Перейти к списку глоссариев
				SwitchGlossaryTab();
				// Предложить термин
				CreateSuggestTerm();
				// Перейти к списку предложенных терминов
				SwitchSuggestTermsTab();
				// Получить количество терминов без указанного глоссария
				unglossaryTermsCount = GetCountSuggestTermsWithoutGlossary();
			}

			// Удалить термин без указанного глоссария
			int termNumber = GlossarySuggestPage.GetTermRowNumberByGlossaryName("");
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.RejectSuggestTerm, termNumber);
			Thread.Sleep(2000);
			// Проверить количество терминов без указанного глоссария
			int unglossaryTermsCountAfter = GetCountSuggestTermsWithoutGlossary();
			Assert.IsTrue(unglossaryTermsCountAfter < unglossaryTermsCount, "Ошибка: предложенный термин не удалился");
		}

		/// <summary>
		/// Метод тестирования удаления предложенного термина для глоссария
		/// </summary>
		[Test]
		public void DeleteWithGlossaryTest()
		{
			// Создать глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);
			// Предложить термин
			CreateSuggestTerm();

			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Удалить термин
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.RejectSuggestTerm);

			// Проверить количество предложенных терминов глоссария
			int count = GlossarySuggestPage.GetSuggestTermsCount();
			Assert.IsTrue(count == 0, "Ошибка: термин не удалился");
		}

		/// <summary>
		/// Метод тестирования редактирования предложенного термина из глоссария
		/// </summary>
		[Test]
		public void EditFromGlossaryTest()
		{
			// Предложить термин глоссарию и открыть редактирование
			SuggestWithGlossaryClickEdit();
			string newTermText = "New Term Text" + DateTime.UtcNow.ToString();

			// Ввести в термин новое значение
			GlossarySuggestPage.FillEditTermItem(1, newTermText);
			// Перейти в другому языку
			GlossarySuggestPage.ClickEditTermItem(2);
			GlossarySuggestPage.FillEditTermItem(2, newTermText);
			// Принять термин
			GlossarySuggestPage.ClickSaveEditTerm();
			GlossarySuggestPage.WaitUntilEditTermFillDisappear();
			// Перейти в глоссарий
			SwitchGlossaryFromSuggestedTerm();

			// Проверить термин в глоссарии
			Assert.IsTrue(GlossaryPage.GetIsExistTerm(newTermText), "Ошибка: термин не сохранился в глоссарии");
		}

		/// <summary>
		/// Метод тестирования редактирования предложенного термина из глоссария - добавление синонима
		/// </summary>
		[Test]
		public void EditFromGlossaryAddSynonymTest()
		{
			// Предложить термин глоссарию и открыть редактирование
			SuggestWithGlossaryClickEdit();
			string newTermText = "New Term Text" + DateTime.UtcNow.ToString();
			
			// Кликнуть добавить термин во второй язык
			GlossarySuggestPage.ClickAddSynonymEditTerm(2);
			GlossarySuggestPage.ClickAddSynonymEditTerm(2);
			GlossarySuggestPage.FillEditTermItem(2, newTermText);
			// Принять термин
			GlossarySuggestPage.ClickSaveEditTerm();
			Assert.IsTrue(GlossarySuggestPage.WaitUntilEditTermFillDisappear(), "Ошибка: предложенный термин не сохранился");
			// Перейти в глоссарий
			SwitchGlossaryFromSuggestedTerm();

			// Проверить термин в глоссарии
			Assert.IsTrue(GlossaryPage.GetIsExistTerm(newTermText), "Ошибка: термин не сохранился в глоссарии");
		}

		/// <summary>
		/// Метод тестирования редактирования предложенного термина без глоссария
		/// </summary>
		[Test]
		public void EditWithoutGlossaryTest()
		{
			// Создать глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Предложить термин
			CreateSuggestTerm();

			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Создать новое имя термина
			string newTermText = "New Term Text" + DateTime.UtcNow.ToString();

			// Получить номер строчки с термином без указанного глоссария
			int rowNumber = GlossarySuggestPage.GetTermRowNumberByGlossaryName("");
			// Проверить, что термин есть
			Assert.IsTrue(rowNumber > 0, "Ошибка: термин не предложился (глоссарий: " + glossaryName + ")");

			// Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
			Driver.Manage().Window.Maximize();
			// Нажать на редактирование
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.EditSuggestTerm, rowNumber);

			// Дождаться появления формы выбора глоссария
			GlossarySuggestPage.WaitChooseGlossaryForm();
			GlossarySuggestPage.ClickChooseGlossaryFormDropdownGlossaryList();
			
			// Выбрать нужный глоссарий
			GlossarySuggestPage.SelectDropdownItem(glossaryName);
			// Сохранить
			GlossarySuggestPage.ClickOkChooseGlossary();
			GlossarySuggestPage.WaitEditTermFillAppear();

			// Ввести в термин новое значение
			GlossarySuggestPage.FillEditTermItem(1, newTermText);
			// Перейти в другому языку
			GlossarySuggestPage.ClickEditTermItem(2);
			GlossarySuggestPage.FillEditTermItem(2, newTermText);
			// Принять термин
			GlossarySuggestPage.ClickSaveEditTerm();
			GlossarySuggestPage.WaitUntilEditTermFillDisappear();
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти в глоссарий
			SwitchCurrentGlossaryF(glossaryName);

			// Проверить термин в глоссарии
			Assert.IsTrue(GlossaryPage.GetIsExistTerm(newTermText), "Ошибка: термин не сохранился в глоссарии");
		}

		/// <summary>
		/// Тест: проверка автоматического переключения языков
		/// Создается глоссарий, открывается форма предложения термина
		/// Для первого термина открывается выпадающий список языков
		/// Проверяется, что там два языка: английский и русский
		/// Выбирается Русский
		/// Проверяется, что для второго термина язык меняется на Английский
		/// </summary>
		[Test]
		public void AutoSwitchingLanguageTest()
		{
			// Создать глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);

			// Нажать Предложить термин
			GlossaryListPage.ClickAddSuggest();
			SuggestTermDialog.WaitPageLoad();

			// Получить id языков
			string idFirstLang = SuggestTermDialog.GetLanguageId(1);
			string idSecondLang = SuggestTermDialog.GetLanguageId(2);
			
			// Кликнуть по English
			SuggestTermDialog.OpenLanguageList(1);

			// Выбрать Русский
			SuggestTermDialog.SelectLanguage(idSecondLang);

			// Проверить, что вторым языком стал Английский
			Assert.AreEqual(idFirstLang, SuggestTermDialog.GetLanguageId(2),
				"Ошибка: второй язык не изменился на английский");
		}

		/// <summary>
		/// Тест: предложение пустого термина (из списка глоссариев)
		/// Проверка: появляется ошибка
		/// </summary>
		[Test]
		public void SuggestEmptyTermFromGlossaryListTest()
		{
			// Нажать Предложить термин
			GlossaryListPage.ClickAddSuggest();
			SuggestTermDialog.WaitPageLoad();

			// Сохранить
			SuggestTermDialog.ClickSave();
			
			// Проверить, что появилась ошибка
			Assert.IsTrue(SuggestTermDialog.GetIsExistCreateTermError(),
				"Ошибка: не появилась ошибка пустого термина");
		}

		/// <summary>
		/// Тест: предложение пустого термина (из глоссария)
		/// Проверка: появляется ошибка
		/// </summary>
		[Test]
		public void SuggestEmptyTermFromGlossaryTest()
		{
			// Создать глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);

			// Нажать Предложить термин
			GlossaryListPage.ClickAddSuggest();
			SuggestTermDialog.WaitPageLoad();
			
			// Сохранить
			SuggestTermDialog.ClickSave();
			
			// Проверить, что появилась ошибка
			Assert.IsTrue(SuggestTermDialog.GetIsExistCreateTermError(),
				"Ошибка: не появилась ошибка пустого термина");
		}

		/// <summary>
		/// Не тест: перед выполнением остальных тестов нужно проверить права пользователя
		/// Если нет прав на предложение терминов без указания глоссария,
		/// добавить права
		/// </summary>
		[Test]
		public void aaaaaFirstTestCheckUserRights()
		{
			AddUserRights();
		}



		/// <summary>
		/// Кликнуть кнопку термина по имени глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <param name="btnId">id кнопки</param>
		protected void ClickButtonSuggestTermRowByGlossary(string glossaryName, GlossarySuggestPageHelper.BUTTON_ID btnId)
		{
			// Получить номер строки термина с нужным глоссарием
			int termNumber = GlossarySuggestPage.GetTermRowNumberByGlossaryName(glossaryName);
			// Проверить, что такой термин есть
			Assert.IsTrue(termNumber > 0, "Ошибка: нет термина с глоссарием " + glossaryName);
			// Кликнуть кнопку в этой строке
			ClickButtonSuggestTermRow(btnId, termNumber);
		}

		/// <summary>
		/// Кликнуть по кнопке в строке
		/// </summary>
		/// <param name="btnId">кнопка</param>
		/// <param name="rowNumber">строка</param>
		protected void ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID btnId, int rowNumber = 1)
		{
			int countBefore = GlossarySuggestPage.GetSuggestTermsCount();
			Console.WriteLine(countBefore);
			// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();
			
			// Нажать на строку
			GlossarySuggestPage.SelectRow(rowNumber);
			// Нажать кнопку
			GlossarySuggestPage.ClickRowButton(rowNumber, btnId);

			// Если не дождаться обновления списка предложенных терминов - появляется модальное окно с ошибкой, которое отлавливает Selenium
			if (GlossarySuggestPage.GetSuggestTermsCount() == countBefore)
			{
				Thread.Sleep(3000);
			}
		}

		/// <summary>
		/// Предложить термин с указанием глоссария
		/// </summary>
		/// <param name="glossaryName"></param>
		protected void SuggestTermSetGlossary(string glossaryName)
		{
			// Предложить термин и выбрать другой глоссарий
			SuggestTermAndSave("Suggest Term 1", "Suggest Term 2", true, glossaryName);
			SuggestTermDialog.WaitPageClose();
		}

		/// <summary>
		/// Предложить термин
		/// </summary>
		protected void CreateSuggestTerm()
		{
			// Предложить термин
			SuggestTermAndSave("Suggest Term 1", "Suggest Term 2");
			SuggestTermDialog.WaitPageClose();
		}

		/// <summary>
		/// Получить количество предложенных терминов без указанного глоссария
		/// </summary>
		/// <returns>количество</returns>
		protected int GetCountSuggestTermsWithoutGlossary()
		{
			return GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount("");
		}

		/// <summary>
		/// Вернуть количество терминов с указанным глоссарием
		/// </summary>
		/// <param name="glossaryName"></param>
		/// <returns></returns>
		protected int GetCountSuggestTermsGlossary(string glossaryName)
		{
			return GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount(glossaryName);
		}

		/// <summary>
		/// Перейти в текущий глоссарий из предложенных терминов
		/// </summary>
		protected void SwitchGlossaryFromSuggestedTerm()
		{
			// Перейти в глоссарий из его предложенных терминов
			GlossarySuggestPage.OpenCurrentGlossary();
			GlossaryPage.WaitPageLoad();
		}

		/// <summary>
		/// Перейти на страницу Предложенных терминов
		/// </summary>
		protected void SwitchSuggestTermsTab()
		{
			MainHelperClass.ClickOpenSuggestTermsPage();
			GlossarySuggestPage.WaitPageLoad();
		}

		/// <summary>
		/// Создать глоссарий, предложить термин, нажать редактирование
		/// </summary>
		protected void SuggestWithGlossaryClickEdit()
		{
			// Создать глоссарий
			string glossaryName = GetUniqueGlossaryNameF();
			CreateGlossaryByName(glossaryName);

			// Предложить термин
			CreateSuggestTerm();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();

			// Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
			Driver.Manage().Window.Maximize();
			// Нажать на редактирование
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.EditSuggestTerm);
		}
	}
}
