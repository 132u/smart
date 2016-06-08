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
	class MTPretranslateTests<TWebDriverProvider> :
		PretranslateBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper.CreateNewProject(
				projectName: _projectUniqueName,
				filesPaths: new[] { _file },
				useMachineTranslation: true,
				rules: new List<KeyValuePair<PreTranslateRulles, WorkflowTask?>>
					{ new KeyValuePair<PreTranslateRulles, WorkflowTask?> (PreTranslateRulles.MT, null )});

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _file);
		}

		[Test, Description("S-29306")]
		public void MTPretranslateTestWithoutConfirmationTest()
		{
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

			Assert.AreEqual(CatType.MT.ToString(), _editorPage.GetInsertResource(segmentNumber: 2),
				"Произошла ошибка: неверный тип подстановки в сегменте №2.");

			Assert.AreEqual(CatType.MT.ToString(), _editorPage.GetInsertResource(segmentNumber: 3),
				"Произошла ошибка: неверный тип подстановки в сегменте №3.");

			Assert.AreEqual(CatType.MT.ToString(), _editorPage.GetInsertResource(segmentNumber: 4),
				"Произошла ошибка: неверный тип подстановки в сегменте №4.");

			Assert.AreEqual(
				MT_FIRST_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка: неверный текст в сегменте №1.");

			Assert.AreEqual(
				MT_SECOND_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка: неверный текст в сегменте №2.");

			Assert.AreEqual(
				MT_THIRD_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 3),
				"Произошла ошибка: неверный текст в сегменте №3.");

			Assert.AreEqual(
				MT_FOURTH_TARGET_SEGMENT,
				_editorPage.GetTargetText(rowNumber: 4),
				"Произошла ошибка: неверный текст в сегменте №4.");
		}

		[Test, Description("S-29307")]
		public void MTPretranslateWithoutConfirmationRevisionTest()
		{
			_editorPage.ClickRevisionTab();

			Assert.AreEqual(RevisionType.InsertMT.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №1.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 2);

			Assert.AreEqual(RevisionType.InsertMT.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №2.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 3);

			Assert.AreEqual(RevisionType.InsertMT.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №3.");

			_editorPage.ClickOnTargetCellInSegment(rowNumber: 4);

			Assert.AreEqual(RevisionType.InsertMT.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка: неверный тип ревизии в строке №4.");
		}
	}
}
