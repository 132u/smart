using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.PretranslateTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class MixPretranslateTests<TWebDriverProvider> :
		PretranslateBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-29308"), Ignore("SCAT-1177")]
		public void PretranslateWithTwoLanguagesDocumentTest()
		{
			//TODO поменять файл
			var file = PathProvider.EditorAutoSubstitutionFile;
			_createProjectHelper.CreateNewProject(
				projectName: _projectUniqueName,
				filesPaths: new[] { file });
			
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, file);

			_editorPage.ClickRevisionTab();

			Assert.AreEqual(RevisionType.Pretranslation.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №1.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 2);

			Assert.AreEqual(RevisionType.Pretranslation.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №2.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 3);

			Assert.AreEqual(RevisionType.Pretranslation.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №3.");
		}

		[Test, Description("S-7280")]
		public void TMPretranslateWithConfirmationOnFirstWorkflowTaskTest()
		{
			_createProjectHelper.CreateNewProject(
				projectName: _projectUniqueName,
				filesPaths: new[] { _file },
				tmxFilesPaths: new[] { PathProvider.PretranslateEarthTmxFile1 },
				tasks: new List<WorkflowTask> { WorkflowTask .Translation, WorkflowTask.Editing },
				rules: new List<KeyValuePair<PreTranslateRulles, WorkflowTask?>>
					{ new KeyValuePair<PreTranslateRulles, WorkflowTask?>(PreTranslateRulles.TM, WorkflowTask.Translation )});
			
			_projectsPage.OpenAssignDialog(_projectUniqueName, _file);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.SetResponsible(ThreadUser.NickName, taskNumber: 2)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _file);

			_selectTaskDialog.SelectTask(TaskMode.Translation);

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 1),
				"Произошла ошибка: сегмент №1 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 2),
				"Произошла ошибка: сегмент №2 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 3),
				"Произошла ошибка: сегмент №3 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 4),
				"Произошла ошибка: сегмент №4 не подтвержден.");

			Assert.AreEqual(
				TM_FIRST_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка: неверный текст в сегменте №1.");

			Assert.AreEqual(
				TM_SECOND_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка: неверный текст в сегменте №2.");

			Assert.AreEqual(
				TM_THIRD_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 3),
				"Произошла ошибка: неверный текст в сегменте №3.");

			Assert.AreEqual(
				TM_FOURTH_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 4),
				"Произошла ошибка: неверный текст в сегменте №4.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_file);

			_selectTaskDialog.SelectTask(TaskMode.Editing);

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 1),
				"Произошла ошибка: сегмент №1 подтвержден.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 2),
				"Произошла ошибка: сегмент №2 подтвержден.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 3),
				"Произошла ошибка: сегмент №3 подтвержден.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 4),
				"Произошла ошибка: сегмент №4 подтвержден.");

			Assert.AreEqual(
				TM_FIRST_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка: неверный текст в сегменте №1.");

			Assert.AreEqual(
				TM_SECOND_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка: неверный текст в сегменте №2.");

			Assert.AreEqual(
				TM_THIRD_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 3),
				"Произошла ошибка: неверный текст в сегменте №3.");

			Assert.AreEqual(
				TM_FOURTH_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 4),
				"Произошла ошибка: неверный текст в сегменте №4.");
		}

		[Test, Description("S-29309")]
		public void TMPretranslateWithConfirmationOnLastWorkflowTaskTest()
		{
			_createProjectHelper.CreateNewProject(
				projectName: _projectUniqueName,
				filesPaths: new[] { _file },
				tmxFilesPaths: new[] { PathProvider.PretranslateEarthTmxFile1 },
				tasks: new List<WorkflowTask> { WorkflowTask.Translation, WorkflowTask.Editing },
				rules: new List<KeyValuePair<PreTranslateRulles, WorkflowTask?>>
					{ new KeyValuePair<PreTranslateRulles, WorkflowTask?>(PreTranslateRulles.TM, WorkflowTask.Editing )});

			_projectsPage.OpenAssignDialog(_projectUniqueName, _file);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.SetResponsible(ThreadUser.NickName, taskNumber: 2)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingSelectTaskDialog(_projectUniqueName, _file);

			_selectTaskDialog.SelectTask(TaskMode.Editing);

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 1),
				"Произошла ошибка: сегмент №1 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 2),
				"Произошла ошибка: сегмент №2 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 3),
				"Произошла ошибка: сегмент №3 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 4),
				"Произошла ошибка: сегмент №4 не подтвержден.");

			Assert.AreEqual(
				TM_FIRST_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка: неверный текст в сегменте №1.");

			Assert.AreEqual(
				TM_SECOND_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка: неверный текст в сегменте №2.");

			Assert.AreEqual(
				TM_THIRD_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 3),
				"Произошла ошибка: неверный текст в сегменте №3.");

			Assert.AreEqual(
				TM_FOURTH_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 4),
				"Произошла ошибка: неверный текст в сегменте №4.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_file);

			_selectTaskDialog.SelectTask(TaskMode.Translation);

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 1),
				"Произошла ошибка: сегмент №1 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 2),
				"Произошла ошибка: сегмент №2 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 3),
				"Произошла ошибка: сегмент №3 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 4),
				"Произошла ошибка: сегмент №4 не подтвержден.");

			Assert.AreEqual(
				TM_FIRST_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка: неверный текст в сегменте №1.");

			Assert.AreEqual(
				TM_SECOND_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка: неверный текст в сегменте №2.");

			Assert.AreEqual(
				TM_THIRD_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 3),
				"Произошла ошибка: неверный текст в сегменте №3.");

			Assert.AreEqual(
				TM_FOURTH_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 4),
				"Произошла ошибка: неверный текст в сегменте №4.");
		}

		[Test, Description("S-29312")]
		public void TMAndMTPretranslateWithoutConfirmationTest()
		{
			_createProjectHelper.CreateNewProject(
				projectName: _projectUniqueName,
				filesPaths: new[] { _file },
				tmxFilesPaths: new[] { PathProvider.PretranslateEarthTmxFile2 },
				useMachineTranslation: true,
				rules: new List<KeyValuePair<PreTranslateRulles, WorkflowTask?>>
					{
						new KeyValuePair<PreTranslateRulles, WorkflowTask?>(PreTranslateRulles.TM, null ),
						new KeyValuePair<PreTranslateRulles, WorkflowTask?>(PreTranslateRulles.MT, null )
					});

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _file);

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 1),
				"Произошла ошибка: сегмент №1 подтвержден.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 2),
				"Произошла ошибка: сегмент №2 подтвержден.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 3),
				"Произошла ошибка: сегмент №3 подтвержден.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 4),
				"Произошла ошибка: сегмент №4 подтвержден.");

			Assert.AreEqual(CatType.MT.ToString(), _editorPage.GetInsertResource(segmentNumber: 1),
			"Произошла ошибка: неверный тип подстановки в сегменте №1.");

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 2),
				"Произошла ошибка: неверный тип подстановки в сегменте №2.");

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 3),
				"Произошла ошибка: неверный тип подстановки в сегменте №3.");

			Assert.AreEqual(CatType.MT.ToString(), _editorPage.GetInsertResource(segmentNumber: 4),
				"Произошла ошибка: неверный тип подстановки в сегменте №4.");

			Assert.AreEqual(
				MT_FIRST_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка: неверный текст в сегменте №1.");

			Assert.AreEqual(
				TM_SECOND_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка: неверный текст в сегменте №2.");

			Assert.AreEqual(
				TM_THIRD_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 3),
				"Произошла ошибка: неверный текст в сегменте №3.");

			Assert.AreEqual(
				MT_FOURTH_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 4),
				"Произошла ошибка: неверный текст в сегменте №4.");
		}

		[Test, Description("S-12003"), Ignore("PRX-15109")]
		public void SourcePretranslateWithoutConfirmationTest()
		{
			var file = PathProvider.PretranslateEarthFileWithDigitals;

			_createProjectHelper.CreateNewProject(
				projectName: _projectUniqueName,
				filesPaths: new[] { file },
				rules: new List<KeyValuePair<PreTranslateRulles, WorkflowTask?>>
					{ new KeyValuePair<PreTranslateRulles, WorkflowTask?>(PreTranslateRulles.SRC, null)});
			
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, file);

			Assert.AreEqual(string.Empty, _editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка: неверный текст в сегменте №1.");

			Assert.AreEqual("34", _editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка: неверный текст в сегменте №2.");

			Assert.AreEqual(string.Empty, _editorPage.GetTargetText(rowNumber: 3),
				"Произошла ошибка: неверный текст в сегменте №3.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 1),
				"Произошла ошибка: сегмент №1 подтвержден.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 2),
				"Произошла ошибка: сегмент №2 подтвержден.");

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber: 3),
				"Произошла ошибка: сегмент №3 подтвержден.");
			
			_editorPage.ClickOnTargetCellInSegment(rowNumber: 2);

			Assert.AreEqual(RevisionType.SourceInsertion, _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии.");
		}
	}
}
