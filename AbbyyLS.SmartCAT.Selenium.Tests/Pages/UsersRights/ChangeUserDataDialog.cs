using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class ChangeUserDataDialog: UsersAndRightsBasePage, IAbstractPage<ChangeUserDataDialog>
	{
		public ChangeUserDataDialog(WebDriver driver) : base(driver)
		{
		}

		public new ChangeUserDataDialog LoadPage()
		{
			if (!IsChangeUserDataDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть диалог редактирования пользователя.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести имя пользователя
		/// </summary>
		/// <param name="name">имя</param>
		public ChangeUserDataDialog FillName(string name)
		{
			CustomTestContext.WriteLine("Ввести имя пользователя {0}.", name);
			Driver.WaitUntilElementIsClickable(By.XPath(NAME));
			Name.SetText(name);

			return LoadPage();
		}

		/// <summary>
		/// Ввести имя пользователя
		/// </summary>
		/// <param name="surname">фамилия</param>
		public ChangeUserDataDialog FillSurname(string surname)
		{
			CustomTestContext.WriteLine("Ввести фамилию пользователя {0}.", surname);
			Surname.SetText(surname);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Save.
		/// </summary>
		public UsersTab ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save.");
			SaveButton.Click();

			return new UsersTab(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Редактировать пользователя
		/// </summary>
		/// <param name="name">имя</param>
		/// <param name="surname">фамилия</param>
		public UsersTab EditUser(string name, string surname)
		{
			FillName(name);
			FillSurname(surname);
			ClickSaveButton();

			return new UsersTab(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог редактирования пользователя.
		/// </summary>
		public bool IsChangeUserDataDialogOpened()
		{
			return Driver.WaitUntilElementIsAppear(By.XPath(NAME));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = NAME)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = SURNAME)]
		protected IWebElement Surname { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string NAME = "//div[contains(@class,'popup-edit-user')]//input[@name='name']";
		protected const string SURNAME = "//div[contains(@class,'popup-edit-user')]//input[@name='surname']";
		protected const string SAVE_BUTTON = "//div[contains(@class,'popup-edit-user')]//input[@value='Save']";

		#endregion
	}
}
