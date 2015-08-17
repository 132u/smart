﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Standalone]
	public class EditorPreviousStageTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void PreviousStageButtonTest()
		{
			EditorHelper
				.AddTextToSegment()
				.ConfirmTranslation()
				.AssertIsSegmentConfirmed()
				.AssertSaveingStatusIsDisappeared()
				.ClickHomeButton()
				.OpenWorkflowSettings()
				.AddTask(TaskMode.Editing)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile), NickName, taskNumber: 2)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.SelectTask(TaskMode.Editing)
				.AssertSegmentIsNotLocked()
				.RollBack();
		}
	}
}
