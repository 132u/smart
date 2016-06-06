using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[TranslationMemories]
	class TmUpdatingValidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void UpdateTMWithCorrectTmx()
		{
			var tmxFile = PathProvider.TmxFile;

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: _tmx);

			var unitsCountBefore = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(UniqueTMName)
										.GetUnitsCount();

			TranslationMemoriesPage.ClickUpdateTmButton();

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(tmxFile);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			var unitsCountAfter = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(UniqueTMName)
										.GetUnitsCount();

			Assert.AreNotEqual(unitsCountBefore, unitsCountAfter,
				string.Format("Произошла ошибка:\n количество юнитов не изменилось. Кол-во юнитов до: {0}. Кол-во юнитов после: {1}", unitsCountBefore, unitsCountAfter));
		}

		[Test]
		public void CheckNotificationDuringTmxFileUploading()
		{
			var tmxFile = PathProvider.SecondTmxFile;

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: _tmx);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(tmxFile);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-3690")]
		public void UpdateTmxWithUnicodeCharactersTest()
		{
			var tmxWithUnicodeCharacters = PathProvider.WithUnicodeCharacters;

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: _tmx);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(tmxWithUnicodeCharacters);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}
	}
}
