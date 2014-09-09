using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    /// <summary>
    /// Группа тестов для проверки ТМ
    /// </summary>
	public class TMTest : BaseTest
    {
      	/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="url">Адрес</param>
		/// <param name="workspaceUrl">Адрес workspace</param>
		/// <param name="browserName">Название браузера</param>
        public TMTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {
            
        }



		/// <summary>
		/// Предварительная подготовка группы тестов
		/// </summary>
        [SetUp]
        public void Setup()
        {
			// Не закрывать браузер
			quitDriverAfterTest = false;

			// Переходим к странице воркспейса
			GoToTranslationMemories();
        }



		private static string[] importTMXFileList = Directory.GetFiles(Path.GetFullPath(@"..\..\..\TestingFiles\TMTestFiles"));

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
        [Test]
        public void CreateTMCheckProjectCreateTMListTest()
        {
            // Выбрать имя TM
            string TMName = "00000" + DateTime.Now.Ticks;
            // Создать ТМ
            CommonHelper.LANGUAGE srcLang = CommonHelper.LANGUAGE.English;
            CommonHelper.LANGUAGE trgLang = CommonHelper.LANGUAGE.Russian;
            CreateTMByNameAndSave(TMName, srcLang, trgLang);

            // Перейти на вкладку Workspace и проверить, что TM есть в списке при создании проекта
            Assert.IsTrue(GetIsExistTMCreateProjectList(TMName), "Ошибка: ТМ нет в списке при создании проекта");
        }

        /// <summary>
        /// Метод тестирования создания ТМ с проверкой списка TM при создании проекта
        /// </summary>
        [Test]
        public void CreateTMAnotherLangCheckProjectCreateTMListTest()
        {
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();
            // Создать ТМ
            CommonHelper.LANGUAGE srcLang = CommonHelper.LANGUAGE.French;
            CommonHelper.LANGUAGE trgLang = CommonHelper.LANGUAGE.German;
            CreateTMByNameAndSave(uniqueTMName, srcLang, trgLang);

            // Перейти на вкладку Workspace и проверить, что TM есть в списке при создании проекта
            Assert.IsTrue(GetIsExistTMCreateProjectList(uniqueTMName, true, srcLang, trgLang), "Ошибка: ТМ нет в списке при создании проекта");
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
            TMPage.ClickSaveNewTM();

            // Проверить выделение ошибки в поле Название
            Assert.IsTrue(TMPage.GetIsCreateTMInputNameError(),
                "Ошибка: поле Название не выделено ошибкой");

            // Проверить появления сообщения об ошибке
            Assert.IsTrue(TMPage.GetIsExistCreateTMErrorNoName(),
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
            TMPage.InputNewTMName(uniqueTMName);
            // Нажать кнопку Сохранить
            TMPage.ClickSaveNewTM();

            // Проверить появления сообщения об ошибке
            Assert.IsTrue(TMPage.GetIsExistCreateTMErrorNoTarget(),
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
            Assert.IsTrue(TMPage.GetIsExistCreateTMErrorExistName(),
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
            Assert.IsTrue(TMPage.GetIsErrorMessageNotTMX(),
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
            CreateTMWithUploadTMX(uniqueTMName, EditorTMXFile);

            // Получить количество сегментов
            int segCountBefore = GetSegmentCount(uniqueTMName);

            // Загрузить TMX файл (обновить ТМ, заменяет старые ТМ на новые)
            UploadDocumentToTMbyButton(uniqueTMName, TMPageHelper.TM_BTN_TYPE.Update, SecondTmFile);
            // Получить количество сегментов
            int segCountAfter = GetSegmentCount(uniqueTMName);
            // Если количество не изменилось, возможно, просто не обновилась страница - принудительно обновить
            if (segCountAfter == segCountBefore)
            {
                ReopenTMInfo(uniqueTMName);
                segCountAfter = GetSegmentCount(uniqueTMName);
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
			// После теста с экспортом необходимо выйти из брайзера,
			// чтобы сбросить выбор в диалоге экспорта (сохранить или открыть файл)
			quitDriverAfterTest = true;

			string TMName = SelectUniqueTMName();
            // Создать ТМ
            CreateTMByNameAndSave(TMName);

            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, TMPageHelper.TM_BTN_TYPE.Export);
            // Экспортировать - Assert внутри
            ExportTM();
        }

        /// <summary>
        /// Метод тестирования кнопки Export в открывающейся информации о ТМ с загруженным TMX (по списку ТМХ файлов для загрузки)
        /// </summary>
		/// <param name="importTMXFile">путь в файлу, импортируемого в проект</param>
        [Test, TestCaseSource("importTMXFileList")]
        public void ExportTMXTest(string importTMXFile)
        {
			// После теста с экспортом необходимо выйти из брайзера,
			// чтобы сбросить выбор в диалоге экспорта (сохранить или открыть файл)
			quitDriverAfterTest = true;
			
			string TMName = SelectUniqueTMName();
            // Создать ТМ с загрузкой файла ТМХ
            CreateTMWithUploadTMX(TMName, importTMXFile);

            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, TMPageHelper.TM_BTN_TYPE.Export);
            // Экспортировать - Assert внутри
            ExportTM();
        }

        /// <summary>
        /// Метод тестирования Delete с проверкой списка TM
        /// </summary>
        [Test]
        public void DeleteTMCheckTMListTest()
        {
            string TMName = ConstTMName;
            CreateTMIfNotExist(TMName);
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, TMPageHelper.TM_BTN_TYPE.Delete);

            // Нажимаем Delete в открывшейся форме
            TMPage.ConfirmDelete();

            // Закрытие формы
            // TODO убрать sleep
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
            // Выбрать имя TM
            // TODO попробовать изменить!
            string TMName = "00000" + DateTime.Now.Ticks;
            CreateTMByNameAndSave(TMName);
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, TMPageHelper.TM_BTN_TYPE.Delete);

            // Нажимаем Delete в открывшейся форме
            TMPage.ConfirmDelete();
            // Закрытие формы
            // TODO убрать sleep
            Thread.Sleep(1000);

            // Перейти на вкладку Workspace и проверить, что TM нет в списке при создании проекта
            Assert.IsTrue(!GetIsExistTMCreateProjectList(TMName), "Ошибка: ТМ не удалилась из списка при создании проекта");
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
            UploadDocumentToTMbyButton(TMName, TMPageHelper.TM_BTN_TYPE.Add, SecondTmFile);
            // Получить количество сегментов
            int segmentCount = GetSegmentCount(TMName);
            // Если количество сегментов = 0, возможно, не обновилась страница - принудительно обновить
            if (segmentCount == 0)
            {
                ReopenTMInfo(TMName);
                segmentCount = GetSegmentCount(TMName);
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
            CreateTMWithUploadTMX(uniqueTMName, EditorTMXFile);

            // Получить количество сегментов
            int segCountBefore = GetSegmentCount(uniqueTMName);

            // Загрузить TMX файл
            UploadDocumentToTMbyButton(uniqueTMName, TMPageHelper.TM_BTN_TYPE.Add, SecondTmFile);
            // Получить количество сегментов после загрузки TMX
            int segCountAfter = GetSegmentCount(uniqueTMName);

            // Если количество сегментов не изменилось, возможно, страница не обновилась - принудительно обновить
            if (segCountAfter <= segCountBefore)
            {
                ReopenTMInfo(uniqueTMName);
                segCountAfter = GetSegmentCount(uniqueTMName);
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
            CreateTMIfNotExist(TMName);
            // Изменить имя на пустое и сохранить
            EditTMFillName(TMName, "");

            // Проверить, что поле Имя выделено ошибкой
            Assert.IsTrue(TMPage.GetIsEditTMNameWithError(),
                "Ошибка: поле Имя не отмечено ошибкой");

            // Проверить, что появилось сообщение об ошибке в имени
            Assert.IsTrue(TMPage.GetIsExistEditErrorNoName(),
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
            Assert.IsTrue(TMPage.GetIsExistEditErrorExistName(),
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
		/// <param name="TMXFileImport">путь в файлу, импортируемого в проект</param>
        [Test, TestCaseSource("importTMXFileList")]
        public void ImportTMXTest(string TMXFileImport)
        {
            // Выбрать уникальное имя TM
            string uniqueTMName = SelectUniqueTMName();
            // Создать ТМ с загрузкой ТМХ
            CreateTMWithUploadTMX(uniqueTMName, TMXFileImport);

            // Проверить, сохранился ли ТМ
            Assert.IsTrue(GetIsExistTM(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");

            int segmentCount = GetSegmentCount(uniqueTMName);
            // Если количество сегментов = 0, возможно, не обновилась страница - принудительно обновить
            if (segmentCount == 0)
            {
                ReopenTMInfo(uniqueTMName);
                segmentCount = GetSegmentCount(uniqueTMName);
            }

            // Проверить, что количество сегментов больше 0
            Assert.IsTrue(segmentCount > 0, "Ошибка: количество сегментов должно быть больше 0");
        }
		


        /// <summary>
		/// Вернуть, есть ли ТМ в списке при создании проекта
        /// </summary>
        /// <param name="TMName">Название ТМ</param>
        /// <param name="isNeedChangeLanguages">Необходимость в смене языка</param>
        /// <param name="srcLang">Язык источника</param>
        /// <param name="trgLang">Язык перевода</param>
        /// <returns></returns>
        private bool GetIsExistTMCreateProjectList(string TMName, bool isNeedChangeLanguages = false,
            CommonHelper.LANGUAGE srcLang = CommonHelper.LANGUAGE.English, CommonHelper.LANGUAGE trgLang = CommonHelper.LANGUAGE.Russian)
        {
            // Перейти на страницу со списком проектов
            SwitchWorkspaceTab();
            // Заполнить создание проекта на первом шаге
            if (isNeedChangeLanguages)
            {
                FirstStepProjectWizard(ProjectName, false, srcLang, trgLang);
            }
            else
            {
                FirstStepProjectWizard(ProjectName);
            }
            
            // Next
            WorkspaceCreateProjectDialog.ClickNextStep();

            // Проверить, что есть ТМ
            return WorkspaceCreateProjectDialog.GetIsExistTM(TMName);
        }
        
        /// <summary>
        /// Изменить название ТМ
        /// </summary>
        /// <param name="TMNameToEdit">Тм для изменения</param>
        /// <param name="newTMName">новое имя</param>
        private void EditTMFillName(string TMNameToEdit, string newTMName)
        {
            // Отрыть информацию о ТМ и нажать Edit
            ClickButtonTMInfo(TMNameToEdit, TMPageHelper.TM_BTN_TYPE.Edit);

            // Ждем открытия формы редактирования
            TMPage.WaitUntilEditTMOpen();

            // Очистить поле Имя
            TMPage.EditTMClearName();

            // Если новое имя не пустое, то заполнить им поле Имя
            if (newTMName.Length > 0)
            {
                TMPage.InputEditTMName(newTMName);
            }

            // Сохранить изменение
            TMPage.ClickEditSaveBtn();

            // Ответ формы
            // TODO убрать sleep
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Открыть форму создания ТМ
        /// </summary>
        private void OpenCreateTMForm()
        {
            Assert.IsTrue(TMPage.OpenCreateTMDialog(), "Ошибка: не открылась форма создания ТМ");
        }

        /// <summary>
        /// Выбрать уникальное имя
        /// </summary>
        /// <returns>имя</returns>
        private string SelectUniqueTMName()
        {
            // Создать уникальное имя для ТМ без проверки существова
            return ConstTMName + DateTime.Now.ToString();
        }

        /// <summary>
        /// Вернуть: есть ли ТМ в списке
        /// </summary>
        /// <param name="TMName">название ТМ</param>
        /// <returns>есть</returns>
        private bool GetIsExistTM(string TMName)
        {
            // Есть ли ТМ с таким именем в списке на странице Translation Memory Bases
            return TMPage.GetIsExistTM(TMName);
        }

        /// <summary>
		/// Создать ТМ и сохранить
        /// </summary>
        /// <param name="TMName">Название ТМ</param>
        /// <param name="sourceLang">Язык источника</param>
        /// <param name="targetLang">Язык перевода</param>
        private void CreateTMByNameAndSave(string TMName,
            CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English,
            CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian)
        {
            // Создать ТМ без сохранения формы
            CreateTMByName(TMName, sourceLang, targetLang);

            // Нажать кнопку Сохранить
            TMPage.ClickSaveNewTM();
            // Закрытие формы

			// Дождаться окончания загрузки
			Assert.IsTrue(TMPage.WaitDocumentDownloadFinish(),
				"Ошибка: документ загружается слишком долго");
        }

        /// <summary>
		/// Создать ТМ (без сохранения)
        /// </summary>
        /// <param name="TMName">Название ТМ</param>
        /// <param name="sourceLang">Язык источника</param>
        /// <param name="targetLang">Язык перевода</param>
        private void CreateTMByName(string TMName,
            CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English,
            CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian)
        {
            // Открыть форму создания ТМ
            OpenCreateTMForm();

            // Ввести имя
            TMPage.InputNewTMName(TMName);

            // Выбрать языки (source и target), чтобы сохранить ТМ
            SelectSourceAndTargetLang(sourceLang, targetLang);
        }

        /// <summary>
        /// Выбрать языки Source и Target
        /// </summary>
        private void SelectSourceAndTargetLang(CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English, CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian)
        {
            // Нажать на Source Language для выпадения списка языков
            TMPage.ClickOpenSourceLangList();
           // Выбираем Английский
            TMPage.SelectSourceLanguage(sourceLang);

            // Нажать на Target Language для выпадения списка языков
            TMPage.ClickTargetLangList();
            // ждем выпадения списка
            // TODO проверить без ожидания
            //Wait.Until((d) => d.FindElement(By.XPath(
                //".//div[contains(@class,'ui-multiselect-menu')][contains(@class,'js-languages-multiselect')]")));
            // Выбираем Русский по value
            TMPage.SelectTargetLanguage(targetLang);
            // Нажать на Target Language для закрытия списка
            TMPage.ClickTargetLangList();
        }

        /// <summary>
        /// Загрузить документ в ТМ
        /// </summary>
        /// <param name="documentName">название документа</param>
        private void UploadDocumentTM(string documentName)
        {
            TMPage.WaitUntilUploadDialog();
            // Нажать на Add для появления диалога загрузки документа
            TMPage.ClickAddUploadBtn();

            // Заполнить диалог загрузки документа
            FillAddDocumentForm(documentName);

            Console.WriteLine(documentName);
            // Нажать на Импорт
            TMPage.ClickImportBtn();
            Console.WriteLine("кликнули импорт");
            if (TMPage.GetIsErrorMessageNotTMX())
            {                
                SendKeys.SendWait(@"{Enter}");
                Thread.Sleep(1000);
                // Нажать на Импорт
                TMPage.ClickImportBtn();
            }

            // Дождаться окончания загрузки
            Assert.IsTrue(TMPage.WaitDocumentDownloadFinish(),
                "Ошибка: документ загружается слишком долго");
        }

        /// <summary>
        /// Загрузить документ в ТМ
        /// </summary>
        /// <param name="TMName">название ТМ</param>
        /// <param name="btnType">по какой кнопке (Update, Add)</param>
        /// <param name="uploadFile">файл для загрузки</param>
        private void UploadDocumentToTMbyButton(string TMName, TMPageHelper.TM_BTN_TYPE btnType, string uploadFile)
        {
            // Отрыть информацию о ТМ и нажать кнопку
            ClickButtonTMInfo(TMName, btnType);
            // Загрузить документ
            UploadDocumentTM(uploadFile);
        }

        /// <summary>
        /// Получить количество сегментов ТМ
        /// </summary>
        /// <param name="TMName">название ТМ</param>
        /// <returns>количество сегментов</returns>
        private int GetSegmentCount(string TMName)
        {
            // Открыть информацию о ТМ
            OpenTMInfo(TMName);

            // Получить количество сегментов
            return TMPage.GetSegmentCount();
        }

        /// <summary>
        /// Кликнуть кнопку в информации о ТМ
        /// </summary>
        /// <param name="TMName"></param>
        /// <param name="btnType"></param>
        private void ClickButtonTMInfo(string TMName, TMPageHelper.TM_BTN_TYPE btnType)
        {
            // Открыть информацию о ТМ
            OpenTMInfo(TMName);
            // Кликнуть кнопку
            TMPage.ClickTMButton(btnType);
        }

        /// <summary>
        /// Переоткрыть информацию о ТМ
        /// </summary>
        /// <param name="TMName">название ТМ</param>
        private void ReopenTMInfo(string TMName)
        {
            if (TMPage.GetIsTMOpened(TMName))
            {
                // Если открыта - закрыть
                TMPage.ClickTMRow(TMName);
            }
            OpenTMInfo(TMName);
        }

        /// <summary>
        /// Открыть информацию о ТМ
        /// </summary>
        /// <param name="TMName">название ТМ</param>
        protected void OpenTMInfo(string TMName)
        {
            if (!TMPage.GetIsTMOpened(TMName))
            {
                TMPage.ClickTMRow(TMName);
                // TODO проверить sleep
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Создать ТМ, если его нет
        /// </summary>
        /// <param name="TMName">название ТМ</param>
        private void CreateTMIfNotExist(string TMName)
        {
            if (!GetIsExistTM(TMName))
            {
                // Если нет такого ТМ, создать  его
                CreateTMByNameAndSave(TMName);
            }
        }

        /// <summary>
        /// Создать ТМ с загрузкой TMX
        /// </summary>
        /// <param name="uniqueTMName">название ТМ</param>
        /// <param name="fileName">файл для загрузки</param>
        private void CreateTMWithUploadTMX(string uniqueTMName, string fileName)
        {
            // Создать ТМ
            CreateTMByName(uniqueTMName);

            // Нажать на Сохранить и Импортировать TMX файл
            TMPage.ClickSaveAndImportCreateTM();

            // Загрузить TMX файл
            UploadDocumentTM(fileName);
        }

        /// <summary>
        /// Экспортировать (работа с внешним диалогом)
        /// </summary>
        protected void ExportTM()
        {
            //ExternalDialogSaveDocument("TMExportTest", true, TMName, false, ".tmx");

            string resultPath = Path.Combine(PathTestResults, "TMExportTest");
            Directory.CreateDirectory(resultPath);

            string newFileName = DateTime.Now.Ticks.ToString();
            resultPath = Path.Combine(resultPath, newFileName + ".tmx");

            ExternalDialogSelectSaveDocument(resultPath);
            Assert.IsTrue(File.Exists(resultPath), "Ошибка: файл не экспортировался\n" + resultPath);
        }
    }
}
