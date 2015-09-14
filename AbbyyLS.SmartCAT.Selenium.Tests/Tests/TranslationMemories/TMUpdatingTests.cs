using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	class TMUpdatingTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspaceHelper = new WorkspaceHelper();
			_workspaceHelper.GoToTranslationMemoriesPage();
			_tmName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
		}

		[Test]
		public void UpdateTMWithCorrectTmx()
		{
			var unitsCountBefore = _translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.GetUnitsCount();

			var unitsCountAfter = _translationMemoriesHelper
				.ClickUpdateTmButton()
				.ImportTmxFile(PathProvider.TMTestFile2, true)
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
				.ClickUpdateTmButton()
				.ImportTmxFile(PathProvider.SecondTmFile, true)
				.AssertTMXFileIsImported();
		}

		[Test, Ignore("PRX-11602")]
		public void UpdateTmxWithoutTmxEndTagTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton()
				.ImportTmxFile(PathProvider.WithoutTmxEndTag, true)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test, Ignore("PRX-11603")]
		public void UpdateTmxWithoutBodyEndTagTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton()
				.ImportTmxFile(PathProvider.WithoutBodyEndTag, true)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test, Ignore("PRX-11603")]
		public void UpdateTmxWithLongSegValueTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton()
				.ImportTmxFile(PathProvider.LongSegValue, true)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test, Ignore("PRX-11603")]
		public void UpdateTmxWithoutTuEndTagTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton()
				.ImportTmxFile(PathProvider.WithoutTuEndTag, true)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test]
		public void UpdateTmxWithUnicodeCharactersTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton()
				.ImportTmxFile(PathProvider.WithUnicodeCharacters, true)
				.AssertTMXFileIsImported();
		}

		[Test, Ignore("PRX-11603")]
		public void UpdateTxtWithTmxExtensionTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton()
				.ImportTmxFile(PathProvider.TxtWithTmxExtension, true)
				.AssertFileImportFailedNotifierDisplayed();
		}

		[Test]
		public void UpdateTMFileWithWrongExtensionTest()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton()
				.ImportTmxFile(PathProvider.DocumentFile, true, false)
				.AssertImportValidationErrorDisplayed();
		}

		[Test]
		public void UpdateTMWithoutFileName()
		{
			_translationMemoriesHelper
				.AssertTranslationMemoryNotExists(_tmName)
				.CreateTranslationMemory(_tmName, importFilePath: PathProvider.EditorTmxFile)
				.AssertTMXFileIsImported()
				.AssertTranslationMemoryExists(_tmName)
				.OpenTranslationMemoryInformation(_tmName)
				.ClickUpdateTmButton()
				.ImportTmxFile("", true, false)
				.AssertImportValidationErrorDisplayed();
		}

		private readonly TranslationMemoriesHelper _translationMemoriesHelper = new TranslationMemoriesHelper();
		private string _tmName;
		private WorkspaceHelper _workspaceHelper;
	}
}
