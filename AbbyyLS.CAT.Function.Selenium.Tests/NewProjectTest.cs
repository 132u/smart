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

            // Заполнить форму для сохранения файла
            string resultPath = Path.Combine(PathTestResults, "ExportedOriginalDocuments");
            Directory.CreateDirectory(resultPath);

            Thread.Sleep(1000);
            SendKeys.SendWait
                (Path.Combine(resultPath, Path.GetFileNameWithoutExtension(filePath) + "_" + ProjectName + Path.GetExtension(filePath)));
            Thread.Sleep(1000);

            SendKeys.SendWait(@"{Enter}");
            Thread.Sleep(5000);
            Assert.IsTrue(File.Exists(Path.Combine
                (resultPath, Path.GetFileNameWithoutExtension(filePath) + "_" + ProjectName + Path.GetExtension(filePath))),
                "Ошибка: файл не экспортировался");
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

            // Заполнить форму для сохранения файла
            string resultPath = Path.Combine(PathTestResults, "ExportedOriginalDocuments");
            Directory.CreateDirectory(resultPath);

            Thread.Sleep(1000);
            SendKeys.SendWait
                (Path.Combine(resultPath, Path.GetFileNameWithoutExtension(filePath) + "_" + ProjectName + Path.GetExtension(filePath)));
            Thread.Sleep(1000);

            SendKeys.SendWait(@"{Enter}");
            Thread.Sleep(5000);
            Assert.IsTrue(File.Exists(Path.Combine
                (resultPath, Path.GetFileNameWithoutExtension(filePath) + "_" + ProjectName + Path.GetExtension(filePath))));
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

            // Заполнить форму для сохранения файла
            string resultPath = Path.Combine(PathTestResults, "ExportedTranslatedDocuments");
            Directory.CreateDirectory(resultPath);

            Thread.Sleep(1000);
            SendKeys.SendWait
                (Path.Combine(resultPath, Path.GetFileNameWithoutExtension(filePath) + "_" + ProjectName + Path.GetExtension(filePath)));
            Thread.Sleep(1000);

            SendKeys.SendWait(@"{Enter}");
            Thread.Sleep(5000);
            Assert.IsTrue(File.Exists(Path.Combine
                (resultPath, Path.GetFileNameWithoutExtension(filePath) + "_" + ProjectName + Path.GetExtension(filePath))));
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
            Driver.FindElement(By.XPath(".//table[contains(@class,'js-documents-table')]//tr[1]/td[1]/span")).Click();
            // Нажать Progress
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-progress')]")).Click();
            
            // Назначить ответственного в окне Progress
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-assigned-block inProgress')]//span[contains(@class,'js-assigned-cancel')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-confirm')]//input[contains(@class,'js-submit-btn')]"))).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-progress')]//div[contains(@class,'js-assigned-block notAssigned')]")).Displayed,
                "Статус назначения не изменился на notAssigned");

            Driver.FindElement(By.XPath(".//table[contains(@class,'js-progress-table')]//tr[1]//td[3]//span")).Click();
            Wait.Until(d => Driver.FindElements(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]"
                )));
            //Имя для cat-stage2
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__item')][@title='Bob Test']")).Click();
            Thread.Sleep(1000);

            // Нажать на Assign
            Wait.Until(d => Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-progress-table')]//tr[1]//td[4]//span[contains(@class,'js-assign')]"
                ))).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-assigned-block assigned')]//span[contains(@class,'js-assigned-cancel')]")).Displayed);
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

    }


}
