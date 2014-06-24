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
		public void CheckWorkflowDefaultTaskType()
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
		public void CheckWorkflowChangeTaskType()
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
			workflowExistingList = ProjectPage.ProjectSettingsGetWFTaskList();
			// Проверка наличия только одной задачи
			Assert.AreEqual(1, workflowExistingList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Editing
			Assert.AreEqual("Editing", workflowExistingList[0],
				"Ошибка: Первая задача не \"Editing\".");

			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка наличия всех типов при добавлении новой задачи Workflow
		/// </summary>
		[Test]
		public void CheckWorkflowNewTaskTypes()
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
		public void CheckWorkflowNewTask()
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
			workflowList = ProjectPage.ProjectSettingsGetWFTaskList();
			// Проверка наличия двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translating
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translating\".");

			// Проверка, что вторая задача - Proofreading
			Assert.AreEqual("Proofreading", workflowList[1],
				"Ошибка: Вторая задача не \"Proofreading\".");

			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка создания задачи Workflow того же типа, что и уже созданная
		/// </summary>
		[Test]
		public void CheckWorkflowNewSameTypeTask()
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
			workflowList = ProjectPage.ProjectSettingsGetWFTaskList();
			// Проверка наличия двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translating\".");

			// Проверка, что вторая задача - Transaltion
			Assert.AreEqual("Translation", workflowList[1],
				"Ошибка: Вторая задача не \"Translation\".");

			Thread.Sleep(1000);
		}

		/// <summary>
		/// Проверка создания задачи в настройках уже созданного проекта
		/// </summary>
		[Test]
		public void CheckChangingWorkflowProject()
		{
			List<string> workflowList = new List<string>();

			// Создание проекта с файлом с ТМ без файла и с заданным МТ
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
			workflowList = ProjectPage.ProjectSettingsGetWFTaskList();
			// Проверка наличия двух задач
			Assert.AreEqual(2, workflowList.Count,
				"Ошибка: Неверное количество задач workflow.");

			// Проверка, что первая задача - Translation
			Assert.AreEqual("Translation", workflowList[0],
				"Ошибка: Первая задача не \"Translating\".");

			// Проверка, что вторая задача - Transaltion
			Assert.AreEqual("Translation", workflowList[1],
				"Ошибка: Вторая задача не \"Translation\".");

			Thread.Sleep(1000);
		}
	}
}
