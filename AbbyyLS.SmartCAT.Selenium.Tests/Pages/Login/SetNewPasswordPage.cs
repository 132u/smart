using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class SetNewPasswordPage : IAbstractPage<SetNewPasswordPage>
	{
		public WebDriver Driver { get; protected set; }

		public SetNewPasswordPage(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public SetNewPasswordPage LoadPage()
		{
			if (!IsNewPasswordEntryPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница для ввода нового пароля.");
			}

			return this;
		}

		public bool IsNewPasswordEntryPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CHANGE_PASSWORD));
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести новый пароль в форму восстановления пароля.
		/// </summary>
		/// <param name="password">пароль</param>
		public SetNewPasswordPage SetNewPassword(string password)
		{
			CustomTestContext.WriteLine("Ввести новый пароль в форму восстановления пароля.");
			NewPasswordField.SetText(password);

			return LoadPage();
		}
		/// <summary>
		/// Кликнуть по кнопке 'Change Password'.
		/// </summary>
		public WorkspacePage ClickChangePasswordButton()
		{
			CustomTestContext.WriteLine("Кликнуть по кнопке 'Change Password'.");
			ChangePassword.Click();

			return new WorkspacePage(Driver).LoadPage();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CHANGE_PASSWORD)]
		protected IWebElement ChangePassword { get; set; }

		[FindsBy(How = How.XPath, Using = NEW_PASSWORD_FIELD)]
		protected IWebElement NewPasswordField { get; set; }

		#endregion

		#region Описание Xpath элементов

		protected const string CHANGE_PASSWORD = "//button[contains(text(), 'Change password')]";
		protected const string NEW_PASSWORD_FIELD = "//input[contains(@type, 'password')]";

		#endregion
	}
}