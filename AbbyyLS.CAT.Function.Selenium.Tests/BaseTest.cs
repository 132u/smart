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
        private string _constTmName;
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

        protected string DeadlineDate
        {
            get
            {
                return _deadlineDate;
            }
        }

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

        private string _secondTmFile;

        protected string SecondTmFile
        {
            get
            {
                return _secondTmFile;
            }
        }


        public BaseTest(string url, string workspaceUrl, string browserName)
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
            _constTmName = ConfigurationManager.AppSettings["TMName"];

            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            _tmName += " " + DateTime.UtcNow.Ticks.ToString();
            _tmFile = Path.GetFullPath("TestingFiles/Earth.tmx");
            _secondTmFile = Path.GetFullPath("TestingFiles/TextEngTestAddTMX.tmx");

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

        public void SwitchTMTab()
        {
            // Нажать кнопку перехода на страницу Базы Translation memory
            _driver.FindElement(By.XPath(
                ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Enterprise/TranslationMemories')]")).Click();

            // ждем загрузки страницы
            _wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'l-corpr__addbtnbox')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Displayed);
        }


        protected void FirstStepProjectWizard(string ProjectName)
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
            ChooseMTCompreno();

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
            ChooseMTCompreno();

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

                FillAddDocumentForm(DocumentName);
            }

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
        }

        protected void FillAddDocumentForm(string DocumentName)
        {
            Thread.Sleep(1000);
            // Заполнить форму для отправки файла
            SendKeys.SendWait(DocumentName);
            Thread.Sleep(1000);
            SendKeys.SendWait(@"{Enter}");


            Thread.Sleep(2000);
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

        public void ChooseMTBing()
        {
            //Выбрать MT Bing
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(1) td:nth-child(1)")).Click();
           
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
        }

        public void ChooseMTCompreno()
        {
            //Выбрать MT Compreno
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
        }

        public void ChooseMTGoogle()
        {
            //Выбрать MT Google
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(3) td:nth-child(1)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
        }

        public void ChooseMTMoses()
        {
            //Выбрать MT Moses
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(4) td:nth-child(1)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
        }

        public void ChooseMTYandex()
        {
            //Выбрать MT Yandex
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

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
            List<string> listToCompare = new List<string>();
            listToCompare.Add(textAfterFirstChange);
            listToCompare.Add(textAfterSecondChange);
            listToCompare.Add(sourceText);

            for (int i = 0; i < listToCompare.Count; ++i)
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

        public void CreateNewTM()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName;
            SelectUniqueTMName(out uniqueTMName);
            // Создать ТМ
            CreateTMByNameAndSave(uniqueTMName);

            // Проверить, сохранился ли ТМ
            Assert.IsTrue(GetIsExistTM(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");

            // Проверить, что количество сегментов равно 0
            int segCount;
            GetSegmentCount(uniqueTMName, out segCount);
            Assert.IsTrue(segCount == 0, "Ошибка: количество сегментов должно быть равно 0");
        }

        public void CreateTMCheckProjectCreateTMList()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName;
            SelectUniqueTMName(out uniqueTMName);
            // Создать ТМ
            CreateTMByNameAndSave(uniqueTMName);

            // Перейти на вкладку SmartCAT и проверить, что TM нет в списке при создании проекта
            Assert.IsTrue(GetIsExistTMCreateProject(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");
        }

        public void CreateTMWithoutName()
        {
            // Открыть форму создания ТМ
            OpenCreateTMForm();

            // Нажать кнопку Сохранить
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'g-btn__text')]")).Click();

            // Проверить выделение ошибки в поле Название
            Assert.IsTrue(_driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@class,'error')]")).Displayed,
                "Ошибка: поле Название не выделено ошибкой");

            // Проверить появления сообщения об ошибке
            Assert.IsTrue(_driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'l-createtm__error')]")).Displayed,
                "Ошибка: не появилось сообщенеи об ошибке");
        }

        public void CreateTMWithExistingName()
        {
            string TMName = _constTmName;
            // Создать ТМ
            CreateTMIfNotExist(TMName);
            // Создать ТМ с тем же (уже существующим) именем
            CreateTMByNameAndSave(TMName);

            // Проверить появление ошибки
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'js-dynamic-errors')]//p[contains(@data-key,'name')]")).Displayed,
                "Ошибка: не появилась ошибка создания ТМ с существующим именем");
        }

        public void CreateTMWithTMX()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName;
            SelectUniqueTMName(out uniqueTMName);
            // Создать ТМ
            CreateTMByName(uniqueTMName);

            // Нажать на Сохранить и Импортировать TMX файл
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'js-save-and-import')]")).Click();
            // ждем появления окна
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//a[contains(@class,'js-upload-btn')]")));
            // Загрузить TMX файл
            UploadDocumentTM(TmFile);

            // Проверить, сохранился ли ТМ
            Assert.IsTrue(GetIsExistTM(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");

            // Проверить, что количество сегментов больше 0
            int segCount;
            GetSegmentCount(uniqueTMName, out segCount);
            Assert.IsTrue(segCount > 0, "Ошибка: количество сегментов должно быть больше 0");
        }

        public void CreateTMWithNotTMX()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName;
            SelectUniqueTMName(out uniqueTMName);
            // Создать ТМ
            CreateTMByName(uniqueTMName);

            // Нажать на Сохранить и Импортировать TMX файл
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'js-save-and-import')]")).Click();

            // ждем появления окна
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//a[contains(@class,'js-upload-btn')]")));

            // Загрузить НЕ(!) TMX файл
            UploadDocumentTM(DocumentFile);

            // Проверить, сохранился ли ТМ
            Assert.IsTrue(GetIsExistTM(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");

            // Проверка: должен появиться значок с ошибкой TMX файла
            // Иногда значок зависает (показывает колесо ожидания, а восклицательный знак появляется только после обновления страницы)
            string xPathOfErrorIcon = uniqueTMName;
            CreateXPathTMRow(ref xPathOfErrorIcon);
            xPathOfErrorIcon += "/..//a[contains(@class,'js-error-icon')]";
            Assert.NotNull(Driver.FindElement(By.XPath(xPathOfErrorIcon)),
                "Ошибка: не появился значок с ошибкой TMX файла");
        }

        public void UpdateTMButton()
        {
            string TMName = _constTmName;
            // Загрузить TMX файл
            if (UploadDocumentToTMbyButton(TMName, "js-upload-btn", TmFile))
            {
                // Документ загружен
                // Получить количество сегментов этого ТМ
                int segCount;
                GetSegmentCount(TMName, out segCount);
                // Проверить, что количество сегментов больше нуля
                Assert.IsTrue(segCount > 0);
            }
            // Иначе - документ не загружен, т.к. был загружен ранее - тест успешен
        }

        public void ExportTMButton()
        {
            string TMName = _constTmName;
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, "js-export-btn");

            // Открытие диалога
            Thread.Sleep(5000);

            // TODO проверка работы в диалоговом окне!!!
        }

        public void DeleteTMCheckTMList()
        {
            string TMName = _constTmName;
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, "js-delete-btn");

            // Нажимаем Delete в открывшейся форме
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-confirm')]//input[contains(@type,'submit')]"))).Click();

            // Закрытие формы
            Thread.Sleep(5000);

            // Проверить, что ТМ удалилась из списка
            Assert.IsTrue(!GetIsExistTM(TMName), "Ошибка: ТМ не удалилась из списка");
        }

        public void DeleteTMCheckProjectCreateTMList()
        {
            string TMName = _constTmName;
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, "js-delete-btn");

            // Нажимаем Delete в открывшейся форме
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-confirm')]//input[contains(@type,'submit')]"))).Click();
            // Закрытие формы
            Thread.Sleep(5000);

            // Перейти на вкладку SmartCAT и проверить, что TM нет в списке при создании проекта
            Assert.IsTrue(!GetIsExistTMCreateProject(TMName));
        }

        public void AddTMXOnClearTMButton()
        {
            // Создать чистый ТМ
            string TMName;
            SelectUniqueTMName(out TMName);
            CreateTMByNameAndSave(TMName);

            // Загрузить TMX
            if (UploadDocumentToTMbyButton(TMName, "js-add-tmx-btn", TmFile))
            {
                // Документ загружен
                // Получить количество сегментов после загрузки TMX
                int segCount;
                GetSegmentCount(TMName, out segCount);

                // Проверить, что количество сегментов больше 0
                Assert.IsTrue(segCount > 0);
            }
            // Иначе - документ не загружен, т.к. был загружен ранее - тест успешен
        }

        public void AddTMXButton()
        {
            string TMName = _constTmName;
            // Получить количество сегментов до загрузки TMX
            int segCountBefore;
            GetSegmentCount(TMName, out segCountBefore);

            if (segCountBefore == 0)
            {
                // Загрузить первый TMX, чтобы увеличить количество сегментов
                UploadDocumentToTMbyButton(TMName, "js-add-tmx-btn", TmFile, false);
                // Получить новое количество сегментов
                GetSegmentCount(TMName, out segCountBefore);
            }

            // Загрузить TMX
            if (UploadDocumentToTMbyButton(TMName, "js-add-tmx-btn", SecondTmFile, false))
            {
                // Получить количество сегментов после загрузки TMX
                int segCountAfter;
                GetSegmentCount(TMName, out segCountAfter);

                // Проверить, что количество сегментов увеличилось (ри AddTMX количество сегментов должно суммироваться)
                Assert.IsTrue(segCountAfter > segCountBefore);
            }
            // Иначе - документ не загружен, т.к. был загружен ранее - тест успешен
        }

        public void EditTMSaveWithoutName()
        {
            string TMName = _constTmName;
            // Изменить имя на пустое и сохранить
            EditTMFillName(TMName, "");

            // Получить xPath формы редактирования ТМ
            string xPath = TMName;
            CreateXPathTMRow(ref xPath);
            xPath += "/../../following-sibling::tr[contains(@class, 'js-editing')]";

            // Проверить, что поле Имя выделено ошибкой
            string nameErrorXPath = xPath + "//input[contains(@class, 'js-tm-name error')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(nameErrorXPath)).Displayed,
                "Ошибка: поле Имя не отмечено ошибкой");

            // Проверить, что появилось сообщение об ошибке в имени
            string errorInfoPath = xPath +
                "//div[contains(@class, 'js-dynamic-errors')]//p[contains(@class, 'js-error-tm-name-required')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(errorInfoPath)).Displayed,
                "Ошибка: не появилось сообщение о пустом имени");
        }

        public void EditTMSaveExistingName()
        {
            string TMName = _constTmName;
            // Создать ТМ с таким именем, если его еще нет
            CreateTMIfNotExist(TMName);

            // Выбрать уникальное имя TM
            string uniqueTMName;
            SelectUniqueTMName(out uniqueTMName);
            // Создать ТМ
            CreateTMByNameAndSave(uniqueTMName);

            // Изменить имя на существующее и сохранить
            EditTMFillName(uniqueTMName, TMName);

            // Проверить, что появилось сообщение об ошибке в имени
            string xPath = uniqueTMName;
            CreateXPathTMRow(ref xPath);
            xPath += "/../../following-sibling::tr[contains(@class, 'js-editing')]";
            string errorInfoPath = xPath +
                "//div[contains(@class,'js-dynamic-errors')]//p[contains(@data-key,'name')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(errorInfoPath)).Displayed,
                "Ошибка: не появилось сообщение об ошибке в имени");
        }

        public void EditTMSaveUniqueName()
        {
            string TMName = _constTmName;
            // Создать ТМ с таким именем, если его еще нет
            CreateTMIfNotExist(TMName);
            // Выбрать уникальное имя TM
            string uniqueTMName;
            SelectUniqueTMName(out uniqueTMName);

            // Изменить имя на уникальное и сохранить
            EditTMFillName(TMName, uniqueTMName);

            // Проверить, что ТМ со старым именем удалился, а с новым именем есть
            Assert.IsTrue(!GetIsExistTM(TMName), "Ошибка: не удалилось старое имя");
            Assert.IsTrue(GetIsExistTM(uniqueTMName), "Ошибка: нет ТМ с новым именем");
        }

        private void EditTMFillName(string TMNameToEdit, string newTMName)
        {
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMNameToEdit, "js-edit-btn");

            string xPath = TMNameToEdit;
            CreateXPathTMRow(ref xPath);
            xPath += "/../../following-sibling::tr[contains(@class, 'js-editing')]";
            // Ждем открытия формы редактирования
            Wait.Until((d) => d.FindElement(By.XPath(xPath)));

            // Очистить поле Имя
            string nameXPath = xPath + "//input[contains(@class, 'js-tm-name')]";
            Driver.FindElement(By.XPath(nameXPath)).Clear();

            // Если новое имя не пустое, то заполнить им поле Имя
            if (newTMName.Length > 0)
            {
                Driver.FindElement(By.XPath(nameXPath)).SendKeys(newTMName);
            }

            // Сохранить изменение
            string saveXPath = xPath + "//span[contains(@class, 'js-save-btn')]";
            Driver.FindElement(By.XPath(saveXPath)).Click();

            // Ответ формы
            Thread.Sleep(2000);
        }

        private void OpenCreateTMForm()
        {
            // Нажать кнопку Создать TM
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'l-corpr__addbtnbox')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            // ждем загрузку формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]")));
        }

        private void SelectUniqueTMName(out string TMName)
        {
            // Выбрать уникальное имя ТМ
            TMName = "TestTM";
            while (GetIsExistTM(TMName))
            {
                TMName += DateTime.Now.ToString();
            }
        }

        private bool GetIsExistTM(string TMName)
        {
            // Есть ли ТМ с таким именем в списке на странице Translation Memory Bases
            return GetIsExistTMInCurrentList(TMName, "//tr[contains(@class, 'js-tm-row')]/td/span");
        }

        private bool GetIsExistTMCreateProject(string TMName)
        {
            // Перейти на страницу SmartCAT
            Driver.FindElement(By.XPath(
                ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Workspace')]")).Click();
            Wait.Until((d) => d.FindElement(By.Id("projects-add-btn")));

            // Начать создание проекта
            FirstStepProjectWizard(ProjectName);
            Driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
            // Дождаться появления списка ТМ
            Wait.Until((d) => d.FindElement(By.Id("project-wizard-tms-body")).Displayed);

            // Есть ли ТМ с таким именем в списке при создании проекта
            return GetIsExistTMInCurrentList(TMName, "//div[@id='project-wizard-tms-body']//table//tr/td[2]/div");
        }

        private bool GetIsExistTMInCurrentList(string TMName, string xPathList)
        {
            // Проверить, что ТМ с этим именем существует
            bool isExist = false;
            IList<IWebElement> TMNames = Driver.FindElements(By.XPath(xPathList));
            foreach (IWebElement el in TMNames)
            {
                if (el.Text == TMName)
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }

        private void CreateTMByNameAndSave(string TMName)
        {
            // Создать ТМ без сохранения формы
            CreateTMByName(TMName);

            // Нажать кнопку Сохранить
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'g-btn__text')]")).Click();
            // Закрытие формы
            Thread.Sleep(5000);
        }

        private void CreateTMByName(string TMName)
        {
            // Открыть форму создания ТМ
            OpenCreateTMForm();

            // Ввести имя
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@class,'l-createtm__nmtext')]")).
                SendKeys(TMName);

            // Выбрать языки (source и target), чтобы сохранить ТМ
            SelectSourceAndTargetLang();
        }

        private void SelectSourceAndTargetLang()
        {
            // Нажать на Source Language для выпадения списка языков
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//span[contains(@class,'l-createtm__srclnl_drpdwn')]")).Click();
            // ждем выпадения списка
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]")));
            // Выбираем Английский
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__item')][@data-id='en']")).Click();

            // Нажать на Target Language для выпадения списка языков
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'js-languages-multiselect')]")).Click();
            // ждем выпадения списка
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'ui-multiselect-menu')][contains(@class,'js-languages-multiselect')]")));
            // Выбираем Русский (№27)
            Driver.FindElement(By.XPath(
                ".//li/label[contains(@for,'ui-multiselect-targetLanguages-option-27')]")).Click();
            // Нажать на Target Language для закрытия списка
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'js-languages-multiselect')]")).Click();
        }

        private bool UploadDocumentTM(string documentName)
        {
            // Загружен ли документ
            bool isUpload = true;

            // Нажать на Add для появления диалога загрузки документа
            Driver.FindElement(By.XPath(
                ".//a[contains(@class,'js-upload-btn')]")).Click();

            // Заполнить диалог загрузки документа
            FillAddDocumentForm(documentName);
            // Нажать на Импорт
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-import')][2]//span[contains(@class,'g-btn__data')]//a")).Click();

            // Проверка, есть ли ошибка, что TMX уже был загружен
            IList<IWebElement> errorEl = Driver.FindElements(By.XPath(".//p[contains(@class, 'js-error-from-server')]"));
            if (errorEl.Count > 0)
            {
                // Документ не загружен
                isUpload = false;
            }

            // Закрытие формы
            Thread.Sleep(5000);
            return isUpload;
        }

        private bool UploadDocumentToTMbyButton(string TMName, string btnName, string uploadFile, bool isNeedOpenInfo = true)
        {
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, btnName, isNeedOpenInfo);

            // Нажимаем Import
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-import')][2]//span[contains(@class,'g-btn__data')]"))).Click();

            // Подождать появление ошибки
            Thread.Sleep(2000);
            // Проверить появление ошибки
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-import')][2]//div[contains(@class,'g-popupbox__error')]")).Displayed,
                "Ошибка: не появилось оповещение об ошибке, что файл не выбран");

            // Загрузить документ
            return UploadDocumentTM(uploadFile);
        }

        private void GetSegmentCount(string TMName, out int segCount)
        {
            // Открыть информацию о ТМ
            ClickTMToShowInfo(TMName);

            string xPath = TMName;
            CreateXPathTMRow(ref xPath);
            xPath += "/../../following-sibling::tr//table//tr/td[2]/div[4]";
            string segmentsCount = Driver.FindElement(By.XPath(xPath)).Text;
            // Нужно получить число сегментов из строки "Segments count: N", разделитель - ":"
            int splitIndex = segmentsCount.IndexOf(":");
            // Отступаем двоеточие и пробел
            splitIndex += 2;
            if (segmentsCount.Length > splitIndex)
            {
                segmentsCount = segmentsCount.Substring(splitIndex);
            }
            // Получить число сегментов из строки
            segCount = int.Parse(segmentsCount);
        }

        private void ClickButtonTMInfo(string TMName, string btnName, bool isNeedOpenInfo = true)
        {
            if (isNeedOpenInfo)
            {
                // Открыть информацию о ТМ
                ClickTMToShowInfo(TMName);
            }

            string xPath = TMName;
            // Получить xPath строки нужного ТМ
            CreateXPathTMRow(ref xPath);
            // Получить xPath нужной кнопки открытой информации нужного ТМ
            xPath += "/../../following-sibling::tr//span[contains(@class,'" + btnName + "')]";

            // Нажать на нужную кнопку
            Driver.FindElement(By.XPath(xPath)).Click();
        }

        private void ClickTMToShowInfo(string TMName)
        {
            // Если такого TM нет - создать его
            CreateTMIfNotExist(TMName);

            // Получить xPath строки с этим ТМ
            string xPath = TMName;
            CreateXPathTMRow(ref xPath);

            // Открыть информацию о ТМ
            Driver.FindElement(By.XPath(xPath)).Click();
            // Подождать открытие информации
            Thread.Sleep(2000);
        }

        private void CreateTMIfNotExist(string TMName)
        {
            if (!GetIsExistTM(TMName))
            {
                // Если нет такого ТМ, создать  его
                CreateTMByNameAndSave(TMName);
            }
        }

        private void CreateXPathTMRow(ref string rowTMName)
        {
            // Получить xPath строки, содержащей имя TM (поиск по имени)
            rowTMName = ".//tr[contains(@class, 'js-tm-row')]/td/span[text()='" + rowTMName + "']";
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
}
