using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	class SortingInGlossariesTests<TWebDriverSettings> : BaseTest<TWebDriverSettings>
		where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupSortingInGlossariesTests()
		{
			_glossariesHelper.GoToGlossariesPage();
		}

		[Test]
		public void SortByNameTest()
		{
			_glossariesHelper
				.ClickSortByName()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByLanguagesTest()
		{
			_glossariesHelper
				.ClickSortByLanguages()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByTermsAddedTest()
		{
			_glossariesHelper
				.ClickSortByTermsAdded()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByTermsUnderReviewTest()
		{
			_glossariesHelper
				.ClickSortByTermsUnderReview()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByCommentTest()
		{
			_glossariesHelper
				.ClickSortByComment()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByProjectGroupsTest()
		{
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			var projectGroupsHelper = new ProjectGroupsHelper();
			var projectGroupUniqueName = ProjectGroupsHelper.GetProjectGroupUniqueName();

			projectGroupsHelper
				.GoToProjectGroupsPage()
				.CreateProjectGroup(projectGroupUniqueName);

			_glossariesHelper
				.GoToGlossariesPage()
				.CreateGlossary(glossaryUniqueName, projectGroupName: projectGroupUniqueName)
				.GoToGlossariesPage()
				.ClickSortByProjectGroups()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByClient()
		{
			_glossariesHelper
				.ClickSortByClient()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByDateModified()
		{
			_glossariesHelper
				.ClickSortGlossariesToDateModified()
				.AssertAlertNoExist();
		}

		[Test]
		public void SortByModifiedByTest()
		{
			_glossariesHelper
				.ClickSortByModifiedBy()
				.AssertAlertNoExist();
		}

		private GlossariesHelper _glossariesHelper = new GlossariesHelper();
		
	}
}
