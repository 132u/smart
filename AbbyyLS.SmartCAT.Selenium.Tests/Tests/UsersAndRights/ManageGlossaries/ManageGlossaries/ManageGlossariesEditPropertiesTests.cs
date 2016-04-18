using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageGlossariesEditPropertiesTests<TWebDriverProvider> : ManageGlossariesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void BeforeTest()
		{
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_clientName = _clientsPage.GetClientUniqueName();
			_projectGroup = _projectGroupsPage.GetProjectGroupUniqueName();

			_loginHelper.Authorize(StartPage, ThreadUser);

			_workspacePage.GoToClientsPage();
			_clientsPage.CreateNewClient(_clientName);

			_workspacePage.GoToProjectGroupsPage();
			_projectGroupsPage.CreateProjectGroup(_projectGroup);

			_loginHelper.Authorize(StartPage, AdditionalUser);

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);
			_glossaryPage.OpenGlossaryProperties();
		}

		[Test]
		public void EditGlossaryNameTest()
		{
			var newGlossaryName = GlossariesHelper.UniqueGlossaryName();
			
			_glossaryPropertiesDialog
				.FillGlossaryName(newGlossaryName)
				.ClickSaveButton();

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(newGlossaryName),
				"Произошла ошибка: глоссарий отсутствует в списке");

			Assert.IsTrue(_glossariesPage.IsGlossaryNotExist(_glossaryUniqueName),
				"Произошла ошибка: глоссарий присутствует в списке.");
		}

		[Test]
		public void EditGlossaryCommentTest()
		{
			var newComment = "New Comment";
			
			_glossaryPropertiesDialog
				.FillGlossaryComment(newComment)
				.ClickSaveButton();

			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			Assert.AreEqual(newComment, _glossaryPropertiesDialog.GetGlossaryComment(),
				"Произошла ошибка: неверный комментарий.");
		}

		[Test]
		public void EditGlossaryClientTest()
		{
			_glossaryPropertiesDialog
				.SelectClient(_clientName)
				.ClickSaveButton();

			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			Assert.AreEqual(_clientName, _glossaryPropertiesDialog.GetGlossaryClient(),
				"Произошла ошибка: Неверно указан клиент глоссария.");
		}

		[Test]
		public void EditGlossaryProjectGroupTest()
		{
			_glossaryPropertiesDialog
				.SelectProjectGroup(_projectGroup)
				.ClickSaveButton();

			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			Assert.AreEqual(_projectGroup, _glossaryPropertiesDialog.GetGlossaryProjectGroup(),
				"Произошла ошибка: Неверно указан название группы поректа глоссария.");
		}

		[Test]
		public void AddGlossaryLanguagesTest()
		{
			_glossaryPropertiesDialog
				.AddLangauge(Language.German)
				.AddLangauge(Language.French)
				.ClickSaveButton();

			Assert.AreEqual(4, _glossaryPage.LanguageColumnCount(),
				"Произошла ошибка:\n неверное количество колонок с языками.");
		}
		
		[Test]
		public void DeleteGlossaryLanguagesTest()
		{
			_glossaryPropertiesDialog
				.ClickDeleteLanguageButton(1)
				.ClickSaveButton();

			Assert.AreEqual(1, _glossaryPage.LanguageColumnCount(),
				"Произошла ошибка:\n неверное количество колонок с языками.");
		}

		[Test]
		public void EditGlossaryLanguagesTest()
		{
			_glossaryPropertiesDialog
				.EditLangauge(Language.French, 1)
				.ClickSaveButton();

			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			Assert.AreEqual(Language.French.ToString(), _glossaryPropertiesDialog.GetLanguage(languageNumber: 1),
				"Произошла ошибка: Указан неверно язык в дропдауне №1");
		}
	}
}
