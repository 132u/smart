using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TmUpdatingInvalidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase("withoutTmxEndTag.tmx", Ignore = "PRX-11602")]
		[TestCase("withoutBodyEndTag.tmx")]
		[TestCase("longSegValue.tmx")]
		[TestCase("withoutTuEndTag.tmx")]
		[TestCase("txtWithTmxExtension.tmx")]
		public void UpdateTmWithIncorrectTmxFileTest(string file)
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName,
				importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(
				Path.Combine(PathProvider.IncorrectTmxFilesFolder, file));

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[TestCase("docxFile.docx")]
		public void UpdateTMImportValidation(string file)
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName,
				importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage.OpenImportTmxDialog(UniqueTMName, update: true);

			ImportTmxDialog.ImportTmxFileExpectingConfirmation(
				Path.Combine(PathProvider.IncorrectTmxFilesFolder, file));

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

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}
	}
}
