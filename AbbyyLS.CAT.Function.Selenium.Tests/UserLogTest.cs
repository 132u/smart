using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;
using OpenQA.Selenium.Interactions;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class UserLogTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            // 1. Авторизация
            Authorization();

            string currentDocument = DocumentFile;
            // При проверке Confirm не работает наш обычный файл, приходится загружать другой
            if (TestContext.CurrentContext.Test.Name.Contains("Confirm"))
            {
                currentDocument = DocumentFileToConfirm;
            }

            // 2. Создание проекта с 1 документов внутри
            CreateProject(ProjectName, true, currentDocument);

            // 3. Назначение задачи на пользователя
            AssignTask();

            // 4. Открытие документа по имени созданного проекта
            OpenDocument();
        }

        public UserLogTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }


        /// <summary>
        /// Метод выгрузки логов 
        /// </summary>        
        public void ExportLog(string path)
        {
            //Получить путь к папке, куда выгружать логи
            string fullResultPath = System.IO.Path.Combine(path, ProjectName);

            //Выбрать документ
            Driver.FindElement(By.CssSelector(".project-documents div.x-grid-body table tr:nth-child(1) td:nth-child(1)")).Click();

            //Нажать кнопку выгрузки логов
            Driver.FindElement(By.Id("log-export-btn")).Click();

            // Заполнить форму для сохранения файла
            FillAddDocumentForm(fullResultPath);

            Assert.IsTrue(System.IO.File.Exists(fullResultPath + ".zip"));
        }

        /// <summary>
        /// Открытие-закрытие документа 
        /// </summary>
        [Test]
        public void OpenCloseDocument()
        {
            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "OpenCloseDocument");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования набора текста в документе 
        /// </summary>
        [Test]
        public void WriteText()
        {
            // Написать текст в первом сегменте
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
               SendKeys("This is a sample text");

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            Assert.AreEqual("This is a sample text", segmentxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "WriteText");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования удаления текста с клавиатуры в документе 
        /// </summary>
        [Test]
        public void DeleteText()
        {
            // Написать текст в первом сегменте
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
               SendKeys("This is a sample text");

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            Assert.AreEqual("This is a sample text", segmentxt);

            //Удалить текст путем нажатия клавиши Backspace
            while (!String.IsNullOrEmpty(segmentxt))
            {
                Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                   SendKeys(OpenQA.Selenium.Keys.Backspace);

                segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            }

            //Убедиться, что в сегменте нет текста
            Assert.AreEqual("", segmentxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "DeleteText");

            System.IO.Directory.CreateDirectory(resultPath);

            //Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подтверждения перевода с помощью нажатия кнопки на панели инструментов 
        /// </summary>
        [Test]
        public void ConfirmTextButton()
        {
            //Набрать текст в первом сегменте и нажать кнопку Confirm Segment
            ConfirmButton();

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "ConfirmTextButton");

            System.IO.Directory.CreateDirectory(resultPath);

            //Thread.Sleep(60000);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования перемещения курсора между сегментами
        /// </summary>
        [Test]
        public void ChooseSegment()
        {
            //Курсор в первом сегменте Source
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Click();

            //Курсор во втором сегменте Source
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(2) td:nth-child(2) div")).Click();

            //Курсор во втором сегменте Target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(2) td:nth-child(3) div")).Click();

            //Курсор в четвертом сегменте Target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(4) td:nth-child(3) div")).Click();

            //Курсор в седьмом сегменте Source
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(7) td:nth-child(2) div")).Click();

            //Курсор в последнем сегменте Target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:last-child td:nth-child(3) div")).Click();

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "ChooseSegment");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования копирования текста из source в target по нажатию кнопки в панели инструментов
        /// </summary>
        [Test]
        public void CopySourceSegmentButton()
        {
            //Копировать текст сегмента
            ToTargetButton();

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "CopySourceSegmentButton");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования копирования текста из source в target по хоткею
        /// </summary>
        [Test]
        public void CopySourceSegmentHotkey()
        {
            //Копировать текст сегмента
            ToTargetHotkey();

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "CopySourceSegmentHotkey");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования Undo/Redo
        /// </summary>
        [Test]
        public void UndoRedoActions()
        {
            // Выбрать source первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).Click();

            // Нажать кнопку копирования
            Driver.FindElement(By.Id("copy-btn")).Click();

            // Текст source'a первого сегмента
            string sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Text;
            // Проверить, такой ли текст в target'те
            string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);

            // Выбрать source второго сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(2) td:nth-child(2)")).Click();

            // Нажать кнопку копирования
            Driver.FindElement(By.Id("copy-btn")).Click();

            // Текст source'a второго сегмента
            sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(2) td:nth-child(2) div")).Text;

            // Проверить, такой ли текст в target'те
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(2) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);

            // Нажать кнопку отмены
            Driver.FindElement(By.Id("undo-btn")).Click();

            // Убедиться, что в target нет текста
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(2) td:nth-child(3) div")).Text;
            Assert.AreEqual("", targetxt);

            // Нажать кнопку возврата отмененного действия
            Driver.FindElement(By.Id("redo-btn")).Click();

            // Убедиться, что в target и source второго одинаковы
            sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(2) td:nth-child(2) div")).Text;
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(2) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);

            // Выбрать target третьего сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(3) td:nth-child(3)")).Click();

            // Написать текст в третьем сегменте
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(3) td:nth-child(3) div")).
               SendKeys("This is a sample text");

            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(3) td:nth-child(3) div")).Text;
            Assert.AreEqual("This is a sample text", targetxt);

            // Выбрать target четвертого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(4) td:nth-child(3)")).Click();

            // Написать текст в четвертом сегменте
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(4) td:nth-child(3) div")).
               SendKeys("This is a sample text");

            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(4) td:nth-child(3) div")).Text;
            Assert.AreEqual("This is a sample text", targetxt);

            // Нажать хоткей отмены
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(4) td:nth-child(3)")).
                SendKeys(OpenQA.Selenium.Keys.Control + "Z");
            // Убедиться, что в target четвертого сегмента нет текста
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(4) td:nth-child(3) div")).Text;
            Assert.AreEqual("", targetxt);

            // Нажать хоткей отмены
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(3) td:nth-child(3)")).
                SendKeys(OpenQA.Selenium.Keys.Control + "Z");
            // Убедиться, что в target третьего сегмента нет текста
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(3) td:nth-child(3) div")).Text;
            Assert.AreEqual("", targetxt);

            // Нажать хоткей возврата отмененного действия
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(3) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + "Y");

            //Убедиться, что текст равен исходному
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(3) td:nth-child(3) div")).Text;
            Assert.AreEqual("This is a sample text", targetxt);

            // Нажать хоткей возврата отмененного действия
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(4) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + "Y");

            //Убедиться, что текст равен исходному
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(4) td:nth-child(3) div")).Text;
            Assert.AreEqual("This is a sample text", targetxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "UndoRedoActions");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования spellcheck
        /// </summary>
        [Test]
        public void UseSpellcheck()
        {
            // Выбрать target первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3)")).Click();

            // Написать текст в первом сегменте с опечаткой
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
               SendKeys("plonet");


            string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual("plonet", targetxt);

            Actions action = new Actions(Driver);

            action.ContextClick(Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div span.spellcheck")));

            action.Perform();

            // Выбрать target первого сегмента
            Driver.FindElement(By.XPath("//div[@role='menu']//span[contains(string(),'planet')]")).Click();

            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual("planet", targetxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "UseSpellcheck");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования кнопки перемещения курсора между полями source и target
        /// </summary>
        [Test]
        public void SourceTargetSegmentsSwitchButton()
        {
            //Переключить курсор между полями source и target
            SourceTargetSwitchButton();

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SourceTargetSegmentsSwitchButton");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования кнопки перемещения курсора между полями source и target по хоткею
        /// </summary>
        [Test]
        public void SourceTargetSegmentsSwitchHotkey()
        {
            //Переключить курсор между полями source и target
            SourceTargetSwitchHotkey();

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SourceTargetSegmentsSwitchHotkey");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из MT по хоткею
        /// </summary>
        [Test]
        public void SubstituteTranslationMTHotkey()
        {
            //Выбираем первый сегмент
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Click();

            //Ждем пока загрузится CAT-панель
            Wait.Until(d => d.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")));

            string catNumber = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[1]")).Text;

            //Нажать хоткей для подстановки из MT перевода первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + catNumber);

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            string catxt = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")).Text;

            Assert.AreEqual(segmentxt, catxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SubstituteTranslationMTHotkey");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из MT по клику на сегмент в САТ-панели
        /// </summary>
        [Test]
        public void SubstituteTranslationMTDoubleClick()
        {
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Click();

            //Ждем пока загрузится CAT-панель
            Wait.Until(d => d.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")));

            //Нажать на сегмент в CAT-панели
            Actions action = new Actions(Driver);

            action.DoubleClick(Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")));

            action.Perform();

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            string catxt = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")).Text;

            Assert.AreEqual(segmentxt, catxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SubstituteTranslationMTDoubleClick");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из TM по клику на сегмент в САТ-панели
        /// </summary>
        [Test]
        public void SubstituteTranslationTMHotkey()
        {
            //Выбираем первый сегмент
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Click();

            //Ждем пока загрузится CAT-панель
            Wait.Until(d => d.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")));

            string catNumber = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[1]")).Text;

            //Нажать хоткей для подстановки из MT перевода первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + catNumber);

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            string catxt = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")).Text;

            Assert.AreEqual(segmentxt, catxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SubstituteTranslationTMHotkey");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из TM по клику на сегмент в САТ-панели
        /// </summary>
        [Test]
        public void SubstituteTranslationTMDoubleClick()
        {
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Click();

            //Ждем пока загрузится CAT-панель
            Wait.Until(d => d.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")));

            //Нажать на сегмент в CAT-панели
            Actions action = new Actions(Driver);

            action.DoubleClick(Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")));

            action.Perform();

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            string catxt = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")).Text;

            Assert.AreEqual(segmentxt, catxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SubstituteTranslationTMDoubleClick");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования создания новой TM на странице настройки проекта
        /// </summary>
        [Test]
        public void AddTMXFileTest()
        {
            Driver.Navigate().Back();

            Driver.Navigate().Back();

            AddTMXFile(ProjectName);
            Driver.FindElement(By.LinkText(ProjectName)).Click();
            OpenDocument();
        }


    }
}
