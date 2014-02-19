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

namespace AbbyyLs.CAT.Projects.Selenium.Tests
{
           
    /// <remarks>
    /// Методы для тестирования Проектов
    /// </remarks>
   
    public class ProjectTest : BaseTest
    {
   
        private string ResultFilePath;



        public string ProjectNameCheck;
        public string DuplicateProjectName;

        public string _documentFileWrong;
        public string _ttxFile;
        public string _txtFile;
        public string _srtFile;

        public string _xliffTC10;




        public ProjectTest(string url, string workspaceUrl, string browserName)
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
        /// открытие конкретного документа 924 в акке AlenaTest несколько раз подряд
        /// </summary>
        public void OpenDocument()
        {
            string document = "http://cat-stable.abbyy-ls.com/smartcat/editor/?DocumentId=924";
            for (int i = 0; i < 100; i++)
            {

                Driver.Navigate().GoToUrl(document);
                Thread.Sleep(5000);


                bool isOk = IsElementPresent(By.XPath(".//tbody//div[contains(text(), 'Vadim Petrovich')]"));
                Assert.IsTrue(isOk);
                //if (isOk)
                //{
                    //WriteFileConsoleResults("Pass", 1);
                //}
                //else
               // {
                   // WriteFileConsoleResults("Fail", 0);
                //}
            }
        }

       
        /// <summary>
        /// метод тестирования загрузки DOC формата (неподдерживаемый формат)
        /// </summary>
        [Test]
        public void ImportWrongFileTest()
        {
            Authorization();
            //WriteFileConsoleResults("Import Wrong Format (DOC)", 2);
            
            //1 шаг - заполнение данных о проекте            
            FirstStepProjectWizard(ProjectName);

            //процесс добавления файла 
            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            FillAddDocumentForm(_documentFileWrong);
           
            Assert.True(IsElementPresent(By.XPath(".//span[text()='OK']")), "No error message");
        }
        // TODO: XLIFF
        //сделать тесты для тестирования xliff

