using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class SortingInGlossariesTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInGlossariesTests()
		{
			_glossariesPage = new GlossariesPage(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_glossariesHelper.GoToGlossariesPage();
		}

		[Test]
		public void SortByNameTest()
		{
			_glossariesPage.ClickSortByName();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByLanguagesTest()
		{
			_glossariesPage.ClickSortByLanguages();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTermsAddedTest()
		{
			_glossariesPage.ClickSortByTermsAdded();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTermsUnderReviewTest()
		{
			_glossariesPage.ClickSortByTermsUnderReview();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCommentTest()
		{
			_glossariesPage.ClickSortByComment();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		[ProjectGroups]
		public void SortByProjectGroupsTest()
		{
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			var projectGroupUniqueName = _projectGroupsPage.GetProjectGroupUniqueName();

			_workspaceHelper.GoToProjectGroupsPage();

			_projectGroupsPage.CreateProjectGroup(projectGroupUniqueName);

			_glossariesHelper
				.GoToGlossariesPage()
				.CreateGlossary(glossaryUniqueName, projectGroupName: projectGroupUniqueName)
				.GoToGlossariesPage();

			_glossariesPage.ClickSortByProjectGroups();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		[Clients]
		public void SortByClient()
		{
			_glossariesPage.ClickSortByClient();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByDateModified()
		{
			_glossariesPage.ClickSortByDateModified();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByModifiedByTest()
		{
			_glossariesPage.ClickSortByModifiedBy();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		private GlossariesHelper _glossariesHelper;

		private GlossariesPage _glossariesPage;

		private WorkspaceHelper _workspaceHelper;
		private ProjectGroupsPage _projectGroupsPage;
	}
}
