﻿using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.LingvoDictionaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Search;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.LingvoDictionaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[LingvoDictionaries]
	[Standalone]
	class LingvoDictionariesTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public LingvoDictionariesTests()
		{
			StartPage = StartPage.Admin;
		}

		[SetUp]
		public void LingvoDictionariesSetUp()
		{
			_adminHelper = new AdminHelper(Driver);
			_commonHelper = new CommonHelper(Driver);
			_loginHelper = new LoginHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);

			_searchPage = new SearchPage(Driver);
			_lingvoDictionariesPage = new LingvoDictionariesPage(Driver);

			if (!ConfigurationManager.Standalone)
			{
				var accountUniqueName = AdminHelper.GetAccountUniqueName();

				_adminHelper
					.CreateAccountIfNotExist(
						accountName: accountUniqueName,
						workflow: true,
						features: new List<string>
					{
						Feature.Clients.ToString(),
						Feature.Domains.ToString(),
						Feature.LingvoDictionaries.ToString(),
					})
					.AddUserToSpecificAccount(ThreadUser.Login, accountUniqueName);

				_commonHelper.GoToSignInPage();

				_loginHelper.LogInSmartCat(
					ThreadUser.Login,
					ThreadUser.NickName,
					ThreadUser.Password,
					accountUniqueName);
			}

			_workspacePage
				.OpenHideMenuIfClosed()
				.ExpandResourcesIfNotExpanded();

			Assert.IsTrue(_workspacePage.IsLingvoDictionariesMenuItemDisplayed(),
				"Произошла ошибка:\n в меню отсутствует 'Lingvo Dictionaries'");

			_workspacePage.GoToLingvoDictionariesPage();

			Assert.IsTrue(_lingvoDictionariesPage.IsLingvoDictionariesListNotEmpty(),
				"Произошла ошибка:\n список словарей пуст.");

			_workspacePage.GoToSearchPage();
		}

		[Test]
		public void SearchTest()
		{
			var search_query = "tester";

			_searchPage.InitSearch(search_query);

			Assert.IsTrue(_searchPage.IsSearchResultDisplay(),
				"Произошла ошибка: \n не появились результаты поиска.");
		}

		[Test]
		public void TranslationReferenceTest()
		{
			var search_query = "tester";
			var translation_reference = "тестировщик";

			_searchPage.InitSearch(search_query);

			Assert.IsTrue(_searchPage.IsTranslationReferenceExist(translation_reference),
				"Произошла ошибка:\n ссылка на перевод отсутствует.");
		}

		[Test]
		public void DefinitionsTabActiveTest()
		{
			var source_lang = "en";
			var target_lang = "en";
			var search_query = "tester";

			_searchPage
				.SelectSourceLanguage(source_lang)
				.SelectTargetLanguage(target_lang)
				.InitSearch(search_query);

			Assert.IsTrue(_searchPage.IsDefinitionTabActive(),
				"Произошла ошибка:\n вкладка Definitions неактивна.");
		}

		[Test]
		public void ReverseTranslationTest()
		{
			var search_query = "tester";
			var translation_word = "испытательное устройство";

			_searchPage
				.InitSearch(search_query)
				.ClickTranslationWord(translation_word)
				.ClickTranslationFormReference();

			Assert.IsTrue(_searchPage.IsWordByWordTranslationAppear(),
				"Произошла ошибка:\n перевод по словам не появился.");

			Assert.IsTrue(_searchPage.IsReverseTranslationListExist(),
				"Произошла ошибка:\n таблица с обратным переводом со ссылками не отображается.");
		}

		[Test]
		public void AutoReverseTest()
		{
			var source_lang = "ru";
			var target_lang = "en";
			var search_query = "tester";

			_searchPage
				.SelectSourceLanguage(source_lang)
				.SelectTargetLanguage(target_lang)
				.InitSearch(search_query);

			Assert.IsTrue(_searchPage.IsAutoreversedMessageExist(),
				"Произошла ошибка:\n сообщение об автоматическом изменении языка не появилось.");

			Assert.IsTrue(_searchPage.IsAutoreversedReferenceExist(),
				"Произошла ошибка:\n ссылка автоматического изменения языка отсутствует.");
		}

		private AdminHelper _adminHelper;
		private LoginHelper _loginHelper;
		private WorkspacePage _workspacePage;
		private CommonHelper _commonHelper;

		private SearchPage _searchPage;
		private LingvoDictionariesPage _lingvoDictionariesPage;
	}
}