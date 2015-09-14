using System;
using System.Collections.Generic;
using System.Threading;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class UsersRightsHelper : WorkspaceHelper
	{
		/// <summary>
		/// Проверить, существует ли нужная нам группа, если нет, то создаем
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsHelper CheckOrCreateGroup(string groupName)
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage
				.OpenHideMenuIfClosed()
				.ClickUsersRightsButton()
				.ClickGroupsButton();

			if (!_usersRightsPage.IsGroupExists(groupName))
			{
				_usersRightsPage
					.ClickCreateGroupButton()
					.AssertAddNewGroupForm()
					.SetNewGroupName(groupName)
					.ClickSaveNewGroupButton()
					.AssertIsGroupCreated(groupName);
			}

			return this;
		}

		/// <summary>
		/// Проверить, есть ли в группе пользователь, если нет, то добавляем
		/// </summary>
		/// <param name="groupName">имя группы</param>
		/// <param name="userName">имя пользователя</param>
		public UsersRightsHelper CheckOrAddUserToGroup(string groupName, string userName)
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.SelectGroup(groupName);
			if (_usersRightsPage.IsEditGroupButtonDisplayed(groupName))
			{
				_usersRightsPage.ClickEditGroupButton(groupName);
			}
			if (!_usersRightsPage.IsGroupUserAdded(groupName, userName))
			{
				_usersRightsPage.ClickAddUsersSearchbox(groupName)
					.AssertIsAddGroupUserButtonExists(groupName, userName)
					.ClickAddGroupUserButton(groupName, userName)
					.ClickSaveButton(groupName)
					.AssertIsGroupUserAdded(groupName, userName);
			}

			return this;
		}

		/// <summary>
		/// Проверить есть у группы права на создание, управление и просмотр проектов, если нет, добавляем
		/// </summary>
		/// <param name="groupName">имя группы</param>
		/// <param name="right">добавляемое право</param>
		public UsersRightsHelper CheckOrAddRightsToGroup(string groupName, RightsType right)
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.SelectGroup(groupName);

			if (_usersRightsPage.IsEditGroupButtonDisplayed(groupName))
			{
				_usersRightsPage.ClickEditGroupButton(groupName);
			}

			_usersRightsPage
				.ClickAddRightsButton(groupName)
				.ClickRightRadio(right)
				.ClickNextButton()
				.ClickForAnyProjectRadio()
				.ClickAddRightButton()
				.AssertDialogBackgroundDisappeared<UsersRightsPage>()
				.AssertIsCreateProjectsRightAdded(groupName);
			
			return this;
		}

		/// <summary>
		/// Сохранить новые настройки группы и переходим на вкладку Workspace
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public UsersRightsHelper SaveNewGroupSettings(string groupName)
		{
			BaseObject.InitPage(_usersRightsPage);
			Thread.Sleep(1000);
			_usersRightsPage
				.ClickSaveButton(groupName)
				.OpenHideMenuIfClosed()
				.ClickProjectsButton();

			return this;
		}

		public List<string> GetUserNameList()
		{
			BaseObject.InitPage(_usersRightsPage);
			return _usersRightsPage.GetUserNameList();
		}

		public List<string> GetGroupNameList()
		{
			BaseObject.InitPage(_usersRightsPage);
			return _usersRightsPage.GetGroupNameList();
		}
		
		public UsersRightsHelper ClickGroupsButton()
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.ClickGroupsButton();

			return this;
		}

		public UsersRightsHelper AssertIsUserExist(string username)
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.AssertIsUserExist(username);

			return this;
		}

		public string GetGroupUniqueName()
		{
			// Sleep необходим, чтобы имена были уникальными, когда создаём несколько имён подряд. Чтобы не вышло, что кол-во тиков одинаковое.
			Thread.Sleep(10);
			return "GroupTest" + DateTime.Now.Ticks.ToString();
		}

		public UsersRightsHelper ClickSortByFirstName()
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.ClickSortByFirstName();

			return this;
		}

		public UsersRightsHelper ClickSortByLastName()
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.ClickSortByLastName();

			return this;
		}

		public UsersRightsHelper ClickSortByShortName()
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.ClickSortByShortName();

			return this;
		}

		public UsersRightsHelper ClickSortByEmailAddress()
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.ClickSortByEmailAddress();

			return this;
		}

		public UsersRightsHelper ClickSortByGroups()
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.ClickSortByGroups();

			return this;
		}

		public UsersRightsHelper ClickSortByCreated()
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.ClickSortByCreated();

			return this;
		}

		public UsersRightsHelper ClickSortByStatus()
		{
			BaseObject.InitPage(_usersRightsPage);
			_usersRightsPage.ClickSortByStatus();

			return this;
		}

		public UsersRightsHelper RemoveUserFromAllGroups(string userName)
		{
			BaseObject.InitPage(_usersRightsPage);

			var groups = _usersRightsPage.GetGroupNameList();

			foreach (var group in groups)
			{
				_usersRightsPage.SelectGroup(group);

				if (_usersRightsPage.IsGroupUserAdded(group, userName))
				{
					_usersRightsPage
						.ClickEditGroupButton(group)
						.ClickDeleteUserButton(group, userName)
						.ClickSaveButton(group);
				}

				_usersRightsPage.SelectGroup(group);
			}

			return this;
		}

		private readonly UsersRightsPage _usersRightsPage = new UsersRightsPage();
	}
}
