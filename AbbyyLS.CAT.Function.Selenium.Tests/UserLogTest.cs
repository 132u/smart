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
    /// <summary>
    /// Группа тестов для проверки лога
    /// </summary>
	public class UserLogTest : BaseTest
    {
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="url">Адрес</param>
		/// <param name="workspaceUrl">Адрес workspace</param>
		/// <param name="browserName">Название браузера</param>
		public UserLogTest(string url, string workspaceUrl, string browserName)
			: base(url, workspaceUrl, browserName)
		{

		}

		/// <summary>
		/// Предварительная подготовка группы тестов
		/// </summary>
		[SetUp]
        public void Setup()
        {
            projectName = "TestLogProject" + DateTime.Now.Ticks;

			// Не закрывать браузер
			quitDriverAfterTest = false;

			// Переходим к странице воркспейса
			GoToWorkspace();
        }

		/// <summary>
		/// Название проекта
		/// </summary>
        protected string projectName;

        /// <summary>
        /// Открытие-закрытие документа 
        /// </summary>
        [Test]
        public void OpenCloseDocument()
        {
            // Открыть документ
            CreateReadyProject(projectName, false, false, DocumentFileToConfirm);
            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования удаления текста с клавиатуры в документе 
        /// </summary>
        [Test]
        public void DeleteText()
        {
            // Открыть документ
            CreateReadyProject(projectName, false, false, DocumentFileToConfirm);

            int segmentNum = 1;
            // Написать текст в первом сегменте
            EditorPage.AddTextTarget(segmentNum, "Translation");

            //Удалить текст путем нажатия клавиши Backspace
            while (EditorPage.GetTargetText(segmentNum).Length > 0)
            {
                EditorPage.SendKeysTarget(segmentNum, OpenQA.Selenium.Keys.Backspace);
            }

			// Дождаться автосохранения
			AutoSave();

            //Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования подтверждения перевода с помощью нажатия кнопки на панели инструментов 
        /// </summary>
        [Test]
        public void ConfirmTextButton()
        {
            // Открыть документ
            CreateReadyProject(projectName, false, false, DocumentFileToConfirm);

            //Набрать текст в первом сегменте и нажать кнопку Confirm Segment
            AddTranslationAndConfirm();

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования перемещения курсора между сегментами
        /// </summary>
        [Test]
        public void ChooseSegment()
        {
            // Открыть документ
            CreateReadyProject(projectName, false, false, DocumentFileToConfirm);

            //Курсор в первом сегменте Source
            EditorPage.ClickSourceCell(1);
            //Курсор во втором сегменте Source
            EditorPage.ClickSourceCell(2);

            //Курсор во втором сегменте Target
            EditorPage.ClickTargetCell(2);
            //Курсор в четвертом сегменте Target
            EditorPage.ClickTargetCell(4);

            //Курсор в седьмом сегменте Source
            EditorPage.ClickSourceCell(7);

            //Курсор в последнем сегменте Target
            EditorPage.ClickTargetCell(EditorPage.GetSegmentsNumber());

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования копирования текста из source в target по нажатию кнопки в панели инструментов
        /// </summary>
        [Test]
        public void CopySourceSegmentButton()
        {
            // Открыть документ
            CreateReadyProject(projectName, false, false, DocumentFileToConfirm);
            
            //Копировать текст сегмента
            ToTargetButton();

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования копирования текста из source в target по хоткею
        /// </summary>
        [Test]
        public void CopySourceSegmentHotkey()
        {
            // Открыть документ
            CreateReadyProject(projectName, false, false, DocumentFileToConfirm);
            
            //Копировать текст сегмента
            ToTargetHotkey();

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования Undo/Redo
        /// </summary>
        [Test]
        public void UndoRedoActions()
        {
            // Открыть документ
            CreateReadyProject(projectName, false, false, DocumentFileToConfirm);

            // Копирование первого сегмента
            ToTargetButton(1);

            // Копирование второго сегмента
            ToTargetButton(2);

            // Кликнуть Undo
            EditorPage.ClickUndoBtn();
            string targetText = EditorPage.GetTargetText(2);
            Assert.IsTrue(targetText == "", "Ошибка: после undo текст в таргете не удалился\n" + targetText);

            // Кликнуть Redo
            EditorPage.ClickRedoBtn();
            Thread.Sleep(500);
            Assert.AreEqual(EditorPage.GetSourceText(2), EditorPage.GetTargetText(2), "Ошибка: после redo текст в target не восстановился");

            // Написать текст в третьем сегменте
            Console.WriteLine("Написать текст в третьем сегменте");
            string text3Segment = "Test for 3d segment";
            EditorPage.AddTextTarget(3, text3Segment);

            // Написать текст в 4 сегменте
            Console.WriteLine("Написать текст в 4 сегменте");
            EditorPage.AddTextTarget(4, "Test for 4th segment");
            
            // Нажать хоткей отмены
            Console.WriteLine("Нажать хоткей отмены");
            EditorPage.SendKeysTarget(4, OpenQA.Selenium.Keys.Control + "Z");
            // Проверить, что 4 сегмент пуст
            Assert.IsTrue(EditorPage.GetTargetText(4) == "", "Ошибка: после хоткея отмены текст в 4 сегменте не удалился");

            // Нажать хоткей отмены
            Console.WriteLine("Нажать хоткей отмены");
            EditorPage.SendKeysTarget(3, OpenQA.Selenium.Keys.Control + "Z");
            // Проверить, что 3 сегмент пуст
            Assert.IsTrue(EditorPage.GetTargetText(3) == "", "Ошибка: после хоткея отмены текст в 3 сегменте не удалился");

            // Нажать хоткей восстановления
            Console.WriteLine("Нажать хоткей восстановления");
            EditorPage.SendKeysTarget(3, OpenQA.Selenium.Keys.Control + "Y");
            EditorPage.ClickTargetCell(5);
            // Проверить, что 3 сегмент не пуст
            Assert.AreEqual(text3Segment, EditorPage.GetTargetText(3), "Ошибка: после хоткея возврата текст в 3 сегменте не появился");
            // TODO убрать sleep
            Thread.Sleep(5000);
            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);
            
            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования кнопки перемещения курсора между полями source и target
        /// </summary>
        [Test]
        public void SourceTargetSegmentsSwitchButton()
        {
            // Открыть документ
            CreateReadyProject(projectName, false, false, DocumentFileToConfirm);
            
            //Переключить курсор между полями source и target
            SourceTargetSwitchButton(1);

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования кнопки перемещения курсора между полями source и target по хоткею
        /// </summary>
        [Test]
        public void SourceTargetSegmentsSwitchHotkey()
        {
            // Открыть документ
            CreateReadyProject(projectName, false, false, DocumentFileToConfirm);
            
            //Переключить курсор между полями source и target
            SourceTargetSwitchHotkey(1);

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из MT по хоткею
        /// </summary>
        [Test]
        public void SubstituteTranslationMTHotkey()
        {
            // Открыть документ
            CreateReadyProject(projectName, true, true);

            //Выбираем первый сегмент
            EditorPage.ClickTargetCell(1);

            //Ждем пока загрузится CAT-панель
            Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: панель CAT пуста");            

            int MTNumber = EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.MT);
            Console.WriteLine("MTNumber: " + MTNumber);

            //Нажать хоткей для подстановки из MT перевода первого сегмента
            EditorPage.SendKeysTarget(1, OpenQA.Selenium.Keys.Control + MTNumber.ToString());

            // Текст в target
            string targetText = EditorPage.GetTargetText(1);
            // Текст в cat-панели
            string catText = EditorPage.GetCATPanelText(MTNumber);
            // Проверить, что они совпадают
            Assert.AreEqual(catText, targetText, "Ошибка: текст target и cat не совпадают");

            // Ожидание cохранения
            Thread.Sleep(1000);

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из MT по клику на сегмент в САТ-панели
        /// </summary>
        [Test]
        public void SubstituteTranslationMTDoubleClick()
        {
            // Открыть документ
            CreateReadyProject(projectName, true, true);

            //Выбираем первый сегмент
            EditorPage.ClickTargetCell(1);

            //Ждем пока загрузится CAT-панель
            Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: панель CAT пуста");

            int MTNumber = EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.MT);
            Console.WriteLine("MTNumber: " + MTNumber);
            // Двойной клик
            EditorPage.DoubleClickCATPanel(MTNumber);

            
            // Текст в target
            string targetText = EditorPage.GetTargetText(1);
            // Текст в cat-панели
            string catText = EditorPage.GetCATPanelText(MTNumber);
            // Проверить, что они совпадают
            Assert.AreEqual(catText, targetText, "Ошибка: текст target и cat не совпадают");

            // Ожидание cохранения
            Thread.Sleep(1000);

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из TM по клику на сегмент в САТ-панели
        /// </summary>
        [Test]
        public void SubstituteTranslationTMHotkey()
        {
            int segmentRow = 4;
            // Открыть документ
            CreateReadyProject(projectName, true);

            //Выбираем первый сегмент
            EditorPage.ClickTargetCell(segmentRow);

            //Ждем пока загрузится CAT-панель
            Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: панель CAT пуста");

            int TMNumber = EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.TM);
            Console.WriteLine("TMNumber: " + TMNumber);

            //Нажать хоткей для подстановки из TM перевода первого сегмента
            EditorPage.SendKeysTarget(segmentRow, OpenQA.Selenium.Keys.Control + TMNumber.ToString());

            // Текст в target
            string targetText = EditorPage.GetTargetText(segmentRow);
            // Текст в cat-панели
            string catText = EditorPage.GetCATPanelText(TMNumber);
            // Проверить, что они совпадают
            Assert.AreEqual(catText, targetText, "Ошибка: текст target и cat не совпадают");

            // Ожидание cохранения
            Thread.Sleep(1000);

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из TM по клику на сегмент в САТ-панели
        /// </summary>
        [Test]
        public void SubstituteTranslationTMDoubleClick()
        {
            int segmentRow = 4;
            // Открыть документ
            CreateReadyProject(projectName, true);

            //Выбираем первый сегмент
            EditorPage.ClickTargetCell(segmentRow);

            //Ждем пока загрузится CAT-панель
            Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: панель CAT пуста");

            int TMNumber = EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.TM);
            Console.WriteLine("TMNumber: " + TMNumber);
            // Двойной клик
            EditorPage.DoubleClickCATPanel(TMNumber);


            // Текст в target
            string targetText = EditorPage.GetTargetText(segmentRow);
            // Текст в cat-панели
            string catText = EditorPage.GetCATPanelText(TMNumber);
            // Проверить, что они совпадают
            Assert.AreEqual(catText, targetText, "Ошибка: текст target и cat не совпадают");

            // Ожидание cохранения
            Thread.Sleep(1000);

            // Нажать кнопку назад
            EditorClickHomeBtn();

			// Перейти к проекту
			OpenProjectPage(projectName);

            //Выгрузить логи
            ExportLog();
        }

        /// <summary>
        /// Метод тестирования выгрузки пустого лога
        /// </summary>
        [Test]
        public void EmptyLogTest()
        {
            // Создание проекта
            CreateProject(projectName, DocumentFile);
            // Открыть проект
            OpenProjectPage(projectName);
            // Дождаться пропадания колеса ожидания
            Assert.IsTrue(ProjectPage.WaitDocumentDownloadFinish(),
                "Ошибка: колесо ожидания долго не пропадает");
            // Выгрузить логи
            ExportLog();
        }



        /// <summary>
        /// Метод выгрузки логов 
        /// </summary>        
        public void ExportLog()
        {
            //Выбрать документ
            SelectDocumentInProject();

            //Нажать кнопку выгрузки логов
            ProjectPage.ClickDownloadLogs();

            // Экспортировать и проверить, что файл сохранен
            ExternalDialogSaveDocument(TestContext.CurrentContext.Test.Name, false, "", false, ".zip");
        }
    }
}
