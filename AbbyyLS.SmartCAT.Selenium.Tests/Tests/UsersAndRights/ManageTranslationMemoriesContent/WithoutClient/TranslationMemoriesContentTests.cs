using System;

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
	class TranslationMemoriesContentTests<TWebDriverProvider> : TranslationMemoriesContentBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_clientName2 = _clientsPage.GetClientUniqueName();
			_translationMemoryWithClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryWithoutClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryWithConcepts = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryWithProject = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var tmxFile = PathProvider.TmxFile;

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName2);
			
			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesHelper
				.CreateTranslationMemory(_translationMemoryWithoutClient)
				.CreateTranslationMemory(_translationMemoryWithClient, client: _clientName2)
				.CreateTranslationMemory(_translationMemoryWithConcepts, importFilePath: tmxFile)
				.CreateTranslationMemory(_translationMemoryWithProject);

			_workspacePage.GoToProjectsPage();
			_createProjectHelper.CreateNewProject(projectUniqueName, selectExistedTm: _translationMemoryWithProject);

			_workspacePage.SignOut();
		}

		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_loginHelper.Authorize(StartPage, AdditionalUser);
				_workspacePage.GoToTranslationMemoriesPage();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[Test]
		public void TranslationMemoriesViewTest()
		{
			_translationMemoriesPage.SearchForTranslationMemory(_translationMemoryWithClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithClient),
				"Произошла ошибка: Отсутсвует память перевод {0}.", _translationMemoryWithClient);

			_translationMemoriesPage.SearchForTranslationMemory(_translationMemoryWithoutClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithoutClient),
				"Произошла ошибка: Отсутсвует память перевод {0}.", _translationMemoryWithoutClient);
		}

		[Test]
		public void TranslationMemoriesSettingsViewTest()
		{
			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemoryWithClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryInformationOpen(_translationMemoryWithClient),
				"Произошла ошибка: Не открылись настройки памяти перевод {0}.", _translationMemoryWithClient);

			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemoryWithoutClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryInformationOpen(_translationMemoryWithoutClient),
				"Произошла ошибка: Не открылись настройки памяти перевод {0}.", _translationMemoryWithoutClient);
		}
		
		[Test]
		public void ExportTranslationMemoryWithClientTest()
		{
			_translationMemoriesPage.ExportTM(_translationMemoryWithClient);

			Assert.IsTrue(_exportNotification.IsFileDownloaded(String.Format("{0}*-export.tmx", _translationMemoryWithClient)),
				"Произошла ошибка: файл не загрузился");
		}

		[Test]
		public void ExportTranslationMemoryWithoutClientTest()
		{
			_translationMemoriesPage.ExportTM(_translationMemoryWithoutClient);

			Assert.IsTrue(_exportNotification.IsFileDownloaded(String.Format("{0}*-export.tmx", _translationMemoryWithoutClient)),
				"Произошла ошибка: файл не загрузился");
		}

		[Test]
		public void TranslationMemoryWithClientSearchTest()
		{
			_translationMemoriesPage
				.CloseAllNotifications<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemoryWithClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithClient),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _translationMemoryWithClient);
		}

		[Test]
		public void TranslationMemoryWithoutClientSearchTest()
		{
			_translationMemoriesPage
				.CloseAllNotifications<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemoryWithoutClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithoutClient),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _translationMemoryWithoutClient);
		}
		
		[Test]
		public void ChangeSourceLanguageInTranslationMemoryWithConceptsTest()
		{
			_translationMemoriesPage
				.OpenTranslationMemoryInformation(_translationMemoryWithConcepts)
				.ClickEditButton()
				.SelectSourceLanguage(Language.Japanese)
				.ClickSaveTranslationMemoryButtonExpectingError();

			Assert.IsTrue(_translationMemoriesPage.IsImpossibleToChangeLanguageErrorMessageDisplayed(),
				"Произошла ошибка: Не появилось сообщение 'The language cannot be changed because TM is already used in SmartCAT.'.");
		}

		[Test]
		public void ChangeSourceLanguageInTranslationMemoryWithProjectTest()
		{
			_translationMemoriesPage
				.OpenTranslationMemoryInformation(_translationMemoryWithProject)
				.ClickEditButton()
				.SelectSourceLanguage(Language.Japanese)
				.ClickSaveTranslationMemoryButtonExpectingError();

			Assert.IsTrue(_translationMemoriesPage.IsImpossibleToChangeLanguageErrorMessageDisplayed(),
				"Произошла ошибка: Не появилось сообщение 'The language cannot be changed because TM is already used in SmartCAT.'.");
		}
	}
}
