using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.TranslationMemories.TranslationMemoriesContent
{
	[Parallelizable(ParallelScope.Fixtures)]
	class EditTranslationMemoriesContentSpecificClientTests<TWebDriverProvider> : EditTranslationMemoriesContentSpecificClientBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void BeforeTest()
		{
			_translationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			
			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			
			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory, client: _commonClientName);
			
			_workspacePage.SignOut();
			_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);

			_workspacePage.GoToTranslationMemoriesPage();
		}

		[Test]
		public void AddTargetLanguagesInTranslationMemoryTest()
		{
			var targetLanguages = new List<string> { Language.Russian.Description(), Language.Lithuanian.Description() };
			targetLanguages.Sort();

			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemory);

			_translationMemoriesPage
				.EditTranslationMemory(_translationMemory, addTargetLanguage: Language.Lithuanian)
				.RefreshPage<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemory);
			
			Assert.AreEqual(targetLanguages, _translationMemoriesPage.GetTranslationMemoryTargetLanguages(_translationMemory),
				"Произошла ошибка:\nНеверно указан исходный язык.");
		}

		[Test]
		public void EditSourceLanguageInTranslationMemoryTest()
		{
			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemory);

			_translationMemoriesPage
				.ClickEditButton()
				.SelectSourceLanguage(Language.German)
				.ClickSaveTranslationMemoryButton()
				.RefreshPage<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemory);

			Assert.AreEqual(
				Language.German.Description(), 
				_translationMemoriesPage.GetTranslationMemorySourceLanguage(_translationMemory),
				"Произошла ошибка: Неверно указаны целевые языки.");
		}

		[Test]
		public void EditTargetLanguageInTranslationMemoryTest()
		{
			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemory);

			_translationMemoriesPage
				.ClickEditButton()
				.SelectTargetLanguage(new List<Language>{Language.Lithuanian, Language.Russian})
				.ClickSaveTranslationMemoryButton()
				.RefreshPage<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemory);
			
			Assert.AreEqual(new List<string> { Language.Lithuanian.Description() }, _translationMemoriesPage.GetTranslationMemoryTargetLanguages(_translationMemory),
				"Произошла ошибка: Неверно указаны целевые языки.");
		}
	}
}
