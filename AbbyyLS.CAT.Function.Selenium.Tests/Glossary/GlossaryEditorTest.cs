using System;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	[TestFixture]
	[Category("Standalone")]
	class GlossaryEditorTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[TestFixtureSetUp]
		public void BeforeClass()
		{
			try
			{
				CreateUniqueNamesByDatetime();

				// Запись имени для дальнейшего использования в группе тестов
				projectNoChangesName = ProjectUniqueName;

				checkAddUserRights();
				createNewGlossary();
				
				CreateProjectIfNotCreated(
					projectNoChangesName,
					setGlossary: Workspace_CreateProjectDialogHelper.SetGlossary.ByName,
					glossaryName: _glossaryName);

				ImportDocumentProjectSettings(PathProvider.DocumentFile, projectNoChangesName);

				AssignTask(1);
			}
			catch (Exception ex)
			{
				ExitDriver();
				Logger.ErrorException("Ошибка в конструкторе : " + ex.Message, ex);
				throw;
			}
		}

		[SetUp]
		public void Setup()
		{
			QuitDriverAfterTest = false;

			GoToUrl(RelativeUrlProvider.Workspace);
			
			WorkspacePage.OpenProjectPage(projectNoChangesName);
			OpenDocument();
		}

		[TestFixtureTearDown]
		public override void TeardownAllBase()
		{
			GoToUrl(RelativeUrlProvider.Glossaries);

			// Зайти в глоссарий
			SwitchCurrentGlossary(_glossaryName);

			// Удалить глоссарий
			DeleteGlossary();

			// Удалить второй глоссарий
			if (_glossaryName2 != null && GlossaryListPage.GetIsExistGlossary(_glossaryName2))
			{
				SwitchCurrentGlossary(_glossaryName2);
				// Удалить глоссарий
				DeleteGlossary();
			}

			ExitDriver();
		}
		
		/// <summary>
		/// Открывает форму добавления термина в редакторе по нажатию кнопки на панели
		/// </summary>
		[Test]
		public void OpenAddTermFormBtn()
		{
			// Открытие формы
			openAddTermForm();
			
		}

		/// <summary>
		/// Открывает форму добавления термина в редакторе по хоткею
		/// </summary>
		[Test]
		[Category("SCAT_102")]
		public void OpenAddTermFormHotKey()
		{
			// Нажать хоткей вызова формы для добавления термина
			EditorPage.AddTermFormByHotkey(1);

			// Проверка, что открылась форма
			Assert.IsTrue(
				EditorPage.WaitAddTermFormDisplay(),
				"Ошибка: Форма для добавления термина не открылась.");
		}

		/// <summary>
		/// Проверка автозапослнения формы при выделенном слове в сорсе
		/// </summary>
		[Test]
		public void AutofillAddTermFormSourceWordSelected()
		{
			//Автозаполнение формы
			autofillFormSourceWordSelected();
		}

		/// <summary>
		/// Проверка автозапослнения формы при выделенном слове в тагрет
		/// </summary>
		[Test]
		public void TargetWordSelectedAutofillAddTermForm()
		{
			autofillFormSourceWordSelected();
			
			// Удаляем текст из таргета и подтверждаем 
			// (чтобы не всплывало PopUp окно, AutoSave иногда очень тормозит)
			AddTermForm.ClickCancelBtn();
			EditorPage.ClearTarget(1);
			AutoSave();
			Thread.Sleep(7000);
		}

		/// <summary>
		/// Добавить одиночный термин из сорса в глоссарий
		/// </summary>
		[Test]
		public void AddSingleSourceTermToGlossary()
		{
			// Автозаполнение формы
			autofillFormSourceWordSelected();
			// Нажать сохранить
			AddTermForm.ClickAddBtn();

			Assert.IsTrue(
				AddTermForm.WaitConfirmSingleTermMessage(),
				"Ошибка: Не было сообщения о добавлении одиночного термина.");

			// Добавить термин
			AddTermForm.ClickAddSingleTerm();
			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();

			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			Assert.IsTrue(
				GlossaryPage.GetIsSingleSourceTermExists("Earth"),
				"Ошибка: Не добавлен одиночный термин из сорса.");
		}

		/// <summary>
		/// Добавить одиночный термин из таргета в глоссарий
		/// </summary>
		[Test]
		public void AddSingleTargetTermToGlossary()
		{
			// Автозаполнение формы
			autofillFormSourceWordSelected();
			
			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(
				AddTermForm.WaitConfirmSingleTermMessage(),
				"Ошибка: Не было сообщения о добавлении одиночного термина.");

			// Добавить термин
			AddTermForm.ClickAddSingleTerm();

			// Если появилось сообщение 'Do you want to add the term anyway?', кликнуть Yes
			if (AddTermForm.WaitAnyWayTermMessage())
				AddTermForm.CliCkYesBtnInAnyWayTermMessage();

			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();

			// Удаляем текст из таргета и подтверждаем (чтобы не всплывало PopUp окно, AutoSave иногда очень тормозит)
			EditorPage.ClearTarget(1);
			Thread.Sleep(500);
			EditorPage.ClickConfirmBtn();
			Thread.Sleep(7000);

			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			GlossaryPage.AssertionIsSingleTargetTermExists("Earth");
		}

		/// <summary>
		/// Добавить термин с сорсом и таргетом в глоссарий
		/// </summary>
		[Test]
		public void AddSourceTargetTermToGlossary()
		{
			openAddTermForm();

			// Добавить сорс
			AddTermForm.TypeSourceTermText("Comet");
			// Добавить термин в таргет
			AddTermForm.TypeTargetTermText("Комета");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");	
	 
			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();

			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Comet", "Комета"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить измененный термин из сорса с таргетом в глоссарий
		/// </summary>
		[Test]
		public void AddModifiedSourceTargetTermToGlossary()
		{
			// Автозаполнение формы
			autofillFormSourceWordSelected();
			// Изменить текст сорса
			AddTermForm.TypeSourceTermText("Mars");
			// Добавить термин в таргет
			AddTermForm.TypeTargetTermText("Марс");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();			

			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			Assert.IsTrue(
				GlossaryPage.GetIsSourceTargetTermExists("Mars", "Марс"), 
				"Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить измененный термин из таргета в глоссарий
		/// </summary>
		[Test]
		public void AddSourceModifiedTargetTermToGlossary()
		{
			// Автозаполнение формы
			autofillFormSourceWordSelected();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("Uran");
			// Изменить термин таргета
			AddTermForm.TypeTargetTermText("Уран");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
			// Удаляем текст из таргета и подтверждаем 
			// (чтобы не всплывало PopUp окно, AutoSave иногда очень тормозит)
			EditorPage.ClearTarget(1);
			EditorPage.ClickConfirmBtn();
			AutoSave();


			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			Assert.IsTrue(
				GlossaryPage.GetIsSourceTargetTermExists("Uran", "Уран"), 
				"Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий термин уже существующий в сорс 
		/// </summary>
		[Test]
		public void AddExistedSourceTermToGlossary()
		{
			// Открытие формы
			openAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("planet");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("планета");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
			// Открытие формы
			openAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("planet");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("планетка");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitContainsTermMessage(),
				"Ошибка: Не было сообщения о повторном добавлении термина.");

			// Добавляем термин повтороно
			AddTermForm.ClickContainsTermYes();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();

			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("planet", "планетка"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий термин уже существующий в таргет
		/// </summary>
		[Test]
		public void AddExistedTargetTermToGlossary()
		{
			// Открытие формы
			openAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("asteroid");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("астероид");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
			// Открытие формы
			openAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("the Asteroid");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("астероид");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitContainsTermMessage(),
				"Ошибка: Не было сообщения о повторном добавлении термина.");

			// Добавляем термин повтороно
			AddTermForm.ClickContainsTermYes();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("the Asteroid", "астероид"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий абсолютно идентичный термин
		/// </summary>
		[Test]
		public void AddExistedTermToGlossary()
		{
			// Открытие формы
			openAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("Sun");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("солнце");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
			// Открытие формы
			openAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("Sun");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("солнце");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitContainsTermMessage(),
				"Ошибка: Не было сообщения о повторном добавлении термина.");

			// Добавляем термин повтороно
			AddTermForm.ClickContainsTermYes();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();

			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetAreTwoEqualTermsExist("Sun", "солнце"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий комментарий
		/// </summary>
		[Test]
		public void AddCommentToGlossary()
		{
			// Открытие формы
			openAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("Neptun");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("Нептун");
			// Добавить комментарий
			AddTermForm.TypeCommentText("Generated By Selenium");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();		   

			// Открыть глоссарий и проверить, добавлен ли комментарий
			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsCommentExists("Generated By Selenium"), "Ошибка: Не добавлен комментарий.");
		}

		/// <summary>
		/// Проверка выпадающего списка с глоссариями, при создании проекта подключалось два глоссария
		/// </summary>
		[Test]
		public void CheckGlossaryListInProjectCreatedWithTwoGlossaries()
		{
			// Нажать кнопку Назад
			EditorPage.ClickHomeBtn();
			// Создать проект с двумя глоссариями
			createProjectWithTwoGlossaries();
			//Открытие настроек проекта			
			ImportDocumentProjectSettings(PathProvider.DocumentFile, _projectName2);
			//Назначение задачи на пользователя
			AssignTask(1);
			//Открытие документа
			OpenDocument();
			// Открытие формы
			openAddTermForm();

			// Посмотреть выпадающий список
			AddTermForm.OpenGlossaryList();
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName),
				"Ошибка: Словарь" + _glossaryName + "отсуствует в выпадающем списке.");
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName2),
				"Ошибка: Словарь" + _glossaryName2 + "отсуствует в выпадающем списке.");

		}

		/// <summary>
		/// Проверка выпадающего списка с глоссариями, проект с двумя глоссариями, второй глоссарий подключается в настройках проекта
		/// </summary>
		[Test]
		public void CheckGlossaryListInProjectCreatedWithOneGlossary()
		{
			_projectName2 = ProjectUniqueName + "2";

			// Нажать кнопку Назад
			EditorPage.ClickHomeBtn();
			// Создать новый глоссарий
			createNewGlossary(false);
			// Создать проект с одним глоссарием
			CreateProject(_projectName2, "", false, "", Workspace_CreateProjectDialogHelper.SetGlossary.ByName, _glossaryName);
			//Открытие настроек проекта			
			ImportDocumentProjectSettings(PathProvider.DocumentFile, _projectName2);
			//Назначение задачи на пользователя
			AssignTask(1);
			// Добавляем второй глоссарий
			ProjectPage.SetGlossaryByName(_glossaryName2);
			//Открытие документа
			OpenDocument();
			// Открытие формы
			openAddTermForm();

			// Посмотреть выпадающий список
			AddTermForm.OpenGlossaryList();
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName),
				"Ошибка: Словарь" + _glossaryName + "отсуствует в выпадающем списке.");
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName2),
				"Ошибка: Словарь" + _glossaryName2 + "отсуствует в выпадающем списке.");

		}

		/// <summary>
		/// Добавление одинаковых терминов в разные глоссарии
		/// </summary>
		[Test]
		public void AddEqualTermsInTwoGlossaries()
		{
			// Нажать кнопку Назад
			EditorPage.ClickHomeBtn();
			// Создать проект с двумя глоссариями
			createProjectWithTwoGlossaries();
			//Открытие настроек проекта			
			ImportDocumentProjectSettings(PathProvider.DocumentFile, _projectName2);
			//Назначение задачи на пользователя
			AssignTask(1);
			//Открытие документа
			OpenDocument();
			// Открытие формы
			openAddTermForm();

			// Посмотреть выпадающий список и выбрать словарь
			AddTermForm.OpenGlossaryList();
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName),
				"Ошибка: Словарь" + _glossaryName + "отсуствует в выпадающем списке.");
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName2),
				"Ошибка: Словарь" + _glossaryName2 + "отсуствует в выпадающем списке.");

			AddTermForm.SelectGlossaryByName(_glossaryName);

			// Добавить сорс
			AddTermForm.TypeSourceTermText("Space");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("Космос");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
			// Открытие формы
			openAddTermForm();
			// Выбрать второй глоссарий
			AddTermForm.OpenGlossaryList();
			AddTermForm.SelectGlossaryByName(_glossaryName2);
			// Добавить сорс
			AddTermForm.TypeSourceTermText("Space");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("Космос");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
		}

		/// <summary>
		/// Добавление удаленного термина в глоссарий
		/// </summary>
		[Test]
		public void DeleteAddTermToGlossary()
		{
			// Открытие формы
			openAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("Galaxy");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("Галактика");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();			
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();

			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Galaxy", "Галактика"), "Ошибка: Не добавлен термин.");

			// Удаляем заданный термин
			deleteTermByName("Galaxy", "Галактика");
			SwitchWorkspaceTab();
			// Открытие проекта
			WorkspacePage.OpenProjectPage(projectNoChangesName);
			// Открытие документа
			OpenDocument();
			Thread.Sleep(500);
			// Открытие формы
			openAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("Galaxy");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("Галактика");

			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();

			// Открыть глоссарий и проверить, есть ли термин
			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Galaxy", "Галактика"), "Ошибка: Не добавлен повторно термин.");
		}

		private string _glossaryName;
		private string _glossaryName2;
		private string _projectName2;
		// Имя проекта, использующегося в нескольких тестах
		// Проект не изменяется при проведении тестов
		private string projectNoChangesName = "";
		
		private void checkAddUserRights()
		{	
			Logger.Trace("Проверка прав пользователей");
			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();
			// Ожидание открытия страницы
			UserRightsPage.AssertionUsersRightsPageDisplayed();
			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();

			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(UserRightsPage.WaitUntilGroupsRightsDisplay(), 
				"Ошибка: Страница прав групп пользователей не открылась.");

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();

			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(
				UserRightsPage.WaitUntilGroupsRightsDisplay(), 
				"Ошибка: Страница прав групп пользователей не открылась.");

			// Получение имени пользователя
			var userName = WorkspacePage.GetUserName();
			// Окрытие группы Administrators
			UserRightsPage.ClickGroupByName("Administrators");
			Thread.Sleep(1000);

			// Получение списка пользователей в группе Administrators
			var usersInGroup = UserRightsPage.GetDisplayUsersInGroup();

			if (!usersInGroup.Contains(userName) || !UserRightsPage.IsManageAllGlossariesRightIsPresent())
			{
				// Нажать Edit
				UserRightsPage.ClickEdit();

				if (!UserRightsPage.IsManageAllGlossariesRightIsPresent())
				{
					// Добавляем право на управление всеми глоссариями
					UserRightsPage.ClickAddRights();
					// Выбираем право
					UserRightsPage.SelectManageGlossaries();
					// Жмем Далее
					UserRightsPage.ClickNext();
					// Выбираем все глоссарии
					UserRightsPage.SelectAllGlossaries();
					// Жмем Далее
					UserRightsPage.ClickNext();
					// Жмем добавить
					UserRightsPage.ClickAdd();
					Thread.Sleep(1000);

					Assert.IsTrue(UserRightsPage.IsManageAllGlossariesRightIsPresent(), 
						"Ошибка: Право управления глоссариями не удалось добавить.");
				}

				if (!usersInGroup.Contains(userName))
				{
					// Вводим имя нового пользователя
					UserRightsPage.AddUserToGroup(userName);
				}
				// Сохранение настроек
				UserRightsPage.ClickSave();
				Thread.Sleep(1000);

				// Окрытие группы Administrators
				UserRightsPage.ClickGroupByName("Administrators");

				Thread.Sleep(1000);
				// Получение списка пользователей в группе Administrators
				usersInGroup = UserRightsPage.GetDisplayUsersInGroup();

				Assert.IsTrue(usersInGroup.Contains(userName), 
					"Ошибка: Пользователя не удалось добавить.");
				Assert.IsTrue(UserRightsPage.IsManageAllGlossariesRightIsPresent(), 
					"Ошибка: Право управления глоссариями не удалось добавить.");

				SwitchWorkspaceTab();
			}
		}

		private void autofillFormSourceWordSelected()
		{
			Logger.Debug("Автозаполнение сорса");
			const int segmentNumber = 1;
			EditorPage.SelectFirstWordSourceByAction(segmentNumber);
			openAddTermForm();

			AddTermForm.AssertionIsTextExistInSourceTerm("Earth");
		}

		private void createNewGlossary(bool firstGlossary = true)
		{
			Logger.Debug(string.Format("Создаем новый словарь. Первый словарь: {0}", firstGlossary));

			string glossaryName;

			SwitchGlossaryTab();

			// Получить уникальное имя для глоссария
			if (firstGlossary) 
			{
				_glossaryName = "00" + GetUniqueGlossaryName();
				glossaryName = _glossaryName;
			}
			else
			{
				if (GlossaryListPage.GetIsExistGlossary(_glossaryName2))
				{
					SwitchWorkspaceTab();
					return;
				}
				_glossaryName2 = "01" + GetUniqueGlossaryName();
				glossaryName = _glossaryName2;
			}

			// Создать глоссарий
			CreateGlossaryByName(glossaryName);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();			

			// Проверить, что глоссарий сохранился
			Assert.IsTrue(GlossaryListPage.GetIsExistGlossary(glossaryName), "Ошибка: глоссарий не создался " + glossaryName);

			SwitchWorkspaceTab();
		}

		/// <summary>
		/// Удаляем термин с заданными сорсом и таргетом		
		/// </summary>
		/// <param name="source">сорс</param> 
		/// <param name="target">таргет</param>  
		private void deleteTermByName(string source, string target)
		{
			var itemsCount = GlossaryPage.GetConceptCount();

			// Расширить окно, чтобы "корзинка" была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();
			// Выделить ячейку, чтобы "корзинка" появилась
			GlossaryPage.ClickTermRowByNameOfTerm(source, target);
			// Нажать на "корзинку"
			GlossaryPage.ClickDeleteBtn();
			GlossaryPage.AssertionConceptGeneralDelete();
			Thread.Sleep(1000);

			// Сравнить количество терминов
			var itemsCountAfter = GlossaryPage.GetConceptCount();
			Assert.IsTrue(
				itemsCountAfter < itemsCount, 
				"Ошибка: количество терминов не уменьшилось");
		}

		private void openAddTermForm()
		{
			Logger.Debug("Открытие формы добавления термина");
			EditorPage.ClickAddTermBtn();

			Assert.IsTrue(EditorPage.WaitAddTermFormDisplay(),
				"Ошибка: Форма для добавления термина не открылась.");
		}

		private void openCurrentGlossary()
		{
			Logger.Debug("Открытие текущего словаря");
			// Нажать кнопку Назад
			EditorPage.ClickHomeBtn();
			// Перейти к списку глоссариев
			SwitchGlossaryTab(); 
			// Перейти в глоссарий
			SwitchCurrentGlossary(_glossaryName);			
		}

		private void createProjectWithTwoGlossaries()
		{
			Logger.Debug("Создание проекта с двумя глоссариями");
			_projectName2 = ProjectUniqueName + "_" + DateTime.UtcNow.Ticks + "2";		 
			// Создать второй словарь
			createNewGlossary(false);					
			// Заполнение полей на первом шаге
			FirstStepProjectWizard(_projectName2);
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Настройка этапов workflow		 
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать существующую ТМ
			ChooseFirstTMInList();
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбор словарей			
			WorkspaceCreateProjectDialog.ClickGlossaryByName(_glossaryName);
			WorkspaceCreateProjectDialog.ClickGlossaryByName(_glossaryName2);
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();			
			// Настройка Pretranslate		  
			WorkspaceCreateProjectDialog.ClickFinishCreate();

			// Дождаться проекта в списке проектов
			WorkspacePage.WaitProjectAppearInList(_projectName2);
		}
	}
}
