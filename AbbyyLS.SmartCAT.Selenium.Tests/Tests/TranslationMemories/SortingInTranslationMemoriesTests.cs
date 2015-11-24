﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	internal class SortingInTranslationMemoriesTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInTranslationMemoriesTests()
		{
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_translationMemoriesHelper = new TranslationMemoriesHelper(Driver);
			_translationMemoriesHelper
				.GoToTranslationMemoriesPage()
				.CloseAllNotifications();
		}

		[Test]
		public void SortByTMNameTest()
		{
			_translationMemoriesHelper.ClickSortByTMName();

			Assert.IsFalse(_translationMemoriesPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCreationDateTest()
		{
			_translationMemoriesHelper.ClickSortByCreationDate();

			Assert.IsFalse(_translationMemoriesPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		private TranslationMemoriesHelper _translationMemoriesHelper;
		private TranslationMemoriesPage _translationMemoriesPage;
	}
}
