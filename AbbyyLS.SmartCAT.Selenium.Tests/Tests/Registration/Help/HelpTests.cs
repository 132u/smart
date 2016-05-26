using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Registration]
	class HelpTests<TWebDriverProvider> :
		RegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public HelpTests()
		{
			StartPage = StartPage.Admin;
		}
		
		[SetUp]
		public void SetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_editorPage = new EditorPage(Driver, needCloseTutorial: false);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test, Description("S-7132"), Ignore("PRX-16834"), ShortCheckList]
		public void NewUserInNewCorporateAccountInWorkspaceHelpTest()
		{
			_adminHelper
				.CreateNewUser(_email, _firstAndLastName, _password)
				.CreateAccountIfNotExist(accountName: _companyName, workflow: true)
				.AddUserToAdminGroupInAccountIfNotAdded(_email, _firstName, _lastName, _companyName);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);
			_signInPage.SubmitFormExpectingNewProjectDocumentUploadPage(_email, _password);

			Assert.IsTrue(_newProjectDocumentUploadPage.IsSelectDocumentButtonAnimationExist(),
				"Произошла ошибка:\n анимация на странице выбора документа не работает.");

			_newProjectDocumentUploadPage.UploadDocumentFiles(new []{ PathProvider.DocumentFile });

			Assert.IsTrue(_newProjectDocumentUploadPage.IsSettingsButtonAnimationExist(),
				"Произошла ошибка:\n анимация кнопки перехода к настройкам проекта не работает.");

			_newProjectDocumentUploadPage.ClickSettingsButton();

			Assert.IsTrue(_newProjectSettingsPage.IsSourceLanguageFieldNameAnimationExist(),
				"Произошла ошибка:\n анимация для названия поля исходного языка не работает.");

			Assert.IsTrue(_newProjectSettingsPage.IsTargetLanguageFieldNameAnimationExist(),
				"Произошла ошибка:\n анимация для названия поля целевого языка не работает.");

			_newProjectSettingsPage.FillGeneralProjectInformation(_projectUniqueName);

			Assert.IsTrue(_newProjectSettingsPage.IsSourceLanguageFieldNameAnimationNotExist(),
				"Произошла ошибка:\n анимация для названия поля исходного языка должна отключиться после заполнения этого поля.");

			Assert.IsTrue(_newProjectSettingsPage.IsTargetLanguageFieldNameAnimationNotExist(),
				"Произошла ошибка:\n анимация для названия поля целевого языка должна отключиться после заполнения этого поля.");

			Assert.IsTrue(_newProjectSettingsPage.IsNextButtonAnimationExist(),
				"Произошла ошибка:\n анимация для для кнопки 'Next' не работает.");

			_newProjectSettingsPage.ClickNextButton();

			Assert.IsTrue(_newProjectWorkflowPage.IsFinishButtonAnimationExist(),
				"Произошла ошибка:\n анимация для для кнопки 'Finish' не работает.");

			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsHelpDocumentTranslationPopupExist(),
				"Произошла ошибка:\n подсказка о том, как начать перевод документа не показывается.");
		}

		[Test, Description("S-13743"), Ignore("PRX-16834"), ShortCheckList]
		public void NewUserInNewCorporateAccountInEditorHelpTest()
		{
			_adminHelper
				.CreateNewUser(_email, _firstAndLastName, _password)
				.CreateAccountIfNotExist(accountName: _companyName, workflow: true)
				.AddUserToAdminGroupInAccountIfNotAdded(_email, _firstName, _lastName, _companyName);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);
			_signInPage.SubmitFormExpectingNewProjectDocumentUploadPage(_email, _password);

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, PathProvider.DocumentFile, needCloseTutorial: false);

			Assert.IsTrue(_editorPage.IsSourceColumnHelpDisplayed(),
				"Произошла ошибка:\n подсказка к ячейке исходного текста не показывается.");

			_editorPage.ClickNextButtonOnSourceFieldHelp();

			Assert.IsTrue(_editorPage.IsTargetColumnHelpDisplayed(),
				"Произошла ошибка:\n подсказка к ячейке исходного текста не показывается.");

			_editorPage.ClickNextButtonOnTargetFieldHelp();

			Assert.IsTrue(_editorPage.IsCatPanelHelpDisplayed(),
				"Произошла ошибка:\n подсказка к Cat-панели не показывается.");

			_editorPage.ClickNextButtonOnCatPanelHelp();

			Assert.IsTrue(_editorPage.IsConfirmHelpDisplayed(),
				"Произошла ошибка:\n подсказка к Cat-панели не показывается.");

			_editorPage.ClickNextButtonOnConfirmHelp();

			Assert.IsTrue(_editorPage.IsButtonBarHelpDisplayed(),
				"Произошла ошибка:\n подсказка к Cat-панели не показывается.");

			_editorPage.ClickNextButtonOnButtonBarHelp();

			Assert.IsTrue(_editorPage.IsFeedbackHelpDisplayed(),
				"Произошла ошибка:\n подсказка к Cat-панели не показывается.");

			_editorPage.ClickFinishButtonOnFeedbackHelp();

			Assert.IsTrue(_editorPage.IsFeedbackHelpDisappeared(),
				"Произошла ошибка:\n подсказка к Cat-панели не закрылась.");
		}

		[Test, Description("S-7131"), ShortCheckList]
		public void NewFreelancerInWorkspaceHelpTest()
		{
			_adminHelper.CreateNewUser(_email, _firstAndLastName, _password);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);

			_signInPage.SubmitForm(_email, _password);

			_selectAccountForm.ClickCreateAccountButton();

			_registrationPage
				.ClickFreelancerForm()
				.ClickConfirmButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsSelectDocumentButtonAnimationExist(),
				"Произошла ошибка:\n анимация на странице выбора документа не работает.");

			_newProjectDocumentUploadPage.UploadDocumentFiles(new[] { PathProvider.DocumentFile });

			Assert.IsTrue(_newProjectDocumentUploadPage.IsSettingsButtonAnimationExist(),
				"Произошла ошибка:\n анимация кнопки перехода к настройкам проекта не работает.");

			_newProjectDocumentUploadPage.ClickSettingsButton();

			Assert.IsTrue(_newProjectSettingsPage.IsSourceLanguageFieldNameAnimationExist(),
				"Произошла ошибка:\n анимация для названия поля исходного языка не работает.");

			Assert.IsTrue(_newProjectSettingsPage.IsTargetLanguageFieldNameAnimationExist(),
				"Произошла ошибка:\n анимация для названия поля целевого языка не работает.");

			_newProjectSettingsPage.FillGeneralProjectInformation(_projectUniqueName);

			Assert.IsTrue(_newProjectSettingsPage.IsSourceLanguageFieldNameAnimationNotExist(),
				"Произошла ошибка:\n анимация для названия поля исходного языка должна отключиться после заполнения этого поля.");

			Assert.IsTrue(_newProjectSettingsPage.IsTargetLanguageFieldNameAnimationNotExist(),
				"Произошла ошибка:\n анимация для названия поля целевого языка должна отключиться после заполнения этого поля.");

			Assert.IsTrue(_newProjectSettingsPage.IsCreateProjectButtonAnimationExist(),
				"Произошла ошибка:\n анимация для кнопки создания проекта не работает.");

			_newProjectSettingsPage.ClickCreateProjectButton();

			_projectsPage.WaitUntilProjectLoadSuccessfully(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsHelpDocumentTranslationPopupExist(),
				"Произошла ошибка:\n подсказка о том, как начать перевод документа не показывается.");
		}

		[Test, Description("S-13742"), ShortCheckList]
		public void NewFreelancerInEditorHelpTest()
		{
			_adminHelper.CreateNewUser(_email, _firstAndLastName, _password);

			_registrationPage.GetPageExpectingRedirectToSignInPage(_email);

			_signInPage.SubmitForm(_email, _password);

			_selectAccountForm.ClickCreateAccountButton();

			_registrationPage
				.ClickFreelancerForm()
				.ClickConfirmButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ClickCreateProjectButton();

			_projectsPage
				.WaitUntilProjectLoadSuccessfully(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, PathProvider.DocumentFile, needCloseTutorial: false);

			Assert.IsTrue(_editorPage.IsSourceColumnHelpDisplayed(),
				"Произошла ошибка:\n подсказка к ячейке исходного текста не показывается.");

			_editorPage.ClickNextButtonOnSourceFieldHelp();

			Assert.IsTrue(_editorPage.IsTargetColumnHelpDisplayed(),
				"Произошла ошибка:\n подсказка к ячейке исходного текста не показывается.");

			_editorPage.ClickNextButtonOnTargetFieldHelp();

			Assert.IsTrue(_editorPage.IsCatPanelHelpDisplayed(),
				"Произошла ошибка:\n подсказка к Cat-панели не показывается.");

			_editorPage.ClickNextButtonOnCatPanelHelp();

			Assert.IsTrue(_editorPage.IsConfirmHelpDisplayed(),
				"Произошла ошибка:\n подсказка к Cat-панели не показывается.");

			_editorPage.ClickNextButtonOnConfirmHelp();

			Assert.IsTrue(_editorPage.IsButtonBarHelpDisplayed(),
				"Произошла ошибка:\n подсказка к Cat-панели не показывается.");

			_editorPage.ClickNextButtonOnButtonBarHelp();

			Assert.IsTrue(_editorPage.IsFeedbackHelpDisplayed(),
				"Произошла ошибка:\n подсказка к Cat-панели не показывается.");

			_editorPage.ClickFinishButtonOnFeedbackHelp();

			Assert.IsTrue(_editorPage.IsFeedbackHelpDisappeared(),
				"Произошла ошибка:\n подсказка к Cat-панели не закрылась.");
		}

		protected string _projectUniqueName;

		protected CreateProjectHelper _createProjectHelper;
		protected ProjectsPage _projectsPage;
		protected EditorPage _editorPage;
	}
}
