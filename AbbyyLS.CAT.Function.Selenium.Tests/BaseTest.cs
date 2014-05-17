//#define OLD_WORKSPACE // включить, если тестируется старый WorkSpace
#define NEW_WORKSPACE // включить, если тестируется новый WorkSpace

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
using System.Drawing.Imaging;

using OpenQA.Selenium.Interactions;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    [TestFixture("DevUrl", "DevWorkspace", "Firefox")]
    [TestFixture("StableUrl2", "StableWorkspace2", "Firefox")]
    [TestFixture("StableUrl2", "StableWorkspace2", "Chrome")]
    [TestFixture("StableUrl2", "StableWorkspace2", "IE")]
    [TestFixture("StageUrl2", "StageWorkspace2", "Firefox")]
    [TestFixture("StageUrl3", "DevWorkspace", "Firefox")]
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
        protected string Login
        {
            get
            {
                return _login;
            }
        }

        private string _password;
        protected string Password
        {
            get
            {
                return _password;
            }
        }
        private string _projectName;
        private string _tmName;
        private string _constTmName;
        private string _glossaryName;

        private string _adminUrl;
        protected string AdminUrl
        {
            get
            {
                return _adminUrl;
            }
        }

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

        private string _documentFileToConfirm;
        protected string DocumentFileToConfirm
        {
            get
            {
                return _documentFileToConfirm;
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
        private string _tmFile2;
        protected string TmFile2
        {
            get
            {
                return _tmFile2;
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

        private string _editorTXTFile;
        protected string EditorTXTFile
        {
            get
            {
                return _editorTXTFile;
            }
        }

        private string _editorTMXFile;
        protected string EditorTMXFile
        {
            get
            {
                return _editorTMXFile;
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

        private string _audioFile;

        protected string AudioFile
        {
            get
            {
                return _audioFile;
            }
        }

        private string _browserName;

        private string BrowserName
        {
            get
            {
                return _browserName;
            }
        }

        // информация о тесте
        DateTime testBeginTime;


        public BaseTest(string url, string workspaceUrl, string browserName)
        {
            _browserName = browserName;
            _url = ConfigurationManager.AppSettings[url];

            CreateDriver();

            _login = ConfigurationManager.AppSettings["Login"];
            _password = ConfigurationManager.AppSettings["Password"];
            _deadlineDate = ConfigurationManager.AppSettings["DeadlineDate"];
            _documentFile = Path.GetFullPath(ConfigurationManager.AppSettings["DocumentFile"]);
            _documentFileToConfirm = Path.GetFullPath(ConfigurationManager.AppSettings["DocumentFileToConfrim"]);
            _constTmName = ConfigurationManager.AppSettings["TMName"];
            _glossaryName = ConfigurationManager.AppSettings["GlossaryName"];

            CreateUniqueNamesByDatetime();
            
            _tmFile = Path.GetFullPath(ConfigurationManager.AppSettings["TMXFile"]);
            _tmFile2 = Path.GetFullPath(ConfigurationManager.AppSettings["TMXFile2"]);

            _editorTXTFile = Path.GetFullPath(ConfigurationManager.AppSettings["EditorTXTFile"]);
            _editorTMXFile = Path.GetFullPath(ConfigurationManager.AppSettings["EditorTMXFile"]);
             
            _secondTmFile = Path.GetFullPath(ConfigurationManager.AppSettings["SecondTMXFile"]);
            _importGlossaryFile = Path.GetFullPath(ConfigurationManager.AppSettings["ImportGlossaryFile"]);
            _imageFile = Path.GetFullPath(ConfigurationManager.AppSettings["TestImageFile"]);
            _audioFile = Path.GetFullPath(ConfigurationManager.AppSettings["TestAudioFile"]);

            _adminUrl = ConfigurationManager.AppSettings[(url + "Admin")];
        }

        private void CreateUniqueNamesByDatetime()
        {
            _projectName = ConfigurationManager.AppSettings["ProjectName"] + " " + DateTime.UtcNow.Ticks.ToString();
            _tmName = ConfigurationManager.AppSettings["TMName"] + " " + DateTime.UtcNow.Ticks.ToString();
        }

        /// <summary>
        /// Метод создания _driver 
        /// </summary>
        private void CreateDriver()
        {
            if (BrowserName == "Firefox")
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
                    _profile.SetPreference
                        ("browser.helperApps.neverAsk.saveToDisk", "text/xml, text/csv, text/plain, text/log, application/zip, application/x-gzip, application/x-compressed, application/x-gtar, multipart/x-gzip, application/tgz, application/gnutar, application/x-tar, application/x-xliff+xml,  application/msword.docx, application/pdf, application/x-pdf, application/octetstream, application/x-ttx, application/x-tmx, application/octet-stream");
                    //_profile.SetPreference("pdfjs.disabled", true);

                    _driver = new FirefoxDriver(_profile);
                    //string profiledir = "../../../Profile";
                    // string profiledir = "TestingFiles/Profile";
                    //_profile = new FirefoxProfile(profiledir);
                    //_driver = new FirefoxDriver(_profile);
                }
            }
            else if (BrowserName == "Chrome")
            {
                _driver = new ChromeDriver();
            }
            else if (BrowserName == "IE")
            {
                //TODO: Сделать запуск из IE
            }

            setDriverTimeoutDefault();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            _driver.Manage().Window.Maximize();
        }

        protected void setDriverTimeoutMinimum()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
        }

        protected void setDriverTimeoutDefault()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
        }



#if OLD_WORKSPACE // тестируется версия со старой страницей проекта

        /// <summary>
        /// Метод назначения задачи на пользователя 
        /// </summary>
        protected void AssignTask()
        {

            // выбрать проект
            Wait.Until((d) => d.FindElement(By.LinkText(_projectName))).Click();

            // TODO проверку, на старой странице или на новой

            // нажать на Progress
            _wait.Until((d) => d.FindElement(By.CssSelector(".project-documents div.x-grid-body table tr:nth-child(1) td:nth-child(1)")));
            Thread.Sleep(500);
            _wait.Until((d) => d.FindElement(By.CssSelector(".project-documents div.x-grid-body table tr:nth-child(1) td:nth-child(1)")));
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
            Thread.Sleep(500);
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

#elif NEW_WORKSPACE // тестируется версия с новой страницей проекта

        /// <summary>
        /// Метод назначения задачи на пользователя 
        /// </summary>
        protected void AssignTask()
        {
            // нажать галочку около документа
            _wait.Until((d) => d.FindElement(By.XPath(
                ".//table[contains(@class,'js-documents-table')]//tr[contains(@class,'js-document-row')]/td[contains(@class,'js-checkbox-area')]/span")));
            _driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-documents-table')]//tr[contains(@class,'js-document-row')]/td[contains(@class,'js-checkbox-area')]/span")).Click();
            // нажать на Progress
            _driver.FindElement(By.XPath(".//span[contains(@class,'js-document-progress')]")).Click();
            WaitUntilDisplayElement(".//div[contains(@class,'js-popup-progress')][2]");

            // Назначить ответственного в окне Progress
            // Ввести нужное имя
            _driver.FindElement(By.XPath(".//table[contains(@class,'js-progress-table')]//tr[1]//td[3]//span")).Click();
            _wait.Until(d => _driver.FindElements(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]"
                )));
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__item')][@title='Bob Test']")).Click();
            // Нажать на Assign
            _wait.Until(d => _driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-progress-table')]//tr[1]//td[4]//span[contains(@class,'js-assign')]"
                ))).Click();
            WaitUntilDisplayElement(".//span[contains(@class,'js-assigned-cancel')]");
            // Нажать на Close
            _driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-progress')][2]//span[contains(@class,'js-popup-close')]")).Click();

            // Обновить страницу, чтобы активен был переход в редактор
            _driver.FindElement(By.XPath(
               ".//table[contains(@class,'js-documents-table')]")).SendKeys(OpenQA.Selenium.Keys.F5);

            /*// Вернуться на главную страницу
            _driver.FindElement(By.XPath(".//a[contains(@href,'/Workspace')]")).Click();
            WaitProjectPageOpen();

            // Открыть информацию о проекте
            ClickProjectOpenInfo(_projectName);
            // Нажать на Accept
            WaitAndClickElement(".//tr[contains(@class,'js-project-panel')]/following-sibling::tr[contains(@class,'js-document-row')]//span[contains(@class,'js-accept')]//a");
           
            // Перейти в проект
            _driver.FindElement(By.XPath(".//a[@class='js-name'][contains(text(),'" + ProjectName + "')]")).Click();*/
        }

