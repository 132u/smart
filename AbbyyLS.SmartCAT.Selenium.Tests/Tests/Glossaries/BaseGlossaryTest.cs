using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	public class BaseGlossaryTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void BaseGlossaryTestSetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_newGlossaryDialog = new NewGlossaryDialog(Driver);
			_glossaryPropertiesDialog = new GlossaryPropertiesDialog(Driver);
			_glossaryImportDialog = new GlossaryImportDialog(Driver);
			_glossarySuccessImportDialog = new GlossarySuccessImportDialog(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_filterDialog = new FilterDialog(Driver);
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToGlossariesPage();
		}

		protected GlossariesHelper _glossariesHelper;
		protected WorkspacePage _workspacePage;
		protected GlossaryPage _glossaryPage;
		protected GlossariesPage _glossariesPage;
		protected NewGlossaryDialog _newGlossaryDialog;
		protected GlossaryPropertiesDialog _glossaryPropertiesDialog;
		protected GlossaryImportDialog _glossaryImportDialog;
		protected GlossarySuccessImportDialog _glossarySuccessImportDialog;
		protected GlossaryStructureDialog _glossaryStructureDialog;
		protected FilterDialog _filterDialog;
		protected string _glossaryUniqueName;
	}
}
