using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class AssignResponsiblesTests<TWebDriverProvider> : AssignResponsiblesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "ТС-120")]
		[Standalone]
		public void OpenAssignDialogFewTasksTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new []{ PathProvider.EditorTxtFile },
				tasks: new[] { WorkflowTask.Translation, WorkflowTask.Proofreading }
				);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.ClickSettingsButton()
				.ClickWorkflowTab();

			_workflowSetUptab
				.AddTask(WorkflowTask.Editing, taskNumber: 3)
				.SaveSettings();

			_projectSettingsPage
				.ClickProjectsTableCheckbox(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.ClickAssignButtonOnPanel();

			Assert.IsTrue(_taskAssignmentPage.IsTaskAssignmentPageOpened(), "Произошла ошибка:\nНе открылся диалог назначения пользователя.");
		}

		// TODO сделано только до 9 шага! аналитики должны уточнить PRX-13683
		[Test(Description = "ТС-13")]
		[Standalone]
		public void AssignFewTasksForFewUsersFewDocumentsTest()
		{
			var document1 = PathProvider.EditorTxtFile;
			var document2 = PathProvider.DocumentFile;
			var document3 = PathProvider.DocumentFile2;
			var document4 = PathProvider.DocumentFileToConfirm1;

			_secondUser = TakeUser(ConfigurationManager.Users);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new []{ document1, document2, document3 },
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing}
				);

			_projectsPage.OpenAssignDialog(_projectUniqueName, document1);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 1)
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, document2);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 1)
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, document3);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 1)
				.SetResponsible(_secondUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialogForSelectedDocuments(_projectUniqueName, new[] { document1, document2, document3 });

			var assignee = new List<string> { ThreadUser.NickName};
			assignee.Sort();

			Assert.AreEqual(assignee, _taskAssignmentPage.GetAssignneeName(1),
				"Произошла ошибка:\n Неверное имя в колонке Assignnees для задачи №1.");

			Assert.IsTrue(_taskAssignmentPage.IsCancelButtonDisplayed(ThreadUser.NickName, taskNumber: 1),
				"Произошла ошибка:\n Не отображается кнопка Cancel для задачи №1.");

			Assert.AreEqual("Different assignees", _taskAssignmentPage.GetAssignneeColumn(2),
				"Произошла ошибка:\n Неверное значение в колонке Assignnees для задачи №2.");

			Assert.IsTrue(_taskAssignmentPage.IsAssigneeDropdownDisplayed(3),
				"Произошла ошибка:\n Не отображается кнопка открытия дропдауна назначения для задачи №3.");
		}

		[Test]
		[Standalone]
		public void AssignUserFewTasksTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new []{ PathProvider.EditorTxtFile },
				tasks: new[] {
						WorkflowTask.Translation,
						WorkflowTask.Editing}
				);

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);
		}
	}
}
