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
	class TranslationMemoriesSearchSpecificClientTests<TWebDriverProvider> : TranslationMemoriesUserRightsBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var groupName = Guid.NewGuid().ToString();
			_commonClientName = _clientsPage.GetClientUniqueName();
			_commonClientName2 = _clientsPage.GetClientUniqueName();
			_translationMemoryWithClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryWithoutClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryWithSecondClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_commonClientName);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_commonClientName2);

			_workspacePage.GoToTranslationMemoriesPage();
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemoryWithoutClient);
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemoryWithClient, client: _commonClientName);
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemoryWithSecondClient, client: _commonClientName2);

			_userRightsHelper.CreateGroupWithSpecificRightsAndSpecificClient(
				AdditionalUser.NickName,
				groupName,
				RightsType.TMSearch,
				_commonClientName);
		}

		[Test]
		public void TranslationMemoriesViewTest()
		{
			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithClient),
				"Произошла ошибка: Отсутсвует память перевод {0}.", _translationMemoryWithClient);

			Assert.IsFalse(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithSecondClient),
				"Произошла ошибка: Присутсвует память перевод {0}.", _translationMemoryWithSecondClient);

			Assert.IsFalse(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithoutClient),
				"Произошла ошибка: Притсутсвует память перевод {0}.", _translationMemoryWithoutClient);
		}

		[Test]
		public void TranslationMemoriesSettingsViewTest()
		{
			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemoryWithClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryInformationOpen(_translationMemoryWithClient),
				"Произошла ошибка: Не открылись настройки памяти перевод {0}.", _translationMemoryWithClient);
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
	}
}
