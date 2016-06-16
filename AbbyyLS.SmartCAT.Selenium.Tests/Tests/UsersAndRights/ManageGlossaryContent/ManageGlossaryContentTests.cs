using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossaryContent
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageGlossaryContentTests<TWebDriverProvider> : ManageGlossaryContentBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void GlossariesViewTest()
		{
			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossaryUniqueName),
				"Произошла ошибка: глоссарий {0} отсутствует в списке.", _glossaryUniqueName);

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossaryUniqueName2),
				"Произошла ошибка: глоссарий {0} отсутствует в списке.", _glossaryUniqueName2);
		}

		[Test]
		public void GlossariesConceptsViewTest()
		{
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			Assert.IsTrue(
				_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка: Глоссарий {0} содержит неверное количество терминов.", _glossaryUniqueName);
			
			Assert.AreEqual(_glossaryPage.GetTermText(), _term1,
				"Произошла ошибка: неверный термин.");

			Assert.AreEqual(_glossaryPage.GetTermText(termNumber: 2), _term2,
				"Произошла ошибка: неверный термин.");

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName2);

			Assert.IsTrue(
				_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка: Глоссарий {0} содержит неверное количество терминов.", _glossaryUniqueName2);

			Assert.AreEqual(_glossaryPage.GetTermText(), _term3,
				"Произошла ошибка: неверный термин.");

			Assert.AreEqual(_glossaryPage.GetTermText(termNumber: 2), _term4,
				"Произошла ошибка: неверный термин.");
		}

		[Test]
		public void ExportGlossariesTest()
		{
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			_glossaryPage.ClickExportGlossary();

			_exportNotification.ClickDownloadNotifier<GlossaryPage>();

			Assert.IsTrue(_glossaryPage.IsGlossaryExportedSuccesfully(Path.Combine(PathProvider.ExportFiles, _glossaryUniqueName.Replace(":", "-") + ".xlsx")),
				"Произошла ошибка:\n файл не был скачан за отведенное время");

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName2);

			_glossaryPage.ClickExportGlossary();

			Assert.IsTrue(_glossaryPage.IsGlossaryExportedSuccesfully(Path.Combine(PathProvider.ExportFiles, _glossaryUniqueName.Replace(":", "-") + ".xlsx")),
				"Произошла ошибка:\n файл не был скачан за отведенное время");
		}

		[Test, Ignore("PRX-16975")]
		public void AddGlossaryLanguagesTest()
		{
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName2);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.AddLangauge(Language.German)
				.AddLangauge(Language.French)
				.ClickSaveButton();

			Assert.AreEqual(4, _glossaryPage.LanguageColumnCount(),
				"Произошла ошибка:\n неверное количество колонок с языками.");
		}

		[Test, Ignore("PRX-16975")]
		public void DeleteGlossaryLanguagesTest()
		{
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.ClickDeleteLanguageButton(1)
				.ClickSaveButton();

			Assert.AreEqual(1, _glossaryPage.LanguageColumnCount(),
				"Произошла ошибка:\n неверное количество колонок с языками.");
		}

		[Test, Ignore("PRX-16975")]
		public void EditGlossaryLanguagesTest()
		{
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName3);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.EditLangauge(Language.French, 1)
				.ClickSaveButton();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName3);

			_glossaryPage.OpenGlossaryProperties();

			Assert.AreEqual(Language.French.ToString(), _glossaryPropertiesDialog.GetLanguage(languageNumber: 1),
				"Произошла ошибка: Указан неверно язык в дропдауне №1");
		}

		[Test]
		public void SuggestTermWithGlossaryFromGlossaryPageTest()
		{
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName3);

			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(_term1, _term2, glossary: _glossaryUniqueName3, defaultLanguages:true)
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.IsTrue(_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(_term1, _term2),
				"Произошла ошибка: строка с термином не найдена в списке");
		}
	}
}
