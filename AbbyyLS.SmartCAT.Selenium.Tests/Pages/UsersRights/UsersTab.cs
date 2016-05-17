using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class UsersTab : UsersAndRightsBasePage, IAbstractPage<UsersTab>
	{
		public UsersTab(WebDriver driver) : base(driver)
		{
		}

		public new UsersTab LoadPage()
		{
			if (!IsUsersTabOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть диалог добавления права.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку добавления пользователя
		/// </summary>
		public AddUserDialog ClickAddUserButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку добавления пользователя");
			AddUserButton.Click();

			return new AddUserDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по имени
		/// </summary>
		public UsersTab ClickSortByFirstName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по имени.");
			SortByFirstName.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки фамилии
		/// </summary>
		public UsersTab ClickSortByLastName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки фамилии");
			SortByLastName.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по ShortName
		/// </summary>
		public UsersTab ClickSortByShortName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по ShortName");
			SortByShortName.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по адресу почты
		/// </summary>
		public UsersTab ClickSortByEmailAddress()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по адресу почты");
			SortByEmail.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по группе
		/// </summary>
		public UsersTab ClickSortByGroups()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по группе");
			SortByGroups.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате создания
		/// </summary>
		public UsersTab ClickSortByCreated()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате создания");
			SortByCreated.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по статусу
		/// </summary>
		public UsersTab ClickSortByStatus()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по статусу");
			SortByStatus.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить статус пользователя
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public string GetStatus(string userName)
		{
			CustomTestContext.WriteLine("Получить статус пользователя {0}.", userName);
			StatusValue = Driver.SetDynamicValue(How.XPath, STATUS_VALUE, userName);

			return StatusValue.Text;
		}

		/// <summary>
		/// Получить группу пользователя
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public string GetGroup(string userName)
		{
			CustomTestContext.WriteLine("Получить группу пользователя {0}.", userName);
			GroupValue = Driver.SetDynamicValue(How.XPath, GROUP_VALUE, userName);

			return GroupValue.Text;
		}

		/// <summary>
		/// Навести курсор на строку пользователя
		/// </summary>
		/// <param name="userName">имя </param>
		public UsersTab HoverUserRow(string userName)
		{
			CustomTestContext.WriteLine("Навести курсор на строку пользователя {0}.", userName);
			NameValue = Driver.SetDynamicValue(How.XPath, NAME_VALUE, userName);
			NameValue.HoverElement();

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(EDIT_BUTTON.Replace("*#*", userName))))
			{
				throw new Exception("Произошла ошибка: не появилась кнопка редактирования пользователя.");
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку редактирования пользователя
		/// </summary>
		/// <param name="userName">имя </param>
		public ChangeUserDataDialog ClickEditButton(string userName)
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования пользователя.");
			EditButton = Driver.SetDynamicValue(How.XPath, EDIT_BUTTON, userName);
			EditButton.Click();

			return new ChangeUserDataDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку удаления пользователя
		/// </summary>
		/// <param name="userName">имя </param>
		public RemoveUserDialog ClickDeleteButton(string userName)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления пользователя.");
			DeleteButton = Driver.SetDynamicValue(How.XPath, DELETE_BUTTON, userName);
			DeleteButton.Click();

			return new RemoveUserDialog(Driver).LoadPage();
		}

		#endregion
		
		#region Составные методы страницы

		/// <summary>
		/// Открыть диалог редактирования пользователя
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public ChangeUserDataDialog OpenChangeUserDialog(string userName)
		{
			HoverUserRow(userName);
			
			return ClickEditButton(userName);
		}

		/// <summary>
		/// Удалить пользователя
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public RemoveUserDialog DeleteUser(string userName)
		{
			HoverUserRow(userName);

			return ClickDeleteButton(userName);
		}

		#endregion
		
		#region Методы, проверяющие состояние страницы
		
		/// <summary>
		/// Проверить, что пользователь есть в списке
		/// </summary>
		/// <param name="username">имя пользователя</param>
		public bool IsUserExistInList(string username)
		{
			CustomTestContext.WriteLine("Проверить, что пользователь есть в списке");

			return GetUserNameList().Contains(username);
		}

		/// <summary>
		/// Проверить, что открылась вкладка Пользователи
		/// </summary>
		public bool IsUsersTabOpened()
		{
			return IsDialogBackgroundDisappeared() && Driver.WaitUntilElementIsDisplay(By.XPath(USER_SURNAME_LIST));
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Получить список пользователей
		/// </summary>
		public List<string> GetUserNameList()
		{
			CustomTestContext.WriteLine("Получить список пользователей");

			var nameList = Driver.GetTextListElement(By.XPath(USER_NAME_LIST));
			var surnameList = Driver.GetTextListElement(By.XPath(USER_SURNAME_LIST));

			if (nameList.Count != surnameList.Count)
			{
				throw new Exception("Произошла ошибка:\n размеры списка фамилий и списка имён не совпадают.");
			}
			var d = nameList.Select((t, i) => (t + " " + surnameList[i]).Trim()).ToList();
			return nameList.Select((t, i) => (t + " " + surnameList[i]).Trim()).ToList();
		}

		/// <summary>
		/// Получить фамилию пользователя
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public string GetSurname(string userName)
		{
			CustomTestContext.WriteLine("Получить фамилию пользователя {0}.", userName);
			SurnameValue = Driver.SetDynamicValue(How.XPath, SURNAME_VALUE, userName);

			return SurnameValue.Text;
		}

		/// <summary>
		/// Получить email пользователя
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public string GetEmail(string userName)
		{
			CustomTestContext.WriteLine("Получить email пользователя {0}.", userName);
			EmailValue = Driver.SetDynamicValue(How.XPath, EMAIL_VALUE, userName);

			return EmailValue.Text;

		}

		/// <summary>
		/// Получить дату создания пользователя
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public DateTime GetCreatedDate(string userName)
		{
			CustomTestContext.WriteLine("Получить дату создания пользователя {0}.", userName);
			CreatedDateValue = Driver.SetDynamicValue(How.XPath, CREATED_DATE_VALUE, userName);

			return DateTime.ParseExact(CreatedDateValue.Text, "M/d/yyyy", CultureInfo.InvariantCulture).Date;
		}

		#endregion
		
		#region Объявление элементов страницы
		
		[FindsBy(How = How.XPath, Using = SORT_BY_FIRST_NAME)]
		protected IWebElement SortByFirstName { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_LAST_NAME)]
		protected IWebElement SortByLastName { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_SHORT_NANE)]
		protected IWebElement SortByShortName { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_EMAIL)]
		protected IWebElement SortByEmail { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_GROUPS)]
		protected IWebElement SortByGroups { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_CREATED)]
		protected IWebElement SortByCreated { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_STATUS)]
		protected IWebElement SortByStatus { get; set; }

		[FindsBy(How = How.XPath, Using = USER_SURNAME_LIST)]
		protected IWebElement UserSurnameList { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_USER_BUTTON)]
		protected IWebElement AddUserButton { get; set; }

		protected IWebElement StatusValue { get; set; }
		protected IWebElement GroupValue { get; set; }
		protected IWebElement EditButton { get; set; }
		protected IWebElement DeleteButton { get; set; }
		protected IWebElement CreatedDateValue { get; set; }
		protected IWebElement SurnameValue { get; set; }
		protected IWebElement NameValue { get; set; }
		protected IWebElement EmailValue { get; set; }

		#endregion
		
		#region Описание XPath элементов

		protected const string USER_SURNAME_LIST = ".//table[contains(@class, 'js-users')]//tr[contains(@class, 'js-users-trwork')]//td[contains(@class, 'js-user-surname')]/p";
		protected const string USER_NAME_LIST = ".//table[contains(@class, 'js-users')]//tr[contains(@class, 'js-users-trwork')]//td[contains(@class, 'js-user-name')]/p";
		protected const string SURNAME_VALUE = "//td[contains(@class, 'user-name')]//p[text()='*#*']/..//following-sibling::td[contains(@class, 'surname')]/p";
		protected const string EMAIL_VALUE = "//td[contains(@class, 'user-name')]//p[text()='*#*']/..//following-sibling::td[contains(@class, 'email')]/p";
		protected const string CREATED_DATE_VALUE = "//td[contains(@class, 'user-name')]//p[text()='*#*']/..//following-sibling::td[contains(@class, 'created')]/p";
		protected const string NAME_VALUE = "//td[contains(@class, 'user-name')]//p[text()='*#*']";

		protected const string SORT_BY_FIRST_NAME = "(//th[contains(@data-sort-by,'Name')]//a)[1]";
		protected const string SORT_BY_LAST_NAME = "//th[contains(@data-sort-by,'Surname')]//a";
		protected const string SORT_BY_SHORT_NANE = "(//th[contains(@data-sort-by,'Name')]//a)[2]";
		protected const string SORT_BY_EMAIL = "//th[contains(@data-sort-by,'EMail')]//a";
		protected const string SORT_BY_GROUPS = "";
		protected const string SORT_BY_CREATED = "//th[contains(@data-sort-by,'CreatedDate')]//a";
		protected const string SORT_BY_STATUS = "//th[contains(@data-sort-by,'Status')]//a";
		protected const string ADD_USER_BUTTON = "//div[contains(@class, 'adduser')]//a[contains(@class, 'purplebtn')]";
		protected const string STATUS_VALUE = "//td//p[text()='*#*']/../following-sibling::td[contains(@class, 'user-status')]//p";
		protected const string GROUP_VALUE = "//td//p[text()='*#*']/../following-sibling::td[contains(@class, 'user-groups')]//p";
		protected const string EDIT_BUTTON = "//td//p[text()='*#*']/../following-sibling::td//i[contains(@class, 'edit-btn')]";
		protected const string DELETE_BUTTON = "//td//p[text()='*#*']/../following-sibling::td//i[contains(@class, 'delete-btn')]";

		#endregion
	}
}
