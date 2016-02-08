using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class EditTmInvalidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(true, "")]
		[TestCase(true, " ")]
		[TestCase(false, "")]
		[TestCase(false, " ")]
		public void EditTMSaveWithInvalidNameTest(bool needUploadTmx, string invalidName)
		{
			var importFilePath = needUploadTmx ? PathProvider.TmxFile : null;

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: importFilePath);

			TranslationMemoriesPage.EditTranslationMemory(UniqueTMName, renameTo: invalidName, isErrorExpecting: true);

			Assert.IsTrue(TranslationMemoriesPage.IsEmptyNameErrorMessageDisplayed(),
				"Ошибка: не появилось сообщение о пустом названии при редактировании ТМ");
		}

		[Test, Ignore("PRX-14254")]
		public void EditTMSaveExistingNameTest()
		{
			var secondTranslationMemoryName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTMName)
				.CreateTranslationMemory(secondTranslationMemoryName);

			TranslationMemoriesPage.EditTranslationMemory(secondTranslationMemoryName, renameTo: UniqueTMName, isErrorExpecting: true);

			Assert.IsTrue(TranslationMemoriesPage.IsExistingNameErrorMessageDisplayed(),
				"Ошибка: не появилось сообщение об ошибки имени при редактировании ТМ");
		}
	}
}
