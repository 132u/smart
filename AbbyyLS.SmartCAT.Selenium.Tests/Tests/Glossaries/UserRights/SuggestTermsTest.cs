using System;
using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class SuggestTermsTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void TestFixtureSetUp()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_suggestTermDialog = new SuggestTermDialog(Driver);

			_term1 = "term1";
			_term2 = "term2";

			_glossaryName = GlossariesHelper.UniqueGlossaryName();

			_workspaceHelper
				.GoToGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage()
				.DeleteAllSuggestTerms()
				.GoToGlossariesPage();
		}

		[Test]
		public void SuggestTermWithoutGlossaryTest()
		{
			var suggestedTermsByGlossaryCountBefore = _glossariesHelper
				.GoToSuggestedTermsPageFromGlossariesPage()
				.SuggestedTermsByGlossaryCount();

			_glossariesHelper
				.GoToGlossariesPage()
				.OpenSuggestTermDialogFromGlossariesPage()
				.FillSuggestTermDialog()
				.ClickSaveButtonInSuggestTermDialogFromGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage();

			 _glossariesHelper.SuggestedTermsByGlossaryCountMatch(suggestedTermsByGlossaryCountBefore + 1);
		}

		[Test]
		public void SuggestTermWithGlossaryTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.GoToGlossariesPage()
				.OpenSuggestTermDialogFromGlossariesPage()
				.FillSuggestTermDialog(glossary: _glossaryName)
				.ClickSaveButtonInSuggestTermDialogFromGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage()
				.SuggestedTermsByGlossaryCountMatch(suggestedTermsCount: 1, glossary: _glossaryName);
		}

		[Test]
		public void SuggestTermWithGlossaryFromGlossaryPageTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(glossary: _glossaryName)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage()
				.SuggestedTermsByGlossaryCountMatch(suggestedTermsCount: 1, glossary: _glossaryName);
		}

		[Test]
		public void SuggestTermWithGlossaryFromAnotherGlossaryTest()
		{
			var glossaryName2 = GlossariesHelper.UniqueGlossaryName();

			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.GoToGlossariesPage()
				.CreateGlossary(glossaryName2)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(glossary: _glossaryName)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage()
				.SuggestedTermsByGlossaryCountMatch(suggestedTermsCount: 1, glossary: _glossaryName);
		}

		[Test]
		public void SuggestTermWithoutGlossaryFromAnotherGlossaryTest()
		{
			var suggestedTermsByGlossaryCountBefore = _glossariesHelper
				.GoToSuggestedTermsPageFromGlossariesPage()
				.SuggestedTermsByGlossaryCount();

			_glossariesHelper
				.GoToGlossariesPage()
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(glossary: "")
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage();

			_glossariesHelper.SuggestedTermsByGlossaryCountMatch(suggestedTermsByGlossaryCountBefore + 1);
		}

		[Test]
		public void SuggestExistingTermWarningFromGlossaryTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.CreateTerm(_term1, _term2)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage(errorExpected: true)
				.AssertDublicateErrorDisplayed()
				.ClickCancelButtonInSuggestedTermDialogFromGlossaryPage()
				.GoToSuggestedTermsPageFromGlossaryPage()
				.AssertSuggestedTermsCountMatch(expectedTermCount: 0);
		}

		[Test]
		public void SuggestExistingTermAcceptFromGlossaryTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.CreateTerm(_term1, _term2)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage(errorExpected: true)
				.AssertDublicateErrorDisplayed()
				.ClickSaveTermAnywayInSuggestTermDialogFromGlossaryPage()
				.GoToSuggestedTermsPageFromGlossaryPage()
				.AssertSuggestedTermValueMatch(term: _term1, rowNumber: 1, columnNumber: 1)
				.AssertSuggestedTermValueMatch(term: _term2, rowNumber: 1, columnNumber: 2)
				.AssertSuggestedTermsCountMatch(expectedTermCount: 1);
		}

		[Test]
		public void SuggestExistingTermWarningFromGlossaryListTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.CreateTerm(_term1, _term2)
				.GoToGlossariesPage()
				.OpenSuggestTermDialogFromGlossariesPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2, glossary: _glossaryName)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage(errorExpected: true)
				.AssertDublicateErrorDisplayed()
				.ClickCancelButtonInSuggestedTermDialogFromGlossariesPage()
				.GoToGlossaryPage(_glossaryName)
				.GoToSuggestedTermsPageFromGlossaryPage()
				.AssertSuggestedTermsCountMatch(expectedTermCount: 0);
		}

		[Test]
		public void SuggestExistingTermAcceptFromGlossaryListTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.CreateTerm(_term1, _term2)
				.GoToGlossariesPage()
				.OpenSuggestTermDialogFromGlossariesPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2, glossary: _glossaryName)
				.ClickSaveButtonInSuggestTermDialogFromGlossariesPage(errorExpected: true)
				.AssertDublicateErrorDisplayed()
				.ClickSaveTermAnywayInSuggestTermDialogFromGlossariesPage()
				.GoToGlossaryPage(_glossaryName)
				.GoToSuggestedTermsPageFromGlossaryPage()
				.AssertSuggestedTermValueMatch(term: _term1, rowNumber: 1, columnNumber: 1)
				.AssertSuggestedTermValueMatch(term: _term2, rowNumber: 1, columnNumber: 2)
				.AssertSuggestedTermsCountMatch(expectedTermCount: 1);
		}

		[Test]
		public void AcceptSuggestedTermWithGlossaryFromGlossariesPageTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage()
				.AcceptSuggestTermInSuggestedTermsPageForAllGlossaries(glossaryName: _glossaryName)
				.GoToGlossariesPage()
				.GoToGlossaryPage(_glossaryName)
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 1)
				.AssertTermMatch(expectedText: _term1);
		}

		[Test]
		public void AcceptSuggestedTermWithGlossaryFromGlossaryPageTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToSuggestedTermsPageFromGlossaryPage()
				.AcceptSuggestTermInSuggestedTermsPageForCurrentGlossary(termRowNumber: 1)
				.GoToGlossariesPage()
				.GoToGlossaryPage(_glossaryName)
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 1)
				.AssertTermMatch(expectedText: _term1);
		}

		[Test]
		public void AcceptSuggestedTermWithGlossaryFromAnotherGlossaryPageTest()
		{
			var glossaryName2 = GlossariesHelper.UniqueGlossaryName();
			
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToGlossariesPage()
				.CreateGlossary(glossaryName2)
				.GoToSuggestedTermsPageFromGlossaryPage()
				.SelectGlossaryInSuggestedTermsPageForCurrentGlossary(_glossaryName)
				.AcceptSuggestTermInSuggestedTermsPageForCurrentGlossary(termRowNumber: 1)
				.GoToGlossariesPage()
				.GoToGlossaryPage(_glossaryName)
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 1)
				.AssertTermMatch(expectedText: _term1);
		}

		[Test]
		public void AcceptSuggestedTermWithoutGlossaryFromGlossariesPageTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.GoToGlossariesPage()
				.OpenSuggestTermDialogFromGlossariesPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage()
				.AcceptSuggestTermInSuggestedTermsPageForAllGlossaries(chooseGlossary: true)
				.SelectGlossaryForSuggestedTerm(_glossaryName)
				.GoToGlossariesPage()
				.GoToGlossaryPage(_glossaryName)
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 1)
				.AssertTermMatch(expectedText: _term1);
		}

		[Test]
		public void DeleteSuggestedTermWithoutGlossaryTest()
		{
			_glossariesHelper
				.OpenSuggestTermDialogFromGlossariesPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage();

			var suggestedTermsCountBeforeDelete = _glossariesHelper.SuggestedTermsCountForGlossary();

			_glossariesHelper
				.DeleteSuggestTermInSuggestedTermsPageForAllGlossaries()
				.AssertSuggestedTermsCountMatch(suggestedTermsCountBeforeDelete - 1);
		}

		[Test]
		public void DeleteSuggestedTermWithGlossaryTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToSuggestedTermsPageFromGlossaryPage()
				.DeleteSuggestTermInSuggestedTermsPageForCurrentGlossary(termRowNumber: 1)
				.AssertSuggestedTermsCountMatch(expectedTermCount: 0);
		}

		[Test]
		public void EditFromGlossaryTest()
		{
			var editTerm = "editTerm";

			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToSuggestedTermsPageFromGlossaryPage()
				.EditSuggestTermInSuggestedTermsPageForCurrentGlossary(termRowNumber: 1, termValue: editTerm)
				.GoToGlossariesPage()
				.GoToGlossaryPage(_glossaryName)
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 1)
				.AssertTermMatch(expectedText: editTerm);
		}

		[Test]
		public void EditWithoutGlossaryTest()
		{
			var editTerm = "editTerm";

			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToGlossariesPage()
				.OpenSuggestTermDialogFromGlossariesPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossariesPage()
				.GoToSuggestedTermsPageFromGlossariesPage()
				.EditSuggestTermInSuggestedTermsPageForAllGlossaries(
					glossaryName: "",
					termValue: editTerm,
					chooseGlossary: true,
					glossaryToChoose: _glossaryName)
				.GoToGlossariesPage()
				.GoToGlossaryPage(_glossaryName)
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 1)
				.AssertTermMatch(expectedText: editTerm);
		}

		[Test]
		public void SuggestEmptyTermFromGlossaryListTest()
		{
			_glossariesHelper
				.OpenSuggestTermDialogFromGlossariesPage()
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage(errorExpected: true)
				.AssertEmptyTermErrorDisplayed();
		}

		[Test]
		public void SuggestEmptyTermFromGlossaryTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage(errorExpected: true)
				.AssertEmptyTermErrorDisplayed();
		}

		[Test]
		public void EditFromGlossaryAddSynonymTest()
		{
			var synonym = "synonym";

			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage()
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonInSuggestTermDialogFromGlossaryPage()
				.GoToSuggestedTermsPageFromGlossaryPage()
				.AddSynonimInSuggestedTermsPageForCurrentGlossary(
					termRowNumber: 1,
					addButtonNumber: 2,
					synonymValue: synonym)
				.GoToGlossariesPage()
				.GoToGlossaryPage(_glossaryName)
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 1)
				.AssertSynonymsMatch(new List<string>() { _term2, synonym }, columnNumber: 2);
		}

		[Test]
		public void AutoReverseLanguagesTest()
		{
			_glossariesHelper
				.CreateGlossary(_glossaryName)
				.OpenSuggestTermDialogFromGlossaryPage();

			Assert.AreEqual(Language.English.ToString(), _suggestTermDialog.GetLanguageText(languageNumber: 1),
				"Произошла ошибка:\nНеверный язык №1 в диалоге предложения термина.");

			Assert.AreEqual(Language.Russian.ToString(), _suggestTermDialog.GetLanguageText(languageNumber: 2),
				"Произошла ошибка:\nНеверный язык №2 в диалоге предложения термина.");

			_suggestTermDialog
				.ClickLanguageList(languageNumber: 1)
				.SelectLanguageInList(language: Language.Russian);

			Assert.AreEqual(Language.English.ToString(), _suggestTermDialog.GetLanguageText(languageNumber: 2),
				"Произошла ошибка:\nНеверный язык №2 в диалоге предложения термина.");
		}

		private string _term1;
		private string _term2;
		private string _glossaryName;

		private WorkspaceHelper _workspaceHelper;
		private GlossariesHelper _glossariesHelper;
		private SuggestTermDialog _suggestTermDialog;
	}
}
