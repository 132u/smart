using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class TaskAssignmentDialogHelper : WorkspaceHelper
	{
		public TaskAssignmentDialogHelper SetResponsible(string name, bool isGroup, int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			_taskAssignmentDialog
				.SelectResponsible(name, isGroup)
				.ClickAssignButton(taskNumber);

			return this;
		}

		public ProjectSettingsHelper CloseTaskAssignmentDialog<T>() where T : class, IAbstractPage<T>, new()
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			_taskAssignmentDialog.ClickCloseTaskAssignmentDialog<T>();
			_taskAssignmentDialog.AssertTaskAssignmentDialogDisappear<T>();
			_taskAssignmentDialog.AssertDialogBackgroundDissapeared<T>();

			return new ProjectSettingsHelper();
		}

		public TaskAssignmentDialogHelper OpenAssigneeDropbox(int taskRowNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			_taskAssignmentDialog.OpenAssigneeDropbox(taskRowNumber);

			return this;
		}

		public TaskAssignmentDialogHelper AssertTaskAssigneeListDisplay(int taskRowNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			_taskAssignmentDialog.AssertTaskAssigneeListDisplay(taskRowNumber);

			return this;
		}

		public List<string> GetResponsibleUsersList()
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			return _taskAssignmentDialog.GetResponsibleUsersList();
		}

		public List<string> GetResponsibleGroupsList()
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			return _taskAssignmentDialog.GetResponsibleGroupsList();
		}

		public TaskAssignmentDialogHelper ClickCancelAssignButton(int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			_taskAssignmentDialog.ClickCancelAssignButton(taskNumber);

			return this;
		}

		public TaskAssignmentDialogHelper AssertGroupExist(string groupName)
		{
			Assert.IsTrue(GetResponsibleGroupsList().Any(i => i == "Group: " + groupName),
				string.Format("Произошла ошибка:\n В выпадающем списке отсутствует новая группа: {0}", groupName));

			return this;
		}

		public TaskAssignmentDialogHelper ConfirmCancel()
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			_taskAssignmentDialog
				.WaitCancelConfirmButtonDisplay()
				.ClickConfirmCancelButton();

			return this;
		}

		public TaskAssignmentDialogHelper AssertAssignStatus(string expectedStatus, int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			_taskAssignmentDialog.AssertAssignStatus(expectedStatus, taskNumber);

			return this;
		}

		public TaskAssignmentDialogHelper AssertCancelAssignButtonExist(int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog);
			_taskAssignmentDialog.AssertCancelAssignButtonExist();

			return this;
		}

		private readonly TaskAssignmentDialog _taskAssignmentDialog = new TaskAssignmentDialog();
	}
}
