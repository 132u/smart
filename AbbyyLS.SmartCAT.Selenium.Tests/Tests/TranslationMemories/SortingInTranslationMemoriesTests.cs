using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	internal class SortingInTranslationMemoriesTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInTranslationMemoriesTests()
		{
			_translationMemoriesHelper.GoToTranslationMemoriesPage();
		}

		[Test]
		public void SortByTMNameTest()
		{
			_translationMemoriesHelper
				.ClickSortByTMName()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByCreationDateTest()
		{
			_translationMemoriesHelper
				.ClickSortByCreationDate()
				.AssertAlertNoExist();
		}

		private TranslationMemoriesHelper _translationMemoriesHelper = new TranslationMemoriesHelper();
	}
}
