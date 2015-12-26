using System.IO;

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

			EditorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsHelper.OpenWorkflowSettings();

			SettingsDialog
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			ProjectSettingsHelper.AssignTasksOnDocument(PathProvider.EditorTxtFile, ThreadUser.NickName, taskNumber: 2);

			ProjectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			SelectTaskDialog.SelectTask(TaskMode.Editing);

			Assert.IsFalse(EditorPage.IsSegmentLocked(), "Произошла ошибка:\n сегмент залочен");

			EditorPage.RollBack();

			Assert.IsTrue(EditorPage.IsSegmentLocked(), "Произошла ошибка:\n сегмент не залочен");
		}

		private ProjectSettingsHelper _projectSettingsHelper;
	}
}
