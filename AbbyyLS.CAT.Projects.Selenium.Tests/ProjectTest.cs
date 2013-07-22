using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;


namespace AbbyyLs.CAT.Projects.Selenium.Tests
{

    /// <remarks>
    /// Методы для тестирования Проектов
    /// </remarks>
    [TestFixture]
    public class ProjectTest
    {

        private IWebDriver _driver;
        private string _login;
        private string _password;
        private string _devUrl;
        private string _stageUrl;
        private string _stableUrl;
        private string _projectName;
        private string _deadlineDate;
        private string _documentFile;
        private string _tmxFile;
        private string Path;
        private string _workspaceUrl;
        public string ProjectNameCheck;
        public string DuplicateProjectName;
        private string _tmName;


        /// <summary>
        /// Старт тестов, переменные
        /// </summary>
        [SetUp]
        public void SetupTest()
        {
            _driver = new FirefoxDriver();
            _login = ConfigurationManager.AppSettings["Login"];
            _password = ConfigurationManager.AppSettings["Password"];
            _devUrl = ConfigurationManager.AppSettings["DevUrl"];
            _stageUrl = ConfigurationManager.AppSettings["StageUrl"];
            _stableUrl = ConfigurationManager.AppSettings["StableUrl"];
            _projectName = ConfigurationManager.AppSettings["ProjectName"];
            _deadlineDate = "03.03.2014";
#warning Как хранить пути к файлам в вебконфиге?
            _documentFile = @"\\cat-dev\Share\CAT\TestFiles\TextEng2.docx";
            _tmxFile = @"\\cat-dev\Share\CAT\TestFiles\tmxEng2.tmx";
            Path = @"\\cat-dev\Share\CAT\TestResult\Result" + DateTime.UtcNow.Ticks.ToString() + ".txt";
            _workspaceUrl = "https://testalena.cat-dev.perevedem.local/smartcat/workspace/";
            _tmName = ConfigurationManager.AppSettings["TMName"];

        }



        /// <summary>
        /// Метод создания файла для записи результатов тестирования
        /// PS. Вызвать этот метод из главного при запуске тестов в начале!!
        /// </summary>
        public void CreateResultFile()
        {
            FileInfo fi = new FileInfo(Path);
            StreamWriter sw = fi.CreateText();
            sw.WriteLine("Test Results");
            sw.Close();


        }

        /// <summary>
        /// Метод для записи результатов тестирования в файл
        /// </summary>
        /// <param name="s">Строка, записываемая в файл</param>
        public void WriteStringIntoFile(string s)
        {

            StreamWriter sw = new StreamWriter(Path, true);
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
            StreamWriter sw = new StreamWriter(Path, true);

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
        /// метод тестирования авторизации
        /// </summary>
        [Test]
        public void AutorizationTest()
        {
            WriteFileConsoleResults("Autorization Test", 2);

            _driver.Navigate().GoToUrl(_stableUrl);
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).SendKeys(_login);
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).SendKeys(_password);
            _driver.FindElement(By.CssSelector("input[type = \"submit\"]")).Click();
            Thread.Sleep(6000);
            if (_driver.FindElement(By.XPath(".//select/option[contains(text(), 'TestAlena')]")).Displayed)
            {
                WriteFileConsoleResults("Login and password are correct", 1);
            }
            else
            {
                WriteFileConsoleResults("Login and password are wrong", 0);
            }

            //выбрать корп акк - TestAlena
            //_driver.FindElement(By.CssSelector("select[name=\"accountId\"] option:contains('TestAlena')"));

            _driver.FindElement(By.XPath(".//select/option[contains(text(), 'TestAlena')]")).Click();
            WriteFileConsoleResults("Corp account is choosen", 2);
            //переходим на сайт
            _driver.FindElement(By.CssSelector("input[type = \"submit\"]")).Click();

