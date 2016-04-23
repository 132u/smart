using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace
{
	public class UserProfileDialog : IAbstractPage<UserProfileDialog>
	{
		public WebDriver Driver { get; protected set; }

		public UserProfileDialog(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public UserProfileDialog LoadPage()
		{
			if (!IsUserProfileDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузился диалог 'User Profile'.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести новый пароль.
		/// </summary>
		/// <param name="newPassword">новый пароль</param>
		public UserProfileDialog SetNewPassword(string newPassword)
		{
			CustomTestContext.WriteLine("Ввести новый пароль.");
			NewPasswordField.SetText(newPassword);

			return LoadPage();
		}

		/// <summary>
		/// Повторно ввести новый пароль.
		/// </summary>
		/// <param name="newPassword">новый пароль</param>
		public UserProfileDialog SetConfirmPassword(string newPassword)
		{
			CustomTestContext.WriteLine("Повторно ввести новый пароль.");
			ConfirmPasswordField.SetText(newPassword);

			return LoadPage();
		}

		/// <summary>
		/// Ввести текущий пароль.
		/// </summary>
		/// <param name="currentPassword">текущий пароль</param>
		public UserProfileDialog SetCurrentPassword(string currentPassword)
		{
			CustomTestContext.WriteLine("Ввести текущий пароль.");
			CurrentPasswordField.SetText(currentPassword);

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'сохранить'.
		/// </summary>
		public UserProfileDialog ClickSavePasswordButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'сохранить'.");
			SavePasswordButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'cancel'.
		/// </summary>
		public UserProfileDialog ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'cancel'.");
			CancellButton.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Изменить текущий пароль.
		/// </summary>
		/// <param name="newPassword">новый пароль</param>
		/// <param name="currentPassword">текущий пароль</param>
		public WorkspacePage ChangePassword(string newPassword, string currentPassword)
		{
			SetNewPassword(newPassword);
			SetConfirmPassword(newPassword);
			SetCurrentPassword(currentPassword);
			ClickSavePasswordButton();
			ClickCancelButton();

			return new WorkspacePage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsUserProfileDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(USER_PROFILE));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = USER_PROFILE)]
		protected IWebElement UserProfile { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_PASSWORD_FIELD)]
		protected IWebElement NewPasswordField { get; set; }

		[FindsBy(How = How.XPath, Using = CURRENT_PASSWORD_FIELD)]
		protected IWebElement CurrentPasswordField { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_PASSWORD_FIELD)]
		protected IWebElement ConfirmPasswordField { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_PASSWORD_BUTTON)]
		protected IWebElement SavePasswordButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCELL_BUTTON)]
		protected IWebElement CancellButton { get; set; }

		#endregion

		#region Описание Xpath элементов

		protected const string USER_PROFILE = "//h2[contains(text(), 'User Profile')]";
		protected const string CURRENT_PASSWORD_FIELD = "//div[contains(@class, 'password')]//input[contains(@class, 'currpass')]";
		protected const string CONFIRM_PASSWORD_FIELD = "//div[contains(@class, 'password')]//input[contains(@class, 'cnfrmpass')]";
		protected const string NEW_PASSWORD_FIELD = "//div[contains(@class, 'password')]//input[contains(@class, 'newpass')]";
		protected const string SAVE_PASSWORD_BUTTON = "//div[contains(@class, 'g-btn g-purplebtn js-save')]//input//parent::div";
		protected const string CANCELL_BUTTON = "//div[contains(@class, 'g-popupbox js-popupbox g-profile')]//a[contains(text(), 'Cancel')]";

		#endregion
	}
}