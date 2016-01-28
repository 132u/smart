﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	internal class SortingOnProjectSettingsPageTests<TWebDriverSettings> : BaseProjectTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInProjectsTests()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.DocumentFile);
			_projectsPage.ClickProject(_projectUniqueName);
		}

		[Test]
		public void SortByTranslationDocumentTest()
		{
			_projectSettingsPage.ClickSortByTranslationDocumentAssumingAlert();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTypeTest()
		{
			_projectSettingsPage.ClickSortByTypeAssumingAlert();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByStatusTest()
		{
			_projectSettingsPage.ClickSortByStatusAssumingAlert();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTargetTest()
		{
			_projectSettingsPage.ClickSortByTargetAssumingAlert();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByAuthorTest()
		{
			_projectSettingsPage.ClickSortByAuthorAssumingAlert();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCreatedTest()
		{
			_projectSettingsPage.ClickSortByCreatedAssumingAlert();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByQATest()
		{
			_projectSettingsPage.ClickSortByQAAssumingAlert();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}
	}
}