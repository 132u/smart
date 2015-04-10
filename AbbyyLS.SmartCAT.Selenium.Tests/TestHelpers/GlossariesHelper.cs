using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class GlossariesHelper : WorkspaceHelper
	{
		public GlossariesHelper AssertClientExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickCreateGlossaryBtn()
				.OpenClientsList()
				.AssertClientsListOpened()
				.AssertClientExistInList(clientName);

			return this;
		}

		public GlossariesHelper AssertClientNotExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickCreateGlossaryBtn()
				.OpenClientsList()
				.AssertClientsListOpened()
				.AssertClientNotExistInList(clientName);

			return this;
		}

		private readonly GlossariesPage _glossariesPage = new GlossariesPage();
	}
}
