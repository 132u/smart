using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class SearchTmTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(true)]
		[TestCase(false)]
		public void CreateTMSearchTM(bool uploadFile)
		{
			var importFilePath = uploadFile ? PathProvider.TmxFile : null;

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: importFilePath);

			TranslationMemoriesPage
				.CloseAllNotifications<TranslationMemoriesPage>()
				.SearchForTranslationMemory(UniqueTMName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", UniqueTMName);
		}
	}
}
