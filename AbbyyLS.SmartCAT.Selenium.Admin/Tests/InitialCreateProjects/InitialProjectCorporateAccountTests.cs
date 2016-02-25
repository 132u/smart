using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateProject]
	class InitialProjectCorporateAccountTests<TWebDriverProvider> : InitialProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public InitialProjectCorporateAccountTests()
		{
			StartPage = StartPage.Workspace;
		}

		[Test]
		public void CreateFirstProjectCorporateAccount()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			if (!_newProjectDocumentUploadPage.IsNewProjectDocumentUploadPageOpened())
			{
				_projectsPage.ClickCreateProjectButton();
			}

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
