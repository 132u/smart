using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	internal class SortingInTmTests<TWebDriverSettings> : BaseTmTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[Test]
		public void SortByTMNameTest()
		{
			TranslationMemoriesPage.ClickSortByTMNameAssumingAlert();

			Assert.IsFalse(TranslationMemoriesPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCreationDateTest()
		{
			TranslationMemoriesPage.ClickSortByCreationDateAssumingAlert();

			Assert.IsFalse(TranslationMemoriesPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}
	}
}
