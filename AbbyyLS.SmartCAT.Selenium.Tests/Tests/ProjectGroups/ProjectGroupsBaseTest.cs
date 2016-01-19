using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.ProjectGroups
{
	class ProjectGroupsBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Setup()
		{
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_newTranslationMemoryDialog = new NewTranslationMemoryDialog(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_newGlossaryDialog = new NewGlossaryDialog(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectGroup = _projectGroupsPage.GetProjectGroupUniqueName();

			_workspacePage.GoToProjectGroupsPage();
		}

		protected ProjectGroupsPage _projectGroupsPage;
		protected TranslationMemoriesPage _translationMemoriesPage;
		protected NewTranslationMemoryDialog _newTranslationMemoryDialog;
		protected GlossariesPage _glossariesPage;
		protected NewGlossaryDialog _newGlossaryDialog;
		protected WorkspacePage _workspacePage;
		protected string _projectGroup;
	}
}
