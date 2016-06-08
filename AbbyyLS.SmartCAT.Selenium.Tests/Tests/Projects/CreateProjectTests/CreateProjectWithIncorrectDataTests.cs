using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[Projects]
	class CreateProjectWithIncorrectDataTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase("*")]
		[TestCase("|")]
		[TestCase("\\")]
		[TestCase(":")]
		[TestCase("\"")]
		[TestCase("<\\>")]
		[TestCase("?")]
		[TestCase("/")]
		public void CreateProjectForbiddenSymbolsTest(string forbiddenChar)
		{
			var projectUniqueNameForbidden = _createProjectHelper.GetProjectUniqueName() + forbiddenChar;

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage.FillGeneralProjectInformation(projectUniqueNameForbidden);

			Assert.IsTrue(_newProjectSettingsPage.IsForbiddenSymbolsInNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о недопустимых символах в имени");

			Assert.IsTrue(_newProjectSettingsPage.IsNameInputValidationMarkerDisplayed(),
				"Произошла ошибка:\n поле 'Название' не отмечено ошибкой");
		}

		[TestCase("")]
		[TestCase(" ")]
		public void CreateProjectEmptyNameTest(string projectName)
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage.FillGeneralProjectInformation(projectName);

			Assert.IsTrue(_newProjectSettingsPage.IsEmptyNameErrorMessageDisplayed(),
				"Произошла ошибка:\n не появилось сообщение о пустом имени проекта");

			Assert.IsTrue(_newProjectSettingsPage.IsNameInputValidationMarkerDisplayed(),
				"Произошла ошибка:\n поле 'Название' не отмечено ошибкой");
		}

		[Test]
		public void CreateProjectEqualLanguagesTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage.FillGeneralProjectInformation(
				_projectUniqueName,
				sourceLanguage: Language.English,
				targetLanguages: new[] { Language.English });

			Assert.True(_newProjectSettingsPage.IsDuplicateLanguageErrorMessageDisplayed(),
				"Произошла ошибка:\n не отображается сообщение о том, что source и target языки совпадают");
		}

		[Test]
		public void ImportDuplicateDocumentTest()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { document });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();

			_addFilesStep.UploadDublicateDocument(new[] { document, document });

			Assert.IsTrue(_dublicateFileErrorDialog.IsDublicateFileErrorDialogOpened(),
				"Произошла ошибка: Не появилось сообщение о том, что файл уже загружен.");
		}
	}
}
