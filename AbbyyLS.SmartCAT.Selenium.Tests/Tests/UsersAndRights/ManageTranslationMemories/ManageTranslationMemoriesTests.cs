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
	class ManageTranslationMemoriesTests<TWebDriverProvider> : ManageTranslationMemoriesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_groupName = Guid.NewGuid().ToString();
			_projectGroupName = _projectGroupsPage.GetProjectGroupUniqueName();
			_clientName = _clientsPage.GetClientUniqueName();
			_commonTranslationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryToDeleteTest = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_commonTranslationMemoryWithClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_commonTranslationMemoryWithProjectGroup = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);

			_workspacePage.GoToProjectGroupsPage();
			_projectGroupsPage.CreateProjectGroup(_projectGroupName);

			_workspacePage.GoToTranslationMemoriesPage();
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemory);
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemoryWithClient, client: _clientName);
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemoryWithProjectGroup, projectGroup: _projectGroupName);
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemoryToDeleteTest, client: _clientName, projectGroup: _projectGroupName);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.FullName,
				_groupName,
				new List<RightsType>{RightsType.TMManagement});

			_workspacePage.SignOut();
		}

		[Test]
		public void CreateTranslationMemoryTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemory),
				"Произошла ошибка: ТМ {0} не представлена в списке ТМ.", _translationMemory);
		}
		
		[Test]
		public void CreateTranslationMemoryWithClientTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory, client: _clientName);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemory),
				"Произошла ошибка: ТМ {0} не представлена в списке ТМ.", _translationMemory);
		}

		[Test]
		public void CreateTranslationMemoryWithProjectGroupTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory, projectGroup: _projectGroupName);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemory),
				"Произошла ошибка: ТМ {0} не представлена в списке ТМ.", _translationMemory);
		}

		[Test]
		public void TranslationMemoriesVisibilityTest()
		{
			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_commonTranslationMemory),
				"Произошла ошибка: ТМ {0} не представлена в списке ТМ.", _commonTranslationMemory);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_commonTranslationMemoryWithClient),
				"Произошла ошибка: ТМ {0} не представлена в списке ТМ.", _commonTranslationMemoryWithClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_commonTranslationMemoryWithProjectGroup),
				"Произошла ошибка: ТМ {0} не представлена в списке ТМ.", _commonTranslationMemoryWithProjectGroup);
		}

		[Test]
		public void TranslationMemorySettingsVisibilityTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory);
			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemory);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryInformationOpen(_translationMemory),
				"Произошла ошибка: Не открылись настройки ТМ {0}", _translationMemory);
		}

		[Test]
		public void EditSettingsTranslationMemoryTest()
		{
			var translationMemoryNewName = string.Concat("!", _translationMemoriesHelper.GetTranslationMemoryUniqueName());
			var newComment = "EditSettingsTranslationMemoryTest";

			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory);
			_translationMemoriesPage.EditTranslationMemory(
				_translationMemory, renameTo: translationMemoryNewName,
				changeCommentTo: newComment,
				addClient: _clientName,
				addProjectGroup: _projectGroupName);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(translationMemoryNewName),
				"Произошла ошибка: ТМ {0} не представлена в списке ТМ.", translationMemoryNewName);

			_translationMemoriesPage.OpenTranslationMemoryInformation(translationMemoryNewName);

			Assert.IsTrue(_translationMemoriesPage.IsProjectGroupSelectedForTM(_projectGroupName),
				"Произошла ошибка: Неверно указана группа проектов для ТМ {0}.", _projectGroupName);

			Assert.IsTrue(_translationMemoriesPage.IsCommentTextMatchExpected(newComment),
				"Произошла ошибка: Комментарий {0} не найден.", translationMemoryNewName);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void ExportClearTranslationMemoryButtonTest(bool withImportFile)
		{
			var importFile = withImportFile ? PathProvider.TmxFile : null;

			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory, importFilePath: importFile);

			_translationMemoriesPage.ExportTM(_translationMemory);

			Assert.IsTrue(_exportNotification.IsFileDownloaded(String.Format("{0}*-export.tmx", _translationMemory)),
				"Произошла ошибка: Файл не загрузился.");
		}

		[TestCase(true)]
		[TestCase(false)]
		public void TranslationMemorySearchTest(bool uploadFile)
		{
			var importFilePath = uploadFile ? PathProvider.TmxFile : null;

			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory, importFilePath: importFilePath);

			_translationMemoriesPage
				.CloseAllNotifications<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemory);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemory),
				"Произошла ошибка: ТМ {0} не представлена в списке ТМ.", _translationMemory);
		}

		[Test]
		public void UpdateTranslationMemoryTest()
		{
			var tmxFile = PathProvider.TmxFile;

			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory, importFilePath: _tmx);

			var unitsCountBefore = _translationMemoriesPage
				.OpenTranslationMemoryInformation(_translationMemory)
				.GetUnitsCount();

			_translationMemoriesPage.ClickUpdateTmButton();

			_importTmxDialog.ImportTmxFileExpectingConfirmation(tmxFile);

			_confirmReplacementDialog.ClickConfirmReplacementButton();

			var unitsCountAfter = _translationMemoriesPage
				.OpenTranslationMemoryInformation(_translationMemory)
				.GetUnitsCount();

			Assert.AreNotEqual(unitsCountBefore, unitsCountAfter, "Произошла ошибка: Количество юнитов не изменилось.");
		}

		[Test]
		public void UploadTmxToExistingTranslationMemoryTest()
		{
			var tmxFile = PathProvider.TmxFile;

			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory, importFilePath: _tmx);

			var unitsCountBefore = _translationMemoriesPage
				.OpenTranslationMemoryInformation(_translationMemory)
				.GetUnitsCount();

			_translationMemoriesPage.ClickAddTmxButton();

			_importTmxDialog.ImportTmxFile(tmxFile);

			var unitsCountAfter = _translationMemoriesPage
				.OpenTranslationMemoryInformation(_translationMemory)
				.GetUnitsCount();

			Assert.AreNotEqual(unitsCountBefore, unitsCountAfter, "Произошла ошибка: Количество юнитов не изменилось.");
		}

		[Test]
		public void EditLanguagesTranslationMemoryTest()
		{
			var targetLanguages = new List<string> { Language.Russian.Description(), Language.Lithuanian.Description() };
			targetLanguages.Sort();

			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory);

			Assert.AreEqual(Language.English.Description(),
				_translationMemoriesPage.GetTranslationMemorySourceLanguage(_translationMemory),
				"Произошла ошибка: Неверно указан исходный язык.");

			Assert.AreEqual(
				new List<string> { Language.Russian.Description() },
				_translationMemoriesPage.GetTranslationMemoryTargetLanguages(_translationMemory),
				"Произошла ошибка: Неверно указаны целевые языки.");
			
			_translationMemoriesPage
				.EditTranslationMemory(_translationMemory, addTargetLanguage: Language.Lithuanian)
				.RefreshPage<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemory);

			Assert.AreEqual(
				Language.English.Description(),
				_translationMemoriesPage.GetTranslationMemorySourceLanguage(_translationMemory),
				"Произошла ошибка: Неверно указан исходный язык.");

			Assert.AreEqual(targetLanguages, _translationMemoriesPage.GetTranslationMemoryTargetLanguages(_translationMemory),
				"Произошла ошибка: Неверно указаны целевые языки.");
		}

		[Test]
		public void DeleteTranslationMemoryTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory);

			_translationMemoriesPage
				.OpenTranslationMemoryInformation(_translationMemory)
				.ClickDeleteButtonInTMInfo();

			_deleteTmDialog.ConfirmReplacement();

			Assert.IsFalse(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemory),
				"Произошла ошибка: ТМ {0} представлена в списке ТМ.", _translationMemory);
		}

		[Test]
		public void DeleteTranslationMemoryThreadUserTest()
		{
			_translationMemoriesPage
				.OpenTranslationMemoryInformation(_translationMemoryToDeleteTest)
				.ClickDeleteButtonInTMInfo();

			_deleteTmDialog.ConfirmReplacement();

			Assert.IsFalse(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryToDeleteTest),
				"Произошла ошибка: ТМ {0} представлена в списке ТМ.", _translationMemoryToDeleteTest);
		}
	}
}
