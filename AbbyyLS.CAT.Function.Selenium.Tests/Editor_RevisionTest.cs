using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

using OpenQA.Selenium.Interactions;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class Editor_RevisionTest : BaseTest
    {
        public Editor_RevisionTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        protected const string REVISION_TYPE_AUTOSAVE = "Autosave";
        protected const string REVISION_TYPE_CONFIRMED = "Confirmed";
        protected const string REVISION_TYPE_INSERT_MT = "Insert MT";
        protected const string REVISION_TYPE_INSERT_TM = "Insert TM";
        protected const string REVISION_TYPE_ROLLBACK = "Rollback";

        /// <summary>
        /// Старт тестов, переменные
        /// </summary>
        [SetUp]
        public void SetupTest()
        {
            
        }

        /// <summary>
        /// ТЕСТ: проверка неактивности кнопки Rollback , если ни одна из ревизий не выбрана
        /// </summary>
        [Test]
        public void RollbackBtnDisabledTest()
        {
            InitProject();

            // Добавить текст в Target
            string text = "Text" + DateTime.Now.Ticks;
            AddTextTarget(1, text);
            // Сохранить
            ClickAutoSaveBtn();
            
            // Открыть вкладку Ревизии
            OpenRevisionTab();

            // Получить количество ревизий
            int revisionListCount = GetRevisionListCount();
            Console.WriteLine("количество строк в ревизиях: " + revisionListCount);

            // Проверить, что количество больше 0 (перевод появился в ревизиях)
            Assert.IsTrue(revisionListCount > 0, "Ошибка: перевод не появился в ревизиях");

            // Проверить, что кнопка заблокирована
            Assert.IsTrue(GetRollbackBtnDisabled(), "Ошибка: кнопка Rollback должна быть заблокирована");
        }

        /// <summary>
        /// ТЕСТ: проверка активности кнопки Rollback , если ревизия выбрана
        /// </summary>
        [Test]
        public void RollbackBtnEnabledTest()
        {
            InitProject();

            // Добавить текст в Target
            string text = "Text" + DateTime.Now.Ticks;
            AddTextTarget(1, text);
            // Сохранить
            ClickAutoSaveBtn();

            // Открыть вкладку Ревизии
            OpenRevisionTab();

            // Получить количество ревизий
            int revisionListCount = GetRevisionListCount();
            Console.WriteLine("количество строк в ревизиях: " + revisionListCount);

            // Проверить, что количество больше 0 (перевод появился в ревизиях)
            Assert.IsTrue(revisionListCount > 0, "Ошибка: перевод не появился в ревизиях");

            // Выбрать ревизию
            SelectRevision(1);

            // Проверить, что кнопка не заблокирована
            Assert.IsFalse(GetRollbackBtnDisabled(), "Ошибка: кнопка Rollback не должна быть заблокирована");
        }

        /// <summary>
        /// ТЕСТ: проверка сортировки по дате/времени по кнопке Time
        /// </summary>
        [Test]
        public void TimeBtnTest()
        {
            InitProject();

            // Количество 
            int revisionNumberCount = 3;
            // Добавить ревизии
            AddTranslationsToSegment(1, revisionNumberCount);

            int revisionCount = GetRevisionListCount();
            Assert.IsTrue(revisionCount >= revisionNumberCount, "Ошибка: количество ревизий не совпадает (" + revisionCount + ")");

            // Текст и время первой ревизии До
            string revisionTextBefore = GetRevisionText(1);
            string revisionTimeBefore = GetRevisionTime(1);

            Console.WriteLine("до: " + revisionTextBefore + " " + revisionTimeBefore);
            // Кликнуть Time
            ClickTimeToSortRevisions();

            // Текст и время первой ревизии После
            string revisionTextAfter = GetRevisionText(1);
            string revisionTimeAfter = GetRevisionTime(1);

            Console.WriteLine("после: " + revisionTextAfter + " " + revisionTimeAfter);

            // Изменился ли текст
            bool isTextChanged = revisionTextBefore != revisionTextAfter;
            // Изменилось ли время
            bool isTimeChanged = revisionTimeBefore != revisionTimeAfter;

            bool isOk = isTextChanged && isTimeChanged;
            string errorMessage = "";
            // Проверить, что текст изменился
            if (!isTextChanged)
            {
                errorMessage += "Ошибка: текст не изменился\n";
            }

            // Проверить, что время изменилось
            if (!isTimeChanged)
            {
                errorMessage += "Ошибка: время не изменилось\n";
            }

            // Вывести ошибки
            Assert.IsTrue(isOk, errorMessage);
        }

        const string byHotkey = "byHotkey";
        const string byButton = "byButton";
        const string byTimeoutAutoSave = "byTimeoutAutoSave";
        /// <summary>
        /// ТЕСТ: проверка хоткея Автосохранение
        /// </summary>
        [Test]
        [TestCase(byHotkey)]
        [TestCase(byButton)]
        [TestCase(byTimeoutAutoSave)]
        public void AutoSaveTest(string byHotkeyOrButtonOrTime)
        {
            InitProject();

            // Добавить текст в Target
            string text = "Text" + DateTime.Now.Ticks;
            AddTextTarget(1, text);
            // Сохранить
            if (byHotkeyOrButtonOrTime == byHotkey)
            {
                // Нажат хоткей автосохранения
                ClickAutoSaveHotkey(1);
            }
            else if (byHotkeyOrButtonOrTime == byButton)
            {
                // Нажать кнопку сохранения
                ClickAutoSaveBtn();
            }
            else if (byHotkeyOrButtonOrTime == byTimeoutAutoSave)
            {
                // Дождаться автосохранения по времени
                Thread.Sleep(5000);
            }

            // Открыть вкладку Ревизии
            OpenRevisionTab();

            // Получить количество ревизий
            int revisionListCount = GetRevisionListCount();
            Console.WriteLine("количество строк в ревизиях: " + revisionListCount);

            // Проверить, что количество больше 0 (перевод появился в ревизиях)
            Assert.IsTrue(revisionListCount > 0, "Ошибка: перевод не появился в ревизиях");

            // Получить тип ревизии
            string revisionType = GetRevisionType(1).Trim();
            // Проверить тип
            Assert.AreEqual(REVISION_TYPE_AUTOSAVE, revisionType,
                "Ошибка: тип не совпадает: " + revisionType);
        }

        /// <summary>
        /// ТЕСТ: проверка хоткея Confirm
        /// </summary>
        [Test]
        [TestCase(byHotkey)]
        [TestCase(byButton)]
        public void ConfirmTest(string byHotkeyOrButton)
        {
            InitProject();

            // Добавить текст в Target
            string text = "Text" + DateTime.Now.Ticks;
            int segmentRowNumber = 1;
            AddTextTarget(segmentRowNumber, text);
            // Подтвердить
            if (byHotkeyOrButton == byHotkey)
            {
                // Нажать хоткей подтверждения
                ClickConfirmHotkey(segmentRowNumber);
            }
            else if (byHotkeyOrButton == byButton)
            {
                // Нажать кнопку подтверждения
                ClickConfirmBtn(segmentRowNumber);
            }

            // Вернуться в сегмент
            ClickSegmentTarget(segmentRowNumber);

            // Открыть вкладку Ревизии
            OpenRevisionTab();

            // Получить количество ревизий
            int revisionListCount = GetRevisionListCount();
            Console.WriteLine("количество строк в ревизиях: " + revisionListCount);

            // Проверить, что количество больше 0 (перевод появился в ревизиях)
            Assert.IsTrue(revisionListCount > 0, "Ошибка: перевод не появился в ревизиях");

            // Получить тип ревизии
            string revisionType = GetRevisionType(1).Trim();
            // Проверить тип
            Assert.AreEqual(REVISION_TYPE_CONFIRMED, revisionType,
                "Ошибка: тип не совпадает: " + revisionType);
        }

        /// <summary>
        /// ТЕСТ: подтвердить после автосохранения
        /// </summary>
        [Test]
        public void ConfirmAfterSaveTest()
        {
            InitProject();

            // Добавить текст в Target
            string text = "Text" + DateTime.Now.Ticks;
            int segmentRowNumber = 1;
            AddTextTarget(segmentRowNumber, text);
            // Сохранить
            ClickAutoSaveBtn();

            // Открыть вкладку Ревизии
            OpenRevisionTab();

            // Получить количество ревизий
            int revisionListCount = GetRevisionListCount();
            Console.WriteLine("количество строк в ревизиях: " + revisionListCount);

            // Проверить, что количество больше 0 (перевод появился в ревизиях)
            Assert.IsTrue(revisionListCount > 0, "Ошибка: перевод не появился в ревизиях");

            // Получить тип ревизии
            string revisionType = GetRevisionType(1).Trim();
            // Проверить тип
            Assert.AreEqual(REVISION_TYPE_AUTOSAVE, revisionType,
                "Ошибка: тип не совпадает: " + revisionType);

            // Подтвердить
            ClickConfirmBtn(segmentRowNumber);
            // Вернуться
            ClickSegmentTarget(segmentRowNumber);

            int revisionListCountAfter = GetRevisionListCount();
            revisionType = GetRevisionType(1);

            bool isOk = true;
            string errorMessage = "";

            // Проверить, что количество ревизий не изменилось
            if (revisionListCountAfter != revisionListCount)
            {
                isOk = false;
                errorMessage += "Ошибка: количество ревизий изменилось\n";
            }

            // Проверить тип ревизии
            if (revisionType != REVISION_TYPE_CONFIRMED)
            {
                isOk = false;
                errorMessage += "Ошибка: неправильный тип ревизии: " + revisionType;
            }

            // Вывести ошибку
            Assert.IsTrue(isOk, errorMessage);
        }

        const string byDoubleClick = "byDoubleClick";
        /// <summary>
        /// ТЕСТ: вставка перевода из MT
        /// </summary>
        [Test]
        [TestCase(byDoubleClick)]
        [TestCase(byHotkey)]
        public void PasteFromMTTest(string byHotkeyOrDoubleClick)
        {
            InitProject(false, true);

            // Проверить, что есть переводы в панели CAT
            Assert.IsTrue(GetCATTranslationListCount() > 0, "Ошибка: нет переводов в панели САТ");
            int catTranslationNum = GetCATRowNumByType("MT");
            Assert.IsTrue(catTranslationNum > 0, "Ошибка: перевод не TM");

            // Target
            IWebElement el = GetSegmentTargetElement(1);
            if (byHotkeyOrDoubleClick == byHotkey)
            {
                // Ctrl+N - для вставки перевода из CAT-MT (N - номер в панели)
                el.SendKeys(OpenQA.Selenium.Keys.Control + catTranslationNum.ToString());
            }
            else if (byHotkeyOrDoubleClick == byDoubleClick)
            {
                // Двойной клик
                Actions action = new Actions(Driver);
                action.DoubleClick(GetCATTranslationElement(catTranslationNum));
                action.Perform();
            }

            string targetText = el.Text;
            Assert.IsTrue(targetText.Length > 0, "Ошибка: текст не добавился");

            // Проверить, что ревизия сохранилась
            Assert.IsTrue(GetRevisionListCount() > 0, "Ошибка: ревизия не сохранилась");

            string revisionType = GetRevisionType(1);
            // Проверить тип
            Assert.AreEqual(REVISION_TYPE_INSERT_MT, revisionType, "Ошибка: неправильный тип ревизии: " + revisionType);
        }

        /// <summary>
        /// ТЕСТ: вставка перевода из TM
        /// </summary>
        [Test]
        [TestCase(byDoubleClick)]
        [TestCase(byHotkey)]
        public void PasteFromTMTest(string byHotkeyOrDoubleClick)
        {
            InitProject(true);

            // Проверить, что есть переводы в панели CAT
            Assert.IsTrue(GetCATTranslationListCount() > 0, "Ошибка: нет переводов в панели САТ");
            int catTranslationNum = GetCATRowNumByType("TM");
            Assert.IsTrue(catTranslationNum > 0, "Ошибка: перевод не TM");

            IWebElement el = GetSegmentTargetElement(1);
            if (byHotkeyOrDoubleClick == byHotkey)
            {
                // Ctrl+1 - для вставки перевода из CAT-MT
                el.SendKeys(OpenQA.Selenium.Keys.Control + catTranslationNum.ToString());
            }
            else if (byHotkeyOrDoubleClick == byDoubleClick)
            {
                // Двойной клик
                Actions action = new Actions(Driver);
                action.DoubleClick(GetCATTranslationElement(catTranslationNum));
                action.Perform();
            }

            string targetText = el.Text;
            Assert.IsTrue(targetText.Length > 0, "Ошибка: текст не добавился");

            // Проверить, что ревизия сохранилась
            Assert.IsTrue(GetRevisionListCount() > 0, "Ошибка: ревизия не сохранилась");

            string revisionType = GetRevisionType(1);
            // проверить тип
            Assert.AreEqual(REVISION_TYPE_INSERT_TM, revisionType, "Ошибка: неправильный тип ревизии: " + revisionType);
        }


        /// <summary>
        /// ТЕСТ: подтверждение нескольких переводов одного сегмента (несколько ревизий)
        /// </summary>
        [Test]
        public void ConfirmSomeTranslations()
        {
            InitProject();

            // Подтвердить несколько переводов в сегменте
            int translationNumber = 2;
            AddTranslationsToSegment(1, translationNumber);

            // Открыть вкладку Ревизии
            OpenRevisionTab();

            // Получить количество ревизий
            int revisionListCount = GetRevisionListCount();
            Console.WriteLine("количество строк в ревизиях: " + revisionListCount);

            // Проверить количество ревизий (== translationNumber)
            Assert.AreEqual(translationNumber, revisionListCount, "Ошибка: неверное количество ревизий: " + revisionListCount);

            // Проверить типы ревизий
            for (int i = 0; i < revisionListCount; ++i)
            {
                Assert.AreEqual(REVISION_TYPE_CONFIRMED, GetRevisionType((i + 1)), "Ошибка: тип ревизии неправильный");
            }
        }

        /// <summary>
        /// ТЕСТ: автосохранение несколько переводов в одну ревизию
        /// </summary>
        [Test]
        public void AutosaveSomeTranslations()
        {
            InitProject();

            int segmentRowNumber = 1;
            int translationNumber = 2;
            for (int i = 0; i < translationNumber; ++i)
            {
                string text = "Text" + DateTime.Now.Ticks;
                // Добавить текст
                AddTextTarget(segmentRowNumber, text);
                // Сохранить
                ClickAutoSaveBtn();
            }

            // Открыть вкладку Ревизии
            OpenRevisionTab();

            // Получить количество ревизий
            int revisionListCount = GetRevisionListCount();
            Console.WriteLine("количество строк в ревизиях: " + revisionListCount);

            // Проверить количество ревизий - должна быть 1
            Assert.AreEqual(1, revisionListCount, "Ошибка: неверное количество ревизий: " + revisionListCount);

            // Проверить тип ревизии
            Assert.AreEqual(REVISION_TYPE_AUTOSAVE, GetRevisionType(1), "Ошибка: тип ревизии неправильный");
        }

        /// <summary>
        /// ТЕСТ: проверка кнопки Rollback
        /// </summary>
        /// <param name="translationNumber">количество подтверждаемых переводов</param>
        /// <param name="rollbackNumber">номер ревизии для отката (нумерация с 1 - первый добавленный перевод)</param>
        [Test]
        [TestCase(3, 3)]
        [TestCase(3, 2)]
        [TestCase(3, 1)]
        public void RollbackTest(int translationNumber, int rollbackNumber)
        {
            InitProject();

            // Проверка параметров
            Assert.IsTrue(rollbackNumber > 0, "Неверный параметр: rollbackNumber - номер добавленного перевода, начиная с 1");
            Assert.IsTrue(translationNumber >= rollbackNumber, "Неверный параметр: rollbackNumber должен быть меньше translationNumber");


            int segmentRowNumber = 1;
            // Подтвердить нескольк переводов в одном сегменте
            List<string> translationList = AddTranslationsToSegment(segmentRowNumber, translationNumber);

            // Текст ревизии для отката
            string revisionText = translationList[rollbackNumber - 1];

            // Проверить, что все ревизии сохранились
            OpenRevisionTab();
            Assert.AreEqual(translationNumber, GetRevisionListCount(), "Ошибка: неправильное количество ревизий");

            // Выделить ревизию
            int revisionRollBackNumber = (translationNumber - rollbackNumber + 1);
            Assert.IsTrue(SelectRevision(revisionRollBackNumber), "Ошибка: такой ревизии нет");
            Assert.IsFalse(GetRollbackBtnDisabled(), "Ошибка: кнопка Rollback заблокирована");
            
            // Кликнуть Rollback
            ClickRollbackBtn();
            
            // Дождаться открытия диалога подтверждения отката
            Assert.IsTrue(WaitUntilDisplayElement(".//div[@id='rollback']"), "Ошибка: не появился диалог подтверждения отката");

            // Нажать да
            Driver.FindElement(By.XPath(".//div[@id='rollback']//a[contains(@class,'x-btn-blue')]")).Click();
            WaitUntilDisappearElement(".//div[@id='rollback']");

            // Текст в сегменте
            string segmentText = GetSegmentTargetElement(segmentRowNumber).Text;
            // Количество ревизий
            int revisionCount = GetRevisionListCount();
            // Тип новой ревизии
            string revisionType = GetRevisionType(1);

            bool isOk = true;
            string errorMessage = "\n";

            // Проверить, что текст в сегменте совпадает с текстом в ревизии
            if (segmentText != revisionText)
            {
                isOk = false;
                errorMessage += "Ошибка: в сегменте неправильный текст: " + segmentText + " вместо " + revisionText + "\n";
            }

            // Проверить, что добавилась новая ревизия
            if (revisionCount != (translationNumber + 1))
            {
                isOk = false;
                errorMessage += "Ошибка: количество ревизий не увеличилось\n";
            }

            // Проверить, что тип новой ревизии - Rollback
            if (revisionType != REVISION_TYPE_ROLLBACK)
            {
                isOk = false;
                errorMessage += "Ошибка: тип ревизии неправильный: " + revisionType;
            }

            // Вывести ошибки
            Assert.IsTrue(isOk, errorMessage);
        }

        /// <summary>
        /// ТЕСТ: отмена Rollback
        /// </summary>
        [Test]
        public void CancelRollback()
        {
            InitProject();

            // Добавить текст в Target
            string text = "Text" + DateTime.Now.Ticks;
            int segmentRowNumber = 1;
            AddTextTarget(segmentRowNumber, text);
            ClickConfirmBtn(segmentRowNumber);
            // Вернуться в сегмент
            ClickSegmentTarget(segmentRowNumber);

            int revisionCountBefore = GetRevisionListCount();
            // Выделить ревизию
            Assert.IsTrue(SelectRevision(1), "Ошибка: такой ревизии нет");
            Assert.IsFalse(GetRollbackBtnDisabled(), "Ошибка: кнопка Rollback заблокирована");

            // Кликнуть Rollback
            ClickRollbackBtn();

            // Дождаться открытия диалога подтверждения отката
            Assert.IsTrue(WaitUntilDisplayElement(".//div[@id='rollback']"), "Ошибка: не появился диалог подтверждения отката");

            // Нажать нет
            Driver.FindElement(By.XPath(".//div[@id='rollback']//a[contains(@class,'x-btn-gray')]")).Click();
            WaitUntilDisappearElement(".//div[@id='rollback']");

            // Проверить, что количество ревизий не увеличилось
            Assert.AreEqual(revisionCountBefore, GetRevisionListCount(), "Ошибка: количество ревизий изменилось");
            // Проверить, что тип последней ревизии - не Rollback
            Assert.AreNotEqual(REVISION_TYPE_ROLLBACK, GetRevisionType(1), "Ошибка: появилась ревизия Rollback");
        }

        /// <summary>
        /// ТЕСТ: подтвердить вставленный из МТ перевод (должно быть две ревизии)
        /// </summary>
        [Test]
        public void ConfirmInsertedMTTest()
        {
            InitProject(false, true);

            // Проверить, что есть переводы MT
            Assert.IsTrue(GetCATTranslationListCount() > 0, "Ошибка: нет переводов в МТ");
            int CATTranslationNum = GetCATRowNumByType("MT");
            Assert.IsTrue(CATTranslationNum > 0, "Ошибка: перевод не МТ");

            int segmentRowNumber = 1;
            // Вставить из МТ
            GetSegmentTargetElement(segmentRowNumber).SendKeys(OpenQA.Selenium.Keys.Control + CATTranslationNum.ToString());
            // Подтвердить
            ClickConfirmBtn(segmentRowNumber);
            // Вернуться
            ClickSegmentTarget(segmentRowNumber);

            // Проверить, что обе ревизии сохранились
            Assert.IsTrue(GetRevisionListCount() == 2, "Ошибка: должно сохраниться две ревизии");

            string revisionTypeConfirmed = GetRevisionType(1);
            Assert.AreEqual(REVISION_TYPE_CONFIRMED, revisionTypeConfirmed,
                "Ошибка: неправильный тип ревизии: вместо " + REVISION_TYPE_CONFIRMED + ": " + revisionTypeConfirmed);

            string revisionTypeInsert = GetRevisionType(2);
            // Проверить тип
            Assert.AreEqual(REVISION_TYPE_INSERT_MT, revisionTypeInsert,
                "Ошибка: неправильный тип ревизии: вместо " + REVISION_TYPE_INSERT_MT + ": " + revisionTypeInsert);
        }

        /// <summary>
        /// ТЕСТ: удаление части подвержденного текста, проверка в ревизии пометки об удалении
        /// </summary>
        [Test]
        public void RemovePartTextTest()
        {
            InitProject();

            // Добавить текст в Target
            string text = "Text" + DateTime.Now.Ticks;
            Console.WriteLine("text: " + text);
            int segmentRowNumber = 1;
            AddTextTarget(segmentRowNumber, text);
            ClickConfirmBtn(segmentRowNumber);
            // Вернуться в сегмент
            ClickSegmentTarget(segmentRowNumber);

            // Ввести новый текст (старый, удалив часть текста)
            string textToRemove = text.Substring(2, 5);
            Console.WriteLine("textToRemove: " + textToRemove);
            text = text.Replace(textToRemove, "");
            Console.WriteLine("text: " + text);
            AddTextTarget(segmentRowNumber, text);
            ClickConfirmBtn(segmentRowNumber);
            // Вернуться в сегмент
            ClickSegmentTarget(segmentRowNumber);

            // Проверить, что ревизии две
            Assert.AreEqual(2, GetRevisionListCount(), "Ошибка: должно быть две ревизии");

            // Проверить, что в последней ревизии выделен удаленный текст
            Assert.IsTrue(IsElementPresent(By.XPath(
                ".//div[@id='revisions-body']//table//tbody//tr[1]//td[contains(@class,'revision-text-cell')]//del")),
                "Ошибка: в ревизии нет пометки об удаленном тексте");
        }

        /// <summary>
        /// ТЕСТ: добавление текста в подтвержденный, проверка в ревизии пометки о добавлении
        /// </summary>
        [Test]
        public void AddPartTextTest()
        {
            InitProject();

            // Добавить текст в Target
            string text = "Text" + DateTime.Now.Ticks;
            Console.WriteLine("text: " + text);
            int segmentRowNumber = 1;
            AddTextTarget(segmentRowNumber, text);
            ClickConfirmBtn(segmentRowNumber);
            // Вернуться в сегмент
            ClickSegmentTarget(segmentRowNumber);

            // Ввести новый текст (старый, добавив часть нового текста)
            string textToAdd = "newText";
            Console.WriteLine("textToAdd: " + textToAdd);
            text = text.Insert(2, textToAdd);
            Console.WriteLine("text: " + text);
            AddTextTarget(segmentRowNumber, text);
            ClickConfirmBtn(segmentRowNumber);
            // Вернуться в сегмент
            ClickSegmentTarget(segmentRowNumber);

            // Проверить, что ревизии две
            Assert.AreEqual(2, GetRevisionListCount(), "Ошибка: должно быть две ревизии");

            // Проверить, что в последней ревизии выделен удаленный текст
            Assert.IsTrue(IsElementPresent(By.XPath(
                ".//div[@id='revisions-body']//table//tbody//tr[1]//td[contains(@class,'revision-text-cell')]//ins")),
                "Ошибка: в ревизии нет пометки об удаленном тексте");
        }

        /// <summary>
        /// Получить элемент Target сегмента
        /// </summary>
        /// <param name="segmentRowNumber">номер строки/сегмента</param>
        /// <returns>элемент IWebDriver</returns>
        protected IWebElement GetSegmentTargetElement(int segmentRowNumber)
        {
            return Driver.FindElement(By.XPath(
                ".//div[@id='segments-body']//table//tbody//tr["
                + segmentRowNumber
                + "]//td[contains(@class,'target-cell')]/div"));
        }

        /// <summary>
        /// Добавить текст в Target
        /// </summary>
        /// <param name="rowNum">номер строки для добавления</param>
        /// <param name="text">текст</param>
        protected void AddTextTarget(int rowNum, string text)
        {
            // Получить Target
            IWebElement targetEl = GetSegmentTargetElement(rowNum);
            targetEl.Click();
            // Очистить
            targetEl.Clear();
            // Ввести в него текст
            targetEl.SendKeys(text);
        }

        /// <summary>
        /// Кликнуть по Target сегмента
        /// </summary>
        /// <param name="segmentRowNumber">номер сегмента</param>
        protected void ClickSegmentTarget(int segmentRowNumber)
        {
            // Кликнуть по Target
            GetSegmentTargetElement(segmentRowNumber).Click();
        }

        /// <summary>
        /// Кликнуть для автосохранения
        /// </summary>
        protected void ClickAutoSaveBtn()
        {
            Console.WriteLine("button");
            Driver.FindElement(By.Id("save-btn"));
            WaitUntilDisplayElement(".//a[@id='save-btn' and contains(@class,'x-btn-disabled')]");
        }

        /// <summary>
        /// Нажать хоткей автосохранения
        /// </summary>
        protected void ClickAutoSaveHotkey(int segmentRowNum)
        {
            Console.WriteLine("hotkey");
            GetSegmentTargetElement(segmentRowNum).SendKeys(OpenQA.Selenium.Keys.Control + "S");
            WaitUntilDisplayElement(".//a[@id='save-btn' and contains(@class,'x-btn-disabled')]");
        }

        /// <summary>
        /// Кликнуть Confirm, дождаться подтверждвения
        /// </summary>
        /// <param name="segmentRowWaitConfirm">номер сегмента для ожидания подтверждения</param>
        protected void ClickConfirmBtn(int segmentRowWaitConfirm)
        {
            Driver.FindElement(By.Id("confirm-btn")).Click();
            // Дождаться подтверждения
            Assert.IsTrue(WaitSegmentConfirm(segmentRowWaitConfirm), "Ошибка: Confirm не прошел");
        }

        /// <summary>
        /// Нажать хоткей Confirm
        /// </summary>
        protected void ClickConfirmHotkey(int segmentRowNum)
        {
            Console.WriteLine("hotkey");
            GetSegmentTargetElement(segmentRowNum).SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Return);
        }

        /// <summary>
        /// Кликнуть по Time для изменения сортировки
        /// </summary>
        protected void ClickTimeToSortRevisions()
        {
            // TODO мб span
            Driver.FindElement(By.XPath(".//div[contains(@class,'revision-date-column')]//span")).Click();
        }

        /// <summary>
        /// Получить: открыта ли вкладка Revisions
        /// </summary>
        /// <returns>открыта</returns>
        protected bool GetIsRevisionsTabAvailable()
        {
            return Driver.FindElement(By.Id("revisions-body")).Displayed;
        }

        /// <summary>
        /// Получить количество строк в таблице ревизий
        /// </summary>
        /// <returns>количество строк</returns>
        protected int GetRevisionListCount()
        {
            return Driver.FindElements(By.XPath(".//div[@id='revisions-body']//table//tbody//tr")).Count;
        }

        /// <summary>
        /// Выделить ревизию
        /// </summary>
        /// <param name="revisionNumber">номер строки</param>
        /// <returns>ревизия существует</returns>
        protected bool SelectRevision(int revisionNumber)
        {
            bool isRevisionExist = revisionNumber <= GetRevisionListCount();
            // Если такая ревизия есть
            if (isRevisionExist)
            {
                Driver.FindElement(By.XPath(
                    ".//div[@id='revisions-body']//table//tbody//tr[" + revisionNumber + "]//td")).Click();
            }

            return isRevisionExist;
        }

        /// <summary>
        /// Получить тип ревизии
        /// </summary>
        /// <param name="rowNumber">номер строки с ревизией</param>
        /// <returns>тип ревизии</returns>
        protected string GetRevisionType(int rowNumber)
        {
            return Driver.FindElement(By.XPath(
                ".//div[@id='revisions-body']//table//tbody//tr["
                + rowNumber
                + "]//td[contains(@class,'revision-type-cell')]/div")).Text;
        }

        /// <summary>
        /// Получить: заблокирована ли кнопка Rollback
        /// </summary>
        /// <returns>заблокирована</returns>
        protected bool GetRollbackBtnDisabled()
        {
            return Driver.FindElement(By.Id("revision-tollback-btn")).GetAttribute("class").Contains("x-btn-disabled");
        }

        /// <summary>
        /// Кликнуть по кнопке Rollback
        /// </summary>
        protected void ClickRollbackBtn()
        {
            Driver.FindElement(By.Id("revision-tollback-btn")).Click();
        }

        /// <summary>
        /// Получить текст ревизии
        /// </summary>
        /// <param name="revisionRowNumber">номер строки ревизии</param>
        /// <returns>текст</returns>
        protected string GetRevisionText(int revisionRowNumber)
        {
            return Driver.FindElement(By.XPath(".//div[@id='revisions-body']//table//tbody//tr["
                + revisionRowNumber
                + "]//td[contains(@class,'revision-text-cell')]")).Text;
        }

        /// <summary>
        /// Получить время строки ревизии
        /// </summary>
        /// <param name="revisionRowNumber">номер строки ревизии</param>
        /// <returns>время</returns>
        protected string GetRevisionTime(int revisionRowNumber)
        {
            return Driver.FindElement(By.XPath(".//div[@id='revisions-body']//table//tbody//tr["
                + revisionRowNumber
                + "]//td[contains(@class,'revision-date-cell')]")).Text;
        }

        /// <summary>
        /// Получить количество сегментов
        /// </summary>
        /// <returns>количество сегментов</returns>
        protected int GetSegmentsCount()
        {
            return Driver.FindElements(By.XPath(".//div[@id='segments-body']//table//tbody//tr")).Count;
        }

        /// <summary>
        /// Дождаться, пока сегмент подтвердится
        /// </summary>
        /// <param name="segmentRowNumber">номер сегмента</param>
        /// <returns>сегмент подтвердился</returns>
        protected bool WaitSegmentConfirm(int segmentRowNumber)
        {
            return WaitUntilDisplayElement(
                ".//div[@id='segments-body']//table//tbody//tr["
                + segmentRowNumber
                + "]//td[contains(@class,'info-cell')]//span[contains(@class,'fa-check')]");
        }

        /// <summary>
        /// Открыть вкладку ревизии
        /// </summary>
        protected void OpenRevisionTab()
        {
            // Проверить, что вкладка Ревизии открыта
            if (!GetIsRevisionsTabAvailable())
            {
                // Открыть вкладку Ревизии
                Driver.FindElement(By.Id("revisions-tab")).Click();
                Wait.Until((d) => d.FindElement(By.Id("segments-body")).Displayed);
            }
        }

        /// <summary>
        /// Получить количество переводов панели CAT
        /// </summary>
        /// <returns>количество переводов</returns>
        protected int GetCATTranslationListCount()
        {
            // Проверить, что панель CAT не доступна
            Assert.IsTrue(IsElementDisplayed(By.Id("cat-body")), "Ошибка: CAT-панель не видна");
            // Получить количество строк
            return Driver.FindElements(By.XPath(".//div[@id='cat-body']//table//tbody//tr")).Count;
        }

        /// <summary>
        /// Получить элемент перевода из панели CAT
        /// </summary>
        /// <param name="translationNumber">номер строки</param>
        /// <returns>элемент</returns>
        protected IWebElement GetCATTranslationElement(int translationNumber)
        {
            return Driver.FindElement(By.XPath(".//div[@id='cat-body']//table//tbody//tr[" + translationNumber + "]//td[1]"));
        }

        /// <summary>
        /// Получить номер строки перевода нужного типа (ТМ или МТ)
        /// </summary>
        /// <param name="type">тип (МТ или ТМ)</param>
        /// <returns>номер строки (если 0 - не найден тип)</returns>
        protected int GetCATRowNumByType(string type)
        {
            Thread.Sleep(2000);
            int rowNum = 0;
            // Элементы с типами всех переводов из САТ-панели
            IList<IWebElement> CATTranslations = Driver.FindElements(By.XPath(".//div[@id='cat-body']//table//tbody//tr//td[3]/div"));
            for (int i = 0; i < CATTranslations.Count; ++i)
            {
                if (CATTranslations[i].Text.Contains(type))
                {
                    rowNum = i + 1;
                    break;
                }
            }
            return rowNum;
        }

        /// <summary>
        /// Добавить несколько переводов в сегмент с подтверждением каждого
        /// </summary>
        /// <param name="segmentRowNumber">номер сегмента</param>
        /// <param name="translationNumber">количество переводов</param>
        /// <returns>список внесенных переводов</returns>
        protected List<string> AddTranslationsToSegment(int segmentRowNumber, int translationNumber)
        {
            List<string> textList = new List<string>();
            // Добавить текст в Target
            for (int i = 0; i < translationNumber; ++i)
            {
                string text = "Text" + DateTime.Now.Ticks;
                // Добавить текст
                AddTextTarget(segmentRowNumber, (text));
                textList.Add(text);
                // Подтвердить
                ClickConfirmBtn(segmentRowNumber);
            }
            // Вернуться
            ClickSegmentTarget(segmentRowNumber);

            return textList;
        }

        protected void InitProject(bool withTM = false, bool withMT = false)
        {
            // 1. Авторизация
            Authorization();

            // Создание проекта
            CreateProject(ProjectName, false, "", EditorTMXFile, withMT);
            
            //открытие настроек проекта            
            ImportDocumentProjectSettings(EditorTXTFile, ProjectName);
            //ImportDocumentProjectSettings(DocumentFileToConfirm, ProjectName);
            // 3. Назначение задачи на пользователя
            AssignTask();

            // 4. Открытие документа по имени созданного проекта
            OpenDocument();
        }
    }
}