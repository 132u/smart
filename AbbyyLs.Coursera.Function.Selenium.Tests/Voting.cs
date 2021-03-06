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
	class Voting : BaseTest
	{
		public Voting(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Тест: Проверка, что при добавлении перевода За него автоматически ставится голос.
		/// - проверка, что кнопка "Голосовать За" заблокирована
		/// - проверка, что рейтинг перевода больше 0
		/// </summary>
		[Test]
		public void AutoVoteOneself()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			AddTranslation(translationText);
			// Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
			int rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);
			// Получить количество голосов за этот перевод
			int translationRating = EditorPage.GetTranslationRatingByRowNumber(rowNumber);
			// Проверить, что иконка "Голосовать За" заблокирована - т.е. голос поставлен автоматически
			Assert.IsTrue(EditorPage.GetIsVoteConsidered(true, rowNumber),
				"Ошибка: кнопка Голосовать За не заблокирована");
			// Проверить, что рейтинг больше нуля
			Assert.IsTrue(translationRating > 0, "Ошибка: рейтинг должен быть больше нуля, автоматический голос за себя не поставлен");
		}

		/// <summary>
		/// Тест: Проверка голосования из редактора: За, Против, За
		/// - проверка, что при голосовании За, рейтинг увеличивается +1
		/// - проверка, что про повторном голосовании За, рейтинг не меняется
		/// - проверка, что после выхода и повторого входа в редактор после голосования За, рейтинг не меняется
		/// - проверка, что после голосования Против, рейтинг меняется на -2
		/// - проверка, что после госования За, рейтинг меняется +2
		/// </summary>
		[Test]
		public void EditorVoteUpDownUp()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			// Выйти из редактора
			ClickHomeEditor();
			// Выйти из этого пользователя
			Header.LogoutUser();

			// Зайти другим пользователем
			LoginUser(User2);
			// Зайти в лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Перейти в строку с добавленным переводом
			EditorPage.ClickTargetByRowNumber(translationRowNum);
			// Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
			int rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);

			// Получить количество голосов за этот перевод
			int ratingBefore = EditorPage.GetTranslationRatingByRowNumber(rowNumber);
			// Проголосовать За
			VoteFromEditor(true, rowNumber);
			rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);
			int ratingAfter = EditorPage.GetTranslationRatingByRowNumber(rowNumber);
			// Проверить, что рейтинг увеличился на 1
			Assert.IsTrue(ratingAfter == (ratingBefore + 1), "Ошибка: (в редакторе, голосование за перевод) рейтинг после голоса ЗА не увеличился на 1");
			ratingBefore = ratingAfter;

			// Повторно проголосовать За
			VoteFromEditor(true, rowNumber);
			rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);
			ratingAfter = EditorPage.GetTranslationRatingByRowNumber(rowNumber);
			// Проверить, что рейтинг не изменился
			Assert.IsTrue(ratingAfter == ratingBefore, "Ошибка: (в редакторе, повторное голосование ЗА) рейтинг не должен был измениться");

			// Выйти из редактора и зайти снова
			ClickHomeEditor();
			// Зайти в лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Перейти в строку с добавленным переводом
			EditorPage.ClickTargetByRowNumber(translationRowNum);
			// Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
			rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);
			// Получить количество голосов за этот перевод
			ratingBefore = EditorPage.GetTranslationRatingByRowNumber(rowNumber);
			// Проголосовать За
			VoteFromEditor(true, rowNumber);
			rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);
			ratingAfter = EditorPage.GetTranslationRatingByRowNumber(rowNumber);
			// Проверить, что рейтинг не изменился
			Assert.IsTrue(ratingAfter == ratingBefore, "Ошибка: (в редакторе, проголосовали ЗА, вышли из редактора, зашли снова, проголосовали ЗА) рейтинг не должен был измениться");
			ratingBefore = ratingAfter;

			// Проголосовать Против
			VoteFromEditor(false, rowNumber);
			rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);
			ratingAfter = EditorPage.GetTranslationRatingByRowNumber(rowNumber);
			// Проверить, что рейтинг изменился на -2
			Assert.IsTrue(ratingAfter == (ratingBefore - 2), "Ошибка: (в редакторе, проголосовали ЗА, проголосовали ПРОТИВ) рейтинг должен измениться на -2");
			ratingBefore = ratingAfter;
			// Проголосовать За
			VoteFromEditor(true, rowNumber);
			rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);
			ratingAfter = EditorPage.GetTranslationRatingByRowNumber(rowNumber);
			// Проверить, что рейтинг изменился на +2
			Assert.IsTrue(ratingAfter == (ratingBefore + 2), "Ошибка: (в редакторе, проголосовали ПРОТИВ, проголосовали ЗА) рейтинг должен измениться на +2");
		}

		/// <summary>
		/// Тест: Проверка голосования из редактора: За
		/// - проверка, что количество голосов За у пользователя увеличилось
		/// - проверка, что количество голосов Против не изменилось
		/// </summary>
		[Test]
		public void EditorVoteUpCheckUserProfile()
		{
			Decimal userRatingBefore, userRatingAfter;
			int numVotesUpBefore, numVotesUpAfter, numVotesDownBefore, numVotesDownAfter, translationsNumberBefore, translationsNumberAfter;
			// Добавить перевод, проголосовать, проверить информацию в профиле
			VoteCheckUserProfile(true, true, out userRatingBefore, out userRatingAfter,
								out numVotesUpBefore, out numVotesUpAfter, out numVotesDownBefore, out numVotesDownAfter,
								out translationsNumberBefore, out translationsNumberAfter);

			bool isError = false;
			string errorMessage = "/n";
			// Проверить, что количество голосов За увеличилось
			if (numVotesUpAfter <= numVotesUpBefore)
			{
				isError = true;
				errorMessage += "Ошибка: не увеличилось количество голосов За\n";
			}
			// Проверить, что количество голосов Против осталось тем же
			if (numVotesDownAfter != numVotesDownBefore)
			{
				isError = true;
				errorMessage += "Ошибка: изменилось количество голосов Против\n";
			}

			Assert.IsFalse(isError, errorMessage);
		}

		/// <summary>
		/// Тест: Проверка голосования из редактора: Против
		/// - проверка, что количество голосов За у пользователя не изменилось
		/// - проверка, что количество голосов Против увеличилось
		/// </summary>
		[Test]
		public void EditorVoteDownCheckUserProfile()
		{
			Decimal userRatingBefore, userRatingAfter;
			int numVotesUpBefore, numVotesUpAfter, numVotesDownBefore, numVotesDownAfter, translationsNumberBefore, translationsNumberAfter;
			// Добавить перевод, проголосовать, проверить информацию в профиле
			VoteCheckUserProfile(true, false, out userRatingBefore, out userRatingAfter,
								out numVotesUpBefore, out numVotesUpAfter, out numVotesDownBefore, out numVotesDownAfter,
								out translationsNumberBefore, out translationsNumberAfter);

			bool isError = false;
			string errorMessage = "/n";
			// Проверить, что количество голосов За не изменилось
			if (numVotesUpAfter != numVotesUpBefore)
			{
				isError = true;
				errorMessage += "Ошибка: изменилось количество голосов За\n";
			}
			// Проверить, что количество голосов Против увеличилось
			if (numVotesDownAfter <= numVotesDownBefore)
			{
				isError = true;
				errorMessage += "Ошибка: не увеличилось количество голосов Против\n";
			}

			Assert.IsFalse(isError, errorMessage);
		}

		/// <summary>
		/// Тест: Проверка голосования из редактора: ЗА, проверка списка последних событий (событие появляется)
		/// </summary>
		[Test]
		public void EditorVoteUpCheckLastEvents()
		{
			// Текст перевода
			string translationText = "Test" + DateTime.Now.Ticks;
			String lastEvTarget, lastEvAuthor;
			HomePageLastEventType lastEvType;
			// Добавить перевод, проголосовать, вернуться к списку событий
			EditorVoteGetLastEventBeforeVoting(true, translationText, out lastEvTarget, out lastEvAuthor, out lastEvType);
			// Дождаться появления события о голосовании
			bool isPresentTranslation = WaitEventInEventListByTarget(translationText, HomePageLastEventType.VoteUpEvent);
			if (!isPresentTranslation)
			{
				// Если событие не появилось - ошибка
				if (GetIsExistInLastEventList(lastEvTarget, lastEvType, lastEvAuthor))
				{
					// Последнее событие есть в списке, значит, событие о голосовании не появилось
					Assert.Fail("Ошибка: голосование не появилось в списке последних событий");
				}
				else
				{
					// Последнее событие пропало из списка, возможно, событие о голосовании уже ушло из списка
					Assert.Fail("Список событий обновился, возможно, событие о голосовании было в списке, но уже ушло из него");
				}
			}
		}

		/// <summary>
		/// Тест: Проверка голосования из редактора: Против, проверка списка последних событий (событие не появляется)
		/// </summary>
		[Test]
		public void EditorVoteDownCheckLastEvents()
		{
			// Текст перевода
			string translationText = "Test" + DateTime.Now.Ticks;
			String lastEvTarget, lastEvAuthor;
			HomePageLastEventType lastEvType;
			// Добавить перевод, проголосовать, вернуться к списку событий
			EditorVoteGetLastEventBeforeVoting(false, translationText, out lastEvTarget, out lastEvAuthor, out lastEvType);

			// Проверить, есть ли событие в списке
			bool isPresentTranslation = WaitEventInEventListByTarget(translationText, HomePageLastEventType.VoteUpEvent);
			if (isPresentTranslation)
			{
				// Если событие не появилось - все верно
				if (GetIsExistInLastEventList(lastEvTarget, lastEvType, lastEvAuthor))
				{
					// Последнее событие есть в списке, значит, событие о голосовании не появилось - все верно
					Assert.Pass();
				}
				else
				{
					// Последнее событие пропало из списка, возможно, событие о голосовании уже ушло из списка
					Assert.Fail("Список событий обновился, возможно, событие о голосовании было в списке, но уже ушло из него");
				}
			}
		}

		/// <summary>
		/// Тест: проверка, что при голосовании Против после голосования За, событие о голосовании За пропадает из списка последних событий
		/// </summary>
		[Test]
		public void EditorVoteUpDownCheckLastEvents()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			// Выйти из редактора
			ClickHomeEditor();
			// Выйти из этого пользователя
			Header.LogoutUser();

			// Зайти другим пользователем
			LoginUser(User2);
			// Зайти в курс
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			OpenCourseByName(courseName);
			// Зайти в лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Перейти в строку с добавленным переводом
			EditorPage.ClickTargetByRowNumber(translationRowNum);
			// Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
			int rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);

			// Проголосовать За
			VoteFromEditor(true, rowNumber);
			ClickHomeEditor();
			// Перейти на главную страницу
			Header.OpenHomepage();
			Assert.IsTrue(WaitEventInEventListByTarget(translationText, HomePageLastEventType.VoteUpEvent), "Ошибка: событие о голосовании не появилось");
			// Зайти в курс
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			OpenCourseByName(courseName);
			// Зайти в лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Перейти в строку с добавленным переводом
			EditorPage.ClickTargetByRowNumber(translationRowNum);
			// Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
			rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);

			// Проголосовать против
			VoteFromEditor(false, rowNumber);
			ClickHomeEditor();
			// Перейти на главную страницу
			Header.OpenHomepage();
			Assert.IsFalse(WaitEventInEventListByTarget(translationText, HomePageLastEventType.VoteUpEvent, false), "Ошибка: событие должно пропасть");
		}

		/// <summary>
		/// Тест: Проверка голосования из списка последних событий: За, Против, За
		/// </summary>
		[Test]
		public void LastEventsVoteUpDownUp()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			// Выйти из редактора
			ClickHomeEditor();
			// Выйти из этого пользователя
			Header.LogoutUser();

			// Зайти другим пользователем
			LoginUser(User2);
			// Перейти на главную страницу
			Header.OpenHomepage();
			// Получить номер строки с событием
			int rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
			// Получить число голосов до голосования
			int ratingBefore = HomePage.GetEventRatingByRowNumber(rowNumber);

			// Проголосовать
			Assert.True(HomePage.GetIsCanVoteEventListByRowNumber(rowNumber, true), "Ошибка: не удалось проголосовать");
			// Получить число голосов после голосования
			int ratingAfter = HomePage.GetEventRatingByRowNumber(rowNumber);
			// Сравнить количество голосов
			Assert.IsTrue(ratingBefore < ratingAfter, "Ошибка: количество голосов не увеличилось");
			ratingBefore = ratingAfter;

			// Проверить, что нельзя проголосовать снова
			Assert.IsFalse(HomePage.GetIsCanVoteEventListByRowNumber(rowNumber, true), "Ошибка: нельзя голосовать дважды");
			// Попробовать проголосовать
			HomePage.VoteVotedEventListByRowNumber(rowNumber, true);
			Thread.Sleep(1000);
			// Проверить, что количество голосов не изменилось
			ratingAfter = HomePage.GetEventRatingByRowNumber(rowNumber);
			Assert.IsTrue(ratingBefore == ratingAfter, "Ошибка: после повторного голосования рейтинг не должен был измениться");

			// Обновить страницу (дождаться, пока появится событие, что проголосовали)
			WaitEventInEventListByTarget(translationText, HomePageLastEventType.VoteUpEvent);
			// Найти нужное событие
			rowNumber = GetEventRowNum(translationText, HomePageLastEventType.VoteUpEvent);
			ratingBefore = HomePage.GetEventRatingByRowNumber(rowNumber);

			// Проголосовать
			HomePage.GetIsCanVoteEventListByRowNumber(rowNumber, true);
			ratingAfter = HomePage.GetEventRatingByRowNumber(rowNumber);
			// Проверить, что число голосов не изменилось
			Assert.IsTrue(ratingBefore == ratingAfter, "Ошибка: число голосов не должно было измениться");

			// Проголосовать Против
			Assert.IsTrue(HomePage.GetIsCanVoteEventListByRowNumber(rowNumber, false), "Ошибка: недоступно голосование против");
			ratingAfter = HomePage.GetEventRatingByRowNumber(rowNumber);
			// Проверить, что количество голосов изменилось
			Assert.IsTrue(ratingAfter == ratingBefore - 2, "Ошибка: число голосов должно было измениться на -2");
			ratingBefore = ratingAfter;

			// Проголосовать За
			Assert.IsTrue(HomePage.GetIsCanVoteEventListByRowNumber(rowNumber, true), "Ошибка: недоступно голосование За");
			ratingAfter = HomePage.GetEventRatingByRowNumber(rowNumber);
			// Проверить, что количество голосов изменилось
			Assert.IsTrue(ratingAfter == ratingBefore + 2, "Ошибка: число голосов должно было измениться на +2");
		}

		/// <summary>
		/// Тест: Проверка голосования из списка последних событий: За
		/// </summary>
		[Test]
		public void LastEventsVoteUpCheckUserProfile()
		{
			Decimal userRatingBefore, userRatingAfter;
			int numVotesUpBefore, numVotesUpAfter, numVotesDownBefore, numVotesDownAfter, translationsNumberBefore, translationsNumberAfter;
			// Добавить перевод, проголосовать, проверить информацию в профиле
			VoteCheckUserProfile(false, true, out userRatingBefore, out userRatingAfter,
								out numVotesUpBefore, out numVotesUpAfter, out numVotesDownBefore, out numVotesDownAfter,
								out translationsNumberBefore, out translationsNumberAfter);

			bool isError = false;
			string errorMessage = "/n";
			// Проверить, что количество голосов За увеличилось
			if (numVotesUpAfter <= numVotesUpBefore)
			{
				isError = true;
				errorMessage += "Ошибка: не увеличилось количество голосов За\n";
			}
			// Проверить, что количество голосов Против осталось тем же
			if (numVotesDownAfter != numVotesDownBefore)
			{
				isError = true;
				errorMessage += "Ошибка: изменилось количество голосов Против\n";
			}

			Assert.IsFalse(isError, errorMessage);
		}

		/// <summary>
		/// Тест: Проверка голосования из списка последних событий: Против
		/// </summary>
		[Test]
		public void LastEventsVoteDownCheckUserProfile()
		{
			Decimal userRatingBefore, userRatingAfter;
			int numVotesUpBefore, numVotesUpAfter, numVotesDownBefore, numVotesDownAfter, translationsNumberBefore, translationsNumberAfter;
			// Добавить перевод, проголосовать, проверить информацию в профиле
			VoteCheckUserProfile(false, false, out userRatingBefore, out userRatingAfter,
								out numVotesUpBefore, out numVotesUpAfter, out numVotesDownBefore, out numVotesDownAfter,
								out translationsNumberBefore, out translationsNumberAfter);

			bool isError = false;
			string errorMessage = "/n";
			// Проверить, что количество голосов За не изменилось
			if (numVotesUpAfter != numVotesUpBefore)
			{
				isError = true;
				errorMessage += "Ошибка: изменилось количество голосов За\n";
			}
			// Проверить, что количество голосов Против увеличилось
			if (numVotesDownAfter <= numVotesDownBefore)
			{
				isError = true;
				errorMessage += "Ошибка: не увеличилось количество голосов Против\n";
			}

			Assert.IsFalse(isError, errorMessage);
		}

		/// <summary>
		/// Тест: Проверка голосования из списка последних событий: ЗА, проверка списка последних событий (событие появляется)
		/// </summary>
		[Test]
		public void LastEventsVoteUpCheckLastEvents()
		{
			// Текст перевода
			string translationText = "Test" + DateTime.Now.Ticks;
			// Добавить перевод, проголосовать
			LastEventsVoteGetLastEventBeforeVoting(true, translationText);
			// Обновить страницу (дождаться, пока появится событие, что проголосовали)
			Assert.IsTrue(WaitEventInEventListByTarget(translationText, HomePageLastEventType.VoteUpEvent), "Ошибка: событие о голосовании не появилось");
		}

		/// <summary>
		/// Тест: Проверка голосования из списка последних событий: Против, проверка списка последних событий (событие не появляется)
		/// </summary>
		[Test]
		public void LastEventsVoteDownCheckLastEvents()
		{
			// Текст перевода
			string translationText = "Test" + DateTime.Now.Ticks;
			// Добавить перевод, проголосовать
			LastEventsVoteGetLastEventBeforeVoting(false, translationText);
			// Обновить страницу (проверить, что событие так и не появляется)
			Assert.IsFalse(WaitEventInEventListByTarget(translationText, HomePageLastEventType.VoteUpEvent), "Ошибка: событие о голосовании не появилось");
		}

		/// <summary>
		/// Тест: проверка, что при голосовании Против после голосования За, событие о голосовании За пропадает из списка последних событий
		/// </summary>
		[Test]
		public void LastEventsVoteUpDownCheckLastEvents()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			// Выйти из редактора
			ClickHomeEditor();
			// Выйти из этого пользователя
			Header.LogoutUser();

			// Зайти другим пользователем
			LoginUser(User2);
			// Перейти на главную
			Header.OpenHomepage();

			// Проголосовать За
			int rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
			Assert.IsTrue(HomePage.GetIsCanVoteEventListByRowNumber(rowNumber, true), "Ошибка: не удалось проголосовать");
			// Обновить страницу (дождаться, пока появится событие, что проголосовали)
			Assert.IsTrue(WaitEventInEventListByTarget(translationText, HomePageLastEventType.VoteUpEvent), "Ошибка: событие о голосовании не появилось");

			// Проголосовать против
			rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
			Assert.IsTrue(HomePage.GetIsCanVoteEventListByRowNumber(rowNumber, false), "Ошибка: не удалось проголосовать");
			Assert.IsFalse(WaitEventInEventListByTarget(translationText, HomePageLastEventType.VoteUpEvent, false), "Ошибка: событие о голосовании ЗА не пропало");
		}



		/// <summary>
		/// Проголосовать из редактора, вернуть последнее событие до голосования
		/// </summary>
		/// <param name="isVoteUp">тип голоса (true: За, false: Против)</param>
		/// <param name="translationText">текст перевода</param>
		/// <param name="lastEvTarget">OUT: target последнего события</param>
		/// <param name="lastEvAuthor">OUT: автор последнего события</param>
		/// <param name="lastEvType">OUT: тип последнего события</param>
		private void EditorVoteGetLastEventBeforeVoting(bool isVoteUp, string translationText, out String lastEvTarget, out String lastEvAuthor, out HomePageLastEventType lastEvType)
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			// Добавить перевод
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			// Выйти из редактора
			ClickHomeEditor();
			// Выйти из этого пользователя
			Header.LogoutUser();

			// Зайти другим пользователем
			LoginUser(User2);
			// Перейти на главную
			Header.OpenHomepage();
			// Получить последнее действие
			GetLastEventInfo(out lastEvTarget, out lastEvType, out lastEvAuthor);

			// Зайти в курс
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			OpenCourseByName(courseName);
			// Зайти в лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Перейти в строку с добавленным переводом
			EditorPage.ClickTargetByRowNumber(translationRowNum);
			// Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
			int rowNumber = EditorPage.GetTranslationRowNumberByTarget(translationText);

			// Проголосовать
			VoteFromEditor(isVoteUp, rowNumber);

			// Выйти из редактора
			ClickHomeEditor();
			// Перейти на главную страницу
			Header.OpenHomepage();
			Thread.Sleep(2000);
		}

		/// <summary>
		/// Проголосовать в списке последних событий, вернуть последнее событие до голосования
		/// </summary>
		/// <param name="isVoteUp">тип голоса</param>
		/// <param name="translationText">текст перевода</param>
		private void LastEventsVoteGetLastEventBeforeVoting(bool isVoteUp, string translationText)
		{
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);
			// Выйти из редактора
			ClickHomeEditor();
			// Выйти из этого пользователя
			Header.LogoutUser();

			// Зайти другим пользователем
			LoginUser(User2);
			// Перейти на главную
			Header.OpenHomepage();

			// Проголосовать
			int rowNumber = GetEventRowNum(translationText, HomePageLastEventType.AddTranslationEvent);
			Assert.IsTrue(HomePage.GetIsCanVoteEventListByRowNumber(rowNumber, isVoteUp), "Ошибка: не удалось проголосовать");
		}
	}
}
