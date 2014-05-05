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
    class Achievements : BaseTest
    {
        public Achievements(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {
        }

        [SetUp]
        public void Setup()
        {
        }

        // Тесты должны проходить только в определенном порядке, поэтому названия с привязкой к алфавиту

        /// <summary>
        /// Тест: получение награды "Переводчик. 1 Уровень" (осуществить 5 переводов)
        /// </summary>
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        /// Тест: получение награды "Эксперт. 1 Уровень" (проголосовать за 5 переводов)
        /// </summary>
        [Test]
        public void b_1_ExpertLevel1()
        {
            // Уровень
            int achieveLevel = 1;
            // Количество голосов для получения уровня 
            int levelLimit = 5;
            // Добавить нужное количество голосов
            GetExpertLevel(achieveLevel, levelLimit);
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
        /// Тест: получение награды "Специалист. 1 уровень" (перевести 1% курса)
        /// </summary>
        [Test]
        public void e_1_SpecialistLevel1()
        {
            int achieveLevel = 1;
            // Сколько голосов нужно получить
            int levelLimit = 1;
            // Получить количество голосов
            GetSpecialistLevel(achieveLevel, levelLimit);
        }

        /// <summary>
        /// Тест: получение награды Лидер онлайн
        /// </summary>
        [Test]
        public void c_1_LeaderListOnlineTest()
        {
            int needUserPosition = 10;
            // Добраться до нужного места в лидерборде
            GetPositionLeaderboardOnline(needUserPosition, "Leader");
        }

        /// <summary>
        /// Тест: получение награды Лидер оффлайн
        /// </summary>
        [Test]
        public void c_2_LeaderListOfflineTest()
        {
            int needUserPosition = 10;
            // Добраться до нужного места в лидерборде
            GetPositionLeaderboardOffline(needUserPosition, "Leader");
        }

        /// <summary>
        /// Тест: получение награды Лидер оффлайн (с 11 до 9 места)
        /// </summary>
        [Test]
        public void c_3_LeaderListOfflinePosition9Test()
        {
            int needUserPosition = 10;
            // Добраться до нужного места в лидерборде
            GetPositionLeaderboardOffline(needUserPosition, "Leader", true);
        }

        /// <summary>
        /// Тест: получение награды Номер1 онлайн
        /// </summary>
        [Test]
        public void d_1_FirstPlaceOnlineTest()
        {
            int needUserPosition = 1;
            // Добраться до нужного места в лидерборде
            GetPositionLeaderboardOnline(needUserPosition);
        }

        /// <summary>
        /// Тест: получение награды Номер1 оффлайн
        /// </summary>
        [Test]
        public void d_2_FirstPlaceOfflineTest()
        {
            int needUserPosition = 1;
            // Добраться до нужного места в лидерборде
            GetPositionLeaderboardOffline(needUserPosition);
        }

        /// <summary>
        /// Проверить получение и потерю награды Переводчик
        /// </summary>
        /// <param name="achieveLevel">уровень награды</param>
        /// <param name="levelLimit">количество переводов для награды</param>
        protected void GetTranslatorLevel(int achieveLevel, int levelLimit)
        {
            string achieveType = "Translator";
            SelectUserGetAchieve(achieveType, achieveLevel);

            bool isTestOk = true;
            string testErrorMessage = "\n";
            int translationsNumberLeft = levelLimit - GetAchieveProgressProfile(achieveType);
            int lectureRowNumber = 1;

            // Перейти к списку доступных курсов
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            string courseName = OpenCourseMinProgress();
            Console.WriteLine("курс: " + courseName);

            // Добавить предложения до N-1
            AddTranslationsCourse(translationsNumberLeft - 1, ref lectureRowNumber);
            // Проверить, что нет сообщения о награде
            CheckAchieveMessages(achieveType, achieveLevel, false, ref isTestOk, ref testErrorMessage);

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
                testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный уровень: " + newAchieveLevel;
            }
            // проверка прогресса
            if (newAchieveProgress != levelLimit)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный прогресс: " + newAchieveProgress;
            }

            // Удалить два перевода
            OpenCoursePage();
            OpenCourseByName(courseName);
            OpenLectureByRowNum(1);
            DeleteTranslations(1, 2);
            ClickBackEditor();

            // Проверить, что уровень и прогресс в профиле не изменились
            OpenUserProfileFromCourse();            
            newAchieveProgress = GetAchieveProgressProfile(achieveType);
            newAchieveLevel = GetAchieveLevelProfile(achieveType);
            // проверка уровня награды
            if (newAchieveLevel != achieveLevel)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после удаления переводов в профиле указывается неправильный уровень: " + newAchieveLevel;
            }
            // проверка прогресса
            if (newAchieveProgress != levelLimit)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после удаления переводов в профиле указывается неправильный прогресс: " + newAchieveProgress;
            }

            // Добавить перевод вместо удаленного
            OpenCoursePage();
            OpenCourseByName(courseName);
            OpenLectureByRowNum(1);
            AddTranslationByRowNum(1, "Test Text");
            ClickBackEditor();

            // Проверить, что уровень и прогресс в профиле не изменились
            OpenUserProfileFromCourse();
            newAchieveProgress = GetAchieveProgressProfile(achieveType);
            newAchieveLevel = GetAchieveLevelProfile(achieveType);
            // проверка уровня награды
            if (newAchieveLevel != achieveLevel)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после добавления перевода в удаленный в профиле указывается неправильный уровень: " + newAchieveLevel;
            }
            // проверка прогресса
            if (newAchieveProgress != levelLimit)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после добавления перевода в удаленный в профиле указывается неправильный прогресс: " + newAchieveProgress;
            }

            // Добавить новый перевод
            OpenCoursePage();
            OpenCourseByName(courseName);
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
                testErrorMessage += "Ошибка: после добавления нового перевода в профиле указывается неправильный уровень: " + newAchieveLevel;
            }
            // прогресс должен увеличиться на 1
            if (newAchieveProgress != (levelLimit + 1))
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после добавления нового перевода в профиле указывается неправильный прогресс: " + newAchieveProgress;
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
        protected int SelectUserGetAchieve(string achieveType, int achieveLevel)
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
                int currentLevel = GetAchieveLevelProfile(achieveType);
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
        /// Выбрать пользователя, место в рейтинге которого ниже нужного
        /// </summary>
        /// <param name="userPosition">место в рейтинге, которое будет зарабатывать пользователь</param>
        /// <returns></returns>
        protected int SelectUserRatingPosition(int userPosition)
        {
            bool isNeedChangeUser = true;
            // -1 - это пользователь Ян (он же Bob)
            // с 0 по 15 - это тестовые пользователи

            int userIndex = -1;
            for (int i = 0; i < TestUserList.Count; ++i)
            {
                // Открыть профиль пользователя
                OpenUserProfileFromHomePage();
                // Текущее место пользователя
                int currentUserPosition = GetUserPosition();
                isNeedChangeUser = currentUserPosition <= userPosition;

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
            int achieveProgress = int.Parse(achieveText.Substring(0, achieveText.IndexOf("/")));
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
                // Выбираем лекцию с личным и общим прогрессом == 0
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
            string translationText = "Test Translation " + DateTime.Now;

            if (!isLectureFinished)
            {
                for (int i = startIndex; i < segmentsList.Count; ++i)
                {
                    segmentsList[i].Click();
                    segmentsList[i].Click();
                    if (segmentsList[i].Text.Trim().Length == 0)
                    {
                        // Заполнить, только если чистое поле
                        segmentsList[i].Clear();
                        segmentsList[i].SendKeys(translationText);

                        // Кликнуть по галочке с Confirm в строке сегмента
                        Driver.FindElement(By.XPath(".//span[contains(@class,'fa-border')]")).Click();
                        WaitUntilDisappearElement(".//span[contains(@class,'fa-border')]", 20);

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
        /// Получить список видимых сегментов
        /// </summary>
        /// <param name="isLectureFinished">OUT: лекция закончилась</param>
        /// <param name="startIndex">OUT: индекс для начала заполнения</param>
        /// <param name="lastLastFactRow">IN/OUT: фактический номер последней видимой строки</param>
        /// <returns></returns>
        protected IList<IWebElement> GetVisibleSegmentList(ref int lastLastFactRow, out bool isLectureFinished, out int startIndex)
        {
            // README : если нужно понять, зачем такой странный алгоритм и к чему непонятные переменные:
            // прочитать описание ниже.

            // При входе в редактор Selenium видит только 34-35 сегментов.
            // Сегменты могут начинаться не с первого:
            // если до этого заходили в редактор и изменяли что-то в какой-то, например, 10 строке,
            // то при следующем входе, курсор будет в 20 строке,
            // а Selenium будет видеть с 3 строки по 38 (например),
            // т.е. при обращении к первой строке, он будет обращаться к фактической 3 строке.
            // Фактический номер строки - тот, который написан в первом столбце.
            // При этом, когда мы обращаемся к какой-то строке по номеру tr:nth-child(N),
            // может произойти ошибка, т.к. Selenium смещает свой видимый список по мере того, как мы заполняем предложения.
            // Т.е. в следующий раз при обращении к первой строке, он уже будет обращаться к 4ой фактической строке (а видеть с 4 по 39).

            // Поэтому беру список видимых строк.
            // Получаю фактический номер первой строки и фактический номер последней строки.
            // (К примеру 1 и 34, соответственно). lastFirstRow = 1, lastLastRow = 34
            // Заполняю все эти видимые строки.
            // Обновляю список видимых сегментов.
            // Снова получаю фактический номер первой и последней строк.
            // (К примеру, 15 и 50, соответственно). curFirstRow = 15, curLastRow = 50
            // Нужно заполнить фактическую 35 строку, но для селениума она сейчас 21я.
            // Чтобы не заполнять с 15 по 34 фактические строки повторно, приходится учитывать предыдущее значение 34,
            // учитывать текущее первое значение 15:
            // фактическая 15 строка - селениум 1 строка
            // фактическиая 35 (34 последняя, нужна следующая - 35) - селениум ? строка
            // => ? = 35 - 15 + 1 = 34 + 1 - 15 + 1
            // => ? = lastLastRow + 1 - curFirstRow + 1
            // А цикл начинается с 0 (номер строки - 1), поэтому start = lastLastRow - curFirstRow + 1


            // Список видимых сегментов
            IList<IWebElement> segmentsList = Driver.FindElements(By.CssSelector("#segments-body div table tr td:nth-child(4) div"));

            // Фактический номер последней видимой строки
            int curLastRow = int.Parse(Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + segmentsList.Count + ") td:nth-child(1) div")).Text.Trim());

            // Для проверки, закончилась лекция или нет - сравниваем последние фактические номера (текущий и предыдущий),
            // если они совпали - мы зашли в заполненную лекцию
            isLectureFinished = curLastRow == lastLastFactRow;

            startIndex = 0;
            if (!isLectureFinished)
            {
                // Фактический номер первой видимой строки
                int curFirstRow = int.Parse(Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(1) div")).Text.Trim());
                startIndex = lastLastFactRow - curFirstRow + 1;
            }

            lastLastFactRow = curLastRow;

            return segmentsList;
        }

        /// <summary>
        /// Проверить сообщение о получении награды
        /// </summary>
        /// <param name="achieveType">тип награды</param>
        /// <param name="achieveLevel">уровень награды</param>
        /// <param name="messageShouldBeVisible">должно ли быть сообщение</param>
        /// <param name="isTestOk">REF: результат теста</param>
        /// <param name="testErrorMessage">REF: сообщение с результатом теста</param>
        protected void CheckAchieveMessages(string achieveType, int achieveLevel, bool messageShouldBeVisible,
                            ref bool isTestOk, ref string testErrorMessage)
        {
            // Проверить, есть ли сообщение в редакторе
            bool isExistEditorMessage = GetIsExistAchieveMessageEditor(achieveType, true, achieveLevel);
            ClickBackEditor();
            // Проверить, есть ли сообщение в лекции
            bool isExistLectureMessage = GetIsExistAchieveMessageLecture(achieveType, true, achieveLevel);

            // Проверка ошибок
            if (isExistEditorMessage != messageShouldBeVisible)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: в редакторе " + (isExistEditorMessage ? "" : "не ") + "появилось оповещение о награде";
            }
            if (isExistLectureMessage)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: в лекции появилось оповещение";
            }
        }

        /// <summary>
        /// Получить: есть ли сообщение о присуждении награды в редакторе
        /// </summary>
        /// <param name="achieveType">тип награды</param>
        /// <param name="achieveLevel">уровень награды</param>
        /// <returns>есть сообщение</returns>
        protected bool GetIsExistAchieveMessageEditor(string achieveType, bool needCheckLevel = false, int achieveLevel = 1)
        {
            bool isAchieveMessage = false;
            bool needCheckNewMessage = true;
            
            while (needCheckNewMessage)
            {
                isAchieveMessage = IsElementDisplayed(By.XPath(".//div[contains(@class,'achievement')]"));
                if (isAchieveMessage)
                {
                    Console.WriteLine("получили награду");
                    string achieveInfo = Driver.FindElement(By.XPath(".//div[contains(@class,'achievement')]//div[contains(@class,'name')]")).Text;
                    string curAchieveType = achieveInfo.Substring(0, achieveInfo.IndexOf("(")).Trim();

                    // Тип награды
                    if (curAchieveType == achieveType)
                    {
                        Console.WriteLine(achieveType);
                        if (needCheckLevel)
                        {
                            int curAchieveLevel = int.Parse(achieveInfo.Substring(achieveInfo.IndexOf("(") + 1, achieveInfo.IndexOf("/") - achieveInfo.IndexOf("(") + 1).Trim());
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
                    Driver.FindElement(By.XPath(".//div[contains(@class,'achievement')]//span[contains(@class,'x-btn-icon-el')]")).Click();
                }
                else
                {
                    // Нет награды
                    Console.WriteLine("не получили нужную награду");
                    needCheckNewMessage = false;
                }
            }

            return isAchieveMessage;
        }

        /// <summary>
        /// Получить: есть ли сообщение о присуждении награды на странице лекции
        /// </summary>
        /// <param name="achieveType">тип награды</param>
        /// <param name="achieveLevel">уровень награды</param>
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

                    // Тип награды
                    if (curAchieveType == achieveType)
                    {
                        Console.WriteLine(achieveType);
                        if (needCheckLevel)
                        {
                            int curAchieveLevel = int.Parse(achieveInfo.Substring(achieveInfo.IndexOf("(") + 1, achieveInfo.IndexOf("/") - achieveInfo.IndexOf("(") + 1).Trim());
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
                    Driver.FindElement(By.XPath(".//div[@id='achieve-popup')]//p/a[contains(@class,'button')]")).Click();
                }
                else
                {
                    // Нет награды
                    Console.WriteLine("не получили нужную награду");
                    needUpdateMessageInfo = false;
                }
            }

            return isAchieveMessage;
        }

        /// <summary>
        /// Получить уровень награды в профиле
        /// </summary>
        /// <param name="achieveType">тип награды</param>
        /// <returns>уровень</returns>
        protected int GetAchieveLevelProfile(string achieveType)
        {
            string achieveText = Driver.FindElement(By.XPath(
                ".//ul[@class='achive-list']//li[" + GetAchieveNumInList(achieveType) + "]//strong")).Text.Trim();
            int indexStart = achieveText.IndexOf("(") + 1;
            // Уровень
            int achieveLevel = int.Parse(achieveText.Substring(indexStart, achieveText.IndexOf("/") - indexStart));
            Console.WriteLine("уровень: " + achieveLevel);
            return achieveLevel;
        }

        /// <summary>
        /// Проверить награду Эксперт
        /// </summary>
        /// <param name="achieveLevel">уровень награды</param>
        /// <param name="levelLimit">количество голосов для уровня награды</param>
        protected void GetExpertLevel(int achieveLevel, int levelLimit)
        {
            string achieveType = "Expert";
            SelectUserGetAchieve(achieveType, achieveLevel);

            bool isTestOk = true;
            string testErrorMessage = "\n";
            int voteNumberLeft = levelLimit - GetAchieveProgressProfile(achieveType);

            // Проголосовать до N - 1
            AddVotesCourses(voteNumberLeft - 1);
            // Проверить, что нет сообщения о награде
            CheckAchieveMessages(achieveType, achieveLevel, false, ref isTestOk, ref testErrorMessage);

            // Проголосовать до N
            AddVotesCourses(1);
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
                testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный уровень: " + newAchieveLevel;
            }
            // проверка прогресса
            if (newAchieveProgress != levelLimit)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный прогресс: " + newAchieveProgress;
            }

            // Проголосовать до N + 1
            AddVotesCourses(1);
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
                testErrorMessage += "Ошибка: после нового голоса в профиле указывается неправильный уровень: " + newAchieveLevel;
            }
            // проверка прогресса
            if (newAchieveProgress != (levelLimit + 1))
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после нового голоса в профиле указывается неправильный прогресс: " + newAchieveProgress;
            }

            // Вывести ошибки:
            Assert.IsTrue(isTestOk, testErrorMessage);
        }

        /// <summary>
        /// Добавить голоса
        /// </summary>
        /// <param name="voteNumberLeft">оставшееся количество голосов</param>
        protected void AddVotesCourses(int voteNumberLeft)
        {
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
                    lectureRowNum = SelectLectureToVote(lectureRowNum);
                    Console.WriteLine("lectureRowNum: " + lectureRowNum);
                    OpenLectureByRowNum(lectureRowNum);

                    int lastLastFactRow = 0, votedCounter = 0;
                    bool isFinished = false;
                    do
                    {
                        isFinished = AddVotesVisibleSentences(ref lastLastFactRow, voteNumberLeft, out votedCounter);
                        voteNumberLeft -= votedCounter;
                        Console.WriteLine("levelLimit после: " + voteNumberLeft);
                    } while (voteNumberLeft > 0 && !isFinished);

                    if (voteNumberLeft <= 0)
                    {
                        break;
                    }

                    if (isFinished)
                    {
                        ++lectureRowNum;
                    }
                }

                Assert.IsTrue(voteNumberLeft <= 0, "Ошибка: закончилась лекция");

                ClickBackEditor();
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
        /// Проголосовать за переводы, за которые пользователь не голосовал
        /// </summary>
        /// <param name="lastLastFactRow">IN/OUT: последний номер в строке с последним сегментом при предыдущем проходе редактора</param>
        /// <param name="limitVotes">IN: максимальное количество голосов для добавления</param>
        /// <param name="countVoted">OUT: количество добавленных голосов</param>
        /// <returns></returns>
        protected bool AddVotesVisibleSentences(ref int lastLastFactRow, int votesNumberLimit, out int votesNumber)
        {
            bool isLectureFinished = false;
            int startIndex = 0;
            // Список видимых сегментов
            IList<IWebElement> segmentsList = 
                GetVisibleSegmentList(ref lastLastFactRow, out isLectureFinished, out startIndex);

            // Сбрасываем счетчик переведенных предложений
            votesNumber = 0;
            string translationText = "Test Translation " + DateTime.Now;

            if (!isLectureFinished)
            {
                for (int i = startIndex; i < segmentsList.Count; ++i)
                {
                    segmentsList[i].Click();
                    segmentsList[i].Click();
                    Console.WriteLine("номер строки: " + (i + 1));
                    // Проголосовать за переводы в сегменте
                    int votedNum = VoteSuggestedTranslations(votesNumberLimit - votesNumber);
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
        /// Проголосовать за предложенные переводы
        /// </summary>
        /// <param name="votesNumberLeft"></param>
        /// <returns></returns>
        protected int VoteSuggestedTranslations(int votesNumberLeft)
        {
            int votedNumber = 0;
            setDriverTimeoutMinimum();
            IList<IWebElement> translationsList = Driver.FindElements(By.XPath(
                ".//div[@id='translations-body']//table//tr//td[5]/div//span[contains(@class,'fa-thumbs-up')]"));
            setDriverTimeoutDefault();
            Console.WriteLine(translationsList.Count > 0 ? "переводы есть" : "переводов нет");

            // Пробуем проголосовать за предложенные переводы
            for (int i = 0; i < translationsList.Count; ++i)
            {
                if (votedNumber < votesNumberLeft)
                {
                    if (!translationsList[i].GetAttribute("class").Contains("disabled"))
                    {
                        Console.WriteLine("пытаемся проголосовать " + i);
                        translationsList[i].Click();
                        if (GetIsVoteConsideredEditor(true, (i + 1)))
                        {
                            ++votedNumber;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return votedNumber;
        }

        /// <summary>
        /// Проверить получение и потерю награды Профессионал
        /// </summary>
        /// <param name="achieveLevel">уровень награды</param>
        /// <param name="levelLimit">количество голосов для награды</param>
        protected void GetProfessionalLevel(int achieveLevel, int levelLimit)
        {
            string achieveType = "Professional";
            int userIndex = SelectUserGetAchieve(achieveType, achieveLevel);

            bool isTestOk = true;
            string testErrorMessage = "\n";

            // Добавить перевод
            string courseName;
            int lectureRowNumber, translationRowNumber;
            string translationText = "Test Translation " + DateTime.Now;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNumber);
            Console.WriteLine("курс: " + courseName + ", номер лекции: " + lectureRowNumber + ", номер для перевода: " + translationRowNumber);
            ClickBackEditor();
            LogoutUser();

            // Проголосовать за него для получения награды
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
                testErrorMessage += "Ошибка: не появилось сообщение о присуждении награды";
            }

            // Проверить прогресс в профиле
            OpenUserProfileFromCourse();
            int newAchieveProgress = GetAchieveProgressProfile(achieveType);
            int newAchieveLevel = GetAchieveLevelProfile(achieveType);
            // проверка уровня награды
            if (newAchieveLevel != achieveLevel)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный уровень: " + newAchieveLevel;
            }
            // проверка прогресса
            if (newAchieveProgress != levelLimit)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после получения ачивки в профиле указывается неправильный прогресс: " + newAchieveProgress;
            }
            LogoutUser();

            // Один из пользователей проголосует против
            for (int i = 0; i < TestUserList.Count; ++i)
            {
                if (i != userIndex)
                {
                    LoginUser(TestUserList[i]);
                    VoteCurrentTranslation(lectureRowNumber, translationRowNumber, translationText, false);
                    Console.WriteLine("проголосовал!");
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
            newAchieveProgress = GetAchieveProgressProfile(achieveType);
            newAchieveLevel = GetAchieveLevelProfile(achieveType);
            // проверка уровня награды
            if (newAchieveLevel != achieveLevel)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после голосва против в профиле указывается неправильный уровень: " + newAchieveLevel;
            }
            // проверка прогресса
            if (newAchieveProgress != levelLimit)
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: после голосва против в профиле указывается неправильный прогресс: " + newAchieveProgress;
            }
            
            // Добавить новый перевод
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNumber);
            Console.WriteLine("курс: " + courseName + ", номер лекции: " + lectureRowNumber + ", номер для перевода: " + translationRowNumber);
            ClickBackEditor();
            LogoutUser();

            // Проголосовать за него для получения награды
            votesLeft = levelLimit;
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

            // Проверить, что сообщения нет
            if (GetIsExistAchieveMessageLecture(achieveType, true, achieveLevel))
            {
                isTestOk = false;
                testErrorMessage += "Ошибка: появилось повторное сообщение о присуждении награды";
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
            ClickBackEditor();
        }

        /// <summary>
        /// Проверить получение и потерю награды, связанной с местом в лидерборде (онлайн)
        /// </summary>
        /// <param name="needUserPosition">нужная позиция пользователя</param>
        /// <param name="achieveType">тип награды</param>
        protected void GetPositionLeaderboardOnline(int needUserPosition, string achieveType)
        {
            // Выбрать пользователя
            // TODO убрать Console
            Console.WriteLine("выбрали пользователя: " + SelectUserRatingPosition(needUserPosition));

            // Открыть лидерборд
            OpenLeaderboardPage();
            // Получить рейтинг пользователя на нужном месте
            Decimal ratingNeedPosition = GetLeaderboardPositionRating(needUserPosition);
            Console.WriteLine("нужно набрать рейтинга: " + ratingNeedPosition);

            bool isNeedAddTranslations = false;
            int minimumNumberToAdd = 0;
            Decimal ratingBefore = 0;
            string courseName = "";
            do
            {
                Console.WriteLine("открыть профиль пользователя");

                OpenUserProfileFromCourse();
                ratingBefore = GetUserRating();
                Console.WriteLine("Сейчас рейтинг пользователя: " + ratingBefore);

                // Нужно добавить переводов
                minimumNumberToAdd = (int)((ratingNeedPosition - ratingBefore) / 3);
                Console.WriteLine("нужно переводов: " + minimumNumberToAdd);
                isNeedAddTranslations = minimumNumberToAdd > 1;

                // Перейти к списку доступных курсов
                OpenCoursePage();
                // Переход в курс с наименьшим прогрессом
                courseName = OpenCourseMinProgress();
                Console.WriteLine("курс: " + courseName);
                int lectureRowNumber = 1;
                if (isNeedAddTranslations)
                {
                    Console.WriteLine("начинаем добавлять переводы");
                    AddTranslationsCourse(minimumNumberToAdd, ref lectureRowNumber);
                }
            } while (isNeedAddTranslations);

            Console.WriteLine("достаточно добавили");
            int userPosition = GetUserPosition();
            Console.WriteLine("Сейчас место пользователя: " + userPosition);
            if (userPosition <= needUserPosition)
            {
                Assert.Fail("Перелёт");
            }

            bool isOk = true;
            string errorMessage = "\n";

            bool isExistEditorAchieve = false, isExistLectureAchieve = false;
            while (userPosition > needUserPosition)
            {
                if (isExistEditorAchieve || isExistLectureAchieve)
                {
                    isOk = false;
                    errorMessage += "Ошибка: оповещение о награде появилось раньше времени\n";
                }
                Console.WriteLine("Сейчас место пользователя: " + userPosition);

                isExistEditorAchieve = AddTranslationWaitAchieve(achieveType);
                isExistLectureAchieve = GetIsExistAchieveMessageLecture(achieveType);

                OpenUserProfileFromCourse();
                userPosition = GetUserPosition();
            }

            if (!isExistEditorAchieve)
            {
                isOk = false;
                errorMessage += "Ошибка: в редакторе не появилось оповещение о получении награды " + achieveType + "\n";
            }
            if (isExistLectureAchieve)
            {
                isOk = false;
                errorMessage += "Ошибка: повторное оповещение о награде на странице лекции\n";
            }

            // TODO проверить в профиле ачивку

            // Удалить два перевода
            OpenCoursePage();
            OpenCourseByName(courseName);
            OpenLectureByRowNum(1);
            DeleteTranslations(1, 1);
            ClickBackEditor();

            // Проверить место пользователя
            OpenUserProfileFromCourse();
            userPosition = GetUserPosition();
            if (userPosition > needUserPosition)
            {
                // TODO Проверить ачивку
            }

            while (userPosition > needUserPosition)
            {
                if (isExistEditorAchieve || isExistLectureAchieve)
                {
                    isOk = false;
                    errorMessage += "Ошибка: появилось повторное оповещение о награде, хотя место пользователя ниже необходимого\n";
                }
                Console.WriteLine("Сейчас место пользователя: " + userPosition);

                isExistEditorAchieve = AddTranslationWaitAchieve(achieveType);
                isExistLectureAchieve = GetIsExistAchieveMessageLecture(achieveType);

                OpenUserProfileFromCourse();
                userPosition = GetUserPosition();
            }
            
            if (isExistEditorAchieve)
            {
                isOk = false;
                errorMessage += "Ошибка: если потерять место, а потом его снова получить - повторно появляется награда (в редакторе)\n";
            }
            if (isExistLectureAchieve)
            {
                isOk = false;
                errorMessage += "Ошибка: если потерять место, а потом его снова получить - повторно появляется награда (в лекции)\n";
            }

            // TODO проверить в профиле ачивку
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
        /// Добавить перевод и проверить сообщение о награде в редакторе
        /// </summary>
        /// <param name="achieveType">тип награды</param>
        /// <returns>есть ли сообщение о награде</returns>
        protected bool AddTranslationWaitAchieve(string achieveType)
        {
            // Перейти к списку доступных курсов
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            string courseName = OpenCourseMinProgress();
            Console.WriteLine("курс: " + courseName);

            OpenLectureByRowNum(SelectLectureToTranslate());

            int lastSentenceNumber = 0;
            bool isAddedTranslation = false;

            do
            {
                isAddedTranslation = AddTranslationEmptySegment(ref lastSentenceNumber);
            } while (!isAddedTranslation);

            bool isAchieveMessageExist = GetIsExistAchieveMessageEditor(achieveType);

            // Выйти из редактора
            ClickBackEditor();

            return isAchieveMessageExist;
        }

        /// <summary>
        /// Добавить перевод в пустой сегмент
        /// </summary>
        /// <param name="lastLastFactRow">последний фактический номер видимой строки</param>
        /// <returns>добавлен ли перевод</returns>
        protected bool AddTranslationEmptySegment(ref int lastLastFactRow)
        {
            bool isLectureFinished = false;
            bool isAddedTranslation = false;
            int startIndex = 0;
            // Список видимых сегментов
            IList<IWebElement> segmentsList = GetVisibleSegmentList(ref lastLastFactRow, out isLectureFinished, out startIndex);

            // Сбрасываем счетчик переведенных предложений
            string translationText = "Test Translation " + DateTime.Now;

            if (!isLectureFinished)
            {
	            for (int i = startIndex; i < segmentsList.Count; ++i)
                {
                    segmentsList[i].Click();
                    segmentsList[i].Click();
                    if (segmentsList[i].Text.Trim().Length == 0)
                    {
                        // Заполнить, только если чистое поле
                        segmentsList[i].Clear();
                        segmentsList[i].SendKeys(translationText);

                        // Кликнуть по галочке с Confirm в строке сегмента
                        Driver.FindElement(By.XPath(".//span[contains(@class,'fa-border')]")).Click();
                        WaitUntilDisappearElement(".//span[contains(@class,'fa-border')]", 20);

                        // Проверить, что перевод появился в предложенных переводах
                        int translationRowNumber = GetSuggestedTranslationRowNum(translationText);
                        if (translationRowNumber > 0)
                        {
                            isAddedTranslation = true;
                            break;
                        }
                    }
                }
            }

            // Добавлен ли перевод
            return isAddedTranslation;
        }

        /// <summary>
        /// Проверить получение и потерю награды, связанной с местом в лидерборде (оффлайн)
        /// </summary>
        /// <param name="needUserPosition">нужная позиция пользователя</param>
        /// <param name="achieveType">тип награды</param>
        /// <param name="isNeedGetStepUpPositionOffline">для теста, когда оффлайн пользователь должен подняться не до нужного места (10), а на место выше (9)</param>
        protected void GetPositionLeaderboardOffline(int needUserPosition, string achieveType, bool isNeedGetStepUpPositionOffline = false)
        {
            // Выбрать пользователя
            int userIndex = SelectUserRatingPosition(needUserPosition);

            OpenHomepage();
            string userName = GetUserNameHomepage();

            // Открыть лидерборд
            OpenLeaderboardPage();
            // Получить рейтинг пользователя на нужном месте
            Decimal ratingNeedPosition = GetLeaderboardPositionRating(needUserPosition);

            bool isNeedAddTranslations = false;
            int minimumNumberToAdd = 0;
            Decimal ratingBefore = 0;
            List<string> courseList = new List<string> ();
            do
            {
                OpenUserProfileFromCourse();
                ratingBefore = GetUserRating();
                Console.WriteLine("Сейчас рейтинг пользователя: " + ratingBefore);

                // Нужно добавить переводов
                minimumNumberToAdd = (int)((ratingNeedPosition - ratingBefore) / 3);
                Console.WriteLine("нужно переводов: " + minimumNumberToAdd);
                isNeedAddTranslations = minimumNumberToAdd > 1;

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
                if (isNeedAddTranslations)
                {
                    AddTranslationsCourse(minimumNumberToAdd, ref lectureRowNumber);
                }
            } while (isNeedAddTranslations);

            int userPosition = GetUserPosition();
            Console.WriteLine("Сейчас место пользователя: " + userPosition);
            if (userPosition <= needUserPosition)
            {
                Assert.Fail("Перелёт");
            }

            // Добавить курсы из профиля

            LogoutUser();
            if (userIndex == TestUserList.Count - 1)
            {
                LoginUser(TestUserList[0]);
            }
            else
            {
                LoginUser(TestUserList[userIndex + 1]);
            }

            int offlineNeedUserPosition = isNeedGetStepUpPositionOffline ? (needUserPosition + 1) : needUserPosition;
            while (userPosition > offlineNeedUserPosition)
            {
                // Проголосовать за перевод этого пользователя
                VoteUserTranslationCourses(userName, courseList);
                // Проверить место пользователя в лидерборде
                OpenLeaderboardPage();
                if (GetIsUserLeaderboardActiveList(userName))
                {
                    userPosition = GetUserPositionLeaderboard(userName);
                }
            }

            LogoutUser();
            if (userIndex == -1)
            {
                LoginUser(User1);
            }
            else
            {
                LoginUser(TestUserList[userIndex]);
            }

            // Проверить, что появилось сообщение о награде
            Assert.IsTrue(GetIsExistAchieveMessageLecture(achieveType), "Ошибка: при входе пользователя ему не появилось сообщение о награде");

            // TODO проверить в профиле ачивку
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
                OpenCoursePage();
                OpenCourseByName(courseList[i]);
                int lectureRowNum = 1;
                while (!isVoted)
                {
                    lectureRowNum = SelectLectureToVote(lectureRowNum);
                    // Открыть лекцию
                    OpenLectureByRowNum(lectureRowNum);
                    int lastLastFactRow = 0;
                    bool isFinished = false;
                    do
                    {
                        isFinished = VoteVisibleUserTranslationLectures(ref lastLastFactRow, userName, out isVoted);
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
                        ClickBackEditor();
                        ++lectureRowNum;
                    }
                }
            }
        }

        /// <summary>
        /// Проголосовать за перевод пользователя (проход по лекциям курса)
        /// </summary>
        /// <param name="lastLastFactRow">фактический номер видимой последней строки</param>
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

            string translationText = "Test Translation " + DateTime.Now;

            if (!isLectureFinished)
            {
	            for (int i = startIndex; i < segmentsList.Count; ++i)
                {
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
                IList<IWebElement> translationsList = Driver.FindElements(By.XPath(
                    ".//div[@id='translations-body']//table//tr//td[5]/div//span[contains(@class,'fa-thumbs-up')]"));
            
                Console.WriteLine(translationsList.Count > 0 ? "переводы есть" : "переводов нет");

                // Пробуем проголосовать за предложенный перевод пользователя
                for (int i = 0; i < translationsList.Count; ++i)
                {
                    string translaterName = Driver.FindElement(By.XPath(
                        ".//div[@id='translations-body']//table//tr[" + (i + 1) + "//td[2]//div")).Text;
                    if (translaterName.Contains(userName))
                    {
                        if (!translationsList[i].GetAttribute("class").Contains("disabled"))
                        {
                            Console.WriteLine("пытаемся проголосовать " + i);
                            translationsList[i].Click();
                            if (GetIsVoteConsideredEditor(true, (i + 1)))
                            {
                                isVoted = true;
                                break;
                            }
                        }
                    }
                }
            }

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

        // TODO
        /// <summary>
        /// Добавить переводы в пустые сегменты
        /// </summary>
        /// <param name="lastLastFactRow">IN/OUT: последний номер в строке с последним сегментом при предыдущем проходе редактора</param>
        /// <param name="translationNumberLimit">IN: максимальное количество переводов для добавления</param>
        /// <param name="translatedNumber">OUT: количество добавленных переводов</param>
        /// <returns>закончилась ли лекция (true: пустые предложения закончились)</returns>
        protected bool FillEmptySegments(ref int lastLastFactRow, int translationNumberLimit, out int translatedNumber, bool isNeedConsiderRating = false, bool isNeedWaitAchieve = false)
        {
            // README : если нужно понять, зачем такой странный алгоритм и к чему непонятные переменные:
            // прочитать описание ниже.

            // При входе в редактор Selenium видит только 34-35 сегментов.
            // Сегменты могут начинаться не с первого:
            // если до этого заходили в редактор и изменяли что-то в какой-то, например, 10 строке,
            // то при следующем входе, курсор будет в 20 строке,
            // а Selenium будет видеть с 3 строки по 38 (например),
            // т.е. при обращении к первой строке, он будет обращаться к фактической 3 строке.
            // Фактический номер строки - тот, который написан в первом столбце.
            // При этом, когда мы обращаемся к какой-то строке по номеру tr:nth-child(N),
            // может произойти ошибка, т.к. Selenium смещает свой видимый список по мере того, как мы заполняем предложения.
            // Т.е. в следующий раз при обращении к первой строке, он уже будет обращаться к 4ой фактической строке (а видеть с 4 по 39).

            // Поэтому беру список видимых строк.
            // Получаю фактический номер первой строки и фактический номер последней строки.
            // (К примеру 1 и 34, соответственно). lastFirstRow = 1, lastLastRow = 34
            // Заполняю все эти видимые строки.
            // Обновляю список видимых сегментов.
            // Снова получаю фактический номер первой и последней строк.
            // (К примеру, 15 и 50, соответственно). curFirstRow = 15, curLastRow = 50
            // Нужно заполнить фактическую 35 строку, но для селениума она сейчас 21я.
            // Чтобы не заполнять с 15 по 34 фактические строки повторно, приходится учитывать предыдущее значение 34,
            // учитывать текущее первое значение 15:
            // фактическая 15 строка - селениум 1 строка
            // фактическиая 35 (34 последняя, нужна следующая - 35) - селениум ? строка
            // => ? = 35 - 15 + 1 = 34 + 1 - 15 + 1
            // => ? = lastLastRow + 1 - curFirstRow + 1
            // А цикл начинается с 0 (номер строки - 1), поэтому start = lastLastRow - curFirstRow + 1

            // Список видимых сегментов
            IList<IWebElement> segmentsList = Driver.FindElements(By.CssSelector("#segments-body div table tr td:nth-child(4) div"));
            IList<IWebElement> sourceSegmentsList = Driver.FindElements(By.CssSelector("#segments-body div table tr td:nth-child(3) div"));
            Console.WriteLine("Всего видно: " + segmentsList.Count);

            // Фактический номер первой видимой строки
            int curFirstRow = int.Parse(Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(1) div")).Text.Trim());
            Console.WriteLine("FN(1) = " + curFirstRow);
            // Фактический номер последней видимой строки
            int curLastRow = int.Parse(Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + segmentsList.Count + ") td:nth-child(1) div")).Text.Trim());
            Console.WriteLine("FN(L) = " + curLastRow);

            // Для проверки, закончилась лекция или нет - сравниваем последние фактические номера (текущий и предыдущий),
            // если они совпали - мы зашли в заполненную лекцию
            bool isFinished = curLastRow == lastLastFactRow;
            // Сбрасываем счетчик переведенных предложений
            translatedNumber = 0;
            // "Вес" перевода - учитывается при добавлении переводов для увеличения рейтинга, вес перевода будет больше 1, если в Source большой текст
            int translationWeight = 1;

            if (!isFinished)
            {
                // Индекс для начала заполнения
                int startIndex = lastLastFactRow - curFirstRow + 1;
                Console.WriteLine("start: " + startIndex);

                string translationText = "Test Translation " + DateTime.Now;

                for (int i = startIndex; i < segmentsList.Count; ++i)
                {
                    segmentsList[i].Click();
                    segmentsList[i].Click();
                    if (segmentsList[i].Text.Trim().Length == 0)
                    {
                        if (isNeedConsiderRating)
                        {
                            // Учет рейтинга - то есть добавляем переводы для увеличения рейтинга
                            // Чтобы не перемахнуть через нужное число - проверяем количество слов в Source
                            // Мы приняли при расчете нужного количества переводов, что за один перевод дается 3 балла к рейтингу
                            // Но, по опыту выяснилось, если число слов больше 30, то баллов больше
                            // и на каждые 30 добавляем 3 балла, то есть уменьшаем оставшееся количество переводов на (N / 30)
                            int sourceWordsCount = sourceSegmentsList[i].Text.Split().Length;
                            translationWeight = (sourceWordsCount / 30);
                            Console.WriteLine("translationWeight: " + translationWeight);
                            Console.WriteLine("translationNumberLimit - translatedNumber: " + (translationNumberLimit - translatedNumber));
                            if (translationWeight > (translationNumberLimit - translatedNumber))
                            {
                                continue;
                            }
                        }

                        // Заполнить, только если чистое поле
                        segmentsList[i].Clear();
                        segmentsList[i].SendKeys(translationText);

                        // Кликнуть по галочке с Confirm в строке сегмента
                        Driver.FindElement(By.XPath(".//span[contains(@class,'fa-border')]")).Click();
                        WaitUntilDisappearElement(".//span[contains(@class,'fa-border')]", 20);

                        // Проверить, что перевод появился в предложенных переводах
                        int translationRowNumber = GetSuggestedTranslationRowNum(translationText);
                        if (translationRowNumber > 0)
                        {
                            // Если появился - перевод принят, увеличиваем счетчик
                            ++translatedNumber;
                            Console.WriteLine("осталось перевести: " + (translationNumberLimit - translatedNumber));

                            if (isNeedConsiderRating)
                            {
                                translatedNumber += translationWeight;
                            }

                            if (isNeedWaitAchieve)
                            {
                                bool isAchieveMessage = IsElementDisplayed(By.XPath(".//div[contains(@class,'achievement')]"));
                                if (isAchieveMessage)
                                {
                                    Console.WriteLine("получили награду");
                                    string achieveInfo = Driver.FindElement(By.XPath(".//div[contains(@class,'achievement')]//div[contains(@class,'name')]")).Text;
                                    string achieveType = achieveInfo.Substring(0, achieveInfo.IndexOf("(")).Trim();
                                    Console.WriteLine(achieveType);
                                    string achieveLevel = achieveInfo.Substring(achieveInfo.IndexOf("(") + 1, achieveInfo.IndexOf("/") - achieveInfo.IndexOf("(") + 1).Trim();
                                    Console.WriteLine(achieveLevel);

                                    Driver.FindElement(By.XPath(".//div[contains(@class,'achievement')]//span[contains(@class,'x-btn-icon-el')]")).Click();

                                    break;
                                }
                            }

                            if (translatedNumber >= translationNumberLimit)
                            {
                                Console.WriteLine("достаточно, выходим из лекции");
                                break;
                            }
                        }
                    }
                }
            }

            // Передать фактический номер текущей последней строки для последующего заполнения
            lastLastFactRow = curLastRow;

            // Закончилась ли лекция
            return isFinished;
        }

        // TODO
        protected void GetSpecialistLevel(int achieveLevel, int levelLimit)
        {
            string achieveType = "Specialist";
            bool isNeedChangeUser = true;

            for (int i = 0; i < TestUserList.Count; ++i)
            {
                // Открыть профиль пользователя
                OpenUserProfileFromHomePage();
                int userLevel = GetAchieveLevelProfile(achieveType);
                isNeedChangeUser = userLevel >= achieveLevel;

                if (isNeedChangeUser)
                {
                    LogoutUser();
                    LoginUser(TestUserList[i]);
                }
                else
                {
                    break;
                }
            }

            FillCourcePercent(levelLimit);
            OpenUserProfileFromCourse();
            int userLevelAfter = GetAchieveLevelProfile(achieveType);
            int percent = GetAchieveProgressPercent(achieveType);

            Console.WriteLine("userLevelAfter " + userLevelAfter);
            Console.WriteLine("percent " + percent);
        }

        // TODO
        protected void FillCourcePercent(int needPercent)
        {
            // Перейти к списку доступных курсов
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            string courseName = OpenCourseMinProgress();
            Console.WriteLine("курс: " + courseName);

            IList<IWebElement> percentElements = Driver.FindElements(By.XPath(".//div[contains(@data-bind,'personalProgressView')]"));

            Console.WriteLine("количество лекций: " + percentElements.Count);

            int sumPersonal = 0;
            foreach (IWebElement el in percentElements)
            {
                sumPersonal += int.Parse(el.Text.Trim().Replace("%", ""));
            }

            Decimal currentPercent = sumPersonal / percentElements.Count;
            Assert.IsTrue(currentPercent < needPercent, "Ошибка: неправильный подсчет в профиле, т.к. курс уже заполнен больше, чем надо для уровня.");

            int fillFull = percentElements.Count / needPercent;

            for (int i = 0; i < fillFull; ++i)
            {
                if (GetPersonalProgress(i + 1) < 100)
                {
                    OpenLectureByRowNum(i + 1);
                    bool isFinished = false;
                    int lastSentenceNumber = 0, translationsNumberLeft = 1000, translatedNumber = 0;
                    do
                    {
                        isFinished = FillEmptySegments(ref lastSentenceNumber, translationsNumberLeft, out translatedNumber);
                    } while (!isFinished);

                    ClickBackEditor();

                    Thread.Sleep(15000);

                    Driver.FindElement(By.XPath(".//div[contains(@data-bind,'personalProgressView')]")).SendKeys(OpenQA.Selenium.Keys.F5);
                }

                Console.WriteLine("личный прогресс: " + GetPersonalProgress(i + 1));
            }


            if (fillFull < percentElements.Count)
            {
                OpenLectureByRowNum(fillFull);
                bool isFinished = false;
                int lastSentenceNumber = 0, translationsNumberLeft = 1000, translatedNumber = 0;
                do
                {
                    isFinished = FillEmptySegments(ref lastSentenceNumber, translationsNumberLeft, out translatedNumber, false, true);
                } while (!isFinished);

                ClickBackEditor();

                Thread.Sleep(15000);

                Driver.FindElement(By.XPath(".//div[contains(@data-bind,'personalProgressView')]")).SendKeys(OpenQA.Selenium.Keys.F5);
            }
            
        }

        // TODO
        protected bool GetIsSegmentVoted()
        {
            setDriverTimeoutMinimum();
            bool isExistVoteUp = IsElementPresent(By.XPath(
                ".//div[@id='translations-body']//table//tr//td[5]/div//span[contains(@class,'fa-thumbs-up')][contains(@class, 'disabled')]"));
            Console.WriteLine("голос за: " + (isExistVoteUp ? "есть" : "нет"));

            bool isExistVoteDown = IsElementPresent(By.XPath(".//div[@id='translations-body']//table//tr//td[5]/div//span[contains(@class,'fa-thumbs-down')][contains(@class, 'disabled')]"));
            Console.WriteLine("голос против: " + (isExistVoteDown ? "есть" : "нет"));
            // TODO добавить против всех

            setDriverTimeoutDefault();
            return isExistVoteUp || isExistVoteDown;
        }

        // TODO
        protected Decimal GetUserCourseProgress()
        {
            Decimal resultProgress = 0;
            // Получить проценты личного прогресса всех лекций
            IList<IWebElement> percentElements = Driver.FindElements(By.XPath(".//div[contains(@data-bind,'personalProgressView')]"));
            int personalProgress = 0;
            foreach (IWebElement el in percentElements)
            {
                // Суммируем личные прогрессы
                personalProgress += int.Parse(el.Text.Replace("%", ""));
            }

            resultProgress = personalProgress / percentElements.Count;
            Console.WriteLine("вижу лекций: " + percentElements.Count);
            Console.WriteLine("итоговый прогресс: " + resultProgress);
            return resultProgress;
        }

        // TODO
        protected int GetAchieveProgressPercent(string achieveType)
        {
            string achieveText = Driver.FindElement(By.XPath(
                ".//ul[@class='achive-list']//li[" + GetAchieveNumInList(achieveType) + "]//small[contains(@data-bind,'progress')]")).Text.Trim();
            // Прогресс
            int achieveProgress = int.Parse(achieveText.Substring(0, achieveText.IndexOf("%")));
            Console.WriteLine("прогресс: " + achieveProgress);
            return achieveProgress;
        }

        // TODO
        protected bool GetIsExistAchieveMessage()
        {
            return IsElementDisplayed(By.XPath(".//div[@id='achieve-popup']"));// && IsElementDisplayed(By.XPath(".//div[@id='achieve-popup']//img"));//TODO добавить проверку картинки
        }

        // TODO
        protected void GetAchieveMessageData(out string achieveType, out int level)
        {
            string messageText = Driver.FindElement(By.XPath(".//div[@id='achieve-popup']//strong")).Text.Trim();
            achieveType = messageText.Substring(0, messageText.IndexOf(" "));
            int indexLevelStart = messageText.IndexOf("(") + 1;
            level = int.Parse(messageText.Substring(indexLevelStart, messageText.IndexOf("/") - indexLevelStart));
        }

        // TODO
        protected int GetAchieveNumInList(string achieveType)
        {
            int rowNumber = 0;
            bool isExist = false;
            IList<IWebElement> achieveList = Driver.FindElements(By.XPath(".//ul[@class='achive-list']//li//strong"));
            for (int i = 0; i < achieveList.Count; ++i)
            {
                if (achieveList[i].Text.Trim().Contains(achieveType))
                {
                    isExist = true;
                    rowNumber = i + 1;
                    break;
                }
            }
            // Проверить, что есть такая награда
            Assert.IsTrue(isExist, "Ошибка: такой тип ачивки не найден");

            // Вернуть номер этой награды в списке
            return rowNumber;
        }
    }
}