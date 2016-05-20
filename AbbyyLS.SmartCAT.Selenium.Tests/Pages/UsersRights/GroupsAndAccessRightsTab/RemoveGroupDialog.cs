using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights.RemoveGroupDialog
{
	internal class RemoveGroupDialog : UsersAndRightsBasePage, IAbstractPage<RemoveGroupDialog>
	{
		public RemoveGroupDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new RemoveGroupDialog LoadPage()
		{
			if (!IsRemoveGroupDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка: Не удалось открыть дилог удаления группы.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Delete.
		/// </summary>
		public GroupsAndAccessRightsTab ClickDeleteButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Delete.");
			Driver.WaitUntilElementIsClickable(By.XPath(DELETE_BUTTON));
			// много кликов, т.к. на тимсити не прожимается нормально
			DeleteButton.JavaScriptClick();
			DeleteButton.DoubleClick();

			return new GroupsAndAccessRightsTab(Driver).LoadPage();
		}

		#endregion


		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся дилог удаления группы
		/// </summary>
		public bool IsRemoveGroupDialogOpened()
		{
			return Driver.WaitUntilElementIsAppear(By.XPath(DELETE_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = DELETE_BUTTON)]
		protected IWebElement DeleteButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DELETE_BUTTON = "//input[@value='Delete']";

		#endregion
	}
}
