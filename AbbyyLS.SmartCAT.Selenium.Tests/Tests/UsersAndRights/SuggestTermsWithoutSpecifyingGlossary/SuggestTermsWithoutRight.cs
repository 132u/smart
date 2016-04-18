using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class SuggestTermsWithoutRight<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public SuggestTermsWithoutRight()
		{
			StartPage = StartPage.SignIn;
		}

		[SetUp]
		public void SetUp()
		{
			_loginHelper = new LoginHelper(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_glossariesPage = new GlossariesPage(Driver);

			_glossaryName = GlossariesHelper.UniqueGlossaryName();

			_additionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
			_loginHelper.LogInSmartCat(
				_additionalUser.Login,
				_additionalUser.NickName,
				_additionalUser.Password,
				"Personal");
		}

		[TearDown]
		public void TearDown()
		{
			if (_additionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, _additionalUser);
			}
		}

		[Test]
		public void SuggestTermsRightPersonalAccountTest()
		{
			_workspacePage.GoToGlossariesPage();

			Assert.IsFalse(_glossariesPage.IsSuggestedTermsButtonExist(),
				"Произошла ошибка:\nКнопка 'Suggested Terms' присутствует");
			
			Assert.IsFalse(_glossariesPage.IsSuggestTermButtonExist(),
				"Произошла ошибка:\nКнопка 'Suggest Term' присутствует");

			_glossariesHelper.CreateGlossary(_glossaryName);

			Assert.IsFalse(_glossariesPage.IsSuggestedTermsButtonExist(),
				"Произошла ошибка:\nКнопка 'Suggested Terms' присутствует");

			Assert.IsFalse(_glossariesPage.IsSuggestTermButtonExist(),
				"Произошла ошибка:\nКнопка 'Suggest Term' присутствует");
		}

		private string _glossaryName;
		private WorkspacePage _workspacePage;
		private LoginHelper _loginHelper;
		private GlossariesHelper _glossariesHelper;
		private GlossariesPage _glossariesPage;
		private TestUser _additionalUser;
	}
}
