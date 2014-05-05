using NUnit.Framework;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class EditorButtonsTest : BaseTest
    {
        public EditorButtonsTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
            // 1. Авторизация
            Authorization();

            string currentDocument = DocumentFile;
            // При проверке Confirm не работает наш обычный файл, приходится загружать другой
            if (TestContext.CurrentContext.Test.Name.Contains("Confirm"))
            {
                currentDocument = DocumentFileToConfirm;
            }

            // 2. Создание проекта с 1 документов внутри
            CreateProject(ProjectName, false, "");
            //открытие настроек проекта
            ImportDocumentProjectSettings(currentDocument, ProjectName);
            //CreateProject(ProjectName, true, DocumentFile, TmFile);
            OpenMainWorkspacePage();
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
            CancelHotkey();
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
}
