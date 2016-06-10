using System;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor.UserPreferences;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Projects]
	class RepetitionsSettingsTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpRepetitionsSettingsTests()
		{
			_repetitionsSettingsDialog = new RepetitionsSettingsDialog(Driver);
			_userPreferencesDialog = new UserPreferencesDialog(Driver);
			_autoPropagationTab = new AutoPropagationTab(Driver);
			_documentName = PathProvider.RepetitionsWithDifferencesInCaseTxtFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { _documentName });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickRepetiotinsButton();
		}

		[TestCase(true, true, true, true, Description = "S-7261"), ShortCheckList]
		[TestCase(true, true, true, false, Description = "S-7263"), ShortCheckList]
		[TestCase(false, false, false, false, Description = "S-7264"), ShortCheckList]
		public void RepetitionsSettingInEditorDisabledTest(
			bool autoPropagateRepetitionsChecked, 
			bool propagateToSegmentsWithDifferencesInCaseChecked, 
			bool propagateToConfirmedSegmentsChecked, 
			bool confirmAutoPropagatedSegmnetsChecked)
		{
			_repetitionsSettingsDialog.SetRepetitionsSettings(
				autoPropagateRepetitionsChecked: autoPropagateRepetitionsChecked,
				propagateToSegmentsWithDifferencesInCaseChecked: propagateToSegmentsWithDifferencesInCaseChecked,
				propagateToConfirmedSegmentsChecked: propagateToConfirmedSegmentsChecked,
				confirmAutoPropagatedSegmnetsChecked: confirmAutoPropagatedSegmnetsChecked);

			_repetitionsSettingsDialog.ClickSaveButton();

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(_documentName);

			_editorPage.ClickUserPreferencesButton();

			_userPreferencesDialog.SwitchToAutoPropagationTab();

			Assert.IsTrue(_autoPropagationTab.CheckCheckboxesDisabled(
					autoPropagateRepetitionsDisabled: autoPropagateRepetitionsChecked,
					propagateToSegmentsWithDifferencesInCaseDisabled: propagateToSegmentsWithDifferencesInCaseChecked,
					propagateToConfirmedSegmentsDisabled: propagateToConfirmedSegmentsChecked,
					confirmAutoPropagatedSegmnetsDisabled: confirmAutoPropagatedSegmnetsChecked),
				"Произошла ошибка:\n задизэйблены неверные чекбоксы");
		}

		[Test, Description("S-7262"), ShortCheckList]
		[TestCase(true)]
		[TestCase(false)]
		public void RepetitionsWithDifferencesInCaseSettingsTest(bool propagateToSegmentsWithDifferencesInCaseChecked)
		{
			var translation = "translation";

			_repetitionsSettingsDialog.SetRepetitionsSettings(
				autoPropagateRepetitionsChecked: true,
					propagateToSegmentsWithDifferencesInCaseChecked: propagateToSegmentsWithDifferencesInCaseChecked,
					propagateToConfirmedSegmentsChecked: true,
					confirmAutoPropagatedSegmnetsChecked: true);

			_repetitionsSettingsDialog.ClickSaveButton();

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(_documentName);

			_editorPage.ClickUserPreferencesButton();

			_userPreferencesDialog.SwitchToAutoPropagationTab();

			Assert.AreEqual(
				propagateToSegmentsWithDifferencesInCaseChecked,
				_autoPropagationTab.IsPropagateToSegmentsWithDifferencesInCaseChecked(),
				"Произошла ошибка:\n выставлены неверные настройки повторов для сегментов, отличающихся регистром.");

			_userPreferencesDialog.ClickCloseButton();

			_editorPage
				.FillTarget(translation, rowNumber: 1)
				.ConfirmSegmentTranslation();

			// Проходит подтверждение и проставляются повторы 
			Thread.Sleep(5000);

			Assert.AreEqual(
				propagateToSegmentsWithDifferencesInCaseChecked 
					? translation 
					: String.Empty, 
				_editorPage.GetTargetText(rowNumber: 4),
				"Произошла ошибка:\n Настройки повторов не применились №4.");
		}

		private RepetitionsSettingsDialog _repetitionsSettingsDialog;
		private UserPreferencesDialog _userPreferencesDialog;
		private AutoPropagationTab _autoPropagationTab;
	}
}
