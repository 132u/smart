using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TmUpdatingValidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void UpdateTMWithCorrectTmx()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			var unitsCountBefore = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(UniqueTMName)
										.GetUnitsCount();

			TranslationMemoriesPage.ClickUpdateTmButton();

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(PathProvider.TMTestFile2);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			var unitsCountAfter = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(UniqueTMName)
										.GetUnitsCount();

			Assert.AreEqual(unitsCountBefore, unitsCountAfter,
				string.Format("Произошла ошибка:\n количество юнитов не изменилось. Кол-во юнитов до: {0}. Кол-во юнитов после: {1}", unitsCountBefore, unitsCountAfter));
		}

		[Test]
		public void CheckNotificationDuringTmxFileUploading()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(PathProvider.SecondTmFile);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-3690")]
		public void UpdateTmxWithUnicodeCharactersTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(PathProvider.WithUnicodeCharacters);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}
	}
}
