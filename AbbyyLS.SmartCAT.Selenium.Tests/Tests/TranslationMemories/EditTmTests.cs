using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	class EditTmTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(true, "")]
		[TestCase(true, " ")]
		[TestCase(false, "")]
		[TestCase(false, " ")]
		public void EditTMSaveWithInvalidNameTest(bool needUploadTmx, string invalidName)
		{
			var importFilePath = needUploadTmx ? _importTmxFile : null;
			
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, importFilePath: importFilePath)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.CloseAllNotifications()
				.FindTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.RenameTranslationMemory(UniqueTranslationMemoryName, string.Empty)
				.AssertNoNameErrorAppearDuringEdition();
		}

		[Test]
		public void EditTMSaveExistingNameTest()
		{
			var secondTranslationMemoryName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.CreateTranslationMemory(secondTranslationMemoryName)
				.AssertTranslationMemoryExist(secondTranslationMemoryName)
				.RenameTranslationMemory(secondTranslationMemoryName, UniqueTranslationMemoryName)
				.AssertExistNameErrorAppearDuringEdition();
		}

		[Test]
		public void EditTMSaveUniqueNameTest()
		{
			var translationMemoryNewName = TranslationMemoriesHelper.GetTranslationMemoryUniqueName();

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.RenameTranslationMemory(UniqueTranslationMemoryName, translationMemoryNewName)
				.AssertTranslationMemoryExist(translationMemoryNewName)
				.AssertTranslationMemoryNotExist(UniqueTranslationMemoryName);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void EditTMNameAndCheckChangesOnProjectWizard(bool needUploadTmx)
		{
			// Для облегчения теста создадим имя ТМ таким, чтобы при создании проекта
			// его можно было выбрать, не прибегая к прокрутке
			var translationMemoryNewName = string.Concat("!", TranslationMemoriesHelper.GetTranslationMemoryUniqueName());
			var importFilePath = needUploadTmx ? _importTmxFile : null;
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, importFilePath: importFilePath)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.CloseAllNotifications()
				.FindTranslationMemory(UniqueTranslationMemoryName)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.RenameTranslationMemory(UniqueTranslationMemoryName, translationMemoryNewName)
				.AssertTranslationMemoryExist(translationMemoryNewName)
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
			var importFilePath = needUploadTmx ? _importTmxFile : null;

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, importFilePath: importFilePath)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.CloseAllNotifications()
				.FindTranslationMemory(UniqueTranslationMemoryName)
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
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.CloseAllNotifications()
				.FindTranslationMemory(UniqueTranslationMemoryName)
				.AssertLanguagesForTranslationMemory(UniqueTranslationMemoryName, EnglishLanguage, new[] {RussianLanguage})
				.AddTargetLanguageToTranslationMemory(UniqueTranslationMemoryName, Language.Lithuanian)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.AssertLanguagesForTranslationMemory(
					UniqueTranslationMemoryName, EnglishLanguage,new[] {RussianLanguage, LithuanianLanguage});
		}

		[TestCase(true)]
		[TestCase(false)]
		public void EditTmProjects(bool needUploadTmx)
		{
			var importFilePath = needUploadTmx ? _importTmxFile : null;
			string projectGroup;

			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTranslationMemoryName, importFilePath: importFilePath)
				.AssertTranslationMemoryExist(UniqueTranslationMemoryName)
				.RefreshPage<TranslationMemoriesPage, TranslationMemoriesHelper>()
				.CloseAllNotifications()
				.FindTranslationMemory(UniqueTranslationMemoryName)
				.AssertProjectGroupExistForTranslationMemory(UniqueTranslationMemoryName, string.Empty)
				.AddFirstProjectGroupToTranslationMemory(UniqueTranslationMemoryName, out projectGroup)
				.AssertProjectGroupExistForTranslationMemory(UniqueTranslationMemoryName, projectGroup);
		}

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		private readonly string _importTmxFile = Directory.GetFiles(PathProvider.TMTestFolder)[1];
		private const string InitialComment = "InitialComment";
		private const string FinalComment = "FinalComment";
		private const string RussianLanguage = "ru";
		private const string EnglishLanguage = "en";
		private const string LithuanianLanguage = "lt";
	}
}
