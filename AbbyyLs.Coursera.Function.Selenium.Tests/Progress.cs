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
    class Progress : BaseTest
    {
        public Progress(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
            Driver.Manage().Window.Maximize();
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
            int personalProgressBefore = GetPersonalProgress(lectureRowNumber);
            int lectureProgressBefore = GetLectureProgress(lectureRowNumber);

            // Открыть лекцию
            OpenLectureByRowNum(lectureRowNumber);
            
            // Добавить переводы
            AddNumOfTranslations(translationRowNum);
            // Нажать выход
            ClickBackEditor();

            // Прогресс после повторного добавления
            WaitUpdateProgress();
            int personalProgressAfter = GetPersonalProgress(lectureRowNumber);
            int lectureProgressAfter = GetLectureProgress(lectureRowNumber);

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
            LogoutUser();
            // Зайти под другим пользователем
            LoginUser(User2);

            // Прогресс до добавления переводов
            int personalProgressBefore = GetPersonalProgress(lectureRowNumber);
            int lectureProgressBefore = GetLectureProgress(lectureRowNumber);

            // Открыть лекцию
            OpenLectureByRowNum(lectureRowNumber);
            // Добавить переводы
            AddNumOfTranslations(translationRowNum);
            // Нажать выход
            ClickBackEditor();

            // Прогресс после добавления
            WaitUpdateProgress();
            int personalProgressAfter = GetPersonalProgress(lectureRowNumber);
            int lectureProgressAfter = GetLectureProgress(lectureRowNumber);

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
            int personalProgressBefore = GetPersonalProgress(lectureRowNumber);
            // Открыть лекцию
            OpenLectureByRowNum(lectureRowNumber);
            // Удалить переводы
            DeleteTranslations(translationRowNum, numberSentencesGrowProgress);
            ClickBackEditor();

            // Прогресс после удаления
            WaitUpdateProgress();
            int personalProgressAfter = GetPersonalProgress(lectureRowNumber);
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
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            string courseName = OpenCourseMinProgress();
            // Найти лекцию с пустым прогрессом
            int lectureRowNumber = SelectEmptyLectureGetRowNumber();
            // Общий прогресс до добавления
            int lectureProgressBefore = GetLectureProgress(lectureRowNumber);
            OpenLectureByRowNum(lectureRowNumber);

            // Добавить переводы в сегменты, в которых нет переводов
            List<int> translatedRows = AddNumOfTranslationsEmptySegments();
            ClickBackEditor();

            // Общий прогресс после добавления переводов
            WaitUpdateProgress();
            int lectureProgressAfter = GetLectureProgress(lectureRowNumber);
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
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            string courseName = OpenCourseMinProgress();
            // Найти лекцию с пустым прогрессом
            int lectureRowNumber = SelectEmptyLectureGetRowNumber();
            OpenLectureByRowNum(lectureRowNumber);

            // Добавить переводы в сегменты, в которых нет переводов
            List<int> translatedRows = AddNumOfTranslationsEmptySegments();
            ClickBackEditor();

            // Общий прогресс после добавления переводов
            WaitUpdateProgress();
            int lectureProgressBefore = GetLectureProgress(lectureRowNumber);
            // Открыть лекцию
            OpenLectureByRowNum(lectureRowNumber);
            // Удалить добавленные переводы
            DeleteTranslations(translatedRows);
            ClickBackEditor();

            // Общий прогресс после удаления переводов
            WaitUpdateProgress();
            int lectureProgressAfter = GetLectureProgress(lectureRowNumber);
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
            LogoutUser();
            // Зайти другим пользователем
            LoginUser(User2);
            
            // Открыть лекцию
            OpenLectureByRowNum(lectureRowNumber);
            // Добавить переводы
            AddNumOfTranslations(translationRowNum);
            ClickBackEditor();

            // Общий прогресс после добавления переводов
            WaitUpdateProgress();
            int lectureProgressBefore = GetLectureProgress(lectureRowNumber);

            // Открыть лекцию
            OpenLectureByRowNum(lectureRowNumber);
            // Удалить добавленные переводы
            DeleteTranslations(translationRowNum, numberSentencesGrowProgress);
            ClickBackEditor();

            // Общий прогресс после удаления переводов
            WaitUpdateProgress();
            int lectureProgressAfter = GetLectureProgress(lectureRowNumber);

            // Проверить, что общий прогресс не изменился
            Assert.AreEqual(lectureProgressBefore, lectureProgressAfter, "Ошибка: общий прогресс не должен меняться");
        }

        /// <summary>
        /// Дождаться обновления прогресса
        /// </summary>
        private void WaitUpdateProgress()
        {
            // Задержка для ожидания изменения прогресса
            Thread.Sleep(15000);
            // Обновить страницу со список лекции
            Driver.FindElement(By.XPath(".//tbody[contains(@data-bind,'lectures')]")).SendKeys(OpenQA.Selenium.Keys.F5);
        }
        
        /// <summary>
        /// Получить личный прогресс для лекции
        /// </summary>
        /// <param name="lectureRowNumber">номер строки с лекцией</param>
        /// <returns>личный прогресс</returns>
        protected int GetPersonalProgress(int lectureRowNumber)
        {
            return int.Parse(Driver.FindElement(By.XPath(
                ".//tbody[contains(@data-bind,'lectures')]//tr[" + lectureRowNumber + "]//div[contains(@data-bind,'personalProgressView')]")).Text.Replace("%","").Trim());
        }

        /// <summary>
        /// Получить общий прогресс для лекции
        /// </summary>
        /// <param name="lectureRowNumber">номер строки с лекцией</param>
        /// <returns>общий прогресс</returns>
        protected int GetLectureProgress(int lectureRowNumber)
        {
            return int.Parse(Driver.FindElement(By.XPath(
                ".//tbody[contains(@data-bind,'lectures')]//tr[" + lectureRowNumber + "]//div[contains(@data-bind,'progressView')]")).Text.Replace("%", "").Trim());
        }

        /// <summary>
        /// Проверить, что достаточно пустых сегментов для заполнения лекции
        /// Если недостаточно - очистка лекции и проверка, что прогресс равен нулю после очистки
        /// </summary>
        /// <param name="lectureRowNumber">номер строки с лекцией</param>
        /// <param name="translationRowNum">номер строки с пустым сегментом</param>
        protected void CheckEmptySegmentsLecture(int lectureRowNumber, int translationRowNum)
        {
            // Максимальное число для добавления переводов, чтобы прогресс увеличился
            if (translationRowNum > maxEditorLinesNum - numberSentencesGrowProgress)
            {
                // Очистить лекцию
                ClearLecture();
                ClickBackEditor();
                // Проверить, что прогресс равен нулю после очистки лекции
                WaitUpdateProgress();
                Assert.IsTrue(GetPersonalProgress(lectureRowNumber) == 0,
                    "Ошибка: не уменьшился прогресс после удаления всех переводов лекции");
            }
            else
            {
                ClickBackEditor();
            }
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
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            courseName = OpenCourseMinProgress();
            // Перейти в лекцию
            lectureRowNumber = SelectEmptyLectureGetRowNumber();
            Console.WriteLine("лекция: " + lectureRowNumber);
            int personalProgress = GetPersonalProgress(lectureRowNumber);
           
            // Проверить пустые сегменты в лекции
            //CheckEmptySegmentsLecture(lectureRowNumber, firstSegmentRowNumber);
            
            // Открыть лекцию
            OpenLectureByRowNum(lectureRowNumber);

            // Добавить переводы
            firstSegmentRowNumber = GetEmptyTranslationRowNumber();
            AddNumOfTranslations(firstSegmentRowNumber);
            ClickBackEditor();

            // Прогресс после добавления
            WaitUpdateProgress();
            int personalProgressAfter = GetPersonalProgress(lectureRowNumber);
            // Проверить, что прогресс увеличился
            Assert.IsTrue(personalProgressAfter > personalProgress, "Ошибка: прогресс не увеличился");
        }

        /// <summary>
        /// Добавить N переводов, начиная с firstSegmentRowNumber сегмента
        /// </summary>
        /// <param name="firstSegmentRowNumber">номер сегмента, с которого начинать добавление переводов</param>
        protected void AddNumOfTranslations(int firstSegmentRowNumber)
        {
            string translationText = "Example Translation " + DateTime.Now.Ticks;
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
            string translationText = "Example Translation " + DateTime.Now.Ticks;
            int currentIndex = 0;
            // Добавить переводы
            for (int i = 0; i < numberSentencesGrowProgress; ++i)
            {
                currentIndex = i + 1;
                while (GetTranslationVariantsNum(currentIndex) > 0)
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
