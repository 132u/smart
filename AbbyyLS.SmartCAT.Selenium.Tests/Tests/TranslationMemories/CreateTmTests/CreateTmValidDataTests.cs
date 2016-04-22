using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[TranslationMemories]
	class CreateTmValidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCaseSource("TranslationMemoryNamesList")]
		public void CreateNewTmTest(string tmName)
		{
			TranslationMemoriesHelper.CreateTranslationMemory(tmName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(tmName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", tmName);
		}

		[Test]
		public void CancelNewTmCreation()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTMName, finalButtonType: DialogButtonType.Cancel);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", UniqueTMName);
		}

		private static readonly string[] TranslationMemoryNamesList =
		{
			"TestTM", 
			"Тестовая ТМ", 
			"我喜爱的哈伯尔阿哈伯尔"
		};
	}
}
