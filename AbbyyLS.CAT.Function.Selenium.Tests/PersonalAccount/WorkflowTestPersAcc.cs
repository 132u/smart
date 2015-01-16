using System;
using NUnit.Framework;
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки рабочего процесса с использованием персонального аккаунта
	/// </summary>
	class WorkflowTestPersAcc : BaseTest
	{
				/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public WorkflowTestPersAcc(string browserName)
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
			GoToWorkspace("Personal");
		}

		/// <summary>
		/// Проверка, что Translation Workflow степ при создании проекта отсутствует для персонального аккаунта
		/// </summary>
		[Test]
		public void CheckThatWorkflowStepIsNotExistInDialogProjectCreation()
		{
			// Создание проекта
			// 1) Заполнение полей
			FirstStepProjectWizard(ProjectName);
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 2) Выбор ТМ
			ChooseFirstTMInList();
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 3) Выбор глоссария
			WorkspaceCreateProjectDialog.ClickNextStep();

			// 4) Выбор МТ
			WorkspaceCreateProjectDialog.ClickNextStep();
			
			// 5) Проверка, что сразу открывается Pretranslation степ
			Assert.IsTrue(ProjectPage.GetPretranslateTitleDisplay(), "Ошибка : Pretranslation степ не открылся");

			WorkspaceCreateProjectDialog.ClickFinishCreate();
			Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(ProjectName),
				"Ошибка: Проект не появился в списке.");
		}
		
		/// <summary>
		/// Проверка, что Translation Workflow степ при создании проекта отсутствует для персонального аккаунта
		/// </summary>
		[Test]
		public void CheckThatWorkflowIsNotExistInProjectSettings()
		{
			//Создаем проект
			CheckThatWorkflowStepIsNotExistInDialogProjectCreation();

			// Открываем проект
			OpenProjectPage(ProjectName);

			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Проверяем, что в настройках проекта нет WorkfloW вкладки
			Assert.IsFalse(ProjectPage.GetSettingsWorkflowDisplay(), "Ошибка: Workflow вкладка отображается в настройках проекта");
		}

		/// <summary>
		/// Проверка, что окно выбора задачи не отображается при входе в редактор с использованием персонального аккаунта
		/// </summary>
		[Test]
		public void CheckChooseTaskDialogDisplay()
		{
			//Создать пустой проект		  
			CreateProject(ProjectName);

			//Добавление документа
			ImportDocumentProjectSettings(TestFile.DocumentFile, ProjectName, "Personal");

			//Открываем документ
			ProjectPage.OpenDocument(1);
			//Проверка, что диалог выбора задачи не появился
			Assert.IsFalse(ResponsiblesDialog.WaitUntilChooseTaskDialogDisplay(), "Ошибка : окно выбора задачи отображается при входе в редактор с использованием персонального аккаунта ");
			
			//Ждем загрузки редактора
			EditorPage.WaitPageLoad();
		}
	}
}
