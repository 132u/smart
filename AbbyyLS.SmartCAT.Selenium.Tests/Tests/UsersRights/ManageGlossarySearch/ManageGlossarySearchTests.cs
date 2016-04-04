using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossarySearch
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageGlossarySearchTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_loginHelper.Authorize(StartPage, AdditionalUser);
				_exportNotification.CancelAllNotifiers<ProjectsPage>();
				_workspacePage.GoToGlossariesPage();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_exportNotification = new ExportNotification(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_glossaryHelper = new GlossariesHelper(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_clientsPage = new ClientsPage(Driver);

			_clientName = _clientsPage.GetClientUniqueName();
			_glossary = GlossariesHelper.UniqueGlossaryName();
			_glossaryWithClient = GlossariesHelper.UniqueGlossaryName();
			var groupName = Guid.NewGuid().ToString();

			_term1 = "term-" + Guid.NewGuid();
			_term2 = "term-" + Guid.NewGuid();
			_term3 = "term-" + Guid.NewGuid();
			_term4 = "term-" + Guid.NewGuid();
			_term5 = "term-" + Guid.NewGuid();
			_term6 = "term-" + Guid.NewGuid();

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
			
			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				groupName,
				new List<RightsType>{RightsType.GlossarySearch});

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);

			_workspacePage.GoToGlossariesPage();
			_glossaryHelper.CreateGlossary(_glossary);
			_glossaryPage.CreateTerm(_term1, _term2);
			_glossaryPage.CreateTerm(_term5, _term6);

			_workspacePage.GoToGlossariesPage();
			_glossaryHelper.CreateGlossary(_glossaryWithClient, client: _clientName);
			_glossaryPage.CreateTerm(_term3, _term4);
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
		}

		[Test]
		public void IsCreateGlossaryButtonDisplayedTest()
		{
			Assert.IsFalse(_glossariesPage.IsCreateGlossaryButtonDisplayed(),
				"Произошла ошибка: Отображается кнопка создания глоссария.");
		}

		[Test]
		public void GlossariesViewTest()
		{
			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossary),
				"Произошла ошибка: Отсутствует глоссарий {0}.", _glossary);

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossaryWithClient),
				"Произошла ошибка: Отсутствует глоссарий {0}.", _glossaryWithClient);
		}

		[Test]
		public void ConceptsViewTest()
		{
			_glossariesPage.ClickGlossaryRow(_glossary);

			Assert.IsTrue(
				_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 2),
				"Произошла ошибка: Глоссарий {0} содержит неверное количество терминов.", _glossary);

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(_term1, _term2),
				"Произошла ошибка: Текст в термине не совпадает с ожидаемым.");

			Assert.IsTrue(_glossaryPage.IsSingleTermWithTranslationExists(_term5, _term6),
				"Произошла ошибка: Текст в термине не совпадает с ожидаемым.");

			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(_glossaryWithClient);

			Assert.IsTrue(
				_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка: Глоссарий {0} содержит неверное количество терминов.", _glossaryWithClient);

			Assert.IsTrue(
				_glossaryPage.IsSingleTermWithTranslationExists(_term3, _term4),
				"Произошла ошибка: Текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void SearchByFirstTermTest()
		{
			_glossariesPage.ClickGlossaryRow(_glossary);

			_glossaryPage.SearchTerm(_term1);

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");

			Assert.AreEqual(_term1, _glossaryPage.GetTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}
		
		[Test]
		public void SearchBySecondTermTest()
		{
			_glossariesPage.ClickGlossaryRow(_glossary);

			_glossaryPage.SearchTerm(_term2);

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");

			Assert.AreEqual(_term2, _glossaryPage.GetTermText(termNumber: 2),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		protected string _glossary;
		protected string _glossaryWithClient;
		protected string _clientName;

		protected string _term1;
		protected string _term2;
		protected string _term3;
		protected string _term4;
		protected string _term5;
		protected string _term6;

		protected GlossaryPage _glossaryPage;
		protected ClientsPage _clientsPage;
		protected WorkspacePage _workspacePage;
		protected LoginHelper _loginHelper;
		protected ExportNotification _exportNotification;
		protected UserRightsHelper _userRightsHelper;
		protected GlossariesHelper _glossaryHelper;
		protected GlossariesPage _glossariesPage;
	}
}
