using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class ManageTermsTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_glossaryHelper = new GlossariesHelper(Driver);
			_searchPage = new SearchPage(Driver);
			_usersTab = new UsersTab(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_termAlreadyExistsDialog = new TermAlreadyExistsDialog(Driver);

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.AddUserToGroupIfNotAlredyAdded("Administrators", ThreadUser.NickName);

			_workspacePage.GoToGlossariesPage();
		}

		[Test]
		public void CreateDefaultTermTest()
		{
			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.CreateTerm();

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");
		}

		[Test]
		public void CreateCustomTermTest()
		{
			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddNewSystemField(GlossarySystemField.Topic);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton()
				.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void CreateExistingTermTest()
		{
			var firstTerm = "firstTerm";
			var secondTerm = "secondTerm";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.CreateTerm(firstTerm, secondTerm);

			_glossaryPage
				.ClickNewEntryButton()
				.FillAllTerm(firstTerm, secondTerm)
				.ClickSaveTermButtonExpectingTermAlreadyExistsDialog();

			Assert.IsTrue(_termAlreadyExistsDialog.IsTermAlreadyExistsDialogOpened(),
				"Произошла ошибка:\n сообщение 'The term already exists' не появилось.");
		}

		[Test]
		public void CreateEmptyTermTest()
		{
			_glossaryHelper.CreateGlossary(_glossaryUniqueName);
				
			_glossaryPage
				.ClickNewEntryButton()
				.ClickSaveTermButton();

			Assert.IsTrue(_glossaryPage.IsEmptyTermErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'Please add at least one term' не появилось");
		}

		[Test]
		public void CreateSynonymsTest()
		{
			var term1 = "Term1";
			var term2 = "Term2";
			var termSynonym1 = "TermSynonym1";
			var termSynonym2 = "TermSynonym2";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.ClickNewEntryButton()
				.FillAllTerm(term1, term2)
				.AddSynonym(columnNumber: 1, text: termSynonym1)
				.AddSynonym(columnNumber: 2, text: termSynonym2)
				.ClickSaveTermButton();

			Assert.AreEqual(2, _glossaryPage.SynonymFieldsCount(termRow: 1, columnNumber: 1),
				"Произошла ошибка:\n неверное количество синонимов в термине №{0} и столбце №{1}", 1, 1);

			Assert.AreEqual(2, _glossaryPage.SynonymFieldsCount(termRow: 1, columnNumber: 2),
				"Произошла ошибка:\n неверное количество синонимов в термине №{0} и столбце №{1}", 1, 2);
		}

		[Test]
		public void CreateEqualSynonymsTest()
		{
			var term1 = "Term1";
			var term2 = "Term2";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.ClickNewEntryButton()
				.FillAllTerm(term1, term2)
				.AddSynonym(columnNumber: 1, text: term1)
				.AddSynonym(columnNumber: 2, text: term2)
				.ClickSaveTermButton();

			Assert.IsTrue(_glossaryPage.IsSynonumUniqueErrorDisplayed(columnNumber: 1),
				"Произошла ошибка:\n Термины не подсвечены красным цветом в стоблце №1");

			Assert.IsTrue(_glossaryPage.IsSynonumUniqueErrorDisplayed(columnNumber: 2),
				"Произошла ошибка:\n Термины не подсвечены красным цветом в стоблце №2");
		}

		[Test]
		public void DeleteTermTest()
		{
			var term1 = "Term1";
			var term2 = "Term2";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm(term1, term2)
				.DeleteTerm(term1, term2);

			Assert.IsTrue(_glossaryPage.IsDeleteButtonDisappeared(term1, term2),
				"Произошла ошибка: \nне исчезла кнопка удаления");

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 0),
				"Произошла ошибка:\nневерное количество терминов");
		}

		[Test]
		public void CancelCreateTermTest()
		{
			_glossaryHelper.CreateGlossary(_glossaryUniqueName);			

			_glossaryPage
				.ClickNewEntryButton()
				.FillAllTerm()
				.ClickCancelButton();

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 0),
				"Произошла ошибка:\n неверное количество терминов");
		}

		[Test]
		public void SearchTermByFirstLanguageTest()
		{
			var uniqueText = DateTime.UtcNow.Ticks + "1Term";
			var term1 = "Term1";
			var term2 = "Term2";
			var term3 = "Term3 " + uniqueText;
			var term4 = "Term4 " + DateTime.UtcNow;

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm(term1, term2)
				.CreateTerm(term3, term4)
				.SearchTerm(uniqueText);

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");

			Assert.AreEqual(term3, _glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void SearchTermBySecondLanguageTest()
		{
			var uniqueText = DateTime.UtcNow.Ticks + "1Term";
			var term1 = "Term1";
			var term2 = "Term2";
			var term3 = "Term3 " + DateTime.UtcNow;
			var term4 = "Term4 " + uniqueText;

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm(term1, term2)
				.CreateTerm(term3, term4)
				.SearchTerm(uniqueText);

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");

			Assert.AreEqual(term3, _glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void SearchTermTest()
		{
			var uniqueData = DateTime.UtcNow.Ticks + "SearchTest";
			var firstTerm = "Test First Term " + uniqueData;
			var secondTerm = "Test Second Term ";
			var glossaryUniqueName2 = GlossariesHelper.UniqueGlossaryName();

			var glossaryList = new List<string>
			{
				_glossaryUniqueName, 
				glossaryUniqueName2
			};

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.CreateTerm(firstTerm, secondTerm + DateTime.UtcNow.Ticks);

			_workspacePage.GoToGlossariesPage();

			_glossaryHelper.CreateGlossary(glossaryUniqueName2);

			_glossaryPage.CreateTerm(firstTerm, secondTerm + DateTime.UtcNow.Ticks);

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(uniqueData);

			Assert.IsTrue(_searchPage.IsGlossariesNamesMatch(glossaryList),
				"Произошла ошибка:\n списки имен глоссариев не совпадают.");

			Assert.IsTrue(_searchPage.IsTermNamesMatch(firstTerm),
				"Произошла ошибка:\n найденные термины не совпадает с заданным термином '{0}'.", firstTerm);
		}

		[Test]
		public void CreateCustomTermInMultiLanguageGlossaryTest()
		{
			var languages = new List<Language>
			{
				Language.German,
				Language.French,
				Language.Japanese,
				Language.Lithuanian
			};

			_glossaryHelper.CreateGlossary(_glossaryUniqueName, languageList: languages);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddNewSystemField(GlossarySystemField.Topic);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton()
				.CloseExpandedTerms();

			Assert.AreEqual(languages.Count, _glossaryPage.LanguageColumnCount(),
				"Произошла ошибка:\n неверное количество колонок с языками.");

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void EditDefaultTerm()
		{
			var term1 = "Term 1";
			var term2 = "Term 2";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm(term1, term2)
				.EditDefaultTerm(term1, term2, term1 + DateTime.Now);

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");
		}

		[Test]
		public void EditCustomTerm()
		{
			var newTerm = "Term Example" + DateTime.Now;
			var languages = new List<Language> 
			{
				Language.German,
				Language.French,
				Language.Japanese,
				Language.Lithuanian
			};

			_glossaryHelper.CreateGlossary(_glossaryUniqueName, languageList: languages);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddNewSystemField(GlossarySystemField.Topic);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton()
				.EditCustomTerms(newTerm);

			Assert.IsTrue(_glossaryPage.IsTermsTextMatchExpected(newTerm),
				"Произошла ошибка:\n один или более терминов не соответствуют ожидаемому значению");

			Assert.IsTrue(_glossaryPage.IsTermDisplayedInLanguagesAndTermsSection(newTerm),
				"Произошла ошибка:\n Термин {0} отсутствует в секции 'Languages And Terms'.", newTerm);
		}

		private GlossariesHelper _glossaryHelper;
		private WorkspacePage _workspacePage;
		private SearchPage _searchPage;
		private UsersTab _usersTab;
		private GlossaryPage _glossaryPage;
		private GlossaryStructureDialog _glossaryStructureDialog;
		private TermAlreadyExistsDialog _termAlreadyExistsDialog;

		private string _glossaryUniqueName;
	}
}
