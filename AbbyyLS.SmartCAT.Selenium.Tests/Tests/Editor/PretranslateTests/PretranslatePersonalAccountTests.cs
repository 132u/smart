using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.PretranslateTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class PretranslatePersonalAccountTests<TWebDriverProvider> :
		PretranslateBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public PretranslatePersonalAccountTests()
		{
			StartPage = StartPage.PersonalAccount;
		}

		[Test, Description("S-29310")]
		public void TMPretanslateWithoutConfirmationTest()
		{
			_createProjectHelper.CreateNewProject(
				projectName: _projectUniqueName,
				filesPaths: new[] { _file },
				tmxFilesPaths: new[] { PathProvider.PretranslateEarthTmxFile1 },
				personalAccount: true,
				rules: new List<KeyValuePair<PreTranslateRulles, WorkflowTask?>>
					{ new KeyValuePair<PreTranslateRulles, WorkflowTask?> (PreTranslateRulles.TM, null )});

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

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 1),
				"Произошла ошибка: неверный тип подстановки в сегменте №1.");

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 2),
				"Произошла ошибка: неверный тип подстановки в сегменте №2.");

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 3),
				"Произошла ошибка: неверный тип подстановки в сегменте №3.");

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 4),
				"Произошла ошибка: неверный тип подстановки в сегменте №4.");

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

			_editorPage.ClickRevisionTab();

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №1.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 2);

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №2.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 3);

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №3.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 4);

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №4.");
		}

		[Test, Description("S-29311")]
		public void TMPretanslateWithConfirmationTest()
		{
			_createProjectHelper.CreateNewProject(
				projectName: _projectUniqueName,
				filesPaths: new[] { _file },
				tmxFilesPaths: new[] { PathProvider.PretranslateEarthTmxFile1 },
				personalAccount: true,
				rules: new List<KeyValuePair<PreTranslateRulles, WorkflowTask?>>
					{ new KeyValuePair<PreTranslateRulles, WorkflowTask?> (PreTranslateRulles.TM, WorkflowTask.Translation )});

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _file);

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 1),
				"Произошла ошибка: сегмент №1 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 2),
				"Произошла ошибка: сегмент №2 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 3),
				"Произошла ошибка: сегмент №3 не подтвержден.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(rowNumber: 4),
				"Произошла ошибка: сегмент №4 не подтвержден.");

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 1),
				"Произошла ошибка: неверный тип подстановки в сегменте №1.");

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 2),
				"Произошла ошибка: неверный тип подстановки в сегменте №2.");

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 3),
				"Произошла ошибка: неверный тип подстановки в сегменте №3.");

			Assert.AreEqual(CatType.TM.ToString(), _editorPage.GetInsertResource(segmentNumber: 4),
				"Произошла ошибка: неверный тип подстановки в сегменте №4.");

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

			_editorPage.ClickRevisionTab();

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №1.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 2);

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №2.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 3);

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №3.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 4);

			Assert.AreEqual(RevisionType.InsertTM.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №4.");
		}
	}
}