#endif

        /// <summary>
        /// Метод открытия документа в редакторе
        /// </summary>
        /// <param name="projectname">имя открываемого проекта</param>
        protected void OpenDocument(string projectname)
        {           
            // Открыть документ
            _driver.FindElement(By.XPath(".//a[contains(@class,'js-editor-link')]")).Click();

            // Дождаться загрузки страницы
            _wait.Until((d) => d.Title.Contains("Editor"));

            // Проверить, существует ли хотя бы один сегмент
            _driver.FindElement(By.CssSelector(
                "#segments-body div table tr:nth-child(1)"
                ));
        }
#if OLD_WORKSPACE
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
#elif NEW_WORKSPACE
        public void Authorization(string accountName = "TestAccount")
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
            _driver.FindElement(By.XPath(".//select/option[contains(text(), '" + accountName + "')]")).Click();
            // Зайти на сайт
            _driver.FindElement(By.CssSelector("input[type = \"submit\"]")).Click();

            Wait.Until((d)=>d.FindElement(By.XPath(".//a[contains(@class,'js-set-locale')]")));
            setDriverTimeoutMinimum();
            if (IsElementPresent(By.XPath(".//a[contains(@class,'js-set-locale')][@data-locale='en']")))
            {
                _driver.FindElement(By.XPath(".//a[contains(@class,'js-set-locale')][@data-locale='en']")).Click();
            }
            setDriverTimeoutDefault();

        }
