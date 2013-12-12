﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;


namespace AbbyyLs.CAT.Projects.Selenium.Tests
{

    /// <remarks>
    /// Методы для тестирования Проектов
    /// </remarks>
    [TestFixture("DevUrl", "DevWorkspace", "Firefox")]
    [TestFixture("StageUrl", "StageWorkspace")]
    [TestFixture("StableUrl2", "StableWorkspace2", "Firefox")]
    [TestFixture("StableUrl2", "StableWorkspace2", "Chrome")]
    [TestFixture("StableUrl2", "StableWorkspace2", "IE")]
    [TestFixture("StageUrl2", "StageWorkspace2", "Firefox")]
    public class ProjectTest
    {

        private IWebDriver _driver;
        private FirefoxProfile _profile;

        private string _login;
        private string _password;

        private string _devUrl;
        private string _stageUrl;
        private string _stableUrl;
        private string _stageUrl2;

        private string _url;
        private string _workspaceUrl;

        private string _projectName;
        private string _deadlineDate;
        private string _tmName;
        private string _documentFile;
        private string _tmxFile;

        private string ResultFilePath;



        public string ProjectNameCheck;
        public string DuplicateProjectName;

        public string _documentFileWrong;
        public string _ttxFile;
        public string _txtFile;
        public string _srtFile;

        public string _xliffTC10;

        //имена для тестов ТС-400
        private string ExistedName; //для теста - изменение имени на существующее
        private string DeletedName; //для теста - изменение имени на удаленное


        public ProjectTest(string url, string workspaceUrl, string browserName)
        {

            this._url = ConfigurationManager.AppSettings[url];
            this._workspaceUrl = ConfigurationManager.AppSettings[workspaceUrl];

            if (browserName == "Firefox")
            {
                if (_driver == null)
                {
                    _profile = new FirefoxProfile(@"C:\Users\a.kurenkova\Desktop\FirefoxProfile");
                    _driver = new FirefoxDriver(_profile);
                }
            }
            else if (browserName == "Chrome")
            {
#warning Проверить версию chromedriver
                _driver = new ChromeDriver();
            }
            else if (browserName == "IE")
            {
                //сделать запуск из ie
            }

        }

        /// <summary>
        /// Старт тестов, переменные
        /// </summary>
        [SetUp]
        public void SetupTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            _login = ConfigurationManager.AppSettings["Login"];
            _password = ConfigurationManager.AppSettings["Password"];
            _projectName = ConfigurationManager.AppSettings["ProjectName"];
            _tmName = ConfigurationManager.AppSettings["TMName"];
            _deadlineDate = ConfigurationManager.AppSettings["DeadlineDate"];

#warning Как хранить пути к файлам в вебконфиге?
            _documentFile = @"C:\Users\a.kurenkova\Desktop\Repos\CAT\CAT.FrontEnd.Tests\AbbyyLS.CAT.Projects.Selenium.Tests\bin\Debug\Scripts\English.docx";
            _tmxFile = @"\\cat-dev\Share\CAT\TestFiles\tmxEng2.tmx";

            _documentFileWrong = @"\\cat-dev\Share\CAT\TestFiles\doc98.doc";
            _ttxFile = @"\\cat-dev\Share\CAT\TestFiles\test.ttx";
            _txtFile = @"\\cat-dev\Share\CAT\TestFiles\test.txt";
            _srtFile = @"\\cat-dev\Share\CAT\TestFiles\test.srt";

            _xliffTC10 = @"\\cat-dev\Share\CAT\TestFiles\Xliff\TC-10En.xliff";

            ResultFilePath = @"\\cat-dev\Share\CAT\TestResult\Result" + DateTime.UtcNow.Ticks.ToString() + ".txt";
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
        /// метод тестирования авторизации пользователя в системе
        /// </summary>
        [Test]
        public void AutorizationTest()
        {
            WriteFileConsoleResults("Autorization Test", 2);

            _driver.Navigate().GoToUrl(_url);
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).SendKeys(_login);
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).SendKeys(_password);
            _driver.FindElement(By.CssSelector("input[type = \"submit\"]")).Click();
            Thread.Sleep(6000);

