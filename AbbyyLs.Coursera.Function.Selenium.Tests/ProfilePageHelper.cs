using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	public class ProfilePageHelper : CommonHelper
	{
		public ProfilePageHelper(IWebDriver driver, WebDriverWait wait) :
			base (driver, wait)
		{
		}

		/// <summary>
		/// Дождаться открытия страницы профиля пользователя
		/// </summary>
		public bool WaitUntilDisplayProfile()
		{
			if (!WaitUntilDisplayElement(By.XPath(PROFILE_DESCRIPTION_XPATH)) ||
				GetFullUserName() == "Сергей Андреев")
			{
				RefreshPage();
				if (!WaitUntilDisplayElement(By.XPath(PROFILE_DESCRIPTION_XPATH)) ||
				GetFullUserName() == "Сергей Андреев")
					return false;
				else
					return true;
			}
			else
				return true;
		}

		/// <summary>
		/// Возвращает количество переведенных предложений
		/// </summary>
		/// <returns>количество переведенных предложений</returns>
		public int GetUserTranslationsNumber()
		{
			return int.Parse(GetTextElement(By.XPath(USER_TRANSLATED_XPATH)).Trim());
		}

		/// <summary>
		/// Возвращает полное имя пользователя
		/// </summary>
		/// <returns>Полное имя пользователя</returns>
		public string GetFullUserName()
		{
			return GetTextElement(By.XPath(USER_FULL_NAME_XPATH));
		}

		/// <summary>
		/// Возвращает инфо пользователя
		/// </summary>
		/// <returns>Инфо пользователя</returns>
		public string GetInfo()
		{
			return GetTextElement(By.XPath(USER_INFO_XPATH));
		}

		/// <summary>
		/// Возвращает рейтинг пользователя
		/// </summary>
		/// <returns>Рейтинг пользователя</returns>
		public Decimal GetUserRating()
		{
			return Decimal.Parse(GetTextElement(By.XPath(USER_RATING_XPATH)).Trim().Replace(".", ","));
		}

		/// <summary>
		/// Возвращает количество голосов ЗА
		/// </summary>
		/// <returns>Количество ЗА</returns>
		public int GetVotesUp()
		{
			return int.Parse(GetTextElement(By.XPath(USER_VOTES_UP_XPATH)).Trim());
		}	
		
		/// <summary>
		/// Возвращает количество голосов ПРОТИВ
		/// </summary>
		/// <returns>Количество ПРОТИВ</returns>
		public int GetVotesDown()
		{
			return int.Parse(GetTextElement(By.XPath(USER_VOTES_DOWN_XPATH)).Trim());
		}

		/// <summary>
		/// Возвращает позицию пользователя
		/// </summary>
		/// <returns>Позиция</returns>
		public int GetPosition()
		{
			return int.Parse(GetTextElement(By.XPath(USER_POSITION_XPATH)).Trim());
		}

		/// <summary>
		/// Проверить, доступен ли аватар
		/// </summary>
		/// <returns>Аватар доступен</returns>
		public bool GetIsAvatarPresentProfile()
		{
			return GetElement(By.XPath(USER_AVATAR_XPATH)).GetAttribute("src").Contains("/avatar/");
		}
		


		protected const string PROFILE_DESCRIPTION_XPATH = ".//div[contains(@class,'profile-description')]";

		protected const string USER_FULL_NAME_XPATH = PROFILE_DESCRIPTION_XPATH + "//div[contains(@data-bind,'fullName')]";
		protected const string USER_INFO_XPATH = PROFILE_DESCRIPTION_XPATH + "//div[contains(@data-bind,'about')]";
		protected const string USER_TRANSLATED_XPATH = PROFILE_DESCRIPTION_XPATH + "//span[contains(@data-bind,'translated')]";
		protected const string USER_RATING_XPATH = PROFILE_DESCRIPTION_XPATH + "//span[contains(@data-bind,'rating')]";
		protected const string USER_VOTES_UP_XPATH = PROFILE_DESCRIPTION_XPATH + "//div[contains(@data-bind,'votesUp')]";
		protected const string USER_VOTES_DOWN_XPATH = PROFILE_DESCRIPTION_XPATH + "//div[contains(@data-bind,'votesDown')]";
		protected const string USER_POSITION_XPATH = PROFILE_DESCRIPTION_XPATH + "//span[contains(@data-bind,'position')]";
		protected const string USER_AVATAR_XPATH = PROFILE_DESCRIPTION_XPATH + "/..//img";
	}
}
