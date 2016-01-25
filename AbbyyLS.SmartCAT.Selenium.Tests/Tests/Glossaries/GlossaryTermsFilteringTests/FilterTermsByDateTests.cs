using System;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FilterTermsByDateTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void FilterTermsByDateTestsSetUp()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.CreateTerm();
		}

		[TestCase(DateRange.Week)]
		[TestCase(DateRange.Month)]
		public void CreatedDateFilterTest(DateRange option)
		{
			var termsBeforeFilter = _glossaryPage.GetTermList();

			_glossaryPage
				.ClickFilterButton()
				.SelectCreatedDateOption(option);

			Assert.AreEqual(DateTime.Today, _filterDialog.GetCreatedDateTime(DateRange.Today),
				"Произошла ошибка:\nНеверная дата в поле конечной даты создания.");

			if (option == DateRange.Week)
			{
				Assert.AreEqual(DateTime.Today.AddDays(-6), _filterDialog.GetCreatedDateTime(DateRange.Week),
					"Произошла ошибка:\nНеверная дата в поле начальной даты создания.");
			}
			else
			{
				Assert.AreEqual(DateTime.Today.AddMonths(-1), _filterDialog.GetCreatedDateTime(DateRange.Month),
					"Произошла ошибка:\nНеверная дата в поле начальной даты создания.");
			}

			_filterDialog.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.GetTermList().SequenceEqual(termsBeforeFilter),
				"Произошла ошибка:\nСписки терминов не совпадают.");
		}

		[TestCase(DateRange.Month)]
		[TestCase(DateRange.Week)]
		public void ModifedDateFilterTest(DateRange option)
		{
			var termsBeforeFilter = _glossaryPage.GetTermList();

			_glossaryPage
				.ClickFilterButton()
				.SelectModifiedDateOption(option);

			Assert.AreEqual(DateTime.Today, _filterDialog.GetModifiedDataText(DateRange.Today),
				"Произошла ошибка:\nНеверная дата в поле конечной даты изменения.");

			if (option == DateRange.Week)
			{
				Assert.AreEqual(DateTime.Today.AddDays(-6), _filterDialog.GetModifiedDataText(DateRange.Week),
					"Произошла ошибка:\nНеверная дата в поле начальной даты изменения.");
			}
			else
			{
				Assert.AreEqual(DateTime.Today.AddMonths(-1), _filterDialog.GetModifiedDataText(DateRange.Month),
					"Произошла ошибка:\nНеверная дата в поле начальной даты изменения.");
			}

			_filterDialog.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.GetTermList().SequenceEqual(termsBeforeFilter),
				"Произошла ошибка:\nСписки терминов не совпадают.");
		}

		[Test]
		public void CreatedStartDateCalendarFilter()
		{
			_glossaryPage
				.ClickFilterButton()
				.SelectCreatedDateOption(DateRange.Week);

			_filterDialog
				.ClickCreatedStartDate()
				.ClickDayInCalendar();

			Assert.AreEqual("Specify a time range...", _filterDialog.GetCreatedDateDropdownText(),
				"Произошла ошибка:\nНеверное значение в дропдауне Created.");
		}

		[Test]
		public void CreatedEndDateCalendarFilter()
		{
			_glossaryPage
				.ClickFilterButton()
				.SelectCreatedDateOption(DateRange.Week);

			_filterDialog
				.ClickCreatedEndDate()
				.ClickDayInCalendar();

			Assert.AreEqual("Specify a time range...", _filterDialog.GetCreatedDateDropdownText(),
				"Произошла ошибка:\nНеверное значение в дропдауне Created.");
		}

		[Test]
		public void ModifiedStartDateCalendarFilter()
		{
			_glossaryPage
				.ClickFilterButton()
				.SelectModifiedDateOption(DateRange.Week);

			_filterDialog
				.ClickModifiedStartDate()
				.ClickDayInCalendar();

			Assert.AreEqual("Specify a time range...", _filterDialog.GetModifiedDateDropdownText(),
				"Произошла ошибка:\nНеверное значение в дропдауне Modified.");
		}

		[Test]
		public void ModifiedEndDateCalendarFilter()
		{
			_glossaryPage
				.ClickFilterButton()
				.SelectModifiedDateOption(DateRange.Week);

			_filterDialog
				.ClickModifiedEndDate()
				.ClickDayInCalendar();

			Assert.AreEqual("Specify a time range...", _filterDialog.GetModifiedDateDropdownText(),
				"Произошла ошибка:\nНеверное значение в дропдауне Modified.");
		}
	}
}