            if (IsElementPresent(By.XPath(".//select/option[contains(text(), 'TestAccount')]")))
            {
                //_driver.FindElement(By.CssSelector("select[name=\"accountId\"] option:contains('TestAccount')"));
                _driver.FindElement(By.XPath(".//select/option[contains(text(), 'TestAccount')]")).Click();
                //переходим на сайт
                _driver.FindElement(By.CssSelector("input[type = \"submit\"]")).Click();
                Thread.Sleep(6000);
                IWebElement myDynamicElement = _driver.FindElement(By.Id("projects-add-btn"));
                _driver.Navigate().GoToUrl(_workspaceUrl);


            }


            Thread.Sleep(6000);
            Assert.True(_driver.Title.Contains("Workspace"), "Ошибка: неверный заголовок страницы");
            //Assert.That(_driver.Title, Is.StringContaining("Workspace"));


        }

        /// <summary>
        /// открытие конкретного документа 924 в акке AlenaTest несколько раз подряд
        /// </summary>
        public void OpenDocument()
        {
            string document = "http://cat-stable.abbyy-ls.com/smartcat/editor/?DocumentId=924";
            for (int i = 0; i < 100; i++)
            {

                _driver.Navigate().GoToUrl(document);
                Thread.Sleep(5000);


                bool isOk = IsElementPresent(By.XPath(".//tbody//div[contains(text(), 'Vadim Petrovich')]"));
                if (isOk)
                {
                    WriteFileConsoleResults("Pass", 1);
                }
                else
                {
                    WriteFileConsoleResults("Fail", 0);
                }
            }
        }

        /// <summary>
        /// метод тестирования создания проекта со всеми данными (от начала до конца)
        /// </summary>
        [Test]
        public void CreateProjectTest()
        {
            AutorizationTest();

            WriteFileConsoleResults("Create Project Test", 2);
            Thread.Sleep(8000);
            //1 шаг - заполнение данных о проекте
            _projectName += " " + DateTime.UtcNow.Ticks.ToString();

            CreateProject(_projectName, true, _documentFile);

            //проверить что проект появился с списке проектов
            CheckProjectInList(_projectName);

            Thread.Sleep(4000);
        }

        /// <summary>
        /// метод тестирования загрузки DOC формата (неподдерживаемый формат)
        /// </summary>
        [Test]
        public void ImportWrongFileTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Import Wrong Format (DOC)", 2);
            Thread.Sleep(5000);
            //1 шаг - заполнение данных о проекте
            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            FirstStepProjectWizard(_projectName);

            //процесс добавления файла 
            //нажатие кнопки Add
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();

            //скрипт Autolt - вызов скрипта для добавления файла
            SetUploadedFile(_documentFileWrong);
            Thread.Sleep(6000);

            Assert.True(IsElementPresent(By.XPath(".//span[text()='OK']")), "No error message");
        }
