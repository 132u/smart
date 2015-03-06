using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Another
{
	internal class EditoAnotherTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditoAnotherTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Метод тестирования кнопки "Back" в редакторе
		/// </summary>
		[Test]
		public void HomeButtonTest()
		{
			// Кнопка Back, проверка перехода
			EditorClickHomeBtn();
		}

		/// <summary>
		/// Проверка работы в редакторе кнопки открытия словаря
		/// </summary>
		[Test]
		public void DictionaryButtonTest()
		{
			OpenEditorDictionary();
		}

		/// <summary>
		/// Метод тестирования кнопки отката на предыдущее состояние сегмента
		/// </summary>
		[Test]
		public void PreviousStageButtonTest()
		{
			// Добавить текст в сегмент, подтвердить, проверка подтверждвения
			AddTranslationAndConfirm(1, "some words for example");

			// Выйти из редактора
			EditorPage.ClickHomeBtn();

			// Дождаться открытия страницы проекта
			ProjectPage.WaitPageLoad();

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Добавление новой задачи
			ProjectPage.ClickProjectSettingsWorkflowNewTask();

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(2, "Editing");

			// Сохранение проекта
			ProjectPage.ClickProjectSettingsSave();

			// Переходим на страницу проектов
			SwitchWorkspaceTab();

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для второй задачи группы Administrator
			SetResponsible(2, UserName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Открытие документа
			ProjectPage.OpenDocument(1);
			ResponsiblesDialog.WaitUntilChooseTaskDialogDisplay();

			// Выбор задачи Editing
			EditorPage.ClickEditingTaskBtn();

			//После выбора задачи жмём на Continue
			EditorPage.ClickContBtn();

			// Проверяем что нет замочка в сегменете
			Assert.False(
				EditorPage.GetIsSegmentLock(1),
				"Ошибка: Сегмент заблокирован.");

			// Переходим к первому сегменту
			EditorPage.ClickTargetCell(1);

			// Проверяем что кнопка отката разблокирована
			Assert.False(
				EditorPage.GetIsRollbackBtnLock(),
				"Ошибка: Кнопка отката изменений сегмента заблокирована.");

			// Жмем кнопку отката изменений
			EditorPage.ClickRollbackBtn();

			// Проверяем что появился замочек в сегменете
			Assert.True(
				EditorPage.GetIsSegmentLock(1),
				"Ошибка: Не появился замочек у перевода сегмента.");
		}
	}
}
