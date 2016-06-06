using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Admin.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[CreateProject]
	class InitialProjectPersonalAccountTests<TWebDriverProvider> : InitialProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public InitialProjectPersonalAccountTests()
		{
			StartPage = StartPage.SignIn;
		}

		[Test]
		public void CreateFirstProjectAdditionalUsersPersonalAccount()
		{
			foreach (var user in ConfigurationManager.AdditionalUsersList.ToList())
			{
				_loginHelper.LogInSmartCat(
					user.Login,
					user.NickName,
					user.Password,
					"Personal");

				var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

				_newProjectDocumentUploadPage
					.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
					.ClickSettingsButton();

				_newProjectSettingsPage.FillGeneralProjectInformation(
					projectUniqueName,
					sourceLanguage: Language.English,
					targetLanguages: new[] { Language.Russian });

				_newProjectWorkflowPage.ClickCreateProjectButton();

				Assert.IsTrue(_projectsPage.IsProjectExist(projectUniqueName),
					"Произошла ошибка: \nне найден проект с новым именем '{0}'.", projectUniqueName);

				_workspacePage.SignOut();
			}
		}
	}
}