#endif

#if OLD_WORKSPACE
        protected void WaitProjectPageOpen()
        {
            Wait.Until((d) => d.FindElement(By.Id("projects-add-btn")));
        }
#elif NEW_WORKSPACE
        protected void WaitProjectPageOpen()
        {
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-project-create')]")));
        }
#endif


#if OLD_WORKSPACE
        protected void FirstStepProjectWizard(string projectName, bool newWorkspaceFlag = true)
        {
            Assert.IsTrue(_driver.FindElement(By.Id("projects-add-btn")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.Id("projects-add-btn")).Click();

            //ждем загрузки формы

            _wait.Until((d) => d.FindElement(By.Id("project-wizard")));

            //заполнение полей на 1 шаге
            FillProjectNameInForm(projectName);

            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).Clear();
            _driver.FindElement(By.CssSelector("input[name=\"DeadlineDate\"]")).SendKeys(_deadlineDate);
        }

        protected void FillProjectNameInForm(string projectName)
        {
            //заполнение полей на 1 шаге
            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).Clear();

            _driver.FindElement(By.CssSelector("input[name=\"Name\"]")).SendKeys(projectName);
        }
#elif NEW_WORKSPACE
        protected void FirstStepProjectWizard(string projectName, bool isNeedDifferentLang = true)
        {
            Assert.IsTrue(_driver.FindElement(By.XPath(".//span[contains(@class,'js-project-create')]")).Displayed);
            //нажать <Create>
            _driver.FindElement(By.XPath(".//span[contains(@class,'js-project-create')]")).Click();

            //ждем загрузки формы

            _wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]")).Displayed);

            //заполнение полей на 1 шаге
            FillProjectNameInForm(projectName);

            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[@name='deadlineDate']")).Clear();
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[@name='deadlineDate']")).SendKeys(_deadlineDate);

            // Выбираем языки
            // Source - En
            _driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-dropdown')]//span[contains(@class,'js-dropdown__text')]")).Click();
            _driver.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__list')]//span[@data-id='9']")).Click();

            if (!isNeedDifferentLang)
            {
                // Target
                _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-languages-multiselect')]")).Click();
                // Убираем русский
                _driver.FindElement(By.XPath(
                    ".//ul[contains(@class,'ui-multiselect-checkboxes')]//li//span[contains(@class,'js-chckbx')]//input[@value='25']")).Click();
                // Добавляем английский
                _driver.FindElement(By.XPath(
                    ".//ul[contains(@class,'ui-multiselect-checkboxes')]//li//span[contains(@class,'js-chckbx')]//input[@value='9']")).Click();
                _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-languages-multiselect')]")).Click();
            }
        }

        protected void FillProjectNameInForm(string projectName)
        {
            //заполнение полей на 1 шаге
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[@name='name']"));
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[@name='name']")).Clear();
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//input[@name='name']")).SendKeys(projectName);
        }

