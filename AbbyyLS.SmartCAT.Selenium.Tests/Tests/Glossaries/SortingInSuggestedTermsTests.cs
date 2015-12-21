using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class SortingInSuggestedTermsTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInSuggestedTermsTests()
		{
			_glossaryPage = new GlossaryPage(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);

			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_glossariesHelper
				.GoToGlossariesPage()
				.CreateGlossary(glossaryUniqueName);
		}

		[Test]
		public void SortByEnglishTermTest()
		{
			_glossaryPage.ClickSortByEnglishTermAssumingAlert();

			Assert.IsFalse(_glossaryPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByRussianTermTest()
		{
			_glossaryPage.ClickSortByRussianTermAssumingAlert();

			Assert.IsFalse(_glossaryPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortTermsToDateModifiedTest()
		{
			_glossaryPage.ClickSortByDateModifiedAssumingAlert();

			Assert.IsFalse(_glossaryPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		private GlossariesHelper _glossariesHelper;
		private GlossaryPage _glossaryPage;
	}
}
