using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace AbbyyLS.CAT.Function.Selenium.Tests.CheckRights
{
	public class CheckCreateProjectRight : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public CheckCreateProjectRight(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Начальная подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void Setup()
		{
			var groupName = "GroupTest" + DateTime.Now.Ticks.ToString();
			var accountName = "TestAccount";
			
			// Переходим к странице воркспейса
			GoToUrl(RelativeUrlProvider.Workspace);

			// Переходим к вкладке "Пользователи и права"
			WorkspacePage.ClickUsersAndRightsBtn();

			// Ожидание открытия страницы
			Assert.IsTrue(
				UserRightsPage.WaitUntilUsersRightsDisplay(),
				"Ошибка: Страница прав пользователя не открылась.");

			// Перейти в подраздел "Группы и права"
			UserRightsPage.OpenGroups();

			//Подождать загрузки страницы
			UserRightsPage.WaitUntilGroupsRightsDisplay();

			// Удалить пользователя из всех групп.
			removeUserFromAllGroups(TestRightsUserName);

			// Создать группу для этого теста.
			createGroupForThisTest(groupName);

			// Добавить права для этой группы(Права:создание проекта)
			addRightsForGroup(groupName);

			// Добавляем пользователя в группу.
			addUserInGroup(TestRightsUserName);

			// Выходим из учётной записи
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			// Дождаться загрузки страницы
			LoginPage.WaitPageLoadLpro();
			
			// Авторизуемся под пользователем, для которого выставляли права
			Authorization(TestRightsLogin, TestRightsPassword, accountName);

			//Временная мера. Перегружаем страницу. (PRX-8283)
			RefreshPage();
		}


		#region Тесты

		/// <summary>
		/// Этот метод реализует тест проверяющий, что мы можем создать проект
		/// </summary>
		[Test]
		public void CheckCreateProject()
		{
			// Создаём новый проект
			createNewProject();
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, что мы можем создать проект
		/// и добавить к нему документ (документ добавляется во время создания проекта)
		/// </summary>
		[Test]
		public void CheckAddDocumentInProject()
		{
			// Создаём новый проект с документом
			createNewProjectWithDocument();

			//Добавляем второй документ после создания
			addDocumentInProject();
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, что ссылка со страницы 
		/// workspace в конкретный проект отсутствует 
		/// </summary>
		[Test]
		public void CheckLinkInProjectNotExist()
		{
			// Создаём новый проект
			createNewProject();

			// Проверка, что ссылка на вход в проект отсутствует. (Название проекта является не ссылкой, а обычным текстом)
			Assert.IsFalse(
				CheckCreateProjectRightHelper.LinkProjectExists(ProjectName),
				"Ошибка: появилась ссылка на проект:" + ProjectName);
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, что мы имеем возможность 
		/// экспортировать из проекта оригинал документа и его перевод
		/// </summary>
		/// <param name="menuItem">пункт в меню Download</param>
		[Test]
		[TestCase("Translation")]
		[TestCase("Original")]
		public void CheckExportDocument(string menuItem)
		{
			// Создаём новый проект с документом
			createNewProjectWithDocument();

			// Добавить 2-ой документ в проект
			addDocumentInProject();

			// Кликаем чекбокс рядом с документом
			CheckCreateProjectRightHelper.CheckBoxDocumentClick(PathProvider.DocumentFile2);
			
			// Кликаем по кнопке Download в открытой свёртке проекта
			CheckCreateProjectRightHelper.DownloadInProjectClick();

			string date = DateTime.Now.ToString("MM/dd/yyyy HH:mm").Replace(".", "/");
			string fileName = CheckCreateProjectRightHelper.GetFileName(PathProvider.DocumentFile2);

			// Выбираем menuItem во всплывающем меню
			CheckCreateProjectRightHelper.ExportClickMenuItemInProject(menuItem);
			
			// Дожидаемся появления плашки, говорящей, что документ готов к загрузке
			Assert.IsTrue(
				TMPage.IsBaloonWithSpecificMessageExist(
					"Document \"" + date + "\" is ready for download. " + fileName),
				"Ошибка: плашка о том, что документ готов к загрузке не появилась.");

			workWithExport(fileName);
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, что мы имеем возможность 
		/// экспортировать из проекта оригиналы документов и их переводы одним архивом
		/// </summary>
		/// <param name="menuItem">пункт в меню Download</param>
		/// <param name="dowloadInProjectClick">Если кликаем Download в меню свёртки проекта - true. Иначе false</param>
		[Test]
		[TestCase("Translation", true)]
		[TestCase("Translation", false)]
		[TestCase("Original", true)]
		[TestCase("Original", false)]
		public void CheckDownloadProject(string menuItem, bool dowloadInProjectClick)
		{
			// Создаём новый проект с документом
			createNewProjectWithDocument();

			// Добавить 2-ой документ в проект
			addDocumentInProject();

			// Кликаем чекбокс рядом с проектом
			CheckCreateProjectRightHelper.CheckBoxProjectClick(ProjectName);

			// Кликаем Download в нужном месте(свёртка или главное меню)
			if (dowloadInProjectClick)
			{
				// Кликаем по кнопке Download в открытой свёртке проекта
				CheckCreateProjectRightHelper.DownloadInProjectClick();

				// Выбираем menuItem во всплывающем меню
				CheckCreateProjectRightHelper.ExportClickMenuItemInProject(menuItem);
			}
			else
			{
				// Кликаем по кнопке Download в главном меню
				CheckCreateProjectRightHelper.DownloadInMenuClick();

				// Выбираем menuItem в главном меню
				CheckCreateProjectRightHelper.ExportClickMenuItemInMenu(menuItem);
			}
			string date = DateTime.Now.ToString("MM/dd/yyyy HH:mm").Replace(".", "/");
			
			// Дожидаемся появления плашки, говорящей, что документ готов к загрузке
			Assert.IsTrue(
				TMPage.IsBaloonWithSpecificMessageExist(
					"Documents are ready for download. " + date),
				"Ошибка: плашка о том, что документ готов к загрузке не появилась.");
			
			// Формируем имя загружаемого архива.
			Logger.Trace(DateTime.UtcNow.ToString("yyyy-MM-dd_HH"));
			var postfix = menuItem == "Original" ? "Source" : "Target";
			workWithExport("Documents_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH_mm") + "*." + postfix + ".zip");
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, что мы имеем возможность удалить проект
		/// </summary>
		/// <param name="closeProject">Свёртка закрыта - true, иначе false</param>
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void CheckDeleteProject(bool closeProject)
		{
			// Создаём новый проект с документом
			createNewProjectWithDocument();

			// Кликаем чекбокс рядом с проектом
			CheckCreateProjectRightHelper.CheckBoxProjectClick(ProjectName);
			
			if (closeProject)
			{
				// Во время создания проекта свёртка открылась
				// Вызов этого метода повторно закрывает её.
				WorkspacePage.OpenProjectInfo(ProjectName);

				// Нажать Удалить
				WorkspacePage.ClickDeleteProjectBtn();

				// Подтвердить
				WorkspacePage.ClickConfirmDelete();

				// Дождаться, пока пропадет диалог подтверждения удаления
				WorkspacePage.WaitUntilDeleteConfirmFormDisappear();
			}
			else
			{
				// Нажать Удалить
				WorkspacePage.ClickDeleteProjectBtn();

				// Дождаться появления диалога удаления
				Assert.IsTrue(CheckCreateProjectRightHelper.WaitDeleteProjectOrFilesDisplayDialog(),
					"Ошибка: не появилась форма подтверждения удаления проекта");

				//Подтвердить,что удаляем проект целиком
				CheckCreateProjectRightHelper.ClickConfirmDeleteProjectOrFiles(
					CheckCreateProjectRightHelper.DELETE_MODE.Project);

				// Дождаться, пока пропадет диалог подтверждения удаления
				Assert.IsTrue(CheckCreateProjectRightHelper.WaitDeleteProjectOrFilesDisappearDialog(),
					"Ошибка: не скрылась форма подтверждения удаления проекта");
			}
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, что мы имеем возможность удалить файлы
		/// Все файлы удаляются через главное меню. Одни файл через свёртку проекта.
		/// </summary>
		/// <param name="allFiles">Все файлы - true, иначе false</param>
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void CheckDeleteDocument(bool allFiles)
		{
			// Создаём новый проект с документом
			createNewProjectWithDocument();

			// Добавить 2-ой документ в проект
			addDocumentInProject();

			if (allFiles)
			{
				// Кликаем чекбокс рядом с первый документом
				CheckCreateProjectRightHelper.CheckBoxDocumentClick(PathProvider.DocumentFile);

				// Кликаем чекбокс рядом со вторым документом документом
				CheckCreateProjectRightHelper.CheckBoxDocumentClick(PathProvider.DocumentFile2);

				// Нажать Удалить
				WorkspacePage.ClickDeleteProjectBtn();

				// Дождаться появления диалога удаления
				Assert.IsTrue(CheckCreateProjectRightHelper.WaitDeleteProjectOrFilesDisplayDialog(),
					"Ошибка: не появилась форма подтверждения удаления проекта");

				//Подтвердить,что удаляем проект целиком
				CheckCreateProjectRightHelper.ClickConfirmDeleteProjectOrFiles(
					CheckCreateProjectRightHelper.DELETE_MODE.Files);

				// Дождаться, пока пропадет диалог подтверждения удаления
				Assert.IsTrue(CheckCreateProjectRightHelper.WaitDeleteProjectOrFilesDisappearDialog(),
					"Ошибка: не скрылась форма подтверждения удаления проекта");
			}
			else
			{
				// Кликаем чекбокс рядом с документом
				CheckCreateProjectRightHelper.CheckBoxDocumentClick(PathProvider.DocumentFile2);

				// Нажать Удалить
				CheckCreateProjectRightHelper.ClickDeleteButtonInProject();

				// Подтвердить
				WorkspacePage.ClickConfirmDelete();

				// Дождаться, пока пропадет диалог подтверждения удаления
				WorkspacePage.WaitUntilDeleteConfirmFormDisappear();
			}
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, что мы не имеем доступа к коннектору
		/// (Это выражается в отсутствиее кнопки "Sign In to Connector")
		/// </summary>
		[Test]
		public void CheckConnectorButtonNotExist()
		{
			// Проверяем, что кнопки "Sign In to Connector" нет
			Assert.IsFalse(
				CheckCreateProjectRightHelper.GetIsNotExistConnectorButton(),
				"Ошибка: кнопка \"Sign In to Connector\" не должна быть видна.");
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, что с правами 
		/// на создание проектов можно открыть свёртку проекта
		/// </summary>
		[Test]
		public void CheckOpenProject()
		{
			// Создали проект(при создании свёртка открывается)
			createNewProjectWithDocument();

			// Проверяем, что свёртка действительно открыта
			Assert.IsTrue(CheckCreateProjectRightHelper.CheckOpenProject(ProjectName),
				"Ошибка: свёртка не открыта для проекта:" + ProjectName);
		}
		
		/// <summary>
		/// Этот метод реализует тест проверяющий, что кнопка поиска ошибок отображается
		/// </summary>
		[Test]
		public void QACheckButtonExist()
		{
			// Создали проект(при создании свёртка открывается)
			createNewProjectWithDocument();

			// Проверяем, что кнопка поиска ошибок отображается
			Assert.IsTrue(CheckCreateProjectRightHelper.QACheckButtonExist(),
				"Ошибка: кнопка поиска ошибок не отображается");
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, форма настроек открывается
		/// </summary>
		[Test]
		public void CheckSettingsFormExist()
		{
			// Создали проект(при создании свёртка открывается)
			createNewProjectWithDocument();

			// Кликаем по кнопке настроек
			CheckCreateProjectRightHelper.ClickProjectSettings();

			// Проверяем, что окно настроек открылось
			Assert.IsTrue(CheckCreateProjectRightHelper.SettingsFormExist(),
				"Ошибка: окно настроек не открылось");
		}

		/// <summary>
		/// Этот метод реализует тест проверяющий, форма настроек открывается
		/// </summary>
		[Test]
		public void CheckAnalysisFormExist()
		{
			// Создали проект(при создании свёртка открывается)
			createNewProjectWithDocument();

			// Кликаем по кнопке настроек
			CheckCreateProjectRightHelper.ClickProjectAnalysisButton();

			// Проверяем, что окно настроек открылось
			Assert.IsTrue(CheckCreateProjectRightHelper.AnalysisFormExist(),
				"Ошибка: окно анализа не открылось");
		}

		#endregion
		
		#region Вспомогательные методы

		/// <summary>
		/// Метод создаёт группу для этого теста
		/// </summary>
		/// <param name="userName">userName пользователя</param>
		private void createGroupForThisTest(string groupName)
		{
			// Кликаем кнопку создания новой группы
			UserRightsPage.ClickCreateGroup();
			
			// Ожидаем открытия формы создания новой группы
			Assert.IsTrue(UserRightsPage.WaitUntilCreateFormDisplay(),
				"Ошибка: форма создания новой группы не открылась.");

			// Вписываем имя новой группы
			UserRightsPage.AddGroupName(groupName);

			// Сохраняем группы
			UserRightsPage.ClickSaveGroup();

			// Проверяем, что форма создания группы закрылась после нажатия кнопки сохранения группы
			Assert.IsTrue(UserRightsPage.WaitUntilCreateFormDisappear(),
				"Ошибка: форма создания новой группы не закрылась.");
		}
		
		/// <summary>
		/// Метод добавляет нужные права для группы с именем groupName(права группы:создание проектов)
		/// </summary>
		/// <param name="groupName">имя группы</param>
		private void addRightsForGroup(string groupName)
		{
			// Кликаем на нужную группу
			UserRightsPage.ClickGroupByName(groupName);

			// Добавляем нужные для этого теста права
			addRightsCreateProjects();
		}

		/// <summary>
		/// Метод создаёт новый проект
		/// </summary>
		/// 
		private void createNewProject()
		{
			// Создаём проект
			CreateProject(ProjectName);

			// Проверка, что проект с именем ProjectName есть на странице
			Assert.IsTrue(
				GetIsExistProject(ProjectName),
				"Ошибка: проекта " + ProjectName + " нет в списке");
		}

		/// <summary>
		/// Метод создаёт новый проект и во время создания добавляет документ
		/// </summary>
		private void createNewProjectWithDocument()
		{
			// Создаём проект с документом
			CreateProject(ProjectName, PathProvider.DocumentFile);

			// Проверка, что проект с именем ProjectName есть на странице
			Assert.IsTrue(
				GetIsExistProject(ProjectName),
				"Ошибка: проекта " + ProjectName + " нет в списке.");

			// Открытие свёртки проекта
			WorkspacePage.OpenProjectInfo(ProjectName);

			// Проверка, что файл добавлен в проект
			Assert.IsTrue(CheckCreateProjectRightHelper.GetIsFileInProjectExist(PathProvider.DocumentFile),
				"Ошибка: файл в проекте не обнаружен.");
		}

		/// <summary>
		/// Метод добавляет в проект новый документ через свёртку
		/// (Считается,что на данном этапе свёртку уже раскрыта)
		/// </summary>
		private void addDocumentInProject() 
		{
			CheckCreateProjectRightHelper.AddDocumentClick();
			ProjectPage.WaitImportDialogDisplay();
			// Заполнить диалог загрузки
			CheckCreateProjectRightHelper.UploadFileOnProjectPage(PathProvider.DocumentFile2);

			// Нажать Finish
			ProjectPage.ClickFinishImportDialog();

			// Дождаться окончания загрузки
			ProjectPage.WaitDocumentDownloadFinish();

			// Проверка, что файл добавлен в проект
			Assert.IsTrue(CheckCreateProjectRightHelper.GetIsFileInProjectExist(PathProvider.DocumentFile2),
				"Ошибка: файл в проекте не обнаружен.");
		}

		/// <summary>
		/// Экспортировать после появления сообщения Download
		/// </summary>
		/// <param name="fileName">базовая часть имени экспортируемого файла</param>
		private void workWithExport(string fileMask)
		{
	
			// Нажать Download
			WorkspacePage.ClickDownloadNotifier();

			string clickTime = DateTime.Now.ToString("MM.dd.yyyy HH:mm:ss");
			
			Logger.Trace("Время клика по кнопке Download = " + clickTime);

			string[] files = GetDownloadFiles(fileMask, 6, PathProvider.ResultsFolderPath);

			Assert.IsTrue(files.Length > 0,
				"Ошибка: файл не загрузился за отведённое время (6 секунд)");

			var directoryInfo = Directory.CreateDirectory(PathProvider.ResultsFolderPath + "\\CreateRightsTest" + DateTime.Now.Ticks);

			DateTime lastChanged = File.GetLastWriteTime(files[0]);
			Logger.Trace("Время последнего изменения файла = " + lastChanged);

			var pathToMove = PathProvider.ResultsFolderPath + "\\" + directoryInfo + "\\" + Path.GetFileNameWithoutExtension(files[0]) + DateTime.Now.Ticks + Path.GetExtension(files[0]);

			Logger.Trace(pathToMove);
			File.Move(files[0], pathToMove);
		}

		/// <summary>
		/// Метод удаляет пользователя из всех групп
		/// </summary>
		private void removeUserFromAllGroups(string userName)
		{
			// Получаем кол-во групп.
			var groupsCount = UserRightsPage.GetGroupsCount();
			Logger.Info("Групп найдено:" + groupsCount);
			for (int i = 0; i < groupsCount; i++)
			{

				// Открываем группу для того, чтобы начать редактирование
				UserRightsPage.GetGroupsCount(i + 1);

				// Проверяем есть ли пользователь в группе
				if (UserRightsPage.CheckUserExistsInGroup(userName))
				{
					// Если пользователь обнаружен в группе, удаляем его из неё
					removeExistingUserFromOneGroup(userName);
				}

				// Ожидаем появления кнопки Edit.
				// Её появление означает, что редактирование группы закончено
				// Если не дождаться, закрыть группу не получится
				UserRightsPage.WaitUntilDisplayEdit();

				// Закрываем группу после редактирования
				UserRightsPage.GetGroupsCount(i + 1);
			}
		}

		/// <summary>
		/// Метод удаляет пользователя из открытой на редактирование группы.
		/// </summary>
		private void removeExistingUserFromOneGroup(string userName)
		{
			// Кликаем на кнопку редактирования группы
			UserRightsPage.EditGroupClick();

			// Получаем список всех пользователей в группе
			var usersInGroup = UserRightsPage.GetUserInGroupList();

			// Находим номер нашего пользователя
			int number = 0;
			for (int i = 0; i < usersInGroup.Count; i++)
			{
				if (usersInGroup[i].GetAttribute("innerHTML") == userName)
				{
					number = i + 1;
					break;
				}
			}
			if (number == 0)
			{
				Assert.Fail("Ошбика: пользователь с именем \"" + userName + "\" не найден в открытой на данный момент группе.");
			}
			// Кликаем на кнопку удаления нужного пользователя 
			UserRightsPage.DeleteUserFromGroupClick(number);

			// Кликаем на кнопку сохранения группы
			UserRightsPage.SaveGroupClick();
		}

		/// <summary>
		/// Метод добавляет право на создание проектов для редактируемой группы.
		/// </summary>
		private void addRightsCreateProjects()
		{
			// Кликаем по кнопке редактирования группы
			UserRightsPage.EditGroupClick();

			// Кликаем по кнопке добавления прав для группы
			UserRightsPage.AddAccessRigthButtonClick();

			// Кликаем по radio button с правом на создание проектов 
			UserRightsPage.RightCreateProjectClick();

			// Кликаем кнопку Next, чтобы перейти к следующему шагу назначения прав
			UserRightsPage.AddRightNextClick();

			// Кликаем по radio button с дающий выбранные права для всех проектов
			UserRightsPage.ForAnyRpojectsClick();

			// Кликаем кнопку добавления прав (Кнопка Add  - завершает диалог)
			UserRightsPage.AddRightDialogClick();
		}

		/// <summary>
		/// Метод добавляет пользователя в редактируемую группу.
		/// </summary>
		/// <param name="userName">имя пользователя</param>
		private void addUserInGroup(string userName)
		{
			// Кликаем по полю поиска пользователей
			UserRightsPage.FindUsersClick();

			// Вводим имя пользователя в поле поиска пользователей для добавления в группу
			UserRightsPage.FindUsersSendText(userName);

			// Во всплывающем списке пользователей выбираем нужного
			UserRightsPage.AddUserInGroupClick(userName);

			// Кликаем кнопку сохранения группы
			UserRightsPage.SaveGroupClick();
		}

		#endregion
	}
}