#endif

#if OLD_WORKSPACE
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
#elif NEW_WORKSPACE
        protected void CreateProject(string ProjectName, bool FileFlag, string DocumentName, bool chooseMT = true)
        {
            FirstStepProjectWizard(ProjectName);

            AddDocument(FileFlag, DocumentName);

            //2 шаг - выбор ТМ
            ChooseExistingTM();

            //3 шаг - выбор глоссария
            ChooseGlossary();

            //4 шаг - выбор МТ
            if (chooseMT)
            {
                ChooseMTCompreno();
            }
            else
            {
                ChooseNoMT();                
            }

            //5 шаг - настройка этапов workflow
            SetUpWorkflow();

            //Finish
            Pretranslate();
        }
#endif

        /// <summary>
        /// метод открытия настроек проекта (последнего в списке) и загрузки нового документа
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        protected void ImportDocumentProjectSettings(string filePath, string projectName)
        {
            // Зайти в проект
            ClickProjectInList(projectName);

            //ждем когда окно с настройками загрузится
            WaitUntilDisplayElement(".//span[contains(@class,'js-document-import')]");

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
            WaitUntilDisplayElement(".//table[contains(@class,'js-tms-table')]");
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]//table[contains(@class,'js-tms-table')]//tbody//tr[1]//td[5]//input")).Click();
            // Нажать Next
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]//div[contains(@class,'l-project-section')]")).Displayed);
            Driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import-document')][2]//span[contains(@class,'js-next')]")).Click();
            // Нажать Finish
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-step last active')]")));
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-finish js-upload-btn')]")).Click();
            // Дождаться загрузки документа
            Wait.Until((d) => d.FindElement(By.XPath(".//img[contains(@title,'Job processing')]")).Displayed);
            bool isDisappeared = false;
            for (int i = 0; i < 10; ++i)
            {
                isDisappeared = WaitUntilDisappearElement(".//img[contains(@title,'Job processing')]", 40);
                if (isDisappeared)
                {
                    break;
                }
            }
            Assert.IsTrue(isDisappeared, "Ошибка: файл так и не загрузился");
        }

        /// <summary>
        /// Открыть страницу Workspace
        /// </summary>
        protected void OpenMainWorkspacePage()
        {
            Driver.FindElement(By.XPath(".//a[contains(@href,'/Workspace')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-project-create')]")));
        }

        protected void CreateProject(string ProjectName, bool FileFlag, string DocumentName, string TmName, bool chooseMT = true)
        {
            FirstStepProjectWizard(ProjectName);

            AddDocument(FileFlag, DocumentName);

            //2 шаг - создание ТМ
            ChooseCreatedTM(TmName);

            //3 шаг - выбор глоссария
            ChooseGlossary();

            //4 шаг - выбор МТ
            if (chooseMT)
            {
                ChooseMTCompreno();
            }
            else
            {
                ChooseNoMT();
            }

            //5 шаг - настройка этапов workflow
            SetUpWorkflow();

            //Finish
            Pretranslate();
        }

#if OLD_WORKSPACE
        public void AddDocument(bool FileFlag, string DocumentName)
        {
            if (FileFlag == true)
            {
                //процесс добавления файла 
                //нажатие кнопки Add
                _driver.FindElement(By.XPath(".//div[@id='project-wizard-form-files-containerEl']//span[text()='Add']")).Click();

                FillAddDocumentForm(DocumentName);
            }

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-form']//span[contains(text(), 'Next')]")).Click();
        }
#elif NEW_WORKSPACE
        public void AddDocument(bool FileFlag, string DocumentName)
        {
            if (FileFlag == true)
            {
                //процесс добавления файла 
                //нажатие кнопки Add
                _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-files-uploader')]//a[contains(@class,'js-add-file')]")).Click();

                FillAddDocumentForm(DocumentName);
            }

            ClickNext();
        }