        /// <summary>
        /// метод тестирования загрузки нескольких файлов при создании проекта (docx+ttx)
        /// </summary>
        [Test]
        public void ImportSomeFilesTest()
        {
            Authorization();
            //WriteFileConsoleResults("Upload some files - docx, ttx", 2);
                       
            FirstStepProjectWizard(ProjectName);

            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            FillAddDocumentForm(DocumentFile);

            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            FillAddDocumentForm(_ttxFile);

            Assert.IsFalse(IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
        }

        /// <summary>
        /// создание проекта без файла
        /// </summary>
        [Test]
        public void CreateProjectNoFile()
        {
            // TODO: Пересмотреть - не протестила с контрпримером чтобы возникла ошибка.
            Authorization();
            //WriteFileConsoleResults("Create Project Without Input Files Test", 2);
                                   
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
            //WriteFileConsoleResults("Upload Ttx File Test", 2);
                        
            FirstStepProjectWizard(ProjectName);

            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            FillAddDocumentForm(_ttxFile);

            Assert.IsFalse(IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
        }

        /// <summary>
        /// Импорт документа формата txt (допустимый формат)
        /// </summary>
        [Test]
        public void ImportTxtFileTest()
        {
            Authorization();
           // WriteFileConsoleResults("Upload Ttx File Test", 2);
                       
            FirstStepProjectWizard(ProjectName);

            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            FillAddDocumentForm(_txtFile);
            
            Assert.IsFalse(IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
        }

        /// <summary>
        /// Импорт документа формата Srt (допустимый формат)
        /// </summary>
        [Test]
        public void ImportSrtFileTest()
        {
            Authorization();
            //WriteFileConsoleResults("Upload Ttx File Test", 2);
            Thread.Sleep(4000);
            
            FirstStepProjectWizard(ProjectName);

            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            FillAddDocumentForm(_srtFile);
            
            Assert.IsFalse(IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
        }


        /// <summary>
        /// Импорт документа в созданный проект без файла
        /// </summary>
        [Test]
        public void ImportDocumentAfterCreationTest()
        {
            Authorization();
           // WriteFileConsoleResults("Create Project Without Input Files Test", 2);
            
            //заполнение полей на 1 шаге           
            CreateProject(ProjectName, false, "");

            Thread.Sleep(4000);
            CheckProjectInList(ProjectName);

            //открытие настроек проекта
            ImportDocumentProjectSettings();

        // TODO: Вставить проверку что документ загружен!!! (как проверить это)
        }

        /// <summary>
        /// метод открытия настроек проекта (последнего в списке) и загрузки нового документа
        /// </summary>
        private void ImportDocumentProjectSettings()
        {
            Thread.Sleep(3000);
            //сортировка по дате
            Driver.FindElement(By.XPath(".//span[text()='Created']")).Click();
            Driver.FindElement(By.XPath(".//span[text()='Created']")).Click();

            //клик на название проекта
            Driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(2) a")).Click();

            //ждем когда окно с настройками загрузится
            Wait.Until((d) => d.FindElement(By.Id("documents-import-btn")));
            Thread.Sleep(3000);
            //WaitProjectSettingUpload();

            Driver.FindElement(By.Id("documents-import-btn")).Click();

            //ждем когда загрузится окно для загрузки документа            
            Wait.Until((d) => d.FindElement(By.Id("document-wizard")));
            //Процесс добавления файла
            Driver.FindElement(By.XPath(".//div[@id='document-wizard-form-body']//span[text()='Add']")).Click();

            FillAddDocumentForm(DocumentFile);
            Driver.FindElement(By.XPath(".//div[@id='document-wizard-form']//span[contains(text(), 'Next')]")).Click();

            Thread.Sleep(4000);
            Driver.FindElement(By.XPath(".//div[@id='document-wizard-body']//div[3]//span[text()='Next']")).Click();

            Wait.Until((d) => d.FindElement(By.Id("document-wizard-workflow")));
            Driver.FindElement(By.XPath(".//div[@id='document-wizard-workflow']//span[text()='Finish']")).Click();            
        }

        /// <summary>
        /// метод проверки наличия проекта в списке проектов
        /// </summary>
        /// <param name="ProjectNameCheck">Имя проекта, которое ищем в списке проектов</param>
        private void CheckProjectInList(string ProjectNameCheck)
        {
            Driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(2) a"));

            //проверка, что проект с именем ProjectNameCheck есть на странице
            Assert.IsTrue(Driver.PageSource.Contains(ProjectNameCheck), "Проверка на наличие проекта среди созданных  - не пройдена");


            //if (Driver.PageSource.Contains(ProjectNameCheck))
            //{
               // WriteFileConsoleResults("Create Project Test Pass", 1);
            //}
            //else
            //{
                //WriteFileConsoleResults("Create Project Test Fail", 0);
            //}
        }

        /// <summary>
        /// Метод проверки удаления проекта (без файлов)
        /// </summary>
        [Test]
        public void DeleteProjectNoFileTest()
        {
            Authorization();
           // WriteFileConsoleResults("Delete Project Without File Test", 2);
                      
            //создать проект, который будем удалять
            CreateProject(ProjectName, false, "");

            Thread.Sleep(3000);

            //сортировка по дате (только что созданный оказывается сверху)
            Driver.FindElement(By.XPath(".//span[text()='Created']")).Click();
            Driver.FindElement(By.XPath(".//span[text()='Created']")).Click();

            //удалить первый проект из списка проектов
            Thread.Sleep(3000);
            Driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(1)")).Click();
            Driver.FindElement(By.Id("project-delete-btn")).Click();
            Wait.Until((d) => d.FindElement(By.Id("messagebox")));
            Driver.FindElement(By.XPath(".//div[@id='messagebox']//span[text()='Yes']")).Click();

            Assert.IsFalse(Driver.PageSource.Contains(ProjectName));
        }

        /// <summary>
        /// Метод проверки удаления проекта (с файлом)
        /// </summary>
        [Test]
        public void DeleteProjectWithFileTest()
        {
            Authorization();
           // WriteFileConsoleResults("Delete Project With File Test", 2);
                     
            CreateProject(ProjectName, true, DocumentFile);

            Thread.Sleep(3000);
            //сортировка по дате (только что созданный оказывается сверху)
            Driver.FindElement(By.XPath(".//span[text()='Created']")).Click();
            Driver.FindElement(By.XPath(".//span[text()='Created']")).Click();

            //удалить первый проект из списка проектов
            Thread.Sleep(3000);
            Driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(1)")).Click();
            Driver.FindElement(By.Id("project-delete-btn")).Click();
            Wait.Until((d) => d.FindElement(By.Id("messagebox")));
            Thread.Sleep(3000);
            Driver.FindElement(By.XPath(".//div[@id='messagebox']//span[text()='Delete project(s)']")).Click();

            Assert.IsFalse(Driver.PageSource.Contains(ProjectName));

        }
        /// <summary>
        /// тестирование совпадения имени проекта с удаленным
        /// </summary>
        [Test]
        public void CreateProjectDeletedNameTest()
        {
            Authorization();
            //WriteFileConsoleResults("Create Project with Deleted Name Test", 2);
                        
            CreateProject(ProjectName, false, "");

            Thread.Sleep(3000);
            //сортировка по дате (только что созданный оказывается сверху)
            Driver.FindElement(By.XPath(".//span[text()='Created']")).Click();
            Driver.FindElement(By.XPath(".//span[text()='Created']")).Click();

            //удалить первый проект из списка проектов
            Thread.Sleep(3000);
            Driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(1)")).Click();
            Driver.FindElement(By.Id("project-delete-btn")).Click();
            Wait.Until((d) => d.FindElement(By.Id("messagebox")));
            Thread.Sleep(3000);
            Driver.FindElement(By.XPath(".//div[@id='messagebox']//span[text()='Yes']")).Click();
            Assert.IsFalse(Driver.PageSource.Contains(ProjectName));

            //создание нового проекта с именем удаленного
            Driver.Navigate().GoToUrl(Url);
            Wait.Until((d) => d.Title.Contains("Workspace"));
            FirstStepProjectWizard(ProjectName);

            Assert.IsFalse(Driver.PageSource.Contains("Unique project name required"));
        }

 
        // TODO: где используется этот метод???
        /// <summary>
        /// метод проверки ТС-301 Импорта файлов
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        /// <param name="status">параметр проверки - 1 испорт ок; 0 импорт не ок</param>
        public bool ImportSomeFilesTest(String filePath, int status)
        {
            //открываем форму настройки
            WriteFileConsoleResults("ImportFilesTest", 2);
            Thread.Sleep(8000);
            Assert.IsTrue(Driver.FindElement(By.Id("projects-add-btn")).Displayed);
            //нажать <Create>
            Driver.FindElement(By.Id("projects-add-btn")).Click();

            //ждем загрузки формы            
            Wait.Until((d) => d.FindElement(By.Id("project-wizard")));

            //заполнение полей на 1 шаге
            Driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            

            Driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(ProjectName);

            Driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).Clear();
            Driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).SendKeys(DeadlineDate);

            //процесс добавления файла 
            //нажатие кнопки Add
            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();

            //скрипт Autolt - вызов скрипта для добавления файла
            FillAddDocumentForm(filePath);
            Thread.Sleep(6000);


            //проверка что загрузились файлы без ошибок

            switch (status)
            {
                case 0:
                    //проверка, что появилось окно об ошибке
                    //если появилось окно, то return true, иначе return false
                    return true;

                case 1:
                    //проверка что окна об ошибке не появилось
                    return true;

                default:
                    return true;
            }
        }
              
        /// <summary>
        /// метод тестирования создания проекта с существующим именем
        /// </summary>
        [Test]
        public void CreateProjectDuplicateNameTest()
        {
            Authorization();
            //WriteFileConsoleResults("Create Project Duplicate Name Test", 2);              
            CreateProject(ProjectName, false, "");
            Thread.Sleep(2000);
            //Driver.Navigate().GoToUrl(_url);
            
            Wait.Until((d) => d.FindElement(By.Id("projects-body")));
            FirstStepProjectWizard(ProjectName);            
            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Next']")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(Driver.PageSource.Contains("Unique project name required"));
        }

        /// <summary>
        /// метод проверки невозможности создания проекта в большим именем(>100 символов)
        /// </summary>
        [Test]
        public void CreateProjectBigNameTest()
        {
            Authorization();
            //WriteFileConsoleResults("Create Project with Big Name (> 100 symbols) Test", 2);           

            string bigName = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901";
            FirstStepProjectWizard(bigName);
            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Next']")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(Driver.PageSource.Contains("The maximum length for this field is 100"));
        }

        /// <summary>
        /// метод проверки на ограничение имени проекта (100 символов)
        /// </summary>
        [Test]
        public void CreateProjectLimitNameTest()
        {
            Authorization();
            //WriteFileConsoleResults("Create Project - Limit Name (100) Test", 2);
   
            string limitName = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            FirstStepProjectWizard(limitName);
            Driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Next']")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(Driver.PageSource.Contains(limitName));
        }

        //TODO пересмотреть метод!! как лучше выбирать языки
        /// <summary>
        /// метод тестирования создания проектов с одинаковыми source и target языками
        /// </summary>
        [Test]
        public void CreateProjectEqualLanguagesTest()
        {
            Authorization();
            //WriteFileConsoleResults("Create Project - Equal Languages", 2);
            
            Driver.FindElement(By.Id("projects-add-btn")).Click();
            Wait.Until((d) => d.FindElement(By.Id("project-wizard")));
           
            Driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            Driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(ProjectName);

            Driver.FindElement(By.CssSelector("input[name=\"TargetLanguages\"]")).Clear();
            Driver.FindElement(By.CssSelector("input[name=\"TargetLanguages\"]")).SendKeys("English");
            Thread.Sleep(2000);
            Driver.FindElement(By.XPath("//ul//li[contains(text(),'English')]")).Click();

            Thread.Sleep(2000);
            Driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();

            Thread.Sleep(4000);
            Assert.IsFalse(Driver.FindElement(By.Id("project-wizard-tms")).Displayed);
            //if(Driver.FindElement(By.Id("project-wizard-tms")).Displayed)
            //{
               // WriteFileConsoleResults("Test Fail", 0);
            //}
            //else
            //{
               // WriteFileConsoleResults("Test Pass", 1);
            //}
           // Thread.Sleep(3000);
        }

        /// <summary>
        /// метод для тестирования недопустимых символов в имени проекта
        /// </summary>
        [Test]
        public void CreateProjectForbiddenSymbolsTest()
        {
            Authorization();
            //WriteFileConsoleResults("Create Project - Forbidden Symbols Test", 2);
           
            string projectNameForbidden = ProjectName + " *|\\:\"<\\>?/ ";

            CreateProject(projectNameForbidden, false, "");                     
            Assert.IsFalse(Driver.PageSource.Contains(" *|\\:\"<\\>?/ "));
        }

        /// <summary>
        /// метод для тестирования проекта с пустым именем
        /// </summary>
        [Test]
        public void CreateProjectEmptyNameTest()
        {
            Authorization();
            //WriteFileConsoleResults("Create project - Empty name", 2);
            
            FirstStepProjectWizard("");

            Thread.Sleep(3000);
            Driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
            Thread.Sleep(2000);

            //проверяем, что появилось сообщение об ошибке около поля Name
            Assert.IsTrue(Driver.PageSource.Contains("This field is required"));
        }

        /// <summary>
        /// метод для тестирования создания имени проекта состоящего из одного пробела
        /// </summary>
        [Test]
        public void CreateProjectSpaceNameTest()
        {
            Authorization();
            //WriteFileConsoleResults("Create project - Empty name", 2);
           
            FirstStepProjectWizard(" ");

            Thread.Sleep(3000);
            Driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
            Thread.Sleep(2000);

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
            //WriteFileConsoleResults("Create project - Name=Space+Name", 2);
            
            string projectName = ProjectName + "  " + "SpacePlusSymbols";
            CreateProject(projectName, false, "");
            Thread.Sleep(2000);

            Assert.IsTrue(Driver.PageSource.Contains(projectName));
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
        /// ТС-10 тестирование импорта xliff из memoQ 
        /// </summary>
        [Test]
        public void TestCase10()
        {
            Authorization();
            //WriteFileConsoleResults("Import Xliff - TC 10", 2);
            
            string projectName = "TC-10 " + DateTime.UtcNow.Ticks.ToString();

            CreateProject(projectName, true, _xliffTC10);
            Thread.Sleep(2000);

            Assert.IsTrue(Driver.PageSource.Contains(projectName));
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
