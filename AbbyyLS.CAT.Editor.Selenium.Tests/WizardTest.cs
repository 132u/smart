using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Text;

namespace AbbyyLs.CAT.Editor.Selenium.Tests
{

    /// <remarks>
    /// Класс, содержащий методы для тестирования визарда
    /// </remarks>

    [TestFixture]
    public class WizardTest
    {
        private IWebDriver _driver;
        private string _baseUrl;
        //private bool acceptNextAlert = true;
        private IJavaScriptExecutor _js;
        private string _documentName;
        private string _documentNameEqual;
        private string _tmName;
        private string _tmNameEqual;
        private string _documentFile;
        private string _tmFile;
        private string _userId;
        private string _login;
        private string _password;
        private string Path;

        ///<summary>
        ///Метод установки различныз значений для теста
        ///</summary>
       
        public void SetupTest()
        {
            //driver = new ChromeDriver();
            _driver = new FirefoxDriver();
            _js = (IJavaScriptExecutor)_driver;
            _baseUrl = "http://cat-dev:10085";
            _userId = "4";
            _login = "a.kurenkova@abbyy-ls.com";
            _password = "8i0fsbrs";
            _documentFile = @"\\cat-dev\Share\CAT\TestFiles\English.docx";
            _tmFile = @"\\cat-dev\Share\CAT\TestFiles\EN-Russian_ABBYY_Lingvo.tmx";
            Path = @"\\cat-dev\Share\CAT\TestResult\Result_" + DateTime.UtcNow.Ticks.ToString() + ".txt";
        }

        /// <summary>
        /// Метод создания файла для записи результатов тестирования
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
        /// Метод для тестирования авторизации в CAT
        /// </summary>
        [Test]
        public void AutorizationTest()
        {
            _driver.Navigate().GoToUrl(_baseUrl + "/");
            _driver.FindElement(By.Id("login")).Clear();
            _driver.FindElement(By.Id("login")).SendKeys(_login);
            _driver.FindElement(By.Id("password")).Clear();
            _driver.FindElement(By.Id("password")).SendKeys(_password);
            _driver.FindElement(By.CssSelector("input.btn")).Click();
            Thread.Sleep(Constants.Big);

            //Assert.AreEqual(baseURL + "/workspace/?UserId=" + UserId, driver.Url);
            //Console.WriteLine("The page is correct: " + driver.Url);
            //File.WriteAllText(Path, "The page is correct: " + driver.Url, Encoding.Default);
            WriteStringIntoFile("TestResults\n" + _driver.Url);
        }

        /// <summary>
        /// Метод для провекри создания документа со всеми настройками:
        /// Документ с англ на рус, ТМ создана вручную, МТ - первая из списка
        /// </summary>
        /// <param name=""></param>

