using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TMUpdatingTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);
			_tmName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			_workspacePage.GoToTranslationMemoriesPage();
		}

		[Test]
		public void UpdateTMWithCorrectTmx()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			var unitsCountBefore = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(_tmName)
										.GetUnitsCount();

			TranslationMemoriesPage.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.TMTestFile2)
				.ClickImportButtonExpectingReplacementConfirmation();

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			var unitsCountAfter = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(_tmName)
										.GetUnitsCount();

			Assert.AreEqual(unitsCountBefore, unitsCountAfter,
				string.Format("Произошла ошибка:\n количество юнитов не изменилось. Кол-во юнитов до: {0}. Кол-во юнитов после: {1}", unitsCountBefore, unitsCountAfter));
		}

		[Test]
		public void CheckNotificationDuringTmxFileUploading()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.SecondTmFile)
				.ClickImportButtonExpectingReplacementConfirmation();

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-11602")]
		public void UpdateTmxWithoutTmxEndTagTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.WithoutTmxEndTag)
				.ClickImportButtonExpectingReplacementConfirmation();

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTmxWithoutBodyEndTagTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.WithoutBodyEndTag)
				.ClickImportButtonExpectingReplacementConfirmation();

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTmxWithLongSegValueTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.LongSegValue)
				.ClickImportButtonExpectingReplacementConfirmation();

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTmxWithoutTuEndTagTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.WithoutTuEndTag)
				.ClickImportButtonExpectingReplacementConfirmation();

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-3690")]
		public void UpdateTmxWithUnicodeCharactersTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.WithUnicodeCharacters)
				.ClickImportButtonExpectingReplacementConfirmation();

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTxtWithTmxExtensionTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.TxtWithTmxExtension)
				.ClickImportButtonExpectingReplacementConfirmation();

			ConfirmReplacementDialog.ClickConfirmReplacementButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UpdateTMFileWithWrongExtensionTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.DocumentFile)
				.ClickImportButtonExpectingReplacementConfirmation();
				
			ConfirmReplacementDialog.ClickConfirmReplacementButtonExpectingError();

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}

		[Test]
		public void UpdateTMWithoutFileName()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton();

			ImportTmxDialog
				.EnterFileName(string.Empty)
				.ClickImportButtonExpectingReplacementConfirmation()
				.ClickConfirmReplacementButtonExpectingError()
				.ClickImportButtonExpectingError();

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}

		private TranslationMemoriesHelper _translationMemoriesHelper;
		private string _tmName;
		private WorkspacePage _workspacePage;
	}
}
