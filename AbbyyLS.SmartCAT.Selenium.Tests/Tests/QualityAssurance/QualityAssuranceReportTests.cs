using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.QualityAssurance
{
	[Parallelizable(ParallelScope.Fixtures)]
	class QualityAssuranceReportTests<TWebDriverProvider> : QualityAssuranceBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_newProjectSettingsPage.ExpandAdvancedSettings();

			_advancedSettingsSection.ClickQualityAssuranceTab();

			_qualityAssuranceAdvancedSettingsSection
				.SetErrorTypeForAllErrors()
				.SetErrorType(_error1, ErrorType.setCritical);

			_newProjectSettingsPage.ClickNextButton();
			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false)
				.ClickSaveButton();

			_projectsPage.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField("first         .")
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToProjectsPage();
			_projectsPage.ClickQACheckButton(_projectUniqueName);
		}

		[Test, Description("S-7060"), Ignore("PRX-16345"), ShortCheckList]
		public void BuildQAReportTest()
		{
			// TODO: допилить тест после фикса PRX-16345
			_qualityAssuranceDialog.ClickCheckerrorsButton();
		}

		[Test, Description("S-7149"), Ignore("PRX-16345"), ShortCheckList]
		public void DownloadQAReportTest()
		{
			// TODO: допилить тест после фикса PRX-16345
			_qualityAssuranceDialog
				.ClickCheckerrorsButton()
				.ClickDownloadReportButton();
		}
	}
}