        [Test]
        public void CreateDocumentSetupAllTest()
        {
            Console.WriteLine("Test - Create Document Setup All");
            WriteStringIntoFile("Test - Create Document Setup All");
            //File.WriteAllText(Path, "Test - CreateDocumentSetupAll", Encoding.Default);
            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            Console.WriteLine(_documentName);
            WriteStringIntoFile(_documentName);
            _tmName = "TM_" + DateTime.UtcNow.Ticks.ToString();
            //_tmName = "123412341";
            _documentNameEqual += _documentName;
            _tmNameEqual += _tmName;
            //1 шаг
            NavigatePage();

            //ждем загрузки страницы
            WaitPageUpload();

            //новый документ
            AddNewDocument();



            //ввод имен документа и файла
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            Thread.Sleep(Constants.Small);

            _driver.FindElement(By.Id("wizard-next")).Click();

            //проверка что сообщение с ошибкой появилось
            //Assert.IsFalse(driver.FindElement(By.CssSelector("#msg")).Displayed);
            //Console.WriteLine("The next step choost TM - OK");

            //шаг выбора ТМ
            CheckTMUpload();
            Thread.Sleep(Constants.Medium);
            //ищем кнопку <Add TM> 
            _driver.FindElement(By.Id("tms-add")).Click();

            //проверяем, что окно для добавления ТМ открыто
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-tm")).Displayed);
            Console.WriteLine("The add-tm window is open");
            WriteStringIntoFile("The add-tm window is open");
            //ввод имени ТМ, загрузка tmx файла
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).SendKeys(_tmName);

            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"fileConent\"]")).SendKeys(_tmFile);
            _driver.FindElement(By.Id("tm-accept")).Click();

            //ждем, что окно для добавления ТМ закрыто, окно шага выбора ТМ - открыто и прогрузилось
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TryMedium)
                    Assert.Fail("timeout");

                if (IsElementPresent(By.Id("wizard-next")))
                {
                    Console.WriteLine("button is here");
                    WriteStringIntoFile("button wizard-next is here");
                    break;
                }

                else
                {
                    Console.WriteLine("no button");
                    WriteStringIntoFile("no button");
                    break;
                }

            }
            Thread.Sleep(Constants.Big);

            _driver.FindElement(By.Id("wizard-next")).Click();

            Console.WriteLine("button next is pressed");
            WriteStringIntoFile("button next is pressed");
            Thread.Sleep(Constants.Big);
            //выбираем первую МТ из списка
            _driver.FindElement(By.CssSelector("#wizard-mts table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("wizard-next")).Click();
            //ждем загрузки
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("wizard-filemanager")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }

            Assert.IsTrue(_driver.FindElement(By.Id("wizard-filemanager")).Displayed);
            Console.WriteLine("Window with <start translation> button open");
            WriteStringIntoFile("Window with <start translation> button open");
            _driver.FindElement(By.Id("wizard-filemanager")).Click();

        }

        [Test]
        public void CreateDocumentDublicateNameTest()
        {
            Console.WriteLine("Test  - CreateDocumentDublicateName");
            WriteStringIntoFile("Test  - CreateDocumentDublicateName");
            Console.WriteLine(_documentNameEqual);
            WriteStringIntoFile(_documentNameEqual);

            NavigatePage();

            //wait page upload
            WaitPageUpload();

            //New
            AddNewDocument();


            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentNameEqual);
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Big);

            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The FAILURE appear - it's OK");
            WriteStringIntoFile("The FAILURE appear - it's OK");
        }

        [Test]
        public void CreateDocumentBigNameTest()
        {
            Console.WriteLine("Test  - CreateDocumentBigName (100 symbols)");
            WriteStringIntoFile("Test  - CreateDocumentBigName (100 symbols)");
            NavigatePage();
            WaitPageUpload();
            //New
            AddNewDocument();

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891");
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Small);

            //Assert.IsFalse(driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The failure do NOT appear - it's OK");
            //File.WriteAllText(Path, "The failure do NOT appear - it's OK", Encoding.Default);
        }

        [Test]
        public void CreateDocumentLimitFileNameTest()
        {
            Console.WriteLine("Test  - CreateDocumentLimitFileName (101 symbols)");
            WriteStringIntoFile("Test  - CreateDocumentLimitFileName (101 symbols)");
            NavigatePage();
            WaitPageUpload();
            //New
            AddNewDocument();

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678911");
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Small);

            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The failure appear - it's OK");
            WriteStringIntoFile("The failure appear - it's OK");
        }

        [Test]
        public void CreateDocumentWrongSymbolsTest()
        {
            Console.WriteLine("Test  - File Wrong Symbols");
            WriteStringIntoFile("Test  - File Wrong Symbols");
            NavigatePage();
            WaitPageUpload();
            //New
            AddNewDocument();

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys("Doc*|\\:\"<\\>?/");
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            Thread.Sleep(Constants.Small);

            Console.WriteLine("Doc name: " + _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).GetAttribute("value"));
            WriteStringIntoFile("Doc name: " + _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).GetAttribute("value"));
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).GetAttribute("value") == "Doc");
            Console.WriteLine("It's OK");
            WriteStringIntoFile("It's OK");
        }

        [Test]
        public void CreateDocumentEqualLanguagesTest()
        {
            Console.WriteLine("Test - Create File with Equal Languages");
            WriteStringIntoFile("Test - Create File with Equal Languages");
            NavigatePage();
            WaitPageUpload();
            AddNewDocument();

            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            Console.WriteLine(_documentName);
            WriteStringIntoFile(_documentName);
            //File.WriteAllText(Path, DocumentName, Encoding.Default);

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            Thread.Sleep(Constants.Small);


            //js.ExecuteScript("alert('!');");
            _js.ExecuteScript("document.querySelector('#wizard-upload input[name=\"TargetLanguage\"]').value=\"English\"; ");
            _js.ExecuteScript("document.querySelector('#wizard-upload input[name=\"SourceLanguage\"]').value=\"English\"; ");

            Thread.Sleep(Constants.Medium);
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-upload input[name=\"TargetLanguage\"]")).GetAttribute("value") == "English");
            Console.Write("Target=english   ");
            WriteStringIntoFile("Target=english");
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-upload input[name=\"SourceLanguage\"]")).GetAttribute("value") == "English");
            Console.Write("Source=english");
            WriteStringIntoFile("Source=english");

            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Medium);
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The FAILURE appear - it's OK");
            WriteStringIntoFile("The FAILURE appear - it's OK");

        }

        [Test]
        public void CreateDocumentEmptyNameTest()
        {
            Console.WriteLine("Test - Create Document Empty Name");
            WriteStringIntoFile("Test - Create Document Empty Name");
            NavigatePage();
            WaitPageUpload();
            AddNewDocument();

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys("");

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            Thread.Sleep(Constants.Small);

            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Medium);
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The FAILURE appear - it's OK");
            WriteStringIntoFile("The FAILURE appear - it's OK");
        }

        [Test]
        public void CreateDocumentEmptyPathTest()
        {
            Console.WriteLine("Test - Create Document Empty Path");
            WriteStringIntoFile("Test - Create Document Empty Path");
            NavigatePage();
            WaitPageUpload();
            AddNewDocument();

            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys("");
            Thread.Sleep(Constants.Small);

            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Small);
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The FAILURE appear - it's OK");
            WriteStringIntoFile("The FAILURE appear - it's OK");
        }

        [Test]
        public void CreateDocumentSpaceNameTest()
        {
            Console.WriteLine("Test - Create Document Empty Name");
            WriteStringIntoFile("Test - Create Document Empty Name");
            NavigatePage();
            WaitPageUpload();
            AddNewDocument();

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(" ");

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            Thread.Sleep(Constants.Small);

            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Medium);
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The FAILURE appear - it's OK");
            WriteStringIntoFile("The FAILURE appear - it's OK");
        }

        [Test]
        public void CreateDocumentCancelTest()
        {
            Console.WriteLine("Test - Create Document - Cancel process");
            WriteStringIntoFile("Test - Create Document - Cancel process");
            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            Console.WriteLine(_documentName);
            WriteStringIntoFile(_documentName);
            _tmName = " TM_" + DateTime.UtcNow.Ticks.ToString();
            _documentNameEqual += _documentName;

            //first step
            NavigatePage();

            //wait page upload
            WaitPageUpload();

            //New
            AddNewDocument();

            //input document name and file link
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            Thread.Sleep(Constants.Small);

            _driver.FindElement(By.Id("wizard-cancel")).Click();
        }
