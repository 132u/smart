﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
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
			_glossariesHelper = new GlossariesHelper(Driver);
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
		[ProjectGroups]
		public void SortByProjectGroupsTest()
		{
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			var projectGroupsHelper = new ProjectGroupsHelper(Driver);
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
		[Clients]
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

		private GlossariesHelper _glossariesHelper;

	}
}
