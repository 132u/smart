using System;
using System.Globalization;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	public class UserProfilePage : HeaderMenu, IAbstractPage<UserProfilePage>
	{
		public UserProfilePage(WebDriver driver): base(driver)
		{
		}

		public new UserProfilePage LoadPage()
		{
			if (!IsUserProfilePageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница личного кабинета.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку редактирования профиля.
		/// </summary>
		public EditProfileDialog ClickEditProfileButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку редактирования профиля.");
			EditProfileButton.Click();

			return new EditProfileDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Получить имя пользователя в профиле
		/// </summary>
		/// <returns>имя пользователя</returns>
		public string GetNickname()
		{
			CustomTestContext.WriteLine("Получить имя пользователя в профиле.");

			return Nickname.Text;
		}

		/// <summary>
		/// Получить информацию о пользователе.
		/// </summary>
		/// <returns>информация о пользователе</returns>
		public string GetAboutMeInformation()
		{
			CustomTestContext.WriteLine("Получить информацию о пользователе.");

			return AboutMe.Text;
		}

		#endregion
		
		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что фото пользователя отображается в пофиле.
		/// </summary>
		public new bool IsUserPhotoDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что фото пользователя отображается в профиле.");

			return UserPhoto.GetAttribute("src").Contains("/avatar/");
		}

		/// <summary>
		/// Проверить, что открылась страница профиля.
		/// </summary>
		public bool IsUserProfilePageOpened()
		{
			return IsEditProfileDialogDisappeared() && Driver.WaitUntilElementIsDisplay(By.XPath(ABOUT_MYSELF_TITLE));
		}
		
		#endregion
		
		#region Вспомогательные методы

		/// <summary>
		/// Получить номер позиции пользователя в профиле.
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

		/// <summary>
		/// Получить количество переведенных предложений пользователя в профиле.
		/// </summary>
		public int GetTranslatedSentencesCount()
		{
			CustomTestContext.WriteLine("Получить количество переведенных предложений пользователя в профиле.");
			var sentencesCount = SentencesQuantity.Text;
			int sentencesCountNumber;

			if (!int.TryParse(sentencesCount, out sentencesCountNumber))
			{
				throw new Exception(string.Format("Произошла ошибка:\nНе удалось преобразование количества переведенных предложений {0} пользователя в число.", sentencesCount));
			}

			return sentencesCountNumber;
		}

		/// <summary>
		/// Получить количество положительных голосов пользователя в профиле.
		/// </summary>
		public int GetVotesUpCount()
		{
			CustomTestContext.WriteLine("Получить количество положительных голосов пользователя в профиле.");
			var votes = VotesUp.Text;
			int votesCount;

			if (!int.TryParse(votes, out votesCount))
			{
				throw new Exception(string.Format("Произошла ошибка:\nНе удалось преобразование количества положительных голосов {0} пользователя в число.", votes));
			}

			return votesCount;
		}

		/// <summary>
		/// Получить количество отрицательных голосов пользователя в профиле.
		/// </summary>
		public int GetVotesDownCount()
		{
			CustomTestContext.WriteLine("Получить количество отрицательных голосов пользователя в профиле.");
			var votes = VotesDown.Text;
			int votesCount;

			if (!int.TryParse(votes, out votesCount))
			{
				throw new Exception(string.Format("Произошла ошибка:\nНе удалось преобразование количества отрицательных голосов {0} пользователя в число.", votes));
			}

			return votesCount;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = USER_POSITION)]
		protected IWebElement UserPosition { get; set; }

		[FindsBy(How = How.XPath, Using = USER_RATING)]
		protected IWebElement UserRating { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_PROFILE_BUTTON)]
		protected IWebElement EditProfileButton { get; set; }

		[FindsBy(How = How.XPath, Using = NICKNAME)]
		protected IWebElement Nickname { get; set; }

		[FindsBy(How = How.XPath, Using = USER_PHOTO)]
		protected IWebElement UserPhoto { get; set; }

		[FindsBy(How = How.XPath, Using = ABOUT_ME)]
		protected IWebElement AboutMe { get; set; }

		[FindsBy(How = How.XPath, Using = ABOUT_MYSELF_TITLE)]
		protected IWebElement AboutMyselfTitle { get; set; }

		[FindsBy(How = How.XPath, Using = SENTENCES_QUANTITY)]
		protected IWebElement SentencesQuantity { get; set; }

		[FindsBy(How = How.XPath, Using = VOTES_UP)]
		protected IWebElement VotesUp { get; set; }

		[FindsBy(How = How.XPath, Using = VOTES_DOWN)]
		protected IWebElement VotesDown { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string USER_POSITION = ".//div[contains(@class,'profile-description')]//span[contains(@data-bind,'position')]";
		protected const string USER_RATING = ".//div[contains(@class,'profile-description')]//span[contains(@data-bind,'rating')]";
		protected const string NICKNAME = "//div[@class='profile-title' and contains(@data-bind, 'fullName')]";
		protected const string EDIT_PROFILE_BUTTON = ".//button[contains(@data-popup,'editor-form')]";
		protected const string USER_PHOTO = ".//div[contains(@class,'profile-description')]/..//img";
		protected const string ABOUT_ME = "//div[@data-bind='text: about']";
		protected const string ABOUT_MYSELF_TITLE = "//div[@class='profile-description']//span[@class='profile-about-title' and contains(text(),'myself')]";
		protected const string CHANGE_PASSWORD_TAB = "//label[contains(@data-bind, 'changePassword().hasPassword()')]";
		protected const string SENTENCES_QUANTITY = ".//div[contains(@class,'profile-description')]//span[contains(@data-bind,'translated')]";
		protected const string VOTES_UP = "//div[contains(@data-bind,'votesUp')]";
		protected const string VOTES_DOWN = "//div[contains(@data-bind,'votesDown')]";

		#endregion
	}
}
