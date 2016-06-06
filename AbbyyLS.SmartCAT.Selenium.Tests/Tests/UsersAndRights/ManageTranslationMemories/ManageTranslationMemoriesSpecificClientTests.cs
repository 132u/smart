using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ManageTranslationMemoriesSpecificClientTests<TWebDriverProvider> : ManageTranslationMemoriesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_groupName = Guid.NewGuid().ToString();
			_projectGroupName = _projectGroupsPage.GetProjectGroupUniqueName();
			_clientName = _clientsPage.GetClientUniqueName();
			_clientName2 = _clientsPage.GetClientUniqueName();
			_clientName3 = _clientsPage.GetClientUniqueName();
			_commonTranslationMemory = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_commonTranslationMemory2 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_commonTranslationMemory3 = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryToDeleteTest = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);
			_clientsPage.CreateNewClient(_clientName2);
			_clientsPage.CreateNewClient(_clientName3);

			_workspacePage.GoToTranslationMemoriesPage();
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemory, client: _clientName);
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemory2, client: _clientName2);
			_translationMemoriesHelper.CreateTranslationMemory(_commonTranslationMemory3, client: _clientName3);
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemoryToDeleteTest, client: _clientName);

			_workspacePage.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.FullName)
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(_groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupSpecificClient(RightsType.TMManagement, _clientName);
			_groupsAndAccessRightsTab
				.ClickSaveButton(_groupName)
				.AddUserToGroupIfNotAlredyAdded(_groupName, AdditionalUser.FullName);

			_workspacePage.SignOut();
		}

		[Test]
		public void CreateTranslationMemoryTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory);
			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemory);

			Assert.AreEqual(_clientName, _translationMemoriesPage.GetClientName(),
				"Произошла ошибка: Неверное имя клиента.");
		}

		[Test]
		public void TranslationMemoryVisibilityTest()
		{
			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_commonTranslationMemory),
				"Произошла ошибка: Отсутствует память перевода {0}.", _commonTranslationMemory);

			Assert.IsFalse(_translationMemoriesPage.IsTranslationMemoryExist(_commonTranslationMemory2),
				"Произошла ошибка: Отсутствует память перевода {0}.", _commonTranslationMemory2);

			Assert.IsFalse(_translationMemoriesPage.IsTranslationMemoryExist(_commonTranslationMemory3),
				"Произошла ошибка: Отсутствует память перевода {0}.", _commonTranslationMemory3);
		}

		[Test]
		public void TranslationMemoryVisibilityTwoClientsTest()
		{
			_loginHelper.Authorize(StartPage, ThreadUser);
			_workspacePage
				.GoToUsersPage()
				.ClickGroupsButton();
			
			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(_groupName);

			_addAccessRightDialog.AddRightToGroupSpecificClient(RightsType.TMManagement, _clientName2);
			_groupsAndAccessRightsTab.ClickSaveButton(_groupName);

			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage, AdditionalUser);

			_workspacePage.GoToTranslationMemoriesPage();

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_commonTranslationMemory),
				"Произошла ошибка: Отсутствует память перевода {0}.", _commonTranslationMemory);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_commonTranslationMemory2),
				"Произошла ошибка: Отсутствует память перевода {0}.", _commonTranslationMemory2);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void ExportTranslationMemoryButtonTest(bool withImportFile)
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

			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory, importFilePath: _tmx, client: _clientName);

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

			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory, importFilePath: _tmx, client: _clientName);

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

		// TODO: файл не был включен в проект. После добавления возникли ошибки (отсутствуют методы)
		// TODO: нужно актуализировать тест
/*
		[Test]
		public void EditLanguagesTranslationMemoryTest()
		{
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemory);

			Assert.IsTrue(_translationMemoriesPage.IsLanguagesForTranslationMemoryExists(
				_translationMemory, _englishLanguage, new List<string> { _russianLanguage }),
				"Произошла ошибка: Списки target языков не совпали.");

			_translationMemoriesPage
				.EditTranslationMemory(_translationMemory, addTargetLanguage: Language.Lithuanian)
				.RefreshPage<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemory);

			Assert.IsTrue(_translationMemoriesPage.IsLanguagesForTranslationMemoryExists(
				_translationMemory, _englishLanguage, new List<string> { _russianLanguage, _lithuanianLanguage }),
				"Произошла ошибка: Списки языков переводов не совпали.");
		}
*/
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
	}
}
