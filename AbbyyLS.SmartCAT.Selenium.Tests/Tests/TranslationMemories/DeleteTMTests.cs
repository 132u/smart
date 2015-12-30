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
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(UniqueTMName)
				.ClickDeleteButtonInTMInfo();

			DeleteTmDialog.ConfirmReplacement();

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", UniqueTMName);
		}

		[Test]
		public void DeleteTranslationMemoryAndCreateNewTranslationMemoryWithTheSameNameTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(UniqueTMName)
				.ClickDeleteButtonInTMInfo();

			DeleteTmDialog.ConfirmReplacement();

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", UniqueTMName);
		}

		[Test]
		public void DeleteTranslationMemoryCheckProjectCreateTranslationMemoryListTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName);

			TranslationMemoriesPage
				.OpenTranslationMemoryInformation(UniqueTMName)
				.ClickDeleteButtonInTMInfo();

			DeleteTmDialog.ConfirmReplacement();

			WorkspacePage.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			NewProjectSettingsPage
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ExpandAdvancedSettings()
				.ClickSelectTmButton();

			Assert.IsFalse(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} представлена в списке при создании проекта.", UniqueTMName);
		}
	}
}
