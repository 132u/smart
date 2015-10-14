using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	class BaseTmTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpTmTests()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			UniqueTranslationMemoryName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
			TranslationMemoriesHelper = _workspaceHelper.GoToTranslationMemoriesPage();
			_createProjectHelper = new CreateProjectHelper(Driver);
			ClientsPage = new ClientsPage(Driver).GetPage();
		}

		public string UniqueTranslationMemoryName { get; set; }

		public TranslationMemoriesHelper TranslationMemoriesHelper { get; private set; }
		public CreateProjectHelper _createProjectHelper;
		private WorkspaceHelper _workspaceHelper;
		public ClientsPage ClientsPage;
	}
}
