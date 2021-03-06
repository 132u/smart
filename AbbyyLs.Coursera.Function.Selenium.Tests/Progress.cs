﻿using System;
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
	class Progress : BaseTest
	{
		public Progress(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Тест: Добавление перевода в сегмент, где нет никаких переводов
		/// Проверка: личный прогресс увеличивается 
		/// </summary>
		[Test]
		public void AddTranslationPersonalProgressTest()
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы для увеличения прогресса
			// Проверка, что прогресс увеличен (Assert внутри)
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);
		}

		/// <summary>
		/// Тест: Добавление перевода в сегмент, где есть перевод пользователя
		/// Проверка: личный прогресс не меняется 
		/// Проверка: общий прогресс не меняется
		/// </summary>
		[Test]
		public void AddTranslationExistingCheckPersonalFullTest()
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы для увеличения прогресса
			// Проверка, что прогресс увеличен (Assert внутри)
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);

			// Прогресс после добавления
			int personalProgressBefore = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);
			int lectureProgressBefore = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);

			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);

			// Добавить переводы
			AddNumOfTranslations(translationRowNum);
			// Нажать выход
			ClickHomeEditor();

			// Прогресс после повторного добавления
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");
			int personalProgressAfter = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);
			int lectureProgressAfter = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);

			bool isError = false;
			string errorMessage = "\n";

			// Проверить, что личный прогресс не изменился
			if (personalProgressBefore != personalProgressAfter)
			{
				isError = true;
				errorMessage += "Ошибка: личный прогресс не должен меняться\n";
			}
			// Проверить, что общий прогресс не изменился
			if (lectureProgressBefore != lectureProgressAfter)
			{
				isError = true;
				errorMessage += "Ошибка: общий прогресс не должен меняться\n";
			}
			// Вывести ошибки
			Assert.IsFalse(isError, errorMessage);
		}

		/// <summary>
		/// Тест: Добавление перевода в сегмент, где есть перевод другого юзера с положительной оценкой
		/// Проверка: личный прогресс увеличился
		/// Проверка: общий прогресс не изменился
		/// </summary>
		[Test]
		public void AddTranslationExistingAnotherUserCheckPersonalFullTest()
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы для увеличения прогресса
			// Проверка, что прогресс увеличен (Assert внутри)
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);

			// Выйти из этого пользователя
			Header.LogoutUser();
			// Зайти под другим пользователем
			LoginUser(User2);

			// Прогресс до добавления переводов
			int personalProgressBefore = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);
			int lectureProgressBefore = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);

			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Добавить переводы
			AddNumOfTranslations(translationRowNum);
			// Нажать выход
			ClickHomeEditor();

			// Прогресс после добавления
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");
			int personalProgressAfter = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);
			int lectureProgressAfter = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);

			bool isError = false;
			string errorMessage = "\n";

			// Проверить, что личный прогресс увеличился
			if (personalProgressAfter <= personalProgressBefore)
			{
				isError = true;
				errorMessage += "Ошибка: личный прогресс не увеличился (был :"
					+ personalProgressBefore + ", стал: " + personalProgressAfter + ")\n";
			}
			// Проверить, что общий прогресс не изменился
			if (lectureProgressBefore != lectureProgressAfter)
			{
				isError = true;
				errorMessage += "Ошибка: общий прогресс не должен меняться (был :"
					+ lectureProgressBefore + ", стал: " + lectureProgressAfter + ")\n";
			}
			// Вывести ошибки
			Assert.IsFalse(isError, errorMessage);
		}

		/// <summary>
		/// Тест: Удаление своего варианта перевода
		/// Проверка: личный прогресс должен уменьшиться
		/// </summary>
		[Test]
		public void DeleteTranslationPersonalProgressTest()
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы для увеличения прогресса
			// Проверка, что прогресс увеличен (Assert внутри)
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);

			// Личный прогресс до удаления
			int personalProgressBefore = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);
			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Удалить переводы
			DeleteMyTranslations(translationRowNum, numberSentencesGrowProgress);
			ClickHomeEditor();

			// Прогресс после удаления
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			int personalProgressAfter = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);
			// Проверить, что прогресс уменьшился
			Assert.IsTrue(personalProgressAfter < personalProgressBefore, "Ошибка: личный прогресс не уменьшился");
		}

		/// <summary>
		/// Тест: Добавление перевода в сегмент, где нет никаких переводов
		/// Проверка: общий прогресс увеличивается
		/// </summary>
		[Test]
		public void AddTranslationEmptySegmentsCheckFullTest()
		{
			// Перейти к списку доступных курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Переход в курс с наименьшим прогрессом
			string courseName = OpenCourseMinProgress();
			// Найти лекцию с пустым прогрессом
			int lectureRowNumber = SelectEmptyLectureGetRowNumber();
			// Общий прогресс до добавления
			int lectureProgressBefore = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);
			OpenLectureByRowNum(lectureRowNumber);

			// Добавить переводы в сегменты, в которых нет переводов
			List<int> translatedRows = AddNumOfTranslationsEmptySegments();
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			int lectureProgressAfter = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);
			// Проверить, что прогресс увеличился
			Assert.IsTrue(lectureProgressAfter > lectureProgressBefore, "Ошибка: общий прогресс не увеличился");
		}

		/// <summary>
		/// Тест: Удаление варианта перевода из сегмента, где только один вариант перевода
		/// Проверка: общий прогресс уменьшается
		/// </summary>
		[Test]
		public void DeleteTranslationsEmptySegmentsCheckFullTest()
		{
			// Перейти к списку доступных курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Переход в курс с наименьшим прогрессом
			string courseName = OpenCourseMinProgress();
			// Найти лекцию с пустым прогрессом
			int lectureRowNumber = SelectEmptyLectureGetRowNumber();
			OpenLectureByRowNum(lectureRowNumber);

			// Добавить переводы в сегменты, в которых нет переводов
			List<int> translatedRows = AddNumOfTranslationsEmptySegments();
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			int lectureProgressBefore = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);
			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Удалить добавленные переводы
			DeleteMyTranslations(translatedRows);
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			int lectureProgressAfter = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);
			// Проверить, что прогресс уменьшился
			Assert.IsTrue(lectureProgressAfter < lectureProgressBefore, "Ошибка: общий прогресс не уменьшился");
		}

		/// <summary>
		/// Тест: Удаление варианта перевода из сегмента, где есть еще варианты перевода
		/// Проверка: общий прогресс не меняется
		/// </summary>
		[Test]
		public void DeleteTranslationsExistingSegmentsCheckFullTest()
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы для увеличения прогресса
			// Проверка, что прогресс увеличен (Assert внутри)
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);

			// Выйти
			Header.LogoutUser();
			// Зайти другим пользователем
			LoginUser(User2);

			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Добавить переводы
			AddNumOfTranslations(translationRowNum);
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			int lectureProgressBefore = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);

			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Удалить добавленные переводы
			DeleteMyTranslations(translationRowNum, numberSentencesGrowProgress);
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			int lectureProgressAfter = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);

			// Проверить, что общий прогресс не изменился
			Assert.AreEqual(lectureProgressBefore, lectureProgressAfter, "Ошибка: общий прогресс не должен меняться");
		}

		/// <summary>
		/// Тест: проверка, что если у сегмента все переводы с отрицательным рейтингом - сегмент не учитывается в прогрессе
		/// </summary>
		[Test]
		public void DecreaseProgressVoteDown()
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);
			Console.WriteLine("courseName: " + courseName);
			Console.WriteLine("lectureRowNumber: " + lectureRowNumber);
			Console.WriteLine("translationRowNum: " + translationRowNum);
			// Прогресс лекции
			int generalProgressBefore = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);
			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);
			for (int i = translationRowNum; i < (translationRowNum + numberSentencesGrowProgress); ++i)
			{
				EditorPage.ClickTargetByRowNumber(i);
				// Проголосовать против всех предложенных переводов для добавленных переводов лекции
				VoteSuggestedTranslations(false, false);
			}
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			// Прогресс после голосования против всех переводов
			int generalProgressAfter = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);
			Console.WriteLine("generalProgressAfter: " + generalProgressAfter);
			// Проверить, что прогресс уменьшился
			Assert.IsTrue(generalProgressAfter < generalProgressBefore, "Ошибка: прогресс лекции должен был уменьшиться");
		}



		/// <summary>
		/// Тест: если проголосовать за перевод, у которого отрицательный рейтинг (других переводов нет), прогресс увеличивается
		/// </summary>
		[Test]
		public void LectureProgressMakeTranslationsPositive()
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);
			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);
			for (int i = translationRowNum; i < (translationRowNum + numberSentencesGrowProgress); ++i)
			{
				EditorPage.ClickTargetByRowNumber(i);
				// Проголосовать против всех предложенных переводов для добавленных переводов лекции
				VoteSuggestedTranslations(false, false);
			}
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			// Прогресс после голосования против всех переводов
			int generalProgressBefore = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);

			Header.LogoutUser();
			LoginUser(User2);

			// Проголосовать за переводы (рейтинг будет 0)
			OpenLectureByRowNum(lectureRowNumber);
			for (int i = translationRowNum; i < (translationRowNum + numberSentencesGrowProgress); ++i)
			{
				EditorPage.ClickTargetByRowNumber(i);
				// Проголосовать за эти переводы (сделать рейтинг = 0)
				VoteSuggestedTranslations(true, false, true, 1);
			}
			ClickHomeEditor();

			Header.LogoutUser();
			LoginUser(TestUserList[0]);

			// Проголосовать за переводы (рейтинг будет > 0)
			OpenLectureByRowNum(lectureRowNumber);
			for (int i = translationRowNum; i < (translationRowNum + numberSentencesGrowProgress); ++i)
			{
				EditorPage.ClickTargetByRowNumber(i);
				// Проголосовать за эти переводы (сделать рейтинг положительным)
				VoteSuggestedTranslations(true, false, true, 1);
			}
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			// Прогресс после
			int generalProgressRatingPositive = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);
			Console.WriteLine("generalProgressRatingPositive: " + generalProgressRatingPositive);

			// Проверить, что прогресс увеличился
			Assert.IsTrue(generalProgressRatingPositive > generalProgressBefore, "Ошибка: прогресс лекции должен был увеличиться");
		}

		/// <summary>
		/// Тест: если проголосовать против перевода, но в этом же сегменте есть переводы с + рейтингом, то прогресс не меняется
		/// </summary>
		[Test]
		public void LectureProgressNegativeAndPositiveTranslations()
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);
			Console.WriteLine("courseName: " + courseName);
			Console.WriteLine("lectureRowNumber: " + lectureRowNumber);
			Console.WriteLine("translationRowNum: " + translationRowNum);

			Header.LogoutUser();
			LoginUser(User2);

			// Добавить переводы в те же сегменты
			OpenLectureByRowNum(lectureRowNumber);
			Console.WriteLine("добавляем переводы вторым пользователем");
			AddNumOfTranslations(translationRowNum);
			ClickHomeEditor();
			// Прогресс лекции
			int generalProgressBefore = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);
			Console.WriteLine("generalProgressBefore: " + generalProgressBefore);
			Console.WriteLine("голосуем");
			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);
			for (int i = translationRowNum; i < (translationRowNum + numberSentencesGrowProgress); ++i)
			{
				EditorPage.ClickTargetByRowNumber(i);
				// Проголосовать против перевода
				VoteSuggestedTranslations(false, false, true, 1);
			}
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			// Прогресс после голосования против всех переводов
			int generalProgressAfter = LecturePage.GetLectureGeneralProgressByNumber(lectureRowNumber);
			Console.WriteLine("generalProgressAfter: " + generalProgressAfter);
			// Проверить, что прогресс уменьшился
			Assert.AreEqual(generalProgressBefore, generalProgressAfter, "Ошибка: прогресс лекции должен был измениться");
		}

		/// <summary>
		/// Тест: если переводы пользователя получили голоса Против - личный  прогресс не меняется (не уменьшается)
		/// </summary>
		[Test]
		public void PersonalProgressNegativeTranslations()
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			// Личный прогресс
			int personalProgress = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);
			Console.WriteLine("personalProgress: " + personalProgress);

			Header.LogoutUser();
			LoginUser(User2);

			// Проголосовать против переводов
			OpenLectureByRowNum(lectureRowNumber);
			for (int i = translationRowNum; i < (translationRowNum + numberSentencesGrowProgress); ++i)
			{
				EditorPage.ClickTargetByRowNumber(i);
				// Проголосовать против переводов
				VoteSuggestedTranslations(false, false);
			}
			ClickHomeEditor();

			// Вернуться в первого пользователя
			Header.LogoutUser();
			LoginUser(User1);

			// Прогресс после
			int personalProgressAfter = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);
			Console.WriteLine("personalProgressAfter: " + personalProgressAfter);

			// Проверить, что прогресс не изменился
			Assert.AreEqual(personalProgress, personalProgressAfter, "Ошибка: личный прогресс изменился");
		}

		/// <summary>
		/// Проверить увеличение прогресса курса
		/// </summary>
		[Test]
		public void CourseProgressUp()
		{
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			decimal courseProgress;
			// Открыть курс с наименьшим прогрессом
			string courseName = SelectCourseMinProgress(out courseProgress);

			// TODO заменить
			//courseName = "The Emergence of the Modern Middle East";
			//courseName = "Machine Learning";
			courseProgress = CoursePage.GetCourseProcentByName(courseName);
			Console.WriteLine("курс: " + courseName);
			Console.WriteLine("прогресс: " + courseProgress);
			OpenCourseByName(courseName);

			// Выбрать пустую лекцию
			int lectureRowNum = SelectEmptyLectureGetRowNumber();
			OpenLectureByRowNum(lectureRowNum);
			int lastLastFactNum = 0;
			bool isLectureFinished = false;
			// Заполнить лекцию
			while (!isLectureFinished)
			{
				isLectureFinished = AddTraslationsVisibleSegments(ref lastLastFactNum);
			}
			// Выйти из лекции
			ClickHomeEditor();
			WaitUpdateTotal();

			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			decimal courseProgressAfter = CoursePage.GetCourseProcentByName(courseName);
			Console.WriteLine("прогресс после: " + courseProgressAfter);
			Assert.IsTrue(courseProgressAfter > courseProgress, "Ошибка: прогресс курса не увеличился");
		}

		/// <summary>
		/// Тест: проверка изменения количества переведенных страниц на главной странице
		/// </summary>
		[Test]
		public void TotalPagesHomepage()
		{
			// количество переведенных предложений до
			int totalPagesBefore = HomePage.GetTotalPages();

			// Заполнить лекцию (как в тесте на увеличение прогресса курса)
			CourseProgressUp();
			// Перейти на главную
			Header.OpenHomepage();
			// количество переведенных предложений после
			int totalPagesAfter = HomePage.GetTotalPages();

			// Проверить, что количество переведенных страниц увеличилось
			Assert.IsTrue(totalPagesAfter > totalPagesBefore, "Ошибка: количество переведенных страниц не увеличилось");
		}

		/// <summary>
		/// Тест: проверка изменения количества переведенных слов на главной странице
		/// </summary>
		[Test]
		public void TotalWordsHomepage()
		{
			// количество переведенных слов до
			int totalWordsBefore = HomePage.GetTotalWords();

			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить переводы
			AddTranslationsToGrowProgress(out courseName, out lectureRowNumber, out translationRowNum);
			WaitUpdateTotal();
			// Перейти на главную
			Header.OpenHomepage();
			// количество переведенных слов после
			int totalWordsAfter = HomePage.GetTotalWords();

			// Проверить, что количество переведенных слов увеличилось
			Assert.IsTrue(totalWordsAfter > totalWordsBefore, "Ошибка: количество переведенных слов не увеличилось");
		}



		/// <summary>
		/// Дождаться обновления прогресса
		/// </summary>
		private void WaitUpdateProgress()
		{
			// Задержка для ожидания изменения прогресса
			Thread.Sleep(15000);
			// Обновить страницу со список лекции
			Driver.Navigate().Refresh();
		}

		/// <summary>
		/// Дождаться обновления прогресса курса и данных на главной странице
		/// </summary>
		private void WaitUpdateTotal()
		{
			// Задержка для ожидания изменения прогресса
			Thread.Sleep(60000);
		}

		/// <summary>
		/// Добавить переводы для увеличения прогресса
		/// Проверить, что прогресс увеличился
		/// </summary>
		/// <param name="courseName">OUT: имя курса</param>
		/// <param name="lectureRowNumber">OUT: номер строки с лекцией</param>
		/// <param name="firstSegmentRowNumber">OUT: номер первого сегмента с добавленным переводом</param>
		protected void AddTranslationsToGrowProgress(out string courseName, out int lectureRowNumber, out int firstSegmentRowNumber)
		{
			// Перейти к списку доступных курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Переход в курс с наименьшим прогрессом
			courseName = OpenCourseMinProgress();
			// Перейти в лекцию
			lectureRowNumber = SelectEmptyLectureGetRowNumber();
			Console.WriteLine("лекция: " + lectureRowNumber);
			int personalProgress = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);

			// Проверить пустые сегменты в лекции
			//CheckEmptySegmentsLecture(lectureRowNumber, firstSegmentRowNumber);

			// Открыть лекцию
			OpenLectureByRowNum(lectureRowNumber);

			// Добавить переводы
			firstSegmentRowNumber = GetEmptyTranslationRowNumber();
			AddNumOfTranslations(firstSegmentRowNumber);
			ClickHomeEditor();

			// Общий прогресс после добавления переводов
			WaitUpdateProgress();
			// Проверить, что вернулись в список лекций
			Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");

			int personalProgressAfter = LecturePage.GetLecturePersonalProgressByNumber(lectureRowNumber);
			// Проверить, что прогресс увеличился
			Assert.IsTrue(personalProgressAfter > personalProgress, "Ошибка: прогресс не увеличился");
		}

		/// <summary>
		/// Добавить N переводов, начиная с firstSegmentRowNumber сегмента
		/// </summary>
		/// <param name="firstSegmentRowNumber">номер сегмента, с которого начинать добавление переводов</param>
		protected void AddNumOfTranslations(int firstSegmentRowNumber)
		{
			string translationText = "Test" + DateTime.Now.Ticks;
			// Добавить переводы
			for (int i = firstSegmentRowNumber; i < (firstSegmentRowNumber + numberSentencesGrowProgress); ++i)
			{
				AddTranslationByRowNum(i, translationText);
			}
		}

		/// <summary>
		/// Добавление переводов в сегменты, для которых нет вариантов перевода
		/// </summary>
		/// <returns>список номеров строк с сегментами, в которые добавили переводы</returns>
		protected List<int> AddNumOfTranslationsEmptySegments()
		{
			List<int> translatedRows = new List<int>();
			string translationText = "Test" + DateTime.Now.Ticks;
			int currentIndex = 0;
			// Добавить переводы
			for (int i = 0; i < numberSentencesGrowProgress; ++i)
			{
				currentIndex = i + 1;
				while (EditorPage.GetQuantityTranslationsByRowNumber(currentIndex) > 0)
				{
					++currentIndex;
					Assert.IsFalse(currentIndex > maxEditorLinesNum, "Ошибка: в лекции недостаточно сегментов без переводов");
				}
				AddTranslationByRowNum(currentIndex, translationText);
				translatedRows.Add(currentIndex);
			}
			return translatedRows;
		}
	}
}
