using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class TaskAssignmentDialogHelper : WorkspaceHelper
	{
		public TaskAssignmentDialogHelper(WebDriver driver) : base(driver)
		{
			_taskAssignmentDialog = new TaskAssignmentPage(Driver);
		}

		public TaskAssignmentDialogHelper SetResponsible(string name, bool isGroup, int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog, Driver);
			_taskAssignmentDialog
				.SelectResponsible(name, isGroup)
				.ClickAssignButton(taskNumber);

			return this;
		}

		public ProjectSettingsHelper CloseTaskAssignmentDialog<T>() where T : class, IAbstractPage<T>
		{
			BaseObject.InitPage(_taskAssignmentDialog, Driver);
			// TODO: дописать метод 

			return new ProjectSettingsHelper(Driver);
		}

		public TaskAssignmentDialogHelper OpenAssigneeDropbox(int taskRowNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog, Driver);
			_taskAssignmentDialog.OpenAssigneeDropbox(taskRowNumber);

			return this;
		}

		public TaskAssignmentDialogHelper AssertTaskAssigneeListDisplay(int taskRowNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog, Driver);
			_taskAssignmentDialog.AssertTaskAssigneeListDisplay(taskRowNumber);

			return this;
		}

		public List<string> GetResponsibleUsersList()
		{
			BaseObject.InitPage(_taskAssignmentDialog, Driver);
			return _taskAssignmentDialog.GetResponsibleUsersList();
		}

		public List<string> GetResponsibleGroupsList()
		{
			BaseObject.InitPage(_taskAssignmentDialog, Driver);
			return _taskAssignmentDialog.GetResponsibleGroupsList();
		}

		public TaskAssignmentDialogHelper ClickCancelAssignButton(int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmentDialog, Driver);
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
			BaseObject.InitPage(_taskAssignmentDialog, Driver);
			_taskAssignmentDialog
				.WaitCancelConfirmButtonDisplay()
				.ClickConfirmCancelButton();

			return this;
		}

		private readonly TaskAssignmentPage _taskAssignmentDialog;
	}
}
