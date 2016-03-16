using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	class CreateProjectWithIncorrectDataTests<TWebDriverProvider>
		: BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
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
				targetLanguage: Language.English);

			Assert.True(_newProjectSettingsPage.IsDuplicateLanguageErrorMessageDisplayed(),
				"Произошла ошибка:\n не отображается сообщение о том, что source и target языки совпадают");
		}

		[TestCase("03/03/20166")]
		[TestCase("03 03/2016")]
		[TestCase("0303/2016")]
		[TestCase("033/03/2016")]
		[TestCase("03/033/2016")]
		[TestCase("03/03/201")]
		public void InvalidDeadlineDateFormat(string dateFormat)
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage.ClickSkipDocumentUploadButton();

			_newProjectSettingsPage.FillGeneralProjectInformation(_projectUniqueName,
				deadline: Deadline.FillDeadlineDate,
				date: dateFormat);

			Assert.IsTrue(_newProjectSettingsPage.IsErrorDeadlineDateMessageDisplayed(),
				"Произошла ошибка:\n При введении некорректной даты '{0}' не было сообщения о неверном формате даты", dateFormat);
		}

		[Test]
		public void ImportDuplicateDocumentTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, PathProvider.DocumentFile);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(
				new[] { PathProvider.DocumentFile, PathProvider.DocumentFile });

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsDuplicateDocumentNameErrorExist(),
				"Произошла ошибка:\n нет появилась ошибка о том, что в проекте уже есть файл с таким именем.");
		}
	}
}
