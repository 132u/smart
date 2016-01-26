using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TmUpdatingInvalidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Ignore("PRX-11602")]
		public void UpdateTmxWithoutTmxEndTagTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(PathProvider.WithoutTmxEndTag);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTmxWithoutBodyEndTagTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(PathProvider.WithoutBodyEndTag);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTmxWithLongSegValueTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(PathProvider.LongSegValue);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTmxWithoutTuEndTagTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(PathProvider.WithoutTuEndTag);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTxtWithTmxExtensionTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(PathProvider.TxtWithTmxExtension);

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTMFileWithWrongExtensionTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(PathProvider.DocumentFile);

			ConfirmReplacementDialog.ClickConfirmReplacementButtonExpectingError();

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}

		[Test]
		public void UpdateTMWithoutFileName()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(string.Empty);

			ConfirmReplacementDialog.ClickConfirmReplacementButtonExpectingError();

			ImportTmxDialog.ClickImportButtonExpectingError();

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}
	}
}
