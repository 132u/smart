using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	class SuggestTermsBaseTest<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void TestFixtureSetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_suggestTermDialog = new SuggestTermDialog(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_suggestedTermsPageForCurrentGlossaries = new SuggestedTermsPageForCurrentGlossaries(Driver);
			_suggestedTermsPageForAllGlossaries = new SuggestedTermsPageForAllGlossaries(Driver);
			_selectGlossaryDialog = new SelectGlossaryDialog(Driver);

			_term1 = "term1";
			_term2 = "term2";

			_glossaryName = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForAllGlossaries.DeleteAllSuggestTerms();

			_workspacePage.GoToGlossariesPage();
		}

		protected string _term1;
		protected string _term2;
		protected string _glossaryName;

		protected WorkspacePage _workspacePage;
		protected SelectGlossaryDialog _selectGlossaryDialog;
		protected GlossariesHelper _glossariesHelper;
		protected SuggestTermDialog _suggestTermDialog;
		protected GlossaryPage _glossaryPage;
		protected GlossariesPage _glossariesPage;
		protected SuggestedTermsPageForCurrentGlossaries _suggestedTermsPageForCurrentGlossaries;
		protected SuggestedTermsPageForAllGlossaries _suggestedTermsPageForAllGlossaries;
	}
}
