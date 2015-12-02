using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TMUploadingTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);
			_workspaceHelper.GoToTranslationMemoriesPage();
			_tmName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
		}

		[Test]
		public void UploadTmxToEmptyTMTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName);

			var unitsCountBefore = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(_tmName)
										.GetUnitsCount();

			Assert.AreEqual(0, unitsCountBefore, "Произошла ошибка:\n количество юнитов не равно 0.");

			TranslationMemoriesPage.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.OneLineTmxFile)
				.ClickImportButton();

			var unitsCountAfter = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(_tmName)
										.GetUnitsCount();

			Assert.AreNotEqual(unitsCountBefore, unitsCountAfter,
				string.Format("Произошла ошибка:\n количество юнитов не изменилось."));
		}

		[Test]
		public void UploadTmxToExistingTMWithTmxTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			var unitsCountBefore = TranslationMemoriesPage
										.OpenTranslationMemoryInformation(_tmName)
										.GetUnitsCount();

			TranslationMemoriesPage.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.TMTestFile2)
				.ClickImportButton();

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

			TranslationMemoriesPage.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.SecondTmFile)
				.ClickImportButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-11602")]
		public void UploadTmxWithoutTmxEndTagTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.WithoutTmxEndTag)
				.ClickImportButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-11603")]
		public void UploadTmxWithoutBodyEndTagTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.WithoutBodyEndTag)
				.ClickImportButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-11603")]
		public void UploadTmxWithLongSegValueTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.LongSegValue)
				.ClickImportButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-11603")]
		public void UploadTmxWithoutTuEndTagTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.WithoutTuEndTag)
				.ClickImportButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-13475")]
		public void UploadTmxWithUnicodeCharactersTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.WithUnicodeCharacters)
				.ClickImportButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportAddingNotifierDisappeared(),
				"Произошла ошибка:\n сообщение о процессе импорта TMX файла не исчезло");

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportCompleteNotifierDisplayed(),
				"Произошла ошибка:\n сообщение о окончании импорта TMX файла не появилось");
		}

		[Test, Ignore("PRX-11603")]
		public void UploadTxtWithTmxExtensionTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.TxtWithTmxExtension)
				.ClickImportButton();

			Assert.IsTrue(TranslationMemoriesPage.IsFileImportFailedNotifierDisplayed(),
				"Произошла ошибка:\n сообщение об ошибке во время импорта TMX файла не появилось");
		}

		[Test]
		public void UploadTMFileWithWrongExtensionTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(PathProvider.DocumentFile)
				.ClickImportButtonExpectingError();

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}

		[Test]
		public void UploadTMWithoutFileName()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton();

			ImportTmxDialog
				.EnterFileName(string.Empty)
				.ClickImportButtonExpectingError();

			Assert.IsTrue(TranslationMemoriesPage.IsImportValidationErrorMessageDisplayed(),
				"Произошла ошибка:\n не сработала валидация");
		}

		private TranslationMemoriesHelper _translationMemoriesHelper;
		private string _tmName;
		private WorkspaceHelper _workspaceHelper;
	}
}
