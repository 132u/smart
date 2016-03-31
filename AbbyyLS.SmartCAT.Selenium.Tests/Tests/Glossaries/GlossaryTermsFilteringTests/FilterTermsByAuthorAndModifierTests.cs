using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FilterTermsByAuthorAndModifierTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossaryTermFilterTestsSetUp()
		{
			_signInPage = new SignInPage(Driver);
			_commonHelper = new CommonHelper(Driver);

			_secondUser = null;

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.CreateTerm();
		}

		[TearDown]
		public void TearDown()
		{
			if (_secondUser != null)
			{
				ReturnUser(ConfigurationManager.Users, _secondUser);
			}
		}

		[Test]
		public void AuthorFilterTest()
		{
			_workspacePage.SignOut();

			_secondUser = TakeUser(ConfigurationManager.Users);

			_commonHelper.GoToSignInPage();

			_signInPage
				.SubmitForm(_secondUser.Login, _secondUser.Password)
				.SelectAccount();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			_glossaryPage.CreateTerm(firstTerm: "term1FromSecondUser", secondTerm: "term2FromSecondUser");

			_workspacePage.RefreshPage<WorkspacePage>();

			_glossaryPage
				.ClickFilterButton()
				.ClickAuthorDropdown()
				.SelectAuthor(ThreadUser.NickName)
				.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.IsCreatedByFilterLabelDisplayed(),
				"Произошла ошибка:\nФильтр 'Created by' не отображается в таблице терминов.");

			Assert.IsTrue(_glossaryPage.GetCreatedByFilterText().Contains(ThreadUser.NickName),
				"Произошла ошибка:\nФильтр 'Created by' не содержит имя автора {0}.", ThreadUser.NickName);

			Assert.AreEqual(_glossaryPage.TermsCount(), 1,
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов.");
		}

		[Test]
		public void ModifierFilterTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);

			_glossaryPage.CreateTerm(firstTerm: "term1FromFirstUser", secondTerm: "term2FromFirstUser");

			_workspacePage.SignOut();

			_signInPage
				.SubmitForm(_secondUser.Login, _secondUser.Password)
				.SelectAccount();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			_glossaryPage
				.EditDefaultTerm("firstTerm", "secondTerm", "edit")
				.CreateTerm(firstTerm: "term1FromSecondUser", secondTerm: "term2FromSecondUser");

			//TODO Убрать рефреш, когда пофиксят PRX-12015
			_workspacePage.RefreshPage<WorkspacePage>();

			_glossaryPage
				.ClickFilterButton()
				.ClickModifierDropdown()
				.SelectModifier(_secondUser.NickName)
				.ClickApplyButton();

			Assert.IsTrue(_glossaryPage.IsModifiedByFilterLabelDisplayed(),
				"Произошла ошибка:\nФильтр 'Modified by' не отображается в таблице терминов.");

			Assert.IsTrue(_glossaryPage.GetModifiedByFilterText().Contains(_secondUser.NickName),
				"Произошла ошибка:\nФильтр 'Modified by' не содержит имя автора {0}.", _secondUser.NickName);

			Assert.AreEqual(2, _glossaryPage.TermsCount(),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов.");
		}

		private TestUser _secondUser;

		private SignInPage _signInPage;
		private CommonHelper _commonHelper;
	}
}
