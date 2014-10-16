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

namespace AbbyyLS.Coursera.Function.Selenium.Tests
{
	class Rating : BaseTest
	{
		public Rating(string browserName)
			: base(browserName)
		{

		}
		
		/// <summary>
		/// Тест: проверка, что рейтинг пользователя увеличивается, если он добавляет перевод
		/// </summary>
		[Test]
		public void AddTranslationCheckRating()
		{
			// Зайти в профиль пользователя
			OpenUserProfileFromHomePage();

			// Получить рейтинг до добавления перевода
			Decimal userRatingBefore = ProfilePage.GetUserRating();

			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			AddTranslation(translationText);
			// Вернуться из редактора
			ClickHomeEditor();

			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить рейтинг после добавления перевода
			Decimal userRatingAfter = ProfilePage.GetUserRating();
			Console.WriteLine(userRatingAfter - userRatingBefore);
			// Проверить, что рейтинг увеличился
			Assert.IsTrue(userRatingAfter > userRatingBefore, "Ошибка: после добавления перевода рейтинг пользователя не увеличился");
		}

		/// <summary>
		/// Тест: проверка, что рейтинг пользователя увеличивается, если проголосовали ЗА его перевод (из редактора)
		/// </summary>
		[Test]
		public void VoteUpEditorCheckRating()
		{
			Decimal userRatingBefore, userRatingAfter;
			int numVotesUpBefore, numVotesUpAfter, numVotesDownBefore, numVotesDownAfter, translationsNumberBefore, translationsNumberAfter;
			// Добавить перевод, проголосовать, проверить информацию в профиле
			VoteCheckUserProfile(true, true, out userRatingBefore, out userRatingAfter,
								out numVotesUpBefore, out numVotesUpAfter, out numVotesDownBefore, out numVotesDownAfter,
								out translationsNumberBefore, out translationsNumberAfter);
			Console.WriteLine(userRatingAfter - userRatingBefore);
			// Проверить, что рейтинг увеличился
			Assert.IsTrue(userRatingAfter > userRatingBefore, "Ошибка: после голосования за перевод пользователя его рейтинг не вырос");
		}

		/// <summary>
		/// Тест: проверка, что рейтинг пользователя уменьшается, если проголосовали Против его перевода (из редактора)
		/// </summary>
		[Test]
		public void VoteDownEditorCheckRating()
		{
			Decimal userRatingBefore, userRatingAfter;
			int numVotesUpBefore, numVotesUpAfter, numVotesDownBefore, numVotesDownAfter, translationsNumberBefore, translationsNumberAfter;
			// Добавить перевод, проголосовать, проверить информацию в профиле
			VoteCheckUserProfile(true, false, out userRatingBefore, out userRatingAfter,
								out numVotesUpBefore, out numVotesUpAfter, out numVotesDownBefore, out numVotesDownAfter,
								out translationsNumberBefore, out translationsNumberAfter);

			Console.WriteLine(userRatingBefore);
			Console.WriteLine(userRatingAfter);
			// Проверить, что рейтинг уменьшился
			Assert.IsTrue(userRatingAfter < userRatingBefore, "Ошибка: после голосования против перевода пользователя его рейтинг не уменьшился");
		}

		/// <summary>
		/// Тест: проверка, что рейтинг пользователя увеличивается, если проголосовали ЗА его перевод (из списка последних событий)
		/// </summary>
		[Test]
		public void VoteUpEventsListCheckRating()
		{
			Decimal userRatingBefore, userRatingAfter;
			int numVotesUpBefore, numVotesUpAfter, numVotesDownBefore, numVotesDownAfter, translationsNumberBefore, translationsNumberAfter;
			// Добавить перевод, проголосовать, проверить информацию в профиле
			VoteCheckUserProfile(false, true, out userRatingBefore, out userRatingAfter,
								out numVotesUpBefore, out numVotesUpAfter, out numVotesDownBefore, out numVotesDownAfter,
								out translationsNumberBefore, out translationsNumberAfter);
			// Проверить, что рейтинг увеличился
			Assert.IsTrue(userRatingAfter > userRatingBefore, "Ошибка: после голосования за перевод пользователя его рейтинг не вырос");
		}

		/// <summary>
		/// Тест: проверка, что рейтинг пользователя уменьшается, если проголосовали Против его перевода (из списка последних событий)
		/// </summary>
		[Test]
		public void VoteDownEventsListCheckRating()
		{
			Decimal userRatingBefore, userRatingAfter;
			int numVotesUpBefore, numVotesUpAfter, numVotesDownBefore, numVotesDownAfter, translationsNumberBefore, translationsNumberAfter;
			// Добавить перевод, проголосовать, проверить информацию в профиле
			VoteCheckUserProfile(false, false, out userRatingBefore, out userRatingAfter,
								out numVotesUpBefore, out numVotesUpAfter, out numVotesDownBefore, out numVotesDownAfter,
								out translationsNumberBefore, out translationsNumberAfter);
			Console.WriteLine(userRatingBefore);
			Console.WriteLine(userRatingAfter);

			// Проверить, что рейтинг уменьшился
			Assert.IsTrue(userRatingAfter < userRatingBefore, "Ошибка: после голосования против перевода пользователя его рейтинг не уменьшился");
		}
	}
}
