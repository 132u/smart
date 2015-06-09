using System;
using System.Collections.Generic;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки назначения пользователя на задачу
	/// </summary>
	[Category("Standalone")]
	class AssignResponsiblesTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
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
				_projectNoChangesName = ProjectUniqueName;
			}
			catch (Exception ex)
			{
				ExitDriver();
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
			// Проверка кол-во открытых окон браузера
			if (Driver.WindowHandles.Count > 1)
			{
				Driver.SwitchTo().Window(Driver.WindowHandles[1]).Close();
				Driver.SwitchTo().Window(Driver.WindowHandles[0]);
			}

			// Переход на страницу workspace
			GoToUrl(RelativeUrlProvider.Workspace);
			WorkspacePage.CloseTour();
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при выборе документа и нажатии по кнопке Assign Task в панели
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceOnAssignTaskBtn()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, PathProvider.EditorTxtFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(_projectNoChangesName);

			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(1);

			// Ожидание открытия диалога выбора исполнителя
			ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay();
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии кнопки открытия прав в Workflow
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceOnAssignBtn()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, PathProvider.EditorTxtFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(_projectNoChangesName);
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при загрузке документа в проект
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceUploadDocument()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, PathProvider.EditorTxtFile);

			//Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(_projectNoChangesName);

			//Открываем диалог загрузки документа
			WorkspacePage.ClickDocumentUploadBtn();

			//Ожидаем пока загрузится диалог
			Assert.IsTrue(ProjectPage.WaitImportDialogDisplay(), "Ошибка: диалог импорта документа не открылся");

			// Добавляем документ
			WorkspacePage.UploadFileInProjectSettings(PathProvider.DocumentFileToConfirm1);

			ProjectPage.ClickNextImportDialog();
			ProjectPage.ClickNextImportDialog();

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilMasterResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при выборе документа и клике по кнопке Assign Task
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnProgressLink()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, PathProvider.EditorTxtFile);
			
			// Открываем проект
			OpenProjectPage(_projectNoChangesName);

			// Открываем окно прав исполнителей
			ProjectPage.ClickDocumentProgress();

			Assert.IsTrue(ProjectPage.GetIsPanelDisplay(), "Ошибка: панель с кнопками (Assign Tasks, Delete, Download, Settings) не появилась");

			// Клик по кнопке Assign Tasks
			ProjectPage.ClickAssignRessponsibleBtn();

			// Ожидание открытия диалога выбора исполнителя
			ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay();
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии кнопки открытия прав в окне проекта
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnAssignBtn()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, PathProvider.EditorTxtFile);

			// Открываем проект
			OpenProjectPage(_projectNoChangesName);

			// Открываем инфо документа 
			ProjectPage.OpenDocumentInfo(1);

			// Открываем окно прав исполнителей
			ProjectPage.ClickAssignRessponsibleBtn();
			
			// Ожидание открытия диалога выбора исполнителя
			ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay();
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии кнопки Assign Tasks
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnAssignTaskBtn()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, PathProvider.EditorTxtFile);
			
			// Открываем проект
			OpenProjectPage(_projectNoChangesName);

			// Открываем инфо документа 
			ProjectPage.SelectDocument(1);

			// Открываем окно прав исполнителей
			ProjectPage.ClickProgressBtn();
			Assert.IsTrue(ProjectPage.GetIsPanelDisplay(), "Ошибка: панель с кнопками (Assign Tasks, Delete, Download, Settings) не появилась");
			// Клик по кнопке Assign Tasks
			ProjectPage.ClickAssignRessponsibleBtn();
			// Ожидание открытия диалога выбора исполнителя
			ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay();
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при загрузке документа в окне проекта
		/// </summary>
		[Test]
		public void ResponsiblesProjectUploadDocument()
		{
			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, PathProvider.EditorTxtFile);
			
			// Открываем проект
			OpenProjectPage(_projectNoChangesName);

			//// Открываем диалог загрузки документа
			ProjectPage.ClickImportBtn();

			//// Ожидаем пока загрузится диалог
			Assert.IsTrue(ProjectPage.WaitImportDialogDisplay(), "Ошибка: диалог импорта документа не открылся");

			// Добавляем документ
			ProjectPage.UploadFileOnProjectPage(PathProvider.DocumentFileToConfirm1);

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
			UserRightsPage.AssertionUsersRightsPageDisplayed();

			// Получаем список 
			var usersList = UserRightsPage.GetUserFullnameList();

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();

			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(
				UserRightsPage.WaitUntilGroupsRightsDisplay(), 
				"Ошибка: Страница прав групп пользователей не открылась.");

			// Получаем список 
			var groupsList = UserRightsPage.GetGroupNameList();

			// Добавляем в начало каждой строки слово Group как в списке диалога
			for (var i= 0; i < groupsList.Count; i++)
			{
				groupsList[i] = "Group: " + groupsList[i];
			}

			// Перейти на страницу проектов
			SwitchWorkspaceTab();

			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, PathProvider.EditorTxtFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(_projectNoChangesName);

			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(1);

			Thread.Sleep(1000);

			// Получаем список из выпадающего списка исполнителей
			var responsibleUsersList = ResponsiblesDialog.GetResponsibleUsersList();

			foreach (var item in responsibleUsersList)
			{
				Assert.IsTrue(
					usersList.Contains(item),
					"Ошибка: Элемент выпадающего списка " + item + " отсутствует в списке пользователей.");

				usersList.Remove(item);
			}

			// Получаем список из выпадающего списка групп исполнителей
			var responsibleGroupList = ResponsiblesDialog.GetResponsibleGroupsList();

			foreach (var item in responsibleGroupList)
			{
				Assert.IsTrue(
					groupsList.Contains(item),
					"Ошибка: Элемент выпадающего списка " + item + " отсутствует в списке групп пользователей.");
				
				groupsList.Remove(item);
			}

			Assert.IsTrue(
				groupsList.Count == 0,
				"Ошибка: В выпадающем списке отсутсвуют " + groupsList.Count + " элемента списка групп пользователей.");

			Assert.IsTrue(
				usersList.Count == 0,
				"Ошибка: В выпадающем списке отсутсвуют " + usersList.Count + " элемента списка пользователей.");
		}

		/// <summary>
		/// Проверка отображения в выпадающем списке исполнителей новой группы пользователей
		/// </summary>
		[Test]
		public void AddNewGroup()
		{
			var groupName = "GroupTest" + DateTime.Now.Ticks.ToString();
			var isPresent = false;

			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();

			// Ожидание открытия страницы
			UserRightsPage.AssertionUsersRightsPageDisplayed();

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();

			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(UserRightsPage.WaitUntilGroupsRightsDisplay(), "Ошибка: Страница прав групп пользователей не открылась.");

			// Добавляем новую группу
			CreateNewGroup(groupName);

			// Перейти на страницу проектов
			SwitchWorkspaceTab();

			// Создание проекта
			CreateProjectIfNotCreated(_projectNoChangesName, PathProvider.EditorTxtFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(_projectNoChangesName);

			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(1);

			Thread.Sleep(1000);

			// Получаем список из выпадающего списка исполнителей
			var responsibleGroupList = ResponsiblesDialog.GetResponsibleGroupsList();

			foreach (var group in responsibleGroupList)
			{
				if (group == "Group: " + groupName)
				{
					isPresent = true;
				}
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
			CreateProjectIfNotCreated(ProjectUniqueName, PathProvider.EditorTxtFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectUniqueName);

			// Выбор в качестве исполнителя для первой задачи группы Administrator
			SetResponsible(1, NickName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectUniqueName);

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
		[Test]
		public void AssignUserFewTasks()
		{
			CreateProject(ProjectUniqueName, PathProvider.EditorTxtFile, false, "", Workspace_CreateProjectDialogHelper.SetGlossary.None, "", false, Workspace_CreateProjectDialogHelper.MT_TYPE.None, true, 1, 1, 0);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectUniqueName);

			// Выбор в качестве исполнителя для первой задачи группы Administrator
			SetResponsible(1, NickName, false);

			// Выбор в качестве исполнителя для второй задачи группы Administrator
			SetResponsible(2, NickName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectUniqueName);

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
			CreateProject(ProjectUniqueName, PathProvider.EditorTxtFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectUniqueName);

			// Выбор в качестве исполнителя для первой задачи группы Administrator
			SetResponsible(1, NickName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectUniqueName);

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
		[Test]
		public void AssignDifferentUsersOneTask()
		{
			// После теста закрываем браузер, т.к. залогиниваемся под альтернативным пользователем
			QuitDriverAfterTest = true;
			
			// Проверка второго пользователя
			сheckUserPresent(NickName2);

			// Создание проекта
			CreateProjectIfNotCreated(ProjectUniqueName, PathProvider.EditorTxtFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectUniqueName);

			// Выбор в качестве исполнителя для первой задачи первого юзера
			SetResponsible(1, NickName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectUniqueName);

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
			GoToUrl(RelativeUrlProvider.Workspace);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectUniqueName);

			// Отменяем исполнителя
			сancelAssignUser(1);

			// Выбор в качестве исполнителя для первой задачи второго юзера
			SetResponsible(1, NickName2, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectUniqueName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка отсутсвия задачи в редакторе
			Assert.AreEqual("", EditorPage.GetStageName(),
				"Ошибка: В режиме менеджера в панели инструментов отображается название этапа");

			// Выходим из редактора
			EditorClickHomeBtn();

			// Разлогиниться
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
			Thread.Sleep(1000);// Sleep не убирать, в Chrome не успевает открыться страница Login
			Authorization(Login2, Password2);

			// Открытие страницы проекта
			OpenProjectPage(ProjectUniqueName);

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
		/// Назначение/сброс пользователя на задачу
		/// </summary>
		[Test]
		public void UnAssignUser()
		{
			QuitDriverAfterTest = true;
			// Создание проекта
			CreateProjectIfNotCreated(ProjectUniqueName, PathProvider.EditorTxtFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectUniqueName);

			// Выбор в качестве исполнителя для первой задачи первого юзера
			SetResponsible(1, NickName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(
				ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectUniqueName);

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

			//Переход на страницу WS 
			GoToUrl(RelativeUrlProvider.Workspace);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectUniqueName);

			// Отменяем исполнителя
			сancelAssignUser(1);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Ожидание закрытия диалога выбора исполнителя
			Assert.IsTrue(
				ResponsiblesDialog.WaitUntilResponsiblesDialogDissapear(),
				"Ошибка: Диалог выбора исполнителя не закрылся.");

			// Открытие страницы проекта
			OpenProjectPage(ProjectUniqueName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Проверка отсутсвия задачи в редакторе
			Assert.AreEqual(
				"", EditorPage.GetStageName(),
				"Ошибка: В режиме менеджера в панели инструментов отображается название этапа");
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
		/// Проверка, что текущий пользователь присутствует
		/// </summary>
		private void сheckUserPresent(string userName)
		{
			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();

			UserRightsPage.AssertionUsersRightsPageDisplayed();

			// Получение списка пользователей
			var usersList = UserRightsPage.GetUserFullnameList();

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
