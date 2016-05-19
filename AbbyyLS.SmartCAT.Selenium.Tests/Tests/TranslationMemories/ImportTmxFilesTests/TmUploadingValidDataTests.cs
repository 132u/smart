using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[TranslationMemories]
	class TmUploadingValidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void UploadTmxToEmptyTMTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName);

			var unitsCountBefore = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(UniqueTMName)
										.GetUnitsCount();

			Assert.AreEqual(0, unitsCountBefore, "Произошла ошибка:\n количество юнитов не равно 0.");

			TranslationMemoriesPage.ClickAddTmxButton();

			ImportTmxDialog
				.ImportTmxFile(PathProvider.OneLineTmxFile)
				.IsFileImportCompleteNotifierDisplayed();

			// костыль PRX-13801
			WorkspacePage.RefreshPage<WorkspacePage>();

			var unitsCountAfter = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(UniqueTMName)
										.GetUnitsCount();

			Assert.AreNotEqual(unitsCountBefore, unitsCountAfter,
				"Произошла ошибка:\n количество юнитов не изменилось");
		}

		[Test]
		public void UploadTmxToExistingTMWithTmxTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			var unitsCountBefore = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(UniqueTMName)
										.GetUnitsCount();

			TranslationMemoriesPage.ClickAddTmxButton();

			ImportTmxDialog.ImportTmxFile(PathProvider.TmxFile);

			// костыль PRX-13801
			WorkspacePage.RefreshPage<WorkspacePage>();

			var unitsCountAfter = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(UniqueTMName)
										.GetUnitsCount();

			Assert.AreNotEqual(unitsCountBefore, unitsCountAfter,
				string.Format("Произошла ошибка:\n количество юнитов не изменилось. Кол-во юнитов до: {0}. Кол-во юнитов после: {1}", unitsCountBefore, unitsCountAfter));
		}

		[Test]
		public void CheckNotificationDuringTmxFileUploading()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(PathProvider.SecondTmxFile);

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-3690")]
		public void UploadTmxWithUnicodeCharactersTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(PathProvider.WithUnicodeCharacters);

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}
	}
}
