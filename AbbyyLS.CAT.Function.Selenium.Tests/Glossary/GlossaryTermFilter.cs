using NUnit.Framework;
using System;
using System.Threading;
using System.Collections.Generic;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Glossary
{
	public class GlossaryTermFilter : BaseTest
	{
		/// <summary>
		/// Группа тестов для проверки фильтра терминов
		/// </summary>
		public GlossaryTermFilter(string browserName)
			: base(browserName)
		{

		}

		protected string _glossaryName;
		protected static DateTime _todayDate = DateTime.Today;
		protected string _today = _todayDate.ToString("M/d/yyyy").Replace(".", "/");
		protected string _weekAgo = _todayDate.AddDays(-6).ToString("M/d/yyyy").Replace(".", "/");
		protected string _monthAgo = _todayDate.AddMonths(-1).ToString("M/d/yyyy").Replace(".", "/");
		protected string _yearAgo = _todayDate.AddYears(-1).ToString("M/d/yyyy").Replace(".", "/");
		protected string _nextYear = _todayDate.AddYears(1).ToString("M/d/yyyy").Replace(".", "/");
		protected string _nextWeek = _todayDate.AddDays(6).ToString("M/d/yyyy").Replace(".", "/");

		[SetUp]
		public void Setup()
		{
			// Не закрывать браузер
			QuitDriverAfterTest = true;

			// Переходим к странице глоссариев
			GoToUrl(RelativeUrlProvider.Glossaries);
			var languagesList = new List<CommonHelper.LANGUAGE>{CommonHelper.LANGUAGE.Lithuanian, CommonHelper.LANGUAGE.Japanese};

			// Создать новый глоссарий
			_glossaryName = CreateGlossaryAndReturnToGlossaryList(languagesList);

			// Проверить, что глоссарий сохранился
			Assert.IsTrue(GlossaryListPage.GetIsExistGlossary(_glossaryName), "Ошибка: глоссарий не создался");

			// Перейти на стр словаря
			SwitchCurrentGlossary(_glossaryName);

			// Добавить термин в словарь
			GlossaryTermFilterPage.AddNewEntry();
		}

		/// <summary>
		/// В глоссарии в фильтре по умолчанию показывать все языки PRX-4795
		/// </summary>
		[Test]
		public void LangCheckboxesFilter()
		{
			// Кликнуть Filter кнопку
			Assert.IsTrue(GlossaryTermFilterPage.OpenFilterForm(), "Ошибка: Filter поп-ап не открылся");

			// Раскрыть дропдаун язков
			GlossaryTermFilterPage.ClickLangDropDownInFilterForm();

			// Проверить, что все языки выбраны
			Assert.IsTrue(GlossaryTermFilterPage.LangCheckboxesInFilterForm(), "Ошибка: в фильтре выбраны не все языки ");

		}

		/// <summary>
		/// ТС-211 Фильтрация по всем языкам
		/// </summary>
		[Test]
		public void AllLangFilter()
		{
			addLanguageToFilter(CommonHelper.LANGUAGE.German);
			addLanguageToFilter(CommonHelper.LANGUAGE.French);

			// Кликнуть Filter кнопку
			Assert.IsTrue(GlossaryTermFilterPage.OpenFilterForm(), "Ошибка: Filter поп-ап не открылся");

			// Получить список выбранных языков в фильтре
			List<string> selectedLanguages = GlossaryTermFilterPage.GetSelectedLangInFilterForm();

			// Раскрыть дропдаун язков
			GlossaryTermFilterPage.ClickLangDropDownInFilterForm();

			// Проверить, что все языки выбраны
			Assert.IsTrue(GlossaryTermFilterPage.LangCheckboxesInFilterForm(), "Ошибка: в фильтре выбраны не все языки ");

			// Кликнуть Apply кнопку
			GlossaryTermFilterPage.ClickApplyBtnInFilterForm();

			// Получить список колонок языков в таблице
			List<string> languagesColumns = GlossaryTermFilterPage.GetLangFromTable();

			// Сравнить список язков в фильтре и в таблице
			Assert.IsTrue(GlossaryTermFilterPage.CompareSelctedLangAndColumnNames(selectedLanguages, languagesColumns), 
				"Ошибка: языки, выбранные в фильтре и в таблице не совпадают");
		}

		/// <summary>
		/// ТС-212 Фильтрация по одному языку
		/// </summary>
		[Test]
		public void OneLangFilter()
		{
			Assert.IsTrue(GlossaryTermFilterPage.OpenFilterForm(), "Ошибка: Filter поп-ап не открылся");

			// Выбрать только один язык
			GlossaryTermFilterPage.CheckOnlyOneLanguage("English");

			// Получить список выбранных языков в фильтре
			List<string> selectedLanguages = GlossaryTermFilterPage.GetSelectedLangInFilterForm();

			// Просто инфа
			foreach (string lang in selectedLanguages)
			{
				Logger.Trace("В фильтре выбран: " + lang);
			}

			// Кликнуть Apply кнопку
			GlossaryTermFilterPage.ClickApplyBtnInFilterForm();

			// Получить список колонок зыков в таблице
			List<string> languagesColumns = GlossaryTermFilterPage.GetLangFromTable();

			// Просто инфа
			foreach (string langColumn in languagesColumns )
			{
				Logger.Trace("В таблице отображается: " + langColumn);
			}

			// Сравнить список язков в фильтре и в таблице
			Assert.IsTrue(GlossaryTermFilterPage.CompareSelctedLangAndColumnNames(selectedLanguages, languagesColumns),
				"Ошибка: языки, выбранные в фильтре и в таблице не совпадают");

		}

		/// <summary>
		/// ТС-213 Языки не выбраны
		/// </summary>
		[Test]
		public void EmptyLangBlockMsgFilter()
		{
			Assert.IsTrue(GlossaryTermFilterPage.OpenFilterForm(), "Ошибка: Filter поп-ап не открылся");

			// Раскрыть дропдаун языков
			GlossaryTermFilterPage.ClickLangDropDownInFilterForm();

			// Анчекнуть все языки в фильтре
			GlossaryTermFilterPage.UnCheckOnlyAllLanguagesInFilter();

			// Кликнуть Apply кнопку
			GlossaryTermFilterPage.ClickApplyBtnInFilterForm();

			// Проверить, что соответствующее сообщение появилось
			Assert.IsTrue(GlossaryTermFilterPage.GetBlockMsgIsDisplay(), "ОШибка: сообщение не появилось \"The Language field cannot be empty.\"");
		}

		/// <summary>
		/// ТС-214 Добавлен новый язык
		/// </summary>
		[Test]
		public void NewLangFilter()
		{
			// Получить кол-во языков в фильтре
			int countBefore = getSelectedLanguageCountInFilter();

			// Кликнуть Apply кнопку
			GlossaryTermFilterPage.ClickApplyBtnInFilterForm();

			// Добавить немецкий язык
			addLanguageToFilter(CommonHelper.LANGUAGE.German);

			// Открыть фильтр
			Assert.IsTrue(GlossaryTermFilterPage.OpenFilterForm(), "Ошибка: Filter поп-ап не открылся");

			// Получить кол-во языков в фильтре после добавления нового языка
			int countAfter = getSelectedLanguageCountInFilter();

			// Сравниить кол-ва языков фильтре
			Assert.IsTrue(countAfter == countBefore + 1,
				"Ошибка: новый язык не добавлен в фильтр");
		}

		/// <summary>
		/// ТС-215 Удаление языка
		/// </summary>
		[Test]
		public void DeletedLangFilter()
		{
			int countBefore = getSelectedLanguageCountInFilter();
			deleteLanguage();
			int countAfter = getSelectedLanguageCountInFilter();
			// Сравниить кол-ва языков фильтре
			Assert.IsTrue(countAfter == countBefore - 1,
				"Ошибка: язык не удалился из фильтра");
		}

		/// <summary>
		/// ТС-232 фильтрация по одному автору
		/// </summary>
		[Test]
		public void AuthorFilter()
		{
			// Зайти под первым пользователем и добавить термин в новый глоссари
			string[] termsFirstAuthor = addTerm();

			// Выйти
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			// Зайти под вторым пользователем
			Authorization(Login2, Password2);

			// Перейти на стр словарей
			GoToUrl(RelativeUrlProvider.Glossaries);

			// Перейти на стр словаря
			SwitchCurrentGlossary(_glossaryName);

			// Добавить термин в словарь
			GlossaryTermFilterPage.AddNewEntry();

			// Открыть фильтр
			GlossaryTermFilterPage.OpenFilterForm();

			// Выбрать Бобби
			GlossaryTermFilterPage.SelectAuthor(UserName);

			// Кликнуть Apply кнопку в фильтре
			GlossaryTermFilterPage.ClickApplyBtnInFilterForm();

			Assert.IsTrue(GlossaryTermFilterPage.GetIsCreatedFilterDisplay(), "Ошибка: в хидере таблицы не появился лэйбл Created:");

			// Получить термины из таблицы
			string[] termsSecondAuthor = GlossaryTermFilterPage.GetSortedTerms();

				Logger.Trace(string.Join(" ", termsSecondAuthor));

			// Сравнить термины в таблице
			Assert.AreEqual(termsFirstAuthor, termsSecondAuthor, "Ошибка: списки терминов до и после фильтрации не совпадают");
		}


		/// <summary>
		/// ТС-242, TC-241 Фильтрация по одному/всем автору(ам)
		/// </summary>
		/// <param name="authourModified"> Один или все авторы изменения </param>
		[TestCase("All")]
		[TestCase("UserName2")]
		[Test]
		public void ModifierAuthorFilter(string authourModified)
		{
			// Зайти под первым пользователем и добавить термин в новый глоссари
			string[] termsFirstAuthor = addTerm();
			
			// Выйти
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			// Зайти под вторым пользователем
			Authorization(Login2, Password2);

			// Перейти на стр словарей
			GoToUrl(RelativeUrlProvider.Glossaries);

			// Перейти на стр словаря
			SwitchCurrentGlossary(_glossaryName);

			// Изменить термин в первой строке
			editTerm("edit");

			// Добавить новый термин в словарь
			GlossaryTermFilterPage.AddNewEntry();

			// Обновить страницу
			RefreshPage();

			// Список терминов до фильтрации
			string[] termsBeforeFilter = GlossaryTermFilterPage.GetSortedTerms();

			// Открыть фильтр
			GlossaryTermFilterPage.OpenFilterForm();

			if (authourModified == "UserName2")
			{
				// Выбрать авторa Ринго в Modified dropdown
				GlossaryTermFilterPage.SelectModifier(UserName2);

				// Кликнуть Apply кнопку в фильтре
				GlossaryTermFilterPage.ClickApplyBtnInFilterForm();

				// Получить массив терминов из таблицы после фильтрации
				string[] termsAfterFilter = GlossaryTermFilterPage.GetSortedTerms();

				// Проверить, что в таблице только термины, созданые и измененные Ринго
				Assert.AreEqual(4, termsAfterFilter.Length, "Ошибка: список терминов после фильтрации некорректный");
			}
			else
			{
				// Выбрать всех авторов в Modified dropdown
				GlossaryTermFilterPage.SelectModifier();

				// Кликнуть Apply кнопку в фильтре
				GlossaryTermFilterPage.ClickApplyBtnInFilterForm();

				// Получить массив терминов из таблицы после фильтрации
				string[] termsAfterFilter = GlossaryTermFilterPage.GetSortedTerms();
				// Проверить, что в таблице только термины, созданые и измененные Ринго
				Assert.AreEqual(termsBeforeFilter, termsAfterFilter, "Ошибка: списки терминов до и после фильтрации не совпадают");

			}

		}

		/// <summary>
		/// ТС-251 фильтрация по любой дате
		/// </summary>
		[Test]
		public void CreatedDateFilter()
		{
			string[] termsBeforeFilter = addTerm();

			// Открыть фильтр
			GlossaryTermFilterPage.OpenFilterForm();

			// Кликаем по Created комбобоксу
			GlossaryTermFilterPage.ClickCreatedDateDropDown();

			// Проверяем , что в Created комбобоксе 4 значения
			Assert.IsTrue(GlossaryTermFilterPage.GetIsAlldateAreDispalyed(), "Ошибка: в Created комбобоксе не все значения");

			// Кликнуть Apply кнопку в фильтре
			GlossaryTermFilterPage.ClickApplyBtnInFilterForm();
			
			// Получить массив терминов из таблицы после фильтрации
			string[] termsAfterFilter = GlossaryTermFilterPage.GetSortedTerms();
			Array.Sort(termsBeforeFilter);
			Array.Sort(termsAfterFilter);
			foreach (var term in termsAfterFilter)
			{
				Logger.Trace(string.Join(" ", term));
			}
			Assert.AreEqual(termsBeforeFilter, termsAfterFilter, "Ошибка: списки терминов до и после фильтрации не совпадают");
		}


		/// <summary>
		/// ТС-252, ТС-253, ТС-262, ТС-263 Фильтрация по дате создания/изменения за неделю/последний месяц
		/// </summary>
		/// <param name="dateType"> Дата изменения или создания</param>
		/// <param name="optionFilter"> Опции в дропдауне </param>
		[TestCase("created", "7 days")]
		[TestCase("created", "month")]
		[TestCase("modified", "7 days")]
		[TestCase("modified", "month")]
		[Test]
		public void DateFilter(string dateType,string optionFilter)
		{
			// Добавили один термин и получили массив терминов  в таблице
			string[] terms = addTerm();

			// Обновили страницу
			RefreshPage();

			// Открыть фильтр
			GlossaryTermFilterPage.OpenFilterForm();

			// Выбрать 'Over the last 7 days' в комбобоксе дата создания
			GlossaryTermFilterPage.SelectCreatedDate(optionFilter);

			// Получить начальную дату создания в фильтре
			string startDate = GlossaryTermFilterPage.GetTodayCreatedDate();

			Assert.AreEqual(startDate, _today, "Ошибка: начальная дата создания неверна");

			if (dateType == "created")
			{
				if (optionFilter == "7 days")
				{
					// Получить конечную дату создания
					string endDate = GlossaryTermFilterPage.GetWeekCreatedDate();

					// Просто инфа, можно убрать
					Logger.Trace("start = " + startDate + "\nend = " + endDate +
						"\n7days = " + _weekAgo
						+ "\ntoday = " + _today);

					Assert.AreEqual(endDate, _weekAgo, "Ошибка: конечная дата создания неверна");
				}
				else if (optionFilter == "month")
				{
					string endDate = GlossaryTermFilterPage.GetMonthCreatedDate();

					// Просто инфа, можно убрать
					Logger.Trace("start = " + startDate + "\nend = " + endDate +
						"\n7days = " + _monthAgo
						+ "\ntoday = " + _today);

					Assert.AreEqual(endDate, _monthAgo, "Ошибка: конечная дата создания неверна");
				}
			}

			if (dateType == "modified")
			{
				if (optionFilter == "7 days")
				{
					// Получить конечную дату создания
					string endDate = GlossaryTermFilterPage.GetWeekModifiedDate();

					// Просто инфа, можно убрать
					Logger.Trace("start = " + startDate + "\nend = " + endDate +
						"\n7days = " + _weekAgo
						+ "\ntoday = " + _today);

					Assert.AreEqual(endDate, _weekAgo, "Ошибка: конечная дата создания неверна");
				}
				else if (optionFilter == "month")
				{
					string endDate = GlossaryTermFilterPage.GetMonthModifiedDate();

					// Просто инфа, можно убрать
					Logger.Trace("start = " + startDate + "\nend = " + endDate +
						"\n7days = " + _monthAgo
						+ "\ntoday = " + _today);

					Assert.AreEqual(endDate, _monthAgo, "Ошибка: конечная дата создания неверна");
				}
			}

			// Кликнуть Apply кнопку в фильтрe
			GlossaryTermFilterPage.ClickApplyBtnInFilterForm();

			// Получить массив терминов из таблицы после фильтрации
			string[] termsAfterFilter = GlossaryTermFilterPage.GetSortedTerms();
			foreach (var term in termsAfterFilter)
			{
				Logger.Trace(term.ToString() + "\n");
			}

			Assert.AreEqual(terms, termsAfterFilter, "Ошибка: списки терминов до и после фильтрации не совпадают");
		}

		/// <summary>
		/// ТС-31, TC-32 Отмена/очистка фильтра
		/// </summary>
		[TestCase("Clear")]
		[TestCase("Cancel")]
		[Test]
		public void ClearCancelFilter(string action)
		{
			// Список терминов до фильтрации
			string[] termsBeforeFilter = GlossaryTermFilterPage.GetSortedTerms();

			RefreshPage();

			// Открыть фильтр
			GlossaryTermFilterPage.OpenFilterForm();

			// Выбрать 'Over the last 7 days' в комбобоксе дата создания
			GlossaryTermFilterPage.SelectCreatedDate("7 days");

			// Выбрать 'Over the last month' в комбобоксе дата изменения
			GlossaryTermFilterPage.SelectModifiedDate("last month");

			// Выбрать Бобби
			GlossaryTermFilterPage.SelectAuthor(UserName);

			// Выбрать всех авторов в Modified dropdown
			GlossaryTermFilterPage.SelectModifier();

			if(action == "Clear")
			{
				// Кликаем кнопку Clear Filter
				GlossaryTermFilterPage.ClickClearFilter();

				// Проверяем, что поля очистились
				Assert.IsTrue(GlossaryTermFilterPage.GetCreatedSetValue() == "Anytime", "Ошибка: в Created комбобоксе неверное значение (должно быть Anytime)");
				Assert.IsTrue(GlossaryTermFilterPage.GetModifiedSetValue() == "Anytime", "Ошибка: в Modified комбобоксе неверное значение (должно быть Anytime)");
				Assert.IsTrue(!GlossaryTermFilterPage.GetCheckboxesAuthor(), "Ошибка: комбобокс авторов не очистился");
				Assert.IsTrue(!GlossaryTermFilterPage.GetCheckboxesModifier(), "Ошибка: Modified комбобокс не очистился");
				Assert.IsTrue(GlossaryTermFilterPage.LangCheckboxesInFilterForm(), "Ошибка: в фильтре выбраны не все языки");
				// Кликнуть Apply кнопку
				GlossaryTermFilterPage.ClickApplyBtnInFilterForm();
			}
			else
			{
				GlossaryTermFilterPage.ClickCancelFilter();
				Assert.IsFalse(GlossaryTermFilterPage.GetCommonPahelIsDisplay(), "Ошибка: фильтры отображаются в желтой панели");
			}

			// Получить массив терминов из таблицы после фильтрации
			string[] termsAfterFilter = GlossaryTermFilterPage.GetSortedTerms();

			// Проверить , что термины не изменились после фильтрации
			Assert.AreEqual(termsBeforeFilter, termsAfterFilter, "Ошибка: списки терминов до и после фильтрации не совпадают");
		}

		/// <summary>
		/// ТС-33 Снятие фильтрации 
		/// </summary>
		/// <param name="filter"> Значение фильтра </param>
		[TestCase("Over the last 7 days")]
		[TestCase("Over the last month")]
		[TestCase("author")]
		[TestCase("modifier")]
		[TestCase("all")]
		[Test]
		public void ClearPanelFilter(string filter)
		{
			// Добавить термин в словарь
			GlossaryTermFilterPage.AddNewEntry();

			// Обновить страницу
			RefreshPage();

			// Открыть фильтр
			GlossaryTermFilterPage.OpenFilterForm();

			// Выбрать язык
			GlossaryTermFilterPage.CheckOnlyOneLanguage("English");

			// Выбрат автора создания
			GlossaryTermFilterPage.SelectAuthor(UserName);

			// Выбрат автора изменения
			GlossaryTermFilterPage.SelectModifier(UserName);

			// Выбрать дату создания "7 дней назад"
			GlossaryTermFilterPage.SelectCreatedDate("7");

			// ВЫбрать дату изменения "месяц назад"
			GlossaryTermFilterPage.SelectModifiedDate("month");

			// Кликнуть Apply кнопку
			GlossaryTermFilterPage.ClickApplyBtnInFilterForm();

			// Проверка, что все фильтре на месте
			Assert.IsTrue(GlossaryTermFilterPage.GetCreatedPanelIsDislay("Over the last 7 days"), "Ошибка: фильтр создания даты не появился в желтой панели");
			Assert.IsTrue(GlossaryTermFilterPage.GetModifiedPanelIsDislay("Over the last month"), "Ошибка: фильтр изменения даты не появился в желтой панели");
			Assert.IsTrue(GlossaryTermFilterPage.GetCreatedPanelIsDislay(UserName), "Ошибка: фильтр автор создания не появился в желтой панели");
			Assert.IsTrue(GlossaryTermFilterPage.GetModifiedPanelIsDislay(UserName), "Ошибка: фильтр автор изменения не появился в желтой панели");

			switch (filter)
			{
				case "Over the last 7 days":
					GlossaryTermFilterPage.DeleteCreatedPanel(filter);
					Assert.IsFalse(GlossaryTermFilterPage.GetCreatedPanelIsDislay(filter), "Ошибка: фильтр создания даты не удалился из желтой панели");
					break;

				case "Over the last month":
					GlossaryTermFilterPage.DeleteModifiedPanel(filter);
					Assert.IsFalse(GlossaryTermFilterPage.GetModifiedPanelIsDislay(filter), "Ошибка: фильтр изменения даты не удалился из желтой панели");
					break;

				case "author":
					GlossaryTermFilterPage.DeleteCreatedPanel(UserName);
					Assert.IsFalse(GlossaryTermFilterPage.GetCreatedPanelIsDislay(filter), "Ошибка: фильтр автора создания не удалился из желтой панели");
					break;

				case "modifier":
					GlossaryTermFilterPage.DeleteModifiedPanel(UserName);
					Assert.IsFalse(GlossaryTermFilterPage.GetModifiedPanelIsDislay(filter), "Ошибка: фильтр автора изменения не удалился из желтой панели");
					break;

				case "all":
					GlossaryTermFilterPage.DeleteAllPanel();
					Assert.IsFalse(GlossaryTermFilterPage.GetCommonPahelIsDisplay(), "Ошибка: фильтры не удалились из желтой панели");
					break;
			}
		}

		/// <summary>
		/// ТС-2542 Варианты задания дат
		/// </summary>
		[Category("PRX_6924")]
		[TestCase("modified", "nextYear", "yearAgo", false)] // начальная дата изменения больше конечной
		[TestCase("created", "nextYear", "yearAgo", false)] // начальная дата создания больше конечной
		[TestCase("modified", "weekAgo", "nextWeek", true)]
		[TestCase("created", "weekAgo", "nextWeek", true)]
		[TestCase("modified", "weekAgo", "today", true)]
		[TestCase("created", "weekAgo", "today", true)]
		[Test]
		public void DateVariantsFilter(string calendar, string startDate, string endDate, bool result)
		{
			// Обновить страницу
			RefreshPage();

			// Открыть фильтр
			GlossaryTermFilterPage.OpenFilterForm();
			switch (endDate)
			{
				case "today":
					endDate = _today;
					break;
				case "nextWeek":
					endDate = _nextWeek;
					break;
				case "yearAgo":
				endDate = _yearAgo;
				break;
			}

			switch (startDate)
			{
				case "weekAgo":
					startDate = _weekAgo;
					break;
				case "nextYear":
					startDate = _nextYear;
					break;
			}

			string startDateFilter ="";
			string expectedRangeTitle = "";

			if (calendar == "created")
			{
				// Ввести начальную дату создания
				GlossaryTermFilterPage.SendCreatedStartDate(startDate);

				// Ввести конечную дату создания
				GlossaryTermFilterPage.SendCreatedEndDate(endDate);

				// Получить значение в начальной дате создания
				startDateFilter = GlossaryTermFilterPage.GetStartCreatedDate();

				//формируем ожидаемую строку
				expectedRangeTitle = "Created: from " + startDate + " to " + endDate;
			}

			if (calendar == "modified")
			{
				// Ввести начальную дату изменения
				GlossaryTermFilterPage.SendModifiedStartDate(startDate);

				// Ввести конечную дату изменения
				GlossaryTermFilterPage.SendModifiedEndDate(endDate);

				// Получить значение в начальной дате изменения
				startDateFilter = GlossaryTermFilterPage.GetStartModifiedDate();
				
				//формируем ожидаемую строку
				expectedRangeTitle = "Modified: from " + startDate + " to " + endDate;
			}

			if (result == false)
			{
				//Проверить, что начальная дата поменялась на текущую
				Assert.IsTrue(startDateFilter == _today, "Ошибка: начальная дата создания не изменилась на текущую дату");
			}
			else
			{
				// Кликнуть Apply кнопку
				GlossaryTermFilterPage.ClickApplyBtnInFilterForm();
				string actualRangeTitle = GlossaryTermFilterPage.GetRangeFilterPanel();
				Assert.AreEqual(actualRangeTitle, expectedRangeTitle, "Ошибка: диапазон дат в желтой панели неверен");
			}

		}

		/// <summary>
		/// ТС-2543 Установка значения «Заданный диапазон»
		/// </summary>
		[TestCase("modified", "endDate")]
		[TestCase("created", "startDate")]
		[TestCase("modified", "startDate")]
		[TestCase("created", "endDate")]
		[Test]
		public void CalendarFilter(string calendar, string date)
		{
			// Обновить страницу
			RefreshPage();

			// Открыть фильтр
			GlossaryTermFilterPage.OpenFilterForm();

			if(calendar == "created" )
			{
				// Выбрать дату создания "7 дней назад"
				GlossaryTermFilterPage.SelectCreatedDate("7");
				if(date == "startDate")
				{
					// Выбрать начальную дату создания в календаре
					GlossaryTermFilterPage.SetStartDayInLeftCalendar();
				}else
				{
					// Выбрать конечную дату создания в календаре
					GlossaryTermFilterPage.SetEndDayInLeftCalendar();
				}

				// Проверить , чтов  кобобоксе календаря слева значение сменилось на "Specify a time range..."
				Assert.AreEqual(GlossaryTermFilterPage.GetTitleInDateCreatedDD(), "Specify a time range...",
					"Ошибка: значение в комбобоксе не поменялась на \"Specify a time range...\"");
			}

			if (calendar == "modified")
			{
				// Выбрать дату создания "7 дней назад"
				GlossaryTermFilterPage.SelectModifiedDate("7");
				if (date == "startDate")
				{
					// Выбрать начальную дату изменения в календаре 
					GlossaryTermFilterPage.SetStartDayInRightCalendar();
				}
				else
				{
					// Выбрать конечну дату изменения в календаре
					GlossaryTermFilterPage.SetEndDayInRightCalendar();
				}

				// Проверить , чтов  кобобоксе календаря справа значение сменилось на "Specify a time range..."
				Assert.AreEqual(GlossaryTermFilterPage.GetTitleInDateModifiedDD(), "Specify a time range...",
					"Ошибка: значение в комбобоксе не поменялась на \"Specify a time range...\"");
			}

		}

		/// <summary>
		/// Добавить термин 
		/// </summary>
		/// <returns> Список терминов из таблицы </returns>
		private string[] addTerm()
		{
			// Добавить термин в словарь
			GlossaryTermFilterPage.AddNewEntry();

			// Получить массив терминов 
			string[] termsFirstAuthor = GlossaryTermFilterPage.GetSortedTerms();
			
			foreach (var item in termsFirstAuthor)
			{
				Logger.Trace(item.ToString() + "\n");
			}

			return termsFirstAuthor;
		}

		/// <summary>
		/// Изменить термин
		/// </summary>
		/// <paparam name="rowNumber"> Номер строки термина</paparam>
		/// <paparam name="text"> Текст </paparam>
		private void editTerm(string text, int rowNumber = 1)
		{
			// Кликнуть по строек термина
			GlossaryTermFilterPage.ClickTermRow();

			// Кликнуть кнопку редактирования термина
			GlossaryTermFilterPage.ClickEditBtn();

			// Ввести текст
			GlossaryTermFilterPage.EditEditableFields(text, rowNumber);

			// Сохранить
			GlossaryTermFilterPage.ClickSaveTerm();
		}

		/// <summary>
		/// Добавить язык в фильтр на стр глоссари
		/// </summary>
		private void addLanguageToFilter(CommonHelper.LANGUAGE language)
		{
			Thread.Sleep(1000);
			// Открыть свойства глоссари
			OpenGlossaryProperties();

			// Добавить язык
			GlossaryEditForm.ClickAddLanguage();

			// Раскрыть дропдаун язка
			GlossaryEditForm.ClickLastLangOpenCloseList();

			// Выбрать язык
			GlossaryEditForm.SelectLanguage(language);
			Thread.Sleep(1000);

			// Сохранить
			GlossaryEditForm.ClickSaveGlossary();
		}

		/// <summary>
		/// Удалить язык из свойств глоссари
		/// </summary>
		/// <param name="number"> Номер языка </param>
		private void deleteLanguage(int number = 1)
		{
			Thread.Sleep(1000);
			OpenGlossaryProperties();
			GlossaryTermFilterPage.DeleteLanguageInProperties(number);
			Thread.Sleep(1000);
			GlossaryEditForm.ClickSaveGlossary();
		}

		/// <summary>
		/// Получить кол-во выбранных языков в фильтре
		/// </summary>
		/// <returns> Кол-во выбранных языков </returns>
		private int getSelectedLanguageCountInFilter()
		{
			// Открыть фильтр
			Assert.IsTrue(GlossaryTermFilterPage.OpenFilterForm(), "Ошибка: Filter поп-ап не открылся");

			// Получить список выбранных языков в фильтре
			List<string> languagesList = GlossaryTermFilterPage.GetSelectedLangInFilterForm();

			// Получить кол-во языков в фильтре
			int count = languagesList.Count;

			return count;
		}
	}
}
