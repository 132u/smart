using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	class Workspace_CreateProjectDialogTest : BaseTest
	{
		public Workspace_CreateProjectDialogTest(string url, string workspaceUrl, string browserName)
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
		public void CreateProjectWFDefaultTaskType()
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
		public void CreateProjectWFChangeTaskType()
		{
			List<string> workflowCreateList = new List<string>();
			List<string> workflowExistingList = new List<string>();

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

			workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowCreateList.Count, 
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Editing
			Assert.AreEqual("Editing", workflowCreateList[0], 
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
			workflowExistingList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowExistingList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Editing
			Assert.AreEqual("Editing", workflowExistingList[0],
				"Ошибка: Первая задача не \"Editing\".");
		}

		/// <summary>
		/// Проверка наличия всех типов при добавлении новой задачи Workflow
		/// </summary>
		[Test]
		public void CreateProjectWFNewTaskTypes()
		{
			List<string> workflowCreateTypeList = new List<string>();

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
			workflowCreateTypeList = WorkspaceCreateProjectDialog.GetWFTaskTypeList(2);
			// Проверка наличия 7 типов задач
			Assert.AreEqual(7, workflowCreateTypeList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowCreateTypeList[0],
				"Ошибка: Первая задача не \"Translation\"");

			//Выход без сохранения проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка наличия добавленной новой задачи Workflow
		/// </summary>
		[Test]
		public void ProjectWFNewTask()
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
		public void CreateProjectWFNewTaskSameType()
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
		public void ProjectChangingWF()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			CreateProject(ProjectName);
			WorkspacePage.WaitDocumentProjectDownload(ProjectName);

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
			// Проверка наличия одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Добавление новой задачи
			ProjectPage.ClickProjectSettingsWorkflowNewTask();

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(2, "LingvoCupTranslation");

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

			// Проверка, что вторая задача - LingvoCupTranslation
			Assert.AreEqual("LingvoCupTranslation", workflowList[1],
				"Ошибка: Вторая задача не \"LingvoCupTranslation\".");
		}

		/// <summary>
		/// Проверка отмены создания задачи в настройках уже созданного проекта
		/// </summary>
		[Test]
		public void ProjectNonChangingWF()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			CreateProject(ProjectName);
			WorkspacePage.WaitDocumentProjectDownload(ProjectName);

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
			// Проверка наличия одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translation\".");

			// Добавление новой задачи
			ProjectPage.ClickProjectSettingsWorkflowNewTask();

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(2, "LingvoCupTranslation");

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
		public void CreateProjectWFDeleteTask()
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

			// 5) Добавление новой задачи Proofreading
			WorkspaceCreateProjectDialog.ClickWorkflowNewTask();
			Thread.Sleep(1000);
			WorkspaceCreateProjectDialog.SetWFTaskList(2, "Proofreading");

			// Удаление первой задачи Translation
			WorkspaceCreateProjectDialog.ClickWorkflowDeleteTask(1);

			workflowCreateList = WorkspaceCreateProjectDialog.GetWFTaskList();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowCreateList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowCreateList[0],
				"Ошибка: Первая задача не \"Proofreading\".");

			// Проверка, что первая задача - отображена с первым номером
			Assert.AreEqual(1, WorkspaceCreateProjectDialog.GetWFVisibleTaskNumber(1),
				"Ошибка: Первая задача отображена не с первым номером");
		}

		/// <summary>
		/// Проверка удаления задачи Workflow в созданном проекте
		/// </summary>
		[Test]
		public void ProjectWFDeleteTask()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта
			CreateProject(ProjectName);
			WorkspacePage.WaitDocumentProjectDownload(ProjectName);

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
			ProjectPage.SetWFTaskListProjectSettings(2, "LingvoCupTranslation");
			Thread.Sleep(1000);

			// Удаление первой задачи Translation
			ProjectPage.ClickProjectSettingsWFDeleteTask(1);

			workflowList = ProjectPage.GetWFTaskListProjectSettings();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - LingvoCupTranslation
			Assert.AreEqual("LingvoCupTranslation", workflowList[0],
				"Ошибка: Первая задача не \"LingvoCupTranslation\".");

			// Проверка, что первая задача - отображена с первым номером
			Assert.AreEqual(1, ProjectPage.GetProjectSettingsWFVisibleTaskNumber(1),
				"Ошибка: Первая задача отображена не с первым номером");
		}

		/// <summary>
		/// Проверка удаления задачи Workflow при создании проекта
		/// </summary>
		[Test]
		public void CreateProjectWFDeleteAllTasks()
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
		}
	}
}
