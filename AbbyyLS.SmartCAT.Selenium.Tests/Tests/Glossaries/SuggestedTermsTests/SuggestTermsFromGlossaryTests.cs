using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class SuggestTermsFromGlossaryTests<TWebDriverProvider>
		: SuggestTermsBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void SuggestTermWithGlossaryFromGlossaryPageTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(glossary: _glossaryName)
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.AreEqual(1, _suggestedTermsPageForAllGlossaries.GetTermsByGlossaryNameCount(_glossaryName),
				"Произошла Ошибка:\n Неверное количество терминов.");
		}

		[Test]
		public void SuggestTermWithGlossaryFromAnotherGlossaryTest()
		{
			var glossaryName2 = GlossariesHelper.UniqueGlossaryName();

			_glossariesHelper.CreateGlossary(_glossaryName);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(glossaryName2);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(glossary: _glossaryName)
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.AreEqual(1, _suggestedTermsPageForAllGlossaries.GetTermsByGlossaryNameCount(_glossaryName),
				"Произошла Ошибка:\n Неверное количество терминов.");
		}

		[Test]
		public void SuggestTermWithoutGlossaryFromAnotherGlossaryTest()
		{
			_glossariesPage.ClickSuggestedTermsButton();

			var suggestedTermsByGlossaryCountBefore = _suggestedTermsPageForAllGlossaries.GetTermsByGlossaryNameCount();

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(glossary: "")
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.AreEqual(suggestedTermsByGlossaryCountBefore + 1, _suggestedTermsPageForAllGlossaries.GetTermsByGlossaryNameCount(),
				"Произошла Ошибка:\n Неверное количество терминов.");
		}

		[Test]
		public void SuggestExistingTermWarningFromGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage
				.CreateTerm(_term1, _term2)
				.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_suggestTermDialog.IsDublicateErrorDisplayed(),
				"Произошла ошибка:\n сообщение о том, что такой термин уже существует, не появилось");

			_suggestTermDialog.ClickCancelButtonExpectingGlossaryPage();

			_glossaryPage.ClickSuggestedTermsButton();

			Assert.AreEqual(0, _suggestedTermsPageForCurrentGlossaries.GetSuggestedTermsCount(),
				"Произошла ошибка:\nНеверное количество терминов.");
		}

		[Test]
		public void SuggestExistingTermAcceptFromGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage
				.CreateTerm(_term1, _term2)
				.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_suggestTermDialog.IsDublicateErrorDisplayed(),
				"Произошла ошибка:\n сообщение о том, что такой термин уже существует, не появилось");

			_suggestTermDialog.ClickSaveButtonExpectingGlossaryPage();

			_glossaryPage.ClickSuggestedTermsButton();

			Assert.IsTrue(_suggestedTermsPageForCurrentGlossaries.IsTermValueMatchExpected(
				expectedTermValue: _term1, rowNumber: 1, columnNumber: 1),
				"Произошла ошибка:\nНеверное значение в термине");

			Assert.IsTrue(_suggestedTermsPageForCurrentGlossaries.IsTermValueMatchExpected(
				expectedTermValue: _term2, rowNumber: 1, columnNumber: 2),
				"Произошла ошибка:\nНеверное значение в термине");

			Assert.AreEqual(1, _suggestedTermsPageForCurrentGlossaries.GetSuggestedTermsCount(),
				"Произошла ошибка:\nНеверное количество терминов.");
		}

		[Test]
		public void AcceptSuggestedTermWithGlossaryFromGlossaryPageTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossaryPage();

			_glossaryPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForCurrentGlossaries
				.AcceptSuggestTermInSuggestedTermsPageForCurrentGlossary(termRowNumber: 1);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.AreEqual(_term1, _glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void AcceptSuggestedTermWithGlossaryFromAnotherGlossaryPageTest()
		{
			var glossaryName2 = GlossariesHelper.UniqueGlossaryName();

			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(glossaryName2);

			_glossaryPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForCurrentGlossaries
				.SelectGlossaryInSuggestedTermsPageForCurrentGlossary(_glossaryName)
				.AcceptSuggestTermInSuggestedTermsPageForCurrentGlossary(termRowNumber: 1);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.AreEqual(_term1, _glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test, Ignore("PRX-14852")]
		public void DeleteSuggestedTermWithGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossaryPage();

			_glossaryPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForCurrentGlossaries
				.DeleteSuggestTermInSuggestedTermsPageForCurrentGlossary(termRowNumber: 1);

			Assert.AreEqual(0, _suggestedTermsPageForCurrentGlossaries.GetSuggestedTermsCount(),
				"Произошла ошибка:\nНеверное количество терминов.");
		}

		[Test]
		[Ignore("PRX-13437")]
		public void SuggestEmptyTermFromGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage
				.ClickSuggestTermButton()
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_suggestTermDialog.IsEmptyTermErrorDisplayed(),
				"Произошла ошибка:\nCообщение 'Enter at least one term.' не появилось.");
		}

		[Test]
		public void EditFromGlossaryTest()
		{
			var editTerm = "editTerm";

			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossaryPage();

			_glossaryPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForCurrentGlossaries
				.EditSuggestTermInSuggestedTermsPageForCurrentGlossary(termRowNumber: 1, termValue: editTerm);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.AreEqual(editTerm, _glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void EditFromGlossaryAddSynonymTest()
		{
			var synonym = "synonym";

			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossaryPage();

			_glossaryPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForCurrentGlossaries.AddSynonimInSuggestedTermsPageForCurrentGlossary(
					termRowNumber: 1, addButtonNumber: 2, synonymValue: synonym);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(_glossaryPage.IsTermContainsExpectedSynonyms(columnNumber: 2, synonyms: new List<string> { _term2, synonym }),
				"Произошла ошибка:\nНеверный список синонимов");
		}

		[Test]
		public void AutoReverseLanguagesTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

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
	}
}
