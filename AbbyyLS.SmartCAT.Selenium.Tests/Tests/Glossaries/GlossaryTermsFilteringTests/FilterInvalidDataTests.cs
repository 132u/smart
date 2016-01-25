using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FilterInvalidDataTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void FilterInvalidDataTestsSetUp()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.CreateTerm();
		}


		[TestCase("Over the last 7 days")]
		[TestCase("Over the last month")]
		[TestCase("author")]
		[TestCase("modifier")]
		[TestCase("all")]
		public void ClearPanelFilterTest(string filter)
		{
			//TODO Убрать рефреш, когда пофиксят PRX-12015
			_workspacePage.RefreshPage<WorkspacePage>();

			_glossaryPage
				.ClickFilterButton()
				.SelectLanguage(Language.English)
				.SelectCreatedDateOption(DateRange.Week)
				.SelectModifiedDateOption(DateRange.Month)
				.ClickAuthorDropdown()
				.SelectAuthor(ThreadUser.NickName)
				.ClickModifierDropdown()
				.SelectModifier(ThreadUser.NickName)
				.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.IsCreatedFilterDisplayedInTableHeader("Over the last 7 days"),
				"Произошла ошибка:\nФильтр Createed не отображается в шапке таблицы терминов.");

			Assert.IsTrue(_glossaryPage.IsModifiedFilterDisplayedInTableHeader("Over the last month"),
				"Произошла ошибка:\nФильтр Modified не отображается в шапке таблицы терминов.");

			Assert.IsTrue(_glossaryPage.IsCreatedFilterDisplayedInTableHeader(ThreadUser.NickName),
				"Произошла ошибка:\nФильтр Createed не отображается в шапке таблицы терминов.");

			Assert.IsTrue(_glossaryPage.IsModifiedFilterDisplayedInTableHeader(ThreadUser.NickName),
				"Произошла ошибка:\nФильтр Modified не отображается в шапке таблицы терминов.");

			switch (filter)
			{
				case "Over the last 7 days":
					_glossaryPage.ClickDeleteCreatedFilter(filter);

					Assert.IsFalse(_glossaryPage.IsCreatedFilterDisplayedInTableHeader(filter),
						"Произошла ошибка:\nФильтр Created отображается в шапке таблицы терминов.");

					break;

				case "Over the last month":
					_glossaryPage.ClickDeleteModifiedFilter(filter);

					Assert.IsFalse(_glossaryPage.IsModifiedFilterDisplayedInTableHeader(filter),
						"Произошла ошибка:\nФильтр Modified отображается в шапке таблицы терминов.");
					break;

				case "author":
					_glossaryPage.ClickDeleteCreatedFilter(ThreadUser.NickName);

					Assert.IsFalse(_glossaryPage.IsCreatedFilterDisplayedInTableHeader(filter),
						"Произошла ошибка:\nФильтр Created отображается в шапке таблицы терминов.");

					break;

				case "modifier":
					_glossaryPage.ClickDeleteModifiedFilter(ThreadUser.NickName);

					Assert.IsFalse(_glossaryPage.IsModifiedFilterDisplayedInTableHeader(ThreadUser.NickName),
						"Произошла ошибка:\nФильтр Modified отображается в шапке таблицы терминов.");

					break;

				case "all":
					_glossaryPage.ClickClearAllFilters();

					Assert.False(_glossaryPage.IsAnyFilterDisplayed(),
						"Произошла ошибка:\nЖелтая панель с фильтрами не пуста.");
					break;
			}
		}

		[TestCase("modified", "nextWeek")]
		[TestCase("modified", "today")]
		[TestCase("created", "nextWeek")]
		[TestCase("created", "today")]
		public void DateRangeFilterTest(string calendarType, string range)
		{
			_glossaryPage.ClickFilterButton();

			var startDate = DateTime.Today.AddDays(-6);
			var dateList = new List<DateTime>();
			DateTime endDate = new DateTime();

			switch (range)
			{
				case "today":
					endDate = DateTime.Today;
					break;

				case "nextWeek":
					endDate = DateTime.Today.AddDays(6);
					break;
			}

			dateList.Add(startDate);
			dateList.Add(endDate);

			if (calendarType == "created")
			{
				_filterDialog
					.SetCreatedEndDate(endDate)
					.SetCreatedStartDate(startDate)
					.ClickApplyButton();

				Assert.IsTrue(_glossaryPage.IsDateRangeInFilterMatch(dateList),
					"Произошла ошибка:\nДиапазон дат в фильтре Created неверный.");
			}
			else
			{
				_filterDialog
					.SetModifiedEndDate(endDate)
					.SetModifiedStartDate(startDate)
					.ClickLanguageDropdown()
					.ClickLanguageDropdown()
					.ClickApplyButton();

				Assert.IsTrue(_glossaryPage.IsDateRangeInFilterMatch(dateList),
					"Произошла ошибка:\nДиапазон дат в фильтре Created неверный.");
			}
		}

		[Test, Ignore("PRX-7565")]
		public void InvalidModifiedDateRangeFilterTest()
		{
			_glossaryPage.ClickFilterButton(); ;
			var startDate = DateTime.Today.AddYears(1);
			var endDate = DateTime.Today.AddYears(-1);
			var today = DateTime.Today;

			var dateList = new List<DateTime>();
			dateList.Add(today);
			dateList.Add(endDate);

			_filterDialog
					.SetModifiedEndDate(endDate)
					.SetModifiedStartDate(startDate)
					.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.IsDateRangeInFilterMatch(dateList),
				"Произошла ошибка:\nДиапазон дат в фильтре Created неверный.");
		}

		[Test, Ignore("PRX-7565")]
		public void InvalidCreatedDateRangeFilterTest()
		{
			_glossaryPage.ClickFilterButton(); ;
			var startDate = DateTime.Today.AddYears(1);
			var endDate = DateTime.Today.AddYears(-1);
			var today = DateTime.Today;

			var dateList = new List<DateTime>();
			dateList.Add(today);
			dateList.Add(endDate);

			_filterDialog
				.SetCreatedEndDate(endDate)
				.SetCreatedStartDate(startDate)
				.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.IsDateRangeInFilterMatch(dateList),
				"Произошла ошибка:\nДиапазон дат в фильтре Created неверный.");
		}
	}
}
