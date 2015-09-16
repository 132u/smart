using System;
using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights
{
	public class ManageTermsTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{

		[SetUp]
		public void SetUp()
		{
			_glossaryHelper = _workspaceHelper
				.GoToUsersRightsPage()
				.ClickGroupsButton()
				.CheckOrAddUserToGroup("Administrators", ConfigurationManager.NickName)
				.GoToGlossariesPage();

			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
		}

		[Test]
		public void CreateDefaultTermTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm()
				.AssertDefaultTermsCountMatch(expectedTermCount: 1);
		}

		[Test]
		public void CreateCustomTermTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddNewSystemField(GlossarySystemField.Topic)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton()
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test]
		public void CreateExistingTermTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm()
				.CreateTerm()
				.AssertAlreadyExistTermErrorDisplayed();
		}

		[Test]
		public void CreateEmptyTermTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.ClickNewEntryButton()
				.ClickSaveButton()
				.AddAtLeastOnTermErrorDisplay();
		}

		[Test]
		public void CreateSynonymsTest()
		{
			var term1 = "Term1";
			var term2 = "Term2";
			var termSynonym1 = "TermSynonym1";
			var termSynonym2 = "TermSynonym2";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName)
				.ClickNewEntryButton()
				.FillTerm(term1, term2)
				.AddSynonym(columnNumber: 1, text: termSynonym1)
				.AddSynonym(columnNumber: 2, text: termSynonym2)
				.ClickSaveButton()
				.AssertSynonymCountMatch(expectedCount: 2, termNumber: 1, columnNumber: 1)
				.AssertSynonymCountMatch(expectedCount: 2, termNumber: 1, columnNumber: 2);
		}

		[Test]
		public void CreateEqualSynonymsTest()
		{
			var term1 = "Term1";
			var term2 = "Term2";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName)
				.ClickNewEntryButton()
				.FillTerm(term1, term2)
				.AddSynonym(columnNumber: 1, text: term1)
				.AddSynonym(columnNumber: 2, text: term2)
				.ClickSaveButton()
				.AssertSynonumUniqueErrorDisplayed(columnNumber: 1)
				.AssertSynonumUniqueErrorDisplayed(columnNumber: 2);
		}

		[Test]
		public void DeleteTermTest()
		{
			var term1 = "Term1";
			var term2 = "Term2";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm(term1, term2)
				.DeleteTerm(term1, term2)
				.AssertDefaultTermsCountMatch(expectedTermCount: 0);
		}

		[Test]
		public void CancelCreateTermTest()
		{
			_glossaryHelper.CreateGlossary(_glossaryUniqueName)
				.ClickNewEntryButton()
				.FillTerm()
				.CancelEditTerm()
				.AssertDefaultTermsCountMatch(expectedTermCount: 0);
		}

		[Test]
		public void SearchTermByFirstLanguageTest()
		{
			var uniqueText = DateTime.UtcNow.Ticks + "1Term";
			var term1 = "Term1";
			var term2 = "Term2";
			var term3 = "Term3 " + uniqueText;
			var term4 = "Term4 " + DateTime.UtcNow;

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm(term1, term2)
				.CreateTerm(term3, term4)
				.SearchTerm(uniqueText)
				.AssertDefaultTermsCountMatch(expectedTermCount: 1)
				.AssertTermMatch(term3);
		}

		[Test]
		public void SearchTermBySecondLanguageTest()
		{
			var uniqueText = DateTime.UtcNow.Ticks + "1Term";
			var term1 = "Term1";
			var term2 = "Term2";
			var term3 = "Term3 " + DateTime.UtcNow;
			var term4 = "Term4 " + uniqueText;

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm(term1, term2)
				.CreateTerm(term3, term4)
				.SearchTerm(uniqueText)
				.AssertDefaultTermsCountMatch(expectedTermCount: 1)
				.AssertTermMatch(term3);
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

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm(firstTerm, secondTerm + DateTime.UtcNow.Ticks)
				.GoToGlossariesPage()
				.CreateGlossary(glossaryUniqueName2)
				.CreateTerm(firstTerm, secondTerm + DateTime.UtcNow.Ticks)
				.GotToSearchPage()
				.InitSearch(uniqueData)
				.AssertGlossariesNamesMatch(glossaryList)
				.AssertTermNamesMatch(firstTerm);
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

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName, languageList: languages)
				.OpenGlossaryStructure()
				.AddNewSystemField(GlossarySystemField.Topic)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton()
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1)
				.AssertLanguageColumnCountMatch(languages.Count);
		}

		[Test]
		public void EditDefaultTerm()
		{
			var term1 = "Term 1";
			var term2 = "Term 2";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm(term1, term2)
				.EditDefaultTerm(term1, term2, term1 + DateTime.Now)
				.AssertDefaultTermsCountMatch(expectedTermCount: 1);
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

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName, languageList: languages)
				.OpenGlossaryStructure()
				.AddNewSystemField(GlossarySystemField.Topic)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton()
				.EditCustomTerms(newTerm)
				.AssertTermDisplayedInLanguagesAndTermsSection(newTerm)
				.AssertTermsTextMatch(newTerm);
		}

		private GlossariesHelper _glossaryHelper;

		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
		private string _glossaryUniqueName;
	}
}
