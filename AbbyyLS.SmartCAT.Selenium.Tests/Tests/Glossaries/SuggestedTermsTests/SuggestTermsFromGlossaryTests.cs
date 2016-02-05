using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Category("QWERTY")]
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
				.FillSuggestTermDialog(_term1, _term2, glossary: _glossaryName)
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.IsTrue(_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином не найдена в списке");
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
				.FillSuggestTermDialog(_term1, _term2, glossary: _glossaryName)
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.IsTrue(_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином не найдена в списке");
		}

		[Test]
		public void SuggestTermWithoutGlossaryFromGlossaryTest()
		{
			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(_term1, _term2, glossary: "")
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.IsTrue(_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином не найдена в списке");
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

			Assert.IsFalse(_suggestedTermsPageForCurrentGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином найдена в списке");
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

			Assert.IsTrue(_suggestedTermsPageForCurrentGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином не найдена в списке");
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

			_suggestedTermsPageForCurrentGlossaries.AcceptSuggestedTerm(_term1, _term2);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(_term1, _term2),
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
				.SelectGlossary(_glossaryName)
				.AcceptSuggestedTerm(_term1, _term2);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(_term1, _term2),
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

			_suggestedTermsPageForCurrentGlossaries.DeleteSuggestedTerm(_term1, _term2);

			Assert.IsFalse(_suggestedTermsPageForCurrentGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином найдена в списке");
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
			var editTerm1 = "editTerm-" + Guid.NewGuid();
			var editTerm2 = "editTerm-" + Guid.NewGuid();

			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossaryPage();

			_glossaryPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForCurrentGlossaries.EditSuggestedTerm(_term1, _term2, editTerm1, editTerm2);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(editTerm1, editTerm2),
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

			_suggestedTermsPageForCurrentGlossaries.AddSynonim(
				_term1, _term2, addButtonNumber: 2, synonymValue: synonym);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(_glossaryPage.IsTermContainsExpectedSynonyms(
				columnNumber: 2, synonyms: new List<string> { _term2, synonym }),
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
