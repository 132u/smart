using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	class CreateTermTests<TWebDriverSettings>
		: BaseGlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_termAlreadyExistsDialog = new TermAlreadyExistsDialog(Driver);

			_glossariesHelper.CreateGlossary(_glossaryUniqueName);
		}
		
		[Test]
		public void NewEntryEditModeTest()
		{
			_glossaryPage.ClickNewEntryButton();

			Assert.IsTrue(_glossaryPage.IsCreationModeActivated(),
				"Произошла ошибка:\n новый термин не открыт в расширенном режиме");
		}


		[Test]
		public void CreateDefaultTermTest()
		{
			_glossaryPage.CreateTerm();

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");
		}

		[Test]
		public void CreateCustomTermTest()
		{
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
		public void CreateCustomTermInMultiLanguageGlossaryTest()
		{
			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.AddLangauge(Language.German)
				.AddLangauge(Language.French)
				.ClickSaveButton();

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddNewSystemField(GlossarySystemField.Topic);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton()
				.CloseExpandedTerms();

			Assert.AreEqual(4, _glossaryPage.LanguageColumnCount(),
				"Произошла ошибка:\n неверное количество колонок с языками.");

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void CancelCreateTermTest()
		{
			_glossaryPage
				.ClickNewEntryButton()
				.FillAllTerm()
				.ClickCancelButton();

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 0),
				"Произошла ошибка:\n неверное количество терминов");
		}

		[Test]
		public void DeleteLanguageExistTermTest()
		{
			_glossaryPage
				.CreateTerm()
				.OpenGlossaryProperties();

			var languagesCountBefore = _glossaryPropertiesDialog.LanguagesCount();

			_glossaryPropertiesDialog.CancelDeleteLanguageInPropertiesDialog();

			var languagesCountAfter = _glossaryPropertiesDialog.LanguagesCount();

			Assert.AreEqual(languagesCountBefore, languagesCountAfter,
				"Произошла ошибка:\n количество языков {0} не совпадает с {1} в диалоге свойств глоссария.",
				languagesCountBefore, languagesCountAfter);
		}

		private TermAlreadyExistsDialog _termAlreadyExistsDialog;
	}
}
