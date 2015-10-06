using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
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
			_glossariesHelper = new GlossariesHelper(Driver);

			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_glossariesHelper
				.GoToGlossariesPage()
				.CreateGlossary(glossaryUniqueName);
		}

		[Test]
		public void SortByEnglishTermTest()
		{
			_glossariesHelper
				.ClickSortByEnglishTerm()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByRussianTermTest()
		{
			_glossariesHelper
				.ClickSortByRussianTerm()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortTermsToDateModifiedTest()
		{
			_glossariesHelper
				.ClickSortTermsToDateModified()
				.AssertAlertNoExist();
		}

		private GlossariesHelper _glossariesHelper;
	}
}
