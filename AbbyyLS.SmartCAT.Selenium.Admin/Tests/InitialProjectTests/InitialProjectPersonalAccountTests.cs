using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Admin.Tests.InitialProjectTests;
using AbbyyLS.SmartCAT.Selenium.Tests;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests;

namespace AbbyyLS.SmartCAT.Selenium.Admin.Tests
{
	class InitialProjectPersonalAccountTests<TWebDriverProvider> : InitialProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public InitialProjectPersonalAccountTests()
		{
			GlobalSetup.SetUp();
		}

		[SetUp]
		public  void SetUp()
		{
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);
		}

		[Test, Category("Project tests")]
		public void CreateFirstProjectAdditionalUsersPersonalAccount()
		{
			foreach (var user in ConfigurationManager.AdditionalUsersList)
			{
				_loginHelper.Authorize(StartPage.PersonalAccount, user);
				var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

				Assert.IsTrue(_newProjectDocumentUploadPage.IsNewProjectDocumentUploadPageOpened(),
					"Произошла ошибка:\nНе открылась первая страница визарда создания проекта.");

				_newProjectDocumentUploadPage.UploadDocumentFile(PathProvider.DocumentFile)
					.ClickSettingsButton();

				_newProjectSettingsPage.FillGeneralProjectInformation(
					projectUniqueName,
					sourceLanguage: Language.English,
					targetLanguage: Language.Russian);

				_newProjectWorkflowPage.ClickCreateProjectButton();

				Assert.IsTrue(_projectsPage.IsProjectExist(projectUniqueName),
					"Произошла ошибка: \nне найден проект с новым именем");
			}
		}
	}
}
