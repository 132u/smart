using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[TranslationMemories]
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
			var tmxFile = PathProvider.GetUniqueFilePath(Path.Combine(PathProvider.IncorrectTmxFilesFolder, file));

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName,
				importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog.ImportTmxFile(tmxFile);

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}


		[TestCase("docxFile.docx")]
		public void UploadTMFileWithWrongExtensionTest(string file)
		{
			var tmxFile = PathProvider.GetUniqueFilePath(Path.Combine(PathProvider.IncorrectTmxFilesFolder, file));

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName,
				importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: false);

			ImportTmxDialog
				.EnterFileName(tmxFile)
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
