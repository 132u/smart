using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class CreateTmCheckInProjectDialogTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateTMCheckProjectCreateTMListTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName);

			WorkspacePage.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			NewProjectSettingsPage
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ExpandAdvancedSettings();

			_translationMemoryAdvancedSettingsSection.ClickSelectTmButton();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", UniqueTMName);
		}

		[Test]
		public void CreateTMFrenchGermanLanguagesCheckProjectCreateTMListTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(
					UniqueTMName,
					sourceLanguage: Language.French,
					targetLanguage: Language.German);

			WorkspacePage.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			NewProjectSettingsPage
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ExpandAdvancedSettings();

			_translationMemoryAdvancedSettingsSection.ClickSelectTmButton();

			Assert.IsFalse(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} представлена в списке при создании проекта.", UniqueTMName);
		}

		[Test]
		public void CreateMultilanguageTM()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTMName, targetLanguage: Language.Russian, secondTargetLanguage: Language.Lithuanian);

			WorkspacePage.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			NewProjectSettingsPage
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ExpandAdvancedSettings();

			_translationMemoryAdvancedSettingsSection.ClickSelectTmButton();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", UniqueTMName);

			NewProjectSetUpTMDialog.ClickCancelButton();

			WorkspacePage.ClickProjectsSubmenuExpectingAlert();

			WorkspacePage.AcceptAlert<ProjectsPage>();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			NewProjectSettingsPage
				.FillGeneralProjectInformation(UniqueTMName, targetLanguage: Language.Lithuanian)
				.ExpandAdvancedSettings();

			_translationMemoryAdvancedSettingsSection.ClickSelectTmButton();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", UniqueTMName);
		}
	}
}
