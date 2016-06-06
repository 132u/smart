using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.MachineTranslations.FastMTTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TranslatingFileTests<TWebDriverProvider> : BaseMTTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspacePage.GoToMachineTranslationPage();
		}

		[Test, Description("S-7273"), ShortCheckList]
		public void TranslateFileUsingFastMTTest()
		{
			var documentsCountBeforeDownload = _fastMTAddFilesPage.GetDocumentsCount();

			_fastMTAddFilesPage.ClickAddButton();

			_fastMTAddFilesSettingsPage
				.UploadDocumentFiles(new [] {PathProvider.DocumentFile})
				.SetTranslationSettings()
				.ClickTranslateButton();

			var documentsCountAfterDownload = _fastMTAddFilesPage.GetDocumentsCount();

			Assert.IsTrue(documentsCountAfterDownload > documentsCountBeforeDownload,
				"Произошла ошибка:\n документ не появился в списке");

			var downloadedDocumentName = _fastMTAddFilesPage.GetDocumentName();

			_fastMTAddFilesPage.DownloadFileAfterTranslation();

			Assert.IsTrue(_workspacePage.IsFileDownloaded(downloadedDocumentName),
				"Произошла ошибка:\n файл не загрузился");
		}
	}
}
