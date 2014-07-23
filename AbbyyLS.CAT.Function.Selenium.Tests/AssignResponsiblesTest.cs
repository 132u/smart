using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	class AssignResponsiblesTest : BaseTest
	{
		public AssignResponsiblesTest(string url, string workspaceUrl, string browserName)
			: base (url, workspaceUrl, browserName)
		{
		}

		/// <summary>
		/// Старт тестов. Авторизация
		/// </summary>
		[SetUp]
		public void Setup()
		{
			Authorization();
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии прогресса в Workflow
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceOnProgressLink()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(ProjectName);

			// Открываем инфо документа 
			WorkspacePage.OpenDocumentInfo(1);

			// Открываем окно с правами пользователя через прогресс документа
			WorkspacePage.ClickDocumentProgress();

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии кнопки открытия прав в Workflow
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceOnAssignBtn()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии прогресса в окне проекта
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnProgressLink()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем окно прав исполнителей
			ProjectPage.ClickDocumentProgress();
			Thread.Sleep(1000);

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии кнопки открытия прав в окне проекта
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnAssignBtn()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем инфо документа 
			ProjectPage.OpenDocumentInfo(1);

			// Открываем окно прав исполнителей
			ProjectPage.ClickAssignBtn();
			Thread.Sleep(1000);

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии кнопки Progress при выборе документа
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnProgressBtn()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем инфо документа 
			ProjectPage.SelectDocument(1);

			// Открываем окно прав исполнителей
			ProjectPage.ClickProgressBtn();
			Thread.Sleep(1000);

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при загрузке документа в проект
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceUploadDocument()
		{
			// Создание проекта
			CreateProject(ProjectName);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(ProjectName);

			// Открываем диалог загрузки документа
			WorkspacePage.ClickDocumentUploadBtn();

			// Ожидаем пока загрузится диалог
			ProjectPage.WaitImportDialogDisplay();

			// Открываем диалог добавления документа
			ProjectPage.ClickAddDocumentInImport();

			// Добавляем документ
			FillAddDocumentForm(EditorTXTFile);

			ProjectPage.ClickNextImportDialog();
			ProjectPage.ClickNextImportDialog();

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilMasterResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при загрузке документа в окне проекта
		/// </summary>
		[Test]
		public void ResponsiblesProjectUploadDocument()
		{
			// Создание проекта
			CreateProject(ProjectName);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем диалог загрузки документа
			ProjectPage.ClickImportBtn();

			// Ожидаем пока загрузится диалог
			ProjectPage.WaitImportDialogDisplay();

			// Открываем диалог добавления документа
			ProjectPage.ClickAddDocumentInImport();

			// Добавляем документ
			FillAddDocumentForm(EditorTXTFile);

			ProjectPage.ClickNextImportDialog();
			ProjectPage.ClickNextImportDialog();

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilMasterResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Проверка соответствия (списков пользователей и групп с одной стороны и выпадающего списка выбора исполнителя с другой)
		/// </summary>
		[Test]
		public void VerifyUsersAndGroupsLists()
		{
			List<string> usersList = new List<string>();
			List<string> groupsList = new List<string>();
			List<string> responsibleUsersList = new List<string>();
			List<string> responsibleGroupList = new List<string>();
			
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();

			// Ожидание открытия страницы
			Assert.IsTrue(UserRightsPage.WaitUntilUsersRightsDisplay(), "Ошибка: Страница прав пользователя не открылась.");

			// Получаем список 
			usersList = UserRightsPage.GetUserFullnameList();

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();

			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(UserRightsPage.WaitUntilGroupsRightsDisplay(), "Ошибка: Страница прав групп пользователей не открылась.");

			// Получаем список 
			groupsList = UserRightsPage.GetGroupNameList();

			// Добавляем в начало каждой строки слово Group как в списке диалога
			for (int i= 0; i < groupsList.Count; i++)
			{
				groupsList[i] = "Group: " + groupsList[i];
			}

			// Перейти на страницу проектов
			SwitchWorkspaceTab();

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(1);

			Thread.Sleep(1000);

			// Получаем список из выпадающего списка исполнителей
			responsibleUsersList = ResponsiblesDialog.GetResponsibleUsersListByRowNumber(1);

			for (int i = 0; i < responsibleUsersList.Count; i++)
			{
				Assert.IsTrue(usersList.Contains(responsibleUsersList[i]),
					"Ошибка: Элемент выпадающего списка " + responsibleUsersList[i] + " отсутствует в списке пользователей.");
				usersList.Remove(responsibleUsersList[i]);
			}

			// Получаем список из выпадающего списка исполнителей
			responsibleGroupList = ResponsiblesDialog.GetResponsibleGroupsListByRowNumber(1);

			for (int i = 0; i < responsibleGroupList.Count; i++)
			{
				Assert.IsTrue(groupsList.Contains(responsibleGroupList[i]),
					"Ошибка: Элемент выпадающего списка " + responsibleGroupList[i] + " отсутствует в списке групп пользователей.");
				groupsList.Remove(responsibleGroupList[i]);
			}

			Assert.IsTrue(groupsList.Count == 0,
				"Ошибка: В выпадающем списке отсутсвуют " + groupsList.Count + " элемента списка групп пользователей.");

			Assert.IsTrue(usersList.Count == 0,
				"Ошибка: В выпадающем списке отсутсвуют " + usersList.Count + " элемента списка пользователей.");
		}

		/// <summary>
		/// Проверка отображения в выпадающем списке исполнителей новой группы пользователей
		/// </summary>
		[Test]
		public void AddNewGroup()
		{
			List<string> responsibleGroupList = new List<string>();
			string groupName = "GroupTest" + DateTime.Now.Ticks.ToString();
			bool isPresent = false;

			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();

			// Ожидание открытия страницы
			Assert.IsTrue(UserRightsPage.WaitUntilUsersRightsDisplay(), "Ошибка: Страница прав пользователя не открылась.");

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();

			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(UserRightsPage.WaitUntilGroupsRightsDisplay(), "Ошибка: Страница прав групп пользователей не открылась.");

			// Добавляем новую группу
			CreateNewGroup(groupName);

			// Перейти на страницу проектов
			SwitchWorkspaceTab();

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(1);

			Thread.Sleep(1000);

			// Получаем список из выпадающего списка исполнителей
			responsibleGroupList = ResponsiblesDialog.GetResponsibleGroupsListByRowNumber(1);

			foreach (string group in responsibleGroupList)
			{				
				if (group == "Group: " + groupName)
					isPresent = true;
			}

			Assert.IsTrue(isPresent,
				"Ошибка: В выпадающем списке отсутсвует новая группа: " + groupName);
		}

		/// <summary>
		/// Назначение пользователя на один из этапов
		/// </summary>
		[Test]
		public void AssignUserOneTask()
		{
			// Проверка присутствия текущего пользователя в группе Administrators
			CheckUserInAdministratorsGroup();

			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для первой задачи группы Administrator
			SetResponsible(1, "Administrators", true);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка наличия записи "StageNumber=1" в адресной строке
			Assert.IsTrue(Driver.Url.Contains("StageNumber=1"),
				"Ошибка: В адресной строке отсутствует запись \"StageNumber=1\".");

			// Проверка задачи в редакторе
			Assert.AreEqual("Translation (T)", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора отсутствует нужная задача.");
		}

		/// <summary>
		/// Назначение пользователя на два этапа
		/// </summary>
		[Test]
		public void AssignUserFewTasks()
		{
			// Проверка присутствия текущего пользователя в группе Administrators
			CheckUserInAdministratorsGroup();
			
			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			
			// Загрузить файл
			WorkspaceCreateProjectDialog.ClickAddDocumentBtn();
			FillAddDocumentForm(EditorTXTFile);

			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Editing
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Editing");
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 6) Настройка Pretranslate. Проверка создания проекта
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для первой задачи группы Administrator
			SetResponsible(1, "Administrators", true);

			// Выбор в качестве исполнителя для второй задачи группы Administrator
			SetResponsible(2, "Administrators", true);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Кликаем открытие первого документа
			ProjectPage.OpenDocument(1);

			Thread.Sleep(1000);

			// Проверка отображения диалога выбора задачи
			Assert.IsTrue(ResponsiblesDialog.WaitUntilChooseTaskDialogDisplay(),
				"Ошибка: Диалог выбора задачи не открылся.");
		}

		/// <summary>
		/// Удаление задачи, назначенной пользователю
		/// </summary>
		[Test]
		public void DeleteUserTask()
		{
			// Проверка присутствия текущего пользователя в группе Administrators
			CheckUserInAdministratorsGroup();

			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для первой задачи группы Administrator
			SetResponsible(1, "Administrators", true);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Удаление первой задачи Translation
			ProjectPage.ClickProjectSettingsWFDeleteTask(1);

			// Ожидание открытия диалога подтверждения
			Assert.IsTrue(ResponsiblesDialog.WaitUntilConfirmDialogDisplay(),
				"Ошибка: Диалог подтверждения не открылся.");
		}

		/// <summary>
		/// Назначение/сброс пользователей на задачу
		/// </summary>
		[Test]
		public void AssignDifferentUsersOneTask()
		{
			// Проверка второго пользователя
			CheckUserPresent(UserName2);
			
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для первой задачи первого юзера
			SetResponsible(1, UserName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка наличия записи "StageNumber=1" в адресной строке
			Assert.IsTrue(Driver.Url.Contains("StageNumber=1"),
				"Ошибка: В адресной строке отсутствует запись \"StageNumber=1\".");

			// Проверка задачи в редакторе
			Assert.AreEqual("Translation (T)", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора отсутствует нужная задача.");

			// Выходим из редактора
			EditorClickBackBtn();

			// Перейти на страницу проектов
			SwitchWorkspaceTab();

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Отменяем исполнителя
			CancelAssignUser(1);

			// Выбор в качестве исполнителя для первой задачи второго юзера
			SetResponsible(1, UserName2, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка отсутствия записи "StageNumber=1" в адресной строке
			Assert.IsFalse(Driver.Url.Contains("StageNumber=1"),
				"Ошибка: В адресной строке присутствует запись \"StageNumber=1\".");

			// Проверка отсутсвия задачи в редакторе
			Assert.AreEqual("View mode", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора есть задача.");

			// Выходим из редактора
			EditorClickBackBtn();

			// Переходим на страницу проектов
			SwitchWorkspaceTab();

			// Разлогиниться
			WorkspacePage.ClickLogoff();

			Authorization("TestAccount", true);

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка наличия записи "StageNumber=1" в адресной строке
			Assert.IsTrue(Driver.Url.Contains("StageNumber=1"),
				"Ошибка: В адресной строке отсутствует запись \"StageNumber=1\".");

			// Проверка задачи в редакторе
			Assert.AreEqual("Translation (T)", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора отсутствует нужная задача.");
		}

		/// <summary>
		/// Проверка назначения невыбранного пользователя
		/// </summary>
		[Test]
		public void AssignNobody()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Кликнуть подтверждение для заданной задачи
			ResponsiblesDialog.ClickAssignBtn(1);

			// Ожидание открытия инфо
			Assert.IsTrue(ResponsiblesDialog.WaitUntilInfoDisplay(),
				"Ошибка: Форма инфо не открылась.");
		}

		/// <summary>
		/// Назначение/сброс пользователя на задачу
		/// </summary>
		[Test]
		public void UnAssignUser()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для первой задачи первого юзера
			SetResponsible(1, UserName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка наличия записи "StageNumber=1" в адресной строке
			Assert.IsTrue(Driver.Url.Contains("StageNumber=1"),
				"Ошибка: В адресной строке отсутствует запись \"StageNumber=1\".");

			// Проверка задачи в редакторе
			Assert.AreEqual("Translation (T)", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора отсутствует нужная задача.");

			// Выходим из редактора
			EditorClickBackBtn();

			// Перейти на страницу проектов
			SwitchWorkspaceTab();

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Отменяем исполнителя
			CancelAssignUser(1);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка отсутствия записи "StageNumber=1" в адресной строке
			Assert.IsFalse(Driver.Url.Contains("StageNumber=1"),
				"Ошибка: В адресной строке присутствует запись \"StageNumber=1\".");

			// Проверка отсутсвия задачи в редакторе
			Assert.AreEqual("View mode", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора есть задача.");
		}



		/// <summary>
		/// Создает новую группу с заданным именем
		/// </summary>
		/// <param name="groupName">Имя группы</param>
		private void CreateNewGroup(string groupName)
		{
			// Добавляем новую группу
			UserRightsPage.ClickCreateGroup();

			// Ожидание открытия формы создания группы
			Assert.IsTrue(UserRightsPage.WaitUntilCreateFormDisplay(), "Ошибка: Форма создания группы не открылась.");

			// Вводим имя новой группы
			UserRightsPage.AddGroupName(groupName);

			// Сохраняем имя новой группы
			UserRightsPage.ClickSaveGroup();

			Thread.Sleep(1000);

			// Проверка, что форма создания грцппы закрылась
			Assert.IsTrue(UserRightsPage.WaitUntilCreateFormDisappear(), "Ошибка: Форма создания группы не закрылась.");
		}

		/// <summary>
		/// Проверка, что текущий пользователь входит в группу Administrators
		/// </summary>
		private void CheckUserInAdministratorsGroup()
		{
			string userName = "";
			List<string> usersInGroup = new List<string>();
			
			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();

			// Ожидание открытия страницы
			Assert.IsTrue(UserRightsPage.WaitUntilUsersRightsDisplay(), "Ошибка: Страница прав пользователя не открылась.");

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();

			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(UserRightsPage.WaitUntilGroupsRightsDisplay(), "Ошибка: Страница прав групп пользователей не открылась.");

			// Получение имени пользователя
			userName = WorkspacePage.GetUserName();

			// Окрытие группы Administrators
			UserRightsPage.ClickGroupByName("Administrators");

			Thread.Sleep(1000);

			// Получение списка пользователей в группе Administrators
			usersInGroup = UserRightsPage.GetDisplayUsersInGroup();

			Assert.IsTrue(usersInGroup.Contains(userName), "Ошибка: Текущего пользователя нет в группе Administrators.");

			// Возвращаемся на страницу проектов
			SwitchWorkspaceTab();
		}

		/// <summary>
		/// Проверка, что текущий пользователь присутствует
		/// </summary>
		private void CheckUserPresent(string userName)
		{
			List<string> usersList = new List<string>();

			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();

			// Ожидание открытия страницы
			Assert.IsTrue(UserRightsPage.WaitUntilUsersRightsDisplay(), "Ошибка: Страница прав пользователя не открылась.");

			// Получение списка пользователей
			usersList = UserRightsPage.GetUserFullnameList();

			Assert.IsTrue(usersList.Contains(userName), "Ошибка: Пользователь " + userName + " отсутствует в списке пользователей.");

			// Возвращаемся на страницу проектов
			SwitchWorkspaceTab();
		}

		/// <summary>
		/// Выбирает из выпадающего списка пользователя или группу по имени
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <param name="name">Имя пользователя или группы</param>
		/// <param name="isGroup">Выбор группы</param>
		private void SetResponsible(int rowNumber, string name, bool isGroup)
		{
			string fullName = "";
			
			// Открыть выпадающий список
			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(rowNumber);

			if (isGroup)
				fullName = "Group: " + name;
			else
				fullName = name;

			// Выбрать для заданной задачи имя исполнителя
			ResponsiblesDialog.SetVisibleResponsible(rowNumber, fullName);

			// Кликнуть подтверждение для заданной задачи
			ResponsiblesDialog.ClickAssignBtn(rowNumber);
		}

		/// <summary>
		/// Открытие диалога выбора исполнителя
		/// </summary>
		/// <param name="projectName">Имя проекта</param>
		private void OpenAssignDialog(string projectName)
		{
			// Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(projectName);

			// Открываем инфо документа 
			WorkspacePage.OpenDocumentInfo(1);

			// Открываем окно с правами пользователя через кнопку прав
			WorkspacePage.ClickDocumentAssignBtn();

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Отмена исполнителя для заданной строки
		/// </summary>
		/// <param name="rowNumber">Номер строки</param>
		private void CancelAssignUser(int rowNumber)
		{
			// Отменяем исполнителя
			ResponsiblesDialog.ClickCancelBtn(rowNumber);

			// Ожидание открытия диалога подтверждения
			Assert.IsTrue(ResponsiblesDialog.WaitUntilConfirmDialogDisplay(),
				"Ошибка: Диалог подтверждения не открылся.");

			// Подтверждение
			ResponsiblesDialog.ClickYesBtn();

			// Ожидание закрытия диалога подтверждения
			Assert.IsTrue(ResponsiblesDialog.WaitUntilConfirmDialogDissapear(),
				"Ошибка: Диалог подтверждения не закрылся.");
		}
	}
}
