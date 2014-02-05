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
    [TestFixture("DevUrl", "DevWorkspace", "Firefox")]
    [TestFixture("StableUrl2", "StableWorkspace2", "Firefox")]
    [TestFixture("StableUrl2", "StableWorkspace2", "Chrome")]
    [TestFixture("StableUrl2", "StableWorkspace2", "IE")]
    [TestFixture("StageUrl2", "StageWorkspace2", "Firefox")]
    public class BaseTest
    {
        private IWebDriver _driver;
        protected IWebDriver Driver 
        {
            get 
            {
                return _driver;
            }
 
        }
        private WebDriverWait _wait;
        protected WebDriverWait Wait
        {
            get
            {
                return _wait;
            }
        }
        private FirefoxProfile _profile;
        private string _url;
        protected string Url 
        {
            get
            {
                return _url;
            }
        }
        private string _login;
        private string _password;
        private string _projectName;
        private string _tmName;
        protected string PathTestResults
        {
            get
            {
                System.IO.DirectoryInfo directoryInfo =
                    System.IO.Directory.GetParent(@"..\..\..\TestResults\");
                
                return directoryInfo.ToString();               
            }
        }

        protected string ProjectName
        {
            get
            {
                return _projectName;
            }
        }
        
        private string _deadlineDate;
        private string _documentFile;
        protected string DocumentFile
        {
            get
            {
                return _documentFile;
            }
        }
        private string _tmFile;

        protected string TmFile
        {
            get
            {
                return _tmFile;
            }
        }


        public BaseTest (string url, string workspaceUrl, string browserName)
        {
            _url = ConfigurationManager.AppSettings[url];
            

            if (browserName == "Firefox")
            {
                
                if (_driver == null)
                {
                    _profile = new FirefoxProfile();
                    _profile.AcceptUntrustedCertificates = true;
                    _profile.SetPreference("browser.download.dir", PathTestResults);
                    _profile.SetPreference("browser.download.folderList", 2);
                    _profile.SetPreference("browser.download.useDownloadDir", false);
                    _profile.SetPreference("browser.download.manager.showWhenStarting", false);
                    _profile.SetPreference("browser.helperApps.alwaysAsk.force", false);
                    _profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "text/xml, text/csv, text/plain, text/log, application/zip, application/x-gzip, application/x-compressed, application/x-gtar, multipart/x-gzip, application/tgz, application/gnutar, application/x-tar");
                    //_profile.SetPreference("pdfjs.disabled", true);
                    

                    _driver = new FirefoxDriver(_profile);
                    //string profiledir = "../../../Profile";
                   // string profiledir = "TestingFiles/Profile";
                    //_profile = new FirefoxProfile(profiledir);
                    //_driver = new FirefoxDriver(_profile);
                }
            }
            else if (browserName == "Chrome")
            {
                //TODO: Проверить версию chromedriver
                _driver = new ChromeDriver();
            }
            else if (browserName == "IE")
            {
                //TODO: Сделать запуск из IE
            }

            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            _login = ConfigurationManager.AppSettings["Login"];
            _password = ConfigurationManager.AppSettings["Password"];
            _projectName = ConfigurationManager.AppSettings["ProjectName"];
            _deadlineDate = ConfigurationManager.AppSettings["DeadlineDate"];
            _documentFile = Path.GetFullPath("TestingFiles/English.docx");
            _tmName = ConfigurationManager.AppSettings["TMName"];

            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            _tmName += " " + DateTime.UtcNow.Ticks.ToString();
            _tmFile = Path.GetFullPath("TestingFiles/Earth.tmx");

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }


        /// <summary>
        /// Метод назначения задачи на пользователя 
        /// </summary>
        protected void AssignTask()
        {

            // выбрать проект
            Wait.Until((d) => d.FindElement(By.LinkText(_projectName))).Click();
         
            // нажать на Progress

            Thread.Sleep(5000);
            _driver.FindElement(By.CssSelector(".project-documents div.x-grid-body table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id(
                "documents-progress-btn"
                )).Click();

            Thread.Sleep(1000);

            // Назначить ответственного в окне Progress

            // Ввести нужное имя (пока хардкод)
            _driver.FindElement(By.CssSelector("#document-settings-workflow-body table tr:nth-child(1) td:nth-child(3)")).Click();
            _wait.Until(d => _driver.FindElements(By.XPath(
                "//input[starts-with(@class,'x-form-field')]"
                )));
            IList<IWebElement> els = _driver.FindElements(By.XPath(
                "//input[starts-with(@class,'x-form-field')]"
                ));
            //Имя для cat-stage2
            els[1].SendKeys("Bob Test");

            //Имя для cat-dev
            //els[1].SendKeys("Bob Dylan");
                        
            //Для cat-stage2
            _driver.FindElement(By.XPath("//div[@class='x-boundlist-item'][starts-with(string(),'Bob Test')]")).Click();

            //Для cat-dev
            //_driver.FindElement(By.CssSelector("div.x-boundlist-item")).Click(); 
                                                
            // Нажать на Assign, чтобы появился Warning
            _wait.Until(d => _driver.FindElement(By.XPath(
                "//a[contains(@class, 'x-btn x-btn-default-small x-btn-default-small-noicon assign')]"
                ))).Click();
            // Нажать на Ok в окне Warning
            _driver.FindElement(By.Id("button-1012")).Click();
            // Нажать Assign
            _wait.Until(d => _driver.FindElement(By.XPath(
                "//a[contains(@class, 'x-btn x-btn-default-small x-btn-default-small-noicon assign')]"
                ))).Click();
            Thread.Sleep(1000);
            // Нажать на Close
            _driver.FindElement(By.XPath("//span[contains(text(), 'Close')]")).Click();

            // Вернуться на главную страницу
            _driver.Navigate().GoToUrl(_url);

            // Нажать на Assigned Tasks
            _wait.Until(d => _driver.FindElement(By.XPath("//span[contains(text(), 'Assigned Tasks')]"))).Click();

            // Нажать на все Accept кнопки
            IList<IWebElement> acceptbuttons = _driver.FindElements(By.XPath("//span[contains(text(), 'Accept')]"));
            foreach (IWebElement el in acceptbuttons)
                el.Click();
            // Нажать на Apply Changes
            _driver.FindElement(By.XPath("//span[contains(text(), 'Apply changes')]")).Click();
            // Вернуться на главную страницу
            _driver.Navigate().GoToUrl(_url);
        }

        /// <summary>
        /// Метод открытия документа в редакторе
        /// </summary>
        /// <param name="projectname">имя открываемого проекта</param>
        protected void OpenDocument(string projectname)
        {
            //WriteFileConsoleResults("Open Document", 2);

            // Строчка нужного проекта
            _driver.FindElement(By.LinkText(projectname)).Click();

            // Далее нажать на появившийся документ
            IWebElement element = Wait.Until(d => _driver.FindElement(By.XPath(
                "//a[starts-with(@href, '/editor')]" // критерий - editor
                )));
            element.Click();

            // Дождаться загрузки страницы
            _wait.Until((d) => d.Title.Contains("Editor"));

            // Проверить, существует ли хотя бы один сегмент
            _driver.FindElement(By.CssSelector(
                "#segments-body div table tr:nth-child(1)"
                ));

        }
        
        public void Authorization() 
        {
            //TODO: Заменить на NLog
            //WriteFileConsoleResults("Autorization Test", 2);

            // Отослать Credentials
            _driver.Navigate().GoToUrl(_url);
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).SendKeys(_login);
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).SendKeys(_password);
            _driver.FindElement(By.CssSelector("input[type =\"submit\"]")).Click();

            // Дождаться пока загрузится поле с выбором аккаунта
            _wait.Until((d) => d.FindElement(By.XPath(
                "//select/option[contains(text(), 'TestAccount')]"
                )));

            // Выбрать тестовый аккаунт
            _driver.FindElement(By.XPath(".//select/option[contains(text(), 'TestAccount')]")).Click();
            // Зайти на сайт
            _driver.FindElement(By.CssSelector("input[type = \"submit\"]")).Click();

            //ждем пока появится кнопка на странице Workspace
            _wait.Until((d) => d.FindElement(By.Id("projects-add-btn")));
            Assert.True(_driver.Title.Contains("Workspace"), "Ошибка: неверный заголовок страницы");

        }


        private void FirstStepProjectWizard(string ProjectName)
        {
            Assert.IsTrue(_driver.FindElement(By.Id("projects-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("projects-add-btn")).Click();

            //ждем загрузки формы
            
            _wait.Until((d) => d.FindElement(By.Id("project-wizard")));

            //заполнение полей на 1 шаге
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();

            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(ProjectName);

            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).SendKeys(_deadlineDate);
        }


        protected void CreateProject(string ProjectName, bool FileFlag, string DocumentName)
        {
            FirstStepProjectWizard(ProjectName);

            AddDocument(FileFlag, DocumentName);
            
            //2 шаг - выбор ТМ
            ChooseExistingTM();

            //3 шаг - выбор МТ
            ChooseMT();

            //4 шаг - выбор глоссария
            ChooseGlossary();

            //5 шаг - настройка этапов workflow
            SetUpWorkflow();

            //Finish
            Pretranslate();
        }


        protected void CreateProject(string ProjectName, bool FileFlag, string DocumentName, string TmName)
        {
            FirstStepProjectWizard(ProjectName);

            AddDocument(FileFlag, DocumentName);

            //2 шаг - создание ТМ
            ChooseCreatedTM(TmName);

            //3 шаг - выбор МТ
            ChooseMT();

            //4 шаг - выбор глоссария
            ChooseGlossary();

            //5 шаг - настройка этапов workflow
            SetUpWorkflow();

            //Finish
            Pretranslate();
        }


        public void AddDocument(bool FileFlag, string DocumentName)
        {
            if (FileFlag == true)
            {
                //процесс добавления файла 
                //нажатие кнопки Add
                _driver.FindElement(By.XPath(".//div[@id='project-wizard-body']//span[text()='Add']")).Click();

                Thread.Sleep(1000);
                // Заполнить форму для отправки файла
                SendKeys.SendWait(DocumentName);
                Thread.Sleep(1000);
                SendKeys.SendWait(@"{Enter}");


                Thread.Sleep(2000);                
            }

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
        }


        public void ChooseExistingTM()
        {    
            //Выбрать существующий TM
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1) td:nth-child(2)")));

            _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1) td:nth-child(2)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//span[contains(text(), 'Next')]")).Click();
        }


        public void ChooseCreatedTM(string TmName)
        {   
            //Создать TM
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1)")));
            
            CreateTMXFile(TmName);

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//span[contains(text(), 'Next')]")).Click();
        }


        public void ChooseMT()
        {
            //Выбрать необходимые MT
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")).Click();

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(4) td:nth-child(1)")).Click();

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(1) td:nth-child(1)")).Click();

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(3) td:nth-child(1)")).Click();

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(5) td:nth-child(1)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
        }


        public void ChooseGlossary()
        {
            //Выбрать необходимый глоссарий
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-tbs table tr:nth-child(1) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-tbs table tr:nth-child(1) td:nth-child(1)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tbs']//span[contains(text(), 'Next')]")).Click();
        }


        public void SetUpWorkflow()
        {
            //Настроить этапы workflow
            _wait.Until(d => _driver.FindElement(By.Id("project-workflow-new-stage-btn")));

            _driver.FindElement(By.Id("project-workflow-new-stage-btn")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-workflow']//span[contains(text(), 'Next')]")).Click();
        }


        public void Pretranslate()
        {
            _wait.Until(d => _driver.FindElement(By.XPath(".//div[@id='project-wizard-pretranslate']//span[contains(text(), 'Finish')]")));
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-pretranslate']//span[contains(text(), 'Finish')]")).Click();
        }


        public void CreateTMXFile(string TmName)
        {
            //Создать ТМ
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//span[text()='Create']")).Click();

            _wait.Until(d => _driver.FindElement(By.XPath(".//div[@id='project-wizard-tm']//span[contains(text(), 'Add new TM Base')]")));

            //Заполнить данные о новой ТМ
            _driver.FindElement(By.CssSelector("#project-wizard-tm input[name='Name']")).Clear();

            _driver.FindElement(By.CssSelector("#project-wizard-tm input[name='Name']")).SendKeys(_tmName);

            //Добавить тмх файл
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tm']//span[text()='Add']")).Click();

            Thread.Sleep(1000);
            // Заполнить форму для отправки файла
            SendKeys.SendWait(TmName);

            Thread.Sleep(1000);

            SendKeys.SendWait(@"{Enter}");

            Thread.Sleep(2000);

            //Нажать на кнопку Accept
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tm']//span[text()='Accept']")).Click();

            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1) td:nth-child(1)")));

            //Выбирать созданный ТМ из списка не надо, он выбран автоматически

            Assert.NotNull(_driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//table//tr[td[contains(string(), '" + _tmName + "')]]")));            
        }

        public void AddTMXFile(string projectname)
        {
            _driver.FindElement(By.LinkText(projectname)).Click();
            //Нажать кнопку Edit
            _driver.FindElement(By.XPath(".//div[@id='project-tms']//span[text()='Edit']")).Click();

            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1)")));

            //Driver.FindElement(By.XPath("//div[@id='project-wizard-tms']//table//tr[contains(@class, 'selected')]/td[1]")).Click();

            CreateTMXFile(TmFile);

            _driver.FindElement(By.XPath("//div[@id='project-wizard']//span[text()='Save']")).Click();

            _wait.Until(d => _driver.FindElement(By.XPath("//div[@id='project-tms']//table//tr[1]")));

            _driver.Navigate().GoToUrl(_url);
        }

        public void ConfirmButton()
        {
            // Написать что-то в source
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys("This is a sample text");

            // Нажать на кнопку подтвердить
            Driver.FindElement(By.Id("confirm-btn")).Click();

            // Убедиться что сегмент подтвержден
            // Если элемента нет, то выкинет NoSuchElementException, поэтому нет Assert
            Driver.FindElement(By.ClassName("icon-ok"));
        }

        public void BackButton()
        {
            // Нажать кнопку назад
            Driver.FindElement(By.Id("back-btn")).Click();

            Wait.Until((d) => d.Title.Contains("Workspace"));

            Assert.AreEqual(true, Driver.Title.Contains("Workspace"));
        }

        public void SourceTargetSwitchButton()
        {
            //Выбрать source первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).Click();

            // Нажать кнопку Tab
            Driver.FindElement(By.Id("toggle-btn")).Click();


            // Проверить где находится курсор, и если в поле source, то все ок
            IWebElement a = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div"));
            IWebElement b = Driver.SwitchTo().ActiveElement();

            Point a_loc = a.Location;
            Point b_loc = b.Location;

            Size a_size = a.Size;
            Size b_size = b.Size;

            Assert.True((a_loc == b_loc) && (a_size == b_size));
        }

        public void SourceTargetSwitchHotkey()
        {
            //Выбрать source первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).Click();

            // Нажать хоткей Tab
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).
                SendKeys(OpenQA.Selenium.Keys.Tab);


            // Проверить где находится курсор, и если в поле source, то все ок
            IWebElement a = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div"));
            IWebElement b = Driver.SwitchTo().ActiveElement();

            Point a_loc = a.Location;
            Point b_loc = b.Location;

            Size a_size = a.Size;
            Size b_size = b.Size;

            Assert.True((a_loc == b_loc) && (a_size == b_size));
        }

        public void ToTargetButton()
        {
            // Выбрать source первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).Click();

            // Текст source'a первого сегмента
            string sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Text;
            // Нажать кнопку копирования
            Driver.FindElement(By.Id("copy-btn")).Click();

            // Проверить, такой ли текст в target'те
            string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);
        }

        public void ToTargetHotkey()
        {
            // Выбрать source первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).Click();

            // Текст source'a первого сегмента
            string sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Text;
            // Нажать хоткей копирования
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).
                SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Insert);
            // Проверить, такой ли текст в target'те
            string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);
        }

        public void CancelButton()
        {
            // Выбрать source первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).Click();

            // Нажать кнопку копирования
            Driver.FindElement(By.Id("copy-btn")).Click();
            // Текст source'a первого сегмента
            string sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Text;
            // Проверить, такой ли текст в target'те
            string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);

            // Нажать кнопку отмены
            Driver.FindElement(By.Id("undo-btn")).Click();
            // Убедиться, что в target нет текста
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual("", targetxt);
        }

        public void CancelHotkey()
        {
            // Выбрать source первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).Click();

            // Нажать кнопку копирования
            Driver.FindElement(By.Id("copy-btn")).Click();
            // Текст source'a первого сегмента
            string sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Text;
            // Проверить, такой ли текст в target'те
            string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);

            // Нажать хоткей отмены
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).
                SendKeys(OpenQA.Selenium.Keys.Control + "Z");
            // Убедиться, что в target нет текста
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual("", targetxt);
        }

        public void RedoAfterCancelButton()
        {
            // Выбрать source первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).Click();

            // Нажать кнопку копирования
            Driver.FindElement(By.Id("copy-btn")).Click();
            // Текст source'a первого сегмента
            string sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Text;
            // Проверить, такой ли текст в target'те
            string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);

            // Нажать кнопку отмены
            Driver.FindElement(By.Id("undo-btn")).Click();
            // Убедиться, что в target нет текста
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual("", targetxt);

            // Нажать кнопку возврата отмененного действия
            Driver.FindElement(By.Id("redo-btn")).Click();

            // Убедиться, что в target и source одинаковы
            sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Text;
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);
        }

        public void RedoAfterCancelHotkey()
        {
            // Выбрать source первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2)")).Click();

            // Нажать кнопку копирования
            Driver.FindElement(By.Id("copy-btn")).Click();
            // Текст source'a первого сегмента
            string sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Text;
            // Проверить, такой ли текст в target'те
            string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);

            // Нажать кнопку отмены
            Driver.FindElement(By.Id("undo-btn")).Click();
            // Убедиться, что в target нет текста
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual("", targetxt);

            // Нажать хоткей возврата отмененного действия
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + "Y");

            // Убедиться, что в target и source одинаковы
            sourcetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(2) div")).Text;
            targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            Assert.AreEqual(sourcetxt, targetxt);
        }

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
                Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                    SendKeys(OpenQA.Selenium.Keys.Alt + OpenQA.Selenium.Keys.F3);
            }
        }

        private void CheckChangeCase(string sourceText, string textAfterFirstChange, string textAfterSecondChange, bool byButtonTrueByHotkeyFalse)
        {
            // Список текстов для сравнения после изменения регистра
            List<string> listToCompare = new List<string> ();
            listToCompare.Add(textAfterFirstChange);
            listToCompare.Add(textAfterSecondChange);
            listToCompare.Add(sourceText);

            for (int i = 0; i < 3; ++i)
            {
                // Нажать изменениe регистра
                ClickChangeCase(byButtonTrueByHotkeyFalse);
                // Убедиться, что регистр слова изменился правильно - сравнить со значением в listToCompare
                string targetxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
                Assert.AreEqual(listToCompare[i], targetxt);
            }
        }

        public void ChangeCaseTextButton()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys("the example sentence");
            // Нажать хоткей выделения всего содержимого ячейки
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Запустить проверку
            CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", true);
        }

        public void ChangeCaseTextHotkey()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys("the example sentence");
            // Нажать хоткей выделения всего содержимого ячейки
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Запустить проверку
            CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", false);
        }

        public void ChangeCaseSomeWordButton()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys("some words for example");
            // Нажать хоткей выделения последнего слова
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);
            // Запустить проверку
            CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", true);
        }

        public void ChangeCaseSomeWordHotkey()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys("some words for example");
            // Нажать хоткей выделения последнего слова
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);
            // Запустить проверку
            CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", false);
        }

        public void ChangeCaseFirstWord()
        {
            // Написать текст в первом сегменте в target
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys("some words for example");
            // Нажать хоткей перехода в начало строки
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
            // Нажать хоткей выделения первого слова
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
            // Запустить проверку по кнопке
            CheckChangeCase("some words for example", "Some words for example", "SOME words for example", true);
            // Запустить проверку по хоткею
            CheckChangeCase("some words for example", "Some words for example", "SOME words for example", false);
        }      

        [TearDown]
        public void Teardown()
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

    public class EditorButtonsTest : BaseTest
    {
        public EditorButtonsTest(string url, string workspaceUrl, string browserName)
            : base (url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
            // 1. Авторизация
            Authorization();

            // 2. Создание проекта с 1 документов внутри
            CreateProject(ProjectName, true, DocumentFile);

            //CreateProject(ProjectName, true, DocumentFile, TmFile);

            // 3. Назначение задачи на пользователя
            AssignTask();

            //AddTMXFile(ProjectName);

            // 4. Открытие документа по имени созданного проекта
            OpenDocument(ProjectName);

        }

        
        /// <summary>
        /// Метод тестирования кнопки "Back" в редакторе
        /// </summary>
        [Test]
        public void BackButtonTest()
        {
            BackButton();
        }

        /// <summary>
        /// Метод тестирования кнопки перемещения курсора между полями source и target без хоткея
        /// </summary>
        [Test]
        public void SourceTargetSwitchButtonTest()
        {
            SourceTargetSwitchButton();

        }

        /// <summary>
        /// Метод тестирования хоткея перемещения курсора между полями source и target
        /// </summary>
        [Test]
        public void SourceTargetSwitchHotkeyTest()
        {
            SourceTargetSwitchHotkey();
        }

        /// <summary>
        /// Метод тестирования кнопки подтвеждения сегмента
        /// </summary>
        [Test]
        public void ConfirmButtonTest()
        {
            ConfirmButton();
        }


        /// <summary>
        /// Метод тестирования кнопки копирования оригинала в перевод
        /// </summary>
        [Test]
        public void ToTargetButtonTest()
        {
            ToTargetButton();
        }

        /// <summary>
        /// Метод тестирования хоткея копирования оригинала в перевод
        /// </summary>
        [Test]
        public void ToTargetHotkeyTest()
        {
            ToTargetHotkey();

        }

        /// <summary>
        /// Метод тестирования кнопки отмены действия
        /// </summary>
        [Test]
        public void CancelButtonTest()
        {
            CancelButton();
        }

        /// <summary>
        /// Метод тестирования хоткея отмены действия
        /// </summary>
        [Test]
        public void CancelHotkeyTest()
        {
            CancelHotkeyTest();
        }

        /// <summary>
        /// Метод тестирования кнопки возврата отмененного действия
        /// </summary>
        [Test]
        public void RedoAfterCancelButtonTest()
        {
            RedoAfterCancelButton();
        }

        /// <summary>
        /// Метод тестирования хоткея возврата отмененного действия
        /// </summary>
        [Test]
        public void RedoAfterCancelHotkeyTest()
        {
            RedoAfterCancelHotkey();
        }

        /// <summary>
        /// Метод тестирования кнопки изменения регистра для всего текста
        /// 
        [Test]
        public void ChangeCaseTextButtonTest()
        {
            ChangeCaseTextButton();
        }

        /// <summary>
        /// Метод тестирования хоткея изменения регистра для всего текста
        /// 
        [Test]
        public void ChangeCaseTextHotkeyTest()
        {
            ChangeCaseTextHotkey();
        }

        /// <summary>
        /// Метод тестирования кнопки изменения регистра для слова (не первого)
        /// 
        [Test]
        public void ChangeCaseSomeWordButtonTest()
        {
            ChangeCaseSomeWordButton();
        }

        /// <summary>
        /// Метод тестирования хоткея изменения регистра для слова (не первого)
        /// 
        [Test]
        public void ChangeCaseSomeWordHotkeyTest()
        {
            ChangeCaseSomeWordHotkey();
        }

        /// <summary>
        /// Метод тестирования кнопки и хоткея изменения регистра для первого слова
        /// 
        [Test]
        public void ChangeCaseFirstWordTest()
        {
            ChangeCaseFirstWord();
        }

    }





    public class UserLogTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            // 1. Авторизация
            Authorization();

            // 2. Создание проекта с 1 документов внутри
            CreateProject(ProjectName, true, DocumentFile);

            //CreateProject(ProjectName, true, DocumentFile, TmFile);

            // 3. Назначение задачи на пользователя
            AssignTask();

            //AddTMXFile(ProjectName);

            // 4. Открытие документа по имени созданного проекта
            OpenDocument(ProjectName);

        }

        public UserLogTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

               
        /// <summary>
        /// Метод выгрузки логов 
        /// </summary>        
        public void ExportLog(string path)
        {
            //Получить путь к папке, куда выгружать логи
            string fullResultPath = System.IO.Path.Combine(path, ProjectName);
            
            //Выбрать документ
            Driver.FindElement(By.CssSelector(".project-documents div.x-grid-body table tr:nth-child(1) td:nth-child(1)")).Click();

            //Нажать кнопку выгрузки логов
            Driver.FindElement(By.Id("log-export-btn")).Click();

            // Заполнить форму для сохранения файла
            Thread.Sleep(1000);

            SendKeys.SendWait(fullResultPath);

            Thread.Sleep(1000);

            SendKeys.SendWait(@"{Enter}");

            Thread.Sleep(5000);

            Assert.IsTrue(System.IO.File.Exists(fullResultPath+".zip"));                        
        }

        /// <summary>
        /// Метод тестирования набора текста в документе 
        /// </summary>
        [Test]
        public void WriteText()
        {
            // Написать текст в первом сегменте
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
               SendKeys("This is a sample text");

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            Assert.AreEqual("This is a sample text", segmentxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "WriteText");
                        
            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования удаления текста с клавиатуры в документе 
        /// </summary>
        [Test]
        public void DeleteText()
        {
            // Написать текст в первом сегменте
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
               SendKeys("This is a sample text");

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            Assert.AreEqual("This is a sample text", segmentxt);

            //Удалить текст путем нажатия клавиши Backspace
            while (!String.IsNullOrEmpty(segmentxt))
            {
                Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                   SendKeys(OpenQA.Selenium.Keys.Backspace);

                segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;
            }

            //Убедиться, что в сегменте нет текста
            Assert.AreEqual("", segmentxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "DeleteText");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подтверждения перевода с помощью нажатия кнопки на панели инструментов 
        /// </summary>
        [Test]
        public void ConfirmTextButton()
        {
            //Набрать текст в первом сегменте и нажать кнопку Confirm Segment
            ConfirmButton();

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "ConfirmTextButton");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования кнопки перемещения курсора между полями source и target
        /// </summary>
        [Test]
        public void SourceTargetSegmentsSwitchButton()
        {
            //Переключить курсор между полями source и target
            SourceTargetSwitchButton();

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SourceTargetSegmentsSwitchButton");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования кнопки перемещения курсора между полями source и target по хоткею
        /// </summary>
        [Test]
        public void SourceTargetSegmentsSwitchHotkey()
        {
            //Переключить курсор между полями source и target
            SourceTargetSwitchHotkey();

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SourceTargetSegmentsSwitchHotkey");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из MT по хоткею
        /// </summary>
        [Test]
        public void SubstituteTranslationMTHotkey()
        {
            //Выбираем первый сегмент
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Click();

            //Ждем пока загрузится CAT-панель
            Wait.Until(d => d.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")));

            string catNumber = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[1]")).Text;
            
            //Нажать хоткей для подстановки из MT перевода первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + catNumber);

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            string catxt = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")).Text;

            Assert.AreEqual(segmentxt, catxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SubstituteTranslationMTHotkey");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из MT по клику на сегмент в САТ-панели
        /// </summary>
        [Test]
        public void SubstituteTranslationMTDoubleClick()
        {
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Click();

            //Ждем пока загрузится CAT-панель
            Wait.Until(d => d.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")));

            //Нажать на сегмент в CAT-панели
            Actions action = new Actions(Driver);

            action.DoubleClick(Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")));

            action.Perform();
            
            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            string catxt = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'MT')]]/td[4]")).Text;

            Assert.AreEqual(segmentxt, catxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SubstituteTranslationMTDoubleClick");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из TM по клику на сегмент в САТ-панели
        /// </summary>
        [Test]
        public void SubstituteTranslationTMHotkey()
        {
            //Выбираем первый сегмент
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Click();

            //Ждем пока загрузится CAT-панель
            Wait.Until(d => d.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")));

            string catNumber = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[1]")).Text;

            //Нажать хоткей для подстановки из MT перевода первого сегмента
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).
                SendKeys(OpenQA.Selenium.Keys.Control + catNumber);

            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            string catxt = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")).Text;

            Assert.AreEqual(segmentxt, catxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SubstituteTranslationTMHotkey");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования подстановки перевода сегмента из TM по клику на сегмент в САТ-панели
        /// </summary>
        [Test]
        public void SubstituteTranslationTMDoubleClick()
        {
            Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Click();

            //Ждем пока загрузится CAT-панель
            Wait.Until(d => d.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")));

            //Нажать на сегмент в CAT-панели
            Actions action = new Actions(Driver);

            action.DoubleClick(Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")));

            action.Perform();
            
            string segmentxt = Driver.FindElement(By.CssSelector("#segments-body div table tr:nth-child(1) td:nth-child(3) div")).Text;

            string catxt = Driver.FindElement(By.XPath("//div[@id='cat']//table//tr[td[contains(string(),'TM')]]/td[4]")).Text;

            Assert.AreEqual(segmentxt, catxt);

            //Создать папку для выгрузки логов
            string resultPath = System.IO.Path.Combine(PathTestResults, "SubstituteTranslationTMDoubleClick");

            System.IO.Directory.CreateDirectory(resultPath);

            // Нажать кнопку назад
            BackButton();

            //Выгрузить логи
            ExportLog(resultPath);
        }

        /// <summary>
        /// Метод тестирования создания новой TM на странице настройки проекта
        /// </summary>
        [Test]
        public void AddTMXFileTest()
        {
            Driver.Navigate().Back();

            Driver.Navigate().Back();

            AddTMXFile(ProjectName);

            OpenDocument(ProjectName);                         
        }
              
        
    }


    class AuthorizationTest : BaseTest
    {
        public AuthorizationTest(string url, string workspaceUrl, string browserName)
            : base (url, workspaceUrl, browserName)
        {

        }

        /// <summary>
        /// метод тестирования авторизации пользователя в системе
        /// </summary>
        [Test]
        public void AuthorizationMethodTest()
        {
            Authorization();
        }
                   
    }



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
        private WebDriverWait _wait;

        private string _login;
        private string _password;

        private string _url;

        

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




        public ProjectTest(string url, string workspaceUrl, string browserName)
        {

            _url = ConfigurationManager.AppSettings[url];

            if (browserName == "Firefox")
            {
                if (_driver == null)
                {
                    string profiledir = "../../../packages/Profile";
                    _profile = new FirefoxProfile(profiledir/*@"C:\Users\a.kurenkova\Desktop\FirefoxProfile"*/);
                    _driver = new FirefoxDriver(_profile);
                }
            }
            else if (browserName == "Chrome")
            {
                //TODO: Проверить версию chromedriver
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

            _documentFile = Path.GetFullPath("../../../Scripts/English.docx");
            _tmxFile = @"\\cat-dev\Share\CAT\TestFiles\tmxEng2.tmx";

            _documentFileWrong = @"\\cat-dev\Share\CAT\TestFiles\doc98.doc";
            _ttxFile = @"\\cat-dev\Share\CAT\TestFiles\test.ttx";
            _txtFile = @"\\cat-dev\Share\CAT\TestFiles\test.txt";
            _srtFile = @"\\cat-dev\Share\CAT\TestFiles\test.srt";

            _xliffTC10 = @"\\cat-dev\Share\CAT\TestFiles\Xliff\TC-10En.xliff";

            ResultFilePath = @"\\cat-dev\Share\CAT\TestResult\Result" + DateTime.UtcNow.Ticks.ToString() + ".txt";
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
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

            _driver.Navigate().GoToUrl(_url);
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"email\"]")).SendKeys(_login);
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"password\"]")).SendKeys(_password);
            _driver.FindElement(By.CssSelector("input[type =\"submit\"]")).Click();

            // Дождаться пока загрузится поле с выбором аккаунта
            _wait.Until((d) => d.FindElement(By.XPath(
                "//select/option[contains(text(), 'TestAccount')]"
                )));

            // Выбрать тестовый аккаунт
            _driver.FindElement(By.XPath(".//select/option[contains(text(), 'TestAccount')]")).Click();
            // Зайти на сайт
            _driver.FindElement(By.CssSelector("input[type = \"submit\"]")).Click();

            //ждем пока появится кнопка на странице Workspace
            _wait.Until((d) => d.FindElement(By.Id("projects-add-btn")));
            Assert.True(_driver.Title.Contains("Workspace"), "Ошибка: неверный заголовок страницы");          


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
            //TODO: заменить скрипт
            //скрипт Autolt - вызов скрипта для добавления файла
            SetUploadedFile(_documentFileWrong);
            Thread.Sleep(6000);

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
            // TODO: Пересмотреть - не протестила с контрпримером чтобы возникла ошибка.
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
                //SetUploadedFile(DocumentName);
                Thread.Sleep(1000);
                // Заполнить форму для отправки файла
                SendKeys.SendWait(DocumentName);
                Thread.Sleep(1000);
                SendKeys.SendWait(@"{Enter}");

                
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

        // TODO: Вставить проверку что документ загружен!!! (как проверить это)

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
                p.StartInfo.FileName = Path.GetFullPath("../../../Scripts/upload_file.exe");
                /*p.StartInfo.FileName = @"C:\Users\n.sokolov\Desktop\cat\CAT.FrontEnd.Tests\Scripts\upload_file.exe"; */
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
