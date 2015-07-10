using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	//???[PriorityMajor]
	class TMInCreateProjectDialog<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateTMCheckProjectCreateTMListTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.GoToProjectsPage()
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName);
		}

		[Test]
		public void CreateTMFrenchGermanLanguagesCheckProjectCreateTMListTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(
					UniqueTranslationMemoryName,
					sourceLanguage : Language.French,
					targetLanguage : Language.German)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.GoToProjectsPage()
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.AssertTranslationMemoryNotExist(UniqueTranslationMemoryName);
		}
	}
}
