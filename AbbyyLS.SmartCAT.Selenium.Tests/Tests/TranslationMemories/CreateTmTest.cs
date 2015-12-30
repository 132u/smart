using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class CreateTmTest<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCaseSource("TranslationMemoryNamesList")]
		public void CreateNewTmTest(string tmName)
		{
			TranslationMemoriesHelper.CreateTranslationMemory(tmName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(tmName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", tmName);
		}

		[Test]
		public void CancelNewTmCreation()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTMName, finalButtonType: DialogButtonType.Cancel);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", UniqueTMName);
		}

		[Test]
		public void CreateTMWithoutNameTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(string.Empty, isCreationErrorExpected: true);

			Assert.IsTrue(NewTranslationMemoryDialog.IsEmptyNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилась ошибка создания ТМ с пустым именем");
		}

		[Test]
		public void CreateTMWithExistingNameTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTMName)
				.CreateTranslationMemory(UniqueTMName, isCreationErrorExpected: true);

			Assert.IsTrue(NewTranslationMemoryDialog.IsExistingNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилась ошибка создания ТМ с существующим именем");
		}

		[Test]
		public void CreateTMWithoutLanguageTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(
					UniqueTMName, 
					targetLanguage: Language.NoLanguage,
					isCreationErrorExpected: true);

			Assert.IsTrue(NewTranslationMemoryDialog.IsNoTargetErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилась ошибка при создании ТМ без языка перевода");
		}

		[Test]
		public void CreateTMWithNotTmxFileTest()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(
					UniqueTMName, 
					importFilePath: PathProvider.DocumentFile,
					finalButtonType: DialogButtonType.None,
					isCreationErrorExpected: true);

			Assert.IsTrue(NewTranslationMemoryDialog.IsWrongFormatErrorDisplayed(),
				"Произошла ошибка:\n не появилась ошибка о загрузке файла с неподходящим расширением (не TMX файл)");
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
				.ExpandAdvancedSettings()
				.ClickSelectTmButton();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", UniqueTMName);

			NewProjectSetUpTMDialog.ClickCancelButton();

			WorkspacePage.ClickProjectsSubmenuExpectingAlert();

			WorkspacePage.AcceptAlert<ProjectsPage>();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			NewProjectSettingsPage
				.FillGeneralProjectInformation(UniqueTMName, targetLanguage: Language.Lithuanian)
				.ExpandAdvancedSettings()
				.ClickSelectTmButton();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", UniqueTMName);
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void CreateTMSearchTM(bool uploadFile)
		{
			var importFilePath = uploadFile ? PathProvider.TMTestFile2 : null;

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: importFilePath);

			TranslationMemoriesPage
				.CloseAllNotifications<TranslationMemoriesPage>()
				.SearchForTranslationMemory(UniqueTMName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", UniqueTMName);
		}

		private static readonly string[] TranslationMemoryNamesList =
		{
			"TestTM", 
			"Тестовая ТМ", 
			"我喜爱的哈伯尔阿哈伯尔"
		};
	}
}
