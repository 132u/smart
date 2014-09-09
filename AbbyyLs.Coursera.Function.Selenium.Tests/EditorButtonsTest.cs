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
			Assert.IsTrue(OpenCoursePage(), "Ошибка: список курсов пустой.");
            // Переход в курс с наименьшим прогрессом
            string courseName = OpenCourseMinProgress();
            // Перейти в лекцию
            int lectureRowNumber = OpenLectureNotFilled();
            // Найти предложение без перевода
            editorRowNumber = GetEmptyTranslationRowNumber();
            // Кликнуть в ячейку Target
			EditorPage.ClickTargetByRowNumber(editorRowNumber);
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
            Assert.IsTrue(LecturePage.WaitUntilDisplayLecturesList(), "Ошибка: не вышли из редактора по кнопке Back");
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
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "the example sentence");

            // Нажать хоткей выделения всего содержимого ячейки
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);

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
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "the example sentence");

            // Нажать хоткей выделения всего содержимого ячейки
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);

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
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "some words for example");
			
            // Нажать хоткей выделения последнего слова
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);

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
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "some words for example");
			
            // Нажать хоткей выделения последнего слова
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);

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
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "some words for example");

            // Нажать хоткей перехода в начало строки
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);

            // Нажать хоткей выделения первого слова
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);

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
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "some words for example");

            // Нажать хоткей перехода в начало строки
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
			
            // Нажать хоткей выделения первого слова
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);

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
			EditorPage.ClickTagInsertBtn();

            // Проверка, что в ячейке появился символ переноса строки
            // Старая проверка: Assert.IsTrue(IsElementPresent(By.XPath(".//div[@id='segments-body']//table//tr[" + editorRowNumber + "]//td[4]/div/img[contains(@class,'tag')]")),
            Assert.IsTrue(EditorPage.GetIsTagPresent(editorRowNumber),
                "Ошибка: в ячейке Target не появился символ переноса строки");
        }

        /// <summary>
        /// Проверка работы в редакторе добавления символа переноса строки по хоткею
        /// </summary>
        [Test]
        public void AddLineBreakHotkey()
        {
            // Хоткей добавления символа переноса строки
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Control + "q");

            // Проверка, что в ячейке появился символ переноса строки
            // Старая проверка: Assert.IsTrue(IsElementPresent(By.XPath(".//div[@id='segments-body']//table//tr[" + editorRowNumber + "]//td[4]/div/img[contains(@class,'tag')]")),
			Assert.IsTrue(EditorPage.GetIsTagPresent(editorRowNumber),
                "Ошибка: в ячейке Target не появился символ переноса строки");
        }

        /// <summary>
        /// Проверка работы в редакторе кнопки Confirm
        /// </summary>
        [Test]
        public void ConfirmButtonTest()
        {
            // Написать текст в первом сегменте в target
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "some words for example");

            // Confirm - кнопка
			EditorPage.ClickConfirmBtn();

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
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "some words for example");

            // Confirm - клавиша Enter
			SendKeys.SendWait(@"{Enter}");
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Enter);

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
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "some words for example");

            // Confirm - хоткей Ctrl+Enter (Return - это Enter)
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Return);

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
			EditorPage.AddTextTargetByRowNumber(editorRowNumber, "some words for example");

            // Confirm - галочка в строке
			EditorPage.ClickSegmentConfirmByRowNumber(editorRowNumber);

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
				EditorPage.ClickToggleBtn();
            }
            else
            {
                // Нажать хоткей Tab
				EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Tab);
            }

            // Проверить, где находится курсор (должен в Source этой же строчки) - сравним содержимое
			IWebElement source_cell = EditorPage.GetSourceByRowNumber(editorRowNumber);
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
			string sourceText = EditorPage.GetSourceByRowNumber(editorRowNumber).Text;

            // ToTarget
            if (byButton)
            {
                // Нажать кнопку копирования
				EditorPage.ClickCopyBtn();
            }
            else
            {
                // Нажать хоткей Ctrl+Insert
				EditorPage.AddTextTargetByRowNumber(editorRowNumber,
					OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Insert);
            }

			EditorPage.ClickConfirmBtn();

			Thread.Sleep(1000);

            // Проверить, такой ли текст в target'те
			string targetText = EditorPage.GetTargetByRowNumber(editorRowNumber).Text;

            Assert.AreEqual(sourceText, targetText, "Ошибка: Текст target не совпадает с вводимым.");
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
				string targetxt = EditorPage.GetTargetByRowNumber(editorRowNumber).Text;

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
				EditorPage.ClickChangeCaseBtn();
            }
            else
            {
                // Нажать хоткей
				EditorPage.AddTextTargetByRowNumber(editorRowNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.F3);
            }
        }
    }
}
