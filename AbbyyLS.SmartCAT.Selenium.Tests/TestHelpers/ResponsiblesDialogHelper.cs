using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ResponsiblesDialogHelper : WorkspaceHelper
	{
		public ResponsiblesDialogHelper SetResponsible(string name, bool isGroup, int taskNumber = 1)
		{
			BaseObject.InitPage(_responsiblesDialog);
			_responsiblesDialog
				.SelectResponsible(name, isGroup)
				.ClickAssignButton(taskNumber);

			return this;
		}

		public ProjectSettingsHelper ClickCloseAssignDialog<T>() where T: class, IAbstractPage<T>, new()
		{
			BaseObject.InitPage(_responsiblesDialog);
			_responsiblesDialog.ClickCloseAssignDialog<T>();

			return new ProjectSettingsHelper();
		}

		public ResponsiblesDialogHelper OpenTaskResponsibles(int taskRowNumber = 1)
		{
			BaseObject.InitPage(_responsiblesDialog);
			_responsiblesDialog.OpenTaskResponsibles(taskRowNumber);

			return this;
		}

		public ResponsiblesDialogHelper AssertTaskResponsiblesListDisplay(int taskRowNumber = 1)
		{
			BaseObject.InitPage(_responsiblesDialog);
			_responsiblesDialog.AssertTaskResponsiblesListDisplay(taskRowNumber);

			return this;
		}

		public List<string> GetResponsibleUsersList()
		{
			BaseObject.InitPage(_responsiblesDialog);
			return _responsiblesDialog.GetResponsibleUsersList();
		}

		public List<string> GetResponsibleGroupsList()
		{
			BaseObject.InitPage(_responsiblesDialog);
			return _responsiblesDialog.GetResponsibleGroupsList();
		}

		public ResponsiblesDialogHelper ClickCancelAssignButton(int taskNumber = 1)
		{
			BaseObject.InitPage(_responsiblesDialog);
			_responsiblesDialog.ClickCancelAssignButton(taskNumber);

			return this;
		}

		public ResponsiblesDialogHelper AssertGroupExist(string groupName)
		{
			Assert.IsTrue(GetResponsibleGroupsList().Any(i => i == "Group: " + groupName),
				string.Format("Произошла ошибка:\n В выпадающем списке отсутствует новая группа: {0}", groupName));

			return this;
		}

		public ResponsiblesDialogHelper ConfirmCancel()
		{
			BaseObject.InitPage(_responsiblesDialog);
			_responsiblesDialog
				.WaitCancelConfirmButtonDisplay()
				.ClickConfirmCancelButton();

			return this;
		}

		public ResponsiblesDialogHelper AssertAssignStatus(string status)
		{
			BaseObject.InitPage(_responsiblesDialog);
			Assert.IsTrue(_responsiblesDialog.GetAssignStatus().Equals(status),
				"Произошла ошибка:\n стоит неверный статус задачи");

			return this;
		}

		public ResponsiblesDialogHelper AssertCancelAssignButtonExist(int taskNumber = 1)
		{
			BaseObject.InitPage(_responsiblesDialog);
			_responsiblesDialog.AssertCancelAssignButtonExist();

			return this;
		}

		private readonly ResponsiblesDialog _responsiblesDialog = new ResponsiblesDialog();
	}
}
