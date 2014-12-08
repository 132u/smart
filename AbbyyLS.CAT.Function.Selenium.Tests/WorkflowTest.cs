using System.Collections.Generic;
using NUnit.Framework;
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки рабочего процесса
	/// </summary>
	class WorkflowTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public WorkflowTest(string browserName)
			: base (browserName)
		{
		}



		/// <summary>
		/// Старт тестов. Авторизация
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Не выходить из браузера после теста
			QuitDriverAfterTest = false;

			// 1. Переход на страницу workspace
			GoToWorkspace();
		}



		/// <summary>
		/// Проверка создания Translation Workflow по-умолчанию при создании проекта
		/// </summary>
		[Test]
		public void DefaultTaskType()
		{
			// Создание проекта до этапа воркфлоу
			FillFourStepsProjectWizard(ProjectName);

			// 5) Проверка workflow
			var workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();

			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowCreateList.Count, 
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual(
				"Translation", 
				workflowCreateList[0], 
				"Ошибка: Первая задача не \"Translation\"");
			
			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			WorkspaceCreateProjectDialog.WaitDialogDisappear();
		}

		/// <summary>
		/// Проверка выбора Editing Workflow при создании проекта
		/// </summary>
		[Test]
		public void ChangeTaskType()
		{
			// Создание проекта до этапа воркфлоу
			FillFourStepsProjectWizard(ProjectName);

			// 5) Проверка workflow
			// Изменение типа созданной задачи на Editing
			WorkspaceCreateProjectDialog.SetWorkflowEditingTask(1);
			Thread.Sleep(1000);
			
			var workflowList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия только одной задачи
			Assert.AreEqual(
				1,
				workflowList.Count, 
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Editing
			Assert.AreEqual(
				"Editing",
				workflowList[0], 
				"Ошибка: Первая задача не \"Editing\".");
			
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 6) Настройка Pretranslate. Проверка создания проекта
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(
				WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Проверяем наличие только одной задачи - Editing
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Editing
			Assert.AreEqual(
				"Editing", 
				workflowList[0],
				"Ошибка: Первая задача не \"Editing\".");
		}

		/// <summary>
		/// Проверка наличия всех типов при добавлении новой задачи Workflow
		/// </summary>
		[Test]
		public void NewTaskTypesOnCreate()
		{
			// Создание проекта до этапа воркфлоу
			FillFourStepsProjectWizard(ProjectName);

			// 5) Добавление новой задачи Workflow
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			Thread.Sleep(1000);

			// Проверка типов второй задачи
			var workflowTypesList = WorkspaceCreateProjectDialog.GetWFTaskTypeList(2);
			// Проверка наличия 3 типов задач
			Assert.AreEqual(3, workflowTypesList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual(
				"Translation", 
				workflowTypesList[0],
				"Ошибка: Первая задача не \"Translation\"");

			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			WorkspaceCreateProjectDialog.WaitDialogDisappear();
		}

		/// <summary>
		/// Проверка наличия добавленной новой задачи Workflow
		/// </summary>
		[Test]
		public void NewTask()
		{
			// Создание проекта
			CreateProject(ProjectName, "",
				false, "",
				Workspace_CreateProjectDialogHelper.SetGlossary.None, "",
				false, Workspace_CreateProjectDialogHelper.MT_TYPE.None,
				true,
				1, 0, 1);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Проверяем задачи Workflow проекта
			var workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual(
				"Translation",
				workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Proofreading
			Assert.AreEqual(
				"Proofreading",
				workflowList[1],
				"Ошибка: Вторая задача не \"Proofreading\".");
		}

		/// <summary>
		/// Проверка создания задачи Workflow того же типа, что и уже созданная
		/// </summary>
		[Test]
		public void NewTaskSameType()
		{
			// Создание проекта
			CreateProject(ProjectName, "",
				false, "",
				Workspace_CreateProjectDialogHelper.SetGlossary.None, "",
				false, Workspace_CreateProjectDialogHelper.MT_TYPE.None,
				true,
				2);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Проверяем задачи Workflow проекта
			var workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual(
				"Translation", 
				workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Transaltion
			Assert.AreEqual(
				"Translation",
				workflowList[1],
				"Ошибка: Вторая задача не \"Translation\".");
		}

		/// <summary>
		/// Проверка создания задачи в настройках уже созданного проекта
		/// </summary>
		[Test]
		public void AddingTask()
		{
			// Создание проекта
			CreateProject(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Добавление новой задачи
			ProjectPage.ClickProjectSettingsWorkflowNewTask();

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(2, "Proofreading");

			// Сохранение проекта
			ProjectPage.ClickProjectSettingsSave();
			Thread.Sleep(1000);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Проверяем задачи Workflow проекта
			var workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual(
				"Translation", 
				workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Proofreading
			Assert.AreEqual(
				"Proofreading",
				workflowList[1],
				"Ошибка: Вторая задача не \"Proofreading\".");
		}

		/// <summary>
		/// Проверка отмены создания задачи в настройках уже созданного проекта
		/// </summary>
		[Test]
		public void CancelAddingTask()
		{
			// Создание проекта
			CreateProject(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Добавление новой задачи
			ProjectPage.ClickProjectSettingsWorkflowNewTask();

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(2, "Proofreading");
			Thread.Sleep(1000);

			// Отмена сохранения проекта
			ProjectPage.ClickProjectSettingsCancel();
			Thread.Sleep(1000);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Проверяем задачи Workflow проекта
			var workflowList = ProjectPage.GetWFTaskListProjectSettings();

			// Проверка наличия одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual(
				"Translation", 
				workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");
		}

		/// <summary>
		/// Проверка удаления задачи Workflow при создании проекта
		/// </summary>
		[Test]
		public void DeleteTaskOnCreate()
		{
			// Создание проекта до этапа воркфлоу
			FillFourStepsProjectWizard(ProjectName);

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWorkflowProofreadingTask(2);
			Thread.Sleep(1000);

			// Удаление первой задачи Translation
			WorkspaceCreateProjectDialog.ClickWorkflowDeleteTask(1);
			Thread.Sleep(1000);

			var workflowList = WorkspaceCreateProjectDialog.GetWFTaskList();
			
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Proofreading
			Assert.AreEqual(
				"Proofreading",
				workflowList[0],
				"Ошибка: Первая задача не \"Proofreading\".");

			// Проверка, что первая задача - отображена с первым номером
			Assert.AreEqual(1, WorkspaceCreateProjectDialog.GetWFVisibleTaskNumber(1),
				"Ошибка: Первая задача отображена не с первым номером");

			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			WorkspaceCreateProjectDialog.WaitDialogDisappear();
		}

		/// <summary>
		/// Проверка удаления задачи Workflow в созданном проекте
		/// </summary>
		[Test]
		public void DeleteTask()
		{
			// Создание проекта
			CreateProject(ProjectName, "",
				false, "",
				Workspace_CreateProjectDialogHelper.SetGlossary.None, "",
				false, Workspace_CreateProjectDialogHelper.MT_TYPE.None,
				true,
				1, 0, 1);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Удаление первой задачи Translation
			ProjectPage.ClickProjectSettingsWFDeleteTask(1);
			Thread.Sleep(1000);

			var workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Proofreading
			Assert.AreEqual(
				"Proofreading",
				workflowList[0],
				"Ошибка: Первая задача не \"Proofreading\".");

			// Проверка, что первая задача - отображена с первым номером
			Assert.AreEqual(1, ProjectPage.GetProjectSettingsWFVisibleTaskNumber(1),
				"Ошибка: Первая задача отображена не с первым номером");
		}

		/// <summary>
		/// Проверка удаления задачи Workflow при создании проекта
		/// </summary>
		[Test]
		public void DeleteAllTasksOnCreate()
		{
			// Создание проекта до этапа воркфлоу
			FillFourStepsProjectWizard(ProjectName);

			// 5) Удаление первой задачи Translation
			WorkspaceCreateProjectDialog.ClickWorkflowDeleteTask(1);
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// Проверка наличия отображения сообщения об ошибке
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsErrorWFEmptyDisplayed(),
				"Ошибка: Не отображается сообщение об ошибке(пустой workflow).");

			// Проверка, что мастер не перешел на следующий шаг
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsStepWF(),
				"Ошибка: Мастер не находится на шаге Workflow.");

			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			WorkspaceCreateProjectDialog.WaitDialogDisappear();
		}

		/// <summary>
		/// Проверка отмены удаления задачи Workflow в созданном проекте
		/// </summary>
		[Test]
		public void CancelDeleteTask()
		{
			// Создание проекта
			CreateProject(ProjectName, "",
				false, "",
				Workspace_CreateProjectDialogHelper.SetGlossary.None, "",
				false, Workspace_CreateProjectDialogHelper.MT_TYPE.None,
				true,
				1, 0, 1);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();
			
			// Записываем задачи Workflow проекта
			var workflowListBefore = ProjectPage.GetWFTaskListProjectSettings();

			// Удаление первой задачи
			ProjectPage.ClickProjectSettingsWFDeleteTask(1);
			Thread.Sleep(1000);

			// Отмена сохранения проекта
			ProjectPage.ClickProjectSettingsCancel();
			Thread.Sleep(1000);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Записываем задачи Workflow проекта
			var workflowListAfter = ProjectPage.GetWFTaskListProjectSettings();

			// Проверка, что задачи не изменились
			Assert.AreEqual(
				workflowListBefore, 
				workflowListAfter,
				"Ошибка: Список задач изменился.");
		}

		/// <summary>
		/// Проверка редактирования задачи в настройках уже созданного проекта
		/// </summary>
		[Test]
		public void ChangingTask()
		{
			var workflowList = new List<string>();
			var workflowTaskBefore = "";
			var workflowTaskAfter = "";

			// Создание проекта
			CreateProject(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Проверяем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			workflowTaskBefore = workflowList[0];

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(1, "Proofreading");
			Thread.Sleep(1000);

			// Сохранение проекта
			ProjectPage.ClickProjectSettingsSave();
			Thread.Sleep(1000);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Проверяем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			workflowTaskAfter = workflowList[0];
			
			// Проверка, что первая задача - Proofreading
			Assert.AreEqual(
				"Proofreading",
				workflowList[0],
				"Ошибка: Вторая задача не \"Proofreading\".");

			// Проверка, что тип задачи изменился
			Assert.AreNotEqual(
				workflowTaskBefore,
				workflowTaskAfter,
				"Ошибка: тип задачи не изменился.");
		}

		/// <summary>
		/// Проверка наличия добавленной новой задачи Workflow при возврате в мастере
		/// </summary>
		[Test]
		public void BackOnCreate()
		{
			// Создание проекта до этапа воркфлоу
			FillFourStepsProjectWizard(ProjectName);

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWorkflowProofreadingTask(2);
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// 6) Возврат к предыдущей странице мастера
			WorkspaceCreateProjectDialog.ClickBackBtn();
			Thread.Sleep(1000);

			// Проверка workflow
			var workflowList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия только двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual(
				"Translation", 
				workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Proofreading
			Assert.AreEqual(
				"Proofreading",
				workflowList[1],
				"Ошибка: Вторая задача не \"Proofreading\".");

			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			WorkspaceCreateProjectDialog.WaitDialogDisappear();
		}

		/// <summary>
		/// Проверка добавления новой задачи Workflow после возврата в мастере
		/// </summary>
		[Test]
		public void AddingTaskAfterBack()
		{
			// Создание проекта до этапа воркфлоу
			FillFourStepsProjectWizard(ProjectName);

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWorkflowProofreadingTask(2);
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// 6) Возврат к предыдущей странице мастера
			WorkspaceCreateProjectDialog.ClickBackBtn();
			Thread.Sleep(1000);

			// 5) Добавление новой задачи Editing
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWorkflowEditingTask(3);
			Thread.Sleep(1000);

			// Проверка workflow
			var workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия трех задач
			Assert.AreEqual(3, workflowCreateList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowCreateList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowCreateList[1],
				"Ошибка: Вторая задача не \"Proofreading\".");

			// Проверка, что третья задача - Editing
			Assert.AreEqual("Editing", workflowCreateList[2],
				"Ошибка: Первая задача не \"Editing\".");

			WorkspaceCreateProjectDialog.ClickNextStep();

			// 6) Настройка Pretranslate. Проверка создания проекта
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Записываем задачи Workflow проекта
			var workflowList = ProjectPage.GetWFTaskListProjectSettings();

			// Проверка отличия задач
			Assert.AreEqual(
				workflowCreateList, 
				workflowList,
				"Ошибка: Задачи отличаются от тех, что были созданы.");
		}

		/// <summary>
		/// Проверка изменения новой задачи Workflow после возврата в мастере
		/// </summary>
		[Test]
		public void ChangingTaskAfterBack()
		{
			// Создание проекта до этапа воркфлоу
			FillFourStepsProjectWizard(ProjectName);

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWorkflowProofreadingTask(2);
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// 6) Возврат к предыдущей странице мастера
			WorkspaceCreateProjectDialog.ClickBackBtn();
			Thread.Sleep(1000);

			// 5) Изменение задачи Proofreading на Editing
			WorkspaceCreateProjectDialog.SetWorkflowEditingTask(2);
			Thread.Sleep(1000);

			// 6) Возврат к предыдущей странице мастера
			WorkspaceCreateProjectDialog.ClickBackBtn();
			Thread.Sleep(1000);

			// 5) Изменение задачи Proofreading на Editing
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Editing");
			Thread.Sleep(1000);

			// Проверка workflow
			var workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия трех задач
			Assert.AreEqual(2, workflowCreateList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowCreateList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Editing
			Assert.AreEqual("Editing", workflowCreateList[1],
				"Ошибка: Первая задача не \"Editing\".");

			WorkspaceCreateProjectDialog.ClickNextStep();

			// 6) Настройка Pretranslate. Проверка создания проекта
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Записываем задачи Workflow проекта
			var workflowList = ProjectPage.GetWFTaskListProjectSettings();

			// Проверка отличия задач
			Assert.AreEqual(
				workflowCreateList, 
				workflowList,
				"Ошибка: Задачи отличаются от тех, что были созданы.");
		}

		/// <summary>
		/// Проверка удаления задачи Workflow после возврата в мастере
		/// </summary>
		[Test]
		public void DeletingTaskAfterBack()
		{
			// Создание проекта до этапа воркфлоу
			FillFourStepsProjectWizard(ProjectName);

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWorkflowProofreadingTask(2);
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// 6) Возврат к предыдущей странице мастера
			WorkspaceCreateProjectDialog.ClickBackBtn();
			Thread.Sleep(1000);

			// 5) Удаление первой задачи Translation
			WorkspaceCreateProjectDialog.ClickWorkflowDeleteTask(1);
			Thread.Sleep(1000);

			// Проверка workflow
			var workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия трех задач
			Assert.AreEqual(1, workflowCreateList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowCreateList[0],
				"Ошибка: Первая задача не \"Proofreading\".");

			WorkspaceCreateProjectDialog.ClickNextStep();

			// 6) Настройка Pretranslate. Проверка создания проекта
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Записываем задачи Workflow проекта
			var workflowList = ProjectPage.GetWFTaskListProjectSettings();

			// Проверка отличия задач
			Assert.AreEqual(workflowCreateList, workflowList,
				"Ошибка: Задачи отличаются от тех, что были созданы.");
		}

		protected void FillFourStepsProjectWizard(string projectName)
		{
			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(projectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseFirstTMInList();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();
		}
	}
}
