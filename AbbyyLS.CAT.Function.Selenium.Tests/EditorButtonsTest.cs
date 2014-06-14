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

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class EditorButtonsTest : BaseTest
    {
        public EditorButtonsTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

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

            // 2. Создание проекта с 1 документом внутри
            CreateProject(ProjectName);
            //открытие настроек проекта
            ImportDocumentProjectSettings(currentDocument, ProjectName);
            // 3. Назначение задачи на пользователя
            AssignTask();

            // 4. Открытие документа
            OpenDocument();
        }


        /// <summary>
        /// Метод тестирования кнопки "Back" в редакторе
        /// </summary>
        [Test]
        public void BackButtonTest()
        {
            // Кнопка Back, проверка перехода
            EditorClickBackBtn();
        }

        /// <summary>
        /// Метод тестирования кнопки перемещения курсора между полями source и target без хоткея
        /// </summary>
        [Test]
        public void SourceTargetSwitchButtonTest()
        {
            int segmentNumber = 1;
            // Перешли из Target в Source по кнопке
            SourceTargetSwitchButton(segmentNumber);

            // Проверить где находится курсор, и если в поле source, то все ок
            Assert.True(EditorPage.GetIsCursorInSourceCell(segmentNumber),
                "Ошибка: после кнопки Toggle не перешли в Target");
        }

        /// <summary>
        /// Метод тестирования хоткея перемещения курсора между полями source и target
        /// </summary>
        [Test]
        public void SourceTargetSwitchHotkeyTest()
        {
            int segmentNumber = 1;
            // Перешли из Target в Source по хоткею
            SourceTargetSwitchHotkey(segmentNumber);

            // Проверить где находится курсор, и если в поле source, то все ок
            Assert.True(EditorPage.GetIsCursorInSourceCell(segmentNumber),
                "Ошибка: после хоткея Toggle не перешли в Target");
        }

        /// <summary>
        /// Метод тестирования кнопки подтвеждения сегмента
        /// </summary>
        [Test]
        public void ConfirmButtonTest()
        {
            // Добавить текст, подтвердить, проверка подтверждвения
            AddTranslationAndConfirm();
        }


        /// <summary>
        /// Метод тестирования кнопки копирования оригинала в перевод
        /// </summary>
        [Test]
        public void ToTargetButtonTest()
        {
            // Кнопка Copy, проверить содержимое Target
            ToTargetButton();
        }

        /// <summary>
        /// Метод тестирования хоткея копирования оригинала в перевод
        /// </summary>
        [Test]
        public void ToTargetHotkeyTest()
        {
            // Хоткей Copy, проверить содержимое Target
            ToTargetHotkey();
        }

        /// <summary>
        /// Метод тестирования кнопки отмены действия
        /// </summary>
        [Test]
        public void CancelButtonTest()
        {
            int segmentNumber = 1;
            // Нажать кнопку копирования
            ToTargetButton(segmentNumber);

            // Нажать кнопку отмены
            EditorPage.ClickUndoBtn();
            // Убедиться, что в target нет текста
            string targetxt = EditorPage.GetTargetText(segmentNumber);
            Assert.AreEqual("", targetxt, "Ошибка: после Undo в Target есть текст");
        }

        /// <summary>
        /// Метод тестирования хоткея отмены действия
        /// </summary>
        [Test]
        public void CancelHotkeyTest()
        {
            int segmentNumber = 1;
            // Нажать кнопку копирования
            ToTargetButton(segmentNumber);

            // Нажать хоткей отмены
            EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "Z");
            // Убедиться, что в target нет текста
            string targetxt = EditorPage.GetTargetText(segmentNumber);
            Assert.AreEqual("", targetxt, "Ошибка: после Undo в Target есть текст");
        }

        /// <summary>
        /// Метод тестирования кнопки возврата отмененного действия
        /// </summary>
        [Test]
        public void RedoAfterCancelButtonTest()
        {
            int segmentNumber = 1;
            // Нажать кнопку копирования
            ToTargetButton(segmentNumber);

            // Нажать кнопку отмены
            EditorPage.ClickUndoBtn();
            // Убедиться, что в target нет текста
            string targetxt = EditorPage.GetTargetText(segmentNumber);
            Assert.AreEqual("", targetxt, "Ошибка: после Undo target не пуст");

            // Нажать кнопку возврата отмененного действия
            EditorPage.ClickRedoBtn();
            // Убедиться, что в target и source одинаковы
            string sourcetxt = EditorPage.GetSourceText(segmentNumber);
            targetxt = EditorPage.GetTargetText(segmentNumber);
            Assert.AreEqual(sourcetxt, targetxt, "Ошибка: после Redo тексты Source и Target не совпадают");
        }

        /// <summary>
        /// Метод тестирования хоткея возврата отмененного действия
        /// </summary>
        [Test]
        public void RedoAfterCancelHotkeyTest()
        {
            int segmentNumber = 1;
            // Нажать кнопку копирования
            ToTargetButton(segmentNumber);

            // Нажать кнопку отмены
            EditorPage.ClickUndoBtn();
            // Убедиться, что в target нет текста
            string targetxt = EditorPage.GetTargetText(segmentNumber);
            Assert.AreEqual("", targetxt, "Ошибка: после Undo target не пуст");

            // Нажать хоткей возврата отмененного действия
            EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "Y");

            // Убедиться, что в target и source одинаковы
            string sourcetxt = EditorPage.GetSourceText(segmentNumber);
            targetxt = EditorPage.GetTargetText(segmentNumber);
            Assert.AreEqual(sourcetxt, targetxt, "Ошибка: после Redo тексты Source и Target не совпадают");
        }

        /// <summary>
        /// Метод тестирования кнопки изменения регистра для всего текста
        /// </summary>
        [Test]
        public void ChangeCaseTextButtonTest()
        {
            int segmentNumber = 1;
            // Написать текст в первом сегменте в target
            EditorPage.AddTextTarget(segmentNumber, "the example sentence");
            // Нажать хоткей выделения всего содержимого ячейки
            EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Запустить проверку
            CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", true, segmentNumber);
        }

        /// <summary>
        /// Метод тестирования хоткея изменения регистра для всего текста
        /// </summary>
        [Test]
        public void ChangeCaseTextHotkeyTest()
        {
            int segmentNumber = 1;
            // Написать текст в первом сегменте в target
            EditorPage.AddTextTarget(segmentNumber, "the example sentence");
            // Нажать хоткей выделения всего содержимого ячейки
            EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Запустить проверку
            CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", false, segmentNumber);
        }

        /// <summary>
        /// Метод тестирования кнопки изменения регистра для слова (не первого)
        /// </summary>
        [Test]
        public void ChangeCaseSomeWordButtonTest()
        {
            int segmentNumber = 1;
            // Написать текст в первом сегменте в target
            EditorPage.AddTextTarget(segmentNumber, "some words for example");
            // Нажать хоткей выделения последнего слова
            EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);
            // Запустить проверку
            CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", true, segmentNumber);
        }

        /// <summary>
        /// Метод тестирования хоткея изменения регистра для слова (не первого)
        /// </summary>
        [Test]
        public void ChangeCaseSomeWordHotkeyTest()
        {
            int segmentNumber = 1;
            // Написать текст в первом сегменте в target
            EditorPage.AddTextTarget(segmentNumber, "some words for example");
            // Нажать хоткей выделения последнего слова
            EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);
            // Запустить проверку
            CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", false, segmentNumber);
        }

        /// <summary>
        /// Метод тестирования кнопки и хоткея изменения регистра для первого слова
        /// </summary>
        [Test]
        public void ChangeCaseFirstWordTest()
        {
            int segmentNumber = 1;
            // Написать текст в первом сегменте в target
            EditorPage.AddTextTarget(segmentNumber, "some words for example");
            // Нажать хоткей перехода в начало строки
            EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Нажать хоткей выделения первого слова
            EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
            // Запустить проверку по кнопке
            CheckChangeCase("some words for example", "Some words for example", "SOME words for example", true, segmentNumber);
            // Запустить проверку по хоткею
            CheckChangeCase("some words for example", "Some words for example", "SOME words for example", false, segmentNumber);
        }

        /// <summary>
        /// Проверить изменение регистра
        /// </summary>
        /// <param name="sourceText">начальный текст</param>
        /// <param name="textAfterFirstChange">текст после первого изменения</param>
        /// <param name="textAfterSecondChange">текст после второго изменения</param>
        /// <param name="byButtonTrueByHotkeyFalse">по кнопке или по хоткею</param>
        protected void CheckChangeCase(string sourceText, string textAfterFirstChange, string textAfterSecondChange, bool byButtonTrueByHotkeyFalse, int segmentNumber)
        {
            // Список текстов для сравнения после изменения регистра
            List<string> listToCompare = new List<string>();
            listToCompare.Add(textAfterFirstChange);
            listToCompare.Add(textAfterSecondChange);
            listToCompare.Add(sourceText);

            for (int i = 0; i < listToCompare.Count; ++i)
            {
                // Нажать изменениe регистра
                ClickChangeCase(byButtonTrueByHotkeyFalse, segmentNumber);
                // Убедиться, что регистр слова изменился правильно - сравнить со значением в listToCompare
                Assert.AreEqual(listToCompare[i], EditorPage.GetTargetText(segmentNumber), "Ошибка: не совпал текст при изменении регистра");
            }
        }

        /// <summary>
        /// Нажать Изменить регистр
        /// </summary>
        /// <param name="byButtonTrueByHotkeyFalse">true - по кнопке, false - по хоткею</param>
        /// <param name="rowNumber">номер сегмента</param>
        protected void ClickChangeCase(bool byButtonTrueByHotkeyFalse, int rowNumber)
        {
            if (byButtonTrueByHotkeyFalse)
            {
                // Нажать кнопку
                EditorPage.ClickChangeCaseBtn();
            }
            else
            {
                // Нажать хоткей
                EditorPage.SendKeysTarget(rowNumber, OpenQA.Selenium.Keys.Alt + OpenQA.Selenium.Keys.F3);
            }
        }

    }
}
