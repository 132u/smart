using System;
using System.Globalization;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Glossaries]
	class SuggestTermsFromGlossariesTests<TWebDriverProvider>
		: SuggestTermsBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7289")]
		public void SuggestTermWithoutGlossaryTest()
		{
			var date = DateTime.UtcNow.ToString("MM/dd/yyyy", new CultureInfo("en-US"));

			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(_term1, _term2)
				.ClickSaveButtonExpectingGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.IsTrue(_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином не найдена в списке");

			Assert.AreEqual(ThreadUser.FullName,
				_suggestedTermsPageForAllGlossaries.GetSuggestedTermAuthor(_term1, _term2),
				"Произошла ошибка:\n неверно указан автор термина ({0} вместо {1})");

			Assert.AreEqual(date,
				_suggestedTermsPageForAllGlossaries.GetSuggestedTermDate(_term1, _term2),
				"Произошла ошибка:\n неверно указана дата");
		}

		[Test, Description("S-7289")]
		public void SuggestTermWithGlossaryTest()
		{
			var date = DateTime.UtcNow.ToString("MM/dd/yyyy", new CultureInfo("en-US"));

			_glossariesHelper.CreateGlossary(_glossaryName);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(_term1, _term2, glossary: _glossaryName)
				.ClickSaveButtonExpectingGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.IsTrue(_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином не найдена в списке");

			Assert.AreEqual(ThreadUser.FullName,
				_suggestedTermsPageForAllGlossaries.GetSuggestedTermAuthor(_term1, _term2),
				"Произошла ошибка:\n неверно указан автор термина ({0} вместо {1})");

			Assert.AreEqual(date,
				_suggestedTermsPageForAllGlossaries.GetSuggestedTermDate(_term1, _term2),
				"Произошла ошибка:\n неверно указана дата");
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

			Assert.IsFalse(_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином найдена в списке");
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

			Assert.IsTrue(_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином не найдена в списке");
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

			_suggestedTermsPageForAllGlossaries.AcceptSuggestedTerm(_term1, _term2);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(_term1, _term2),
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
				.AcceptSuggestedTermExpectingSelectGlossaryDialog(_term1, _term2);

			_selectGlossaryDialog.SelectGlossaryForSuggestedTerm(_glossaryName);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(_term1, _term2),
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

			_suggestedTermsPageForAllGlossaries.DeleteSuggestedTerm(_term1, _term2);

			Assert.IsFalse(_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином найдена в списке");
		}

		[Test]
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
			var editTerm1 = "editTerm-" + Guid.NewGuid();
			var editTerm2 = "editTerm-" + Guid.NewGuid();

			_glossariesHelper.CreateGlossary(_glossaryName);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: _term1, term2: _term2)
				.ClickSaveButtonExpectingGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForAllGlossaries
				.EditSuggestedTermExpectingSelectGlossaryDialog(_term1, _term2);

			_selectGlossaryDialog.SelectGlossaryForSuggestedTerm(_glossaryName);

			_suggestedTermsPageForAllGlossaries
				.FillSuggestedTermInEditMode(termNumber: 1, termValue: editTerm1)
				.FillSuggestedTermInEditMode(termNumber: 2, termValue: editTerm2)
				.ClickAcceptTermButtonInEditMode();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryName);

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(editTerm1, editTerm2),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}
	}
}
