using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	public class ManageGlossariesSpecificClientTests<TWebDriverProvider> : ManageGlossariesSpecificClientBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void BeforeTest()
		{
			try
			{
				CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
				_loginHelper = new LoginHelper(Driver);
				_loginHelper.Authorize(StartPage, AdditionalUser);
				_exportNotification.CancelAllNotifiers<Pages.Projects.ProjectsPage>();
				_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			}
			catch (Exception ex)
			{
				CustomTestContext.WriteLine("Произошла ошибка в SetUp {0}", ex.ToString());
				throw;
			}
		}

		[Test]
		public void ClientNameInCreateGlossaryDialogTest()
		{
			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickCreateGlossaryButton();

			Assert.AreEqual(_commonClientName, _newGlossaryDialog.GetClientName(),
				"Произошла ошибка: Неверное название клиента в диалоге создания глоссария.");
		}
		
		[Test]
		public void CreateGlossaryTest()
		{
			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossaryUniqueName),
				"Произошла ошибка: глоссарий отсутствует в списке");
		}

		[Test]
		public void ViewGlossariesTest()
		{
			var glossaryUniqueName2 = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(glossaryUniqueName2);

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(glossaryUniqueName2),
				"Произошла ошибка: глоссарий {0} отсутствует в списке", glossaryUniqueName2);

			Assert.IsFalse(_glossariesPage.IsGlossaryExist(_commonGlossaryUniqueName),
				"Произошла ошибка: глоссарий {0} присутствует в списке", _commonGlossaryUniqueName);
		}
		
		[Test]
		public void ViewConceptsAnotherUserTest()
		{
			var term1 = "term-" + Guid.NewGuid();
			var term2 = "term-" + Guid.NewGuid();
			var glossaryUniqueName2 = GlossariesHelper.UniqueGlossaryName();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			
			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(glossaryUniqueName2, client: _commonClientName);
			_glossaryPage.CreateTerm(term1, term2);

			_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);

			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(glossaryUniqueName2);

			Assert.IsTrue(
				_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(
				_glossaryPage.IsSingleTermWithTranslationExists(term1, term2),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void ViewConceptsTest()
		{
			var term1 = "term-" + Guid.NewGuid();
			var term2 = "term-" + Guid.NewGuid();
			
			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);
			_glossaryPage.CreateTerm(term1, term2);

			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			Assert.IsTrue(
				_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(
				_glossaryPage.IsSingleTermWithTranslationExists(term1, term2),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}
	}
}

