using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using OpenQA.Selenium.Interactions;

namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	class Leaderboard : BaseTest
	{
		public Leaderboard(string browserName)
			: base(browserName)
		{

		}

		[SetUp]
		public void LeaderBoard()
		{
		}

		/// <summary>
		/// Тест: Проверка отображения пользователя в общем списке
		/// т.е. даже если пользователь не входит в 10ку юзеров, он все равно должен быть отображен в конце таблицы
		/// </summary>
		[Test]
		public void ExistUserMainList()
		{
			// Получить текущее имя пользователя
			string userName = HomePage.GetUserName();

			// Перейти в список лидеров
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			Header.OpenLeaderboardPage();

			// Проверить, что пользователь есть в списке
			Assert.IsTrue(GetIsUserExistLeaderboard(userName), "Ошибка: пользователя нет в списке");
		}

		/// <summary>
		/// Тест: Проверка, что место пользователя из Профиля Пользователя совпадает с местом в Лидерборде
		/// </summary>
		[Test]
		public void UserPositionLeaderboard()
		{
			// Получить текущее имя пользователя
			string userName = HomePage.GetUserName();

			// Пролистать список лидеров до пользователя
			int numInList = ScrollLeaderboardToUser(userName);

			// Имя пользователя по этому номеру:
			string userCurrentPositionName = LeaderboardPage.GetNameByRowNumber(numInList);

			Assert.AreEqual(userName, userCurrentPositionName, "Ошибка: на этом месте пользователь с другим именем");
		}

		/// <summary>
		/// Тест: Проверка, что рейтинг из Профиля Пользователя совпадает с рейтингом в Лидерборде
		/// </summary>
		[Test]
		public void UserRatingLeaderboard()
		{
			// Получить текущее имя пользователя
			string userName = HomePage.GetUserName();

			// Открыть профиль пользователя
			OpenUserProfileFromHomePage();
			// Получить рейтинг пользователя в профиле
			Decimal userRating = ProfilePage.GetUserRating();
			// Перейти к списку лидеров
			Header.OpenLeaderboardPage();
			// Получить рейтинг пользователя в лидерборде
			Decimal leaderboardRating = LeaderboardPage.GetRaitingActiveUser();
			// Сравнить рейтинг
			Assert.AreEqual(userRating, leaderboardRating, "Ошибка: рейтинг в лидерборде не совпадает с рейтингом в профиле");
		}

		/// <summary>
		/// Тест: Проверка пользователя в лидерборде для курса - пользователь есть в лидерах для курса
		/// </summary>
		[Test]
		public void CourseLeaderboardUserExist()
		{
			// Получить текущее имя пользователя
			string userName = HomePage.GetUserName();
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			ClickHomeEditor();

			// Перейти к списку лидеров
			Header.OpenLeaderboardPage();

			// Открыть курс в лидерборде
			OpenLeaderboardCourse(courseName);

			// Проверка, есть ли пользователь
			Assert.IsTrue(GetIsUserExistLeaderboard(userName), "Ошибка: пользователя нет в списке");
		}

		/// <summary>
		/// Тест: Проверка пользователя в лидерборде для курса - рейтинг для курса меньше общего рейтинга
		/// </summary>
		[Test]
		public void CourseLeaderboardCompareRating()
		{
			// Получить текущее имя пользователя
			string userName = HomePage.GetUserName();
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			ClickHomeEditor();
			// Перейти в список курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Открыть другой курс
			OpenAnotherCourse(courseName);
			// Открыть первую лекцию
			OpenLectureByRowNum(1);
			// Добавить перевод			
			AddTranslationByRowNum(GetEmptyTranslationRowNumber(), translationText);
			ClickHomeEditor();
			// Перейти к списку лидеров
			Header.OpenLeaderboardPage();
			// Получить рейтинг пользователя в лидерборде
			Decimal leaderboardRating = LeaderboardPage.GetRaitingActiveUser();
			// Открыть курс в лидерборде
			OpenLeaderboardCourse(courseName); //courseName
			// Получить рейтинг для курса
			Decimal courseRating = LeaderboardPage.GetRaitingActiveUser();
			// Сравнить рейтинг общий и для курса
			Assert.IsTrue(courseRating < leaderboardRating, "Ошибка: рейтинг для курса должен быть меньше общего рейтинга");
		}



		/// <summary>
		/// Вернуть, есть ли пользователь в списке лидеров
		/// </summary>
		/// <returns>есть ли пользователь в списке</returns>
		protected bool GetIsUserExistLeaderboard(string userName)
		{
			setDriverTimeoutMinimum();
			bool isExist = LeaderboardPage.GetIsUserLeaderboardDownList(userName) ||
				LeaderboardPage.GetIsUserLeaderboardActiveList(userName);
			setDriverTimeoutDefault();
			return isExist;
		}

		/// <summary>
		/// В лидерборде открыть конкретный курс
		/// </summary>
		/// <param name="courseName">название курса</param>
		private void OpenLeaderboardCourse(string courseName)
		{
			int totalNum = LeaderboardPage.GetLeadersQuantity();
			// Открыть список курсов
			LeaderboardPage.OpenCoursesList();
			// Проверка наличия курса в списке
			Assert.IsTrue(FindCourseInList(courseName), "Ошибка: курса нет в выпадающем списке");
			// Выбрать курс из списка
			LeaderboardPage.SelectCourseByName(courseName);
			// Дождаться пока подгрузятся результаты (изменится общее количество лидеров)
			Assert.IsTrue(LeaderboardPage.WaitUntilLeadersQuantityChanged(totalNum), "Ошибка: Выбранный курс не загрузился.");
		}

		/// <summary>
		/// Проверка наличия курса в списке
		/// </summary>
		/// <param name="courseName">Имя курса</param>
		/// <returns>Курс присутсвует в списке</returns>
		private bool FindCourseInList(string courseName)
		{
			bool isPresent = false;
			List<string> courseList = LeaderboardPage.GetCoursesList();
			foreach (string course in courseList)
			{
				if (course == courseName)
				{
					isPresent = true;
					break;
				}
			}
			return isPresent;
		}
	}
}
