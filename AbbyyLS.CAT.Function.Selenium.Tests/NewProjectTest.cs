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

using OpenQA.Selenium.Interactions;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{

    /// <remarks>
    /// Методы для тестирования Проектов
    /// </remarks>

    public class NewProjectTest : BaseTest
    {

        private string ResultFilePath;



        public string ProjectNameCheck;
        public string DuplicateProjectName;

        public string _documentFileWrong;
        public string _ttxFile;
        public string _txtFile;
        public string _srtFile;

        public string _xliffTC10;

        private string _filesForImportCorrectPath = Path.GetFullPath(@"..\..\..\TestingFiles\FilesForImportCorrect");
        private string _filesForConfirmPath = Path.GetFullPath(@"..\..\..\TestingFiles\FilesForConfirm");
        private string _filesForImportErrorPath = Path.GetFullPath(@"..\..\..\TestingFiles\FilesForImportError");

        private static string[] filesForImportCorrect = Directory.GetFiles(Path.GetFullPath(@"..\..\..\TestingFiles\FilesForImportCorrect"));
        private static string[] filesForConfirm = Directory.GetFiles(Path.GetFullPath(@"..\..\..\TestingFiles\FilesForConfirm"));
        private static string[] filesForImportError = Directory.GetFiles(Path.GetFullPath(@"..\..\..\TestingFiles\FilesForImportError"));


        public NewProjectTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }


        /// <summary>
        /// Старт тестов, переменные
        /// </summary>
        [SetUp]
        public void SetupTest()
        {
            _documentFileWrong = Path.GetFullPath(@"..\..\..\TestingFiles\doc98.doc");
            _ttxFile = Path.GetFullPath(@"..\..\..\TestingFiles\test.ttx");
            _txtFile = Path.GetFullPath(@"..\..\..\TestingFiles\test.txt");
            _srtFile = Path.GetFullPath(@"..\..\..\TestingFiles\test.srt");

            _xliffTC10 = Path.GetFullPath(@"..\..\..\TestingFiles\Xliff\TC-10En.xliff");

            ResultFilePath = Path.GetFullPath(@"..\..\..\TestResults\Result");
        }

        /// <summary>
        /// Метод создания файла для записи результатов тестирования
        /// 
        public void CreateResultFile()
        {
            FileInfo fi = new FileInfo(ResultFilePath);
            StreamWriter sw = fi.CreateText();
            sw.WriteLine("Test Results");
            sw.Close();

        }

        /// <summary>
        /// Метод для записи результатов тестирования в файл и в консоль
        /// </summary>
        /// <param name="s">Строка, записываемая в файл</param>
        public void WriteStringIntoFile(string s)
        {
            StreamWriter sw = new StreamWriter(ResultFilePath, true);
            sw.WriteLine(s);
            sw.Close();
        }

        /// <summary>
        /// Вывод в консоль failed тестов красным цветом
        /// </summary>
        /// <param name="s">Строка, выводимая в консоль </param>
        public void FailConsoleWrite(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        /// <summary>
        /// Вывод в консоль успешно пройденных тестов зеленым цветом
        /// </summary>
        /// <param name="s">Строка, выводимая в консоль</param>
        public void PassConsoleWrite(string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        /// <summary>
        /// Вывод результатов в консоль и в файл 
        /// </summary>
        /// <param name="s">Строка, выводимая в консоль и добавляемая в файл</param>
        /// <param name="p">Параметр, отвечающий за цвет: 0 - fail(red), 1 - pass(green), 2 - black</param>
        public void WriteFileConsoleResults(string s, int p)
        {
            StreamWriter sw = new StreamWriter(ResultFilePath, true);

            switch (p)
            {
                //проваленные тесты
                case 0:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(s);
                    Console.ResetColor();
                    sw.WriteLine(s);
                    sw.WriteLine("\n");
                    sw.Close();
                    break;
                //успешно пройденные тесты
                case 1:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(s);
                    Console.ResetColor();
                    sw.WriteLine(s);
                    sw.WriteLine("\n");
                    sw.Close();
                    break;
                case 2:
                    Console.WriteLine(s);
                    sw.WriteLine(s);
                    sw.WriteLine("\n");
                    sw.Close();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// метод тестирования загрузки DOC формата (неподдерживаемый формат)
        /// </summary>
        [Test]
        public void ImportWrongFileTest()
        {
            Authorization();

            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard(ProjectName);

            //процесс добавления файла
            ImportDocumentCreateProject(_documentFileWrong);
            Assert.True(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-info-popup')]//span[contains(@class,'js-popup-close')]")),
                "Ошибка: не появилось сообщение о неверном формате загружаемого документа");
        }

        /// <summary>
        /// метод тестирования загрузки нескольких файлов при создании проекта (docx+ttx)
        /// </summary>
        [Test]
        public void ImportSomeFilesTest()// убран функционал
        {
            Authorization();
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard(ProjectName);

            // Загрузить документ
            ImportDocumentCreateProject(DocumentFile);
            // Загрузить второй документ
            ImportDocumentCreateProject(_ttxFile);

            Assert.IsFalse(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-info-popup')]//span[contains(@class,'js-popup-close')]")),
                "Ошибка: появилось сообщение о неверном формате загружаемого документа");
        }

        /// <summary>
        /// создание проекта без файла
        /// </summary>
        [Test]
        public void CreateProjectNoFile()
        {
            Authorization();
            // Создать проект
            CreateProject(ProjectName, false, "");
            //проверить что проект появился с списке проектов
            CheckProjectInList(ProjectName);
        }

        /// <summary>
        /// Импорт документа формата ttx (допустимый формат)
        /// </summary>
        [Test]
        public void ImportTtxFileTest()
        {
            Authorization();
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard(ProjectName);

            // Загрузить документ
            ImportDocumentCreateProject(_ttxFile);

            Assert.IsFalse(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-info-popup')]//span[contains(@class,'js-popup-close')]")),
                "Ошибка: появилось сообщение о неверном формате загружаемого документа");
        }

        /// <summary>
        /// Импорт документа формата txt (допустимый формат)
        /// </summary>
        [Test]
        public void ImportTxtFileTest()
        {
            Authorization();
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard(ProjectName);

            // Загрузить документ
            ImportDocumentCreateProject(_txtFile);

            Assert.IsFalse(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-info-popup')]//span[contains(@class,'js-popup-close')]")),
                "Ошибка: появилось сообщение о неверном формате загружаемого документа");
        }

        /// <summary>
        /// Импорт документа формата Srt (допустимый формат)
        /// </summary>
        [Test]
        public void ImportSrtFileTest()
        {
            Authorization();
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard(ProjectName);

            // Загрузить документ
            ImportDocumentCreateProject(_srtFile);

            Assert.IsFalse(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-info-popup')]//span[contains(@class,'js-popup-close')]")),
                "Ошибка: появилось сообщение о неверном формате загружаемого документа");
        }


        /// <summary>
        /// Импорт документа в созданный проект без файла
        /// </summary>
        [Test]
        public void ImportDocumentAfterCreationTest()
        {
            Authorization();
            // Создать проект
            CreateProject(ProjectName, false, "");
            // Проверить, что проект есть в списке
            CheckProjectInList(ProjectName);

            //открытие настроек проекта
            ImportDocumentProjectSettings(DocumentFile, ProjectName);

            // TODO: Вставить проверку что документ загружен!!! (как проверить это)
            Assert.IsTrue(Driver.PageSource.Contains("docx"), "Fail - Не найден импортируемый документ docx после создания проекта");
        }

        

        protected void ImportDocumentIntoProject(string filePath)
        {
            // Кликнуть по Импорт
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-import')]")).Click();

            //ждем когда загрузится окно для загрузки документа 
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]")));
            //Процесс добавления файла
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-import-document')][2]//a[contains(@class,'js-add-file')]")).Click();

            FillAddDocumentForm(filePath);

            // Нажать Next
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]//span[contains(@class,'js-next')]")).Click();
            // Нажать Next
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]//div[contains(@class,'l-project-section')]")).Displayed);
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]//span[contains(@class,'js-next')]")).Click();
            // Нажать Finish
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-step last active')]")));
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-finish js-upload-btn')]")).Click();
        }

        /// <summary>
        /// Метод проверки удаления проекта (без файлов)
        /// </summary>
        [Test]
        public void DeleteProjectNoFileTest()
        {
            Authorization();
            //создать проект, который будем удалять
            CreateProject(ProjectName, false, "");

            // Удалить проект
            DeleteProjectFromList(ProjectName);

            Assert.IsFalse(GetIsProjectInList(ProjectName), "Ошибка: проект не удалился");
        }

        /// <summary>
        /// Метод проверки удаления проекта (с файлом)
        /// </summary>
        [Test]
        public void DeleteProjectWithFileTest()
        {
            Authorization();
            // Создать проект
            CreateProject(ProjectName, true, DocumentFile);

            // Дождаться, пока документ догрузится
            bool isWaitDisappeared = 
                WaitUntilDisappearElement(".//a[@class='js-name'][contains(text(),'" + ProjectName + "')]/..//img[contains(@class,'l-project-doc__progress')]", 50);
            Assert.IsTrue(isWaitDisappeared, "Ошибка: документ не загрузился");

            // Кликнуть по строке с проектом, чтобы открылась информация о нем (чтобы видно было документ)
            Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-tasks-table')]//tr//td[2]//a[@class='js-name'][text()='" + ProjectName + "']/../../../td[3]")).Click();
            Thread.Sleep(2000);
            // Выделить галочку проекта
            SelectProjectInList(ProjectName);
            // Нажать кнопку удалить
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-delete-btn')]")).Click();
            // Нажать Удалить проект
            Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'js-popup-delete-mode')]//input[contains(@class,'js-delete-project-btn')]")),
                "Ошибка: нет кнопки удалить проект (диалог удаления документа или проекта)");
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-delete-mode')]//input[contains(@class,'js-delete-project-btn')]")).Click();

            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-delete-mode')]", 30);
            // Проверить, что проект удалился
            Assert.IsTrue(GetIsProjectNotExist(ProjectName), "Ошибка: проект не удалился");
        }

        /// <summary>
        /// тестирование совпадения имени проекта с удаленным
        /// </summary>
        [Test]
        public void CreateProjectDeletedNameTest()
        {
            Authorization();
            // Создать проект
            CreateProject(ProjectName, false, "");

            // Удалить проект
            DeleteProjectFromList(ProjectName);

            // Проверить, остался ли проект в списке
            Assert.IsFalse(GetIsProjectInList(ProjectName), "Ошибка: проект не удалился");

            //создание нового проекта с именем удаленного
            FirstStepProjectWizard(ProjectName);

            // Проверить, что не появилось сообщение о существующем имени
            AssertErrorDuplicateName(false);
        }

        /// <summary>
        /// метод тестирования создания проекта с существующим именем
        /// </summary>
        [Test]
        public void CreateProjectDuplicateNameTest()
        {
            Authorization();
            // Создать проект
            CreateProject(ProjectName, false, "");
            Thread.Sleep(1000);
            // Начать создание проекта с этим же именем
            FirstStepProjectWizard(ProjectName);
            ClickNext();
            // Проверить, что появилась ошибка и поле Имя выделено ошибкой - ASSERT внутри
            AssertErrorDuplicateName();
        }

        /// <summary>
        /// метод проверки невозможности создания проекта в большим именем(>100 символов)
        /// </summary>
        [Test]
        public void CreateProjectBigNameTest()//Переделать тест, теперь поле с ограничением символов
        {
            Authorization();
            string bigName = ProjectName + "12345678901234567890123456789012345678901234567890123456789012345678901";
            // Проверить, что создалось имя длиннее 100 символов
            Assert.IsTrue(bigName.Length > 100, "Измените тест: длина имени должна быть больше 100");
            // Создать проект с превышающим лимит именем
            CreateProject(bigName, false, "");
            // Проверить, что проект не сохранился
            Assert.IsTrue(!GetIsProjectInList(bigName), "Ошибка: проект с запрещенно большим именем создался");
        }

        /// <summary>
        /// метод проверки на ограничение имени проекта (100 символов)
        /// </summary>
        [Test]
        public void CreateProjectLimitNameTest()
        {
            Authorization();
            string limitName = ProjectName + "1234567890123456789012345678901234567890123456789012345678901234567890";
            // Проверить, что создалось имя максимальной длины
            Assert.IsTrue(limitName.Length == 100, "Измените тест: длина имени должна быть ровно 100");
            // Создать проект с максимальным возможным именем
            CreateProject(limitName, false, "");
            // Проверить, что проект создался
            Assert.IsTrue(GetIsProjectInList(limitName), "Ошибка: проект с лимитным именем не создался");
        }

        /// <summary>
        /// метод тестирования создания проектов с одинаковыми source и target языками
        /// </summary>
        [Test]
        public void CreateProjectEqualLanguagesTest()
        {
            Authorization();
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard(ProjectName, false);
            ClickNext();

            // Проверить, что появилось сообщение о совпадающих языках
            Assert.IsTrue(IsElementDisplayed(By.XPath(
                ".//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-dynamic-errors')]//p[contains(@class,'js-error-sourceLanguage-match-targetLanguage')]")),
                "Ошибка: не появилось сообщение о совпадающих языках");
            
            // Проверить, что не перешли на следующий шаг
            Assert.IsFalse(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-create')]")),
                "Fail - переход на шаг настройки ТМ осуществлен");
        }


        /// <summary>
        /// метод для тестирования недопустимых символов в имени проекта
        /// </summary>
        [Test]
        public void CreateProjectForbiddenSymbolsTest()
        {
            Authorization();
            // Создать имя с недопустимыми символами
            string projectNameForbidden = ProjectName + " *|\\:\"<\\>?/ ";
            // Создать проект
            FirstStepProjectWizard(projectNameForbidden);
            ClickNext();
            // Проверить, что появилась ошибка и поле Имя выделено ошибкой
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-invalid-chars')]")),
                "Ошибка: не появилось сообщение о существующем имени");
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[contains(@class,'error')]")),
                "Ошибка: не появилось сообщение о существующем имени");
        }

        /// <summary>
        /// метод для тестирования проекта с пустым именем
        /// </summary>
        [Test]
        public void CreateProjectEmptyNameTest()
        {
            Authorization();
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard("");
            ClickNext();

            // Проверить, что появилась ошибка и поле Имя выделено ошибкой
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-required')]")),
                "Ошибка: не появилось сообщение о существующем имени");
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[contains(@class,'error')]")),
                "Ошибка: не появилось сообщение о существующем имени");
        }

        /// <summary>
        /// метод для тестирования создания имени проекта состоящего из одного пробела
        /// </summary>
        [Test]
        public void CreateProjectSpaceNameTest()
        {
            Authorization();
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard(" ");
            ClickNext();

            // Проверить, что появилась ошибка и поле Имя выделено ошибкой
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-required')]")),
                "Ошибка: не появилось сообщение о существующем имени");
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[contains(@class,'error')]")),
                "Ошибка: не появилось сообщение о существующем имени");
        }

        /// <summary>
        /// метод тестирования создания проекта с именем содержащим пробелы
        /// </summary>
        [Test]
        public void CreateProjectSpacePlusSymbolsNameTest()
        {
            Authorization();
            string projectName = ProjectName + "  " + "SpacePlusSymbols";
            // Создаем проект
            CreateProject(projectName, false, "");
            // Проверить, что проект создался
            Assert.IsTrue(GetIsProjectInList(projectName), "Ошибка: проект не создался");
        }

        /// <summary>
        /// метод для тестирования импорта разбираемых на сегменты файлов из заданной папки в существующий проект
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        [Test]
        [TestCaseSource("filesForImportCorrect")]
        public void ImportFilesAfterCreationCorrectTest(string filePath)
        {
            CreateProjectImportDocument(filePath);
            //Назначение задачи на пользователя
            AssignTask();

            // Зайти в редактор документа
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-editor-link')]")).Click();

            // Дождаться загрузки страницы
            Wait.Until((d) => d.Title.Contains("Editor"));

            // Проверить, существует ли хотя бы один сегмент
            Assert.IsTrue(IsElementPresent(By.CssSelector(
                "#segments-body div table tr:nth-child(1)"
                )));
        }

        /// <summary>
        /// Создать проект, добавить документ
        /// </summary>
        /// <param name="filePath"></param>
        protected void CreateProjectImportDocument(string filePath)
        {
            Authorization();
            //Создать пустой проект          
            CreateProject(ProjectName, false, "");
            Thread.Sleep(1000);
            //Проверка на наличие проекта
            CheckProjectInList(ProjectName);

            //Добавление документа
            ImportDocumentProjectSettings(filePath, ProjectName);
        }

        /// <summary>
        /// метод для тестирования импорта не разбираемых на сегменты файлов из заданной папки в существующий проект
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        [Test]
        [TestCaseSource("filesForImportError")]
        public void ImportFilesAfterCreationErrorTest(string filePath)
        {
            Authorization();

            //Создать пустой проект
            CreateProject(ProjectName, false, "");

            //Проверка на наличие проекта
            CheckProjectInList(ProjectName);

            //Добавление документа
            ImportDocumentProjectSettings(filePath, ProjectName);
            
            //Назначение задачи на пользователя
            AssignTask();

            // Строчка нужного проекта
            Driver.FindElement(By.LinkText(ProjectName)).Click();
            // Зайти в редактор документа
            Driver.FindElement(By.XPath(".//a[contains(@class,'js-editor-link')]")).Click();

            // Дождаться загрузки страницы
            Wait.Until((d) => d.Title.Contains("Editor"));

            // Проверить, существует ли хотя бы один сегмент
            Assert.IsTrue(IsElementPresent(By.CssSelector(
                "#segments-body div table tr:nth-child(1)"
                )));
        }

        /// <summary>
        /// метод для тестирования экспорта из проекта разбираемых на сегменты файлов из заданной папки
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        [Test]
        [TestCaseSource("filesForImportCorrect")]
        public void ExportOriginalCorrectDocumentTest(string filePath)
        {
            //Создать проект и импортировать файл 
            ImportFilesAfterCreationCorrectTest(filePath);
            // Выйти из редактора
            BackButton();
            // Зайти в проект
            Driver.FindElement(By.XPath(".//a[@class='js-name'][contains(text(),'" + ProjectName + "')]")).Click();
            // Нажать галочку у документа
            Driver.FindElement(By.XPath(".//table[contains(@class,'js-documents-table')]//td[contains(@class,'checkbox')]//input")).Click();
            //Нажать на кнопку Export
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-export-block')]")).Click();
            //Выбрать Исходный файл из выпадающего списка
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-document-export original')]//a")).Click();

            // Сохранить файл
            ExternalDialogSaveDocument("ExportedOriginalDocuments", filePath);
        }

        /// <summary>
        /// метод для тестирования экспорта из проекта не разбираемых на сегменты файлов из заданной папки
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        [Test]
        [TestCaseSource("filesForImportError")]
        public void ExportOriginalErrorDocumentTest(string filePath)
        {
            //Создать проект и импортировать файл 
            ImportFilesAfterCreationErrorTest(filePath);
            BackButton();

            // Зайти в проект
            Driver.FindElement(By.XPath(".//a[@class='js-name'][contains(text(),'" + ProjectName + "')]")).Click();
            // Нажать галочку у документа
            Driver.FindElement(By.XPath(".//table[contains(@class,'js-documents-table')]//td[contains(@class,'checkbox')]//input")).Click();

            //Нажать на кнопку Export
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-export-block')]")).Click();
            //Выбрать Исходный файл из выпадающего списка
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-document-export original')]//a")).Click();

            // Сохранить файл
            ExternalDialogSaveDocument("ExportedOriginalDocuments", filePath);
        }

        /// <summary>
        /// метод для тестирования экспорта из проекта переведенных файлов из заданной папки
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        [Test]
        [TestCaseSource("filesForConfirm")]
        public void ExportTranslatedCorrectDocumentTest(string filePath)
        {
            //Создать проект и импортировать файл 
            ImportFilesAfterCreationCorrectTest(filePath);
            // Подтвердить перевод
            ConfirmButton();
            // Выйти из редактора
            BackButton();
            // Зайти в проект
            Driver.FindElement(By.XPath(".//a[@class='js-name'][contains(text(),'" + ProjectName + "')]")).Click();
            // Нажать галочку у документа
            Driver.FindElement(By.XPath(".//table[contains(@class,'js-documents-table')]//td[contains(@class,'checkbox')]//input")).Click();
            
            //Нажать на кнопку Export
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-export-block')]")).Click();
            //Выбрать Перевод из выпадающего списка
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-document-export translation')]//a")).Click();

            // Сохранить файл
            ExternalDialogSaveDocument("ExportedTranslatedDocuments", filePath);
        }


        /// <summary>
        /// метод для тестирования отмены назначения документа пользователю
        /// </summary>
        [Test]
        public void ReassignDocumentToUserTest()
        {
            Authorization();

            //Создать пустой проект
            CreateProject(ProjectName, false, "");

            Thread.Sleep(4000);
            //Проверка на наличие проекта
            CheckProjectInList(ProjectName);

            //Добавление документа
            ImportDocumentProjectSettings(DocumentFile, ProjectName);
            //Назначение задачи на пользователя
            AssignTask();

            // Выбрать документ
            SelectDocumentInProject(1);
            // Нажать Progress
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-progress')]")).Click();
            
            // Назначить ответственного в окне Progress
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-assigned-block') and contains(@class,'inProgress')]//span[contains(@class,'js-assigned-cancel')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]"))).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-progress')][2]//span[contains(@class,'notAssigned')]")),
                "Статус назначения не изменился на notAssigned");

            Driver.FindElement(By.XPath(".//table[contains(@class,'js-progress-table')]//td[contains(@class,'assineer')]//span")).Click();
            Wait.Until(d => Driver.FindElements(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]"
                )));
            //Имя для cat-stage2
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__item')][@title='Bob Test']")).Click();
            Thread.Sleep(1000);

            // Нажать на Assign
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-assign')]//a")).Click();

            WaitUntilDisplayElement(".//span[contains(@class,'js-assigned-cancel')]");
            // Нажать на Close
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-progress')][2]//span[contains(@class,'js-popup-close')]")).Click();
        }

        /// <summary>
        /// Загрузка в проект документа, который уже был загружен
        /// </summary>
        [Test]
        public void ImportDuplicateDocumentTest()
        {
            // Создать проект, загрузить документ, зайти в редактор
            CreateProjectImportDocument(DocumentFile);
            // Кликнуть по Импорт
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-import')]")).Click();

            //ждем когда загрузится окно для загрузки документа 
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]")));
            //Процесс добавления файла
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-import-document')][2]//a[contains(@class,'js-add-file')]")).Click();
            // Указать тот же документ
            FillAddDocumentForm(DocumentFile);

            // Проверить появление оповещения об ошибки
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-info-popup')]")),
                "Ошибка: не появилось сообщение о повторном файле");
        }

        /// <summary>
        /// Удаление документа из проекта
        /// </summary>
        [Test]
        public void DeleteDocumentFromProject()
        {
            // Создать проект, загрузить документ, зайти в редактор
            CreateProjectImportDocument(DocumentFile);

            // Выбрать документ
            Driver.FindElement(By.XPath(".//table[contains(@class,'js-documents-table')]//tr[1]/td[1]/span")).Click();
            // Нажать удалить
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-delete')]")).Click();

            // Подтвердить
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-confirm')]")).Displayed);
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]")).Click();
            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-confirm')]");

            // Проверить, что документа нет
            Assert.IsFalse(IsElementPresent(By.XPath(".//table[contains(@class,'js-documents-table')]//tr[1]/td[1]/span")),
                "Ошибка: документ не удалился");
        }

        /// <summary>
        /// отмена создания проекта на первом шаге
        /// </summary>
        [Test]
        public void CancelFirstTest()
        {
            // Зайти под пользователем и открыть форму создания проекта
            LoginOpenCreateProject();
            // Нажать Отмену
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-popup-close')]")).Click();
            // Проверить, что форма создания проекта закрылась
            Assert.IsTrue(WaitUntilDisappearElement(".//div[contains(@class,'js-popup-create-project')][2]"), "Ошибка: не закрылась форма создания проекта");
        }

        // TODO: Убрать если у нас не будет кнопки back для возврата на первый шаг для отмены создания. СЕйчас реализовано, что кнопки нет, но в документации - кнопка описана.
        /// <summary>
        /// отмена создания проекта(подтверждение отмены)
        /// </summary>
        //[Test]
        public void CancelYesTest()
        {

        }

        // TODO: Убрать если у нас не будет кнопки back для возврата на первый шаг для отмены создания. СЕйчас реализовано, что кнопки нет, но в документации - кнопка описана.
        /// <summary>
        /// отмена создания проекта - No 
        /// </summary>
        //[Test]
        public void CancelNoTest()
        {

        }

        /// <summary>
        /// изменение имени проекта на новое по нажатию кнопки Back
        /// </summary>
        [Test]
        public void ChangeProjectNameOnNew()
        {
            // Авторизация
            Authorization();
            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            ClickNext();
            // Нажать Back
            ClickBack();
            // Проверили, что вернулись на первый шаг
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[@name='name']")),
                "Ошибка: по кнопке Back не вернулись на предыдущий шаг (где имя проекта)");
            // Изменить имя
            FillProjectNameInForm("TestProject" + DateTime.Now.Ticks);
            // Next
            ClickNext();
            // Проверить, что ошибки не появилось
            AssertErrorDuplicateName(false);
            // Проверить, что перешли на шаг выбора ТМ
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-create')]")),
                "Ошибка: не перешли на следующий шаг (выбора ТМ)");
        }

        /// <summary>
        /// изменение имени проекта на существующее
        /// </summary>
        [Test]
        public void ChangeProjectNameOnExist()
        {
            Authorization();
            // Создать проект
            CreateProject(ProjectName, false, "");
            Thread.Sleep(1000);

            // Открыли форму создания проекта, заполнили поля
            string newProjectName = "TestProject" + DateTime.Now.Ticks;
            FirstStepProjectWizard(newProjectName);
            // Next
            ClickNext();
            // Нажать Back
            ClickBack();
            // Проверили, что вернулись на первый шаг
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[@name='name']")),
                "Ошибка: по кнопке Back не вернулись на предыдущий шаг (где имя проекта)");
            // Изменить имя
            FillProjectNameInForm(ProjectName);
            // Next
            ClickNext();
            // Проверить, что ошибка появилась
            AssertErrorDuplicateName(true);
            // Проверить, что не перешли на шаг выбора ТМ
            Assert.IsFalse(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-create')]")),
                "Ошибка: перешли на следующий шаг (выбора ТМ)");
        }

        /// <summary>
        /// Тест: создание проекта, возврат на первый шаг
        /// Проверка, что настройки сохранились
        /// - имя проекта
        /// - target язык
        /// - Deadline дата
        /// </summary>
        [Test]
        public void BackFirstStepCheckSettings()
        {
            Authorization();
            
            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            ClickNext();
            // Нажать Back
            ClickBack();
            // Проверили, что вернулись на первый шаг
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[@name='name']")),
                "Ошибка: по кнопке Back не вернулись на первый шаг");

            // Получить прописанное имя проекта
            string resultProjectName = Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-project')][2]//input[contains(@name,'name')]")).GetAttribute("value");
            // Target язык
            string resultTargetLanguage = Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'ui-multiselect-value')]")).Text;
            // Deadline дата
            string resultDeadline = Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-project')][2]//input[contains(@name,'deadlineDate')]")).GetAttribute("value");

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на первый шаг не сохранились настройки:\n";

            if (resultProjectName != ProjectName)
            {
                isError = true;
                errorMessage += "- имя проекта не сохранилось\n";
            }

            if (resultTargetLanguage != "Russian")
            {
                isError = true;
                errorMessage += "- язык Target не сохранился\n";
            }

            if (resultDeadline != DeadlineDate)
            {
                isError = true;
                errorMessage += "- Deadline дата не сохранилась\n";
            }

            // Проверить ошибки
            Assert.IsFalse(isError, errorMessage);
        }

        /// <summary>
        /// Тест: создание проекта, возврат на шаг выбора ТМ
        /// Проверка, что настройки сохранились
        /// - выбранный ТМ
        /// </summary>
        [Test]
        public void BackChooseTMStepCheckSettings()
        {
            Authorization();

            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            ClickNext();
            // Выбрать ТМ
            ChooseExistingTM();
            // Нажать Back
            ClickBack();
            // Проверить 
            CheckTMSettings();
        }

        /// <summary>
        /// Тест: создание проекта, выбор ТМ, возврат на предыдущий шаг, обратно к выбору ТМ
        /// Проверка, что настройки сохранились
        /// - выбранный ТМ
        /// </summary>
        [Test]
        public void BackNextChooseTMStepCheckSettings()
        {
            Authorization();

            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            ClickNext();
            // Выбрать ТМ
            Driver.FindElement(By.XPath(".//table[contains(@class,'js-tms-popup-table')]//tr[1]//td[1]//input")).Click();
            // Нажать Back
            ClickBack();
            // Next
            ClickNext();
            // Проверить 
            CheckTMSettings();
        }

        /// <summary>
        /// Тест: создание проекта, возврат на шаг выбора глоссария
        /// Проверка, что настройки сохранились
        /// - выбранный глоссарий
        /// </summary>
        [Test]
        public void BackChooseGlossaryStepCheckSettings()
        {
            Authorization();

            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            ClickNext();
            // Выбрать ТМ
            ChooseExistingTM();
            // Выбрать глоссарий
            ChooseFirstGlossary();

            // Нажать Back
            ClickBack();
            // Проверить, сохранился ли выбор глоссария
            CheckGlossarySettings();
        }

        /// <summary>
        /// Тест: создание проекта, выбор глоссария, возврат на предыдущий шаг, обратно к выбору глоссария
        /// Проверка, что настройки сохранились
        /// - выбранный глоссарий
        /// </summary>
        [Test]
        public void BackNextChooseGlossaryStepCheckSettings()
        {
            Authorization();

            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            ClickNext();
            // Выбрать ТМ
            ChooseExistingTM();
            // Выбрать глоссарий
            Driver.FindElement(By.XPath(".//table[contains(@class,'js-glossaries-table')]//tbody//tr[1]//input")).Click();

            // Нажать Back
            ClickBack();
            // Next
            ClickNext();
            // Проверить, сохранился ли выбор глоссария
            CheckGlossarySettings();
        }

        /// <summary>
        /// Тест: создание проекта, возврат на шаг выбора MT
        /// Проверка, что настройки сохранились
        /// - выбранный MT
        /// </summary>
        [Test]
        public void BackChooseMTStepCheckSettings()
        {
            Authorization();

            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            ClickNext();
            // Выбрать ТМ
            ChooseExistingTM();
            // Выбрать глоссарий
            ChooseFirstGlossary();
            // Выбрать compreno
            ChooseMTCompreno();

            // Нажать Back
            ClickBack();

            // Проверить сохранился ли выбор МТ
            CheckMTSettings();
        }

        /// <summary>
        /// Тест: создание проекта, выбор МТ, возврат на предыдущий шаг, обратно к МТ
        /// Проверка, что настройки сохранились
        /// - выбранный MT
        /// </summary>
        [Test]
        public void BackNextChooseMTStepCheckSettings()
        {
            Authorization();

            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            ClickNext();
            // Выбрать ТМ
            ChooseExistingTM();
            // Выбрать глоссарий
            ChooseFirstGlossary();
            // Выбрать compreno
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-mts-table')]//tbody//tr[3]//input")).Click();
            // Нажать Back
            ClickBack();
            // Next
            ClickNext();

            // Проверить, сохранился ли выбор МТ
            CheckMTSettings();
        }

        /// <summary>
        /// Тест: создание проекта, выбор stage, возврат к предудыщему, обратно к stage
        /// Проверка, что настройки сохранились
        /// - выбранный Stage
        /// </summary>
        [Test]
        public void BackNextChooseStageCheckSettings()
        {
            Authorization();

            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            ClickNext();
            // Выбрать ТМ
            ChooseExistingTM();
            // Выбрать глоссарий
            ChooseFirstGlossary();
            // Выбрать compreno
            ChooseMTCompreno();
            // Выбрать Stage
            string stageText = "Editing";
            Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-workflow-table')]//tbody//td[2]//span[contains(@class,'js-dropdown__text')]")).Click();
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__item')][contains(@title,'" + stageText + "')]")).Click();

            // Нажать Back
            ClickBack();
            WaitUntilDisplayElement(".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-mts-table')]");
            // Next
            ClickNext();

            // Проверили, что вернулись на шаг выбора stage
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-workflow-table')]")),
                "Ошибка: не вернулись на предыдущий шаг (выбор Stage)");

            // Значение Stage
            string resultStage = Driver.FindElement(By.XPath(
                 ".//table[contains(@class,'js-workflow-table')]//tbody//td[2]//span[contains(@class,'js-dropdown__text')]")).Text;

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на шаг с выбором Stage не сохранились настройки:\n";

            if (!resultStage.Contains(stageText))
            {
                isError = true;
                errorMessage += "- stage не сохранился\n";
            }

            // Проверить ошибки
            Assert.IsFalse(isError, errorMessage);
        }

        /// <summary>
        /// изменение имени проекта на удаленное
        /// </summary>
        [Test]
        public void ChangeProjectNameOnDeleted()
        {
            Authorization();
            
            // Создать проект
            CreateProject(ProjectName, false, "");
            // Удалить проект
            DeleteProjectFromList(ProjectName);
            // Проверить, остался ли проект в списке
            Assert.IsFalse(GetIsProjectInList(ProjectName), "Ошибка: проект не удалился");

            //создание нового проекта с именем удаленного
            string newProjectName = "TestProject" + DateTime.Now.Ticks;
            FirstStepProjectWizard(newProjectName);
            // Next
            ClickNext();
            // Нажать Back
            ClickBack();
            // Проверили, что вернулись на первый шаг
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[@name='name']")),
                "Ошибка: по кнопке Back не вернулись на предыдущий шаг (где имя проекта)");
            // Изменить имя
            FillProjectNameInForm(ProjectName);
            // Next
            ClickNext();
            // Проверить, что ошибка не появилась
            AssertErrorDuplicateName(false);
            // Проверить, что перешли на шаг выбора ТМ
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-create')]")),
                "Ошибка: перешли на следующий шаг (выбора ТМ)");
        }

        const string EXPORT_TYPE_SOURCE = "Original";
        const string EXPORT_TYPE_TMX = "TMX";
        const string EXPORT_TYPE_TARGET = "Translation";

        [Test]
        [TestCase(EXPORT_TYPE_SOURCE)]
        [TestCase(EXPORT_TYPE_TMX)]
        [TestCase(EXPORT_TYPE_TARGET)]
        public void NewExportDocumentFromProjectTest(string exportType)
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = CreateCommonProjectOneDocument();
            CancelAllExportDownload();

            // Зайти в проект
            ClickProjectInList(projectName);
            // Нажать галочку у документа
            SelectDocumentInProject();
            // Нажать "красный" экспорт
            SelectExportMainProjectPanel(exportType);
            // Экспортировать документ
            WorkWithExport();
        }

        [Test]
        [TestCase(EXPORT_TYPE_SOURCE)]
        [TestCase(EXPORT_TYPE_TMX)]
        [TestCase(EXPORT_TYPE_TARGET)]
        public void NewExportProjectFromProjectListTest(string exportType)
        {
            // TODO изменить выбор проекта
            Authorization();
            CreateProject(ProjectName, true, DocumentFile, "", false);
            ImportDocumentProjectSettings(DocumentFileToConfirm, ProjectName);
            AssignTask();
            OpenDocument(2);
            ConfirmButton();
            BackButton();
            Assert.Pass();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Выбрать этот проект
            SelectProjectInList(projectName);
            SelectExportMainProjectPanel(exportType);
            WorkWithExport();
        }

        [Test]
        [TestCase(EXPORT_TYPE_SOURCE)]
        [TestCase(EXPORT_TYPE_TMX)]
        [TestCase(EXPORT_TYPE_TARGET)]
        public void NewExportProjectFromProjectListProjectSettingsTest(string exportType)
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Открыть информацию о проекте
            ClickProjectOpenInfo(projectName);
            SelectExportProjectListRow(exportType);
            WorkWithExport();
        }

        [Test]
        [TestCase(EXPORT_TYPE_SOURCE)]
        [TestCase(EXPORT_TYPE_TMX)]
        [TestCase(EXPORT_TYPE_TARGET)]
        public void NewExportDocumentFromProjectListDocumentSettingsTest(string exportType)
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestDelete";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Открыть информацию о проекте
            ClickProjectOpenInfo(projectName);
            ClickDocumentOpenInfo(1);
            SelectExportProjectListDocumentRow(exportType);
            WorkWithExport();
        }

        [Test]
        public void NewExportCloseNotifier()
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Открыть информацию о проекте
            ClickProjectOpenInfo(projectName);
            SelectExportProjectListRow(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();
            ClickCancelExportNotifier();

            // Дождаться, пока информационное окно пропадет
            Assert.IsTrue(WaitUntilDisappearElement(
                GetExportNotifierXPath()),
                "Ошибка: сообщение с экспортом не закрылось");
        }

        const string PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_LIST = "updateProjectList";
        const string PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_PAGE = "updateProjectPage";
        const string PLACE_SEARCH_NOTIFIER_OPEN_PROJECT = "openProject";
        const string PLACE_SEARCH_NOTIFIER_OPEN_PROJECT_LIST = "openProjectList";
        const string PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY = "openGlossary";
        [Test]
        [TestCase(PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_LIST)]
        [TestCase(PLACE_SEARCH_NOTIFIER_OPEN_PROJECT)]
        [TestCase(PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY)]
        public void NewExportSaveNotifierProjectList(string placeSearch)
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Открыть информацию о проекте
            ClickProjectOpenInfo(projectName);
            SelectExportProjectListRow(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            switch (placeSearch)
            {
                case PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_LIST:
                    Driver.Navigate().Refresh();
                    break;
                case PLACE_SEARCH_NOTIFIER_OPEN_PROJECT:
                    ClickProjectInList(projectName);
                    break;
                case PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY:
                    SwitchGlossaryTab();
                    Assert.IsFalse(IsElementPresent(By.XPath(
                        GetExportNotifierXPath())),
                        "Ошибка: информационное окно об экспорте появилось на странице глоссария");
                    OpenMainWorkspacePage();
                    break;
                default:
                    Assert.Fail("Неверный аргумент: " + placeSearch);
                    break;
            }            
            
            // Проверить, что есть окно с экпортом
            Assert.IsTrue(IsElementPresent(By.XPath(
                GetExportNotifierXPath())),
                "Ошибка: информационное окно об экспорте пропало после обновления страницы");
        }

        [Test]
        [TestCase(PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_PAGE)]
        [TestCase(PLACE_SEARCH_NOTIFIER_OPEN_PROJECT_LIST)]
        [TestCase(PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY)]
        public void NewExportSaveNotifierProjectPage(string placeSearch)
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Открыть проект
            ClickProjectInList(projectName);
            SelectDocumentInProject();
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            switch (placeSearch)
            {
                case PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_PAGE:
                    Driver.Navigate().Refresh();
                    break;
                case PLACE_SEARCH_NOTIFIER_OPEN_PROJECT_LIST:
                    OpenMainWorkspacePage();
                    break;
                case PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY:
                    SwitchGlossaryTab();
                    Assert.IsFalse(IsElementPresent(By.XPath(
                        GetExportNotifierXPath())),
                        "Ошибка: информационное окно об экспорте появилось на странице глоссария");
                    OpenMainWorkspacePage();
                    ClickProjectInList(projectName);
                    break;
                default:
                    Assert.Fail("Неверный аргумент: " + placeSearch);
                    break;
            }

            // Проверить, что есть окно с экпортом
            Assert.IsTrue(IsElementPresent(By.XPath(
                GetExportNotifierXPath())),
                "Ошибка: информационное окно об экспорте пропало после обновления страницы");
        }

        [Test]
        public void NewExportSaveNotifierAnotherProjectPage(string placeSearch)
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";
            string projectName2 = "TestProject 635359966174263015";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Открыть проект
            ClickProjectInList(projectName);
            SelectDocumentInProject();
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            // Вернуться к списку проектов
            OpenMainWorkspacePage();
            ClickProjectInList(projectName2);

            // Проверить, что есть окно с экпортом
            Assert.IsTrue(IsElementPresent(By.XPath(
                GetExportNotifierXPath())),
                "Ошибка: информационное окно об экспорте пропало после обновления страницы");
        }

        [Test]
        public void NewExportDocumentFromProjectCheckNotifierText()
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            ClickProjectInList(projectName);
            SelectDocumentInProject();
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            string notifierText = GetNotifierText();

            // TODO заменить название документа
            Assert.IsTrue(notifierText.Contains("littleEarth.docx"),
                "Ошибка: неправильный текст в сообщении об экспорте: нет названия документа\nСейчас текст: " + notifierText);
        }

        [Test]
        public void NewExportDocumentsFromProjectCheckNotifierText()
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            ClickProjectInList(projectName);
            SelectDocumentInProject(1);
            SelectDocumentInProject(2);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            string notifierText = GetNotifierText();

            // TODO заменить название документа
            Assert.IsTrue(notifierText.Contains("Documents"),
                "Ошибка: неправильный текст в сообщении об экспорте: нет указания на несколько документов.\nСейчас текст: " + notifierText);
        }

        [Test]
        public void NewExportDocumentFromProjectListCheckNotifierText()
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Открыть информацию о проекте
            ClickProjectOpenInfo(projectName);
            ClickDocumentOpenInfo(1);
            SelectExportProjectListDocumentRow(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            string notifierText = GetNotifierText();

            // TODO заменить название документа
            Assert.IsTrue(notifierText.Contains("littleEarth.docx"),
                "Ошибка: неправильный текст в сообщении об экспорте: нет названия документа\nСейчас текст: " + notifierText);
        }

        [Test]
        public void NewExportProjectMultiDocCheckNotifierText()
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Выбрать этот проект
            SelectProjectInList(projectName);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            string notifierText = GetNotifierText();

            // TODO заменить название документа
            Assert.IsTrue(notifierText.Contains("Documents"),
                "Ошибка: неправильный текст в сообщении об экспорте: нет множественного числа (документОВ)\nСейчас текст: " + notifierText);
        }

        [Test]
        public void NewExportProjectOneDocCheckNotifierText()
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359111650514550";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Выбрать этот проект
            SelectProjectInList(projectName);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            string notifierText = GetNotifierText();

            // TODO заменить название документа
            Assert.IsTrue(notifierText.Contains("littleEarth.docx"),
                "Ошибка: неправильный текст в сообщении об экспорте: нет названия документа\nСейчас текст: " + notifierText);
        }

        [Test]
        public void NewExportProjectsCheckNotifierText()
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359111650514550";
            string projectName2 = "TestProject 635359966174263015";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Выбрать этот проект
            SelectProjectInList(projectName);
            SelectProjectInList(projectName2);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            string notifierText = GetNotifierText();

            // TODO заменить название документа
            Assert.IsTrue(notifierText.Contains("Documents"),
                "Ошибка: неправильный текст в сообщении об экспорте проектов: нет множественного числа\nСейчас текст: " + notifierText);
        }

        [Test]
        public void NewExportDocumentCheckNotifierDate()
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestProject 635359166399768218";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            ClickProjectInList(projectName);
            SelectDocumentInProject();
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();

            DateTime curDate = DateTime.Now.AddHours(3);

            string notifierText = GetNotifierText();
            Console.WriteLine("notifierText:\n" + notifierText);
            int startIndex = notifierText.IndexOf("/") - 2;
            Assert.IsTrue(startIndex > 0, "Ошибка: неверный формат даты в сообщении: " + notifierText);

            string month = notifierText.Substring(startIndex, 2);
            startIndex += 3; // "mm/" = 3
            string day = notifierText.Substring(startIndex, 2);
            startIndex += 3; // "dd/" = 3
            string year = notifierText.Substring(startIndex, 4);
            startIndex += 5; // "yyyy " = 5;
            string hour = notifierText.Substring(startIndex, 2);
            startIndex += 3; // "hh:" = 3
            string min = notifierText.Substring(startIndex, 2);
            Console.WriteLine(month + "/" + day + "/" + year + " " + hour + ":" + min);

            DateTime notifierDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(min), 0);

            double timeSubtract = curDate.Subtract(notifierDate).Ticks;
            timeSubtract = timeSubtract < 0 ? (-1 * timeSubtract) : timeSubtract;

            Assert.IsTrue(timeSubtract < TimeSpan.TicksPerHour,
                "Ошибка: неправильная дата в сообщений об экспорте: " + notifierText);
        }

        [Test]
        public void NewExportMultiNotifiers()
        {
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestDelete";
            string projectName2 = "TestProject 635360046537156162";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Выбрать этот проект
            SelectProjectInList(projectName);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();
            Thread.Sleep(2000);

            SelectProjectInList(projectName);
            SelectProjectInList(projectName2);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);

            Assert.IsTrue(WaitUntilDisplayElement(GetExportNotifierXPath() + "[2]"),
                "Ошибка: не появилось второе сообщение об экспорте");

            int notifierNum = Driver.FindElements(By.XPath(GetExportNotifierXPath())).Count;

            Assert.IsTrue(notifierNum > 1, "Ошибка: сообщение об экспорте только одно (или ни одного)");
        }

        [Test]
        public void NewExportChangeNotifiers()
        {
            // должны быть с разными документами
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestDelete";
            string projectName2 = "TestProject 635360046537156162";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Выбрать этот проект
            SelectProjectInList(projectName);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();
            SelectProjectInList(projectName);
            SelectProjectInList(projectName2);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);

            Assert.IsTrue(WaitUntilDisplayElement(GetExportNotifierXPath() + "[2]"),
                "Ошибка: не появилось второе сообщение об экспорте");
            Thread.Sleep(1000);            
            string firstNotifierText = GetNotifierText();
            Driver.FindElement(By.XPath(GetExportNotifierXPath() + "[1]")).Click();
            Thread.Sleep(1000);
            string secondNotifierText = GetNotifierText();
            Assert.AreNotEqual(firstNotifierText, secondNotifierText, "Ошибка: сообщение не изменилось");
        }


        const int maxNotifierNumber = 5;

        [Test]
        public void NewExportLimitNotifiers()
        {
            // TODO изменить выбор проекта
            // если с эксплортом одного и того же документа баг - поменять
            Authorization();
            string projectName = "TestDelete";
            string projectName2 = "TestProject 635360046537156162";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Выбрать этот проект
            SelectProjectInList(projectName);
            for (int i = 0; i < maxNotifierNumber; ++i)
            {
                SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
                Thread.Sleep(2000);
                Assert.IsTrue(WaitUntilDisplayElement(GetExportNotifierXPath() + "[" + (i + 1) + "]"),
                    "Ошибка: не появилось новое сообщение об экспорте (" + (i + 1) + ")");
            }
        }

        [Test]
        public void NewExportMoreLimitNotifiers()
        {
            // TODO изменить выбор проекта
            // если с эксплортом одного и того же документа баг - поменять
            Authorization();
            string projectName = "TestDelete";
            string projectName2 = "TestProject 635360046537156162";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Выбрать этот проект
            SelectProjectInList(projectName);
            for (int i = 0; i < maxNotifierNumber; ++i)
            {
                SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
                Thread.Sleep(2000);
                Assert.IsTrue(WaitUntilDisplayElement(GetExportNotifierXPath() + "[" + (i + 1) + "]"),
                    "Ошибка: не появилось новое сообщение об экспорте (" + (i + 1) + ")");
            }

            // Вызвать еще раз экспорт
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            Thread.Sleep(2000);
            int notifierNum2 = Driver.FindElements(By.XPath(GetExportNotifierXPath())).Count;

            Assert.IsFalse(notifierNum2 > maxNotifierNumber, "Ошибка: слишком много сообщений об экспорте");
        }

        [Test]
        public void NewExportChangeNotifiersDownload()
        {
            // должны быть с разными документами
            // TODO изменить выбор проекта
            Authorization();
            string projectName = "TestDelete";
            string projectName2 = "TestProject 635360046537156162";

            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // Выбрать этот проект
            SelectProjectInList(projectName);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);
            WaitExportDownloadBtn();
            SelectProjectInList(projectName);
            SelectProjectInList(projectName2);
            SelectExportMainProjectPanel(EXPORT_TYPE_SOURCE);

            Assert.IsTrue(WaitUntilDisplayElement(GetExportNotifierXPath() + "[2]"),
                "Ошибка: не появилось второе сообщение об экспорте");
            Thread.Sleep(1000);
            string firstNotifierText = GetNotifierText();
            Driver.FindElement(By.XPath(GetExportNotifierXPath() + "[1]")).Click();
            Thread.Sleep(1000);

            ClickDownloadExportNotifier();
            // проверить загрузку
            // TODO мб не реализовать...
            Assert.Fail("доделать тест");
        }

        [Test]
        [TestCase(EXPORT_TYPE_SOURCE)]
        [TestCase(EXPORT_TYPE_TMX)]
        [TestCase(EXPORT_TYPE_TARGET)]
        public void NewExportRenameвDocument(string exportType)
        {
            Authorization();
            // Закрыть все окна с экспортом
            CancelAllExportDownload();

            // TODO изменить выбор проекта
            string projectName = "TestDelete";
            ClickProjectOpenInfo(projectName);
            ClickDocumentOpenInfo(1);

            Driver.FindElement(By.XPath(".//tr[contains(@class,'js-document-panel')]//span[contains(@class,'js-settings-btn')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-document-settings')][2]")).Displayed);
            IWebElement nameEl = Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-document-settings')][2]//input[contains(@class,'js-name')]"));
            nameEl.Clear();
            projectName = "TestProject" + DateTime.Now.Ticks;
            nameEl.SendKeys(projectName);
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-document-settings')][2]//span[contains(@class,'js-save')]")).Click();
            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-document-settings')][2]");

            SelectExportProjectListDocumentRow(exportType);

            WaitExportDownloadBtn();

            string notifierText = GetNotifierText();

            Assert.IsTrue(notifierText.Contains(projectName), "Ошибка: экспортируется документ со старым названием");
        }

        

        /// <summary>
        /// Удаление проект на вкладке проектов по имени
        /// </summary>
        /// <param name="ProjectNameToDelete">имя проекта, который надо удалить</param>
        private void DeleteProjectFromList(string ProjectNameToDelete)
        {
            // Выбрать этот проект
            SelectProjectInList(ProjectNameToDelete);
            // Нажать Удалить
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-delete-btn')]")).Click();
            // Подтвердить
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-confirm')]")).Displayed);
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]")).Click();
            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-confirm')]", 30);
            Thread.Sleep(500);
        }

        /// <summary>
        /// метод проверки наличия проекта в списке проектов
        /// </summary>
        /// <param name="ProjectNameCheck">Имя проекта, которое ищем в списке проектов</param>
        private void CheckProjectInList(string ProjectNameCheck)
        {
            //проверка, что проект с именем ProjectNameCheck есть на странице
            Assert.IsTrue(GetIsProjectInList(ProjectNameCheck), "Проверка на наличие проекта среди созданных  - не пройдена");
        }

        /// <summary>
        /// Проверить, что проект есть в списке
        /// </summary>
        /// <param name="projectNameCheck">имя проекта</param>
        /// <returns>есть в списке</returns>
        private bool GetIsProjectInList(string projectNameCheck)
        {
            return IsElementPresent(By.XPath(
                ".//table[contains(@class,'js-tasks-table')]//tr//td[2]//a[@class='js-name'][text()='" + projectNameCheck + "']"));
        }

        /// <summary>
        /// Проверить, что проекта нет (проверка без лишнего ожидания)
        /// </summary>
        /// <param name="projectNameCheck">имя проекта</param>
        /// <returns>нет в списке</returns>
        private bool GetIsProjectNotExist(string projectNameCheck)
        {
            setDriverTimeoutMinimum();
            bool isNotExist = !GetIsProjectInList(projectNameCheck);
            setDriverTimeoutDefault();
            return isNotExist;
        }

        /// <summary>
        /// Загрузить документ в форме создания проекта
        /// </summary>
        /// <param name="documentName">имя документа</param>
        private void ImportDocumentCreateProject(string documentName)
        {
            //процесс добавления файла 
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//a[contains(@class,'js-add-file')]")).Click();
            FillAddDocumentForm(documentName);
        }


        /// <summary>
        /// Авторизация и открытие формы создания проекта
        /// </summary>
        protected void LoginOpenCreateProject()
        {
            // Авторизация
            Authorization();
            Assert.IsTrue(Driver.FindElement(By.XPath(".//span[contains(@class,'js-project-create')]")).Displayed);
            // Открыть создание проекта
            OpenCreateProjectForm();
        }

        /// <summary>
        /// Открыть форму создания проекта
        /// </summary>
        protected void OpenCreateProjectForm()
        {
            //нажать <Create>
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-project-create')]")).Click();

            //ждем загрузки формы
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]")).Displayed);
        }

        /// <summary>
        /// Проверка, есть ли ошибка существующего имени
        /// </summary>
        /// <param name="shouldErrorExist">условие для результата проверки - должна ли быть ошибка</param>
        protected void AssertErrorDuplicateName(bool shouldErrorExist = true)
        {
            if (!shouldErrorExist)
            {
                // Т.к. ожидаем, что ошибка не появится, опускаем таймаут
                setDriverTimeoutMinimum();
            }

            // Проверить, что поле Имя отмечено ошибкой
            bool isExistErrorInput = IsElementDisplayed(By.XPath(
                ".//div[contains(@class,'js-popup-create-project')][2]//input[@name='name'][contains(@class,'error')]"));
            // Проверить, что есть сообщение, что имя существует
            bool isExistErrorMessage = IsElementDisplayed(By.XPath(
                ".//div[contains(@class,'js-popup-create-project')][2]//p[contains(@class,'js-error-name-exists')]"));

            string errorMessage = "\n";
            bool isError = false;
            // Ошибка должна появиться
            if (shouldErrorExist)
            {
                if (!isExistErrorInput)
                {
                    isError = true;
                    errorMessage += "Ошибка: поле Название не отмечено ошибкой\n";
                }
                if (!isExistErrorMessage)
                {
                    isError = true;
                    errorMessage += "Ошибка: не появилось сообщение о существующем имени";
                }
            }
            // Ошибка НЕ должна появиться
            else
            {
                if (isExistErrorInput)
                {
                    isError = true;
                    errorMessage += "Ошибка: поле Название отмечено ошибкой\n";
                }
                if (isExistErrorMessage)
                {
                    isError = true;
                    errorMessage += "Ошибка: появилось сообщение о существующем имени";
                }
            }

            // Проверить условие
            Assert.IsFalse(isError, errorMessage);

            if (!shouldErrorExist)
            {
                setDriverTimeoutDefault();
            }
        }

        /// <summary>
        /// Проверка, что выбран Compreno MT
        /// </summary>
        protected void CheckMTSettings()
        {
            // Проверили, что вернулись на шаг выбора MT
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-mts-table')]")),
                "Ошибка: не вернулись на шаг выбора МТ");

            // Значение checkbox у MT
            string resultFirstMTCheck = Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-mts-table')]//tbody//tr[3]//input")).GetAttribute("checked");

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на шаг с выбором MT не сохранились настройки:\n";

            if (resultFirstMTCheck != "true")
            {
                isError = true;
                errorMessage += "- checkbox выбора MT не сохранился\n";
            }

            // Проверить ошибки
            Assert.IsFalse(isError, errorMessage);
        }

        /// <summary>
        /// Проверка, что выбран 1й глоссарий
        /// </summary>
        protected void CheckGlossarySettings()
        {
            // Проверили, что вернулись на шаг выбора глоссария
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-glossaries-table')]")),
                "Ошибка: не вернулись на шаг выбор глоссария");

            // Значение checkbox у глоссария
            string resultFirstGlossaryCheck = Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-glossaries-table')]//tbody//tr[1]//input")).GetAttribute("checked");

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на шаг с выбором глоссария не сохранились настройки:\n";

            if (resultFirstGlossaryCheck != "true")
            {
                isError = true;
                errorMessage += "- checkbox выбора глоссария не сохранился\n";
            }

            // Проверить ошибки
            Assert.IsFalse(isError, errorMessage);
        }

        /// <summary>
        /// Проверка:
        /// - выбран checkbox первого ТМ
        /// - выбран radio первого ТМ
        /// </summary>
        protected void CheckTMSettings()
        {
            // Проверили, что вернулись на шаг выбора ТМ
            Assert.IsTrue(IsElementDisplayed(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]")),
                "Ошибка: не вернулись на шаг выбора ТМ");

            // Значение checkbox первого ТМ
            string resultFirstTMCheck = Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-tms-popup-table')]//tr[1]//td[1]//input")).GetAttribute("checked");
            // Значение radio первого ТМ
            string resultFirstTMRadio = Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-tms-popup-table')]//tr[1]//td[6]//input")).GetAttribute("checked");

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на шаг с выбором ТМ не сохранились настройки:\n";

            if (resultFirstTMCheck != "true")
            {
                isError = true;
                errorMessage += "- checkbox выбора ТМ не сохранился\n";
            }

            if (resultFirstTMRadio != "true")
            {
                isError = true;
                errorMessage += "- radio Write выбора ТМ не сохранился\n";
            }

            // Проверить ошибки
            Assert.IsFalse(isError, errorMessage);
        }

        /// <summary>
        /// Нажатие Back при создании проекта
        /// </summary>
        protected void ClickBack()
        {
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-back')]")).Click();
        }

        
        protected string GetExportNotifierXPath()
        {
            return ".//div[@id='export-documents-notifier']//div[contains(@class,'g-exportDocNotifications-item')]";
        }

        protected string GetNotifierText()
        {
            return Driver.FindElement(By.XPath(GetExportNotifierXPath() + "//div[not(contains(@style,'none'))]/span")).Text;
        }

        /// <summary>
        /// Нажать отмену для всех загрузок
        /// </summary>
        protected void CancelAllExportDownload()
        {
            int numberElements = 0;
            setDriverTimeoutMinimum();
            do
            {
                IList<IWebElement> cancelList = Driver.FindElements(By.XPath(
                    GetExportNotifierXPath() + "//a[contains(@class,'js-cancel')]"));

                numberElements = cancelList.Count;
                if (numberElements > 0)
                {
                    // Закрыть верхнее (последнее)
                    cancelList[cancelList.Count - 1].Click();
                    Thread.Sleep(2000);
                }
            } while (numberElements > 0);
            setDriverTimeoutDefault();
        }

        protected void WorkWithExport()
        {
            // Дождаться появления Download в Notifier 
            WaitExportDownloadBtn();
            // Нажать Download
            ClickDownloadExportNotifier();

            // Заполнить форму для сохранения файла
            string resultPath = Path.Combine(PathTestResults, "ExportedOriginalDocuments");
            Directory.CreateDirectory(resultPath);

            resultPath = Path.Combine(resultPath,
                Path.GetFileNameWithoutExtension(DocumentFileToConfirm) + "_" + DateTime.Now.Ticks + Path.GetExtension(DocumentFileToConfirm));
            // TODO !!!!!!!!!!!!!!!!!
            







            // изменить разширение!!!! TMX

            Console.WriteLine("file: " + resultPath);
            Thread.Sleep(1000);
            SendKeys.SendWait(@"{Down}");
            Thread.Sleep(1000);
            SendKeys.SendWait(@"{Enter}");
            Thread.Sleep(3000);
            SendKeys.SendWait(resultPath);
            Thread.Sleep(3000);
            SendKeys.SendWait(@"{Enter}");
            Thread.Sleep(3000);

            Assert.IsTrue(File.Exists(resultPath),
                "Ошибка: файл не экспортировался");
        }

        protected void WaitExportDownloadBtn()
        {
            // Дождаться появления информационного окна
            Assert.IsTrue(WaitUntilDisplayElement(
                GetExportNotifierXPath()),
                "Ошибка: не появилось информационное окно об экспорте");

            // Дождаться появления кнопки Download
            Assert.IsTrue(WaitNotifierDownloadBtn(),
                "Ошибка: не появилась кнопка Download");
        }

        protected const int MAX_NUMBER_RESTART_EXPORT = 1;
        protected bool WaitNotifierDownloadBtn()
        {
            // TODO заменить id нотификатора
            string prepareXPath = ".//div[@id='export-documents-notifier']//span[contains(text(),'Preparing')]";
            string downloadXPath = ".//div[@id='export-documents-notifier']//a[contains(@class,'js-download-result')]";
            string restartXPath = ".//div[@id='export-documents-notifier']//a[@class='js-restart-task']";

            int restartCounter = 0;
            bool isExistDownloadBtn = false;

            // Дождаться, пока пропадет Prepare
            Assert.IsTrue(WaitUntilDisappearElement(prepareXPath, 40),
                    "Ошибка: сообщение Prepare висит слишком долго");
            // Проверить, появилась ли кнопка Download
            isExistDownloadBtn = IsElementDisplayed(By.XPath(downloadXPath));

            // Пробовать Restart, пока не появится Download, но не больше максимального количества попыток
            while (!isExistDownloadBtn && restartCounter < MAX_NUMBER_RESTART_EXPORT)
            {                
                // Если нет сообщения об ошибке (Restart) - вообще нет сообщений - ошибка
                Assert.IsTrue(IsElementPresent(By.XPath(restartXPath)), "Ошибка: нет ни сообщения Download, ни сообщения об ошибке");
                
                // Нажать Restart
                Driver.FindElement(By.XPath(restartXPath)).Click();

                // Проверить, что Restart начался
                bool isRestartStarted = IsElementDisplayed(By.XPath(prepareXPath))
                    || IsElementDisplayed(By.XPath(downloadXPath));

                // Нет ни сообщения о Prepare, ни download
                Assert.IsTrue(isRestartStarted, "Ошибка: после нажатия на Restart не начался экпорт заново");
                // Дождаться, пока пропадет Prepare
                Assert.IsTrue(WaitUntilDisappearElement(prepareXPath, 40),
                    "Ошибка: сообщение Prepare висит слишком долго");
                // Проверить Download
                isExistDownloadBtn = IsElementDisplayed(By.XPath(downloadXPath));
                ++restartCounter;
            }

            if (!isExistDownloadBtn)
            {
                // Если сделали N Restart - вывести ошибку, что тест дальше не может проверить функционал
                Assert.IsTrue(restartCounter < MAX_NUMBER_RESTART_EXPORT,
                    "Ошибка при экспорте! Выводится сообщение, что при экспорте произошла ошибка! Тест дальше проходить не может");
            }

            return isExistDownloadBtn;
        }

        protected void ClickDownloadExportNotifier()
        {
            Driver.FindElement(By.XPath(".//div[@id='export-documents-notifier']//a[contains(@class,'js-download-result')]")).Click();
        }

        protected void ClickCancelExportNotifier()
        {
            Driver.FindElement(By.XPath(".//div[@id='export-documents-notifier']//a[contains(@class,'js-cancel')]")).Click();
        }

        protected void SelectExportMainProjectPanel(string exportType)
        {
            //Нажать на кнопку Export
            IWebElement el = Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-export-block')]"));
            Assert.IsFalse(el.GetAttribute("class").Contains("disable"), "Ошибка: кнопка Экспорт заблокирована");
            el.Click();
            //Выбрать тип экспорта
            exportType = GetExportTypePage(exportType);
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-document-export') and contains(@data-download-type,'" + exportType + "')]//a")).Click();
        }

        protected void SelectExportProjectListRow(string exportType)
        {
            //Нажать на кнопку Export
            Driver.FindElement(By.XPath(".//tr[contains(@class,'js-project-panel')]//span[contains(@class,'js-document-export-block')]")).Click();
            //Выбрать тип экспорта
            exportType = GetExportTypePage(exportType);
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-project-panel-export') and contains(@data-download-type,'" + exportType + "')]//a")).Click();
        }

        protected string GetExportTypePage(string exportType)
        {
            string exportTypePage = "";
            switch (exportType)
            {
                case EXPORT_TYPE_SOURCE:
                    exportTypePage = "Source";
                    break;
                case EXPORT_TYPE_TMX:
                    exportTypePage = "Tmx";
                    break;
                case EXPORT_TYPE_TARGET:
                    exportTypePage = "Target";
                    break;
            }
            return exportTypePage;
        }


        protected void SelectDocumentInProjectList()
        {

            // TODO проверить
            Thread.Sleep(2000);
            Driver.FindElement(By.XPath(
                ".//tr[@class='js-project-panel']/following-sibling::tr[contains(@class, 'js-document-row')]//td[contains(@class,'checkbox')]//input"))
                .Click();
        }

        protected void ClickDocumentOpenInfo(int documentNumber)
        {
            // Кликнуть на открытие информации о документе
            string documentXPath = ".//tr[@class='js-project-panel']/following-sibling::tr[contains(@class, 'js-document-row')]["
                + documentNumber +
                "]//td[contains(@class,'openCloseCell')]";
            Assert.IsTrue(IsElementPresent(By.XPath(documentXPath)), "Ошибка: документов меньше " + documentNumber);

            Driver.FindElement(By.XPath(documentXPath)).Click();
        }

        protected void SelectExportProjectListDocumentRow(string exportType)
        {
            //Нажать на кнопку Export
            Driver.FindElement(By.XPath(".//tr[contains(@class,'js-document-panel')]//span[contains(@class,'js-document-export-block')]")).Click();
            //Выбрать тип экспорта
            exportType = GetExportTypePage(exportType);
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-document-panel-export') and contains(@data-download-type,'" + exportType + "')]//a")).Click();
        }


        

        protected string CreateCommonProjectOneDocument()
        {
            setDriverTimeoutMinimum();
            string currentProjectName = ProjectNameExportTestOneDoc;
            if (!GetIsProjectInList(currentProjectName))
            {
                CreateProject(currentProjectName, false, "", "");
                ImportDocumentProjectSettings(DocumentFileToConfirm, currentProjectName);
                AssignTask();
                OpenDocument();
                ConfirmButton();
                BackButton();
            }
            setDriverTimeoutDefault();
            return currentProjectName;
        }

        protected string CreateCommonProjectMultiDocuments()
        {
            setDriverTimeoutMinimum();
            string currentProjectName = ProjectNameExportTestMultiDoc;
            if (!GetIsProjectInList(currentProjectName))
            {
                CreateProject(currentProjectName, false, "", "");
                ImportDocumentProjectSettings(DocumentFileToConfirm, currentProjectName);
                AssignTask();
                OpenDocument();
                ConfirmButton();
                BackButton();
                ImportDocumentProjectSettings(DocumentFileToConfirm2, currentProjectName);
                AssignTask(2);
                OpenDocument(2);
                ConfirmButton();
                BackButton();
            }
            setDriverTimeoutDefault();
            return currentProjectName;
        }

        [TearDown]
        public void Teardown()
        {
            CancelAllExportDownload();
        }
    }


}
