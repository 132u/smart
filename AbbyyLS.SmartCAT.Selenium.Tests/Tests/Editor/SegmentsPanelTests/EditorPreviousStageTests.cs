using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorPreviousStageTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "Проверяет блокировку сегментов при добавлении задачи на редактирование")]
		public void PreviousStageButtonTest()
		{
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);

			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsHelper.OpenWorkflowSettings();

			_settingsDialog.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_projectSettingsHelper.AssignTasksOnDocument(PathProvider.EditorTxtFile, ThreadUser.NickName, taskNumber: 2);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			_selectTaskDialog.SelectTask(TaskMode.Editing);

			Assert.IsFalse(_editorPage.IsSegmentLocked(), "Произошла ошибка:\n сегмент залочен");

			_editorPage.RollBack();

			Assert.IsTrue(_editorPage.IsSegmentLocked(), "Произошла ошибка:\n сегмент не залочен");
		}
		
		private ProjectSettingsHelper _projectSettingsHelper;
	}
}
