using System;
using System.Text;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа основных тестов проекта с использованием корпаративного аккаунта
	/// </summary>
	[Category("Standalone")]
	public class Project_MainTest<TWebDriverSettings> : NewProjectTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Старт тестов
		/// </summary>
		[SetUp]
		public void SetupTest()
		{
			// Переходим к странице воркспейса
			GoToUrl(RelativeUrlProvider.Workspace);
		}

		/// <summary>
		/// метод для тестирования отмены назначения документа пользователю
		/// </summary>
		[Test]
		public void ReassignDocumentToUserTest()
		{
			//Создать пустой проект
			CreateProject(ProjectUniqueName);
			//Добавление документа
			ImportDocumentProjectSettings(PathProvider.DocumentFile, ProjectUniqueName);
			//Назначение задачи на пользователя
			AssignTask();
			// Выбрать документ
			ProjectPage.SelectDocument(1);
			ProjectPage.ClickAssignRessponsibleBtn();
			ProjectPage.WaitProgressDialogOpen();
			// Нажать Отмену назначения
			ProjectPage.ClickCancelAssignBtn();
			// Подтвердить
			ProjectPage.ConfirmClickYes();
			// TODO проверить без sleep
			Thread.Sleep(2000);

			// Проверить, изменился ли статус на not Assigned
			Assert.IsTrue(ProjectPage.GetAssignName() != UserName,
				"Имя в дропдауне назначения пользователя не изменился");

			// Назначить ответственного в окне Progress
			ProjectPage.ClickUserNameCell();
			ProjectPage.ClickAssignUserListUser(UserName);

			// Нажать на Assign
			ProjectPage.ClickAssignBtn();
			// Дождаться появления Cancel
			ProjectPage.WaitCancelAssignBtnDisplay();
			// Нажать на Close
			ProjectPage.CloseAssignDialogClick();
		}

		/// <summary>
		/// изменение имени проекта на новое по нажатию кнопки Back
		/// </summary>
		[Test]
		public void ChangeProjectNameOnNew()
		{
			// Открыли форму создания проекта, заполнили поля
			FirstStepProjectWizard(ProjectUniqueName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Нажать Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			
			// Проверили, что вернулись на первый шаг
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsFirstStep(),
				"Ошибка: по кнопке Back не вернулись на предыдущий первый шаг (выбор имени проекта)");

			// Ввести название проекта
			WorkspaceCreateProjectDialog.FillProjectName("TestProject" + DateTime.Now.Ticks);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Проверить, что ошибки не появилось
			AssertErrorDuplicateName(shouldErrorExist: false);

			// Проверить, что перешли на шаг выбора workflow
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsStepWF(),
				"Ошибка: после ввода нового имени проекта не перешли на следующий шаг (выбор WF)");
		}

		/// <summary>
		/// изменение имени проекта на существующее
		/// </summary>
		[Test]
		public void ChangeProjectNameOnExist()
		{
			// Создать проект
			CreateProject(ProjectUniqueName);
			Thread.Sleep(1000);

			// Открыли форму создания проекта, заполнили поля
			var newProjectName = "TestProject" + DateTime.Now.Ticks;
			FirstStepProjectWizard(newProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Нажать Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			
			// Проверили, что вернулись на первый шаг
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsFirstStep(),
				"Ошибка: по кнопке Back не вернулись на предыдущий шаг (где имя проекта)");

			// Изменить имя
			WorkspaceCreateProjectDialog.FillProjectName(ProjectUniqueName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Проверить, что ошибка появилась
			AssertErrorDuplicateName();
			// Проверить, что не перешли на шаг выбора workflow
			Assert.IsFalse(
				WorkspaceCreateProjectDialog.GetIsStepWF(),
				"Ошибка: после появления сообщения о об ошибке дублирования имени проекта, перешли на следующий шаг (выбор WF)");
		}

		/// <summary>
		/// изменение имени проекта на удаленное
		/// </summary>
		[Test]
		public void ChangeProjectNameOnDeleted()
		{
			// Создать проект
			CreateProject(ProjectUniqueName);
			// Удалить проект
			DeleteProjectFromList(ProjectUniqueName);

			// Проверить, остался ли проект в списке
			Assert.IsTrue(
				GetIsNotExistProject(ProjectUniqueName),
				"Ошибка: проект не удалился");

			//создание нового проекта с именем удаленного
			var newProjectName = "TestProject" + DateTime.Now.Ticks;
			FirstStepProjectWizard(newProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Нажать Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			// Подтвердить переход
			SkipNotSelectedTM();

			// Проверили, что вернулись на первый шаг
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsFirstStep(),
				"Ошибка: по кнопке Back не вернулись на предыдущий шаг (где имя проекта)");

			// Изменить имя
			WorkspaceCreateProjectDialog.FillProjectName(ProjectUniqueName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Проверить, что ошибка не появилась
			AssertErrorDuplicateName(shouldErrorExist: false);
			Thread.Sleep(2000);

			// Проверить, что перешли на шаг выбора workflow
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsStepWF(),
				"Ошибка: после ввода имени удаленного проекта не перешли на следующий шаг (выбор WF)");
		}

		/// <summary>
		/// Проверка на автоматическое присвоение имени проекта при загрузке файла
		/// </summary>
		[Test]
		public void AutofillProjectName()
		{
			const string fileName = "littleEarth";

			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();
			// Загрузить файл
			WorkspaceCreateProjectDialog.UploadFileToNewProject(PathProvider.DocumentFile);

			Assert.IsTrue(
				WorkspaceCreateProjectDialog.CheckProjectName(fileName),
				"Ошибка: Имя проекта автоматически не присвоилось");
		}

		/// <summary>
		/// Проверка, что автоматическое присвоенное имя не изменяется после повторного добавления файла
		/// </summary>
		[Test]
		public void AutofillProjectNameAddTwoFiles()
		{
			const string fileName = "littleEarth";

			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();
			// Загрузить файл
			WorkspaceCreateProjectDialog.UploadFileToNewProject(PathProvider.DocumentFile);

			Assert.IsTrue(
				WorkspaceCreateProjectDialog.CheckProjectName(fileName),
				"Ошибка: Имя проекта автоматически не присвоилось");

			// Загрузить второй файл
			WorkspaceCreateProjectDialog.UploadFileToNewProject(PathProvider.DocumentFile);

			Assert.IsTrue(
				WorkspaceCreateProjectDialog.CheckProjectName(fileName),
				"Ошибка: Автоматическое присвоенное имя проекта изменилось при загрузке второго файла");
		}

		/// <summary>
		/// Проверка, что автоматическое присвоенное имя не изменяется после удаления файла
		/// </summary>
		[Test]
		public void AutofillProjectNameDeleteFile()
		{
			const string fileName = "littleEarth";

			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();
			// Загрузить файл
			WorkspaceCreateProjectDialog.UploadFileToNewProject(PathProvider.DocumentFile);

			Assert.IsTrue(
				WorkspaceCreateProjectDialog.CheckProjectName(fileName),
				"Ошибка: Имя проекта автоматически не присвоилось");

			// Удалить файл
			WorkspaceCreateProjectDialog.ClickDeleteFile(fileName);
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.WaitUntilFileDisappear(fileName),
				"Ошибка: Файл {0} не был удален.", fileName);
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.CheckProjectName(fileName),
				"Ошибка:  Автоматическое присвоенное имя проекта изменилось после удаления файла");		  
		}

		/// <summary>
		/// Создание проекта с текущей датой дедлайна
		/// </summary>
		[Test]
		public void CreateProjectCurrentDeadlineDate()
		{
			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();
			// Ввести название проекта
			WorkspaceCreateProjectDialog.FillProjectName(ProjectUniqueName);
			// Наначить датой дедлайна текущую
			ChooseCurrentDeadlineDate();
			// Нажать следующий шаг
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Нажать "Готово"
			WorkspaceCreateProjectDialog.ClickFinishCreate();

			// Дождаться проекта в списке проектов
			WorkspacePage.WaitProjectAppearInList(ProjectUniqueName);
		}

		/// <summary>
		/// Создание проекта с дедлайном позже текущей даты
		/// </summary>
		[Test]
		public void CreateProjectFutureDeadlineDate()
		{
			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();
			// Ввести название проекта
			WorkspaceCreateProjectDialog.FillProjectName(ProjectUniqueName);
			// Назначаем дедлайн позже текущей даты
			ChooseFutureDeadlineDate();
			// Нажать следующий шаг
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Нажать "Готово"
			WorkspaceCreateProjectDialog.ClickFinishCreate();

			// Дождаться проекта в списке проектов
			WorkspacePage.WaitProjectAppearInList(ProjectUniqueName);
		}

		/// <summary>
		/// Создание проекта с прошедшей датой дедлайна
		/// </summary>
		[Test]
		public void CreateProjectPastDeadlineDate()
		{
			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();
			// Ввести название проекта
			WorkspaceCreateProjectDialog.FillProjectName(ProjectUniqueName);
			// Назначить дедлайн на уже прошедшую дату
			ChoosePastDeadlineDate();
			// Нажать следующий шаг
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Нажать "Готово"
			WorkspaceCreateProjectDialog.ClickFinishCreate();
			// Дождаться проекта в списке проектов
			WorkspacePage.WaitProjectAppearInList(ProjectUniqueName);
		}

		/// <summary>
		/// Неверный формат даты дедлайна
		/// </summary>
		[Test]
		public void InvalidDeadlineDateFormat(
			[Values("03/03/20166", "03 03/2016", "0303/2016", "033/03/2016", "03/033/2016", "03/03/201")] string dateFormat)
		{
			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();
			// Ввести название проекта
			WorkspaceCreateProjectDialog.FillProjectName(ProjectUniqueName);
			// Назначить дедлайн (неверный формат даты)
			WorkspaceCreateProjectDialog.FillDeadlineDate(dateFormat);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			
			// Дождаться сообщения об ошибке			
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsErrorMessageInvalidDeadlineDate(),
				"Ошибка: При введении некорректной даты (" + dateFormat + ") не было сообщения о неверном формате даты");
		}

		//<add key="Login" value="bobby@mailforspam.com" />
		// <add key="Password" value="YrdyNpnnu" />
		// <add key="UserName" value="Bobby Test" />
		// TODO: Убрать если у нас не будет кнопки back для возврата на первый шаг для отмены создания. СЕйчас реализовано, что кнопки нет, но в документации - кнопка описана.
		/// <summary>
		/// отмена создания проекта(подтверждение отмены)
		/// </summary>
		//[Test]
		public void CancelYesTest()
		{

		}

		// TODO: Убрать если у нас не будет кнопки back для возврата на первый шаг для отмены создания. СЕйчас реализовано, что кнопки нет, но в документации - кнопка описана.
		/// <summary>
		/// отмена создания проекта - No 
		/// </summary>
		//[Test]
		public void CancelNoTest()
		{

		}

		/// <summary>
		/// Выбрать текущую дату дедлайна в календаре
		/// </summary>
		protected void ChooseCurrentDeadlineDate()
		{
			// Кликнуть по полю выбора даты дедлайна, чтобы появился календарь
			WorkspaceCreateProjectDialog.ClickDeadlineInput();

			Assert.IsTrue(
				WorkspaceCreateProjectDialog.WaitUntilDisplayCalendar(),
				"Ошибка: Не появился календарь для выбора даты дедлайна");

			// Выбрать в календаре текущую дату
			WorkspaceCreateProjectDialog.ClickDeadlineCurrentDate();

			Assert.IsNotNullOrEmpty(
				WorkspaceCreateProjectDialog.GetDeadlineValue(),
				"Ошибка: Дата дедлайна не выбрана");
		}

		/// <summary>
		/// Выбрать дату дедлайна в календаре после текущей
		/// </summary>
		protected void ChooseFutureDeadlineDate()
		{
			// Кликнуть по полю выбора даты дедлайна, чтобы появился календарь
			WorkspaceCreateProjectDialog.ClickDeadlineInput();

			Assert.IsTrue(
				WorkspaceCreateProjectDialog.WaitUntilDisplayCalendar(),
				"Ошибка: Не появился календарь для выбора даты дедлайна");

			// Перейти на следующий месяц календаря
			WorkspaceCreateProjectDialog.ClickNextMonthPicker();
			// Выбрать в календаре произвольную дату
			WorkspaceCreateProjectDialog.ClickDeadlineSomeDate();

			Assert.IsNotNullOrEmpty(
				WorkspaceCreateProjectDialog.GetDeadlineValue(),
				"Ошибка: Дата дедлайна не выбрана");
		}

		/// <summary>
		/// Выбрать прошедшую дату дедлайна в календаре 
		/// </summary>
		protected void ChoosePastDeadlineDate()
		{
			// Кликнуть по полю выбора даты дедлайна, чтобы появился календарь
			WorkspaceCreateProjectDialog.ClickDeadlineInput();

			Assert.IsTrue(
				WorkspaceCreateProjectDialog.WaitUntilDisplayCalendar(),
				"Ошибка: Не появился календарь для выбора даты дедлайна");

			// Перейти на предыдущий месяц календаря
			WorkspaceCreateProjectDialog.ClickPreviousMonthPicker();
			// Выбрать в календаре произвольную дату
			WorkspaceCreateProjectDialog.ClickDeadlineSomeDate();

			Assert.IsNotNullOrEmpty(
				WorkspaceCreateProjectDialog.GetDeadlineValue(),
				"Ошибка: Дата дедлайна не выбрана");
		}

		/// <summary>
		/// На основе стандартного имени проекта генерирует имя, длинна которого равна maxLength символов.
		/// </summary>
		/// <param name="projectName"> имя проекта </param>
		/// /// <param name="maxLength"> длина возвращаемой строки </param>
		public string GenerateMaxLengthNameForProject(string projectName, int maxLength)
		{
			int increment = maxLength - projectName.Length;
			if (projectName.Length >= maxLength)
				return projectName.Substring(0, maxLength);

			var maxLengthProjectName = new StringBuilder(projectName);
			var rand = new Random();
			for (int i = 0; i < maxLength - projectName.Length; i++)
			{
				maxLengthProjectName.Append(rand.Next(9));
			}

			return maxLengthProjectName.ToString();
		}
	}
}