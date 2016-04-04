using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.TranslationMemories.TranslationMemoriesSearch
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TranslationMemoriesSearchTests<TWebDriverProvider> : TranslationMemoriesUserRightsBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var groupName = Guid.NewGuid().ToString();
			_clientName2 = _clientsPage.GetClientUniqueName();
			_translationMemoryWithClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryWithoutClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName2);

			_workspacePage.GoToTranslationMemoriesPage();
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemoryWithClient);
			_translationMemoriesHelper.CreateTranslationMemory(_translationMemoryWithoutClient, client: _clientName2);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				groupName,
				new List<RightsType>{RightsType.TMSearch});
		}

		[Test]
		public void TranslationMemoriesVisibilityTest()
		{
			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithClient),
				"Произошла ошибка: Отсутсвует память перевод {0}.", _translationMemoryWithClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithoutClient),
				"Произошла ошибка: Отсутсвует память перевод {0}.", _translationMemoryWithoutClient);
		}

		[Test]
		public void TranslationMemoriesSettingsVisibilityTest()
		{
			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemoryWithClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryInformationOpen(_translationMemoryWithClient),
				"Произошла ошибка: Не открылись настройки памяти перевод {0}.", _translationMemoryWithClient);

			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemoryWithoutClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryInformationOpen(_translationMemoryWithoutClient),
				"Произошла ошибка: Не открылись настройки памяти перевод {0}.", _translationMemoryWithoutClient);
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
	}
}
