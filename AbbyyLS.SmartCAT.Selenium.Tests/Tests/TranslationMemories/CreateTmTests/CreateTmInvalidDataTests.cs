using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class CreateTmInvalidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateTMWithoutNameTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(string.Empty, isCreationErrorExpected: true);

			Assert.IsTrue(NewTranslationMemoryDialog.IsEmptyNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилась ошибка создания ТМ с пустым именем");
		}

		[Test]
		public void CreateTMWithExistingNameTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTMName)
				.CreateTranslationMemory(UniqueTMName, isCreationErrorExpected: true);

			Assert.IsTrue(NewTranslationMemoryDialog.IsExistingNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилась ошибка создания ТМ с существующим именем");
		}

		[Test]
		public void CreateTMWithoutLanguageTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(
					UniqueTMName,
					targetLanguage: Language.NoLanguage,
					isCreationErrorExpected: true);

			Assert.IsTrue(NewTranslationMemoryDialog.IsNoTargetErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилась ошибка при создании ТМ без языка перевода");
		}

		[Test]
		public void CreateTMWithNotTmxFileTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(
					UniqueTMName,
					importFilePath: PathProvider.DocumentFile,
					finalButtonType: DialogButtonType.None,
					isCreationErrorExpected: true);

			Assert.IsTrue(NewTranslationMemoryDialog.IsWrongFormatErrorDisplayed(),
				"Произошла ошибка:\n не появилась ошибка о загрузке файла с неподходящим расширением (не TMX файл)");
		}
	}
}
