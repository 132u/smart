﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

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
				.UploadDocumentFiles(new[] { PathProvider.EditorTxtFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(_projectUniqueName)
				.ExpandAdvancedSettings();

			_advancedSettingsSection.ClickQualityAssuranceTab();
			_qualityAssuranceAdvancedSettingsSection.SetErrorTypeForAllErrors();
		}

		[Test, Description("S-7059")]
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

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false)
				.ClickSaveButton();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField("first         .")
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsMessageWithCrititcalErrorDisplayed(),
				"Произошла ошибка: не появилось сообщение о том, что перевод содержит критическую ошибку.");

			_editorPage
				.CloseCriticalErrorMessageIfExist()
				.HoverYellowTriangle();

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(),
				"Произошла ошибка: семент подтвержден.");

			Assert.IsTrue(_editorPage.IsErrorsPopupContainsCorrectError(_error1),
				"Произошла ошибка: попап не содержит ошибку {0}.", _error1);

			Assert.IsTrue(_editorPage.IsCriticalError(_error1),
				"Произошла ошибка: ошибка {0} не отмечена, как критическая.", _error1);

			_editorPage
				.FillSegmentTargetField("первое первое", rowNumber: 2)
				.ConfirmSegmentTranslation()
				.HoverYellowTriangle(segmentNumber: 2);

			Assert.IsFalse(_editorPage.IsMessageWithCrititcalErrorDisplayed(),
				"Произошла ошибка: не появилось сообщение о том, что перевод содержит критическую ошибку.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 2),
				"Произошла ошибка: семент не подтвержден.");
			
			Assert.IsTrue(_editorPage.IsErrorsPopupContainsCorrectError(_error2),
				"Произошла ошибка: попап не содержит ошибку {0}.", _error2 );

			Assert.IsFalse(_editorPage.IsCriticalError(_error2),
				"Произошла ошибка: ошибка {0} отмечена, как критическая.", _error2 );
		}

		[Test, Description("S-7145")]
		public void NoReportedErrorsTest()
		{
			_qualityAssuranceAdvancedSettingsSection.SetErrorType(_error1, ErrorType.setCritical);

			_newProjectSettingsPage.ClickNextButton();
			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false)
				.ClickSaveButton();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField("first         .")
				.ConfirmSegmentTranslation()
				.ClickQACheckTab();
			
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

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			_editorPage
				.ClickOnTargetCellInSegment()
				.ConfirmSegmentTranslation()
				.ClickQACheckTab();

			Assert.IsTrue(_editorPage.IsQAErrorTableDissapeared(),
				"Произошла ошибка: таблица с ошибками отображается.");
		}
		
		[Test, Description("S-7146")]
		public void CriticalErrorsTest()
		{
			_newProjectSettingsPage.ClickNextButton();
			_newProjectWorkflowPage.ClickCreateProjectButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false)
				.ClickSaveButton();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(PathProvider.EditorTxtFile);

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

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask();

			_editorPage
				.ClickOnTargetCellInSegment()
				.ConfirmSegmentTranslation()
				.ClickQACheckTab();
			
			Assert.IsTrue(_editorPage.IsMessageWithCrititcalErrorDisplayed(),
				"Произошла ошибка: не появилось сообщение о том, что перевод содержит критическую ошибку.");

			Assert.AreEqual(_error1, _editorPage.GetErrorTextFromQaCheckTab(),
				"Произошла ошибка: неверная ошибка в вкладке 'QA Check'.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(),
				"Произошла ошибка: семент подтвержден.");

			_editorPage
				.CloseCriticalErrorMessageIfExist()
				.HoverYellowTriangle();
			
			Assert.IsTrue(_editorPage.IsErrorsPopupContainsCorrectError(_error1),
				"Произошла ошибка: попап не содержит ошибку {0}.", _error1);

			Assert.IsTrue(_editorPage.IsCriticalError(_error1),
				"Произошла ошибка: ошибка {0} не отмечена, как критическая.", _error1);
		}
	}
}