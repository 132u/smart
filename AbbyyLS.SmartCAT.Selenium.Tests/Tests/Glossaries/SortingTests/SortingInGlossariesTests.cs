using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Glossaries]
	class SortingInGlossariesTests<TWebDriverSettings>
		: BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInGlossariesTests()
		{
			_glossariesPage = new GlossariesPage(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);

			_workspacePage.GoToGlossariesPage();
		}

		[Test]
		public void SortByNameTest()
		{
			_glossariesPage.ClickSortByNameAssumingAlert();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByLanguagesTest()
		{
			_glossariesPage.ClickSortByLanguagesAssumingAlert();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTermsAddedTest()
		{
			_glossariesPage.ClickSortByTermsAddedAssumingAlert();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByTermsUnderReviewTest()
		{
			_glossariesPage.ClickSortByTermsUnderReviewAssumingAlert();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByCommentTest()
		{
			_glossariesPage.ClickSortByCommentAssumingAlert();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		[ProjectGroups]
		public void SortByProjectGroupsTest()
		{
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			var projectGroupUniqueName = _projectGroupsPage.GetProjectGroupUniqueName();

			_workspacePage.GoToProjectGroupsPage();

			_projectGroupsPage.CreateProjectGroup(projectGroupUniqueName);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(glossaryUniqueName, projectGroupName: projectGroupUniqueName);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSortByProjectGroupsAssumingAlert();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		[Clients]
		public void SortByClient()
		{
			_glossariesPage.ClickSortByClientAssumingAlert();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByDateModified()
		{
			_glossariesPage.ClickSortByDateModifiedAssumingAlert();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		[Test]
		public void SortByModifiedByTest()
		{
			_glossariesPage.ClickSortByModifiedByAssumingAlert();

			Assert.IsFalse(_glossariesPage.IsAlertExist(), "Произошла ошибка: \n при сортировке появился Alert.");
		}

		private GlossariesHelper _glossariesHelper;
		private GlossariesPage _glossariesPage;
		private WorkspacePage _workspacePage;
		private ProjectGroupsPage _projectGroupsPage;
	}
}
