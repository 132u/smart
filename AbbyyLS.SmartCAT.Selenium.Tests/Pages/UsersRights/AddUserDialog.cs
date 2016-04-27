using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class AddUserDialog : UsersAndRightsBasePage, IAbstractPage<AddUserDialog>
	{
		public AddUserDialog(WebDriver driver) : base(driver)
		{
		}

		public new AddUserDialog LoadPage()
		{
			if (!IsAddUserDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка: не удалось открыть диалог добавления пользователя.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести имя пользователя
		/// </summary>
		/// <param name="name">имя</param>
		public AddUserDialog FillName(string name)
		{
			CustomTestContext.WriteLine("Ввести имя пользователя {0}.", name);
			Name.SetText(name);

			return LoadPage();
		}

		/// <summary>
		/// Ввести фамилию пользователя
		/// </summary>
		/// <param name="surname">фамилия</param>
		public AddUserDialog FillSurname(string surname)
		{
			CustomTestContext.WriteLine("Ввести фамилию пользователя {0}.", surname);
			Surname.SetText(surname);

			return LoadPage();
		}

		/// <summary>
		/// Ввести email
		/// </summary>
		/// <param name="email">email</param>
		public AddUserDialog FillEmail(string email)
		{
			CustomTestContext.WriteLine("Ввести email пользователя {0}.", email);
			Email.SetText(email);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Add.
		/// </summary>
		public UsersTab ClickAddButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Add.");
			AddButton.JavaScriptClick();
			Driver.WaitUntilElementIsDisappeared(By.XPath(ADD_BUTTON));

			return new UsersTab(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Добавить пользователя
		/// </summary>
		/// <param name="name">имя</param>
		/// <param name="surname">фамилия</param>
		/// <param name="email">email</param>
		public UsersTab AddUser(string name, string surname, string email)
		{
			FillName(name);
			FillSurname(surname);
			FillEmail(email);

			return ClickAddButton();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог создания пользователя
		/// </summary>
		public bool IsAddUserDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(NAME));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = NAME)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = SURNAME)]
		protected IWebElement Surname { get; set; }

		[FindsBy(How = How.XPath, Using = EMAIL)]
		protected IWebElement Email { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string NAME = "//div[contains(@class, 'popup-add-user')]//input[@name='name']";
		protected const string SURNAME = "//div[contains(@class, 'popup-add-user')]//input[@name='surname']";
		protected const string EMAIL = "//div[contains(@class, 'popup-add-user')]//input[@name='email']";
		protected const string ADD_BUTTON = "//div[contains(@class, 'popup-add-user')]//input[@value='Add']";

		#endregion
	}
}