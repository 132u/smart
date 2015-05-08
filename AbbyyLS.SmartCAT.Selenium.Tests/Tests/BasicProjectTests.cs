﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[PriorityMajor]
	[TestFixture]
	class BasicProjectTests<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			QuitDriverAfterTest = true;
			WorkspaceHelper.GoToProjectsPage();
			_createProjectHelper = new CreateProjectHelper();
		}

		[Test]
		public void CreateProjectNoFileTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName);
		}

		[Test]
		public void DeleteProjectNoFileTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.SelectProjectInList(projectUniqueName)
				.DeleteProjectFromList()
				.AssertProjectSuccessfullyDeleted(projectUniqueName);
		}

		[Test]
		public void DeleteProjectWithFileTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName, PathProvider.DocumentFile)
				.CheckProjectAppearInList(projectUniqueName)
				.OpenProjectInfo(projectUniqueName)
				.SelectProjectInList(projectUniqueName)
				.DeleteProjectWithFileFromList()
				.AssertProjectSuccessfullyDeleted(projectUniqueName);
		}

		[Test]
		public void CreateProjectDeletedNameTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.SelectProjectInList(projectUniqueName)
				.DeleteProjectFromList()
				.AssertProjectSuccessfullyDeleted(projectUniqueName)
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName)
				.ClickNextOnGeneralProjectInformationPage();
		}

		[Test]
		public void CreateProjectDuplicateNameTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName)
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName)
				.CheckInputsOnGeneralProjectInformationPage()
				.AssertErrorDuplicateName();
		}

		[Test]
		public void CreateProjectLongNameTest()
		{
			var longProjectUniqueName = _createProjectHelper.GetProjectUniqueName() + _longName;
			_createProjectHelper
				.CreateNewProject(longProjectUniqueName)
				.CheckProjectAppearInList(longProjectUniqueName.Substring(0, 100));
		}

		[Test]
		public void CreateProjectEqualLanguagesTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueName, 
					sourceLanguage: Language.English, targetLanguage: Language.English)
				.CheckInputsOnGeneralProjectInformationPage()
				.AssertErrorDuplicateLanguage();
		}

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
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectUniqueNameForbidden)
				.CheckInputsOnGeneralProjectInformationPage()
				.AssertErrorForbiddenSymbols();
		}

		[TestCase("")]
		[TestCase(" ")]
		public void CreateProjectEmptyNameTest(string projectName)
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectName)
				.CheckInputsOnGeneralProjectInformationPage()
				.AssertErrorNoName();
		}

		[Test]
		public void CreateProjectTwoWordsNameTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName() + " " + "SpacePlusSymbols";
			_createProjectHelper
				.CreateNewProject(projectUniqueName)
				.CheckProjectAppearInList(projectUniqueName);
		}

		[Test]
		public void DeleteDocumentFromProject()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_createProjectHelper
				.CreateNewProject(projectUniqueName)
				.GoToProjectSettingsPage(projectUniqueName)
				.UploadDocument(PathProvider.DocumentFile)
				.DeleteDocument(PathProvider.DocumentFile);
		}

		[Test]
		public void CancelCreateProjectOnFirstStepTest()
		{
			_createProjectHelper
				.OpenCreateProjectDialog()
				.CancelCreateProject();
		}

		[Test]
		public void DeleteFileFromWizard()
		{
			_createProjectHelper
				.OpenCreateProjectDialog()
				.AddFileFromWizard(PathProvider.DocumentFile)
				.DeleteFileFromWizard(PathProvider.DocumentFile);
		}

		private CreateProjectHelper _createProjectHelper;
		private const string _longName = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
	}
}