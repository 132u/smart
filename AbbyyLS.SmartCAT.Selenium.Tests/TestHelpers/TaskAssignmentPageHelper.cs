using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class TaskAssignmentPageHelper : WorkspaceHelper
	{
		public TaskAssignmentPageHelper(WebDriver driver) : base(driver)
		{
			_taskAssignmenPage = new TaskAssignmentPage(Driver);
			_selectAssigneePage = new SelectAssigneePage(Driver);
		}

		public TaskAssignmentPageHelper SetResponsible(string name, bool isGroup, int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmenPage, Driver);
			_taskAssignmenPage
				.SelectResponsible(name, isGroup)
				.ClickAssignButton(taskNumber);

			return this;
		}

		public TaskAssignmentPageHelper SelectAssignee(string assigneeName)
		{
			BaseObject.InitPage(_selectAssigneePage, Driver);
			_selectAssigneePage
				.ClickAnotherAssigneeButton()
				.ExpandAssigneeDropdown()
				.SelectAssigneeInDropdown(assigneeName)
				.ClickAssignButton()
				.AssertAssignButtonDisappeared()
				.AssertCancelAssigneeButtonDisplayed();

			return this;
		}

		public TaskAssignmentPageHelper CloseTaskAssignmentDialog()
		{
			BaseObject.InitPage(_selectAssigneePage, Driver);
			_selectAssigneePage.ClickCloseTaskAssignmentPage();

			return this;
		}

		public TaskAssignmentPageHelper OpenAssigneeDropbox(int taskRowNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmenPage, Driver);
			_taskAssignmenPage.OpenAssigneeDropbox(taskRowNumber);

			return this;
		}
		
		public ProjectSettingsHelper ClickSaveButton(int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmenPage, Driver);
			_taskAssignmenPage.ClicSaveAssignButton();

			return new ProjectSettingsHelper(Driver);
		}

		public ProjectSettingsHelper ClickCancelAssignButton()
		{
			BaseObject.InitPage(_taskAssignmenPage, Driver);
			_taskAssignmenPage.ClickCancelAssignButton();

			return new ProjectSettingsHelper(Driver);
		}

		public TaskAssignmentPageHelper SelectAssigneesForEntireDocument(int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmenPage, Driver);
			_taskAssignmenPage
				.ExpandSelectAssigneesDropdown(taskNumber)
				.SelectAssignmentType(AssignmentType.Simple, taskNumber);

			return this;
		}

		public TaskAssignmentPageHelper SelectDistributeDocumentAmongAssignees(int taskNumber = 1)
		{
			BaseObject.InitPage(_taskAssignmenPage, Driver);
			_taskAssignmenPage
				.ExpandSelectAssigneesDropdown(taskNumber)
				.SelectAssignmentType(AssignmentType.Split, taskNumber);

			return this;
		}

		private readonly TaskAssignmentPage _taskAssignmenPage;
		private readonly SelectAssigneePage _selectAssigneePage;
	}
}
