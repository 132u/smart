using System;
using System.Threading;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Тесты глоссариев
	/// </summary>
	public class GlossaryTest : BaseTest
	{
		public GlossaryTest(string browserName)
			: base(browserName)
		{
		}
		
		[SetUp]
		public void SetupGlossary()
		{
			Logger.Info("Начало работы метода SetupGlossary. Подготовка перед каждым тест-сетом.");

			Logger.Debug("Значение параметра QuitDriverAfterTest = false. Не закрывать браузер после каждого теста.");
			QuitDriverAfterTest = false;

			GoToUrl(RelativeUrlProvider.Glossaries);
		}

		protected void OpenEditGlossaryStructure()
		{
			Logger.Debug("Открытие редактирования структуры");
			
			GlossaryPage.OpenEditGlossaryList();
			GlossaryPage.OpenEditStructureForm();
			GlossaryEditStructureForm.WaitPageLoad();
		}

		protected void EditGlossaryStructureAddField()
		{
			Logger.Debug("Добавить поле в структуру глоссария");

			OpenEditGlossaryStructure();

			GlossaryEditStructureForm.AssertionIsConceptTableDisplay();

			// Нажать на поле Domain
			if (GlossaryEditStructureForm.ClickFieldToAdd(GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Domain))
			{
				GlossaryEditStructureForm.ClickAddToListBtn();
			}

			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы
			GlossaryEditStructureForm.WaitFormClose();
			// Дождаться закрытия формы
			Thread.Sleep(1000);
		}

		protected void CreateItemAndSave(
			string firstTerm = "",
			string secondTerm = "", 
			bool shouldSaveOk = true)
		{
			Logger.Debug(string.Format("Создать и сохранить термин. Текст первого термина: {0}, текст второго термина: {1}, сохранять термин: {2}",
				firstTerm, secondTerm, shouldSaveOk));

			// Открыть форму добавления термина и заполнить поля
			FillCreateItem(firstTerm, secondTerm);
			// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();

			GlossaryPage.ClickSaveTermin();

			if (shouldSaveOk)
			{
				// TODO проверить!
				GlossaryPage.AssertionConceptGeneralSave();
			}
		}

		protected void FillCreateItem(string firstTerm = "", string secondTerm = "")
		{
			Logger.Debug(string.Format("Создать новый термин. Текст первого термина {0}, текст второго термина {1}", firstTerm, secondTerm));
			
			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			// Дождаться появления строки для ввода
			GlossaryPage.AssertionConceptTableAppear();

			// Заполнить термин
			if (firstTerm.Length == 0)
			{
				firstTerm = "Term First Language" + DateTime.Now;
			}

			GlossaryPage.FillTerm(1, firstTerm);

			if (secondTerm.Length == 0)
			{
				secondTerm = "Term Second Language" + DateTime.Now;
			}

			GlossaryPage.FillTerm(2, secondTerm);
		}

		/// <summary>
		/// Предложить термин и сохранить
		/// </summary>
		/// <param name="termFirst">текст первого термина</param>
		/// <param name="termSecond">текст второго термина</param>
		/// <param name="isNeedSelectGlossary">нужно ли выбирать глоссарий</param>
		/// <param name="glossaryName">название глоссария</param>
		protected void SuggestTermAndSave(
			string termFirst, 
			string termSecond, 
			bool isNeedSelectGlossary = false, 
			string glossaryName = "")
		{
			// Открыть форму предложения термина и заполнить полня
			SuggestTermFillTerms(termFirst, termSecond);

			// Проверяем языки En-Ru
			if (SuggestTermDialog.GetLanguageId(1) != "9")
			{
				SuggestTermDialog.OpenLanguageList(1);
				SuggestTermDialog.SelectLanguage("9");
			}

			if (SuggestTermDialog.GetLanguageId(2) != "25")
			{
				SuggestTermDialog.OpenLanguageList(2);
				SuggestTermDialog.SelectLanguage("25");
			}

			if (isNeedSelectGlossary)
			{
				// Открыть список с глоссариями
				SuggestTermDialog.OpenGlossaryList();
				// Выбрать наш первый созданный глоссарий в выпавшем списке
				SuggestTermDialog.SelectGlossary(glossaryName);
			}

			// Сохранить
			SuggestTermDialog.ClickSave();
			// TODO убрать sleep
			Thread.Sleep(500);
		}

		/// <summary>
		/// Заполнить термины
		/// </summary>
		/// <param name="suggestTermFirst">текст первого термина</param>
		/// <param name="suggestTermSecond">текст второго термина</param>
		protected void SuggestTermFillTerms(string suggestTermFirst, string suggestTermSecond)
		{
			// Нажать Предложить термин
			GlossaryListPage.ClickAddSuggest();
			SuggestTermDialog.WaitPageLoad();
			// Заполнить термин
			SuggestTermDialog.FillTerm(1, suggestTermFirst);
			SuggestTermDialog.FillTerm(2, suggestTermSecond);
		}

		protected void FillNewItemExtended()
		{
			Logger.Debug("Заполнить термин в расширенной форме");
			GlossaryPage.FillItemTermsExtended("Example Term Text " + DateTime.Now);
		}

		/// <summary>
		/// Изменить все термины в расширенной версии
		/// </summary>
		/// <param name="text">текст</param>
		protected void EditAllExtendedItems(string text)
		{
			GlossaryPage.EditTermsExtended(text);
		}
					  
		public void AddUserRights()
		{
			// хардкор!
			// Проверить, что есть кнопка Предложить термин на странице со списком глоссариев
			if (GlossaryListPage.GetIsAddSuggestExist())
			{
				Logger.Trace("Все хорошо - у пользователя права есть!");
				
				return;
			}
			// Иначе: добавляем права

			// Перейти в пользователи и права
			WorkspacePage.ClickUsersAndRightsBtn();

			// Ожидание открытия страницы
			UserRightsPage.AssertionUsersRightsPageDisplayed();

			// Перейти в группы
			UserRightsPage.OpenGroups();
			// Выбрать Administrators
			UserRightsPage.SelectAdmins();
			// Edit
			UserRightsPage.ClickEdit();
			// Add right
			UserRightsPage.ClickAddRights();
			// Suggest concepts without glossary
			UserRightsPage.SelectSuggestWithoutGlossary();
			// Next
			UserRightsPage.ClickNext();
			// Add
			UserRightsPage.ClickAdd();
			Thread.Sleep(1000);
			// Add right
			UserRightsPage.ClickAddRights();
			// Suggest concepts without glossary
			UserRightsPage.SelectGlossarySearch();
			// Next
			UserRightsPage.ClickNext();
			// All Glossaries
			UserRightsPage.SelectAllGlossaries();
			// Next
			UserRightsPage.ClickNext();
			// Add
			UserRightsPage.ClickAdd();
			Thread.Sleep(1000);
			// Save
			UserRightsPage.ClickSave();
			Thread.Sleep(1000);
			SwitchGlossaryTab();

			Assert.IsTrue(
				GlossaryListPage.GetIsAddSuggestExist(), 
				"ОШИБКА! Права не добавились.");
		}
	}
}
