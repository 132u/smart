using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TmUploadingInvalidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase("withoutTmxEndTag.tmx", Ignore = "PRX-11602")]
		[TestCase("withoutBodyEndTag.tmx")]
		[TestCase("longSegValue.tmx")]
		[TestCase("withoutTuEndTag.tmx")]
		[TestCase("txtWithTmxExtension.tmx")]
		public void UploadIncorrectTmxFileTest(string file)
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName,
				importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(Path.Combine(PathProvider.IncorrectTmxFilesFolder, file));

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}


		[TestCase("docxFile.docx")]
		public void UploadTMFileWithWrongExtensionTest(string file)
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName,
				importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog
				.EnterFileName(Path.Combine(PathProvider.IncorrectTmxFilesFolder, file))
				.ClickImportButtonExpectingError();

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}

		[Test]
		public void UploadTMWithoutFileName()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog
				.EnterFileName(string.Empty)
				.ClickImportButtonExpectingError();

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}
	}
}