            //проверяем что перешли на страницу новую
            //Assert.AreEqual("https://login.cat-dev.perevedem.local/smartcat/workspace/", _driver.Url);

            WriteFileConsoleResults("Page is opened\n  " + _driver.Url, 1);
            WriteFileConsoleResults("Autorization Test Pass", 1);
        }

        /// <summary>
        /// метод тестирования создания проекта со всеми данными 
        /// </summary>
        [Test]
        public void CreateProjectTest()
        {

            WriteFileConsoleResults("Create Project Test", 2);
            Thread.Sleep(8000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("project-add-btn")).Click();

            //ждем загрузки формы
            WaitProjectFormUpload();

            //заполнение полей на 1 шаге
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            ProjectNameCheck = _projectName + " " + DateTime.UtcNow.Ticks.ToString();
            DuplicateProjectName += ProjectNameCheck;
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(ProjectNameCheck);

            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).SendKeys(_deadlineDate);

            //процесс добавления файла 
            //нажатие кнопки Add
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();

            //скрипт Autolt - вызов скрипта для добавления файла
            SetUploadedFile(_documentFile);
            Thread.Sleep(6000);

            //кнопка Next
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();


            //2 шаг - выбор ТМ
            //выбор из существующих
            //проверка что ТМ появились
            WaitTMUpload();

            //выбор 1 из списка
            _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1) td:nth-child(2)")).Click();

            //кнопка Next
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//span[contains(text(), 'Next')]")).Click();

            //3 шаг - выбор МТ
            //проверка что МT появились
            WaitMTUpload();


            //compreno
            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(3) td:nth-child(1)")).Click();
            //moses
            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(5) td:nth-child(1)")).Click();

            //кнопка Next
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();

            //4 шаг - выбор глоссария
            //проверка что ТB появились
            WaitTBUpload();

            _driver.FindElement(By.CssSelector("#project-wizard-tbs table tr:nth-child(1) td:nth-child(1)")).Click();

            //Finish
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tbs']//span[contains(text(), 'Finish')]")).Click();

            //проверить что проект появился с списке проектов
            CheckProjectInList();

            Thread.Sleep(4000);
        }
        /// <summary>
        /// создание проекта без файла
        /// </summary>
        public void CreateProjectNoFile()
        {
            WriteFileConsoleResults("Create Project Without Input Files Test", 2);
            Thread.Sleep(8000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("project-add-btn")).Click();

            //ждем загрузки формы
            WaitProjectFormUpload();

            //заполнение полей на 1 шаге
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(_projectName + " " + DateTime.UtcNow.Ticks.ToString());

            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).SendKeys(_deadlineDate);

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();

            Thread.Sleep(6000);

            if (_driver.FindElement(By.Id("project-wizard-tms")).Displayed)
            {
                WriteFileConsoleResults("Test Pass", 1);
            }
            else
            {
                WriteFileConsoleResults("Test Fail", 0);
            }

        }

        private void CheckProjectInList()
        {
            _driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(2) a"));

            //проверка, что проект с именем ProjectNameCheck есть на странице
            Assert.IsTrue(_driver.PageSource.Contains(ProjectNameCheck));
            //PassConsoleWrite("Create Project Test Pass");
            //WriteStringIntoFile("Create Project Test Pass");

            if (_driver.PageSource.Contains(ProjectNameCheck))
            {
                WriteFileConsoleResults("Create Project Test Pass", 1);
            }
            else
            {
                WriteFileConsoleResults("Create Project Test Fail", 0);
            }
        }

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

