using System.Threading;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class UsersRightsHelper : WorkspaceHelper
	{
		/// <summary>
		/// Проверяем, существует ли нужная нам группа, если нет, то создаем
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsHelper CheckOrCreateGroup(string groupName)
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.ClickUsersRightsBtn().ClickGroupsBtn();
			if (!_usersRightsPage.IsGroupExists(groupName))
			{
				_usersRightsPage.ClickCreateGroupBtn()
					.AssertAddNewGroupForm()
					.SetNewGroupName(groupName)
					.ClickSaveNewGroupBtn()
					.AssertIsGroupCreated(groupName);
			}

			return this;
		}

		/// <summary>
		/// Проверяем, есть ли в группе пользователь, если нет, то добавляем
		/// </summary>
		/// <param name="groupName">имя группы</param>
		/// <param name="userName">имя пользователя</param>
		public UsersRightsHelper CheckOrAddUserToGroup(string groupName, string userName)
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.SelectGroup(groupName);
			if (_usersRightsPage.IsEditGroupBtnDisplayed(groupName))
			{
				_usersRightsPage.ClickEditGroupBtn(groupName);
			}
			if (!_usersRightsPage.IsGroupUserAdded(groupName, userName))
			{
				_usersRightsPage.ClickAddUsersSearchbox(groupName)
					.AssertIsAddGroupUserBtnExists(groupName, userName)
					.ClickAddGroupUserBtn(groupName, userName)
					.SelectGroup(groupName)
					.AssertIsGroupUserAdded(groupName, userName);
			}

			return this;
		}

		/// <summary>
		/// Проверяем есть у группы права на создание, управление и просмотр проектов, если нет, добавляем
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsHelper CheckOrAddRightsToGroup(string groupName)
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.SelectGroup(groupName);
			if (_usersRightsPage.IsEditGroupBtnDisplayed(groupName))
			{
				_usersRightsPage.ClickEditGroupBtn(groupName);
			}
			if (!_usersRightsPage.IsCreateProjectsRightAdded(groupName))
			{
				_usersRightsPage.ClickAddRightsBtn(groupName)
					.ClickCreateProjectsRadio()
					.ClickNextBtn()
					.ClickDefinedByConditionRadio()
					.ClickAddRightBtn()
					.AssertIsCreateProjectsRightAdded(groupName);
			}
			if (!_usersRightsPage.IsManageProjectsRightAdded(groupName))
			{
				Thread.Sleep(1000);
				_usersRightsPage.ClickAddRightsBtn(groupName)
					.ClickManageProjectsRadio()
					.ClickNextBtn()
					.ClickDefinedByConditionRadio()
					.ClickAddRightBtn()
					.AssertIsManageProjectsRightAdded(groupName);
			}
			if (!_usersRightsPage.IsViewProjectsRightAdded(groupName))
			{
				Thread.Sleep(1000);
				_usersRightsPage.ClickAddRightsBtn(groupName)
					.ClickViewProjectsRadio()
					.ClickNextBtn()
					.ClickDefinedByConditionRadio()
					.ClickAddRightBtn()
					.AssertIsViewProjectsRightAdded(groupName);
			}

			return this;
		}

		/// <summary>
		/// Сохраняем новые настройки группы и переходим на вкладку Workspace
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsHelper SaveNewGroupSettings(string groupName)
		{
			BaseObject.InitPage(_usersRightsPage);
			Thread.Sleep(1000);
			_usersRightsPage.ClickSaveBtn(groupName).ClickProjectsBtn();

			return this;
		}

		private readonly UsersRightsPage _usersRightsPage = new UsersRightsPage();
	}
}
