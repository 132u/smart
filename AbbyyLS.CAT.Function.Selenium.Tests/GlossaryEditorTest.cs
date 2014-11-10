using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	[TestFixture]
	class GlossaryEditorTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public GlossaryEditorTest(string browserName)
			: base (browserName)
		{
		}

		private string _glossaryName;
		private string _glossaryName2;	   
		private string _projectName2;
		// Имя проекта, использующегося в нескольких тестах
		// Проект не изменяется при проведении тестов
		private string projectNoChangesName = "";
		private bool beforeTests = false;
	   
		

		[TestFixtureSetUp]
		public void BeforeClass()
		{
			// Создание уникального имени проекта
			CreateUniqueNamesByDatetime();

			// Запись имени для дальнейшего использования в группе тестов
			projectNoChangesName = ProjectName;
		}

		[SetUp]
		public void Setup()
		{
			// Не выходить из браузера после теста
			quitDriverAfterTest = false;

			GoToWorkspace();

			if (!beforeTests)
			{
				//Проверка прав пользователя
				CheckAddUserRights();
				//Создание нового словаря
				CreateNewGlossary();
				//Создание проекта	
				CreateProjectIfNotCreated(projectNoChangesName, "", false, "", Workspace_CreateProjectDialogHelper.SetGlossary.ByName, _glossaryName);
				//Открытие настроек проекта			
				ImportDocumentProjectSettings(DocumentFile, projectNoChangesName);
				//Назначение задачи на пользователя
				AssignTask(1);

				beforeTests = true;
			}
			else
			{
				// Открыть проект
				WorkspacePage.OpenProjectPage(projectNoChangesName);
			}

			//Открытие документа
			OpenDocument();
		}

		[TestFixtureTearDown]
		public override void TeardownAllBase()
		{
			GoToGlossaries();
			// Зайти в глоссарий
			SwitchCurrentGlossary(_glossaryName);
			// Удалить глоссарий
			DeleteGlossary();
			// Удалить второй глоссарий
			if (_glossaryName2 != null && GetIsExistGlossary(_glossaryName2))
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
			OpenAddTermForm();
			
		}

		/// <summary>
		/// Открывает форму добавления термина в редакторе по хоткею
		/// </summary>
		[Test]
		public void OpenAddTermFormHotKey()
		{
			// Нажать хоткей вызова формы для добавления термина
			EditorPage.SendKeysTarget(1, OpenQA.Selenium.Keys.Control + "E");

			// Проверка, что открылась форма
			Assert.IsTrue(EditorPage.WaitAddTermFormDisplay(),
				"Ошибка: Форма для добавления термина не открылась.");
		}

		/// <summary>
		/// Проверка автозапослнения формы при выделенном слове в сорсе
		/// </summary>
		[Test]
		public void AutofillAddTermFormSourceWordSelected()
		{
			//Автозаполнение формы
			AutofillFormSourceWordSelected();		
		}

		/// <summary>
		/// Проверка автозапослнения формы при выделенном слове в тагрет
		/// </summary>
		[Test]
		public void TargetWordSelectedAutofillAddTermForm()
		{
			AutofillFormTargetWordSelected();
			
			// Удаляем текст из таргета и подтверждаем (чтобы не всплывало PopUp окно, AutoSave иногда очень тормозит)
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
			AutofillFormSourceWordSelected();
			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitConfirmSingleTermMessage(),
				"Ошибка: Не было сообщения о добавлении одиночного термина.");
			// Добавить термин
			AddTermForm.ClickAddSingleTerm();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");
			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
			// Открыть глоссарий и проверить, есть ли термин
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSingleSourceTermExists("Earth"),
				"Ошибка: Не добавлен одиночный термин из сорса.");
		}

		/// <summary>
		/// Добавить одиночный термин из таргета в глоссарий
		/// </summary>
		[Test]
		public void AddSingleTargetTermToGlossary()
		{
			// Автозаполнение формы
			AutofillFormTargetWordSelected();
			
			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitConfirmSingleTermMessage(),
				"Ошибка: Не было сообщения о добавлении одиночного термина.");
			// Добавить термин
			AddTermForm.ClickAddSingleTerm();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");
			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();

			// Удаляем текст из таргета и подтверждаем (чтобы не всплывало PopUp окно, AutoSave иногда очень тормозит)
			EditorPage.ClearTarget(1);
			Thread.Sleep(500);
			EditorPage.ClickConfirmBtn();
			Thread.Sleep(7000);

			// Открыть глоссарий и проверить, есть ли термин
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSingleTargetTermExists("Земля"),
				"Ошибка: Не добавлен одиночный термин из таргета.");
		}

		/// <summary>
		/// Добавить термин с сорсом и таргетом в глоссарий
		/// </summary>
		[Test]
		public void AddSourceTargetTermToGlossary()
		{
			OpenAddTermForm();
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
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Comet", "Комета"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить измененный термин из сорса с таргетом в глоссарий
		/// </summary>
		[Test]
		public void AddModifiedSourceTargetTermToGlossary()
		{
			// Автозаполнение формы
			AutofillFormSourceWordSelected();
			// Изменить текст сорса
			AddTermForm.TypeSourceTermText("Mars");
			// Добавить термин в таргет
			AddTermForm.TypeTargetTermText("Марс");
			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");
			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();			
			// Открыть глоссарий и проверить, есть ли термин
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Mars", "Марс"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить измененный термин из таргета в глоссарий
		/// </summary>
		[Test]
		public void AddSourceModifiedTargetTermToGlossary()
		{
			// Автозаполнение формы
			AutofillFormTargetWordSelected();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("Uran");
			// Изменить термин таргета
			AddTermForm.TypeTargetTermText("Уран");
			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");
			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();

			// Удаляем текст из таргета и подтверждаем (чтобы не всплывало PopUp окно, AutoSave иногда очень тормозит)
			EditorPage.ClearTarget(1);
			EditorPage.ClickConfirmBtn();
			AutoSave();


			// Открыть глоссарий и проверить, есть ли термин
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Uran", "Уран"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий термин уже существующий в сорс 
		/// </summary>
		[Test]
		public void AddExistedSourceTermToGlossary()
		{
			// Открытие формы
			OpenAddTermForm();
			// Добавить сорс
			AddTermForm.TypeSourceTermText("planet");
			// Добавить таргет
			AddTermForm.TypeTargetTermText("планета");
			// Нажать сохранить
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Термин сохранен, нажать ок
			AddTermForm.ClickTermSaved();
			// Открытие формы
			OpenAddTermForm();
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
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("planet", "планетка"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий термин уже существующий в таргет
		/// </summary>
		[Test]
		public void AddExistedTargetTermToGlossary()
		{
			// Открытие формы
			OpenAddTermForm();
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
			OpenAddTermForm();
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
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("the Asteroid", "астероид"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий абсолютно идентичный термин
		/// </summary>
		[Test]
		public void AddExistedTermToGlossary()
		{
			// Открытие формы
			OpenAddTermForm();
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
			OpenAddTermForm();
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
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetAreTwoEqualTermsExist("Sun", "солнце"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий комментарий
		/// </summary>
		[Test]
		public void AddCommentToGlossary()
		{
			// Открытие формы
			OpenAddTermForm();
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
			OpenCurrentGlossary();
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
			CreateProjectWithTwoGlossaries();
			//Открытие настроек проекта			
			ImportDocumentProjectSettings(DocumentFile, _projectName2);
			//Назначение задачи на пользователя
			AssignTask(1);
			//Открытие документа
			OpenDocument();
			// Открытие формы
			OpenAddTermForm();
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
			_projectName2 = ProjectName + "_" + DateTime.UtcNow.Ticks.ToString() + "2";

			// Нажать кнопку Назад
			EditorPage.ClickHomeBtn();
			// Создать новый глоссарий
			CreateNewGlossary(false);
			// Создать проект с одним глоссарием
			CreateProject(_projectName2, "", false, "", Workspace_CreateProjectDialogHelper.SetGlossary.ByName, _glossaryName);
			//Открытие настроек проекта			
			ImportDocumentProjectSettings(DocumentFile, _projectName2);
			//Назначение задачи на пользователя
			AssignTask(1);
			// Добавляем второй глоссарий
			ProjectPage.SetGlossaryByName(_glossaryName2);
			//Открытие документа
			OpenDocument();
			// Открытие формы
			OpenAddTermForm();
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
			CreateProjectWithTwoGlossaries();
			//Открытие настроек проекта			
			ImportDocumentProjectSettings(DocumentFile, _projectName2);
			//Назначение задачи на пользователя
			AssignTask(1);
			//Открытие документа
			OpenDocument();
			// Открытие формы
			OpenAddTermForm();
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
			OpenAddTermForm();
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
			OpenAddTermForm();
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
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Galaxy", "Галактика"), "Ошибка: Не добавлен термин.");

			// Удаляем заданный термин
			DeleteTermByName("Galaxy", "Галактика");
			SwitchWorkspaceTab();
			// Открытие проекта
			WorkspacePage.OpenProjectPage(projectNoChangesName);
			// Открытие документа
			ProjectPage.OpenDocument(1);
			Thread.Sleep(500);
			// Открытие формы
			OpenAddTermForm();
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
			OpenCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Galaxy", "Галактика"), "Ошибка: Не добавлен повторно термин.");
		}



		/// <summary>
		/// Проверяем, что там у пользователя с правами
		/// </summary>
		private void CheckAddUserRights()
		{			
			// Переходим к вкладке прав пользователей
			WorkspacePage.ClickUsersAndRightsBtn();
			// Ожидание открытия страницы
			Assert.IsTrue(UserRightsPage.WaitUntilUsersRightsDisplay(), "Ошибка: Страница прав пользователя не открылась.");

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();
			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(UserRightsPage.WaitUntilGroupsRightsDisplay(), "Ошибка: Страница прав групп пользователей не открылась.");

			// Открываем страницу групп пользователей
			UserRightsPage.OpenGroups();
			// Ожидание открытия страницы прав групп пользователей
			Assert.IsTrue(UserRightsPage.WaitUntilGroupsRightsDisplay(), "Ошибка: Страница прав групп пользователей не открылась.");

			// Получение имени пользователя
			string userName = WorkspacePage.GetUserName();

			// Окрытие группы Administrators
			UserRightsPage.ClickGroupByName("Administrators");
			Thread.Sleep(1000);

			// Получение списка пользователей в группе Administrators
			List<string> usersInGroup = UserRightsPage.GetDisplayUsersInGroup();

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

				Assert.IsTrue(usersInGroup.Contains(userName), "Ошибка: Пользователя не удалось добавить.");
				Assert.IsTrue(UserRightsPage.IsManageAllGlossariesRightIsPresent(), 
					"Ошибка: Право управления глоссариями не удалось добавить.");

				SwitchWorkspaceTab();
			}
		}

		/// <summary>
		/// Автозаполнение сорса
		/// </summary>
		private void AutofillFormSourceWordSelected()
		{
			// Курсор в Source
			EditorPage.ClickToggleBtn();
			// Нажать хоткей перехода в начало строки
			EditorPage.SendKeysSource(1, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
			// Нажать хоткей выделения первого слова
			EditorPage.SendKeysSource(1, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
			//Открываем форму добавления термина
			OpenAddTermForm();
			Assert.IsTrue(AddTermForm.GetSourceTermText("Earth"), "Ошибка: Нет автозаполнения сорса.");
		}

		/// <summary>
		/// Автозаполнение таргета
		/// </summary>
		private void AutofillFormTargetWordSelected()
		{
			// Удаляем старый текст из таргета
			EditorPage.ClearTarget(1);
			// Написать что-то в target
			EditorPage.AddTextTarget(1, "Земля это такая планета.");
			// Нажать хоткей перехода в начало строки
			EditorPage.SendKeysTarget(1, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
			// Нажать хоткей выделения первого слова
			EditorPage.SendKeysTarget(1, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
			//Открываем форму добавления термина
			OpenAddTermForm();
			Assert.IsTrue(AddTermForm.GetTargetTermText("Земля"), "Ошибка: Нет автозаполнения таргета.");
		}

		/// <summary>
		/// Создаем новый словарь с заданным именем		 
		/// </summary>
		/// <param name="firstGlossary">словарь номер один или два</param> 
		private void CreateNewGlossary(bool firstGlossary = true)
		{
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
				if (GetIsExistGlossary(_glossaryName2))
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
			Assert.IsTrue(GetIsExistGlossary(glossaryName), "Ошибка: глоссарий не создался " + glossaryName);

			SwitchWorkspaceTab();
		}

		/// <summary>
		/// Удаляем термин с заданными сорсом и таргетом		
		/// </summary>
		/// <param name="source">сорс</param> 
		/// <param name="target">таргет</param>  
		private void DeleteTermByName(string source, string target)
		{
			int itemsCount = GetCountOfItems();

			// Расширить окно, чтобы "корзинка" была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();
			// Выделить ячейку, чтобы "корзинка" появилась
			GlossaryPage.ClickTermRowByNameOfTerm(source, target);
			// Нажать на "корзинку"
			GlossaryPage.ClickDeleteBtn();
			GlossaryPage.WaitConceptGeneralDelete();
			Thread.Sleep(1000);
			// Сравнить количество терминов
			int itemsCountAfter = GetCountOfItems();
			Assert.IsTrue(itemsCountAfter < itemsCount, "Ошибка: количество терминов не уменьшилось");
		}

		/// <summary>
		/// Открыть форму добавления термина	  
		/// </summary>
		private void OpenAddTermForm()
		{
			// Нажать кнопку вызова формы для добавления термина
			EditorPage.ClickAddTermBtn();
			// Проверка, что открылась форма
			Assert.IsTrue(EditorPage.WaitAddTermFormDisplay(),
				"Ошибка: Форма для добавления термина не открылась.");
		}

		/// <summary>
		/// Открыть текущий словарь	  
		/// </summary>
		private void OpenCurrentGlossary()
		{
			// Нажать кнопку Назад
			EditorPage.ClickHomeBtn();			
			// Перейти к списку глоссариев
			SwitchGlossaryTab(); 
			// Перейти в глоссарий
			SwitchCurrentGlossary(_glossaryName);			
		}

		/// <summary>
		/// Создать проект с двумя глоссариями	 
		/// </summary>
		private void CreateProjectWithTwoGlossaries()
		{
			_projectName2 = ProjectName + "_" + DateTime.UtcNow.Ticks.ToString() + "2";		 
				// Создать второй словарь
				CreateNewGlossary(false);					
			// Заполнение полей на первом шаге
			FirstStepProjectWizard(_projectName2);
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
			// Настройка этапов workflow		 
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Настройка Pretranslate		  
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			// Дождаться проекта в списке проектов		   
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(_projectName2), "Ошибка: проект не появился в списке Workspace");			
		}
	}
}