#warning XLIFF
        //сделать тесты для тестирования xliff

        /// <summary>
        /// метод тестирования загрузки нескольких файлов при создании проекта (docx+ttx)
        /// </summary>
        [Test]
        public void ImportSomeFilesTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Upload some files - docx, ttx", 2);
            Thread.Sleep(5000);

            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            FirstStepProjectWizard(_projectName);

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            SetUploadedFile(_documentFile);
            Thread.Sleep(4000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            SetUploadedFile(_ttxFile);

            Assert.IsTrue(!IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
        }


        /// <summary>
        /// метод заполнения данных о проекте на 1 шаге (имя, дедлайн) 
        /// </summary>
        private void FirstStepProjectWizard(string ProjectName)
        {
            Assert.IsTrue(_driver.FindElement(By.Id("projects-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("projects-add-btn")).Click();

            //ждем загрузки формы
            WaitProjectFormUpload();

            //заполнение полей на 1 шаге
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();


            //DuplicateProjectName += ProjectNameCheck;
            //DeletedName += ProjectNameCheck;

            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(ProjectName);

            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).SendKeys(_deadlineDate);
        }

        /// <summary>
        /// создание проекта без файла
        /// </summary>
        [Test]
        public void CreateProjectNoFile()
        {
#warning Пересмотреть - не протестила с контрпримером чтобы возникла ошибка.
            AutorizationTest();
            WriteFileConsoleResults("Create Project Without Input Files Test", 2);
            Thread.Sleep(5000);
            //заполнение полей на 1 шаге
            _projectName += " " + DateTime.UtcNow.Ticks.ToString();

            CreateProject(_projectName, false, "");

            //проверить что проект появился с списке проектов
            CheckProjectInList(_projectName);


        }
        /// <summary>
        /// метод создания проекта от начала (1 шаг) до конца (шаг pretranslate)
        /// </summary>
        /// <param name="ProjectName">Имя проекта</param>
        /// <param name="FileFlag">Наличие файла (true-проект с файлом)(</param>
        public void CreateProject(string ProjectName, bool FileFlag, string DocumentName)
        {
            FirstStepProjectWizard(ProjectName);

            if (FileFlag == true)
            {
                //процесс добавления файла 
                //нажатие кнопки Add
                _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();

                //скрипт Autolt - вызов скрипта для добавления файла
                SetUploadedFile(DocumentName);
                Thread.Sleep(2000);
                WriteFileConsoleResults("Upload file finish", 2);

                Thread.Sleep(500);
            }

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();

            //2 шаг - выбор ТМ
            WaitTMUpload();
            _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1) td:nth-child(2)")).Click();
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//span[contains(text(), 'Next')]")).Click();
            //3 шаг - выбор МТ
            WaitMTUpload();
            Thread.Sleep(500);
            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")).Click();
            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(4) td:nth-child(1)")).Click();
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
            //4 шаг - выбор глоссария
            WaitTBUpload();
            if (IsElementPresent(By.CssSelector("#project-wizard-tbs table tr:nth-child(1) td:nth-child(1)")))
            {
                _driver.FindElement(By.CssSelector("#project-wizard-tbs table tr:nth-child(1) td:nth-child(1)")).Click();
            }
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tbs']//span[contains(text(), 'Next')]")).Click();
            //5 шаг - настройка этапов workflow
            WaitWorkflowStageUpload();
                     
            _driver.FindElement(By.Id("project-workflow-new-stage-btn")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-workflow']//span[contains(text(), 'Next')]")).Click();
            //Finish
            WaitPretranslateUpload();
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-pretranslate']//span[contains(text(), 'Finish')]")).Click();

        }

        /// <summary>
        /// Импорт документа формата ttx (допустимый формат)
        /// </summary>
        [Test]
        public void ImportTtxFileTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Upload Ttx File Test", 2);
            Thread.Sleep(4000);

            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            FirstStepProjectWizard(_projectName);

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            SetUploadedFile(_ttxFile);
            Thread.Sleep(4000);
            Assert.IsTrue(!IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
        }
        /// <summary>
        /// Импорт документа формата txt (допустимый формат)
        /// </summary>
        [Test]
        public void ImportTxtFileTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Upload Ttx File Test", 2);
            Thread.Sleep(4000);

            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            FirstStepProjectWizard(_projectName);

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            SetUploadedFile(_txtFile);
            Thread.Sleep(4000);
            Assert.IsTrue(!IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
        }
        /// <summary>
        /// Импорт документа формата Srt (допустимый формат)
        /// </summary>
        [Test]
        public void ImportSrtFileTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Upload Ttx File Test", 2);
            Thread.Sleep(4000);

            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            FirstStepProjectWizard(_projectName);

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();
            SetUploadedFile(_srtFile);
            Thread.Sleep(4000);
            Assert.IsTrue(!IsElementPresent(By.XPath(".//span[text()='OK']")), "Сообщение об ошибке о загрузке неверного формата");
        }


        /// <summary>
        /// Импорт документа в созданный проект без файла
        /// </summary>
        [Test]
        public void ImportDocumentAfterCreationTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create Project Without Input Files Test", 2);
            Thread.Sleep(3000);
            //заполнение полей на 1 шаге
            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            CreateProject(_projectName, false, "");

            Thread.Sleep(4000);
            CheckProjectInList(_projectName);

            //открытие настроек проекта
            ImportDocumentProjectSettings();

#warning Вставить проверку что документ загружен!!! (как проверить это)

        }
        /// <summary>
        /// метод открытия настроек проекта (последнего в списке) и загрузки нового документа
        /// </summary>
        private void ImportDocumentProjectSettings()
        {
            Thread.Sleep(3000);
            //сортировка по дате
            _driver.FindElement(By.XPath(".//span[text()='Created']")).Click();
            _driver.FindElement(By.XPath(".//span[text()='Created']")).Click();

            //клик на название проекта
            _driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(2) a")).Click();

            Thread.Sleep(3000);

            //_driver.FindElement(By.Id("projects-edit-btn")).Click();
            //ждем когда окно с настройками загрузится
            WaitProjectSettingUpload();

            Thread.Sleep(4000);

            _driver.FindElement(By.Id("documents-import-btn")).Click();

            //ждем когда загрузится окно для загрузки документа
            WaitImportDocumentFormUpload();
            _driver.FindElement(By.XPath(".//div[@id='document-wizard-form-body']//span[text()='Add']")).Click();

            //скрипт Autolt - вызов скрипта для добавления файла
            SetUploadedFile(_documentFile);
            Thread.Sleep(6000);

            _driver.FindElement(By.XPath(".//div[@id='document-wizard-form']//span[text()='Next']")).Click();

            Thread.Sleep(4000);
            _driver.FindElement(By.XPath(".//div[@id='document-wizard-body']//div[3]//span[text()='Next']")).Click();
            WaitDocumentWizardWorkflowFormUpload();
            _driver.FindElement(By.XPath(".//div[@id='document-wizard-workflow']//span[text()='Finish']")).Click();

        }

        /// <summary>
        /// метод проверки наличия проекта в списке проектов
        /// </summary>
        /// <param name="ProjectNameCheck">Имя проекта, которое ищем в списке проектов</param>
        private void CheckProjectInList(string ProjectNameCheck)
        {
            _driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(2) a"));

            //проверка, что проект с именем ProjectNameCheck есть на странице
            Assert.IsTrue(_driver.PageSource.Contains(ProjectNameCheck), "Проверка на наличие проекта среди созданных  - не пройдена");


            if (_driver.PageSource.Contains(ProjectNameCheck))
            {
                WriteFileConsoleResults("Create Project Test Pass", 1);
            }
            else
            {
                WriteFileConsoleResults("Create Project Test Fail", 0);
            }
        }

        /// <summary>
        /// метод ожидания загрузки окна для импорта документа
        /// </summary>
        private void WaitImportDocumentFormUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");
                if (IsElementPresent(By.Id("document-wizard")))
                {
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Form Document Import is not opened", 0);
                    break;
                }
            }
        }
        /// <summary>
        /// метод ожидания загрузки wf при импорте документа 
        /// </summary>
        private void WaitDocumentWizardWorkflowFormUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");
                if (IsElementPresent(By.Id("document-wizard-workflow")))
                {
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Error during import file - WF form is not opened", 0);
                    break;
                }
            }

        }
        /// <summary>
        /// метод ожидания загрузки окна с настройками (project setting)
        /// </summary>
        private void WaitProjectSettingUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");
                //ждем когда кнопка Import появится
                if (IsElementPresent(By.Id("documents-import-btn")))
                {
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Form Project-Setting-Page is not opened", 0);
                    break;
                }
            }
        }
        /// <summary>
        /// метод ожидания загрузки Глоссариев в визарде
        /// </summary>
        private void WaitTBUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");

                if (IsElementPresent(By.Id("project-wizard-tbs")))
                {
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Form Project-Wizard-TBs is not opened", 0);
                    break;
                }
            }
        }
        /// <summary>
        /// метод ожидания загрузки окна workflow в визарде
        /// </summary>
        private void WaitWorkflowStageUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");

                if (IsElementPresent(By.Id("project-wizard-pretranslate")))
                {
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Form Project-Wizard-Workflow is not opened", 0);
                    break;
                }

            }

        }
        /// <summary>
        /// метод ожидания загрузки окна pretranslate в визарде 
        /// </summary>
        private void WaitPretranslateUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");
                if (IsElementPresent(By.Id("project-wizard-pretranslate")))
                {
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Form Project-Wizard-Pretranslate is not opened", 0);
                    break;
                }
            }
        }
        /// <summary>
        /// метод ожидания загрузки МТ баз в визарде
        /// </summary>
        private void WaitMTUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");

                if (IsElementPresent(By.Id("project-wizard-mts")))
                {
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Form Project-Wizard-MTs is not opened", 0);
                    break;
                }
            }
        }
        /// <summary>
        /// метод ожидания загрузки ТМ в визарде
        /// </summary>
        private void WaitTMUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");

                if (IsElementPresent(By.Id("project-wizard-tms")))
                {
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Form Project-Wizard-TMs is not opened", 0);
                    break;
                }
            }
        }
        /// <summary>
        /// ожидание загрузки окна настройки проектов (визард)
        /// </summary>
        private void WaitProjectFormUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");
                if (IsElementPresent(By.Id("project-wizard")))
                {
                    WriteFileConsoleResults("Form Project-Wizard is opened", 1);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Form Project-Wizard is not opened", 0);
                    break;
                }
            }
        }
        /// <summary>
        /// ожидание загрузки окна messagebox
        /// </summary>
        private void WaitMessageboxUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");
                if (IsElementPresent(By.Id("messagebox")))
                {
                    WriteFileConsoleResults("MessageBox is opened", 1);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("MessageBox is not opened", 0);
                    break;
                }
            }
        }

        /// <summary>
        /// Метод проверки удаления проекта (без файлов)
        /// </summary>
        [Test]
        public void DeleteProjectNoFileTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Delete Project Without File Test", 2);
            WaitWorkspacePageUpload();
            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            //создать проект, который будем удалять
            CreateProject(_projectName, false, "");

            Thread.Sleep(3000);

            //сортировка по дате (только что созданный оказывается сверху)
            _driver.FindElement(By.XPath(".//span[text()='Created']")).Click();
            _driver.FindElement(By.XPath(".//span[text()='Created']")).Click();

            //удалить первый проект из списка проектов
            Thread.Sleep(3000);
            _driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("project-delete-btn")).Click();
            WaitMessageboxUpload();
            _driver.FindElement(By.XPath(".//div[@id='messagebox']//span[text()='Yes']")).Click();

            Assert.IsFalse(_driver.PageSource.Contains(_projectName));
        }

        /// <summary>
        /// Метод проверки удаления проекта (с файлом)
        /// </summary>
        [Test]
        public void DeleteProjectWithFileTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Delete Project With File Test", 2);
            WaitWorkspacePageUpload();
            _projectName += " " + DateTime.UtcNow.Ticks.ToString();

            CreateProject(_projectName, true, _documentFile);

            Thread.Sleep(3000);
            //сортировка по дате (только что созданный оказывается сверху)
            _driver.FindElement(By.XPath(".//span[text()='Created']")).Click();
            _driver.FindElement(By.XPath(".//span[text()='Created']")).Click();

            //удалить первый проект из списка проектов
            Thread.Sleep(3000);
            _driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("project-delete-btn")).Click();
            WaitMessageboxUpload();
            Thread.Sleep(3000);
            _driver.FindElement(By.XPath(".//div[@id='messagebox']//span[text()='Yes']")).Click();

            Assert.IsFalse(_driver.PageSource.Contains(_projectName));

        }
        /// <summary>
        /// тестирование совпадения имени проекта с удаленным
        /// </summary>
        [Test]
        public void CreateProjectDeletedNameTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create Project with Deleted Name Test", 2);
            WaitWorkspacePageUpload();

            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            CreateProject(_projectName, false, "");

            Thread.Sleep(3000);
            //сортировка по дате (только что созданный оказывается сверху)
            _driver.FindElement(By.XPath(".//span[text()='Created']")).Click();
            _driver.FindElement(By.XPath(".//span[text()='Created']")).Click();

            //удалить первый проект из списка проектов
            Thread.Sleep(3000);
            _driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("project-delete-btn")).Click();
            WaitMessageboxUpload();
            Thread.Sleep(3000);
            _driver.FindElement(By.XPath(".//div[@id='messagebox']//span[text()='Yes']")).Click();
            Assert.IsFalse(_driver.PageSource.Contains(_projectName));
            //создание нового проекта с именем удаленного
            _driver.Navigate().GoToUrl(_url);
            WaitWorkspacePageUpload();
            FirstStepProjectWizard(_projectName);

            Assert.IsFalse(_driver.PageSource.Contains("Unique project name required"));
        }

        /// <summary>
        /// Метод проверки создания ТМ
        /// </summary>
        [Test]
        public void CreateTMTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create TM Test", 2);
            WaitWorkspacePageUpload();
            Thread.Sleep(3000);

            Assert.IsTrue(_driver.FindElement(By.Id("projects-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("projects-add-btn")).Click();

            //ждем загрузки формы
            WaitProjectFormUpload();

            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            _tmName += " " + DateTime.UtcNow.Ticks.ToString();

            FirstStepProjectWizard(_projectName);

            //процесс добавления файла 
            //нажатие кнопки Add
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();

            //скрипт Autolt - вызов скрипта для добавления файла
            SetUploadedFile(_documentFile);
            WriteFileConsoleResults("Upload file finish", 2);

            Thread.Sleep(500);

            //кнопка Next
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();

            //проверка что ТМ появились
            WaitTMUpload();

            //создание новой ТМ
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//span[text()='Create']")).Click();
            //ппроверка что появилась форма создания ТМ
            WaitTMCreateFormUpload();

            //заполняем данные о новой ТМ
            _driver.FindElement(By.CssSelector("#project-wizard-tm input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("#project-wizard-tm input[name=\"Name\"]")).SendKeys(_tmName);

            //добавить тмх файл
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tm']//span[text()='Add']")).Click();

            SetUploadedFile(_tmxFile);
            Thread.Sleep(6000);

            //подтвердить создание
            _driver.FindElement(By.XPath("//div[@id='project-wizard-tm']//span[text()='Accept']")).Click();

            WaitTMUpload();

            Assert.IsTrue(_driver.PageSource.Contains(_tmName));

        }
#warning где используется этот метод???
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
            Assert.IsTrue(_driver.FindElement(By.Id("projects-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("projects-add-btn")).Click();

            //ждем загрузки формы
            WaitProjectFormUpload();

            //заполнение полей на 1 шаге
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            ProjectNameCheck = _projectName + " " + DateTime.UtcNow.Ticks.ToString();

            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(ProjectNameCheck);

            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).SendKeys(_deadlineDate);

            //процесс добавления файла 
            //нажатие кнопки Add
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();

            //скрипт Autolt - вызов скрипта для добавления файла
            SetUploadedFile(filePath);
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
        /// ожидание пока загрузится форма с ТМ
        /// </summary>
        private void WaitTMCreateFormUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 4000)
                    Assert.Fail("timeout");

                if (IsElementPresent(By.Id("project-wizard-tm")))
                {
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    WriteFileConsoleResults("Form Project-Wizard-TM is not opened", 0);
                    break;
                }
            }
        }

        /// <summary>
        /// вызов скрипта autoit v3 в тестах
        /// </summary>
        /// <param name="filepath">ссылка на документ, загружаемый в проект</param>
        public void SetUploadedFile(String filePath)
        {
            //путь к скрипту
            //FileInfo autoIt = new FileInfo("../Scripts/upload_file.exe");
            try
            {
                //запуск скрипта 
                Process p = new Process();
#warning Скрипт загрузки файла переместить в папку с проектом и указать отнсоительный путь
                p.StartInfo.FileName = @"C:\Users\a.kurenkova\Desktop\Repos\CAT\CAT.FrontEnd.Tests\AbbyyLS.CAT.Projects.Selenium.Tests\bin\Debug\Scripts\upload_file.exe";
                p.StartInfo.Arguments = filePath;
                p.Start();
            }
            catch (IOException e)
            {
                FailConsoleWrite(e.StackTrace);
            }
            catch (ThreadInterruptedException e)
            {
                FailConsoleWrite(e.StackTrace);
            }
        }
        /// <summary>
        /// метод ожидания загрузки стартовой workspace страницы
        /// </summary>
        private void WaitWorkspacePageUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 10000)
                    Assert.Fail("timeout");

                if (IsElementPresent(By.Id("projects-body")))
                {
                    Thread.Sleep(6000);
                    break;
                }
                else
                {
                    WriteStringIntoFile("Workspace Page is not loaded");
                    FailConsoleWrite("Workspace Page is not loaded");
                    break;
                }
            }
        }
        /// <summary>
        /// метод тестирования создания проекта с существующим именем
        /// </summary>
        [Test]
        public void CreateProjectDuplicateNameTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create Project Duplicate Name Test", 2);
            Thread.Sleep(2000);
            WaitWorkspacePageUpload();
            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            CreateProject(_projectName, false, "");
            Thread.Sleep(2000);
            //_driver.Navigate().GoToUrl(_url);
            WaitWorkspacePageUpload();
            FirstStepProjectWizard(_projectName);
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.PageSource.Contains("Unique project name required"));
        }
        /// <summary>
        /// метод проверки невозможности создания проекта в большим именем(>100 символов)
        /// </summary>
        [Test]
        public void CreateProjectBigNameTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create Project with Big Name (> 100 symbols) Test", 2);
            WaitWorkspacePageUpload();

            string bigName = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901";
            FirstStepProjectWizard(bigName);
            Thread.Sleep(2000);
            Assert.IsTrue(_driver.PageSource.Contains("The maximum length for this field is 100"));
        }
        /// <summary>
        /// метод проверки на ограничение имени проекта (100 символов)
        /// </summary>
        [Test]
        public void CreateProjectLimitNameTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create Project - Limit Name (100) Test", 2);
            WaitWorkspacePageUpload();

            string limitName = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            CreateProject(limitName, false, "");
            Assert.IsTrue(_driver.PageSource.Contains(limitName));

        }
