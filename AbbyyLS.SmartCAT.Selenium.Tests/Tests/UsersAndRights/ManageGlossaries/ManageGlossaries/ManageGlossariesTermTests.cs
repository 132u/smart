using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ManageGlossariesTermTests<TWebDriverProvider> : ManageGlossariesBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateTermTest()
		{
			_glossaryPage.CreateTerm();

			Assert.IsTrue(
				_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");
		}

		[Test]
		public void DeleteTermTest()
		{
			var term1 = "Term1";
			var term2 = "Term2";

			_glossaryPage
				.CreateTerm(term1, term2)
				.DeleteTerm(term1, term2);

			Assert.IsTrue(
				_glossaryPage.IsDeleteButtonDisappeared(term1, term2),
				"Произошла ошибка: \nне исчезла кнопка удаления");

			Assert.IsTrue(
				_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 0),
				"Произошла ошибка:\nневерное количество терминов");
		}

		[Test]
		public void EditTermTest()
		{
			var term1 = "Term 1";
			var term2 = "Term 2";

			_glossaryPage
				.CreateTerm(term1, term2)
				.EditDefaultTerm(term1, term2, term1 + DateTime.Now);

			Assert.IsTrue(
				_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");
		}
		
		[Test]
		public void SuggestTermWithGlossaryFromGlossaryPageTest()
		{
			var term1 = "term-" + Guid.NewGuid();
			var term2 = "term-" + Guid.NewGuid();
			
			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1, term2, glossary: _glossaryUniqueName)
				.ClickSaveButtonExpectingGlossaryPage();

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickSuggestedTermsButton();

			Assert.IsTrue(
				_suggestedTermsPageForAllGlossaries.IsSuggestedTermDisplayed(term1, term2),
				"Произошла ошибка: строка с термином не найдена в списке");
		}

		[Test]
		public void AcceptSuggestedTermWithGlossaryFromGlossaryPageTest()
		{
			var term1 = "term-" + Guid.NewGuid();
			var term2 = "term-" + Guid.NewGuid();

			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);
			_glossaryPage.ClickSuggestTermButton();

			_suggestTermDialog
				.FillSuggestTermDialog(term1: term1, term2: term2)
				.ClickSaveButtonExpectingGlossaryPage();

			_glossaryPage.ClickSuggestedTermsButton();

			_suggestedTermsPageForCurrentGlossaries.AcceptSuggestedTerm(term1, term2);

			_workspacePage.GoToGlossariesPage();

			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			Assert.IsTrue(
				_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");

			Assert.IsTrue(
				_glossaryPage.IsSingleTermWithTranslationExists(term1, term2),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");
		}

		[Test]
		public void AddTermsInEditorTest()
		{
			var projectName = _createProjectHelper.GetProjectUniqueName();
			var term1 = "term-" + Guid.NewGuid();
			var term2 = "term-" + Guid.NewGuid();
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);

			_workspacePage.GoToProjectsPage();
			_createProjectHelper.CreateNewProject(projectName, glossaryName: glossaryUniqueName);
			_projectsPage.ClickProject(projectName);

			_projectSettingsHelper
				.UploadDocument(new[] { PathProvider.DocumentFile })
				.AssignTasksOnDocument(PathProvider.DocumentFile, AdditionalUser.NickName, projectName);

			_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);

			_projectsPage
				.OpenProjectInfo(projectName)
				.ClickDocumentRefExpectingEditorPage(PathProvider.DocumentFile);
			
			_editorPage.ClickAddTermButton();

			_addTermDialog.AddNewTerm(term1, term2, glossaryName: glossaryUniqueName);

			Assert.IsTrue(_editorPage.IsTermSaved(), "Произошла ошибка:\n сообщение о том, что термин сохранен, не появилось");

			Assert.IsTrue(
				_editorPage.IsTermSavedMessageDisappeared(),
				"Произошла ошибка:\n сообщение о том, что термин сохранен, не исчезло");
		}

		[Test]
		public void ViewGlossariesConceptsTest()
		{
			var glossaryUniqueName2 = GlossariesHelper.UniqueGlossaryName();
			var dictionary = new Dictionary<string, string> { { "first", "первый" }, { "sentence", "предложение" } };

			_loginHelper.Authorize(StartPage, ThreadUser);

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(glossaryUniqueName2, client: _clientName);

			_glossaryPage.CreateTerms(dictionary);

			_workspacePage.SignOut();
			_loginHelper.Authorize(StartPage, AdditionalUser);

			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(glossaryUniqueName2);

			Assert.IsTrue(
				_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 2),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}
	}
}
