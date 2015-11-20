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
			_projectSettingsPage = new ProjectSettingsPage(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName, filePath: PathProvider.DocumentFile)
				.GoToProjectSettingsPage(projectUniqueName);
		}

		[Test]
		public void SortByTranslationDocumentTest()
		{
			_projectSettingsPage.ClickSortByTranslationDocument();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTypeTest()
		{
			_projectSettingsPage.ClickSortByType();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByStatusTest()
		{
			_projectSettingsPage.ClickSortByStatus();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTargetTest()
		{
			_projectSettingsPage.ClickSortByTarget();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByAuthorTest()
		{
			_projectSettingsPage.ClickSortByAuthor();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCreatedTest()
		{
			_projectSettingsPage.ClickSortByCreated();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByQATest()
		{
			_projectSettingsPage.ClickSortByQA();

			Assert.IsFalse(_projectSettingsPage.IsAlertExist(),
				"Произошла ошибка: \n при сортировке появился Alert.");
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsPage _projectSettingsPage;
	}
}
