using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Admin.Tests.InitialProjectTests;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	class InitialProjectCorporateAccountTests<TWebDriverProvider> : InitialProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public InitialProjectCorporateAccountTests()
		{
			GlobalSetup.SetUp();
			StartPage = StartPage.Workspace;
		}

		[Test, Category("Project tests")]
		public void CreateFirstProjectCorporateAccount()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsNewProjectDocumentUploadPageOpened(),
				"Произошла ошибка:\nНе открылась первая страница визарда создания проекта.");

			_newProjectDocumentUploadPage
				.UploadDocumentFile(PathProvider.DocumentFile)
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(
					projectUniqueName,
					sourceLanguage: Language.English,
					targetLanguage: Language.Russian)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			Assert.IsTrue(_projectsPage.IsProjectExist(projectUniqueName),
				"Произошла ошибка: \nне найден проект с новым именем");
		}
	}
}
