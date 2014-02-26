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
        private string _glossaryName;
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

        protected string GlossaryName
        {
            get
            {
                return _glossaryName;
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

        protected string ConstTMName
        {
            get
            {
                return _constTmName;
            }
        }

        private string _importGlossaryFile;

        protected string ImportGlossaryFile
        {
            get
            {
                return _importGlossaryFile;
            }
        }

        private string _imageFile;

        protected string ImageFile
        {
            get
            {
                return _imageFile;
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
                    _profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "text/xml, text/csv, text/plain, text/log, application/zip, application/x-gzip, application/x-compressed, application/x-gtar, multipart/x-gzip, application/tgz, application/gnutar, application/x-tar, application/pdf, application/octet-stream");
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
            _documentFile = Path.GetFullPath(ConfigurationManager.AppSettings["DocumentFile"]);
            _tmName = ConfigurationManager.AppSettings["TMName"];
            _constTmName = ConfigurationManager.AppSettings["TMName"];
            _glossaryName = ConfigurationManager.AppSettings["GlossaryName"];

            _projectName += " " + DateTime.UtcNow.Ticks.ToString();
            _tmName += " " + DateTime.UtcNow.Ticks.ToString();
            _tmFile = Path.GetFullPath(ConfigurationManager.AppSettings["TMXFile"]);
            _secondTmFile = Path.GetFullPath(ConfigurationManager.AppSettings["SecondTMXFile"]);
            _importGlossaryFile = Path.GetFullPath(ConfigurationManager.AppSettings["ImportGlossaryFile"]);
            _imageFile = Path.GetFullPath(ConfigurationManager.AppSettings["TestImageFile"]);

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

        protected string GetUserNameProfile()
        {
            string userName = "";
            // Нажать на Профиль
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-link-profile')]")).Click();
            // Дождаться открытия окна с профилем
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'g-popupbox g-profile')]")).Displayed);
            // Получить имя пользователя
            userName = Driver.FindElement(By.XPath(
                ".//div[contains(@class,'g-popupbox g-profile')]//p[contains(@class, 'g-profile__cntrl name')]//input")).GetAttribute("value");
            // Закрыть профиль
            Driver.FindElement(By.XPath(".//div[contains(@class,'g-popupbox g-profile')]//span[contains(@class, 'js-popup-close')]")).Click();
            Thread.Sleep(2000);

            return userName;
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
