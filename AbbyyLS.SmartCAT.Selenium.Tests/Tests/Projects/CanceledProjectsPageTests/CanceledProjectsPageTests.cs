using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Registration;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	[Standalone]
	class CanceledProjectsPageTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpCanceledProjectsPageTests()
		{
			_document = PathProvider.DocumentFile;

			_loginHelper.Authorize(StartPage.Admin, ThreadUser);

			_restoreProjectDialog = new RestoreProjectDialog(Driver);

			var selectAccountForm = new SelectAccountForm(Driver);
			var registrationPage = new RegistrationPage(Driver);
			var signInPage = new SignInPage(Driver);

			var accountName = AdminHelper.GetAccountUniqueName();

			_adminHelper
				.CreateAccountIfNotExist(accountName: accountName, workflow: true)
				.AddUserToAdminGroupInAccountIfNotAdded(ThreadUser.Login, ThreadUser.Name, ThreadUser.Surname, accountName);

			registrationPage.GetPageExpectingRedirectToSignInPage(ThreadUser.Login);

			signInPage.SubmitForm(ThreadUser.Login, ThreadUser.Password);

			selectAccountForm.SelectAccount(accountName);

			_createProjectHelper.CreateNewProject(_projectUniqueName, new[] { _document});
		}

		[Test, Description("S-7064"), ShortCheckList]
		public void CancelledProjectsTabExistsTest()
		{
			Assert.IsFalse(_projectsPage.IsCancelledProjectsTabDisplayed(),
				"Произошла ошибка:\n вкладка 'Отменённые проекты' не должна отображаться");

			_projectsPage
				.CancelProject(_projectUniqueName)
				.RefreshPage<ProjectsPage>();

			Assert.IsTrue(_projectsPage.IsCancelledProjectsTabDisplayed(),
				"Произошла ошибка:\n вкладка 'Отменённые проекты' не отображается");

			_projectsPage.GoToCancelledProjectsPage();

			Assert.IsTrue(_cancelledProjectsPage.IsProjectNameContainsInCancelledProjects(_projectUniqueName),
				"Произошла ошибка:\n Проект {0} не находится на странице отменённых проектов.", _projectUniqueName);
		}

		[Test, Description("S-7062"), ShortCheckList]
		public void RestoreCancelledProjectTest()
		{
			_projectsPage
				.CancelProject(_projectUniqueName)
				.RefreshPage<ProjectsPage>()
				.GoToCancelledProjectsPage();

			_cancelledProjectsPage.OpenRestoreProjectDialog(_projectUniqueName);

			_restoreProjectDialog.ClickRestoreButton();

			_cancelledProjectsPage.ClickProjectsTab();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _document);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка: \n Редактор не открылся");
		}

		private RestoreProjectDialog _restoreProjectDialog;
		private string _document;
	}
}
