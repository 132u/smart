using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
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

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName, PathProvider.DocumentFile)
				.GoToProjectSettingsPage(projectUniqueName);
		}

		[Test]
		public void SortByTranslationDocumentTest()
		{
			_projectSettingsHelper
				.ClickSortByTranslationDocument()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByTypeTest()
		{
			_projectSettingsHelper
				.ClickSortByType()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByStatusTest()
		{
			_projectSettingsHelper
				.ClickSortByStatus()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByTargetTest()
		{
			_projectSettingsHelper
				.ClickSortByTarget()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByAuthorTest()
		{
			_projectSettingsHelper
				.ClickSortByAuthor()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByCreatedTest()
		{
			_projectSettingsHelper
				.ClickSortByCreated()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByQATest()
		{
			_projectSettingsHelper
				.ClickSortByQA()
				.AssertAlertNoExist();
		}

		private ProjectSettingsHelper _projectSettingsHelper;
		private CreateProjectHelper _createProjectHelper;
	}
}