#warning надо получить название проекта из списка проектов!!!!! для проверки создания

        /// <summary>
        /// Метод проверки удаления проекта
        /// </summary>
        [Test]
        public void DeleteProjectTest()
        {
            WriteFileConsoleResults("Delete Project Test", 2);

            WaitWorkspacePageUpload();

            Thread.Sleep(6000);
            _driver.FindElement(By.CssSelector("#projects-body table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("project-delete-btn")).Click();
            Thread.Sleep(10000);
            _driver.FindElement(By.XPath(".//span[text()='Yes']")).Click();

            //проверить что удалено, что проект бывший последним не совпадает с тем что сейчас после удаления

            Assert.IsFalse(_driver.PageSource.Contains(ProjectNameCheck));

            if (!_driver.PageSource.Contains(ProjectNameCheck))
            {
                WriteFileConsoleResults("Delete Project Test Pass", 1);
            }
            else
            {
                WriteFileConsoleResults("Delete Project Test Fail", 2);
            }

        }
        /// <summary>
        /// тестирование совпадения имени проекта с удаленным
        /// </summary>
        public void CreateProjectDeletedNameTest()
        {
            WriteFileConsoleResults("Create Project with Deleted Name Test", 2);
            Thread.Sleep(4000);

            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);

            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            //вводим большое имя проекта
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(ProjectNameCheck);

            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
            Thread.Sleep(4000);

            if (_driver.FindElement(By.Id("project-wizard-tms")).Displayed)
            {
                WriteFileConsoleResults("Test Pass", 1);
            }
            else
            {
                WriteFileConsoleResults("Test Fail", 0);
            }

        }
        /// <summary>
        /// Метод проверки создания ТМ
        /// </summary>
        [Test]
        public void CreateTMTest()
        {
            WriteFileConsoleResults("Create TM Test", 2);
            WaitWorkspacePageUpload();
            Thread.Sleep(6000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("project-add-btn")).Click();

            //ждем загрузки формы
            WaitProjectFormUpload();

            //заполнение полей на 1 шаге
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(_projectName + " " + DateTime.UtcNow.Ticks.ToString());

            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).SendKeys(_deadlineDate);

            //процесс добавления файла 
            //нажатие кнопки Add
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();

            //скрипт Autolt - вызов скрипта для добавления файла
            SetUploadedFile(_documentFile);
            Thread.Sleep(6000);

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
            _driver.FindElement(By.CssSelector("#project-wizard-tm input[name=\"Name\"]")).SendKeys(_tmName + " " + DateTime.UtcNow.Ticks.ToString());

            //добавить тмх файл
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tm']//span[text()='Add']")).Click();

            SetUploadedFile(_tmxFile);
            Thread.Sleep(6000);

            //подтвердить создание
            _driver.FindElement(By.XPath("//div[@id='project-wizard-tm']//span[text()='Accept']")).Click();

            WaitTMUpload();

            //проверить что ТМ появилась в списке
            //дойти создание проекта до конца и посмотреть имя в настройках

        }
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
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("project-add-btn")).Click();

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

                p.StartInfo.FileName = @"\\cat-dev\Share\CAT\TestFiles\upload_file.exe ";
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

        private void WaitWorkspacePageUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 20000)
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
        /// метод тестирование создания проекта с существующим именем
        /// </summary>
        /// запускать перед удалением
        public void CreateProjectDuplicateNameTest()
        {
            WriteFileConsoleResults("Create Project Duplicate Name Test", 2);
            Thread.Sleep(8000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("project-add-btn")).Click();

            //ждем загрузки формы
            WaitProjectFormUpload();

            //заполнение полей на 1 шаге
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(DuplicateProjectName);

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();

            //проверка что создать дубл.имя нельзя
            if (IsElementPresent(By.Id("project-wizard-tms")))
            {
                WriteFileConsoleResults("Test Fail", 0);
            }
            else
            {
                WriteFileConsoleResults("Test Pass", 1);
            }


            Thread.Sleep(6000);

        }
        /// <summary>
        /// метод проверки невозможности создания проекта в большим именем(>100 символов)
        /// </summary>
        public void CreateProjectBigNameTest()
        {
            WriteFileConsoleResults("Create Project with Big Name (> 100 symbols) Test", 2);
            Thread.Sleep(6000);

            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);

            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();

            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            //вводим большое имя проекта
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");

            Thread.Sleep(2000);
            //нажимаем кнопку next и проверяем открылось окно следующего шага или нет
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();

            Thread.Sleep(6000);

            if (_driver.FindElement(By.Id("project-wizard-tms")).Displayed)
            {
                WriteFileConsoleResults("Test Fail", 0);
            }
            else
            {
                WriteFileConsoleResults("Test Pass", 1);
            }
            //Thread.Sleep(4000);


        }
        /// <summary>
        /// метод проверки на ограничение имени проекта (100 символов)
        /// </summary>
        public void CreateProjectLimitNameTest()
        {
            WriteFileConsoleResults("Create Project - Limit Name (100) Test", 2);
            Thread.Sleep(4000);

            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);

            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            //вводим большое имя проекта
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891");

            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
            Thread.Sleep(4000);

            if (_driver.FindElement(By.Id("project-wizard-tms")).Displayed)
            {
                WriteFileConsoleResults("Test Pass", 1);
            }
            else
            {
                WriteFileConsoleResults("Test Fail", 0);
            }
        }
        /// <summary>
        /// метод тестирования создания проектов с одинаковыми sourc и target языками
        /// </summary>
        public void CreateProjectEqualLanguagesTest()
        {
            WriteFileConsoleResults("Create Project - Equal Languages", 2);
            Thread.Sleep(4000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
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
        public void CreateProjectForbiddenSymbolsTest()
        {
            WriteFileConsoleResults("Create Project - Forbidden Symbols Test", 2);
            Thread.Sleep(4000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys("*|\\:\"<\\>?/");
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

        public void CreateProjectForbiddenSymbolsTest2()
        {

            string NamePr, NameCheck;
            NamePr = _projectName + " " + DateTime.UtcNow.Ticks.ToString();
            WriteFileConsoleResults("Create Project - Forbidden Symbols Test 2", 2);
            Thread.Sleep(4000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(NamePr + "*|\\:\"<\\>?/");
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
            Thread.Sleep(4000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//span[contains(text(), 'Back')]")).Click();
            //получаем имя проекта
            NameCheck = _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).GetAttribute("value");

            if (NameCheck == NamePr)
            {
                WriteFileConsoleResults("Test Pass", 1);
            }
            else
            {
                WriteFileConsoleResults("Test Fail", 0);
            }


        }
        /// <summary>
        /// метод для тестирования проекта с пустым именем
        /// </summary>
        public void CreateProjectEmptyNameTest()
        {
            WriteFileConsoleResults("Create project - Empty name", 2);
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys("");
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();

            Thread.Sleep(3000);
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
        /// метод для тестирования создания имени проекта состоящего из одного пробела
        /// </summary>
        public void CreateProjectSpaceNameTest()
        {
            WriteFileConsoleResults("Create project - Name = SPACE", 2);
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(" ");
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();

            Thread.Sleep(3000);
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

#warning Доделать тесты ТС-36
        public void CreateProjectSpaceNameTest2()
        {
            string NamePr = _projectName + " " + DateTime.UtcNow.Ticks.ToString();
            string NameCheck;
            WriteFileConsoleResults("Create project - Name=Space+Name", 2);
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(" " + NamePr);
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
            Thread.Sleep(4000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//span[contains(text(), 'Back')]")).Click();
            //получаем имя проекта
            NameCheck = _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).GetAttribute("value");


        }
        /// <summary>
        /// отмена создания проекта на первом шаге
        /// </summary>
        public void CancelFirstTest()
        {
            WriteFileConsoleResults("Cancel project creation after 1 step", 2);
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(_projectName + " " + DateTime.UtcNow.Ticks.ToString());
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Cancel']")).Click();
            Thread.Sleep(3000);

            //должны оказаться на workspace без открытых окон, только список проектов
            if (IsElementPresent(By.Id("project-wizard")))
            {
                WriteFileConsoleResults("Test Fail", 0);
            }

            else
            {
                WriteFileConsoleResults("Test Pass", 1);
            }

        }
        /// <summary>
        /// отмена создания проекта(подтверждение отмены)
        /// </summary>
        public void CancelYesTest()
        {
            WriteFileConsoleResults("Cancel project creation with message - yes", 2);
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            string NameCheck = _projectName + " " + DateTime.UtcNow.Ticks.ToString();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(NameCheck);
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Next']")).Click();
            WaitTMUpload();
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Back']")).Click();
            WaitProjectFormUpload();
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Cancel']")).Click();

            if (IsElementPresent(By.XPath(".//span[text()='Yes']")))
            {
                _driver.FindElement(By.XPath(".//span[text()='Yes']")).Click();
                Thread.Sleep(4000);
                if (!IsElementPresent(By.Id("project-wizard")))
                {
                    //проверка что в списке проектов нет проекта
                    if (!_driver.PageSource.Contains(NameCheck))
                    {
                        WriteFileConsoleResults("Test Pass", 1);
                    }
                    else
                    {
                        WriteFileConsoleResults("Test Fail\n Project in list", 0);
                    }

                }
                else
                {
                    WriteFileConsoleResults("Test Fail\n Message window did not close", 0);
                }

            }
            else
            {
                WriteFileConsoleResults("Test Fail\n No message window", 0);
            }

        }
        /// <summary>
        /// отмена создания проекта - No 
        /// </summary>
        public void CancelNoTest()
        {
            WriteFileConsoleResults("Cancel project creation with message - yes", 2);
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            string NameCheck = _projectName + " " + DateTime.UtcNow.Ticks.ToString();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(NameCheck);
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Next']")).Click();
            WaitTMUpload();
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Back']")).Click();
            WaitProjectFormUpload();
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Cancel']")).Click();

            if (IsElementPresent(By.XPath(".//span[text()='No']")))
            {
                _driver.FindElement(By.XPath(".//span[text()='No']")).Click();
                Thread.Sleep(2000);
                if (IsElementPresent(By.Id("project-wizard")))
                {
                    WriteFileConsoleResults("Test Pass", 1);
                }
                else
                {
                    WriteFileConsoleResults("Test Fail\n Message window did not close", 0);
                }
            }
            else
            {
                WriteFileConsoleResults("Test Fail\n No message window", 0);
            }

        }
#warning pageSource  - уточнить про проверку этим методом
        /// <summary>
        /// изменение имени проекта на новое по нажатию кнопки Back
        /// </summary>
        public void ChangeProjectNameOnNew()
        {
            //string OldProjectName = _projectName + " " + DateTime.UtcNow.Ticks.ToString();
            //string NewProjectName = _projectName + " " + DateTime.UtcNow.Ticks.ToString();
            string OldProjectName = "1";
            string NewProjectName = "2";
            WriteFileConsoleResults("Change project name on New by button <Back>", 2);
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.FindElement(By.Id("project-add-btn")).Displayed);
            _driver.FindElement(By.Id("project-add-btn")).Click();
            WaitProjectFormUpload();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(OldProjectName);
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Next']")).Click();
            WaitTMUpload();
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Back']")).Click();
            WaitProjectFormUpload();
            Thread.Sleep(2000);
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(NewProjectName);
            Thread.Sleep(2000);
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Next']")).Click();
            WaitTMUpload();
            //нажать по Значку закрытия окна "x"
            _driver.FindElement(By.XPath(".//div[@id='project-wizard']//div[@id='project-wizard_header']//img")).Click();
            Thread.Sleep(2000);
            if (_driver.PageSource.Contains(OldProjectName))
            {
                WriteFileConsoleResults("Test Pass", 1);
            }
            else
            {
                WriteFileConsoleResults("Test Fail", 0);
            }

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


        /// <summary>
        /// завершение теста
        /// </summary>
        public void CloseTest()
        {
            _driver.Quit();
        }


    }
}
