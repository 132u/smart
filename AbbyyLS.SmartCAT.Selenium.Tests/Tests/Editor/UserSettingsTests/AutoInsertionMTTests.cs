using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.UserSettingsTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class AutoInsertionMTTests<TWebDriverProvider> : UserSettingsBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_document = PathProvider.EditorTxtFile;
			_documentName = Path.GetFileNameWithoutExtension(_document);

			_createProjectHelper.CreateNewProject(
					_projectUniqueName,
					new[] { _document },
					useMachineTranslation: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(
					_projectUniqueName,
					_documentName);

			_editorPage.ClickUserPreferencesButton();

			_userPreferencesDialog
				.UncheckAllCheckboxes()
				.ClickCopySourceCheckBox()
				.ClickInsertTranslationCheckBox()
				.SetMatchesInPercents(TMMatchesPercents.Hundred)
				.SwitchToSegmentConfirmationTab()
				.ClickGoToTheNextSegment()
				.ClickSaveButton();

			_editorPage.ClickUserPreferencesButton();
		}

		[Test, Description("S-28973"), ShortCheckList]
		public void AutoInsertionMtTest()
		{
			var secondSentense = "Вторая фраза.";
			var rowNumber = 2;

			_userPreferencesDialog
				.UncheckAllCheckboxes()
				.ClickInsertMtCheckBox()
				.ClickSaveButton();

			_editorPage.ClickOnTargetCellInSegment(rowNumber);

			var revisionType = _editorPage.GetRevisionType();

			Assert.AreEqual(secondSentense, _editorPage.GetTargetText(rowNumber),
				"Произошла ошибка:\n Не произошла автоподстановка машинного перевода в сегмент - {0}.", rowNumber);

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber),
				"Произошла ошибка:\n Произошло подтверждение сегмента - {0}.", rowNumber);

			Assert.AreEqual(_MTDescription, revisionType,
				"Произошла ошибка:\n Подставленная ревизия не совпадает с ожидаемой - {0}", revisionType);
		}

		[Test, Description("S-28974"), ShortCheckList]
		public void CopySourceToTargetTest()
		{
			var secondSourse = "Second phrase.";
			var rowNumber = 2;

			_userPreferencesDialog
				.UncheckAllCheckboxes()
				.ClickCopySourceToTargetCheckBox()
				.ClickSaveButton();

			_editorPage.ClickOnTargetCellInSegment(rowNumber);

			var revisiontype = _editorPage.GetRevisionType();

			Assert.AreEqual(secondSourse, _editorPage.GetTargetText(rowNumber),
				"Произошла ошибка:\n Не произошла автоподстановка сорс сегмента в таргет сегмента - {0}.", rowNumber);

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(rowNumber),
				"Произошла ошибка:\n Произошло подтверждение сегмента - {0}.", rowNumber);

			Assert.AreEqual(_sourceInsertionDescription, revisiontype,
				"Произошла ошибка:\n Подставленная ревизия не совпадает с ожидаемой - {0}", revisiontype);
		}
	}
}