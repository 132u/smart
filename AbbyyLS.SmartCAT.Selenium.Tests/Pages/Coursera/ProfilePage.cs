using System;
using System.Globalization;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class ProfilePage : HeaderMenu, IAbstractPage<ProfilePage>
	{
		public WebDriver Driver { get; protected set; }

		public ProfilePage(WebDriver driver): base(driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public ProfilePage GetPage()
		{
			var profilePage = new ProfilePage(Driver);
			InitPage(profilePage, Driver);

			return profilePage;
		}

		public void LoadPage()
		{
			if (!IsProfilePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница личного кабинета.");
			}
		}

		#region Простые методы страницы

		#endregion

		#region Составные методы страницы

		#endregion
		
		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась страница лидеров.
		/// </summary>
		private bool IsProfilePageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(PROFILE_TITLE));
		}

		#endregion
		
		#region Вспомогательные методы

		/// <summary>
		/// Получить номер позиции пользователя в списке лидеров.
		/// </summary>
		public int GetUserProfilePositionNumber()
		{
			CustomTestContext.WriteLine("Получить номер позиции пользователя в профиле.");
			var position = UserPosition.Text;
			int positionNubmer;

			if (!int.TryParse(position, out positionNubmer))
			{
				throw new Exception(string.Format("Произошла ошибка:\nНе удалось преобразование номера позиции {0} пользователя в число.", position));
			}

			return positionNubmer;
		}

		/// <summary>
		/// Получить рейтинг пользователя в профиле.
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		public double GetUserProfileRating()
		{
			CustomTestContext.WriteLine("Получить рейтинг пользователя в профиле.");
			var position = UserRating.Text;
			double positionNubmer;

			if (!double.TryParse(position, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out positionNubmer))
			{
				throw new Exception(string.Format("Произошла ошибка:\nНе удалось преобразование рейтинга {0} пользователя в профиле.", position));
			}

			return positionNubmer;
		}

		#endregion
		
		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = USER_POSITION)]
		protected IWebElement UserPosition { get; set; }

		[FindsBy(How = How.XPath, Using = USER_RATING)]
		protected IWebElement UserRating { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string EMAIL = ".//input[@name='email']";
		protected const string USER_POSITION = ".//div[contains(@class,'profile-description')]//span[contains(@data-bind,'position')]";
		protected const string USER_RATING = ".//div[contains(@class,'profile-description')]//span[contains(@data-bind,'rating')]";
		protected const string PROFILE_TITLE = "//div[@class='profile-title' and contains(@data-bind, 'fullName')]";

		#endregion
	}
}
