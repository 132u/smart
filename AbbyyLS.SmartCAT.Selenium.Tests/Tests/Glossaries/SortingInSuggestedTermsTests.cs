using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Standalone]
	class SortingInSuggestedTermsTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInSuggestedTermsTests()
		{
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

		private GlossariesHelper _glossariesHelper = new GlossariesHelper();
	}
}
