using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class SuggestTermsFromGlossariesTests<TWebDriverProvider>
		: SuggestTermsBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void SuggestTermWithoutGlossaryTest()
		{
			_glossariesPage.ClickSuggestedTermsButton();

			var suggestedTermsByGlossaryCountBefore = _suggestedTermsPageForAllGlossaries.GetTermsByGlossaryNameCount();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog()
				.ClickSaveButtonExpectingGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.AreEqual(suggestedTermsByGlossaryCountBefore + 1, _suggestedTermsPageForAllGlossaries.GetTermsByGlossaryNameCount(),
				"Произошла Ошибка:\n Неверное количество терминов.");
		}

		[Test]
		public void SuggestTermWithGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(glossary: _glossaryName)
				.ClickSaveButtonExpectingGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.AreEqual(1, _suggestedTermsPageForAllGlossaries.GetTermsByGlossaryNameCount(_glossaryName),
				"Произошла Ошибка:\n Неверное количество терминов.");
		}

		[Test]
		public void SuggestExistingTermWarningFromGlossaryListTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.CreateTerm(_term1, _term2);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2, glossary: _glossaryName)
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_suggestTermDialog.IsDublicateErrorDisplayed(),
				"Произошла ошибка:\n сообщение о том, что такой термин уже существует, не появилось");

			_suggestTermDialog.ClickCancelButtonExpectingGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			_glossaryPage.ClickSuggestedTermsButton();

			Assert.AreEqual(0, _suggestedTermsPageForCurrentGlossaries.GetSuggestedTermsCount(),
				"Произошла ошибка:\nНеверное количество терминов.");
		}

		[Test]
		public void SuggestExistingTermAcceptFromGlossaryListTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.CreateTerm(_term1, _term2);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2, glossary: _glossaryName)
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_suggestTermDialog.IsDublicateErrorDisplayed(),
				"Произошла ошибка:\n сообщение о том, что такой термин уже существует, не появилось");

			_suggestTermDialog.ClickSaveTermAnywayButtonExpectingGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

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
		public void AcceptSuggestedTermWithGlossaryFromGlossariesPageTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForAllGlossaries
				.AcceptSuggestTermInSuggestedTermsPageForAllGlossaries(glossaryName: _glossaryName);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.AreEqual(_term1, _glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void AcceptSuggestedTermWithoutGlossaryFromGlossariesPageTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryName);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForAllGlossaries
				.AcceptSuggestTermExpectingSelectGlossaryDialog();

			_selectGlossaryDialog.SelectGlossaryForSuggestedTerm(_glossaryName);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.AreEqual(_term1, _glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void DeleteSuggestedTermWithoutGlossaryTest()
		{
			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			var suggestedTermsCountBeforeDelete = _suggestedTermsPageForAllGlossaries.GetTermRowNumberByGlossaryName();

			_suggestedTermsPageForAllGlossaries.DeleteSuggestTermInSuggestedTermsPageForAllGlossaries();

			Assert.AreEqual(suggestedTermsCountBeforeDelete - 1,
				_suggestedTermsPageForCurrentGlossaries.GetSuggestedTermsCount(),
				"Произошла ошибка:\nНеверное количество терминов.");
		}

		[Test]
		[Ignore("PRX-13437")]
		public void SuggestEmptyTermFromGlossaryListTest()
		{
			_glossariesPage
				.ClickSuggestTermButton()
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_suggestTermDialog.IsEmptyTermErrorDisplayed(),
				"Произошла ошибка:\nCообщение 'Enter at least one term.' не появилось.");
		}

		[Test]
		public void EditWithoutGlossaryTest()
		{
			var editTerm = "editTerm";

			_glossariesHelper.CreateGlossary(_glossaryName);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForAllGlossaries
				.EditSuggestTermInSuggestedTermsPageForAllGlossaries();

			_selectGlossaryDialog.SelectGlossaryForSuggestedTerm(_glossaryName);

			_suggestedTermsPageForAllGlossaries
				.FillSuggestedTermInEditMode(termNumber: 1, termValue: editTerm)
				.FillSuggestedTermInEditMode(termNumber: 2, termValue: editTerm)
				.ClickAcceptTermButtonInEditMode();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.AreEqual(editTerm, _glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}
	}
}
