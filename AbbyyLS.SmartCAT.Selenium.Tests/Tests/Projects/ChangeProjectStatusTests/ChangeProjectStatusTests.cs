﻿using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	class ChangeProjectStatusTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> 
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7157"), ShortCheckList]
		public void SetCancelledStatusFromProjectsPageTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage
				.CancelProject(_projectUniqueName)
				.RefreshPage<ProjectsPage>()
				.GoToCancelledProjectsPage();

			Assert.IsTrue(_cancelledProjectsPage.IsProjectNameContainsInCancelledProjects(_projectUniqueName),
				"Произошла ошибка:\n Проект с указанным именем не находится на странице отменённых проектов.");
		}

		[Test, Description("S-13756"), ShortCheckList]
		public void SetCancelledStatusFromProjectSettingsPageTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.SetCancelledStatus()
				.GoToProjectsPage();

			_projectsPage.GoToCancelledProjectsPage();

			Assert.IsTrue(_cancelledProjectsPage.IsProjectNameContainsInCancelledProjects(_projectUniqueName),
				"Произошла ошибка:\n Проект с указанным именем не находится на странице отменённых проектов.");
		}

		[Test, Description("S-7159"), ShortCheckList]
		public void InProgressStatusAfterCreateProjectTest()
		{
			var targetSegment = "первое предложение.";
			var document = PathProvider.EditorTxtFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new [] {document});

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document);
			
			_editorPage
				.FillSegmentTargetField(targetSegment)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage()
				.GoToProjectsPage();

			Assert.AreEqual(_projectsPage.GetProjectStatus(_projectUniqueName), ProjectStatus.InProgress.Description(),
				"Произошла ошибка:\n Статус проекта не {0}.", DocumentStatus.InProgress.Description()); 
		}

		[Test, Description("S-7161"), ShortCheckList]
		public void CompleteStatusAfterConfirmAllSegmentsTest()
		{
			var targetSegment = "первое предложение.";
			var document = PathProvider.OneLineTxtFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new[] {document});

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, document);

			_editorPage
				.FillSegmentTargetField(targetSegment)
				.ConfirmSegmentTranslation()
				.ClickDoneButton()
				.ClickHomeButtonExpectingProjectSettingsPage()
				.GoToProjectsPage();

			Assert.IsTrue(_projectsPage.IsProjectStatusCompleted(_projectUniqueName),
				"Произошла ошибка:\n Статус проекта не {0}.", DocumentStatus.Completed.Description());
		}

		[Test, Description("S-29235"), ShortCheckList]
		public void StatusCheckForProjectWithoutPretranslateTest()
		{
			var document = PathProvider.OneLineTxtFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new[] { document });

			Assert.AreEqual(_projectsPage.GetProjectStatus(_projectUniqueName), ProjectStatus.Created.Description(),
				"Произошла ошибка:\n Статус документа не {0}", ProjectStatus.Created.Description());
		}

		[Test, Description("S-29237"), ShortCheckList]
		public void StatusCheckForProjectWithXliffDocxTest()
		{
			var xliffDocument = PathProvider.EditorXliffFile;
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new[] { xliffDocument, document });

			Assert.AreEqual(_projectsPage.GetProjectStatus(_projectUniqueName), ProjectStatus.Pretranslated.Description(),
				"Произошла ошибка:\n Статус документа не {0}", ProjectStatus.Pretranslated.Description());
		}

		[Test, Description("S-29236"), ShortCheckList]
		public void StatusCheckForProjectWithDocTmTest()
		{
			var tmx = PathProvider.EditorTmxFile;
			var document = PathProvider.EditorTxtFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new[] { document },
				tmxFilesPaths: new[] { tmx },
				rules: new List<KeyValuePair<PreTranslateRulles, WorkflowTask?>>
					{ new KeyValuePair<PreTranslateRulles, WorkflowTask?> (PreTranslateRulles.TM, null )});
			_projectsPage.RefreshPage<ProjectsPage>();

			Assert.AreEqual(_projectsPage.GetProjectStatus(_projectUniqueName), ProjectStatus.Pretranslated.Description(),
				"Произошла ошибка:\n Статус документа не {0}", ProjectStatus.Pretranslated.Description());
		}
	}
}