#endif
        protected void FillAddDocumentForm(string DocumentName)
        {
            Thread.Sleep(3000);
            // Заполнить форму для отправки файла
            SendKeys.SendWait(DocumentName);
            Thread.Sleep(5000);
            SendKeys.SendWait(@"{Tab}");
            Thread.Sleep(1000);
            SendKeys.SendWait(@"{Tab}");
            Thread.Sleep(1000);
            SendKeys.SendWait(@"{Enter}");

            Thread.Sleep(3000);
        }

#if OLD_WORKSPACE
        public void ChooseExistingTM()
        {
            //Выбрать существующий TM
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1) td:nth-child(2)")));

            _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1) td:nth-child(2)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tms']//span[contains(text(), 'Next')]")).Click();
        }
#elif NEW_WORKSPACE
        public void ChooseExistingTM()
        {
            //Выбрать существующий TM
            _wait.Until(d => _driver.FindElement(By.XPath(".//table[contains(@class,'js-tms-popup-table')]")));

            _driver.FindElement(By.XPath(".//table[contains(@class,'js-tms-popup-table')]//tbody//tr[1]//td[1]//input")).Click();

            ClickNext();
        }
#endif


        public void ChooseCreatedTM(string TmName)
        {
            //Создать TM
            //_wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-tms table tr:nth-child(1)")));

            CreateTMXFile(TmName);

            ClickNext();
        }


        public void ChooseMT()
        {
            //Выбрать необходимые MT
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(1) td:nth-child(1)")).Click();

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")).Click();

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(3) td:nth-child(1)")).Click();

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(4) td:nth-child(1)")).Click();

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(5) td:nth-child(1)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
        }

        public void ChooseMTBing()
        {
            //Выбрать MT Bing
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(3) td:nth-child(1)")).Click();
           
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
        }

#if OLD_WORKSPACE
        public void ChooseMTCompreno()
        {
            //Выбрать MT Compreno
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(1) td:nth-child(1)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
        }

        public void ChooseNoMT()
        {
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-mts']//span[contains(text(), 'Next')]")).Click();
        }
#elif NEW_WORKSPACE
        public void ChooseMTCompreno()
        {
            //Выбрать MT Compreno
            _driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-mts-table')]//tbody//tr[3]//input")).Click();
            ClickNext();
        }

        public void ChooseNoMT()
        {
            ClickNext();
        }

