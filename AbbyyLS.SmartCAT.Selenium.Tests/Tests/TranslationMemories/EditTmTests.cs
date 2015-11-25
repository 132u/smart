using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
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
			
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: importFilePath);

			TranslationMemoriesPage.EditTranslationMemory(UniqueTMName, renameTo: invalidName, isErrorExpecting: true);

			Assert.IsTrue(TranslationMemoriesPage.IsEmptyNameErrorMessageDisplayed(),
				"Ошибка: не появилось сообщение о пустом названии при редактировании ТМ");
		}

		[Test]
		public void EditTMSaveExistingNameTest()
		{
			var secondTranslationMemoryName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTMName)
				.CreateTranslationMemory(secondTranslationMemoryName);

			TranslationMemoriesPage.EditTranslationMemory(secondTranslationMemoryName, renameTo: UniqueTMName, isErrorExpecting: true);

			Assert.IsTrue(TranslationMemoriesPage.IsExistingNameErrorMessageDisplayed(),
				"Ошибка: не появилось сообщение об ошибки имени при редактировании ТМ");
		}

		[Test]
		public void EditTMSaveUniqueNameTest()
		{
			var translationMemoryNewName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName);

			TranslationMemoriesPage.EditTranslationMemory(UniqueTMName, renameTo:translationMemoryNewName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(translationMemoryNewName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", translationMemoryNewName);

			Assert.IsFalse(TranslationMemoriesPage.IsTranslationMemoryExist(UniqueTMName),
				"Произошла ошибка:\n ТМ {0} представлена в списке ТМ", UniqueTMName);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void EditTMNameAndCheckChangesOnProjectWizard(bool needUploadTmx)
		{
			// Для облегчения теста создадим имя ТМ таким, чтобы при создании проекта
			// его можно было выбрать, не прибегая к прокрутке
			var translationMemoryNewName = string.Concat("!", TranslationMemoriesHelper.GetTranslationMemoryUniqueName());
			var importFilePath = needUploadTmx ? PathProvider.TMTestFile2 : null;
			var projectUniqueName = CreateProjectHelper.GetProjectUniqueName();
			
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: importFilePath);

			TranslationMemoriesPage.EditTranslationMemory(UniqueTMName, renameTo: translationMemoryNewName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(translationMemoryNewName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", translationMemoryNewName);

			WorkspaceHelper.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			NewProjectSettingsPage
				.FillGeneralProjectInformation(projectUniqueName)
				.ExpandAdvancedSettings()
				.ClickSelectTmButton();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(translationMemoryNewName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", translationMemoryNewName);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void EditTMComment(bool needUploadTmx)
		{
			var importFilePath = needUploadTmx ? PathProvider.TMTestFile2 : null;

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: importFilePath);

			TranslationMemoriesPage
				.EditTranslationMemory(UniqueTMName, changeCommentTo: InitialComment)
				.OpenTranslationMemoryInformation(UniqueTMName);

			Assert.IsTrue(TranslationMemoriesPage.IsCommentTextMatchExpected(InitialComment),
				"Ошибка: комментарий {0} не найден.", InitialComment);

			TranslationMemoriesPage
				.EditTranslationMemory(UniqueTMName, changeCommentTo: FinalComment)
				.OpenTranslationMemoryInformation(UniqueTMName);

			Assert.IsTrue(TranslationMemoriesPage.IsCommentTextMatchExpected(FinalComment),
				"Ошибка: комментарий {0} не найден.", InitialComment);
		}

		[Test]
		public void EditTmLanguages()
		{
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName);

			Assert.IsFalse(TranslationMemoriesPage.IsLanguagesForTranslationMemoryExists(
				UniqueTMName, EnglishLanguage, new List<string> { RussianLanguage }),
				"Произошла ошибка:\nСписки target языков не совпали");

			TranslationMemoriesPage
				.EditTranslationMemory(UniqueTMName, addTargetLanguage: Language.Lithuanian)
				.RefreshPage<TranslationMemoriesPage>()
				.SearchForTranslationMemory(UniqueTMName);

			Assert.IsFalse(TranslationMemoriesPage.IsLanguagesForTranslationMemoryExists(
				UniqueTMName, EnglishLanguage, new List<string> { RussianLanguage, LithuanianLanguage }),
				"Произошла ошибка:\nСписки target языков не совпали");
		}

		[TestCase(true)]
		[TestCase(false)]
		[ProjectGroups]
		public void EditTmProjectGroups(bool needUploadTmx)
		{
			var importFilePath = needUploadTmx ? PathProvider.TMTestFile2 : null;
			string projectGroup;

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: importFilePath);

			TranslationMemoriesPage.OpenTranslationMemoryInformation(UniqueTMName);

			Assert.IsTrue(TranslationMemoriesPage.IsProjectGroupSelectedForTM(string.Empty),
				"Произошла ошибка:\n неверно указана группа проектов для ТМ {0}", UniqueTMName);

			TranslationMemoriesPage
				.AddFirstProjectGroupToTranslationMemory(UniqueTMName, out projectGroup)
				.OpenTranslationMemoryInformation(UniqueTMName);

			Assert.IsTrue(TranslationMemoriesPage.IsProjectGroupSelectedForTM(projectGroup),
				"Произошла ошибка:\n неверно указана группа проектов для ТМ {0}", projectGroup);
		}

		private const string InitialComment = "InitialComment";
		private const string FinalComment = "FinalComment";
		private const string RussianLanguage = "ru";
		private const string EnglishLanguage = "en";
		private const string LithuanianLanguage = "lt";
	}
}
