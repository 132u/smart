using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class EditTranslationMemoriesContentTests<TWebDriverProvider> : TranslationMemoriesContentBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				_translationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

				_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

				_workspacePage.GoToTranslationMemoriesPage();
				_translationMemoriesHelper.CreateTranslationMemory(_translationMemory);

				_workspacePage.SignOut();
				_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);

				_workspacePage.GoToTranslationMemoriesPage();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
			
		}

		[Test]
		public void AddTargetLanguagesInTranslationMemoryTest()
		{
			var languages = new List<string> { Language.Lithuanian.Description(), Language.Russian.Description() };
			languages.Sort();

			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemory);

			_translationMemoriesPage
				.EditTranslationMemory(_translationMemory, addTargetLanguage: Language.Lithuanian)
				.RefreshPage<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemory);

			Assert.AreEqual(
				languages,
				_translationMemoriesPage.GetTranslationMemoryTargetLanguages(_translationMemory),
				"Произошла ошибка: Неверно указаны целевые языки.");
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

			Assert.AreEqual(Language.German.Description(), _translationMemoriesPage.GetTranslationMemorySourceLanguage(_translationMemory),
				"Произошла ошибка:\nНеверно указан исходный язык.");
		}

		[Test]
		public void EditTargetLanguageInTranslationMemoryTest()
		{
			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemory);

			_translationMemoriesPage
				.ClickEditButton()
				.SelectTargetLanguage(new List<Language>{Language.Russian, Language.Lithuanian})
				.ClickSaveTranslationMemoryButton()
				.RefreshPage<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemory);

			Assert.AreEqual(new List<string> { Language.Lithuanian.Description() }, _translationMemoriesPage.GetTranslationMemoryTargetLanguages(_translationMemory),
				"Произошла ошибка: Неверно указаны целевые языки.");
		}
	}
}
