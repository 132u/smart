using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Glossaries]
	public class ImportGlossaryTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_exportNotification.CloseAllNotifications<GlossariesPage>();
		}

		[Test, Description("S-7293"), ShortCheckList]
		public void ImportGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.ClickImportButton();

			_glossaryImportDialog
				.ImportGlossary(PathProvider.GlossaryFileForImport)
				.ClickImportButtonInImportDialogWaitSuccess();

			_glossarySuccessImportDialog.ClickCloseButton();

			//TODO PRX-17197 убрать рефреш (костыль)
			_workspacePage.RefreshPage<GlossaryPage>();

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test, Description("S-7293"), ShortCheckList]
		public void ImportGlossaryWrongStructureTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.ClickImportButton();

			_glossaryImportDialog
				.ImportGlossary(PathProvider.GlossaryFileForImportWrongStructure)
				.ClickImportInImportInImportDialog();

			Assert.IsTrue(_glossaryImportDialog.IsErrorReportButtonDisplayed(_glossaryUniqueName),
				"Произошла ошибка:\n не отобразилась ошибка структуры загружаемого документа");
		}

		[Test]
		public void ImportGlossaryReplaceAllTermsTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm()
				.CreateTerm("termsecond", "termsecond")
				.ClickImportButton();

			_glossaryImportDialog
				.ClickReplaceTermsButton()
				.ImportGlossary(PathProvider.GlossaryFileForImport)
				.ClickImportButtonInImportDialogWaitSuccess();

			_glossarySuccessImportDialog.ClickCloseButton();

			//TODO костыль PRX-17197
			_workspacePage.RefreshPage<GlossaryPage>();

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}
	}
}
