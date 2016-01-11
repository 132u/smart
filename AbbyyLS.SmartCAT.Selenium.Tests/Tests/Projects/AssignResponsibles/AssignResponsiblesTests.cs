using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;

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
				filePath: PathProvider.EditorTxtFile,
				tasks: new[] { WorkflowTask.Translation, WorkflowTask.Proofreading }
				);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage
				.ClickSettingsButton()
				.ClickWorkflowTab()
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
				filePath: document1,
				tasks: new[] { 
					WorkflowTask.Translation,
					WorkflowTask.Proofreading,
					WorkflowTask.Editing}
				);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage
				.ClickDocumentUploadButton()
				.UploadDocument(new[] { document2, document3 });

			_documentUploadGeneralInformationDialog
				.ClickFinish<ProjectSettingsPage>()
				.WaitUntilDocumentProcessed();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, documentNumber: 1);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 1)
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, documentNumber: 2);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 1)
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_projectsPage.OpenAssignDialog(_projectUniqueName, documentNumber: 3);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 1)
				.SetResponsible(_secondUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_projectsPage.OpenAssignDialogForSelectedDocuments(_projectUniqueName, new[] { document1, document2, document3 });
				
			Assert.AreEqual(ThreadUser.NickName, _taskAssignmentPage.GetAssignneeName(1),
				"Произошла ошибка:\n Неверное имя в колонке Assignnees для задачи №1.");

			Assert.IsTrue(_taskAssignmentPage.IsCancelButtonDisplayed(1),
				"Произошла ошибка:\n Не отображается кнопка Cancel для задачи №1.");

			Assert.AreEqual("Different assignees", _taskAssignmentPage.GetAssignneeColumn(2),
				"Произошла ошибка:\n Неверное значение в колонке Assignnees для задачи №2.");

			Assert.IsTrue(_taskAssignmentPage.IsAssignButtonDisplayed(3),
				"Произошла ошибка:\n Не отображается кнопка Assign для задачи №3.");
		}

		[Test]
		[Standalone]
		public void AssignUserFewTasksTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filePath: PathProvider.EditorTxtFile,
				tasks: new[] {
						WorkflowTask.Translation,
						WorkflowTask.Editing}
				);

			_projectsPage
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false)
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);
		}
	}
}
