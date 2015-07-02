using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[PriorityMajor]
	[Standalone]
	[TestFixture]
	class BasicProjectTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
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
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
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
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
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
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
				.AssertErrorForbiddenSymbols();
		}

		[TestCase("")]
		[TestCase(" ")]
		public void CreateProjectEmptyNameTest(string projectName)
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.FillGeneralProjectInformation(projectName)
				.ClickNextOnGeneralProjectInformationPage(errorExpected: true)
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
				.DeleteDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		[Test]
		public void CancelCreateProjectOnFirstStepTest()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.CancelCreateProject();
		}

		[Test]
		public void DeleteFileFromWizard()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.AddFileFromWizard(PathProvider.DocumentFile)
				.DeleteFileFromWizard(PathProvider.DocumentFile);
		}

		private CreateProjectHelper _createProjectHelper;
		private const string _longName = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
	}
}