using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Glossaries]
	class FilterTermsByLanguageTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void FilterTermsByLanguageTestsSetUp()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.CreateTerm();
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
			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.AddLangauge(Language.French)
				.AddLangauge(Language.German)
				.ClickSaveButton();

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

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.AddLangauge(Language.French)
				.ClickSaveButton();

			_glossaryPage.ClickFilterButton();

			var languagesFilterAfter = _filterDialog.SelectedLanguagesInDropdown();

			Assert.AreEqual(languagesFilterBefore.Count + 1, languagesFilterAfter.Count,
				"Произошла ошибка:\nНеверное количество языков в фильтре.");
		}

		[Test]
		public void DeletedLanguageFilterTest()
		{
			_glossaryPage.ClickFilterButton();

			var languagesFilterBefore = _filterDialog.SelectedLanguagesInDropdown();

			_filterDialog.ClickApplyButton();

			_glossaryPage.OpenGlossaryProperties();

			_newGlossaryDialog.ClickDeleteLanguageButton();

			_glossaryPropertiesDialog.ClickSaveButton();

			_glossaryPage.ClickFilterButton();

			var languagesFilterAfter = _filterDialog.SelectedLanguagesInDropdown();

			Assert.AreEqual(languagesFilterBefore.Count - 1, languagesFilterAfter.Count,
				"Произошла ошибка:\nНеверное количество языков в фильтре.");
		}
	}
}
