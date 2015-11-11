using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
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
				.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectGeneralInformationDialog
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			NewProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTranslationMemoryName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", UniqueTranslationMemoryName);
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
				.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectGeneralInformationDialog
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			NewProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			Assert.IsFalse(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTranslationMemoryName),
				"Произошла ошибка:\n ТМ {0} представлена в списке при создании проекта.", UniqueTranslationMemoryName);
		}
	}
}
