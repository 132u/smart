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
	class Achievements : BaseTest
	{
		public Achievements(string browserName)
			: base(browserName)
		{
		}

		// Тесты должны проходить только в определенном порядке, поэтому названия с привязкой к алфавиту

		/// <summary>
		/// Тест: получение награды "Переводчик. 1 Уровень" (осуществить 5 переводов)
		/// </summary>
		//[Test]
		public void a_1_TranslatorLevel1()
		{
			// Уровень
			int achieveLevel = 1;
			// Количество переводов для получения уровня 
			int levelLimit = 5;
			// Добавить нужное количество переводов
			GetTranslatorLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Переводчик. 2 Уровень" (осуществить 50 переводов)
		/// </summary>
		//[Test]
		public void a_1_TranslatorLevel2()
		{
			// Уровень
			int achieveLevel = 2;
			// Количество переводов для получения уровня 
			int levelLimit = 50;
			// Добавить нужное количество переводов
			GetTranslatorLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Переводчик. 3 Уровень" (осуществить 250 переводов)
		/// </summary>
		//[Test]
		public void a_1_TranslatorLevel3()
		{
			// Уровень
			int achieveLevel = 3;
			// Количество переводов для получения уровня 
			int levelLimit = 250;
			// Добавить нужное количество переводов
			GetTranslatorLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Переводчик. 4 Уровень" (осуществить 750 переводов)
		/// </summary>
		//[Test]
		public void a_1_TranslatorLevel4()
		{
			// Уровень
			int achieveLevel = 4;
			// Количество переводов для получения уровня 
			int levelLimit = 750;
			// Добавить нужное количество переводов
			GetTranslatorLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Переводчик. 5 Уровень" (осуществить 1500 переводов)
		/// </summary>
		//[Test]
		public void a_1_TranslatorLevel5()
		{
			// Уровень
			int achieveLevel = 5;
			// Количество переводов для получения уровня 
			int levelLimit = 1500;
			// Добавить нужное количество переводов
			GetTranslatorLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Эксперт. 1 Уровень" (проголосовать за 5 переводов) - голосование За
		/// </summary>
		[Test]
		public void b_1_ExpertLevel1VotesUp()
		{
			// Уровень
			int achieveLevel = 1;
			// Количество голосов для получения уровня 
			int levelLimit = 5;
			// Добавить нужное количество голосов
			GetExpertLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Эксперт. 1 Уровень" (проголосовать за 5 переводов) - голосование Против
		/// </summary>
		[Test]
		public void b_2_ExpertLevel1VotesDown()
		{
			// Уровень
			int achieveLevel = 1;
			// Количество голосов для получения уровня 
			int levelLimit = 5;
			// Добавить нужное количество голосов
			GetExpertLevel(achieveLevel, levelLimit, false);
		}

		/// <summary>
		/// Тест: получение награды "Эксперт. 2 Уровень" (проголосовать за 50 переводов)
		/// </summary>
		[Test]
		public void b_1_ExpertLevel2()
		{
			// Уровень
			int achieveLevel = 2;
			// Количество голосов для получения уровня 
			int levelLimit = 50;
			// Добавить нужное количество голосов
			GetExpertLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Эксперт. 3 Уровень" (проголосовать за 250 переводов)
		/// </summary>
		[Test]
		public void b_1_ExpertLevel3()
		{
			// Уровень
			int achieveLevel = 3;
			// Количество голосов
			int levelLimit = 250;
			// Добавить нужное количество голосов
			GetExpertLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Эксперт. 4 Уровень" (проголосовать за 750 переводов)
		/// </summary>
		[Test]
		public void b_1_ExpertLevel4()
		{
			// Уровень
			int achieveLevel = 4;
			// Количество голосов
			int levelLimit = 750;
			// Добавить нужное количество голосов
			GetExpertLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Эксперт. 5 Уровень" (проголосовать за 1500 переводов)
		/// </summary>
		[Test]
		public void b_1_ExpertLevel5()
		{
			// Уровень
			int achieveLevel = 5;
			// Количество голосов
			int levelLimit = 1500;
			// Добавить нужное количество голосов
			GetExpertLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: награда Эксперт - проверка изменения прогресса при голосовании из виджета голосования
		/// </summary>
		[Test]
		public void b_3_ExpertProgressTest()
		{
			string achieveType = "Эксперт";
			// Выбрать пользователя, у которого отображается прогресс (не 5 уровень)
			SelectUserWithoutAchieveLevel(achieveType, 5);

			bool isOk = true;
			string errorMessage = "\n";

			// Прогресс до голосования
			int achieveProgressBefore = GetAchieveProgressProfile(achieveType);
			// Проголосовать За из виджета
			VoteFromWidget("vwVoteUp");
			// Прогресс после голосования
			int achieveProgressAfter = GetAchieveProgressProfile(achieveType);

			// Проверка прогресса
			if (achieveProgressAfter <= achieveProgressBefore)
			{
				isOk = false;
				errorMessage += "Ошибка: после голосования За прогресс Эксперта не увеличился\n";
			}

			// Прогресс до голосования
			achieveProgressBefore = achieveProgressAfter;
			// Проголосовать Против из виджета
			VoteFromWidget("vwVoteDown");
			// Прогресс после голосования
			achieveProgressAfter = GetAchieveProgressProfile(achieveType);

			// Проверка прогресса
			if (achieveProgressAfter <= achieveProgressBefore)
			{
				isOk = false;
				errorMessage += "Ошибка: после голосования Против прогресс Эксперта не увеличился\n";
			}

			// Прогресс до голосования
			achieveProgressBefore = achieveProgressAfter;
			// Нажать Пропустить
			VoteFromWidget("vwSkip");
			// Прогресс после голосования
			achieveProgressAfter = GetAchieveProgressProfile(achieveType);

			// Проверка прогресса
			if (achieveProgressAfter != achieveProgressBefore)
			{
				isOk = false;
				errorMessage += "Ошибка: после Пропустить изменился прогресс\n";
			}

			// Вывести ошибки
			Assert.IsTrue(isOk, errorMessage);
		}

		/// <summary>
		/// Тест: получение награды "Профессионал. 1 уровень" (получить 3 голоса за перевод)
		/// </summary>
		[Test]
		public void f_1_ProfessionalLevel1()
		{
			int achieveLevel = 1;
			// Сколько голосов нужно получить
			int levelLimit = 3;
			// Получить количество голосов
			GetProfessionalLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Профессионал. 2 уровень" (получить 7 голосов за перевод)
		/// </summary>
		[Test]
		public void f_1_ProfessionalLevel2()
		{
			int achieveLevel = 2;
			// Сколько голосов нужно получить
			int levelLimit = 7;
			// Получить количество голосов
			GetProfessionalLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Профессионал. 3 уровень" (получить 15 голосов за перевод)
		/// </summary>
		[Test]
		public void f_1_ProfessionalLevel3()
		{
			int achieveLevel = 3;
			// Сколько голосов нужно получить
			int levelLimit = 15;
			// Получить количество голосов
			GetProfessionalLevel(achieveLevel, levelLimit);
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 1 уровень. Model Thinking" (перевести 1% курса)
		/// </summary>
		[Test]
		public void e_1_SpecialistLevel1_Model()
		{
			int achieveLevel = 1;
			// Сколько процентов нужно заполнить
			int levelLimit = 1;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Модель мышления");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 2 уровень. Model Thinking" (перевести 10% курса)
		/// </summary>
		[Test]
		public void e_1_SpecialistLevel2_Model()
		{
			int achieveLevel = 2;
			// Сколько процентов нужно заполнить
			int levelLimit = 10;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Модель мышления");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 3 уровень. Model Thinking" (перевести 30% курса)
		/// </summary>
		[Test]
		public void e_1_SpecialistLevel3_Model()
		{
			int achieveLevel = 3;
			// Сколько процентов нужно заполнить
			int levelLimit = 30;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Модель мышления");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 4 уровень. Model Thinking" (перевести 60% курса)
		/// </summary>
		[Test]
		public void e_1_SpecialistLevel4_Model()
		{
			int achieveLevel = 4;
			// Сколько процентов нужно заполнить
			int levelLimit = 60;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Модель мышления");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 5 уровень. Model Thinking" (перевести 100% курса)
		/// </summary>
		[Test]
		public void e_1_SpecialistLevel5_Model()
		{
			int achieveLevel = 5;
			// Сколько процентов нужно заполнить
			int levelLimit = 100;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Модель мышления");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 1 уровень. The Emergence of the Modern Middle East" (перевести 1% курса)
		/// </summary>
		[Test]
		public void e_2_SpecialistLevel1_East()
		{
			int achieveLevel = 1;
			// Сколько процентов нужно заполнить
			int levelLimit = 1;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Возникновение современного Ближнего Востока");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 2 уровень. The Emergence of the Modern Middle East" (перевести 10% курса)
		/// </summary>
		[Test]
		public void e_2_SpecialistLevel2_East()
		{
			int achieveLevel = 2;
			// Сколько процентов нужно заполнить
			int levelLimit = 10;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Возникновение современного Ближнего Востока");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 3 уровень. The Emergence of the Modern Middle East" (перевести 30% курса)
		/// </summary>
		[Test]
		public void e_2_SpecialistLevel3_East()
		{
			int achieveLevel = 3;
			// Сколько процентов нужно заполнить
			int levelLimit = 30;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Возникновение современного Ближнего Востока");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 4 уровень. The Emergence of the Modern Middle East" (перевести 60% курса)
		/// </summary>
		[Test]
		public void e_2_SpecialistLevel4_East()
		{
			int achieveLevel = 4;
			// Сколько процентов нужно заполнить
			int levelLimit = 60;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Возникновение современного Ближнего Востока");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 5 уровень. The Emergence of the Modern Middle East" (перевести 100% курса)
		/// </summary>
		[Test]
		public void e_2_SpecialistLevel5_East()
		{
			int achieveLevel = 5;
			// Сколько процентов нужно заполнить
			int levelLimit = 100;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Возникновение современного Ближнего Востока");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 1 уровень. Cryptography" (перевести 1% курса)
		/// </summary>
		[Test]
		public void e_3_SpecialistLevel1_Cryptography()
		{
			int achieveLevel = 1;
			// Сколько процентов нужно заполнить
			int levelLimit = 1;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Криптография");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 2 уровень. Cryptography" (перевести 10% курса)
		/// </summary>
		[Test]
		public void e_3_SpecialistLevel2_Cryptography()
		{
			int achieveLevel = 2;
			// Сколько процентов нужно заполнить
			int levelLimit = 10;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Криптография");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 3 уровень. Cryptography" (перевести 30% курса)
		/// </summary>
		[Test]
		public void e_3_SpecialistLevel3_Cryptography()
		{
			int achieveLevel = 3;
			// Сколько процентов нужно заполнить
			int levelLimit = 30;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Криптография");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 4 уровень. Cryptography" (перевести 60% курса)
		/// </summary>
		[Test]
		public void e_3_SpecialistLevel4_Cryptography()
		{
			int achieveLevel = 4;
			// Сколько процентов нужно заполнить
			int levelLimit = 60;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Криптография");
		}

		/// <summary>
		/// Тест: получение награды "Специалист. 5 уровень. Cryptography" (перевести 100% курса)
		/// </summary>
		[Test]
		public void e_3_SpecialistLevel5_Cryptography()
		{
			int achieveLevel = 5;
			// Сколько процентов нужно заполнить
			int levelLimit = 100;
			// Получить награду
			GetSpecialistLevel(achieveLevel, levelLimit, "Криптография");
		}

		/// <summary>
		/// Тест: получение награды Лидер онлайн
		/// </summary>
		[Test]
		public void c_1_LeaderListOnlineTest()
		{
			int needUserPosition = 10;
			// Добраться до нужного места в лидерборде
			GetPositionLeaderboardOnline(needUserPosition, "Лидер");
		}

		/// <summary>
		/// Тест: получение награды Лидер оффлайн
		/// </summary>
		[Test]
		public void c_2_LeaderListOfflineTest()
		{
			int needUserPosition = 10;
			// Добраться до нужного места в лидерборде
			GetPositionLeaderboardOffline(needUserPosition, "Лидер");
		}

		/// <summary>
		/// Тест: получение награды Лидер оффлайн (с 11 до 9 места)
		/// </summary>
		[Test]
		public void c_3_LeaderListOfflinePosition9Test()
		{
			int needUserPosition = 10;
			// Добраться до нужного места в лидерборде
			GetPositionLeaderboardOffline(needUserPosition, "Лидер", true);
		}

		/// <summary>
		/// Тест: получение награды Номер1 онлайн
		/// </summary>
		[Test]
		public void d_1_FirstPlaceOnlineTest()
		{
			int needUserPosition = 1;
			// Добраться до нужного места в лидерборде
			GetPositionLeaderboardOnline(needUserPosition, "Номер один");
		}

		/// <summary>
		/// Тест: получение награды Номер1 оффлайн
		/// </summary>
		[Test]
		public void d_2_FirstPlaceOfflineTest()
		{
			int needUserPosition = 1;
			// Добраться до нужного места в лидерборде
			GetPositionLeaderboardOffline(needUserPosition, "Номер один");
		}

		/// <summary>
		/// Проверить получение и потерю награды Переводчик
		/// </summary>
		/// <param name="achieveLevel">уровень награды</param>
		/// <param name="levelLimit">количество переводов для награды</param>
		protected void GetTranslatorLevel(int achieveLevel, int levelLimit)
		{
			string achieveType = "Переводчик";
			SelectUserWithoutAchieveLevel(achieveType, achieveLevel);

			bool isTestOk = true;
			string testErrorMessage = "\n";
			int translationsNumberLeft = levelLimit - GetAchieveProgressProfile(achieveType);
			int lectureRowNumber = 1;

			// Перейти к списку доступных курсов
			OpenCoursePage();
			// Переход в курс с наименьшим прогрессом
			string courseName = OpenCourseMinProgress();
			Console.WriteLine("курс: " + courseName);

			if (translationsNumberLeft - 1 > 0)
			{
				// Добавить предложения до N-1
				AddTranslationsCourse(translationsNumberLeft - 1, ref lectureRowNumber);
				// Проверить, что нет сообщения о награде
				CheckAchieveMessages(achieveType, achieveLevel, false, ref isTestOk, ref testErrorMessage);
			}

			// Добавить перевод
			AddTranslationsCourse(1, ref lectureRowNumber);
			// Проверить сообщение
			CheckAchieveMessages(achieveType, achieveLevel, true, ref isTestOk, ref testErrorMessage);

			// Проверить прогресс в профиле
			OpenUserProfileFromCourse();
			int newAchieveProgress = GetAchieveProgressProfile(achieveType);
			int newAchieveLevel = GetAchieveLevelProfile(achieveType);
			// проверка уровня награды
			if (newAchieveLevel != achieveLevel)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный уровень: " + newAchieveLevel + "\n";
			}
			// проверка прогресса
			if (newAchieveProgress != levelLimit)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный прогресс: " + newAchieveProgress + "\n";
			}

			// Удалить два перевода
			OpenCoursePage();
			OpenCourseByName(courseName);
			OpenLectureByRowNum(1);
			DeleteMyTranslations(1, 2);
			ClickHomeEditor();

			// Проверить, что уровень и прогресс в профиле не изменились
			OpenUserProfileFromCourse();
			newAchieveProgress = GetAchieveProgressProfile(achieveType);
			newAchieveLevel = GetAchieveLevelProfile(achieveType);
			// проверка уровня награды
			if (newAchieveLevel != achieveLevel)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после удаления переводов в профиле указывается неправильный уровень: " + newAchieveLevel + "\n";
			}
			// проверка прогресса
			if (newAchieveProgress != levelLimit)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после удаления переводов в профиле указывается неправильный прогресс: " + newAchieveProgress + "\n";
			}

			// Добавить перевод вместо удаленного
			OpenCoursePage();
			OpenCourseByName(courseName);
			OpenLectureByRowNum(1);
			AddTranslationByRowNum(1, "Test Text");
			ClickHomeEditor();

			// Проверить, что уровень и прогресс в профиле не изменились
			OpenUserProfileFromCourse();
			newAchieveProgress = GetAchieveProgressProfile(achieveType);
			newAchieveLevel = GetAchieveLevelProfile(achieveType);
			// проверка уровня награды
			if (newAchieveLevel != achieveLevel)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после добавления перевода в удаленный в профиле указывается неправильный уровень: " + newAchieveLevel + "\n";
			}
			// проверка прогресса
			if (newAchieveProgress != levelLimit)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после добавления перевода в удаленный в профиле указывается неправильный прогресс: " + newAchieveProgress + "\n";
			}

			// Добавить новый перевод
			OpenCoursePage();
			OpenCourseByName(courseName);
			++lectureRowNumber;
			AddTranslationsCourse(1, ref lectureRowNumber);
			// Проверить, что нет сообщения
			CheckAchieveMessages(achieveType, achieveLevel, false, ref isTestOk, ref testErrorMessage);

			// Проверить профиль
			OpenUserProfileFromCourse();
			newAchieveProgress = GetAchieveProgressProfile(achieveType);
			newAchieveLevel = GetAchieveLevelProfile(achieveType);
			// уровень должен остаться
			if (newAchieveLevel != achieveLevel)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после добавления нового перевода в профиле указывается неправильный уровень: " + newAchieveLevel + "\n";
			}
			// прогресс должен увеличиться на 1
			if (newAchieveProgress != (levelLimit + 1))
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после добавления нового перевода в профиле указывается неправильный прогресс: " + newAchieveProgress + "\n";
			}

			// Вывести ошибки:
			Assert.IsTrue(isTestOk, testErrorMessage);
		}

		/// <summary>
		/// Выбрать пользователя с уровнем награды ниже
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <param name="achieveLevel">уровень награды, который будет зарабатывать пользователь</param>
		/// <returns></returns>
		protected int SelectUserWithoutAchieveLevel(string achieveType, int achieveLevel, string specialistCourseName = "")
		{
			bool isNeedChangeUser = true;
			// -1 - это пользователь Ян (он же Bob)
			// с 0 по 15 - это тестовые пользователи

			int userIndex = -1;
			for (int i = 0; i < TestUserList.Count; ++i)
			{
				// Открыть профиль пользователя
				OpenUserProfileFromHomePage();
				// Текущий уровень награды
				int currentLevel = GetAchieveLevelProfile(achieveType, specialistCourseName);
				isNeedChangeUser = currentLevel >= achieveLevel;

				if (isNeedChangeUser)
				{
					LogoutUser();
					LoginUser(TestUserList[i]);
				}
				else
				{
					userIndex = i - 1;
					break;
				}
			}

			Assert.IsFalse(isNeedChangeUser, "Проблема: все пользователи исчерпаны");
			return userIndex;
		}

		/// <summary>
		/// Выбрать пользователя, у которого нет нужной награды
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <returns>индекс пользователя</returns>
		protected int SelectUserWithoutAchieve(string achieveType)
		{
			bool isNeedChangeUser = true;
			// -1 - это пользователь Ян (он же Bob)
			// с 0 по 15 - это тестовые пользователи

			int userIndex = -1;
			for (int i = 0; i < TestUserList.Count; ++i)
			{
				// Открыть профиль пользователя
				OpenUserProfileFromHomePage();
				// Проверить, получена ли у пользователя награда
				isNeedChangeUser = GetIsAchieveReceivedProfile(achieveType);

				if (isNeedChangeUser)
				{
					LogoutUser();
					LoginUser(TestUserList[i]);
				}
				else
				{
					userIndex = i - 1;
					break;
				}
			}

			Assert.IsFalse(isNeedChangeUser, "Проблема: все пользователи исчерпаны");
			return userIndex;
		}

		/// <summary>
		/// Получить прогресс награды в профиле
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <returns>прогресс</returns>
		protected int GetAchieveProgressProfile(string achieveType)
		{
			string achieveText = Driver.FindElement(By.XPath(
				".//ul[@class='achive-list']//li[" + GetAchieveNumInList(achieveType) + "]//small[contains(@data-bind,'progress')]")).Text.Trim();
			// Прогресс
			int startIndex = achieveText.IndexOf(" ") + 1;
			int achieveProgress = int.Parse(achieveText.Substring(startIndex, achieveText.IndexOf("/") - startIndex));
			Console.WriteLine("прогресс: " + achieveProgress);
			return achieveProgress;
		}

		/// <summary>
		/// Добавить нужное количество переводов в курс
		/// </summary>
		/// <param name="translationsNumberLeft">оставшееся количество переводов для добавления</param>
		/// <param name="startLectureRowNumber">номер лекции, с которой начать добавление</param>
		protected void AddTranslationsCourse(int translationsNumberLeft, ref int startLectureRowNumber)
		{
			while (translationsNumberLeft > 0)
			{
				// Перейти в лекцию
				startLectureRowNumber = SelectLectureToTranslate(startLectureRowNumber);
				Console.WriteLine("лекция номер " + startLectureRowNumber);
				OpenLectureByRowNum(startLectureRowNumber);

				int lastSentenceNumber = 0, translatedNumber = 0;
				bool isFinished = false;

				// Заполнить лекцию
				do
				{
					// Заполнить пустые сегменты (где нет переводов)
					isFinished = AddTranslationsEmptyVisibleSegments(ref lastSentenceNumber, translationsNumberLeft, out translatedNumber);
					translationsNumberLeft -= translatedNumber;
					Console.WriteLine("translationsNumberLimit после: " + translationsNumberLeft);
				} while (translationsNumberLeft > 0 && !isFinished);

				if (isFinished)
				{
					++startLectureRowNumber;
				}
			}
		}

		/// <summary>
		/// Выбрать лекцию для перевода (личный прогресс меньше 100)
		/// </summary>
		/// <param name="startRowNumber">начальный номер лекции для открытия</param>
		/// <returns>номер строки с лекцией</returns>
		protected int SelectLectureToTranslate(int startRowNumber = 1)
		{
			// Получить проценты личного прогресса всех лекций
			IList<IWebElement> percentElements = Driver.FindElements(By.XPath(".//div[contains(@data-bind,'personalProgressView')]"));
			// Проверить, что список лекций не пуст
			Assert.IsTrue(percentElements.Count > 0, "Ошибка: список лекций пуст");

			bool isLectureExist = false;
			int personalProgress = 0;
			int rowNumber = 0;
			startRowNumber = startRowNumber < 1 ? 1 : startRowNumber;
			for (int i = (startRowNumber - 1); i < percentElements.Count; ++i)
			{
				// Выбираем лекцию с личным прогрессом < 100
				personalProgress = int.Parse(percentElements[i].Text.Replace("%", ""));
				if (personalProgress < 100)
				{
					isLectureExist = true;
					rowNumber = i + 1;
					break;
				}
			}
			Assert.IsTrue(isLectureExist, "Ошибка: нет подходящей лекции (с общим и личным прогрессом == 0");

			// Вернуть номер строки с лекцией
			return rowNumber;
		}

		/// <summary>
		/// Добавить переводы в пустые сегменты лекции
		/// </summary>
		/// <param name="lastLastFactRow">IN/OUT: последний номер в строке с последним сегментом при предыдущем проходе редактора</param>
		/// <param name="translationNumberLimit">IN: максимальное количество переводов для добавления</param>
		/// <param name="translatedNumber">OUT: количество добавленных переводов</param>
		/// <returns>закончилась ли лекция (true: пустые предложения закончились)</returns>
		protected bool AddTranslationsEmptyVisibleSegments(ref int lastLastFactRow, int translationNumberLimit, out int translatedNumber)
		{
			bool isLectureFinished = false;
			int startIndex = 0;
			// Список видимых сегментов
			IList<IWebElement> segmentsList = GetVisibleSegmentList(ref lastLastFactRow, out isLectureFinished, out startIndex);

			// Сбрасываем счетчик переведенных предложений
			translatedNumber = 0;
			string translationText = "Test" + DateTime.Now.Ticks;

			if (!isLectureFinished)
			{
				for (int i = startIndex; i < segmentsList.Count; ++i)
				{
					segmentsList[i].Click();
					segmentsList[i].Click();
					if (segmentsList[i].Text.Trim().Length == 0)
					{
						segmentsList[i].SendKeys(translationText);
						// Кликнуть по галочке с Confirm в строке сегмента
						Driver.FindElement(By.XPath(".//div[@id='segments-body']//span[contains(@class,'fa-check')]")).Click();
						WaitUntilDisappearElement(".//div[@id='segments-body']//span[contains(@class,'fa-border')]", 20);

						// Проверить, что перевод появился в предложенных переводах
						int translationRowNumber = GetSuggestedTranslationRowNum(translationText);
						if (translationRowNumber > 0)
						{
							// Если появился - перевод принят, увеличиваем счетчик
							++translatedNumber;
							Console.WriteLine("осталось перевести: " + (translationNumberLimit - translatedNumber));

							if (translatedNumber >= translationNumberLimit)
							{
								Console.WriteLine("достаточно, выходим из лекции");
								break;
							}
						}
					}
				}
			}

			// Закончилась ли лекция
			return isLectureFinished;
		}

		/// <summary>
		/// Проверить сообщение о получении награды
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <param name="achieveLevel">уровень награды</param>
		/// <param name="messageShouldBeVisible">должно ли быть сообщение</param>
		/// <param name="isTestOk">IN/OUT: результат теста</param>
		/// <param name="testErrorMessage">IN/OUT: сообщение с результатом теста</param>
		protected void CheckAchieveMessages(string achieveType, int achieveLevel, bool messageShouldBeVisible,
							ref bool isTestOk, ref string testErrorMessage)
		{
			// Проверить, есть ли сообщение в редакторе
			bool isExistEditorMessage = GetIsExistAchieveMessageEditor(achieveType, true, achieveLevel);
			ClickHomeEditor();
			// Проверить, есть ли сообщение в лекции
			bool isExistLectureMessage = GetIsExistAchieveMessageLecture(achieveType, true, achieveLevel);

			// Проверка ошибок
			if (isExistEditorMessage != messageShouldBeVisible)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: в редакторе " + (isExistEditorMessage ? "" : "не ") + "появилось оповещение о награде\n";
			}
			if (isExistLectureMessage)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: в лекции появилось оповещение\n";
			}
		}

		/// <summary>
		/// Получить: есть ли сообщение о присуждении награды в редакторе
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <param name="needCheckLevel">нужна ли проверка уровня награды</param>
		/// <param name="achieveLevel">уровень награды для проверки</param>
		/// <returns>есть сообщение</returns>
		protected bool GetIsExistAchieveMessageEditor(string achieveType, bool needCheckLevel = false, int achieveLevel = 1)
		{
			bool isAchieveMessage = false;
			bool needCheckNewMessage = true;
			setDriverTimeoutWait();
			while (needCheckNewMessage)
			{
				isAchieveMessage = IsElementDisplayed(By.XPath(".//div[contains(@class,'achievement')]"));
				if (isAchieveMessage)
				{
					string achieveInfo = Driver.FindElement(By.XPath(".//div[contains(@class,'achievement')]//div[contains(@class,'name')]")).Text;
					Console.WriteLine("получили награду " + achieveInfo);
					Console.WriteLine("а мы ждем тип " + achieveType);
					string curAchieveType = "";
					if (achieveInfo.Contains("("))
					{
						curAchieveType = achieveInfo.Substring(0, achieveInfo.IndexOf("(")).Trim();
					}
					else
					{
						curAchieveType = achieveInfo.Trim();
					}

					// Тип награды
					if (curAchieveType == achieveType)
					{
						Console.WriteLine(achieveType);
						if (needCheckLevel)
						{
							int startIndex = achieveInfo.IndexOf("(") + 1;
							int curAchieveLevel = int.Parse(achieveInfo.Substring(startIndex, achieveInfo.IndexOf("/") - startIndex).Trim());
							// Уровень награды
							if (achieveLevel == curAchieveLevel)
							{
								// Сообщение появилось
								Console.WriteLine(achieveLevel);
								needCheckNewMessage = false;
							}
						}
						else
						{
							needCheckNewMessage = false;
						}
					}
					MakeScreen();
					Driver.FindElement(By.XPath(".//div[contains(@class,'achievement')]//span[contains(@class,'x-btn-button-orange')]")).Click();
				}
				else
				{
					// Нет награды
					Console.WriteLine("не получили нужную награду");
					needCheckNewMessage = false;
				}
			}
			setDriverTimeoutDefault();
			return isAchieveMessage;
		}

		/// <summary>
		/// Получить: есть ли сообщение о присуждении награды на странице лекции
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <param name="needCheckLevel">нужна ли проверка уровня награды</param>
		/// <param name="achieveLevel">уровень награды для проверки</param>
		/// <returns>есть сообщение</returns>
		protected bool GetIsExistAchieveMessageLecture(string achieveType, bool needCheckLevel = false, int achieveLevel = 1)
		{
			bool isAchieveMessage = false;
			bool needUpdateMessageInfo = true;
			while (needUpdateMessageInfo)
			{
				isAchieveMessage = IsElementDisplayed(By.XPath(".//div[@id='achieve-popup']"));
				if (isAchieveMessage)
				{
					Console.WriteLine("получили награду");
					string achieveInfo = Driver.FindElement(By.XPath(".//div[@id='achieve-popup']//strong")).Text;
					string curAchieveType = achieveInfo.Substring(0, achieveInfo.IndexOf(" ")).Trim();
					Console.WriteLine("инфо: " + achieveInfo);
					Console.WriteLine("тип: " + curAchieveType);
					// Тип награды
					if (curAchieveType.Contains(achieveType))
					{
						Console.WriteLine(achieveType);
						Console.WriteLine(achieveInfo);
						if (needCheckLevel)
						{
							int indexStart = achieveInfo.IndexOf("(") + 1;
							Console.WriteLine(achieveInfo.Substring(indexStart, achieveInfo.IndexOf("/") - indexStart));
							int curAchieveLevel = int.Parse(achieveInfo.Substring(indexStart, achieveInfo.IndexOf("/") - indexStart).Trim());
							// Уровень награды
							if (achieveLevel == curAchieveLevel)
							{
								// Сообщение появилось
								Console.WriteLine(achieveLevel);
								needUpdateMessageInfo = false;
							}
						}
						else
						{
							needUpdateMessageInfo = false;
						}
					}
					Driver.FindElement(By.XPath(".//div[@id='achieve-popup']//p/a[contains(@class,'button')]")).Click();
				}
				else
				{
					// Нет награды
					Console.WriteLine("не получили такую награду");
					needUpdateMessageInfo = false;
				}
			}

			return isAchieveMessage;
		}

		/// <summary>
		/// Получить уровень награды в профиле
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <param name="specialistCourseName">название курса (для награды Специалист)</param>
		/// <returns>уровень</returns>
		protected int GetAchieveLevelProfile(string achieveType, string specialistCourseName = "")
		{
			string achieveText = Driver.FindElement(By.XPath(
				".//ul[@class='achive-list']//li[" + GetAchieveNumInList(achieveType, specialistCourseName) + "]//strong")).Text.Trim();
			int indexStart = achieveText.IndexOf("(") + 1;
			// Уровень
			int achieveLevel = int.Parse(achieveText.Substring(indexStart, achieveText.IndexOf("/") - indexStart));
			Console.WriteLine("уровень: " + achieveLevel);
			return achieveLevel;
		}

		/// <summary>
		/// Получить номер награды в списке
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <param name="specialistCourseName">название курса для награды Специалист</param>
		/// <returns>номер награды в списке</returns>
		protected int GetAchieveNumInList(string achieveType, string specialistCourseName = "")
		{
			int rowNumber = 0;
			bool isExist = false;
			// Список наград
			IList<IWebElement> achieveList = Driver.FindElements(By.XPath(".//ul[@class='achive-list']//li//strong"));
			for (int i = 0; i < achieveList.Count; ++i)
			{
				// Если в названии награды есть название искомой награды
				if (achieveList[i].Text.Trim().Contains(achieveType))
				{
					bool isAchieveFound = true;
					// Если награда Специалист и нужно проверить название курса (награда Специалист отдельно для разных курсов)
					if (specialistCourseName.Length > 0)
					{
						string achieveDescrText = Driver.FindElement(By.XPath(".//ul[@class='achive-list']//li[" + (i + 1) + "]//small[contains(@data-bind,'explanation')]")).Text;
						// Есит название курса?
						if (!achieveDescrText.Contains(specialistCourseName))
						{
							isAchieveFound = false;
						}
					}

					if (isAchieveFound)
					{
						// награда найдена
						isExist = true;
						rowNumber = i + 1;
						break;
					}
				}
			}
			// Проверить, что есть такая награда
			Assert.IsTrue(isExist, "Ошибка: такой тип ачивки не найден");

			// Вернуть номер этой награды в списке
			return rowNumber;
		}

		/// <summary>
		/// Проверить награду Эксперт
		/// </summary>
		/// <param name="achieveLevel">уровень награды</param>
		/// <param name="levelLimit">количество голосов для уровня награды</param>
		/// <param name="isVoteUp">голосование За или Против (true - За, false - Против)</param>
		protected void GetExpertLevel(int achieveLevel, int levelLimit, bool isVoteUp = true)
		{
			string achieveType = "Эксперт";
			SelectUserWithoutAchieveLevel(achieveType, achieveLevel);

			bool isTestOk = true;
			string testErrorMessage = "\n";
			int voteNumberLeft = levelLimit - GetAchieveProgressProfile(achieveType);

			if (voteNumberLeft - 1 > 0)
			{
				// Проголосовать до N - 1
				AddVotesCourses(voteNumberLeft - 1, isVoteUp);
				// Проверить, что нет сообщения о награде
				CheckAchieveMessages(achieveType, achieveLevel, false, ref isTestOk, ref testErrorMessage);
			}
			// Проголосовать до N
			AddVotesCourses(1, isVoteUp);
			WaitAchieveMessage();
			// Проверить, что оповещение появилось
			CheckAchieveMessages(achieveType, achieveLevel, true, ref isTestOk, ref testErrorMessage);

			// Проверить прогресс в профиле
			OpenUserProfileFromCourse();
			int newAchieveProgress = GetAchieveProgressProfile(achieveType);
			int newAchieveLevel = GetAchieveLevelProfile(achieveType);
			// проверка уровня награды
			if (newAchieveLevel != achieveLevel)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный уровень: " + newAchieveLevel + "\n";
			}
			// проверка прогресса
			if (newAchieveProgress != levelLimit)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный прогресс: " + newAchieveProgress + "\n";
			}

			// Проголосовать до N + 1
			AddVotesCourses(1, isVoteUp);
			WaitAchieveMessage();
			// Проверить, что оповещение не появилось
			CheckAchieveMessages(achieveType, achieveLevel, false, ref isTestOk, ref testErrorMessage);

			// Проверить прогресс в профиле
			OpenUserProfileFromCourse();
			newAchieveProgress = GetAchieveProgressProfile(achieveType);
			newAchieveLevel = GetAchieveLevelProfile(achieveType);
			// проверка уровня награды
			if (newAchieveLevel != achieveLevel)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после нового голоса в профиле указывается неправильный уровень: " + newAchieveLevel + "\n";
			}
			// проверка прогресса
			if (newAchieveProgress != (levelLimit + 1))
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после нового голоса в профиле указывается неправильный прогресс: " + newAchieveProgress + "\n";
			}

			// Вывести ошибки:
			Assert.IsTrue(isTestOk, testErrorMessage);
		}

		/// <summary>
		/// Подождать появления оповещения о награде
		/// </summary>
		private void WaitAchieveMessage()
		{
			// Задержка для ожидания появления оповещения о награде
			Thread.Sleep(10000);
		}

		/// <summary>
		/// Добавить голоса в курс
		/// </summary>
		/// <param name="voteNumberLeft">оставшееся количество голосов</param>
		/// <param name="isVoteUp">голосование За или Против</param>
		protected void AddVotesCourses(int voteNumberLeft, bool isVoteUp)
		{
			Console.WriteLine("AddVotesCourses: voteNumberLeft: " + voteNumberLeft);
			if (voteNumberLeft > 0)
			{
				OpenCoursePage();
				// Переход в курс с наибольшим прогрессом
				Decimal progress;
				string courseName = SelectCourseMaxProgress(out progress);
				OpenCourseByName(courseName);
				Console.WriteLine("курс: " + courseName);
				int lectureRowNum = 1;
				while (voteNumberLeft > 0)
				{
					// Открыть лекцию
					Console.WriteLine("Открыть лекцию");
					lectureRowNum = SelectLectureToVote(lectureRowNum);
					Console.WriteLine("lectureRowNum: " + lectureRowNum);
					OpenLectureByRowNum(lectureRowNum);

					int lastLastFactRow = 0, votedCounter = 0;
					bool isFinished = false;
					do
					{
						isFinished = AddVotesVisibleSentences(ref lastLastFactRow, voteNumberLeft, isVoteUp, out votedCounter);
						Console.WriteLine(isFinished ? "закончилась" : "не закончилась");
						voteNumberLeft -= votedCounter;
						Console.WriteLine("voteNumberLeft после: " + voteNumberLeft);
					} while (voteNumberLeft > 0 && !isFinished);

					if (voteNumberLeft <= 0)
					{
						Console.WriteLine("voteNumberLeft <= 0");
						break;
					}

					if (isFinished)
					{
						++lectureRowNum;
					}
				}

				Assert.IsTrue(voteNumberLeft <= 0, "Ошибка: закончилась лекция");
			}
		}

		/// <summary>
		/// Выбрать лекцию для голосования (общий прогресс больше 0)
		/// </summary>
		/// <param name="startLecture">с какой лекции начинать поиск</param>
		/// <returns>номер строки с лекцией</returns>
		protected int SelectLectureToVote(int startLecture = 1)
		{
			// Получить проценты личного прогресса всех лекций
			IList<IWebElement> percentElements = Driver.FindElements(By.XPath(".//div[contains(@data-bind,'personalProgressView')]"));
			// Проверить, что список лекций не пуст
			Assert.IsTrue(percentElements.Count > 0, "Ошибка: список лекций пуст");
			bool isLectureExist = false;
			int fullProgress = 0;
			int rowNumber = 0;

			Assert.IsTrue(startLecture < percentElements.Count, "Ошибка: лекции закончились");
			for (int i = (startLecture - 1); i < percentElements.Count; ++i)
			{
				fullProgress = int.Parse(
						Driver.FindElement(By.XPath(
						".//tbody[contains(@data-bind,'lectures')]//tr[" + (i + 1) + "]//td/div[contains(@data-bind,'progressView')]"))
						.Text.Replace("%", ""));
				if (fullProgress > 0)
				{
					isLectureExist = true;
					// нумерация строк с 1, а цикл с 0
					rowNumber = i + 1;
					break;
				}
			}
			Assert.IsTrue(isLectureExist, "Ошибка: нет подходящей лекции (с личным прогрессом < 100 и общим меньше 100 )");

			// Вернуть номер строки с лекцией
			return rowNumber;
		}

		/// <summary>
		/// Проголосовать за переводы в видмых сегментах
		/// </summary>
		/// <param name="lastLastFactRow">IN/OUT: последний номер в строке с последним сегментом при предыдущем проходе редактора</param>
		/// <param name="votesNumberLimit">IN: максимальное количество голосов для добавления</param>
		/// <param name="isVoteUp">IN: голосование За или Против</param>
		/// <param name="votesNumber">OUT: количество добавленных голосов</param>
		/// <returns>лекция закончилась</returns>
		protected bool AddVotesVisibleSentences(ref int lastLastFactRow, int votesNumberLimit, bool isVoteUp, out int votesNumber)
		{
			bool isLectureFinished = false;
			int startIndex = 0;
			// Список видимых сегментов
			IList<IWebElement> segmentsList =
				GetVisibleSegmentList(ref lastLastFactRow, out isLectureFinished, out startIndex);
			Console.WriteLine("isLectureFinished: " + (isLectureFinished ? "да" : "нет"));
			// Сбрасываем счетчик переведенных предложений
			votesNumber = 0;

			if (!isLectureFinished)
			{
				// Пройтись по всем сегментам
				for (int i = startIndex; i < segmentsList.Count; ++i)
				{
					// Кликнуть по Target
					segmentsList[i].Click();
					segmentsList[i].Click();
					// Проголосовать за переводы в сегменте (только за те, за которые нет голосов пользователя)
					int votedNum = VoteSuggestedTranslations(isVoteUp, true, true, votesNumberLimit - votesNumber);
					// Проверить количество сделанных голосов
					votesNumber += votedNum;
					if (votesNumber >= votesNumberLimit)
					{
						Console.WriteLine("Достаточно проголосовано, выходим из лекции");
						break;
					}
				}
			}
			return isLectureFinished;
		}

		/// <summary>
		/// Проверить получение и потерю награды Профессионал
		/// </summary>
		/// <param name="achieveLevel">уровень награды</param>
		/// <param name="levelLimit">количество голосов для награды</param>
		protected void GetProfessionalLevel(int achieveLevel, int levelLimit)
		{
			string achieveType = "Профессионал";
			// Выбрать пользователя, который еще не получил эту награду
			int userIndex = SelectUserWithoutAchieveLevel(achieveType, achieveLevel);

			bool isTestOk = true;
			string testErrorMessage = "\n";

			// Добавить перевод
			string courseName;
			int lectureRowNumber, translationRowNumber;
			string translationText = "Test" + DateTime.Now.Ticks;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNumber);
			Console.WriteLine("курс: " + courseName + ", номер лекции: " + lectureRowNumber + ", номер для перевода: " + translationRowNumber);
			ClickHomeEditor();
			// Выйти из пользователя
			LogoutUser();

			// Проголосовать за него для получения награды N раз
			int votesLeft = levelLimit;
			for (int i = 0; i < TestUserList.Count; ++i)
			{
				if (i != userIndex)
				{
					LoginUser(TestUserList[i]);
					VoteCurrentTranslation(lectureRowNumber, translationRowNumber, translationText, true);
					Console.WriteLine("проголосовал!");
					LogoutUser();
					--votesLeft;
					if (votesLeft <= 0)
					{
						break;
					}
				}
			}

			// Вернуться в пользователя
			if (userIndex == -1)
			{
				LoginUser(User1);
			}
			else if (userIndex < TestUserList.Count)
			{
				LoginUser(TestUserList[userIndex]);
			}

			// Проверить, что сообщение есть
			if (!GetIsExistAchieveMessageLecture(achieveType, true, achieveLevel))
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: не появилось сообщение о присуждении награды\n";
			}

			// Проверить уровень ачивки в профиле
			OpenUserProfileFromCourse();
			int newAchieveLevel = GetAchieveLevelProfile(achieveType);
			// проверка уровня награды
			if (newAchieveLevel != achieveLevel)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный уровень: " + newAchieveLevel + "\n";
			}
			LogoutUser();

			// Один из пользователей проголосует против
			for (int i = 0; i < TestUserList.Count; ++i)
			{
				if (i != userIndex)
				{
					LoginUser(TestUserList[i]);
					OpenCoursePage();
					OpenCourseByName(courseName);
					VoteCurrentTranslation(lectureRowNumber, translationRowNumber, translationText, false);
					LogoutUser();
					break;
				}
			}

			// Вернуться в пользователя
			if (userIndex == -1)
			{
				LoginUser(User1);
			}
			else if (userIndex < TestUserList.Count)
			{
				LoginUser(TestUserList[userIndex]);
			}

			// Проверить прогресс в профиле
			OpenUserProfileFromCourse();
			int newAchieveProgress = GetAchieveProgressProfile(achieveType);
			newAchieveLevel = GetAchieveLevelProfile(achieveType);
			// проверка уровня награды
			if (newAchieveLevel != achieveLevel)
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: после голоса против в профиле указывается неправильный уровень: " + newAchieveLevel + "\n";
			}

			// Добавить новый перевод
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNumber);
			Console.WriteLine("курс: " + courseName + ", номер лекции: " + lectureRowNumber + ", номер для перевода: " + translationRowNumber);
			ClickHomeEditor();
			// Выйти из пользователя
			LogoutUser();

			// Проголосовать за него для получения награды
			votesLeft = levelLimit;
			for (int i = 0; i < TestUserList.Count; ++i)
			{
				if (i != userIndex)
				{
					LoginUser(TestUserList[i]);
					OpenCoursePage();
					OpenCourseByName(courseName);
					VoteCurrentTranslation(lectureRowNumber, translationRowNumber, translationText, true);
					Console.WriteLine("проголосовал!");
					LogoutUser();
					--votesLeft;
					if (votesLeft <= 0)
					{
						break;
					}
				}
			}

			// Вернуться в пользователя
			if (userIndex == -1)
			{
				LoginUser(User1);
			}
			else if (userIndex < TestUserList.Count)
			{
				LoginUser(TestUserList[userIndex]);
			}

			// Проверить, что сообщения нет
			if (GetIsExistAchieveMessageLecture(achieveType, true, achieveLevel))
			{
				isTestOk = false;
				testErrorMessage += "Ошибка: появилось повторное сообщение о присуждении награды\n";
			}

			// Вывести ошибки:
			Assert.IsTrue(isTestOk, testErrorMessage);
		}

		/// <summary>
		/// Проголосовать за конкретный перевод
		/// </summary>
		/// <param name="lectureRowNumber">номер строки лекции</param>
		/// <param name="translationRowNumber">номер строки перевода</param>
		/// <param name="translationText">текст перевода</param>
		/// <param name="voteUp">голос За или Против</param>
		protected void VoteCurrentTranslation(int lectureRowNumber, int translationRowNumber, string translationText, bool voteUp)
		{
			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Перейти в строку с добавленным переводом
			ClickEditorRowByNum(translationRowNumber);
			// Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
			int rowNumber = GetSuggestedTranslationRowNum(translationText);
			// Проголосовать За
			VoteFromEditor(voteUp, rowNumber);
			ClickHomeEditor();
		}

		/// <summary>
		/// Проверить получение и потерю награды, связанной с местом в лидерборде (онлайн)
		/// </summary>
		/// <param name="needUserPosition">нужная позиция пользователя</param>
		/// <param name="achieveType">тип награды</param>
		protected void GetPositionLeaderboardOnline(int needUserPosition, string achieveType)
		{
			// Выбрать пользователя
			SelectUserWithoutAchieve(achieveType);
			// TODO впилить изменение рейтинга

			// Хранит значение: была ли в течение теста какая-нибудь ошибка
			bool isOk = true;
			// Хранит сообщения обо всех найденных ошибках
			string errorMessage = "\n";
			if (!GetIsAchieveProgressUserPositionRight(achieveType, needUserPosition))
			{
				isOk = false;
				errorMessage += "Ошибка: перед началом теста у пользователя неправильно отображается прогресс в награде\n";
			}

			bool isAchieveMessageLectureAppeared = false, isAchieveMessageEditorAppeared = false, isAchieveProfileReceived = false, isAchieveReceived = false;
			string courseName = "";
			while (!isAchieveReceived)
			{
				// Открыть курс
				OpenCoursePage();
				courseName = OpenCourseMinProgress();

				// Заполнить лекцию до появления оповещения о награде
				isAchieveMessageEditorAppeared = FillLectureWaitAchieve(achieveType);
				// Проверить оповещение в лекции
				isAchieveMessageLectureAppeared = GetIsExistAchieveMessageLecture(achieveType);
				// Открыть профиль
				OpenUserProfileFromCourse();
				isAchieveProfileReceived = GetIsAchieveReceivedProfile(achieveType);
				// Из-за того, что рейтинг может долго пересчитываться, оповещение о награде может прийти позже
				// т.е. когда зайдем в профиль - награда будет и в профиле, и появится оповещение о награде
				if (isAchieveProfileReceived)
				{
					isAchieveMessageLectureAppeared = GetIsExistAchieveMessageLecture(achieveType);
				}

				isAchieveReceived = isAchieveMessageEditorAppeared || isAchieveMessageLectureAppeared || isAchieveProfileReceived;
			}

			// Проверка: оповещение о награде было, а в профиле награда не как получена
			if ((isAchieveMessageEditorAppeared || isAchieveMessageLectureAppeared) && !isAchieveProfileReceived)
			{
				isOk = false;
				errorMessage += "Ошибка: появилось оповещение о награде, а в профиле награда не как получена\n";
			}

			// Проверка: в профиле награда получена, а оповещения не было
			if (isAchieveProfileReceived && !(isAchieveMessageEditorAppeared || isAchieveMessageLectureAppeared))
			{
				isOk = false;
				errorMessage += "Ошибка: в профиле награда получена, но оповещения не было\n";
			}

			// Проверка: повторное оповещение (в редакторе и в лекции)
			if (isAchieveMessageEditorAppeared && isAchieveMessageLectureAppeared)
			{
				isOk = false;
				errorMessage += "Ошибка: оповещение появилось дважды: в редакторе и в лекции\n";
			}

			// Проверить прогресс награды (прогресса быть не должно)
			if (GetAchieveProgressUserPosition(achieveType) != 0)
			{
				isOk = false;
				errorMessage += "Ошибка: после получения награды не должно быть прогресса\n";
			}

			// Проверяем позицию пользователя
			if (GetUserPosition() > needUserPosition)
			{
				isOk = false;
				errorMessage += "Ошибка: неправильное место пользователя в профиле\n";
			}

			// Удалить переводы, чтобы понизить место
			OpenCoursePage();
			OpenCourseByName(courseName);
			OpenLectureByRowNum(1);
			DeleteMyTranslations(1, 5);
			ClickHomeEditor();

			// Проверить место пользователя
			OpenUserProfileFromCourse();
			if (GetUserPosition() > needUserPosition)
			{
				// Проверить в профиле, что награда не потерялась
				if (!GetIsAchieveReceivedProfile(achieveType))
				{
					isOk = false;
					errorMessage += "Ошибка: после удаления переводов в профиле награда потеряна\n";
				}
				// Проверить, что у награды нет прогресса
				if (GetAchieveProgressUserPosition(achieveType) > 0)
				{
					isOk = false;
					errorMessage += "Ошибка: в профиле после получения награды и после удаления части переводов указан прогресс\n";
				}
			}

			// Добавить переводы и проверить, появилось ли оповещение о награде в редакторе
			isAchieveMessageEditorAppeared = AddTranslationsWaitAchieve(5, achieveType);
			// Проверить оповещение в лекции
			isAchieveMessageLectureAppeared = GetIsExistAchieveMessageLecture(achieveType);

			if (isAchieveMessageEditorAppeared || isAchieveMessageLectureAppeared)
			{
				isOk = false;
				errorMessage += "Ошибка: если потерять место, а потом его снова получить - повторно появляется оповещение о награде награда\n";
			}

			OpenUserProfileFromCourse();
			// Проверить в профиле, что награда получена
			if (!GetIsAchieveReceivedProfile(achieveType))
			{
				isOk = false;
				errorMessage += "Ошибка: после добавления заново переводов в профиле награда потеряна\n";
			}
			// Проверить, что у награды нет прогресса
			if (GetAchieveProgressUserPosition(achieveType) > 0)
			{
				isOk = false;
				errorMessage += "Ошибка: в профиле после добавления заново переводов около награды отображается прогресс\n";
			}

			// Вывести ошибки
			Assert.IsTrue(isOk, errorMessage);
		}

		/// <summary>
		/// Получить рейтинг пользователя на конкретной позиции в лидерборде (для мест на первой странице лидерборда)
		/// </summary>
		/// <param name="userPosition">позиция в лидерборде</param>
		/// <returns>рейтинг</returns>
		protected Decimal GetLeaderboardPositionRating(int userPosition)
		{
			Decimal retVal = 0;
			if (userPosition <= 10)
			{
				// Получить рейтинг по месту пользователя в лидерборде
				retVal = Decimal.Parse(Driver.FindElement(By.XPath(
				".//tbody//tr//td[contains(@data-bind,'position')][text()='" + userPosition + "']/..//td[contains(@data-bind,'rating')]")).Text.Trim().Replace(".", ","));
			}
			return retVal;
		}

		/// <summary>
		/// Добавить до получения награды, но не более max количества
		/// </summary>
		/// <param name="maxNumTranslatoins">максимальное количество переводов для добавления</param>
		/// <param name="achieveType">тип награды</param>
		/// <returns>получили награду</returns>
		protected bool AddTranslationsWaitAchieve(int maxNumTranslatoins, string achieveType)
		{
			// Открыть лекцию для перевода
			OpenCoursePage();
			OpenCourseMinProgress();
			OpenLectureByRowNum(SelectLectureToTranslate());

			bool isLectureFinished = false, isAchieveReceived = false;
			int numTranslationsLeft = maxNumTranslatoins;
			int lastLastFactRow = 0;

			Console.WriteLine("Начинаем добавлять 5 переводов");

			// Добавить до получения награды, но не более количества переводов
			while ((numTranslationsLeft > 0) && !isAchieveReceived)
			{
				// Если лекция заполнена, то зайти в др.лекцию
				if (isLectureFinished)
				{
					Console.WriteLine("Лекция  закончилась, перезаходим");
					ClickHomeEditor();
					OpenLectureByRowNum(SelectLectureToTranslate());
					isLectureFinished = false;
				}

				Console.WriteLine("Добавляем переводы");
				// Добавить переводы
				isAchieveReceived = AddTranslationEmptySegment(ref numTranslationsLeft, out isLectureFinished, achieveType, ref lastLastFactRow);
			}

			Console.WriteLine("Выходим");
			// Выйти из редактора
			ClickHomeEditor();

			return isAchieveReceived;
		}

		/// <summary>
		/// Добавить N переводоы в пустые сегменты
		/// </summary>
		/// <param name="numTranslationsLeft">IN/OUT: сколько осталось добавить переводов</param>
		/// <param name="isLectureFinished">OUT: лекция закончилась</param>
		/// <param name="achieveType">тип награды (оповещение о которой ожидается)</param>
		/// <param name="lastLastFactRow">IN/OUT: фактический номер последней видимой строки</param>
		/// <returns>появилось ли оповещение о награде</returns>
		protected bool AddTranslationEmptySegment(ref int numTranslationsLeft, out bool isLectureFinished, string achieveType, ref int lastLastFactRow)
		{
			bool isAchieveReceived = false;
			int startIndex = 0;
			// Список видимых сегментов
			IList<IWebElement> segmentsList = GetVisibleSegmentList(ref lastLastFactRow, out isLectureFinished, out startIndex);

			// Сбрасываем счетчик переведенных предложений
			string translationText = "Test" + DateTime.Now.Ticks;

			// Если лекция не закончилась, осталось добавить больше 0, награда не получена - добавляем переводы
			if (!isLectureFinished && numTranslationsLeft > 0 && !isAchieveReceived)
			{
				// Пройтись по всем видимым сегментам
				for (int i = startIndex; i < segmentsList.Count; ++i)
				{
					segmentsList[i].Click();
					segmentsList[i].Click();
					if (!GetIsExistMyTranslationSegment())
					{
						segmentsList[i].Clear();
						// Заполнить, если нет перевода
						segmentsList[i].SendKeys(translationText);

						// Кликнуть по галочке с Confirm в строке сегмента
						Driver.FindElement(By.XPath(".//div[@id='segments-body']//span[contains(@class,'fa-check')]")).Click();
						WaitUntilDisappearElement(".//div[@id='segments-body']//span[contains(@class,'fa-border')]", 20);

						// Проверить, что перевод появился в предложенных переводах
						int translationRowNumber = GetSuggestedTranslationRowNum(translationText);
						if (translationRowNumber > 0)
						{
							--numTranslationsLeft;
							isAchieveReceived = GetIsExistAchieveMessageEditor(achieveType);
							if (numTranslationsLeft <= 0)
							{
								break;
							}
						}
					}
				}
			}

			// Появилась ли награда
			return isAchieveReceived;
		}

		/// <summary>
		/// Получить: получена ли награда (по типу отображения в профиле)
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <returns>получена награда</returns>
		protected bool GetIsAchieveReceivedProfile(string achieveType)
		{
			return !Driver.FindElement(By.XPath(
				".//ul[@class='achive-list']//li[" + GetAchieveNumInList(achieveType) + "]//span[contains(@class,'achive-type')]")).GetAttribute("class").Contains("not_av");
		}

		/// <summary>
		/// Проверить получение и потерю награды, связанной с местом в лидерборде (оффлайн)
		/// </summary>
		/// <param name="needUserPosition">нужная позиция пользователя</param>
		/// <param name="achieveType">тип награды</param>
		/// <param name="isNeedGetStepUpPositionOffline">для теста, когда оффлайн пользователь должен подняться не до нужного места (10), а на место выше (9)</param>
		protected void GetPositionLeaderboardOffline(int needUserPosition, string achieveType, bool isNeedGetStepUpPositionOffline = false)
		{
			//  TODO впилить изменение рейтинга

			// Выбрать пользователя
			int userIndex = SelectUserWithoutAchieve(achieveType);

			OpenHomepage();
			string userName = GetUserNameHomepage();

			// Открыть лидерборд
			OpenLeaderboardPage();
			// Получить рейтинг пользователя на нужном месте
			Decimal ratingNeedPosition = GetLeaderboardPositionRating(needUserPosition);
			Console.WriteLine("ratingNeedPosition: " + ratingNeedPosition);
			bool isNeedAddTranslations = false;
			int minimumNumberToAdd = 0;
			Decimal ratingBefore = 0;
			List<string> courseList = new List<string>();
			do
			{
				// Рейтинг в профиле
				OpenUserProfileFromCourse();
				ratingBefore = GetUserRating();
				Console.WriteLine("Сейчас рейтинг пользователя: " + ratingBefore);

				// Нужно добавить переводов
				minimumNumberToAdd = (int)((ratingNeedPosition - ratingBefore) / 3);
				Console.WriteLine("нужно переводов: " + minimumNumberToAdd);
				isNeedAddTranslations = minimumNumberToAdd > 1;

				if (isNeedAddTranslations)
				{
					// Перейти к списку доступных курсов
					OpenCoursePage();
					// Переход в курс с наименьшим прогрессом				
					string courseName = OpenCourseMinProgress();

					if (!courseList.Contains(courseName))
					{
						courseList.Add(courseName);
					}
					Console.WriteLine("курс: " + courseName);
					int lectureRowNumber = 1;
					// Добавить переводы для увеличения рейтинга
					AddTranslationsCourse(minimumNumberToAdd, ref lectureRowNumber);
					ClickHomeEditor();
				}
			} while (isNeedAddTranslations);

			// Проверить позицию пользователя
			OpenUserProfileFromCourse();
			int userPosition = GetUserPosition();
			Console.WriteLine("Сейчас место пользователя: " + userPosition);
			if (userPosition <= needUserPosition)
			{
				Assert.Fail("Перелёт");
			}

			// Зайти другим пользователем
			LogoutUser();
			if (userIndex == TestUserList.Count - 1)
			{
				LoginUser(TestUserList[0]);
			}
			else
			{
				LoginUser(TestUserList[userIndex + 1]);
			}

			Console.WriteLine("Зашли др пользователем");

			// Какую позицию должен достичь пользователь голосованием
			int offlineNeedUserPosition = isNeedGetStepUpPositionOffline ? (needUserPosition - 1) : needUserPosition;
			Console.WriteLine("offline");
			// TODO заменить
			courseList.Add("The Emergence of the Modern Middle East");
			while (userPosition > offlineNeedUserPosition)
			{
				Console.WriteLine("Надо голосовать");
				// Проголосовать за перевод этого пользователя
				VoteUserTranslationCourses(userName, courseList);
				Console.WriteLine("Проголосовали");
				// Подождать, чтобы обновился лидерборд
				Thread.Sleep(2000);
				// Проверить место пользователя в лидерборде
				OpenLeaderboardPage();
				// Если пользователь в основном списке
				if (GetIsUserLeaderboardActiveList(userName))
				{
					// Проверить позицию пользователя
					userPosition = GetUserPositionLeaderboard(userName);
				}
			}

			// Вернуться в пользователя
			LogoutUser();
			if (userIndex == -1)
			{
				LoginUser(User1);
			}
			else
			{
				LoginUser(TestUserList[userIndex]);
			}

			bool isOk = true;
			string errorMessage = "\n";

			// Проверить, что при входе пользователя появилось оповещение о присуждении награды
			if (!GetIsExistAchieveMessageLecture(achieveType))
			{
				isOk = false;
				errorMessage += "Ошибка: при входе пользователя не появилось сообщение о присуждении награды\n";
			}
			// Открыть профиль
			OpenUserProfileFromCourse();
			// Проверить, что в профиле награда отображается как полученная
			if (!GetIsAchieveReceivedProfile(achieveType))
			{
				isOk = false;
				errorMessage += "Ошибка: в профиле не показывается, что награда получена\n";
			}

			// Вывести ошибки
			Assert.IsTrue(isOk, errorMessage);
		}

		/// <summary>
		/// Проголосовать за перевод пользователя (проход по курсам)
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <param name="courseList">список курсов</param>
		protected void VoteUserTranslationCourses(string userName, List<string> courseList)
		{
			bool isVoted = false;
			for (int i = 0; i < courseList.Count; ++i)
			{
				// Открыть курс
				OpenCoursePage();
				OpenCourseByName(courseList[i]);
				int lectureRowNum = 1;
				while (!isVoted)
				{
					// Выбрать лекцию (если лекция заканчивается - ищем следующие лекции)
					lectureRowNum = SelectLectureToVote(lectureRowNum);
					// Открыть лекцию
					OpenLectureByRowNum(lectureRowNum);
					int lastLastFactRow = 0;
					bool isFinished = false;
					do
					{
						// Проголосовать за перевод пользователя в лекции
						isFinished = VoteVisibleUserTranslationLectures(ref lastLastFactRow, userName, out isVoted);
						// Если проголосовали - выходим
						if (isVoted)
						{
							break;
						}
					} while (!isVoted && !isFinished);

					if (isVoted)
					{
						break;
					}

					if (isFinished)
					{
						// Если лекция закончилась - поиск последующих лекций
						++lectureRowNum;
					}
				}

				// Выход из лекции
				ClickHomeEditor();
			}
		}

		/// <summary>
		/// Проголосовать за перевод пользователя (проход по лекциям курса)
		/// </summary>
		/// <param name="lastLastFactRow">IN/OUT: фактический номер видимой последней строки</param>
		/// <param name="userName">имя пользователя, за чьи переводы надо проголосовать</param>
		/// <param name="isVoted">OUT: проголосовали</param>
		/// <returns>лекция закончилась</returns>
		protected bool VoteVisibleUserTranslationLectures(ref int lastLastFactRow, string userName, out bool isVoted)
		{
			bool isLectureFinished = false;
			int startIndex = 0;
			isVoted = false;
			// Список видимых сегментов
			IList<IWebElement> segmentsList = GetVisibleSegmentList(ref lastLastFactRow, out isLectureFinished, out startIndex);

			string translationText = "Test" + DateTime.Now.Ticks;

			if (!isLectureFinished)
			{
				// Проход по сегментам
				for (int i = startIndex; i < segmentsList.Count; ++i)
				{
					// Кликнуть по Target
					segmentsList[i].Click();
					segmentsList[i].Click();
					Console.WriteLine("номер строки: " + (i + 1));
					// Проголосовать за перевод пользователя в сегменте
					isVoted = VoteSuggestedUserTranslations(userName);
					if (isVoted)
					{
						Console.WriteLine("Достаточно проголосовано, выходим из лекции");
						break;
					}
				}
			}
			return isLectureFinished;
		}

		/// <summary>
		/// Проголосовать за предложенный пользователем перевод (в сегменте)
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <returns>проголосовали</returns>
		protected bool VoteSuggestedUserTranslations(string userName)
		{
			bool isVoted = false;
			setDriverTimeoutMinimum();
			// Проверить, что среди предложенных переводов есть перевод нужного пользователя
			bool isExistUserTranslation =
				IsElementPresent(By.XPath(".//div[@id='translations-body']//table//tr//td[2]//div[contains(text(),'" + userName + "')]"));
			setDriverTimeoutDefault();

			if (isExistUserTranslation)
			{
				// Список элементов для голосования
				IList<IWebElement> translationsList = Driver.FindElements(By.XPath(
					".//div[@id='translations-body']//table//tr//td[5]/div//span[contains(@class,'fa-thumbs-up')]"));

				Console.WriteLine(translationsList.Count > 0 ? "переводы есть" : "переводов нет");

				// Пробуем проголосовать за предложенный перевод пользователя
				for (int i = 0; i < translationsList.Count; ++i)
				{
					// Имя переводчика предложенного перевода
					string translaterName = Driver.FindElement(By.XPath(SelectByUrl(
						".//div[@id='translations-body']//table//tr[" + (i + 1) + "]//td[2]//div",
						".//div[@id='translations-body']//table[" + (i + 1) + "]//td[2]//div"))).Text;
					// Если имя переводчика содержит имя нужного переводчика - голосуем
					if (translaterName.Contains(userName))
					{
						// Если не отдавали голос - голосуем
						if (!translationsList[i].GetAttribute("class").Contains("disabled"))
						{
							Console.WriteLine("пытаемся проголосовать " + i);
							translationsList[i].Click();
							// Проверяем - принят ли голос
							if (GetIsVoteConsideredEditor(true, (i + 1)))
							{
								isVoted = true;
								break;
							}
						}
					}
				}
			}

			// Проглосовали
			return isVoted;
		}

		/// <summary>
		/// Получить позицию пользователя в лидерборде
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		/// <returns></returns>
		protected int GetUserPositionLeaderboard(string userName)
		{
			return int.Parse(
				Driver.FindElement(By.XPath(
				".//tr[not(contains(@style,'display: none;'))]//td[3]/a[contains(text(),'" + userName + "')]/../..//td[1]")).Text.Trim());
		}

		/// <summary>
		/// Проверить получение и потерю награды "Специалист"
		/// </summary>
		/// <param name="achieveLevel">уровень награды</param>
		/// <param name="levelLimit">процент для получения уровня</param>
		/// <param name="courseName">имя курса</param>
		protected void GetSpecialistLevel(int achieveLevel, int levelLimit, string courseName)
		{
			string achieveType = "Специалист";
			SelectUserWithoutAchieveLevel(achieveType, achieveLevel, courseName);

			// TODO убрать
			string courseNameToOpen = "";
			if (courseName == "Модель мышления")
				courseNameToOpen = "Model Thinking";

			if (courseName == "Возникновение современного Ближнего Востока")
				courseNameToOpen = "The Emergence of the Modern Middle East";

			if (courseName == "Криптография")
				courseNameToOpen = "Cryptography";

			bool isLevelReceived = false, isAchieveMessageLectureAppeared = false, isAchieveMessageAppeared = false;
			while (!isLevelReceived)
			{
				// Открыть курс
				OpenCoursePage();
				OpenCourseByName(courseNameToOpen);

				// Заполнить лекцию до появления оповещения о награде
				isAchieveMessageAppeared = FillLectureWaitAchieve(achieveType, true, achieveLevel);
				// Проверить оповещение в лекции
				isAchieveMessageLectureAppeared = GetIsExistAchieveMessageLecture(achieveType, true, achieveLevel);

				// Открыть профиль
				OpenUserProfileFromCourse();
				// Проверить, что достигли уровень
				isLevelReceived = GetAchieveLevelProfile(achieveType, courseName) == achieveLevel;
				Console.WriteLine("рейтинг: " + GetUserRating());
			}

			bool isOk = true;
			string errorMessage = "\n";

			if (!isAchieveMessageAppeared)
			{
				isOk = false;
				errorMessage += "Ошибка: не появилось оповещение о том, что получили награду Специалист, хотя в профиле уровень награды поднялся\n";
			}
			if (isAchieveMessageLectureAppeared)
			{
				isOk = false;
				errorMessage += "Ошибка: появилось оповещение о награде Специалист в лекции (повторное оповещение)\n";
			}
			// Проверить прогресс награды
			if (GetAchieveProgressPercent(achieveType, courseName) < levelLimit)
			{
				isOk = false;
				errorMessage += "Ошибка: в профиле в прогрессе награды указан неправльный прогресс\n";
			}

			// Удалить переводы
			OpenCoursePage();
			OpenCourseByName(courseNameToOpen);
			OpenLectureByRowNum(1);
			DeleteMyTranslations(1, 5);
			ClickHomeEditor();
			OpenUserProfileFromCourse();

			// Проверить, что не потеряли уровень
			if (GetAchieveLevelProfile(achieveType, courseName) < achieveLevel)
			{
				isOk = false;
				errorMessage += "Ошибка: потеряли уровень награды, когда удалили часть переводов\n";
			}
			// Проверить, что прогресс не уменьшился
			if (GetAchieveProgressPercent(achieveType, courseName) < levelLimit)
			{
				isOk = false;
				errorMessage += "Ошибка: потеряли прогресс награды, когда удалили часть переводов\n";
			}

			// Добавить переводы
			int lastRowNumber = 0, addedTranslationsNumber;
			OpenCoursePage();
			OpenCourseByName(courseNameToOpen);
			OpenLectureByRowNum(1);
			AddTranslationsEmptyVisibleSegments(ref lastRowNumber, 5, out addedTranslationsNumber);

			// Проверка оповещения в редакторе
			isAchieveMessageAppeared = GetIsExistAchieveMessageEditor(achieveType, true, achieveLevel);
			ClickHomeEditor();
			// Проверка оповещения в лекции
			isAchieveMessageLectureAppeared = GetIsExistAchieveMessageLecture(achieveType, true, achieveLevel);

			if (isAchieveMessageAppeared)
			{
				isOk = false;
				errorMessage += "Ошибка: заново получили ачивку (при повторном добавлении переводов)\n";
			}
			if (isAchieveMessageLectureAppeared)
			{
				isOk = false;
				errorMessage += "Ошибка: заново получили ачивку и оповещение появилось в лекции\n";
			}

			// Вывести ошибки
			Assert.IsTrue(isOk, errorMessage);
		}

		/// <summary>
		/// Заполнить лекцию, ожидая оповещение о награде
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <param name="isNeedAchieveLevel">нужна ли проверка уровня награды</param>
		/// <param name="achieveLevel">уровень награды</param>
		/// <returns>появилось оповещение о награде</returns>
		protected bool FillLectureWaitAchieve(string achieveType, bool isNeedAchieveLevel = false, int achieveLevel = 1)
		{
			// Открыть лекцию
			OpenLectureByRowNum(SelectLectureToTranslate());

			bool isAchieveAppeared = false;
			bool isLectureFilled = false;
			int lastSentenceNumber = 0;
			do
			{
				// Заполнить всю лекцию до появления награды
				isLectureFilled = AddTranslationsEmptyVisibleSegmentsWaitAchieve(ref lastSentenceNumber, achieveType, isNeedAchieveLevel, achieveLevel, out isAchieveAppeared);
			} while (!isLectureFilled && !isAchieveAppeared);
			// Если получили награду или полностью заполнили лекцию - выходим

			// Выйти из редактора
			ClickHomeEditor();

			return isAchieveAppeared;
		}

		/// <summary>
		/// Добавить переводы и ожидать оповещение о награде
		/// </summary>
		/// <param name="lastLastFactRow">IN/OUT: фактический номер последнего видимого сегмента</param>
		/// <param name="achieveType">тип награды</param>
		/// <param name="isNeedAchieveLevel">нужна ли проверка уровня награды</param>
		/// <param name="achieveLevel">уровень награды</param>
		/// <param name="isAchieveAppeared">OUT: появилось оповещение о награде</param>
		/// <returns></returns>
		protected bool AddTranslationsEmptyVisibleSegmentsWaitAchieve(ref int lastLastFactRow, string achieveType, bool isNeedAchieveLevel, int achieveLevel, out bool isAchieveAppeared)
		{
			bool isLectureFinished = false;
			int startIndex = 0;
			// Список видимых сегментов
			IList<IWebElement> segmentsList = GetVisibleSegmentList(ref lastLastFactRow, out isLectureFinished, out startIndex);

			// Сбрасываем счетчик переведенных предложений
			string translationText = "Test" + DateTime.Now.Ticks;
			isAchieveAppeared = false;

			if (!isLectureFinished)
			{
				// Пройтись по всем видимым сегментам
				for (int i = startIndex; i < segmentsList.Count; ++i)
				{
					// Кликнуть по Target
					segmentsList[i].Click();
					segmentsList[i].Click();
					// Если перевода этого пользователя нет - добавить перевод
					if (!GetIsExistMyTranslationSegment())
					{
						segmentsList[i].Clear();
						segmentsList[i].SendKeys(translationText);
						// Кликнуть по галочке с Confirm в строке сегмента
						Driver.FindElement(By.XPath(".//div[@id='segments-body']//span[contains(@class,'fa-check')]")).Click();
						WaitUntilDisappearElement(".//div[@id='segments-body']//span[contains(@class,'fa-border')]", 20);

						// Проверить, что перевод появился в предложенных переводах
						int translationRowNumber = GetSuggestedTranslationRowNum(translationText);
						if (translationRowNumber > 0)
						{
							isAchieveAppeared = GetIsExistAchieveMessageEditor(achieveType, isNeedAchieveLevel, achieveLevel);

							if (isAchieveAppeared)
							{
								break;
							}
						}
					}
				}
			}

			// Закончилась ли лекция
			return isLectureFinished;
		}

		/// <summary>
		/// Получить прогресс (процент) награды в профиле
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <param name="specialistCourseName">название курса для награды Специалист</param>
		/// <returns>процентный прогресс</returns>
		protected int GetAchieveProgressPercent(string achieveType, string specialistCourseName = "")
		{
			// текст прогресса для награды
			string achieveText = Driver.FindElement(By.XPath(
				".//ul[@class='achive-list']//li[" + GetAchieveNumInList(achieveType, specialistCourseName) + "]//small[contains(@data-bind,'progress')]")).Text.Trim();
			int startIndex = achieveText.IndexOf(" ") + 1;
			// Прогресс
			int achieveProgress = int.Parse(achieveText.Substring(startIndex, achieveText.IndexOf("%") - startIndex));
			return achieveProgress;
		}

		/// <summary>
		/// Получить прогресс награды в профиле (количество пользователей перед текущим пользователем)
		/// </summary>
		/// <param name="achieveType">тип награды</param>
		/// <returns>прогресс награды (если 0 - либо прогресса нет, либо в прогрессе указан 0)</returns>
		protected int GetAchieveProgressUserPosition(string achieveType)
		{
			int achieveProgress = 0;
			string achieveText = Driver.FindElement(By.XPath(
				".//ul[@class='achive-list']//li[" + GetAchieveNumInList(achieveType) + "]//small[contains(@data-bind,'progress')]")).Text.Trim();
			// Прогресс
			if (achieveText.Length > 0)
			{
				int startIndex = achieveText.IndexOf(" ") + 1;
				int endIndex = achieveText.IndexOf(" ", startIndex);
				achieveProgress = int.Parse(achieveText.Substring(startIndex, endIndex - startIndex));
			}

			Console.WriteLine("прогресс: " + achieveProgress);
			return achieveProgress;
		}

		/// <summary>
		/// Проверить, правильно ли отображается прогресс в профиле для наград, связанных с местом пользователя
		/// прогресс в награде не всегда в точности совпадает с (МестоПользователя - 10) или (МестоПользователя - 1),
		/// т.к. учитывается, что несколько пользователей с одним и тем же рейтингом может занимать несколько мест.
		/// Т.е. пользователь занимает 12 место, а перед ним на 10 и 11 месте два пользователя с одинаковым рейтингом,
		/// тогда будет показано, что до 10 места перед ним только 1 человек, а не 2, т.к. при повышении рейтинга оба пользователя сместятся одновременно
		/// </summary>
		/// <param name="achieveType">тир награды</param>
		/// <param name="needUserPosition">место пользователя для получения награды</param>
		/// <returns></returns>
		protected bool GetIsAchieveProgressUserPositionRight(string achieveType, int needUserPosition)
		{
			// Проверить, верен ли прогресс в профиле
			return GetAchieveProgressUserPosition(achieveType) >= (GetUserPosition() - needUserPosition);
		}

		/// <summary>
		/// Проголосовать в виджете
		/// </summary>
		/// <param name="btnXPath">xPath кнопки в виджете</param>
		protected void VoteFromWidget(string btnXPath)
		{
			// Открыть виджет
			Driver.FindElement(By.Id("estimate")).Click();
			// Дождаться открытия виджета
			Wait.Until((d) => d.FindElement(By.Id("popup-voting")).Displayed);
			// Проголосовать в виджете
			Driver.FindElement(By.XPath(".//div[@id='popup-voting']//button[contains(@data-bind,'" + btnXPath + "')]")).Click();
			Driver.FindElement(By.XPath(".//div[@id='popup-voting']//span[@class='close']")).Click();
			// Обновить страницу
			Driver.FindElement(By.Id("estimate")).SendKeys(OpenQA.Selenium.Keys.F5);
			WaitUntilDisappearElement("estimate");
		}
	}
}