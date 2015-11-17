using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class CreateTmTest<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCaseSource("TranslationMemoryNamesList")]
		public void CreateNewTmTest(string tmName)
		{
			TranslationMemoriesHelper
				.GetTranslationMemoryUniqueName(ref tmName)
				.CreateTranslationMemory(tmName)
				.AssertTranslationMemoryExists(tmName);
		}

		[Test]
		public void CancelNewTmCreation()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, finalButtonType: DialogButtonType.Cancel)
				.AssertTranslationMemoryNotExists(UniqueTranslationMemoryName);
		}

		[Test]
		public void CreateTMWithoutNameTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(string.Empty, isCreationErrorExpected: true)
				.AssertNoNameErrorAppearedInCreationDialog();
		}

		[Test]
		public void CreateTMWithExistingNameTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.CreateTranslationMemory(UniqueTranslationMemoryName, isCreationErrorExpected: true)
				.AssertExistNameErrorAppearedInCreationDialog();
		}

		[Test]
		public void CreateTMWithoutLanguageTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(
					UniqueTranslationMemoryName, 
					targetLanguage: Language.NoLanguage, 
					isCreationErrorExpected: true)
				.AssertNoTargetErrorAppearedInCreationDialog();
		}

		[Test]
		public void CreateTMWithNotTmxFileTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(
					UniqueTranslationMemoryName, 
					importFilePath: PathProvider.DocumentFile,
					finalButtonType: DialogButtonType.None,
					isCreationErrorExpected: true)
				.AssertNotTmxFileErrorAppearedInCreationDialog();
		}

		[Test]
		public void CreateMultilanguageTM()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, secondTargetLanguage: Language.Lithuanian)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectGeneralInformationDialog
				.FillGeneralProjectInformation(CreateProjectHelper.GetProjectUniqueName())
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			NewProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTranslationMemoryName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", UniqueTranslationMemoryName);

			NewProjectSetUpWorkflowDialog.CancelCreateProject();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectGeneralInformationDialog
				.FillGeneralProjectInformation(UniqueTranslationMemoryName, targetLanguage: Language.Lithuanian)
				.ClickNextButton<NewProjectSetUpWorkflowDialog>();

			NewProjectSetUpWorkflowDialog.ClickNextButton<NewProjectSetUpTMDialog>();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(UniqueTranslationMemoryName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", UniqueTranslationMemoryName);
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void CreateTMSearchTM(bool uploadFile)
		{
			var importFilePath = uploadFile ? PathProvider.TMTestFile2 : null;

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, importFilePath: importFilePath)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.CloseAllNotifications()
				.FindTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName);
		}

		private static readonly string[] TranslationMemoryNamesList =
		{
			"TestTM", 
			"Тестовая ТМ", 
			"我喜爱的哈伯尔阿哈伯尔"
		};
	}
}
