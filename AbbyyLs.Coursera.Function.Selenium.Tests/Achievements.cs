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
            int achieveLevel = 3;
            // Количество переводов для получения уровня 
            int levelLimit = 250;
            // Добавить нужное количество переводов
            GetTranslatorLevel(achieveLevel, levelLimit);
        }

        /// <summary>
        /// Тест: получение награды "Эксперт. 1 Уровень" (проголосовать за переводы к 5 сегментам)
        /// </summary>
        [Test]
        public void b_1_ExpertLevel1()
        {
            // Уровень
            int achieveLevel = 1;
            // Количество переводов для получения уровня 
            int levelLimit = 5;
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
        /// Тест: получение награды "Специалист. 1 уровень" (перевести 1% курса)
        /// </summary>
        [Test]
        public void e_1_SpecialistLevel1()
        {

        }

        /// <summary>
        /// Тест: добраться до 10 места в лидерборде
        /// </summary>
        [Test]
        public void c_1_LeaderListTest()
        {
            int needUserPosition = 10;
            // Добраться до нужного места в лидерборде
            GetPositionLeaderboard(needUserPosition);
        }

        /// <summary>
        /// Тест: добраться до 1 места в лидерборде
        /// </summary>
        [Test]
        public void d_1_FirstPlaceTest()
        {
            int needUserPosition = 1;
            // Добраться до нужного места в лидерборде
            GetPositionLeaderboard(needUserPosition);
        }

        [Test]
        public void TmpTest()
        {
            OpenCoursePage();
            // Открыть курс
            string courseName = OpenCourseMinProgress();
            Console.WriteLine(courseName);
            Decimal curProgress = GetUserCourseProgress();

            OpenUserProfileFromCourse();
            
            Decimal userPr1 = GetUserRating();
            Console.WriteLine("рейтинг пользователя " + userPr1);
            AddTranslationsLimitNumber(1);
            OpenUserProfileFromCourse();
            Decimal userPr2 = GetUserRating();
            Console.WriteLine("рейтинг пользователя " + userPr2);
            AddTranslationsLimitNumber(1);
            OpenUserProfileFromCourse();
            Decimal userPr3 = GetUserRating();
            Console.WriteLine("рейтинг пользователя " + userPr3);
            AddTranslationsLimitNumber(1);
            OpenUserProfileFromCourse();
            Decimal userPr4 = GetUserRating();
            Console.WriteLine("рейтинг пользователя " + userPr4);
            AddTranslationsLimitNumber(1);
            OpenUserProfileFromCourse();
            Decimal userPr5 = GetUserRating();
            Console.WriteLine("рейтинг пользователя " + userPr5);
            AddTranslationsLimitNumber(1);
            OpenUserProfileFromCourse();
            Decimal userPr6 = GetUserRating();
            Console.WriteLine("рейтинг пользователя " + userPr6);
            AddTranslationsLimitNumber(1);
            OpenUserProfileFromCourse();
            Decimal userPr7 = GetUserRating();
            Console.WriteLine("рейтинг пользователя " + userPr7);
            AddTranslationsLimitNumber(1);
            OpenUserProfileFromCourse();
            Decimal userPr8 = GetUserRating();
            Console.WriteLine("рейтинг пользователя " + userPr8);
            AddTranslationsLimitNumber(1);
            OpenUserProfileFromCourse();
            Decimal userPr9 = GetUserRating();
            Console.WriteLine("рейтинг пользователя " + userPr9);

        }

        /// <summary>
        /// Получить уровень в награде "Переводчик"
        /// </summary>
        /// <param name="levelLimit">количество переводов для получения уровня</param>
        protected void GetTranslatorLevel(int levelAchieve, int levelLimit)
        {
            string achieveType = "Translator";
            bool isNeedChangeUser = true;
            int translationsNumber = 0;

            for (int i = 0; i < TestUserList.Count; ++i )
            {
                // Открыть профиль пользователя
                OpenUserProfileFromHomePage();
                // Количество переведенных пользователем предложений
                translationsNumber = GetAchieveProgress(achieveType);
                isNeedChangeUser = translationsNumber >= levelLimit;

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

            Assert.IsFalse(isNeedChangeUser, "Проблема: все пользователи исчерпаны");

            int levelBefore = GetAchieveLevel(achieveType);
            Assert.IsTrue(levelBefore < levelAchieve, "Ошибка: текущий уровень должен быть меньше достигаемого");

            Console.WriteLine("переведено до: " + translationsNumber);
            Console.WriteLine("уровень до: " + levelBefore);

            // Добавить предложения до N-1
            AddTranslationsLimitNumber(levelLimit - translationsNumber - 1);

            // Проверить, что не появилось окно о присуждении награды
            Assert.IsFalse(GetIsExistAchieveMessage(), "Ошибка: появилось окно присуждения награды раньше времени");

            // Добавить предложение до N
            AddTranslationsLimitNumber(1);

            // Проверить, что появилось окно о присуждении награды
            Assert.IsTrue(GetIsExistAchieveMessage(), "Ошибка: не появилось окно присуждения награды");
            string currentAchieveType;
            int currentLevelAchieve;
            // Получить текущие данные о награде
            GetAchieveMessageData(out currentAchieveType, out currentLevelAchieve);

            // Проверить, что появился правильный тип награды
            Assert.AreEqual(achieveType, currentAchieveType, "Ошибка: отображается не та награда: " + currentAchieveType);
            // Проверить, что появился правильный уровень
            Assert.AreEqual(levelAchieve, currentLevelAchieve,
                "Ошибка: отображается не тот уровень награды в сообщении: " + currentLevelAchieve);

            // Открыть профиль
            OpenUserProfileFromCourse();
            // Получить количество переводов
            int translationsNumberAfter = GetAchieveProgress(achieveType);
            Console.WriteLine("переведено после: " + translationsNumberAfter);

            // Проверить, что отображается правильное число переводов в профиле в информации о награде
            Assert.AreEqual(levelLimit, translationsNumberAfter, "Ошибка: неправильный подсчет переводов");

            // Проверить, что отображается правильный уровень в профиле в информации о награде
            int levelAfter = GetAchieveLevel(achieveType);
            Assert.AreEqual(levelAchieve, levelAfter,
                "Ошибка: отображается не тот уровень награды в профиле: " + levelAfter);

            OpenCoursePage();
            // Добавить предложение до N + 1
            AddTranslationsLimitNumber(1);

            // Проверить, что не появилось окно о присуждении награды
            Assert.IsFalse(GetIsExistAchieveMessage(), "Ошибка: появилось окно присуждения награды");
        }

        /// <summary>
        /// Добавить переводы
        /// </summary>
        /// <param name="translationsNumberLeft">количество переводов, которое надо добавить</param>
        /// <param name="isNeedConsiderRating">нужно ли учитывать рейтинг (если учитывать рейтинг, то количество переводов зависит от количества слова в Source</param>
        protected void AddTranslationsLimitNumber(int translationsNumberLeft, bool isNeedConsiderRating = false)
        {
            int lectureRowNumber = 0;

            // Перейти к списку доступных курсов
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            string courseName = OpenCourseMinProgress();
            Console.WriteLine("курс: " + courseName);

            while (translationsNumberLeft > 0)
            {
                Console.WriteLine("Добавляем переводы в новую лекцию");
                // Перейти в лекцию
                lectureRowNumber = SelectLectureToTranslate(lectureRowNumber + 1);
                Console.WriteLine("лекция номер " + lectureRowNumber);
                OpenLectureByRowNum(lectureRowNumber);

                int lastSentenceNumber = 0, translatedNumber = 0;
                bool isFinished = false;

                do
                {
                    // Заполнить группу предложений
                    // Лекция заполняется не за раз, а группами, т.к. селениум видит только 34-35 предложений за раз и приходится
                    // переобновлять хранящиеся в списке сегменты, чтобы можно было заполнить их все
                    isFinished = FillEmptySegments(ref lastSentenceNumber, translationsNumberLeft, out translatedNumber, isNeedConsiderRating);
                    
                    translationsNumberLeft -= translatedNumber;
                    Console.WriteLine("translationsNumberLimit после: " + translationsNumberLeft);
                } while (translationsNumberLeft > 0 && !isFinished);

                // Выйти из редактора
                ClickBackEditor();
                MakeScreenShot();

                if (translationsNumberLeft <= 0)
                {
                    Console.WriteLine("добавление остановлено, т.к. добавлено достаточно предложений");
                }
                if (isFinished)
                {
                    Console.WriteLine("добавление остановлено, т.к. закончились предложения");
                }
            }
        }

        /// <summary>
        /// Добавить переводы в пустые сегменты
        /// </summary>
        /// <param name="lastLastFactRow">IN/OUT: последний номер в строке с последним сегментом при предыдущем проходе редактора</param>
        /// <param name="translationNumberLimit">IN: максимальное количество переводов для добавления</param>
        /// <param name="translatedNumber">OUT: количество добавленных переводов</param>
        /// <returns>закончилась ли лекция (true: пустые предложения закончились)</returns>
        protected bool FillEmptySegments(ref int lastLastFactRow, int translationNumberLimit, out int translatedNumber, bool isNeedConsiderRating = false)
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

            if (!isFinished)
            {
                // Индекс для начала заполнения
                int startIndex = lastLastFactRow - curFirstRow + 1;
                Console.WriteLine("start: " + startIndex);
                // Если зашли в лекцию, в которую уже заходили - лучше начинать не с первой строки
                if (curFirstRow > lastLastFactRow && curFirstRow > 1)
                {
                    startIndex = 17;
                }

                string translationText = "Test Translation " + DateTime.Now;

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
                            if (isNeedConsiderRating)
                            {
                                // Учет рейтинга - то есть добавляем переводы для увеличения рейтинга
                                // Чтобы не перемахнуть через нужное число - проверяем количество слов в Source
                                // Мы приняли при расчете нужного количества переводов, что за один перевод дается 3 балла к рейтингу
                                // Но, по опыту выяснилось, если число слов больше 30, то баллов больше
                                // и на каждые 30 добавляем 3 балла, то есть уменьшаем оставшееся количество переводов на (N / 30)
                                int sourceWordsCount = sourceSegmentsList[i].Text.Split().Length;
                                translatedNumber += (sourceWordsCount / 30);
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

        /// <summary>
        /// Достичь уровня в награде Эксперт
        /// </summary>
        /// <param name="levelAchieve">уровень награды</param>
        /// <param name="levelLimit">количество голосов, которые нужно сделать</param>
        protected void GetExpertLevel(int levelAchieve,  int levelLimit)
        {
            string achieveType = "Expert";
            bool isNeedChangeUser = true;
            int voteNumber = 0;

            for (int i = 0; i < TestUserList.Count; ++i)
            {
                // Открыть профиль пользователя
                OpenUserProfileFromHomePage();
                // Количество переведенных пользователем предложений
                voteNumber = GetAchieveProgress(achieveType);
                isNeedChangeUser = voteNumber >= levelLimit;

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

            Assert.IsFalse(isNeedChangeUser, "Проблема: все пользователи исчерпаны");
            int levelBefore = GetAchieveLevel(achieveType);
            Assert.IsTrue(levelBefore < levelAchieve, "Ошибка: текущий уровень должен быть меньше достигаемого");

            Console.WriteLine("переведено до: " + voteNumber);
            Console.WriteLine("уровень до: " + levelBefore);

            int voteNumberLeft = levelLimit - voteNumber - 1;
            // Проголосовать до N - 1
            VoteSegments(voteNumberLeft);
            // Проверить, что нет оповещения о награде
            Assert.IsFalse(GetIsExistAchieveMessage(), "Ошибка: раньше времени появилось оповещение о награде");
            // Проголосовать до N
            VoteSegments(1);
            // Проверить, что оповещение появилось
            Assert.IsTrue(GetIsExistAchieveMessage(), "Ошибка: не появилось оповещение о награде");

            string currentAchieveType;
            int currentLevelAchieve;
            // Получить текущие данные о награде
            GetAchieveMessageData(out currentAchieveType, out currentLevelAchieve);

            // Открыть профиль
            OpenUserProfileFromCourse();
            // Получить количество переводов
            int voteNumberAfter = GetAchieveProgress(achieveType);
            Assert.AreEqual(levelLimit, voteNumberAfter, "Ошибка: неправильный подсчет голосов");

            // Проверить, что отображается правильный уровень в профиле в информации о награде
            int levelAfter = GetAchieveLevel(achieveType);
            Assert.AreEqual(levelAchieve, levelAfter,
                "Ошибка: отображается не тот уровень награды в профиле: " + levelAfter);

            // Проголосовать до N + 1
            VoteSegments(1);
            // Проверить, что оповещение не появилось
            Assert.IsFalse(GetIsExistAchieveMessage(), "Ошибка: появилось лишнее оповещение о награде");
        }

        protected void VoteSegments(int voteNumberLeft)
        {
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            string courseName = OpenCourseMinProgress();
            Console.WriteLine("курс: " + courseName);
            while (voteNumberLeft > 0)
            {
                // Открыть лекцию
                int lectureRowNum = SelectLectureToVote();
                Console.WriteLine("lectureRowNum: " + lectureRowNum);
                OpenLectureByRowNum(lectureRowNum);

                int lastLastFactRow = 0, votedCounter = 0;
                bool isFinished = false;
                do
                {
                    isFinished = VoteNumOfSegments(ref lastLastFactRow, voteNumberLeft, out votedCounter);
                    voteNumberLeft -= votedCounter;
                    Console.WriteLine("levelLimit после: " + voteNumberLeft);
                } while (voteNumberLeft > 0 && !isFinished);

                if (voteNumberLeft <= 0)
                {
                    break;
                }
            }

            Assert.IsTrue(voteNumberLeft <= 0, "Ошибка: закончилась лекция");

            ClickBackEditor();
        }

        /// <summary>
        /// Проголосовать за перевод в отдельных сегментах (не голосовать за сегменты, где уже есть мой голос)
        /// </summary>
        /// <param name="lastLastFactRow">IN/OUT: последний номер в строке с последним сегментом при предыдущем проходе редактора</param>
        /// <param name="limitVotes">IN: максимальное количество голосов для добавления</param>
        /// <param name="countVoted">OUT: количество добавленных голосов</param>
        /// <returns></returns>
        protected bool VoteNumOfSegments(ref int lastLastFactRow, int votesNumberLimit, out int votesNumber)
        {
            // Описание аналогично FillEmptySegments(...)

            Console.WriteLine("LAST FN(L) = " + lastLastFactRow);
            Console.WriteLine("Осталось проголосовать: " + votesNumberLimit);

            // Видимые сегменты
            IList<IWebElement> segmentsList = Driver.FindElements(By.CssSelector("#segments-body div table tr td:nth-child(4) div"));
            Console.WriteLine("Всего видно: " + segmentsList.Count);

            // Фактический номер первой видимой строки
            int curFirstRow = int.Parse(Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(1) div")).Text.Trim());
            Console.WriteLine("FN(1) = " + curFirstRow);
            // Фактический номер последней видимой строки
            int curLastRow = int.Parse(Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + segmentsList.Count + ") td:nth-child(1) div")).Text.Trim());
            Console.WriteLine("FN(L) = " + curLastRow);

            votesNumber = 0;
            // Проверяем, закончилась ли лекция
            bool isFinished = curLastRow == lastLastFactRow;

            if (!isFinished)
            {
                // Индекс для начала заполнения
                int startIndex = lastLastFactRow - curFirstRow + 1;
                Console.WriteLine("start: " + startIndex);
                // Если зашли в лекцию, в которую уже заходили - лучше начинать не с первой строки
                if (curFirstRow > lastLastFactRow && curFirstRow > 1)
                {
                    startIndex = 17;
                }

                for (int i = startIndex; i < segmentsList.Count; ++i)
                {
                    segmentsList[i].Click();
                    segmentsList[i].Click();
                    Console.WriteLine("номер строки: " + (i + 1));
                    // Проверить, проголосовали ли уже за какой-либо перевод здесь?
                    bool isVoted = VoteSuggestedTranslation();

                    if (isVoted)
                    {
                        ++votesNumber;
                        if (votesNumber >= votesNumberLimit)
                        {
                            Console.WriteLine("Достаточно проголосовано, выходим из лекции");
                            break;
                        }
                    }
                }
            }
            lastLastFactRow = curLastRow;
            return isFinished;
        }

        /// <summary>
        /// Выбор лекции для голосования (с личным прогрессом меньше 100 и общим > 0)
        /// </summary>
        /// <returns>номер строки с лекцией</returns>
        protected int SelectLectureToVote()
        {
            // Получить проценты личного прогресса всех лекций
            IList<IWebElement> percentElements = Driver.FindElements(By.XPath(".//div[contains(@data-bind,'personalProgressView')]"));
            // Проверить, что список лекций не пуст
            Assert.IsTrue(percentElements.Count > 0, "Ошибка: список лекций пуст");
            bool isLectureExist = false;
            int fullProgress = 0, personalProgress = 0;
            int rowNumber = 0;
            for (int i = 0; i < percentElements.Count; ++i)
            {
                // Выбираем лекцию с личным и общим прогрессом меньше 100
                personalProgress = int.Parse(percentElements[i].Text.Replace("%", ""));
                if (personalProgress < 100)
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
            }
            Assert.IsTrue(isLectureExist, "Ошибка: нет подходящей лекции (с личным прогрессом < 100 и общим меньше 100 )");

            // Вернуть номер строки с лекцией
            return rowNumber;
        }

        /// <summary>
        /// Выбор лекции для перевода (личный прогресс меньше 100)
        /// </summary>
        /// <param name="startRowNumber">номер лекции для начала поиска</param>
        /// <returns>номер строки</returns>
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
        /// Проголосовать за предложенный перевод
        /// </summary>
        /// <returns>удалось проголосовать</returns>
        protected bool VoteSuggestedTranslation()
        {
            bool voteIsDone = false;

            // Если для сегмента не было голосов
            if (!GetIsSegmentVoted())
            {
                setDriverTimeoutMinimum();
                IList<IWebElement> translationsList = Driver.FindElements(By.XPath(
                    ".//div[@id='translations-body']//table//tr//td[5]/div//span[contains(@class,'fa-thumbs-up')]"));
                setDriverTimeoutDefault();
                Console.WriteLine(translationsList.Count > 0 ? "переводы есть" : "переводов нет");

                // Пробуем проголосовать за предложенные переводы
                for (int i = 0; i < translationsList.Count; ++i)
                {
                    if (!translationsList[i].GetAttribute("class").Contains("disabled"))
                    {
                        Console.WriteLine("пытаемся проголосовать " + i);
                        translationsList[i].Click();
                        if (GetIsVoteConsideredEditor(true, (i + 1)))
                        {
                            Console.WriteLine("проголосовалось");
                            voteIsDone = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("неа, не проголосовалось");
                        }
                    }
                    else
                    {
                        Console.WriteLine("проголосовано " + i);
                    }
                }
            }

            // Проголосовали
            return voteIsDone;
        }

        /// <summary>
        /// Вернуть: был ли голос в этом сегменте (за/против)
        /// </summary>
        /// <returns>голос был</returns>
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

        /// <summary>
        /// Получить рейтинг пользователя по месту в лидерборде
        /// </summary>
        /// <param name="userPosition">место в лидерборде</param>
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
        /// Набрать количество голосов за перевод
        /// </summary>
        /// <param name="votesGetNumber">количество голосов для уровня</param>
        protected void GetProfessionalLevel(int levelAchieve, int levelLimit)
        {
            string achieveType = "Professional";
            int currentLevel = 0;
            bool isNeedChangeUser = false;
            int userIndex = 0;

            for (int i = 0; i < TestUserList.Count; ++i)
            {
                // Открыть профиль пользователя
                OpenUserProfileFromHomePage();
                // Количество переведенных пользователем предложений
                currentLevel = GetAchieveLevel(achieveType);
                isNeedChangeUser = currentLevel >= levelAchieve;

                if (isNeedChangeUser)
                {
                    LogoutUser();
                    LoginUser(TestUserList[i]);
                }
                else
                {
                    Console.WriteLine("пользователь: " + (i - 1));
                    userIndex = (i - 1);
                    break;
                }
            }

            string courseName;
            int lectureRowNumber, translationRowNumber;
            string translationText = "Test Translation " + DateTime.Now;
            AddTranslation(translationText, out courseName, out lectureRowNumber, out translationRowNumber);
            Console.WriteLine("курс: " + courseName + ", номер лекции: " + lectureRowNumber + ", номер для перевода: " + translationRowNumber);
            ClickBackEditor();
            LogoutUser();

            int startUser = userIndex + 1;
            for (int i = startUser; i < levelLimit + startUser; ++i)
            {
                LoginUser(TestUserList[i]);
                VoteCurrentTranslation(lectureRowNumber, translationRowNumber, translationText);
                Console.WriteLine("проголосовал!");
                LogoutUser();
            }

            if (userIndex == -1)
            {
                LoginUser(User1);
            }
            else if (userIndex < TestUserList.Count)
            {
                LoginUser(TestUserList[userIndex]);
            }

            bool isExistMessage = GetIsExistAchieveMessage();
            Console.WriteLine(isExistMessage ? "e" : "n");

            string mesAchieveType;
            int mesAchieveLevel;
            GetAchieveMessageData(out mesAchieveType, out mesAchieveLevel);
            Assert.AreEqual(achieveType, mesAchieveType, "Ошибка: сообщение не о той награде: " + mesAchieveType);
            Assert.AreEqual(levelAchieve, mesAchieveLevel, "Ошибка: в сообщении не тот уровень награды: " + mesAchieveLevel);

            Console.WriteLine("сообщение: ачивка: " + achieveType + "уровень: " + mesAchieveLevel);
            OpenUserProfileFromCourse();

            int professionalProgress = GetAchieveProgress(achieveType);
            Console.WriteLine("в профиле прогресс: " + professionalProgress);
            Assert.AreEqual(levelLimit, professionalProgress, "Ошибка: в профиле неправильный подсчет голосов");
        }

        /// <summary>
        /// Проголосовать за конкретный перевод
        /// </summary>
        /// <param name="lectureRowNumber">номер строки с лекцией</param>
        /// <param name="translationRowNumber">номер строки с сегментом</param>
        /// <param name="translationText">текст перевода</param>
        protected void VoteCurrentTranslation(int lectureRowNumber, int translationRowNumber, string translationText)
        {
            // Открыть лекцию
            OpenLectureByRowNum(lectureRowNumber);
            // Перейти в строку с добавленным переводом
            ClickEditorRowByNum(translationRowNumber);
            // Получить номер строки с добавленым переводом в списке предложенных переводов для предложения
            int rowNumber = GetSuggestedTranslationRowNum(translationText);
            // Проголосовать За
            VoteFromEditor(true, rowNumber);
            ClickBackEditor();
        }

        /// <summary>
        /// Увеличить рейтинг пользователя, чтобы попасть на определенное место в лидерборде
        /// </summary>
        /// <param name="needUserPosition">нужная позиция в лидерборде</param>
        protected void GetPositionLeaderboard(int needUserPosition)
        {
            OpenCoursePage();
            // Открыть профиль пользователя
            OpenUserProfileFromCourse();

            // Позиция пользователя
            int userPosition = GetUserPosition();

            if (userPosition <= needUserPosition)
            {
                Assert.Fail("Проблема! юзер слишком мощный!");
                // TODO Использовать другого пользователя
            }

            // Открыть лидерборд
            OpenLeaderboardPage();

            // Получить рейтинг пользователя на нужном месте
            Decimal ratingNeedPosition = GetLeaderboardPositionRating(needUserPosition);
            Console.WriteLine("рейтинг пользователя в лидерборде: " + ratingNeedPosition);

            bool isNeedAddTranslations = false;
            int minimumNumberToAdd = 0;
            Decimal ratingBefore = 0;
            do
            {
                OpenUserProfileFromCourse();
                ratingBefore = GetUserRating();
                Console.WriteLine("Сейчас рейтинг пользователя: " + ratingBefore);

                // Нужно добавить переводов
                minimumNumberToAdd = (int)((ratingNeedPosition - ratingBefore) / 3);
                Console.WriteLine("нужно переводов: " + minimumNumberToAdd);
                isNeedAddTranslations = minimumNumberToAdd > 0;
                if (isNeedAddTranslations)
                {
                    AddTranslationsLimitNumber(minimumNumberToAdd, true);
                }
            } while (isNeedAddTranslations);

            userPosition = GetUserPosition();
            Console.WriteLine("Сейчас место пользователя: " + userPosition);
            if (userPosition <= needUserPosition)
            {
                Console.WriteLine("Перелёт");
                Assert.Fail();
            }

            while (userPosition > needUserPosition)
            {
                Console.WriteLine("добить до места!");
                AddTranslationsLimitNumber(1);
                OpenUserProfileFromCourse();
                userPosition = GetUserPosition();
                Console.WriteLine("место: " + userPosition);
                Console.WriteLine("рейтинг: " + GetUserRating());
            }
            Console.WriteLine("Отличненько!");
            OpenLeaderboardPage();
            Thread.Sleep(3000);
        }

        /// <summary>
        /// Получение итогового личного прогресса пользователя для курса
        /// </summary>
        /// <returns>прогресс пользователя по курсу</returns>
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

        /// <summary>
        /// Получить прогресс ачивки в профиле
        /// </summary>
        /// <param name="achieveType">тип ачивки</param>
        /// <returns>прогресс</returns>
        protected int GetAchieveProgress(string achieveType)
        {
            string achieveText = Driver.FindElement(By.XPath(
                ".//ul[@class='achive-list']//li[" + GetAchieveNumInList(achieveType) + "]//small[contains(@data-bind,'progress')]")).Text.Trim();
            // Прогресс
            int achieveProgress = int.Parse(achieveText.Substring(0, achieveText.IndexOf("/")));
            Console.WriteLine("прогресс: " + achieveProgress);
            return achieveProgress;
        }

        /// <summary>
        /// Получить текущий уровень награды в профиле
        /// </summary>
        /// <param name="achieveType">тип награды</param>
        /// <returns>уровень</returns>
        protected int GetAchieveLevel(string achieveType)
        {
            string achieveText = Driver.FindElement(By.XPath(
                ".//ul[@class='achive-list']//li[" + GetAchieveNumInList(achieveType) + "]//strong")).Text.Trim();
            int indexStart = achieveText.IndexOf("(") + 1;
            // Уровень
            int achieveLevel = int.Parse(achieveText.Substring(indexStart, achieveText.IndexOf("/") - indexStart));
            Console.WriteLine("уровень: " + achieveLevel);
            return achieveLevel;
        }

        protected bool GetIsExistAchieveMessage()
        {
            return IsElementDisplayed(By.XPath(".//div[@id='achieve-popup']"));// && IsElementDisplayed(By.XPath(".//div[@id='achieve-popup']//img"));//TODO добавить проверку картинки
        }

        /// <summary>
        /// Получить информацию о награде в всплывающем окне о присуждении награды
        /// </summary>
        /// <param name="achieveType">OUT: тип награды</param>
        /// <param name="level">OUT: присужденный уровень</param>
        protected void GetAchieveMessageData(out string achieveType, out int level)
        {
            string messageText = Driver.FindElement(By.XPath(".//div[@id='achieve-popup']//strong")).Text.Trim();
            achieveType = messageText.Substring(0, messageText.IndexOf(" "));
            int indexLevelStart = messageText.IndexOf("(") + 1;
            level = int.Parse(messageText.Substring(indexLevelStart, messageText.IndexOf("/") - indexLevelStart));
        }

        /// <summary>
        /// Получить номер награды в списке (для дальнейшего получения прогресса награды)
        /// </summary>
        /// <param name="achieveType">тип награды</param>
        /// <returns>номер в списке</returns>
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