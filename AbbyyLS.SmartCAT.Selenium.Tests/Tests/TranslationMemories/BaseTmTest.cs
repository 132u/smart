using NUnit.Framework;
using AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	class BaseTmTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUpTmTests()
		{
			UniqueTranslationMemoryName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();
			TranslationMemoriesHelper = WorkspaceHelper.GoToTranslationMemoriesPage();
		}

		public string UniqueTranslationMemoryName { get; private set; }

		public TranslationMemoriesHelper TranslationMemoriesHelper { get; private set; }
	}
}
