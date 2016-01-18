using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Clients
{
	[Category("QWERTY")]
	public class BaseClientsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpClientTest()
		{
			_workspacePage = new WorkspacePage(Driver);
			_clientsPage = new ClientsPage(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_newTranslationMemoryDialog = new NewTranslationMemoryDialog(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_newGlossaryDialog = new NewGlossaryDialog(Driver);
			_clientName = _clientsPage.GetClientUniqueName();

			_workspacePage.GoToClientsPage();
		}

		protected ClientsPage _clientsPage;
		protected TranslationMemoriesPage _translationMemoriesPage;
		protected NewTranslationMemoryDialog _newTranslationMemoryDialog;
		protected WorkspacePage _workspacePage;
		protected GlossariesPage _glossariesPage;
		protected NewGlossaryDialog _newGlossaryDialog;

		protected string _clientName;
	}
}
