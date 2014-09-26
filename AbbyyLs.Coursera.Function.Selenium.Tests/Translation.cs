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
	class Translation : BaseTest
	{
		public Translation(string url, string workspaceUrl, string browserName)
			: base(url, workspaceUrl, browserName)
		{

		}

		[SetUp]
		public void Setup()
		{
		}

		/// <summary>
		/// Тест: добавление перевода в сегмент, где нет перевода пользователя
		/// Проверка, что количество переведенных предложений в профиле увеличилось
		/// </summary>
		[Test]
		public void AddTranslationNoTranslations()
		{
			// Зайти в профиль пользователя
			OpenUserProfileFromHomePage();

			// Получить количество переведенных предложений до добавления перевода
			int translationsNumberBefore = ProfilePage.GetUserTranslationsNumber();

			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			AddTranslation(translationText);
			// Вернуться из редактора
			ClickHomeEditor();
			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить количество переведенных предложений после добавления перевода
			int translationsNumberAfter = ProfilePage.GetUserTranslationsNumber();
			// Проверить, что рейтинг увеличился
			Assert.IsTrue(translationsNumberAfter > translationsNumberBefore,
				"Ошибка: после добавления перевода количество переведенных предложений пользователя не увеличилось");
		}

		/// <summary>
		/// Тест: добавление перевода в сегмент, где уже есть перевод пользователя
		/// Проверка, что количество переведенных предложений в профиле увеличилось
		/// </summary>
		[Test]
		public void AddTranslationExistTranslations()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);

			// Вернуться из редактора
			ClickHomeEditor();
			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить количество переведенных предложений до добавления перевода
			int translationsNumberBefore = ProfilePage.GetUserTranslationsNumber();

			// Перейти к списку курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Зайти в курс
			OpenCourseByName(courseName);
			// Перейти в лекцию
			OpenLectureByRowNum(lectureRowNumber);
			string newTranslationText = "Test " + DateTime.Now.Ticks;

			// Добавить новый перевод в ту же ячейку
			AddTranslationByRowNum(translationRowNum, newTranslationText);
			// Вернуться из редактора
			ClickHomeEditor();
			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить количество переведенных предложений до добавления перевода
			int translationsNumberAfter = ProfilePage.GetUserTranslationsNumber();

			// Проверить, что количество переводов увеличилось
			Assert.IsTrue(translationsNumberAfter > translationsNumberBefore, "Ошибка: количество переводов не увеличилось");
		}

		/// <summary>
		/// Тест: добавление перевода в сегмент, где уже есть перевод другого пользователя
		/// Проверка, что количество переведенных предложений в профиле увеличилось
		/// </summary>
		[Test]
		public void AddTranslationExistTranslationsAnotherUsers()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);

			// Вернуться из редактора
			ClickHomeEditor();
			// Выйти из пользователя
			Header.LogoutUser();
			// Зайти другим пользователем
			LoginUser(User2);

			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить количество переведенных предложений до добавления перевода
			int translationsNumberBefore = ProfilePage.GetUserTranslationsNumber();

			// Перейти к списку курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Зайти в курс
			OpenCourseByName(courseName);
			// Перейти в лекцию
			OpenLectureByRowNum(lectureRowNumber);
			string newTranslationText = "Test " + DateTime.Now.Ticks;

			// Добавить новый перевод в ту же ячейку
			AddTranslationByRowNum(translationRowNum, newTranslationText);
			// Вернуться из редактора
			ClickHomeEditor();
			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить количество переведенных предложений до добавления перевода
			int translationsNumberAfter = ProfilePage.GetUserTranslationsNumber();

			// Проверить, что количество переводов увеличилось
			Assert.IsTrue(translationsNumberAfter > translationsNumberBefore, "Ошибка: количество переводов не увеличилось");
		}

		/// <summary>
		/// Тест: удаление перевода
		/// Проверка, что количество переведенных предложений в профиле уменьшилось
		/// </summary>
		[Test]
		public void DeleteTranslation()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);

			// Вернуться из редактора
			ClickHomeEditor();
			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить количество переведенных предложений до добавления перевода
			int translationsNumberBefore = ProfilePage.GetUserTranslationsNumber();

			// Перейти к списку курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Зайти в курс
			OpenCourseByName(courseName);
			// Перейти в лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Кликнуть по ячейке
			EditorPage.ClickTargetByRowNumber(translationRowNum);
			// Удалить перевод
			EditorPage.DeleteTranslation();
			// Вернуться из редактора
			ClickHomeEditor();
			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить количество переведенных предложений до добавления перевода
			int translationsNumberAfter = ProfilePage.GetUserTranslationsNumber();

			// Проверить, что количество переводов уменьшилось
			Assert.IsTrue(translationsNumberAfter < translationsNumberBefore, "Ошибка: количество переводов не уменьшилось");
		}

		/// <summary>
		/// Тест: редактирование перевода
		/// Проверка, что количество переведенных предложений в профиле не изменилось
		/// </summary>
		[Test]
		public void EditTranslation()
		{
			// Добавить перевод
			string translationText = "Test" + DateTime.Now.Ticks;
			string courseName;
			int lectureRowNumber, translationRowNum;
			AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNum);

			// Вернуться из редактора
			ClickHomeEditor();
			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить количество переведенных предложений до добавления перевода
			int translationsNumberBefore = ProfilePage.GetUserTranslationsNumber();

			// Перейти к списку курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
			// Зайти в курс
			OpenCourseByName(courseName);
			// Перейти в лекцию
			OpenLectureByRowNum(lectureRowNumber);
			// Кликнуть по ячейке
			EditorPage.ClickTargetByRowNumber(translationRowNum);
			// Кликнуть редактировать перевод
			int rowNum = EditorPage.GetTranslationRowNumberByTarget(translationText);
			EditorPage.ClickEditTranslationByRowNumber(rowNum);

			// Ввести текст
			EditorPage.AddTextTargetByRowNumber(translationRowNum, DateTime.Now.ToString());

			// Кликнуть Confirm
			EditorPage.ClickConfirmBtn();

			// Дождаться Confirm
			AssertConfirmIsDone(translationRowNum);
			// Вернуться из редактора
			ClickHomeEditor();
			// Зайти в профиль
			OpenUserProfileFromCourse();

			// Получить количество переведенных предложений до добавления перевода
			int translationsNumberAfter = ProfilePage.GetUserTranslationsNumber();

			// Проверить, что количество переводов не изменилось
			Assert.AreEqual(translationsNumberBefore, translationsNumberAfter, "Ошибка: количество переводов изменилось");
		}

		/// <summary>
		/// Тест: добавление нескольких переводов в рендомные лекции каждого курса
		/// </summary>
		[Test]
		public void AddRandomTranslationEveryCource()
		{
			string translationText = "Test" + DateTime.Now.Ticks;
			Random randomLec = new Random();
			Random randomSeg = new Random();
			// Количество лекций из курса
			int lec = 4;
			// Количество сегментов из лекции
			int seg = 3;

			// Перейти к списку доступных курсов
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");

			// Получить список всех курсов
			List<string> coursList =  CoursePage.GetCoursesNameList();

			foreach(string course in coursList)
			{
				Console.WriteLine("Открытие курса: " + course);
				// Открыть лекцию
				OpenCourseByName(course);
				
				// Получить список лекций
				List<string> lectureList = LecturePage.GetLecturesNameList();
				int lecCount = lectureList.Count;

				for (int i = 0; i < lec; i++)
				{
					// Получение диапазона лекций
					int minLec = (lecCount * i / lec);
					int maxLec = (lecCount * (i + 1) / lec) - 1;
					int lectureNum = randomLec.Next(minLec, maxLec);
					
					Console.WriteLine("Открытие лекции: " + lectureList[lectureNum]);
					// Открытие лекции
					OpenLectureByRowNum(lectureNum + 1);
					Thread.Sleep(3000);

					// Получение количества сегментов
					int segCount = EditorPage.GetSegmentsCount();
					Console.WriteLine("Всего сегментов в лекции: " + segCount.ToString());

					// Добавление перевода в первый сегмент
					AddTranslationByPosition(1, translationText);

					for (int j = 0; j < seg; j++)
					{
						// Получение диапазона сегментов
						int minSeg = (segCount * j / seg) + 1;
						int maxSeg = segCount * (j + 1) / seg;
						int segmentNum = randomSeg.Next(minSeg, maxSeg);

						// Добавление перевода
						AddTranslationByPosition(segmentNum, translationText);
					}

					// Добавление перевода в последний сегмент
					AddTranslationByPosition(segCount, translationText);

					// Выход из редактора
					ClickHomeEditor();
				}

				// Выход из курса
				OpenCoursePage();
			}
		}



		private void AddTranslationByPosition(int positionNum, string text)
		{
			Console.WriteLine("Добавление перевода в сегмент №" + positionNum);
			
			// Добавление перевода
			EditorPage.AddTextTargetByPosition(positionNum, text);
			EditorPage.ClickConfirmBtn();
			// Дождаться Confirm
			Assert.IsTrue(EditorPage.WaitUntilDisappearBorderByPosition(positionNum), "Ошибка: рамка вокруг галочки не пропадает - Confirm не прошел");

			Console.WriteLine(" OK");
		}
	}
}