#warning пересмотреть метод!! как лучше выбирать языки
        /// <summary>
        /// метод тестирования создания проектов с одинаковыми source и target языками
        /// </summary>
        [Test]
        public void CreateProjectEqualLanguagesTest()
        {
            WriteFileConsoleResults("Create Project - Equal Languages", 2);
            Thread.Sleep(4000);
            Assert.IsTrue(_driver.FindElement(By.Id("projects-add-btn")).Displayed);
            _driver.FindElement(By.Id("projects-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(_projectName + " " + DateTime.UtcNow.Ticks.ToString());

            _driver.FindElement(By.CssSelector("input[name=\"TargetLanguages\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"TargetLanguages\"]")).SendKeys("English");
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath("//ul//li[contains(text(),'English')]")).Click();

            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();

            Thread.Sleep(4000);
            if (_driver.FindElement(By.Id("project-wizard-tms")).Displayed)
            {
                WriteFileConsoleResults("Test Fail", 0);
            }
            else
            {
                WriteFileConsoleResults("Test Pass", 1);
            }
            Thread.Sleep(3000);
        }

        /// <summary>
        /// метод для тестирования недопустимых символов в имени проекта
        /// </summary>
        [Test]
        public void CreateProjectForbiddenSymbolsTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create Project - Forbidden Symbols Test", 2);
            WaitWorkspacePageUpload();

            _projectName += " *|\\:\"<\\>?/ " + DateTime.UtcNow.Ticks.ToString();
            CreateProject(_projectName, false, "");
            Assert.IsFalse(_driver.PageSource.Contains(" *|\\:\"<\\>?/ "));

        }
        /// <summary>
        /// метод для тестирования проекта с пустым именем
        /// </summary>
        [Test]
        public void CreateProjectEmptyNameTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create project - Empty name", 2);
            WaitWorkspacePageUpload();

            FirstStepProjectWizard("");

            Thread.Sleep(3000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
            Thread.Sleep(2000);
            //проверяем, что появилось сообщение об ошибке около поля Name
            Assert.IsTrue(_driver.PageSource.Contains("This field is required"));
        }
        /// <summary>
        /// метод для тестирования создания имени проекта состоящего из одного пробела
        /// </summary>
        [Test]
        public void CreateProjectSpaceNameTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create project - Empty name", 2);
            WaitWorkspacePageUpload();

            FirstStepProjectWizard(" ");

            Thread.Sleep(3000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
            Thread.Sleep(2000);
            //проверяем, что появилось сообщение об ошибке около поля Name
            Assert.IsTrue(_driver.PageSource.Contains("This field is required"));
        }
        /// <summary>
        /// метод тестирования создания проекта с именем содержащим пробелы
        /// </summary>
        [Test]
        public void CreateProjectSpacePlusSymbolsNameTest()
        {
            AutorizationTest();
            WriteFileConsoleResults("Create project - Name=Space+Name", 2);
            WaitWorkspacePageUpload();
            _projectName += "  " + DateTime.UtcNow.Ticks.ToString();
            CreateProject(_projectName, false, "");
            Thread.Sleep(2000);
            Assert.IsTrue(_driver.PageSource.Contains(_projectName));
        }