#endif
        public void ChooseMTGoogle()
        {
            //Выбрать MT Google
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-mts table tr:nth-child(2) td:nth-child(1)")).Click();

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

#if OLD_WORKSPACE
        public void ChooseGlossary()
        {
            //Выбрать необходимый глоссарий
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-tbs table tr:nth-child(1) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-tbs table tr:nth-child(1) td:nth-child(1)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tbs']//span[contains(text(), 'Next')]")).Click();
        }

        public void ChooseFirstGlossary()
        {
            //Выбрать необходимый глоссарий
            _wait.Until(d => _driver.FindElement(By.CssSelector("#project-wizard-tbs table tbody tr:nth-child(1) td:nth-child(1)")));

            _driver.FindElement(By.CssSelector("#project-wizard-tbs table tbody tr:nth-child(1) td:nth-child(1)")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tbs']//span[contains(text(), 'Next')]")).Click();
        }

        public void ClickNext()
        {
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-tbs']//span[contains(text(), 'Next')]")).Click();
        }

#elif NEW_WORKSPACE
        public void ChooseGlossary()
        {
            //Выбрать необходимый глоссарий
            //_driver.FindElement(By.XPath(
                //".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-glossaries-table')]//span[contains(@class,'js-chckbx')]")).Click();

            ClickNext();
        }

        public void ChooseFirstGlossary()
        {
            //Выбрать необходимый глоссарий
            _driver.FindElement(By.XPath(".//table[contains(@class,'js-glossaries-table')]//tbody//tr[1]//input")).Click();

            ClickNext();
        }

        public void ClickNext()
        {
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-next')]")).Click();
        }
#endif

#if OLD_WORKSPACE
        public void SetUpWorkflow()
        {
            //Настроить этапы workflow
            _wait.Until(d => _driver.FindElement(By.Id("project-workflow-new-stage-btn")));

            _driver.FindElement(By.Id("project-workflow-new-stage-btn")).Click();

            _driver.FindElement(By.XPath(".//div[@id='project-wizard-workflow']//span[contains(text(), 'Next')]")).Click();
        }
#elif NEW_WORKSPACE
        public void SetUpWorkflow()
        {
            //Настроить этапы workflow
            //_wait.Until(d => _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-new-stage')]")));
            //_driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-new-stage')]")).Click();

            ClickNext();
        }
#endif

#if OLD_WORKSPACE
        public void Pretranslate()
        {
            _wait.Until(d => _driver.FindElement(By.XPath(".//div[@id='project-wizard-pretranslate']//span[contains(text(), 'Finish')]")));
            _driver.FindElement(By.XPath(".//div[@id='project-wizard-pretranslate']//span[contains(text(), 'Finish')]")).Click();
        }
#elif NEW_WORKSPACE
        public void Pretranslate()
        {
            _wait.Until(d => _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-finish js-upload-btn')]")));
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-finish js-upload-btn')]")).Click();
        }
#endif

        public void CreateTMXFile(string TmName)
        {
            //Создать ТМ
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-create')]")).Click();

            _wait.Until(d => _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-tm')][2]")).Displayed);

            //Заполнить данные о новой ТМ
            IWebElement tmNameEl = _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@class,'js-tm-name')]"));
            tmNameEl.Clear();
            tmNameEl.SendKeys(_tmName);

            //Добавить тмх файл
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'js-save-and-import')]")).Click();
            _wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-import')][2]")).Displayed);
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import')][2]//a[contains(@class,'js-upload-btn')]")).Click();

            Thread.Sleep(1000);
            // Заполнить форму для отправки файла
            SendKeys.SendWait(TmName);
            Thread.Sleep(1000);
            SendKeys.SendWait(@"{Enter}");
            Thread.Sleep(2000);

            //Нажать на кнопку Import
            _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-import')][2]//span[contains(@class,'js-import-button')]")).Click();

            WaitUntilDisappearElement(".//div[contains(@class,'js-popup-import')][2]");
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
                SendKeys("Translation");

            // Нажать на кнопку подтвердить
            Driver.FindElement(By.Id("confirm-btn")).Click();
            // Убедиться что сегмент подтвержден
            Assert.IsTrue(WaitUntilDisappearElement(
                ".//div[@id='segments-body']//table//tr[1]//td[4]//span[contains(@class,'fa-border')]"),
                "Ошибка: рамка вокрут галочки не пропадает - Confirm не прошел");
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

            return userName;
        }

        protected void SwitchDomainTab()
        {
            // Перейти на страницу со списком глоссариев
            Driver.FindElement(By.XPath(
                ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Domains')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'g-btn g-redbtn js-add-domain')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Displayed);
        }

        protected void SwitchTMTab()
        {
            // Нажать кнопку перехода на страницу Базы Translation memory
            Driver.FindElement(By.XPath(
                ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Enterprise/TranslationMemories')]")).Click();

            // ждем загрузки страницы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'l-corpr__addbtnbox')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Displayed);
        }

        protected void SwitchGlossaryTab()
        {
            // Перейти на страницу со списком глоссариев
            Driver.FindElement(By.XPath(
                ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Glossaries')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//span[contains(@class,'js-create-glossary-button')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Displayed);
        }

        protected void SwitchSearchTab()
        {
            // Перейти на страницу поиска
            Driver.FindElement(By.XPath(
                ".//ul[@class='g-corprmenu__list']//a[contains(@href,'/Start')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//form[contains(@class,'js-search-form')]")).Displayed);
        }

        protected void InitSearch(string searchText)
        {
            // Ввести слово для поиска
            Driver.FindElement(By.XPath(".//form[contains(@class,'js-search-form')]//textarea[@id='searchText']")).Clear();
            Driver.FindElement(By.XPath(".//form[contains(@class,'js-search-form')]//textarea[@id='searchText']")).SendKeys(searchText);
            // Нажать Перевести
            Driver.FindElement(By.XPath(".//form[contains(@class,'js-search-form')]//span[contains(@class,'g-redbtn search')]//input")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-search-results')]")));
        }

        protected bool GetIsDomainExist(string domainName)
        {
            // Получить список всех проектов
            IList<IWebElement> projectsList = Driver.FindElements(By.XPath(
                ".//table[contains(@class,'js-domains js-sortable-table')]//tr[contains(@class,'l-corpr__trhover js-row')]"));
            bool bProjectExist = false;
            foreach (IWebElement el in projectsList)
            {
                // Проверить имя проекта
                if (el.Text == domainName)
                {
                    bProjectExist = true;
                    break;
                }
            }

            return bProjectExist;
        }

        protected void CreateDomain(string domainName, bool shouldCreateOk = true)
        {
            // Нажать "Добавить проект"
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'g-btn g-redbtn js-add-domain')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            string rowXPath = ".//table[contains(@class,'js-domains')]//tr[contains(@class,'l-corpr__trhover js-row')]";
            // Получить все строки в таблице с проектами
            IList<IWebElement> rowsList = Driver.FindElements(By.XPath(rowXPath));
            string tdXPath = "";
            for (int i = 0; i < rowsList.Count; ++i)
            {
                // Найти нескрытую строку для ввода имени нового проекта
                if (!rowsList[i].GetAttribute("class").Contains("g-hidden"))
                {
                    tdXPath = rowXPath + "[" + (i + 1) + "]//td[contains(@class,'js-cell')]";
                    if (Driver.FindElement(By.XPath(tdXPath)).GetAttribute("class").Contains("domainNew"))
                    {
                        string projectNameXPath = tdXPath +
                            "//div[contains(@class,'js-edit-mode')]//input[contains(@class,'js-domain-name-input')]";
                        // Ввести имя проекта
                        Driver.FindElement(By.XPath(projectNameXPath)).SendKeys(domainName);
                        break;
                    }
                }
            }

            // Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
            Driver.Manage().Window.Maximize();
            tdXPath += "//div[contains(@class,'l-corpr__domainbox js-edit-mode')]//a[contains(@class,'save js-save-domain')]";
            // Сохранить новый проект
            Driver.FindElement(By.XPath(tdXPath)).Click();
            if (shouldCreateOk)
            {
                WaitUntilDisappearElement(tdXPath);
            }
            else
            {
                Thread.Sleep(1000);
            }
        }

        protected void CreateDomainIfNotExist(string domainName)
        {
            if (!GetIsDomainExist(domainName))
            {
                // Если проект не найден, создать его
                CreateDomain(domainName);
            }
        }

        protected void WaitAndClickElement(string xPath)
        {
            // Дождаться появления элемента
            Wait.Until((d) => d.FindElement(By.XPath(xPath)).Displayed);
            // Кликнуть по элементу
            Driver.FindElement(By.XPath(xPath)).Click();
        }

        protected string GetCreateProjectTMListXPath()
        {
#if OLD_WORKSPACE
            Wait.Until((d) => d.FindElement(By.XPath(".//div[@id='project-wizard-tms-body']")).Displayed);
            return ".//div[@id='project-wizard-tms-body']//table//tr/td[2]/div";
#elif NEW_WORKSPACE
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]")).Displayed);
            return ".//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]//tr/td[contains(@class,'js-name')]";
#endif
        }

        protected void SelectProjectInList(string ProjectNameToSelect)
        {
            // Кликнуть по галочке в строке с проектом
            Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-tasks-table')]//tr//td[2]//a[@class='js-name'][text()='" + ProjectNameToSelect + "']/../../../td[1]")).Click();
        }

        protected void ClickProjectInList(string ProjectNameToClick)
        {
            // Кликнуть по названию проекта
            Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-tasks-table')]//tr//td[2]//a[@class='js-name'][text()='" + ProjectNameToClick + "']")).Click();
        }

        protected void ClickProjectOpenInfo(string ProjectNameToClick)
        {
            // Кликнуть на открытие информации о проекте
            Driver.FindElement(By.XPath(
                ".//table[contains(@class,'js-tasks-table')]//tr//td[2]//a[@class='js-name'][text()='" + ProjectNameToClick + "']/../../..//td[contains(@class,'openCloseCell')]")).Click();
        }

        protected bool WaitUntilDisappearElement(string xPath, int maxWaitSeconds = 10)
        {
            bool isDisplayed = false;
            TimeSpan timeBegin = DateTime.Now.TimeOfDay;
            setDriverTimeoutMinimum();
            do
            {
                isDisplayed = IsElementDisplayed(By.XPath(xPath));
                if (!isDisplayed || DateTime.Now.TimeOfDay.Subtract(timeBegin).Seconds > maxWaitSeconds)
                {
                    break;
                }
                Thread.Sleep(1000);
            } while (isDisplayed);
            setDriverTimeoutDefault();
            return !isDisplayed;
        }

        protected bool WaitUntilDisplayElement(string xPath)
        {
            try
            {
                Wait.Until((d) => d.FindElement(By.XPath(xPath)).Displayed);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка на наличие элемента на экране
        /// </summary>
        protected bool IsElementPresent(By by)
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

        /// <summary>
        /// Проверка на отображение элемента на экране
        /// </summary>
        protected bool IsElementDisplayed(By by)
        {
            try
            {
                return Driver.FindElement(by).Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка на доступность элемента
        /// </summary>
        protected bool IsElementEnabled(By by)
        {
            try
            {
                return Driver.FindElement(by).Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        protected void OpenCreateGlossary()
        {
            // Нажать кнопку Create a glossary
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-create-glossary-button')]//a[contains(@class,'g-btn__text g-redbtn__text')]")).Click();
            // ждем загрузку формы
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]")));
        }

        protected void CreateGlossaryByName(string glossaryName, bool bNeedWaitSuccessSave = true)
        {
            // Открыть форму создания глоссария
            OpenCreateGlossary();

            // Ввести имя
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@class,'js-glossary-name')]")).
                SendKeys(glossaryName);

            // Добавить комментарий
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-edit-glossary')][2]//textarea[contains(@name,'Comment')]")).
                SendKeys("Test Glossary Generated by Selenium");

            // Нажать сохранить
            Driver.FindElement(By.XPath(
               ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@class,'js-save')]")).Click();

            if (bNeedWaitSuccessSave)
            {
                // Ожидание успешного сохранения
                WaitUntilDisplayElement(".//span[contains(@class,'js-add-concept')]");
            }
            else
            {
                Thread.Sleep(1000);
            }
        }

        [SetUp]
        public void Setup()
        {
            // Вывести время начала теста
            testBeginTime = DateTime.Now;
            Console.WriteLine(TestContext.CurrentContext.Test.Name + "\nStart: " + testBeginTime.ToString());
            
            if (_driver == null)
            {
                // Если конструктор заново не вызывался, то надо заполнить _driver
                CreateDriver();
                // И заново создать уникальные названия
                CreateUniqueNamesByDatetime();
            }
            //_driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void Teardown()
        {
            // Если тест провалился
            if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
            {
                // Сделать скриншот
                ITakesScreenshot screenshotDriver = _driver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();

                // Создать папку для скриншотов провалившихся тестов
                string failResultPath = System.IO.Path.Combine(PathTestResults, "FailedTests");
                System.IO.Directory.CreateDirectory(failResultPath);
                // Создать имя скриншота по имени теста
                string screenName = TestContext.CurrentContext.Test.Name;
                if (screenName.Contains("("))
                {
                    // Убрать из названия теста аргументы (файлы)
                    screenName = screenName.Substring(0, screenName.IndexOf("("));
                }
                screenName += DateTime.Now.Ticks.ToString() + ".png";
                // Создать полное имя файла
                screenName = System.IO.Path.Combine(failResultPath, screenName);
                // Сохранить скриншот
                screenshot.SaveAsFile(screenName, ImageFormat.Png);
            }

            // Закрыть драйвер
            _driver.Quit();
            // Очистить, чтобы при следующем тесте пересоздавалось
            _driver = null;

            // Вывести информацию о прохождении теста
            DateTime testFinishTime = DateTime.Now;
            // Время окончания теста
            Console.WriteLine("Finish: " + testFinishTime.ToString());
            // Длительность теста
            TimeSpan duration = TimeSpan.FromTicks(testFinishTime.Ticks - testBeginTime.Ticks);
            string durResult = "Duration: ";
            if (duration.TotalMinutes > 1)
            {
                durResult += duration.TotalMinutes + "min";
            }
            else
            {
                durResult += duration.TotalSeconds + "sec";
            }

            durResult += " (" + duration.TotalMilliseconds + "ms).";
            Console.WriteLine(durResult);

            if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
            {
                // Если тест провалился
                Console.WriteLine("Fail!");
            }
        }
    }
}
