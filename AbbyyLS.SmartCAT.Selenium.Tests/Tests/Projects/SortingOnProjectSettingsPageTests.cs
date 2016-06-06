using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Projects]
	internal class SortingOnProjectSettingsPageTests<TWebDriverSettings> : BaseProjectTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInProjectsTests()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { document });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);
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
