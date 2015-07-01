using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	//???[PriorityMajor]
	class DeleteTMTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void DeleteTranslationMemory()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemoryIfNotExist(_translationMemoryName)
				.OpenTranslationMemoryInformation(_translationMemoryName)
				.DeleteTranslationMemory(_translationMemoryName)
				.AssertTranslationMemoryNotExist(_translationMemoryName);
		}

		[Test]
		public void DeleteTranslationMemoryAndCreateNewTranslationMemoryWithTheSameNameTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemoryIfNotExist(_translationMemoryName)
				.OpenTranslationMemoryInformation(_translationMemoryName)
				.DeleteTranslationMemory(_translationMemoryName)
				.AssertTranslationMemoryNotExist(_translationMemoryName)
				.CreateTranslationMemory(_translationMemoryName)
				.AssertTranslationMemoryExist(_translationMemoryName);
		}

		[Test]
		public void DeleteTranslationMemoryCheckProjectCreateTranslationMemoryListTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.OpenTranslationMemoryInformation(UniqueTranslationMemoryName)
				.DeleteTranslationMemory(UniqueTranslationMemoryName)
				.GoToProjectsPage()
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.AssertTranslationMemoryNotExist(UniqueTranslationMemoryName);
		}

		private const string _translationMemoryName = "TestTM";
	}
}
