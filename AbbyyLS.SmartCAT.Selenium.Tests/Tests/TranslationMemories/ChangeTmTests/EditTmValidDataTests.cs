using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class EditTmValidDataTests<TWebDriverProvider>
		: BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
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
			var importFilePath = needUploadTmx ? PathProvider.TmxFile : null;
			var projectUniqueName = CreateProjectHelper.GetProjectUniqueName();
			
			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName, importFilePath: importFilePath);

			TranslationMemoriesPage.EditTranslationMemory(UniqueTMName, renameTo: translationMemoryNewName);

			Assert.IsTrue(TranslationMemoriesPage.IsTranslationMemoryExist(translationMemoryNewName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", translationMemoryNewName);

			WorkspacePage.GoToProjectsPage();

			ProjectsPage.ClickCreateProjectButton();

			NewProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			NewProjectSettingsPage
				.FillGeneralProjectInformation(projectUniqueName)
				.ExpandAdvancedSettings();

			_translationMemoryAdvancedSettingsSection.ClickSelectTmButton();

			Assert.IsTrue(NewProjectSetUpTMDialog.IsTranslationMemoryExist(translationMemoryNewName),
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", translationMemoryNewName);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void EditTMComment(bool needUploadTmx)
		{
			var importFilePath = needUploadTmx ? PathProvider.TmxFile : null;

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
			var targetLanguages = new List<string> { Language.Russian.Description(), Language.Lithuanian.Description() };
			targetLanguages.Sort();

			TranslationMemoriesHelper.CreateTranslationMemory(UniqueTMName);

			Assert.AreEqual(new List<string> { Language.Russian.Description() },
				TranslationMemoriesPage.GetTranslationMemoryTargetLanguages(UniqueTMName),
				"Произошла ошибка: Неверно указаны целевые языки.");

			Assert.AreEqual(
				Language.English.Description(),
				TranslationMemoriesPage.GetTranslationMemorySourceLanguages(UniqueTMName),
				"Произошла ошибка: Неверно указан исходный язык.");

			TranslationMemoriesPage
				.EditTranslationMemory(UniqueTMName, addTargetLanguage: Language.Lithuanian)
				.RefreshPage<TranslationMemoriesPage>()
				.SearchForTranslationMemory(UniqueTMName);

			Assert.AreEqual(targetLanguages,
				TranslationMemoriesPage.GetTranslationMemoryTargetLanguages(UniqueTMName),
				"Произошла ошибка: Неверно указаны целевые языки.");

			Assert.AreEqual(
				Language.English.Description(),
				TranslationMemoriesPage.GetTranslationMemorySourceLanguages(UniqueTMName),
				"Произошла ошибка: Неверно указан исходный язык.");
		}

		[TestCase(true)]
		[TestCase(false)]
		[ProjectGroups]
		public void EditTmProjectGroups(bool needUploadTmx)
		{
			var importFilePath = needUploadTmx ? PathProvider.TmxFile : null;
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
