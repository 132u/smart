using System;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.Glossary.UserRights
{
	/// <summary>
	/// Группа тестов для проверки терминов глоссария
	/// </summary>
	[TestFixture]
	[Category("Standalone")]
	public class SuggestTermsTest<TWebDriverSettings> : GlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[TestFixtureSetUp]
		public void SetupGlossarySuggestedTermsTest()
		{
			Logger.Info("Начало работы метода SetupGlossarySuggestedTermsTest(). Подготовка перед каждым тест-сетом.");

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
		/// Метод тестирования предложения термина без указания глоссария со страницы со списком глоссариев
		/// </summary>
		[Test]
		public void SuggestWithoutGlossaryFromGlossaryListTest()
		{
			Logger.Info("Начало работы теста SuggestWithoutGlossaryFromGlossaryListTest().");

			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без указанного глоссария
			var unglossaryTermsCountBefore = GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount("");
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Предложить термин
			CreateSuggestTerm();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без глоссария
			var unglossaryTermsCountAfter = GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount("");

			// Проверить, что таких терминов стало больше
			Assert.IsTrue(unglossaryTermsCountAfter > unglossaryTermsCountBefore, 
				"Ошибка: предложенный термин не сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения термина для глоссария со страницы со списком глоссариев
		/// </summary>
		[Test]
		public void SuggestWithGlossaryFromGlossaryListTest()
		{
			Logger.Info("Начало работы теста SuggestWithGlossaryFromGlossaryListTest().");

			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Предложить термин для этого глоссария
			SuggestTermSetGlossary(glossaryName);
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Проверить, что терминов для этого глоссария больше нуля
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount(glossaryName) > 0,
				"Ошибка: нет предложенного термина для этого глоссария");
		}

		/// <summary>
		/// Метод тестирования предложения термина со страницы глоссария
		/// </summary>
		[Test]
		public void SuggestWithGlossaryFromGlossaryTest()
		{
			Logger.Info("Начало работы теста SuggestWithGlossaryFromGlossaryTest().");

			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);
			// Предложить термин
			CreateSuggestTerm();
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти к предложенным терминам
			SwitchSuggestTermsTab();

			// Проверить, что терминов для этого глоссария больше нуля
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount(glossaryName) > 0, 
				"Ошибка: нет предложенного термина для этого глоссария");
		}

		/// <summary>
		/// Метод тестирования предложения термина со страницы другого глоссария
		/// </summary>
		[Test]
		public void SuggestWithGlossaryFromAnotherGlossaryTest()
		{
			Logger.Info("Начало работы теста SuggestWithGlossaryFromAnotherGlossaryTest().");

			// Создать один глоссарий
			var firstGlossaryName = CreateGlossaryAndReturnToGlossaryList();

			// Создать другой глоссарий
			var secondGlossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(secondGlossaryName);
			// Предложить термин для первого глоссария на странице второго глоссария
			SuggestTermSetGlossary(firstGlossaryName);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();

			// Проверить, что терминов для первого глоссария больше нуля
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount(firstGlossaryName) > 0, 
				"Ошибка: нет предложенного термина для первого глоссария");
		}

		/// <summary>
		/// Метод тестирования предложения термина со страницы глоссария без привязки к глоссарию
		/// </summary>
		[Test]
		public void SuggestWithoutGlossaryFromAnotherGlossaryTest()
		{
			Logger.Info("Начало работы теста SuggestWithoutGlossaryFromAnotherGlossaryTest().");

			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без указанного глоссария
			var unglossaryTermsCountBefore = GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount("");
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Создать  глоссарий
			CreateGlossaryByName(GetUniqueGlossaryName());
			// Предложить термин с отсутствием глоссария
			SuggestTermSetGlossary("");
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без указанного глоссария
			var unglossaryTermsCountAfter = GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount("");

			// Проверить, что количество терминов без глоссария увеличилось
			Assert.IsTrue(unglossaryTermsCountAfter > unglossaryTermsCountBefore, 
				"Ошибка: термин без указанного глоссария не сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения существующего термина со страницы глоссария, проверка появления предупреждения
		/// </summary>
		[Test]
		public void SuggestExistingTermWarningFromGlossaryTest()
		{
			Logger.Info("Начало работы теста SuggestExistingTermWarningFromGlossaryTest().");

			// Создать  глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			var uniquePrefix = DateTime.Now.ToString();
			var SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
			var SuggestTerm2 = "Suggest Term 2" + uniquePrefix;

			// Создать термин
			CreateItemAndSave(SuggestTerm1, SuggestTerm2);
			// Предложить термин
			SuggestTermAndSave(SuggestTerm1, SuggestTerm2);
			Thread.Sleep(2000);

			// Проверить, что появилось предупреждение
			SuggestTermDialog.AssertionIsExistDuplicateWarning();

			// Нажать отмену
			SuggestTermDialog.ClickCancel();
			SuggestTermDialog.AssertSuggestTermDialogClosed();
			// Перейти в предложенные термины
			SwitchSuggestTermsTab();

			// Проверить, что нет предложенных терминов
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCount() == 0, 
				"Ошибка: предложенный термин сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения существующего термина со страницы глоссария, одобрение
		/// </summary>
		[Test]
		public void SuggestExistingTermAcceptFromGlossaryTest()
		{
			Logger.Info("Начало работы теста SuggestExistingTermAcceptFromGlossaryTest().");

			// Создать  глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			var uniquePrefix = DateTime.Now.ToString();
			var SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
			var SuggestTerm2 = "Suggest Term 2" + uniquePrefix;

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
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCount() > 0, 
				"Ошибка: предложенный термин не сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения существующего термина из списка глоссариев, проверка появления предупреждения
		/// </summary>
		[Test]
		public void SuggestExistingTermWarningFromGlossaryListTest()
		{
			Logger.Info("Начало работы теста SuggestExistingTermWarningFromGlossaryListTest().");

			// Создать  глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			var uniquePrefix = DateTime.Now.ToString();
			var SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
			var SuggestTerm2 = "Suggest Term 2" + uniquePrefix;

			// Создать термин
			CreateItemAndSave(SuggestTerm1, SuggestTerm2);

			// Перейти в список глоссариев
			SwitchGlossaryTab();

			// Предложить термин  с указанием глоссария
			SuggestTermAndSave(SuggestTerm1, SuggestTerm2, true, glossaryName);
			Thread.Sleep(2000);

			// Проверить, что появилось предупреждение
			SuggestTermDialog.AssertionIsExistDuplicateWarning();

			// Нажать отмену
			SuggestTermDialog.ClickCancel();
			Thread.Sleep(2000);
			// Перейти в глоссарий
			SwitchCurrentGlossary(glossaryName);
			Thread.Sleep(2000);
			// Перейти в предложенные термины
			SwitchSuggestTermsTab();

			// Проверить, что нет предложенных терминов
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCount() == 0, 
				"Ошибка: предложенный термин сохранился");
		}

		/// <summary>
		/// Метод тестирования предложения существующего термина из списка глоссариев, одобрение
		/// </summary>
		[Test]
		public void SuggestExistingTermAcceptFromGlossaryListTest()
		{
			Logger.Info("Начало работы теста SuggestExistingTermAcceptFromGlossaryListTest().");

			// Создать  глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			var uniquePrefix = DateTime.Now.ToString();
			var suggestTerm1 = "Suggest Term 1" + uniquePrefix;
			var suggestTerm2 = "Suggest Term 2" + uniquePrefix;

			// Создать термин
			CreateItemAndSave(suggestTerm1, suggestTerm2);
			// Перейти в список глоссариев
			SwitchGlossaryTab();
			// Предложить термин  с указанием глоссария
			SuggestTermAndSave(suggestTerm1, suggestTerm2, true, glossaryName);
			Thread.Sleep(2000);
			// Согласиться
			SuggestTermDialog.ClickSave();
			Thread.Sleep(2000);
			// Перейти в глоссарий
			SwitchCurrentGlossary(glossaryName);
			Thread.Sleep(2000);
			// Перейти в предложенные термины
			SwitchSuggestTermsTab();

			// Проверить, что предложенный термин сохранился
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCount() > 0, 
				"Ошибка: предложенный термин не сохранился");
		}

		/// <summary>
		/// Метод тестирования одобрения термина с указанным глоссарием со страницы со списком глоссариев
		/// </summary>
		[Test]
		public void AcceptWithGlossaryFromGlossaryListTest()
		{
			Logger.Info("Начало работы теста AcceptWithGlossaryFromGlossaryListTest().");

			// Создать  глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Предложить термин
			CreateSuggestTerm();
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// В строке с термином для текущего глоссария нажать "Принять"
			ClickButtonSuggestTermRowByGlossary(
				glossaryName, 
				GlossarySuggestPageHelper.BUTTON_ID.AcceptSuggestTerm);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти в глоссарий
			SwitchCurrentGlossary(glossaryName);

			// Проверить количество терминов
			Assert.IsTrue(GlossaryPage.GetConceptCount() > 0, "Ошибка: термин не добавился");
		}

		/// <summary>
		/// Метод тестирования одобрения термина с указанным глоссарием со страницы этого глоссария
		/// </summary>
		[Test]
		public void AcceptWithGlossaryFromGlossaryTest()
		{
			Logger.Info("Начало работы теста AcceptWithGlossaryFromGlossaryTest().");

			// Создать  глоссарий
			var glossaryName = GetUniqueGlossaryName();
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
			Assert.IsTrue(GlossaryPage.GetConceptCount() > 0, "Ошибка: термин не добавился");
		}

		/// <summary>
		/// Метод тестирования одобрения термина с указанным глоссарием со страницы другого глоссария
		/// </summary>
		[Test]
		public void AcceptWithGlossaryFromAnotherGlossaryTest()
		{
			Logger.Info("Начало работы теста AcceptWithGlossaryFromAnotherGlossaryTest().");

			// Создать глоссарий
			var glossaryNameWithSuggestTerm = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryNameWithSuggestTerm);
			// Предложить термин
			CreateSuggestTerm();
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Создать другой глоссарий
			var glossaryName = GetUniqueGlossaryName();
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
			SwitchCurrentGlossary(glossaryNameWithSuggestTerm);

			// Проверить количество терминов
			Assert.IsTrue(GlossaryPage.GetConceptCount() > 0, "Ошибка: термин не добавился");
		}

		/// <summary>
		/// Метод тестирования одобрения термина без глоссария со страницы со списком глоссариев
		/// </summary>
		[Test]
		public void AcceptWithoutGlossaryFromGlossaryListTest()
		{
			Logger.Info("Начало работы теста AcceptWithoutGlossaryFromGlossaryListTest().");

			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Предложить термин
			CreateSuggestTerm();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
		   // Найти термин без указанного глоссария
			var termNumber = GlossarySuggestPage.GetTermRowNumberByGlossaryName("");
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.AcceptSuggestTerm, termNumber);
			// Дождаться появления формы выбора глоссария
			GlossarySuggestPage.WaitChooseGlossaryForm();
			// Выбрать нужный глоссарий
			GlossarySuggestPage.ClickChooseGlossaryFormDropdownGlossaryList();
			GlossarySuggestPage.SelectDropdownItem(glossaryName);
			// Сохранить
			GlossarySuggestPage.ClickOkChooseGlossary();
			// TODO никак не убрать
			Thread.Sleep(5000);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти в глоссарий
			SwitchCurrentGlossary(glossaryName);

			// Проверить количество терминов
			Assert.IsTrue(GlossaryPage.GetConceptCount() > 0, "Ошибка: термин не добавился");
		}

		/// <summary>
		/// Метод тестирования удаления предложенного термина без глоссария
		/// </summary>
		[Test]
		public void DeleteWithoutGlossaryTest()
		{
			Logger.Info("Начало работы теста DeleteWithoutGlossaryTest().");

			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Получить количество терминов без указанного глоссария
			var unglossaryTermsCount = GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount("");

			if (unglossaryTermsCount == 0)
			{
				// Перейти к списку глоссариев
				SwitchGlossaryTab();
				// Предложить термин
				CreateSuggestTerm();
				// Перейти к списку предложенных терминов
				SwitchSuggestTermsTab();
				// Получить количество терминов без указанного глоссария
				unglossaryTermsCount = GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount("");
			}

			// Удалить термин без указанного глоссария
			var termNumber = GlossarySuggestPage.GetTermRowNumberByGlossaryName("");
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.RejectSuggestTerm, termNumber);
			Thread.Sleep(2000);

			// Проверить количество терминов без указанного глоссария
			var unglossaryTermsCountAfter = GlossarySuggestPage.GetSuggestTermsCurrentGlossaryCount("");
			Assert.IsTrue(unglossaryTermsCountAfter < unglossaryTermsCount, 
				"Ошибка: предложенный термин не удалился");
		}

		/// <summary>
		/// Метод тестирования удаления предложенного термина для глоссария
		/// </summary>
		[Test]
		public void DeleteWithGlossaryTest()
		{
			Logger.Info("Начало работы теста DeleteWithGlossaryTest().");

			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);
			// Предложить термин
			CreateSuggestTerm();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Удалить термин
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.RejectSuggestTerm);

			// Проверить количество предложенных терминов глоссария
			Assert.IsTrue(GlossarySuggestPage.GetSuggestTermsCount() == 0, 
				"Ошибка: термин не удалился");
		}

		/// <summary>
		/// Метод тестирования редактирования предложенного термина из глоссария
		/// </summary>
		[Test]
		public void EditFromGlossaryTest()
		{
			Logger.Info("Начало работы теста EditFromGlossaryTest().");

			// Предложить термин глоссарию и открыть редактирование
			SuggestWithGlossaryClickEdit();
			var newTermText = "New Term Text" + DateTime.UtcNow;

			// Ввести в термин новое значение
			GlossarySuggestPage.FillEditTermItem(1, newTermText);
			// Перейти в другому языку
			GlossarySuggestPage.ClickEditTermItem(2);
			GlossarySuggestPage.FillEditTermItem(2, newTermText);
			//Добавить термин 
			GlossarySuggestPage.ClickSaveTerm();
			GlossarySuggestPage.AssertionEditTermFillDisappear();
			// Перейти в глоссарий
			SwitchGlossaryFromSuggestedTerm();

			// Проверить термин в глоссарии
			Assert.IsTrue(
				GlossaryPage.GetIsExistTerm(newTermText), 
				"Ошибка: термин не сохранился в глоссарии");
		}

		/// <summary>
		/// Метод тестирования редактирования предложенного термина из глоссария - добавление синонима
		/// </summary>
		[Test]
		public void EditFromGlossaryAddSynonymTest()
		{
			Logger.Info("Начало работы теста EditFromGlossaryAddSynonymTest().");

			// Предложить термин глоссарию и открыть редактирование
			SuggestWithGlossaryClickEdit();
			var newTermText = "New Term Text" + DateTime.UtcNow;
			
			// Кликнуть добавить термин во второй язык
			GlossarySuggestPage.ClickAddSynonymEditTerm(2);
			GlossarySuggestPage.FillEditTermItem(2, newTermText);
			//Добавить термин
			GlossarySuggestPage.ClickSaveTerm();

			GlossarySuggestPage.AssertionEditTermFillDisappear();
			// Перейти в глоссарий
			SwitchGlossaryFromSuggestedTerm();

			// Проверить термин в глоссарии
			Assert.IsTrue(
				GlossaryPage.GetIsExistTerm(newTermText),
				"Ошибка: термин не сохранился в глоссарии");
		}

		/// <summary>
		/// Метод тестирования редактирования предложенного термина без глоссария
		/// </summary>
		[Test]
		public void EditWithoutGlossaryTest()
		{
			Logger.Info("Начало работы теста EditWithoutGlossaryTest().");

			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Предложить термин
			CreateSuggestTerm();
			// Перейти к списку предложенных терминов
			SwitchSuggestTermsTab();
			// Создать новое имя термина
			var newTermText = "New Term Text" + DateTime.UtcNow;
			// Получить номер строчки с термином без указанного глоссария
			var rowNumber = GlossarySuggestPage.GetTermRowNumberByGlossaryName("");

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
			GlossarySuggestPage.AssertionEditTermFillAppear();
			// Ввести в термин новое значение
			GlossarySuggestPage.FillEditTermItem(1, newTermText);
			// Перейти в другому языку
			GlossarySuggestPage.ClickAddSynonymEditTerm(2);
			GlossarySuggestPage.FillEditTermItem(2, newTermText);
			//Добавить термин
			GlossarySuggestPage.ClickSaveTerm();

			GlossarySuggestPage.AssertionEditTermFillDisappear();
			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Перейти в глоссарий
			SwitchCurrentGlossary(glossaryName);

			// Проверить термин в глоссарии
			Assert.IsTrue(
				GlossaryPage.GetIsExistTerm(newTermText), 
				"Ошибка: термин не сохранился в глоссарии");
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
			Logger.Info("Начало работы теста AutoSwitchingLanguageTest().");

			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Нажать Предложить термин
			GlossaryListPage.ClickAddSuggest();
			SuggestTermDialog.WaitPageLoad();

			// Получить id языков
			var idFirstLang = SuggestTermDialog.GetLanguageId(1);
			var idSecondLang = SuggestTermDialog.GetLanguageId(2);
			
			// Кликнуть по English
			SuggestTermDialog.OpenLanguageList(1);

			// Выбрать Русский
			SuggestTermDialog.SelectLanguage(idSecondLang);

			// Проверить, что вторым языком стал Английский
			Assert.AreEqual(
				idFirstLang, 
				SuggestTermDialog.GetLanguageId(2),
				"Ошибка: второй язык не изменился на английский");
		}

		/// <summary>
		/// Тест: предложение пустого термина (из списка глоссариев)
		/// Проверка: появляется ошибка
		/// </summary>
		[Test]
		public void SuggestEmptyTermFromGlossaryListTest()
		{
			Logger.Info("Начало работы теста SuggestEmptyTermFromGlossaryListTest().");

			// Нажать Предложить термин
			GlossaryListPage.ClickAddSuggest();
			SuggestTermDialog.WaitPageLoad();

			// Сохранить
			SuggestTermDialog.ClickSave();
			
			// Проверить, что появилась ошибка
			SuggestTermDialog.AssertionIsExistCreateTermError();
		}

		/// <summary>
		/// Тест: предложение пустого термина (из глоссария)
		/// Проверка: появляется ошибка
		/// </summary>
		[Test]
		public void SuggestEmptyTermFromGlossaryTest()
		{
			Logger.Info("Начало работы теста SuggestEmptyTermFromGlossaryTest().");

			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Нажать Предложить термин
			GlossaryListPage.ClickAddSuggest();
			SuggestTermDialog.WaitPageLoad();
			
			// Сохранить
			SuggestTermDialog.ClickSave();
			
			// Проверить, что появилась ошибка
			SuggestTermDialog.AssertionIsExistCreateTermError();
		}

		protected void ClickButtonSuggestTermRowByGlossary(string glossaryName, GlossarySuggestPageHelper.BUTTON_ID btnId)
		{
			Logger.Debug(string.Format("Кликнуть кнопку {0} термина по имени глоссария {1}", btnId, glossaryName));

			var termNumber = GlossarySuggestPage.GetTermRowNumberByGlossaryName(glossaryName);

			Assert.IsTrue(termNumber > 0, "Ошибка: нет термина с глоссарием " + glossaryName);

			// Кликнуть кнопку в этой строке
			ClickButtonSuggestTermRow(btnId, termNumber);
		}

		protected void ClickButtonSuggestTermRow(
			GlossarySuggestPageHelper.BUTTON_ID btnId, 
			int rowNumber = 1)
		{
			Logger.Debug(string.Format("Кликнуть по кнопке {0} в строке {1}", btnId, rowNumber));
			var countBefore = GlossarySuggestPage.GetSuggestTermsCount();

			Logger.Trace(countBefore);
			// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();
	
			// Нажать на строку
			GlossarySuggestPage.SelectRow(rowNumber);

			GlossarySuggestPage.ClickRowButton(rowNumber, btnId);

			// Если не дождаться обновления списка предложенных терминов - появляется модальное окно с ошибкой, которое отлавливает Selenium
			if (GlossarySuggestPage.GetSuggestTermsCount() == countBefore)
			{
				Thread.Sleep(3000);
			}
		}

		protected void SuggestTermSetGlossary(string glossaryName)
		{
			Logger.Debug(string.Format("Предложить термин с указанием глоссария {0}", glossaryName));

			SuggestTermAndSave("Suggest Term 1", "Suggest Term 2", true, glossaryName);
			SuggestTermDialog.WaitPageClose();
		}

		protected void CreateSuggestTerm()
		{
			Logger.Debug("Предложить термин");

			SuggestTermAndSave("Suggest Term 1", "Suggest Term 2");
			SuggestTermDialog.WaitPageClose();
		}

		protected void SwitchGlossaryFromSuggestedTerm()
		{
			Logger.Debug("Переход в текущий глоссарий из предложенных терминов");

			GlossarySuggestPage.OpenCurrentGlossary();
			GlossaryPage.WaitPageLoad();
		}

		protected void SwitchSuggestTermsTab()
		{
			Logger.Debug("Переход на страницу предложенных терминов");

			MainHelperClass.ClickOpenSuggestTermsPage();
			GlossarySuggestPage.WaitPageLoad();
		}

		protected void SuggestWithGlossaryClickEdit()
		{
			var glossaryName = GetUniqueGlossaryName();
			
			CreateGlossaryByName(glossaryName);
			CreateSuggestTerm();
			SwitchSuggestTermsTab();

			// Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
			Driver.Manage().Window.Maximize();

			// Нажать на редактирование
			ClickButtonSuggestTermRow(GlossarySuggestPageHelper.BUTTON_ID.EditSuggestTerm);
		}
	}
}
