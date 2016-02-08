using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class CompleteRegistrationDialog : BaseObject, IAbstractPage<CompleteRegistrationDialog>
	{
		public WebDriver Driver { get; protected set; }

		public CompleteRegistrationDialog(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public new CompleteRegistrationDialog GetPage()
		{
			InitPage(this, Driver);

			return this;
		}

		public void LoadPage()
		{
			if (!IsCompleteRegistrationDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылась последняя страница регистрации.");
			}
		}
		
		#region Методы, проверяющие состояние страницы
		
		/// <summary>
		/// Проверить, что открылась последняя страница регистрации
		/// </summary>
		public bool IsCompleteRegistrationDialogOpened()
		{
			return IsThanksMessageDisplayed();
		}

		/// <summary>
		/// Проверить, что появилось сообщение о окончании регистраци.
		/// </summary>
		public bool IsThanksMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение о окончании регистраци.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(THANKS_MESSAGE));
		}

		#endregion

		#region Описание XPath элементов

		protected const string THANKS_MESSAGE = "//p[contains(text(),'Thanks for the registration')]";

		#endregion
	}
}