#warning не дописан - как проверить, что имя изменилось  - проверить что в списке док нет старого
        [Test]
        public void ChangeDocumentNameDuringCreationTest()
        {
            Console.WriteLine("Test - Change Document Name During Creation");
            WriteStringIntoFile("Test - Change Document Name During Creation");
            //имя документа "старого"
            _documentName = "OldDoc_" + DateTime.UtcNow.Ticks.ToString();
            Console.WriteLine(_documentName);
            WriteStringIntoFile(_documentName);
            _tmName = " TM_" + DateTime.UtcNow.Ticks.ToString();


            //first step
            NavigatePage();

            //wait page upload
            WaitPageUpload();

            //New
            AddNewDocument();

            //input document name and file link
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            Thread.Sleep(Constants.Small);
            //нажали next
            _driver.FindElement(By.Id("wizard-next")).Click();

            for (int second = 0; ; second++)
            {
                if (second >= Constants.TryBig) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("wizard-back")))
                    {
                        Console.WriteLine("Back is displayed");
                        WriteStringIntoFile("Back is displayed");
                        break;
                    }
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }
            Thread.Sleep(Constants.Small);
            //back
            _driver.FindElement(By.Id("wizard-back")).Click();
            Thread.Sleep(Constants.Small);
            //новые имена
            _documentName = "NewDoc_" + DateTime.UtcNow.Ticks.ToString();
            Console.WriteLine(_documentName);
            WriteStringIntoFile(_documentName);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);
            _driver.FindElement(By.Id("wizard-next")).Click();

            //не доходим создание документа до конца, перезагружаем страницу
            NavigatePage();
            //второй шаг
            //смотрим в списке если нет док с именем Old - то все ок

            Console.WriteLine("end change-test, check names");
            WriteStringIntoFile("end change-test, check names");
            //Assert.AreEqual(_documentName, _js.ExecuteScript("document.querySelector('#docs table tbody tr:nth-child(1) td:nth-child(2) a').text;"));
            //Assert.AreEqual(_documentName, _driver.FindElement(By.CssSelector("#docs table tbody tr:nth-child(1) td:nth-child(2) a")).GetAttribute("value"));

        }

        [Test]
        public void AddTMDublicateNameTest()
        {
            Console.WriteLine("Test - Add TM with dublicate name");
            WriteStringIntoFile("Test - Add TM with dublicate name");


            NavigatePage();
            WaitPageUpload();

            //New
            AddNewDocument();

            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Small);
            //показывает ли сообщение об ошибке - если нет, то все хорошо

            //add TM
            //choose TM step
            CheckTMUpload();
            Thread.Sleep(Constants.Medium);
            //find <Add TM> button
            _driver.FindElement(By.Id("tms-add")).Click();

            //check if Add TM window is opened
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-tm")).Displayed);
            Console.WriteLine("The add-tm window is open");
            WriteStringIntoFile("The add-tm window is open");
            //File.WriteAllText(Path, "The add-tm window is open", Encoding.Default);
            //input TM name, tmx file
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).SendKeys(_tmNameEqual);

            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"fileConent\"]")).SendKeys(_tmFile);
            _driver.FindElement(By.Id("tm-accept")).Click();

            Thread.Sleep(Constants.Medium);
            //сообщение об ошибке
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The FAILURE appear - it's OK");
            WriteStringIntoFile("The FAILURE appear - it's OK");

        }

        [Test]
        public void AutoCreateTMTest()
        {
            Console.WriteLine("Test  - AutoCreation TM");
            WriteStringIntoFile("Test - AutoCreation TM");
            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            _tmName = "TM_" + DateTime.UtcNow.Ticks.ToString();
            Console.WriteLine(_documentName);
            WriteStringIntoFile(_documentName);
            //first step
            NavigatePage();
            WaitPageUpload();

            //New
            AddNewDocument();

            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();

            //choose TM step
            CheckTMUpload();

            //автоматическое создание ТМ 
            Thread.Sleep(Constants.Medium);
            _driver.FindElement(By.Id("wizard-next")).Click();
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector("#msg")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Small);
            }

            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The Attention window appear - it's OK");
            WriteStringIntoFile("The Attention window appear - it's OK");
            //agree to create TM automatically
            _driver.FindElement(By.Id("msg-yes")).Click();

            //ждем, когда перейдем на след.шаг
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TryMedium)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector("#wizard-mts table tr")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }
            Thread.Sleep(Constants.Medium);

            //выбор МТ
            _driver.FindElement(By.CssSelector("#wizard-mts table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("wizard-next")).Click();
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("wizard-filemanager")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }

            Assert.IsTrue(_driver.FindElement(By.Id("wizard-filemanager")).Displayed);
            Console.WriteLine("It's almost end");
            WriteStringIntoFile("It's almost end");
            //File.WriteAllText(Path, "It's almost end", Encoding.Default);
            _driver.FindElement(By.Id("wizard-filemanager")).Click();
        }

        [Test]
        public void AddTMEmptyNameTest()
        {
            Console.WriteLine("Test - Add TM with Empty name");
            WriteStringIntoFile("Test - Add TM with Empty name");
            //File.WriteAllText(Path, "Test - Add TM with dublicate name", Encoding.Default);

            NavigatePage();
            WaitPageUpload();

            //New
            AddNewDocument();

            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Small);

            CheckTMUpload();
            Thread.Sleep(Constants.Big);
            //find <Add TM> button
            _driver.FindElement(By.Id("tms-add")).Click();

            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-tm")).Displayed);
            Console.WriteLine("The add-tm window is open");
            WriteStringIntoFile("The add-tm window is open");
            //File.WriteAllText(Path, "The add-tm window is open", Encoding.Default);
            //input TM name, tmx file
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).SendKeys("");

            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"fileConent\"]")).SendKeys(_tmFile);
            _driver.FindElement(By.Id("tm-accept")).Click();
            Thread.Sleep(Constants.Small);
            //сообщение об ошибке
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The FAILURE appear - it's OK");
            WriteStringIntoFile("The FAILURE appear - it's OK");
        }

        [Test]
        public void AddTMNameWithSpaceTest()
        {
            Console.WriteLine("Test - Add TM with Space name");
            WriteStringIntoFile("Test - Add TM with Space name");
            NavigatePage();
            WaitPageUpload();

            //New
            AddNewDocument();

            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Small);

            CheckTMUpload();
            Thread.Sleep(Constants.Big);
            //find <Add TM> button
            _driver.FindElement(By.Id("tms-add")).Click();

            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-tm")).Displayed);
            Console.WriteLine("The add-tm window is open");
            WriteStringIntoFile("The add-tm window is open");
            //File.WriteAllText(Path, "The add-tm window is open", Encoding.Default);
            //input TM name, tmx file
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).SendKeys(" ");

            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"fileConent\"]")).SendKeys(_tmFile);
            _driver.FindElement(By.Id("tm-accept")).Click();
            Thread.Sleep(Constants.Small);
            //сообщение об ошибке
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The FAILURE appear - it's OK");
            WriteStringIntoFile("The FAILURE appear - it's OK");
        }

        [Test]
        public void AddTMEmptypPathTest()
        {
            Console.WriteLine("Test - Add TM with Space name");
            WriteStringIntoFile("Test - Add TM with Space name");
            NavigatePage();
            WaitPageUpload();

            //New
            AddNewDocument();

            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            _tmName = "TM_" + DateTime.UtcNow.Ticks.ToString();
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Small);

            CheckTMUpload();
            Thread.Sleep(Constants.Big);
            //find <Add TM> button
            _driver.FindElement(By.Id("tms-add")).Click();

            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-tm")).Displayed);
            Console.WriteLine("The add-tm window is open");
            WriteStringIntoFile("The add-tm window is open");
            //File.WriteAllText(Path, "The add-tm window is open", Encoding.Default);
            //input TM name, tmx file
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).SendKeys(_tmName);

            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"fileConent\"]")).SendKeys("");
            _driver.FindElement(By.Id("tm-accept")).Click();
            Thread.Sleep(Constants.Small);

            //ждем, когда перейдем на след.шаг
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TryMedium)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector("#wizard-mts table tr")))
                    {
                        Console.WriteLine("It's OK");
                        WriteStringIntoFile("It's OK");
                        break;
                    }
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }
            Thread.Sleep(Constants.Medium);


        }

        [Test]
        public void AddTMNoTMXFileTest()
        {
            Console.WriteLine("Test - Add TM with Space name");
            WriteStringIntoFile("Test - Add TM with Space name");
            NavigatePage();
            WaitPageUpload();

            //New
            AddNewDocument();

            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            _tmName = "TM_" + DateTime.UtcNow.Ticks.ToString();

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Small);

            CheckTMUpload();
            Thread.Sleep(Constants.Big);
            //find <Add TM> button
            _driver.FindElement(By.Id("tms-add")).Click();

            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-tm")).Displayed);
            Console.WriteLine("The add-tm window is open");
            WriteStringIntoFile("The add-tm window is open");
            //File.WriteAllText(Path, "The add-tm window is open", Encoding.Default);
            //input TM name, tmx file
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).SendKeys(_tmName);

            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"fileConent\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("tm-accept")).Click();
            Thread.Sleep(Constants.Small);
            //сообщение об ошибке
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The FAILURE appear - it's OK");
            WriteStringIntoFile("The FAILURE appear - it's OK");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ChooseTMFromList()
        {
        }

