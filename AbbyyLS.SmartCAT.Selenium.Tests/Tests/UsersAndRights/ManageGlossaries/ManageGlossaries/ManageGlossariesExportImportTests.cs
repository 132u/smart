using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ManageGlossariesExportImportTests<TWebDriverProvider> : ManageGlossariesBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void ExportGlossaryTest()
		{
			_glossaryPage
				.CreateTerm()
				.CreateTerm("secondTerm1", "secondTerm2")
				.ClickExportGlossary();

			_eglossaryExportNotification.ClickDownloadNotifier<GlossaryPage>();

			Assert.IsTrue(_glossaryPage.IsGlossaryExportedSuccesfully(Path.Combine(PathProvider.ExportFiles, _glossaryUniqueName.Replace(":", "-") + ".xlsx")),
				"Произошла ошибка:\n файл не был скачан за отведенное время");
		}

		[Test]
		public void ImportGlossaryTest()
		{
			var file = PathProvider.GlossaryFileForImport;

			_glossaryPage.ClickImportButton();

			_glossaryImportDialog
				.ImportGlossary(file)
				.ClickImportButtonInImportDialogWaitSuccess();

			_glossarySuccessImportDialog.ClickCloseButton();
			
			//TODO PRX-17197 убрать рефреш (костыль)
			_workspacePage.RefreshPage<GlossaryPage>();

			Assert.IsTrue(
				_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}
	}
}
