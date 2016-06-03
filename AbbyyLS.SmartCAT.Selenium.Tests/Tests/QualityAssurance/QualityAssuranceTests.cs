using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.QualityAssurance
{
	[Parallelizable(ParallelScope.Fixtures)]
	class QualityAssuranceTests<TWebDriverProvider> : QualityAssuranceBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { _document })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ExpandAdvancedSettings();

			_advancedSettingsSection.ClickQualityAssuranceTab();
			_qualityAssuranceAdvancedSettingsSection.SetErrorTypeForAllErrors();
		}

		[Test, Description("S-7059"), ShortCheckList]
		public void AddQualityAssuranceErrorsTest()
		{
			_qualityAssuranceAdvancedSettingsSection
				.SetErrorType(_error1, ErrorType.setCritical)
				.SetErrorType(_error2, ErrorType.enableError);

			_newProjectSettingsPage.ClickNextButton();
			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog.ClickQualityAssuranceSettingsButton();

			Assert.IsTrue(_qualityAssuranceSettings.IsErrorChecked(_error1),
				"Произошла ошибка: ошибка '{0}' не отмечена.", _error1);

			Assert.IsTrue(_qualityAssuranceSettings.IsErrorChecked(_error2),
				"Произошла ошибка: ошибка '{0}' не отмечена.", _error2);

			Assert.IsTrue(_qualityAssuranceSettings.IsErrorCritical(_error1),
				"Произошла ошибка: ошибка '{0}' не отмечена, как критическая.", _error1);

			Assert.IsFalse(_qualityAssuranceSettings.IsErrorCritical(_error2),
				"Произошла ошибка: ошибка '{0}' отмечена, как критическая.", _error2);

			_qualityAssuranceSettings.ClickCancelButtonWithoutChanges();

			_projectSettingsDialog.SaveSettingsExpectingProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, _document);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _document);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField("first         .")
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsMessageWithCrititcalErrorDisplayed(),
				"Произошла ошибка: не появилось сообщение о том, что перевод содержит критическую ошибку.");

			_editorPage
				.CloseCriticalErrorMessageIfExist()
				.HoverSegmentErrorLogo();

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(),
				"Произошла ошибка: семент подтвержден.");

			Assert.IsTrue(_editorPage.IsErrorsPopupContainsCorrectError(_error1),
				"Произошла ошибка: попап не содержит ошибку {0}.", _error1);

			Assert.IsTrue(_editorPage.IsCriticalError(_error1),
				"Произошла ошибка: ошибка {0} не отмечена, как критическая.", _error1);

			_editorPage
				.FillSegmentTargetField("Первое первое", rowNumber: 2)
				.ConfirmSegmentTranslation()
				.HoverSegmentErrorLogo(segmentNumber: 2);

			Assert.IsFalse(_editorPage.IsMessageWithCrititcalErrorDisplayed(),
				"Произошла ошибка: появилось сообщение о том, что перевод содержит критическую ошибку.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 2),
				"Произошла ошибка: семент не подтвержден.");
			
			Assert.IsTrue(_editorPage.IsErrorsPopupContainsCorrectError(_error2),
				"Произошла ошибка: попап не содержит ошибку {0}.", _error2 );

			Assert.IsFalse(_editorPage.IsCriticalError(_error2),
				"Произошла ошибка: ошибка {0} отмечена, как критическая.", _error2 );
		}

		[Test, Description("S-7145"), ShortCheckList]
		public void NoReportedErrorsTest()
		{
			_qualityAssuranceAdvancedSettingsSection.SetErrorType(_error1, ErrorType.setCritical);

			_newProjectSettingsPage.ClickNextButton();
			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, _document);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _document);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField("first         .")
				.ConfirmSegmentTranslation()
				.ClickQualityAssuranceCheckTab();
			
			Assert.AreEqual(_error1, _editorPage.GetErrorTextFromQaCheckTab(),
				"Произошла ошибка: неверная ошибка в вкладке 'QA Check'.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.ClickSettingsButton();

			_projectSettingsDialog.ClickQualityAssuranceSettingsButton();

			_qualityAssuranceSettings
				.UncheckErrotTitleCheckbox()
				.ClickCheckDocumentsAfterApplyCheckbox()
				.ClickApplyButton();

			_projectSettingsDialog.SaveSettings();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();

			_editorPage
				.ClickOnTargetCellInSegment()
				.ConfirmSegmentTranslation()
				.ClickQualityAssuranceCheckTab();

			Assert.IsTrue(_editorPage.IsQAErrorTableDissapeared(),
				"Произошла ошибка: таблица с ошибками отображается.");
		}
		
		[Test, Description("S-7146"), ShortCheckList]
		public void CriticalErrorsTest()
		{
			_newProjectSettingsPage.ClickNextButton();
			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, _document);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _document);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField("first         .")
				.ConfirmSegmentTranslation();

			Assert.IsFalse(_editorPage.IsMessageWithCrititcalErrorDisplayed(),
				"Произошла ошибка: не появилось сообщение о том, что перевод содержит критическую ошибку.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(),
				"Произошла ошибка: семент не подтвержден.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.ClickSettingsButton();

			_projectSettingsDialog.ClickQualityAssuranceSettingsButton();

			_qualityAssuranceSettings
				.CheckErrorCheckbox(_error1) 
				.CheckCriticalErrorCheckbox(_error1) 
				.ClickCheckDocumentsAfterApplyCheckbox()
				.ClickApplyButton();

			_projectSettingsDialog.SaveSettings();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();

			_editorPage
				.ClickOnTargetCellInSegment()
				.ConfirmSegmentTranslation()
				.ClickQualityAssuranceCheckTab();
			
			Assert.IsTrue(_editorPage.IsMessageWithCrititcalErrorDisplayed(),
				"Произошла ошибка: не появилось сообщение о том, что перевод содержит критическую ошибку.");

			Assert.AreEqual(_error1, _editorPage.GetErrorTextFromQaCheckTab(),
				"Произошла ошибка: неверная ошибка в вкладке 'QA Check'.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(),
				"Произошла ошибка: семент подтвержден.");

			_editorPage
				.CloseCriticalErrorMessageIfExist()
				.HoverSegmentErrorLogo();
			
			Assert.IsTrue(_editorPage.IsErrorsPopupContainsCorrectError(_error1),
				"Произошла ошибка: попап не содержит ошибку {0}.", _error1);

			Assert.IsTrue(_editorPage.IsCriticalError(_error1),
				"Произошла ошибка: ошибка {0} не отмечена, как критическая.", _error1);
		}

		[Test, Description("S-7231"), ShortCheckList]
		public void IdenticalSourceAndTargetErrorTest()
		{
			_qualityAssuranceAdvancedSettingsSection.SetErrorType(_error3, ErrorType.setCritical);

			_newProjectSettingsPage.ClickNextButton();
			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, _document);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _document);

			_selectTaskDialog.SelectTask();

			_editorPage
				.ClickCopySourceToTargetButton()
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsSegmentErrorLogoDisplayed(),
				"Произошла ошибка: логотип с ошибкой не появился после подтверждения таргета, содержащего ошибку.");

			_editorPage
				.CloseCriticalErrorMessageIfExist()
				.HoverSegmentErrorLogo();

			Assert.IsTrue(_editorPage.IsErrorsPopupContainsCorrectError(_error3),
				"Произошла ошибка: попап не содержит ошибку {0}.", _error3);

			Assert.IsTrue(_editorPage.IsCriticalError(_error3),
				"Произошла ошибка: ошибка {0} не отмечена, как критическая.", _error3);

			_editorPage
				.ClickOnTargetCellInSegment()
				.ClickYellowTriangle();

			Assert.IsTrue(_editorPage.IsQAErrorTableDisplayed(),
				"Произошла ошибка: не открылась вкладка 'QA Check'.");
		}

		[Test, Description("S-7233"), ShortCheckList]
		public void FixQualityAssuranceErrorTest()
		{
			_qualityAssuranceAdvancedSettingsSection.SetErrorType(_error2, ErrorType.setCritical);

			_newProjectSettingsPage.ClickNextButton();
			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, _document);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _document);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField("first first")
				.ConfirmSegmentTranslation()
				.CloseCriticalErrorMessageIfExist();
			
			Assert.IsTrue(_editorPage.IsSegmentErrorLogoDisplayed(),
				"Произошла ошибка: не появился желтый треуголник в первом сегменте.");

			_editorPage
				.FillSegmentTargetField("first")
				.ConfirmSegmentTranslation();

			Assert.True(_editorPage.IsYellowTriangleErrorInactive(),
				"Произошла ошибка: желтый треуголник в первом сегменте активен.");
		}
	}
}
