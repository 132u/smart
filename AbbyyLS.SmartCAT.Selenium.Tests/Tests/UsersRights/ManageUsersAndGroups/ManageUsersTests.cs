using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageUsersAndGroupstest;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageClientsAndProjectGroups
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageUsersTests<TWebDriverProvider> : ManageUsersAndGroupsBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void BeforeTest()
		{
			_workspacePage.GoToUsersPage();
			_name = Guid.NewGuid().ToString();
			_surname = Guid.NewGuid().ToString();
			_email = Guid.NewGuid() + "@mailforspam.com";
			_today = DateTime.Today.Date;

			_workspacePage.GoToUsersPage();
			_usersTab.ClickAddUserButton();

			_addUserDialog.AddUser(_name, _surname, _email);
		}

		[Test]
		public void UserVisibilityTest()
		{
			Assert.IsTrue(_usersTab.IsUserExistInList(_name + " " + _surname),
				"Произошла ошибка: пользователь {0} отсутствует в таблице.", _name);

			Assert.AreEqual(_surname, _usersTab.GetSurname(_name),
				"Произошла ошибка: неверная фамилия пользователя {0}.", _name);

			Assert.AreEqual(_today, _usersTab.GetCreatedDate(_name),
				"Произошла ошибка: неверная дата создания пользователя {0}.", _name);

			Assert.AreEqual(_email, _usersTab.GetEmail(_name),
				"Произошла ошибка: неверный email пользователя {0}.", _name);
		}
		
		[Test]
		public void EditUserTest()
		{
			var newName = Guid.NewGuid().ToString();
			var newSurName = Guid.NewGuid().ToString();

			_usersTab.OpenChangeUserDialog(_name);
			_changeUserDataDialog.EditUser(newName, newSurName);

			Assert.IsTrue(_usersTab.IsUserExistInList(newName + " " + newSurName),
				"Произошла ошибка: пользователь {0} отсутствует в таблице.", newName);

			Assert.AreEqual(newSurName, _usersTab.GetSurname(newName),
				"Произошла ошибка: неверная фамилия пользователя {0}.", newName);

			Assert.AreEqual(_today, _usersTab.GetCreatedDate(newName),
				"Произошла ошибка: неверная дата создания пользователя {0}.", newName);
		}

		[Test]
		public void DeleteUserTest()
		{
			_usersTab.DeleteUser(_name);

			_removeUserDialog.ClickDeleteButton();

			Assert.IsFalse(_usersTab.IsUserExistInList(_name + " " + _surname),
				"Произошла ошибка: пользователь {0} присутствует в таблице.", _name);
		}

		protected string _name;
		protected string _surname;
		protected string _email;
		protected DateTime _today;
	}
}
