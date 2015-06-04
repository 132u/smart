﻿using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings;
using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	class CreateTmTest<TWebDriverSettings> : BaseTmTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[TestCaseSource("TranslationMemoryNamesList")]
		public void CreateNewTmTest(string tmName)
		{
			TranslationMemoriesHelper
				.GetTranslationMemoryUniqueName(ref tmName)
				.CreateTranslationMemory(tmName)
				.AssertTranslationMemoryExist(tmName);
		}

		[Test]
		public void CancelNewTmCreation()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, finalButtonType: DialogButtonType.Cancel)
				.AssertTranslationMemoryNotExist(UniqueTranslationMemoryName);
		}

		[Test]
		public void CreateTMWithoutNameTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(string.Empty, isCreationErrorExpected: true)
				.AssertNoNameErrorAppear();
		}

		[Test]
		public void CreateTMWithExistingNameTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.CreateTranslationMemory(UniqueTranslationMemoryName, isCreationErrorExpected: true)
				.AssertExistNameErrorAppear();
		}

		[Test]
		public void CreateTMWithoutLanguageTest()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(
					UniqueTranslationMemoryName, 
					targetLanguage: Language.NoLanguage, 
					isCreationErrorExpected: true)
				.AssertNoTargetErrorAppear();
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
				.AssertNotTmxFileErrorAppear();
		}

		[Test]
		public void CreateMultilanguageTM()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, secondTargetLanguage: Language.Lithuanian)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.GoToProjectsPage()
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(ProjectName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.CancelCreateProject()
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(UniqueTranslationMemoryName, targetLanguage: Language.Lithuanian)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName);
		}

		private static readonly string[] TranslationMemoryNamesList =
		{
			"TestTM", 
			"Тестовая ТМ", 
			"我喜爱的哈伯尔阿哈伯尔"
		};
	}
}