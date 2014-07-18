using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class Project_MainTest : NewProjectTest
    {
        public Project_MainTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// создание проекта без файла
        /// </summary>
        [Test]
        public void CreateProjectNoFile()
        {
            // Создать проект
            CreateProject(ProjectName);
            //проверить что проект появился с списке проектов
            CheckProjectInList(ProjectName);
        }

        /// <summary>
        /// Метод проверки удаления проекта (без файлов)
        /// </summary>
        [Test]
        public void DeleteProjectNoFileTest()
        {
            //создать проект, который будем удалять
            CreateProject(ProjectName);

            // Удалить проект
            DeleteProjectFromList(ProjectName);

            Assert.IsTrue(GetIsNotExistProject(ProjectName), "Ошибка: проект не удалился");
        }

        /// <summary>
        /// Метод проверки удаления проекта (с файлом)
        /// </summary>
        [Test]
        public void DeleteProjectWithFileTest()
        {
            // Создать проект
            CreateProject(ProjectName, DocumentFile);

            // Дождаться, пока документ догрузится
			Assert.IsTrue(WorkspacePage.WaitProjectLoad(ProjectName), "Ошибка: документ не загрузился");

            // Кликнуть по строке с проектом, чтобы открылась информация о нем (чтобы видно было документ)
            WorkspacePage.OpenProjectInfo(ProjectName);
            // TODO Sleep
            Thread.Sleep(2000);
            // Выделить галочку проекта
            SelectProjectInList(ProjectName);
            // Нажать кнопку удалить
            WorkspacePage.ClickDeleteProjectBtn();
            // Дождаться диалога выбора режима удаления
            Assert.IsTrue(WorkspacePage.WaitDeleteModeDialog(),
                "Ошибка: не появился диалог удаления проекта");
            // Нажать Удалить Проект
            Assert.IsTrue(WorkspacePage.ClickDeleteProjectDeleteMode(),
                "Ошибка: нет кнопки Удалить проект");

            // Проверить, что проект удалился
            Assert.IsTrue(GetIsNotExistProject(ProjectName), "Ошибка: проект не удалился");
        }

        /// <summary>
        /// тестирование совпадения имени проекта с удаленным
        /// </summary>
        [Test]
        public void CreateProjectDeletedNameTest()
        {
            // Создать проект
            CreateProject(ProjectName);

            // Удалить проект
            DeleteProjectFromList(ProjectName);

            // Проверить, остался ли проект в списке
            Assert.IsTrue(GetIsNotExistProject(ProjectName), "Ошибка: проект не удалился");

            //создание нового проекта с именем удаленного
            FirstStepProjectWizard(ProjectName);

            // Проверить, что не появилось сообщение о существующем имени
            AssertErrorDuplicateName(false);
        }

        /// <summary>
        /// метод тестирования создания проекта с существующим именем
        /// </summary>
        [Test]
        public void CreateProjectDuplicateNameTest()
        {
            // Создать проект
            CreateProject(ProjectName);
            Thread.Sleep(1000);
            // Начать создание проекта с этим же именем
            FirstStepProjectWizard(ProjectName);
            WorkspaceCreateProjectDialog.ClickNextStep();
            // Проверить, что появилась ошибка и поле Имя выделено ошибкой - ASSERT внутри
            AssertErrorDuplicateName();
        }

        /// <summary>
        /// метод проверки невозможности создания проекта в большим именем(>100 символов)
        /// </summary>
        [Test]
        public void CreateProjectBigNameTest()//Переделать тест, теперь поле с ограничением символов
        {
            string bigName = ProjectName + "12345678901234567890123456789012345678901234567890123456789012345678901";
            // Проверить, что создалось имя длиннее 100 символов
            Assert.IsTrue(bigName.Length > 100, "Измените тест: длина имени должна быть больше 100");
            // Создать проект с превышающим лимит именем
            CreateProjectWithoutCheckExist(bigName);
            // Проверить, что проект не сохранился
            Assert.IsTrue(GetIsNotExistProject(bigName), "Ошибка: проект с запрещенно большим именем создался");
        }

        /// <summary>
        /// метод проверки на ограничение имени проекта (100 символов)
        /// </summary>
        [Test]
        public void CreateProjectLimitNameTest()
        {
            string limitName = ProjectName + "1234567890123456789012345678901234567890123456789012345678901234567890";
            // Проверить, что создалось имя максимальной длины
            Assert.IsTrue(limitName.Length == 100, "Измените тест: длина имени должна быть ровно 100");
            // Создать проект с максимальным возможным именем
            CreateProject(limitName);
            // Проверить, что проект создался
            CheckProjectInList(limitName);
        }

        /// <summary>
        /// метод тестирования создания проектов с одинаковыми source и target языками
        /// </summary>
        [Test]
        public void CreateProjectEqualLanguagesTest()
        {
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard(ProjectName, false);
            WorkspaceCreateProjectDialog.ClickNextStep();

            // Проверить, что появилось сообщение о совпадающих языках
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsExistErrorDuplicateLanguage(),
                "Ошибка: не появилось сообщение о совпадающих языках");

            // Проверить, что не перешли на следующий шаг
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsFirstStep(),
                "Ошибка: не остались на первом шаге");
        }


        /// <summary>
        /// метод для тестирования недопустимых символов в имени проекта
        /// </summary>
        [Test]
        public void CreateProjectForbiddenSymbolsTest()
        {
            // Создать имя с недопустимыми символами
            string projectNameForbidden = ProjectName + " *|\\:\"<\\>?/ ";
            // Создать проект
            FirstStepProjectWizard(projectNameForbidden);
            WorkspaceCreateProjectDialog.ClickNextStep();

            // Проверить, что появилась ошибка и поле Имя выделено ошибкой
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsExistErrorForbiddenSymbols(),
                "Ошибка: не появилось сообщение о запрещенных символах в имени");
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsNameInputError(),
                "Ошибка: поле с именем не отмечено ошибкой");
        }

        /// <summary>
        /// метод для тестирования проекта с пустым именем
        /// </summary>
        [Test]
        public void CreateProjectEmptyNameTest()
        {
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard("");
            WorkspaceCreateProjectDialog.ClickNextStep();

            // Проверить, что появилась ошибка и поле Имя выделено ошибкой
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsExistErrorMessageNoName(),
                "Ошибка: не появилось сообщение о существующем имени");
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsNameInputError(),
                "Ошибка: поле с именем не отмечено ошибкой");
        }

        /// <summary>
        /// метод для тестирования создания имени проекта состоящего из одного пробела
        /// </summary>
        [Test]
        public void CreateProjectSpaceNameTest()
        {
            //1 шаг - заполнение данных о проекте
            FirstStepProjectWizard(" ");
            WorkspaceCreateProjectDialog.ClickNextStep();

            // Проверить, что появилась ошибка и поле Имя выделено ошибкой
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsExistErrorMessageNoName(),
                "Ошибка: не появилось сообщение о существующем имени");
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsNameInputError(),
                "Ошибка: поле с именем не отмечено ошибкой");
        }

        /// <summary>
        /// метод тестирования создания проекта с именем содержащим пробелы
        /// </summary>
        [Test]
        public void CreateProjectSpacePlusSymbolsNameTest()
        {
            string projectName = ProjectName + "  " + "SpacePlusSymbols";
            // Создаем проект (проверка внутри)
            CreateProject(projectName);
        }

        /// <summary>
        /// метод для тестирования отмены назначения документа пользователю
        /// </summary>
        [Test]
        public void ReassignDocumentToUserTest()
        {
            //Создать пустой проект
            CreateProject(ProjectName);

            //Добавление документа
            ImportDocumentProjectSettings(DocumentFile, ProjectName);
            //Назначение задачи на пользователя
            AssignTask();

            // Выбрать документ
            SelectDocumentInProject(1);
            // Открыть диалог Progress
            ProjectPage.ClickProgressBtn();
            ProjectPage.WaitProgressDialogOpen();

            // Нажать Отмену назначения
            ProjectPage.ClickCancelAssignBtn();
            // Подтвердить
            ProjectPage.ConfirmClickYes();
            // TODO проверить без sleep
            Thread.Sleep(2000);
            // Проверить, изменился ли статус на not Assigned
            Assert.IsTrue(ProjectPage.GetIsAssignStatusNotAssigned(),
                "Статус назначения не изменился на notAssigned");

            // Назначить ответственного в окне Progress
            ProjectPage.ClickUserNameCell();
            // Выбрать нужное имя
            ProjectPage.WaitAssignUserList();
            ProjectPage.ClickAssignUserListUser(UserName);
            // Нажать на Assign
            ProjectPage.ClickAssignBtn();

            // Дождаться появления Cancel
            ProjectPage.WaitCancelAssignBtnDisplay();
            // Нажать на Close
            ProjectPage.CloseAssignDialogClick();
        }

        /// <summary>
        /// Удаление документа из проекта
        /// </summary>
        [Test]
        public void DeleteDocumentFromProject()
        {
            // Создать проект, загрузить документ
            CreateProjectImportDocument(DocumentFile);

            // Выбрать документ
            SelectDocumentInProject(1);
            // Нажать удалить
            ProjectPage.ClickDeleteBtn();

            // Подтвердить
            ProjectPage.ConfirmClickYes();
            Thread.Sleep(5000);
            // Проверить, что документа нет
            Assert.IsFalse(ProjectPage.GetIsExistDocument(1),
                "Ошибка: документ не удалился");
        }

        /// <summary>
        /// отмена создания проекта на первом шаге
        /// </summary>
        [Test]
        public void CancelFirstTest()
        {
            // Нажать <Create>
            WorkspacePage.ClickCreateProject();
            // Ждем загрузки формы
            WorkspaceCreateProjectDialog.WaitDialogDisplay();

            // Нажать Отмену
            WorkspaceCreateProjectDialog.ClickCloseDialog();
            // Проверить, что форма создания проекта закрылась
            Assert.IsTrue(WorkspaceCreateProjectDialog.WaitDialogDisappear(), "Ошибка: не закрылась форма создания проекта");
        }

        /// <summary>
        /// изменение имени проекта на новое по нажатию кнопки Back
        /// </summary>
        [Test]
        public void ChangeProjectNameOnNew()
        {
            // Открыли форму создания проекта, заполнили поля
            FirstStepProjectWizard(ProjectName);
            // Next
            WorkspaceCreateProjectDialog.ClickNextStep();
            // Нажать Back
            WorkspaceCreateProjectDialog.ClickBackBtn();
            // Проверили, что вернулись на первый шаг
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsFirstStep(),
                "Ошибка: по кнопке Back не вернулись на предыдущий шаг (где имя проекта)");

            // Ввести название проекта
            WorkspaceCreateProjectDialog.FillProjectName("TestProject" + DateTime.Now.Ticks);
            // Next
            WorkspaceCreateProjectDialog.ClickNextStep();
            // Проверить, что ошибки не появилось
            AssertErrorDuplicateName(false);
            // Проверить, что перешли на шаг выбора ТМ
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsStepChooseTM(),
                "Ошибка: не перешли на следующий шаг (выбора ТМ)");
        }

        /// <summary>
        /// изменение имени проекта на существующее
        /// </summary>
        [Test]
        public void ChangeProjectNameOnExist()
        {
            // Создать проект
            CreateProject(ProjectName);
            Thread.Sleep(1000);

            // Открыли форму создания проекта, заполнили поля
            string newProjectName = "TestProject" + DateTime.Now.Ticks;
            FirstStepProjectWizard(newProjectName);
            // Next
            WorkspaceCreateProjectDialog.ClickNextStep();
            // Нажать Back
            WorkspaceCreateProjectDialog.ClickBackBtn();
            // Проверили, что вернулись на первый шаг
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsFirstStep(),
                "Ошибка: по кнопке Back не вернулись на предыдущий шаг (где имя проекта)");
            // Изменить имя
            WorkspaceCreateProjectDialog.FillProjectName(ProjectName);
            // Next
            WorkspaceCreateProjectDialog.ClickNextStep();
            // Проверить, что ошибка появилась
            AssertErrorDuplicateName(true);
            // Проверить, что не перешли на шаг выбора ТМ
            Assert.IsFalse(WorkspaceCreateProjectDialog.GetIsStepChooseTM(),
                "Ошибка: перешли на следующий шаг (выбора ТМ)");
        }

        /// <summary>
        /// изменение имени проекта на удаленное
        /// </summary>
        [Test]
        public void ChangeProjectNameOnDeleted()
        {
            // Создать проект
            CreateProject(ProjectName);
            // Удалить проект
            DeleteProjectFromList(ProjectName);
            // Проверить, остался ли проект в списке
            Assert.IsTrue(GetIsNotExistProject(ProjectName), "Ошибка: проект не удалился");

            //создание нового проекта с именем удаленного
            string newProjectName = "TestProject" + DateTime.Now.Ticks;
            FirstStepProjectWizard(newProjectName);
            // Next
            WorkspaceCreateProjectDialog.ClickNextStep();
            // Нажать Back
            WorkspaceCreateProjectDialog.ClickBackBtn();
            // Проверили, что вернулись на первый шаг
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsFirstStep(),
                "Ошибка: по кнопке Back не вернулись на предыдущий шаг (где имя проекта)");
            // Изменить имя
            WorkspaceCreateProjectDialog.FillProjectName(ProjectName);
            // Next
            WorkspaceCreateProjectDialog.ClickNextStep();
            // Проверить, что ошибка не появилась
            AssertErrorDuplicateName(false);
            Thread.Sleep(2000);
            // Проверить, что перешли на шаг выбора ТМ
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsStepChooseTM(),
                "Ошибка: перешли на следующий шаг (выбора ТМ)");
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



        /// <summary>
        /// Удаление проект на вкладке проектов по имени
        /// </summary>
        /// <param name="ProjectNameToDelete">имя проекта, который надо удалить</param>
        protected void DeleteProjectFromList(string ProjectNameToDelete)
        {
            // Выбрать этот проект
            SelectProjectInList(ProjectNameToDelete);
            // Нажать Удалить
            WorkspacePage.ClickDeleteProjectBtn();
            // Подтвердить
            Assert.IsTrue(WorkspacePage.ClickConfirmDelete(), "Ошибка: не появилась форма подтверждения удаления проекта");
            // Дождаться, пока пропадет диалог подтверждения удаления
            Assert.IsTrue(WorkspacePage.WaitUntilDeleteConfirmFormDisappear(), "Ошибка: не появилась форма подтверждения удаления проекта");
            Thread.Sleep(500);
        }

        /// <summary>
        /// Проверка, есть ли ошибка существующего имени
        /// </summary>
        /// <param name="shouldErrorExist">условие для результата проверки - должна ли быть ошибка</param>
        protected void AssertErrorDuplicateName(bool shouldErrorExist = true)
        {
            if (!shouldErrorExist)
            {
                // Т.к. ожидаем, что ошибка не появится, опускаем таймаут
                setDriverTimeoutMinimum();
            }

            // Проверить, что поле Имя отмечено ошибкой
            bool isExistErrorInput = WorkspaceCreateProjectDialog.GetIsNameInputError();
            // Проверить, что есть сообщение, что имя существует
            bool isExistErrorMessage = WorkspaceCreateProjectDialog.GetIsExistErrorMessageNameExists();

            string errorMessage = "\n";
            bool isError = false;
            // Ошибка должна появиться
            if (shouldErrorExist)
            {
                if (!isExistErrorInput)
                {
                    isError = true;
                    errorMessage += "Ошибка: поле Название не отмечено ошибкой\n";
                }
                if (!isExistErrorMessage)
                {
                    isError = true;
                    errorMessage += "Ошибка: не появилось сообщение о существующем имени";
                }
            }
            // Ошибка НЕ должна появиться
            else
            {
                if (isExistErrorInput)
                {
                    isError = true;
                    errorMessage += "Ошибка: поле Название отмечено ошибкой\n";
                }
                if (isExistErrorMessage)
                {
                    isError = true;
                    errorMessage += "Ошибка: появилось сообщение о существующем имени";
                }
            }

            // Проверить условие
            Assert.IsFalse(isError, errorMessage);

            if (!shouldErrorExist)
            {
                setDriverTimeoutDefault();
            }
        }
    }
}