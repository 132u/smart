using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	public class GlossaryTermFilterTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossaryTermFilterTestsSetUp()
		{
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_glossaryHelper = new GlossariesHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_commonHelper = new CommonHelper(Driver);
			_loginHelper = new LoginHelper(Driver);

			_glossaryPage = new GlossaryPage(Driver);
			_filterDialog = new FilterDialog(Driver);
			_signInPage = new SignInPage(Driver);

			_glossaryHelper
				.GoToGlossariesPage()
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm();
		}

		[Test]
		public void LanguageCheckboxesFilterTest()
		{
			_glossaryPage.ClickFilterButton();

			Assert.IsTrue(_filterDialog.AreLanguagesCheckedInDropdown(),
				"Произошла ошибка:\nНе все языки отмечены в дропдауне.");
		}

		[Test]
		public void AddLanguagesTest()
		{
			_glossaryHelper
				.OpenGlossaryProperties()
				.AddLangauge(Language.French)
				.AddLangauge(Language.German)
				.ClickSaveButtonInPropetiesDialog();

			_glossaryPage.ClickFilterButton();

			Assert.IsTrue(_filterDialog.AreLanguagesCheckedInDropdown(),
				"Произошла ошибка:\nНе все языки отмечены в дропдауне.");

			var languagesFilter = _filterDialog.SelectedLanguagesInDropdown();

			_filterDialog.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.GetLanguagesColumnList().SequenceEqual(languagesFilter),
				"Произошла ошибка:\nСписок языков, выбранных в фильтре, не совпал со списокм колонок с языками.");

		}

		[Test]
		public void OneLanguageFilterTest()
		{
			_glossaryPage
				.ClickFilterButton()
				.SelectLanguage(Language.English);

			var languagesFilter = _filterDialog.SelectedLanguagesInDropdown();

			_filterDialog.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.GetLanguagesColumnList().SequenceEqual(languagesFilter),
				"Произошла ошибка:\nСписок языков, выбранных в фильтре, не совпал со списокм колонок с языками.");

		}

		[Test]
		public void UncheckAllFilterLanguagesTest()
		{
			_glossaryPage
				.ClickFilterButton()
				.ClickLanguageDropdown()
				.UncheckAllFilterLanguages()
				.ClickApplyButtonFailureExpected();

			Assert.IsTrue(_filterDialog.IsEmptyLanguageerrorDisplayed(),
				"Произошла ошибка:\nСообщение 'The \"Language\" field cannot be empty.' не появилось.");
		}

		[Test]
		public void AddNewLanguageFilterTest()
		{
			_glossaryPage.ClickFilterButton();

			var languagesFilterBefore = _filterDialog.SelectedLanguagesInDropdown();

			_filterDialog.ClickApplyButton();

			_glossaryHelper
				.OpenGlossaryProperties()
				.AddLangauge(Language.French)
				.ClickSaveButtonInPropetiesDialog();

			_glossaryPage.ClickFilterButton();
			
			var languagesFilterAfter = _filterDialog.SelectedLanguagesInDropdown();

			Assert.AreEqual(languagesFilterBefore.Count + 1, languagesFilterAfter.Count,
				"Произошла ошибка:\nНеверное количество языков в фильтре.");
		}

		[Test]
		public void DeletedLanguageFilterTest()
		{
			_glossaryPage.ClickFilterButton();;

			var languagesFilterBefore = _filterDialog.SelectedLanguagesInDropdown();

			_filterDialog.ClickApplyButton();
			_glossaryHelper
				.OpenGlossaryProperties()
				.DeleteLanguage()
				.ClickSaveButtonInPropetiesDialog();

			_glossaryPage.ClickFilterButton();

			var languagesFilterAfter = _filterDialog.SelectedLanguagesInDropdown();

			Assert.AreEqual(languagesFilterBefore.Count - 1, languagesFilterAfter.Count,
				"Произошла ошибка:\nНеверное количество языков в фильтре.");
		}

		[Test]
		public void AuthorFilterTest()
		{
			_workspaceHelper.SignOut();

			_secondUser = TakeUser(ConfigurationManager.Users);

			_commonHelper.GoToSignInPage();
			_signInPage
				.SubmitForm(_secondUser.Login, _secondUser.Password)
				.SelectAccount();

			_glossaryHelper
				.GoToGlossariesPage()
				.GoToGlossaryPage(_glossaryUniqueName)
				.CreateTerm(firstTerm: "term1FromSecondUser", secondTerm: "term2FromSecondUser");

			_workspaceHelper.RefreshPage();

			_glossaryPage
				.ClickFilterButton()
				.ClickAuthorDropdown()
				.SelectAuthor(ThreadUser.NickName)
				.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.IsCreatedByFilterLabelDisplayed(),
				"Произошла ошибка:\nФильтр 'Created by' не отображается в таблице терминов.");

			Assert.IsTrue(_glossaryPage.GetCreatedByFilterText().Contains(ThreadUser.NickName),
				"Произошла ошибка:\nФильтр 'Created by' не содержит имя автора {0}.", ThreadUser.NickName);

			Assert.AreEqual(_glossaryPage.TermsCount(), 1,
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов.");
		}

		[Test]
		public void ModifierFilterTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			_glossaryHelper.CreateTerm(firstTerm: "term1FromFirstUser", secondTerm: "term2FromFirstUser");
			_workspaceHelper
				.SignOut();
			_signInPage
				.SubmitForm(_secondUser.Login, _secondUser.Password)
				.SelectAccount();

			_glossaryHelper
				.GoToGlossariesPage()
				.GoToGlossaryPage(_glossaryUniqueName)
				.EditDefaultTerm("firstTerm", "secondTerm", "edit")
				.CreateTerm(firstTerm: "term1FromSecondUser", secondTerm: "term2FromSecondUser");

			//TODO Убрать рефреш, когда пофиксят PRX-12015
			_workspaceHelper.RefreshPage();

			_glossaryPage
				.ClickFilterButton()
				.ClickModifierDropdown()
				.SelectModifier(_secondUser.NickName)
				.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.IsModifiedByFilterLabelDisplayed(),
				"Произошла ошибка:\nФильтр 'Modified by' не отображается в таблице терминов.");

			Assert.IsTrue(_glossaryPage.IsModifiedByFilterText().Contains(_secondUser.NickName),
				"Произошла ошибка:\nФильтр 'Modified by' не содержит имя автора {0}.", _secondUser.NickName);

			Assert.AreEqual(2, _glossaryPage.TermsCount(),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов.");
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
		public void ModiifedDateFilterTest(DateRange option)
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
		public void ClearFilterTest()
		{
			//TODO Убрать рефреш, когда пофиксят PRX-12015
			_workspaceHelper.RefreshPage();

			var termsBeforeFilter = _glossaryPage.GetTermList();

			_glossaryPage
				.ClickFilterButton()
				.SelectCreatedDateOption(DateRange.Week)
				.SelectModifiedDateOption(DateRange.Month)
				.ClickAuthorDropdown()
				.SelectAuthor(ThreadUser.NickName)
				.ClickModifierDropdown()
				.SelectModifier(ThreadUser.NickName)
				.ClickClearButton();
			
			Assert.AreEqual(DateRange.Anytime.Description(), _filterDialog.GetCreatedDateDropdownText(),
				"Произошла ошибка:\nВ дропдауне Created указано недефолтное значение.");

			Assert.AreEqual(DateRange.Anytime.Description(), _filterDialog.GetModifiedDateDropdownText(),
				"Произошла ошибка:\nВ дропдауне Modified указано недефолтное значение.");

			Assert.IsTrue(_filterDialog.AreAuthorChecboxesUnchecked(),
				"Произошла ошибка:\nВ дропдауне Author стоит галочка.");
			
			Assert.IsTrue(_filterDialog.AreModifierChecboxesUnchecked(),
				"Произошла ошибка:\nВ дропдауне Modifier стоит галочка.");

			Assert.IsTrue(_filterDialog.AreLanguagesCheckedInDropdown(),
				"Произошла ошибка:\nНе все языки отмечены в дропдауне.");

			_filterDialog.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.GetTermList().SequenceEqual(termsBeforeFilter),
				"Произошла ошибка:\nСписки терминов не совпадают.");
		}

		[Test]
		public void CancelFilterTest()
		{
			//TODO Убрать рефреш, когда пофиксят PRX-12015
			_workspaceHelper.RefreshPage();

			var termsBeforeFilter = _glossaryPage.GetTermList();

			_glossaryPage
				.ClickFilterButton()
				.SelectCreatedDateOption(DateRange.Week)
				.SelectModifiedDateOption(DateRange.Month)
				.ClickAuthorDropdown()
				.SelectAuthor(ThreadUser.NickName)
				.ClickModifierDropdown()
				.SelectModifier(ThreadUser.NickName)
				.ClickCancelButton();

			Assert.IsTrue(_glossaryPage.GetTermList().SequenceEqual(termsBeforeFilter),
					"Произошла ошибка:\nСписки терминов не совпадают.");
		}

		[TestCase("Over the last 7 days")]
		[TestCase("Over the last month")]
		[TestCase("author")]
		[TestCase("modifier")]
		[TestCase("all")]
		public void ClearPanelFilterTest(string filter)
		{
			//TODO Убрать рефреш, когда пофиксят PRX-12015
			_workspaceHelper.RefreshPage();

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
					Assert.IsTrue(_glossaryPage.IsClearFilterSectionDisplayed(),
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
			_glossaryPage.ClickFilterButton();;
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

		[TearDown]
		public void TearDown()
		{
			if (_secondUser != null)
			{
				ReturnUser(ConfigurationManager.Users, _secondUser);
			}
		}

		private GlossaryPage _glossaryPage;
		private SignInPage _signInPage;
		private FilterDialog _filterDialog ;
		private LoginHelper _loginHelper;
		private CommonHelper _commonHelper;
		private GlossariesHelper _glossaryHelper;
		private WorkspaceHelper _workspaceHelper;
		private string _glossaryUniqueName;
		private TestUser _secondUser;
	}
}