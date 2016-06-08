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
	class AutoInsertionFromTMTests<TWebDriverProvider> : UserSettingsBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_firstSourceString = "The Central Bank of the Russian Federation";
			_secondSourceString = "In November 1991 the Central Bank of RSFSR";

			_document = PathProvider.AutoInsertionFromTMTxtFile;
			_documentName = Path.GetFileNameWithoutExtension(_document);

			_createProjectHelper
				.CreateNewProject(
					_projectUniqueName,
					new[] { _document },
					sourceLanguage: Language.Russian,
					targetLanguages: new[] { Language.English },
					tmxFilesPaths: new []{ PathProvider.AutoInsertionFromTMTmxFile });

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

		[Test, Description("S-28971"), ShortCheckList]
		public void AutoInsertionMatchesNoLessOneHundredPercentsWithoutConfirmTest()
		{
			var percents1 = 98;
			var percents2 = 101;
			var percents3 = 102;
			var thirdSegment = 3;
			var fourSegment = 4;
			var fiveSegment = 5;

			_editorPage.ClickOnTargetCellInSegment(thirdSegment);

			Assert.AreEqual(String.Empty, _editorPage.GetTargetText(thirdSegment),
				"Произошла ошибка:\n Произошла автоподстановка в сегмента - {0}.", thirdSegment);

			Assert.AreEqual(percents1, _editorPage.CatTranslationMatchPercent(rowNumber: 1),
				"Произошла ошибка:\n Количество процентов совпадения из ТМ не - {0}%.", percents1);

			_editorPage.ClickOnTargetCellInSegment(fourSegment);

			Assert.IsTrue(_editorPage.IsTargetContainsText(fourSegment, _firstSourceString),
				"Произошла ошибка:\n Не произошла автоподстановка - {0} сегмента.", _firstSourceString);

			Assert.AreEqual(percents2, _editorPage.CatTranslationMatchPercent(rowNumber: 1),
				"Произошла ошибка:\n Количество процентов совпадения из ТМ не - {0}%.", percents2);

			Assert.AreEqual(_insertTMDescription, _editorPage.GetRevisionType(),
				"Произошла ошибка:\n Не отобразилась ревизия - {0} после подстановки из ТМ.",
				_insertTMDescription);

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(fourSegment),
				"Произошла ошибка:\n Подтвердился таргет сегмент - {0}.", fourSegment);

			_editorPage.ClickOnTargetCellInSegment(fiveSegment);

			Assert.IsTrue(_editorPage.IsTargetContainsText(fiveSegment, _secondSourceString),
				"Произошла ошибка:\n Не произошла автоподстановка сегмента - {0}.", _secondSourceString);

			Assert.AreEqual(percents3, _editorPage.CatTranslationMatchPercent(rowNumber: 1),
				"Произошла ошибка:\n Количество процентов совпадения из ТМ не - {0}%.", percents3);

			Assert.AreEqual(_insertTMDescription, _editorPage.GetRevisionType(),
				"Произошла ошибка:\n Не отобразилась ревизия - {0} после подстановки из ТМ.",
				_insertTMDescription);

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(fiveSegment),
				"Произошла ошибка:\n Подтвердился таргет сегмент - {0}.", fiveSegment);
		}

		[Test, Description("S-28972"), ShortCheckList]
		public void AutoInsertionMatchesGreaterThanOneHundredPercentsWithoutConfirmTest()
		{
			var percents1 = 83;
			var percents2 = 98;
			var percents3 = 102;
			var secondSegment = 2;
			var thirdSegment = 3;
			var fiveSegment = 5;

			_editorPage.ClickUserPreferencesButton();

			_userPreferencesDialog
				.ClickConfirmMatcheshCheckBox()
				.SetMatchesInPercents(TMMatchesPercents.NinetyFive)
				.ClickSaveButton();

			_editorPage.ClickOnTargetCellInSegment(secondSegment);

			Assert.AreEqual(String.Empty, _editorPage.GetTargetText(secondSegment),
				"Произошла ошибка:\n Произошла автоподстановка в сегмент - {0}.", secondSegment);

			Assert.AreEqual(percents1, _editorPage.CatTranslationMatchPercent(rowNumber: 1),
				"Произошла ошибка:\n Количество процентов совпадения из ТМ не - {0}%.", percents1);

			_editorPage.ClickOnTargetCellInSegment(thirdSegment);

			Assert.IsTrue(_editorPage.IsTargetContainsText(thirdSegment, String.Empty),
				"Произошла ошибка:\n Произошла автоподстановка в сегмент - {0}.", thirdSegment);

			Assert.AreEqual(percents2, _editorPage.CatTranslationMatchPercent(rowNumber: 1),
				"Произошла ошибка:\n Количество процентов совпадения из ТМ не - {0}%.", percents2);

			Assert.AreEqual(_insertTMDescription, _editorPage.GetRevisionType(),
				"Произошла ошибка:\n Не отобразилась ревизия - {0} после подстановки из ТМ.", _insertTMDescription);

			Assert.IsFalse(_editorPage.IsSegmentConfirmed(thirdSegment),
				"Произошла ошибка:\n Подтвердился таргет сегмент - {0}.", thirdSegment);

			_editorPage.ClickOnTargetCellInSegment(fiveSegment);

			Assert.IsTrue(_editorPage.IsTargetContainsText(fiveSegment, _secondSourceString),
				"Произошла ошибка:\n Не произошла автоподстановка - {0} в сегмент.", _secondSourceString);

			Assert.AreEqual(percents3, _editorPage.CatTranslationMatchPercent(rowNumber: 1),
				"Произошла ошибка:\n Количество процентов совпадения из ТМ не - {0}%.", percents3);

			Assert.AreEqual(_insertTMDescription, _editorPage.GetRevisionType(),
				"Произошла ошибка:\n Не отобразилась ревизия - {0} после подстановки из ТМ.", _insertTMDescription);

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(fiveSegment),
				"Произошла ошибка:\n Не подтвердился таргет сегмент - {0}.", fiveSegment);
		}

		private string _firstSourceString;
		private string _secondSourceString;
	}
}