using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.UserSettingsTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class AutoInsertionSymbolsTests<TWebDriverProvider> : UserSettingsBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_segmentFromFile1 = "543 21";
			_segmentFromFile2 = "5 $";

			_document = PathProvider.EditorAutoInsertionFile;
			_documentName = Path.GetFileNameWithoutExtension(_document);

			_createProjectHelper
				.CreateNewProject(
					_projectUniqueName,
					new[] {_document});

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
		}

		[Test, Description("S-7215"), ShortCheckList]
		public void AutoInsertionSourceNumbersToTargetTest()
		{
			var secondRow = 2;

			_editorPage.ClickOnTargetCellInSegment(rowNumber: secondRow);

			var targetText = _editorPage.GetTargetText(rowNumber: secondRow);
			var revisionType = _editorPage.GetRevisionType();

			Assert.AreEqual(_segmentFromFile1, targetText,
				"Произошла ошибка:\n Не заполнился таргет сегмента или заполнился неверно - {0}.", targetText);

			Assert.AreEqual(_sourceInsertionDescription, revisionType,
				"Произошла ошибка:\n Указан неверный тип созданной ревизии - {0}.", revisionType);
		}

		[Test, Description("S-7216"), ShortCheckList]
		public void GoToTheNextSegmentTest()
		{
			var secondRow = 2;
			var thirdRow = 3;

			_editorPage
				.ClickOnTargetCellInSegment(rowNumber: secondRow)
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: thirdRow),
				"Произошла ошибка:\n Не произошло переключения на следующий сегмент - {0}.", thirdRow);
		}

		[Test, Description("S-29234"), ShortCheckList]
		public void GoToTheNextUnconfirmedSegmentTest()
		{
			var phrase1 = Guid.NewGuid().ToString();
			var firstRow = 1;
			var secondRow = 2;
			var thirdRow = 3;

			_editorPage.ClickUserPreferencesButton();

			_userPreferencesDialog
				.SwitchToSegmentConfirmationTab()
				.ClickGoToTheUnconfirmedSegment()
				.ClickSaveButton();

			_editorPage
				.ClickOnTargetCellInSegment(rowNumber: secondRow)
				.ConfirmSegmentTranslation()
				.FillTarget(phrase1, rowNumber: firstRow)
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: thirdRow),
				"Произошла ошибка:\n Не произошло переключения на следующий неподтвержденный сегмент - {0}.",
				thirdRow);
		}

		[Test, Description("S-29101"), ShortCheckList]
		public void AutoInsertionSymbolsWithoutSegmentConfirmationTest()
		{
			var secondRow = 2;
			var thirdRow = 3;

			_editorPage
				.ClickOnTargetCellInSegment(secondRow)
				.ClickOnTargetCellInSegment(thirdRow)
				.ClickRevisionTab();

			var revisionType = _editorPage.GetRevisionType();

			Assert.AreEqual(_segmentFromFile1, _editorPage.GetTargetText(secondRow),
				"Произошла ошибка:\n В таргет сегменте номер - {0} , не появились небуквенные символы.", secondRow);

			Assert.AreEqual(_segmentFromFile2, _editorPage.GetTargetText(thirdRow),
				"Произошла ошибка:\n В таргет сегменте номер - {0}, не появились небуквенные символы.", thirdRow);

			Assert.AreEqual(_sourceInsertionDescription, revisionType,
				"Произошла ошибка:\n Отобразился неправильный тип созданной ревизии - {0}.", revisionType);
		}

		[Test, Description("S-28970"), ShortCheckList]
		public void AutoInsertionNumbersWithSegmentConfirmationTest()
		{
			var secondRow = 2;

			_editorPage.ClickUserPreferencesButton();

			_userPreferencesDialog
				.ClickConfirmSegmentCheckBox()
				.ClickSaveButton();

			_editorPage
				.ClickOnTargetCellInSegment(secondRow)
				.ClickRevisionTab();

			var revisionType = _editorPage.GetRevisionType();
			var targetText = _editorPage.GetTargetText(secondRow);

			Assert.AreEqual(_segmentFromFile1, targetText,
				"Произошла ошибка:\n В таргет сегменте номер - {0}, не появились небуквенные символы - {1}.",
				secondRow, targetText);

			Assert.AreEqual(_sourceInsertionDescription, revisionType,
				"Произошла ошибка:\n Отобразился неправильный тип созданной ревизии - {0}.", revisionType);

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(secondRow), 
				"Произошла ошибка:\n Не произошло автоподтверждение сегмента после подставноки символов.");
		}

		[Test, Description("S-29102"), ShortCheckList]
		public void AutoInsertionSymbolsWithSegmentConfirmationTest()
		{
			var thirdRow = 3;

			_editorPage.ClickUserPreferencesButton();

			_userPreferencesDialog
				.ClickConfirmSegmentCheckBox()
				.ClickSaveButton();

			_editorPage
				.ClickOnTargetCellInSegment(thirdRow)
				.ClickRevisionTab();

			var revisionType = _editorPage.GetRevisionType();

			Assert.AreEqual(_segmentFromFile2, _editorPage.GetTargetText(thirdRow),
				"Произошла ошибка:\n В таргет сегменте номер - {0}, не появились небуквенные символы.", thirdRow);

			Assert.AreEqual(_sourceInsertionDescription, revisionType,
				"Произошла ошибка:\n Отобразился неправильный тип созданной ревизии.");

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(thirdRow),
				"Произошла ошибка:\n Не произошло автоподтверждение сегмента после подставноки символов.");
		}

		private string _segmentFromFile1;
		private string _segmentFromFile2;
	}
}