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
	class TranslationMemoriesContentSpecificClientTests<TWebDriverProvider> : EditTranslationMemoriesContentSpecificClientBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_clientName2 = _clientsPage.GetClientUniqueName();
			_translationMemoryWithClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryWithCommonClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			_translationMemoryWithoutClient = _translationMemoriesHelper.GetTranslationMemoryUniqueName();
			
			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			
			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName2);

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesHelper
				.CreateTranslationMemory(_translationMemoryWithoutClient)
				.CreateTranslationMemory(_translationMemoryWithCommonClient, client: _commonClientName)
				.CreateTranslationMemory(_translationMemoryWithClient, client: _clientName2);
			
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
		public void TranslationMemoriesVisibilityTest()
		{
			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithCommonClient),
				"Произошла ошибка: Отсутсвует память перевод {0}.", _translationMemoryWithCommonClient);

			Assert.IsFalse(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithClient),
				"Произошла ошибка: Притсутсвует память перевод {0}.", _translationMemoryWithClient);

			Assert.IsFalse(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithoutClient),
				"Произошла ошибка: Притсутсвует память перевод {0}.", _translationMemoryWithoutClient);
		}

		[Test]
		public void TranslationMemoriesSettingsVisibilityTest()
		{
			_translationMemoriesPage.OpenTranslationMemoryInformation(_translationMemoryWithCommonClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryInformationOpen(_translationMemoryWithCommonClient),
				"Произошла ошибка: Не открылись настройки памяти перевод {0}.", _translationMemoryWithCommonClient);
		}

		[Test]
		public void ExportTranslationMemoryTest()
		{
			_translationMemoriesPage.ExportTM(_translationMemoryWithCommonClient);

			Assert.IsTrue(_exportNotification.IsFileDownloaded(String.Format("{0}*-export.tmx", _translationMemoryWithCommonClient)),
				"Произошла ошибка: файл не загрузился");
		}

		[Test]
		public void TranslationMemorySearchTest()
		{
			_translationMemoriesPage
				.CloseAllNotifications<TranslationMemoriesPage>()
				.SearchForTranslationMemory(_translationMemoryWithCommonClient);

			Assert.IsTrue(_translationMemoriesPage.IsTranslationMemoryExist(_translationMemoryWithCommonClient),
				"Произошла ошибка:\n ТМ {0} не представлена в списке ТМ", _translationMemoryWithCommonClient);
		}
	}
}
