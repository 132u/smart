﻿using System.Collections.Generic;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	class EditTmTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(true, "")]
		[TestCase(true, " ")]
		[TestCase(false, "")]
		[TestCase(false, " ")]
		public void EditTMSaveWithInvalidNameTest(bool needUploadTmx, string invalidName)
		{
			var importFilePath = needUploadTmx ? PathProvider.TMTestFile2 : null;
			
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, importFilePath: importFilePath);

			if (needUploadTmx)
			{
				TranslationMemoriesHelper.AssertTMXFileIsImported();
			}

			TranslationMemoriesHelper
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.RenameTranslationMemory(UniqueTranslationMemoryName, string.Empty)
				.AssertNoNameErrorAppearInEditionForm();
		}

		[Test]
		public void EditTMSaveExistingNameTest()
		{
			var secondTranslationMemoryName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.CreateTranslationMemory(secondTranslationMemoryName)
				.AssertTranslationMemoryExists(secondTranslationMemoryName)
				.RenameTranslationMemory(secondTranslationMemoryName, UniqueTranslationMemoryName)
				.AssertExistNameErrorAppearInEditionForm();
		}

		[Test]
		public void EditTMSaveUniqueNameTest()
		{
			var translationMemoryNewName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.RenameTranslationMemory(UniqueTranslationMemoryName, translationMemoryNewName)
				.AssertEditionFormDisappeared()
				.AssertTranslationMemoryExists(translationMemoryNewName)
				.AssertTranslationMemoryNotExists(UniqueTranslationMemoryName);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void EditTMNameAndCheckChangesOnProjectWizard(bool needUploadTmx)
		{
			// Для облегчения теста создадим имя ТМ таким, чтобы при создании проекта
			// его можно было выбрать, не прибегая к прокрутке
			var translationMemoryNewName = string.Concat("!", TranslationMemoriesHelper.GetTranslationMemoryUniqueName());
			var importFilePath = needUploadTmx ? PathProvider.TMTestFile2 : null;
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, importFilePath: importFilePath);
			
			if (needUploadTmx)
			{
				TranslationMemoriesHelper.AssertTMXFileIsImported();
			}

			TranslationMemoriesHelper
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.RenameTranslationMemory(UniqueTranslationMemoryName, translationMemoryNewName)
				.AssertEditionFormDisappeared()
				.AssertTranslationMemoryExists(translationMemoryNewName)
				.GoToProjectsPage()
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage()
				.ClickNextOnWorkflowPage()
				.AssertTranslationMemoryExist(translationMemoryNewName)
				.CancelCreateProject();
		}

		[TestCase(true)]
		[TestCase(false)]
		public void EditTMComment(bool needUploadTmx)
		{
			var importFilePath = needUploadTmx ? PathProvider.TMTestFile2 : null;

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTranslationMemoryName, importFilePath: importFilePath);

			if (needUploadTmx)
			{
				TranslationMemoriesHelper.AssertTMXFileIsImported();
			}

			TranslationMemoriesHelper.
				AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.EditComment(UniqueTranslationMemoryName, InitialComment)
				.AssertCommentExist(InitialComment)
				.EditComment(UniqueTranslationMemoryName, FinalComment)
				.AssertCommentExist(FinalComment);
		}

		[Test]
		public void EditTmLanguages()
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName)
				.AssertLanguagesForTranslationMemory(UniqueTranslationMemoryName, EnglishLanguage, new List<string> {RussianLanguage})
				.AddTargetLanguageToTranslationMemory(UniqueTranslationMemoryName, Language.Lithuanian)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.FindTranslationMemory(UniqueTranslationMemoryName)
				.AssertLanguagesForTranslationMemory(
					UniqueTranslationMemoryName, EnglishLanguage, new List<string> { RussianLanguage, LithuanianLanguage });
		}

		[TestCase(true)]
		[TestCase(false)]
		public void EditTmProjectGroups(bool needUploadTmx)
		{
			var importFilePath = needUploadTmx ? PathProvider.TMTestFile2 : null;
			string projectGroup;

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, importFilePath: importFilePath)
				.AssertTranslationMemoryExists(UniqueTranslationMemoryName);

			if (needUploadTmx)
			{
				TranslationMemoriesHelper.AssertTMXFileIsImported();
			}

			TranslationMemoriesHelper
				.AssertProjectGroupExistForTranslationMemory(UniqueTranslationMemoryName, string.Empty)
				.AddFirstProjectGroupToTranslationMemory(UniqueTranslationMemoryName, out projectGroup)
				.AssertProjectGroupExistForTranslationMemory(UniqueTranslationMemoryName, projectGroup);
		}

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		private const string InitialComment = "InitialComment";
		private const string FinalComment = "FinalComment";
		private const string RussianLanguage = "ru";
		private const string EnglishLanguage = "en";
		private const string LithuanianLanguage = "lt";
	}
}
