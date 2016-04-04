using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class UsersAndRightsBasePage : WorkspacePage, IAbstractPage<UsersAndRightsBasePage>
	{
		public UsersAndRightsBasePage(WebDriver driver) : base(driver)
		{
		}

		public new UsersAndRightsBasePage LoadPage()
		{
			if (!IsUsersAndRightsBasePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось перейти на вкладку 'Пользователи и права'.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Перейти на вкладку "Группы и права"
		/// </summary>
		public GroupsAndAccessRightsTab ClickGroupsButton()
		{
			CustomTestContext.WriteLine("Перейти на вкладку 'Группы и права'.");
			GroupsRightsButton.Click();

			return new GroupsAndAccessRightsTab(Driver).LoadPage();
		}
		
		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница Users and Rights
		/// </summary>
		public bool IsUsersAndRightsBasePageOpened()
		{
			return GroupsRightsButton.Displayed;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = GROUPS_RIGHTS_BTN_XPATH)]
		protected IWebElement GroupsRightsButton { get; set; }

		#endregion

		#region Описание XPath элементов
		
		protected const string GROUPS_RIGHTS_BTN_XPATH = "//a[contains(@href,'/Groups/Index')]";

		#endregion
	}
}
