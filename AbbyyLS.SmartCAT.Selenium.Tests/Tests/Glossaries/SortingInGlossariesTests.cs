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
			_glossariesHelper.ClickSortByName();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByLanguagesTest()
		{
			_glossariesHelper.ClickSortByLanguages();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTermsAddedTest()
		{
			_glossariesHelper.ClickSortByTermsAdded();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTermsUnderReviewTest()
		{
			_glossariesHelper.ClickSortByTermsUnderReview();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCommentTest()
		{
			_glossariesHelper.ClickSortByComment();

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
				.GoToGlossariesPage()
				.ClickSortByProjectGroups();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		[Clients]
		public void SortByClient()
		{
			_glossariesHelper.ClickSortByClient();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByDateModified()
		{
			_glossariesHelper.ClickSortGlossariesToDateModified();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByModifiedByTest()
		{
			_glossariesHelper.ClickSortByModifiedBy();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		private GlossariesHelper _glossariesHelper;

		private GlossariesPage _glossariesPage;

		private WorkspaceHelper _workspaceHelper;
		private ProjectGroupsPage _projectGroupsPage;
	}
}
