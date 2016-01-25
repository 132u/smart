using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	class SearchTermsTests<TWebDriverSettings>
		: BaseGlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_searchPage = new SearchPage(Driver);

			_glossariesHelper.CreateGlossary(_glossaryUniqueName);
		}

		[Test]
		public void SearchTermByFirstLanguageTest()
		{
			var uniqueText = DateTime.UtcNow.Ticks + "1Term";
			var term1 = "Term1";
			var term2 = "Term2";
			var term3 = "Term3 " + uniqueText;
			var term4 = "Term4 " + DateTime.UtcNow;

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

			_glossaryPage.CreateTerm(firstTerm, secondTerm + DateTime.UtcNow.Ticks);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(glossaryUniqueName2);

			_glossaryPage.CreateTerm(firstTerm, secondTerm + DateTime.UtcNow.Ticks);

			_workspacePage.GoToSearchPage();

			_searchPage.InitSearch(uniqueData);

			Assert.IsTrue(_searchPage.IsGlossariesNamesMatch(glossaryList),
				"Произошла ошибка:\n списки имен глоссариев не совпадают.");

			Assert.IsTrue(_searchPage.IsTermNamesMatch(firstTerm),
				"Произошла ошибка:\n найденные термины не совпадает с заданным термином '{0}'.", firstTerm);
		}

		private SearchPage _searchPage;
	}
}
