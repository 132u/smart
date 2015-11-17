using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class DeleteTMTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void DeleteTranslationMemory()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemoryIfNotExist(_translationMemoryName)
				.OpenTranslationMemoryInformation(_translationMemoryName)
				.DeleteTranslationMemory(_translationMemoryName)
				.AssertTranslationMemoryNotExists(_translationMemoryName);
		}

		[Test]
		public void DeleteTranslationMemoryAndCreateNewTranslationMemoryWithTheSameNameTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemoryIfNotExist(_translationMemoryName)
				.OpenTranslationMemoryInformation(_translationMemoryName)
				.DeleteTranslationMemory(_translationMemoryName)
				.AssertTranslationMemoryNotExists(_translationMemoryName)
				.CreateTranslationMemory(_translationMemoryName)
				.AssertTranslationMemoryExists(_translationMemoryName);
		}

		[Test]
		public void DeleteTranslationMemoryCheckProjectCreateTranslationMemoryListTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.OpenTranslationMemoryInformation(UniqueTranslationMemoryName)
				.DeleteTranslationMemory(UniqueTranslationMemoryName)
				.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			NewProjectSettingsPage
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ExpandAdvancedSettings()
				.ClickSelectTmButton();

			Assert.IsFalse(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTranslationMemoryName),
				"Произошла ошибка:\n ТМ {0} представлена в списке при создании проекта.", UniqueTranslationMemoryName);
		}

		private const string _translationMemoryName = "TestTM";
	}
}
