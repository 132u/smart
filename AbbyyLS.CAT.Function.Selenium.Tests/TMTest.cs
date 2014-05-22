using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class TMTest : BaseTest
    {
        private static string[] importTMXFileList = Directory.GetFiles(Path.GetFullPath(@"..\..\..\TestingFiles\TMTestFiles"));

        public TMTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {
            
        }
       

        [SetUp]
        public void Setup()
        {
            // Авторизация
            Authorization();

            // Перейти на вкладку Базы Translation memory
            SwitchTMTab();
        }

        /// <summary>
        /// Метод тестирования создания ТМ (без TMX)
        /// </summary>
        [Test]
        public void CreateNewTMTest()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();
            // Создать ТМ
            CreateTMByNameAndSave(uniqueTMName);

            // Проверить, сохранился ли ТМ
            Assert.IsTrue(GetIsExistTM(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");

            // Проверить, что количество сегментов равно 0
            Assert.IsTrue(GetSegmentCount(uniqueTMName) == 0, "Ошибка: количество сегментов должно быть равно 0");
        }

        /// <summary>
        /// Метод тестирования создания ТМ с проверкой списка TM при создании проекта
        /// </summary>
        //[Test] // - В списке ТМ при создании проекта Selenium не видит весь список (не может прокрутить список вниз), поэтому не видит ТМ
        public void CreateTMCheckProjectCreateTMListTest()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();
            // Создать ТМ
            CreateTMByNameAndSave(uniqueTMName);

            // Перейти на вкладку SmartCAT и проверить, что TM есть в списке при создании проекта
            Assert.IsTrue(GetIsExistTMCreateProject(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");
        }

        /// <summary>
        /// Метод тестирования создания ТМ без имени
        /// </summary>
        [Test]
        public void CreateTMWithoutNameTest()
        {
            // Открыть форму создания ТМ
            OpenCreateTMForm();
            // Нажать кнопку Сохранить
            WaitAndClickElement(".//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'g-btn__text')]");

            // Проверить выделение ошибки в поле Название
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@class,'error')]")).Displayed,
                "Ошибка: поле Название не выделено ошибкой");

            // Проверить появления сообщения об ошибке
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'l-createtm__error')]")).Displayed,
                "Ошибка: не появилось сообщение об ошибке");
        }

        /// <summary>
        /// Метод тестирования создания ТМ без указания языков
        /// </summary>
        [Test]
        public void CreateTMWithoutLanguageTest()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();

            // Открыть форму создания ТМ
            OpenCreateTMForm();

            // Ввести имя
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@class,'l-createtm__nmtext')]")).
                SendKeys(uniqueTMName);

            // Нажать кнопку Сохранить
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'g-btn__text')]")).Click();

            // Проверить появления сообщения об ошибке
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'l-createtm__error')]//p[contains(@class,'js-error-targetLanguage-required')]")).Displayed,
                "Ошибка: не появилось сообщение об ошибке");
        }

        /// <summary>
        /// Метод тестирования создания ТМ с существующим именем
        /// </summary>
        [Test]
        public void CreateTMWithExistingNameTest()
        {
            string TMName = ConstTMName;
            // Создать ТМ
            CreateTMIfNotExist(TMName);
            // Создать ТМ с тем же (уже существующим) именем
            CreateTMByNameAndSave(TMName);

            // Проверить появление ошибки
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'js-dynamic-errors')]//p[contains(@data-key,'name')]")).Displayed,
                "Ошибка: не появилась ошибка создания ТМ с существующим именем");
        }

        /// <summary>
        /// Метод тестирования создания ТМ с загрузкой НЕ(!) TMX файла
        /// </summary>
        [Test]
        public void CreateTMWithNotTMXTest()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();
            // Создать ТМ с загрузкой НЕ(!) TMX файла
            CreateTMWithUploadTMX(uniqueTMName, DocumentFile);

            // Проверить, что появилось сообщение о неверном расширении файла
            Assert.IsTrue(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-import')][2]//div[contains(@class,'g-popupbox__error')]//p[contains(@class,'js-error-invalid-file-extension')]")).Displayed,
                "Ошибка: не появилось сообщение о неверном расширении файла");
        }

        /// <summary>
        /// Метод тестирования кнопки Update TM в открывающейся информации о ТМ
        /// </summary>
        [Test]
        public void UpdateTMButtonTest()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();
            // Создать ТМ и загрузить TMX файл
            CreateTMWithUploadTMX(uniqueTMName, TmFile);

            // Получить количество сегментов
            int segCountBefore = GetSegmentCount(uniqueTMName, false);

            // Загрузить TMX файл (обновить ТМ, заменяет старые ТМ на новые)
            UploadDocumentToTMbyButton(uniqueTMName, "js-upload-btn", SecondTmFile, false);
            // Получить количество сегментов
            int segCountAfter = GetSegmentCount(uniqueTMName, false);
            // Если количество не изменилось, возможно, просто не обновилась страница - принудительно обновить
            if (segCountAfter == segCountBefore)
            {
                ReopenTMInfo(uniqueTMName);
                segCountAfter = GetSegmentCount(uniqueTMName, false);
            }
            // Проверить, что количество сегментов изменилось
            Assert.IsTrue(segCountBefore != segCountAfter, "Ошибка: количество сегментов должно измениться");
        }

        /// <summary>
        /// Метод тестирования кнопки Export в открывающейся информации о пустой ТМ
        /// </summary>
        [Test]
        public void ExportClearTMButtonTest()
        {
            string TMName = SelectUniqueTMName();
            // Создать ТМ
            CreateTMByNameAndSave(TMName);

            // Создать уникальное название для экспортируемого файла
            string uniqueExportName = ConstTMName + DateTime.UtcNow.Ticks.ToString();
            string resultPath = System.IO.Path.Combine(PathTestResults, "TMExportTest");
            System.IO.Directory.CreateDirectory(resultPath);
            uniqueExportName = System.IO.Path.Combine(resultPath, uniqueExportName);

            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, "js-export-btn");
            // Сохранить файл
            ExternalDialogSelectSaveDocument(uniqueExportName);

            // Проверить, экспортировался ли файл
            Assert.IsTrue(System.IO.File.Exists(uniqueExportName + ".tmx"), "Ошибка: файл не экспортировался");
        }

        /// <summary>
        /// Метод тестирования кнопки Export в открывающейся информации о ТМ с загруженным TMX (по списку ТМХ файлов для загрузки)
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        [Test, TestCaseSource("importTMXFileList")]
        public void ExportTMXTest(string importTMXFile)
        {
            string TMName = SelectUniqueTMName();
            // Создать ТМ с загрузкой файла ТМХ
            CreateTMWithUploadTMX(TMName, importTMXFile);

            // Создать уникальное название для экспортируемого файла
            string uniqueExportName = ConstTMName + DateTime.UtcNow.Ticks.ToString();
            string resultPath = System.IO.Path.Combine(PathTestResults, "TMExportTest");
            System.IO.Directory.CreateDirectory(resultPath);
            uniqueExportName = System.IO.Path.Combine(resultPath, uniqueExportName);

            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, "js-export-btn", false);
            // Сохранить файл
            ExternalDialogSelectSaveDocument(uniqueExportName);

            // Проверить, экспортировался ли файл
            Assert.IsTrue(System.IO.File.Exists(uniqueExportName + ".tmx"), "Ошибка: файл не экспортировался");
        }

        /// <summary>
        /// Метод тестирования Delete с проверкой списка TM
        /// </summary>
        [Test]
        public void DeleteTMCheckTMListTest()
        {
            string TMName = ConstTMName;
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, "js-delete-btn");

            // Нажимаем Delete в открывшейся форме
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-confirm')]//input[contains(@type,'submit')]"))).Click();

            // Закрытие формы
            Thread.Sleep(1000);

            // Проверить, что ТМ удалилась из списка
            Assert.IsTrue(!GetIsExistTM(TMName), "Ошибка: ТМ не удалилась из списка");
        }

        /// <summary>
        /// Метод тестирования Delete с проверкой списка TM при создании проекта
        /// </summary>
        [Test]
        public void DeleteTMCheckProjectCreateTMListTest()
        {
            string TMName = ConstTMName;
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, "js-delete-btn");

            // Нажимаем Delete в открывшейся форме
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-confirm')]//input[contains(@type,'submit')]"))).Click();
            // Закрытие формы
            Thread.Sleep(1000);

            // Перейти на вкладку SmartCAT и проверить, что TM нет в списке при создании проекта
            Assert.IsTrue(!GetIsExistTMCreateProject(TMName));
        }

        /// <summary>
        /// Метод тестирования кнопки Add TMX для пустого ТМ
        /// </summary>
        [Test]
        public void AddTMXOnClearTMButtonTest()
        {
            // Создать новый ТМ
            string TMName = SelectUniqueTMName();
            CreateTMByNameAndSave(TMName);
            // Загрузить ТМХ по кнопке в информации о ТМ
            UploadDocumentToTMbyButton(TMName, "js-add-tmx-btn", TmFile);
            // Получить количество сегментов
            int segmentCount = GetSegmentCount(TMName, false);
            // Если количество сегментов = 0, возможно, не обновилась страница - принудительно обновить
            if (segmentCount == 0)
            {
                ReopenTMInfo(TMName);
                segmentCount = GetSegmentCount(TMName, false);
            }

            // Проверить, что количество сегментов больше нуля (ТМХ загрузился)
            Assert.IsTrue(segmentCount > 0, "Ошибка: количество сегментов должно быть больше нуля");
        }

        /// <summary>
        /// Метод тестирования кнопки Add TMX для ТМ с ТМХ
        /// </summary>
        [Test]
        public void AddTMXExistingTMButtonTest()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();
            // Создать ТМ и загрузить ТМХ файл
            CreateTMWithUploadTMX(uniqueTMName, TmFile);

            // Получить количество сегментов
            int segCountBefore = GetSegmentCount(uniqueTMName, false);

            // Загрузить TMX файл
            UploadDocumentToTMbyButton(uniqueTMName, "js-add-tmx-btn", SecondTmFile, false);
            // Получить количество сегментов после загрузки TMX
            int segCountAfter = GetSegmentCount(uniqueTMName, false);

            // Если количество сегментов не изменилось, возможно, страница не обновилась - принудительно обновить
            if (segCountAfter <= segCountBefore)
            {
                ReopenTMInfo(uniqueTMName);
                segCountAfter = GetSegmentCount(uniqueTMName, false);
            }

            // Проверить, что количество сегментов увеличилось (при AddTMX количество сегментов должно суммироваться)
            Assert.IsTrue(segCountAfter > segCountBefore, "Ошибка: количество сегментов должно увеличиться");
        }

        /// <summary>
        /// Тестирование редактирования ТМ: изменение имени на пустое
        /// </summary>
        [Test]
        public void EditTMSaveWithoutNameTest()
        {
            string TMName = ConstTMName;
            // Изменить имя на пустое и сохранить
            EditTMFillName(TMName, "");

            // Получить xPath формы редактирования ТМ
            string xPath = CreateXPathTMRow(TMName);
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

        /// <summary>
        /// Тестирование редактирования ТМ: изменение имени на существующее
        /// </summary>
        [Test]
        public void EditTMSaveExistingNameTest()
        {
            string TMName = ConstTMName;
            // Создать ТМ с таким именем, если его еще нет
            CreateTMIfNotExist(TMName);

            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();
            // Создать ТМ
            CreateTMByNameAndSave(uniqueTMName);

            // Изменить имя на существующее и сохранить
            EditTMFillName(uniqueTMName, TMName);

            // Проверить, что появилось сообщение об ошибке в имени
            string xPath = CreateXPathTMRow(uniqueTMName);
            xPath += "/../../following-sibling::tr[contains(@class, 'js-editing')]";
            string errorInfoPath = xPath +
                "//div[contains(@class,'js-dynamic-errors')]//p[contains(@data-key,'name')]";
            Assert.IsTrue(Driver.FindElement(By.XPath(errorInfoPath)).Displayed,
                "Ошибка: не появилось сообщение об ошибке в имени");
        }

        /// <summary>
        /// Тестирование редактирования ТМ: изменение имени на новое
        /// </summary>
        [Test]
        public void EditTMSaveUniqueNameTest()
        {
            string TMName = ConstTMName;
            // Создать ТМ с таким именем, если его еще нет
            CreateTMIfNotExist(TMName);
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();

            // Изменить имя на уникальное и сохранить
            EditTMFillName(TMName, uniqueTMName);

            // Проверить, что ТМ со старым именем удалился, а с новым именем есть
            Assert.IsTrue(!GetIsExistTM(TMName), "Ошибка: не удалилось старое имя");
            Assert.IsTrue(GetIsExistTM(uniqueTMName), "Ошибка: нет ТМ с новым именем");
        }

        /// <summary>
        /// Создание ТМ с загрузкой ТМХ (по списку ТМХ файлов), проверка, что ТМХ загрузился
        /// </summary>
        /// <param name="filePath">путь в файлу, импортируемого в проект</param>
        [Test, TestCaseSource("importTMXFileList")]
        public void ImportTMXTest(string TMXFileImport)
        {
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();
            // Создать ТМ с загрузкой ТМХ
            CreateTMWithUploadTMX(uniqueTMName, TMXFileImport);

            // Проверить, сохранился ли ТМ
            Assert.IsTrue(GetIsExistTM(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");

            int segmentCount = GetSegmentCount(uniqueTMName, false);
            // Если количество сегментов = 0, возможно, не обновилась страница - принудительно обновить
            if (segmentCount == 0)
            {
                ReopenTMInfo(uniqueTMName);
                segmentCount = GetSegmentCount(uniqueTMName, false);
            }

            // Проверить, что количество сегментов больше 0
            Assert.IsTrue(segmentCount > 0, "Ошибка: количество сегментов должно быть больше 0");
        }
        
        private void EditTMFillName(string TMNameToEdit, string newTMName)
        {
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMNameToEdit, "js-edit-btn");

            string xPath = CreateXPathTMRow(TMNameToEdit);
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

        private string SelectUniqueTMName()
        {
            // Создать уникальное имя для ТМ без проверки существова
            return ConstTMName + DateTime.Now.ToString();
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
            WaitProjectPageOpen();

            // Начать создание проекта
            FirstStepProjectWizard(ProjectName);
            // Перейти на шаг выбора ТМ
            AddDocument(false, "");

            // Есть ли ТМ с таким именем в списке при создании проекта
            return GetIsExistTMInCurrentList(TMName, GetCreateProjectTMListXPath());
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
            Thread.Sleep(1000);
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
                ".//span[contains(@class,'js-dropdown__list')]")).Displayed);
            // Выбираем Английский
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__item')][@data-id='9']")).Click();

            // Нажать на Target Language для выпадения списка языков
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'js-languages-multiselect')]")).Click();
            // ждем выпадения списка
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//div[contains(@class,'ui-multiselect-menu')][contains(@class,'js-languages-multiselect')]")));
            // Выбираем Русский по value
            Driver.FindElement(By.XPath(
                ".//li/label/span/input[@value='25']")).Click();
            // Нажать на Target Language для закрытия списка
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'js-languages-multiselect')]")).Click();
        }

        private void UploadDocumentTM(string documentName)
        {
            // Нажать на Add для появления диалога загрузки документа
            WaitAndClickElement(".//a[contains(@class,'js-upload-btn')]");

            // Заполнить диалог загрузки документа
            FillAddDocumentForm(documentName);
            // Нажать на Импорт
            WaitAndClickElement(".//div[contains(@class,'js-popup-import')][2]//span[contains(@class,'g-btn__data')]");

            // Закрытие формы
            Thread.Sleep(5000);
        }

        private void UploadDocumentToTMbyButton(string TMName, string btnName, string uploadFile, bool isNeedOpenInfo = true)
        {
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, btnName, isNeedOpenInfo);
            // Загрузить документ
            UploadDocumentTM(uploadFile);
        }

        private int GetSegmentCount(string TMName, bool isNeedToShowInfo = true)
        {
            if (isNeedToShowInfo)
            {
                // Открыть информацию о ТМ
                ClickTMToShowInfo(TMName);
            }

            string xPath = CreateXPathTMRow(TMName);
            xPath += "/../../following-sibling::tr//table//tr/td[2]/div[4]";
            string segmentsCount = Wait.Until((d) => d.FindElement(By.XPath(xPath))).Text;
            // Нужно получить число сегментов из строки "Segments count: N", разделитель - ":"
            int splitIndex = segmentsCount.IndexOf(":");
            // Отступаем двоеточие и пробел
            splitIndex += 2;
            if (segmentsCount.Length > splitIndex)
            {
                segmentsCount = segmentsCount.Substring(splitIndex);
            }
            // Получить число сегментов из строки
            return int.Parse(segmentsCount.Trim());
        }

        private void ClickButtonTMInfo(string TMName, string btnName, bool isNeedOpenInfo = true)
        {
            if (isNeedOpenInfo)
            {
                // Открыть информацию о ТМ
                ClickTMToShowInfo(TMName);
            }

            // Получить xPath строки нужного ТМ
            string xPath = CreateXPathTMRow(TMName);
            // Получить xPath нужной кнопки открытой информации нужного ТМ
            xPath += "/../../following-sibling::tr//span[contains(@class,'" + btnName + "')]//a";

            // Нажать на нужную кнопку
            WaitAndClickElement(xPath);
        }

        private void ClickTMToShowInfo(string TMName)
        {
            // Если такого TM нет - создать его
            CreateTMIfNotExist(TMName);

            // Получить xPath строки с этим ТМ
            string xPath = CreateXPathTMRow(TMName);

            // Открыть информацию о ТМ
            ClickRowTM(xPath);
        }

        private void ClickRowTM(string rowXPath)
        {
            // Открыть информацию о ТМ
            Driver.FindElement(By.XPath(rowXPath)).Click();
            // Подождать открытие информации
            Thread.Sleep(500);
        }

        private void ReopenTMInfo(string TMName)
        {
            // Получить xPath строки с этим ТМ
            string xPath = CreateXPathTMRow(TMName);
            // Закрыть информацию
            ClickRowTM(xPath);
            // Открыть информацию
            ClickRowTM(xPath);
        }

        private void CreateTMIfNotExist(string TMName)
        {
            if (!GetIsExistTM(TMName))
            {
                // Если нет такого ТМ, создать  его
                CreateTMByNameAndSave(TMName);
            }
        }

        private string CreateXPathTMRow(string TMName)
        {
            return ".//tr[contains(@class, 'js-tm-row')]/td/span[text()='" + TMName + "']";
        }

        private void CreateTMWithUploadTMX(string uniqueTMName, string FileName)
        {
            // Создать ТМ
            CreateTMByName(uniqueTMName);

            // Нажать на Сохранить и Импортировать TMX файл
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'js-save-and-import')]")).Click();
            // ждем появления окна
            Wait.Until((d) => d.FindElement(By.XPath(
                ".//a[contains(@class,'js-upload-btn')]")));
            // Загрузить TMX файл
            UploadDocumentTM(FileName);
        }
    }
}
