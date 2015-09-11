using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	class TMUploadingTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			WorkspaceHelper.GoToTranslationMemoriesPage();
			_tmName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
		}

		[Test]
		public void UploadTmxToEmptyTMTest()
		{
			var unitsCountBefore = _translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName)
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.GetUnitsCount();

			var unitsCountAfter = _translationMemoriesHelper
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.SecondTmFile)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.OpenTranslationMemoryInformation(_tmName)
				.GetUnitsCount();

			Assert.IsTrue(unitsCountBefore != unitsCountAfter,
				string.Format("Произошла ошибка:\n количество юнитов не изменилось. Кол-во юнитов до: {0}. Кол-во юнитов после: {1}", unitsCountBefore, unitsCountAfter));
		}

		[Test]
		public void UploadTmxToExistingTMWithTmxTest()
		{
			var unitsCountBefore = _translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.GetUnitsCount();

			var unitsCountAfter = _translationMemoriesHelper
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.TMTestFile2)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.OpenTranslationMemoryInformation(_tmName)
				.GetUnitsCount();

			Assert.IsTrue(unitsCountBefore != unitsCountAfter,
				string.Format("Произошла ошибка:\n количество юнитов не изменилось. Кол-во юнитов до: {0}. Кол-во юнитов после: {1}", unitsCountBefore, unitsCountAfter));
		}

		[Test]
		public void CheckNotificationDuringTmxFileUploading()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.SecondTmFile)
				.AssertTMXFileIsImported();
		}

		[Test, Ignore("PRX-11602")]
		public void UploadTmxWithoutTmxEndTagTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.WithoutTmxEndTag)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test, Ignore("PRX-11603")]
		public void UploadTmxWithoutBodyEndTagTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.WithoutBodyEndTag)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test, Ignore("PRX-11603")]
		public void UploadTmxWithLongSegValueTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.LongSegValue)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test, Ignore("PRX-11603")]
		public void UploadTmxWithoutTuEndTagTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.WithoutTuEndTag)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test]
		public void UploadTmxWithUnicodeCharactersTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.WithUnicodeCharacters)
				.AssertTMXFileIsImported();
		}

		[Test, Ignore("PRX-11603")]
		public void UploadTxtWithTmxExtensionTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.TxtWithTmxExtension)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test]
		public void UploadTMFileWithWrongExtensionTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton()
				.ImportTmxFile(PathProvider.DocumentFile, success: false)
				.AssertImportValidationErrorDisplayed();
		}

		[Test]
		public void UploadTMWithoutFileName()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickAddTmxButton()
				.ImportTmxFile("", success: false)
				.AssertImportValidationErrorDisplayed();
		}

		private readonly TranslationMemoriesHelper _translationMemoriesHelper = new TranslationMemoriesHelper();
		private string _tmName;
	}
}
