using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки назначения пользователя на задачу
	/// </summary>
	class AssignResponsiblesTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public AssignResponsiblesTest(string browserName)
			: base (browserName)
		{
		}

		/// <summary>
		/// Подготовка для группы тестов
		/// </summary>
		[TestFixtureSetUp]
		public void SetupAll()
		{
			try
			{
				// Создание уникального имени проекта
				CreateUniqueNamesByDatetime();

				// Запись имени для дальнейшего использования в группе тестов
				_projectNoChangesName = ProjectName;
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Ошибка в конструкторе : " + ex.Message, ex);
				throw;
			}
		}

		/// <summary>
		/// Подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Не выходить из браузера после теста
			QuitDriverAfterTest = false;

			// Переход на страницу workspace
			GoToWorkspace();
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии прогресса в Workflow
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceOnProgressLink()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);
			
			// Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(_projectNoChangesName);

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
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(_projectNoChangesName);
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при загрузке документа в проект
		/// </summary>
		[Category("PRX_6987")]
		[Test]
		public void ResponsiblesWorkspaceUploadDocument()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);

			//Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(_projectNoChangesName);

			//Открываем диалог загрузки документа
			WorkspacePage.ClickDocumentUploadBtn();

			//Ожидаем пока загрузится диалог
			ProjectPage.WaitImportDialogDisplay();

			// Добавляем документ
			UploadFile(TestFile.DocumentFileToConfirm, ADD_FILE_TO_PROJECT);

			ProjectPage.ClickNextImportDialog();
			ProjectPage.ClickNextImportDialog();

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilMasterResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии прогресса в окне проекта
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnProgressLink()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);
			
			// Открываем проект
			OpenProjectPage(_projectNoChangesName);

			// Открываем окно прав исполнителей
			ProjectPage.ClickDocumentProgress();
			
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
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);

			// Открываем проект
			OpenProjectPage(_projectNoChangesName);

			// Открываем инфо документа 
			ProjectPage.OpenDocumentInfo(1);

			// Открываем окно прав исполнителей
			ProjectPage.ClickAssignRessponsibleBtn();
			
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
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);
			
			// Открываем проект
			OpenProjectPage(_projectNoChangesName);

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
		/// Проверка отображения окна с правами пользователя при загрузке документа в окне проекта
		/// </summary>
		[Category("PRX_6987")]
		[Test]
		public void ResponsiblesProjectUploadDocument()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);
			
			// Открываем проект
			OpenProjectPage(_projectNoChangesName);

			//// Открываем диалог загрузки документа
			ProjectPage.ClickImportBtn();

			//// Ожидаем пока загрузится диалог
			ProjectPage.WaitImportDialogDisplay();

			// Добавляем документ
			UploadFile(TestFile.DocumentFileToConfirm, ADD_FILE_ON_PROJECT_PAGE);

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
			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();

			// Ожидание открытия страницы
			Assert.IsTrue(UserRightsPage.WaitUntilUsersRightsDisplay(), "Ошибка: Страница прав пользователя не открылась.");

			// Получаем список 
			List<string> usersList = UserRightsPage.GetUserFullnameList();

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();

			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(UserRightsPage.WaitUntilGroupsRightsDisplay(), "Ошибка: Страница прав групп пользователей не открылась.");

			// Получаем список 
			List<string> groupsList = UserRightsPage.GetGroupNameList();

			// Добавляем в начало каждой строки слово Group как в списке диалога
			for (int i= 0; i < groupsList.Count; i++)
			{
				groupsList[i] = "Group: " + groupsList[i];
			}

			// Перейти на страницу проектов
			SwitchWorkspaceTab();

			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(_projectNoChangesName);

			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(1);

			Thread.Sleep(1000);

			// Получаем список из выпадающего списка исполнителей
			List<string> responsibleUsersList = ResponsiblesDialog.GetResponsibleUsersListByRowNumber(1);

			for (int i = 0; i < responsibleUsersList.Count; i++)
			{
				Assert.IsTrue(usersList.Contains(responsibleUsersList[i]),
					"Ошибка: Элемент выпадающего списка " + responsibleUsersList[i] + " отсутствует в списке пользователей.");
				usersList.Remove(responsibleUsersList[i]);
			}

			// Получаем список из выпадающего списка групп исполнителей
			List<string> responsibleGroupList = ResponsiblesDialog.GetResponsibleGroupsListByRowNumber(1);

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

			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(_projectNoChangesName);

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
				"Ошибка: В выпадающем списке отсутствует новая группа: " + groupName);
		}

		/// <summary>
		/// Назначение пользователя на один из этапов
		/// </summary>
		[Test]
		public void AssignUserOneTask()
		{
			// Создание проекта
			CreateProjectIfNotCreated(ProjectName, TestFile.EditorTXTFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для первой задачи группы Administrator
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

			// Проверка задачи в редакторе
			Assert.AreEqual("Translation (T):", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора отсутствует нужная задача.");
		}

		/// <summary>
		/// Назначение пользователя на два этапа
		/// </summary>
		[Category("PRX_6987")]
		[Test]
		public void AssignUserFewTasks()
		{
			CreateProject(ProjectName, TestFile.EditorTXTFile, false, "", Workspace_CreateProjectDialogHelper.SetGlossary.None, "", false, Workspace_CreateProjectDialogHelper.MT_TYPE.None, true, 1, 1, 0);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для первой задачи группы Administrator
			SetResponsible(1, UserName, false);

			// Выбор в качестве исполнителя для второй задачи группы Administrator
			SetResponsible(2, UserName, false);

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
			// Создание проекта
			CreateProject(ProjectName, TestFile.EditorTXTFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для первой задачи группы Administrator
			SetResponsible(1, UserName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Удаление первой задачи Translation
			ProjectPage.ClickProjectSettingsWFDeleteTask(1);

			// Ожидание открытия диалога подтверждения
			Assert.IsTrue(ResponsiblesDialog.WaitUntilConfirmDialogDisplay(),
				"Ошибка: Диалог подтверждения не открылся.");
		}

		/// <summary>
		/// Назначение/сброс пользователей на задачу
		/// </summary>
		[Category("PRX_6556")]
		[Test]
		public void AssignDifferentUsersOneTask()
		{
			// После теста закрываем браузер, т.к. залогиниваемся под альтернативным пользователем
			QuitDriverAfterTest = true;
			
			// Проверка второго пользователя
			сheckUserPresent(UserName2);

			// Создание проекта
			CreateProjectIfNotCreated(ProjectName, TestFile.EditorTXTFile);

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

			// Подтверждение назначения
			ProjectPage.ClickAllAcceptBtns();
			Thread.Sleep(1000);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка задачи в редакторе
			Assert.AreEqual("Translation (T):", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора отсутствует нужная задача.");

			// Выходим из редактора
			EditorClickHomeBtn();

			// Переход в WS
			GoToWorkspace();

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Отменяем исполнителя
			сancelAssignUser(1);

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

			// Проверка отсутсвия задачи в редакторе
			Assert.AreEqual("View mode:", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора есть задача.");

			// Выходим из редактора
			EditorClickHomeBtn();

			// Разлогиниться
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			Authorization("TestAccount", true);

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Подтверждение назначения
			ProjectPage.ClickAllAcceptBtns();
			Thread.Sleep(1000);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка задачи в редакторе
			Assert.AreEqual("Translation (T):", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора отсутствует нужная задача.");
		}

		/// <summary>
		/// Проверка назначения невыбранного пользователя
		/// </summary>
		[Test]
		public void AssignNobody()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, TestFile.EditorTXTFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(_projectNoChangesName);

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
			CreateProjectIfNotCreated(ProjectName, TestFile.EditorTXTFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для первой задачи первого юзера
			SetResponsible(1, UserName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(
				ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Подтверждение назначения
			ProjectPage.ClickAllAcceptBtns();
			Thread.Sleep(1000);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка задачи в редакторе
			Assert.AreEqual(
				"Translation (T):", EditorPage.GetStageName(),
				"Ошибка: В шапке редактора отсутствует нужная задача.");

			// Выходим из редактора
			EditorClickHomeBtn();

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Отменяем исполнителя
			сancelAssignUser(1);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(
				ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка отсутсвия задачи в редакторе
			Assert.AreEqual(
				"View mode:", EditorPage.GetStageName(),
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
			Assert.IsTrue(
				UserRightsPage.WaitUntilCreateFormDisplay(), 
				"Ошибка: Форма создания группы не открылась.");

			// Вводим имя новой группы
			UserRightsPage.AddGroupName(groupName);

			// Сохраняем имя новой группы
			UserRightsPage.ClickSaveGroup();

			Thread.Sleep(1000);

			// Проверка, что форма создания грцппы закрылась
			Assert.IsTrue(
				UserRightsPage.WaitUntilCreateFormDisappear(), 
				"Ошибка: Форма создания группы не закрылась.");
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
			Assert.IsTrue(
				UserRightsPage.WaitUntilUsersRightsDisplay(), 
				"Ошибка: Страница прав пользователя не открылась.");

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();

			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(
				UserRightsPage.WaitUntilGroupsRightsDisplay(), 
				"Ошибка: Страница прав групп пользователей не открылась.");

			// Получение имени пользователя
			userName = WorkspacePage.GetUserName();

			// Окрытие группы Administrators
			UserRightsPage.ClickGroupByName("Administrators");

			Thread.Sleep(1000);

			// Получение списка пользователей в группе Administrators
			usersInGroup = UserRightsPage.GetDisplayUsersInGroup();

			Assert.IsTrue(
				usersInGroup.Contains(userName), 
				"Ошибка: Текущего пользователя нет в группе Administrators.");

			// Возвращаемся на страницу проектов
			SwitchWorkspaceTab();
		}

		/// <summary>
		/// Проверка, что текущий пользователь присутствует
		/// </summary>
		private void сheckUserPresent(string userName)
		{
			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();

			// Ожидание открытия страницы
			Assert.IsTrue(
				UserRightsPage.WaitUntilUsersRightsDisplay(), 
				"Ошибка: Страница прав пользователя не открылась.");

			// Получение списка пользователей
			List<string> usersList = UserRightsPage.GetUserFullnameList();

			Assert.IsTrue(
				usersList.Contains(userName), 
				"Ошибка: Пользователь " + userName + " отсутствует в списке пользователей.");

			// Возвращаемся на страницу проектов
			SwitchWorkspaceTab();
		}

		/// <summary>
		/// Отмена исполнителя для заданной строки
		/// </summary>
		/// <param name="rowNumber">Номер строки</param>
		private void сancelAssignUser(int rowNumber)
		{
			// Отменяем исполнителя
			ResponsiblesDialog.ClickCancelBtn(rowNumber);

			// Ожидание открытия диалога подтверждения
			Assert.IsTrue(
				ResponsiblesDialog.WaitUntilConfirmDialogDisplay(),
				"Ошибка: Диалог подтверждения не открылся.");

			// Подтверждение
			ResponsiblesDialog.ClickYesBtn();

			// Ожидание закрытия диалога подтверждения
			Assert.IsTrue(
				ResponsiblesDialog.WaitUntilConfirmDialogDissapear(),
				"Ошибка: Диалог подтверждения не закрылся.");
		}

		// Имя проекта, использующегося в нескольких тестах
		// Проект не изменяется при проведении тестов
		private string _projectNoChangesName = string.Empty;
	}
}
