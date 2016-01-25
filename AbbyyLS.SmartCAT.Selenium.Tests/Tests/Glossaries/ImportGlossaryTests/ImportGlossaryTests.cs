using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class ImportGlossaryTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void ImportGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.ClickImportButton();

			_glossaryImportDialog
				.ImportGlossary(PathProvider.ImportGlossaryFile)
				.ClickImportButtonInImportDialog();

			_glossarySuccessImportDialog.ClickCloseButton();

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
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
				.ImportGlossary(PathProvider.ImportGlossaryFile)
				.ClickImportButtonInImportDialog();

			_glossarySuccessImportDialog.ClickCloseButton();

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}
	}
}
