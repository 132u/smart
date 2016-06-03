using System;
using System.IO;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.UserSettingsTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	internal class UserSettingsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public UserSettingsTests()
		{
			StartPage = StartPage.SignIn;
		}

		[SetUp]
		public void SetUp()
		{
			_projectUniqueName = "ProjectName" + Guid.NewGuid();
			_signInPage = new SignInPage(Driver);
			_selectAccountForm = new SelectAccountForm(Driver);
			_editorPage = new EditorPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_userPreferencesDialog = new UserPreferencesDialog(Driver);

			var document = PathProvider.EditorAutoSubstitutionFile;

			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_signInPage.SubmitForm(AdditionalUser.Login, AdditionalUser.Password);

			_selectAccountForm.SelectAccount();

			_createProjectHelper
				.CreateNewProject(
					_projectUniqueName,
					new[] { document });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document);

			_editorPage.ClickUserPreferencesButton();
		}

		[Test, Description("S-7215"), ShortCheckList]
		public void AutoSubstitutionSourceNumbersToTargetTest()
		{
			var segmentFromFile = "543 21";

			_userPreferencesDialog
				.CheckConfirmSegmentCheckbox()
				.ClickSaveButton();

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 2);

			Assert.AreEqual(segmentFromFile, _editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка:\n Не заполнился таргет сегмента или заполнился неверно. {0}",
				_editorPage.GetTargetText(rowNumber: 2));

			Assert.AreEqual(RevisionType.SourceInsertion.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка:\n Указан неверный тип созданной ревизии. {0}",
				_editorPage.GetRevisionType());
		}

		[Test, Description("S-7216"), ShortCheckList]
		public void GoToTheNextSegmentTest()
		{
			_userPreferencesDialog
				.SwitchToSegmentConfirmationTab()
				.ClickGoToTheNextSegment()
				.ClickSaveButton();

			_editorPage
				.ClickOnTargetCellInSegment(rowNumber: 2)
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 3),
				"Произошла ошибка:\n Не произошло переключения на следующий сегмент.");
		}

		[Test, Description("S-29234"), ShortCheckList]
		public void GoToTheNextUnconfirmedSegmentTest()
		{
			var phrase1 = Guid.NewGuid().ToString();

			_userPreferencesDialog
				.SwitchToSegmentConfirmationTab()
				.ClickGoToTheUnconfirmedSegment()
				.ClickSaveButton();

			_editorPage
				.ClickOnTargetCellInSegment(rowNumber: 2)
				.ConfirmSegmentTranslation()
				.FillTarget(phrase1, rowNumber: 1)
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 3),
				"Произошла ошибка:\n Не произошло переключения на следующий неподтвержденный сегмент.");
		}

		[TearDown]
		public void TearDown()
		{
			try
			{
				_editorPage.ClickUserPreferencesButton();
				_userPreferencesDialog
					.SwitchToSegmentConfirmationTab()
					.ClickGoToTheNextSegment();
			}
			catch (Exception)
			{
				throw new Exception("Произошла ошибка при выполнении TearDown к тестам UserSettingsTests.");
			}
		}

		private string _projectUniqueName;
		private EditorPage _editorPage;
		private ProjectsPage _projectsPage;
		private CreateProjectHelper _createProjectHelper;
		private UserPreferencesDialog _userPreferencesDialog;
		private SignInPage _signInPage;
		private SelectAccountForm _selectAccountForm;
	}
}