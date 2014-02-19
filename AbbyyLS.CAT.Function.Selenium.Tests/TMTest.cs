using NUnit.Framework;

namespace AbbyyLs.CAT.Projects.Selenium.Tests
{
    public class TMTest : BaseTest
    {
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
            CreateNewTM();
        }

        /// <summary>
        /// Метод тестирования создания ТМ с проверкой списка TM при создании проекта
        /// </summary>
        [Test]
        public void CreateTMCheckProjectCreateTMListTest()
        {
            CreateTMCheckProjectCreateTMList();
        }

        /// <summary>
        /// Метод тестирования создания ТМ без имени
        /// </summary>
        [Test]
        public void CreateTMWithoutNameTest()
        {
            CreateTMWithoutName();
        }

        /// <summary>
        /// Метод тестирования создания ТМ с существующим именем
        /// </summary>
        [Test]
        public void CreateTMWithExistingNameTest()
        {
            CreateTMWithExistingName();
        }

        /// <summary>
        /// Метод тестирования создания ТМ c загрузкой TMX файла
        /// </summary>
        [Test]
        public void CreateTMWithTMXTest()
        {
            CreateTMWithTMX();
        }

        /// <summary>
        /// Метод тестирования создания ТМ с загрузкой НЕ(!) TMX файла
        /// </summary>
        [Test]
        public void CreateTMWithNotTMXTest()
        {
            CreateTMWithNotTMX();
        }

        /// <summary>
        /// Метод тестирования кнопки Update TM в открывающейся информации о ТМ
        /// </summary>
        [Test]
        public void UpdateTMButtonTest()
        {
            UpdateTMButton();
        }

        /// <summary>
        /// Метод тестирования кнопки Export в открывающейся информации о ТМ
        /// </summary>
        [Test]
        public void ExportTMButtonTest()
        {
            ExportTMButton();
        }

        /// <summary>
        /// Метод тестирования Delete с проверкой списка TM
        /// </summary>
        [Test]
        public void DeleteTMCheckTMListTest()
        {
            DeleteTMCheckTMList();
        }

        /// <summary>
        /// Метод тестирования Delete с проверкой списка TM при создании проекта
        /// </summary>
        [Test]
        public void DeleteTMCheckProjectCreateTMListTest()
        {
            DeleteTMCheckProjectCreateTMList();
        }

        /// <summary>
        /// Метод тестирования кнопки Add TMX для пустого ТМ
        /// </summary>
        [Test]
        public void AddTMXOnClearTMButtonTest()
        {
            AddTMXOnClearTMButton();
        }

        /// <summary>
        /// Метод тестирования кнопки Add TMX для ТМ с ТМХ
        /// </summary>
        [Test]
        public void AddTMXExistingTMXButtonTest()
        {
            AddTMXButton();
        }

        /// <summary>
        /// Тестирование редактирования ТМ: изменение имени на пустое
        /// </summary>
        [Test]
        public void EditTMSaveWithoutNameTest()
        {
            EditTMSaveWithoutName();
        }

        /// <summary>
        /// Тестирование редактирования ТМ: изменение имени на существующее
        /// </summary>
        [Test]
        public void EditTMSaveExistingNameTest()
        {
            EditTMSaveExistingName();
        }

        /// <summary>
        /// Тестирование редактирования ТМ: изменение имени на новое
        /// </summary>
        [Test]
        public void EditTMSaveUniqueNameTest()
        {
            EditTMSaveUniqueName();
        }
    }
}
