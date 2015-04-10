using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class TranslationMemoriesHelper : WorkspaceHelper
	{
		public TranslationMemoriesHelper AssertClientExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickTranslationMemoriesBtn()
				.ClickCreateNewTmButton()
				.ClickOpenClientsList()
				.AssertClientsListDisplayed()
				.AssertClientExistInTmCreationDialog(clientName);

			return this;
		}

		public TranslationMemoriesHelper AssertClientNotExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickTranslationMemoriesBtn()
				.ClickCreateNewTmButton()
				.ClickOpenClientsList()
				.AssertClientsListDisplayed()
				.AssertClientNotExistInTmCreationDialog(clientName);

			return this;
		}

		private readonly TranslationMemoriesPage _translationMemoriesPage = new TranslationMemoriesPage();
	}
}
