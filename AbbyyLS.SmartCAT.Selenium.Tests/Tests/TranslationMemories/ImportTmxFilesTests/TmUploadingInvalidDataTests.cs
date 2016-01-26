using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories.ImportTmxFilesTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TmUploadingInvalidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{

		[Test, Ignore("PRX-11602")]
		public void UploadTmxWithoutTmxEndTagTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(PathProvider.WithoutTmxEndTag);

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UploadTmxWithoutBodyEndTagTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(PathProvider.WithoutBodyEndTag);

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UploadTmxWithLongSegValueTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(PathProvider.LongSegValue);

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UploadTmxWithoutTuEndTagTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(PathProvider.WithoutTuEndTag);

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UploadTxtWithTmxExtensionTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(PathProvider.TxtWithTmxExtension);

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UploadTMFileWithWrongExtensionTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(PathProvider.DocumentFile);

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}

		[Test]
		public void UploadTMWithoutFileName()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(string.Empty);

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}
	}
}
