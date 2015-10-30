using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	internal class SortingInProjectsTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInProjectsTests()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName, PathProvider.DocumentFile)
				.GoToProjectSettingsPage(projectUniqueName);
		}

		[Test]
		public void SortByTranslationDocumentTest()
		{
			_projectSettingsHelper.ClickSortByTranslationDocument();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTypeTest()
		{
			_projectSettingsHelper.ClickSortByType();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByStatusTest()
		{
			_projectSettingsHelper.ClickSortByStatus();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTargetTest()
		{
			_projectSettingsHelper.ClickSortByTarget();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByAuthorTest()
		{
			_projectSettingsHelper.ClickSortByAuthor();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCreatedTest()
		{
			_projectSettingsHelper.ClickSortByCreated();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByQATest()
		{
			_projectSettingsHelper.ClickSortByQA();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		private ProjectSettingsHelper _projectSettingsHelper;
		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsPage _projectSettingsPage;
	}
}
