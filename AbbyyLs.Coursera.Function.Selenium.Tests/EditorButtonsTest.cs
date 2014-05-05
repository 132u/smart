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
    class EditorButtonsTest : BaseTest
    {
        public EditorButtonsTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        private int editorRowNumber;

        [SetUp]
        public void Setup()
        {
            // Перейти к списку доступных курсов
            OpenCoursePage();
            // Переход в курс с наименьшим прогрессом
            string courseName = OpenCourseMinProgress();
            // Перейти в лекцию
            int lectureRowNumber = OpenLectureNotFilled();
            // Найти предложение без перевода
            editorRowNumber = GetEmptyTranslationRowNumber();
            // Кликнуть в ячейку Target
            Driver.FindElement(By.CssSelector(
                "#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).Click();
        }

        /// <summary>
        /// Проверка работоспособности кнопки Back
        /// </summary>
        [Test]
        public void BackButtonTest()
        {
            // Нажать Back
            ClickBackEditor();

            // Проверить, что вернулись в список лекций
            Assert.IsTrue(IsElementPresent(By.XPath(".//tbody[contains(@data-bind,'lectures')]")), "Ошибка: не вышли из редактора по кнопке Back");
        }

        /// <summary>
        /// Проверка работы в редакторе функционала кнопки Toggle
        /// </summary>
        [Test]
        public void SourceTargetSwitchButton()
        {
            SourceTargetSwitchCheck(true);
        }

        /// <summary>
        /// Проверка работы в редакторе функционала хоткея Toggle
        /// </summary>
        [Test]
        public void SourceTargetSwitchHotkey()
        {
            SourceTargetSwitchCheck(false);
        }

        /// <summary>
        /// Проверка работы в редакторе функционала кнопки Copy To Target
        /// </summary>
        [Test]
        public void ToTargetButton()
        {
            ToTargetCheck(true);
        }

        /// <summary>
        /// Проверка работы в редакторе функционала хоткея Copy To Target
        /// </summary>
        [Test]
        public void ToTargetHotkey()
        {
            ToTargetCheck(false);
        }

        /// <summary>
        /// Проверка работы в редакторе изменения регистра предложения по кнопке
        /// </summary>
        [Test]
        public void ChangeCaseTextButton()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("the example sentence");
            // Нажать хоткей выделения всего содержимого ячейки
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Запустить проверку
            CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", true);
        }

        /// <summary>
        /// Проверка работы в редакторе изменения регистра предложения по хоткею
        /// </summary>
        [Test]
        public void ChangeCaseTextHotkey()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("the example sentence");
            // Нажать хоткей выделения всего содержимого ячейки
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Запустить проверку
            CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", false);
        }

        /// <summary>
        /// Проверка работы в редакторе изменения регистра последнего слова по кнопке
        /// </summary>
        [Test]
        public void ChangeCaseSomeWordButton()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("some words for example");
            // Нажать хоткей выделения последнего слова
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);
            // Запустить проверку
            CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", true);
        }

        /// <summary>
        /// Проверка работы в редакторе изменения регистра последнего слова по хоткею
        /// </summary>
        [Test]
        public void ChangeCaseSomeWordHotkey()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("some words for example");
            // Нажать хоткей выделения последнего слова
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);
            // Запустить проверку
            CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", false);
        }

        /// <summary>
        /// Проверка работы в редакторе изменения регистра первого слова по кнопке
        /// </summary>
        [Test]
        public void ChangeCaseFirstWordButton()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("some words for example");
            // Нажать хоткей перехода в начало строки
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Нажать хоткей выделения первого слова
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
            // Запустить проверку по кнопке
            CheckChangeCase("some words for example", "Some words for example", "SOME words for example", true);
        }

        /// <summary>
        /// Проверка работы в редакторе изменения регистра первого слова по хоткею
        /// </summary>
        [Test]
        public void ChangeCaseFirstWordHotkey()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("some words for example");
            // Нажать хоткей перехода в начало строки
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Нажать хоткей выделения первого слова
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
            // Запустить проверку по хоткею
            CheckChangeCase("some words for example", "Some words for example", "SOME words for example", false);
        }

        /// <summary>
        /// Проверка работы в редакторе добавления символа переноса строки по кнопке
        /// </summary>
        [Test]
        public void AddLineBreakButton()
        {
            // Кликнуть по кнопке добавления символа переноса строки
            Driver.FindElement(By.XPath(".//a[contains(@data-qtip,'Ctrl+Q')]")).Click();
            // Проверка, что в ячейке появился символ переноса строки
            // Старая проверка: Assert.IsTrue(IsElementPresent(By.XPath(".//div[@id='segments-body']//table//tr[" + editorRowNumber + "]//td[4]/div/img[contains(@class,'tag')]")),
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[@id='segments-body']//table//tr[" + editorRowNumber + "]//td[4]/div/span[contains(@class,'tag')]")),
                "Ошибка: в ячейке Target не появился символ переноса строки");
        }

        /// <summary>
        /// Проверка работы в редакторе добавления символа переноса строки по хоткею
        /// </summary>
        [Test]
        public void AddLineBreakHotkey()
        {
            // Хоткей добавления символа переноса строки
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                    SendKeys(OpenQA.Selenium.Keys.Control + "Q");
            // Проверка, что в ячейке появился символ переноса строки
            // Старая проверка: Assert.IsTrue(IsElementPresent(By.XPath(".//div[@id='segments-body']//table//tr[" + editorRowNumber + "]//td[4]/div/img[contains(@class,'tag')]")),
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[@id='segments-body']//table//tr[" + editorRowNumber + "]//td[4]/div/span[contains(@class,'tag')]")),
                "Ошибка: в ячейке Target не появился символ переноса строки");
        }

        /// <summary>
        /// Проверка работы в редакторе кнопки Confirm
        /// </summary>
        [Test]
        public void ConfirmButtonTest()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("some words for example");

            // Confirm - кнопка
            Driver.FindElement(By.Id("confirm-btn")).Click();

            // Проверить, что Confirm прошел (Assert внутри)
            AssertConfirmIsDone(editorRowNumber);
        }

        /// <summary>
        /// Проверка работы в редакторе Confirm по клавише Enter
        /// </summary>
        [Test]
        public void ConfirmByEnterTest()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("some words for example");

            // Confirm - клавиша Enter
            SendKeys.SendWait(@"{Enter}");

            // Проверить, что Confirm прошел (Assert внутри)
            AssertConfirmIsDone(editorRowNumber);
        }

        /// <summary>
        /// Проверка работы в редакторе Confirm по хоткею Ctrl+Enter
        /// </summary>
        [Test]
        public void ConfirmHotkeyCtrlEnterTest()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("some words for example");

            // Confirm - хоткей Ctrl+Enter (Return - это Enter)
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Return);

            // Проверить, что Confirm прошел (Assert внутри)
            AssertConfirmIsDone(editorRowNumber);
        }

        /// <summary>
        /// Проверка работы в редакторе Confirm по галочке в строке
        /// </summary>
        [Test]
        public void ConfirmByTickButtonTest()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                SendKeys("some words for example");

            // Confirm - галочка в строке
            Driver.FindElement(By.XPath(".//div[@id='segments-body']//table//tr[" + editorRowNumber + "]//td[5]//span[contains(@class,'fa-check')]")).Click();

            // Проверить, что Confirm прошел (Assert внутри)
            AssertConfirmIsDone(editorRowNumber);
        }

        /// <summary>
        /// Проверка работы в редакторе функционала Toggle
        /// </summary>
        /// <param name="byButton">проверка по кнопке или по хоткею (true - кнопка, false - хоткей) </param>
        protected void SourceTargetSwitchCheck(bool byButton)
        {
            // Toggle
            if (byButton)
            {
                // Кнопка Toggle
                Driver.FindElement(By.Id("toggle-btn")).Click();
            }
            else
            {
                // Нажать хоткей Tab
                Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                    SendKeys(OpenQA.Selenium.Keys.Tab);
            }

            // Проверить, где находится курсор (должен в Source этой же строчки) - сравним содержимое
            IWebElement source_cell = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(3) div"));
            IWebElement active_cell = Driver.SwitchTo().ActiveElement();

            Point source_loc = source_cell.Location;
            Point active_loc = active_cell.Location;

            Size source_size = source_cell.Size;
            Size active_size = active_cell.Size;

            string source_text = source_cell.Text;
            string active_text = active_cell.Text;

            Assert.True((source_loc == active_loc) && (source_size == active_size) && (source_text == active_text), "Ошибка: курсор не находится в Source");
        }

        /// <summary>
        /// Проверка работы в редакторе функционала Copy To Target
        /// </summary>
        /// <param name="byButton">по кнопке или по хоткею (true - по кнопке, false - по хоткею)</param>
        protected void ToTargetCheck(bool byButton)
        {
            // Выбрать source первого сегмента
            string sourceText = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(3) div")).Text;

            // ToTarget
            if (byButton)
            {
                // Нажать кнопку копирования
                Driver.FindElement(By.Id("copy-btn")).Click();
            }
            else
            {
                // Нажать хоткей Ctrl+Insert
                Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                    SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Insert);
            }

            // Проверить, такой ли текст в target'те
            string targetText = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).Text;
            Assert.AreEqual(sourceText, targetText);
        }

        /// <summary>
        /// Проверка изменения регистра
        /// </summary>
        /// <param name="sourceText">первоначальный текст</param>
        /// <param name="textAfterFirstChange">текст после первого изменения регистра</param>
        /// <param name="textAfterSecondChange">текст после второго изменения регистра</param>
        /// <param name="byButtonTrueByHotkeyFalse">по кнопке или по хоткею (true - по кнопке, false - по хоткею)</param>
        private void CheckChangeCase(string sourceText, string textAfterFirstChange, string textAfterSecondChange, bool byButtonTrueByHotkeyFalse)
        {
            // Список текстов для сравнения после изменения регистра
            List<string> listToCompare = new List<string>();
            listToCompare.Add(textAfterFirstChange);
            listToCompare.Add(textAfterSecondChange);
            listToCompare.Add(sourceText);

            for (int i = 0; i < listToCompare.Count; ++i)
            {
                // Нажать изменениe регистра
                ClickChangeCase(byButtonTrueByHotkeyFalse);
                // Убедиться, что регистр слова изменился правильно - сравнить со значением в listToCompare
                string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).Text;
                Assert.AreEqual(listToCompare[i], targetxt);
            }
        }

        /// <summary>
        /// Кликнуть изменение регистра
        /// </summary>
        /// <param name="byButtonTrueByHotkeyFalse">по кнопке или по хоткею (true - по кнопке, false - по хоткею)</param>
        private void ClickChangeCase(bool byButtonTrueByHotkeyFalse)
        {
            if (byButtonTrueByHotkeyFalse)
            {
                // Нажать кнопку
                Driver.FindElement(By.Id("change-case-btn")).Click();
            }
            else
            {
                // Нажать хоткей
                Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(" + editorRowNumber + ") td:nth-child(4) div")).
                    SendKeys(OpenQA.Selenium.Keys.Alt + OpenQA.Selenium.Keys.F3);
            }
        }
    }
}
