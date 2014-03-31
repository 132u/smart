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

            Assert.True(IsElementPresent(By.XPath(".//span[text()='OK']")), "No error message");
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

            Assert.IsFalse(IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
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

            Assert.IsFalse(IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
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

            Assert.IsFalse(IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
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

            Assert.IsFalse(IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
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

        /// <summary>
        /// метод открытия настроек проекта (последнего в списке) и загрузки нового документа
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        private void ImportDocumentProjectSettings(string filePath, string projectName)
        {
            // Проверить, что проект в списке
            ClickProjectInList(projectName);

            //ждем когда окно с настройками загрузится
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-document-import')]")));
            Thread.Sleep(1000);
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-import')]")).Click();

            //ждем когда загрузится окно для загрузки документа 
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]")));
            //Процесс добавления файла
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-import-document')][2]//div[contains(@class,'js-file-input-container')]//a[contains(@class,'js-upload-btn')]")).Click();

            FillAddDocumentForm(filePath);
            // Выбрать язык назначения - Русский
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]//div[contains(@class,'js-languages-multiselect')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'ui-multiselect-menu')]")).Enabled);
            Driver.FindElement(By.XPath(
                ".//ul[contains(@class,'ui-multiselect-checkboxes')]//input[contains(@class,'ui-multiselect-input')][@value='25']")).Click();
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
            // Обновить страницу
            Driver.FindElement(By.XPath(".//a[contains(@href,'/Glossaries')]")).SendKeys(OpenQA.Selenium.Keys.F5);

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

            // Кликнуть по строке с проектом, чтобы открылась информация о нем (чтобы видно было документ)
            Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-tasks-table')]//tr//td[2]//a[@class='js-name'][text()='" + ProjectName + "']/../../../td[3]")).Click();
            Thread.Sleep(2000);
            // Выделить галочку проекта
            SelectProjectInList(ProjectName);
            // Нажать кнопку удалить
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-delete-btn')]")).Click();
            // Нажать Удалить проект
            Driver.FindElement(By.XPath(".//input[contains(@class,'js-delete-project-btn')]")).Click();
            // Обновить страницу
            Driver.FindElement(By.XPath(".//a[contains(@href,'/Glossaries')]")).SendKeys(OpenQA.Selenium.Keys.F5);
            // Проверить, что проект удалился
            Assert.IsFalse(GetIsProjectInList(ProjectName), "Ошибка: проект не удалился");

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

            //!!!!!!!!!!!!!!!! МБ не надо обновлять????
            // Обновить страницу, чтобы проект удалился
            Driver.FindElement(By.XPath(".//a[contains(@href,'/Glossaries')]")).SendKeys(OpenQA.Selenium.Keys.F5);
            // Проверить, остался ли проект в списке
            Assert.IsFalse(GetIsProjectInList(ProjectName), "Ошибка: проект не удалился");

            //создание нового проекта с именем удаленного
            FirstStepProjectWizard(ProjectName);

            // Проверить, что не появилось сообщение о существующем имени
            Assert.IsFalse(Driver.PageSource.Contains("Unique project name required"));
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
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-next')]")).Click();
            // Проверить, что появилась ошибка
            Assert.IsTrue(Driver.PageSource.Contains("Unique project name required"));
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
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-next')]")).Click();
            
            // Проверить, что невозможно перейти на следующий шаг
            Assert.IsFalse(Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'check-tm')]")).Displayed,
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
            CreateProject(projectNameForbidden, false, "");
            // Проверить, что проект не создался
            Assert.IsTrue(!GetIsProjectInList(projectNameForbidden), "Ошибка: проект с запрещенными символами в имени создался");
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
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-next')]")).Click();

            //проверяем, что появилось сообщение об ошибке около поля Name
            Assert.IsTrue(Driver.PageSource.Contains("This field is required"), "Fail - нет сообщения об ошибке при создании проекта без имени");
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
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-next')]")).Click();

            //проверяем, что появилось сообщение об ошибке около поля Name
            Assert.IsTrue(Driver.PageSource.Contains("This field is required"));
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
            Authorization();
            //Создать пустой проект          
            CreateProject(ProjectName, false, "");
            //Проверка на наличие проекта
            CheckProjectInList(ProjectName);

            //Добавление документа
            ImportDocumentProjectSettings(filePath, ProjectName);
            Driver.Navigate().GoToUrl(Url);
            Thread.Sleep(1000);

            //Назначение задачи на пользователя
            AssignTask();

            // Строчка нужного проекта
            //Driver.FindElement(By.LinkText("TestProject")).Click();//TODO заменить документ
            // переход на новую страницу
            string href = Driver.FindElement(By.LinkText("TestProject")).GetAttribute("href");
            href = href.Replace("workspace#", "");
            string resultHref = href.Substring(0, href.IndexOf("-", href.IndexOf("project"))) + "/" + href.Substring(href.IndexOf("-", href.IndexOf("project")) + 1);
            Driver.Navigate().GoToUrl(resultHref);
            /*Console.WriteLine("At");
            Thread.Sleep(5000);
            // Далее нажать на появившийся документ
            IWebElement element = Wait.Until(d => Driver.FindElement(By.XPath(
                ".//a[starts-with(@href, '/editor')]" // критерий - editor
                )));

            element.Click();

            // Дождаться загрузки страницы
            Wait.Until((d) => d.Title.Contains("Editor"));

            // Проверить, существует ли хотя бы один сегмент
            Assert.IsTrue(IsElementPresent(By.CssSelector(
                "#segments-body div table tr:nth-child(1)"
                )));*/
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
            Driver.Navigate().GoToUrl(Url);
            Thread.Sleep(1000);

            //Назначение задачи на пользователя
            AssignTask();

            // Строчка нужного проекта
            //Driver.FindElement(By.LinkText("TestProject")).Click();//TODO заменить документ
            // переход на новую страницу
            string href = Driver.FindElement(By.LinkText("TestProject")).GetAttribute("href");
            href = href.Replace("workspace#", "");
            string resultHref = href.Substring(0, href.IndexOf("-", href.IndexOf("project"))) + "/" + href.Substring(href.IndexOf("-", href.IndexOf("project")) + 1);
            Driver.Navigate().GoToUrl(resultHref);

            /*// Далее нажать на появившийся документ
            IWebElement element = Wait.Until(d => Driver.FindElement(By.XPath(
                "//a[starts-with(@href, '/editor')]" // критерий - editor
                )));
            element.Click();

            // Дождаться загрузки страницы
            Wait.Until((d) => d.Title.Contains("Editor"));

            // Убедиться, что документ не разобран на сегменты
            Assert.IsFalse(IsElementPresent(By.CssSelector(
                "#segments-body div table tr:nth-child(1)"
                )));*/
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
            BackButton();

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
            ConfirmButton();
            BackButton();

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

        // TODO: Убрать если у нас не будет кнопки back для возврата на первый шаг для отмены создания. СЕйчас реализовано, что кнопки нет, но в документации - кнопка описана.
        /// <summary>
        /// отмена создания проекта на первом шаге
        /// </summary>
        //[Test]
        public void CancelFirstTest()
        {

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

        // TODO: Кнопка back сейчас убрана.  втребованиях осталось про кнопку. убрана временно или навсегда? уточнить
        /// <summary>
        /// изменение имени проекта на новое по нажатию кнопки Back
        /// </summary>
        //[Test]
        public void ChangeProjectNameOnNew()
        {

        }

        // TODO: Кнопка back сейчас убрана.  втребованиях осталось про кнопку. убрана временно или навсегда? уточнить
        /// <summary>
        /// изменение имени проекта на существующее
        /// </summary>
        //[Test]
        public void ChangeProjectNameOnExist()
        {

        }

        // TODO: Кнопка back сейчас убрана. в требованиях осталось про кнопку. убрана временно или навсегда? уточнить
        //[Test]
        public void ChangeProjectNameOnDeleted()
        {

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
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-bd js-popup-confirm')]")).Displayed);
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-bd js-popup-confirm')]//input[@type='submit']")).Click();
        }

        private void SelectProjectInList(string ProjectNameToSelect)
        {
            // Кликнуть по галочке в строке с проектом
            Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-tasks-table')]//tr//td[2]//a[@class='js-name'][text()='" + ProjectNameToSelect + "']/../../../td[1]")).Click();
        }

        private void ClickProjectInList(string ProjectNameToClick)
        {
            // Кликнуть по названию проекта
            Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-tasks-table')]//tr//td[2]//a[@class='js-name'][text()='" + ProjectNameToClick + "']")).Click();
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

        private bool GetIsProjectInList(string ProjectNameCheck)
        {
            setDriverTimeoutMinimum();
            // Вернуть, есть ли проект в списке
            bool retVal = Driver.FindElements(By.XPath(
                ".//table[contains(@class,'js-tasks-table')]//tr//td[2]//a[@class='js-name'][text()='" + ProjectNameCheck + "']")).Count > 0;
            setDriverTimeoutDefault();
            return retVal;
        }

        private void ImportDocumentCreateProject(string documentName)
        {
            //процесс добавления файла 
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//a[contains(@class,'js-upload-btn')]")).Click();
            FillAddDocumentForm(documentName);
        }


        /// <summary>
        /// Проверка на наличие элемента на экране
        /// </summary>
        private bool IsElementPresent(By by)
        {
            try
            {
                Driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }


}