#warning Убрать если у нас не будет кнопки back для возврата на первый шаг для отмены создания. СЕйчас реализовано, что кнопки нет, но в документации - кнопка описана.
        /// <summary>
        /// отмена создания проекта на первом шаге
        /// </summary>
        //[Test]
        public void CancelFirstTest()
        {

        }
#warning Убрать если у нас не будет кнопки back для возврата на первый шаг для отмены создания. СЕйчас реализовано, что кнопки нет, но в документации - кнопка описана.
        /// <summary>
        /// отмена создания проекта(подтверждение отмены)
        /// </summary>
        //[Test]
        public void CancelYesTest()
        {

        }
#warning Убрать если у нас не будет кнопки back для возврата на первый шаг для отмены создания. СЕйчас реализовано, что кнопки нет, но в документации - кнопка описана.
        /// <summary>
        /// отмена создания проекта - No 
        /// </summary>
        //[Test]
        public void CancelNoTest()
        {

        }
#warning Кнопка back сейчас убрана.  втребованиях осталось про кнопку. убрана временно или навсегда? уточнить
        /// <summary>
        /// изменение имени проекта на новое по нажатию кнопки Back
        /// </summary>
        //[Test]
        public void ChangeProjectNameOnNew()
        {

        }
#warning Кнопка back сейчас убрана.  втребованиях осталось про кнопку. убрана временно или навсегда? уточнить
        /// <summary>
        /// изменение имени проекта на существующее
        /// </summary>
        //[Test]
        public void ChangeProjectNameOnExist()
        {

        }
#warning Кнопка back сейчас убрана. в требованиях осталось про кнопку. убрана временно или навсегда? уточнить
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
            AutorizationTest();
            WriteFileConsoleResults("Import Xliff - TC 10", 2);
            WaitWorkspacePageUpload();
            _projectName = "TC-10 " + DateTime.UtcNow.Ticks.ToString();

            CreateProject(_projectName, true, _xliffTC10);
            Thread.Sleep(2000);


            Assert.IsTrue(_driver.PageSource.Contains(_projectName));

        }



        /// <summary>
        /// Проверка на наличие элемента на экране
        /// </summary>
        private bool IsElementPresent(By by)
        {
            try
            {
                _driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        /// <summary>
        /// Завершение тестов
        /// </summary>
        [TearDown]
        public void TeardownTest()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception)
            {

            }
        }
    }
}
