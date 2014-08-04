using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки рабочего процесса
	/// </summary>
	class WorkflowTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="url">Адрес</param>
		/// <param name="workspaceUrl">Адрес workspace</param>
		/// <param name="browserName">Название браузера</param>
		public WorkflowTest(string url, string workspaceUrl, string browserName)
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
		/// Проверка создания Translation Workflow по-умолчанию при создании проекта
		/// </summary>
		[Test]
		public void DefaultTaskType()
		{
			List<string> workflowCreateList = new List<string>();
			
			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();
						
			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();
			
			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Проверка workflow
			workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowCreateList.Count, 
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowCreateList[0], 
				"Ошибка: Первая задача не \"Translation\"");
			
			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка выбора Editing Workflow при создании проекта
		/// </summary>
		[Test]
		public void ChangeTaskType()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Проверка workflow
			// Изменение типа созданной задачи на Editing
			WorkspaceCreateProjectDialog.SetWFTaskList(1, "Editing");

			workflowList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowList.Count, 
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Editing
			Assert.AreEqual("Editing", workflowList[0], 
				"Ошибка: Первая задача не \"Editing\".");
			
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 6) Настройка Pretranslate. Проверка создания проекта
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Проверяем наличие только одной задачи - Editing
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Editing
			Assert.AreEqual("Editing", workflowList[0],
				"Ошибка: Первая задача не \"Editing\".");
		}

		/// <summary>
		/// Проверка наличия всех типов при добавлении новой задачи Workflow
		/// </summary>
		[Test]
		public void NewTaskTypesOnCreate()
		{
			List<string> workflowTypesList = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Workflow
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			Thread.Sleep(1000);

			// Проверка типов второй задачи
			workflowTypesList = WorkspaceCreateProjectDialog.GetWFTaskTypeList(2);
			// Проверка наличия 7 типов задач
			Assert.AreEqual(7, workflowTypesList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowTypesList[0],
				"Ошибка: Первая задача не \"Translation\"");

			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка наличия добавленной новой задачи Workflow
		/// </summary>
		[Test]
		public void NewTask()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			Thread.Sleep(1000);
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Proofreading");
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 6) Настройка Pretranslate. Проверка создания проекта
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Проверяем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowList[1],
				"Ошибка: Вторая задача не \"Proofreading\".");
		}

		/// <summary>
		/// Проверка создания задачи Workflow того же типа, что и уже созданная
		/// </summary>
		[Test]
		public void NewTaskSameType()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Translation
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			Thread.Sleep(1000);
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Translation");
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 6) Настройка Pretranslate. Проверка создания проекта
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Проверяем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Transaltion
			Assert.AreEqual("Translation", workflowList[1],
				"Ошибка: Вторая задача не \"Translation\".");

			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка создания задачи в настройках уже созданного проекта
		/// </summary>
		[Test]
		public void AddingTask()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			CreateProject(ProjectName);
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Добавление новой задачи
			ProjectPage.ClickProjectSettingsWorkflowNewTask();

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(2, "Proofreading");

			// Сохранение проекта
			ProjectPage.ClickProjectSettingsSave();
			Thread.Sleep(1000);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Проверяем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowList[1],
				"Ошибка: Вторая задача не \"Proofreading\".");
		}

		/// <summary>
		/// Проверка отмены создания задачи в настройках уже созданного проекта
		/// </summary>
		[Test]
		public void CancelAddingTask()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			CreateProject(ProjectName);
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Добавление новой задачи
			ProjectPage.ClickProjectSettingsWorkflowNewTask();

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(2, "Proofreading");

			// Отмена сохранения проекта
			ProjectPage.ClickProjectSettingsCancel();
			Thread.Sleep(1000);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Проверяем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");
		}

		/// <summary>
		/// Проверка удаления задачи Workflow при создании проекта
		/// </summary>
		[Test]
		public void DeleteTaskOnCreate()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			Thread.Sleep(1000);
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Proofreading");

			// Удаление первой задачи Translation
			WorkspaceCreateProjectDialog.ClickWorkflowDeleteTask(1);

			workflowList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowList[0],
				"Ошибка: Первая задача не \"Proofreading\".");

			// Проверка, что первая задача - отображена с первым номером
			Assert.AreEqual(1, WorkspaceCreateProjectDialog.GetWFVisibleTaskNumber(1),
				"Ошибка: Первая задача отображена не с первым номером");

			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка удаления задачи Workflow в созданном проекте
		/// </summary>
		[Test]
		public void DeleteTask()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			CreateProject(ProjectName);
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Добавление новой задачи
			ProjectPage.ClickProjectSettingsWorkflowNewTask();

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(2, "Proofreading");
			Thread.Sleep(1000);

			// Удаление первой задачи Translation
			ProjectPage.ClickProjectSettingsWFDeleteTask(1);

			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowList[0],
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
			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Удаление первой задачи Translation
			WorkspaceCreateProjectDialog.ClickWorkflowDeleteTask(1);
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// Проверка наличия отображения сообщения об ошибке
			Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsErrorWFEmptyDisplayed(),
				"Ошибка: Не отображается сообщение об ошибке(пустой workflow).");

			// Проверка, что мастер не перешел на следующий шаг
			Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsStepWF(),
				"Ошибка: Мастер не находится на шаге Workflow.");

			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка отмены удаления задачи Workflow в созданном проекте
		/// </summary>
		[Test]
		public void CancelDeleteTask()
		{
			List<string> workflowListBefore = new List<string>();
			List<string> workflowListAfter = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Proofreading");
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 6) Настройка Pretranslate. Проверка создания проекта
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);
			
			// Записываем задачи Workflow проекта
			workflowListBefore = ProjectPage.GetWFTaskListProjectSettings();

			// Удаление первой задачи
			ProjectPage.ClickProjectSettingsWFDeleteTask(1);

			// Отмена сохранения проекта
			ProjectPage.ClickProjectSettingsCancel();
			Thread.Sleep(1000);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Записываем задачи Workflow проекта
			workflowListAfter = ProjectPage.GetWFTaskListProjectSettings();

			// Проверка, что задачи не изменились
			Assert.AreEqual(workflowListBefore, workflowListAfter,
				"Ошибка: Список задач изменился.");
		}

		/// <summary>
		/// Проверка редактирования задачи в настройках уже созданного проекта
		/// </summary>
		[Test]
		public void ChangingTask()
		{
			List<string> workflowList = new List<string>();
			string workflowTaskBefore = "";
			string workflowTaskAfter = "";

			// Создание проекта
			CreateProject(ProjectName);
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Проверяем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			workflowTaskBefore = workflowList[0];

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(1, "Proofreading");

			// Сохранение проекта
			ProjectPage.ClickProjectSettingsSave();
			Thread.Sleep(1000);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Проверяем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			workflowTaskAfter = workflowList[0];
			
			// Проверка, что первая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowList[0],
				"Ошибка: Вторая задача не \"Proofreading\".");

			// Проверка, что тип задачи изменился
			Assert.AreNotEqual(workflowTaskBefore, workflowTaskAfter,
				"Ошибка: тип задачи не изменился.");
		}

		/// <summary>
		/// Проверка наличия добавленной новой задачи Workflow при возврате в мастере
		/// </summary>
		[Test]
		public void BackOnCreate()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Proofreading");
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// 6) Возврат к предыдущей странице мастера
			WorkspaceCreateProjectDialog.ClickBackBtn();
			Thread.Sleep(1000);

			// Проверка workflow
			workflowList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия только двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Проверка, что вторая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowList[1],
				"Ошибка: Вторая задача не \"Proofreading\".");

			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка добавления новой задачи Workflow после возврата в мастере
		/// </summary>
		[Test]
		public void AddingTaskAfterBack()
		{
			List<string> workflowCreateList = new List<string>();
			List<string> workflowList = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Proofreading");
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// 6) Возврат к предыдущей странице мастера
			WorkspaceCreateProjectDialog.ClickBackBtn();
			Thread.Sleep(1000);

			// 5) Добавление новой задачи Editing
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWFTaskList(3, "Editing");
			Thread.Sleep(1000);

			// Проверка workflow
			workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();
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

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Записываем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();

			// Проверка отличия задач
			Assert.AreEqual(workflowCreateList, workflowList,
				"Ошибка: Задачи отличаются от тех, что были созданы.");
		}

		/// <summary>
		/// Проверка изменения новой задачи Workflow после возврата в мастере
		/// </summary>
		[Test]
		public void ChangingTaskAfterBack()
		{
			List<string> workflowCreateList = new List<string>();
			List<string> workflowList = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Proofreading");
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// 6) Возврат к предыдущей странице мастера
			WorkspaceCreateProjectDialog.ClickBackBtn();
			Thread.Sleep(1000);

			// 5) Изменение задачи Proofreading на Editing
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Editing");
			Thread.Sleep(1000);

			// Проверка workflow
			workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();
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

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Записываем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();

			// Проверка отличия задач
			Assert.AreEqual(workflowCreateList, workflowList,
				"Ошибка: Задачи отличаются от тех, что были созданы.");
		}

		/// <summary>
		/// Проверка удаления задачи Workflow после возврата в мастере
		/// </summary>
		[Test]
		public void DeletingTaskAfterBack()
		{
			List<string> workflowCreateList = new List<string>();
			List<string> workflowList = new List<string>();

			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseExistingTM();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Proofreading");
			WorkspaceCreateProjectDialog.ClickNextStep();
			Thread.Sleep(1000);

			// 6) Возврат к предыдущей странице мастера
			WorkspaceCreateProjectDialog.ClickBackBtn();
			Thread.Sleep(1000);

			// 5) Удаление первой задачи Translation
			WorkspaceCreateProjectDialog.ClickWorkflowDeleteTask(1);
			Thread.Sleep(1000);

			// Проверка workflow
			workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();
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

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);

			// Записываем задачи Workflow проекта
			workflowList = ProjectPage.GetWFTaskListProjectSettings();

			// Проверка отличия задач
			Assert.AreEqual(workflowCreateList, workflowList,
				"Ошибка: Задачи отличаются от тех, что были созданы.");
		}
	}
}
