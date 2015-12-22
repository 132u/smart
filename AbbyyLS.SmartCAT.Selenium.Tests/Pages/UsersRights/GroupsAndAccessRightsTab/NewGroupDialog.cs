using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class NewGroupDialog: GroupsAndAccessRightsTab, IAbstractPage<NewGroupDialog>
	{
		public NewGroupDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new NewGroupDialog GetPage()
		{
			var newGroupDialog = new NewGroupDialog(Driver);
			InitPage(newGroupDialog, Driver);

			return newGroupDialog;
		}

		public new void LoadPage()
		{
			if (!IsNewGroupDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть диалог создания новой группы.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести имя создаваемой группы
		/// </summary>
		/// <param name="groupName">имя группы</param>
		public NewGroupDialog SetNewGroupName(string groupName)
		{
			CustomTestContext.WriteLine("Ввести имя создаваемой группы: {0}.", groupName);
			GroupName.SetText(groupName);

			return GetPage();
		}

		/// <summary>
		/// Сохранить новую группу
		/// </summary>
		public GroupsAndAccessRightsTab ClickSaveNewGroupButton()
		{
			CustomTestContext.WriteLine("Сохранить новую группу.");
			SaveNewGroupButton.Click();

			return new GroupsAndAccessRightsTab(Driver).GetPage();
		}

		#endregion


		#region Составные методы страницы

		/// <summary>
		/// Создать новую группу
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public GroupsAndAccessRightsTab CreateNewGroup(string groupName)
		{
			SetNewGroupName(groupName);
			ClickSaveNewGroupButton();
			WaitUntilDialogBackgroundDisappeared();

			return new GroupsAndAccessRightsTab(Driver).GetPage();
		}

		#endregion


		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что диалог создания группы открылся
		/// </summary>
		public bool IsNewGroupDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(GROUP_NAME));
		}

		#endregion


		#region Вспомогательные методы


		#endregion


		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = GROUP_NAME)]
		protected IWebElement GroupName { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_NEW_GROUP_BUTTON)]
		protected IWebElement SaveNewGroupButton { get; set; }

		#endregion
		
		#region Описание XPath элементов

		protected const string GROUP_NAME = "//input[contains(@class, 'group-popup__name')]";
		protected const string SAVE_NEW_GROUP_BUTTON = "//div[contains(@class, 'g-popupbox__ft')]//a[contains(string(),'Create Group')]";

		#endregion
	}
}
