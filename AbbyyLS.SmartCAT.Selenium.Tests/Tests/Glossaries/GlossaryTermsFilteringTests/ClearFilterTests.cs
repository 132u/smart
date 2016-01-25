using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class GlossaryTermFilterTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossaryTermFilterTestsSetUp()
		{
			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.CreateTerm();
		}

		[Test]
		public void ClearFilterTest()
		{
			//TODO Убрать рефреш, когда пофиксят PRX-12015
			_workspacePage.RefreshPage<WorkspacePage>();

			var termsBeforeFilter = _glossaryPage.GetTermList();

			_glossaryPage
				.ClickFilterButton()
				.SelectCreatedDateOption(DateRange.Week)
				.SelectModifiedDateOption(DateRange.Month)
				.ClickAuthorDropdown()
				.SelectAuthor(ThreadUser.NickName)
				.ClickModifierDropdown()
				.SelectModifier(ThreadUser.NickName)
				.ClickClearButton();
			
			Assert.AreEqual(DateRange.Anytime.Description(), _filterDialog.GetCreatedDateDropdownText(),
				"Произошла ошибка:\nВ дропдауне Created указано недефолтное значение.");

			Assert.AreEqual(DateRange.Anytime.Description(), _filterDialog.GetModifiedDateDropdownText(),
				"Произошла ошибка:\nВ дропдауне Modified указано недефолтное значение.");

			Assert.IsTrue(_filterDialog.AreAuthorChecboxesUnchecked(),
				"Произошла ошибка:\nВ дропдауне Author стоит галочка.");
			
			Assert.IsTrue(_filterDialog.AreModifierChecboxesUnchecked(),
				"Произошла ошибка:\nВ дропдауне Modifier стоит галочка.");

			Assert.IsTrue(_filterDialog.AreLanguagesCheckedInDropdown(),
				"Произошла ошибка:\nНе все языки отмечены в дропдауне.");

			_filterDialog.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.GetTermList().SequenceEqual(termsBeforeFilter),
				"Произошла ошибка:\nСписки терминов не совпадают.");
		}

		[Test]
		public void CancelFilterTest()
		{
			//TODO Убрать рефреш, когда пофиксят PRX-12015
			_workspacePage.RefreshPage<WorkspacePage>();

			var termsBeforeFilter = _glossaryPage.GetTermList();

			_glossaryPage
				.ClickFilterButton()
				.SelectCreatedDateOption(DateRange.Week)
				.SelectModifiedDateOption(DateRange.Month)
				.ClickAuthorDropdown()
				.SelectAuthor(ThreadUser.NickName)
				.ClickModifierDropdown()
				.SelectModifier(ThreadUser.NickName)
				.ClickCancelButton();

			Assert.IsTrue(_glossaryPage.GetTermList().SequenceEqual(termsBeforeFilter),
					"Произошла ошибка:\nСписки терминов не совпадают.");
		}
	}
}
