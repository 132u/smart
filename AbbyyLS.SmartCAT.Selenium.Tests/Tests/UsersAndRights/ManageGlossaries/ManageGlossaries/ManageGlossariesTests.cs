using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class ManageGlossariesTests<TWebDriverProvider> : ManageGlossariesBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateGlossaryInGlossariesPageTest()
		{
			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossaryUniqueName),
				"Произошла ошибка: глоссарий {0} отсутствует в списке.", _glossaryUniqueName);
		}

		[Test]
		public void CreateGlossarySpecificClientInGlossariesPageTest()
		{
			var clientName = _clientsPage.GetClientUniqueName();
			var glossaryUniqueName2 = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.SignOut();
			_loginHelper.Authorize(StartPage, ThreadUser);
			
			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(clientName);

			_workspacePage.SignOut();
			_loginHelper.Authorize(StartPage, AdditionalUser);

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(glossaryUniqueName2, client: clientName);

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(glossaryUniqueName2),
				"Произошла ошибка: глоссарий {0} отсутствует в списке.", glossaryUniqueName2);
		}

		[Test]
		public void ViewGlossariesInAccountTest()
		{
			var glossaryUniqueName2 = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.SignOut();
			_loginHelper.Authorize(StartPage, ThreadUser);

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(glossaryUniqueName2, client: _clientName);

			_workspacePage.SignOut();
			_loginHelper.Authorize(StartPage, AdditionalUser);

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(glossaryUniqueName2),
				"Произошла ошибка: глоссарий {0} отсутствует в списке.", glossaryUniqueName2);
		}

		[Test]
		public void DeleteGlossaryTest()
		{
			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog.ClickDeleteGlossaryButton();

			Assert.IsTrue(_glossaryPropertiesDialog.IsConfirmDeleteMessageDisplayed(),
				"Произошла ошибка: \nне появилось сообщение с подтверждением удаления глоссария");

			_glossaryPropertiesDialog.ClickConfirmDeleteGlossaryButton();

			Assert.IsTrue(_glossariesPage.IsGlossaryNotExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий присутствует в списке.");
		}
	}
}
