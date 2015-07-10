using NUnit.Framework;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	class BaseTmTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpTmTests()
		{
			UniqueTranslationMemoryName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
			TranslationMemoriesHelper = WorkspaceHelper.GoToTranslationMemoriesPage();
		}

		public string UniqueTranslationMemoryName { get; set; }

		public TranslationMemoriesHelper TranslationMemoriesHelper { get; private set; }
	}
}
