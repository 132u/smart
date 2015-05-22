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
				Authorization(Login, Password);


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
			openAddTermForm();
		}

		/// <summary>
		/// Открывает форму добавления термина в редакторе по хоткею
		/// </summary>
		[Test]
		[Category("SCAT_102")]
		public void OpenAddTermFormHotKey()
		{
			EditorPage.AddTermFormByHotkey(1);

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
			autofillFormSourceWordSelected();

			AddTermForm.ClickAddBtn();

			Assert.IsTrue(
				AddTermForm.WaitConfirmSingleTermMessage(),
				"Ошибка: Не было сообщения о добавлении одиночного термина.");

			// Добавить термин
			AddTermForm.ClickAddSingleTerm();
			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

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
			autofillFormSourceWordSelected();
			
			AddTermForm.ClickAddBtn();
			Assert.IsTrue(
				AddTermForm.WaitConfirmSingleTermMessage(),
				"Ошибка: Не было сообщения о добавлении одиночного термина.");

			AddTermForm.ClickAddSingleTerm();

			if (AddTermForm.WaitAnyWayTermMessage())
				AddTermForm.CliCkYesBtnInAnyWayTermMessage();

			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

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

			AddTermForm.TypeSourceTermText("Comet");

			AddTermForm.TypeTargetTermText("Комета");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");	

			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Comet", "Комета"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить измененный термин из сорса с таргетом в глоссарий
		/// </summary>
		[Test]
		public void AddModifiedSourceTargetTermToGlossary()
		{
			autofillFormSourceWordSelected();

			AddTermForm.TypeSourceTermText("Mars");

			AddTermForm.TypeTargetTermText("Марс");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");			

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
			autofillFormSourceWordSelected();

			AddTermForm.TypeSourceTermText("Uran");

			AddTermForm.TypeTargetTermText("Уран");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			// Удаляем текст из таргета и подтверждаем 
			// (чтобы не всплывало PopUp окно, AutoSave иногда очень тормозит)
			EditorPage.ClearTarget(1);
			EditorPage.ClickConfirmBtn();
			AutoSave();

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
			openAddTermForm();

			AddTermForm.TypeSourceTermText("planet");

			AddTermForm.TypeTargetTermText("планета");


			AddTermForm.ClickAddBtn();
			Assert.IsTrue(
				AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			AddTermForm.ClickCloseBtnInTermSavedMsg();

			AddTermForm.WaitTermSavedMessageDisappear();

			openAddTermForm();

			AddTermForm.TypeSourceTermText("planet");

			AddTermForm.TypeTargetTermText("планетка");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitContainsTermMessage(),
				"Ошибка: Не было сообщения о повторном добавлении термина.");

			AddTermForm.ClickContainsTermYes();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("planet", "планетка"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий термин уже существующий в таргет
		/// </summary>
		[Test]
		public void AddExistedTargetTermToGlossary()
		{
			openAddTermForm();

			AddTermForm.TypeSourceTermText("asteroid");

			AddTermForm.TypeTargetTermText("астероид");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			AddTermForm.ClickCloseBtnInTermSavedMsg();

			AddTermForm.WaitTermSavedMessageDisappear();

			openAddTermForm();

			AddTermForm.TypeSourceTermText("the Asteroid");

			AddTermForm.TypeTargetTermText("астероид");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitContainsTermMessage(),
				"Ошибка: Не было сообщения о повторном добавлении термина.");

			AddTermForm.ClickContainsTermYes();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("the Asteroid", "астероид"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий абсолютно идентичный термин
		/// </summary>
		[Test]
		public void AddExistedTermToGlossary()
		{
			openAddTermForm();

			AddTermForm.TypeSourceTermText("Sun");

			AddTermForm.TypeTargetTermText("солнце");

			AddTermForm.ClickAddBtn();
			
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			AddTermForm.ClickCloseBtnInTermSavedMsg();

			AddTermForm.WaitTermSavedMessageDisappear();

			openAddTermForm();

			AddTermForm.TypeSourceTermText("Sun");

			AddTermForm.TypeTargetTermText("солнце");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitContainsTermMessage(),
				"Ошибка: Не было сообщения о повторном добавлении термина.");

			AddTermForm.ClickContainsTermYes();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetAreTwoEqualTermsExist("Sun", "солнце"), "Ошибка: Не добавлен термин.");
		}

		/// <summary>
		/// Добавить в глоссарий комментарий
		/// </summary>
		[Test]
		public void AddCommentToGlossary()
		{
			openAddTermForm();

			AddTermForm.TypeSourceTermText("Neptun");

			AddTermForm.TypeTargetTermText("Нептун");

			AddTermForm.TypeCommentText("Generated By Selenium");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");	   

			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsCommentExists("Generated By Selenium"), "Ошибка: Не добавлен комментарий.");
		}

		/// <summary>
		/// Проверка выпадающего списка с глоссариями, при создании проекта подключалось два глоссария
		/// </summary>
		[Test]
		public void CheckGlossaryListInProjectCreatedWithTwoGlossaries()
		{
			EditorPage.ClickHomeBtn();

			createProjectWithTwoGlossaries();
		
			ImportDocumentProjectSettings(PathProvider.DocumentFile, _projectName2);

			AssignTask(1);

			OpenDocument();

			openAddTermForm();

			AddTermForm.OpenGlossaryList();
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName),
				string.Format("Ошибка: Словарь {0} отсуствует в выпадающем списке.", _glossaryName));
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName2),
				string.Format("Ошибка: Словарь {0} отсуствует в выпадающем списке.", _glossaryName2));

		}

		/// <summary>
		/// Проверка выпадающего списка с глоссариями, проект с двумя глоссариями, второй глоссарий подключается в настройках проекта
		/// </summary>
		[Test]
		public void CheckGlossaryListInProjectCreatedWithOneGlossary()
		{
			_projectName2 = ProjectUniqueName + "2";

			EditorPage.ClickHomeBtn();

			createNewGlossary(false);

			CreateProject(_projectName2, "", false, "", Workspace_CreateProjectDialogHelper.SetGlossary.ByName, _glossaryName);
	
			ImportDocumentProjectSettings(PathProvider.DocumentFile, _projectName2);

			AssignTask(1);

			ProjectPage.SetGlossaryByName(_glossaryName2);

			OpenDocument();

			openAddTermForm();

			AddTermForm.OpenGlossaryList();
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName),
				string.Format("Ошибка: Словарь {0} отсуствует в выпадающем списке.", _glossaryName));
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName2),
				string.Format("Ошибка: Словарь {0} отсуствует в выпадающем списке.", _glossaryName2));

		}

		/// <summary>
		/// Добавление одинаковых терминов в разные глоссарии
		/// </summary>
		[Test]
		public void AddEqualTermsInTwoGlossaries()
		{
			EditorPage.ClickHomeBtn();

			createProjectWithTwoGlossaries();

			ImportDocumentProjectSettings(PathProvider.DocumentFile, _projectName2);

			AssignTask(1);

			OpenDocument();

			openAddTermForm();

			AddTermForm.OpenGlossaryList();
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName),
				string.Format("Ошибка: Словарь {0} отсуствует в выпадающем списке.", _glossaryName));
			Assert.IsTrue(AddTermForm.CheckGlossaryByName(_glossaryName2),
				string.Format("Ошибка: Словарь {0} отсуствует в выпадающем списке.", _glossaryName2));

			AddTermForm.SelectGlossaryByName(_glossaryName);

			AddTermForm.TypeSourceTermText("Space");

			AddTermForm.TypeTargetTermText("Космос");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");

			AddTermForm.ClickCloseBtnInTermSavedMsg();

			AddTermForm.WaitTermSavedMessageDisappear();

			openAddTermForm();

			AddTermForm.OpenGlossaryList();

			AddTermForm.SelectGlossaryByName(_glossaryName2);

			AddTermForm.TypeSourceTermText("Space");

			AddTermForm.TypeTargetTermText("Космос");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
				"Ошибка: Не было сообщения о сохранении термина.");
		}

		/// <summary>
		/// Добавление удаленного термина в глоссарий
		/// </summary>
		[Test]
		public void DeleteAddTermToGlossary()
		{
			openAddTermForm();

			AddTermForm.TypeSourceTermText("Galaxy");

			AddTermForm.TypeTargetTermText("Галактика");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

			openCurrentGlossary();
			Assert.IsTrue(GlossaryPage.GetIsSourceTargetTermExists("Galaxy", "Галактика"), "Ошибка: Не добавлен термин.");

			deleteTermByName("Galaxy", "Галактика");
			SwitchWorkspaceTab();

			WorkspacePage.OpenProjectPage(projectNoChangesName);

			OpenDocument();
			Thread.Sleep(500);

			openAddTermForm();

			AddTermForm.TypeSourceTermText("Galaxy");

			AddTermForm.TypeTargetTermText("Галактика");

			AddTermForm.ClickAddBtn();
			Assert.IsTrue(AddTermForm.WaitTermSavedMessage(),
			   "Ошибка: Не было сообщения о сохранении термина.");

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

			WorkspacePage.ClickUsersAndRightsBtn();

			UserRightsPage.AssertionUsersRightsPageDisplayed();

			UserRightsPage.OpenGroups();

			Assert.IsTrue(UserRightsPage.WaitUntilGroupsRightsDisplay(), 
				"Ошибка: Страница прав групп пользователей не открылась.");

			UserRightsPage.OpenGroups();

			Assert.IsTrue(
				UserRightsPage.WaitUntilGroupsRightsDisplay(), 
				"Ошибка: Страница прав групп пользователей не открылась.");

			var userName = WorkspacePage.GetUserName();

			UserRightsPage.ClickGroupByName("Administrators");
			Thread.Sleep(1000);

			// Получение списка пользователей в группе Administrators
			var usersInGroup = UserRightsPage.GetDisplayUsersInGroup();

			if (!usersInGroup.Contains(userName) || !UserRightsPage.IsManageAllGlossariesRightIsPresent())
			{
				UserRightsPage.ClickEdit();

				if (!UserRightsPage.IsManageAllGlossariesRightIsPresent())
				{
					UserRightsPage.ClickAddRights();

					UserRightsPage.SelectManageGlossaries();

					UserRightsPage.ClickNext();

					UserRightsPage.SelectAllGlossaries();
					
					UserRightsPage.ClickNext();
					
					UserRightsPage.ClickAdd();
					Thread.Sleep(1000);

					Assert.IsTrue(UserRightsPage.IsManageAllGlossariesRightIsPresent(), 
						"Ошибка: Право управления глоссариями не удалось добавить.");
				}
				if (!usersInGroup.Contains(userName))
				{
					UserRightsPage.AddUserToGroup(userName);
				}

				UserRightsPage.ClickSave();
				Thread.Sleep(1000);

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

			CreateGlossaryByName(glossaryName);

			SwitchGlossaryTab();			

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
			GlossaryPage.HoverOnTermRowByNameOfTerm(source, target);

			GlossaryPage.ClickDeleteBtn();

			GlossaryPage.AssertionConceptGeneralDelete();
			Thread.Sleep(1000);

			var itemsCountAfter = GlossaryPage.GetConceptCount();
			Assert.IsTrue(
				itemsCountAfter < itemsCount, 
				"Ошибка: количество терминов не уменьшилось");
		}

		private void openAddTermForm()
		{
			EditorPage.ClickAddTermBtn();

			Assert.IsTrue(EditorPage.WaitAddTermFormDisplay(),
				"Ошибка: Форма для добавления термина не открылась.");
		}

		private void openCurrentGlossary()
		{
			Logger.Debug("Открытие текущего словаря");

			EditorPage.ClickHomeBtn();

			SwitchGlossaryTab(); 

			SwitchCurrentGlossary(_glossaryName);			
		}

		private void createProjectWithTwoGlossaries()
		{
			Logger.Debug("Создание проекта с двумя глоссариями");
			_projectName2 = ProjectUniqueName + "_" + DateTime.UtcNow.Ticks + "2";	
	 
			createNewGlossary(false);		
			
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

			WorkspaceCreateProjectDialog.WaitDialogDisappear();

			// Дождаться проекта в списке проектов
			WorkspacePage.WaitProjectAppearInList(_projectName2);
		}
	}
}