#warning комментарии к тестам, как ты написала строчкой ниже принято писать через 3 ///. Прочитай на тему  xml comments
        // /создание файла с именем уже удаленного
        [Test]
        public void CreateDocumentWithDeletedDocNameTest()
        {
            Console.WriteLine("Test  - Create Document with Deleted File Name");
            WriteStringIntoFile("Test  - Create Document with Deleted File Name");
            //создать док
            CreateDocumentSetupAllTest();
            //удалить док
            DeleteDocumentTest();

            //создать другой док
            Console.WriteLine(_documentName);
            WriteStringIntoFile(_documentName);
            //File.WriteAllText(Path, DocumentName, Encoding.Default);
            //1 шаг
            NavigatePage();
            WaitPageUpload();

            //новый документ
            AddNewDocument();

            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();

            CheckTMUpload();
            Console.WriteLine("Test pass");
            WriteStringIntoFile("Test pass");
        }
        [Test]
        public void DeleteDocumentTest()
        {
            NavigatePage();

            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector("#docs table tbody tr")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }
            Thread.Sleep(Constants.Small);

            _driver.FindElement(By.CssSelector("#docs table tbody tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("docs-delete")).Click();
            Thread.Sleep(Constants.Small);
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("Attention window is open");
            WriteStringIntoFile("Attention window is open");
            _driver.FindElement(By.Id("msg-yes")).Click();
        }

        /// <summary>
        /// ТМ с именем как у глобальной - ABBYY Global TM
        /// </summary>
        [Test]
        public void AddMGlobalNameTest()
        {
            Console.WriteLine("Test - Add TM with Global TM name");
            WriteStringIntoFile("Test - Add TM with Global TM name");

            NavigatePage();
            WaitPageUpload();

            //New
            AddNewDocument();

            _documentName = "Doc_" + DateTime.UtcNow.Ticks.ToString();
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(_documentFile);
            _driver.FindElement(By.Id("wizard-next")).Click();
            Thread.Sleep(Constants.Small);
            //показывает ли сообщение об ошибке - если нет, то все хорошо

            //add TM
            //choose TM step
            CheckTMUpload();
            Thread.Sleep(Constants.Medium);
            //find <Add TM> button
            _driver.FindElement(By.Id("tms-add")).Click();

            //check if Add TM window is opened
            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard-tm")).Displayed);
            Console.WriteLine("The add-tm window is open");
            WriteStringIntoFile("The add-tm window is open");
            //File.WriteAllText(Path, "The add-tm window is open", Encoding.Default);
            //input TM name, tmx file
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"Name\"]")).SendKeys("ABBYY Global TM");

            _driver.FindElement(By.CssSelector("#wizard-tm input[name=\"fileConent\"]")).SendKeys(_tmFile);
            _driver.FindElement(By.Id("tm-accept")).Click();

            Thread.Sleep(Constants.Medium);

            for (int second = 0; ; second++)
            {
                if (second >= Constants.TryMedium)
                    Assert.Fail("timeout");

                if (IsElementPresent(By.Id("wizard-next")))
                {
                    Console.WriteLine("button is here");
                    WriteStringIntoFile("button is here");
                    break;
                }

                else
                {
                    Console.WriteLine("no button");
                    WriteStringIntoFile("no button");
                    break;
                }

            }
            Thread.Sleep(Constants.Big);

            _driver.FindElement(By.Id("wizard-next")).Click();

            Console.WriteLine("button next is pressed");
            WriteStringIntoFile("button next is pressed");
            Thread.Sleep(Constants.Big);
            //выбираем первую МТ из списка
            _driver.FindElement(By.CssSelector("#wizard-mts table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("wizard-next")).Click();
            //wait
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("wizard-filemanager")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }

            Assert.IsTrue(_driver.FindElement(By.Id("wizard-filemanager")).Displayed);
            Console.WriteLine("Window with <start translation> button open");
            WriteStringIntoFile("Window with <start translation> button open");
            //File.WriteAllText(Path, "Window with <start translation> button open", Encoding.Default);
            _driver.FindElement(By.Id("wizard-filemanager")).Click();


        }

        [Test]
        public void UploadFileForEditorTest(string Path, string SourceLang, string TargetLang)
        {
            Console.WriteLine("Upload files for Editor Test");
            _documentName = "EditorDoc_" + DateTime.UtcNow.Ticks.ToString();
            Console.WriteLine(_documentName);


            NavigatePage();

            WaitPageUpload();

            AddNewDocument();

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).Clear();
            Thread.Sleep(Constants.Small);
            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")).SendKeys(_documentName);

            _driver.FindElement(By.CssSelector("#wizard-upload input[name=\"File\"]")).SendKeys(Path);
            Thread.Sleep(Constants.Small);
            string Target1 = string.Format("document.querySelector('#wizard-upload input[name=\"TargetLanguage\"]').value=\"{0}\"; ", TargetLang);
            _js.ExecuteScript(Target1);
            string Source1 = string.Format("document.querySelector('#wizard-upload input[name=\"SourceLanguage\"]').value=\"{0}\"; ", SourceLang);
            _js.ExecuteScript(Source1);

            _driver.FindElement(By.Id("wizard-next")).Click();

            //добавляет ТМ - создаем автоматически + global tm

            CheckTMUpload();
            Thread.Sleep(Constants.Big);
            //выбор глобальной ТМ
            _driver.FindElement(By.CssSelector("#wizard-tms-body table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("wizard-next")).Click();

            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector("#msg")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Small);
            }

            Assert.IsTrue(_driver.FindElement(By.CssSelector("#msg")).Displayed);
            Console.WriteLine("The Attention window appear - it's OK");

            _driver.FindElement(By.Id("msg-yes")).Click();

            //ждем, когда перейдем на след.шаг
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TryMedium)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector("#wizard-mts table tr")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }
            Thread.Sleep(Constants.Medium);


            //подключаем все МТ
            _driver.FindElement(By.CssSelector("#wizard-mts table tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.CssSelector("#wizard-mts table tr:nth-child(2) td:nth-child(1)")).Click();
            _driver.FindElement(By.CssSelector("#wizard-mts table tr:nth-child(3) td:nth-child(1)")).Click();
            _driver.FindElement(By.CssSelector("#wizard-mts table tr:nth-child(4) td:nth-child(1)")).Click();

            _driver.FindElement(By.Id("wizard-next")).Click();

            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("wizard-filemanager")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }

            Assert.IsTrue(_driver.FindElement(By.Id("wizard-filemanager")).Displayed);
            Console.WriteLine("Window with <start translation> button open");
            _driver.FindElement(By.Id("wizard-translate")).Click();
        }





        [Test]
        public void SettingsTest()
        {
            NavigatePage();

            for (
                int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector("#docs table tbody tr")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }
            Thread.Sleep(Constants.Small);

            _driver.FindElement(By.CssSelector("#docs table tbody tr:nth-child(1) td:nth-child(1)")).Click();
            _driver.FindElement(By.Id("docs-settings")).Click();

            Thread.Sleep(Constants.Big);

            Assert.IsTrue(IsElementPresent(By.CssSelector("div.x-grid-checkheader.x-grid-checkheader-checked")));
            Console.WriteLine("TM is chosen as WRITE - OK");
            WriteStringIntoFile("TM is chosen as WRITE - OK");
            //File.WriteAllText(Path, "TM is chosen as WRITE - OK", Encoding.Default);

        }

        public void NavigatePage()
        {
            _driver.Navigate().GoToUrl(_baseUrl + "/workspace/?UserId=" + _userId);
            Assert.AreEqual(_baseUrl + "/workspace/?UserId=" + _userId, _driver.Url);
            Console.WriteLine("The page is correct: " + _driver.Url);
            WriteStringIntoFile("The page is correct: " + _driver.Url);
            //File.WriteAllText(Path, "The page is correct: " + driver.Url, Encoding.Default);
        }

        public void WaitPageUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
#warning для читабельности кода- то, что у тебя идет за if или за for, переноси на следующую строчку.
                    if (IsElementPresent(By.CssSelector("#docs-add:not(.x-disabled)")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }
        }

        public void CheckTMUpload()
        {
            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall)
                    Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("tms-add")))
                    {
                        Console.WriteLine("TM add button appear");
                        WriteStringIntoFile("TM add button appear");
                        break;
                    }
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Small);
            }
        }

        public void AddNewDocument()
        {
            _driver.FindElement(By.Id("docs-add")).Click();

            for (int second = 0; ; second++)
            {
                if (second >= Constants.TrySmall) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector("#wizard-upload input[name=\"DocumentName\"]")))
                        break;
                }
                catch (Exception)
                { }
                Thread.Sleep(Constants.Medium);
            }



            Assert.IsTrue(_driver.FindElement(By.CssSelector("#wizard")).Displayed);
            Console.WriteLine("Wizard is open");
            WriteStringIntoFile("Wizard is open");

        }
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

        public void CloseTest()
        {
            _driver.Quit();
        }
    }
}
