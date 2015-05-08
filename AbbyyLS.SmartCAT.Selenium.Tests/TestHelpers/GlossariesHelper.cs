﻿using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class GlossariesHelper : WorkspaceHelper
	{
		public GlossariesHelper AssertClientExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickCreateGlossaryButton()
				.OpenClientsList()
				.AssertClientsListOpened()
				.AssertClientExistInList(clientName);

			return this;
		}

		public GlossariesHelper AssertClientNotExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickCreateGlossaryButton()
				.OpenClientsList()
				.AssertClientsListOpened()
				.AssertClientNotExistInList(clientName);

			return this;
		}

		public GlossariesHelper AssertProjectGroupExist (string projectGroupName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickCreateGlossaryButton()
				.AssertNewGlossaryDialogAppear()
				.OpenProjectGroupsList()
				.AssertProjectGroupsListOpened()
				.AssertProjectGroupExistInList(projectGroupName);

			return this;
		}

		public GlossariesHelper AssertProjectGroupNotExist(string projectGroupName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickCreateGlossaryButton()
				.OpenProjectGroupsList()
				.AssertProjectGroupsListOpened()
				.AssertProjectGroupNotExistInList(projectGroupName);

			return this;
		}

		private readonly GlossariesPage _glossariesPage = new GlossariesPage();
	}
}