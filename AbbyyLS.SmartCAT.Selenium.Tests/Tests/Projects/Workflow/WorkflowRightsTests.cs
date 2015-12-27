using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects.Workflow
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class WorkflowRightsTests<TWebDriverProvider> : WorkflowBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void WorkflowRightsTestsSetUp()
		{
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			_groupName = _groupsAndAccessRightsTab.GetGroupUniqueName();

			_workspaceHelper.GoToUsersPage();

			_usersTab
				.ClickGroupsButton()
				.RemoveUserFromAllGroups(AdditionalUser.NickName);

			if (!_groupsAndAccessRightsTab.IsGroupExists(_groupName))
			{
				_groupsAndAccessRightsTab.OpenNewGroupDialog();

				_newGroupDialog.CreateNewGroup(_groupName);
			}

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(_groupName, AdditionalUser.NickName);

			_workspaceHelper.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileName(PathProvider.EditorTxtFile), AdditionalUser.NickName);
		}

		[Test, Description("ТС-56 Пользователь назначен  на один этап одного документа")]
		public void OneTaskTest()
		{
			_projectSettingsPage
				.ClickSettingsButton()
				.ClickWorkflowTab()
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_workspaceHelper.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);
			
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDocumentRef(PathProvider.EditorTxtFile);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(), "Произошла ошибка:\n Редактор не открылся.");
		}

		[Test, Description("ТС-561 Назначить пользователя на несколько этапов одного документа")]
		public void TwoTaskTest()
		{
			_projectSettingsPage
				.ClickSettingsButton()
				.ClickWorkflowTab()
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();

			_workspaceHelper.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_workspaceHelper.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDocumentRef(PathProvider.EditorTxtFile);

			Assert.AreEqual(2, _selectTaskDialog.GetTaskCount(),
				"Произошла ошибка:\n Неверное количество задач в диалоге выбора при входе в редактор.");
		}

		[Test, Description("ТС-562 Назначить пользователя на несколько этапов одного документа 2 тест")]
		public void TwoTaskCancelOneTaskTest()
		{
			_projectSettingsPage
				.ClickSettingsButton()
				.ClickWorkflowTab()
				.AddTask(WorkflowTask.Editing)
				.SaveSettings();
		
			_workspaceHelper.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.SetResponsible(AdditionalUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton();

			_workspaceHelper
				.GoToProjectsPage()
				.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.ClickCancelAssignButton(taskNumber: 1)
				.ClickSaveButton();

			_workspaceHelper.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);
			
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDocumentRef(PathProvider.EditorTxtFile);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка:\n Редактор не открылся.");
		}

		[Test]
		[Description("ТС-054 Отмена участия исполнителя  с этапа, который в процессе работы")]
		public void DeclineAssigneeInProgressTest()
		{
			var text2 = "Translation 2";

			_workspaceHelper.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			_projectsPage.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDocumentRef(PathProvider.EditorTxtFile)
				.CloseTutorialIfExist();

			_editorPage
				.FillTarget(_text, rowNumber: 1)
				.ConfirmSegmentTranslation()
				.FillTarget(text2, rowNumber: 2)
				.ConfirmSegmentTranslation();

			_editorPage.ClickHomeButtonExpectingProjectsPage();

			_workspaceHelper.SignOut();

			_loginHelper.LogInSmartCat(ThreadUser.Login, ThreadUser.NickName, ThreadUser.Password);

			_projectsPage.OpenAssignDialog(_projectUniqueName);

			_taskAssignmentPage
				.ClickCancelAssignButton()
				.ClickSaveButton();

			_workspaceHelper.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDocumentRef(PathProvider.EditorTxtFile);

			Assert.AreEqual(_text, _editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\n Неверное значение в таргете сегмента №1.");

			Assert.AreEqual(text2, _editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка:\n Неверное значение в таргете сегмента №2.");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspaceHelper.SignOut();

			_loginHelper.LogInSmartCat(
				AdditionalUser.Login,
				AdditionalUser.NickName,
				AdditionalUser.Password);

			Assert.IsTrue(_projectsPage.IsProjectNotExistInList(_projectUniqueName),
				"Произошла ошибка:\nПроект {0} присутствует в списке.", _projectUniqueName);
		}

		private string _groupName;
	}
}
